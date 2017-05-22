// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-22
// Last Modified:			2017-05-22
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using Microsoft.AspNetCore.Http.Authentication;
//using cloudscribe.Web.Common.Extensions;
//using cloudscribe.Web.Common.Models;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace cloudscribe.Core.Web.Components
{
    public class AccountService
    {
        public AccountService(
            SiteContext currentSite,
            SiteUserManager<SiteUser> userManager,
            SiteSignInManager<SiteUser> signInManager,
            IIdentityServerIntegration identityServerIntegration,
            ILogger<AccountService> logger
            )
        {
            Site = currentSite;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.identityServerIntegration = identityServerIntegration;
            log = logger;
        }

        private readonly ISiteContext Site;
        private readonly SiteUserManager<SiteUser> userManager;
        private readonly SiteSignInManager<SiteUser> signInManager;
        private readonly IIdentityServerIntegration identityServerIntegration;
        // is logging really a concern in here
        // maybe should do that from controller
        private ILogger log;

        public async Task<UserLoginResult> TryExternalLogin()
        {
            SiteUser user = null;
            IUserContext userContext = null;
            var signinResult = SignInResult.Failed;
            var rejectReasons = new List<string>();
            var mustAcceptTerms = false;
            var needsAccountApproval = false;
            var needsEmailConfirmation = false;
            var needsPhoneConfirmation = false;

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // log.LogDebug("ExternalLoginCallback redirecting to login because GetExternalLoginInfoAsync returned null ");
                rejectReasons.Add("signInManager.GetExternalLoginInfoAsync returned null");
            }

            if ( info != null && (userManager.Site.RequireConfirmedEmail
                || userManager.Site.RequireConfirmedPhone
                || userManager.Site.RequireApprovalBeforeLogin
                || !string.IsNullOrWhiteSpace(userManager.Site.RegistrationAgreement)
                ))
            {
                signinResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

                if (signinResult == SignInResult.Failed)
                {
                    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                    var userName = await userManager.SuggestLoginNameFromEmail(Site.Id, email);
                    var newUser = new SiteUser
                    {
                        SiteId = Site.Id,
                        UserName = userName,
                        Email = email,
                        DisplayName = email.Substring(0, email.IndexOf("@")),
                        FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                        LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                        AccountApproved = Site.RequireApprovalBeforeLogin ? false : true
                    };
                    var identityResult = await userManager.CreateAsync(newUser);
                    if (identityResult.Succeeded)
                    {
                        identityResult = await userManager.AddLoginAsync(newUser, info);
                        user = newUser;
                    }
                    //try again
                    signinResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

                }
                
                if (user != null)
                {
                    userContext = new UserContext(user);

                    if (userManager.Site.RequireConfirmedEmail)
                    {
                        if (!await userManager.IsEmailConfirmedAsync(user))
                        {
                            var reason = $"login not allowed for {user.Email} because email is not confirmed";
                            rejectReasons.Add(reason);
                            needsEmailConfirmation = true;
                        }
                    }

                    if (userManager.Site.RequireApprovalBeforeLogin)
                    {
                        if (!user.AccountApproved)
                        {
                            var reason = $"login not allowed for {user.Email} because account not approved yet";
                            rejectReasons.Add(reason);
                        }
                    }

                    if (userManager.Site.RequireConfirmedPhone)
                    {
                        if (!user.PhoneNumberConfirmed || string.IsNullOrEmpty(user.PhoneNumber))
                        {
                            var reason = $"login not allowed for {user.Email} because phone not added or verified yet";
                            rejectReasons.Add(reason);
                            needsPhoneConfirmation = true;

                        }
                    }

                    if ((user.IsLockedOut) || (user.IsDeleted))
                    {
                        var reason = $"login not allowed for {user.Email} because account either locked out or flagged as deleted";
                        rejectReasons.Add(reason);
                    }

                    if (!string.IsNullOrWhiteSpace(userManager.Site.RegistrationAgreement))
                    {
                        // TODO: we need to capture user acceptance of terms with date
                    }

                }
            }

            if (user != null && rejectReasons.Count == 0)
            {
                var persistent = false;
                //if (userManager.Site.AllowPersistentLogin)
                //{
                //    persistent = model.RememberMe;
                //}


                //if (Site.UseEmailForLogin)
                //{
                //    signinResult = await signInManager.PasswordSignInAsync(
                //        user.Email,
                //        model.Password,
                //        persistent,
                //        lockoutOnFailure: false);
                //}
                //else
                //{
                //    signinResult = await signInManager.PasswordSignInAsync(
                //        model.UserName,
                //        model.Password,
                //        persistent,
                //        lockoutOnFailure: false);
                //}
            }


            return new UserLoginResult(
                signinResult,
                rejectReasons,
                userContext,
                mustAcceptTerms,
                needsAccountApproval,
                needsEmailConfirmation,
                needsPhoneConfirmation
                );

        }

        public async Task<UserLoginResult> TryLogin(LoginViewModel model)
        {
            SiteUser user = null;
            IUserContext userContext = null;
            var signinResult = SignInResult.Failed;
            var rejectReasons = new List<string>();
            var mustAcceptTerms = false;
            var needsAccountApproval = false;
            var needsEmailConfirmation = false;
            var needsPhoneConfirmation = false;

            if (userManager.Site.RequireConfirmedEmail 
                || userManager.Site.RequireConfirmedPhone
                || userManager.Site.RequireApprovalBeforeLogin
                || !string.IsNullOrWhiteSpace(userManager.Site.RegistrationAgreement)
                )
            {
               user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    userContext = new UserContext(user);
                    
                    if (userManager.Site.RequireConfirmedEmail)
                    {
                        if (!await userManager.IsEmailConfirmedAsync(user))
                        {
                            var reason = $"login not allowed for {user.Email} because email is not confirmed";
                            rejectReasons.Add(reason);
                            needsEmailConfirmation = true;
                        }
                    }

                    if (userManager.Site.RequireApprovalBeforeLogin)
                    {
                        if (!user.AccountApproved)
                        {
                            var reason = $"login not allowed for {user.Email} because account not approved yet";
                            rejectReasons.Add(reason);
                        }
                    }

                    if(userManager.Site.RequireConfirmedPhone)
                    {
                        if(!user.PhoneNumberConfirmed || string.IsNullOrEmpty(user.PhoneNumber))
                        {
                            var reason = $"login not allowed for {user.Email} because phone not added or verified yet";
                            rejectReasons.Add(reason);
                            needsPhoneConfirmation = true;
                            
                        }
                    }

                    if ((user.IsLockedOut) || (user.IsDeleted))
                    {  
                        var reason = $"login not allowed for {user.Email} because account either locked out or flagged as deleted";
                        rejectReasons.Add(reason);
                    }

                    if(!string.IsNullOrWhiteSpace(userManager.Site.RegistrationAgreement))
                    {
                       // TODO: we need to capture user acceptance of terms with date
                    }

                }
            }

            if(rejectReasons.Count == 0)
            {
                var persistent = false;
                if (userManager.Site.AllowPersistentLogin)
                {
                    persistent = model.RememberMe;
                }


                if (Site.UseEmailForLogin)
                {
                    signinResult = await signInManager.PasswordSignInAsync(
                        model.Email,
                        model.Password,
                        persistent,
                        lockoutOnFailure: false);
                }
                else
                {
                    signinResult = await signInManager.PasswordSignInAsync(
                        model.UserName,
                        model.Password,
                        persistent,
                        lockoutOnFailure: false);
                }
            }
            

            return new UserLoginResult(
                signinResult, 
                rejectReasons, 
                userContext,
                mustAcceptTerms,
                needsAccountApproval,
                needsEmailConfirmation,
                needsPhoneConfirmation
                );

        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string returnUrl = null)
        {
            return signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
        }


    }
}
