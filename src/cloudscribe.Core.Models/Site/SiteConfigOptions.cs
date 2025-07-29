﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-07
// Last Modified:			2017-10-19
// 

namespace cloudscribe.Core.Models
{
    public class SiteConfigOptions
    {
       
        public string DefaultTheme { get; set; } = "";
        public string FirstSiteTheme { get; set; } = "";
        public string RolesThatCannotBeDeleted { get; set; } = string.Empty;
        public bool AllowAdminsToChangeUserPasswords { get; set; }
        public string ReservedSiteNames { get; set; } = string.Empty;
    }
}
