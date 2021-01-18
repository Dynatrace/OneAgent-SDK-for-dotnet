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

using System;

namespace Dynatrace.OneAgent.Sdk.Api.Metrics
{
    /// <summary>
    /// Object created by <see cref="IOneAgentSdk.CreateFloatStatisticsMetric"/>
    /// </summary>
    [ObsoleteAttribute("This interface is obsolete. Refer to https://github.com/Dynatrace/OneAgent-SDK-for-dotnet#metrics for details.", false)] 
    public interface IFloatStatistics : IMetric
    {
        /// <summary>
        /// Adds new value to this statistics metric.
        /// </summary>
        /// <param name="value">The sample to be added.</param>
        /// <param name="dimensionValue">
        /// E. g. name of the concerned resource (disk name, page name, ...).
        /// Dimension must be set to a non-empty value, when a non-empty value for 
        /// <c>dimensionName</c> has been provided in 
        /// <see cref="IOneAgentSdk.CreateFloatStatisticsMetric(string, string, string)"/>.
        /// </param>
        void AddValue(double value, string dimensionValue = null);
    }
}
