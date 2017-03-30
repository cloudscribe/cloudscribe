// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class ProfileService<TUser> : IProfileService
        where TUser : SiteUser
    {
        private readonly IUserClaimsPrincipalFactory<TUser> _claimsFactory;
        private readonly SiteUserManager<TUser> _userManager;
        private readonly IJwtClaimsProcessor<TUser> _claimsProcessor;


        public ProfileService(
            SiteUserManager<TUser> userManager,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IJwtClaimsProcessor<TUser> claimsProcessor
            )
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _claimsProcessor = claimsProcessor;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);
            var filtered = _claimsProcessor.ProcessClaims(context, principal.Claims, user);
            context.IssuedClaims.AddRange(filtered);
        }
        

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
