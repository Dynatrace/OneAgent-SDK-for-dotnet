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

namespace Dynatrace.OneAgent.Sdk.Api
{
    /// <summary>
    /// Tracer used for tracing an incoming web request.
    /// See <see cref="IOneAgentSdk.TraceIncomingWebRequest"/>.
    /// </summary>
    public interface IIncomingWebRequestTracer : ITracer, IIncomingTaggable
    {
        /// <summary>
        /// Sets the remote IP address of the incoming web request.
        /// The remote address can only be set before the tracer is started.
        /// 
        /// This information is very useful to gain information about load balancers, proxies and
        /// ultimately the end user that is sending the request.
        /// 
        /// The remote address is the peer address of the socket connection via which the request was received.
        /// In case one or more proxies are used, this will be the address of the last proxy in the proxy chain.
        /// To enable OneAgent to determine the client IP address (i.e., the address where the request originated),
        /// an application should also call <see cref="AddRequestHeader"/> to add HTTP request headers.
        /// </summary>
        /// <param name="remoteAddress">remote IP address</param>
        void SetRemoteAddress(string remoteAddress);

        /// <summary>
        /// All HTTP POST parameters should be provided to this method.
        /// Selective capturing will be done based on sensor configuration.
        /// </summary>
        /// <param name="name">HTTP parameter name</param>
        /// <param name="value">HTTP parameter value</param>
        void AddParameter(string name, string value);

        /// <summary>
        /// All HTTP request headers should be provided to this method.
        /// Selective capturing will be done based on sensor configuration.
        /// Request headers can only be set *before* starting the tracer.
        ///
        /// This method can be called multiple times with the same header name to provide multiple values.
        /// </summary>
        /// <param name="name">HTTP request header field name</param>
        /// <param name="value">HTTP request header field value</param>
        void AddRequestHeader(string name, string value);

        /// <summary>
        /// All HTTP response headers returned by the server should be provided to this method.
        /// Selective capturing will be done based on sensor configuration.
        /// Response headers can only be set *before* ending the tracer.
        ///
        /// This method can be called multiple times with the same header name to provide multiple values.
        /// </summary>
        /// <param name="name">HTTP response header field name</param>
        /// <param name="value">HTTP response header field value</param>
        void AddResponseHeader(string name, string value);

        /// <summary>
        /// Sets the HTTP response status code.
        /// </summary>
        /// <param name="statusCode">HTTP status code of the response sent to the client</param>
        void SetStatusCode(int statusCode);
    }
}
