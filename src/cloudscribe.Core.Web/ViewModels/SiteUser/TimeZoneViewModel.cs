// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2016-06-06
// Last Modified:		    2016-06-06
// 

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class TimeZoneViewModel
    {
        public TimeZoneViewModel()
        {
            allTimeZones = new List<SelectListItem>();
        }

        public string TimeZoneId { get; set; }

        private IEnumerable<SelectListItem> allTimeZones = null;
        public IEnumerable<SelectListItem> AllTimeZones
        {
            get { return allTimeZones; }
            set { allTimeZones = value; }
        }
    }
}
