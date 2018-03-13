using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dynatrace.OneAgent.Sdk.Api
{
	public interface IDatabaseRequestTracer : ITracer
	{
		/// <summary>
		/// Adds optional information about retrieved rows of the traced database request. Must be set before end() 
		/// of this tracer is being called.
		/// </summary>
		/// <param name="rowsReturned">number of rows returned by this traced database request. Only positive values are allowed.</param>
		void SetRowsReturned(int rowsReturned);


		/// <summary>
		/// Adds optional information about round-trip count to database server. Must be set before end() 
		/// of this tracer is being called.
		/// </summary>
		/// <param name="roundTripCount">count of round-trips that took place. Only positive values are allowed</param>
		void SetRoundTripCount(int roundTripCount);
	}
}
