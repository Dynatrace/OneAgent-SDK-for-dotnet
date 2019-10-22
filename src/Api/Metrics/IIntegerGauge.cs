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

namespace Dynatrace.OneAgent.Sdk.Api.Metrics
{
    /// <summary>
    /// Object created by <see cref="IOneAgentSdk.CreateIntegerGaugeMetric"/>
    /// </summary>
    public interface IIntegerGauge : IMetric
    {
        /// <summary>
        /// Sets the current value.
        /// </summary>
        /// <param name="currentValue">The current value</param>
        /// <param name="dimensionValue">
        /// E. g. name of the concerned resource (disk name, page name, ...).
        /// Dimension must be set to a non-empty value, when a non-empty value for 
        /// <c>dimensionName</c> has been provided in 
        /// <see cref="IOneAgentSdk.CreateIntegerGaugeMetric(string, string, string)"/>.
        /// </param>
        void SetValue(long currentValue, string dimensionValue = null);
    }
}
