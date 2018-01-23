// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-27
// Last Modified:			2017-07-26
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class SiteAuthCookieValidator
    {
        public static async Task ValidatePrincipalAsync(CookieValidatePrincipalContext context)
        {
            var tenant = context.HttpContext.GetTenant<SiteContext>();

            if (tenant == null)
            {
                context.RejectPrincipal();
            }

            var siteGuidClaim = new Claim("SiteGuid", tenant.Id.ToString());

            if (!context.Principal.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<SiteAuthCookieValidator>>();
                logger.LogInformation("rejecting principal because it does not have siteguid");
                context.RejectPrincipal();
            }
            else
            {
                await SecurityStampValidator.ValidatePrincipalAsync(context);
            }
            
        }

    }
}
