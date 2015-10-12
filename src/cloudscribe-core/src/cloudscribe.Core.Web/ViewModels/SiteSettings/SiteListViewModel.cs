// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-24
// Last Modified:			2015-10-12
//

using cloudscribe.Core.Models;
using cloudscribe.Web.Pagination;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SiteListViewModel
    {
        public SiteListViewModel()
        {
            Sites = new List<ISiteInfo>();
            Paging = new PaginationSettings();
        }

        public string Heading { get; set; }
        public List<ISiteInfo> Sites { get; set; }
        public PaginationSettings Paging { get; set; }
    }
}
