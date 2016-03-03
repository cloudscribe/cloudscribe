// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-27
// Last Modified:			2016-03-03
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using SaasKit.Multitenancy;

using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
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
            IHttpContextAccessor contextAccessor,
           // ITenantResolver<SiteSettings> siteResolver,
            ISecurityStampValidator securityStampValidator,
            ILogger<SiteAuthCookieValidator> logger)
        {
            this.contextAccessor = contextAccessor;
            this.securityStampValidator = securityStampValidator;
           // this.siteResolver = siteResolver;
            log = logger;
        }

        private ISecurityStampValidator securityStampValidator;
        //private ITenantResolver<SiteSettings> siteResolver;
        private ILogger log;
        private IHttpContextAccessor contextAccessor;

        public Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            // 
            // TODO: uncomment this after next release of aspnet core
            // and fix the broken
            // it needs to resolve options per tenant
            //await securityStampValidator.ValidateAsync(context);
            
            var tenant = contextAccessor.HttpContext.GetTenant<SiteSettings>();

            if (tenant == null)
            {
                context.RejectPrincipal();
            }
            
            var siteGuidClaim = new Claim("SiteGuid", tenant.SiteGuid.ToString());

            if (!context.Principal.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
            {
                log.LogInformation("rejecting principal because it does not have siteguid");
                context.RejectPrincipal();
            }

            return Task.FromResult(0);
        }
    }
}
