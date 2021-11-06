using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using static UFE.UFEedClient.Consts;
using static UFE.WireMessage.Types.Type;
using UFE.UFEedClient.FIX50SP2.Field;
using static UFE.UFEField.Types.UFEFieldLocation;

namespace UFE.UFEedClient.Test
{
	public class Tests
	{
		private UFEedClient _uc;
		private readonly List<UFEMessage> _receivedSubMsgs = new List<UFEMessage>();
		private readonly List<UFEMessage> _receivedResMsgs = new List<UFEMessage>();
		private readonly List<UFEMessage> _receivedRepMsgs = new List<UFEMessage>();
		private readonly object _authenticatedLock = new object();
		private string _login, _password;
		private bool _authenticated;

		[SetUp]
		public void Setup()
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
				Assert.Fail($"ZeroMQ error happened {args.Error}");
				args.Continue = false;
			};
			_uc.ErrorHappened += (sender, args) =>
			{
				Assert.Fail($"Error happened {args.Message}");
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

		[TearDown]
		public void TearDown()
		{
			_uc.Dispose();
		}

		[Test]
		public void TestLogonAuth()
		{
			_uc.Configuration.Responder = RESPONDER_DEFAULT;
			TestLogon();
			
			// authentication check
			// - this requires manual run of f8ptest initiator to connect to one of the auth session in ufegw 
			Console.Out.WriteLine("waiting for auth...");
			lock (_authenticatedLock)
			{
				Monitor.Wait(_authenticatedLock, TimeSpan.FromSeconds(1000));
				Assert.IsTrue(_authenticated);
			}
			Console.Out.WriteLine($"authenticated == {_authenticated}");
			Thread.Sleep(1000);
		}
		
		[Test]
		public void TestLogonWithoutResponder()
		{
			_uc.Configuration.Responder = null;
			TestLogon();
		}

		[Test]
		public void TestMessage()
		{
			// NOS creation
			UFEField g1 = null;
			var nos = _uc.CreateMessage()
				.SetLongName("NewOrderSingle")
				.SetType(StFixmsg)
				.SetServiceId(1)
				.SetName(MsgType.NEWORDERSINGLE)
				.AddField(ClOrdID.tag, "123")
				.AddField(TransactTime.tag, DateTime.Now)
				.AddField(ExecInst.tag, ExecInst.ALL_OR_NONE)
				.AddField(OrdType.tag, OrdType.LIMIT)
				.AddField(Price.tag, 123.456, precision: 4)
				.AddField(OrderQty.tag, 456.789, precision: 2)
				.AddField(Side.tag, Side.BUY)
				.AddGroup(NoAllocs.tag, out g1, (builder, group) =>
				{
					UFEField g2 = null;
					builder.AddGroupItem(group)
						.SetLongName("NoAlloc")
						.SetType(StFixmsg)
						.SetSeq(1)
						.AddField(AllocAccount.tag, "ABC", FlBody)
						.AddField(AllocQty.tag, 2)
						.AddGroup(NoPartyIDs.tag, out g2, (builder1, group1) =>
						{
							builder1.AddGroupItem(group1)
								.SetLongName("NoPartyIDs")
								.SetType(StFixmsg)
								.SetSeq(1)
								.AddField(PartyID.tag, "sgo");
						});
					builder.AddGroupItem(group)
						.SetLongName("NoAlloc")
						.SetType(StFixmsg)
						.SetSeq(2)
						.AddField(AllocAccount.tag, "CDE", FlBody)
						.AddField(AllocQty.tag, 4)
						.AddGroup(NoPartyIDs.tag, out g2, (builder1, group1) =>
						{
							builder1.AddGroupItem(group1)
								.SetLongName("NoPartyIDs")
								.SetType(StFixmsg)
								.SetSeq(1)
								.AddField(PartyID.tag, "ssv");
						});
				})
				.AddField(UFE_RESPONSE_CODE, new UFEMessage.Status{ Long = UFE_OK }, FlSystem)
				.AddField(UFE_SESSION_TOKEN, Guid.NewGuid(), FlSystem)
				;
			var s1 = nos.Print();
			var s2 = nos.Build().Print();
			Assert.AreEqual(s1, s2);
		}

		private void TestLogon()
		{
			_uc.Start();
			
			// logon
			var login = _uc.CreateMessage()
				.SetLongName("login")
				.SetType(StSystem)
				.SetServiceId(UFE_CMD_LOGIN)
				.AddField(UFE_CMD, new UFEMessage.Status{Long = UFE_CMD_LOGIN})
				.AddField(UFE_LOGIN_ID, "webuser")
				.AddField(UFE_LOGIN_PW, "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8");
			var response = _uc.Request(login);
			Assert.AreEqual(1, _receivedResMsgs.Count);
			Assert.NotNull(_receivedResMsgs[0].FindField(UFE_SESSION_TOKEN));
			var sessToken = _receivedResMsgs[0].FindFieldValue(UFE_SESSION_TOKEN);
			Assert.NotNull(sessToken);
			Assert.IsTrue(sessToken is Guid);
			Assert.AreNotEqual(0, sessToken.ToString().Length);
			Assert.AreEqual(response.FindFieldValue(UFE_SESSION_TOKEN), sessToken);

			// service list request
			response = _uc.Request(_uc
				.CreateMessage()
				.SetLongName("service_list")
				.SetType(StSystem)
				.SetServiceId(UFE_CMD_SERVICE_LIST)
				.AddField(UFE_CMD, new UFEMessage.Status {Long = UFE_CMD_SERVICE_LIST}));
			Assert.AreEqual(1, response.Groups.Count);

			// subscription check
			lock (_receivedSubMsgs)
			{
				Monitor.Wait(_receivedSubMsgs, TimeSpan.FromSeconds(10));
				Assert.Greater(_receivedSubMsgs.Count, 0);
			}
		}
	}
}
