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

using System;
using System.Threading.Tasks;

namespace Dynatrace.OneAgent.Sdk.Api
{
    /// <summary>
    /// Common interface for timing-related methods.
    /// Not to be directly used by SDK user.
    /// </summary>
    public interface ITracer
	{

		/// <summary>
		///  Starts this Tracer for a synchronous call.
		///  This will capture all entry fields of the Tracer and start the time measurement.
		///  Some entry fields must be set, before the Tracer is being started.
		///  See documentation of corresponding field for details.
		///  In case no other restriction is documented, fields must be set prior calling End().
		///  Start() might only be called once per Tracer.
		/// </summary>
		void Start();

		/// <summary>
		/// Same as Start() for an asynchronous call (which is typically a call with the await keyword).
		/// </summary>
		Task StartAsync();

		/// <summary>
		/// Sets error information for this traced operation. An application should call
		/// this function to notify a Tracer that the traced operations has failed (e.g.
		/// an Exception has been thrown).
		/// Error(string) must only be called once. If a traced operation
		/// results in multiple errors and the application wants all of them to be
		/// captured, it must concatenate/combine them and then call
		/// Error(string) once before calling End().
		/// </summary>
		/// <param name="message">error message(s)</param>
		void Error(string message);

		/// <summary>
		/// Ends this Tracer and stops time measurement. End() might only be called
		/// once per Tracer.
		/// </summary>
		void End();


        /// <summary>
        /// Convenience method.
        /// Traces an Action, which represents a synchronous call
        /// without return value. It automatically calls Start() and End(), 
        /// and in case of an exception also the Error(string) method.
        /// </summary>
        /// <param name="action">An action that wraps the method call you want to trace</param>
        void Trace(Action action);

        /// <summary>
        /// Convenience method.
        /// Traces a Func, which represents a synchronous call
        /// with a return value. It automatically calls Start() and End(), 
        /// and in case of an exception also the Error(string) method.
        /// </summary>
        /// <typeparam name="T">The return type of the wrapped method</typeparam>
        /// <param name="func">A func that wraps the method call you want to trace</param>
        /// <returns>The return value of the wrapped method</returns>
        T Trace<T>(Func<T> func);


        /// <summary><![CDATA[
        /// Convenience method.
        /// Traces a Func<Task>, which represents an asynchronous call
        /// without a return value. It automatically calls StartAsync() and End(), 
        /// and in case of an exception also the Error(string) method.
        /// ]]></summary>
        /// <param name="func">A func that wraps the method call you want to trace</param>
        /// <returns>The task that you wrapped in the Task, which you can await on</returns>
        Task TraceAsync(Func<Task> func);

        /// <summary><![CDATA[
        /// Convenience method.
        /// Traces a Func<Task<T>>, which represents an asynchronous call
        /// with a return value. It automatically calls StartAsync() and End(), 
        /// and in case of an exception also the Error(string) method.
        /// ]]></summary>
        /// <typeparam name="T">The return type of the wrapped method</typeparam>
        /// <param name="func">A func that wraps the method call you want to trace</param>
        /// <returns><![CDATA[ The task that you wrapped in the Task<T>, which you can await on ]]></returns>
        Task<T> TraceAsync<T>(Func<Task<T>> func);
	}
}
