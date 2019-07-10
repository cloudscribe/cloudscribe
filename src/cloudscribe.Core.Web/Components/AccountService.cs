// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-22
// Last Modified:			2019-04-22
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
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.Web.Components
{
    public class AccountService : IAccountService
    {
        public AccountService(
            SiteUserManager<SiteUser> userManager,
            SignInManager<SiteUser> signInManager,
            IIdentityServerIntegration identityServerIntegration,
            ISocialAuthEmailVerfificationPolicy socialAuthEmailVerificationPolicy,
            ILdapHelper ldapHelper,
            IUserCommands userCommands,
            IProcessAccountLoginRules loginRulesProcessor,
            INewUserDisplayNameResolver displayNameResolver,
            IOptions<CustomSocialAuthSchemes> customSchemesAccessor
            //,ILogger<AccountService> logger
            )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            IdentityServerIntegration = identityServerIntegration;
            SocialAuthEmailVerificationPolicy = socialAuthEmailVerificationPolicy;
            LoginRulesProcessor = loginRulesProcessor;
            DisplayNameResolver = displayNameResolver;
            CustomSocialAuthSchemes = customSchemesAccessor.Value;
            LdapHelper = ldapHelper;
            UserCommands = userCommands;

        }

        protected SiteUserManager<SiteUser> UserManager { get; private set; }
        protected SignInManager<SiteUser> SignInManager { get; private set; }
        protected IIdentityServerIntegration IdentityServerIntegration { get; private set; }
        protected ISocialAuthEmailVerfificationPolicy SocialAuthEmailVerificationPolicy { get; private set; }
        protected ILdapHelper LdapHelper { get; private set; }
        protected IProcessAccountLoginRules LoginRulesProcessor { get; private set; }
        protected INewUserDisplayNameResolver DisplayNameResolver { get; private set; }
        protected CustomSocialAuthSchemes CustomSocialAuthSchemes { get; private set; }
        protected IUserCommands UserCommands { get; private set; }

        protected virtual async Task<SiteUser> CreateUserFromExternalLogin(
            ExternalLoginInfo externalLoginInfo, 
            string providedEmail = null,
            bool? didAcceptTerms = null
            )
        {
            var email = providedEmail;
            if (string.IsNullOrWhiteSpace(email))
            {
                email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                if(string.IsNullOrWhiteSpace(email))
                {
                    email = externalLoginInfo.Principal.FindFirstValue("email");
                }
            }

            DateTime? termsAcceptedDate = null;
            if (didAcceptTerms == true && !string.IsNullOrWhiteSpace(UserManager.Site.RegistrationAgreement)) { termsAcceptedDate = DateTime.UtcNow; }

            if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
            {
                var userName = await UserManager.SuggestLoginNameFromEmail(UserManager.Site.Id, email);
                var newUser = new SiteUser
                {
                    SiteId = UserManager.Site.Id,
                    UserName = userName,
                    Email = email,
                    FirstName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName),
                    LastName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Surname),
                    AccountApproved = UserManager.Site.RequireApprovalBeforeLogin ? false : true,
                    EmailConfirmed = SocialAuthEmailVerificationPolicy.HasVerifiedEmail(externalLoginInfo),
                    AgreementAcceptedUtc = termsAcceptedDate,
                    LastLoginUtc = DateTime.UtcNow
                };
                //https://github.com/joeaudette/cloudscribe/issues/346
                newUser.DisplayName = DisplayNameResolver.ResolveDisplayName(newUser);

                var identityResult = await UserManager.CreateAsync(newUser);
                if (identityResult.Succeeded)
                {
                    identityResult = await UserManager.AddLoginAsync(newUser, externalLoginInfo);
                    return newUser;
                }
            }
            return null;
        }

        public virtual async Task<bool> IsExistingAccount(string email)
        {
            if(string.IsNullOrWhiteSpace(email)) { return false; }
            var user = await UserManager.FindByNameAsync(email);
            if(user != null) { return true; }
            return false;
        }

        
        public virtual async Task<UserLoginResult> TryExternalLogin(string providedEmail = "", bool? didAcceptTerms = null)
        {
            var template = new LoginResultTemplate();
            IUserContext userContext = null;
            var email = providedEmail;
            
            template.ExternalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (template.ExternalLoginInfo == null)
            {
                template.RejectReasons.Add("signInManager.GetExternalLoginInfoAsync returned null");
            }
            else
            {
                template.User = await UserManager.FindByLoginAsync(template.ExternalLoginInfo.LoginProvider, template.ExternalLoginInfo.ProviderKey);
                
                if (template.User == null)
                {
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        var emailClaim = template.ExternalLoginInfo.Principal.Claims.Where(x => x.Type == ClaimTypes.Email || x.Type == "email").FirstOrDefault();
                        if(emailClaim != null)
                        {
                            email = emailClaim.Value;
                        }
                    }
                    
                    if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
                    {
                        template.User = await UserManager.FindByNameAsync(email);   
                    }

                    if (template.User == null)
                    {
                        template.IsNewUserRegistration = true;
                        template.User = await CreateUserFromExternalLogin(template.ExternalLoginInfo, email, didAcceptTerms);
                    }

                    if (template.User != null)
                    {
                        var identityResult = await UserManager.AddLoginAsync(template.User, template.ExternalLoginInfo);
                    }

                }
                  
            }
 
            if (template.User != null)
            {
                //this will get persisted if login succeeds
                template.User.BrowserKey = Guid.NewGuid().ToString();
                if(UserManager.Site.SingleBrowserSessions)
                {
                    // need to save here because the signin code below looks up the user again using provider info
                    // and creates the claims principal
                    // and we need it to see the updated browserkey to set the claim
                    await UserManager.UpdateAsync(template.User);
                }
                
                await LoginRulesProcessor.ProcessAccountLoginRules(template);
            }
            
            if (template.SignInResult == SignInResult.Failed && template.User != null && template.RejectReasons.Count == 0)
            {
                var updatTokenResult = await SignInManager.UpdateExternalAuthenticationTokensAsync(template.ExternalLoginInfo);
                //var accessToken = template.ExternalLoginInfo.AuthenticationTokens.Where(x => x.Name == "access_token").FirstOrDefault();


                template.SignInResult = await SignInManager.ExternalLoginSignInAsync(template.ExternalLoginInfo.LoginProvider, template.ExternalLoginInfo.ProviderKey, isPersistent: false);

                

                if (template.SignInResult.Succeeded)
                {
                    //update last login time and browser key set above
                    template.User.LastLoginUtc = DateTime.UtcNow;
                    await UserManager.UpdateAsync(template.User);

                }      
            }

            if(template.User != null
                && template.SignInResult != SignInResult.Success 
                && template.SignInResult != SignInResult.TwoFactorRequired)
            {
                //clear the external login 
                await SignInManager.SignOutAsync();
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
        
        public virtual async Task<UserLoginResult> TryLogin(LoginViewModel model)
        {
            var template = new LoginResultTemplate();
            IUserContext userContext = null;
            LdapUser ldapUser = null;
            var isFakeLdapEmail = false;
            
            if(UserManager.Site.UseEmailForLogin && !string.IsNullOrWhiteSpace(model.UserName) && model.UserName.IndexOf("@") > -1)
            {
                template.User = await UserManager.FindByEmailAsync(model.UserName);
            }

            if(template.User == null)
            {
                template.User = await UserManager.FindByNameAsync(model.UserName);  
            }

            if (template.User == null || string.IsNullOrWhiteSpace(template.User.PasswordHash)) //no password on cloudscribe user so could be ldap
            {
                if (LdapHelper.IsImplemented && !string.IsNullOrWhiteSpace(UserManager.Site.LdapServer) && !string.IsNullOrWhiteSpace(UserManager.Site.LdapDomain))
                {
                    ldapUser = await LdapHelper.TryLdapLogin(UserManager.Site as ILdapSettings, model.UserName, model.Password);
                }
            }

            if (ldapUser != null) //ldap auth success
            {
                if(template.User == null)
                {
                    //ldap auth success but no siteuser exists so create one and sign in
                    var cloudscribeUser = new SiteUser()
                    {
                        SiteId = UserManager.Site.Id,
                        UserName = model.UserName,
                        Email = ldapUser.Email,
                        DisplayName = ldapUser.CommonName,
                        FirstName = ldapUser.FirstName,
                        LastName = ldapUser.LastName,
                        LastLoginUtc = DateTime.UtcNow,
                        AccountApproved = true
                    };

                    if (string.IsNullOrWhiteSpace(cloudscribeUser.DisplayName))
                    {
                        cloudscribeUser.DisplayName = model.UserName;
                    }

                    if (string.IsNullOrWhiteSpace(cloudscribeUser.Email))
                    {
                        // identity doesn't allow create user with no email so fake it here then null it out below after sign in. 
                        // the cloudscribe site rules middleware will then force the user to provide an email
                        cloudscribeUser.Email = model.UserName + "@fake-email.com";
                        isFakeLdapEmail = true;
                    }

                    var createdResult = await UserManager.CreateAsync(cloudscribeUser);
                    if (createdResult.Succeeded)
                    {
                        template.User = cloudscribeUser;
                        await SignInManager.SignInAsync(cloudscribeUser, model.RememberMe);
                        template.SignInResult = SignInResult.Success;
                        if (isFakeLdapEmail)
                        {
                            // clear the fake email, the user should then be forced to provide an email by site rules middleware
                            cloudscribeUser.Email = null;
                            cloudscribeUser.NormalizedEmail = null;
                            await UserCommands.Update(cloudscribeUser);
                        }

                    }

                }
                else
                {
                    //siteuser already created for ldap user so just sign in
                    await SignInManager.SignInAsync(template.User, model.RememberMe);
                    template.SignInResult = SignInResult.Success;

                }
                
            }
           
            if (template.User != null && ldapUser == null) //these rules don't apply for ldap users
            {
                await LoginRulesProcessor.ProcessAccountLoginRules(template);
            }

            if(template.User != null)
            {
                //this will get persisted if login succeeds
                template.User.BrowserKey = Guid.NewGuid().ToString();

                userContext = new UserContext(template.User);
            }
           
            if(userContext != null 
                && template.SignInResult == SignInResult.Failed 
                &&  template.RejectReasons.Count == 0)
            {
                var persistent = false;
                if (UserManager.Site.AllowPersistentLogin)
                {
                    persistent = model.RememberMe;
                }

                //template.SignInResult = await SignInManager.PasswordSignInAsync(
                //    model.UserName,
                //    model.Password,
                //    persistent,
                //    lockoutOnFailure: false);

                template.SignInResult = await SignInManager.PasswordSignInAsync(
                    template.User,
                    model.Password,
                    persistent,
                    lockoutOnFailure: false);


                if (template.SignInResult.Succeeded)
                {
                    //update last login time
                    template.User.LastLoginUtc = DateTime.UtcNow;

                    if (string.IsNullOrEmpty(template.User.SecurityStamp))
                    {
                        // if security stamp is empty then the securitystamp validation
                        // fails when it checks after 30 minutes
                        // users created via usermanager this gets populated but not
                        // populated for the admin user created by seeding data
                        // changes to the user such as password change also will populate it
                        // but we can go ahead and check here and populate it if it is empty
                        await UserManager.UpdateSecurityStampAsync(template.User);

                        if (template.User.PasswordHash == "admin||0")
                        {
                            // initial admin user has not updated the password, need to hash it
                            await UserManager.ChangeUserPassword(template.User, "admin", validatePassword: false);

                            await SignInManager.SignOutAsync();
                            // security stamp needs to be there before authentication to avoid the problem
                            
                            template.SignInResult = await SignInManager.PasswordSignInAsync(
                                model.UserName,
                                model.Password,
                                persistent,
                                lockoutOnFailure: false);
                            
                        }

                    }
                    else
                    {
                        await UserManager.UpdateAsync(template.User);
                    }
  
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
            template.User = await SignInManager.GetTwoFactorAuthenticationUserAsync();

            if (template.User != null)
            {
                await LoginRulesProcessor.ProcessAccountLoginRules(template);
            }

            if (template.User != null)
            {
                //this will get persisted if login succeeds
                template.User.BrowserKey = Guid.NewGuid().ToString();
                userContext = new UserContext(template.User);
            }
            
            if(userContext != null
                && template.SignInResult == SignInResult.Failed //initial state
                && template.RejectReasons.Count == 0
                )
            {
                var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
                template.SignInResult = await SignInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);
            }
            
            if (template.SignInResult.Succeeded)
            {
                //update last login time and browser key
                template.User.LastLoginUtc = DateTime.UtcNow;
                await UserManager.UpdateAsync(template.User);

                if(UserManager.Site.SingleBrowserSessions)
                {
                    //the sign in we just did won't have the new browserkey claim so sign out and sign in again to ensure it gets the claim
                    await SignInManager.SignOutAsync();
                    await SignInManager.SignInAsync(template.User, isPersistent: rememberMe);
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

        public virtual async Task<UserLoginResult> TryLoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model)
        {
            var template = new LoginResultTemplate();
            IUserContext userContext = null;
            template.User = await SignInManager.GetTwoFactorAuthenticationUserAsync();

            if (template.User != null)
            {
                await LoginRulesProcessor.ProcessAccountLoginRules(template);
            }

            if (template.User != null)
            {
                //this will get persisted if login succeeds
                template.User.BrowserKey = Guid.NewGuid().ToString();
                userContext = new UserContext(template.User);
            }

            if (userContext != null
                && template.SignInResult == SignInResult.Failed //initial state
                && template.RejectReasons.Count == 0
                )
            {
                var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);
                template.SignInResult = await SignInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
            }

            if(template.SignInResult.Succeeded)
            {
                //update last login time and browser key
                template.User.LastLoginUtc = DateTime.UtcNow;
                await UserManager.UpdateAsync(template.User);

                if (UserManager.Site.SingleBrowserSessions)
                {
                    //the sign in we just did won't have the new browserkey claim so sign out and sign in again to ensure it gets the claim
                    await SignInManager.SignOutAsync();
                    await SignInManager.SignInAsync(template.User, isPersistent: false);
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



        public virtual async Task<UserLoginResult> TryRegister(
            RegisterViewModel model, 
            ModelStateDictionary modelState,
            HttpContext httpContext,
            IHandleCustomRegistration customRegistration
            )
        {
            var template = new LoginResultTemplate();
            IUserContext userContext = null;

            var userName = !string.IsNullOrWhiteSpace(model.Username) ? model.Username : await UserManager.SuggestLoginNameFromEmail(UserManager.Site.Id, model.Email);
            var userNameAvailable = await UserManager.LoginIsAvailable(Guid.Empty, userName);
            if (!userNameAvailable)
            {
                userName = await UserManager.SuggestLoginNameFromEmail(UserManager.Site.Id, model.Email);
            }
            
            var user = new SiteUser
            {
                SiteId = UserManager.Site.Id,
                UserName = userName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DisplayName = model.DisplayName,
                LastLoginUtc = DateTime.UtcNow,
                BrowserKey = Guid.NewGuid().ToString(),
                AccountApproved = UserManager.Site.RequireApprovalBeforeLogin ? false : true
            };

            await customRegistration.ProcessUserBeforeCreate(user, httpContext);
            

            if (model.DateOfBirth.HasValue)
            {
                user.DateOfBirth = model.DateOfBirth.Value;
            }

            if (!string.IsNullOrWhiteSpace(UserManager.Site.RegistrationAgreement))
            {
                if (model.AgreeToTerms)
                {
                    user.AgreementAcceptedUtc = DateTime.UtcNow;
                }
            }
            
            var result = await UserManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                template.User = user;
                template.IsNewUserRegistration = true;
                await LoginRulesProcessor.ProcessAccountLoginRules(template);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    if(!string.IsNullOrWhiteSpace(error.Description) && error.Description.IndexOf("Email") > -1 && error.Description.IndexOf("is already taken") > -1 )
                    {
                        //asp identity is returning an error message like "Email someaddress@somedomain is alreaady taken"
                        // this is account disclosure and we don't want that so return a more generic error message
                        //modelState.AddModelError(string.Empty, "Provided email address not accepted, please try again with a different email address.");
                        // even the above message would give a clue so don't add anything, the user still sees message "Invalid registration attempt."
                    }
                    else
                    {
                        modelState.AddModelError(string.Empty, error.Description);
                    }
                    
                }
            }
           

            if(template.RejectReasons.Count == 0 
                && user != null 
                && template.SignInResult == SignInResult.Failed // failed is initial state, could have been changed to lockedout
                && result.Errors.Count<IdentityError>() == 0
                ) 
            {
                await SignInManager.SignInAsync(user, isPersistent: false);
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

        public virtual async Task<ResetPasswordInfo> GetPasswordResetInfo(string email)
        {
            IUserContext userContext = null;
            string token = null;

            var user = await UserManager.FindByNameAsync(email);
            if(user != null)
            {
                token = await UserManager.GeneratePasswordResetTokenAsync(user);
                userContext = new UserContext(user);
            }

            return new ResetPasswordInfo(userContext, token);
        }

        public virtual async Task<ResetPasswordResult> ResetPassword(string email, string password, string resetCode)
        {
            IUserContext userContext = null;
            IdentityResult result = IdentityResult.Failed(null);

            var user = await UserManager.FindByNameAsync(email);
            if (user != null)
            {
                userContext = new UserContext(user);
                result = await UserManager.ResetPasswordAsync(user, resetCode, password);
            }

            return new ResetPasswordResult(userContext, result);
        }

        public virtual async Task<VerifyEmailInfo> GetEmailVerificationInfo(Guid userId)
        {
            IUserContext userContext = null;
            string token = null;
            var user = await UserManager.Fetch(UserManager.Site.Id, userId);
            if(user != null)
            {
                token = await UserManager.GenerateEmailConfirmationTokenAsync((SiteUser)user);
                userContext = new UserContext(user);
            }

            return new VerifyEmailInfo(userContext, token);
        }

        public virtual async Task<VerifyEmailResult> ConfirmEmailAsync(string userId, string code)
        {
            IUserContext userContext = null;
            IdentityResult result = IdentityResult.Failed(null);

            var user = await UserManager.FindByIdAsync(userId);
            if(user != null)
            {
                userContext = new UserContext(user);
                result = await UserManager.ConfirmEmailAsync(user, code);
            }

            return new VerifyEmailResult(userContext, result);
        }

        public virtual async Task<IUserContext> GetTwoFactorAuthenticationUserAsync()
        {
            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if(user != null)
            {
                return new UserContext(user);
            }

            return null;
        }

        public virtual async Task<TwoFactorInfo> GetTwoFactorInfo(string provider = null)
        {
            IUserContext userContext = null;
            IList<string> userFactors = new List<string>();
            string token = null;
            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if(user != null)
            {
                if (!string.IsNullOrWhiteSpace(provider))
                {
                    token = await UserManager.GenerateTwoFactorTokenAsync(user, provider);
                }
                userContext = new UserContext(user);
                userFactors = await UserManager.GetValidTwoFactorProvidersAsync(user);
            }

            return new TwoFactorInfo(userContext, userFactors, token);
        }

        public virtual async Task HandleUserRolesChanged(ClaimsPrincipal principal)
        {
            if (principal == null) return;
            var userId = principal.GetUserId();
            if (string.IsNullOrEmpty(userId)) return;
            var user = await UserManager.FindByIdAsync(userId);
            await SignInManager.SignOutAsync();
            if (user != null)
            {
                user.RolesChanged = false;
                var result = await UserManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                }
            }
            
        }

        public virtual async Task<bool> AcceptRegistrationAgreement(ClaimsPrincipal principal)
        {
            if (principal == null) return false;
            var userId = principal.GetUserId();
            if (string.IsNullOrEmpty(userId)) return false;
            var user = await UserManager.FindByIdAsync(userId);
            user.AgreementAcceptedUtc = DateTime.UtcNow;
            var result = await UserManager.UpdateAsync(user);
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

        public virtual async Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool rememberMe, bool rememberBrowser)
        {
            return await SignInManager.TwoFactorSignInAsync(provider, code, rememberMe, rememberBrowser);
        }

        public virtual async Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool rememberMe, bool rememberBrowser)
        {
            return await SignInManager.TwoFactorAuthenticatorSignInAsync(code, rememberMe, rememberBrowser);
        }

        public virtual async Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string code)
        {
            return await SignInManager.TwoFactorRecoveryCodeSignInAsync(code);
        }

        public virtual AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string returnUrl = null)
        {
            return SignInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
        }

        public virtual async Task<List<Microsoft.AspNetCore.Authentication.AuthenticationScheme>> GetExternalAuthenticationSchemes()
        {
            var result = await SignInManager.GetExternalAuthenticationSchemesAsync();
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

        protected virtual bool IsSocialAuthConfigured(Microsoft.AspNetCore.Authentication.AuthenticationScheme scheme)
        {
            if(CustomSocialAuthSchemes.Schemes.Contains(scheme.Name))
            {
                return true;
            }

            switch(scheme.Name)
            {
                case "Microsoft":
                    if(!string.IsNullOrWhiteSpace(UserManager.Site.MicrosoftClientId)) { return true; }
                    break;

                case "Google":
                    if (!string.IsNullOrWhiteSpace(UserManager.Site.GoogleClientId)) { return true; }
                    break;
                case "Facebook":
                    if (!string.IsNullOrWhiteSpace(UserManager.Site.FacebookAppId)) { return true; }
                    break;
                case "Twitter":
                    if (!string.IsNullOrWhiteSpace(UserManager.Site.TwitterConsumerKey)) { return true; }
                    break;
                case "OpenIdConnect":
                    if (!string.IsNullOrWhiteSpace(UserManager.Site.OidConnectAppId)) { return true; }
                    break;
               
                    
            }

            return false;
        }

        public bool IsSignedIn(ClaimsPrincipal user)
        {
            return SignInManager.IsSignedIn(user);
        }

        public virtual async Task SignOutAsync()
        {
            await SignInManager.SignOutAsync();
        }

        public virtual async Task<bool> LoginNameIsAvailable(Guid userId, string loginName)
        {
            return await UserManager.LoginIsAvailable(userId, loginName);
        }

    }
}
