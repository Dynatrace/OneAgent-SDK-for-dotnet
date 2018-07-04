using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dynatrace.OneAgent.Sdk.Api.Enums
{
	/// <summary>
	/// Encapsulates database vendor names that are known to Dynatrace.
	/// </summary>
	public static class DatabaseVendor
	{
		public static String APACHE_HIVE => "ApacheHive";
		public static String CLOUDSCAPE => "Cloudscape";
		public static String HSQLDB => "HSQLDB";
		public static String PROGRESS => "Progress";
		public static String MAXDB => "MaxDB";
		public static String HANADB => "HanaDB";
		public static String INGRES => "Ingres";
		public static String FIRST_SQL => "FirstSQL";
		public static String ENTERPRISE_DB => "EnterpriseDB";
		public static String CACHE => "Cache";
		public static String ADABAS => "Adabas";
		public static String FIREBIRD => "Firebird";
		public static String DB2 => "DB2";
		public static String DERBY_CLIENT => "Derby Client";
		public static String DERBY_EMBEDDED => "Derby Embedded";
		public static String FILEMAKER => "Filemaker";
		public static String INFORMIX => "Informix";
		public static String INSTANT_DB => "InstantDb";
		public static String INTERBASE => "Interbase";
		public static String MYSQL => "MySQL";
		public static String MARIADB => "MariaDB";
		public static String NETEZZA => "Netezza";
		public static String ORACLE => "Oracle";
		public static String PERVASIVE => "Pervasive";
		public static String POINTBASE => "Pointbase";
		public static String POSTGRESQL => "PostgreSQL";
		public static String SQLSERVER => "SQL Server";
		public static String SQLITE => "sqlite";
		public static String SYBASE => "Sybase";
		public static String TERADATA => "Teradata";
		public static String VERTICA => "Vertica";
		public static String CASSANDRA => "Cassandra";
		public static String H2 => "H2";
		public static String COLDFUSION_IMQ => "ColdFusion IMQ";
		public static String REDSHIFT => "Amazon Redshift";
		public static String COUCHBASE => "Couchbase";
	}
}