using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dynatrace.OneAgent.Sdk.Api
{
	public interface IIncomingWebRequestTracer : ITracer, IIncomingTaggable
	{

		void SetRemoteAddress(string remoteAddress);

		void AddRequestHeader(string requestHeaderKey, string requestHeaderValue);


		void AddParameter(string parameterKey, string parameterValue);

		void AddResponseHeader(string responseHeaderKey, string responseHeaderValue);


		void SetResponseCode(int responseCode);
	}
}
