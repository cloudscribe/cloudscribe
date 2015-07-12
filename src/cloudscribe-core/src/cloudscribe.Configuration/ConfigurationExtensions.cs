// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
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

        public static bool UserStoreDebugEnabled(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:UserStoreDebugEnabled", false);
        }

        public static string DefaultRolesForNewUsers(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:DefaultRolesForNewUsers", "Authenticated Users");
        }

        public static string RecaptchaSiteKey(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:RecaptchaSiteKey", string.Empty);
        }

        public static string RecaptchaSecretKey(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:RecaptchaSecretKey", string.Empty);
        }

        public static string DefaultInitialSkin(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:DefaultInitialSkin", "bootstrap");
        }

        public static bool SslIsRequiredByWebServer(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:SslIsRequiredByWebServer", false);
        }

        public static bool DisableSetup(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:DisableSetup", false);
        }

        public static string SetupInstallScriptPathFormat(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:SetupInstallScriptPathFormat", "~/Config/applications/{0}/install/{1}/");
        }

        public static string SetupUpgradeScriptPathFormat(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:SetupUpgradeScriptPathFormat", "~/Config/applications/{0}/upgrade/{1}/");
        }

        public static string SetupHeaderConfigPath(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:SetupHeaderConfigPath", "~/Config/Setup/SetupHeader.config");
        }

        public static string SetupHeaderConfigPathRtl(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:SetupHeaderConfigPathRtl", "~/Config/Setup/SetupHeader-rtl.config");
        }

        public static bool TryToCreateMsSqlDatabase(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:TryToCreateMsSqlDatabase", false);
        }

        public static bool ShowConnectionErrorOnSetup(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:ShowConnectionErrorOnSetup", false);
        }

        public static bool SetupTryAnywayIfFailedAlterSchemaTest(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:SetupTryAnywayIfFailedAlterSchemaTest", false);
        }

    }
}
