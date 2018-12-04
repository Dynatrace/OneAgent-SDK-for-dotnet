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

namespace Dynatrace.OneAgent.Sdk.Api
{
    /// <summary>
    /// Common interface for outgoing requests which include linking.
    /// Not to be referenced directly by SDK users.
    /// </summary>
    public interface IOutgoingTaggable
    {
        /// <summary>
        /// Creates a Dynatrace tag and returns the string representation of it. 
        /// This tag has to be transported with the remoting protocol to the destination.
        /// See <see cref="IIncomingTaggable"/> on how to continue a path with provided tag on the server side.
        /// </summary>
        /// <returns>the tag, never null</returns>
        string GetDynatraceStringTag();

        /// <summary>
        /// Same as <see cref="GetDynatraceStringTag"/> but returning the tag as binary representation.
        /// </summary>
        /// <returns>the tag, never null</returns>
		byte[] GetDynatraceByteTag();
	}
}