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
    /// Tracer used for tracing an outgoing web request.
    /// See <see cref="IOneAgentSdk.TraceOutgoingWebRequest"/>.
    /// </summary>
    public interface IOutgoingWebRequestTracer : ITracer, IOutgoingTaggable
    {
        /// <summary>
        /// HTTP request headers sent with the outgoing request
        /// Request headers can only be set before starting the tracer.
        ///
        /// This method can be called multiple times with the same header name to provide multiple values.
        /// </summary>
        /// <param name="name">HTTP request header field name</param>
        /// <param name="value">HTTP request header field value</param>
        void AddRequestHeader(string name, string value);

        /// <summary>
        /// HTTP response headers returned by the server
        /// Response headers can only be set before ending the tracer.
        ///
        /// This method can be called multiple times with the same header name to provide multiple values.
        /// </summary>
        /// <param name="name">HTTP response header field name</param>
        /// <param name="value">HTTP response header field value</param>
        void AddResponseHeader(string name, string value);

        /// <summary>
        /// Sets the HTTP response status code.
        /// </summary>
        /// <param name="statusCode">HTTP status code returned by server</param>
        void SetStatusCode(int statusCode);
    }
}
