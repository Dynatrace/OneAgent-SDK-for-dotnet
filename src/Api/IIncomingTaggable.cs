using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dynatrace.OneAgent.Sdk.Api {
	public interface IIncomingTaggable {
		void SetDynatraceStringTag(string tag);

		void SetDynatraceByteTag(byte[] tag);
	}
}
