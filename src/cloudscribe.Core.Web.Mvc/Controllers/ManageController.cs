// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2017-10-06
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using cloudscribe.Web.Common;
using cloudscribe.Web.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers.Mvc
{
    [Authorize]
    public class ManageController : Controller
    {
        public ManageController(
            SiteContext currentSite,
            SiteUserManager<SiteUser> userManager,
            SignInManager<SiteUser> signInManager,
            ISmsSender smsSender,
            IStringLocalizer<CloudscribeCore> localizer,
            ITimeZoneIdResolver timeZoneIdResolver,
            ITimeZoneHelper timeZoneHelper,
            IHandleCustomUserInfo customUserInfo,
            ILogger<ManageController> logger,
            UrlEncoder urlEncoder
            )
        {
            Site = currentSite; 
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.smsSender = smsSender;
            sr = localizer;
            this.timeZoneIdResolver = timeZoneIdResolver;
            tzHelper = timeZoneHelper;
            this.customUserInfo = customUserInfo;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        private readonly ILogger _logger;
        private readonly ISiteContext Site;
        private readonly SiteUserManager<SiteUser> userManager;
        private readonly SignInManager<SiteUser> signInManager;
        //private readonly IAuthEmailSender emailSender;
        private readonly ISmsSender smsSender;
        private IStringLocalizer sr;
        private ITimeZoneIdResolver timeZoneIdResolver;
        private ITimeZoneHelper tzHelper;
        private IHandleCustomUserInfo customUserInfo;
        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private readonly UrlEncoder _urlEncoder;

        [TempData]
        public string StatusMessage { get; set; }


        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
            var model = new AccountIndexViewModel
            {
                HasPassword = (user.PasswordHash.Length > 0),
                PhoneNumber = user.PhoneNumber.Length > 0 ? user.PhoneNumber : null,
                TwoFactor = user.TwoFactorEnabled,
                Logins = await userManager.GetLoginsAsync(user),
                BrowserRemembered = await signInManager.IsTwoFactorClientRememberedAsync(user),
                TimeZone = user.TimeZoneId
                
            };

            if(string.IsNullOrEmpty(model.TimeZone))
            {
                model.TimeZone = await timeZoneIdResolver.GetSiteTimeZoneId();
            }
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> TimeZone()
        {
            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());

            var model = new TimeZoneViewModel();

            model.TimeZoneId = user.TimeZoneId;
            if (string.IsNullOrEmpty(model.TimeZoneId))
            {
                model.TimeZoneId = await timeZoneIdResolver.GetSiteTimeZoneId();
            }
            model.AllTimeZones = tzHelper.GetTimeZoneList().Select(x =>
                           new SelectListItem
                           {
                               Text = x,
                               Value = x,
                               Selected = model.TimeZoneId == x
                           });

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TimeZone(string timeZoneId)
        {
            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());

            if (user != null)
            {
                user.TimeZoneId = timeZoneId;
                await userManager.UpdateAsync(user);
                this.AlertSuccess(sr["Your time zone has been updated."]);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UserInfo()
        {
            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
            var model = new UserInfoViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                WebSiteUrl = user.WebSiteUrl,
                PhoneNumber = user.PhoneNumber
            };

            var viewName = await customUserInfo.GetUserInfoViewName(Site, user, HttpContext);
            await customUserInfo.HandleUserInfoGet(
                Site,
                user,
                model,
                HttpContext,
                ViewData);

            return View(viewName, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserInfo(UserInfoViewModel model)
        {
            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
            var viewName = await customUserInfo.GetUserInfoViewName(Site, user, HttpContext);

            bool isValid = ModelState.IsValid;
            bool customDataIsValid = await customUserInfo.HandleUserInfoValidation(
                Site,
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
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                if(model.DateOfBirth.HasValue)
                {
                    user.DateOfBirth = model.DateOfBirth;
                }
                
                
                user.WebSiteUrl = model.WebSiteUrl;

                await customUserInfo.HandleUserInfoPostSuccess(
                        Site,
                        user,
                        model,
                        HttpContext
                        );


                await userManager.UpdateAsync(user);

                

                this.AlertSuccess(sr["Your information has been updated."]);
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage/RemoveLogin
        [HttpGet]
        public async Task<IActionResult> RemoveLogin()
        {
            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
            var linkedAccounts = await userManager.GetLoginsAsync(user);
            ViewData["ShowRemoveButton"] = await userManager.HasPasswordAsync(user) || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }


        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());

            if (user != null)
            {
                var result = await userManager.RemoveLoginAsync(user, loginProvider, providerKey);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    this.AlertSuccess(sr["The external login was removed."]);
                }
                else
                {
                    this.AlertDanger(sr["oops something went wrong, the external login was not removed, please try again."]);

                }
            }
            return RedirectToAction("ManageLogins");

        }


        // GET: /Manage/AddPhoneNumber
        [HttpGet]
        public IActionResult AddPhoneNumber()
        {
            return View();
        }


        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
            var code = await userManager.GenerateChangePhoneNumberTokenAsync(user, model.Number);
            await smsSender.SendSmsAsync(
                Site, 
                model.Number,
                string.Format(sr["Your security code is: {0}"], code)
                );
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });

        }

        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            ViewData["Title"] = sr["Two-factor authentication"];

            var user = await userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await userManager.CountRecoveryCodesAsync(user),
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Disable2faWarning()
        {
            ViewData["Title"] = sr["Disable two-factor authentication (2FA)"];

            var user = await userManager.FindByIdAsync(User.GetUserId());
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable2fa()
        {
            ViewData["Title"] = sr["Disable two-factor authentication (2FA)"];

            var user = await userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            var disable2faResult = await userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }
            
            _logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
            return RedirectToAction(nameof(TwoFactorAuthentication));
        }

        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            ViewData["Title"] = sr["Enable authenticator"];

            var user = await userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            var unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
            }

            var model = new EnableAuthenticatorViewModel
            {
                SharedKey = FormatKey(unformattedKey),
                AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
        {
            ViewData["Title"] = sr["Enable authenticator"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            // Strip spaces and hypens
            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await userManager.VerifyTwoFactorTokenAsync(
                user, userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("model.Code", "Verification code is invalid.");
                return View(model);
            }

            await userManager.SetTwoFactorEnabledAsync(user, true);
            _logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
            return RedirectToAction(nameof(GenerateRecoveryCodes));
        }

        [HttpGet]
        public IActionResult ResetAuthenticatorWarning()
        {
            ViewData["Title"] = sr["Reset authenticator key"];

            return View(nameof(ResetAuthenticator));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticator()
        {
            ViewData["Title"] = sr["Reset authenticator key"];

            var user = await userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            await userManager.SetTwoFactorEnabledAsync(user, false);
            await userManager.ResetAuthenticatorKeyAsync(user);
            _logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

            return RedirectToAction(nameof(EnableAuthenticator));
        }

        [HttpGet]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            ViewData["Title"] = sr["Recovery codes"];

            var user = await userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
            }

            var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            var model = new GenerateRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

            _logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

            return View(model);
        }


        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await userManager.FindByIdAsync(User.GetUserId());
            if (user != null)
            {
                await userManager.SetTwoFactorEnabledAsync(user, true);
                await signInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }


        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await userManager.FindByIdAsync(User.GetUserId());
            if (user != null)
            {
                await userManager.SetTwoFactorEnabledAsync(user, false);
                await signInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }


        // GET: /Manage/VerifyPhoneNumber
        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
            var code = await userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            // Send an SMS to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }


        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user != null)
            {
                var result = await userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);

                    this.AlertSuccess(sr["Your phone number was added."]);

                    return RedirectToAction("Index");
                }
            }
            // If we got this far, something failed, redisplay the form
            ModelState.AddModelError(string.Empty, sr["Failed to verify phone number"]);
            return View(model);

        }

        //
        // GET: /Manage/RemovePhoneNumber
        [HttpGet]
        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user != null)
            {
                var result = await userManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    this.AlertSuccess(sr["Your phone number was removed."]);
                }
                else
                {
                    this.AlertDanger(sr["oops something went wrong please try again"]);
                }
            }
            return RedirectToAction("Index");
        }


        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user != null)
            {
                var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);

                    this.AlertSuccess(sr["Your password has been changed."]);
                    return RedirectToAction("Index");
                }
                else
                {
                    this.AlertDanger(sr["oops something went wrong please try again"]);
                }
                AddErrors(result);
            }
           
            return View(model);

        }


        // GET: /Manage/SetPassword
        [HttpGet]
        public IActionResult SetPassword()
        {
            return View();
        }


        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user != null)
            {
                var result = await userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    this.AlertSuccess(sr["Your password has been set."]);

                    return RedirectToAction("Index");
                }
                else
                {
                    this.AlertDanger(sr["oops something went wrong please try again"]);
                }

                AddErrors(result);
                
            }

            return View(model);     
        }


        // GET: /Manage/ManageLogins
        [HttpGet]
        public async Task<IActionResult> ManageLogins()
        {
            if(!Site.HasAnySocialAuthEnabled())
            {
                return RedirectToAction("Index");
            }

            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await userManager.GetLoginsAsync(user);
            var externalSchemes = await signInManager.GetExternalAuthenticationSchemesAsync();
            var otherLogins = externalSchemes.Where(auth => userLogins.All(ul => auth.Name != ul.LoginProvider)).ToList();
            
            var model = new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins

            };
            model.ShowRemoveButton = await userManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;

            return View(model);  
        }


        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, User.GetUserId());
            return new ChallengeResult(provider, properties);
        }


        // GET: /Manage/LinkLoginCallback
        [HttpGet]
        public async Task<IActionResult> LinkLoginCallback()
        {
            var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var info = await signInManager.GetExternalLoginInfoAsync(User.GetUserId());
            if (info == null)
            {
                this.AlertDanger(sr["oops something went wrong please try again"]);
                return RedirectToAction("ManageLogins");
            }
            var result = await userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                this.AlertDanger(sr["oops something went wrong, please try again"]);
            }

            return RedirectToAction("ManageLogins");

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

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenicatorUriFormat,
                _urlEncoder.Encode(userManager.Site.SiteName),
                _urlEncoder.Encode(email),
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