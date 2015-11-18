// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-27
// Last Modified:			2015-11-18
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class MultiTenantAuthCookieValidator
    {
        public MultiTenantAuthCookieValidator(
            ISiteResolver siteResolver,
            ILogger<MultiTenantAuthCookieValidator> logger
            )
        {
            this.siteResolver = siteResolver;
            log = logger;
        }

        private ISiteResolver siteResolver;
        private ILogger log;

        public Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            ISiteSettings site = siteResolver.Resolve();
            if (site == null) return Task.FromResult(0);

            Claim siteGuidClaim = new Claim("SiteGuid", site.SiteGuid.ToString());

            if (!context.Principal.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
            {
                log.LogInformation("rejecting principal because it does not have siteguid");
                context.RejectPrincipal();
            }

            

            return Task.FromResult(0);
        }
    }
}
