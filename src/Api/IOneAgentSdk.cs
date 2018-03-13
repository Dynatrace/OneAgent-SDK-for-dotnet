using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynatrace.OneAgent.Sdk.Api.Enums;
using Dynatrace.OneAgent.Sdk.Api.Infos;

namespace Dynatrace.OneAgent.Sdk.Api {
	public interface IOneAgentSdk {

		IIncomingRemoteCallTracer TraceIncomingRemoteCall(String serviceMethod, String serviceName, String serviceEndpoint);

		IDatabaseRequestTracer TraceSQLDatabaseRequest(IDatabaseInfo databaseInfo, String statement);

		IDatabaseInfo CreateDatabaseInfo(String name, String vendor, ChannelType channelType, String channelEndpoint);
	}
}