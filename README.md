# Dynatrace OneAgent SDK for .NET

This SDK allows Dynatrace customers to instrument .NET applications. This is useful to enhance the visibility for
proprietary frameworks or custom frameworks not directly supported by
[Dynatrace OneAgent](https://www.dynatrace.com/technologies/net-monitoring/) out-of-the-box.

This is the official .NET implementation of the [Dynatrace OneAgent SDK](https://github.com/Dynatrace/OneAgent-SDK).

## Table of Contents

* [Package contents](#package-contents)
* [Requirements](#requirements)
* [Integration](#integration)
  * [Dependencies](#dependencies)
  * [Troubleshooting](#troubleshooting)
* [API Concepts](#api-concepts)
  * [IOneAgentSDK object](#ioneagentsdk-object)
  * [Trace Context](#tracecontext)
  * [Tracers](#tracers)
* [Features](#features)
  * [Trace SQL database requests](#trace-sql-database-requests)
  * [Trace remote calls](#trace-remote-calls)
  * [Trace messaging](#trace-messaging)
  * [Trace web requests](#trace-web-requests)
  * [In-process linking](#in-process-linking)
  * [Add custom request attributes](#add-custom-request-attributes)
  * [Logging callback](#logging-callback)
  * [SdkState and IOneAgentInfo](#sdkstate-and-ioneagentinfo)
* [Further reading](#further-readings)
* [Help & Support](#help--support)
* [Release notes](#release-notes)

## Package contents

* `samples`: sample application which demonstrates the usage of the SDK
* `src`: source code of the SDK (API and implementation stub - for reference only,
    not intended to be edited/extended/built by the user)
* `LICENSE`: license under which the SDK and sample applications are published

The SDK implementation is provided by the installed OneAgent at runtime. The classes in `src/DummyImpl` are a stub
that is used if no OneAgent is installed on the host so that your application is not affected by any
missing OneAgent dependency.

## Requirements

* Dynatrace OneAgent (required versions see below)
* .NET Full Framework >= 4.5 or .NET Core >= 1.0 (the SDK is built using .NET Standard 1.0)

|OneAgent SDK for .NET|Required OneAgent version|Support status|
|:-----------------------|:------------------------|:-----------------------|
| 1.8.0                  | >=1.233                 | Supported              |
| 1.7.0                  | >=1.179                 | Supported              |
| 1.6.0                  | >=1.173                 | Supported              |
| 1.5.0                  | >=1.171                 | Supported              |
| 1.4.0                  | >=1.167                 | Supported              |
| 1.3.0                  | >=1.165                 | Supported              |
| 1.2.0                  | >=1.161                 | Supported              |
| 1.1.0                  | >=1.157                 | Supported              |
| 1.0.0-alpha            | 1.153-1.155             | EAP (not supported)    |

## Integration

Using this SDK should not cause any errors if no OneAgent is present (e.g. in testing).

### Dependencies

If you want to integrate the OneAgent SDK into your application, just add the following NuGet dependency:

[Dynatrace.OneAgent.Sdk NuGet package](https://www.nuget.org/packages/Dynatrace.OneAgent.Sdk)

The Dynatrace OneAgent SDK for .NET has no further dependencies.

### Troubleshooting

Make sure that:

* OneAgent is installed on the host running your application
* the installed version of OneAgent is compatible with the SDK version you are using
  (see [Requirements](#requirements), check using [SdkState and IOneAgentInfo](#sdkstate-and-ioneagentinfo))
* process monitoring is enabled in Dynatrace
* you have set the OneAgent SDK [logging callback](#logging-callback) and check its output

Also note that injection of the OneAgent code module will not work if you start your application from within Visual
Studio. In that case the SDK will behave as if OneAgent is not installed. If you need to debug your application with
the code module injected, you can start the application from Explorer or `cmd.exe` and then attach Visual Studio as
debugger to the running process (Debug -> Attach to Process...).

## API Concepts

Common concepts of the Dynatrace OneAgent SDK are explained the
[Dynatrace OneAgent SDK repository](https://github.com/Dynatrace/OneAgent-SDK).

### IOneAgentSdk object

Use `OneAgentSdkFactory.CreateInstance` to obtain an instance of `IOneAgentSDK`, which is used to create tracers
and info objects.
You should reuse this object over the whole application and if possible CLR lifetime:

```csharp
IOneAgentSdk oneAgentSdk = OneAgentSdkFactory.CreateInstance();
```

<a name="tracecontext"></a>

## Trace Context

An instance of the `OneAgentSDK` can be used to get the current `ITraceContextInfo` which holds information
about the *Trace-Id* and *Span-Id* of the current PurePath node.
This information can then be used to provide e.g. additional context in log messages.

Please note that `TraceContextInfo` is not intended for tagging or context-propagation use cases.
Dedicated APIs (e.g. [remote calls](#remoting) or [web requests](#webreqeusts)) as well as
built-in OneAgent sensors take care of linking services correctly.

```csharp
ITraceContextInfo traceContextInfo = oneAgentSdk.TraceContextInfo;
string traceId = traceContextInfo.TraceId;
string spanId = traceContextInfo.SpanId;
_logger.LogInformation($"[!dt dt.trace_id={traceId},dt.span_id={spanId}] sending request ...");
```

### Tracers

To trace any kind of call you first need to create a Tracer. The Tracer object represents the logical and physical
endpoint that you want to call. A Tracer serves two purposes. First to time the call (duration, cpu and more) and
report errors. That is why each Tracer has these four methods. Either one of the `Error` methods must be called at
most once, and it must be in between `Start` and `End`. Each Tracer can only be used once and you need to create a
new instance for each request/call that you want to trace (i.e., `Start` cannot be called twice on the same instance).

```csharp
void Start();

void Error(Exception exception);

void Error(String message);

void End();
```

The `Start` method only supports synchronous methods (in other words C# methods without the `async` keyword).
If you call `Start()` in an async method, then with high probability the SDK won't capture the specific data.

Sample usage:

```csharp
public static async Task SampleMethodAsync()
{
    IOneAgentSdk oneAgentSdk = OneAgentSdkFactory.CreateInstance();
    IDatabaseInfo dbInfo = oneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
    IDatabaseRequestTracer dbTracer = oneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

    await dbTracer.Start();
    try
    {
        DatabaseApi.DatabaseCall();
    }
    catch (Exception e)
    {
        dbTracer.Error(e);
        // handle or rethrow
    }
    finally
    {
        dbTracer.End();
    }
}

```

> **Note:** Previous versions of the OneAgent SDK supported tracing of asynchronous methods (which are C# methods
> that are marked with the `async` keyword) using the method `StartAsync()`. This method has been **removed** for
> technical reasons and the way of tracing asynchronous code is the `TraceAsync` method (see below).

Additionally the SDK also offers a convenient `Trace` method. This method can be called in both asynchronous and
synchronous methods. In case of an async method you can pass the given async method to the `TraceAsync` method and
await on the result of the `TraceAsync` method.

```csharp
void Trace(Action action);

T Trace<T>(Func<T> func);

Task TraceAsync(Func<Task> func);

Task<T> TraceAsync<T>(Func<Task<T>> func);
```

Sample usage:

```csharp
public static async Task SampleMethodAsync()
{
    IOneAgentSdk oneAgentSdk = OneAgentSdkFactory.CreateInstance();
    IDatabaseInfo dbInfo = oneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
    IDatabaseRequestTracer dbTracer = oneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

    var result = await dbTracer.TraceAsync(() => DatabaseApi.AsyncDatabaseCall());
}
```

The `Trace` method internally calls the `Start` method and the `TraceAsync` method calls `StartAsync`.
In case of an exception they also call the `Error` method. Both finally call the `End` method.
Additionally, they also take care of collecting timing information across threads in case the C# async method
is executed on multiple threads.

To summarize this, in case of

* synchronous methods you can either use the `Start`, `End` and `Error` methods, or the convenience method `Trace`,
* asynchronous methods you can use the `TraceAsync` method.

Some tracers offer methods to provide information in addition to the parameters required for creating the tracer using
the `IOneAgentSdk` object. These additional pieces of information might be relevant for service detection and naming.
If this is the case, they can only be set *before* starting the tracer as stated in the respective method documentation.
After ending a tracer, it must not be used any longer. Since none of its methods must be called, no further information
can be provided to an ended tracer.

To allow tracing across process and technology boundaries, tracers can be supplied with so-called tags.
Tags are strings or byte arrays generated by the SDK that enable Dynatrace to trace a transaction end-to-end.
The user has to take care of transporting the tag from one process to the other.

## Features

The feature sets differ slightly with each language implementation. More functionality will be added over time, see
[Planned features for OneAgent SDK](https://answers.dynatrace.com/spaces/483/dynatrace-product-ideas/idea/198106/planned-features-for-oneagent-sdk.html)
for details on upcoming features.

A more detailed specification of the features can be found in
[Dynatrace OneAgent SDK](https://github.com/Dynatrace/OneAgent-SDK).

|Feature                                                                         |Required OneAgent SDK for .NET  version|
|:-------------------------------------------------------------------------------|:--------------------------------------|
|ITraceContextInfo                                                               |>=1.8.0                                |
|Support for W3C Trace Context (`IOutgoingWebRequestTracer.InjectTracingHeaders`)|>=1.6.0                                |
|Trace incoming web requests                                                     |>=1.5.0                                |
|Trace outgoing web requests                                                     |>=1.4.0                                |
|Custom request attributes                                                       |>=1.4.0                                |
|In-process linking, `SdkState` and `IOneAgentInfo`                              |>=1.3.0                                |
|Trace messaging                                                                 |>=1.2.0                                |
|Trace remote calls                                                              |>=1.1.0                                |
|Logging callback                                                                |>=1.1.0                                |
|Trace SQL database requests                                                     |>=1.0.0-alpha                          |

### Trace SQL database requests

A SQL database request is traced by calling `TraceSQLDatabaseRequest`.
See [DatabaseRequestTracerSamples.cs](/sample/Dynatrace.OneAgent.Sdk.Sample/DatabaseRequestTracerSamples.cs)
for the full list of examples (sync/async/lambda/exception/...)

**Example of a synchronous database call (see [DatabaseRequestTracerSamples.cs](/sample/Dynatrace.OneAgent.Sdk.Sample/DatabaseRequestTracerSamples.cs) for more details):**

```csharp
IDatabaseInfo dbInfo = oneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
IDatabaseRequestTracer dbTracer = oneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

dbTracer.Start();
try
{
    ExecuteDbCallVoid();
}
catch (Exception e)
{
    dbTracer.Error(e);
    // handle or rethrow
}
finally
{
    dbTracer.End();
}
```

**Example of an asynchronous database call (see [DatabaseRequestTracerSamples.cs](/sample/Dynatrace.OneAgent.Sdk.Sample/DatabaseRequestTracerSamples.cs) for more details):**

```csharp
IDatabaseInfo dbInfo = oneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
IDatabaseRequestTracer dbTracer = oneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

await dbTracer.StartAsync();
try
{
    await ExecuteDbCallVoidAsync();
}
catch (Exception e)
{
    dbTracer.Error(e);
    // handle or rethrow
}
finally
{
    dbTracer.End();
}
```

**Example of tracing database call in an async lambda expression (see [DatabaseRequestTracerSamples.cs](/sample/Dynatrace.OneAgent.Sdk.Sample/DatabaseRequestTracerSamples.cs) for more details):**

```csharp
IDatabaseInfo dbInfo = oneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
IDatabaseRequestTracer dbTracer = oneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

int res = dbTracer.Trace(() => ExecuteDbCallInt());
```

See also our initial blog post about the OneAgent SDK for .NET, which shows how databases can be traced using the
`IDatabaseRequestTracer` and how these traces are presented and analyzed in Dynatrace:
[Extend framework support with OneAgent SDK for .NET](https://www.dynatrace.com/news/blog/extend-framework-support-with-oneagent-sdk-for-net-eap/)

Please note that SQL database traces are only created if they occur within some other SDK trace (e.g. incoming remote call)
or a OneAgent built-in trace (e.g. incoming web request).

### Trace remote calls

You can use the SDK to trace proprietary IPC communication from one process to the other.
This will enable you to see full Service Flow, PurePath and Smartscape topology for remoting technologies
that Dynatrace is not aware of.

To trace any kind of remote call you first need to create a Tracer. The Tracer object represents the endpoint
that you want to call, as such you need to supply the name of the remote service and remote method.
In addition you need to transport the tag in your remote call to the server side if you want to trace it end-to-end.

```csharp
IOutgoingRemoteCallTracer outgoingRemoteCallTracer = oneAgentSdk.TraceOutgoingRemoteCall(
    "RemoteMethod", "RemoteServiceName",
    "mrcp://endpoint/service", ChannelType.TCP_IP, "myRemoteHost:1234");
outgoingRemoteCallTracer.SetProtocolName("MyRemoteCallProtocol");

outgoingRemoteCallTracer.Start();
try
{
    string tag = outgoingRemoteCallTracer.GetDynatraceStringTag();
    // make the call and transport the tag across to the server to link both sides of the remote call together
}
catch (Exception e)
{
    outgoingRemoteCallTracer.Error(e);
    // handle or rethrow
}
finally
{
    outgoingRemoteCallTracer.End();
}
```

On the server side you need to wrap the handling and processing of your remote call as well.
This will not only trace the server side call and everything that happens, it will also connect it to the calling side.

```csharp
IIncomingRemoteCallTracer incomingRemoteCallTracer = oneAgentSdk
    .TraceIncomingRemoteCall("RemoteMethod", "RemoteServiceName", "mrcp://endpoint/service");

string incomingDynatraceStringTag = ...; // retrieve from incoming call metadata
 // link both sides of the remote call together
incomingRemoteCallTracer.SetDynatraceStringTag(incomingDynatraceStringTag);
incomingRemoteCallTracer.SetProtocolName("MyRemoteCallProtocol");

incomingRemoteCallTracer.Start();
try
{
    ProcessRemoteCall();
}
catch (Exception e)
{
    incomingRemoteCallTracer.Error(e);
    // handle or rethrow
}
finally
{
    incomingRemoteCallTracer.End();
}
```

### Trace messaging

You can use the SDK to trace messages sent or received via messaging & queuing systems.
When tracing messages, we distinguish between:

* sending a message
* receiving a message
* processing a received message

To trace an outgoing message, you need to create an `IMessagingSystemInfo` and call `TraceOutgoingMessage`
with that instance:

```csharp
string serverEndpoint = "messageserver.example.com:1234";
string topic = "my-topic";
IMessagingSystemInfo messagingSystemInfo = oneAgentSdk
    .CreateMessagingSystemInfo("MyCustomMessagingSystem", topic, MessageDestinationType.TOPIC, ChannelType.TCP_IP, serverEndpoint);

IOutgoingMessageTracer outgoingMessageTracer = oneAgentSdk.TraceOutgoingMessage(messagingSystemInfo);

outgoingMessageTracer.Start();
try
{
    Message message = new Message();
    message.CorrelationId = "my-correlation-id-1234"; // optional, determined by application

    // transport the Dynatrace tag along with the message to allow the outgoing message tracer to be linked
    // with the message processing tracer on the receiving side
    message.Headers[OneAgentSdkConstants.DYNATRACE_MESSAGE_PROPERTYNAME] = outgoingMessageTracer.GetDynatraceByteTag();

    SendResult result = MyMessagingSystem.SendMessage(message);

    outgoingMessageTracer.SetCorrelationId(message.CorrelationId);    // optional
    outgoingMessageTracer.SetVendorMessageId(result.VendorMessageId); // optional
}
catch (Exception e)
{
    outgoingMessageTracer.Error(e);
    // handle or rethrow
    throw e;
}
finally
{
    outgoingMessageTracer.End();
}
```

On the incoming side, we need to differentiate between the blocking receiving part and processing the received message.
Therefore two different tracers are used: `IIncomingMessageReceiveTracer` and `IIncomingMessageProcessTracer`.

```csharp
string serverEndpoint = "messageserver.example.com:1234";
string topic = "my-topic";
IMessagingSystemInfo messagingSystemInfo = oneAgentSdk
    .CreateMessagingSystemInfo("MyCustomMessagingSystem", topic, MessageDestinationType.TOPIC, ChannelType.TCP_IP, serverEndpoint);

IIncomingMessageReceiveTracer receiveTracer = oneAgentSdk.TraceIncomingMessageReceive(messagingSystemInfo);

receiveTracer.Start();
try
{
    // blocking call until message is available:
    ReceiveResult receiveResult = MyMessagingSystem.ReceiveMessage();
    Message message = receiveResult.Message;

    IIncomingMessageProcessTracer processTracer = oneAgentSdk.TraceIncomingMessageProcess(messagingSystemInfo);

    // retrieve Dynatrace tag created using the outgoing message tracer to link both sides together:
    if (message.Headers.ContainsKey(OneAgentSdkConstants.DYNATRACE_MESSAGE_PROPERTYNAME))
    {
        processTracer.SetDynatraceByteTag(message.Headers[OneAgentSdkConstants.DYNATRACE_MESSAGE_PROPERTYNAME]);
    }
    // start processing:
    processTracer.Start();
    processTracer.SetCorrelationId(message.CorrelationId);           // optional
    processTracer.SetVendorMessageId(receiveResult.VendorMessageId); // optional
    try
    {
        ProcessMessage(message); // do the work ...
    }
    catch (Exception e)
    {
        processTracer.Error(e);
        // handle or rethrow
        throw e;
    }
    finally
    {
        processTracer.End();
    }
}
catch (Exception e)
{
    receiveTracer.Error(e);
    // handle or rethrow
    throw e;
}
finally
{
    receiveTracer.End();
}
```

In case of a non-blocking receive (e.g. via an event handler), there is no need to use
`IIncomingMessageReceiveTracer` - just trace processing of the message by using the `IIncomingMessageProcessTracer`:

```csharp
void OnMessageReceived(ReceiveResult receiveResult)
{
    string serverEndpoint = "messageserver.example.com:1234";
    string topic = "my-topic";
    IMessagingSystemInfo messagingSystemInfo = oneAgentSdk
        .CreateMessagingSystemInfo("MyCustomMessagingSystem", topic, MessageDestinationType.TOPIC, ChannelType.TCP_IP, serverEndpoint);

    Message message = receiveResult.Message;

    IIncomingMessageProcessTracer processTracer = oneAgentSdk.TraceIncomingMessageProcess(messagingSystemInfo);

    // retrieve Dynatrace tag created using the outgoing message tracer to link both sides together:
    if (message.Headers.ContainsKey(OneAgentSdkConstants.DYNATRACE_MESSAGE_PROPERTYNAME))
    {
        processTracer.SetDynatraceByteTag(message.Headers[OneAgentSdkConstants.DYNATRACE_MESSAGE_PROPERTYNAME]);
    }
    // start processing:
    processTracer.Start();
    processTracer.SetCorrelationId(message.CorrelationId);           // optional
    processTracer.SetVendorMessageId(receiveResult.VendorMessageId); // optional
    try
    {
        ProcessMessage(message); // do the work ...
    }
    catch (Exception e)
    {
        processTracer.Error(e);
        // handle or rethrow
        throw e;
    }
    finally
    {
        processTracer.End();
    }
}
```

See also:

* The documentation on [messaging tracers in the specification repository](https://github.com/Dynatrace/OneAgent-SDK#messaging).
* This blog post explaining [end-to-end tracing for additional message queues with the OneAgent SDK
](https://www.dynatrace.com/news/blog/end-to-end-tracing-for-additional-message-queues-with-oneagent-sdk/).

### Trace web requests

#### Trace outgoing web requests

You can use the SDK to trace outgoing web requests. This allows tracing web requests performed using HTTP client
libraries which are not supported by the Dynatrace OneAgent out-of-the-box.
Always include the Dynatrace header with the request as it is required to match the request on the server side.
This ensures requests are traced end-to-end when the server is monitored using a OneAgent or OneAgent SDK.

```csharp
IOutgoingWebRequestTracer tracer = SampleApplication.OneAgentSdk.TraceOutgoingWebRequest(request.Url, request.Method);

foreach (KeyValuePair<string, string> header in request.Headers)
{
    tracer.AddRequestHeader(header.Key, header.Value);
}

await tracer.TraceAsync(async () =>
{
    // add the Dynatrace tag or W3C Trace Context (based on your configuration) to request headers to allow
    // the agent in the web server to link the request together for end-to-end tracing
    tracer.InjectTracingHeaders((key, value) => request.Headers[key] = value);

    MyCustomHttpResponse response = await request.ExecuteAsync();

    tracer.SetStatusCode(response.StatusCode);

    foreach (KeyValuePair<string, string> header in response.Headers)
    {
        tracer.AddResponseHeader(header.Key, header.Value);
    }
});
```

#### Trace incoming web requests

You can use the SDK to trace incoming web requests. This might be useful if Dynatrace does not support
the web server framework or language processing the incoming web requests.

To trace an incoming web request you first need to create an `IWebApplicationInfo` object.
This info object represents the endpoint of your web server (web server name, application name and context root; see our
[documentation](https://www.dynatrace.com/support/help/how-to-use-dynatrace/services-and-transactions/basic-concepts/service-detection-and-naming/#web-request-services)
for further information).
This object should be reused for all traced web requests within the same application.

```csharp
IWebApplicationInfo webAppInfo =
    oneAgentSdk.CreateWebApplicationInfo("WebShopProduction", "AuthenticationService", "/api/auth");
//                                       Web server name      Application ID           Context Root
```

To trace an incoming web request you then need to create a Tracer object.
Make sure you provide all HTTP headers from the request to the SDK by calling `AddRequestHeader`.
This allows both sides of the web requests to be linked together for end-to-end tracing.

```csharp
IIncomingWebRequestTracer tracer = oneAgentSdk.TraceIncomingWebRequest(webAppInfo, request.Url, request.Method);
tracer.SetRemoteAddress(request.RemoteClientAddress);

// adding all request headers ensures that tracing headers required
// for end-to-end linking of requests are provided to the SDK
foreach (KeyValuePair<string, string> header in request.Headers)
{
    tracer.AddRequestHeader(header.Key, header.Value);
}
foreach (KeyValuePair<string, string> param in request.PostParameters)
{
    tracer.AddParameter(param.Key, param.Value);
}

// start tracer
return tracer.Trace(() =>
{
    // handle request and build response ...

    foreach (KeyValuePair<string, string> header in response.Headers)
    {
        tracer.AddResponseHeader(header.Key, header.Value);
    }
    tracer.SetStatusCode(response.StatusCode);

    return response;
});
```

### In-process linking

In order to trace interactions between different threads, so-called in-process links are used.
An in-process link is created on the originating thread and then used for creating an `IInProcessLinkTracer`
on the target thread.

Calls detected while the tracer is active (i.e., between `Start` and `End` or within any of the `Trace` methods) are
traced as part of the originating service call.
This works for calls detected out-of-the-box by the OneAgent as well as calls traced using the OneAgent SDK.

```csharp
// create an in-process link on the originating thread
IInProcessLink inProcessLink = oneAgentSdk.CreateInProcessLink();

// delegate work to another thread, in this case we use a custom background worker implementation
customBackgroundWorker.EnqueueWorkItem(() =>
{
    // use the in-process link to link the PurePath on the target thread to its origin
    IInProcessLinkTracer inProcessLinkTracer = oneAgentSdk.TraceInProcessLink(inProcessLink);
    inProcessLinkTracer.Start();
    // processing and performing further calls...
    inProcessLinkTracer.End();

    // calls executed after ending the IInProcessLinkTracer will
    // *not* be traced as part of the originating service call
});
```

Note that you can re-use in-process links to create multiple in-process link tracers.

### Add custom request attributes

You can use the SDK to add custom request attributes to the currently traced service call.
These attributes (key-value pairs) can be used to search and filter requests in Dynatrace.

In order to add a custom request attribute, the `AddCustomRequestAttribute` methods are used.
No reference to a tracer is needed as OneAgent SDK will select the currently active PurePath.
This may be a PurePath created by OneAgent SDK or a PurePath created by built-in sensors of the OneAgent.
The methods take two arguments - a key, specifying the name of the attribute, and a value,
which can be either a `string`, `long` or `double`.
These methods can be called several times to add multiple attributes to the same request.
If the same attribute key is used several times, all values will be recorded.

```csharp
oneAgentSdk.AddCustomRequestAttribute("region", "EMEA");
oneAgentSdk.AddCustomRequestAttribute("salesAmount", 2500);
oneAgentSdk.AddCustomRequestAttribute("service-quality", 0.707106);

oneAgentSdk.AddCustomRequestAttribute("account-group", 1);
oneAgentSdk.AddCustomRequestAttribute("account-group", 2);
oneAgentSdk.AddCustomRequestAttribute("account-group", 3);
```

If no service call is currently being traced, the attributes will be discarded.
Therefore, for calls traced with OneAgent SDK, custom request attributes have to be added *after*
starting the tracer (or from within an `ITracer.Trace` method) in order to have an active PurePath.  
Strings exceeding the lengths specified [here](https://github.com/Dynatrace/OneAgent-SDK#string-length)
will be truncated.

See also our blog post explaining how custom request attributes are configured, displayed and
analyzed in Dynatrace:
[Capture any request attributes using OneAgent SDK
](https://www.dynatrace.com/news/blog/capture-any-request-attributes-using-oneagent-sdk/)

### Logging callback

The SDK provides a logging-callback to give information back to the calling application in case of an error.
The user application has to provide a callback like the following:

```csharp
class StdErrLoggingCallback : ILoggingCallback
{
    public void Error(string message) => Console.Error.WriteLine("[OneAgent SDK] Error:   " + message);
    public void Warn (string message) => Console.Error.WriteLine("[OneAgent SDK] Warning: " + message);
}

public static void Main(string[] args)
{
    IOneAgentSdk oneAgentSdk = OneAgentSdkFactory.CreateInstance();
    var loggingCallback = new StdErrLoggingCallback();
    oneAgentSdk.SetLoggingCallback(loggingCallback);
}
```

In general it is a good idea to forward these logging events to your application specific logging framework.

### SdkState and IOneAgentInfo

For troubleshooting and avoiding any ineffective tracing calls you can check the state of the SDK as follows:

```csharp
IOneAgentSdk oneAgentSdk = OneAgentSdkFactory.CreateInstance();
SdkState state = oneAgentSdk.CurrentState;
switch (state)
{
    case SdkState.ACTIVE:               // SDK ready for use
    case SdkState.TEMPORARILY_INACTIVE: // capturing disabled, tracing calls can be spared
    case SdkState.PERMANENTLY_INACTIVE: // SDK permanently inactive, tracing calls can be spared
}
```

It is good practice to check the SDK state regularly as it may change at every point of time
(except PERMANENTLY_INACTIVE, which never changes over application lifetime).

Information about the OneAgent used by the SDK can be retrieved using `IOneAgentInfo`:

```csharp
IOneAgentSdk oneAgentSdk = OneAgentSdkFactory.CreateInstance();
IOneAgentInfo agentInfo = oneAgentSdk.AgentInfo;
if (agentInfo.AgentFound)
{
    Console.WriteLine($"OneAgent Version: {agentInfo.Version}");
    if (agentInfo.AgentCompatible)
    {
        // agent is fully compatible with current SDK version
    }
}
```

See [SdkState.cs](./src/Api/Enums/SdkState.cs) and [IOneAgentInfo.cs](./src/Api/Infos/IOneAgentInfo.cs)
for further information.

## Further readings

* [What is the OneAgent SDK?](https://www.dynatrace.com/support/help/extend-dynatrace/oneagent-sdk/what-is-oneagent-sdk/) in the Dynatrace documentation
* [Feedback & Roadmap thread in AnswerHub](https://answers.dynatrace.com/spaces/483/dynatrace-product-ideas/idea/198106/planned-features-for-oneagent-sdk.html)

## Help & Support

**Support policy**

The Dynatrace OneAgent SDK for .NET has GA status. The features are fully supported by Dynatrace.

For detailed support policy see [Dynatrace OneAgent SDK help](https://github.com/Dynatrace/OneAgent-SDK#help).

### Get Help

* Ask a question in the [product forums](https://answers.dynatrace.com/spaces/482/view.html)
* Read the [product documentation](https://www.dynatrace.com/support/help/)

### Open a [GitHub issue](https://github.com/Dynatrace/OneAgent-SDK-for-dotnet/issues) to

* Report minor defects, minor items or typos
* Ask for improvements or changes in the SDK API
* Ask any questions related to the community effort

SLAs don't apply for GitHub tickets

### Customers can open a ticket on the [Dynatrace support portal](https://support.dynatrace.com/supportportal/) to

* Get support from the Dynatrace technical support engineering team
* Manage and resolve product-related technical issues

SLAs apply according to the customer's support level.

## Release Notes

see also [Releases](https://github.com/Dynatrace/OneAgent-SDK-for-dotnet/releases)

|Version    |Description                                  |
|:----------|:--------------------------------------------|
|1.8.0      |Removes deprecated APIs and types. Adds `TraceContextInfo`. |
|1.7.1      |Deprecates metrics-related types and APIs |
|1.7.0      |Adds metrics support (preview only) and deprecates `ITracer.StartAsync` API method |
|1.6.0      |Adds W3C Trace Context support (`IOutgoingWebRequestTracer.InjectTracingHeaders`)|
|1.5.0      |Adds incoming web request tracing |
|1.4.0      |Adds custom request attributes and outgoing web request tracing |
|1.3.0      |Adds in-process linking, `ITracer.Error(Exception)`, `SdkState` and `IOneAgentInfo` |
|1.2.0      |Adds message tracing                         |
|1.1.0      |First GA release - starting with this version OneAgent SDK for .NET is now officially supported by Dynatrace|
|1.1.0-alpha|Adds remote call tracing and logging callback|
|1.0.0-alpha|EAP release                                  |
