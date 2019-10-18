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

using Dynatrace.OneAgent.Sdk.Api;
using Dynatrace.OneAgent.Sdk.Api.Metrics;
using Xunit;

namespace Dynatrace.OneAgent.Sdk.Test
{
    /// <summary>
    /// Tests the dummy implementation of all OneAgent SDK metrics 
    /// by ensuring that the dummy implementation returns valid
    /// instances and that it does not throw any exceptions.
    /// </summary>
    public static class DummyMetricTests
    {
        private static IOneAgentSdk OneAgentSdk { get; } = OneAgentSdkFactory.CreateInstance();
        private const string MetricKey = "MyMetricKey";
        private const string MetricUnit = "MyMetricUnit";
        private const string MetricDimension = "MyMetricDimension";
        private const long DummyIntegerValue = 0;
        private const double DummyFloatingPointValue = 0.0;

        [Fact]
        public static void IntegerCounterNotNull()
        {
            using (IMetric metricWithoutDimension = OneAgentSdk.CreateIntegerCounterMetric(MetricKey, MetricUnit, null))
            using (IMetric metricWithDimension = OneAgentSdk.CreateIntegerCounterMetric(MetricKey, MetricUnit, MetricDimension))
            {
                Assert.NotNull(metricWithoutDimension);
                Assert.NotNull(metricWithDimension);
            }
        }

        [Fact]
        public static void IntegerCounterDoesNotThrow()
        {
            var exception = Record.Exception(() =>
            {
                using (IIntegerCounter counter = OneAgentSdk.CreateIntegerCounterMetric(MetricKey, MetricUnit, MetricDimension))
                {
                    counter.IncreaseBy(DummyIntegerValue, MetricDimension);
                }
            });
            Assert.Null(exception);
        }

        [Fact]
        public static void FloatCounterNotNull()
        {
            using (IMetric metricWithoutDimension = OneAgentSdk.CreateFloatCounterMetric(MetricKey, MetricUnit, null))
            using (IMetric metricWithDimension = OneAgentSdk.CreateFloatCounterMetric(MetricKey, MetricUnit, MetricDimension))
            {
                Assert.NotNull(metricWithoutDimension);
                Assert.NotNull(metricWithDimension);
            }
        }

        [Fact]
        public static void FloatCounterDoesNotThrow()
        {
            var exception = Record.Exception(() =>
            {
                using (IFloatCounter counter = OneAgentSdk.CreateFloatCounterMetric(MetricKey, MetricUnit, MetricDimension))
                {
                    counter.IncreaseBy(DummyFloatingPointValue, MetricDimension);
                }
            });
            Assert.Null(exception);
        }

        [Fact]
        public static void IntegerGaugeNotNull()
        {
            using (IMetric metricWithoutDimension = OneAgentSdk.CreateIntegerGaugeMetric(MetricKey, MetricUnit, null))
            using (IMetric metricWithDimension = OneAgentSdk.CreateIntegerGaugeMetric(MetricKey, MetricUnit, MetricDimension))
            {
                Assert.NotNull(metricWithoutDimension);
                Assert.NotNull(metricWithDimension);
            }
        }

        [Fact]
        public static void IntegerGaugeDoesNotThrow()
        {
            var exception = Record.Exception(() =>
            {
                using (IIntegerGauge gauge = OneAgentSdk.CreateIntegerGaugeMetric(MetricKey, MetricUnit, MetricDimension))
                {
                    gauge.SetValue(DummyIntegerValue, MetricDimension);
                }
            });
            Assert.Null(exception);
        }

        [Fact]
        public static void FloatGaugeNotNull()
        {
            using (IMetric metricWithoutDimension = OneAgentSdk.CreateFloatGaugeMetric(MetricKey, MetricUnit, null))
            using (IMetric metricWithDimension = OneAgentSdk.CreateFloatGaugeMetric(MetricKey, MetricUnit, MetricDimension))
            {
                Assert.NotNull(metricWithoutDimension);
                Assert.NotNull(metricWithDimension);
            }
        }

        [Fact]
        public static void FloatGaugeDoesNotThrow()
        {
            var exception = Record.Exception(() =>
            {
                using (IFloatGauge gauge = OneAgentSdk.CreateFloatGaugeMetric(MetricKey, MetricUnit, MetricDimension))
                {
                    gauge.SetValue(DummyFloatingPointValue, MetricDimension);
                }
            });
            Assert.Null(exception);
        }

        [Fact]
        public static void IntegerStatisticsNotNull()
        {
            using (IMetric metricWithoutDimension = OneAgentSdk.CreateIntegerStatisticsMetric(MetricKey, MetricUnit, null))
            using (IMetric metricWithDimension = OneAgentSdk.CreateIntegerStatisticsMetric(MetricKey, MetricUnit, MetricDimension))
            {
                Assert.NotNull(metricWithoutDimension);
                Assert.NotNull(metricWithDimension);
            }
        }

        [Fact]
        public static void IntegerStatisticsDoesNotThrow()
        {
            var exception = Record.Exception(() =>
            {
                using (IIntegerStatistics statistics = OneAgentSdk.CreateIntegerStatisticsMetric(MetricKey, MetricUnit, MetricDimension))
                {
                    statistics.AddValue(DummyIntegerValue, MetricDimension);
                }
            });
            Assert.Null(exception);
        }

        [Fact]
        public static void FloatStatisticsNotNull()
        {
            using (IMetric metricWithoutDimension = OneAgentSdk.CreateFloatStatisticsMetric(MetricKey, MetricUnit, null))
            using (IMetric metricWithDimension = OneAgentSdk.CreateFloatStatisticsMetric(MetricKey, MetricUnit, MetricDimension))
            {
                Assert.NotNull(metricWithoutDimension);
                Assert.NotNull(metricWithDimension);
            }
        }

        [Fact]
        public static void FloatStatisticsDoesNotThrow()
        {
            var exception = Record.Exception(() =>
            {
                using (IFloatStatistics statistics = OneAgentSdk.CreateFloatStatisticsMetric(MetricKey, MetricUnit, MetricDimension))
                {
                    statistics.AddValue(DummyFloatingPointValue, MetricDimension);
                }
            });
            Assert.Null(exception);
        }
    }
}
