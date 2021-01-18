//
// Copyright 2021 Dynatrace LLC
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
    internal class DummyIncomingMessageReceiveTracer : IIncomingMessageReceiveTracer
    {
        public void Start() { }

        public Task StartAsync() => Task.FromResult(0);

        public void Error(string message) { }

        public void Error(Exception exception) { }

        public void End() { }

        public void Trace(Action action) => action?.Invoke();

        public T Trace<T>(Func<T> func)
        {
            return func != null ? func() : default(T);
        }

        public Task TraceAsync(Func<Task> func)
        {
            return func != null ? func() : null;
        }

        public Task<T> TraceAsync<T>(Func<Task<T>> func)
        {
            return func != null ? func() : null;
        }
    }
}
