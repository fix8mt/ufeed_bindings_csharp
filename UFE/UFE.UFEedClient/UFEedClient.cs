using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using ZeroMQ;
using static UFE.UFEedClient.Consts;
using static UFE.UFEField.Types.UFEFieldLocation;
using static UFE.UFEField.Types.UFEFieldType;
using static UFE.WireMessage.Types.Type;
using UFEMessageType = UFE.WireMessage.Types.Type;

namespace UFE.UFEedClient
{
	/// <summary>
	/// A class to communicate to UFE
	/// <example>
	///	using (var uc = new UFEedClient(new UFEedConfiguration{Subscriber = SUBSCRIBER_DEFAULT}))
	///	{
	///		uc.SubscriptionMessageReceived += (sender, args) => { ... };
	///		uc.ResponderMessageReceived += (sender, args) => { ... };
	///		uc.ZeroMQErrorHappened += (sender, args) => { ... };
	///		uc.ErrorHappened += (sender, args) => { ... };
	///		uc.Start();
	///		var login = uc.CreateMessage("login", StSystem, UFE_CMD_LOGIN)
	///			.add_field(UFE_CMD, UFE_CMD_LOGIN)
	///			.add_field(UFE_LOGIN_ID, "abcdef")
	///			.add_field(UFE_LOGIN_PW, "1e884898da28047151d0e56f8df6292773603d0d6");
	///		var response = uc.Request(login);
	/// 		...
	/// 	}
	/// </example>
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public class UFEedClient : IDisposable
	{
		private readonly ZContext _context;
		private readonly ZSocket _reqSocket;
		private ZSocket _repSocket;
		private ByteString _sessionId = ByteString.Empty;
		private Task _workerTask;
		private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
		private ZError _error;
		private bool _started;
		private uint _responderSeq;
		private SpinLock _reqRepLock = new SpinLock();
		private SpinLock _backChannelLock = new SpinLock();

		/// <summary>
		/// Constructs UFEedClient
		/// </summary>
		/// <param name="configuration">configuration to setup UFEedClient object</param>
		public UFEedClient(UFEedConfiguration configuration)
		{
			Configuration = configuration;
			_context = new ZContext();
			_context.ThreadPoolSize = configuration.MaxIoThreads;
			_reqSocket = new ZSocket(_context, ZSocketType.REQ);
		}

		/// <summary>
		/// Disposes UFEedClient resources, implements IDisposable.Dispose()
		/// </summary>
		public void Dispose()
		{
			Stop();
			if (!_sessionId.IsEmpty)
			{
				var logout = CreateMessage()
					.SetName("logout")
					.SetType(StSystem)
					.SetServiceId(UFE_CMD_LOGOUT);
				logout.AddField(UFE_CMD, UFE_CMD_LOGOUT);
				Request(logout);
			}
			_sessionId = ByteString.Empty;
			_reqSocket.Dispose();
			_context.Dispose();
		}

		/// <summary>
		/// Returns UFEedClient configuration
		/// </summary>
		public UFEedConfiguration Configuration { get; }

		/// <summary>
		/// Starts UFEedClient. When started in synchronous mode (wait = true)
		/// it does not return until Stop() is called from a different thread.
		/// </summary>
		/// <param name="wait">true for synchronous call, false for asynchronous</param>
		public void Start(bool wait = false)
		{
			if (_started)
				return;
			_started = true;
			_reqSocket.Connect(Configuration.Requester);
			if (wait)
				Worker();
			else
				_workerTask = Task.Factory.StartNew(Worker, _cancellationToken.Token);
		}

		/// <summary>
		/// Stops UFEedClient
		/// </summary>
		public void Stop()
		{
			if (!_started)
				return;
			_cancellationToken.Cancel();
			_workerTask.Wait();
		}

		/// <summary>
		/// Creates <c>UFEMessage</c> builder
		/// </summary>
		/// <returns>new <c>UFEMessage.Builder</c></returns>
		public UFEMessage.Builder CreateMessage()
		{
			return new UFEMessage.Builder();
		}

		/// <summary>
		/// Creates UFEMEssage builder from <c>WireMessage</c>
		/// </summary>
		/// <param name="wm"><c>WireMessage</c> to create from</param>
		/// <returns>new <c>UFEMessage.Builder</c></returns>
		public UFEMessage.Builder CreateMessage(WireMessage wm)
		{
			return new UFEMessage.Builder(wm);
		}

		/// <summary>
		/// Synchronously sends request to UFE and waits for UFE response
		/// </summary>
		/// <param name="request">Request to send </param>
		/// <returns>Response received</returns>
		/// <exception cref="UFEedClientException">Can throw UFEedClientException</exception>
		public UFEMessage Request(UFEMessage.Builder request)
		{
			var wm = request.WireMessage;
			// have we already logged in?
			if (!_sessionId.IsEmpty)
			{
				wm.Fields.Add(new UFEField{Tag = UFE_SESSION_TOKEN, Sval = _sessionId, Location = FlSystem, Type = FtUuid});
				wm = SendRequestRecvResponse(wm);
			}
			// well ok, is this a login attempt?
			else if (wm.ServiceId == UFE_CMD_LOGIN)
			{
				wm = SendRequestRecvResponse(wm);
				var sessionField = wm.Fields.FirstOrDefault(field => field.Tag == UFE_SESSION_TOKEN);
				if (sessionField != null)
					_sessionId = sessionField.Sval;
			}
			// neither?
			else
				throw new UFEedClientException("No session token found - you must log on before making a request");

			// send REP WireMessage to handler function
			var response = CreateMessage(wm).Build();
			ResponseMessageReceived?.Invoke(this, new UFEMessageEventArgs(response));
			return response;
		}

		/// <summary>
		/// Send message to responder channel.
		/// IMPORTANT: Should be called as much as possible from within ResponderMessageReceived callback thread
		/// </summary>
		/// <param name="msg">Message to send</param>
		public void Respond(UFEMessage msg)
		{
			if (string.IsNullOrEmpty(Configuration.Responder))
				 throw new UFEedClientException($"No rep socket was created. Please specify config.Responder");
			
			var lockTaken = false;
			try
			{
				_backChannelLock.Enter(ref lockTaken);
				_repSocket.SendMessage(new ZMessage(new[]
				{
					new ZFrame(ByteString.CopyFrom(Configuration.ResponderTopic, Encoding.Default).ToByteArray()),
					new ZFrame(msg.WireMessage.ToByteArray())
				}));
			}
			finally
			{
				if (lockTaken) _backChannelLock.Exit();
			}
		}
		
		/// <summary>
		/// Event for receiving subscription message. Will be called in a different thread in most cases.
		/// </summary>
		public event EventHandler<UFEMessageEventArgs> SubscriptionMessageReceived;

		/// <summary>
		/// Event for receiving responder message. Will be called in a different thread in most cases.
		/// </summary>
		public event EventHandler<UFEMessageEventArgs> ResponderMessageReceived;

		/// <summary>
		/// Event for receiving response message for a sent request.
		/// </summary>
		public event EventHandler<UFEMessageEventArgs> ResponseMessageReceived;

		/// <summary>
		/// Event for authentication request. Will be called in a different thread in most cases.
		/// Consumer must set UFEAuthEventArgs.Accept to false in case of wrong login/password. 
		/// </summary>
		public event EventHandler<UFEAuthEventArgs> AuthenticateRequested; 

		/// <summary>
		/// Event for ZeroMQ errors.
		/// Consumer must set UFEZeroMQErrorEventArgs.Continue to false in case of unrecoverable errors 
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public event EventHandler<UFEZeroMQErrorEventArgs> ZeroMQErrorHappened;

		/// <summary>
		/// Event for UFEedClintErrors
		/// Consumer must set UFEErrorEventArgs.Continue to false in case of unrecoverable errors 
		/// </summary>
		public event EventHandler<UFEErrorEventArgs> ErrorHappened;

		private void Worker()
		{
			ZSocket subSocket = null;
			try
			{
				subSocket = new ZSocket(_context, ZSocketType.SUB);
				subSocket.Connect(Configuration.Subscriber);
				subSocket.Subscribe(Configuration.SubscriberTopic);

				_repSocket = new ZSocket(_context, ZSocketType.REP);
				if (!string.IsNullOrEmpty(Configuration.Responder))
					_repSocket.Bind(Configuration.Responder);
				
				var token = _cancellationToken.Token;
				var sockets = new[] {subSocket, _repSocket};
				var polls = new[] {ZPollItem.CreateReceiver(), ZPollItem.Create(
					(ZSocket socket, out ZMessage message, out ZError error) =>
					{
						message = null;
						bool ret;
						var lockTaken = false;
						try
						{
							_backChannelLock.Enter(ref lockTaken);
							ret = socket.ReceiveMessage(ref message, out error);
						}
						finally
						{
							if (lockTaken) _backChannelLock.Exit();
						}
						return ret;
					}, null)};

				var timeout = TimeSpan.FromMilliseconds(Configuration.PollIntervalMs);
				while (!token.IsCancellationRequested)
				{
					try
					{
						if (sockets.PollIn(polls, out var msgs, out _error, timeout))
						{
							if (msgs == null)
								continue;
							for (var i = 0; i < msgs.Length; ++i)
							{
								// i==0, subscriber message
								// i==1, responder message
								var msg = msgs[i];
								if (msg == null)
									continue;
								if (msg.Count == 2)
								{
									var wm = new WireMessage();
									wm.MergeFrom(msg[1].Read());
									var um = CreateMessage(wm).Build();
									if (i == 0)
									{
										SubscriptionMessageReceived?.Invoke(this, new UFEMessageEventArgs(um));
									}
									else
									{
										ResponderMessageReceived?.Invoke(this, new UFEMessageEventArgs(um));
										ProcessRespondMessage(um);
									}
								}
								else
								{
									var eventArgs = new UFEErrorEventArgs($"Message has {msg.Count} frames but 2 is expected");
									ErrorHappened?.Invoke(this, eventArgs);
									if (!eventArgs.Continue)
										_cancellationToken.Cancel();
								}
							}
						}
						else
						{
							if (_error == ZError.EAGAIN)
								continue;
							var eventArgs = new UFEZeroMQErrorEventArgs(_error, "ALL");
							ZeroMQErrorHappened?.Invoke(this, eventArgs);
							if (!eventArgs.Continue)
								_cancellationToken.Cancel();
						}
					}
					catch (Exception e)
					{
						var eventArgs = new UFEErrorEventArgs($"Exception {e} raised", e);
						ErrorHappened?.Invoke(this, eventArgs);
						if (!eventArgs.Continue)
							_cancellationToken.Cancel();
					}
				}
			}
			finally
			{
				_repSocket?.Dispose();
				subSocket?.Dispose();
			}
		}

		private WireMessage SendRequestRecvResponse(WireMessage wm)
		{
			var lockTaken = false; 
			ZMessage response;
			try
			{
				_reqRepLock.Enter(ref lockTaken);
				_reqSocket.SendMessage(new ZMessage(new[]
				{
					new ZFrame(ByteString.CopyFrom(Configuration.RequesterTopic, Encoding.Default).ToByteArray()),
					new ZFrame(wm.ToByteArray())
				}));
				response = _reqSocket.ReceiveMessage();
			}
			finally
			{
				if (lockTaken) _reqRepLock.Exit();
			}

			if (response.Count != 2)
				throw new UFEedClientException($"Message has {response.Count} frames but 2 is expected");
			var wmr = new WireMessage();
			wmr.MergeFrom(response[1].Read());
			return wmr;
		}
		
		private UFEMessage ProcessRespondMessage(UFEMessage msg)
		{
			var rmsg = CreateMessage()
				.SetServiceId(msg.WireMessage.ServiceId)
				.SetSubServiceId(msg.WireMessage.SubserviceId)
				;

			ByteString reqToken = null;
			long responseCode = UFE_OK;
			string responseText = null;
			var rToken = msg.FindField(UFE_REQUEST_TOKEN);
			if (rToken != null)
				reqToken = rToken.Sval;
			switch (msg.WireMessage.Type)
			{
				case StFixmsg:
					break;
				case StSystem:
					var cmd = msg.FindField(UFE_CMD);
					if (cmd == null)
					{
						responseText =  $"command not present {msg.WireMessage.Type} on topic={Configuration.ResponderTopic}";
						responseCode = NO_CMD;
					}
					else
					{
						var cmdToken = cmd.Ival;
						switch(cmdToken)
						{
							case UFE_CMD_AUTHENTICATE: // authentication response
								var usrToken = msg.FindField(553); // Username
								var userName = usrToken == null ? string.Empty : usrToken.Sval.ToString();
								var pwToken = msg.FindField(554); // Password
								var password = pwToken == null ? string.Empty : pwToken.Sval.ToString();
								var args = new UFEAuthEventArgs(userName, password);
								AuthenticateRequested?.Invoke(this, args);
								if (args.Accept)
								{
									responseText = "authentication was successful";
									responseCode = LOGIN_ACCEPTED;
								}
								else
								{
									responseText = "user or password is incorrect";
									responseCode = UNKNOWN_USER;
								}
								break;
							default:
								responseCode = NO_CMD;
								break;
						}
						rmsg.AddField(UFE_CMD_RESPONSE, cmdToken, FlSystem);
					}
					break;
				default:
					responseText = $"unknown message type or command {msg.WireMessage.Type}  on topic={Configuration.ResponderTopic}";
					responseCode = UNKNOWN_TYPE;
					break;
			}
			// Generate response and send
			rmsg.WireMessage.Seq = ++_responderSeq;
			rmsg.AddField(UFE_RESPONSE_CODE, new UFEMessage.Status{ Long = responseCode }, FlSystem);
			rmsg.AddField(COMMON_REFSEQNUM, rmsg.WireMessage.Seq, FlSystem);
			if (reqToken != null && reqToken.Length != 0)	// if request token received, echo back
				rmsg.AddField(UFE_REQUEST_TOKEN, reqToken, FlSystem);
			switch (responseCode)
			{
				case UFE_OK:
				case LOGIN_ACCEPTED:
					rmsg.AddField(UFE_RESPONSE_TOKEN, Guid.NewGuid(), FlSystem);
					rmsg.WireMessage.Type = StResponse;
					break;
				default: // is an error
					rmsg.WireMessage.Type = StError;
					break;
			}

			if (!string.IsNullOrEmpty(responseText)) // both errors and success can pass a text msg
				rmsg.AddField(COMMON_TEXT, responseText, FlSystem);

			var rmsg1 = rmsg.Build();
			Respond(rmsg1);
			return rmsg1;
		}

	}
}
