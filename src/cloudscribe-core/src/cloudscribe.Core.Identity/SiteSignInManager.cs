// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-27
// Last Modified:		    2015-08-02
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
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
            MultiTenantCookieOptionsResolver tenantResolver,
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
            this.context = contextAccessor.HttpContext;
            this.tenantResolver = tenantResolver;
       
        }

        private SiteUserManager<TUser> UserManager { get; set; }
        private HttpContext context;
        private MultiTenantCookieOptionsResolver tenantResolver;
        

        //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/SignInManager.cs

        //here we need to override the authenticationscheme per site 

        public override async Task SignInAsync(TUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
        {
            var userPrincipal = await CreateUserPrincipalAsync(user);

            if (authenticationMethod != null)
            {
                userPrincipal.Identities.First().AddClaim(new Claim(ClaimTypes.AuthenticationMethod, authenticationMethod));
            }
            await context.Authentication.SignInAsync(AuthenticationScheme.Application,
                userPrincipal,
                authenticationProperties ?? new AuthenticationProperties());
        }

        public override async Task RefreshSignInAsync(TUser user)
        {
            var auth = new AuthenticateContext(tenantResolver.ResolveAuthScheme(AuthenticationScheme.Application));
            await context.Authentication.AuthenticateAsync(auth);
            var authenticationMethod = auth.Principal?.FindFirstValue(ClaimTypes.AuthenticationMethod);
            await SignInAsync(user, new AuthenticationProperties(auth.Properties), authenticationMethod);
        }

        public override async Task SignOutAsync()
        {
            await context.Authentication.SignOutAsync(AuthenticationScheme.Application);
            await context.Authentication.SignOutAsync(AuthenticationScheme.External);
            await context.Authentication.SignOutAsync(AuthenticationScheme.TwoFactorUserId);
        }

        public override async Task<bool> IsTwoFactorClientRememberedAsync(TUser user)
        {
            var userId = await UserManager.GetUserIdAsync(user);
            var result = await context.Authentication.AuthenticateAsync(AuthenticationScheme.TwoFactorRememberMe);
            return (result != null && result.FindFirstValue(ClaimTypes.Name) == userId);
        }

        public override async Task RememberTwoFactorClientAsync(TUser user)
        {
            var userId = await UserManager.GetUserIdAsync(user);
            var rememberBrowserIdentity = new ClaimsIdentity(tenantResolver.ResolveAuthScheme(AuthenticationScheme.TwoFactorRememberMe));
            rememberBrowserIdentity.AddClaim(new Claim(ClaimTypes.Name, userId));

            await context.Authentication.SignInAsync(AuthenticationScheme.TwoFactorRememberMe,
                new ClaimsPrincipal(rememberBrowserIdentity),
                new AuthenticationProperties { IsPersistent = true });
        }

        public override Task ForgetTwoFactorClientAsync()
        {
            return context.Authentication.SignOutAsync(AuthenticationScheme.TwoFactorRememberMe);
        }

        public override IEnumerable<AuthenticationDescription> GetExternalAuthenticationSchemes()
        {
            //https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNet.Http.Abstractions/Authentication/AuthenticationManager.cs
            //https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNet.Http/Authentication/DefaultAuthenticationManager.cs

            //return context.Authentication.GetAuthenticationSchemes().Where(d => !string.IsNullOrEmpty(d.Caption));
            return context.Authentication.GetAuthenticationSchemes();
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
            //var auth = new AuthenticateContext(IdentityOptions.ExternalCookieAuthenticationScheme);
            var auth = new AuthenticateContext(AuthenticationScheme.External);

            await context.Authentication.AuthenticateAsync(auth);
            if (auth.Principal == null || auth.Properties == null || !auth.Properties.ContainsKey(LoginProviderKey))
            {
                return null;
            }

            if (expectedXsrf != null)
            {
                if (!auth.Properties.ContainsKey(XsrfKey))
                {
                    return null;
                }
                var userId = auth.Properties[XsrfKey] as string;
                if (userId != expectedXsrf)
                {
                    return null;
                }
            }

            var providerKey = auth.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var provider = auth.Properties[LoginProviderKey] as string;
            if (providerKey == null || provider == null)
            {
                return null;
            }
            // REVIEW: fix this wrap
            return new ExternalLoginInfo(auth.Principal, provider, providerKey, new AuthenticationDescription(auth.Description).Caption);
        }

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

        //public override async Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent,
        //    bool rememberClient)
        //{
        //    var twoFactorInfo = await RetrieveTwoFactorInfoAsync();
        //    if (twoFactorInfo == null || twoFactorInfo.UserId == null)
        //    {
        //        return SignInResult.Failed;
        //    }
        //    var user = await UserManager.FindByIdAsync(twoFactorInfo.UserId);
        //    if (user == null)
        //    {
        //        return SignInResult.Failed;
        //    }

        //    var error = await PreSignInCheck(user);
        //    if (error != null)
        //    {
        //        return error;
        //    }
        //    if (await UserManager.VerifyTwoFactorTokenAsync(user, provider, code))
        //    {
        //        // When token is verified correctly, clear the access failed count used for lockout
        //        await ResetLockout(user);
        //        // Cleanup external cookie
        //        if (twoFactorInfo.LoginProvider != null)
        //        {
        //            await Context.Authentication.SignOutAsync(IdentityOptions.ExternalCookieAuthenticationScheme);
        //        }
        //        if (rememberClient)
        //        {
        //            await RememberTwoFactorClientAsync(user);
        //        }
        //        await UserManager.ResetAccessFailedCountAsync(user);
        //        await SignInAsync(user, isPersistent, twoFactorInfo.LoginProvider);
        //        return SignInResult.Success;
        //    }
        //    // If the token is incorrect, record the failure which also may cause the user to be locked out
        //    await UserManager.AccessFailedAsync(user);
        //    return SignInResult.Failed;
        //}


    }
}
