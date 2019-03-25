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

using Dynatrace.OneAgent.Sdk.Api.Enums;
using Dynatrace.OneAgent.Sdk.Api.Infos;

namespace Dynatrace.OneAgent.Sdk.Api.DummyImpl
{
    internal class OneAgentSdkDummy : IOneAgentSdk
    {
        private readonly DummyOneAgentInfo dummyOneAgentInfo = new DummyOneAgentInfo();
        private readonly DummyDatabaseInfo dummyDatabaseInfo = new DummyDatabaseInfo();
        private readonly DummyDatabaseRequestTracer dummyDatabaseRequestTracer = new DummyDatabaseRequestTracer();
        private readonly DummyIncomingRemoteCallTracer dummyIncomingRemoteCallTracer = new DummyIncomingRemoteCallTracer();
        private readonly DummyOutgoingRemoteCallTracer dummyOutgoingRemoteCallTracer = new DummyOutgoingRemoteCallTracer();
        private readonly DummyMessagingSystemInfo dummyMessagingSystemInfo = new DummyMessagingSystemInfo();
        private readonly DummyOutgoingMessageTracer dummyOutgoingMessageTracer = new DummyOutgoingMessageTracer();
        private readonly DummyIncomingMessageReceiveTracer dummyIncomingMessageReceiveTracer = new DummyIncomingMessageReceiveTracer();
        private readonly DummyIncomingMessageProcessTracer dummyIncomingMessageProcessTracer = new DummyIncomingMessageProcessTracer();
        private readonly DummyInProcessLink dummyInProcessLink = new DummyInProcessLink();
        private readonly DummyInProcessLinkTracer dummyInProcessLinkTracer = new DummyInProcessLinkTracer();

        public SdkState CurrentState => SdkState.PERMANENTLY_INACTIVE;

        public IOneAgentInfo AgentInfo => dummyOneAgentInfo;

        public IDatabaseInfo CreateDatabaseInfo(string name, string vendor, ChannelType channelType, string channelEndpoint)
            => dummyDatabaseInfo;

        public IIncomingRemoteCallTracer TraceIncomingRemoteCall(string serviceMethod, string serviceName, string serviceEndpoint)
            => dummyIncomingRemoteCallTracer;

        public IOutgoingRemoteCallTracer TraceOutgoingRemoteCall(string serviceMethod, string serviceName, string serviceEndpoint, ChannelType channelType, string channelEndpoint)
            => dummyOutgoingRemoteCallTracer;

        public IDatabaseRequestTracer TraceSQLDatabaseRequest(IDatabaseInfo databaseInfo, string statement)
            => dummyDatabaseRequestTracer;

        public void SetLoggingCallback(ILoggingCallback loggingCallback)
        {
        }

        public IMessagingSystemInfo CreateMessagingSystemInfo(string vendorName, string destinationName, MessageDestinationType destinationType, ChannelType channelType, string channelEndpoint)
            => dummyMessagingSystemInfo;

        public IOutgoingMessageTracer TraceOutgoingMessage(IMessagingSystemInfo messagingSystem)
            => dummyOutgoingMessageTracer;

        public IIncomingMessageReceiveTracer TraceIncomingMessageReceive(IMessagingSystemInfo messagingSystem)
            => dummyIncomingMessageReceiveTracer;

        public IIncomingMessageProcessTracer TraceIncomingMessageProcess(IMessagingSystemInfo messagingSystem)
            => dummyIncomingMessageProcessTracer;

        public IInProcessLink CreateInProcessLink() => dummyInProcessLink;

        public IInProcessLinkTracer TraceInProcessLink(IInProcessLink inProcessLink) => dummyInProcessLinkTracer;
    }
}
