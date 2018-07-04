**Disclaimer: This SDK is currently in beta and still work in progress.**

# Dynatrace OneAgent SDK for .NET

This SDK allows Dynatrace customers to instrument .NET applications. This is useful to enhance the visibility for proprietary frameworks or custom frameworks not directly supported by Dynatrace OneAgent out-of-the-box.

It provides the .NET implementation of the [Dynatrace OneAgent SDK](https://github.com/Dynatrace/OneAgent-SDK). 

## Package contents

- `LICENSE`: license under which the whole SDK and sample applications are published

## Features
Dynatrace OneAgent SDK for .NET currently implements support for the following features (corresponding to features specified in [Dynatrace OneAgent SDK](https://github.com/Dynatrace/OneAgent-SDK)):
-  database calls (including async database calls)

## Documentation
The reference documentation is included in this package. The most recent version is also available online at 

A high level documentation/description of OneAgent SDK concepts is available at [https://github.com/Dynatrace/OneAgent-SDK/](https://github.com/Dynatrace/OneAgent-SDK/).

## Integrating into your application

### Dependencies
If you want to integrate the OneAgent SDK into your application, just add the following NuGet dependency:

TODO: Package is not yet published

The Dynatrace OneAgent SDK for .NET has no further dependencies.

### Troubleshooting



## OneAgent SDK for .NET Requirements

- Any .NET Full framework or .NET Core version that supports .NET Standard 1.0
- Dynatrace OneAgent .NET (supported versions see below)

# API Concepts

Common concepts of the Dynatrace OneAgent SDK are explained the [Dynatrace OneAgent SDK repository](https://github.com/Dynatrace/OneAgent-SDK).

## Get an Api object

Use OneAgentSDKFactory.CreateInstance() to obtain an OneAgentSDK instance. You should reuse this object over the whole application 
and if possible CLR lifetime:

```Java
OneAgentSdk oneAgentSdk = OneAgentSdkFactory.CreateInstance();
switch (oneAgentSdk.getCurrentState()) {
case ACTIVE:
	break;
case PERMANENTLY_INACTIVE:
	break;
case TEMPORARILY_INACTIVE:
	break;
default:
	break;
}
```

It is good practice to check the SDK state regularly as it may change at every point of time (except PERMANENTLY_INACTIVE never changes over CLR lifetime).

## Common concepts: Tracers

To trace any kind of call you first need to create a Tracer. The Tracer object represents the logical and physical endpoint that you want to call. A Tracer serves two purposes. First to time the call (duraction, cpu and more) and report errors. That is why each Tracer has these three methods. The error method must be called only once, and it must be in between start and end.

```Java
void Start();

void Error(String message);

void End();
```
The second purpose of a Tracer is to allow tracing across process boundaries. To achieve that these kind of traces supply so called tags. Tags are strings or byte arrays that enable Dynatrace to trace a transaction end to end. As such the tag is the one information that you need to transport across these calls yourselfs.


### Compatibility OneAgent SDK for .NET releases with OneAgent for .NET releases
OneAgent 150

## Feedback

In case of questions, issues or feature requests feel free to contact [Michael Kopp](https://github.com/mikopp), [Alram Lechner](https://github.com/AlramLechnerDynatrace) or file an issue. Your feedback is welcome!


## OneAgent SDK for .NET release notes
|Version|Date|Description|
|:------|:----------|:------------------|
