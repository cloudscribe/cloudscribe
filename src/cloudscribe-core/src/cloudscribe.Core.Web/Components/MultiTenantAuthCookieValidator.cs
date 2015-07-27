// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-27
// Last Modified:			2015-07-27
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class MultiTenantAuthCookieValidator
    {
        public MultiTenantAuthCookieValidator(ISiteResolver siteResolver)
        {
            this.siteResolver = siteResolver;
        }

        private ISiteResolver siteResolver;

        public Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            ISiteSettings site = siteResolver.Resolve();
            if (site == null) return Task.FromResult(0);

            Claim siteGuidClaim = new Claim("SiteGuid", site.SiteGuid.ToString());

            if (!context.Principal.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
            {
                context.RejectPrincipal();
            }

            return Task.FromResult(0);
        }
    }
}
