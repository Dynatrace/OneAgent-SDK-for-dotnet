using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynatrace.OneAgent.Sdk.Api.Enums;
using Dynatrace.OneAgent.Sdk.Api.Infos;

namespace Dynatrace.OneAgent.Sdk.Api {

	/// <summary>
	/// Interface implemented by OneAgentSDK. Retrieved by OneAgentSdkFactory.CreateInstance(). For details see:
	/// <a href="https://github.com/Dynatrace/OneAgent-SDK#oneagentsdkobject">https://github.com/Dynatrace/OneAgent-SDK#oneagentsdkobject</a>
	/// </summary>
	public interface IOneAgentSdk {

		IIncomingRemoteCallTracer TraceIncomingRemoteCall(String serviceMethod, String serviceName, String serviceEndpoint);

		/// <summary>
		///  Creates a tracer for tracing outgoing SQL database requests.
		/// </summary>
		/// <param name="databaseInfo">information about database</param>
		/// <param name="statement">database SQL statement</param>
		/// <returns>DatabaseRequestTracer to work with</returns>
		IDatabaseRequestTracer TraceSQLDatabaseRequest(IDatabaseInfo databaseInfo, String statement);

		/// <summary>
		/// Initializes a DatabaseInfo instance that is required for tracing database requests.
		/// </summary>
		/// <param name="name">name of the database</param>
		/// <param name="vendor">database vendor name (e.g. Oracle, MySQL, ...), can be a user defined name 
		///  If possible use a constant defined in com.dynatrace.oneagent.sdk.api.enums.DatabaseVendor</param>
		/// <param name="channelType">communication protocol used to communicate with the database.</param>
		/// <param name="channelEndpoint">this represents the communication endpoint for the database. This information allows Dynatrace to tie the database requests to a specific process or cloud service. It is optional.
		/// * for TCP/IP: host name/IP of the server-side (can include port in the form of "host:port")
		/// * for UNIX domain sockets: name of domain socket file
		/// * for named pipes: name of pipe</param>
		/// <returns>DatabaseInfo instance to work with</returns>
		IDatabaseInfo CreateDatabaseInfo(String name, String vendor, ChannelType channelType, String channelEndpoint);
	}
}