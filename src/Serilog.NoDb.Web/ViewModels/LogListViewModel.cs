// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-26
// Last Modified:			2016-06-26
// 

using cloudscribe.Web.Pagination;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serilog.NoDb.Web.ViewModels
{
    public class LogListViewModel
    {
        public LogListViewModel()
        {
            Items = new List<LogEvent>();
        }

        public List<LogEvent> Items { get; set; }
        public PaginationSettings Paging { get; set; }
        public string TimeZoneId { get; set; } = "America/New_York";
    }
}
