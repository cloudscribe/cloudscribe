// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-09-04
// Last Modified:			2015-10-09
// 

namespace cloudscribe.Core.Web.Components
{
    public class SetupOptions
    {
        public SetupOptions()
        { }

        public string InstallScriptPathFormat { get; set; } = "/config/applications/{0}/install/{1}";
        public string UpgradeScriptPathFormat { get; set; } = "/config/applications/{0}/upgrade/{1}";
        public bool TryToCreateMsSqlDatabase { get; set; } = false;
        public bool SslIsRequiredByWebServer { get; set; } = false;
        public string DefaultLayout { get; set; } = "Default_Layout.cshtml";
        public string RolesThatCannotBeDeleted { get; set; } = string.Empty;
        public bool DisableSetup { get; set; } = false;
        public bool ShowErrors { get; set; } = true;
        public bool ShowConnectionError { get; set; } = false;
        public bool TryAnywayIfFailedAlterSchemaTest { get; set; } = false;
        public string SetupHeaderConfigPath { get; set; } = "/config/setup/SetupHeader.config";
        public string SetupHeaderConfigPathRtl { get; set; } = "/config/setup/SetupHeader-rtl.config";
    }
}
