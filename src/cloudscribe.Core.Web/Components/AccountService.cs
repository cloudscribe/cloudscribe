// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-22
// Last Modified:			2018-02-25
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using cloudscribe.Core.Models.Identity;

namespace cloudscribe.Core.Web.Components
{
    public class AccountService : IAccountService
    {
        public AccountService(
            SiteUserManager<SiteUser> userManager,
            SignInManager<SiteUser> signInManager,
            IIdentityServerIntegration identityServerIntegration,
            ISocialAuthEmailVerfificationPolicy socialAuthEmailVerificationPolicy,
            IProcessAccountLoginRules loginRulesProcessor,
            INewUserDisplayNameResolver displayNameResolver
            //,ILogger<AccountService> logger
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.identityServerIntegration = identityServerIntegration;
            this.socialAuthEmailVerificationPolicy = socialAuthEmailVerificationPolicy;
            this.loginRulesProcessor = loginRulesProcessor;
            this.displayNameResolver = displayNameResolver;


            //log = logger;
        }

        protected readonly SiteUserManager<SiteUser> userManager;
        protected readonly SignInManager<SiteUser> signInManager;
        protected readonly IIdentityServerIntegration identityServerIntegration;
        protected readonly ISocialAuthEmailVerfificationPolicy socialAuthEmailVerificationPolicy;
        protected readonly IProcessAccountLoginRules loginRulesProcessor;
        protected readonly INewUserDisplayNameResolver displayNameResolver;
        // private ILogger log;

        private async Task<SiteUser> CreateUserFromExternalLogin(
            ExternalLoginInfo externalLoginInfo, 
            string providedEmail = null,
            bool? didAcceptTerms = null
            )
        {
            var email = providedEmail;
            if (string.IsNullOrWhiteSpace(email))
            {
                email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            }

            DateTime? termsAcceptedDate = null;
            if (didAcceptTerms == true && !string.IsNullOrWhiteSpace(userManager.Site.RegistrationAgreement)) { termsAcceptedDate = DateTime.UtcNow; }

            if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
            {
                var userName = await userManager.SuggestLoginNameFromEmail(userManager.Site.Id, email);
                var newUser = new SiteUser
                {
                    SiteId = userManager.Site.Id,
                    UserName = userName,
                    Email = email,
                    FirstName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName),
                    LastName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Surname),
                    AccountApproved = userManager.Site.RequireApprovalBeforeLogin ? false : true,
                    EmailConfirmed = socialAuthEmailVerificationPolicy.HasVerifiedEmail(externalLoginInfo),
                    AgreementAcceptedUtc = termsAcceptedDate,
                    LastLoginUtc = DateTime.UtcNow
                };
                //https://github.com/joeaudette/cloudscribe/issues/346
                newUser.DisplayName = displayNameResolver.ResolveDisplayName(newUser);

                var identityResult = await userManager.CreateAsync(newUser);
                if (identityResult.Succeeded)
                {
                    identityResult = await userManager.AddLoginAsync(newUser, externalLoginInfo);
                    return newUser;
                }
            }
            return null;
        }

        
        public virtual async Task<UserLoginResult> TryExternalLogin(string providedEmail = "", bool? didAcceptTerms = null)
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
                template.User = await userManager.FindByLoginAsync(template.ExternalLoginInfo.LoginProvider, template.ExternalLoginInfo.ProviderKey);
                
                if(template.User == null)
                {
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        email = template.ExternalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                    }

                    if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
                    {
                        template.User = await userManager.FindByNameAsync(email);
                    }
                }
                
                if (template.User == null)
                {
                    template.User = await CreateUserFromExternalLogin(template.ExternalLoginInfo, email, didAcceptTerms);
                    if(template.User != null) { template.IsNewUserRegistration = true; }
                }
            }
 
            if (template.User != null)
            {
                await loginRulesProcessor.ProcessAccountLoginRules(template);
            }
            
            if (template.SignInResult == SignInResult.Failed && template.User != null && template.RejectReasons.Count == 0)
            {
                template.SignInResult = await signInManager.ExternalLoginSignInAsync(template.ExternalLoginInfo.LoginProvider, template.ExternalLoginInfo.ProviderKey, isPersistent: false);
                if(template.SignInResult.Succeeded)
                {
                  
                    //update last login time
                    if(!template.IsNewUserRegistration)
                    {
                        //already tracked if user was just created
                        template.User.LastLoginUtc = DateTime.UtcNow;
                        await userManager.UpdateAsync(template.User);
                    }
                    
                }      
            }

            if(template.User != null
                && template.SignInResult != SignInResult.Success 
                && template.SignInResult != SignInResult.TwoFactorRequired)
            {
                //clear the external login 
                await signInManager.SignOutAsync();
            }

            if(template.User != null) { userContext = new UserContext(template.User); }
            
            return new UserLoginResult(
                template.SignInResult,
                template.RejectReasons,
                userContext,
                template.IsNewUserRegistration,
                template.MustAcceptTerms,
                template.NeedsAccountApproval,
                template.NeedsEmailConfirmation,
                template.EmailConfirmationToken,
                template.NeedsPhoneConfirmation,
                template.ExternalLoginInfo
                );

        }

        //public bool IsValidPassowrd(string password)
        //{
        //    return passwordValidator.
        //}
        
        public virtual async Task<UserLoginResult> TryLogin(LoginViewModel model)
        {
            var template = new LoginResultTemplate();
            IUserContext userContext = null;
           
            if(userManager.Site.UseEmailForLogin)
            {
                template.User = await userManager.FindByNameAsync(model.Email);
            }
            else
            {
                template.User = await userManager.FindByNameAsync(model.UserName);
            }
            
            if (template.User != null)
            {
                await loginRulesProcessor.ProcessAccountLoginRules(template);
            }

            if(template.User != null)
            {
                userContext = new UserContext(template.User);
            }
           
            if(userContext != null 
                && template.SignInResult == SignInResult.Failed 
                &&  template.RejectReasons.Count == 0)
            {
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

                if(template.SignInResult.Succeeded)
                {
                    //update last login time
                    template.User.LastLoginUtc = DateTime.UtcNow;
                    await userManager.UpdateAsync(template.User);
                }
            }
            
            return new UserLoginResult(
                template.SignInResult, 
                template.RejectReasons, 
                userContext,
                template.IsNewUserRegistration,
                template.MustAcceptTerms,
                template.NeedsAccountApproval,
                template.NeedsEmailConfirmation,
                template.EmailConfirmationToken,
                template.NeedsPhoneConfirmation
                );
        }

        public virtual async Task<UserLoginResult> Try2FaLogin(LoginWith2faViewModel model, bool rememberMe)
        {
            var template = new LoginResultTemplate();
            IUserContext userContext = null;
            template.User = await signInManager.GetTwoFactorAuthenticationUserAsync();

            if (template.User != null)
            {
                await loginRulesProcessor.ProcessAccountLoginRules(template);
            }

            if (template.User != null)
            {
                userContext = new UserContext(template.User);
            }
            
            if(userContext != null
                && template.SignInResult == SignInResult.Failed //initial state
                && template.RejectReasons.Count == 0
                )
            {
                var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
                template.SignInResult = await signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);
            }
            
            if (template.SignInResult.Succeeded)
            {
                //update last login time
                template.User.LastLoginUtc = DateTime.UtcNow;
                await userManager.UpdateAsync(template.User);
            }

            return new UserLoginResult(
                template.SignInResult,
                template.RejectReasons,
                userContext,
                template.IsNewUserRegistration,
                template.MustAcceptTerms,
                template.NeedsAccountApproval,
                template.NeedsEmailConfirmation,
                template.EmailConfirmationToken,
                template.NeedsPhoneConfirmation
                );

        }

        public virtual async Task<UserLoginResult> TryLoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model)
        {
            var template = new LoginResultTemplate();
            IUserContext userContext = null;
            template.User = await signInManager.GetTwoFactorAuthenticationUserAsync();

            if (template.User != null)
            {
                await loginRulesProcessor.ProcessAccountLoginRules(template);
            }

            if (template.User != null)
            {
                userContext = new UserContext(template.User);
            }

            if (userContext != null
                && template.SignInResult == SignInResult.Failed //initial state
                && template.RejectReasons.Count == 0
                )
            {
                var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);
                template.SignInResult = await signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
            }

            if(template.SignInResult.Succeeded)
            {
                //update last login time
                template.User.LastLoginUtc = DateTime.UtcNow;
                await userManager.UpdateAsync(template.User);
            }

            return new UserLoginResult(
                template.SignInResult,
                template.RejectReasons,
                userContext,
                template.IsNewUserRegistration,
                template.MustAcceptTerms,
                template.NeedsAccountApproval,
                template.NeedsEmailConfirmation,
                template.EmailConfirmationToken,
                template.NeedsPhoneConfirmation
                );

        }



        public virtual async Task<UserLoginResult> TryRegister(
            RegisterViewModel model, 
            ModelStateDictionary modelState,
            HttpContext httpContext,
            IHandleCustomRegistration customRegistration
            )
        {
            var template = new LoginResultTemplate();
            IUserContext userContext = null;

            var userName = model.Username.Length > 0 ? model.Username : await userManager.SuggestLoginNameFromEmail(userManager.Site.Id, model.Email);
            var userNameAvailable = await userManager.LoginIsAvailable(Guid.Empty, userName);
            if (!userNameAvailable)
            {
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
                LastLoginUtc = DateTime.UtcNow,
                AccountApproved = userManager.Site.RequireApprovalBeforeLogin ? false : true
            };

            await customRegistration.ProcessUserBeforeCreate(user, httpContext);
            

            if (model.DateOfBirth.HasValue)
            {
                user.DateOfBirth = model.DateOfBirth.Value;
            }

            if (userManager.Site.RegistrationAgreement.Length > 0)
            {
                if (model.AgreeToTerms)
                {
                    user.AgreementAcceptedUtc = DateTime.UtcNow;
                }
            }

            var result = await userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                template.User = user;
                template.IsNewUserRegistration = true;
                await loginRulesProcessor.ProcessAccountLoginRules(template);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    modelState.AddModelError(string.Empty, error.Description);
                }
            }
           

            if(template.RejectReasons.Count == 0 
                && user != null 
                && template.SignInResult == SignInResult.Failed // failed is initial state, could have been changed to lockedout
                && result.Errors.Count<IdentityError>() == 0
                ) 
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                template.SignInResult = SignInResult.Success;
            }

            if(template.User != null)
            {
                userContext = new UserContext(template.User);
            }

            return new UserLoginResult(
                template.SignInResult,
                template.RejectReasons,
                userContext,
                template.IsNewUserRegistration,
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

        public virtual async Task<VerifyEmailResult> ConfirmEmailAsync(string userId, string code)
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

        public async Task HandleUserRolesChanged(ClaimsPrincipal principal)
        {
            if (principal == null) return;
            var userId = principal.GetUserId();
            if (string.IsNullOrEmpty(userId)) return;
            var user = await userManager.FindByIdAsync(userId);
            await signInManager.SignOutAsync();
            if (user != null)
            {
                user.RolesChanged = false;
                var result = await userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                }
            }
            
        }

        public async Task<bool> AcceptRegistrationAgreement(ClaimsPrincipal principal)
        {
            if (principal == null) return false;
            var userId = principal.GetUserId();
            if (string.IsNullOrEmpty(userId)) return false;
            var user = await userManager.FindByIdAsync(userId);
            user.AgreementAcceptedUtc = DateTime.UtcNow;
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded) return true;

            return false;
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

        public async Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool rememberMe, bool rememberBrowser)
        {
            return await signInManager.TwoFactorAuthenticatorSignInAsync(code, rememberMe, rememberBrowser);
        }

        public async Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string code)
        {
            return await signInManager.TwoFactorRecoveryCodeSignInAsync(code);
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string returnUrl = null)
        {
            return signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
        }

        public async Task<List<Microsoft.AspNetCore.Authentication.AuthenticationScheme>> GetExternalAuthenticationSchemes()
        {
            var result = await signInManager.GetExternalAuthenticationSchemesAsync();
            var allProviders = result.OrderBy(x => x.DisplayName).ToList();
            var filteredProviders = new List<Microsoft.AspNetCore.Authentication.AuthenticationScheme>();
            foreach(var provider in allProviders)
            {
                if(IsSocialAuthConfigured(provider))
                {
                    filteredProviders.Add(provider);
                }
            }

            return filteredProviders;
        }

        private bool IsSocialAuthConfigured(Microsoft.AspNetCore.Authentication.AuthenticationScheme scheme)
        {
            switch(scheme.Name)
            {
                case "Microsoft":
                    if(!string.IsNullOrWhiteSpace(userManager.Site.MicrosoftClientId)) { return true; }
                    break;

                case "Google":
                    if (!string.IsNullOrWhiteSpace(userManager.Site.GoogleClientId)) { return true; }
                    break;
                case "Facebook":
                    if (!string.IsNullOrWhiteSpace(userManager.Site.FacebookAppId)) { return true; }
                    break;
                case "Twitter":
                    if (!string.IsNullOrWhiteSpace(userManager.Site.TwitterConsumerKey)) { return true; }
                    break;
                case "OpenIdConnect":
                    if (!string.IsNullOrWhiteSpace(userManager.Site.OidConnectAppId)) { return true; }
                    break;
            }

            return false;
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
