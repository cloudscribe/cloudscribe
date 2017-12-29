// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-16
// Last Modified:			2017-12-28
//

using cloudscribe.Core.Models.Geography;
using cloudscribe.Pagination.Models;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.CoreData
{
    public class StateListPageViewModel
    {
        public StateListPageViewModel()
        {
            Country = new GeoCountryViewModel();
            States = new PagedResult<IGeoZone>();
            //Paging = new PaginationSettings();
        }
        
        public GeoCountryViewModel Country { get; set; }
        public PagedResult<IGeoZone> States { get; set; }
        //public PaginationSettings Paging { get; set; }  
        public int ReturnPageNumber { get; set; } = 1;
        public int CountryListReturnPageNumber { get; set; } = 1;
       
    }
}
