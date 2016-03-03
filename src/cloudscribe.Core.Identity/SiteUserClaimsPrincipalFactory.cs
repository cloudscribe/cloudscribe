// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-27
// Last Modified:			2016-03-03
// 


using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.OptionsModel;
using System;
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
            ISiteRepository siteRepository,
            SiteUserManager<TUser> userManager,
            SiteRoleManager<TRole> roleManager, 
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
            if (siteRepository == null) { throw new ArgumentNullException(nameof(siteRepository)); }

            siteRepo = siteRepository;
            options = optionsAccessor.Value;
        }

        private ISiteRepository siteRepo;
        private IdentityOptions options;

        public override async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            // this one was using IdentityOptions.ApplicationCookieAuthenticationScheme
            //ClaimsPrincipal principal = await base.CreateAsync(user);

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var userId = await UserManager.GetUserIdAsync(user);
            var userName = await UserManager.GetUserNameAsync(user);

            //var id = new ClaimsIdentity(
            //    tenantResolver.ResolveAuthScheme(AuthenticationScheme.Application),
            //    Options.ClaimsIdentity.UserNameClaimType,
            //    Options.ClaimsIdentity.RoleClaimType
            //    );

            var id = new ClaimsIdentity(
                options.Cookies.ApplicationCookie.AuthenticationScheme,
                Options.ClaimsIdentity.UserNameClaimType,
                Options.ClaimsIdentity.RoleClaimType
                );

            id.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, userName));

            if (UserManager.SupportsUserSecurityStamp)
            {
                id.AddClaim(new Claim(Options.ClaimsIdentity.SecurityStampClaimType,
                    await UserManager.GetSecurityStampAsync(user)));
            }

            if (UserManager.SupportsUserRole)
            {
                var roles = await UserManager.GetRolesAsync(user);
                foreach (var roleName in roles)
                {
                    id.AddClaim(new Claim(Options.ClaimsIdentity.RoleClaimType, roleName));
                    if (RoleManager.SupportsRoleClaims)
                    {
                        var role = await RoleManager.FindByNameAsync(roleName);
                        if (role != null)
                        {
                            id.AddClaims(await RoleManager.GetClaimsAsync(role));
                        }
                    }
                }
            }
            if (UserManager.SupportsUserClaim)
            {
                id.AddClaims(await UserManager.GetClaimsAsync(user));
            }

            ClaimsPrincipal principal = new ClaimsPrincipal(id);


            if (principal.Identity is ClaimsIdentity)
            {
                ClaimsIdentity identity = (ClaimsIdentity)principal.Identity;

                Claim displayNameClaim = new Claim("DisplayName", user.DisplayName);
                if (!identity.HasClaim(displayNameClaim.Type, displayNameClaim.Value))
                {
                    identity.AddClaim(displayNameClaim);
                }

                Claim emailClaim = new Claim(ClaimTypes.Email, user.Email);
                if (!identity.HasClaim(emailClaim.Type, emailClaim.Value))
                {
                    identity.AddClaim(emailClaim);
                }

                ISiteSettings site = await siteRepo.Fetch(user.SiteId, CancellationToken.None);

                if (site != null)
                {
                    Claim siteGuidClaim = new Claim("SiteGuid", site.SiteGuid.ToString());
                    if (!identity.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
                    {
                        identity.AddClaim(siteGuidClaim);
                    }

                }
                

                if (principal.IsInRole("Admins"))
                {
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
