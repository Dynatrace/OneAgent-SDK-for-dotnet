using System;
using System.Collections.Generic;
using System.Text;
using Dynatrace.OneAgent.Sdk.Api;

namespace Dynatrace.OneAgent.Sdk.Sample
{
    class RemoteCallTracerSamples
    {
		public static void SimpleIncomingRemoting()
		{
			var instance = OneAgentSdkFactory.CreateInstance();
			var incomingRemoteCallTracer = instance.TraceIncomingRemoteCall("TestMethod", "testService", "testEndpoint");

			incomingRemoteCallTracer.Start();

			Random rnd = new Random();
			int sum = 0;
			for (int i = 0; i < 1000; i++)
			{
				sum += rnd.Next(-10,10);
			}

			Console.WriteLine(sum);
			incomingRemoteCallTracer.End();
		}
    }
}
