// Author:					Joe Audette
// Created:					2015-06-20
// Last Modified:			2015-06-20
// 


using cloudscribe.Configuration;
using Microsoft.Framework.ConfigurationModel;

namespace cloudscribe.DbHelpers.Firebird
{
    public static class Extensions
    {
        public static string GetFirebirdWriteConnectionString(this IConfiguration configuration)
        {
            return configuration.GetOrDefault("AppSettings:FirebirdWriteConnectionString",
                configuration.Get("AppSettings:FirebirdConnectionString")
                );
        }

        public static string GetFirebirdReadConnectionString(this IConfiguration configuration)
        {
            return configuration.GetOrDefault("AppSettings:FirebirdReadConnectionString",
                configuration.Get("AppSettings:FirebirdConnectionString")
                );
        }
    }
}
