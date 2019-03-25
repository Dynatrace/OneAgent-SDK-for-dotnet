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
using System;
using System.Collections.Concurrent;
using System.Threading;


namespace Dynatrace.OneAgent.Sdk.Sample
{
    public static class InProcessLinkTracerSamples
    {
        public static void InProcessLinkTracerSample()
        {
            // start a custom background worker
            CustomBackgroundWorker customBackgroundWorker = new CustomBackgroundWorker();

            // we're using the incoming remote call tracer to represent an active service call
            IIncomingRemoteCallTracer incomingRemoteCallTracer = SampleApplication.OneAgentSdk
                        .TraceIncomingRemoteCall("RemoteMethod", "RemoteServiceName", "mrcp://endpoint/service");

            incomingRemoteCallTracer.Start();
            try
            {
                // create an in-process link on the originating thread
                IInProcessLink inProcessLink = SampleApplication.OneAgentSdk.CreateInProcessLink();

                // delegate work to another thread, in this case we use a custom background worker implementation
                customBackgroundWorker.EnqueueWorkItem(() =>
                {
                    // use the in-process link to link the trace on the target thread to its origin
                    IInProcessLinkTracer inProcessLinkTracer = SampleApplication.OneAgentSdk.TraceInProcessLink(inProcessLink);
                    inProcessLinkTracer.Start();
                    DatabaseRequestTracerSamples.Sync_StartEnd(); // performs a database request traced using the IDatabaseRequestTracer
                    inProcessLinkTracer.End();

                    // this call performed after ending the IInProcessLinkTracer will *not* be traced as part of the incoming remote call
                    DatabaseRequestTracerSamples.Sync_StartEnd();
                });

                // the same link can be re-used multiple times
                customBackgroundWorker.EnqueueWorkItem(() =>
                {
                    IInProcessLinkTracer inProcessLinkTracer = SampleApplication.OneAgentSdk.TraceInProcessLink(inProcessLink);
                    inProcessLinkTracer.Trace(DatabaseRequestTracerSamples.Sync_StartEnd);
                });
            }
            catch (Exception e)
            {
                incomingRemoteCallTracer.Error(e);
                throw e;
            }
            finally
            {
                incomingRemoteCallTracer.End();
                customBackgroundWorker.Shutdown();
            }
        }

        class CustomBackgroundWorker : IDisposable
        {
            private readonly Thread thread;
            private readonly BlockingCollection<Action> queue = new BlockingCollection<Action>();

            public CustomBackgroundWorker()
            {
                thread = new Thread(() =>
                {
                    while (!queue.IsCompleted)
                    {
                        try
                        {
                            queue.Take()?.Invoke();
                        }
                        catch (InvalidOperationException)
                        {

                        }
                    }
                });
                thread.Start();
            }

            public void EnqueueWorkItem(Action workItem) => queue.Add(workItem);

            public void Shutdown() => queue.CompleteAdding();

            public void Dispose() => Shutdown();
        }
    }
}
