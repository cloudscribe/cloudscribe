// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-27
// Last Modified:		    2015-07-31
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
            ICookieAuthenticationSchemeSet schemeSet,
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
            this.schemeSet = schemeSet;


        }

        private SiteUserManager<TUser> UserManager { get; set; }
        private HttpContext context;
        private ICookieAuthenticationSchemeSet schemeSet;

        //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/SignInManager.cs

        //here we need to override the authenticationscheme per site 
        
        public override async Task SignInAsync(TUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
        {
            var userPrincipal = await CreateUserPrincipalAsync(user);
            // Review: should we guard against CreateUserPrincipal returning null?
            if (authenticationMethod != null)
            {
                userPrincipal.Identities.First().AddClaim(new Claim(ClaimTypes.AuthenticationMethod, authenticationMethod));
            }
            await context.Authentication.SignInAsync(schemeSet.ApplicationScheme,
                userPrincipal,
                authenticationProperties ?? new AuthenticationProperties());
        }

        public override async Task RefreshSignInAsync(TUser user)
        {
            var auth = new AuthenticateContext(schemeSet.ApplicationScheme);
            await context.Authentication.AuthenticateAsync(auth);
            var authenticationMethod = auth.Principal?.FindFirstValue(ClaimTypes.AuthenticationMethod);
            await SignInAsync(user, new AuthenticationProperties(auth.Properties), authenticationMethod);
        }

        public override async Task SignOutAsync()
        {
            await context.Authentication.SignOutAsync(schemeSet.ApplicationScheme);
            await context.Authentication.SignOutAsync(schemeSet.ExternalScheme);
            await context.Authentication.SignOutAsync(schemeSet.TwoFactorUserIdScheme);
        }

        public override async Task<bool> IsTwoFactorClientRememberedAsync(TUser user)
        {
            var userId = await UserManager.GetUserIdAsync(user);
            var result = await context.Authentication.AuthenticateAsync(schemeSet.TwoFactorRememberMeScheme);
            return (result != null && result.FindFirstValue(ClaimTypes.Name) == userId);
        }

        public override async Task RememberTwoFactorClientAsync(TUser user)
        {
            var userId = await UserManager.GetUserIdAsync(user);
            var rememberBrowserIdentity = new ClaimsIdentity(schemeSet.TwoFactorRememberMeScheme);
            rememberBrowserIdentity.AddClaim(new Claim(ClaimTypes.Name, userId));
            await context.Authentication.SignInAsync(schemeSet.TwoFactorRememberMeScheme,
                new ClaimsPrincipal(rememberBrowserIdentity),
                new AuthenticationProperties { IsPersistent = true });
        }

        public override Task ForgetTwoFactorClientAsync()
        {
            return context.Authentication.SignOutAsync(schemeSet.TwoFactorRememberMeScheme);
        }

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
