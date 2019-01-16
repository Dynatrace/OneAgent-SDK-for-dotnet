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
using System;
using System.Threading;

namespace Dynatrace.OneAgent.Sdk.Sample
{
    class CombinedSamples
    {
        public static void RemoteCallWithDatabase()
        {
            IOutgoingRemoteCallTracer outgoingRemoteCallTracer = SampleApplication.OneAgentSdk
                .TraceOutgoingRemoteCall("RemoteMethod", "RemoteServiceName", "mrcp://endpoint/service", ChannelType.TCP_IP, "myRemoteHost:1234");
            outgoingRemoteCallTracer.SetProtocolName("MyRemoteCallProtocol");

            outgoingRemoteCallTracer.Start();
            try
            {
                string outgoingDynatraceStringTag = outgoingRemoteCallTracer.GetDynatraceStringTag();
                // make the call and transport the tag across to server

                // represents server side processing
                Thread server = new Thread(() =>
                {
                    IIncomingRemoteCallTracer incomingRemoteCallTracer = SampleApplication.OneAgentSdk
                        .TraceIncomingRemoteCall("RemoteMethod", "RemoteServiceName", "mrcp://endpoint/service");

                    string incomingDynatraceStringTag = outgoingDynatraceStringTag; // retrieve from incoming call metadata
                    incomingRemoteCallTracer.SetDynatraceStringTag(incomingDynatraceStringTag);

                    incomingRemoteCallTracer.Start();
                    try
                    {
                        incomingRemoteCallTracer.SetProtocolName("MyRemoteCallProtocol");

                        // execute database request on server
                        DatabaseRequestTracerSamples.Sync_StartEnd();
                    }
                    catch (Exception e)
                    {
                        incomingRemoteCallTracer.Error(e.Message);
                        // handle or rethrow
                    }
                    finally
                    {
                        incomingRemoteCallTracer.End();
                    }
                });
                server.Start();
                server.Join(); // sync call, wait for db result
            }
            catch (Exception e)
            {
                outgoingRemoteCallTracer.Error(e.Message);
                // handle or rethrow
            }
            finally
            {
                outgoingRemoteCallTracer.End();
            }
        }
    }
}
