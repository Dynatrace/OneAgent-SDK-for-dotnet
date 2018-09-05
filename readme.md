**Disclaimer: This SDK is currently in EAP and still work in progress.**

# Dynatrace OneAgent SDK for .NET

This SDK allows Dynatrace customers to instrument .NET applications. This is useful to enhance the visibility for proprietary frameworks or custom frameworks not directly supported by [Dynatrace OneAgent](https://www.dynatrace.com/technologies/net-monitoring/) out-of-the-box.

This is the official .NET implementation of the [Dynatrace OneAgent SDK](https://github.com/Dynatrace/OneAgent-SDK). 

## Table of Contents

* [Package contents](#package-contents)
* [Requirements](#requirements)
* [Integration](#integration)
  * [Dependencies](#dependencies)
  * [Troubleshooting](#troubleshooting)
* [API Concepts](#api-concepts)
  * [OneAgentSDK object](#oneagentsdk-object)
  * [Tracers](#tracers)
* [Features](#features)
  * [Trace SQL database requests](#trace-sql-database-requests)
* [Administrative Apis](#administrative-apis)
* [Further reading](#further-readings)
* [Help & Support](#help-support)
* [Release notes](#release-notes)

## Package contents

* `samples`: sample applications, which demonstrates the usage of the SDK.
* `src`: source code of the SDK
* `LICENSE`: license under which the whole SDK and sample applications are published

## Requirements

* Dynatrace OneAgent (required versions see below)
* Any .NET Full framework or .NET Core version that supports .NET Standard 1.0

|OneAgent SDK for .NET|Required OneAgent version|
|:-----------------------|:------------------------|
|1.0.x                   |>=1.153                  |

## Integration

Using this SDK should not cause any errors if no OneAgent is present (e.g. in testing).

### Dependencies

If you want to integrate the OneAgent SDK into your application, just add the following NuGet dependency:

[Dynatrace.OneAgent.Sdk NuGet package](https://www.nuget.org/packages/Dynatrace.OneAgent.Sdk)

The Dynatrace OneAgent SDK for .NET has no further dependencies.


### Troubleshooting

* Make sure OneAgent is installed and running on the host monitoring your application
* Make sure process monitoring is enabled

## API Concepts

Common concepts of the Dynatrace OneAgent SDK are explained the [Dynatrace OneAgent SDK repository](https://github.com/Dynatrace/OneAgent-SDK).

### OneAgentSDK object

Use OneAgentSDKFactory.CreateInstance() to obtain an OneAgentSDK instance. You should reuse this object over the whole application and if possible CLR lifetime:

```csharp
OneAgentSdk oneAgentSdk = OneAgentSdkFactory.CreateInstance();
```


### Tracers

To trace any kind of call you first need to create a Tracer. The Tracer object represents the logical and physical endpoint that you want to call. A Tracer serves two purposes. First to time the call (duraction, cpu and more) and report errors. That is why each Tracer has these three methods. The error method must be called only once, and it must be in between start and end.


```csharp
void Start();

void Error(String message);

void End();
```

The Start method only supports synchronous methods (in other words C# methods without the async keyword). If you call Start() in an async method, then with high probability the SDK won’t capture the specific data.

To support asynchronous methods (which are C# methods that are marked with the async keyword) the SDK offers a StartAsync() method. 

Sample usage:

```csharp
public static async Task SampleMethodAsync()
{
	var instance = OneAgentSdkFactory.CreateInstance();
	var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
	var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");

	dbTracer.StartAsync(); //instead of Start() we call the StartAsync() method
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
Additionally the .NET OneAgent SDK also offers a convenient Trace() method with multiple overloads. This method can be called in both asynchronous and synchronous methods. In case of an async method you can pass the given async method to the Trace method and await on the result of the Trace method.

Sample usage:

```csharp
public static async Task SampleMethodAsync()
{
	var instance = OneAgentSdkFactory.CreateInstance();
	var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor",  Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
	var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");

	var result = await dbTracer.Trace(() => DatabaseApi.AsyncDatabaseCall());
}

```
The Trace method internally calls the Start or StartAsync and the End method, and in case of an exception it also calls the Error method. Additionally, it also takes care of collecting timing information across threads in case of the C# async method is executed on multiple threads.

The Trace method with its overloads supports both synchronous and asynchronous methods.
So, to summarize this, in case of 
* synchronous methods you can either use the Start(), End(), and Error() methods, or the convenient Trace() method,
* asynchronous methods you can either use the StartAsync(), End(), and Error() methods, or the convenient Trace() method. 


The second purpose of a Tracer is to allow tracing across process boundaries.


## Features

The feature sets differ slightly with each language implementation. More functionality will be added over time, see [Planned features for OneAgent SDK](https://answers.dynatrace.com/spaces/483/dynatrace-product-ideas/idea/198106/planned-features-for-oneagent-sdk.html) for details on upcoming features.

A more detailed specification of the features can be found in [Dynatrace OneAgent SDK](https://github.com/Dynatrace/OneAgent-SDK).

|Feature                                  |Required OneAgent SDK for .NET  version|
|:------                                  |:----------------------------------------|
|Trace SQL database requests              |>=1.0.0                                  |



### Trace SQL database requests

A SQL database request is traced by calling TraceSQLDatabaseRequest(). See [DatabaseRequestTracerSamples.cs](/sample/Dynatrace.OneAgent.Sdk.Sample/DatabaseRequestTracerSamples.cs) for the full list of examples (sync/async/lambda/exception/...)

**Example synchronous database call (see [DatabaseRequestTracerSamples.cs](/sample/Dynatrace.OneAgent.Sdk.Sample/DatabaseRequestTracerSamples.cs) for more details):**

```csharp
var instance = OneAgentSdkFactory.CreateInstance();

var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");

dbTracer.Start();
try
{
    VoidMethod();
}
catch
{
    dbTracer.Error("DB call failed");
}
finally
{
    dbTracer.End();
}
```

**Example asynchronous database call (see [DatabaseRequestTracerSamples.cs](/sample/Dynatrace.OneAgent.Sdk.Sample/DatabaseRequestTracerSamples.cs) for more details):**

```csharp
var instance = OneAgentSdkFactory.CreateInstance();
var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");

dbTracer.StartAsync();
try
{
    await VoidTask();
}
catch
{
    dbTracer.Error("DB call failed");
}
finally
{
    dbTracer.End();
}
```

**Example tracing database call in a async lambda expression (see [DatabaseRequestTracerSamples.cs](/sample/Dynatrace.OneAgent.Sdk.Sample/DatabaseRequestTracerSamples.cs) for more details):**

```csharp
var instance = OneAgentSdkFactory.CreateInstance();
var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");

var res = await dbTracer.Trace(() => IntTask());
```



### Administrative Apis

not yet available

## Further readings

* [What is the OneAgent SDK?](https://www.dynatrace.com/support/help/extend-dynatrace/oneagent-sdk/what-is-oneagent-sdk/) in the Dynatrace documentation
* [Feedback & Roadmap thread in AnswerHub](https://answers.dynatrace.com/spaces/483/dynatrace-product-ideas/idea/198106/planned-features-for-oneagent-sdk.html)

## Help & Support

The Dynatrace OneAgent SDK for .NET is an open source project, currently in early access status (EAP).

### Get Help

* Ask a question in the [product forums](https://answers.dynatrace.com/spaces/482/view.html)
* Read the [product documentation](https://www.dynatrace.com/support/help/)

### Open a [GitHub issue](https://github.com/Dynatrace/OneAgent-SDK-for-Java/issues) to

* Report minor defects, minor items or typos
* Ask for improvements or changes in the SDK API
* Ask any questions related to the community effort

SLAs don't apply for GitHub tickets

## Release Notes

see also [Releases](https://github.com/Dynatrace/OneAgent-SDK-for-dotnet/releases)

|Version|Description                                 |
|:------|:-------------------------------------------|
|1.0.0  |Initial release                             |
