// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-06-27
// Last Modified:			2019-04-20
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
            //IOidcHybridFlowHelper oidcHybridFlowHelper,
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

            _queries = siteQueries;
            _options = optionsAccessor.Value;
            _customClaimProviders = customClaimProviders;
           // _oidcHybridFlowHelper = oidcHybridFlowHelper;
        }
        
        private readonly ISiteQueries _queries;
        private IdentityOptions _options;
        private readonly IEnumerable<ICustomClaimProvider> _customClaimProviders;
        //private readonly IOidcHybridFlowHelper _oidcHybridFlowHelper;

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

                foreach (var provider in _customClaimProviders)
                {
                    await provider.AddClaims(user, identity);
                }

                if(!string.IsNullOrWhiteSpace(user.BrowserKey))
                {
                    var browserKeyClaim = new Claim("browser-key", user.BrowserKey);
                    if (!identity.Claims.Any(x => x.Type == "browser-key"))
                    {
                        identity.AddClaim(browserKeyClaim);
                    }
                }
                

                var displayNameClaim = new Claim("DisplayName", user.DisplayName);
                if (!identity.HasClaim(displayNameClaim.Type, displayNameClaim.Value))
                {
                    identity.AddClaim(displayNameClaim);
                }

                if (!string.IsNullOrWhiteSpace(user.AvatarUrl))
                {
                    var avatarUrlClaim = new Claim("AvatarUrl", user.AvatarUrl);
                    if (!identity.HasClaim(avatarUrlClaim.Type, avatarUrlClaim.Value))
                    {
                        identity.AddClaim(avatarUrlClaim);
                    }
                }
                
                if(!string.IsNullOrEmpty(user.Email))
                {
                    var emailClaim = new Claim("email", user.Email);
                    if (!identity.HasClaim(emailClaim.Type, emailClaim.Value))
                    {
                        identity.AddClaim(emailClaim);
                    }
                }
                

                var site = await _queries.Fetch(user.SiteId, CancellationToken.None);

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

                //var jwt = await _oidcHybridFlowHelper.GetCurrentJwt(principal);

                //if (!string.IsNullOrEmpty(jwt))
                //{
                //    var accessTokenClaim = new Claim("access_token", jwt);
                //    if (!identity.HasClaim(accessTokenClaim.Type, accessTokenClaim.Value))
                //    {
                //        identity.AddClaim(accessTokenClaim);
                //    }
                //}


               

            }
            
            return principal;
        }
    }
}
