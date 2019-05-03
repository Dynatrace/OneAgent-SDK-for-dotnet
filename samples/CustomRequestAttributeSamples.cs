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

namespace Dynatrace.OneAgent.Sdk.Sample
{
    static class CustomRequestAttributeSamples
    {
        public static void CustomRequestAttributeSample()
        {
            // we're using the incoming remote call tracer as an example for an active service call
            IIncomingRemoteCallTracer incomingRemoteCallTracer = SampleApplication.OneAgentSdk
                        .TraceIncomingRemoteCall("RemoteMethod", "RemoteServiceName", "mrcp://endpoint/service");

            incomingRemoteCallTracer.Trace(() =>
            {
                // do processing ...

                // set custom request attributes
                SampleApplication.OneAgentSdk.AddCustomRequestAttribute("region", "EMEA");            // string value
                SampleApplication.OneAgentSdk.AddCustomRequestAttribute("salesAmount", 2500);         // long value
                SampleApplication.OneAgentSdk.AddCustomRequestAttribute("service-quality", 0.707106); // double value

                // set multiple values for same key
                SampleApplication.OneAgentSdk.AddCustomRequestAttribute("account-group", 1);
                SampleApplication.OneAgentSdk.AddCustomRequestAttribute("account-group", 2);
                SampleApplication.OneAgentSdk.AddCustomRequestAttribute("account-group", 3);
            });
        }
    }
}
