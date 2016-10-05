// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class ProfileService<TUser> : IProfileService
        where TUser : SiteUser
    {
        private readonly IUserClaimsPrincipalFactory<TUser> _claimsFactory;
        private readonly SiteUserManager<TUser> _userManager;

        public ProfileService(SiteUserManager<TUser> userManager,
            IUserClaimsPrincipalFactory<TUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);
            context.AddFilteredClaims(principal.Claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
