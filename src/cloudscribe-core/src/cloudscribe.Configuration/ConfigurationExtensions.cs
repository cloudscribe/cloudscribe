// Author:					Joe Audette
// Created:					2015-06-18
// Last Modified:			2015-07-04
// 


using Microsoft.Framework.Configuration;
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

        public static string RolesThatCannotBeDeleted(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:RolesThatCannotBeDeleted", string.Empty);
        }

        public static bool DontUseEmailForLogin(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:DontUseEmailForLogin", false);
        }

        public static bool AllowDeleteChildSites(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:AllowDeleteChildSites", true);
        }


        public static int RelatedSiteId (this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:RelatedSiteId", 1);
        }

        public static string CkEditorCustomConfigPath(this IConfiguration config)
        {
            return config.GetOrDefault("CkEditor:CustomConfigPath", "~/js/cloudscribe-ckeditor-config.js");
        }

        public static int DefaultPageSize_RoleList(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:DefaultPageSize_RoleList", 10);
        }

        public static int DefaultPageSize_RoleMemberList(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:DefaultPageSize_RoleMemberList", 10);
        }

        public static int DefaultPageSize_StateList(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:DefaultPageSize_StateList", 10);
        }

        public static int DefaultPageSize_CountryList(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:DefaultPageSize_CountryList", 10);
        }

        public static int DefaultPageSize_UserList(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:DefaultPageSize_UserList", 10);
        }

        public static int DefaultPageSize_SiteList(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:DefaultPageSize_SiteList", 10);
        }

    }
}
