// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-27
// Last Modified:			2015-06-27
// 


using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Framework.OptionsModel;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.AspNet.Identity
{
    public class SiteUserClaimsPrincipalFactory<TUser, TRole> : UserClaimsPrincipalFactory<TUser, TRole>
        where TUser : SiteUser
        where TRole : SiteRole
    {
        public SiteUserClaimsPrincipalFactory(
            ISiteRepository siteRepository,
            UserManager<TUser> userManager,
            RoleManager<TRole> roleManager, 
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
            if (siteRepository == null) { throw new ArgumentNullException(nameof(siteRepository)); }

            siteRepo = siteRepository;
        }

        private ISiteRepository siteRepo;

        public override async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            ClaimsPrincipal principal = await base.CreateAsync(user);

            if (principal.Identity is ClaimsIdentity)
            {
                ClaimsIdentity identity = (ClaimsIdentity)principal.Identity;

                Claim displayNameClaim = new Claim("DisplayName", user.DisplayName);
                if (!identity.HasClaim(displayNameClaim.Type, displayNameClaim.Value))
                {
                    identity.AddClaim(displayNameClaim);
                }

                if (principal.IsInRole("Admins"))
                {
                    ISiteSettings site = await siteRepo.Fetch(user.SiteId);
                    if (site != null && site.IsServerAdminSite)
                    {
                        // if the user is an admin of a server admin site
                        // add ServerAdmins role
                        Claim serverAdminRoleClaim = new Claim(ClaimTypes.Role, "ServerAdmins");
                        identity.AddClaim(serverAdminRoleClaim);

                    }
                }

            }
            
            return principal;

        }
    }
}
