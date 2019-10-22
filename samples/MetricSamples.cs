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

using Dynatrace.OneAgent.Sdk.Api.Metrics;
using System;

namespace Dynatrace.OneAgent.Sdk.Sample
{
    /// <summary>
    /// The samples in this class demonstrate the usage of the OneAgent SDK metrics.
    /// </summary>
    static class MetricSamples
    {
        /// <summary>
        /// Demonstrates the usage of an <see cref="IIntegerCounter"/> metric without dimension information.
        /// This sample also applies to the other metric types.
        /// </summary>
        public static void IntegerCounterSample()
        {
            // The metric type and parameters should match type and parameters configured in Dynatrace.
            // Providing a dimension name when creating a metric and a dimension value when using a metric is optional.
            using (IIntegerCounter counter = SampleApplication.OneAgentSdk.CreateIntegerCounterMetric("MyCounter", "Count"))
            {
                counter.IncreaseBy(1L);
            }
        }

        /// <summary>
        /// Demonstrates the usage of an <see cref="IFloatStatistics"/> metric with dimension information.
        /// This sample also applies to the other metric types.
        /// </summary>
        public static void FloatStatisticsSample()
        {
            var myBusinessLogic = new MyBusinessLogic();

            // The metric type and parameters should match type and parameters configured in Dynatrace.
            using (IFloatStatistics floatStatistics = SampleApplication.OneAgentSdk.CreateFloatStatisticsMetric("AvgResponseTimeByRegion", "MilliSecond", "Region"))
            {
                myBusinessLogic.StatisticsValueGenerated += (s, e) => floatStatistics.AddValue(e.Value, e.Region);
                myBusinessLogic.DoWork();
            }
        }

        private class StatisticsValueGeneratedEventArgs : EventArgs
        {
            public double Value { get; }
            public string Region { get; }

            public StatisticsValueGeneratedEventArgs(double value, string region)
            {
                Value = value;
                Region = region;
            }
        }

        private class MyBusinessLogic
        {
            private const int NumIterations = 5;

            public event EventHandler<StatisticsValueGeneratedEventArgs> StatisticsValueGenerated;

            public void DoWork()
            {
                for (var i = 0; i < NumIterations; ++i)
                {
                    StatisticsValueGenerated?.Invoke(
                        this,
                        new StatisticsValueGeneratedEventArgs(
                            (double)i + 0.5,
                            i % 2 == 0 ? "NORAM" : "EMEA"));
                }
            }
        }
    }
}
