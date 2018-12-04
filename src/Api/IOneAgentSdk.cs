//
// Copyright 2018 Dynatrace LLC
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

namespace Dynatrace.OneAgent.Sdk.Api
{
    /// <summary>
    /// Interface implemented by OneAgentSDK. Retrieved by OneAgentSdkFactory.CreateInstance(). For details see:
    /// <a href="https://github.com/Dynatrace/OneAgent-SDK-for-dotnet#oneagentsdk-object">https://github.com/Dynatrace/OneAgent-SDK-for-dotnet#oneagentsdk-object</a>
    /// </summary>
    public interface IOneAgentSdk
    {
        /// <summary>
        /// Installs a callback that gets informed, if any SDK action has failed. For details see <see cref="ILoggingCallback"/> interface.
        /// The provided callback must be thread-safe, when using this <see cref="IOneAgentSdk"/> instance in multithreaded environments.
        /// </summary>
        /// <param name="loggingCallback">may be null, to remove current callback. provided callback replaces any previously set callback.</param>
        void SetLoggingCallback(ILoggingCallback loggingCallback);

        #region Remote Calls (incoming and outgoing)

        /// <summary>
        /// Creates a tracer for an incoming remote call.
        /// </summary>
        /// <param name="serviceMethod">name of the called remote method</param>
        /// <param name="serviceName">name of the remote service</param>
        /// <param name="serviceEndpoint">logical deployment endpoint on the server side
        /// In case of a clustered/load balanced service, the serviceEndpoint represents the common logical endpoint (e.g. registry://staging-environment/myservices/serviceA). As such a single serviceEndpoint can have many processes on many hosts that services requests for it.</param>
        /// <returns>IIncomingRemoteCallTracer instance to work with</returns>
        IIncomingRemoteCallTracer TraceIncomingRemoteCall(string serviceMethod, string serviceName, string serviceEndpoint);

        /// <summary>
        /// Creates a tracer for an outgoing remote call.
        /// </summary>
        /// <param name="serviceMethod">name of the called remote method</param>
        /// <param name="serviceName">name of the remote service</param>
        /// <param name="serviceEndpoint">logical deployment endpoint on the server side In case of a clustered/load balanced service, the serviceEndpoint represents the common logical endpoint (e.g. registry://staging-environment/myservices/serviceA) where as the @channelEndpoint represents the actual communication endpoint. As such a single serviceEndpoint can have many channelEndpoints.</param>
        /// <param name="channelType">communication protocol used by remote call</param>
        /// <param name="channelEndpoint">this represents the communication endpoint for the remote service. This information allows Dynatrace to tie the database requests to a specific process or cloud service. It is optional.
        /// for TCP/IP: host name/IP of the server-side (can include port)
        /// for UNIX domain sockets: path of domain socket file
        /// for named pipes: name of pipe
        /// </param>
        /// <returns>IOutgoingRemoteCallTracer instance to work with</returns>
        IOutgoingRemoteCallTracer TraceOutgoingRemoteCall(string serviceMethod, string serviceName, string serviceEndpoint, ChannelType channelType, string channelEndpoint);

        #endregion

        #region Database Calls (outgoing only)

        /// <summary>
        /// Initializes a DatabaseInfo instance that is required for tracing database requests.
        /// </summary>
        /// <param name="name">name of the database</param>
        /// <param name="vendor">database vendor name (e.g. Oracle, MySQL, ...), can be a user defined name
        ///  If possible use a constant defined in com.dynatrace.oneagent.sdk.api.enums.DatabaseVendor</param>
        /// <param name="channelType">communication protocol used to communicate with the database.</param>
        /// <param name="channelEndpoint">this represents the communication endpoint for the database. This information allows Dynatrace to tie the database requests to a specific process or cloud service. It is optional.
        /// * for TCP/IP: host name/IP of the server-side (can include port in the form of "host:port")
        /// * for UNIX domain sockets: name of domain socket file
        /// * for named pipes: name of pipe</param>
        /// <returns>DatabaseInfo instance to work with</returns>
        IDatabaseInfo CreateDatabaseInfo(string name, string vendor, ChannelType channelType, string channelEndpoint);

        /// <summary>
        ///  Creates a tracer for tracing outgoing SQL database requests.
        /// </summary>
        /// <param name="databaseInfo">information about database</param>
        /// <param name="statement">database SQL statement</param>
        /// <returns>DatabaseRequestTracer to work with</returns>
        IDatabaseRequestTracer TraceSQLDatabaseRequest(IDatabaseInfo databaseInfo, string statement);

        #endregion
    }
}
