using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynatrace.OneAgent.Sdk.Api.DummyImpl;

namespace Dynatrace.OneAgent.Sdk.Api {
	public class OneAgentSdkFactory {

		/// <summary>
		/// This method returns an instance of the OneAgent SDK.
		/// </summary>
		/// <returns></returns>
		public static IOneAgentSdk CreateInstance() {
			return new OneAgentSdkDummy();
		}
	}
}