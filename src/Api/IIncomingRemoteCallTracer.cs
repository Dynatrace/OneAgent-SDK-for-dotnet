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

namespace Dynatrace.OneAgent.Sdk.Api
{
    /// <summary>
    /// Tracer used for tracing an incoming remote call.
    /// </summary>
    public interface IIncomingRemoteCallTracer : ITracer, IIncomingTaggable
    {

        /// <summary>
        /// Sets the name of the used remoting protocol.
        /// This is optional and only used for display purposes.
        /// Can only be set before starting the tracer.
        /// </summary>
        /// <param name="protocolName"></param>
        void SetProtocolName(string protocolName);
    }
}
