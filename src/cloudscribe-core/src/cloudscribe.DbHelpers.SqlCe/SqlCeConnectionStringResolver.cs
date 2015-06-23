// Author:					Joe Audette
// Created:					2014-06-22
// Last Modified:			2015-06-23
// 

using Microsoft.AspNet.Hosting;
using Microsoft.Framework.ConfigurationModel;
using System;

namespace cloudscribe.DbHelpers.SqlCe
{
    public class SqlCeConnectionStringResolver
    {
        public SqlCeConnectionStringResolver(
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }
            if (hostingEnvironment == null) { throw new ArgumentNullException(nameof(hostingEnvironment)); }

            env = hostingEnvironment;
            config = configuration;

        }

        private IHostingEnvironment env;
        private IConfiguration config;
        private string sqlCeFilePath = string.Empty;

        public string SqlCeFilePath
        {
            get { return sqlCeFilePath; }
        }

        public string Resolve()
        {
            string sqlCeFileName = config.Get("AppSettings:SqlCeApp_Data_FileName");
            string connectionString;

            if (!string.IsNullOrEmpty(sqlCeFileName))
            {
                //TODO: is App_Data folder still a  thing in dnxcore apps?
                // is there another folder outside the web root that we could use easily?
                // I know that the dlls are no longer below the webroot, need to look at
                // publishing artifacts to see where we might could put it
                sqlCeFilePath = env.MapPath("~/App_Data/" + sqlCeFileName);
                connectionString = "Data Source=" + sqlCeFilePath + ";Persist Security Info=False;";

                return connectionString;
            }

            connectionString = config.Get("AppSettings:SqlCeConnectionString");


            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("could not find connection string AppSettings:SqlCeConnectionString");
            }

            return connectionString;
        }
    }
}
