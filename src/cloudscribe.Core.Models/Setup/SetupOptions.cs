// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-09-04
// Last Modified:			2016-01-03
// 

namespace cloudscribe.Core.Models
{
    public class SetupOptions
    {
        public SetupOptions()
        { }

        public string ConfigBasePath { get; set; } = "/cloudscribe_config";
        public string InstallScriptPathFormat { get; set; } = "/cloudscribe_config/applications/{0}/install/{1}";
        public string UpgradeScriptPathFormat { get; set; } = "/cloudscribe_config/applications/{0}/upgrade/{1}";
        public bool TryToCreateMsSqlDatabase { get; set; } = false;
        public bool SslIsRequiredByWebServer { get; set; } = false;
        public string DefaultLayout { get; set; } = "Default_Layout.cshtml";
        public string RolesThatCannotBeDeleted { get; set; } = string.Empty;
        public bool DisableSetup { get; set; } = false;
        public bool ShowErrors { get; set; } = true;
        public bool ShowConnectionError { get; set; } = false;
        public bool TryAnywayIfFailedAlterSchemaTest { get; set; } = false;
        public string SetupHeaderConfigPath { get; set; } = "/cloudscribe_config/setup/SetupHeader.config";
        public string SetupHeaderConfigPathRtl { get; set; } = "/cloudscribe_config/setup/SetupHeader-rtl.config";
    }
}
