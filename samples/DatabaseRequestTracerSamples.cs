//
// Copyright 2019 Dynatrace LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using Dynatrace.OneAgent.Sdk.Api;
using Dynatrace.OneAgent.Sdk.Api.Enums;
using Dynatrace.OneAgent.Sdk.Api.Infos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dynatrace.OneAgent.Sdk.Sample
{
    /// <summary>
    /// Samples demonstrating how database requests can be traced using <see cref="IDatabaseRequestTracer"/>
    /// </summary>
    /// <remarks>
    /// In order for these traces to be captured by Dynatrace and shown in the UI you need to create a Custom Service
    /// as database requests occurring outside of service methods (i.e., not on a PurePath) are ignored.
    /// --> Go to the Dynatrace UI and select [Settings > Server-side service monitoring > Custom service detection > .NET services]
    ///     There you can add <see cref="Dynatrace.OneAgent.Sdk.Sample.SampleApplication.Main"/> to caputre everything or add
    ///     each of the desired methods of <see cref="Dynatrace.OneAgent.Sdk.Sample.DatabaseRequestTracerSamples"/> separately.
    /// </remarks>
    static class DatabaseRequestTracerSamples
    {
        public static void Sync_StartEnd()
        {
            IDatabaseInfo dbInfo = SampleApplication.OneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
            IDatabaseRequestTracer dbTracer = SampleApplication.OneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

            dbTracer.Start();
            try
            {
                ExecuteDbCallVoid();
            }
            catch (Exception e)
            {
                dbTracer.Error(e);
                // handle or rethrow
            }
            finally
            {
                dbTracer.End();
            }
        }

        public static void Sync_Lambda()
        {
            IDatabaseInfo dbInfo = SampleApplication.OneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
            IDatabaseRequestTracer dbTracer = SampleApplication.OneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

            int res = dbTracer.Trace(() => ExecuteDbCallInt());
        }

        public static async Task Async_Lambda()
        {
            IDatabaseInfo dbInfo = SampleApplication.OneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
            IDatabaseRequestTracer dbTracer = SampleApplication.OneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

            int res = await dbTracer.TraceAsync(() => ExecuteDbCallIntAsync());
        }

        public static async Task Async_Lambda_And_Async_Lambda()
        {
            IDatabaseInfo dbInfo = SampleApplication.OneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
            IDatabaseRequestTracer dbTracer = SampleApplication.OneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

            int res = await dbTracer.TraceAsync(() => ExecuteDbCallIntAsync());

            IDatabaseInfo dbInfo2 = SampleApplication.OneAgentSdk.CreateDatabaseInfo("MyDb2", "MyVendor2", ChannelType.TCP_IP, "database.example.com:1234");
            IDatabaseRequestTracer dbTracer2 = SampleApplication.OneAgentSdk.TraceSQLDatabaseRequest(dbInfo2, "Select * From AA");

            int res2 = await dbTracer2.TraceAsync(() => ExecuteDbCallIntAsync());
        }

        public static async Task Async_Exception_Lambda()
        {
            IDatabaseInfo dbInfo = SampleApplication.OneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
            IDatabaseRequestTracer dbTracer = SampleApplication.OneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");
            try
            {
                await dbTracer.TraceAsync(() => ExecuteDbCallVoidExceptionAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in {nameof(Async_Exception_Lambda)}, Message: {e.Message}");
            }
        }

        public static async Task Async_Exception_Lambda_And_Async_Exception_Lambda()
        {
            IDatabaseInfo dbInfo = SampleApplication.OneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
            IDatabaseRequestTracer dbTracer = SampleApplication.OneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");
            try
            {
                await dbTracer.TraceAsync(() => ExecuteDbCallVoidExceptionAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in {nameof(Async_Exception_Lambda_And_Async_Exception_Lambda)}, Message: {e.Message}");
            }

            IDatabaseInfo dbInfo2 = SampleApplication.OneAgentSdk.CreateDatabaseInfo("MyDb2", "MyVendor2", ChannelType.TCP_IP, "database.example.com:1234");
            IDatabaseRequestTracer dbTracer2 = SampleApplication.OneAgentSdk.TraceSQLDatabaseRequest(dbInfo2, "Select2 * From AA");

            try
            {
                int result = await dbTracer2.TraceAsync(() => ExecuteDbCallIntExceptionAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in {nameof(Async_Exception_Lambda_And_Async_Exception_Lambda)}, Message: {e.Message}");
            }
        }

        public static async Task Async_Lambda_And_Sync_Exception_Lambda()
        {
            IDatabaseInfo dbInfo = SampleApplication.OneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
            IDatabaseRequestTracer dbTracer = SampleApplication.OneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");
            await dbTracer.TraceAsync(() => ExecuteDbCallIntAsync());

            IDatabaseInfo dbInfo2 = SampleApplication.OneAgentSdk.CreateDatabaseInfo("MyDb2", "MyVendor2", ChannelType.TCP_IP, "database.example.com:1234");
            IDatabaseRequestTracer dbTracer2 = SampleApplication.OneAgentSdk.TraceSQLDatabaseRequest(dbInfo2, "Select2 * From AA");

            try
            {
                int result = dbTracer2.Trace(() => ExecuteDbCallIntException());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in {nameof(Async_Lambda_And_Sync_Exception_Lambda)}, Message: {e.Message}");
            }
        }

        public static async Task Sync_Exception_Lambda_And_Async_Lambda()
        {
            IDatabaseInfo dbInfo = SampleApplication.OneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
            IDatabaseRequestTracer dbTracer = SampleApplication.OneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

            try
            {
                dbTracer.Trace(() => ExecuteDbCallVoidException());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in {nameof(Sync_Exception_Lambda_And_Async_Lambda)}, Message: {e.Message}");
            }

            IDatabaseInfo dbInfo2 = SampleApplication.OneAgentSdk.CreateDatabaseInfo("MyDb2", "MyVendor2", ChannelType.TCP_IP, "database.example.com:1234");
            IDatabaseRequestTracer dbTracer2 = SampleApplication.OneAgentSdk.TraceSQLDatabaseRequest(dbInfo2, "Select2 * From AA");

            int result = await dbTracer2.TraceAsync(() => ExecuteDbCallIntAsync());
        }

        /// <summary>
        /// Depicts various cases of misusing the SDK resulting in warnings through the logging callback
        /// </summary>
        public static void Sync_Misuse_LoggingCallbackTest()
        {
            IDatabaseInfo dbInfo = SampleApplication.OneAgentSdk.CreateDatabaseInfo("MyDb", "MyVendor", ChannelType.TCP_IP, "database.example.com:1234");
            IDatabaseRequestTracer dbTracer = SampleApplication.OneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "Select * From AA");

            dbTracer.End();                 // Warning: missing Start
            dbTracer.Start();               // ok
            dbTracer.Start();               // Warning: already started
            dbTracer.End();                 // ok
            dbTracer.SetRoundTripCount(0);  // Warning: has to be set before End
            dbTracer.SetRowsReturned(0);    // Warning: has to be set before End
            dbTracer.End();                 // Warning: already ended
            dbTracer.Start();               // Warning: already ended
        }

        #region DbCalls

        //these methods simulate DB calls. Imagine that these are methods on a class which is an API to a database.
        private static async Task ExecuteDbCallVoidAsync() => await Task.Delay(100);

        private static async Task<int> ExecuteDbCallIntAsync()
        {
            await Task.Delay(100);
            return await Task.FromResult(42);
        }

        private static int ExecuteDbCallInt()
        {
            Thread.Sleep(100);
            return 42;
        }

        private static void ExecuteDbCallVoid()
        {
            Thread.Sleep(100);
        }

        private static async Task ExecuteDbCallVoidExceptionAsync()
        {
            await Task.Delay(100);
            throw new Exception("Exception occurred.");
        }

        private static async Task<int> ExecuteDbCallIntExceptionAsync()
        {
            await Task.Delay(100);
            throw new Exception("Exception occurred.");
        }

        private static int ExecuteDbCallIntException()
        {
            Thread.Sleep(100);
            throw new Exception("Exception occurred.");
        }

        private static void ExecuteDbCallVoidException()
        {
            Thread.Sleep(100);
            throw new Exception("Exception occurred.");
        }

        #endregion
    }
}
