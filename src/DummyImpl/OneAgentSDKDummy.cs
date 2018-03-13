using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynatrace.OneAgent.Sdk.Api.Enums;
using Dynatrace.OneAgent.Sdk.Api.Infos;

namespace Dynatrace.OneAgent.Sdk.Api.DummyImpl {
	class OneAgentSdkDummy : IOneAgentSdk {
		public IDatabaseInfo CreateDatabaseInfo(string name, string vendor, ChannelType channelType, string channelEndpoint) {
			return null;
		}

		public IIncomingRemoteCallTracer TraceIncomingRemoteCall(string serviceMethod, string serviceName, string serviceEndpoint) {			
			return null;
		}

		public IDatabaseRequestTracer TraceSQLDatabaseRequest(IDatabaseInfo databaseInfo, string statement) {
			return null;
		}
	}
}
