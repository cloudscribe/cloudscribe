// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Validation;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;
using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;
using Microsoft.AspNetCore.Identity;
using cloudscribe.Core.Models.Identity;
using System;
using cloudscribe.Core.IdentityServerIntegration.Handlers;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class ResourceOwnerPasswordValidator<TUser> : IResourceOwnerPasswordValidator
        where TUser : SiteUser
    {
        private readonly SignInManager<TUser>   _signInManager;
        private readonly IHandlePasswordValidation<TUser> _handler;
        private readonly ILdapHelper            _ldapHelper;
        private readonly IUserCommands          _userCommands;
        private readonly SiteUserManager<TUser> _userManager;

        public ResourceOwnerPasswordValidator(
               SiteUserManager<TUser> userManager,
               SignInManager<TUser> signInManager,
               IHandlePasswordValidation<TUser> handler,
               ILdapHelper ldapHelper,
               IUserCommands userCommands)
        {
            _userManager   = userManager;
            _signInManager = signInManager;
            _handler       = handler;
            _ldapHelper    = ldapHelper;
            _userCommands  = userCommands;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByNameAsync(context.UserName);
            bool signInSuccess = false;

            //try ordinary sign in if not an LDAP User. LDAP users have an account with no password set.
            if (user != null && !string.IsNullOrWhiteSpace(user.PasswordHash))
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

                        await _handler.HandlePasswordValidationSuccess(user);
                        signInSuccess = true;
                    }
                    else if (_userManager.SupportsUserLockout)
                    {
                        await _userManager.AccessFailedAsync(user);
                    }
                }
            }

            if(!signInSuccess) // failed a standard login - so possibly try LDAP (if enabled)
            {
                if (_ldapHelper.IsImplemented && !string.IsNullOrWhiteSpace(_userManager.Site.LdapServer) && !string.IsNullOrWhiteSpace(_userManager.Site.LdapDomain))
                {
                    LdapUser ldapUser = await _ldapHelper.TryLdapLogin(
                        _userManager.Site as ILdapSettings,
                        context.UserName,
                        context.Password
                    );
                    if (ldapUser != null && ldapUser.ResultStatus == "PASS") //ldap auth success
                    {
                        signInSuccess = true;
                        if (user != null) //and a CS user exists also
                        {
                            if (_userManager.SupportsUserLockout)
                            {
                                await _userManager.ResetAccessFailedCountAsync(user);
                            }

                            var sub = await _userManager.GetUserIdAsync(user);
                            context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);
                            await _handler.HandlePasswordValidationSuccess(user);
                        }
                        else // No CS user exists yet so create the CS placeholder user to match the LDAP one
                        {
                            var isFakeLdapEmail = false;

                            var cloudscribeUser = new SiteUser()
                            {
                                SiteId          = _userManager.Site.Id,
                                UserName        = context.UserName,
                                Email           = ldapUser.Email,
                                DisplayName     = ldapUser.CommonName,
                                FirstName       = ldapUser.FirstName,
                                LastName        = ldapUser.LastName,
                                LastLoginUtc    = DateTime.UtcNow,
                                AccountApproved = true
                            };

                            if (string.IsNullOrWhiteSpace(cloudscribeUser.DisplayName))
                            {
                                cloudscribeUser.DisplayName = context.UserName;
                            }

                            if (string.IsNullOrWhiteSpace(cloudscribeUser.Email))
                            {
                                // identity doesn't allow create user with no email so fake it here then null it out below after sign in.
                                // the cloudscribe site rules middleware will then force the user to provide an email
                                cloudscribeUser.Email = context.UserName + "@fake-email.com";
                                isFakeLdapEmail = true;
                            }

                            var createdResult = await _userManager.CreateAsync((TUser)cloudscribeUser);
                            if (createdResult.Succeeded)
                            {
                                //need to get newly created user's ID
                                user = await _userManager.FindByNameAsync(context.UserName);
                                var sub = await _userManager.GetUserIdAsync(user);
                                context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);

                                if (isFakeLdapEmail)
                                {
                                    // clear the fake email, the user should then be forced to provide an email by site rules middleware
                                    cloudscribeUser.Email = null;
                                    cloudscribeUser.NormalizedEmail = null;
                                    await _userCommands.Update(cloudscribeUser);
                                }

                                await _handler.HandlePasswordValidationSuccess((TUser)cloudscribeUser);
                            }
                        }
                    }

                }
            }
            //So standard login failed and LDAP login failed
            if (!signInSuccess)
            {
                context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidGrant);
            }

        }
    }
}
