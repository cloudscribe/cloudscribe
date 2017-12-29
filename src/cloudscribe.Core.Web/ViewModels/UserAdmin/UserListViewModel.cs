// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-08
// Last Modified:			2017-12-29
//

using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.UserAdmin
{
    public class UserListViewModel
    {
        public UserListViewModel()
        {

            UserList = new PagedResult<IUserInfo>();
            
        }

        public Guid SiteId { get; set; } = Guid.Empty;
        public string Heading { get; set; }
        public PagedResult<IUserInfo> UserList { get; set; }
       
        public string TimeZoneId { get; set; } = "America/New_York";
        public string AlphaQuery { get; set; } = string.Empty;
        public string SearchQuery { get; set; } = string.Empty;
        public string IpQuery { get; set; } = string.Empty;
        public bool ShowAlphaPager { get; set; } = true;
        //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First
        public int SortMode { get; set; } = 1;
        public string ActionName { get; set; } = "Index";

    }
}
