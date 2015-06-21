// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2015-06-21
// 


using cloudscribe.Configuration;
using Microsoft.Framework.ConfigurationModel;
using System;

namespace cloudscribe.DbHelpers.MySql
{
    public static class Extensions
    {
        public static string GetMySqlWriteConnectionString(this IConfiguration configuration)
        {
            string connectionString = configuration.GetOrDefault("AppSettings:MySqlWriteConnectionString",
                configuration.Get("AppSettings:MySqlConnectionString")
                );

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("could not find connection string AppSettings:MySqlConnectionString");
            }

            return connectionString;
        }

        public static string GetMySqlReadConnectionString(this IConfiguration configuration)
        {
            string connectionString = configuration.GetOrDefault("AppSettings:MySqlReadConnectionString",
                configuration.Get("AppSettings:MySqlConnectionString")
                );

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("could not find connection string AppSettings:MySqlConnectionString");
            }

            return connectionString;
        }
    }
}
