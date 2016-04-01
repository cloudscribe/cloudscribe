// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-27
// Last Modified:		    2016-03-04
// 

//TODO: we need to override many or most of the methods of the base class
// https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/SignInManager.cs
// specifically wherever it is using IdentityOptions

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Features.Authentication;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class SiteSignInManager<TUser> : SignInManager<TUser> where TUser : SiteUser
    {
        public SiteSignInManager(
            SiteUserManager<TUser> userManager, 
            IHttpContextAccessor contextAccessor,
            //MultiTenantCookieOptionsResolver tenantResolver,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            ISiteRepository siteRepository,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger)
            :base(userManager, 
                 contextAccessor,
                 claimsFactory,
                 optionsAccessor,
                 logger
                 )
        {

            UserManager = userManager;
            SiteUserManager = userManager;
            this.context = contextAccessor.HttpContext;
           // this.tenantResolver = tenantResolver;
            log = logger;
            multiTenantOptions = multiTenantOptionsAccessor.Value;
            siteRepo = siteRepository;
            Options = optionsAccessor.Value;

        }

        private SiteUserManager<TUser> SiteUserManager { get; set; }
        private HttpContext context;
        //private MultiTenantCookieOptionsResolver tenantResolver;
        private ILogger<SignInManager<TUser>> log;
        private MultiTenantOptions multiTenantOptions;
        private ISiteRepository siteRepo;
        private IdentityOptions Options;


        //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/SignInManager.cs

        //here we need to override the authenticationscheme per site 

        public override async Task SignInAsync(TUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
        {
            log.LogInformation("SignInAsync called");

            var userPrincipal = await CreateUserPrincipalAsync(user);

            if (authenticationMethod != null)
            {
                userPrincipal.Identities.First().AddClaim(new Claim(ClaimTypes.AuthenticationMethod, authenticationMethod));
            }


            //await context.Authentication.SignInAsync(AuthenticationScheme.Application,
            //    userPrincipal,
            //    authenticationProperties ?? new AuthenticationProperties());

            await context.Authentication.SignInAsync(Options.Cookies.ApplicationCookie.AuthenticationScheme,
                userPrincipal,
                authenticationProperties ?? new AuthenticationProperties());
        }

        public override async Task RefreshSignInAsync(TUser user)
        {
            log.LogInformation("SignInAsync called");
            //var auth = new AuthenticateContext(tenantResolver.ResolveAuthScheme(AuthenticationScheme.Application));
            var auth = new AuthenticateContext(Options.Cookies.ApplicationCookie.AuthenticationScheme);
            await context.Authentication.AuthenticateAsync(auth);
            var authenticationMethod = auth.Principal?.FindFirstValue(ClaimTypes.AuthenticationMethod);
            await SignInAsync(user, new AuthenticationProperties(auth.Properties), authenticationMethod);
        }

        public override async Task SignOutAsync()
        {
            log.LogInformation("SignOutAsync called");

            //await context.Authentication.SignOutAsync(AuthenticationScheme.Application);
            //await context.Authentication.SignOutAsync(AuthenticationScheme.External);
            //await context.Authentication.SignOutAsync(AuthenticationScheme.TwoFactorUserId);
            await context.Authentication.SignOutAsync(Options.Cookies.ApplicationCookie.AuthenticationScheme);
            await context.Authentication.SignOutAsync(Options.Cookies.ExternalCookie.AuthenticationScheme);
            await context.Authentication.SignOutAsync(Options.Cookies.TwoFactorUserIdCookie.AuthenticationScheme);
        }

        public override async Task<bool> IsTwoFactorClientRememberedAsync(TUser user)
        {
            log.LogInformation("IsTwoFactorClientRememberedAsync called");

            var userId = await UserManager.GetUserIdAsync(user);
            //var result = await context.Authentication.AuthenticateAsync(AuthenticationScheme.TwoFactorRememberMe);
            var result = await context.Authentication.AuthenticateAsync(Options.Cookies.TwoFactorRememberMeCookie.AuthenticationScheme);


            return (result != null && result.FindFirstValue(ClaimTypes.Name) == userId);
        }

        public override async Task RememberTwoFactorClientAsync(TUser user)
        {
            log.LogInformation("RememberTwoFactorClientAsync called");

            var userId = await UserManager.GetUserIdAsync(user);

            //var rememberBrowserIdentity = new ClaimsIdentity(tenantResolver.ResolveAuthScheme(AuthenticationScheme.TwoFactorRememberMe));
            var rememberBrowserIdentity = new ClaimsIdentity(Options.Cookies.TwoFactorRememberMeCookie.AuthenticationScheme);

            rememberBrowserIdentity.AddClaim(new Claim(ClaimTypes.Name, userId));

            //await context.Authentication.SignInAsync(AuthenticationScheme.TwoFactorRememberMe,
            //    new ClaimsPrincipal(rememberBrowserIdentity),
            //    new AuthenticationProperties { IsPersistent = true });

            await context.Authentication.SignInAsync(Options.Cookies.TwoFactorRememberMeCookie.AuthenticationScheme,
                new ClaimsPrincipal(rememberBrowserIdentity),
                new AuthenticationProperties { IsPersistent = true });
        }

        public override Task ForgetTwoFactorClientAsync()
        {
            log.LogInformation("ForgetTwoFactorClientAsync called");

            //return context.Authentication.SignOutAsync(AuthenticationScheme.TwoFactorRememberMe);
            return context.Authentication.SignOutAsync(Options.Cookies.TwoFactorRememberMeCookie.AuthenticationScheme);
        }

        public override IEnumerable<AuthenticationDescription> GetExternalAuthenticationSchemes()
        {
            //log.LogInformation("GetExternalAuthenticationSchemes called");
            //https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNet.Http.Abstractions/Authentication/AuthenticationManager.cs
            //https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNet.Http/Authentication/DefaultAuthenticationManager.cs

            IEnumerable<AuthenticationDescription> all = context.Authentication.GetAuthenticationSchemes().Where(d => !string.IsNullOrEmpty(d.DisplayName));
            

            if (multiTenantOptions.Mode != MultiTenantMode.None)
            {
                // here we need to filter the list to ones configured for the current tenant
                if(multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if(multiTenantOptions.UseRelatedSitesMode)
                    {
                        ISiteSettings site = siteRepo.FetchNonAsync(multiTenantOptions.RelatedSiteId);

                        return BuildFilteredAuthList(site, all);
                    }

                }

                return BuildFilteredAuthList(SiteUserManager.Site, all);

            }


            return all;
            //return context.Authentication.GetAuthenticationSchemes();
        }

        private IEnumerable<AuthenticationDescription> BuildFilteredAuthList(ISiteSettings site, IEnumerable<AuthenticationDescription> all)
        {
            //log.LogInformation("BuildFilteredAuthList called");

            if (site == null)
            {
                //log.LogInformation("BuildFilteredAuthList returning all because site was null");
                return all;
            }

            List<AuthenticationDescription> filtered = new List<AuthenticationDescription>();

            foreach(AuthenticationDescription authDesc in all)
            {
                //log.LogInformation("BuildFilteredAuthList authDesc.AuthenticationScheme was " + authDesc.AuthenticationScheme);

                switch (authDesc.AuthenticationScheme)
                {
                    case "Microsoft":
                        if ((site.MicrosoftClientId.Length > 0) && (site.MicrosoftClientSecret.Length > 0))
                        {
                            filtered.Add(authDesc);
                        }
                        break;

                    case "Google":
                        if ((site.GoogleClientId.Length > 0) && (site.GoogleClientSecret.Length > 0))
                        {
                            filtered.Add(authDesc);
                        }
                        break;

                    case "Facebook":
                        if((site.FacebookAppId.Length > 0)&& (site.FacebookAppSecret.Length > 0))
                        {
                            filtered.Add(authDesc);
                        }
                        break;

                    case "Twitter":
                        if ((site.TwitterConsumerKey.Length > 0) && (site.TwitterConsumerSecret.Length > 0))
                        {
                            filtered.Add(authDesc);
                        }
                        break;


                }

                
            }

            return filtered;

        }


        private const string LoginProviderKey = "LoginProvider";
        private const string XsrfKey = "XsrfId";
        /// <summary>
        /// Gets the external login information for the current login, as an asynchronous operation.
        /// </summary>
        /// <param name="expectedXsrf">Flag indication whether a Cross Site Request Forgery token was expected in the current request.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="ExternalLoginInfo"/>
        /// for the sign-in attempt.</returns>
        public override async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null)
        {
            log.LogInformation("GetExternalLoginInfoAsync called " + LoginProviderKey);

            //var auth = new AuthenticateContext(IdentityOptions.ExternalCookieAuthenticationScheme);
            //https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNet.Http.Features/Authentication/AuthenticateContext.cs
            //var auth = new AuthenticateContext(AuthenticationScheme.External);

            var auth = new AuthenticateContext(Options.Cookies.ExternalCookie.AuthenticationScheme);
            
            await context.Authentication.AuthenticateAsync(auth);

            if (auth.Principal == null)
            {
                log.LogInformation("GetExternalLoginInfoAsync returning null because auth.Principal was null");
                return null;
            }


            if (auth.Properties == null )
            {
                log.LogInformation("GetExternalLoginInfoAsync returning null because  auth.Properties was null");
                return null;
            }

            if (!auth.Properties.ContainsKey(LoginProviderKey))
            {
                log.LogInformation("GetExternalLoginInfoAsync returning null because loginproviderkey " + LoginProviderKey + " was not in auth.properties");
                return null;
            }

            

            if (expectedXsrf != null)
            {
                if (!auth.Properties.ContainsKey(XsrfKey))
                {
                    log.LogInformation("GetExternalLoginInfoAsync returned null because auth.Properties did not contain XsfKey");
                    return null;
                }
                var userId = auth.Properties[XsrfKey] as string;
                if (userId != expectedXsrf)
                {
                    log.LogInformation("GetExternalLoginInfoAsync returning null because userId != auth.Properties[XsrfKey]");
                    return null;
                }
            }

            var providerKey = auth.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var provider = auth.Properties[LoginProviderKey] as string;
            if (providerKey == null || provider == null)
            {
                log.LogInformation("GetExternalLoginInfoAsync returning null because (providerKey == null || provider == null) ");
                return null;
            }
            // REVIEW: fix this wrap
            return new ExternalLoginInfo(auth.Principal, provider, providerKey, new AuthenticationDescription(auth.Description).DisplayName);
        }

        public override async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent)
        {
            log.LogInformation("ExternalLoginSignInAsync called for " + loginProvider + " with key " + providerKey);

            var user = await UserManager.FindByLoginAsync(loginProvider, providerKey);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            var error = await PreSignInCheck(user);
            if (error != null)
            {
                return error;
            }
            return await SignInOrTwoFactorAsync(user, isPersistent, loginProvider);
        }

        private async Task<SignInResult> PreSignInCheck(TUser user)
        {
            log.LogInformation("PreSignInCheck called");

            if (!await CanSignInAsync(user))
            {
                return SignInResult.NotAllowed;
            }
            if (await IsLockedOut(user))
            {
                return await LockedOut(user);
            }
            return null;
        }

        private async Task<bool> IsLockedOut(TUser user)
        {
            return UserManager.SupportsUserLockout && await UserManager.IsLockedOutAsync(user);
        }

        private async Task<SignInResult> LockedOut(TUser user)
        {
            Logger.LogWarning("User {userId} is currently locked out.", await UserManager.GetUserIdAsync(user));
            return SignInResult.LockedOut;
        }

        private async Task<SignInResult> SignInOrTwoFactorAsync(TUser user, bool isPersistent, string loginProvider = null)
        {
            log.LogDebug("SignInOrTwoFactorAsync called");

            if (UserManager.SupportsUserTwoFactor &&
                await UserManager.GetTwoFactorEnabledAsync(user) &&
                (await UserManager.GetValidTwoFactorProvidersAsync(user)).Count > 0)
            {
                if (!await IsTwoFactorClientRememberedAsync(user))
                {
                    // Store the userId for use after two factor check
                    var userId = await UserManager.GetUserIdAsync(user);
                    //var twoFactorScheme = Options.Cookies.TwoFactorUserIdCookieAuthenticationScheme;
                    // the above is not getting the value we set in startup though the external one below is working 
                    var twoFactorScheme = Options.Cookies.TwoFactorUserIdCookie.AuthenticationScheme;
                    await context.Authentication.SignInAsync(
                        twoFactorScheme, StoreTwoFactorInfo(userId, loginProvider));
                    return SignInResult.TwoFactorRequired;
                }
            }
            // Cleanup external cookie
            if (loginProvider != null)
            {
                //await context.Authentication.SignOutAsync(Options.Cookies.ExternalCookieAuthenticationScheme);
                await context.Authentication.SignOutAsync(Options.Cookies.ExternalCookie.AuthenticationScheme);
            }
            await SignInAsync(user, isPersistent, loginProvider);
            return SignInResult.Success;
        }

        /// <summary>
        /// Gets the<typeparamref name= "TUser" /> for the current two factor authentication login, as an asynchronous operation.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation containing the<typeparamref name="TUser"/>
        /// for the sign-in attempt.</returns>
        public override async Task<TUser> GetTwoFactorAuthenticationUserAsync()
        {
            var info = await RetrieveTwoFactorInfoAsync();
            if (info == null)
            {
                return null;
            }

            return await UserManager.FindByIdAsync(info.UserId);
        }

        private class TwoFactorAuthenticationInfo
        {
            public string UserId { get; set; }
            public string LoginProvider { get; set; }
        };

        private async Task<TwoFactorAuthenticationInfo> RetrieveTwoFactorInfoAsync()
        {
            var result = await context.Authentication.AuthenticateAsync(Options.Cookies.TwoFactorUserIdCookieAuthenticationScheme);
            if (result != null)
            {
                return new TwoFactorAuthenticationInfo
                {
                    UserId = result.FindFirstValue(ClaimTypes.Name),
                    LoginProvider = result.FindFirstValue(ClaimTypes.AuthenticationMethod)
                };
            }
            return null;
        }

        /// <summary>
        /// Attempts to sign in the specified <paramref name="user"/> and <paramref name="password"/> combination
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
        /// for the sign-in attempt.</returns>
        public override async Task<SignInResult> PasswordSignInAsync(TUser user, string password,
            bool isPersistent, bool lockoutOnFailure)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var error = await PreSignInCheck(user);
            if (error != null)
            {
                return error;
            }
            if (await IsLockedOut(user))
            {
                return await LockedOut(user);
            }
            if (await UserManager.CheckPasswordAsync(user, password))
            {
                await ResetLockout(user);
                return await SignInOrTwoFactorAsync(user, isPersistent);
            }
            Logger.LogWarning(2, "User {userId} failed to provide the correct password.", await UserManager.GetUserIdAsync(user));

            if (UserManager.SupportsUserLockout && lockoutOnFailure)
            {
                // If lockout is requested, increment access failed count which might lock out the user
                await UserManager.AccessFailedAsync(user);
                if (await UserManager.IsLockedOutAsync(user))
                {
                    return await LockedOut(user);
                }
            }
            return SignInResult.Failed;
        }

        private Task ResetLockout(TUser user)
        {
            if (UserManager.SupportsUserLockout)
            {
                return UserManager.ResetAccessFailedCountAsync(user);
            }
            return Task.FromResult(0);
        }

        internal ClaimsPrincipal StoreTwoFactorInfo(string userId, string loginProvider)
        {
            var identity = new ClaimsIdentity(Options.Cookies.TwoFactorUserIdCookieAuthenticationScheme);


            identity.AddClaim(new Claim(ClaimTypes.Name, userId));
            if (loginProvider != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, loginProvider));
            }
            return new ClaimsPrincipal(identity);
        }

        public override AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl, string userId = null)
        {
            log.LogInformation("ConfigureExternalAuthenticationProperties called for " + provider + " with redirectUrl " + redirectUrl);

            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            properties.Items[LoginProviderKey] = provider;
            if (userId != null)
            {
                properties.Items[XsrfKey] = userId;
            }
      
            return properties;
        }

        /// <summary>
        /// Validates the security stamp for the specified <paramref name="principal"/> against
        /// the persisted stamp for the <paramref name="userId"/>, as an asynchronous operation.
        /// </summary>
        /// <param name="principal">The principal whose stamp should be validated.</param>
        /// <param name="userId">The ID for the user.</param>
        /// <returns>The task object representing the asynchronous operation. The task will contain the <typeparamref name="TUser"/>
        /// if the stamp matches the persisted value, otherwise it will return false.</returns>
        //public virtual async Task<TUser> ValidateSecurityStampAsync(ClaimsPrincipal principal)
        //{
        //    if (principal == null)
        //    {
        //        return null;
        //    }
        //    var user = await UserManager.GetUserAsync(principal);
        //    if (user != null && UserManager.SupportsUserSecurityStamp)
        //    {
        //        var securityStamp =
        //            principal.FindFirstValue(Options.ClaimsIdentity.SecurityStampClaimType);
        //        if (securityStamp == await UserManager.GetSecurityStampAsync(user))
        //        {
        //            return user;
        //        }
        //    }
        //    return null;
        //}

        //https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNet.Http/Authentication/DefaultAuthenticationManager.cs
        //public override IEnumerable<AuthenticationDescription> GetAuthenticationSchemes()
        //{
        // https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNet.Http/Features/Authentication/HttpAuthenticationFeature.cs

        //    var handler = HttpAuthenticationFeature.Handler;
        //    if (handler == null)
        //    {
        //        return new AuthenticationDescription[0];
        //    }

        //    var describeContext = new DescribeSchemesContext();
        //    handler.GetDescriptions(describeContext);
        //    return describeContext.Results.Select(description => new AuthenticationDescription(description));
        //}

        // https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication/AuthenticationHandler.cs
        //public void GetDescriptions(DescribeSchemesContext describeContext)
        //{
        //    describeContext.Accept(Options.Description.Items);

        //    if (PriorHandler != null)
        //    {
        //        PriorHandler.GetDescriptions(describeContext);
        //    }
        //}

        public override async Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent,
            bool rememberClient)
        {
            var twoFactorInfo = await RetrieveTwoFactorInfoAsync();
            if (twoFactorInfo == null || twoFactorInfo.UserId == null)
            {
                return SignInResult.Failed;
            }
            var user = await UserManager.FindByIdAsync(twoFactorInfo.UserId);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            var error = await PreSignInCheck(user);
            if (error != null)
            {
                return error;
            }
            if (await UserManager.VerifyTwoFactorTokenAsync(user, provider, code))
            {
                // When token is verified correctly, clear the access failed count used for lockout
                await ResetLockout(user);
                // Cleanup external cookie
                if (twoFactorInfo.LoginProvider != null)
                {
                    await context.Authentication.SignOutAsync(Options.Cookies.ExternalCookie.AuthenticationScheme);
                }
                if (rememberClient)
                {
                    await RememberTwoFactorClientAsync(user);
                }
                await UserManager.ResetAccessFailedCountAsync(user);
                await SignInAsync(user, isPersistent, twoFactorInfo.LoginProvider);
                return SignInResult.Success;
            }
            // If the token is incorrect, record the failure which also may cause the user to be locked out
            await UserManager.AccessFailedAsync(user);
            return SignInResult.Failed;
        }


    }
}
