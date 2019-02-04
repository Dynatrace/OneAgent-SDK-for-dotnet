# Dynatrace OneAgent SDK for .NET

This SDK allows Dynatrace customers to instrument .NET applications. This is useful to enhance the visibility for proprietary frameworks
or custom frameworks not directly supported by [Dynatrace OneAgent](https://www.dynatrace.com/technologies/net-monitoring/) out-of-the-box.

This is the official .NET implementation of the [Dynatrace OneAgent SDK](https://github.com/Dynatrace/OneAgent-SDK).

## Table of Contents

* [Package contents](#package-contents)
* [Requirements](#requirements)
* [Integration](#integration)
  * [Dependencies](#dependencies)
  * [Troubleshooting](#troubleshooting)
* [API Concepts](#api-concepts)
  * [IOneAgentSDK object](#ioneagentsdk-object)
  * [Tracers](#tracers)
* [Features](#features)
  * [Trace SQL database requests](#trace-sql-database-requests)
  * [Trace remote calls](#trace-remote-calls)
  * [Trace messaging](#trace-messaging)
  * [Logging callback](#logging-callback)
* [Further reading](#further-readings)
* [Help & Support](#help--support)
* [Release notes](#release-notes)

## Package contents

* `samples`: sample application which demonstrates the usage of the SDK
* `src`: source code of the SDK (API and implementation stub - for reference only, not intended to be edited/extended/built by the user)
* `LICENSE`: license under which the SDK and sample applications are published

The SDK implementation is provided by the installed OneAgent at runtime. The classes in `src/DummyImpl` are a stub that is used if no
OneAgent is installed on the host so that your application is not affected by any missing OneAgent dependency.

## Requirements

* Dynatrace OneAgent (required versions see below)
* .NET Full Framework >= 4.5 or .NET Core >= 1.0 (the SDK is built using .NET Standard 1.0)

|OneAgent SDK for .NET|Required OneAgent version|
|:-----------------------|:------------------------|
|1.2.0                   |>=1.161                  |
|1.1.0                   |>=1.157                  |
|1.0.0-alpha             |1.153-1.155              |

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
  (see [Requirements](#requirements))
* process monitoring is enabled in Dynatrace
* you have set the OneAgent SDK [logging callback](#logging-callback) and check its output

## API Concepts

Common concepts of the Dynatrace OneAgent SDK are explained the [Dynatrace OneAgent SDK repository](https://github.com/Dynatrace/OneAgent-SDK).

### IOneAgentSdk object

Use `OneAgentSdkFactory.CreateInstance` to obtain an OneAgentSDK instance.
You should reuse this object over the whole application and if possible CLR lifetime:

```csharp
IOneAgentSdk oneAgentSdk = OneAgentSdkFactory.CreateInstance();
```

### Tracers

To trace any kind of call you first need to create a Tracer. The Tracer object represents the logical and physical endpoint that you want to call. A Tracer serves two purposes. First to time the call (duration, cpu and more) and report errors. That is why each Tracer has these three methods. The `Error` method must be called only once, and it must be in between `Start` and `End`. Each Tracer can only be used once and you need to create a new instance for each request/call that you want to trace (i.e., `Start` cannot be called twice on the same instance).

```csharp
void Start();

void Error(String message);

void End();
```

The Start method only supports synchronous methods (in other words C# methods without the async keyword). If you call Start() in an async method, then with high probability the SDK won't capture the specific data.

To support asynchronous methods (which are C# methods that are marked with the async keyword) the SDK offers a StartAsync() method.

Sample usage:

```csharp
public static async Task SampleMethodAsync()
{
    IOneAgentSdk oneAgentSdk = OneAgentSdkFactory.CreateInstance();
    IDatabaseInfo dbInfo = oneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
    IDatabaseRequestTracer dbTracer = oneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

    await dbTracer.StartAsync(); // instead of Start() we call the StartAsync() method
    try
    {
        await DatabaseApi.AsyncDatabaseCall();
    }
    catch
    {
        dbTracer.Error("DB call failed");
    }
    finally
    {
        dbTracer.End();
    }
}
```

Additionally the SDK also offers a convenient `Trace` method. This method can be called in both asynchronous and synchronous methods. In case of an async method you can pass the given async method to the `TraceAsync` method and await on the result of the `TraceAsync` method.

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

The `Trace` method internally calls the `Start` method and the `TraceAsync` method calls `StartAsync`. In case of an exception they also call the `Error` method. Both finally call the `End` method. Additionally, they also take care of collecting timing information across threads in case the C# async method is executed on multiple threads.

To summarize this, in case of

* synchronous methods you can either use the `Start`, `End` and `Error` methods, or the convenience method `Trace`,
* asynchronous methods you can either use the `StartAsync`, `End` and `Error` methods, or the convenience method `TraceAsync`.

To allow tracing across process and technology boundaries, tracers can be supplied with so-called tags. Tags are strings or byte arrays generated by the SDK that enable Dynatrace to trace a transaction end-to-end. The user has to take care of transporting the tag from one process to the other.

## Features

The feature sets differ slightly with each language implementation. More functionality will be added over time, see [Planned features for OneAgent SDK](https://answers.dynatrace.com/spaces/483/dynatrace-product-ideas/idea/198106/planned-features-for-oneagent-sdk.html) for details on upcoming features.

A more detailed specification of the features can be found in [Dynatrace OneAgent SDK](https://github.com/Dynatrace/OneAgent-SDK).

|Feature                                  |Required OneAgent SDK for .NET  version|
|:------                                  |:--------------------------------------|
|Trace messaging                          |>=1.2.0                                |
|Trace remote calls                       |>=1.1.0                                |
|Logging callback                         |>=1.1.0                                |
|Trace SQL database requests              |>=1.0.0-alpha                          |

### Trace SQL database requests

A SQL database request is traced by calling `TraceSQLDatabaseRequest`. See [DatabaseRequestTracerSamples.cs](/sample/Dynatrace.OneAgent.Sdk.Sample/DatabaseRequestTracerSamples.cs) for the full list of examples (sync/async/lambda/exception/...)

**Example synchronous database call (see [DatabaseRequestTracerSamples.cs](/sample/Dynatrace.OneAgent.Sdk.Sample/DatabaseRequestTracerSamples.cs) for more details):**

```csharp
IDatabaseInfo dbInfo = oneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
IDatabaseRequestTracer dbTracer = oneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

dbTracer.Start();
try
{
    ExecuteDbCallVoid();
}
catch
{
    dbTracer.Error("DB call failed");
    // handle or rethrow
}
finally
{
    dbTracer.End();
}
```

**Example asynchronous database call (see [DatabaseRequestTracerSamples.cs](/sample/Dynatrace.OneAgent.Sdk.Sample/DatabaseRequestTracerSamples.cs) for more details):**

```csharp
IDatabaseInfo dbInfo = oneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
IDatabaseRequestTracer dbTracer = oneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

await dbTracer.StartAsync();
try
{
    await ExecuteDbCallVoidAsync();
}
catch
{
    dbTracer.Error("DB call failed");
    // handle or rethrow
}
finally
{
    dbTracer.End();
}
```

**Example tracing database call in a async lambda expression (see [DatabaseRequestTracerSamples.cs](/sample/Dynatrace.OneAgent.Sdk.Sample/DatabaseRequestTracerSamples.cs) for more details):**

```csharp
IDatabaseInfo dbInfo = oneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
IDatabaseRequestTracer dbTracer = oneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

int res = dbTracer.Trace(() => ExecuteDbCallInt());
```

### Trace remote calls

You can use the SDK to trace proprietary IPC communication from one process to the other. This will enable you to see full Service Flow, PurePath and Smartscape topology for remoting technologies that Dynatrace is not aware of.

To trace any kind of remote call you first need to create a Tracer. The Tracer object represents the endpoint that you want to call, as such you need to supply the name of the remote service and remote method. In addition you need to transport the tag in your remote call to the server side if you want to trace it end-to-end.

```csharp
IOutgoingRemoteCallTracer outgoingRemoteCallTracer = oneAgentSdk.TraceOutgoingRemoteCall(
    "RemoteMethod", "RemoteServiceName",
    "mrcp://endpoint/service", ChannelType.TCP_IP, "myRemoteHost:1234");
outgoingRemoteCallTracer.SetProtocolName("MyRemoteCallProtocol");

outgoingRemoteCallTracer.Start();
try
{
    string tag = outgoingRemoteCallTracer.GetDynatraceStringTag();
    // make the call and transport the tag across to server
}
catch (Exception e)
{
    outgoingRemoteCallTracer.Error(e.Message);
    // handle or rethrow
}
finally
{
    outgoingRemoteCallTracer.End();
}
```

On the server side you need to wrap the handling and processing of your remote call as well. This will not only trace the server side call and everything that happens, it will also connect it to the calling side.

```csharp
IIncomingRemoteCallTracer incomingRemoteCallTracer = oneAgentSdk
    .TraceIncomingRemoteCall("RemoteMethod", "RemoteServiceName", "mrcp://endpoint/service");

string incomingDynatraceStringTag = ...; // retrieve from incoming call metadata
incomingRemoteCallTracer.SetDynatraceStringTag(incomingDynatraceStringTag);

incomingRemoteCallTracer.Start();
incomingRemoteCallTracer.SetProtocolName("MyRemoteCallProtocol");
try
{
    ProcessRemoteCall();
}
catch (Exception e)
{
    incomingRemoteCallTracer.Error(e.Message);
    // handle or rethrow
}
finally
{
    incomingRemoteCallTracer.End();
}
```

### Trace messaging

You can use the SDK to trace messages sent or received via messaging & queuing systems. When tracing messages, we distinguish between:

* sending a message
* receiving a message
* processing a received message

To trace an outgoing message, you need to create an `IMessagingSystemInfo` and call `TraceOutgoingMessage` with that instance:

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

    message.Headers[OneAgentSdkConstants.DYNATRACE_MESSAGE_PROPERTYNAME] = outgoingMessageTracer.GetDynatraceByteTag();

    SendResult result = MyMessagingSystem.SendMessage(message);

    outgoingMessageTracer.SetCorrelationId(message.CorrelationId);    // optional
    outgoingMessageTracer.SetVendorMessageId(result.VendorMessageId); // optional
}
catch(Exception ex)
{
    outgoingMessageTracer.Error(ex.Message);
    // handle or rethrow
    throw ex;
}
finally
{
    outgoingMessageTracer.End();
}
```

On the incoming side, we need to differentiate between the blocking receiving part and processing the received message. Therefore two
different tracers are used: `IIncomingMessageReceiveTracer` and `IIncomingMessageProcessTracer`.

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

    // start processing:
    IIncomingMessageProcessTracer processTracer = oneAgentSdk.TraceIncomingMessageProcess(messagingSystemInfo);
    if (message.Headers.ContainsKey(OneAgentSdkConstants.DYNATRACE_MESSAGE_PROPERTYNAME))
    {
        processTracer.SetDynatraceByteTag(message.Headers[OneAgentSdkConstants.DYNATRACE_MESSAGE_PROPERTYNAME]);
    }
    processTracer.Start();
    processTracer.SetCorrelationId(message.CorrelationId);           // optional
    processTracer.SetVendorMessageId(receiveResult.VendorMessageId); // optional
    try
    {
        ProcessMessage(message); // do the work ...
    }
    catch (Exception ex)
    {
        processTracer.Error(ex.Message);
        // handle or rethrow
        throw ex;
    }
    finally
    {
        processTracer.End();
    }
}
catch (Exception ex)
{
    receiveTracer.Error(ex.Message);
    // handle or rethrow
    throw ex;
}
finally
{
    receiveTracer.End();
}
```

In case of a non-blocking receive (e.g. via an event handler), there is no need to use `IIncomingMessageReceiveTracer` - just trace processing
of the message by using the `IIncomingMessageProcessTracer`:

```csharp
void OnMessageReceived(ReceiveResult receiveResult)
{
    string serverEndpoint = "messageserver.example.com:1234";
    string topic = "my-topic";
    IMessagingSystemInfo messagingSystemInfo = oneAgentSdk
        .CreateMessagingSystemInfo("MyCustomMessagingSystem", topic, MessageDestinationType.TOPIC, ChannelType.TCP_IP, serverEndpoint);

    Message message = receiveResult.Message;

    // start processing:
    IIncomingMessageProcessTracer processTracer = oneAgentSdk.TraceIncomingMessageProcess(messagingSystemInfo);
    if (message.Headers.ContainsKey(OneAgentSdkConstants.DYNATRACE_MESSAGE_PROPERTYNAME))
    {
        processTracer.SetDynatraceByteTag(message.Headers[OneAgentSdkConstants.DYNATRACE_MESSAGE_PROPERTYNAME]);
    }
    processTracer.Start();
    processTracer.SetCorrelationId(message.CorrelationId);           // optional
    processTracer.SetVendorMessageId(receiveResult.VendorMessageId); // optional
    try
    {
        ProcessMessage(message); // do the work ...
    }
    catch (Exception ex)
    {
        processTracer.Error(ex.Message);
        // handle or rethrow
        throw ex;
    }
    finally
    {
        processTracer.End();
    }
}
```

### Logging callback

The SDK provides a logging-callback to give information back to the calling application in case of an error. The user application has to provide a callback like the following:

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

## Further readings

* [What is the OneAgent SDK?](https://www.dynatrace.com/support/help/extend-dynatrace/oneagent-sdk/what-is-oneagent-sdk/) in the Dynatrace documentation
* [Feedback & Roadmap thread in AnswerHub](https://answers.dynatrace.com/spaces/483/dynatrace-product-ideas/idea/198106/planned-features-for-oneagent-sdk.html)

## Help & Support

The Dynatrace OneAgent SDK for .NET is in GA status. The features are fully supported by Dynatrace.

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
* Manage and resolve product related technical issues

SLAs apply according to the customer's support level.

## Release Notes

see also [Releases](https://github.com/Dynatrace/OneAgent-SDK-for-dotnet/releases)

|Version    |Description                                  |
|:----------|:--------------------------------------------|
|1.2.0      |Adds message tracing                         |
|1.1.0      |First GA release - starting with this version OneAgent SDK for .NET is now officially supported by Dynatrace|
|1.1.0-alpha|Adds remote call tracing and logging callback|
|1.0.0-alpha|EAP release                                  |
