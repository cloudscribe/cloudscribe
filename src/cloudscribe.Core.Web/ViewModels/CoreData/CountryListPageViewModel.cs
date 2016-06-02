// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2016-06-02
//

using cloudscribe.Core.Models.Geography;
using cloudscribe.Web.Pagination;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.CoreData
{
    public class CountryListPageViewModel
    {
        public CountryListPageViewModel()
        {
            Paging = new PaginationSettings();

        }

        public List<IGeoCountry> Countries { get; set; }
        public PaginationSettings Paging { get; set; }

    }
}
