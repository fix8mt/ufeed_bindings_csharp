# UFEed C# Binding

-   [Introduction](#introduction)
-   [Getting started](#getting-started)
-   [Interface](#interface)
    -   [UFEMessage and UFEMessage Builder](#ufemessage-and-ufemessage-builder)
    -   [UFEedClient](#ufeedclient)
-   [Constants](#constants)
    -   [FIX variants constants](#fix-variants-constants)
-   [Building](#building)


------------------------------------------------------------------------

# Introduction

The UFEed C# Adapter (`UFEed_C#`) provides a low level native C# interface to
the UFEGW. Interactions with the UFEGW are based around a `UFEedClient`
object from `UFE.UFEedClient` namespace which can be used to send and
receive Messages to and from the UFEGW.

Use the following [Universal FIX engine documentaion](https://fix8mt.atlassian.net/wiki/spaces/FMT/pages/634438/Universal+FIX+Engine+Home) for a reference.

Features of `UFEedClient`:

-   System API support ([see 4. Implementation Guide - Section
    1.3](https://fix8mt.atlassian.net/wiki/spaces/FIX8PRO/pages/628308/4.+Implementation+Guide))

-   Business API support (eg. NewOrderSingle and standard FIX messages)

-   Provides a 4-way communications API in order to make requests,
    publish messages, receive responses and subscribe to broadcast
    messages

-   User defined implementation of callback interface to handle these
    PUB, SUB, REQ and REP message events

-   Dynamic configuration of PUB, SUB, REQ, REP addressing and topics

-   Internal session management

Features of a `UFEMessage` and `UFEMessage.Builder`:

-   A generic container of fields, i.e. tag/typed value pairs with
    possible nested messages called groups

-   Smart field creation and access, rendering field value to ival, sval
    or fval depending on context

-   Named Message getters/setters (name, long_name, seq, service_id,
    subservice_id)

# Getting started

The `UFEed_C#` is provided as a .NetCore nuget project for Linux and Windows. 3rd-party
dependencies are included to nuget package and will be downloaded during
`nuget restore` command. ZeroMQ C# wrapper
(<https://github.com/zeromq/clrzmq4>) has native build for .NetCore,
it's included in distribution as `ZeroMQ.1.0.0.nupkg`. Within the
packed or target build `UFEed_C#` archive we find the following directory structure
(example is Linux):

```
ufeed_bindings_csharp
├── packages
│   ├── UFE.UFEedClient.<version>.nupkg
│   └── ZeroMQ.1.0.0.nupkg
├── Program.cs
└── UFE.UFEedClient.Sample.netcore.csproj
```

# Interface

The main `UFEed_C#` interfaces/classes are `UFEMessage` and `UFEedClient`.
Most of `UFEMessage` field setters and `UFEedClient` setters follow
'builder' pattern to simplify C# language constructs (i.e. setters
return the reference to an object it was called from):

```c#
// logon
var login = _uc.CreateMessage()
    .SetLongName("login")
    .SetType(StSystem)
    .SetServiceId(UFE_CMD_LOGIN)
    .AddField(UFE_CMD, UFE_CMD_LOGIN)
    .AddField(UFE_LOGIN_ID, "webuser")
    .AddField(UFE_LOGIN_PW, "5e884898da28047151d0e56f8dc");
```

## UFEMessage and UFEMessage Builder

The `UFEMessage` class provides read-only access to underlying `WireMessage` object and mapped message fields and groups. `UFEMessage.BuildeR` on the other hand gives write-access to underlying
`WireMessage`. Messages are objects with which requests are made from the `UFEed_C#` to the UFEGW. `UFEMessage` ctors are public but in most cases `UFEedClient::CreateMessage()` factory method shall be used to extend `UFEMessage` class to other underlying protocols and middleware plugins in the future. `UFEMessage.Builder` class follows "builder" pattern to build a message.

```c#
public class Builder
{
    /// <summary>
    /// Ctors
    /// </summary>
    public Builder(WireMessage wm);

    /// <summary>
    /// Message properties
    /// </summary>
    public string Name { get; set; }
    public string LongName { get; set; }
    public WireMessage.Types.Type  Type { get; set; }
    public int ServiceId { get; set; }
    public int SubServiceId { get; set; }
    public int Seq { get; set; }

    /// <summary>
    /// Message properties setters for builder pattern
    /// </summary>
    public Builder SetName(string name);
    public Builder SetLongName(string longName);
    public Builder SetType(WireMessage.Types.Type type);
    public Builder SetServiceId(int serviceId);
    public Builder SetSubServiceId(int subServiceId);
    public Builder SetSeq(int seq);

    /// <summary>
    /// Underlying <c>WireMessage</c> readonly property
    /// </summary>
    public WireMessage WireMessage { get; }

    /// <summary>
    /// Adds field with typed value
    /// </summary>
    /// <param name="tag">Field tag</param>
    /// <param name="val">Field typed value</param>
    /// <param name="loc">Field location</param>
    /// <returns>self</returns>
    public Builder AddField(uint tag, long val, UFEFieldLocation loc = UFEFieldLocation.FlBody);
    public Builder AddField(uint tag, ByteString val, UFEFieldLocation loc = UFEFieldLocation.FlBody);
    public Builder AddField(uint tag, string val, UFEFieldLocation loc = UFEFieldLocation.FlBody);
    public Builder AddField(uint tag, char val, UFEFieldLocation loc = UFEFieldLocation.FlBody);
    public Builder AddField(uint tag, double val, UFEFieldLocation loc = UFEFieldLocation.FlBody);
    public Builder AddField(uint tag, bool val, UFEFieldLocation loc = UFEFieldLocation.FlBody);
    public Builder AddField(uint tag, DateTime val, UFEFieldLocation loc = UFEFieldLocation.FlBody);
    public Builder AddField(uint tag, Guid val, UFEFieldLocation loc = UFEFieldLocation.FlBody);
    public Builder AddField(uint tag, Message.Status val, UFEFieldLocation loc = UFEFieldLocation.FlBody);
    public Builder AddField(uint tag, object val, UFEFieldLocation loc = UFEFieldLocation.FlBody);
    public Builder AddFields(IEnumerable<UFEField> fields);
    public Builder AddGroup(uint tag, out UFEField group, Action<Builder, UFEField> tr, UFEFieldLocation loc = UFEFieldLocation.FlBody)
    public Builder AddGroupItem(UFEField group);
    
    /// <summary>
    /// Creates UFEMessage from builder 
    /// </summary>
    /// <returns>built UFEMessage</returns>
    public UFEMessage Build();
}
```

`UFEMessage`:

```c#
public class UFEMessage
{
    /// <summary>
    /// Ctors
    /// </summary>
    /// <param name="wm">Optional <c>WireMessage</c> to construct from</param>
    public UFEMessage(WireMessage wm);

    /// <summary>
    /// Message properties
    /// </summary>
    public string Name { get; }
    public string LongName { get; }
    public WireMessage.Types.Type  Type { get; }
    public int ServiceId { get; }
    public int SubServiceId { get; }
    public int Seq { get; }

    /// <summary>
    /// Underlying <c>WireMessage</c> readonly property
    /// </summary>
    public WireMessage WireMessage { get; }

    /// <summary>
    /// Mapped fields property
    /// </summary>
    public Dictionary<uint, UFEField> Fields { get; };

    /// <summary>
    /// Mapped groups property
    /// </summary>
    public Dictionary<uint, List<UFEMessage>> Groups { get; };

    /// <summary>
    /// Finds field by tag
    /// </summary>
    /// <param name="tag">field tag to find</param>
    /// <returns>Found field or null if not found</returns>
    public UFEField FindField(uint tag);

    /// <summary>
    /// Finds field value by tag
    /// </summary>
    /// <param name="tag">field tag to find</param>
    /// <returns>Found field value or null if not found</returns>
    public object FindFieldValue(uint tag);

    /// <summary>
    /// Finds group by tag
    /// </summary>
    /// <param name="tag">group tag to find</param>
    /// <returns>Found group or null if not found</returns>
    public List<UFEMessage> FindGroup(uint tag);

    /// <summary>
    /// Creates a new builder
    /// </summary>
    /// <returns>message builder from this message</returns>
    public Builder NewBuilder();
}
```

`UFEMessage` and `UFEMessage.Builder` usage sample:

```c#
// logon
var login = _uc.CreateMessage()
    .SetLongName("login")
    .SetType(StSystem)
    .SetServiceId(UFE_CMD_LOGIN)
    .AddField(UFE_CMD, UFE_CMD_LOGIN)
    .AddField(UFE_LOGIN_ID, "webuser")
    .AddField(UFE_LOGIN_PW, "5e884898da28047151d0e56f8dc");
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
```

`UFEMessage` and `UFEMessage.Builder` create NewOrderSingle with groups:

```c#
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
    .AddField(Side.tag, Side.BUY)
    .AddGroup(NoAllocs.tag, out g1, (builder, group) =>
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
```

## UFEedClient

The `UFEedClient` class is used as the interface to make both System and 
Business API calls to the UFEGW. Sessions between `UFEedClient` and the
UFEGW are made up of ZeroMQ PUB/SUB and REQ/REP sockets. The network
addresses and message topics inherent to these sockets are configurable
via `UFEedClient`. In addition, the `UFEedClient` manages these UFEGW
sessions on behalf of the user (after the user has successfully logged
in).

`UFEedClient` provides a event callback interface that is a part of
`UFEedClient` that must be subscribed by `UFEedClient` consumer:

```c#
class UFEedClient
{
...
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
}
```

`UFEedClient` is configured with `UFEconfiguration` class:

```c#
public class UFEedConfiguration
{
    /// <summary>
    /// Subscriber endpoint, defaults to "tcp://127.0.0.1:55745"
    /// </summary>
    public string Subscriber { get; set; };

    /// <summary>
    /// Requester endpoint, defaults to "tcp://127.0.0.1:55746"
    /// </summary>
    public string Requester { get; set; };

    /// <summary>
    /// Publisher endpoint, defaults to "tcp://*:55747"
    /// </summary>
    public string Publisher { get; set; };

    /// <summary>
    /// Responder endpoint, defaults to "tcp://*:55748"
    /// </summary>
    public string Responder { get; set; };

    /// <summary>
    /// Subscriber topic, defaults to "ufegw-publisher"
    /// </summary>
    public string SubscriberTopic { get; set; };

    /// <summary>
    /// Requester topic, defaults to "ufegw-responder"
    /// </summary>
    public string RequesterTopic { get; set; };

    /// <summary>
    /// Publisher topic, defaults to "ufeedclient-publisher"
    /// </summary>
    public string PublisherTopic { get; set; };

    /// <summary>
    /// Responder topic, defaults to "ufeedclient-responder"
    /// </summary>
    public string ResponderTopic { get; set; };

    /// <summary>
    /// Max IO threads for ZMQ background work, defaults to 1
    /// </summary>
    public int MaxIoThreads { get; set; ;

    /// <summary>
    /// Poll interval in milliseconds, defaults to 10
    /// </summary>
    public uint PollIntervalMs { get; set; };
}
```

`UFEedClient` interface:

```c#
public class UFEedClient : IDisposable
{
    /// <summary>
    /// Constructs UFEedClient
    /// </summary>
    /// <param name="configuration">configuration to setup UFEedClient object</param>
    public UFEedClient(UFEedConfiguration configuration);

    /// <summary>
    /// Disposes UFEedClient resources, implements IDisposable.Dispose()
    /// </summary>
    public void Dispose();

    /// <summary>
    /// Returns UFEedClient configuration
    /// </summary>
    public UFEedConfiguration Configuration { get; }

    /// <summary>
    /// Starts UFEedClient. When started in synchronous mode (wait = true)
    /// it does not return until Stop() is called from a different thread.
    /// </summary>
    /// <param name="wait">true for synchronous call, false for asynchronous</param>
    public void Start(bool wait = false);

    /// <summary>
    /// Stops UFEedClient
    /// </summary>
    public void Stop();

    /// <summary>
    /// Creates UFEMEssage from <c>WireMessage</c>
    /// </summary>
    /// <param name="wm"><c>WireMessage</c> to create from</param>
    /// <returns>new <c>UFEMessage</c></returns>
    public UFEMessage CreateMessage(WireMessage wm = null);

    /// <summary>
    /// Synchronously sends request to UFE and waits for UFE response, REQ/REP pattern
    /// </summary>
    /// <param name="request">Request to send </param>
    /// <returns>Response received</returns>
    /// <exception cref="UFEedClientException">Can throw UFEedClientException</exception>
    public UFEMessage Request(UFEMessage,Builder request);

    /// <summary>
    /// Send message to responder channel. Back channel pattern.
    /// IMPORTANT: Should be called as much as possible from within ResponderMessageReceived callback thread
    /// </summary>
    /// <param name="msg">Message to send</param>
    public void Respond(UFEMessage.Builder msg);
    
    /// <summary>
    /// Events
    /// </summary>
    public event EventHandler<UFEMessageEventArgs> SubscriptionMessageReceived;
    public event EventHandler<UFEMessageEventArgs> ResponderMessageReceived;
    public event EventHandler<UFEMessageEventArgs> ResponseMessageReceived;
    public event EventHandler<UFEAuthEventArgs> AuthenticateRequested; 
    public event EventHandler<UFEZeroMQErrorEventArgs> ZeroMQErrorHappened;
    public event EventHandler<UFEErrorEventArgs> ErrorHappened;
}
```

`UFEedClient` usage sample:

```c#
using (var uc = new UFEedClient(new UFEedConfiguration{Subscriber = SUBSCRIBER_DEFAULT}))
{
    uc.SubscriptionMessageReceived += (sender, args) => { ... };
    uc.ResponderMessageReceived += (sender, args) => { ... };
    uc.ZeroMQErrorHappened += (sender, args) => { ... };
    uc.ErrorHappened += (sender, args) => { ... };
    uc.Start();
    var login = uc.CreateMessage()
        .SetLongName("login")
        .SetType(StSystem)
        .SetServiceId(UFE_CMD_LOGIN)
        .AddField(UFE_CMD, UFE_CMD_LOGIN)
        .AddField(UFE_LOGIN_ID, "abcdef")
        .AddField(UFE_LOGIN_PW, "1e884898da28047151d0e56f8df");
    var response = uc.Request(login);
        ...
}
```

# Constants

The `UFEed_C#` maintains a list of constant values that translate to integer
codes in the UFEGW. These integer codes are used to identify System API
services as well as general FIX functionality. A full list of these
constants is available at `UFE/UFE.UFEedClient/UFEConsts.cs` file. 
Constants could be regenerated using `UFE.UFEedClient.GenConsts` project.

## FIX variants constants

The `UFEed_C#` provides constants for all stock FIX variants:

```c#
using UFE.UFEedClient.FIX50SP2.Field;
...
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
    .AddField(Side.tag, Side.BUY);
```

# Building

The `UFEed_C#` build follows standard .NetCore build pattern. Solution file `UFE.netcore.sln` contains all the projects to be build.

The `UFEed_C#` provides a sample to use as a starting point for UFEed C#
development. To build the sample, you have to open
`UFE.UFEedClient.Sample.netcore.csproj` provided in the installation
package and reference `UFE.UFEedClient` project.
