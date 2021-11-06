using System;
using System.Collections.Generic;
using System.Threading;
using static UFE.UFEedClient.Consts;
using static UFE.WireMessage.Types.Type;
using UFE.UFEedClient.FIX50SP2.Field;
using static UFE.UFEField.Types.UFEFieldLocation;

namespace UFE.UFEedClient.Sample
{
	public class Sample: IDisposable
	{
		private UFEedClient _uc;
		private readonly List<UFEMessage> _receivedSubMsgs = new List<UFEMessage>();
		private readonly List<UFEMessage> _receivedResMsgs = new List<UFEMessage>();
		private readonly List<UFEMessage> _receivedRepMsgs = new List<UFEMessage>();
		private readonly object _authenticatedLock = new object();
		private string _login, _password;
		private bool _authenticated;

		public Sample()
		{
			_uc = new UFEedClient(new UFEedConfiguration
			{
				Subscriber = SUBSCRIBER_DEFAULT, 
				ResponderTopic = "ufegw-requester",
				Responder = RESPONDER_DEFAULT,
				MaxIoThreads = 2,
				PollIntervalMs = 100
			});
			_uc.SubscriptionMessageReceived += (sender, args) =>
			{
				lock (_receivedSubMsgs)
				{
					_receivedSubMsgs.Add(args.Message);
					Monitor.Pulse(_receivedSubMsgs);
				}
			};
			_uc.ResponseMessageReceived += (sender, args) =>
			{
				_receivedResMsgs.Add(args.Message);
			};
			_uc.ResponderMessageReceived += (sender, args) =>
			{
				_receivedRepMsgs.Add(args.Message);
			};
			_uc.ZeroMQErrorHappened += (sender, args) =>
			{
				Console.Error.WriteLine($"ZeroMQ error happened {args.Error}");
				args.Continue = false;
			};
			_uc.ErrorHappened += (sender, args) =>
			{
				Console.Error.WriteLine($"Error happened {args.Message}");
				args.Continue = false;
			};
			_uc.AuthenticateRequested += (sender, args) =>
			{
				// accept any args.User/args.Password
				args.Accept = true;
				lock (_authenticatedLock)
				{
					_login = args.User;
					_password = args.Password;
					_authenticated = true;
					Monitor.Pulse(_authenticatedLock);
				}
			};
		}

		public void Dispose()
		{
			_uc.Dispose();
		}

		public void LogonAuth()
		{
			_uc.Configuration.Responder = RESPONDER_DEFAULT;
			_uc.Start();
			
			// logon
			var login = _uc.CreateMessage()
				.SetLongName("login")
				.SetType(StSystem)
				.SetServiceId(UFE_CMD_LOGIN)
				.AddField(UFE_CMD, new UFEMessage.Status{Long = UFE_CMD_LOGIN})
				.AddField(UFE_LOGIN_ID, "webuser")
				.AddField(UFE_LOGIN_PW, "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8")
				;
			var response = _uc.Request(login);
			if (1 != _receivedResMsgs.Count)
				throw new UFEedClientException("Received message count mismatch");
			if (_receivedResMsgs[0].FindField(UFE_SESSION_TOKEN) == null)
				throw new UFEedClientException("Could not find session token");
			var sessToken = _receivedResMsgs[0].FindFieldValue(UFE_SESSION_TOKEN);
			if (sessToken == null || !(sessToken is Guid) || sessToken.ToString().Length==0)
				throw new UFEedClientException($"Invalid session token {sessToken}");
			if (response.FindFieldValue(UFE_SESSION_TOKEN).ToString() != sessToken.ToString())
				throw new UFEedClientException($"Session token mimatch {response.FindFieldValue(UFE_SESSION_TOKEN)} != {sessToken}");

			// service list request
			response = _uc.Request(_uc
				.CreateMessage()
				.SetLongName("service_list")
				.SetType(StSystem)
				.SetServiceId(UFE_CMD_SERVICE_LIST)
				.AddField(UFE_CMD, new UFEMessage.Status{Long = UFE_CMD_SERVICE_LIST}));
			if (1 != response.Groups.Count)
				throw new UFEedClientException("Missing groups in response");

			// subscription check
			lock (_receivedSubMsgs)
			{
				Monitor.Wait(_receivedSubMsgs, TimeSpan.FromSeconds(10));
				if (_receivedSubMsgs.Count <= 0)
					throw new UFEedClientException("No received messages");
			}

			// authentication check
			// - this requires manual run of f8ptest initiator to connect to one of the auth session in ufegw 
			Console.Out.WriteLine("waiting for auth...");
			lock (_authenticatedLock)
			{
				Monitor.Wait(_authenticatedLock, TimeSpan.FromSeconds(1000));
				if (!_authenticated)
					throw new UFEedClientException("Not authenticated");
			}
			Console.Out.WriteLine($"authenticated == {_authenticated}");
			Thread.Sleep(1000);
		
			// NOS creation
			var nos = _uc.CreateMessage()
				.SetLongName("NewOrderSingle")
				.SetType(StFixmsg)
				.SetServiceId(1)
				.SetName(MsgType.NEWORDERSINGLE)
				.AddField(ClOrdID.tag, "123")
				.AddField(TransactTime.tag, DateTime.Now)
				.AddField(ExecInst.tag, ExecInst.ALL_OR_NONE)
				.AddField(OrdType.tag, OrdType.LIMIT)
				.AddField(Side.tag, Side.BUY)
				.AddGroup(NoAllocs.tag, out var g1, (builder, group) =>
				{
					builder.AddGroupItem(group)
						.SetLongName("NoAlloc")
						.SetType(StFixmsg)
						.SetSeq(1)
						.AddField(AllocAccount.tag, "ABC", FlBody)
						.AddField(AllocQty.tag, 2);
					builder.AddGroupItem(group)
						.SetLongName("NoAlloc")
						.SetType(StFixmsg)
						.SetSeq(2)
						.AddField(AllocAccount.tag, "CDE", FlBody)
						.AddField(AllocQty.tag, 4);
				});
				;
		}
	}
	
	class Program
	{
		static void Main(string[] args)
		{
			using (var sample = new Sample())
			{
				sample.LogonAuth();
			}
		}
	}
}