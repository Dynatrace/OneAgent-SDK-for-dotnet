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

namespace Dynatrace.OneAgent.Sdk.Api
{
    /// <summary>
    /// Constants used in <see cref="IOneAgentSdk"/>
    /// </summary>
    public static class OneAgentSdkConstants
    {
        /// <summary>
        /// Using this headername to transport Dynatrace tag inside an outgoing http request ensures compatibility to Dynatrace built-in sensors.
        /// </summary>
        public const string DYNATRACE_HTTP_HEADERNAME = "X-dynaTrace";

        /// <summary>
        /// Using this propertyname to transport Dynatrace tag along with the message, ensures compatibility to Dynatrace built-in sensors.
        /// </summary>
        public const string DYNATRACE_MESSAGE_PROPERTYNAME = "dtdTraceTagInfo";
    }
}
