using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dynatrace.OneAgent.Sdk.Api
{
	public interface IIncomingRemoteCallTracer : ITracer, IIncomingTaggable
	{

		/// <summary>
		///  Sets the name of the used remoting protocol.
		/// </summary>
		/// <param name="protocolName"></param>
		void SetProtocolName(String protocolName);
	}
}
