using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dynatrace.OneAgent.Sdk.Api
{
	interface IOutgoingRemoteCallTracer : ITracer, IOutgoingTaggable
	{
		void SetProtocolName(String protocolName);
	}
}
