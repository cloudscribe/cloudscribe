using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class ProfileService<TUser> : IProfileService
        where TUser : SiteUser
    {
        private readonly IUserClaimsPrincipalFactory<TUser> _claimsFactory;
        private readonly SiteUserManager<TUser> _userManager;
      
        public ProfileService(
            SiteUserManager<TUser> userManager,
            IUserClaimsPrincipalFactory<TUser> claimsFactory
            )
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var issued = context.IssuedClaims;
            

            context.AddRequestedClaims(principal.Claims);

            var requestedClaims = context.RequestedClaimTypes.ToList();
            var allClaims = principal.Claims.ToList();
            var foundClaims = principal.Claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).Select(x => x.Type).ToList();
            var neededClaims = requestedClaims.Except(foundClaims);
            var claimsToAdd = new List<Claim>();

            //try to add needed claims if we know what they are
            foreach (var c in neededClaims)
            {
                if (c == JwtClaimTypes.Name)
                {
                    claimsToAdd.Add(new Claim(JwtClaimTypes.Name, user.DisplayName));
                }

                if (c == JwtClaimTypes.Email)
                {
                    claimsToAdd.Add(new Claim(JwtClaimTypes.Email, user.Email));
                }

                if (c == JwtClaimTypes.BirthDate && user.DateOfBirth.HasValue)
                {
                    claimsToAdd.Add(new Claim(JwtClaimTypes.BirthDate, user.DateOfBirth.Value.ToString("YYYY-MM-DD")));
                }

                if (c == JwtClaimTypes.EmailVerified)
                {
                    claimsToAdd.Add(new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed.ToString().ToLowerInvariant()));
                }

                if (c == JwtClaimTypes.FamilyName && !string.IsNullOrWhiteSpace(user.LastName))
                {
                    claimsToAdd.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
                }

                if (c == JwtClaimTypes.GivenName && !string.IsNullOrWhiteSpace(user.FirstName))
                {
                    claimsToAdd.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
                }

                if (c == JwtClaimTypes.PhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
                {
                    claimsToAdd.Add(new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber));
                }

                if (c == JwtClaimTypes.PhoneNumberVerified)
                {
                    claimsToAdd.Add(new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed.ToString().ToLowerInvariant()));
                }

                if (c == JwtClaimTypes.WebSite && !string.IsNullOrWhiteSpace(user.WebSiteUrl))
                {
                    claimsToAdd.Add(new Claim(JwtClaimTypes.WebSite, user.WebSiteUrl));
                }

                if (c == JwtClaimTypes.ZoneInfo && !string.IsNullOrWhiteSpace(user.TimeZoneId))
                {
                    claimsToAdd.Add(new Claim(JwtClaimTypes.ZoneInfo, user.TimeZoneId));
                }

                if (c == JwtClaimTypes.Gender && !string.IsNullOrWhiteSpace(user.Gender))
                {
                    claimsToAdd.Add(new Claim(JwtClaimTypes.Gender, user.Gender));
                }

                

            }
            if (claimsToAdd.Count > 0)
            {
                context.IssuedClaims.AddRange(claimsToAdd);
            }


        }
        

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
