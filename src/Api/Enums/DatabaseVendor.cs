//
// Copyright 2019 Dynatrace LLC
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

#pragma warning disable CS1591
namespace Dynatrace.OneAgent.Sdk.Api.Enums
{
    /// <summary>
    /// Enumerates all well-known database vendors. Used for <see cref="IOneAgentSdk.CreateDatabaseInfo"/>
    /// Using these constants ensures that services captured by OneAgentSDK are handled the same way as traced via built-in sensors.
    /// </summary>
    public static class DatabaseVendor
    {
        public static string APACHE_HIVE => "ApacheHive";
        public static string CLOUDSCAPE => "Cloudscape";
        public static string HSQLDB => "HSQLDB";
        public static string PROGRESS => "Progress";
        public static string MAXDB => "MaxDB";
        public static string HANADB => "HanaDB";
        public static string INGRES => "Ingres";
        public static string FIRST_SQL => "FirstSQL";
        public static string ENTERPRISE_DB => "EnterpriseDB";
        public static string CACHE => "Cache";
        public static string ADABAS => "Adabas";
        public static string FIREBIRD => "Firebird";
        public static string DB2 => "DB2";
        public static string DERBY_CLIENT => "Derby Client";
        public static string DERBY_EMBEDDED => "Derby Embedded";
        public static string FILEMAKER => "Filemaker";
        public static string INFORMIX => "Informix";
        public static string INSTANT_DB => "InstantDb";
        public static string INTERBASE => "Interbase";
        public static string MYSQL => "MySQL";
        public static string MARIADB => "MariaDB";
        public static string NETEZZA => "Netezza";
        public static string ORACLE => "Oracle";
        public static string PERVASIVE => "Pervasive";
        public static string POINTBASE => "Pointbase";
        public static string POSTGRESQL => "PostgreSQL";
        public static string SQLSERVER => "SQL Server";
        public static string SQLITE => "sqlite";
        public static string SYBASE => "Sybase";
        public static string TERADATA => "Teradata";
        public static string VERTICA => "Vertica";
        public static string CASSANDRA => "Cassandra";
        public static string H2 => "H2";
        public static string COLDFUSION_IMQ => "ColdFusion IMQ";
        public static string REDSHIFT => "Amazon Redshift";
        public static string COUCHBASE => "Couchbase";
    }
}
#pragma warning restore CS1591
