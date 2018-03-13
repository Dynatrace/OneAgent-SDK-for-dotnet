using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dynatrace.OneAgent.Sdk.Api.Enums {
	public enum ChannelType {
		OTHER,
		TCP_IP,
		UNIX_DOMAIN_SOCKET,
		NAMED_PIPE,
		IN_PROCESS
	}
}
