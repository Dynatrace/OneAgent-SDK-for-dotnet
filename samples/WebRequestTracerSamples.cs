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
using Dynatrace.OneAgent.Sdk.Api.Infos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dynatrace.OneAgent.Sdk.Sample
{
    static class WebRequestTracerSamples
    {
        public static void OutgoingWebRequest()
        {
            var request = new MyCustomHttpRequest("https://www.example.com:8080/api/auth/user?group=42&location=Linz", "GET");
            request.Headers["Accept"] = "application/json; q=1.0, application/xml; q=0.8";
            request.Headers["Accept-Charset"] = "utf-8";
            request.Headers["Cache-Control"] = "no-cache,no-store,must-revalidate";

            IOutgoingWebRequestTracer tracer = SampleApplication.OneAgentSdk.TraceOutgoingWebRequest(request.Url, request.Method);

            foreach (KeyValuePair<string, string> header in request.Headers)
            {
                tracer.AddRequestHeader(header.Key, header.Value);
            }

            // start tracer and send request
            tracer.Trace(() =>
            {
                // add the Dynatrace tag or W3C Trace Context (based on your configuration) to request headers to allow
                // the agent in the web server to link the request together for end-to-end tracing
                tracer.InjectTracingHeaders((key, value) => request.Headers[key] = value); // Option 1: passing a stateful lambda, directly accessing 'request.Headers'

                MyCustomHttpResponse response = request.Execute();

                tracer.SetStatusCode(response.StatusCode);

                foreach (KeyValuePair<string, string> header in response.Headers)
                {
                    tracer.AddResponseHeader(header.Key, header.Value);
                }

            });
        }

        public static async Task OutgoingWebRequestAsync()
        {
            var request = new MyCustomHttpRequest("https://www.example.com:8080/api/auth/user?group=42&location=Linz", "GET");
            request.Headers["Accept"] = "application/json; q=1.0, application/xml; q=0.8";
            request.Headers["Accept-Charset"] = "utf-8";
            request.Headers["Cache-Control"] = "no-cache,no-store,must-revalidate";
            request.Headers["X-MyRequestHeader"] = "MyRequestValue";

            IOutgoingWebRequestTracer tracer = SampleApplication.OneAgentSdk.TraceOutgoingWebRequest(request.Url, request.Method);

            foreach (KeyValuePair<string, string> header in request.Headers)
            {
                tracer.AddRequestHeader(header.Key, header.Value);
            }

            // start tracer and send request
            await tracer.TraceAsync(async () =>
            {
                // add the Dynatrace tag or W3C Trace Context (based on your configuration) to request headers to allow
                // the agent in the web server to link the request together for end-to-end tracing
                tracer.InjectTracingHeaders((key, value, carrier) => carrier[key] = value, request.Headers); // Option 2: passing a stateless implementation, which gets 'request.Headers' passed as 'carrier'

                MyCustomHttpResponse response = await request.ExecuteAsync();

                tracer.SetStatusCode(response.StatusCode);

                foreach (KeyValuePair<string, string> header in response.Headers)
                {
                    tracer.AddResponseHeader(header.Key, header.Value);
                }
            });
        }

        private static MyCustomHttpResponse HandleIncomingWebRequest(MyCustomHttpRequest request, bool sendOutgoingRequest = false)
        {
            // create web application info object describing our web service
            IWebApplicationInfo webAppInfo = SampleApplication.OneAgentSdk
                .CreateWebApplicationInfo("WebShopProduction", "AuthenticationService", "/api/auth");

            IIncomingWebRequestTracer tracer = SampleApplication.OneAgentSdk
                .TraceIncomingWebRequest(webAppInfo, request.Url, request.Method);
            tracer.SetRemoteAddress(request.RemoteClientAddress);

            // adding all request headers ensures that tracing headers required
            // for end-to-end linking of requests are provided to the SDK
            foreach (KeyValuePair<string, string> header in request.Headers)
            {
                tracer.AddRequestHeader(header.Key, header.Value);
            }
            foreach (KeyValuePair<string, string> param in request.PostParameters)
            {
                tracer.AddParameter(param.Key, param.Value);
            }

            // start tracer
            return tracer.Trace(() =>
            {
                // send a nested outgoing request for demonstration
                if (sendOutgoingRequest)
                {
                    MyCustomHttpRequest outgoingRequest = new MyCustomHttpRequest("https://www.example.com:8081/api/auditlog", "POST");
                    IOutgoingWebRequestTracer outgoingTracer = SampleApplication.OneAgentSdk.TraceOutgoingWebRequest(outgoingRequest.Url, outgoingRequest.Method);
                    outgoingTracer.Trace(() =>
                    {
                        outgoingTracer.InjectTracingHeaders((name, value) => outgoingRequest.Headers[name] = value);
                        MyCustomHttpResponse incomingResponse = outgoingRequest.Execute();
                        outgoingTracer.SetStatusCode(incomingResponse.StatusCode);
                    });
                }

                var response = new MyCustomHttpResponse();

                // handle request and build response ...

                foreach (KeyValuePair<string, string> header in response.Headers)
                {
                    tracer.AddResponseHeader(header.Key, header.Value);
                }
                tracer.SetStatusCode(response.StatusCode);

                return response;
            });
        }

        public static void IncomingWebRequest()
        {
            MyCustomHttpRequest sampleRequest = CreateSampleWebRequest();
            HandleIncomingWebRequest(sampleRequest);
        }

        public static void LinkedIncomingOutgoingWebRequestWithTraceContext()
        {
            MyCustomHttpRequest sampleRequest = CreateSampleWebRequest();

            // simulate incoming W3C Trace Context (sample taken from W3C spec)
            sampleRequest.Headers["traceparent"] = "00-0af7651916cd43dd8448eb211c80319c-00f067aa0ba902b7-01";
            sampleRequest.Headers["tracestate"] = "rojo=00f067aa0ba902b7,congo=t61rcWkgMzE";

            HandleIncomingWebRequest(sampleRequest, sendOutgoingRequest: true);
        }

        public static void LinkedOutgoingIncomingWebRequest()
        {
            var request = new MyCustomHttpRequest("https://www.example.com:8080/api/auth/user?group=42&location=Linz", "GET");
            request.Headers["Accept"] = "application/json; q=1.0, application/xml; q=0.8";
            request.Headers["Accept-Charset"] = "utf-8";
            request.Headers["Cache-Control"] = "no-cache,no-store,must-revalidate";

            IOutgoingWebRequestTracer tracer = SampleApplication.OneAgentSdk.TraceOutgoingWebRequest(request.Url, request.Method);

            foreach (KeyValuePair<string, string> header in request.Headers)
            {
                tracer.AddRequestHeader(header.Key, header.Value);
            }

            // start tracer and send request
            tracer.Trace(() =>
            {
                // add the Dynatrace tag or W3C Trace Context (based on your configuration) to request headers to allow
                // the agent in the web server to link the request together for end-to-end tracing
                tracer.InjectTracingHeaders((key, value) => request.Headers[key] = value); // Option 1: passing a stateful lambda, directly accessing 'request.Headers'

                MyCustomHttpResponse response = null;

                // represents server side processing
                Thread server = new Thread(() =>
                {
                    response = HandleIncomingWebRequest(request);
                });
                server.Start();
                server.Join(); // sync request, wait for result

                tracer.SetStatusCode(response.StatusCode);

                foreach (KeyValuePair<string, string> header in response.Headers)
                {
                    tracer.AddResponseHeader(header.Key, header.Value);
                }
            });
        }

        public static void LinkedOutgoingIncomingOutgoingWebRequest()
        {
            MyCustomHttpRequest request = new MyCustomHttpRequest("https://www.example.com:8080/api/auth/user?group=42&location=Linz", "GET");
            request.Headers["Accept"] = "application/json; q=1.0, application/xml; q=0.8";
            request.Headers["Accept-Charset"] = "utf-8";
            request.Headers["Cache-Control"] = "no-cache,no-store,must-revalidate";

            IOutgoingWebRequestTracer tracer = SampleApplication.OneAgentSdk.TraceOutgoingWebRequest(request.Url, request.Method);

            foreach (KeyValuePair<string, string> header in request.Headers)
            {
                tracer.AddRequestHeader(header.Key, header.Value);
            }

            // start tracer and send request
            tracer.Trace(() =>
            {
                // add the Dynatrace tag or W3C Trace Context (based on your configuration) to request headers to allow
                // the agent in the web server to link the request together for end-to-end tracing
                tracer.InjectTracingHeaders((key, value, carrier) => carrier[key] = value, request.Headers); // Option 2: passing a stateless implementation, which gets 'request.Headers' passed as 'carrier'

                MyCustomHttpResponse response = null;

                // represents server side processing
                Thread server = new Thread(() =>
                {
                    response = HandleIncomingWebRequest(request, sendOutgoingRequest: true);
                });
                server.Start();
                server.Join(); // sync request, wait for result

                tracer.SetStatusCode(response.StatusCode);

                foreach (KeyValuePair<string, string> header in response.Headers)
                {
                    tracer.AddResponseHeader(header.Key, header.Value);
                }
            });
        }

        #region Helper classes for demonstration

        private static MyCustomHttpRequest CreateSampleWebRequest()
        {
            var request = new MyCustomHttpRequest(
                "https://www.example.com:8080/api/auth/user?group=42&location=Linz", "GET", "198.51.100.123");

            request.Headers["Accept"] = "application/json; q=1.0, application/xml; q=0.8";
            request.Headers["Accept-Charset"] = "utf-8";
            request.Headers["Cache-Control"] = "no-cache,no-store,must-revalidate";
            request.Headers["X-MyRequestHeader"] = "MyRequestValue";

            return request;
        }

        private class MyCustomHttpRequest
        {
            public string Url { get; }
            public string Method { get; }
            public string RemoteClientAddress { get; }
            public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();
            public IDictionary<string, string> PostParameters { get; } = new Dictionary<string, string>();

            public MyCustomHttpRequest(string url, string method, string remoteClientAddress = null)
            {
                Url = url;
                Method = method;
                RemoteClientAddress = remoteClientAddress;
            }

            public MyCustomHttpResponse Execute()
            {
                Thread.Sleep(50);
                return new MyCustomHttpResponse();
            }

            public async Task<MyCustomHttpResponse> ExecuteAsync()
            {
                await Task.Delay(50);
                return new MyCustomHttpResponse();
            }
        }

        private class MyCustomHttpResponse
        {
            public IDictionary<string, string> Headers { get; }

            public int StatusCode { get; } = 200;

            public MyCustomHttpResponse()
            {
                Headers = new Dictionary<string, string>();
                Headers["Content-Type"] = "application/json; charset=UTF-8";
                Headers["Content-Length"] = "42";
                Headers["Via"] = "1.1 varnish (Varnish/5.1), 1.1 varnish (Varnish/5.1)";
                Headers["X-MyResponseHeader"] = "MyResponseValue";
            }
        }

        #endregion
    }
}
