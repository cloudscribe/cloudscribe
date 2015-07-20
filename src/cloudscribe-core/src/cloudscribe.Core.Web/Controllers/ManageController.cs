// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2015-07-20
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using cloudscribe.Messaging;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace cloudscribe.Core.Web.Controllers
{
    [Authorize]
    public class ManageController : CloudscribeBaseController
    {
        public ManageController(
            ISiteResolver siteResolver,
            SiteUserManager<SiteUser> userManager,
            SignInManager<SiteUser> signInManager)
        {
            Site = siteResolver.Resolve();
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private ISiteSettings Site;
        public SiteUserManager<SiteUser> UserManager { get; private set; }
        public SignInManager<SiteUser> SignInManager { get; private set; }


        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["SiteName"] = Site.SiteName;

            var user = await UserManager.FindByIdAsync(Context.User.GetUserId());
            var model = new AccountIndexViewModel
            {
                HasPassword = (user.PasswordHash.Length > 0),
                PhoneNumber = user.PhoneNumber,
                TwoFactor = user.TwoFactorEnabled,
                Logins = await UserManager.GetLoginsAsync(user),
                BrowserRemembered = await SignInManager.IsTwoFactorClientRememberedAsync(user)
            };
            
            return View(model);
        }

        //
        // GET: /Manage/RemoveLogin
        [HttpGet]
        public async Task<IActionResult> RemoveLogin()
        {
            ViewData["SiteName"] = Site.SiteName;
            var user = await UserManager.FindByIdAsync(Context.User.GetUserId());
            var linkedAccounts = await UserManager.GetLoginsAsync(user);
            ViewBag.ShowRemoveButton = await UserManager.HasPasswordAsync(user) || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }


        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ViewData["SiteName"] = Site.SiteName;

            var user = await UserManager.FindByIdAsync(Context.User.GetUserId());

            if (user != null)
            {
                var result = await UserManager.RemoveLoginAsync(user, loginProvider, providerKey);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    this.AlertSuccess("The external login was removed.");
                }
                else
                {
                    this.AlertDanger("oops something went wrong, the external login was not removed, please try again.");

                }
            }
            return RedirectToAction("ManageLogins");

        }


        // GET: /Manage/AddPhoneNumber
        [HttpGet]
        public IActionResult AddPhoneNumber()
        {
            ViewData["SiteName"] = Site.SiteName;

            return View();
        }


        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            ViewData["SiteName"] = Site.SiteName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await UserManager.FindByIdAsync(Context.User.GetUserId());
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(user, model.Number);
            await MessageServices.SendSmsAsync(model.Number, "Your security code is: " + code);
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });

        }


        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await UserManager.FindByIdAsync(Context.User.GetUserId());
            if (user != null)
            {
                await UserManager.SetTwoFactorEnabledAsync(user, true);
                await SignInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }


        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await UserManager.FindByIdAsync(Context.User.GetUserId());
            if (user != null)
            {
                await UserManager.SetTwoFactorEnabledAsync(user, false);
                await SignInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }


        // GET: /Manage/VerifyPhoneNumber
        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            ViewData["SiteName"] = Site.SiteName;
            var user = await UserManager.FindByIdAsync(Context.User.GetUserId());
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            // Send an SMS to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }


        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            ViewData["SiteName"] = Site.SiteName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByIdAsync(Context.User.GetUserId());
            if (user != null)
            {
                var result = await UserManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);

                    this.AlertSuccess("Your phone number was added.");

                    return RedirectToAction("Index");
                }
            }
            // If we got this far, something failed, redisplay the form
            ModelState.AddModelError(string.Empty, "Failed to verify phone number");
            return View(model);

        }

        //
        // GET: /Manage/RemovePhoneNumber
        [HttpGet]
        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await UserManager.FindByIdAsync(Context.User.GetUserId());
            if (user != null)
            {
                var result = await UserManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    this.AlertSuccess("Your phone number was removed.");
                }
                else
                {
                    this.AlertDanger("oops something went wrong please try again");
                }
            }
            return RedirectToAction("Index");
        }


        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            ViewData["SiteName"] = Site.SiteName;

            return View();
        }


        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ViewData["SiteName"] = Site.SiteName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByIdAsync(Context.User.GetUserId());
            if (user != null)
            {
                var result = await UserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);

                    this.AlertSuccess("Your password has been changed.");
                    return RedirectToAction("Index");
                }
                else
                {
                    this.AlertDanger("oops something went wrong please try again");
                }
                AddErrors(result);
            }
           
            return View(model);

        }


        // GET: /Manage/SetPassword
        [HttpGet]
        public IActionResult SetPassword()
        {
            ViewData["SiteName"] = Site.SiteName;

            return View();
        }


        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            ViewData["SiteName"] = Site.SiteName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByIdAsync(Context.User.GetUserId());
            if (user != null)
            {
                var result = await UserManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    this.AlertSuccess("your password has been set");

                    return RedirectToAction("Index");
                }
                else
                {
                    this.AlertDanger("oops something went wrong please try again");
                }

                AddErrors(result);
                
            }

            return View(model);
            
            
        }


        // GET: /Manage/ManageLogins
        [HttpGet]
        public async Task<IActionResult> ManageLogins()
        {
            ViewData["SiteName"] = Site.SiteName;

            var user = await UserManager.FindByIdAsync(Context.User.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(user);
            var otherLogins = SignInManager.GetExternalAuthenticationSchemes().Where(auth => userLogins.All(ul => auth.AuthenticationScheme != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });

            
        }


        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, User.GetUserId());
            return new ChallengeResult(provider, properties);
        }


        // GET: /Manage/LinkLoginCallback
        [HttpGet]
        public async Task<IActionResult> LinkLoginCallback()
        {
            var user = await UserManager.FindByIdAsync(Context.User.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var info = await SignInManager.GetExternalLoginInfoAsync(User.GetUserId());
            if (info == null)
            {
                this.AlertDanger("oops something went wrong please try again");
                return RedirectToAction("ManageLogins");
            }
            var result = await UserManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                this.AlertDanger("oops something went wrong, please try again");
            }

            return RedirectToAction("ManageLogins");

        }

        #region Helpers
 
        
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