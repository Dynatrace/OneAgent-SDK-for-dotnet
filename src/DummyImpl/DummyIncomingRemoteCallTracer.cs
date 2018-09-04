using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynatrace.OneAgent.Sdk.Api.DummyImpl
{
	internal class DummyIncomingRemoteCallTracer : IIncomingRemoteCallTracer
	{
		public void End()
		{
		}

		public void Error(string message)
		{
		}

		public void SetDynatraceByteTag(byte[] tag)
		{
		}

		public void SetDynatraceStringTag(string tag)
		{
		}

		public void SetProtocolName(string protocolName)
		{
		}

		public void Start()
		{
		}

		public void StartAsync()
		{

		}

		public void Trace(Action action)
		{
			action();
		}

		public T Trace<T>(Func<T> func)
		{
			return func();
		}

		public Task Trace(Func<Task> func)
		{
			return func();
		}

		public Task<T> Trace<T>(Func<Task<T>> func)
		{
			return func();
		}
	}
}
