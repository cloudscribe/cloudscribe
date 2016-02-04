// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-23
//	Last Modified:		    2016-02-04
// 

using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public interface ITimeZoneResolver
    {
        Task<TimeZoneInfo> GetUserTimeZone();
        Task<TimeZoneInfo> GetSiteTimeZone();
    }
}
