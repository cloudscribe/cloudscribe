// Author:					Joe Audette
// Created:					2014-08-26
// Last Modified:			2015-04-20
// 


using System;
using System.Configuration;
using System.Globalization;

namespace cloudscribe.Configuration
{
    public static class AppSettings
    {
        public static bool GetBool(string key, bool defaultIfNotFound)
        {
            if (string.IsNullOrEmpty(key)) { return defaultIfNotFound; }

            if (ConfigurationManager.AppSettings[key] == null) { return defaultIfNotFound; }

            if (string.Equals(ConfigurationManager.AppSettings[key], "true", StringComparison.InvariantCultureIgnoreCase))
            { return true; }

            if (string.Equals(ConfigurationManager.AppSettings[key], "false", StringComparison.InvariantCultureIgnoreCase))
            { return false; }


            return defaultIfNotFound;

        }

        public static string GetString(string key, string defaultIfNotFound)
        {
            if (string.IsNullOrEmpty(key)) { return defaultIfNotFound; }
            if (ConfigurationManager.AppSettings[key] == null) return defaultIfNotFound;
            return ConfigurationManager.AppSettings[key];
        }

        public static int GetInt(string key, int defaultValue)
        {
            if (string.IsNullOrEmpty(key)) { return defaultValue; }
            int setting;
            return int.TryParse(ConfigurationManager.AppSettings[key], out setting) ? setting : defaultValue;
        }

        public static long GetLong(string key, long defaultValue)
        {
            if (string.IsNullOrEmpty(key)) { return defaultValue; }
            long setting;
            return long.TryParse(ConfigurationManager.AppSettings[key], out setting) ? setting : defaultValue;
        }

        public static string DbPlatform
        {
            get { return GetString("DbPlatform", "MSSQL"); }
        }

        public static string MSSQLOwnerPrefix
        {
            get { return GetString("MSSQLOwnerPrefix", "[dbo]."); }
        }

        public static string MSSQLConnectionString
        {
            get { return GetString("MSSQLConnectionString", string.Empty); }
        }

        public static string MSSQLWriteConnectionString
        {
            get { return GetString("MSSQLWriteConnectionString", string.Empty); }
        }

        public static bool CacheMSSQLParameters
        {
            get { return GetBool("CacheMSSQLParameters", true); }
        }

        public static bool TryToCreateMsSqlDatabase
        {
            get { return GetBool("TryToCreateMsSqlDatabase", false); }
        }

        public static bool UseConnectionStringSection
        {
            get { return GetBool("UseConnectionStringSection", false); }
        }


        public static string MySqlConnectionString
        {
            get { return GetString("MySqlConnectionString", string.Empty); }
        }

        public static string MySqlWriteConnectionString
        {
            get { return GetString("MySqlWriteConnectionString", string.Empty); }
        }

        public static string PostgreSQLConnectionString
        {
            get { return GetString("PostgreSQLConnectionString", string.Empty); }
        }

        public static string PostgreSQLWriteConnectionString
        {
            get { return GetString("PostgreSQLWriteConnectionString", string.Empty); }
        }

        public static string FirebirdConnectionString
        {
            get { return GetString("FirebirdConnectionString", string.Empty); }
        }

        public static string FirebirdWriteConnectionString
        {
            get { return GetString("FirebirdWriteConnectionString", string.Empty); }
        }


        public static string SqlCeApp_Data_FileName
        {
            get { return GetString("SqlCeApp_Data_FileName", "cloudscribe.sdf"); }
        }

        public static string SqliteApp_Data_FileName
        {
            get { return GetString("SqliteApp_Data_FileName", "cloudscribe.sqlite"); }
        }

        public static string SqlCeConnectionString
        {
            get { return GetString("SqlCeConnectionString", string.Empty); }
        }

        public static string SqliteConnectionString
        {
            get { return GetString("SqliteConnectionString", string.Empty); }
        }

        public static bool SetupTryAnywayIfFailedAlterSchemaTest
        {
            get { return GetBool("SetupTryAnywayIfFailedAlterSchemaTest", false); }
        }

        public static bool Cache_Disabled
        {
            get { return GetBool("Cache_Disabled", false); } 
        }

        public static int CacheDurationInSeconds_SiteSettings
        {
            get { return GetInt("CacheDurationInSeconds_SiteSettings", 3600); } // 1 hour
        }

        public static int CacheDurationInSeconds_SiteUser
        {
            get { return GetInt("CacheDurationInSeconds_SiteUser", 30); } 
        }

        public static int CacheDurationInSeconds_SiteUserLogins
        {
            get { return GetInt("CacheDurationInSeconds_SiteUserLogins", 30); }
        }


        public static bool CacheTimeZoneList
        {
            get { return GetBool("CacheTimeZoneList", true); }
        }

        public static int CacheDurationInSeconds_TimeZoneList
        {
            get { return GetInt("CacheDurationInSeconds_TimeZoneList", 3600); } // 1 hour
        }

        public static int DefaultPageSize_SiteList
        {
            get { return GetInt("DefaultPageSize_SiteList", 10); }
        }

        public static int DefaultPageSize_UserList
        {
            get { return GetInt("DefaultPageSize_UserList", 10); }
        }

        public static int DefaultPageSize_CountryList
        {
            get { return GetInt("DefaultPageSize_CountryList", 10); }
        }

        public static int DefaultPageSize_StateList
        {
            get { return GetInt("DefaultPageSize_StateList", 10); }
        }

        public static int DefaultPageSize_RoleMemberList
        {
            get { return GetInt("DefaultPageSize_RoleMemberList", 10); }
        }

        public static string RouteConfigPath
        {
            get { return GetString("RouteConfigPath", "~/Config/RouteRegistrars/"); }
        }


        public static bool DisableSetup
        {
            get { return GetBool("DisableSetup", false); }
        }

        public static bool TryEnsureCustomMachineKeyOnSetup
        {
            get { return GetBool("TryEnsureCustomMachineKeyOnSetup", false); }
        }


        public static bool AllowDeleteChildSites
        {
            get { return GetBool("AllowDeleteChildSites", true); }
        }
        

        /// <summary>
        /// a single role name or a semi colon separated list of role names
        /// to be added when a new user account is created
        /// default Authenticated Users is the role for all loged in users
        /// to differentiate vs the pseudo Role "All Users" which allows anonymous access to content
        /// we recommend do not remove this default role, if you want to add more put a semi colon 
        /// between each one
        /// </summary>
        public static string DefaultRolesForNewUsers
        {
            get { return GetString("DefaultRolesForNewUsers", "Authenticated Users"); }
        }


        public static bool UseFoldersInsteadOfHostnamesForMultipleSites
        {
            get { return GetBool("UseFoldersInsteadOfHostnamesForMultipleSites", true); }
        }

        public static bool RegisterDefaultRoutesForFolderSites
        {
            get { return GetBool("RegisterDefaultRoutesForFolderSites", true); }
        }

        public static bool UseUrlRewriteForFolderSites
        {
            get { return GetBool("UseUrlRewriteForFolderSites", false); }
        }

        public static string RolesThatCannotBeDeleted
        {
            get { return GetString("RolesThatCannotBeDeleted", string.Empty); }
        }

        public static bool UseRelatedSiteMode
        {
            get {
                //if (UseFoldersInsteadOfHostnamesForMultipleSites) { return true; }

                return GetBool("UseRelatedSiteMode", false); 
            }
        }

        public static int RelatedSiteId
        {
            get { return GetInt("RelatedSiteId", 1); }
        }

        /// <summary>
        /// TODO: if true then users must use loginname to login instead of email
        /// </summary>
        public static bool DontUseEmailForLogin
        {
            get { return GetBool("DontUseEmailForLogin", false); }
        }

        public static bool AutoGenerateAndHideUserNamesWhenUsingEmailForLogin
        {
            get { return GetBool("AutoGenerateAndHideUserNamesWhenUsingEmailForLogin", false); }
        }

        public static bool UseSameContentFolderForRelatedSiteMode
        {
            get { return GetBool("UseSameContentFolderForRelatedSiteMode", false); }
        }

        public static string PasswordGeneratorChars
        {
            get { return GetString("PasswordGeneratorChars", "abcdefgijkmnopqrstwxyzABCDEFGHJKLMNPQRSTWXYZ23456789*$"); }
        }

        public static int PasswordMaxLength
        {
            get { return GetInt("PasswordMaxLength", 100); }
        }

        public static int PasswordMinLength
        {
            get { return GetInt("PasswordMinLength", 7); }
        }

        public static string CacheProviderType
        {
            get { return GetString("Cache:ProviderType", "mojoPortal.Web.Caching.MemoryCacheAdapter, mojoPortal.Web"); }
        }

        public static string DistributedCacheName
        {
            get { return GetString("Cache:DistributedCacheName", "MyCache"); }
        }

        public static string DistributedCacheServers
        {
            get { return GetString("Cache:DistributedCacheServers", "localhost:22223"); }
        }

        public static string AzureCacheSecurityMode
        {
            get { return GetString("Cache:AzureCacheSecurityMode", "Message"); }
        }

        public static string AzureCacheAuthorizationInfo
        {
            get { return GetString("Cache:AzureCacheAuthorizationInfo", string.Empty); }
        }


        public static bool AzureCacheUseSsl
        {
            get { return GetBool("Cache:AzureCacheUseSsl", true); }
        }

        public static bool UseCacheDependencyFiles
        {
            get { return GetBool("UseCacheDependencyFiles", false); }
        }

        public static bool LogCacheActivity
        {
            get { return GetBool("LogCacheActivity", false); }
        }

        public static bool DebugLogEnabled
        {
            get { return GetBool("DebugLogEnabled", false); }
        }

        public static bool UserStoreDebugEnabled
        {
            get { return GetBool("UserStoreDebugEnabled", false); }
        }

        public static bool LogFullUrls
        {
            get { return GetBool("LogFullUrls", false); }
        }

        /// <summary>
        /// to support proxy servers where the client ip may be passed in a custom server variable
        /// </summary>
        public static string ClientIpServerVariable
        {
            get { return GetString("ClientIpServerVariable", string.Empty); }
        }

        /// <summary>
        /// to suport proxy servers which may use non standard variables
        /// http://www.mojoportal.com/Forums/Thread.aspx?thread=7424&mid=34&pageid=5&ItemID=5&pagenumber=1#post30680
        /// </summary>
        public static string RemoteHostServerVariable
        {
            get { return GetString("RemoteHostServerVariable", "REMOTE_HOST"); }
        }

        /// <summary>
        /// if IIS or apache is set to require ssl for all pages then set thsi to true.
        /// </summary>
        public static bool SslIsRequiredByWebServer
        {
            get { return GetBool("SSLIsRequiredByWebServer", false); }
        }

        /// <summary>
        /// enumerating items in memory cache is resource intensive and blocking
        /// should be avoided in production environments except for brief troubleshooting
        /// </summary>
        public static bool Glimpse_AllowMemoryCacheEnumeration
        {
            get { return GetBool("Glimpse_AllowMemoryCacheEnumeration", false); }
        }

        public static bool MvcSiteMapProvider_UseExternalDIContainer
        {
            get { return GetBool("MvcSiteMapProvider_UseExternalDIContainer", false); }
        }

        public static string MvcSiteMapProvider_SiteMapFileName
        {
            get { return GetString("MvcSiteMapProvider_SiteMapFileName", "~/site.sitemap"); }
        }

        public static string SetupHeaderConfigPath
        {
            get { return GetString("SetupHeaderConfigPath", "~/Config/Setup/SetupHeader.config"); }
        }

        public static string SetupHeaderConfigPathRtl
        {
            get { return GetString("SetupHeaderConfigPathRtl", "~/Config/Setup/SetupHeader-rtl.config"); }
        }

        public static string DefaultInitialSkin
        {
            get { return GetString("DefaultInitialSkin", "bootstrap"); }
        }

        public static string MobileDetectionExcludeUrlsCsv
        {
            get { return GetString("MobileDetectionExcludeUrlsCsv", string.Empty); }
        }

        //http://googlewebmastercentral.blogspot.com/2012/11/giving-tablet-users-full-sized-web.html
        //https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11092~1#post46239
        // Android phones can be differentiated by Android; Mobile;

        public static string MobilePhoneUserAgents
        {
            get { return GetString("MobilePhoneUserAgents", "iphone,ipod,iemobile,android;blackberry"); }
        }

        public static string SetupInstallScriptPathFormat
        {
            get { return GetString("SetupInstallScriptPathFormat", "~/Config/applications/{0}/install/{1}/"); }
        }

        public static string SetupUpgradeScriptPathFormat
        {
            get { return GetString("SetupInstallScriptPathFormat", "~/Config/applications/{0}/upgrade/{1}/"); }
        }


        public static bool EnableBundleOptimizations
        {
            get { return GetBool("EnableBundleOptimizations", false); }
        }

        //  ~/Config/applications/{0}/upgrade/{1}/

    }
}
