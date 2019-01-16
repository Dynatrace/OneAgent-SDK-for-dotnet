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
    /// Common interface for incoming requests which include linking.
    /// Not to be referenced directly by SDK users.
    /// </summary>
    public interface IIncomingTaggable
	{

        /// <summary>
        /// Sets the tag using the string format.
        /// 
        /// An application can call this function to set the incoming tag of an "incoming
        /// taggable" tracer using the string representation. An "incoming taggable"
        /// tracer has one tag. Calling this method more than once will overwrite any
        /// tag that was set by either <see cref="SetDynatraceByteTag(byte[])"/> or
        /// <see cref="SetDynatraceStringTag(string)"/>.
        ///  
        /// This function can not be used after the tracer was started.
        /// </summary>
        /// <param name="tag">if null or an empty string, the incoming tag will be reset (cleared)</param>
        void SetDynatraceStringTag(string tag);

        /// <summary>
        /// Same as <see cref="SetDynatraceStringTag(string)"/> but tag is provided in binary format.
        /// </summary>
        /// <param name="tag">if null or an empty array, the incoming tag will be reset (cleared).</param>
        void SetDynatraceByteTag(byte[] tag);
	}
}
