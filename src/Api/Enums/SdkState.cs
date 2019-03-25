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

namespace Dynatrace.OneAgent.Sdk.Api.Enums
{
    /// <summary>
    /// Defines the state, in which the SDK can be.
    /// </summary>
    public enum SdkState
    {
        /// <summary>
        /// SDK is connected to agent and capturing data.
        /// </summary>
        ACTIVE,

        /// <summary>
        /// SDK is connected to agent, but capturing is disabled.
        /// In this state, SDK user can skip creating SDK transactions and save CPU time.
        /// SDK state should be checked regularly as it may change at every point of time.
        /// </summary>
        TEMPORARILY_INACTIVE,

        /// <summary>
        /// SDK isn't connected to agent. So it will never capture data. 
        /// This SDK state will never change in current application life time.
        /// It is good practice to never call any SDK api and safe CPU time therefore.
        /// </summary>
        PERMANENTLY_INACTIVE
    }
}
