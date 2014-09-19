
using System;
using System.Configuration;
using cloudscribe.Configuration;

namespace cloudscribe.DbHelpers.MSSQL
{
    public static class ConnectionString
    {
        public static string GetReadConnectionString()
        {
            if (AppSettings.UseConnectionStringSection) { return GetReadConnectionStringFromConnectionStringSection(); }

            return AppSettings.MSSQLConnectionString;

        }

        /// <summary>
        /// Gets the connection string for write.
        /// </summary>
        /// <returns></returns>
        public static string GetWriteConnectionString()
        {
            if (AppSettings.UseConnectionStringSection) { return GetWriteConnectionStringFromConnectionStringSection(); }

            if (AppSettings.MSSQLWriteConnectionString.Length > 0)
            {
                return AppSettings.MSSQLWriteConnectionString;
            }

            return AppSettings.MSSQLConnectionString;

        }

        private static string GetWriteConnectionStringFromConnectionStringSection()
        {
            if (ConfigurationManager.ConnectionStrings["MSSQLWriteConnectionString"] != null)
            {
                return ConfigurationManager.ConnectionStrings["MSSQLWriteConnectionString"].ConnectionString;
            }

            return ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString;

        }

        private static string GetReadConnectionStringFromConnectionStringSection()
        {
            return ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString;

        }

        

    }




}

