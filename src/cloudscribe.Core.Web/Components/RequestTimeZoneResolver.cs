// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-23
//	Last Modified:		    2016-03-03
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using Microsoft.AspNet.Http;
using SaasKit.Multitenancy;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class RequestTimeZoneResolver : ITimeZoneResolver
    {
        public RequestTimeZoneResolver(
            IHttpContextAccessor contextAccessor,  
            ITenantResolver<SiteSettings> siteResolver,
            SiteUserManager<SiteUser> userManager
            )
        {
            this.contextAccessor = contextAccessor;
            this.siteResolver = siteResolver;
            this.userManager = userManager;
        }

        private IHttpContextAccessor contextAccessor;
        private ITenantResolver<SiteSettings> siteResolver;
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

            return await GetSiteTimeZone();
        }

        public Task<TimeZoneInfo> GetSiteTimeZone()
        {
            var tenant = contextAccessor.HttpContext.GetTenant<SiteSettings>();

            if(tenant != null)
            {
                if((tenant.TimeZoneId.Length > 0))
                return Task.FromResult(TimeZoneInfo.FindSystemTimeZoneById(tenant.TimeZoneId));
            }

            return Task.FromResult(TimeZoneInfo.Utc);
        }

    }
}
