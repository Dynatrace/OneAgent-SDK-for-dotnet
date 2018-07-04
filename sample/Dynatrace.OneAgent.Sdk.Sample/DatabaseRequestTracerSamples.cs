using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dynatrace.OneAgent.Sdk.Api;

namespace Dynatrace.OneAgent.Sdk.Sample
{
	/// <summary>
	/// Contains samples that use the IDatabaseRequestTracer.
	/// 
	/// Method name convention:
	/// [Sync|Async]_[Exception|NoException]_[StartEnd|StartAsyncEnd|Lambda]_(And)
	/// Example: a method that simulates an async db call, which throws an exception and uses the Trace method with a lambda:
	/// Async_Exception_Lambda
	/// Example2: a method that simulates 2 db calls, the first is a sync call that doesn't throw an exception and uses the Start() and End() method, 
	/// the second one is an async that throws an exception and uses the Trace method with a lambda:
	/// Sync_NoException_StartEnd_And_Async_Exception_Lambda
	/// </summary>
	public static class DatabaseRequestTracerSamples
	{
		public static void Sync_NoException_StartEnd()
		{
			var instance = OneAgentSdkFactory.CreateInstance();
			var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");

			dbTracer.Start();
			try
			{
				VoidMethod();
			}
			catch
			{
				dbTracer.Error("DB call failed");
			}
			finally
			{
				dbTracer.End();
			}
		}

		public static async Task Async_NoException_StartEnd()
		{
			var instance = OneAgentSdkFactory.CreateInstance();
			var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");

			dbTracer.StartAsync();
			try
			{
				await VoidTask();
			}
			catch
			{
				dbTracer.Error("DB call failed");
			}
			finally
			{
				dbTracer.End();
			}
		}

		public static async Task Async_NoException_Lambda()
		{
			var instance = OneAgentSdkFactory.CreateInstance();
			var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");

			var res = await dbTracer.Trace(() => IntTask());
		}

		public static void Sync_NoException_Lambda()
		{
			var instance = OneAgentSdkFactory.CreateInstance();
			var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");
			dbTracer.Start();

			var res = dbTracer.Trace(() => IntMethod());
		}

		public static async Task Async_NoException_Lambda_And_Async_NoException_Lambda()
		{
			var instance = OneAgentSdkFactory.CreateInstance();
			var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");

			var res = await dbTracer.Trace(() => IntTask());

			var instance2 = OneAgentSdkFactory.CreateInstance();
			var dbinfo2 = instance.CreateDatabaseInfo("MyDb2", "MyVendor2", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer2 = instance.TraceSQLDatabaseRequest(dbinfo2, "Select * From AA");

			var res2 = await dbTracer2.Trace(() => IntTask());
		}

		public static async Task Async_NoException_StartAsyncEnd_And_Async_NoException_Lambda()
		{
			var instance = OneAgentSdkFactory.CreateInstance();
			var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");

			dbTracer.StartAsync();
			await VoidTask();
			dbTracer.End();


			var instance2 = OneAgentSdkFactory.CreateInstance();
			var dbinfo2 = instance.CreateDatabaseInfo("MyDb2", "MyVendor2", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer2 = instance.TraceSQLDatabaseRequest(dbinfo2, "Select * From AA");
			var res2 = await dbTracer2.Trace(() => IntTask());
		}

		public static async Task Async_Exception_StartAsyncEnd()
		{
			var instance = OneAgentSdkFactory.CreateInstance();
			var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");

			dbTracer.StartAsync();
			try
			{
				await VoidTaskWithException();
			}
			catch
			{
				dbTracer.Error("DB call failed");
			}
			finally
			{
				dbTracer.End();
			}
		}

		public static async Task Async_Exception_Lambda()
		{
			var instance = OneAgentSdkFactory.CreateInstance();
			var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");
			try
			{
				await dbTracer.Trace(() => VoidTaskWithException());
			}
			catch (Exception e)
			{
				Console.WriteLine($"Exception in {nameof(Async_Exception_Lambda)}, Message: {e.Message}");
			}
		}

		public static async Task Async_Exception_Lambda_And_Async_Exception_Lambda()
		{

			var instance = OneAgentSdkFactory.CreateInstance();
			var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");
			try
			{
				await dbTracer.Trace(() => VoidTaskWithException());
			}
			catch(Exception e)
			{
				Console.WriteLine($"Exception in {nameof(Async_Exception_Lambda_And_Async_Exception_Lambda)}, Message: {e.Message}");
			}

			var instance2 = OneAgentSdkFactory.CreateInstance();
			var dbinfo2 = instance2.CreateDatabaseInfo("MyDb2", "MyVendor2", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer2 = instance2.TraceSQLDatabaseRequest(dbinfo2, "Select2 * From AA");

			try
			{
				var result = await dbTracer2.Trace(() => IntTaskWithException());
			}
			catch (Exception e)
			{
				Console.WriteLine($"Exception in {nameof(Async_Exception_Lambda_And_Async_Exception_Lambda)}, Message: {e.Message}");
			}
		}

		public static async Task Async_NoException_Lambda_And_Sync_Exception_Lambda()
		{
			var instance = OneAgentSdkFactory.CreateInstance();
			var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");
			await dbTracer.Trace(() => IntTask());

			var instance2 = OneAgentSdkFactory.CreateInstance();
			var dbinfo2 = instance2.CreateDatabaseInfo("MyDb2", "MyVendor2", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer2 = instance2.TraceSQLDatabaseRequest(dbinfo2, "Select2 * From AA");

			try
			{
				var result = dbTracer2.Trace(() => IntMethodWithException());
			}
			catch (Exception e)
			{
				Console.WriteLine($"Exception in {nameof(Async_NoException_Lambda_And_Sync_Exception_Lambda)}, Message: {e.Message}");
			}
		}
		
		public static async Task Sync_Exception_Lambda_And_Async_NoException_Lambda()
		{
			var instance = OneAgentSdkFactory.CreateInstance();
			var dbinfo = instance.CreateDatabaseInfo("MyDb", "MyVendor", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer = instance.TraceSQLDatabaseRequest(dbinfo, "Select * From AA");

			try
			{
				dbTracer.Trace(() => VoidMethodWithException());
			}
			catch (Exception e)
			{
				Console.WriteLine($"Exception in {nameof(Sync_Exception_Lambda_And_Async_NoException_Lambda)}, Message: {e.Message}");
			}

			var instance2 = OneAgentSdkFactory.CreateInstance();
			var dbinfo2 = instance2.CreateDatabaseInfo("MyDb2", "MyVendor2", Dynatrace.OneAgent.Sdk.Api.Enums.ChannelType.TCP_IP, "MyChannelEndpoint");
			var dbTracer2 = instance2.TraceSQLDatabaseRequest(dbinfo2, "Select2 * From AA");
			
			var result = await dbTracer2.Trace(() => IntTask());
		}
		
		#region DbCalls
		//these methods simulate DB calls. Imagine that these are methods on a class which is an API to a database.
		public static async Task VoidTask() => await Task.Delay(500); //async void

		public static async Task<int> IntTask() => await Task.FromResult(42); //async with return value

		public static int IntMethod() => 42; //sync with return value

		public static void VoidMethod() { Console.WriteLine("VoidMethod runs"); } //sync void


		//methods with exceptions:
		public static async Task VoidTaskWithException()
		{
			await Task.Delay(500);
			throw new Exception("Bammm");
		}

		public static async Task<int> IntTaskWithException()
		{
			Console.WriteLine("start IntTaskWithException");
			await Task.Delay(200);
			throw new Exception("Bamm");
		}

		public static int IntMethodWithException()
		{
			throw new Exception("Bamm");
		}

		public static void VoidMethodWithException()
		{
			throw new Exception("Bamm");
		}

		#endregion
	}
}
