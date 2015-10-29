// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-23
//	Last Modified:		    2015-08-23
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class RequestTimeZoneResolver : ITimeZoneResolver
    {
        public RequestTimeZoneResolver(
            IHttpContextAccessor contextAccessor,
            ISiteResolver siteResolver,
            SiteUserManager<SiteUser> userManager
            )
        {
            this.contextAccessor = contextAccessor;
            this.siteResolver = siteResolver;
            this.userManager = userManager;
        }

        private IHttpContextAccessor contextAccessor;
        private ISiteResolver siteResolver;
        private SiteUserManager<SiteUser> userManager;

        public async Task<TimeZoneInfo> GetUserTimeZone()
        {
            HttpContext context = contextAccessor.HttpContext;
            if(context.User.Identity.IsAuthenticated)
            {
                SiteUser user = await userManager.FindByIdAsync(context.User.GetUserId());
                if((user != null)&&(user.TimeZoneId.Length > 0))
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);
                }
            }

            return GetSiteTimeZone();
        }

        public TimeZoneInfo GetSiteTimeZone()
        {
            ISiteSettings site = siteResolver.Resolve();
            if((site != null)&&(site.TimeZoneId.Length > 0))
            {
                return TimeZoneInfo.FindSystemTimeZoneById(site.TimeZoneId);
            }

            return TimeZoneInfo.Utc;
        }

    }
}
