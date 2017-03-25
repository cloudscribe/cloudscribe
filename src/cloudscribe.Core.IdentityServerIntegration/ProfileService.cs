// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using IdentityModel;

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
            //context.AddFilteredClaims(principal.Claims);
            var filtered = FilterClaims(context, principal.Claims);
            context.IssuedClaims.AddRange(filtered);
        }

        private List<Claim> FilterClaims(ProfileDataRequestContext context, IEnumerable<Claim> claims)
        {
            var result = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
            //if(context.RequestedClaimTypes.Contains(JwtClaimTypes.Role))
            //{
                var roleClaims = claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                foreach (var roleClaim in roleClaims)
                {
                //result.Add(new Claim(JwtClaimTypes.Role, roleClaim.Value));
                result.Add(new Claim(roleClaim.Type, roleClaim.Value));
            }

            //}

            return result;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
