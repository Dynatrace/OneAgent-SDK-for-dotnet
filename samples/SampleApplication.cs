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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Dynatrace.OneAgent.Sdk.Sample
{
    class StdErrLoggingCallback : ILoggingCallback
    {
        public void Error(string message) => Console.Error.WriteLine("[OneAgent SDK] Error: " + message);

        public void Warn(string message) => Console.Error.WriteLine("[OneAgent SDK] Warning: " + message);
    }

    public class SampleApplication
    {
        private static readonly Lazy<IOneAgentSdk> _oneAgentSdk = new Lazy<IOneAgentSdk>(() =>
        {
            IOneAgentSdk instance = OneAgentSdkFactory.CreateInstance();
            instance.SetLoggingCallback(new StdErrLoggingCallback());
            return instance;
        });

        /// <summary>
        /// shared instance of <see cref="IOneAgentSdk"/> for our sample application
        /// </summary>
        public static IOneAgentSdk OneAgentSdk => _oneAgentSdk.Value;

        /// <summary>
        /// CLI for executing the samples provided in the various sample classes
        /// </summary>
        public static void Main(string[] args)
        {
            Assembly sdkAssembly = typeof(IOneAgentSdk).GetTypeInfo().Assembly;
            string assemblyVersion = sdkAssembly.GetName().Version.ToString();
            string assemblyInformationalVersion = sdkAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine($"OneAgent SDK Version:   {assemblyInformationalVersion} ({assemblyVersion})");
            Console.WriteLine($"OneAgent SDK State:     {OneAgentSdk.CurrentState}");
            Console.WriteLine($"OneAgent SDK AgentInfo: AgentFound={OneAgentSdk.AgentInfo.AgentFound}, AgentCompatible={OneAgentSdk.AgentInfo.AgentCompatible}");
            Console.WriteLine($"                        AgentVersion={OneAgentSdk.AgentInfo.Version ?? "null"}\n");

            string infoMessage;
            if (OneAgentSdk.AgentInfo.AgentFound)
            {
                infoMessage = "A OneAgent was found";
                if (OneAgentSdk.AgentInfo.AgentCompatible)
                {
                    infoMessage += " and is compatible with this SDK version.";
                }
                else
                {
                    infoMessage += ", but is NOT compatible with SDK version.";
                }
            }
            else
            {
                infoMessage = "NO OneAgent was found!";
            }
            Console.WriteLine(infoMessage);

            switch (OneAgentSdk.CurrentState)
            {
                case Api.Enums.SdkState.ACTIVE:
                    Console.WriteLine("The SDK is active.");
                    break;
                case Api.Enums.SdkState.TEMPORARILY_INACTIVE:
                    Console.WriteLine("The SDK is temporarily inactive.");
                    break;
                case Api.Enums.SdkState.PERMANENTLY_INACTIVE:
                    Console.WriteLine("The SDK is permanently inactive. Leaving sample app...");
                    return;
            }
            Console.WriteLine("---------------------------------------------------------------\n");

            var testClasses = new[]
            {
                typeof(DatabaseRequestTracerSamples),
                typeof(RemoteCallTracerSamples),
                typeof(MessageTracerSamples),
                typeof(InProcessLinkTracerSamples),
                typeof(CustomRequestAttributeSamples),
                typeof(WebRequestTracerSamples),
                typeof(MetricSamples),
                typeof(CombinedSamples)
            };
            var testMethods = GetTestMethods(testClasses).ToList();
            var groups = from t in testMethods
                         group t.Name by t.DeclaringType.FullName into g
                         select new { TestClass = g.Key, TestMethods = g.ToList() };

            if (args.Length == 1)
            { // single run
                var mi = GetMethodInfoForTestName(testMethods, args[0]);
                if (mi != null)
                {
                    RunTestMethod(mi);
                }
                Thread.Sleep(5000); // assure everything is sent
            }
            else
            { // interactive CLI
                var index = 1;
                Console.WriteLine("Available samples:");
                foreach (var g in groups)
                {
                    Console.WriteLine($"\n[{g.TestClass}]");
                    foreach (var m in g.TestMethods)
                    {
                        Console.WriteLine($"\t{index++}: {m}");
                    }
                }
                Console.WriteLine("\n---------------------------------------------------------------\n");
                while (true)
                {
                    Console.WriteLine($"Enter sample index (e.g. '1') or fully qualified method name (or press Enter to exit):");
                    Console.Write("> ");
                    string testName = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(testName) || testName == "q")
                    {
                        return;
                    }
                    var mi = GetMethodInfoForTestName(testMethods, testName);
                    if (mi != null)
                    {
                        RunTestMethod(mi);
                    }
                    else
                    {
                        Console.WriteLine($"Test '{testName}' not found.");
                        Console.WriteLine("Please provide a qualified test name ('TestClass.TestMethod') or its index");
                    }
                }
            }
        }

        private static IEnumerable<MethodInfo> GetTestMethods(IEnumerable<Type> testClasses)
        {
            return testClasses.SelectMany(
                t => t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public));
        }

        private static MethodInfo GetMethodInfoForTestName(List<MethodInfo> testMethods, string testName)
        {
            MethodInfo mi;
            if (int.TryParse(testName, out var i))
            {
                mi = testMethods.ElementAtOrDefault(i - 1);
            }
            else
            {
                mi = testMethods.FirstOrDefault(m =>
                {
                    var fqn = $"{m.DeclaringType.FullName}.{m.Name}";
                    return fqn.Equals(testName, StringComparison.CurrentCultureIgnoreCase);
                });
            }
            return mi;
        }
        private static void RunTestMethod(MethodInfo mi)
        {
            Console.WriteLine($"Invoking {mi.DeclaringType.Name}.{mi.Name}");
            object ret = mi.Invoke(null, null);
            (ret as Task)?.Wait();
            Console.WriteLine("Done\n");
        }
    }
}
