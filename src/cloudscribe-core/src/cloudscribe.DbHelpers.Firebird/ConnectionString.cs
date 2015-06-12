using System;
//using System.Configuration;
using cloudscribe.Configuration;


namespace cloudscribe.DbHelpers.Firebird
{
    public static class ConnectionString
    {
        public static String GetReadConnectionString()
        {
            return AppSettings.FirebirdConnectionString;

        }

        public static String GetWriteConnectionString()
        {
            //if (AppSettings.UseConnectionStringSection) { return GetReadConnectionStringFromConnectionStringSection(); }

            if (AppSettings.FirebirdWriteConnectionString.Length > 0)
            {
                return AppSettings.FirebirdWriteConnectionString;
            }

            return AppSettings.FirebirdConnectionString;

        }

        //private static string GetWriteConnectionStringFromConnectionStringSection()
        //{
        //    if (AppSettings.UseConnectionStringSection) { return GetWriteConnectionStringFromConnectionStringSection(); }

        //    if (ConfigurationManager.ConnectionStrings["FirebirdWriteConnectionString"] != null)
        //    {
        //        return ConfigurationManager.ConnectionStrings["FirebirdWriteConnectionString"].ConnectionString;
        //    }

        //    return ConfigurationManager.ConnectionStrings["FirebirdConnectionString"].ConnectionString;

        //}

        //private static string GetReadConnectionStringFromConnectionStringSection()
        //{
        //    return ConfigurationManager.ConnectionStrings["FirebirdConnectionString"].ConnectionString;

        //}
    }
}
