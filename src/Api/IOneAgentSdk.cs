//
// Copyright 2019 Dynatrace LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using Dynatrace.OneAgent.Sdk.Api.Enums;
using Dynatrace.OneAgent.Sdk.Api.Infos;
using Dynatrace.OneAgent.Sdk.Api.Metrics;

namespace Dynatrace.OneAgent.Sdk.Api
{
    /// <summary>
    /// Interface implemented by OneAgentSDK. Retrieved by OneAgentSdkFactory.CreateInstance(). For details see:
    /// <a href="https://github.com/Dynatrace/OneAgent-SDK-for-dotnet#oneagentsdk-object">https://github.com/Dynatrace/OneAgent-SDK-for-dotnet#oneagentsdk-object</a>
    /// </summary>
    public interface IOneAgentSdk
    {
        /// <summary>
        /// The current state of the SDK. See <see cref="SdkState"/> for details.
        /// </summary>
        SdkState CurrentState { get; }

        /// <summary>
        /// Detailed information about the OneAgent used by the SDK. See <see cref="IOneAgentInfo"/> for details.
        /// </summary>
        IOneAgentInfo AgentInfo { get; }

        /// <summary>
        /// Installs a callback that gets informed, if any SDK action has failed. For details see <see cref="ILoggingCallback"/> interface.
        /// The provided callback must be thread-safe, when using this <see cref="IOneAgentSdk"/> instance in multithreaded environments.
        /// The log messages are primarily intended as a development and debugging aid and are subject to change, please do not try to parse them or assert on them.
        /// </summary>
        /// <param name="loggingCallback">May be null, to remove the current callback. The provided callback replaces any previously set callback.</param>
        void SetLoggingCallback(ILoggingCallback loggingCallback);

        #region Remote Calls (incoming and outgoing)

        /// <summary>
        /// Creates a tracer for an incoming remote call.
        /// </summary>
        /// <param name="serviceMethod">name of the called remote method</param>
        /// <param name="serviceName">name of the remote service</param>
        /// <param name="serviceEndpoint">logical deployment endpoint on the server side
        /// In case of a clustered/load balanced service, the serviceEndpoint represents the common logical endpoint (e.g. registry://staging-environment/myservices/serviceA). As such a single serviceEndpoint can have many processes on many hosts that services requests for it.</param>
        IIncomingRemoteCallTracer TraceIncomingRemoteCall(string serviceMethod, string serviceName, string serviceEndpoint);

        /// <summary>
        /// Creates a tracer for an outgoing remote call.
        /// </summary>
        /// <param name="serviceMethod">name of the called remote method</param>
        /// <param name="serviceName">name of the remote service</param>
        /// <param name="serviceEndpoint">logical deployment endpoint on the server side In case of a clustered/load balanced service, the serviceEndpoint represents the common logical endpoint (e.g. registry://staging-environment/myservices/serviceA) where as the @channelEndpoint represents the actual communication endpoint. As such a single serviceEndpoint can have many channelEndpoints.</param>
        /// <param name="channelType">communication protocol used by remote call</param>
        /// <param name="channelEndpoint">this represents the communication endpoint for the remote service. It is optional.
        /// for TCP/IP: host name/IP of the server-side (can include port)
        /// for UNIX domain sockets: path of domain socket file
        /// for named pipes: name of pipe
        /// </param>
        IOutgoingRemoteCallTracer TraceOutgoingRemoteCall(string serviceMethod, string serviceName, string serviceEndpoint, ChannelType channelType, string channelEndpoint);

        #endregion

        #region Database Calls (outgoing only)

        /// <summary>
        /// Initializes a DatabaseInfo instance that is required for tracing database requests.
        /// </summary>
        /// <param name="name">name of the database</param>
        /// <param name="vendor">name of the database vendor - use <see cref="DatabaseVendor"/> for well-known vendors and otherwise provide a custom string</param>
        /// <param name="channelType">communication protocol used to communicate with the database.</param>
        /// <param name="channelEndpoint">This represents the communication endpoint for the database. This information allows Dynatrace to tie the database requests to a specific process or cloud service. It is optional.
        /// * for TCP/IP: host name/IP of the server-side (can include port in the form of "host:port")
        /// * for UNIX domain sockets: name of domain socket file
        /// * for named pipes: name of pipe</param>
        IDatabaseInfo CreateDatabaseInfo(string name, string vendor, ChannelType channelType, string channelEndpoint);

        /// <summary>
        ///  Creates a tracer for tracing outgoing SQL database requests.
        /// </summary>
        /// <param name="databaseInfo">information about database</param>
        /// <param name="statement">database SQL statement</param>
        IDatabaseRequestTracer TraceSQLDatabaseRequest(IDatabaseInfo databaseInfo, string statement);

        #endregion

        #region Messaging (outgoing & incoming)

        /// <summary>
        /// Creates an IMessagingSystemInfo instance that is required for tracing messages.
        /// </summary>
        /// <param name="vendorName">use <see cref="MessageSystemVendor"/> for well-known vendors and otherwise provide a custom name</param>
        /// <param name="destinationName">destination name (e.g. queue name, topic name)</param>
        /// <param name="destinationType">destination type</param>
        /// <param name="channelType">communication protocol used</param>
        /// <param name="channelEndpoint">optional and depending on protocol:
        /// * for TCP/IP: host name/IP of the server-side (can include port)
        /// * for UNIX domain sockets: name of domain socket file
        /// * for named pipes: name of pipe
        /// </param>
        IMessagingSystemInfo CreateMessagingSystemInfo(string vendorName, string destinationName, MessageDestinationType destinationType, ChannelType channelType, string channelEndpoint);

        /// <summary>
        /// Creates a tracer for an outgoing asynchronous message (send).
        /// </summary>
        /// <param name="messagingSystem">Information about the messaging system, created using <see cref="CreateMessagingSystemInfo"/></param>
        IOutgoingMessageTracer TraceOutgoingMessage(IMessagingSystemInfo messagingSystem);

        /// <summary>
        /// Creates a tracer for an incoming asynchronous message (receive).
        /// </summary>
        /// <param name="messagingSystem">Information about the messaging system, created using <see cref="CreateMessagingSystemInfo"/></param>
        IIncomingMessageReceiveTracer TraceIncomingMessageReceive(IMessagingSystemInfo messagingSystem);

        /// <summary>
        /// Creates a tracer for processing (consuming) a received message (onMessage).
        /// </summary>
        /// <param name="messagingSystem">Information about the messaging system, created using <see cref="CreateMessagingSystemInfo"/></param>
        IIncomingMessageProcessTracer TraceIncomingMessageProcess(IMessagingSystemInfo messagingSystem);

        #endregion

        #region In-Process Linking

        /// <summary>
        /// Creates an in-process link.
        /// An application can call this function to retrieve an in-process link, which can then be used
        /// to trace related processing at a later time and/or in a different thread.
        /// </summary>
        /// <remarks>
        /// See https://github.com/Dynatrace/OneAgent-SDK#in-process-linking for more information.
        /// </remarks>
        /// <returns>An instance of <see cref="IInProcessLink"/> to be used with <see cref="TraceInProcessLink(IInProcessLink)"/></returns>
        IInProcessLink CreateInProcessLink();

        /// <summary>
        /// Creates a tracer for tracing asynchronous related processing in the same process.
        /// </summary>
        /// <remarks>
        /// See https://github.com/Dynatrace/OneAgent-SDK#in-process-linking for more information.
        /// </remarks>
        /// <param name="inProcessLink">An in-process link retrieved from <see cref="CreateInProcessLink"/></param>
        IInProcessLinkTracer TraceInProcessLink(IInProcessLink inProcessLink);

        #endregion

        #region Custom request attributes

        /// <summary>
        /// Adds a custom request attribute (key-value pair) to the currently traced service call.
        /// These attributes can be used to search and filter requests in Dynatrace.
        /// These methods can be called several times to add multiple attributes to the same request.
        /// If the same attribute key is used several times, all values will be recorded.
        /// If no service call is currently being traced, the attributes will be discarded.
        /// </summary>
        /// <param name="key">key of the attribute (required)</param>
        /// <param name="value">value of the attribute (required)</param>
        void AddCustomRequestAttribute(string key, string value);

        /// <summary>
        /// See <see cref="AddCustomRequestAttribute(string, string)"/>.
        /// </summary>
        void AddCustomRequestAttribute(string key, long value);

        /// <summary>
        /// See <see cref="AddCustomRequestAttribute(string, string)"/>.
        /// </summary>
        void AddCustomRequestAttribute(string key, double value);

        #endregion

        #region Web request tracing

        /// <summary>
        /// Creates a tracer for an outgoing web request.
        /// </summary>
        /// <remarks>
        /// To allow continuing the PurePath on the server/service side, the sender needs to retrieve a Dynatrace tag using
        /// <see cref="IOutgoingTaggable.GetDynatraceStringTag"/> from <see cref="IOutgoingWebRequestTracer"/> after starting it
        /// and send the tag along with the HTTP request in the Dynatrace HTTP request header with the name defined in
        /// <see cref="OneAgentSdkConstants.DYNATRACE_HTTP_HEADERNAME"/>.
        /// </remarks>
        /// <param name="url">
        /// The target URL. OneAgent will extract any scheme, hostname, port, path and query.
        /// </param>
        /// <param name="method">HTTP request method (GET, POST, ...)</param>
        IOutgoingWebRequestTracer TraceOutgoingWebRequest(string url, string method);

        /// <summary>
        /// Creates an IWebApplicationInfo instance as required for tracing incoming web requests.
        /// This information determines the identity and name of the resulting web request service in Dynatrace.
        /// Also see https://www.dynatrace.com/support/help/server-side-services/introduction/how-does-dynatrace-detect-and-name-services/#web-request-services for a detailed description of the parameters.
        /// </summary>
        /// <param name="webServerName">
        /// Logical name of the web server.
        /// In case of a cluster, every node in the cluster must report the same name here.
        /// Make sure not to use the host header for this parameter as this may not reflect your actual setup.
        /// </param>
        /// <param name="applicationId">Application ID of the web application</param>
        /// <param name="contextRoot">
        /// Context root of the application.
        /// The path of all URLs traced with the returned IWebApplicationInfo should start with the provided context root.
        /// </param>
        IWebApplicationInfo CreateWebApplicationInfo(string webServerName, string applicationId, string contextRoot);

        /// <summary>
        /// Creates a tracer for an incoming web request.
        /// </summary>
        /// <param name="webApplicationInfo">IWebApplicationInfo created using <see cref="CreateWebApplicationInfo"/></param>
        /// <param name="url">
        /// The requested URL. OneAgent will extract any scheme, hostname, port, path and query.
        /// </param>
        /// <param name="method">HTTP request method (GET, POST, ...)</param>
        IIncomingWebRequestTracer TraceIncomingWebRequest(IWebApplicationInfo webApplicationInfo, string url, string method);

        #endregion

        #region Metrics

        /// <summary>
        /// <para>Creates an integer counter metric instance. Counters are used for all metrics, that are counting something like sent/received bytes to/from network.</para>
        /// <para>Counters sum up provided samples and reports the sum only.</para>
        /// </summary>
        /// <param name="metricKey">
        /// Name (tenant-wide ID) of the metric. It must be ASCII-compatible, must not contain NUL characters and must not be longer than 100 bytes.
        /// If metrics with the same key are created multiple times, it is undefined whether distinct instances are returned.
        ///</param>
        /// <param name="unit">
        /// Unit of the metric. Used as label only. It is optional. Note that you must not report the same
        /// metric with different units, as that would lead to incorrect aggregation. If you cannot
        /// ensure consistent units, use a different metric key per unit, e.g. "NORMAL-METRIC-KEY.UNIT-NAME".
        /// </param>
        /// <param name="dimensionName">
        /// Name of dimension. Must be set to a non-empty string, when dimension will be reported 
        /// on returned counter instance. Must be set to null or empty, when dimension won't be reported.
        /// </param>
        /// <returns>
        /// The metric instance being used for reporting. Returned instances are thread safe and should be reused whenever possible.
        /// Calling this method twice or more with same metric key might return same instance.
        /// </returns>
        IIntegerCounter CreateIntegerCounterMetric(string metricKey, string unit = null, string dimensionName = null);

        /// <summary>
        /// Floating point variant of <see cref="CreateIntegerCounterMetric(string, string, string)"/>
        /// </summary>
        /// <param name="metricKey">Same restrictions apply as for metricKey in <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <param name="unit">See <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <param name="dimensionName">See parameter <c>dimensionName</c> for <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <returns>For details see return value of <see cref="CreateIntegerCounterMetric(string, string, string)"/></returns>
        IFloatCounter CreateFloatCounterMetric(string metricKey, string unit = null, string dimensionName = null);

        /// <summary>
        /// <para>
        /// Creates a gauge metric instance. Gauges can be used for metrics describing a current state like 
        /// temperature, number of items in a cache.
        /// </para>
        /// <para>Gauges are intended for periodical sampling and reporting min, max and average of provided samples.</para>
        /// </summary>
        /// <param name="metricKey">Same restrictions apply as for metricKey in <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <param name="unit">See <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <param name="dimensionName">See parameter <c>dimensionName</c> for <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <returns>For details see return value of <see cref="CreateIntegerCounterMetric(string, string, string)"/></returns>
        IIntegerGauge CreateIntegerGaugeMetric(string metricKey, string unit = null, string dimensionName = null);

        /// <summary>
        /// Floating point variant of <see cref="CreateIntegerGaugeMetric(string, string, string)"/>
        /// </summary>
        /// <param name="metricKey">Same restrictions apply as for metricKey in <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <param name="unit">See <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <param name="dimensionName">See parameter <c>dimensionName</c> for <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <returns>For details see return value of <see cref="CreateIntegerCounterMetric(string, string, string)"/></returns>
        IFloatGauge CreateFloatGaugeMetric(string metricKey, string unit = null, string dimensionName = null);

        /// <summary>
        /// <para>
        /// Creates a statistics metric instance. Statistics can/should be used for event driven metrics like 
        /// packet size of network interface.
        /// </para>
        /// <para>Statistics are reporting min, max, average and count.</para>
        /// </summary>
        /// <param name="metricKey">Same restrictions apply as for metricKey in <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <param name="unit">See <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <param name="dimensionName">See parameter <c>dimensionName</c> for <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <returns>For details see return value of <see cref="CreateIntegerCounterMetric(string, string, string)"/></returns>
        IIntegerStatistics CreateIntegerStatisticsMetric(string metricKey, string unit = null, string dimensionName = null);

        /// <summary>
        /// Floating point variant of <see cref="CreateIntegerStatisticsMetric(string, string, string)"/>
        /// </summary>
        /// <param name="metricKey">Same restrictions apply as for metricKey in <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <param name="unit">See <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <param name="dimensionName">See parameter <c>dimensionName</c> for <see cref="CreateIntegerCounterMetric(string, string, string)"/></param>
        /// <returns>For details see return value of <see cref="CreateIntegerCounterMetric(string, string, string)"/></returns>
        IFloatStatistics CreateFloatStatisticsMetric(string metricKey, string unit = null, string dimensionName = null);

        #endregion
    }
}
