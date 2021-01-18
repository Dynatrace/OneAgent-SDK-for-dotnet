//
// Copyright 2021 Dynatrace LLC
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

using System;

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

        /// <summary>
        /// <para>Sets HTTP request headers required for linking requests end-to-end.</para>
        /// <para>
        /// Based on your configuration, this method will add the 'X-dynaTrace' header and/or the W3C Trace Context headers ('traceparent' and 'tracestate').<br/>
        /// Therefore it is no longer necessary to manually add the Dynatrace tag and thus <see cref="IOutgoingTaggable.GetDynatraceStringTag"/>
        /// must not be used together with this method.
        /// </para>
        /// <para>This method can only be called on an active tracer (i.e., between start and end).</para>
        ///
        /// <para>Example usage:</para>
        /// <example><code>
        /// var requestHeaders = new Dictionary&lt;string, string>();
        /// outgoingWebRequestTracer.InjectTracingHeaders((name, value) => requestHeaders[name] = value);
        /// </code></example>
        /// </summary>
        ///
        /// <param name="headerSetter">
        ///     <para>An Action&lt;string, string> that takes the parameters (name, value) and sets the respective headers on the HTTP request.<br/>
        ///     If a header with this name already exists, the value is overwritten.</para>
        ///     <para>First parameter (arg1) = name: a valid HTTP header name, never null or empty<br/>
        ///     Second parameter (arg2) = value: the header value to be set, never null</para>
        /// </param>
        void InjectTracingHeaders(Action<string, string> headerSetter);

        /// <summary>
        /// <para>Same as <see cref="InjectTracingHeaders(Action{string, string})"/> but the (nullable) parameter <paramref name="carrier"/>
        /// provided to this method is passed along to <paramref name="headerSetter"/>.</para>
        ///
        /// <para>Example usage:</para>
        /// <example><code>
        /// var requestHeaders = new Dictionary&lt;string, string>();
        /// outgoingWebRequestTracer.InjectTracingHeaders((name, value, carrier) => carrier[name] = value, requestHeaders);
        /// </code></example>
        /// </summary>
        ///
        /// <typeparam name="TCarrier"></typeparam>
        /// <param name="headerSetter">
        ///     <para>An Action&lt;string, string> that takes the parameters (name, value) and sets the respective headers on the HTTP request.<br/>
        ///     If a header with this name already exists, the value is overwritten.</para>
        ///     First parameter (arg1) = name: a valid HTTP header name, never null or empty<br/>
        ///     Second parameter (arg2) = value: the header value to be set, never null<br/>
        ///     Third parameter (arg3) = carrier: the header carrier (i.e., the web request object or its map of headers),
        ///     as passed to <see cref="InjectTracingHeaders{TCarrier}(Action{string, string, TCarrier}, TCarrier)"/> (could be null therefore)
        /// </param>
        /// <param name="carrier">the (nullable) header carrier object passed along to <paramref name="headerSetter"/></param>
        void InjectTracingHeaders<TCarrier>(Action<string, string, TCarrier> headerSetter, TCarrier carrier);
    }
}
