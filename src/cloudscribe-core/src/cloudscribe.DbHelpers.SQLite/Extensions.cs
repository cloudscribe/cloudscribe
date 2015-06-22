// Author:					Joe Audette
// Created:					2015-06-22
// Last Modified:			2015-06-22
// 


using Microsoft.AspNet.Hosting;
using Microsoft.Framework.ConfigurationModel;
using System;

namespace cloudscribe.DbHelpers.SQLite
{
    public static class Extensions
    {
        public static string GetSQLiteConnectionString(this IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            string fileName = configuration.Get("AppSettings:SqliteApp_Data_FileName");
            string connectionString;

            if (!string.IsNullOrEmpty(fileName))
            {
                //TODO: is App_Data folder still a  thing in dnxcore apps?
                // is there another folder outside the web root that we could use easily?
                // I know that the dlls are no longer below the webroot, need to look at
                // publishing artifacts to see where we might could put it
                string path = hostingEnvironment.MapPath("~/App_Data/" + fileName);
                connectionString = "data source=" + path + ";version=3;";

                return connectionString;
            }

            connectionString = configuration.Get("AppSettings:SQLiteConnectionString");


            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("could not find connection string AppSettings:SQLiteConnectionString");
            }

            return connectionString;
        }
    }
}
