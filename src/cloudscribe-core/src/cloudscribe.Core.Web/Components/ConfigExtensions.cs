// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-04
// Last Modified:			2015-08-05
// 


using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.Components
{
    public static class ConfigExtensions
    {

        public static string RolesThatCannotBeDeleted(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:RolesThatCannotBeDeleted", string.Empty);
        }

        public static bool AllowDeleteChildSites(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:AllowDeleteChildSites", true);
        }

        public static int DefaultPageSize_RoleList(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:DefaultPageSize_RoleList", 10);
        }

        public static int DefaultPageSize_RoleMemberList(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:DefaultPageSize_RoleMemberList", 10);
        }

        public static int DefaultPageSize_StateList(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:DefaultPageSize_StateList", 10);
        }

        public static int DefaultPageSize_CountryList(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:DefaultPageSize_CountryList", 10);
        }

        public static int DefaultPageSize_UserList(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:DefaultPageSize_UserList", 10);
        }

        public static int DefaultPageSize_SiteList(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:DefaultPageSize_SiteList", 10);
        }

        public static string RecaptchaSiteKey(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:RecaptchaSiteKey", string.Empty);
        }

        public static string RecaptchaSecretKey(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:RecaptchaSecretKey", string.Empty);
        }

        public static string DefaultInitialSkin(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:DefaultInitialSkin", "bootstrap");
        }

        public static bool SslIsRequiredByWebServer(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:SslIsRequiredByWebServer", false);
        }

        public static bool DisableSetup(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:DisableSetup", false);
        }

        public static string SetupInstallScriptPathFormat(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:SetupInstallScriptPathFormat", "/config/applications/{0}/install/{1}");
        }

        public static string SetupUpgradeScriptPathFormat(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:SetupUpgradeScriptPathFormat", "/config/applications/{0}/upgrade/{1}");
        }

        public static string SetupHeaderConfigPath(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:SetupHeaderConfigPath", "/config/setup/SetupHeader.config");
        }

        public static string SetupHeaderConfigPathRtl(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:SetupHeaderConfigPathRtl", "/config/setup/SetupHeader-rtl.config");
        }

        public static bool TryToCreateMsSqlDatabase(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:TryToCreateMsSqlDatabase", false);
        }

        public static bool ShowConnectionErrorOnSetup(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:ShowConnectionErrorOnSetup", false);
        }

        public static bool SetupTryAnywayIfFailedAlterSchemaTest(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:SetupTryAnywayIfFailedAlterSchemaTest", false);
        }

        public static string CkEditorCustomConfigPath(this ConfigHelper config)
        {
            return config.GetOrDefault("AppSettings:CkEditorCustomConfigPath", "~/js/app/cloudscribe-ckeditor-config.js");
        }


    }
}
