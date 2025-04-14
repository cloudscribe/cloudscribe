﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2024-07-08
// 

using cloudscribe.Common.Gdpr;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using cloudscribe.Web.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers.Mvc
{
    public class ManageController : Controller
    {
        public ManageController(
            SiteContext                       currentSite,
            SiteUserManager<SiteUser>         userManager,
            SignInManager<SiteUser>           signInManager,
            IAccountService                   accountService,
            IStringLocalizer<CloudscribeCore> localizer,
            DateTimeUtils.ITimeZoneIdResolver timeZoneIdResolver,
            DateTimeUtils.ITimeZoneHelper     timeZoneHelper,
            IHandleCustomUserInfo             customUserInfo,
            ILogger<ManageController>         logger,
            UrlEncoder                        urlEncoder,
            ISiteAccountCapabilitiesProvider  siteCapabilities,
            ISiteMessageEmailSender           emailSender,
            IEmailChangeHandler               emailChangeHandler,
            IOptions<MultiTenantOptions>      multiTenantOptionsAccessor,
            SiteManager                       siteManager
            )
        {
            CurrentSite        = currentSite;
            UserManager        = userManager;
            SignInManager      = signInManager;
            AccountService     = accountService;
            StringLocalizer    = localizer;
            TimeZoneIdResolver = timeZoneIdResolver;
            TimeZoneHelper     = timeZoneHelper;
            CustomUserInfo     = customUserInfo;
            Log                = logger;
            UrlEncoder         = urlEncoder;
            SiteCapabilities   = siteCapabilities;
            EmailSender        = emailSender;
            EmailChangeHandler = emailChangeHandler;
            SiteManager        = siteManager;
            MultiTenantOptions = multiTenantOptionsAccessor.Value;
        }

        protected IAccountService                  AccountService     { get; private set; }
        protected ILogger                          Log                { get; private set; }
        protected ISiteContext                     CurrentSite        { get; private set; }
        protected SiteUserManager<SiteUser>        UserManager        { get; private set; }
        protected SignInManager<SiteUser>          SignInManager      { get; private set; }
        protected IStringLocalizer                 StringLocalizer    { get; private set; }
        protected IHandleCustomUserInfo            CustomUserInfo     { get; private set; }
        protected UrlEncoder                       UrlEncoder         { get; private set; }
        protected ISiteAccountCapabilitiesProvider SiteCapabilities   { get; private set; }
        protected ISiteMessageEmailSender          EmailSender        { get; private set; }
        protected IEmailChangeHandler              EmailChangeHandler { get; private set; }
        protected SiteManager                      SiteManager        { get; private set; }
        protected MultiTenantOptions               MultiTenantOptions { get; private set; }


        protected cloudscribe.DateTimeUtils.ITimeZoneIdResolver TimeZoneIdResolver { get; private set; }
        protected cloudscribe.DateTimeUtils.ITimeZoneHelper     TimeZoneHelper { get; private set; }

        protected const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";


        [TempData]
        public string StatusMessage { get; set; }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> Index()
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());
            var model = new AccountIndexViewModel
            {
                HasPassword       = (!string.IsNullOrWhiteSpace(user.PasswordHash)),
                PhoneNumber       = !string.IsNullOrWhiteSpace(user.PhoneNumber) ? user.PhoneNumber : null,
                TwoFactor         = user.TwoFactorEnabled,
                Logins            = await UserManager.GetLoginsAsync(user),
                BrowserRemembered = await SignInManager.IsTwoFactorClientRememberedAsync(user),
                TimeZone          = user.TimeZoneId,
                Email             = user.Email,
                UserName = user.UserName
            };

            if (string.IsNullOrEmpty(model.TimeZone))
            {
                model.TimeZone = await TimeZoneIdResolver.GetSiteTimeZoneId();
            }

            return View("Index", model);
        }

        [Authorize]
        [HttpGet]
        public virtual IActionResult PersonalData()
        {
            string userId = User.GetUserId();
            return View("PersonalData", userId);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> DownloadPersonalData()
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            Log.LogInformation("User with ID '{UserId}' asked for their personal data.", UserManager.GetUserId(User));

            var personalData = new Dictionary<string, string>();
            var personalDataProps = typeof(SiteUser).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(PersonalDataMarkerAttribute)));

            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            var logins = await UserManager.GetLoginsAsync(user);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            personalData.Add($"Authenticator Key", await UserManager.GetAuthenticatorKeyAsync(user));

            var locations = await UserManager.GetUserLocations(user.SiteId, user.Id, 1, 100);
            int i = 1;
            foreach (var location in locations.Data)
            {
                personalData.Add($"IpAddress {i}", location.IpAddress);
                i += 1;
            }

            var fileName = Request.Host.Host.Replace(":", "") + "-PersonalData.json";

            Response.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");
            return new FileContentResult(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(personalData)), "text/json");

        }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> ChangeUserEmail()
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var model = new ChangeUserEmailViewModel
            {
                HasPassword            = await UserManager.HasPasswordAsync(user),
                AccountApproved        = user.AccountApproved,
                CurrentEmail           = user.Email,
                AllowUserToChangeEmail = CurrentSite.AllowUserToChangeEmail,
                EmailIsConfigured      = await SiteCapabilities.SupportsEmailNotification(CurrentSite),
                RequireConfirmedEmail  = CurrentSite.RequireConfirmedEmail
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ChangeUserEmail(ChangeUserEmailViewModel model)
        {
            ModelState.Clear();

            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var requirePassword          = await UserManager.HasPasswordAsync(user);
            model.HasPassword            = requirePassword;
            model.AccountApproved        = user.AccountApproved;
            model.CurrentEmail           = user.Email;
            model.AllowUserToChangeEmail = CurrentSite.AllowUserToChangeEmail;
            model.EmailIsConfigured      = await SiteCapabilities.SupportsEmailNotification(CurrentSite);
            model.RequireConfirmedEmail  = CurrentSite.RequireConfirmedEmail;

            if (requirePassword)
            {
                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Password is required");
                    return View(model);
                }

                if (!await UserManager.CheckPasswordAsync(user, model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Password not correct.");
                    model.Password = "";
                    return View(model);
                }
            }

            if (!model.AccountApproved)
            {
                this.AlertDanger(StringLocalizer["This user account is not currently approved."]);
                return View(model);
            }

            if (!model.AllowUserToChangeEmail)
            {
                this.AlertDanger(StringLocalizer["Site is not configured to allow email changing."]);
                return View(model);
            }

            // Note - need to check the behaviour of this in related sites mode
            if (await UserManager.EmailExistsInDB(CurrentSite.Id, model.NewEmail))
            {
                this.AlertDanger(StringLocalizer["Error - email could not be changed. Contact the site administrator for support."]);
                return View(model);
            }

            var token = await UserManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);

            var siteUrl = Url.Action(new UrlActionContext
            {
                Action = "Index",
                Controller = "Home",
                Protocol = HttpContext.Request.Scheme
            });

            if (!model.RequireConfirmedEmail)
            {
                try
                {
                    var success = await EmailChangeHandler.HandleEmailChangeWithoutUserConfirmation(model, user, token, siteUrl);

                    if (success)
                        this.AlertSuccess(model.SuccessNotification, true);
                    else
                        this.AlertDanger(model.SuccessNotification, true);
                }
                catch (Exception ex)
                {
                    this.AlertDanger(StringLocalizer["Error - email could not be changed. Contact the site administrator for support."], true);
                    Log.LogError(ex, $"Unexpected error occurred changing email address for user ID '{user.Id}'.");
                }
            }
            else
            {
                // send token in confirmation email
                try
                {
                    var confirmationUrl = Url.Action(new UrlActionContext
                    {
                        Action = "ConfirmEmailChange",
                        Controller = "Manage",
                        Values = new { userId = user.Id.ToString(), newEmail = model.NewEmail, code = token },
                        Protocol = HttpContext.Request.Scheme
                    });

                    var success = await EmailChangeHandler.HandleEmailChangeWithUserConfirmation(model, user, token, confirmationUrl, siteUrl);

                    if (success)
                    {
                        this.AlertSuccess(model.SuccessNotification, true);
                        return View("EmailChangeConfirmationSent", model);
                    }
                    else
                    {
                        this.AlertDanger(model.SuccessNotification, true);
                    }
                }
                catch (Exception ex)
                {
                    this.AlertDanger(StringLocalizer["Error - email could not be changed. Contact the site administrator for support."], true);
                    Log.LogError(ex, $"Unexpected error occurred sending email change confirmation for user ID '{user.Id}'.");
                }
            }

            return await Index();  // Route back to Index Page, taking toast alerts along
        }

        private async Task<IActionResult> RouteToIndexPage(SiteUser user)
        {
            var model = new AccountIndexViewModel
            {
                HasPassword       = (!string.IsNullOrWhiteSpace(user.PasswordHash)),
                PhoneNumber       = !string.IsNullOrWhiteSpace(user.PhoneNumber) ? user.PhoneNumber : null,
                TwoFactor         = user.TwoFactorEnabled,
                Logins            = await UserManager.GetLoginsAsync(user),
                BrowserRemembered = await SignInManager.IsTwoFactorClientRememberedAsync(user),
                TimeZone          = user.TimeZoneId,
                Email             = user.Email
            };

            if (string.IsNullOrEmpty(model.TimeZone))
            {
                model.TimeZone = await TimeZoneIdResolver.GetSiteTimeZoneId();
            }

            return View("Index", model);
        }


        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> ConfirmEmailChange(string userId, string newEmail, string code)
        {
            ModelState.Clear();

            if (userId == null || code == null)
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var model = new ChangeUserEmailViewModel
            {
                HasPassword            = await UserManager.HasPasswordAsync(user),
                AccountApproved        = user.AccountApproved,
                CurrentEmail           = user.Email,
                NewEmail               = newEmail,
                AllowUserToChangeEmail = CurrentSite.AllowUserToChangeEmail,
                EmailIsConfigured      = await SiteCapabilities.SupportsEmailNotification(CurrentSite),
                RequireConfirmedEmail  = CurrentSite.RequireConfirmedEmail
            };

            try
            {
                var siteUrl = Url.Action(new UrlActionContext
                {
                    Action = "Index",
                    Controller = "Home",
                    Protocol = HttpContext.Request.Scheme
                });

                var success = await EmailChangeHandler.HandleEmailChangeConfirmation(model, user, newEmail, code, siteUrl);

                if (success)
                    this.AlertSuccess(model.SuccessNotification, true);
                else
                    this.AlertDanger(model.SuccessNotification, true);
            }
            catch (Exception ex)
            {
                this.AlertDanger(StringLocalizer["Error - email could not be changed. Contact the site administrator for support."], true);
                Log.LogError(ex, $"Unexpected error occurred changing email address for user ID '{user.Id}'.");
            }

            return await Index(); // Route back to Index Page, taking toast alerts along
        }


        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> DeletePersonalData()
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var model = new DeletePersonalDataViewModel
            {
                HasPassword = await UserManager.HasPasswordAsync(user)
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> DeletePersonalData(DeletePersonalDataViewModel model)
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var requirePassword = await UserManager.HasPasswordAsync(user);
            if (requirePassword)
            {
                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Password is required");
                    model.HasPassword = requirePassword;
                    return View(model);
                }

                if (!await UserManager.CheckPasswordAsync(user, model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Password not correct.");
                    model.HasPassword = requirePassword;
                    return View();
                }
            }

            var result = await UserManager.DeleteAsync(user);
            var userId = await UserManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await SignInManager.SignOutAsync();

            Log.LogInformation($"User with ID {userId} deleted themselves.");

            return this.RedirectToSiteRoot(CurrentSite);
        }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> TimeZone()
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());

            var model = new TimeZoneViewModel
            {
                TimeZoneId = user.TimeZoneId
            };
            if (string.IsNullOrEmpty(model.TimeZoneId))
            {
                model.TimeZoneId = await TimeZoneIdResolver.GetSiteTimeZoneId();
            }
            model.AllTimeZones = TimeZoneHelper.GetTimeZoneList().Select(x =>
                           new SelectListItem
                           {
                               Text = x,
                               Value = x,
                               Selected = model.TimeZoneId == x
                           });

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> TimeZone(string timeZoneId)
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());

            if (user != null)
            {
                user.TimeZoneId = timeZoneId;
                await UserManager.UpdateAsync(user);
                this.AlertSuccess(StringLocalizer["Your time zone has been updated."]);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> UserInfo()
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());
            var model = new UserInfoViewModel
            {
                FirstName   = user.FirstName,
                LastName    = user.LastName,
                DateOfBirth = user.DateOfBirth,
                WebSiteUrl  = user.WebSiteUrl,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl   = user.AvatarUrl,
                UserName = user.UserName
            };

            var viewName = await CustomUserInfo.GetUserInfoViewName(CurrentSite, user, HttpContext);
            await CustomUserInfo.HandleUserInfoGet(
                CurrentSite,
                user,
                model,
                HttpContext,
                ViewData);

            return View(viewName, model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> UserInfo(UserInfoViewModel model)
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());
            var viewName = await CustomUserInfo.GetUserInfoViewName(CurrentSite, user, HttpContext);

            bool isValid = ModelState.IsValid;
            bool customDataIsValid = await CustomUserInfo.HandleUserInfoValidation(
                CurrentSite,
                user,
                model,
                HttpContext,
                ViewData,
                ModelState);

            if (!isValid || !customDataIsValid)
            {
                return View(viewName, model);
            }

            if (user != null)
            {
                user.FirstName   = model.FirstName;
                user.LastName    = model.LastName;
                user.PhoneNumber = model.PhoneNumber;

                if (model.DateOfBirth.HasValue)
                {
                    user.DateOfBirth = model.DateOfBirth;
                }

                user.RolesChanged = (user.AvatarUrl != model.AvatarUrl);
                user.WebSiteUrl   = model.WebSiteUrl;
                user.AvatarUrl    = model.AvatarUrl;

                await CustomUserInfo.HandleUserInfoPostSuccess(
                        CurrentSite,
                        user,
                        model,
                        HttpContext
                        );


                await UserManager.UpdateAsync(user);

                this.AlertSuccess(StringLocalizer["Your information has been updated."]);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> RemoveLogin()
        {
            var user                     = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());
            var linkedAccounts           = await UserManager.GetLoginsAsync(user);
            ViewData["ShowRemoveButton"] = await UserManager.HasPasswordAsync(user) || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());

            if (user != null)
            {
                var result = await UserManager.RemoveLoginAsync(user, loginProvider, providerKey);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    this.AlertSuccess(StringLocalizer["The external login was removed."]);
                }
                else
                {
                    this.AlertDanger(StringLocalizer["oops something went wrong, the external login was not removed, please try again."]);

                }
            }
            return RedirectToAction("ManageLogins");

        }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> TwoFactorAuthentication()
        {
            ViewData["Title"] = StringLocalizer["Two-factor authentication"];

            var user = await UserManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator     = await UserManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled         = user.TwoFactorEnabled,
                RecoveryCodesLeft    = await UserManager.CountRecoveryCodesAsync(user),
                Is2faRequiredByAdmin = UserManager.Site.Require2FA
            };

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> Disable2faWarning()
        {
            ViewData["Title"] = StringLocalizer["Disable two-factor authentication (2FA)"];

            var user = await UserManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }

            return View(nameof(Disable2fa));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Disable2fa()
        {
            ViewData["Title"] = StringLocalizer["Disable two-factor authentication (2FA)"];

            var user = await UserManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            var disable2faResult = await UserManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }

            Log.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
            return RedirectToAction(nameof(TwoFactorAuthentication));
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Disable2faForUser(Guid userId, string returnUrl)
        {
            var userIdString = userId.ToString();

            var user = await UserManager.FindByIdAsync(userIdString);
            if (user == null)
            {
                this.AlertDanger(StringLocalizer[$"oops something went wrong, the user matching the supplied id '{userIdString}' was not recognised."]);
            }

            var disable2faResult = await UserManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                this.AlertDanger(StringLocalizer[$"Unexpected error occured disabling 2FA for user with ID '{userIdString}'."]);
            }
            else
            {
                this.AlertSuccess(StringLocalizer[$"2FA successfully disabled for user '{user.DisplayName}'."]);
            }

            Log.LogInformation("Admin has disabled 2FA for user ID {UserId}.", userIdString);

            return Redirect(returnUrl);
        }


        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> EnableAuthenticator()
        {
            ViewData["Title"] = StringLocalizer["Enable authenticator"];

            var user = await UserManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await UserManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
            }

            var model = new EnableAuthenticatorViewModel
            {
                SharedKey = FormatKey(unformattedKey),
                AuthenticatorUri = await GenerateQrCodeUri(user.Email, unformattedKey)
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
        {
            ViewData["Title"] = StringLocalizer["Enable authenticator"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await UserManager.VerifyTwoFactorTokenAsync(
                user, UserManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("model.Code", "Verification code is invalid.");
                return View(model);
            }

            await UserManager.SetTwoFactorEnabledAsync(user, true);
            Log.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
            return RedirectToAction(nameof(GenerateRecoveryCodes));
        }

        [Authorize]
        [HttpGet]
        public virtual IActionResult ResetAuthenticatorWarning()
        {
            ViewData["Title"] = StringLocalizer["Reset authenticator key"];

            return View(nameof(ResetAuthenticator));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ResetAuthenticator()
        {
            ViewData["Title"] = StringLocalizer["Reset authenticator key"];

            var user = await UserManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            await UserManager.SetTwoFactorEnabledAsync(user, false);
            await UserManager.ResetAuthenticatorKeyAsync(user);
            Log.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

            return RedirectToAction(nameof(EnableAuthenticator));
        }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> GenerateRecoveryCodes()
        {
            ViewData["Title"] = StringLocalizer["Recovery codes"];

            var user = await UserManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
            }

            var recoveryCodes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            var model = new GenerateRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

            Log.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await UserManager.FindByIdAsync(User.GetUserId());
            if (user != null)
            {
                await UserManager.SetTwoFactorEnabledAsync(user, true);
                await SignInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await UserManager.FindByIdAsync(User.GetUserId());
            if (user != null)
            {
                await UserManager.SetTwoFactorEnabledAsync(user, false);
                await SignInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user != null)
            {
                var result = await UserManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    this.AlertSuccess(StringLocalizer["Your phone number was removed."]);
                }
                else
                {
                    this.AlertDanger(StringLocalizer["oops something went wrong please try again"]);
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public virtual IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.OldPassword == model.NewPassword)
            {
                this.AlertDanger(StringLocalizer["New password cannot be the same as current password"]);
                return View(model);
            }

            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user != null)
            {
                var result = await UserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);

                    this.AlertSuccess(StringLocalizer["Your password has been changed."]);
                    return RedirectToAction("Index");
                }
                else
                {
                    this.AlertDanger(StringLocalizer["oops something went wrong please try again"]);
                }
                AddErrors(result);
            }

            return View(model);

        }

        [Authorize]
        [HttpGet]
        public virtual IActionResult SetPassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user != null)
            {
                var result = await UserManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    this.AlertSuccess(StringLocalizer["Your password has been set."]);

                    return RedirectToAction("Index");
                }
                else
                {
                    this.AlertDanger(StringLocalizer["oops something went wrong please try again"]);
                }

                AddErrors(result);

            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> ManageLogins()
        {
            if (!CurrentSite.HasAnySocialAuthEnabled())
            {
                return RedirectToAction("Index");
            }

            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(user);
            var externalSchemes = await AccountService.GetExternalAuthenticationSchemes();
            var otherLogins = externalSchemes.Where(auth => userLogins.All(ul => auth.Name != ul.LoginProvider)).ToList();

            var model = new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins

            };
            model.ShowRemoveButton = await UserManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, User.GetUserId());
            return new ChallengeResult(provider, properties);
        }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> LinkLoginCallback()
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var info = await SignInManager.GetExternalLoginInfoAsync(User.GetUserId());
            if (info == null)
            {
                this.AlertDanger(StringLocalizer["oops something went wrong please try again"]);
                return RedirectToAction("ManageLogins");
            }
            var result = await UserManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                this.AlertDanger(StringLocalizer["oops something went wrong, please try again"]);
            }

            return RedirectToAction("ManageLogins");

        }

        [HttpGet]
        public virtual async Task<IActionResult> EmailRequired()
        {
            if (!AccountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user == null || !string.IsNullOrWhiteSpace(user.Email))
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            var model = new EmailRequiredViewModel
            {

            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> EmailRequired(EmailRequiredViewModel model)
        {
            var user = await UserManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user == null || !string.IsNullOrWhiteSpace(user.Email))
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var emailUser = await UserManager.FindByEmailAsync(model.Email);
            if (emailUser != null && emailUser.Id != user.Id)
            {
                ModelState.AddModelError("invalidEmail", StringLocalizer["The provided email address was not accepted, please use a different email address."]);
                return View(model);
            }

            var result = await UserManager.SetEmailAsync(user, model.Email);

            if (result.Succeeded)
            {
                user.RolesChanged = true; //needed to get the new email claim
                await UserManager.UpdateAsync(user);

                return this.RedirectToSiteRoot(CurrentSite);
            }

            return View(model);
        }

        #region Helpers

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private async Task<string> GenerateQrCodeUri(string email, string unformattedKey)
        {
            // get the user's site accounting for possibility of being in related sites mode
            // in which case our URI and stored tokens all use the RelatedSiteId
            var effectiveSite = await SiteManager.GetSiteForDataOperations(UserManager.Site.Id, true);

            return string.Format(
                AuthenicatorUriFormat,
                UrlEncoder.Encode(effectiveSite.SiteName),
                UrlEncoder.Encode(email),
                unformattedKey);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}