using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynatrace.OneAgent.Sdk.Api
{
	public interface ITracer
	{

		/// <summary>
		///  Starts this Tracer for a synchronous call.
		///  This will capture all entry fields of the Tracer and
		///  start the time measurement. Some entry fields must be set, before the Tracer
		///  is being started (eg.
		///  IncomingWebRequestTracer#addRequestHeader(String, String)). See
		///  documentation of corresponding field for details. In case no other
		///  restriction is documented, fields must be set prior calling End().
		///  Start() might only be called once per Tracer.
		/// </summary>
		void Start();

		/// <summary>
		/// Same as Start() for asynchronous a call (which is typically a call with the await keyword).
		/// </summary>
		void StartAsync();

		/// <summary>
		/// Sets error information for this traced operation. An application should call
		/// this function to notify a Tracer that the traced operations has failed (e.g.
		/// an Exception has been thrown).
		/// error(String)} must only be called once. If a traced operation
		/// results in multiple errors and the application wants all of them to be
		/// captured, it must concatenate/combine them and then call
		/// Error(String)} once before calling {@link #end()}.
		/// </summary>
		/// <param name="message">error message(s)</param>
		void Error(String message);

		/// <summary>
		/// Ends this Tracer and stops time measurement. End() might only be called
		/// once per Tracer.
		/// </summary>
		void End();


		/// <summary>
		/// Convenient method.
		/// Traces an Action, which represents a synchronous call
		/// without return value. It automatically calls Start() and End(), 
		/// and in case of an exception also the Error(String) method.
		/// </summary>
		/// <param name="action">An action that wraps the method call you want to trace</param>
		void Trace(Action action);

		/// <summary>
		/// Convenient method.
		/// Traces a Func, which represents a synchronous call
		/// with a return value. It automatically calls Start() and End(), 
		/// and in case of an exception also the Error(String) method.
		/// </summary>
		/// <typeparam name="T">The return type of the wraped method</typeparam>
		/// <param name="func">A func that wraps the method call you want to trace</param>
		/// <returns>The return value of the wraped method</returns>
		T Trace<T>(Func<T> func);


		/// <summary>
		/// Convenient method.
		/// Traces an Func<Task>, which represents a asynchronous call
		/// without return value. It automatically calls StartAsync() and End(), 
		/// and in case of an exception also the Error(String) method.
		/// </summary>
		/// <param name="func">A func that wraps the method call you want to trace</param>
		/// <returns>The task that you waped in the Task, which you can await on</returns>
		Task Trace(Func<Task> func);

		/// <summary>
		/// Convenient method.
		/// Traces a Func<Task<T>>, which represents a asynchronous call
		/// with a return value. It automatically calls StartAsync() and End(), 
		/// and in case of an exception also the Error(String) method.
		/// </summary>
		/// <typeparam name="T">The return type of the wraped method</typeparam>
		/// <param name="func">A func that wraps the method call you want to trace</param>
		/// <returns>The task that you waped in the Task<T>, which you can await on</returns>
		Task<T> Trace<T>(Func<Task<T>> func);
	}
}
