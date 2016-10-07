// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Validation;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;
using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class ResourceOwnerPasswordValidator<TUser> : IResourceOwnerPasswordValidator
        where TUser : SiteUser
    {
        private readonly SiteSignInManager<TUser> _signInManager;
        private readonly SiteUserManager<TUser> _userManager;

        public ResourceOwnerPasswordValidator(SiteUserManager<TUser> userManager,
            SiteSignInManager<TUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user != null)
            {
                if (await _signInManager.CanSignInAsync(user))
                {
                    if (_userManager.SupportsUserLockout &&
                        await _userManager.IsLockedOutAsync(user))
                    {
                        context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidGrant);
                    }
                    else if (await _userManager.CheckPasswordAsync(user, context.Password))
                    {
                        if (_userManager.SupportsUserLockout)
                        {
                            await _userManager.ResetAccessFailedCountAsync(user);
                        }

                        var sub = await _userManager.GetUserIdAsync(user);
                        context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);
                    }
                    else if (_userManager.SupportsUserLockout)
                    {
                        await _userManager.AccessFailedAsync(user);
                    }
                }
            }
        }
    }
}
