// Author:					Joe Audette
// Created:					2015-06-18
// Last Modified:			2015-06-18
// 


using Microsoft.Framework.ConfigurationModel;
using System;

namespace cloudscribe.Configuration
{
    public static class ConfigurationExtensions
    {

        public static string GetOrDefault(this IConfiguration config, string key, string defaultIfNotFound)
        {
            string result = config.Get(key);

            if(string.IsNullOrEmpty(result)) { return defaultIfNotFound; }

            return result;
        }

        public static int GetOrDefault(this IConfiguration config, string key, int defaultIfNotFound)
        {
            int? result = config.Get<int>(key);

            if (!result.HasValue) { return defaultIfNotFound; }

            return result.Value;
        }

        public static Guid GetOrDefault(this IConfiguration config, string key, Guid defaultIfNotFound)
        {
            Guid? result = config.Get<Guid>(key);

            if (!result.HasValue) { return defaultIfNotFound; }

            return result.Value;
        }
    }
}
