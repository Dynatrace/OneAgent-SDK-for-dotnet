using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dynatrace.OneAgent.Sdk.Api
{
	/// <summary>
	///  Common interface for incoming requests which include linking.
	///  Not to be	directly used by SDK user.
	/// </summary>
	public interface IIncomingTaggable
	{

		/// <summary>
		///  Sets the tag using the string format.
		///  
		///  An application can call this function to set the incoming tag of an "incoming
		///  taggable" tracer using the string representation. An "incoming taggable"
		///  tracer has one tag. Calling this method more than once, will overwrite any
		///  tag that was set by either {@link #setDynatraceByteTag(byte[])} or
		///  {@link #setDynatraceStringTag(String)}.
		///  
		/// This function can not be used after the tracer was started.
		/// </summary>
		/// <param name="tag">if null or an empty string, the incoming tag will be reset (cleared)</param>
		void SetDynatraceStringTag(string tag);

		/// <summary>
		/// Same as {@link #setDynatraceStringTag(String)}, but tag is provided in binary format.
		/// </summary>
		/// <param name="tag">if null or an empty string, the incoming tag will be reset (cleared).</param>
		void SetDynatraceByteTag(byte[] tag);
	}
}
