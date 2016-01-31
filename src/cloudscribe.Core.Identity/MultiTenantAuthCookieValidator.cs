// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-27
// Last Modified:			2015-11-18
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity;
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
            ISecurityStampValidator securityStampValidator,
            ILogger<MultiTenantAuthCookieValidator> logger
            )
        {
            this.securityStampValidator = securityStampValidator;
            this.siteResolver = siteResolver;
            log = logger;
        }

        private ISecurityStampValidator securityStampValidator;
        private ISiteResolver siteResolver;
        private ILogger log;

        public async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            // TODO: uncomment this after next release of aspnet core
            // and fix the broken
            // it needs to resolve options per tenant
            //await securityStampValidator.ValidateAsync(context);
            
            ISiteSettings site = siteResolver.Resolve();
            if (site == null)
            {
                context.RejectPrincipal();
            }

            Claim siteGuidClaim = new Claim("SiteGuid", site.SiteGuid.ToString());

            if (!context.Principal.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
            {
                log.LogInformation("rejecting principal because it does not have siteguid");
                context.RejectPrincipal();
            }
            
           // return Task.FromResult(0);
        }
    }
}
