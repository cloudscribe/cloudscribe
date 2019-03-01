// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-06
// Last Modified:			2017-12-29
// 

using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.RoleAdmin
{
    public class RoleMemberListViewModel
    {
        public RoleMemberListViewModel()
        {
            Role = new RoleViewModel();
            Members = new PagedResult<IUserInfo>();
            
        }

        public Guid SiteId { get; set; } = Guid.Empty;
        //public bool UseEmailForLogin { get; set; } = true;
        public string Heading1 { get; set; }
        public string Heading2 { get; set; }
        public RoleViewModel Role { get; set; }
        public PagedResult<IUserInfo> Members { get; set; }
        
        public string SearchQuery { get; set; } = string.Empty;
        

    }
}
