// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Validation;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;
using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;
using Microsoft.AspNetCore.Identity;
using cloudscribe.Core.Models.Identity;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class ResourceOwnerPasswordValidator<TUser> : IResourceOwnerPasswordValidator
        where TUser : SiteUser
    {
        private readonly SignInManager<TUser> _signInManager;
        private readonly ILdapHelper _ldapHelper;
        private readonly SiteUserManager<TUser> _userManager;

        public ResourceOwnerPasswordValidator(
            SiteUserManager<TUser> userManager,
            SignInManager<TUser> signInManager,
            ILdapHelper ldapHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ldapHelper = ldapHelper;
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
            else
            {
                if (_ldapHelper.IsImplemented && !string.IsNullOrWhiteSpace(_userManager.Site.LdapServer) && !string.IsNullOrWhiteSpace(_userManager.Site.LdapDomain))
                {
                    LdapUser ldapUser = await _ldapHelper.TryLdapLogin(_userManager.Site as ILdapSettings, context.UserName, context.Password);
                    if (ldapUser != null) //ldap auth success
                    {
                        // lets assume that the ldap user has already been created as a user in cs
                        // So - how best do we find them?

                        var cs_ldapUser = await _userManager.FindByEmailAsync(ldapUser.Email);
                        if(cs_ldapUser == null)
                            cs_ldapUser = await _userManager.FindByNameAsync(context.UserName);

                        if (cs_ldapUser != null)
                        {
                            var sub = await _userManager.GetUserIdAsync(cs_ldapUser);
                            context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);
                        }
                    }
                }
            }
        }
    }
}
