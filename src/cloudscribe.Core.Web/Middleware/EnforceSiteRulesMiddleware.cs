// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-26
// Last Modified:			2018-08-19
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Middleware
{
    public class EnforceSiteRulesMiddleware
    {
        public EnforceSiteRulesMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory
            )
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<EnforceSiteRulesMiddleware>();
        }

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public async Task Invoke(
            HttpContext context,
            SiteContext currentSite,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IUserContextResolver userResolver,
            IAccountService accountService
            )
        {
            var multiTenantOptions = multiTenantOptionsAccessor.Value;
            var folderSegment = "";
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if (!string.IsNullOrWhiteSpace(currentSite.SiteFolderName))
                {
                    folderSegment = "/" + currentSite.SiteFolderName;
                }
            }
            var userContext = await userResolver.GetCurrentUser();

            // enforce SiteRules

            
            if(userContext != null)
            {
                // handle roles changes - basically sets RolesChanged flag to false then sign out and in again to get new roles in cookie
                if (userContext.RolesChanged)
                {
                    await accountService.HandleUserRolesChanged(context.User);
                }

                // handle user still authenticated after lockout 
                if(userContext.IsLockedOut)
                {
                    await accountService.SignOutAsync();
                }

                //handle must change password
                if (userContext.MustChangePwd)
                {
                    var changePasswordUrl = folderSegment + "/manage/changepassword";

                    if (!context.Request.Path.StartsWithSegments(changePasswordUrl))
                    {
                        var logMessage = $"user {userContext.Email} has must change password so redirecting to change password page from requested path {context.Request.Path}";
                        _logger.LogWarning(logMessage);
                        context.Response.Redirect(changePasswordUrl);
                    }
                }


            }
            else
            {
                //userContext is null, make sure user is not signed in, ie if account was deleted but user was currently logged in we need to catch that
                if(context.User.Identity.IsAuthenticated)
                {
                    await accountService.SignOutAsync();
                }
            }

            // handle site closed
            if (currentSite.SiteIsClosed 
                && !context.User.IsInRole("Administrators") 
                && !context.User.IsInRole("Content Administrators")
                )
            {           
                var closedUrl = folderSegment + "/closed";
                // not redirecting for account urls because admin needs to be able to login to unclose the site
                if(
                    (!context.Request.Path.StartsWithSegments(closedUrl))
                    && (!context.Request.Path.StartsWithSegments(folderSegment + "/account")) 
                    )
                {
                    var logMessage = $"site closed so redirecting to closed for requested path {context.Request.Path}";
                    _logger.LogWarning(logMessage);
                    context.Response.Redirect(closedUrl);

                } 
            }

            // handle must agree to terms
            if(userContext != null
               && (!string.IsNullOrWhiteSpace(currentSite.RegistrationAgreement))
               && (userContext.AgreementAcceptedUtc == null || userContext.AgreementAcceptedUtc < currentSite.TermsUpdatedUtc)
                && !context.User.IsInRole("Administrators")
                && !context.User.IsInRole("Content Administrators")
                )
            {
                var agreementUrl = folderSegment + "/account/termsofuse";

                if (
                    (!context.Request.Path.StartsWithSegments(agreementUrl))
                    && (!context.Request.Path.StartsWithSegments("/oops/error"))
                    && (!context.Request.Path.StartsWithSegments("/account/logoff"))
                    && (!context.Request.Path.Value.EndsWith("css.map")) //css.map
                    )
                {
                    var logMessage = $"user {userContext.Email} has not accepted terms of use so redirecting to terms of use acceptance page from requested path {context.Request.Path}";
                    _logger.LogWarning(logMessage);
                    context.Response.Redirect(agreementUrl);
                }
                
            }


            await _next(context);

        }

    }
}
