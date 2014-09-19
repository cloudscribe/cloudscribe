using System;
using System.Configuration;
using cloudscribe.Configuration;


namespace cloudscribe.DbHelpers.pgsql
{
    public static class ConnectionString
    {
        public static String GetReadConnectionString()
        {
            return AppSettings.PostgreSQLConnectionString;

        }

        public static String GetWriteConnectionString()
        {
            if (AppSettings.PostgreSQLWriteConnectionString.Length > 0)
            {
                return AppSettings.PostgreSQLWriteConnectionString;
            }
            return AppSettings.PostgreSQLConnectionString;

        }

        private static string GetWriteConnectionStringFromConnectionStringSection()
        {
            if (ConfigurationManager.ConnectionStrings["PostgreSQLWriteConnectionString"] != null)
            {
                return ConfigurationManager.ConnectionStrings["PostgreSQLWriteConnectionString"].ConnectionString;
            }

            return ConfigurationManager.ConnectionStrings["PostgreSQLConnectionString"].ConnectionString;

        }

        private static string GetReadConnectionStringFromConnectionStringSection()
        {
            return ConfigurationManager.ConnectionStrings["PostgreSQLConnectionString"].ConnectionString;

        }

    }
}
