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
using Dynatrace.OneAgent.Sdk.Api.Enums;
using Dynatrace.OneAgent.Sdk.Api.Infos;
using Xunit;

namespace Dynatrace.OneAgent.Sdk.Test
{
    /// <summary>
    /// Tests that the dummy implementation of <see cref="IOneAgentSdk"/>
    /// (which is in place when no (compatible) OneAgent is found or if the SDK is disabled):
    /// does not cause the application to crash when being used and correctly returns state and agent info
    /// </summary>
    public static class OneAgentSdkDummyTests
    {
        static IOneAgentSdk OneAgentSdk { get; } = OneAgentSdkFactory.CreateInstance();

        [Fact]
        public static void CurrentState()
        {
            Assert.Equal(SdkState.PERMANENTLY_INACTIVE, OneAgentSdk.CurrentState);
        }

        [Fact]
        public static void OneAgentInfo()
        {
            IOneAgentInfo agentInfo = OneAgentSdk.AgentInfo;
            Assert.False(agentInfo.AgentFound);
            Assert.False(agentInfo.AgentCompatible);
            Assert.Null(agentInfo.Version);
        }

        [Fact]
        public static void SetLoggingCallback()
        {
            OneAgentSdk.SetLoggingCallback(null);
            OneAgentSdk.SetLoggingCallback(new TestLoggingCallback());
            OneAgentSdk.SetLoggingCallback(null);
        }

        [Fact]
        public static void CreateInProcessLink()
        {
            Assert.NotNull(OneAgentSdk.CreateInProcessLink());
        }

        [Fact]
        public static void AddCustomRequestAttributes()
        {
            OneAgentSdk.AddCustomRequestAttribute("test", "test");
            OneAgentSdk.AddCustomRequestAttribute("test", null);
            OneAgentSdk.AddCustomRequestAttribute("test", 1L);
            OneAgentSdk.AddCustomRequestAttribute("test", 1D);
            OneAgentSdk.AddCustomRequestAttribute(null, "test");
            OneAgentSdk.AddCustomRequestAttribute(null, null);
            OneAgentSdk.AddCustomRequestAttribute(null, 1L);
            OneAgentSdk.AddCustomRequestAttribute(null, 1D);
        }

        class TestLoggingCallback : ILoggingCallback
        {
            public void Error(string message) { }
            public void Warn(string message) { }
        }
    }
}
