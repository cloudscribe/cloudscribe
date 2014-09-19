using System;
using System.Configuration;
using cloudscribe.Configuration;

namespace cloudscribe.DbHelpers.MySql
{
    public static class ConnectionString
    {
        public static String GetReadConnectionString()
        {
            if (AppSettings.UseConnectionStringSection) { return GetReadConnectionStringFromConnectionStringSection(); }

            return AppSettings.MySqlConnectionString;

        }

        public static String GetWriteConnectionString()
        {
            if (AppSettings.UseConnectionStringSection) { return GetWriteConnectionStringFromConnectionStringSection(); }

            if (AppSettings.MySqlWriteConnectionString.Length > 0)
            {
                return AppSettings.MySqlWriteConnectionString;
            }

            return AppSettings.MySqlConnectionString;
        }

        private static string GetWriteConnectionStringFromConnectionStringSection()
        {
            if (ConfigurationManager.ConnectionStrings["MySqlWriteConnectionString"] != null)
            {
                return ConfigurationManager.ConnectionStrings["MySqlWriteConnectionString"].ConnectionString;
            }

            return ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        }

        private static string GetReadConnectionStringFromConnectionStringSection()
        {
            return ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        }
    }
}
