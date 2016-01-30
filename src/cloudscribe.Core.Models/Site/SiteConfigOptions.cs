// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-07
// Last Modified:			2016-01-28
// 

namespace cloudscribe.Core.Models
{
    public class SiteConfigOptions
    {
       
        public string DefaultLayout { get; set; } = "Default_Layout.cshtml";
        public string RolesThatCannotBeDeleted { get; set; } = string.Empty;

    }
}
