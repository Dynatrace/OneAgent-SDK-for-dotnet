using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dynatrace.OneAgent.Sdk.Api {
	public interface  IIncomingRemoteCallTracer : ITracer, IIncomingTaggable {
		/**
	 * Sets the name of the used remoting protocol.
	 *
	 * @param protocolName		protocol name
	 */
		void SetProtocolName(String protocolName);
	}
}
