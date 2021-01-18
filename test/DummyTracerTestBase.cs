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

using Dynatrace.OneAgent.Sdk.Api;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dynatrace.OneAgent.Sdk.Test
{
    /// <summary>
    /// Tests that the dummy implementations of tracers (which are in place when no (compatible) OneAgent is found or if the SDK is disabled):
    /// - do not cause the application to crash when being used
    /// - call the Actions and Funcs provided via Trace and TraceAsync and
    /// - do not swallow any exceptions
    /// </summary>
    public abstract class DummyTracerTestBase<T> where T : ITracer
    {
        public class ExpectedException : Exception { }

        protected static IOneAgentSdk OneAgentSdk { get; } = OneAgentSdkFactory.CreateInstance();

        protected abstract T CreateTracer();

        protected abstract void ExecuteTracerSpecificCalls(T tracer);

        private void ExecuteTracerCalls(T tracer)
        {
            IIncomingTaggable incomingTaggable = tracer as IIncomingTaggable;
            IOutgoingTaggable outgoingTaggable = tracer as IOutgoingTaggable;
            if (incomingTaggable != null)
            {
                incomingTaggable.SetDynatraceByteTag(new byte[0] { });
                incomingTaggable.SetDynatraceStringTag("");
                incomingTaggable.SetDynatraceByteTag(null);
                incomingTaggable.SetDynatraceStringTag(null);
            }
            if (outgoingTaggable != null)
            {
                outgoingTaggable.GetDynatraceByteTag();
                outgoingTaggable.GetDynatraceStringTag();
            }
            ExecuteTracerSpecificCalls(tracer);
        }

        [Fact]
        public void StartErrorEnd()
        {
            var tracer = CreateTracer();
            tracer.Start();
            tracer.Error("error");
            ExecuteTracerCalls(tracer);
            tracer.End();
        }

        [Fact]
        public void StartErrorExceptionEnd()
        {
            var tracer = CreateTracer();
            tracer.Start();
            tracer.Error(new Exception("some exception"));
            ExecuteTracerCalls(tracer);
            tracer.End();
        }

        [Fact]
        public void TraceAction()
        {
            var tracer = CreateTracer();
            Action action = () =>
            {
                ExecuteTracerCalls(tracer);
                throw new ExpectedException();
            };
            Assert.Throws<ExpectedException>(() => tracer.Trace(action));
        }

        [Fact]
        public void TraceFunc()
        {
            var tracer = CreateTracer();
            Func<int> func = () =>
            {
                ExecuteTracerCalls(tracer);
                throw new ExpectedException();
            };
            Assert.Throws<ExpectedException>(() => tracer.Trace(func));
        }

        [Fact]
        public async Task TraceAsyncFuncTaskAsync()
        {
            var tracer = CreateTracer();
            Func<Task> func = () =>
            {
                return Task.Run(() =>
                {
                    ExecuteTracerCalls(tracer);
                    throw new ExpectedException();
                });
            };
            await Assert.ThrowsAsync<ExpectedException>(() => tracer.TraceAsync(func));
        }

        [Fact]
        public async Task TraceAsyncFuncTaskTAsync()
        {
            var tracer = CreateTracer();
            Func<Task<int>> func = () =>
            {
                return Task.Run((Func<int>)(() =>
                {
                    ExecuteTracerCalls(tracer);
                    throw new ExpectedException();
                }));
            };
            await Assert.ThrowsAsync<ExpectedException>(() => tracer.TraceAsync(func));
        }

        [Fact]
        public void TraceNullAction()
        {
            var tracer = CreateTracer();
            Action action = null;
            tracer.Trace(action);
        }

        [Fact]
        public void TraceNullFunc()
        {
            var tracer = CreateTracer();
            Func<int> func = null;
            tracer.Trace(func);
        }

        [Fact]
        public void TraceAsyncNullFuncTask()
        {
            var tracer = CreateTracer();
            Func<Task> func = null;
            Assert.Null(tracer.TraceAsync(func));
        }

        [Fact]
        public void TraceAsyncFuncNullTask()
        {
            var tracer = CreateTracer();
            Func<Task> func = () => null;
            Assert.Null(tracer.TraceAsync(func));
        }

        [Fact]
        public void TraceAsyncNullFuncTaskT()
        {
            var tracer = CreateTracer();
            Func<Task<int>> func = null;
            Assert.Null(tracer.TraceAsync(func));
        }

        [Fact]
        public void TraceAsyncFuncNullTaskT()
        {
            var tracer = CreateTracer();
            Func<Task<int>> func = () => null;
            Assert.Null(tracer.TraceAsync(func));
        }
    }
}
