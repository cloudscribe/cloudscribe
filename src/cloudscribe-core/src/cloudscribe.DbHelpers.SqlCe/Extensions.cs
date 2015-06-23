//// Author:					Joe Audette
//// Created:					2015-06-21
//// Last Modified:			2015-06-21
//// 


//using Microsoft.AspNet.Hosting;
//using Microsoft.Framework.ConfigurationModel;
//using System;

//namespace cloudscribe.DbHelpers.SqlCe
//{
//    public static class Extensions
//    {
//        public static string GetSqlCeConnectionString(this IConfiguration configuration, IHostingEnvironment hostingEnvironment)
//        {
//            string sqlCeFileName = configuration.Get("AppSettings:SqlCeApp_Data_FileName");
//            string connectionString;

//            if (!string.IsNullOrEmpty(sqlCeFileName))
//            {
//                //TODO: is App_Data folder still a  thing in dnxcore apps?
//                // is there another folder outside the web root that we could use easily?
//                // I know that the dlls are no longer below the webroot, need to look at
//                // publishing artifacts to see where we might could put it
//                string path = hostingEnvironment.MapPath("~/App_Data/" + sqlCeFileName);
//                connectionString = "Data Source=" + path + ";Persist Security Info=False;";

//                return connectionString;
//            }

//            connectionString = configuration.Get("AppSettings:SqlCeConnectionString");


//            if (string.IsNullOrEmpty(connectionString))
//            {
//                throw new ArgumentException("could not find connection string AppSettings:SqlCeConnectionString");
//            }

//            return connectionString;
//        }
//    }
//}
