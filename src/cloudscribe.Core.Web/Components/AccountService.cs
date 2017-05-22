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

        public async Task<UserLoginResult> TryLogin(LoginViewModel model)
        {
            var signinResult = SignInResult.Failed;
            SiteUser user = null;
            IUserContext userContext = null;
            List<string> rejectReasons = new List<string>();

            if (userManager.Site.RequireConfirmedEmail || userManager.Site.RequireApprovalBeforeLogin)
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

                    if ((user.IsLockedOut) || (user.IsDeleted))
                    {  
                        var reason = $"login not allowed for {user.Email} because account either locked out or flagged as deleted";
                        rejectReasons.Add(reason);
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
            

            return new UserLoginResult(signinResult, rejectReasons, userContext);

        }


    }
}
