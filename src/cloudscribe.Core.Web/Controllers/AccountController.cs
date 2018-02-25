// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2017-10-12
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Analytics;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers.Mvc
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController(
            IAccountService accountService,
            SiteContext currentSite,
            IpAddressTracker ipAddressTracker,
            ISiteMessageEmailSender emailSender,
            SiteTimeZoneService timeZoneHelper,
            ISmsSender smsSender,
            IIdentityServerIntegration identityServerIntegration,
            IStringLocalizer<CloudscribeCore> localizer,
            IRecaptchaKeysProvider recaptchaKeysProvider,
            IHandleCustomRegistration customRegistration,
            IHandleAccountAnalytics analyticsHandler,
            ILogger<AccountController> logger
            )
        {
            this.accountService = accountService;
            Site = currentSite; 
            this.identityServerIntegration = identityServerIntegration;
            this.emailSender = emailSender;
            this.smsSender = smsSender;
            this.ipAddressTracker = ipAddressTracker;
            sr = localizer;
            log = logger;
            this.recaptchaKeysProvider = recaptchaKeysProvider;
            this.timeZoneHelper = timeZoneHelper;
            this.customRegistration = customRegistration;
            analytics = analyticsHandler;
        }

        private readonly IAccountService accountService;
        private readonly ISiteContext Site;
        private readonly IIdentityServerIntegration identityServerIntegration;
        private readonly ISiteMessageEmailSender emailSender;
        private readonly ISmsSender smsSender;
        private IpAddressTracker ipAddressTracker;
        private ILogger log;
        private IStringLocalizer sr;
        private IRecaptchaKeysProvider recaptchaKeysProvider;
        private SiteTimeZoneService timeZoneHelper;
        private IHandleCustomRegistration customRegistration;
        private IHandleAccountAnalytics analytics;

        private async Task<IActionResult> HandleLoginSuccess(UserLoginResult result, string returnUrl)
        {
            analytics.HandleLoginSuccess(result).Forget();

            if (result.User != null)
            {
                await ipAddressTracker.TackUserIpAddress(Site.Id, result.User.Id);
            }
            
            if (!string.IsNullOrEmpty(returnUrl))
            {
                // when site is closed login is still allowed
                // but don't redirect to closed paged
                if(
                    (!returnUrl.Contains("/closed"))
                    && (!returnUrl.Contains("/oops/error"))
                    )
                {
                    return LocalRedirect(returnUrl);
                }
                
            }

            return this.RedirectToSiteRoot(Site);
        }

        private bool ShouldSendConfirmation(IUserContext user)
        {
            if (user.EmailConfirmSentUtc == null) return true; //never sent yet
            var timeSpan = DateTime.UtcNow - user.EmailConfirmSentUtc;
            if (timeSpan.Value != null && timeSpan.Value.TotalDays > 1) return true; // at most resend once per day if user tries to login

            return false;
        }

        private async Task<IActionResult> HandleLoginNotAllowed(UserLoginResult result)
        {
            analytics.HandleLoginNotAllowed(result).Forget();

            if (result.User != null)
            {      
                await ipAddressTracker.TackUserIpAddress(Site.Id, result.User.Id);

                if (result.NeedsEmailConfirmation)
                {
                    if(ShouldSendConfirmation(result.User))
                    {
                        var callbackUrl = Url.Action(new UrlActionContext
                        {
                            Action = "ConfirmEmail",
                            Controller = "Account",
                            Values = new { userId = result.User.Id.ToString(), code = result.EmailConfirmationToken },
                            Protocol = HttpContext.Request.Scheme
                        });

                        emailSender.SendAccountConfirmationEmailAsync(
                            Site,
                            result.User.Email,
                            sr["Confirm your account"],
                            callbackUrl).Forget();


                        this.AlertSuccess(sr["Please check your email inbox, we just sent you a link that you need to click to confirm your account"], true);
                    }
                   
                    return RedirectToAction("EmailConfirmationRequired", new { userId = result.User.Id, didSend = true });
                }

                if (result.NeedsAccountApproval)
                {
                    var timeSpan = DateTime.UtcNow - result.User.CreatedUtc;
                    if (timeSpan.TotalDays < 1)
                    {
                        // account was just created so send notification to approver
                        emailSender.AccountPendingApprovalAdminNotification(Site, result.User).Forget();
                    }

                    return RedirectToAction("PendingApproval", new { userId = result.User.Id, didSend = true });
                }
            }

            return this.RedirectToSiteRoot(Site);
        }

        private async Task<IActionResult> HandleRequiresTwoFactor(UserLoginResult result, string returnUrl, bool rememberMe)
        {
            analytics.HandleRequiresTwoFactor(result).Forget();

            if (result.User != null)
            {
                await ipAddressTracker.TackUserIpAddress(Site.Id, result.User.Id);

                log.LogWarning($"redirecting from login for {result.User.Email} because 2 factor not configured yet for account");
            }

            return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, rememberMe });

            //return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        private async Task<IActionResult> HandleLockout(UserLoginResult result = null)
        {
            analytics.HandleLockout(result).Forget();

            ViewData["Title"] = sr["Locked out"];

            if (result != null && result.User != null)
            {
                await ipAddressTracker.TackUserIpAddress(Site.Id, result.User.Id);

                log.LogWarning($"redirecting to lockout page for {result.User.Email} because account is locked");
            }

            return View("Lockout");
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (accountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(Site);
            }

            returnUrl = identityServerIntegration.EnsureFolderSegmentIfNeeded(Site, returnUrl);
            //identityserver integration point
            var idProvider = await identityServerIntegration.GetAuthorizationContextAsync(returnUrl);

            if (!string.IsNullOrEmpty(idProvider))
            {
                // if IdP is passed, then bypass showing the login screen
                return ExternalLogin(idProvider, returnUrl);
            }

            ViewData["Title"] = sr["Log In"];
            ViewData["ReturnUrl"] = returnUrl;

            var model = new LoginViewModel();

            var recaptchaKeys = await recaptchaKeysProvider.GetKeys().ConfigureAwait(false);
            
            if ((Site.CaptchaOnLogin)&& (!string.IsNullOrEmpty(recaptchaKeys.PublicKey)))
            {
                model.RecaptchaSiteKey = recaptchaKeys.PublicKey;
                model.UseInvisibleCaptcha = recaptchaKeys.Invisible;
            }
            model.UseEmailForLogin = Site.UseEmailForLogin;
            model.LoginInfoTop = Site.LoginInfoTop;
            model.LoginInfoBottom = Site.LoginInfoBottom;
            var externalSchemes = await accountService.GetExternalAuthenticationSchemes();
            model.ExternalAuthenticationList = externalSchemes.ToList();
            // don't disable db auth if there are no social auth providers configured
            model.DisableDbAuth = Site.DisableDbAuth && Site.HasAnySocialAuthEnabled();

            return View(model);
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["Title"] = sr["Log In"];
            ViewData["ReturnUrl"] = returnUrl;

            analytics.HandleLoginSubmit("Onsite").Forget();

            var recaptchaKeys = await recaptchaKeysProvider.GetKeys().ConfigureAwait(false);
            if ((Site.CaptchaOnLogin) && (!string.IsNullOrEmpty(recaptchaKeys.PublicKey)))
            {
                model.RecaptchaSiteKey = recaptchaKeys.PublicKey;
                model.UseInvisibleCaptcha = recaptchaKeys.Invisible;
            }
            model.UseEmailForLogin = Site.UseEmailForLogin;
            model.LoginInfoTop = Site.LoginInfoTop;
            model.LoginInfoBottom = Site.LoginInfoBottom;
            model.ExternalAuthenticationList = await accountService.GetExternalAuthenticationSchemes();
            // don't disable db auth if there are no social auth providers configured
            model.DisableDbAuth = Site.DisableDbAuth && Site.HasAnySocialAuthEnabled();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.Where(k => ModelState[k].Errors.Count > 0).Select(k => new { propertyName = k, errorMessage = ModelState[k].Errors[0].ErrorMessage });
                var trackedError = errors.FirstOrDefault().errorMessage;
                analytics.HandleLoginFail("Onsite", trackedError).Forget();

                return View(model);
            }

            if ((Site.CaptchaOnLogin) && (!string.IsNullOrEmpty(recaptchaKeys.PrivateKey)))
            {
                var recpatchaSecretKey = recaptchaKeys.PrivateKey;
                var captchaResponse = await this.ValidateRecaptcha(Request, recpatchaSecretKey);

                if (!captchaResponse.Success)
                {
                    analytics.HandleLoginFail("Onsite", "reCAPTCHA Error").Forget();

                    ModelState.AddModelError("recaptchaerror", sr["reCAPTCHA Error occured. Please try again"]);
                    return View(model);
                }
            }

            var result = await accountService.TryLogin(model);
            if (result.SignInResult.Succeeded)
            {
                return await HandleLoginSuccess(result, returnUrl);
            }

            foreach (var reason in result.RejectReasons)
            {
                //these reasons are not meant to be shown in the ui
                // but we can log them so admin will see failed attempts in the log along with reasons
                log.LogWarning(reason);
            }
            
            if (result.SignInResult.IsNotAllowed)
            {
                return await HandleLoginNotAllowed(result);
            }
            
            if (result.SignInResult.RequiresTwoFactor)
            {
                return await HandleRequiresTwoFactor(result, returnUrl, model.RememberMe);
            }

            if (result.SignInResult.IsLockedOut)
            {
                return await HandleLockout(result);
            }
            else
            {
                analytics.HandleLoginFail("Onsite", sr["Invalid login attempt."]).Forget();

                log.LogInformation($"login did not succeed for {model.Email}");
                ModelState.AddModelError(string.Empty, sr["Invalid login attempt."]);
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            ViewData["Title"] = sr["Two-factor authentication"];

            // Ensure the user has gone through the username & password screen first
            var user = await accountService.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            ViewData["Title"] = sr["Two-factor authentication"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var result = await accountService.Try2FaLogin(model, rememberMe);

            if (result.SignInResult.Succeeded)
            {
                return await HandleLoginSuccess(result, returnUrl);
            }

            foreach (var reason in result.RejectReasons)
            {
                //these reasons are not meant to be shown in the ui
                // but we can log them so admin will see failed attempts in the log along with reasons
                log.LogWarning(reason);
            }

            if (result.SignInResult.IsNotAllowed)
            {
                return await HandleLoginNotAllowed(result);
            }
            
            if (result.SignInResult.IsLockedOut)
            {
                return await HandleLockout(result);
            }
            else
            {
                analytics.HandleLoginFail("Onsite", sr["Invalid authenticator code."]).Forget();


                ModelState.AddModelError(string.Empty, sr["Invalid authenticator code."]);
                return View(model);
            }


        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            ViewData["Title"] = sr["Recovery code verification"];

            // Ensure the user has gone through the username & password screen first
            var user = await accountService.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            ViewData["Title"] = sr["Recovery code verification"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var result = await accountService.TryLoginWithRecoveryCode(model);

            if (result.SignInResult.Succeeded)
            {
                return await HandleLoginSuccess(result, returnUrl);
            }

            foreach (var reason in result.RejectReasons)
            {
                //these reasons are not meant to be shown in the ui
                // but we can log them so admin will see failed attempts in the log along with reasons
                log.LogWarning(reason);
            }

            if (result.SignInResult.IsNotAllowed)
            {
                return await HandleLoginNotAllowed(result);
            }

            if (result.SignInResult.IsLockedOut)
            {
                return await HandleLockout(result);
            }
            else
            {
                analytics.HandleLoginFail("Onsite", sr["Invalid recovery code entered."]).Forget();


                ModelState.AddModelError(string.Empty, sr["Invalid recovery code entered."]);
                return View(model);
            }


        }


        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            if(accountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(Site);
            }
            if(!Site.AllowNewRegistration)
            {
                return new StatusCodeResult(404);
            }
            // login is equivalent to register for new social auth users
            // if db auth is disabled just redirect
            if(Site.DisableDbAuth && Site.HasAnySocialAuthEnabled())
            {
                return RedirectToAction("Login");
            }

            ViewData["Title"] = sr["Register"];

            returnUrl = identityServerIntegration.EnsureFolderSegmentIfNeeded(Site, returnUrl);
            //identityserver integration point
            var idProvider = await identityServerIntegration.GetAuthorizationContextAsync(returnUrl);

            if (!string.IsNullOrEmpty(idProvider))
            {
                // if IdP is passed, then bypass showing the login screen
                return ExternalLogin(idProvider, returnUrl);
            }
            ViewData["ReturnUrl"] = returnUrl;

            var model = new RegisterViewModel();
            model.SiteId = Site.Id;
            
            if ((Site.CaptchaOnRegistration)&& (Site.RecaptchaPublicKey.Length > 0))
            {
                model.RecaptchaSiteKey = Site.RecaptchaPublicKey;
                model.UseInvisibleCaptcha = Site.UseInvisibleRecaptcha;
            }
            model.UseEmailForLogin = Site.UseEmailForLogin;
            model.RegistrationPreamble = Site.RegistrationPreamble;
            model.RegistrationAgreement = Site.RegistrationAgreement;
            model.AgreementRequired = Site.RegistrationAgreement.Length > 0;
            model.ExternalAuthenticationList = await accountService.GetExternalAuthenticationSchemes();

            var viewName = await customRegistration.GetRegisterViewName(Site, HttpContext);

            await customRegistration.HandleRegisterGet(
                Site,
                model,
                HttpContext,
                ViewData);

            return View(viewName, model);
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["Title"] = sr["Register"];

            ViewData["ReturnUrl"] = returnUrl;

            analytics.HandleRegisterSubmit("Onsite").Forget();

            if ((Site.CaptchaOnRegistration) && (Site.RecaptchaPublicKey.Length > 0))
            {
                model.RecaptchaSiteKey = Site.RecaptchaPublicKey;
                model.UseInvisibleCaptcha = Site.UseInvisibleRecaptcha;
            }
            model.UseEmailForLogin = Site.UseEmailForLogin;
            model.RegistrationPreamble = Site.RegistrationPreamble;
            model.RegistrationAgreement = Site.RegistrationAgreement;
            model.AgreementRequired = Site.RegistrationAgreement.Length > 0;
            model.ExternalAuthenticationList = await accountService.GetExternalAuthenticationSchemes();

            bool isValid = ModelState.IsValid;
            
            bool customDataIsValid = await customRegistration.HandleRegisterValidation(
                Site,
                model,
                HttpContext,
                ViewData,
                ModelState);

            if (isValid && customDataIsValid)
            {
                if ((Site.CaptchaOnRegistration) && (Site.RecaptchaPublicKey.Length > 0))
                {
                    string recpatchaSecretKey = Site.RecaptchaPrivateKey;

                    var captchaResponse = await this.ValidateRecaptcha(Request, recpatchaSecretKey);

                    if (!captchaResponse.Success)
                    {
                        ModelState.AddModelError("recaptchaerror", sr["reCAPTCHA Error occured. Please try again"]);
                        isValid = false;
                    }
                }

                if (Site.RegistrationAgreement.Length > 0)
                {
                    if (!model.AgreeToTerms)
                    {
                        ModelState.AddModelError("agreementerror", sr["You must agree to the terms"]);
                        isValid = false;
                    }
                }

                var viewName = await customRegistration.GetRegisterViewName(Site, HttpContext);

                if (!isValid || !customDataIsValid)
                {
                    return View(viewName, model);
                }
                
                var result = await accountService.TryRegister(model, ModelState, HttpContext, customRegistration);

                if (result.SignInResult.Succeeded || (result.SignInResult.IsNotAllowed && result.User != null))
                {
                    await customRegistration.HandleRegisterPostSuccess(
                        Site,
                        model,
                        HttpContext,
                        result);

                    if(result.SignInResult.Succeeded)
                    {
                        return await HandleLoginSuccess(result, returnUrl);
                    }
                    
                }
                
                foreach (var reason in result.RejectReasons)
                {
                    //these reasons are not meant to be shown in the ui
                    // but we can log them so admin will see failed attempts in the log along with reasons
                    log.LogWarning(reason);
                }

                if (result.SignInResult.IsNotAllowed)
                {
                    return await HandleLoginNotAllowed(result);
                }

                var te = result.RejectReasons.FirstOrDefault();
                if(string.IsNullOrEmpty(te)) { te = "unknown"; }

                analytics.HandleRegisterFail("Onsite", te).Forget();

                log.LogInformation($"registration did not succeed for {model.Email}");
               // ModelState.AddModelError(string.Empty, sr["Invalid registration attempt."]);
                return View(viewName, model);           
            }

            var errors = ModelState.Keys.Where(k => ModelState[k].Errors.Count > 0).Select(k => new { propertyName = k, errorMessage = ModelState[k].Errors[0].ErrorMessage });
            var trackedError = errors.FirstOrDefault().errorMessage;
            analytics.HandleRegisterFail("Onsite", trackedError).Forget();

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            log.LogDebug("ExternalLogin called for " + provider + " with returnurl " + returnUrl);

            analytics.HandleLoginSubmit(provider).Forget();

            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = accountService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            log.LogDebug("ExternalLoginCallback called with returnurl " + returnUrl);

            if (remoteError != null)
            {
                var errormessage = string.Format(sr["Error from external provider: {0}"], remoteError);

                analytics.HandleLoginFail("Social", errormessage).Forget();

                ModelState.AddModelError("providererror", errormessage);
                return RedirectToAction("Login");
            }

            var result = await accountService.TryExternalLogin();

            if (result.SignInResult.Succeeded)
            {
                if (result.User != null)
                {
                    if (result.MustAcceptTerms)
                    {
                        await analytics.HandleLoginSuccess(result);
                        
                        return RedirectToAction("TermsOfUse");
                    }
                }
                return await HandleLoginSuccess(result, returnUrl);
            }

            foreach (var reason in result.RejectReasons)
            {
                // these reasons are not meant to be shown in the ui
                // but we can log them so admin will see failed attempts in the log along with reasons
                log.LogWarning(reason);
            }

            if (result.SignInResult.IsNotAllowed)
            {
                return await HandleLoginNotAllowed(result);
            }

            if (result.ExternalLoginInfo == null)
            {
                log.LogDebug("ExternalLoginCallback redirecting to login because GetExternalLoginInfoAsync returned null ");
                return RedirectToAction(nameof(Login));
            }

            if (result.SignInResult.RequiresTwoFactor)
            {
                return await HandleRequiresTwoFactor(result, returnUrl, false);
            }

            if (result.SignInResult.IsLockedOut)
            {
                return await HandleLockout(result); 
            }

            // result.Failed

            

            // If the user does not have an account, then ask the user to create an account.
            // check the claims from the provider to see if we have what we need
            // only need to show this form if there is no email or if there is a required registration agreement

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["LoginProvider"] = result.ExternalLoginInfo.LoginProvider;
            var email = result.ExternalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            var model = new ExternalLoginConfirmationViewModel();
            model.Email = email;
            model.RegistrationPreamble = Site.RegistrationPreamble;
            model.RegistrationAgreement = Site.RegistrationAgreement;
            model.AgreementRequired = Site.RegistrationAgreement.Length > 0;
            return View("ExternalLoginConfirmation", model);
        }

        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            // this is posted if the SiteUser has not been created yet 
            log.LogDebug("ExternalLoginConfirmation called with returnurl " + returnUrl);

            if (accountService.IsSignedIn(User)) // this should be false
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                var result = await accountService.TryExternalLogin(model.Email, model.AgreeToTerms);
                if (result.SignInResult.Succeeded)
                {
                    return await HandleLoginSuccess(result, returnUrl);
                }

                foreach (var reason in result.RejectReasons)
                {
                    // these reasons are not meant to be shown in the ui
                    // but we can log them so admin will see failed attempts in the log along with reasons
                    log.LogWarning(reason);
                }

                if (result.SignInResult.IsNotAllowed)
                {
                    return await HandleLoginNotAllowed(result);
                }

                if (result.ExternalLoginInfo == null)
                {
                    log.LogWarning("ExternalLoginInfo was null");

                    analytics.HandleLoginFail("Social", "ExternalLoginInfo was null").Forget();

                    return View("ExternalLoginFailure");
                }

            }
            else
            {
                log.LogDebug("ExternalLoginConfirmation called with ModelStateInvalid ");
                model.RegistrationPreamble = Site.RegistrationPreamble;
                model.RegistrationAgreement = Site.RegistrationAgreement;
                model.AgreementRequired = Site.RegistrationAgreement.Length > 0;
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PendingApproval(Guid userId, bool didSend = false)
        {
            if (accountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(Site);
            }

            var model = new PendingNotificationViewModel();
            model.UserId = userId;
            model.DidSend = didSend;

            return View("PendingApproval", model);
        }

        [HttpGet]
        public async Task<IActionResult> TermsOfUse()
        {
            if (!accountService.IsSignedIn(User) || string.IsNullOrWhiteSpace(Site.RegistrationAgreement))
            {
                return this.RedirectToSiteRoot(Site);
            }

            ViewData["Title"] = sr["Registration Agreement Required"];

            var model = new AcceptTermsViewModel();
            model.TermsUpdatedDate = await timeZoneHelper.ConvertToLocalTime(Site.TermsUpdatedUtc);
            model.AgreementRequired = true;
            model.RegistrationAgreement = Site.RegistrationAgreement;
            model.RegistrationPreamble = Site.RegistrationPreamble;

            return View(model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TermsOfUse(AcceptTermsViewModel model)
        {
            if (!accountService.IsSignedIn(User) || string.IsNullOrWhiteSpace(Site.RegistrationAgreement))
            {
                return this.RedirectToSiteRoot(Site);
            }

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = sr["Registration Agreement Required"];
                model.TermsUpdatedDate = await timeZoneHelper.ConvertToLocalTime(Site.TermsUpdatedUtc);
                model.AgreementRequired = true;
                model.RegistrationAgreement = Site.RegistrationAgreement;
                model.RegistrationPreamble = Site.RegistrationPreamble;

                return View(model);
            }

            var result = await accountService.AcceptRegistrationAgreement(User);
            //return Redirect("/");
            if(result)
            {
                return this.RedirectToSiteRoot(Site);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult EmailConfirmationRequired(Guid userId, bool didSend = false)
        {
            if (accountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(Site);
            }
            var model = new PendingNotificationViewModel();
            model.UserId = userId;
            model.DidSend = didSend;

            return View("EmailConfirmationRequired", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyEmail(Guid userId)
        {
            var info = await accountService.GetEmailVerificationInfo(userId);
            
            if(info.User == null)
            {
                return this.RedirectToSiteRoot(Site);
            }

            if(info.User.EmailConfirmed)
            {
                return this.RedirectToSiteRoot(Site);
            }

            var callbackUrl = Url.Action("ConfirmEmail", "Account",
                            new { userId = info.User.Id.ToString(), code = info.EmailVerificationToken },
                            protocol: HttpContext.Request.Scheme);

            emailSender.SendAccountConfirmationEmailAsync(
                            Site,
                            info.User.Email,
                            sr["Confirm your account"],
                            callbackUrl).Forget();

            await ipAddressTracker.TackUserIpAddress(Site.Id, info.User.Id);

            return RedirectToAction("EmailConfirmationRequired", new { userId = info.User.Id, didSend = true });
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (accountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(Site);
            }
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var result = await accountService.ConfirmEmailAsync(userId, code);
            
            if (result.User == null)
            {
                return View("Error");
            }
            
            if(result.IdentityResult.Succeeded)
            {
                if (Site.RequireApprovalBeforeLogin && !result.User.AccountApproved)
                {
                    await emailSender.AccountPendingApprovalAdminNotification(Site, result.User).ConfigureAwait(false);
                    return RedirectToAction("PendingApproval", new { userId = result.User.Id, didSend = true });      
                }
            }
            
            return View(result.IdentityResult.Succeeded ? "ConfirmEmail" : "Error");
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await accountService.SignOutAsync();
            //return Redirect("/");
            return this.RedirectToSiteRoot(Site);
        }

        // identityserver integration point
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var clientId = await identityServerIntegration.GetLogoutContextClientIdAsync(logoutId);
            if (!string.IsNullOrEmpty(clientId))
            {
                // if the logout request is authenticated, it's safe to automatically sign-out
                return await Logout(new IdentityServerLogoutViewModel { LogoutId = logoutId });
            }

            var vm = new IdentityServerLogoutViewModel
            {
                LogoutId = logoutId
            };

            return View(vm);
        }

        // identityserver integration point
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(IdentityServerLogoutViewModel model)
        {
            await accountService.SignOutAsync();
            // set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logoutModel = await identityServerIntegration.GetLogoutContextModelAsync(model.LogoutId);
            
            return View("LoggedOut", logoutModel);
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> UsernameAvailable(Guid? userId, string userName)
        {
            if(string.IsNullOrWhiteSpace(userName))
            {
                return Json(false);
            }
            // same validation is used when editing or creating a user
            // if editing then the loginname is valid if found attached to the selected user
            // otherwise if found it is not already in use and not available
            Guid selectedUserGuid = Guid.Empty;
            if (userId.HasValue) { selectedUserGuid = userId.Value; }
            bool available = await accountService.LoginNameIsAvailable(selectedUserGuid, userName);

            
            
            return Json(available);
        }

        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {    
            if (ModelState.IsValid)
            {
                var info = await accountService.GetPasswordResetInfo(model.Email);
                if (info.User == null || (Site.RequireConfirmedEmail && !(info.User.EmailConfirmed)))
                {
                    if(info.User != null)
                    {
                        log.LogWarning(info.User.Email + ": user tried to use pasword recovery but no email was sent because user email is not confirmed and security settings require confirmed email");
                    }
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                var resetUrl = Url.Action("ResetPassword", "Account", 
                    new { userId = info.User.Id.ToString(), code = info.PasswordResetToken }, 
                    protocol: HttpContext.Request.Scheme);

                // await emailSender.SendPasswordResetEmailAsync(
                // not awaiting this awaitable method on purpose
                // so it does not delay the ui response
                // vs would show a warning squiggly line here if not for .Forget()
                //http://stackoverflow.com/questions/35175960/net-core-alternative-to-threadpool-queueuserworkitem
                //http://stackoverflow.com/questions/22629951/suppressing-warning-cs4014-because-this-call-is-not-awaited-execution-of-the
                emailSender.SendPasswordResetEmailAsync(
                    Site,
                    model.Email,
                    sr["Reset Password"],
                    resetUrl).Forget();
                

                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        { 
            return View();
        }


        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {   
            return code == null ? View("Error") : View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await accountService.ResetPassword(model.Email, model.Password, model.Code);

            if (result.User == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            
            if (result.IdentityResult.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result.IdentityResult);
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        
        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var twoFactorInfo = await accountService.GetTwoFactorInfo();
            
            if (twoFactorInfo.User == null)
            {
                return View("Error");
            }
            
            var factorOptions = twoFactorInfo.UserFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        { 
            if (!ModelState.IsValid)
            {
                return View();
            }

            var twoFactorInfo = await accountService.GetTwoFactorInfo(model.SelectedProvider);
            if (twoFactorInfo.User == null)
            {
                return View("Error");
            }

            if (string.IsNullOrWhiteSpace(twoFactorInfo.TwoFactorToken))
            {
                return View("Error");
            }
            
            if (model.SelectedProvider == "Email")
            {
                await emailSender.SendSecurityCodeEmailAsync(
                    Site,
                    twoFactorInfo.User.Email, 
                    sr["Security Code"], 
                    twoFactorInfo.TwoFactorToken);
            }
            else if (model.SelectedProvider == "Phone")
            {
                var message = string.Format(sr["Your security code is: {0}"], twoFactorInfo.TwoFactorToken);
                await smsSender.SendSmsAsync(Site, twoFactorInfo.User.PhoneNumber, message);
            }

            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }


        // GET: /Account/VerifyCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {   
            // Require that the user has already logged in via username/password or external login
            var user = await accountService.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {   
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await accountService.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return LocalRedirect(model.ReturnUrl);
                }

                return this.RedirectToSiteRoot(Site);
            }

            if (result.IsLockedOut)
            {
                return await HandleLockout();
            }
            else
            {
                ModelState.AddModelError("", sr["Invalid code."]);
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            ViewData["Title"] = sr["Oops something went wrong"];
            return View();
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
