using cloudscribe.Configuration;
using System.Configuration;

namespace cloudscribe.DbHelpers.SqlCe
{
    public static class ConnectionString
    {
        public static string GetConnectionString()
        {
            if (AppSettings.UseConnectionStringSection) { return GetConnectionStringFromConnectionStringSection(); }

            if (AppSettings.SqlCeApp_Data_FileName.Length > 0)
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/" + AppSettings.SqlCeApp_Data_FileName);
                string connectionString = "Data Source=" + path + ";Persist Security Info=False;";

                return connectionString;
            }

            return AppSettings.SqlCeConnectionString;

        }

        private static string GetConnectionStringFromConnectionStringSection()
        {
            return ConfigurationManager.ConnectionStrings["SqlCeConnectionString"].ConnectionString;

        }

        
    }
}
