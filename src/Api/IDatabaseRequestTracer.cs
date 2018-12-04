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
    /// Interface for outgoing database tracer.
    /// </summary>
    public interface IDatabaseRequestTracer : ITracer
	{
		/// <summary>
		/// Adds optional information about retrieved rows of the traced database request.
        /// Can only be set prior to calling End.
		/// </summary>
		/// <param name="rowsReturned">number of rows returned by this traced database request. Only positive values are allowed.</param>
		void SetRowsReturned(int rowsReturned);


        /// <summary>
        /// Adds optional information about round-trip count to database server.
        /// Can only be set prior to calling End.
        /// </summary>
        /// <param name="roundTripCount">count of round-trips that took place. Only positive values are allowed</param>
        void SetRoundTripCount(int roundTripCount);
	}
}
