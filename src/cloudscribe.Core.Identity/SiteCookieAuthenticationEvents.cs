// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-27
// Last Modified:			2016-10-08
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class SiteCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        public SiteCookieAuthenticationEvents(
            SiteAuthCookieValidator validator) : base()
        {
            OnValidatePrincipal = validator.ValidatePrincipal;
        }
    }


    public class SiteAuthCookieValidator
    {
        public SiteAuthCookieValidator(
            //ISecurityStampValidator securityStampValidator,
            ILogger<SiteAuthCookieValidator> logger)
        {
            //this.securityStampValidator = securityStampValidator;
            this.logger = logger;
        }

        //private ISecurityStampValidator securityStampValidator;
        private ILogger logger;

        public Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            // 
            // TODO: uncomment this after next release of aspnet core
            // and fix the broken
            // it needs to resolve options per tenant
            //await securityStampValidator.ValidateAsync(context);

            var tenant = context.HttpContext.GetTenant<SiteContext>();

            if (tenant == null)
            {
                context.RejectPrincipal();
            }

            var siteGuidClaim = new Claim("SiteGuid", tenant.Id.ToString());

            if (!context.Principal.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
            {
                logger.LogInformation("rejecting principal because it does not have siteguid");
                context.RejectPrincipal();
            }

            //TODO: should we lookup the user here and reject if locked out or deleted?

            return Task.FromResult(0);
        }
    }

}
