// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-26
// Last Modified:			2018-08-19
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Web.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
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

        /// <summary>
        /// we don't want to execute this for js or css or images
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsExcludedPath(HttpRequest request)
        {
            if (request.IsAjaxRequest()) return true;

            var path = request.Path.ToString();
            if (path.Contains("/pwa/init")) return true;
            if (path.Contains("/serviceworker")) return true;
            if (path.EndsWith(".css")) return true;
            if (path.EndsWith(".js")) return true;
            if (path.EndsWith(".jpeg")) return true;
            if (path.EndsWith(".jpg")) return true;
            if (path.EndsWith(".gif")) return true;
            if (path.EndsWith(".png")) return true;

            return false;
        }

        public async Task Invoke(
            HttpContext context,
            SiteContext currentSite,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IUserContextResolver userResolver,
            IAccountService accountService
            )
        {
            var isExcludedPath = IsExcludedPath(context.Request);

            if(!isExcludedPath)
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

                //userContext is null, make sure user is not signed in, ie if account was deleted but user was currently logged in we need to catch that
                if (userContext == null && context.User.Identity.IsAuthenticated)
                {
                    await accountService.SignOutAsync();
                }

                // handle roles changes - basically sets RolesChanged flag to false then sign out and in again to get new roles in cookie
                if (userContext != null && userContext.RolesChanged)
                {
                    await accountService.HandleUserRolesChanged(context.User);
                }

                // handle user still authenticated after lockout 
                if (userContext != null && userContext.IsLockedOut)
                {
                    await accountService.SignOutAsync();
                }

                if (userContext != null && currentSite.SingleBrowserSessions && !string.IsNullOrWhiteSpace(userContext.BrowserKey))
                {
                    var browserKeyClaim = context.User.Claims.Where(x => x.Type == "browser-key").FirstOrDefault();
                    if (browserKeyClaim == null || browserKeyClaim.Value != userContext.BrowserKey)
                    {
                        var logMessage = $"user {userContext.Email} BrowserKey doesn't match claim so signing user out";
                        _logger.LogWarning(logMessage);
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
                    if (
                        (!context.Request.Path.StartsWithSegments(closedUrl))
                        && (!context.Request.Path.StartsWithSegments(folderSegment + "/account"))
                        )
                    {
                        var logMessage = $"site closed so redirecting to closed for requested path {context.Request.Path}";
                        _logger.LogWarning(logMessage);
                        context.Response.Redirect(closedUrl);

                    }
                }

                else if (userContext != null && string.IsNullOrWhiteSpace(userContext.Email))
                {
                    var setEmailUrl = folderSegment + "/manage/emailrequired";

                    if (!context.Request.Path.StartsWithSegments(setEmailUrl))
                    {
                        var logMessage = $"user {userContext.UserName} has must provide an email adddress so redirecting to email required page from requested path {context.Request.Path}";
                        _logger.LogWarning(logMessage);
                        context.Response.Redirect(setEmailUrl);
                    }
                }

                //handle must change password
                else if (userContext != null && userContext.MustChangePwd)
                {
                    var changePasswordUrl = folderSegment + "/manage/changepassword";

                    if (!context.Request.Path.StartsWithSegments(changePasswordUrl))
                    {
                        var logMessage = $"user {userContext.Email} has must change password so redirecting to change password page from requested path {context.Request.Path}";
                        _logger.LogWarning(logMessage);
                        context.Response.Redirect(changePasswordUrl);
                    }
                }

                else if (userContext != null && currentSite.Require2FA && !userContext.TwoFactorEnabled && !context.User.IsInRole("Administrators"))
                {
                    var twoFactorUrl1 = folderSegment + "/manage/twofactorauthentication";
                    var twoFactorUrl2 = folderSegment + "/manage/enableauthenticator";

                    if (!context.Request.Path.StartsWithSegments(twoFactorUrl1) && !context.Request.Path.StartsWithSegments(twoFactorUrl2))
                    {
                        var logMessage = $"user {userContext.Email} has must setup 2fa so redirecting to 2fa path from requested path {context.Request.Path}";
                        _logger.LogWarning(logMessage);
                        context.Response.Redirect(twoFactorUrl1);
                    }

                }

                // handle must agree to terms
                else if (userContext != null
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

            }
            
            await _next(context);

        }

    }
}
