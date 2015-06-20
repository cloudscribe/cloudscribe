// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2015-06-20
// 


using cloudscribe.Configuration;
using Microsoft.Framework.ConfigurationModel;

namespace cloudscribe.DbHelpers.MySql
{
    public static class Extensions
    {
        public static string GetMySqlWriteConnectionString(this IConfiguration configuration)
        {
            return configuration.GetOrDefault("AppSettings:MySqlWriteConnectionString",
                configuration.Get("AppSettings:MySqlConnectionString")
                );
        }

        public static string GetMySqlReadConnectionString(this IConfiguration configuration)
        {
            return configuration.GetOrDefault("AppSettings:MySqlReadConnectionString",
                configuration.Get("AppSettings:MySqlConnectionString")
                );
        }
    }
}
