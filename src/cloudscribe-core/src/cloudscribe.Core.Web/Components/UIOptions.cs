// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-09-04
// Last Modified:			2015-09-04
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class UIOptions
    {
        public UIOptions()
        { }

        public int DefaultPageSize_CountryList { get; set; } = 10;
        public int DefaultPageSize_StateList { get; set; } = 10;
        public int DefaultPageSize_RoleList { get; set; } = 10;
        public int DefaultPageSize_RoleMemberList { get; set; } = 10;

    }
}
