// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-27
// Last Modified:			2017-10-09
// 


using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class SiteUserClaimsPrincipalFactory<TUser, TRole> : UserClaimsPrincipalFactory<TUser, TRole>
        where TUser : SiteUser
        where TRole : SiteRole
    {
        public SiteUserClaimsPrincipalFactory(
            ISiteQueries siteQueries,
            SiteUserManager<TUser> userManager,
            SiteRoleManager<TRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            IEnumerable<ICustomClaimProvider> customClaimProviders // an extension point if you need to be able to add custom claims you can inject one or more of these
            ) 
            : base(userManager, roleManager, optionsAccessor)
        {
            if (siteQueries == null)
            {
                throw new ArgumentNullException(nameof(siteQueries));
            }

            queries = siteQueries;
            options = optionsAccessor.Value;
            this.customClaimProviders = customClaimProviders;
        }
        
        private ISiteQueries queries;
        private IdentityOptions options;
        private IEnumerable<ICustomClaimProvider> customClaimProviders;

        public override async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            // base class takes care of all the default stuff like roles name id etc
            var principal = await base.CreateAsync(user);
            
            if (principal.Identity is ClaimsIdentity)
            {
                var identity = (ClaimsIdentity)principal.Identity;

                foreach (var provider in customClaimProviders)
                {
                    await provider.AddClaims(user, identity);
                }
                
                var displayNameClaim = new Claim("DisplayName", user.DisplayName);
                if (!identity.HasClaim(displayNameClaim.Type, displayNameClaim.Value))
                {
                    identity.AddClaim(displayNameClaim);
                }

                var emailClaim = new Claim(ClaimTypes.Email, user.Email);
                if (!identity.HasClaim(emailClaim.Type, emailClaim.Value))
                {
                    identity.AddClaim(emailClaim);
                }

                var site = await queries.Fetch(user.SiteId, CancellationToken.None);

                if (site != null)
                {
                    var siteGuidClaim = new Claim("SiteGuid", site.Id.ToString());

                    if (!identity.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
                    {
                        identity.AddClaim(siteGuidClaim);
                    }
                }                

                if (principal.IsInRole("Administrators"))
                {
                    if (site != null && site.IsServerAdminSite)
                    {
                        Claim serverAdminRoleClaim = new Claim(Options.ClaimsIdentity.RoleClaimType, "ServerAdmins");
                        identity.AddClaim(serverAdminRoleClaim);
                    }
                }
            }
            
            return principal;
        }
    }
}
