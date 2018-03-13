using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dynatrace.OneAgent.Sdk.Api {
	public interface ITracer {

		void Start();

		// only to be called once, concatenation has to be handled by user, DOES NOT end tracer
		// if multiple overloads exist, at most one of them can be called and only once
		void Error(String message);

		void End();
	}
}
