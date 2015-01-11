using System;
using System.Configuration;
using cloudscribe.Configuration;


namespace cloudscribe.DbHelpers.SQLite
{
    public static class ConnectionString
    {
        public static string GetConnectionString()
        {
            if (AppSettings.UseConnectionStringSection) { return GetConnectionStringFromConnectionStringSection(); }

            
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/" + AppSettings.SqliteApp_Data_FileName);

                string connectionString = "data source="
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/" + AppSettings.SqliteApp_Data_FileName)
                    + ";version=3;";

                return connectionString;
            

            //string connectionString = AppSettings.SqliteConnectionString;
            //if (connectionString == "defaultdblocation")
            //{

            //    connectionString = "data source="
            //        + System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/" + AppSettings.SqliteApp_Data_FileName)
            //        + ";version=3;";

            //}
            //return connectionString;
        }

        private static string GetConnectionStringFromConnectionStringSection()
        {
            return ConfigurationManager.ConnectionStrings["SqlCeConnectionString"].ConnectionString;

        }

        // these methods are only for compatibility with import and upgrade utils
        // there is no replication supported for SQLite so there is no real need for different connection
        // strings for read/write
        public static String GetReadConnectionString()
        {
            return GetConnectionString();
        }

        public static String GetWriteConnectionString()
        {
            return GetConnectionString();
        }
    }
}
