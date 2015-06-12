using cloudscribe.Configuration;
//using System.Configuration;
//using Microsoft.Framework.DependencyInjection;
//using Microsoft.AspNet.Hosting;

namespace cloudscribe.DbHelpers.SqlCe
{
    public static class ConnectionString
    {
        public static string GetConnectionString()
        {
            // if (AppSettings.UseConnectionStringSection) { return GetConnectionStringFromConnectionStringSection(); }

            //TODO: I guess for now the connection stirng path will have to be provided in 
            // AppSettings.SqlCeConnectionString
            // IHostingEnvironment provided in Startup.cs has MapPath
            // but can't use it here in a static method, would need either constructor injection
            // or a service locator (which may exists in the new dI framework but can't find documentation currently)

            //if (AppSettings.SqlCeApp_Data_FileName.Length > 0)
            //{

            //    string path = HostingEnvironment.MapPath("~/App_Data/" + AppSettings.SqlCeApp_Data_FileName);
            //    string connectionString = "Data Source=" + path + ";Persist Security Info=False;";

            //    return connectionString;
            //}

            return AppSettings.SqlCeConnectionString;

        }

        //private static string GetConnectionStringFromConnectionStringSection()
        //{
        //    return ConfigurationManager.ConnectionStrings["SqlCeConnectionString"].ConnectionString;

        //}


    }
}
