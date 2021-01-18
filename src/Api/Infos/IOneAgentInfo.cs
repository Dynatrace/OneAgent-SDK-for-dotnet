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
    /// Provides details about the OneAgent used by the SDK.
    /// Retrieved using <see cref="IOneAgentSdk.AgentInfo"/>.
    /// </summary>
    public interface IOneAgentInfo
    {
        /// <summary>
        /// Indicates if OneAgent is available.
        /// True if OneAgent was found - even if it is not compatible with this SDK version.
        /// </summary>
        bool AgentFound { get; }

        /// <summary>
        /// Indicates if found OneAgent is fully compatible with this SDK version.
        /// True if OneAgent is available and compatible, false in any other case.
        /// </summary>
        bool AgentCompatible { get; }

        /// <summary>
        /// Version of OneAgent found.
        /// Null in case of no OneAgent available or OneAgent doesn't support reporting the version.
        /// </summary>
        string Version { get; }
    }
}
