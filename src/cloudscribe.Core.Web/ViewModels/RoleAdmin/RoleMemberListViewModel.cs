// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-06
// Last Modified:			2015-10-12
// 

using cloudscribe.Core.Models;
using cloudscribe.Web.Pagination;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.RoleAdmin
{
    public class RoleMemberListViewModel
    {
        public RoleMemberListViewModel()
        {
            Role = new RoleViewModel();
            Members = new List<IUserInfo>();
            Paging = new PaginationSettings();
        }

        public Guid SiteId { get; set; } = Guid.Empty;
        public bool UseEmailForLogin { get; set; } = true;
        public string Heading1 { get; set; }
        public string Heading2 { get; set; }
        public RoleViewModel Role { get; set; }
        public IList<IUserInfo> Members { get; set; }
        public PaginationSettings Paging { get; set; }
        public string SearchQuery { get; set; } = string.Empty;
        

    }
}
