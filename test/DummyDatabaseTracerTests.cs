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

namespace Dynatrace.OneAgent.Sdk.Test
{
    public class DummyDatabaseTracerTests : DummyTracerTestBase<IDatabaseRequestTracer>
    {
        readonly IDatabaseInfo dbInfo = OneAgentSdk.CreateDatabaseInfo("name", "vendor", ChannelType.OTHER, null);

        protected override IDatabaseRequestTracer CreateTracer() => OneAgentSdk.TraceSQLDatabaseRequest(dbInfo, "SELECT * FROM foo");

        protected override void ExecuteTracerSpecificCalls(IDatabaseRequestTracer tracer)
        {
            tracer.SetRowsReturned(42);
            tracer.SetRoundTripCount(1);
        }
    }
}