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

namespace cloudscribe.Core.Web.ViewModels.RoleAdmin
{
    public class RoleListViewModel
    {
        public RoleListViewModel()
        {
            SiteRoles = new PagedResult<ISiteRole>();
            
        }

        public Guid SiteId { get; set; } = Guid.Empty;
        public string Heading { get; set; }

        public string SearchQuery { get; set; }
        public PagedResult<ISiteRole> SiteRoles { get; set; }
       

    }
}
