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

namespace Dynatrace.OneAgent.Sdk.Api.Infos
{
    /// <summary>
    /// Provides information about a PurePath node using the TraceContext
    /// model as defined in https://www.w3.org/TR/trace-context.
    /// The Span-Id represents the currently active PurePath node.
    /// Retrieved using <see cref="IOneAgentSdk.TraceContextInfo"/>.
    /// </summary>
    /// <remarks>
    /// This Trace-Id and Span-Id information is not intended for tagging and context-propagation
    /// scenarios and primarily designed for log-enrichment use cases.
    /// </remarks>
    public interface ITraceContextInfo
    {
        /// <returns>
        /// True if the value returned by <see cref="TraceId"/> is not equal to
        /// <see cref="OneAgentSdkConstants.INVALID_TRACE_ID"/> and the value returned by
        /// <see cref="SpanId"/> is not equal to <see cref="OneAgentSdkConstants.INVALID_SPAN_ID"/>
        /// </returns>
        bool IsValid { get; }

        /// <returns>
        /// The Trace-Id represented as lower-case, hex-encoded string
        /// (see: https://tools.ietf.org/html/rfc4648#section-8).
        /// <see cref="OneAgentSdkConstants.INVALID_TRACE_ID"/> in case of:
        /// - No OneAgent is present.
        /// - OneAgent doesn't support reporting the trace id.
        /// - There is no currently active PurePath context.
        /// </returns>
        string TraceId { get; }

        /// <returns>
        /// The Span-Id represented as lower-case, hex-encoded string
        /// (see: https://tools.ietf.org/html/rfc4648#section-8).
        /// <see cref="OneAgentSdkConstants.INVALID_SPAN_ID"/> in case of:
        /// - No OneAgent is present.
        /// - OneAgent doesn't support reporting the span id.
        /// - There is no currently active PurePath context.
        /// </returns>
        string SpanId { get; }
    }
}
