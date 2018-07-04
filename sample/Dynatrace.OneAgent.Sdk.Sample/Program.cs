using System;
using System.Threading.Tasks;
using Dynatrace.OneAgent.Sdk.Api;

namespace Dynatrace.OneAgent.Sdk.Sample
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Enter test name");
			var testName = Console.ReadLine();

			//TODO: Find a better solution for this
			if (testName == "Sync_NoException_StartEnd")
			{
				DatabaseRequestTracerSamples.Sync_NoException_StartEnd();
			}
			if (testName == "Async_NoException_StartEnd")
			{
				DatabaseRequestTracerSamples.Async_NoException_StartEnd().Wait();
			}
			if (testName == "Async_NoException_Lambda")
			{
				DatabaseRequestTracerSamples.Async_NoException_Lambda().Wait();
			}
			if (testName == "Sync_NoException_Lambda")
			{
				DatabaseRequestTracerSamples.Sync_NoException_Lambda();
			}
			if (testName == "Async_NoException_Lambda_And_Async_NoException_Lambda")
			{
				DatabaseRequestTracerSamples.Async_NoException_Lambda_And_Async_NoException_Lambda().Wait();
			}
			if (testName == "Async_NoException_StartAsyncEnd_And_Async_NoException_Lambda")
			{
				DatabaseRequestTracerSamples.Async_NoException_StartAsyncEnd_And_Async_NoException_Lambda().Wait();
			}
			if (testName == "Async_Exception_StartAsyncEnd")
			{
				DatabaseRequestTracerSamples.Async_Exception_StartAsyncEnd().Wait();
			}
			if (testName == "Async_Exception_Lambda")
			{
				DatabaseRequestTracerSamples.Async_Exception_Lambda().Wait();
			}
			if (testName == "Async_Exception_Lambda_And_Async_Exception_Lambda")
			{
				DatabaseRequestTracerSamples.Async_Exception_Lambda_And_Async_Exception_Lambda().Wait();
			}
			if (testName == "Async_NoException_Lambda_And_Sync_Exception_Lambda")
			{
				DatabaseRequestTracerSamples.Async_NoException_Lambda_And_Sync_Exception_Lambda().Wait();
			}
			if (testName == "Sync_Exception_Lambda_And_Async_NoException_Lambda")
			{
				DatabaseRequestTracerSamples.Sync_Exception_Lambda_And_Async_NoException_Lambda().Wait();
			}

			Console.WriteLine("Done, press any key to exit");
			Console.Read();
		}
	}
}
