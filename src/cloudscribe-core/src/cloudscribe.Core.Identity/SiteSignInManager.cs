// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-27
// Last Modified:		    2015-07-27
// 
//

using cloudscribe.Core.Models;
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
            UserManager<TUser> userManager, 
            IHttpContextAccessor contextAccessor,
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



            this.context = contextAccessor.HttpContext;


        }

        private HttpContext context;

        //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity/SignInManager.cs

        //maybe we can override the authenticationscheme per site here

        // 2015-07-27 these are from the latest source code
        // I think beta5 is not in sync
        // looks like beta6 came out today so I will upgrade before moving forward

        //public override async Task SignInAsync(TUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
        //{
        //    var userPrincipal = await CreateUserPrincipalAsync(user);
        //    // Review: should we guard against CreateUserPrincipal returning null?
        //    if (authenticationMethod != null)
        //    {
        //        userPrincipal.Identities.First().AddClaim(new Claim(ClaimTypes.AuthenticationMethod, authenticationMethod));
        //    }
        //    await context.Authentication.SignInAsync(IdentityOptions.ApplicationCookieAuthenticationScheme,
        //        userPrincipal,
        //        authenticationProperties ?? new AuthenticationProperties());
        //}

        //public override async Task RefreshSignInAsync(TUser user)
        //{
        //    var auth = new AuthenticateContext(IdentityOptions.ApplicationCookieAuthenticationScheme);
        //    await context.Authentication.AuthenticateAsync(auth);
        //    var authenticationMethod = auth.Principal?.FindFirstValue(ClaimTypes.AuthenticationMethod);
        //    await SignInAsync(user, new AuthenticationProperties(auth.Properties), authenticationMethod);
        //}

        //public override async Task SignOutAsync()
        //{
        //    await context.Authentication.SignOutAsync(IdentityOptions.ApplicationCookieAuthenticationScheme);
        //    await context.Authentication.SignOutAsync(IdentityOptions.ExternalCookieAuthenticationScheme);
        //    await context.Authentication.SignOutAsync(IdentityOptions.TwoFactorUserIdCookieAuthenticationScheme);
        //}

        //public override async Task<bool> IsTwoFactorClientRememberedAsync(TUser user)
        //{
        //    var userId = await UserManager.GetUserIdAsync(user);
        //    var result = await context.Authentication.AuthenticateAsync(IdentityOptions.TwoFactorRememberMeCookieAuthenticationScheme);
        //    return (result != null && result.FindFirstValue(ClaimTypes.Name) == userId);
        //}

        //public override async Task RememberTwoFactorClientAsync(TUser user)
        //{
        //    var userId = await UserManager.GetUserIdAsync(user);
        //    var rememberBrowserIdentity = new ClaimsIdentity(IdentityOptions.TwoFactorRememberMeCookieAuthenticationType);
        //    rememberBrowserIdentity.AddClaim(new Claim(ClaimTypes.Name, userId));
        //    await context.Authentication.SignInAsync(IdentityOptions.TwoFactorRememberMeCookieAuthenticationScheme,
        //        new ClaimsPrincipal(rememberBrowserIdentity),
        //        new AuthenticationProperties { IsPersistent = true });
        //}

        //public override Task ForgetTwoFactorClientAsync()
        //{
        //    return context.Authentication.SignOutAsync(IdentityOptions.TwoFactorRememberMeCookieAuthenticationScheme);
        //}


    }
}
