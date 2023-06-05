// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-04
// Last Modified:			2017-12-29
//

using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cloudscribe.Core.Web.ViewModels.RoleAdmin
{
    public class AddUserToRoleListViewModel : RoleListViewModel
    {
        public AddUserToRoleListViewModel()
        {
            SiteRoles = new PagedResult<ISiteRole>();
            UserRoles = new List<string>();
        }

        public IList<string> UserRoles { get; set; } 

        public Guid UserId { get; set; }

        public IList<string> SelectedRoles { get; set; }

        public string SelectedCheckboxesCSV { get; set; }
    }
}
