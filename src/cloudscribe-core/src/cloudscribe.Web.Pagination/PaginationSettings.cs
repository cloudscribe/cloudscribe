// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2015-07-15
//

namespace cloudscribe.Web.Pagination
{
    
    public class PaginationSettings
    {
        public int TotalItems { get; set; } = 0;
       
        public int ItemsPerPage { get; set; } = 10;
        
        public int CurrentPage { get; set; } = 1;
        
        public int MaxPagerItems { get; set; } = 10;

        public bool ShowFirstLast { get; set; } = false;

    }

}
