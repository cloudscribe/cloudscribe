// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2018-03-07
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
            //ISmsSender smsSender,
            IStringLocalizer<CloudscribeCore> localizer,
            ITimeZoneIdResolver timeZoneIdResolver,
            ITimeZoneHelper timeZoneHelper,
            IHandleCustomUserInfo customUserInfo,
            ILogger<ManageController> logger,
            UrlEncoder urlEncoder
            )
        {
            _currentSite = currentSite; 
            _userManager = userManager;
            _signInManager = signInManager;
            _sr = localizer;
            _timeZoneIdResolver = timeZoneIdResolver;
            _tzHelper = timeZoneHelper;
            _customUserInfo = customUserInfo;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        private readonly ILogger _logger;
        private readonly ISiteContext _currentSite;
        private readonly SiteUserManager<SiteUser> _userManager;
        private readonly SignInManager<SiteUser> _signInManager;
        private IStringLocalizer _sr;
        private ITimeZoneIdResolver _timeZoneIdResolver;
        private ITimeZoneHelper _tzHelper;
        private IHandleCustomUserInfo _customUserInfo;
        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private readonly UrlEncoder _urlEncoder;

        [TempData]
        public string StatusMessage { get; set; }


        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
            var model = new AccountIndexViewModel
            {
                HasPassword = (user.PasswordHash.Length > 0),
                PhoneNumber = user.PhoneNumber.Length > 0 ? user.PhoneNumber : null,
                TwoFactor = user.TwoFactorEnabled,
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                TimeZone = user.TimeZoneId
                
            };

            if(string.IsNullOrEmpty(model.TimeZone))
            {
                model.TimeZone = await _timeZoneIdResolver.GetSiteTimeZoneId();
            }
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> TimeZone()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());

            var model = new TimeZoneViewModel
            {
                TimeZoneId = user.TimeZoneId
            };
            if (string.IsNullOrEmpty(model.TimeZoneId))
            {
                model.TimeZoneId = await _timeZoneIdResolver.GetSiteTimeZoneId();
            }
            model.AllTimeZones = _tzHelper.GetTimeZoneList().Select(x =>
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
            var user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());

            if (user != null)
            {
                user.TimeZoneId = timeZoneId;
                await _userManager.UpdateAsync(user);
                this.AlertSuccess(_sr["Your time zone has been updated."]);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UserInfo()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
            var model = new UserInfoViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                WebSiteUrl = user.WebSiteUrl,
                PhoneNumber = user.PhoneNumber
            };

            var viewName = await _customUserInfo.GetUserInfoViewName(_currentSite, user, HttpContext);
            await _customUserInfo.HandleUserInfoGet(
                _currentSite,
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
            var user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
            var viewName = await _customUserInfo.GetUserInfoViewName(_currentSite, user, HttpContext);

            bool isValid = ModelState.IsValid;
            bool customDataIsValid = await _customUserInfo.HandleUserInfoValidation(
                _currentSite,
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

                await _customUserInfo.HandleUserInfoPostSuccess(
                        _currentSite,
                        user,
                        model,
                        HttpContext
                        );


                await _userManager.UpdateAsync(user);

                

                this.AlertSuccess(_sr["Your information has been updated."]);
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage/RemoveLogin
        [HttpGet]
        public async Task<IActionResult> RemoveLogin()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
            var linkedAccounts = await _userManager.GetLoginsAsync(user);
            ViewData["ShowRemoveButton"] = await _userManager.HasPasswordAsync(user) || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }


        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());

            if (user != null)
            {
                var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    this.AlertSuccess(_sr["The external login was removed."]);
                }
                else
                {
                    this.AlertDanger(_sr["oops something went wrong, the external login was not removed, please try again."]);

                }
            }
            return RedirectToAction("ManageLogins");

        }


        // GET: /Manage/AddPhoneNumber
        //[HttpGet]
        //public IActionResult AddPhoneNumber()
        //{
        //    return View();
        //}


        //// POST: /Manage/AddPhoneNumber
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    // Generate the token and send it
        //    var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
        //    var code = await userManager.GenerateChangePhoneNumberTokenAsync(user, model.Number);
        //    await smsSender.SendSmsAsync(
        //        Site, 
        //        model.Number,
        //        string.Format(sr["Your security code is: {0}"], code)
        //        );
        //    return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });

        //}

        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            ViewData["Title"] = _sr["Two-factor authentication"];

            var user = await _userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Disable2faWarning()
        {
            ViewData["Title"] = _sr["Disable two-factor authentication (2FA)"];

            var user = await _userManager.FindByIdAsync(User.GetUserId());
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
            ViewData["Title"] = _sr["Disable two-factor authentication (2FA)"];

            var user = await _userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
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
            ViewData["Title"] = _sr["Enable authenticator"];

            var user = await _userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
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
            ViewData["Title"] = _sr["Enable authenticator"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            // Strip spaces and hypens
            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("model.Code", "Verification code is invalid.");
                return View(model);
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            _logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
            return RedirectToAction(nameof(GenerateRecoveryCodes));
        }

        [HttpGet]
        public IActionResult ResetAuthenticatorWarning()
        {
            ViewData["Title"] = _sr["Reset authenticator key"];

            return View(nameof(ResetAuthenticator));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticator()
        {
            ViewData["Title"] = _sr["Reset authenticator key"];

            var user = await _userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            _logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

            return RedirectToAction(nameof(EnableAuthenticator));
        }

        [HttpGet]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            ViewData["Title"] = _sr["Recovery codes"];

            var user = await _userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{User.GetUserId()}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
            }

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            var model = new GenerateRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

            _logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

            return View(model);
        }


        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await _userManager.FindByIdAsync(User.GetUserId());
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }


        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await _userManager.FindByIdAsync(User.GetUserId());
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }


        // GET: /Manage/VerifyPhoneNumber
        //[HttpGet]
        //public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        //{
        //    var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
        //    var code = await userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
        //    // Send an SMS to verify the phone number
        //    return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        //}


        //// POST: /Manage/VerifyPhoneNumber
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var user = await userManager.FindByIdAsync(HttpContext.User.GetUserId());
        //    if (user != null)
        //    {
        //        var result = await userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
        //        if (result.Succeeded)
        //        {
        //            await signInManager.SignInAsync(user, isPersistent: false);

        //            this.AlertSuccess(sr["Your phone number was added."]);

        //            return RedirectToAction("Index");
        //        }
        //    }
        //    // If we got this far, something failed, redisplay the form
        //    ModelState.AddModelError(string.Empty, sr["Failed to verify phone number"]);
        //    return View(model);

        //}

        //
        // GET: /Manage/RemovePhoneNumber
        [HttpGet]
        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user != null)
            {
                var result = await _userManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    this.AlertSuccess(_sr["Your phone number was removed."]);
                }
                else
                {
                    this.AlertDanger(_sr["oops something went wrong please try again"]);
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
            var user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    this.AlertSuccess(_sr["Your password has been changed."]);
                    return RedirectToAction("Index");
                }
                else
                {
                    this.AlertDanger(_sr["oops something went wrong please try again"]);
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

            var user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user != null)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    this.AlertSuccess(_sr["Your password has been set."]);

                    return RedirectToAction("Index");
                }
                else
                {
                    this.AlertDanger(_sr["oops something went wrong please try again"]);
                }

                AddErrors(result);
                
            }

            return View(model);     
        }


        // GET: /Manage/ManageLogins
        [HttpGet]
        public async Task<IActionResult> ManageLogins()
        {
            if(!_currentSite.HasAnySocialAuthEnabled())
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(user);
            var externalSchemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            var otherLogins = externalSchemes.Where(auth => userLogins.All(ul => auth.Name != ul.LoginProvider)).ToList();
            
            var model = new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins

            };
            model.ShowRemoveButton = await _userManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;

            return View(model);  
        }


        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, User.GetUserId());
            return new ChallengeResult(provider, properties);
        }


        // GET: /Manage/LinkLoginCallback
        [HttpGet]
        public async Task<IActionResult> LinkLoginCallback()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync(User.GetUserId());
            if (info == null)
            {
                this.AlertDanger(_sr["oops something went wrong please try again"]);
                return RedirectToAction("ManageLogins");
            }
            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                this.AlertDanger(_sr["oops something went wrong, please try again"]);
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
                _urlEncoder.Encode(_userManager.Site.SiteName),
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