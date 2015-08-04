// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-04
// Last Modified:			2015-08-04
// 


using cloudscribe.Configuration;
using Microsoft.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public static class ConfigurationExtensions
    {

        public static string RolesThatCannotBeDeleted(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:RolesThatCannotBeDeleted", string.Empty);
        }

        public static bool AllowDeleteChildSites(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:AllowDeleteChildSites", true);
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
            return config.GetOrDefault("AppSettings:SetupInstallScriptPathFormat", "/config/applications/{0}/install/{1}");
        }

        public static string SetupUpgradeScriptPathFormat(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:SetupUpgradeScriptPathFormat", "/config/applications/{0}/upgrade/{1}");
        }

        public static string SetupHeaderConfigPath(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:SetupHeaderConfigPath", "/config/setup/SetupHeader.config");
        }

        public static string SetupHeaderConfigPathRtl(this IConfiguration config)
        {
            return config.GetOrDefault("AppSettings:SetupHeaderConfigPathRtl", "/config/setup/SetupHeader-rtl.config");
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
