// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-27
// Last Modified:			2016-09-30
// 


using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
            IEnumerable<ICustomClaimProvider> customClaimProviders
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

            var userId = await UserManager.GetUserIdAsync(user);
            var userName = await UserManager.GetUserNameAsync(user);

            var id = new ClaimsIdentity(
                options.Cookies.ApplicationCookie.AuthenticationScheme,
                Options.ClaimsIdentity.UserNameClaimType,
                Options.ClaimsIdentity.RoleClaimType);

            id.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
            
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, userName));
            //needed by identityserver integration
            id.AddClaim(new Claim("sub", userId)); //JwtClaimTypes.Subject
            id.AddClaim(new Claim("name", userName)); //JwtClaimTypes.Name

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

            foreach(var provider in customClaimProviders)
            {
                await provider.AddClaims(user, id);
            }

            var principal = new ClaimsPrincipal(id);
            
            if (principal.Identity is ClaimsIdentity)
            {
                var identity = (ClaimsIdentity)principal.Identity;

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
                        Claim serverAdminRoleClaim = new Claim(ClaimTypes.Role, "ServerAdmins");
                        identity.AddClaim(serverAdminRoleClaim);
                    }
                }
            }
            
            return principal;
        }
    }
}
