using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynatrace.OneAgent.Sdk.Api.Enums;
using Dynatrace.OneAgent.Sdk.Api.Infos;

namespace Dynatrace.OneAgent.Sdk.Api.DummyImpl
{
	internal class OneAgentSdkDummy : IOneAgentSdk
	{

		private DummyDatabaseInfo dummyDatabaseInfo = new DummyDatabaseInfo();
		private DummyDatabaseRequestTracer dummyDatabaseRequestTracer = new DummyDatabaseRequestTracer();
		private DummyIncomingRemoteCallTracer dummyIncomingRemoteCallTracer = new DummyIncomingRemoteCallTracer();

		public IDatabaseInfo CreateDatabaseInfo(string name, string vendor, ChannelType channelType, string channelEndpoint)
		{
			return dummyDatabaseInfo;
		}

		public IIncomingRemoteCallTracer TraceIncomingRemoteCall(string serviceMethod, string serviceName, string serviceEndpoint)
		{
			return dummyIncomingRemoteCallTracer;
		}

		public IDatabaseRequestTracer TraceSQLDatabaseRequest(IDatabaseInfo databaseInfo, string statement)
		{
			return dummyDatabaseRequestTracer;
		}
	}
}