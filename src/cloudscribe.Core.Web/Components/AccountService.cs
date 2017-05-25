// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-22
// Last Modified:			2017-05-24
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using cloudscribe.Messaging.Email;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
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
            SiteUserManager<SiteUser> userManager,
            SiteSignInManager<SiteUser> signInManager,
            IIdentityServerIntegration identityServerIntegration,
            ISocialAuthEmailVerfificationPolicy socialAuthEmailVerificationPolicy,
            ISmtpOptionsProvider smtpOptionsProvider
            //,ILogger<AccountService> logger
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.identityServerIntegration = identityServerIntegration;
            this.socialAuthEmailVerificationPolicy = socialAuthEmailVerificationPolicy;
            this.smtpOptionsProvider = smtpOptionsProvider;
            //log = logger;
        }

        //private readonly ISiteContext Site;
        private readonly SiteUserManager<SiteUser> userManager;
        private readonly SiteSignInManager<SiteUser> signInManager;
        private readonly IIdentityServerIntegration identityServerIntegration;
        private readonly ISocialAuthEmailVerfificationPolicy socialAuthEmailVerificationPolicy;
        private readonly ISmtpOptionsProvider smtpOptionsProvider;
        private SmtpOptions smtpOptions = null;
        // is logging really a concern in here
        // maybe should do that from controller
        // private ILogger log;

        private class LoginResultTemplate
        {
            public bool MustAcceptTerms { get; set; }
            public bool NeedsAccountApproval { get; set; }
            public bool NeedsEmailConfirmation { get; set; }
            public string EmailConfirmationToken { get; set; } = string.Empty;
            public bool NeedsPhoneConfirmation { get; set; }
            public ExternalLoginInfo ExternalLoginInfo { get; set; } = null;
            public List<string> RejectReasons { get; set; } = new List<string>();
            public SiteUser User { get; set; } = null;
            public SignInResult SignInResult { get; set; } = SignInResult.Failed;
        }

        private async Task<bool> RequireConfirmedEmail()
        {
            if (!userManager.Site.RequireConfirmedEmail) return false;
            if (smtpOptions == null) { smtpOptions = await smtpOptionsProvider.GetSmtpOptions().ConfigureAwait(false); }
            return !string.IsNullOrEmpty(smtpOptions.Server);
        }

        private async Task ProcessAccountLoginRules(LoginResultTemplate template)
        {
            if (template.User == null) return;
            var requireConfirmedEmail = await RequireConfirmedEmail();

            if (requireConfirmedEmail)
            {
                if (!template.User.EmailConfirmed)
                {
                    var reason = $"login not allowed for {template.User.Email} because email is not confirmed";
                    template.RejectReasons.Add(reason);
                    template.NeedsEmailConfirmation = true;
                    template.EmailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(template.User);
                    template.SignInResult = SignInResult.NotAllowed;
                }
            }

            if (userManager.Site.RequireApprovalBeforeLogin)
            {
                if (!template.User.AccountApproved)
                {
                    var reason = $"login not allowed for {template.User.Email} because account not approved yet";
                    template.RejectReasons.Add(reason);
                    template.NeedsAccountApproval = true;
                    template.SignInResult = SignInResult.NotAllowed;
                }
            }

            if (userManager.Site.RequireConfirmedPhone && userManager.Site.SmsIsConfigured())
            {
                if (string.IsNullOrEmpty(template.User.PhoneNumber))
                {
                    // we can't add a reason here that would block login
                    // we need to enforce user to add phone number via middleware redirect
                    // not by blocking login
                    // because without logging in user cannot update the phone
                    template.NeedsPhoneConfirmation = true;
                }
                else
                {
                    if (!template.User.PhoneNumberConfirmed)
                    {
                        var reason = $"login not allowed for {template.User.Email} because phone not added or verified yet";
                        template.RejectReasons.Add(reason);
                        template.NeedsPhoneConfirmation = true;

                    }
                }

            }

            if (!string.IsNullOrWhiteSpace(userManager.Site.RegistrationAgreement))
            {
                // TODO: we need to capture user acceptance of terms with date
                // need to not block login otherwise how can we make the user agree agree to terms on account
                // need to enforce it with middleware
                template.MustAcceptTerms = true;
            }

            if (template.User.IsLockedOut)
            {
                var reason = $"login not allowed for {template.User.Email} because account is locked out";
                template.RejectReasons.Add(reason);
                template.SignInResult = SignInResult.LockedOut;

               
            }

            if (template.User.IsDeleted)
            {
                var reason = $"login not allowed for {template.User.Email} because account is flagged as deleted";
                template.RejectReasons.Add(reason);
                template.User = null;


            }
        }

        public async Task<UserLoginResult> TryExternalLogin(string providedEmail = "")
        {
            var template = new LoginResultTemplate();
            IUserContext userContext = null;
            
            var email = providedEmail;

            template.ExternalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
            if (template.ExternalLoginInfo == null)
            {
                template.RejectReasons.Add("signInManager.GetExternalLoginInfoAsync returned null");
            }
            else
            {
                if(string.IsNullOrWhiteSpace(email))
                {
                    email = template.ExternalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                }
                
                if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
                {
                    template.User = await userManager.FindByNameAsync(email);
                    if(template.User == null)
                    {
                        template.User = await CreateUserFromExternalLogin(template.ExternalLoginInfo);
                    }
                }
            }
 
            if (template.User != null)
            {
                await ProcessAccountLoginRules(template);
            }
            
            if (template.SignInResult == SignInResult.Failed && template.User != null && template.RejectReasons.Count == 0)
            {
                template.SignInResult = await signInManager.ExternalLoginSignInAsync(template.ExternalLoginInfo.LoginProvider, template.ExternalLoginInfo.ProviderKey, isPersistent: false);
                if(template.SignInResult.Succeeded)
                {
                    // TODO:
                    //update last login time
                }
                
            }

            if(template.SignInResult != SignInResult.Success && template.SignInResult != SignInResult.TwoFactorRequired)
            {
                //clear the external login 
                await signInManager.SignOutAsync();
            }

            if(template.User != null) { userContext = new UserContext(template.User); }
            
            return new UserLoginResult(
                template.SignInResult,
                template.RejectReasons,
                userContext,
                template.MustAcceptTerms,
                template.NeedsAccountApproval,
                template.NeedsEmailConfirmation,
                template.EmailConfirmationToken,
                template.NeedsPhoneConfirmation,
                template.ExternalLoginInfo
                );

        }

        


        private async Task<SiteUser> CreateUserFromExternalLogin(ExternalLoginInfo externalLoginInfo)
        {
            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
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
                    AccountApproved = userManager.Site.RequireApprovalBeforeLogin ? false : true,
                    EmailConfirmed = socialAuthEmailVerificationPolicy.HasVerifiedEmail(externalLoginInfo)
                };
                var identityResult = await userManager.CreateAsync(newUser);
                if (identityResult.Succeeded)
                {
                    identityResult = await userManager.AddLoginAsync(newUser, externalLoginInfo);
                    return newUser;
                }

               
            }

            return null;
        }

        public async Task<UserLoginResult> TryLogin(LoginViewModel model)
        {
            var template = new LoginResultTemplate();
            IUserContext userContext = null;
           
            template.User = await userManager.FindByNameAsync(model.Email);
            if (template.User != null)
            {
                await ProcessAccountLoginRules(template);  
            }
            

            if(template.User != null && template.SignInResult == SignInResult.Failed &&  template.RejectReasons.Count == 0)
            {
                userContext = new UserContext(template.User);
                var persistent = false;
                if (userManager.Site.AllowPersistentLogin)
                {
                    persistent = model.RememberMe;
                }


                if (userManager.Site.UseEmailForLogin)
                {
                    template.SignInResult = await signInManager.PasswordSignInAsync(
                        model.Email,
                        model.Password,
                        persistent,
                        lockoutOnFailure: false);
                }
                else
                {
                    template.SignInResult = await signInManager.PasswordSignInAsync(
                        model.UserName,
                        model.Password,
                        persistent,
                        lockoutOnFailure: false);
                }
            }
            

            return new UserLoginResult(
                template.SignInResult, 
                template.RejectReasons, 
                userContext,
                template.MustAcceptTerms,
                template.NeedsAccountApproval,
                template.NeedsEmailConfirmation,
                template.EmailConfirmationToken,
                template.NeedsPhoneConfirmation
                );

        }

        

        public async Task<UserLoginResult> TryRegister(RegisterViewModel model)
        {
            var template = new LoginResultTemplate();
            IUserContext userContext = null;

            var userName = model.Username.Length > 0 ? model.Username : await userManager.SuggestLoginNameFromEmail(userManager.Site.Id, model.Email);
            var userNameAvailable = await userManager.LoginIsAvailable(Guid.Empty, userName);
            if (!userNameAvailable)
            {
                //ModelState.AddModelError("usernameerror", sr["Username not accepted please try a different value"]);
                //isValid = false;
                userName = await userManager.SuggestLoginNameFromEmail(userManager.Site.Id, model.Email);
            }

            var user = new SiteUser
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
                template.User = user;
                await ProcessAccountLoginRules(template);
            }

            if(template.RejectReasons.Count == 0 && user != null && template.SignInResult == SignInResult.Failed) // failed is initial state, could have been changed to lockedout
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                userContext = new UserContext(user);
                template.SignInResult = SignInResult.Success;
            }

            return new UserLoginResult(
                template.SignInResult,
                template.RejectReasons,
                userContext,
                template.MustAcceptTerms,
                template.NeedsAccountApproval,
                template.NeedsEmailConfirmation,
                template.EmailConfirmationToken,
                template.NeedsPhoneConfirmation
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
