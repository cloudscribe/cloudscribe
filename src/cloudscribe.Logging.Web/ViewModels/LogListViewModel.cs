// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-21
//	Last Modified:		    2015-12-26
// 

using cloudscribe.Web.Pagination;
using System;
using System.Collections.Generic;

namespace cloudscribe.Logging.Web
{
    public class LogListViewModel
    {
        public LogListViewModel()
        {
            LogPage = new List<ILogItem>();
            Paging = new PaginationSettings();
        }

        //public string Heading { get; set; }
        public List<ILogItem> LogPage { get; set; }
        public PaginationSettings Paging { get; set; }
        public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Utc;

    }
}
