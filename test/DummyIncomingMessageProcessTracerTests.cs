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

using Dynatrace.OneAgent.Sdk.Api;
using Dynatrace.OneAgent.Sdk.Api.Enums;
using Dynatrace.OneAgent.Sdk.Api.Infos;
using Xunit;

namespace Dynatrace.OneAgent.Sdk.Test
{
    /// <summary>
    /// See <see cref="DummyTracerTestBase{T}"/>
    /// </summary>
    public class DummyIncomingMessageProcessTracerTests : DummyTracerTestBase<IIncomingMessageProcessTracer>
    {
        readonly IMessagingSystemInfo messagingSystemInfo = OneAgentSdk
            .CreateMessagingSystemInfo("vendor", "destination", MessageDestinationType.QUEUE, ChannelType.OTHER, null);

        protected override IIncomingMessageProcessTracer CreateTracer() => OneAgentSdk.TraceIncomingMessageProcess(messagingSystemInfo);

        protected override void ExecuteTracerSpecificCalls(IIncomingMessageProcessTracer tracer)
        {
            tracer.SetCorrelationId("correlation-id");
            tracer.SetVendorMessageId("message-id");
            tracer.SetCorrelationId("");
            tracer.SetVendorMessageId("");
            tracer.SetCorrelationId(null);
            tracer.SetVendorMessageId(null);
        }

        [Fact]
        private void TraceNullInfo()
        {
            Assert.NotNull(OneAgentSdk.TraceIncomingMessageProcess(null));
        }
    }
}
