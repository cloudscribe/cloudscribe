// Author:					Joe Audette
// Created:					2015-06-18
// Last Modified:			2015-06-20
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
            string result = config.Get(key);

            if (string.IsNullOrEmpty(result)) { return defaultIfNotFound; }

            return Convert.ToInt32(result);
        }

        public static bool GetOrDefault(this IConfiguration config, string key, bool defaultIfNotFound)
        {
            string result = config.Get(key);

            if (string.IsNullOrEmpty(result)) { return defaultIfNotFound; }

            return Convert.ToBoolean(result);
        }

        public static Guid GetOrDefault(this IConfiguration config, string key, Guid defaultIfNotFound)
        {
            string result = config.Get(key);

            if (string.IsNullOrEmpty(result)) { return defaultIfNotFound; }
            if(result.Length != 36) { return defaultIfNotFound; }

            return new Guid(result);
        }

        public static bool UseRelatedSiteMode(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:UseRelatedSiteMode", false);
        }

        public static bool UseFoldersInsteadOfHostnamesForMultipleSites(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:UseFoldersInsteadOfHostnamesForMultipleSites", true);
        }
        

        public static int RelatedSiteId (this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:RelatedSiteId", 1);
        }

        public static string CkEditorCustomConfigPath(this IConfiguration config)
        {
            return config.GetOrDefault("CkEditor:CustomConfigPath", "~/js/cloudscribe-ckeditor-config.js");
        }


    }
}
