using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynatrace.OneAgent.Sdk.Api.DummyImpl;

namespace Dynatrace.OneAgent.Sdk.Api {
	public class OneAgentSdkFactory {

		public static IOneAgentSdk CreateInstance() {
			return new OneAgentSdkDummy();
		}
	}
}