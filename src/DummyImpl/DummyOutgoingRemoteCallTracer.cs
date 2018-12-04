//
// Copyright 2018 Dynatrace LLC
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

using System;
using System.Threading.Tasks;

namespace Dynatrace.OneAgent.Sdk.Api.DummyImpl
{
    internal class DummyOutgoingRemoteCallTracer : IOutgoingRemoteCallTracer
	{
		public void End()
		{
		}

		public void Error(string message)
		{
		}

		public byte[] GetDynatraceByteTag() => new byte[0];

		public string GetDynatraceStringTag() => string.Empty;

		public void SetProtocolName(string protocolName)
		{
		}

		public void Start()
		{
		}

        public Task StartAsync()
        {
            return Task.FromResult(0);
        }

        public void Trace(Action action)
		{
		}

		public T Trace<T>(Func<T> func) => func();

		public Task TraceAsync(Func<Task> func) => func();

		public Task<T> TraceAsync<T>(Func<Task<T>> func) => func();
	}
}
