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

namespace Dynatrace.OneAgent.Sdk.Api
{
    /// <summary>
    /// Constants used for working with the OneAgent SDK.
    /// </summary>
    public static class OneAgentSdkConstants
    {
        /// <summary>
        /// This HTTP header is used to transport the Dynatrace tag with an HTTP request.
        /// The tag is used to link traces together and ensures compatibility to OneAgent built-in sensors.
        /// </summary>
        public const string DYNATRACE_HTTP_HEADERNAME = "X-dynaTrace";

        /// <summary>
        /// This property is used to transport the Dynatrace tag along with a message.
        /// The tag is used to link traces together and ensures compatibility to OneAgent built-in sensors.
        /// </summary>
        public const string DYNATRACE_MESSAGE_PROPERTYNAME = "dtdTraceTagInfo";

        /// <summary>
        /// An invalid trace identifier in lowercase hex (base16) representation.
        /// All characters are "0".
        /// Returned by <see cref="Infos.ITraceContextInfo.TraceId"/>.
        /// </summary>
        public const string INVALID_TRACE_ID = "00000000000000000000000000000000";

        /// <summary>
        /// An invalid span identifier in lowercase hex (base16) representation.
        /// All characters are "0".
        /// Returned by <see cref="Infos.ITraceContextInfo.SpanId"/>.
        /// </summary>
        public const string INVALID_SPAN_ID = "0000000000000000";
    }
}
