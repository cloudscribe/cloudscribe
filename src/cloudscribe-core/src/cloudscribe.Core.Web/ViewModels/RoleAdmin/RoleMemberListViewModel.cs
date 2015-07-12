// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-06
// Last Modified:			2015-07-09
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Navigation;
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

        public string Heading { get; set; }
        public RoleViewModel Role { get; set; }
        public IList<IUserInfo> Members { get; set; }
        public PaginationSettings Paging { get; set; }

        private string searchQuery = string.Empty;
        public string SearchQuery
        {
            get { return searchQuery; }
            set { searchQuery = value; }
        }

    }
}
