// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-22
// Last Modified:			2017-05-23
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
            //SiteContext currentSite,
            SiteUserManager<SiteUser> userManager,
            SiteSignInManager<SiteUser> signInManager,
            IIdentityServerIntegration identityServerIntegration,
            ILogger<AccountService> logger
            )
        {
            //Site = currentSite;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.identityServerIntegration = identityServerIntegration;
            log = logger;
        }

        //private readonly ISiteContext Site;
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
            ExternalLoginInfo externalLoginInfo = null;
            var signinResult = SignInResult.Failed;
            var rejectReasons = new List<string>();
            var mustAcceptTerms = false;
            var needsAccountApproval = false;
            var needsEmailConfirmation = false;
            var needsPhoneConfirmation = false;
            string emailConfirmationToken = string.Empty;

            externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                rejectReasons.Add("signInManager.GetExternalLoginInfoAsync returned null");
            }

            if (externalLoginInfo != null && (userManager.Site.RequireConfirmedEmail
                || userManager.Site.RequireConfirmedPhone
                || userManager.Site.RequireApprovalBeforeLogin
                || !string.IsNullOrWhiteSpace(userManager.Site.RegistrationAgreement)
                ))
            {
                signinResult = await signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, isPersistent: false);

                if (signinResult == SignInResult.Failed)
                {
                    var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                    if(!string.IsNullOrWhiteSpace(email))
                    {
                        var userName = await userManager.SuggestLoginNameFromEmail(userManager.Site.Id, email);
                        var newUser = new SiteUser
                        {
                            SiteId = userManager.Site.Id,
                            UserName = userName,
                            Email = email,
                            DisplayName = email.Substring(0, email.IndexOf("@")),
                            FirstName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName),
                            LastName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Surname),
                            AccountApproved = userManager.Site.RequireApprovalBeforeLogin ? false : true
                        };
                        var identityResult = await userManager.CreateAsync(newUser);
                        if (identityResult.Succeeded)
                        {
                            identityResult = await userManager.AddLoginAsync(newUser, externalLoginInfo);
                            user = newUser;
                        }
                    }
                    
                    

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
                            emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
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

            if (signinResult == SignInResult.Failed && user != null && rejectReasons.Count == 0)
            {
                //try again
                signinResult = await signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, isPersistent: false);
                
            }


            return new UserLoginResult(
                signinResult,
                rejectReasons,
                userContext,
                mustAcceptTerms,
                needsAccountApproval,
                needsEmailConfirmation,
                emailConfirmationToken,
                needsPhoneConfirmation,
                externalLoginInfo
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
            string emailConfirmationToken = string.Empty;

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
                            emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
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


                if (userManager.Site.UseEmailForLogin)
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
                emailConfirmationToken,
                needsPhoneConfirmation
                );

        }

        

        public async Task<UserLoginResult> TryRegister(RegisterViewModel model)
        {
            SiteUser user = null;
            IUserContext userContext = null;
            var signinResult = SignInResult.Failed;
            var rejectReasons = new List<string>();
            var mustAcceptTerms = false;
            var needsAccountApproval = false;
            var needsEmailConfirmation = false;
            var needsPhoneConfirmation = false;
            string emailConfirmationToken = string.Empty;

            var userName = model.Username.Length > 0 ? model.Username : await userManager.SuggestLoginNameFromEmail(userManager.Site.Id, model.Email);
            var userNameAvailable = await userManager.LoginIsAvailable(Guid.Empty, userName);
            if (!userNameAvailable)
            {
                //ModelState.AddModelError("usernameerror", sr["Username not accepted please try a different value"]);
                //isValid = false;
                userName = await userManager.SuggestLoginNameFromEmail(userManager.Site.Id, model.Email);
            }

            user = new SiteUser
            {
                SiteId = userManager.Site.Id,
                UserName = userName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DisplayName = model.DisplayName,
                AccountApproved = userManager.Site.RequireApprovalBeforeLogin ? false : true
            };

            if (model.DateOfBirth.HasValue)
            {
                user.DateOfBirth = model.DateOfBirth.Value;
            }

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (userManager.Site.RequireConfirmedEmail)
                {
                    if (!await userManager.IsEmailConfirmedAsync(user))
                    {
                        var reason = $"login not allowed for {user.Email} because email is not confirmed";
                        rejectReasons.Add(reason);
                        needsEmailConfirmation = true;
                        emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
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
                

                //if (!string.IsNullOrWhiteSpace(userManager.Site.RegistrationAgreement))
                //{
                //    // TODO: we need to capture user acceptance of terms with date
                //    if(!model.AgreeToTerms)
                //    {
                //        var reason = $"login not allowed for {user.Email} because registration afgreement not accepted";
                //        rejectReasons.Add(reason);
                //    }
                //}


            }

            if(rejectReasons.Count == 0 && user != null)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                userContext = new UserContext(user);
                signinResult = SignInResult.Success;
            }

            return new UserLoginResult(
                signinResult,
                rejectReasons,
                userContext,
                mustAcceptTerms,
                needsAccountApproval,
                needsEmailConfirmation,
                emailConfirmationToken,
                needsPhoneConfirmation
                );

        }

        public async Task<ResetPasswordInfo> GetPasswordResetInfo(string email)
        {
            IUserContext userContext = null;
            string token = null;

            var user = await userManager.FindByNameAsync(email);
            if(user != null)
            {
                token = await userManager.GeneratePasswordResetTokenAsync(user);
                userContext = new UserContext(user);
            }

            return new ResetPasswordInfo(userContext, token);
        }

        public async Task<ResetPasswordResult> ResetPassword(string email, string password, string resetCode)
        {
            IUserContext userContext = null;
            IdentityResult result = IdentityResult.Failed(null);

            var user = await userManager.FindByNameAsync(email);
            if (user != null)
            {
                userContext = new UserContext(user);
                result = await userManager.ResetPasswordAsync(user, resetCode, password);
            }

            return new ResetPasswordResult(userContext, result);
        }

        public async Task<VerifyEmailInfo> GetEmailVerificationInfo(Guid userId)
        {
            IUserContext userContext = null;
            string token = null;
            var user = await userManager.Fetch(userManager.Site.Id, userId);
            if(user != null)
            {
                token = await userManager.GenerateEmailConfirmationTokenAsync((SiteUser)user);
                userContext = new UserContext(user);
            }


            return new VerifyEmailInfo(userContext, token);
        }

        public async Task<VerifyEmailResult> ConfirmEmailAsync(string userId, string code)
        {
            IUserContext userContext = null;
            IdentityResult result = IdentityResult.Failed(null);

            var user = await userManager.FindByIdAsync(userId);
            if(user != null)
            {
                userContext = new UserContext(user);
                result = await userManager.ConfirmEmailAsync(user, code);
            }

            return new VerifyEmailResult(userContext, result);
        }

        public async Task<IUserContext> GetTwoFactorAuthenticationUserAsync()
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if(user != null)
            {
                return new UserContext(user);
            }

            return null;
        }

        public async Task<TwoFactorInfo> GetTwoFactorInfo(string provider = null)
        {
            IUserContext userContext = null;
            IList<string> userFactors = new List<string>();
            string token = null;
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if(user != null)
            {
                if (!string.IsNullOrWhiteSpace(provider))
                {
                    token = await userManager.GenerateTwoFactorTokenAsync(user, provider);
                }
                userContext = new UserContext(user);
                userFactors = await userManager.GetValidTwoFactorProvidersAsync(user);
            }

            return new TwoFactorInfo(userContext, userFactors, token);
        }

        //public async Task<string> GenerateTwoFactorTokenAsync(string provider)
        //{
        //    var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
        //    if(user != null)
        //    {
        //        return await userManager.GenerateTwoFactorTokenAsync(user, provider);
        //    }

        //    return null;
        //}

        public async Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool rememberMe, bool rememberBrowser)
        {
            return await signInManager.TwoFactorSignInAsync(provider, code, rememberMe, rememberBrowser);
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string returnUrl = null)
        {
            return signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
        }

        public IEnumerable<AuthenticationDescription> GetExternalAuthenticationSchemes()
        {
            return signInManager.GetExternalAuthenticationSchemes();
        }

        public bool IsSignedIn(ClaimsPrincipal user)
        {
            return signInManager.IsSignedIn(user);
        }

        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<bool> LoginNameIsAvailable(Guid userId, string loginName)
        {
            return await userManager.LoginIsAvailable(userId, loginName);
        }

    }
}
