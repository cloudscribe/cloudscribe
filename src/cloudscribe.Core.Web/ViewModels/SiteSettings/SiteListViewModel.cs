// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-24
// Last Modified:			2017-12-29
//

using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SiteListViewModel
    {
        public SiteListViewModel()
        {
            Sites = new PagedResult<ISiteInfo>();
            
        }

        public PagedResult<ISiteInfo> Sites { get; set; }
        
    }
}
