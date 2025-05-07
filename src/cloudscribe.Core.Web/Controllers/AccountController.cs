﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2022-02-08
//

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Site;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Common.Models;
using cloudscribe.Web.Common.Recaptcha;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers.Mvc
{

    public class AccountController : Controller
    {

        public AccountController(
            IAccountService accountService,
            SiteContext currentSite,
            IpAddressTracker ipAddressTracker,
            ISiteMessageEmailSender emailSender,
            SiteTimeZoneService timeZoneHelper,
            IIdentityServerIntegration identityServerIntegration,
            IStringLocalizer<CloudscribeCore> localizer,
            IRecaptchaKeysProvider recaptchaKeysProvider,
            IRecaptchaServerSideValidator recaptchaServerSideValidator,
            IHandleCustomRegistration customRegistration,
            Analytics.IHandleAccountAnalytics analyticsHandler,
            IHttpContextAccessor httpContextAccessor,
            RemainingSessionTimeResolver remainingSessionTimeResolver,
            SiteUserManager<SiteUser> userManager,
            SignInManager<SiteUser> signInManager,
            ILogger<AccountController> logger,
            IEmailValidationService emailValidationService
            )
        {
            AccountService               = accountService;
            CurrentSite                  = currentSite;
            IdentityServerIntegration    = identityServerIntegration;
            EmailSender                  = emailSender;
            IpAddressTracker             = ipAddressTracker;
            StringLocalizer              = localizer;
            Log                          = logger;
            RecaptchaKeysProvider        = recaptchaKeysProvider;
            RecaptchaServerSideValidator = recaptchaServerSideValidator;
            TimeZoneHelper               = timeZoneHelper;
            CustomRegistration           = customRegistration;
            Analytics                    = analyticsHandler;
            HttpContextAccessor          = httpContextAccessor;
            RemainingSessionTimeResolver = remainingSessionTimeResolver;
            UserManager = userManager;
            SignInManager = signInManager;
            EmailValidationService = emailValidationService;
        }

        protected IAccountService AccountService { get; private set; }
        protected ISiteContext CurrentSite { get; private set; }
        protected IIdentityServerIntegration IdentityServerIntegration { get; private set; }
        protected ISiteMessageEmailSender EmailSender { get; private set; }
        protected IpAddressTracker IpAddressTracker { get; private set; }
        protected ILogger Log { get; private set; }
        protected IStringLocalizer StringLocalizer { get; private set; }
        protected IRecaptchaKeysProvider RecaptchaKeysProvider { get; private set; }
        protected IRecaptchaServerSideValidator RecaptchaServerSideValidator { get; private set; }
        protected SiteTimeZoneService TimeZoneHelper { get; private set; }
        protected IHandleCustomRegistration CustomRegistration { get; private set; }
        protected Analytics.IHandleAccountAnalytics Analytics { get; private set; }
        protected RemainingSessionTimeResolver RemainingSessionTimeResolver { get; private set; }
        protected IHttpContextAccessor HttpContextAccessor;
        protected SiteUserManager<SiteUser> UserManager { get; private set; }
        public SignInManager<SiteUser> SignInManager { get; }
        public IEmailValidationService EmailValidationService { get; }

        protected virtual async Task<IActionResult> HandleLoginSuccess(UserLoginResult result, string returnUrl)
        {
            await Analytics.HandleLoginSuccess(result);

            if (result.User != null)
            {
                await IpAddressTracker.TackUserIpAddress(CurrentSite.Id, result.User.Id);
            }

            if (result.PasswordExpiryReminder != null)
            {
                this.AlertDanger(StringLocalizer[result.PasswordExpiryReminder], true);
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                // when site is closed login is still allowed
                // but don't redirect to closed paged
                if(
                    (!returnUrl.Contains("/closed"))
                    && (!returnUrl.Contains("/oops/error"))
                    && (!returnUrl.Contains("/offline"))
                    && (!returnUrl.Contains("/account/logout"))
                    && (!returnUrl.Contains("/manage/changepassword"))
                    // also don't go back to password change confirmation if we just changed passwd
                    && (!returnUrl.Contains("/account/resetpasswordconfirmation"))
                    // and likewise if auto-logged-out
                    && (!returnUrl.Contains("/account/autologoutnotification"))
                    )
                {
                    return LocalRedirect(returnUrl);
                }

            }

            return this.RedirectToSiteRoot(CurrentSite);
        }

        protected virtual bool ShouldSendConfirmation(IUserContext user)
        {
            if (user.EmailConfirmSentUtc == null) return true; //never sent yet
            var timeSpan = DateTime.UtcNow - user.EmailConfirmSentUtc;
            if (timeSpan.Value != null && timeSpan.Value.TotalDays > 1) return true; // at most resend once per day if user tries to login

            return false;
        }

        protected virtual async Task<IActionResult> HandleLoginNotAllowed(UserLoginResult result, string returnUrl)
        {
            await Analytics.HandleLoginNotAllowed(result);

            if (result.User != null)
            {
                await IpAddressTracker.TackUserIpAddress(CurrentSite.Id, result.User.Id);

                if (result.NeedsEmailConfirmation)
                {
                    if(ShouldSendConfirmation(result.User))
                    {
                        var callbackUrl = Url.Action(new UrlActionContext
                        {
                            Action = "ConfirmEmail",
                            Controller = "Account",
                            Values = new { userId = result.User.Id.ToString(), code = result.EmailConfirmationToken, returnUrl },
                            Protocol = HttpContext.Request.Scheme
                        });

                        await EmailSender.SendAccountConfirmationEmailAsync(
                            CurrentSite,
                            result.User.Email,
                            StringLocalizer["Confirm your account"],
                            callbackUrl,
                            result.EmailConfirmationToken
                            );


                        this.AlertSuccess(StringLocalizer["Please check your email inbox, we just sent you a link that you need to click to confirm your account"], true);
                    }

                    return RedirectToAction("EmailConfirmationRequired", new { userId = result.User.Id, didSend = true, returnUrl });
                }

                if (result.NeedsAccountApproval)
                {
                    var timeSpan = DateTime.UtcNow - result.User.CreatedUtc;
                    if (timeSpan.TotalDays < 1)
                    {
                        // account was just created so send notification to approver
                        await EmailSender.AccountPendingApprovalAdminNotification(CurrentSite, result.User);
                    }

                    return RedirectToAction("PendingApproval", "Account", new { userId = result.User.Id, didSend = true });

                }
            }

            return this.RedirectToSiteRoot(CurrentSite);
        }

        protected virtual async Task<IActionResult> HandleRequiresTwoFactor(UserLoginResult result, string returnUrl, bool rememberMe)
        {
            await Analytics.HandleRequiresTwoFactor(result);

            if (result.User != null)
            {
                await IpAddressTracker.TackUserIpAddress(CurrentSite.Id, result.User.Id);

                Log.LogWarning($"redirecting from login for {result.User.Email} because 2 factor not configured yet for account");
            }

            return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, rememberMe });

            //return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        protected virtual async Task<IActionResult> HandleLockout(UserLoginResult result = null)
        {
            await Analytics.HandleLockout(result);

            ViewData["Title"] = StringLocalizer["Locked out"];

            if (result != null && result.User != null)
            {
                await IpAddressTracker.TackUserIpAddress(CurrentSite.Id, result.User.Id);

                Log.LogWarning($"redirecting to lockout page for {result.User.Email} because account is locked");
            }

            return View("Lockout");
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> Login(string returnUrl = null)
        {
            if (AccountService.IsSignedIn(User))
            {
                if (returnUrl != null)
                {
                    return LocalRedirect(returnUrl);
                }
                return this.RedirectToSiteRoot(CurrentSite);
            }

            returnUrl = IdentityServerIntegration.EnsureFolderSegmentIfNeeded(CurrentSite, returnUrl);
            //identityserver integration point
            var idProvider = await IdentityServerIntegration.GetAuthorizationContextAsync(returnUrl);

            if (!string.IsNullOrEmpty(idProvider))
            {
                // if IdP is passed, then bypass showing the login screen
                return await ExternalLogin(idProvider, returnUrl);
            }

            ViewData["Title"] = StringLocalizer["Log In"];
            ViewData["ReturnUrl"] = returnUrl;

            var model = new LoginViewModel();

            var recaptchaKeys = await RecaptchaKeysProvider.GetKeys().ConfigureAwait(false);

            if ((CurrentSite.CaptchaOnLogin)&& (!string.IsNullOrEmpty(recaptchaKeys.PublicKey)))
            {
                model.RecaptchaSiteKey = recaptchaKeys.PublicKey;
                model.UseInvisibleCaptcha = recaptchaKeys.Invisible;
            }
            model.UseEmailForLogin = CurrentSite.UseEmailForLogin;
            model.LoginInfoTop = CurrentSite.LoginInfoTop;
            model.LoginInfoBottom = CurrentSite.LoginInfoBottom;
            var externalSchemes = await AccountService.GetExternalAuthenticationSchemes();
            model.ExternalAuthenticationList = externalSchemes.ToList();
            // don't disable db auth if there are no social auth providers configured
            model.DisableDbAuth = CurrentSite.DisableDbAuth && CurrentSite.HasAnySocialAuthEnabled();

            return View(model);
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["Title"] = StringLocalizer["Log In"];
            ViewData["ReturnUrl"] = returnUrl;

            await Analytics.HandleLoginSubmit("Onsite");

            var recaptchaKeys = await RecaptchaKeysProvider.GetKeys().ConfigureAwait(false);
            if ((CurrentSite.CaptchaOnLogin) && (!string.IsNullOrEmpty(recaptchaKeys.PublicKey)))
            {
                model.RecaptchaSiteKey = recaptchaKeys.PublicKey;
                model.UseInvisibleCaptcha = recaptchaKeys.Invisible;
            }
            model.UseEmailForLogin = CurrentSite.UseEmailForLogin;
            model.LoginInfoTop = CurrentSite.LoginInfoTop;
            model.LoginInfoBottom = CurrentSite.LoginInfoBottom;
            model.ExternalAuthenticationList = await AccountService.GetExternalAuthenticationSchemes();
            // don't disable db auth if there are no social auth providers configured
            model.DisableDbAuth = CurrentSite.DisableDbAuth && CurrentSite.HasAnySocialAuthEnabled();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.Where(k => ModelState[k].Errors.Count > 0).Select(k => new { propertyName = k, errorMessage = ModelState[k].Errors[0].ErrorMessage });
                var trackedError = errors.FirstOrDefault().errorMessage;
                await Analytics.HandleLoginFail("Onsite", trackedError);

                return View(model);
            }

            if ((CurrentSite.CaptchaOnLogin) && (!string.IsNullOrEmpty(recaptchaKeys.PrivateKey)))
            {
                var captchaResponse = await RecaptchaServerSideValidator.ValidateRecaptcha(Request, CurrentSite.RecaptchaPrivateKey);
                if (!captchaResponse.Success)
                {
                    await Analytics.HandleLoginFail("Onsite", "reCAPTCHA Error");
                    ModelState.AddModelError("recaptchaerror", StringLocalizer["reCAPTCHA Error occured. Please try again"]);
                    return View(model);
                }
            }

            var result = await AccountService.TryLogin(model);
            if (result.SignInResult.Succeeded)
            {
                Log.LogInformation($"login succeeded for {model.UserName}");
                return await HandleLoginSuccess(result, returnUrl);
            }

            foreach (var reason in result.RejectReasons)
            {
                //these reasons are not meant to be shown in the ui
                // but we can log them so admin will see failed attempts in the log along with reasons
                Log.LogWarning(reason);
            }

            if (result.SignInResult.IsNotAllowed)
            {
                return await HandleLoginNotAllowed(result, returnUrl);
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
                await Analytics.HandleLoginFail("Onsite", StringLocalizer["Invalid login attempt."]);

                Log.LogInformation($"login did not succeed for {model.UserName}");
                ModelState.AddModelError(string.Empty, StringLocalizer["Invalid login attempt."]);
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            ViewData["Title"] = StringLocalizer["Two-factor authentication"];

            // Ensure the user has gone through the username & password screen first
            var user = await AccountService.GetTwoFactorAuthenticationUserAsync();

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
        public virtual async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            ViewData["Title"] = StringLocalizer["Two-factor authentication"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await AccountService.Try2FaLogin(model, rememberMe);

            if (result.SignInResult.Succeeded)
            {
                return await HandleLoginSuccess(result, returnUrl);
            }

            foreach (var reason in result.RejectReasons)
            {
                //these reasons are not meant to be shown in the ui
                // but we can log them so admin will see failed attempts in the log along with reasons
                Log.LogWarning(reason);
            }

            if (result.SignInResult.IsNotAllowed)
            {
                return await HandleLoginNotAllowed(result, returnUrl);
            }

            if (result.SignInResult.IsLockedOut)
            {
                return await HandleLockout(result);
            }
            else
            {
                await Analytics.HandleLoginFail("Onsite", StringLocalizer["Invalid authenticator code."]);

                ModelState.AddModelError(string.Empty, StringLocalizer["Invalid authenticator code."]);
                return View(model);
            }


        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            ViewData["Title"] = StringLocalizer["Recovery code verification"];

            // Ensure the user has gone through the username & password screen first
            var user = await AccountService.GetTwoFactorAuthenticationUserAsync();
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
        public virtual async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            ViewData["Title"] = StringLocalizer["Recovery code verification"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await AccountService.TryLoginWithRecoveryCode(model);

            if (result.SignInResult.Succeeded)
            {
                return await HandleLoginSuccess(result, returnUrl);
            }

            foreach (var reason in result.RejectReasons)
            {
                //these reasons are not meant to be shown in the ui
                // but we can log them so admin will see failed attempts in the log along with reasons
                Log.LogWarning(reason);
            }

            if (result.SignInResult.IsNotAllowed)
            {
                return await HandleLoginNotAllowed(result, returnUrl);
            }

            if (result.SignInResult.IsLockedOut)
            {
                return await HandleLockout(result);
            }
            else
            {
                await Analytics.HandleLoginFail("Onsite", StringLocalizer["Invalid recovery code entered."]);

                ModelState.AddModelError(string.Empty, StringLocalizer["Invalid recovery code entered."]);
                return View(model);
            }


        }


        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> Register(string returnUrl = null)
        {
            if(AccountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }
            if(!CurrentSite.AllowNewRegistration)
            {
                return new StatusCodeResult(404);
            }
            // login is equivalent to register for new social auth users
            // if db auth is disabled just redirect
            if(CurrentSite.DisableDbAuth && CurrentSite.HasAnySocialAuthEnabled())
            {
                return RedirectToAction("Login");
            }

            ViewData["Title"] = StringLocalizer["Register"];

            returnUrl = IdentityServerIntegration.EnsureFolderSegmentIfNeeded(CurrentSite, returnUrl);
            //identityserver integration point
            var idProvider = await IdentityServerIntegration.GetAuthorizationContextAsync(returnUrl);

            if (!string.IsNullOrEmpty(idProvider))
            {
                // if IdP is passed, then bypass showing the login screen
                return await ExternalLogin(idProvider, returnUrl);
            }
            ViewData["ReturnUrl"] = returnUrl;

            var model = new RegisterViewModel
            {
                SiteId = CurrentSite.Id
            };

            if ((CurrentSite.CaptchaOnRegistration)&& (!string.IsNullOrWhiteSpace(CurrentSite.RecaptchaPublicKey)))
            {
                model.RecaptchaSiteKey = CurrentSite.RecaptchaPublicKey;
                model.UseInvisibleCaptcha = CurrentSite.UseInvisibleRecaptcha;
            }
           // model.UseEmailForLogin = CurrentSite.UseEmailForLogin;
            model.RegistrationPreamble = CurrentSite.RegistrationPreamble;
            model.RegistrationAgreement = CurrentSite.RegistrationAgreement;
            model.AgreementRequired = !string.IsNullOrWhiteSpace(CurrentSite.RegistrationAgreement);
            model.ExternalAuthenticationList = await AccountService.GetExternalAuthenticationSchemes();

            var viewName = await CustomRegistration.GetRegisterViewName(CurrentSite, HttpContext);

            await CustomRegistration.HandleRegisterGet(
                CurrentSite,
                model,
                HttpContext,
                ViewData);

            return View(viewName, model);
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["Title"] = StringLocalizer["Register"];
            ViewData["ReturnUrl"] = returnUrl;

            await Analytics.HandleRegisterSubmit("Onsite");

            if ((CurrentSite.CaptchaOnRegistration) && (!string.IsNullOrWhiteSpace(CurrentSite.RecaptchaPublicKey)))
            {
                model.RecaptchaSiteKey = CurrentSite.RecaptchaPublicKey;
                model.UseInvisibleCaptcha = CurrentSite.UseInvisibleRecaptcha;
            }
            //model.UseEmailForLogin = CurrentSite.UseEmailForLogin;
            model.RegistrationPreamble = CurrentSite.RegistrationPreamble;
            model.RegistrationAgreement = CurrentSite.RegistrationAgreement;
            model.AgreementRequired = !string.IsNullOrWhiteSpace(CurrentSite.RegistrationAgreement);
            model.ExternalAuthenticationList = await AccountService.GetExternalAuthenticationSchemes();

            bool isValid = ModelState.IsValid;

            bool customDataIsValid = await CustomRegistration.HandleRegisterValidation(
                CurrentSite,
                model,
                HttpContext,
                ViewData,
                ModelState);

            EmailValidation emailValidation = EmailValidationService.RegisterEmailValidation(model);

            if (emailValidation.IsValid)
            {
                isValid = true;
            }
            else
            {
                ModelState.AddModelError("registrationEmailError", emailValidation.ErrorMessage);
                isValid = false;
            }

            if (isValid && customDataIsValid)
            {
                if ((CurrentSite.CaptchaOnRegistration) && (!string.IsNullOrWhiteSpace(CurrentSite.RecaptchaPublicKey)))
                {
                    var captchaResponse = await RecaptchaServerSideValidator.ValidateRecaptcha(Request, CurrentSite.RecaptchaPrivateKey);

                    if (!captchaResponse.Success)
                    {
                        ModelState.AddModelError("recaptchaerror", StringLocalizer["reCAPTCHA Error occured. Please try again"]);
                        isValid = false;
                    }
                }

                if (!string.IsNullOrWhiteSpace(CurrentSite.RegistrationAgreement))
                {
                    if (!model.AgreeToTerms)
                    {
                        ModelState.AddModelError("agreementerror", StringLocalizer["You must agree to the terms"]);
                        isValid = false;
                    }
                }

                var viewName = await CustomRegistration.GetRegisterViewName(CurrentSite, HttpContext);

                if (!isValid || !customDataIsValid)
                {
                    return View(viewName, model);
                }

                var result = await AccountService.TryRegister(model, ModelState, HttpContext, CustomRegistration);


                if (result.SignInResult.Succeeded || (result.SignInResult.IsNotAllowed && result.User != null))
                {
                    //only send the new account notification email if we don't require account approval or email verification
                    if (! (CurrentSite.RequireApprovalBeforeLogin || CurrentSite.RequireConfirmedEmail))
                    {
                        await EmailSender.NewAccountAdminNotification(CurrentSite, result.User).ConfigureAwait(false);
                    }

                    await CustomRegistration.HandleRegisterPostSuccess(
                        CurrentSite,
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
                    Log.LogWarning(reason);
                }

                if (result.SignInResult.IsNotAllowed)
                {
                    return await HandleLoginNotAllowed(result, returnUrl);
                }

                var te = result.RejectReasons.FirstOrDefault();
                if(string.IsNullOrEmpty(te)) { te = "unknown"; }

                await Analytics.HandleRegisterFail("Onsite", te);

                if(!ModelState.Any(x => x.Key.Equals("validationError")))
                {
                    // separate pathway for registration attempt on an email address that is already taken
                    if (ModelState.Any(x => x.Key.Equals("duplicateEmail")))
                    {
                        Log.LogInformation($"registration did not succeed for {model.Email} due to the email already existing");

                        var existingUser = await AccountService.GetUserInfo(model.Email);

                        bool needsConfirming = CurrentSite.RequireConfirmedEmail && !existingUser.EmailConfirmed;
                        bool needsApproval = CurrentSite.RequireApprovalBeforeLogin && !existingUser.AccountApproved;

                        string resetUrl = Url.Action("ForgotPassword", "Account", null, protocol: HttpContext.Request.Scheme);
                        string loginUrl = Url.Action("Login", "Account", null, protocol: HttpContext.Request.Scheme);

                        string confirmUrl = null;

                        if (needsConfirming)
                        {
                            var verifyEmailInfo = await AccountService.GetEmailVerificationInfo(existingUser.Id);
                            confirmUrl = Url.Action(
                                "ConfirmEmail",
                                "Account",
                                new { UserId = existingUser.Id, Code = verifyEmailInfo.EmailVerificationToken },
                                protocol: HttpContext.Request.Scheme
                            );
                        }

                        await EmailSender.SendAccountExistsEmailAsync(
                            CurrentSite,
                            model.Email,
                            StringLocalizer["A new user registration was attempted with your email address"],
                            resetUrl,
                            loginUrl,
                            confirmUrl,
                            needsApproval);

                        this.AlertSuccess(StringLocalizer["Please check your email inbox, we just sent you a link that you need to click to confirm your account"], true);

                        return RedirectToAction("EmailConfirmationRequired", new { userId = existingUser.Id, didSend = true, returnUrl });
                    }
                }

                Log.LogInformation($"registration did not succeed for {model.Email}");
                ModelState.AddModelError("registrationError", StringLocalizer["Invalid registration attempt."]);
                return View(viewName, model);
            }

            var errors = ModelState.Keys.Where(k => ModelState[k].Errors.Count > 0).Select(k => new { propertyName = k, errorMessage = ModelState[k].Errors[0].ErrorMessage });
            var trackedError = errors.FirstOrDefault().errorMessage;
            await Analytics.HandleRegisterFail("Onsite", trackedError);

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ExternalLogin(string provider, string returnUrl = null)
        {
            Log.LogDebug("ExternalLogin called for " + provider + " with returnurl " + returnUrl);

            await Analytics.HandleLoginSubmit(provider);

            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = AccountService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            Log.LogDebug("ExternalLoginCallback called with returnurl " + returnUrl);

            if (remoteError != null)
            {
                var errormessage = string.Format(StringLocalizer["Error from external provider: {0}"], remoteError);

                await Analytics.HandleLoginFail("Social", errormessage);

                ModelState.AddModelError("providererror", errormessage);
                return RedirectToAction("Login");
            }

            var result = await AccountService.TryExternalLogin();

            if (result.SignInResult.Succeeded)
            {
                if (result.User != null)
                {
                    if (result.MustAcceptTerms)
                    {
                        await Analytics.HandleLoginSuccess(result);

                        return RedirectToAction("TermsOfUse");
                    }
                    if (result.IsNewExternalAuthMapping)
                    {
                        // external login has created a new UserLogin entity, requiring email notification
                        await EmailSender.SendNewExternalLoginMappingEmailAsync(
                            CurrentSite,
                            result.User.Email,
                            StringLocalizer[$"A new external login user registration has been created"],
                            result.ExternalLoginInfo.ProviderKey,
                            result.ExternalLoginInfo.ProviderDisplayName,
                            Url.Action("ManageLogins", "Manage", null, HttpContext.Request.Scheme)
                            );
                    }
                }

                return await HandleLoginSuccess(result, returnUrl);
            }

            foreach (var reason in result.RejectReasons)
            {
                // these reasons are not meant to be shown in the ui
                // but we can log them so admin will see failed attempts in the log along with reasons
                Log.LogWarning(reason);
            }

            if (result.RejectReasons.Contains("Site is not configured to allow new user registrations") )
            {
                return RedirectToAction(nameof(Login));
            }

            if (result.SignInResult.IsNotAllowed)
            {
                return await HandleLoginNotAllowed(result, returnUrl);
            }

            if (result.ExternalLoginInfo == null)
            {
                Log.LogDebug("ExternalLoginCallback redirecting to login because GetExternalLoginInfoAsync returned null ");
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
            if (String.IsNullOrEmpty(email))
            {
                var emailClaim_upn = result.ExternalLoginInfo.Principal.Claims.Where(x => x.Type == ClaimTypes.Upn).FirstOrDefault();
                if (emailClaim_upn != null && IsValidEmail(emailClaim_upn.Value))
                {
                    email = emailClaim_upn.Value;
                }
                else
                {
                    var emailClaim_name = result.ExternalLoginInfo.Principal.Claims.Where(x => x.Type == ClaimTypes.Name)
                                                                                    .Where(x => IsValidEmail(x.Value)).FirstOrDefault();
                    if (emailClaim_name != null)
                    {
                        email = emailClaim_name.Value;
                    }
                }
            }

            var model = new ExternalLoginConfirmationViewModel
            {
                Email = email,
                RegistrationPreamble = CurrentSite.RegistrationPreamble,
                RegistrationAgreement = CurrentSite.RegistrationAgreement,
                AgreementRequired = !string.IsNullOrWhiteSpace(CurrentSite.RegistrationAgreement)
            };
            return View("ExternalLoginConfirmation", model);
        }

        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            // this is posted if the SiteUser has not been created yet
            Log.LogDebug("ExternalLoginConfirmation called with returnurl " + returnUrl);

            if (AccountService.IsSignedIn(User)) // this should be false
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                var result = await AccountService.TryExternalLogin(model.Email, model.AgreeToTerms);
                if (result.SignInResult.Succeeded)
                {
                    if (result.IsNewExternalAuthMapping)
                    {
                        // external login has created a new UserLogin entity, requiring email notification
                        await EmailSender.SendNewExternalLoginMappingEmailAsync(
                            CurrentSite,
                            model.Email,
                            StringLocalizer[$"A new external login user registration has been created"],
                            result.ExternalLoginInfo.ProviderKey,
                            result.ExternalLoginInfo.ProviderDisplayName,
                            Url.Action("ManageLogins", "Manage", null, HttpContext.Request.Scheme)
                            );
                    }

                    return await HandleLoginSuccess(result, returnUrl);
                }

                foreach (var reason in result.RejectReasons)
                {
                    // these reasons are not meant to be shown in the ui
                    // but we can log them so admin will see failed attempts in the log along with reasons
                    Log.LogWarning(reason);
                }

                if (result.SignInResult.IsNotAllowed)
                {
                    return await HandleLoginNotAllowed(result, returnUrl);
                }

                if (result.ExternalLoginInfo == null)
                {
                    Log.LogWarning("ExternalLoginInfo was null");

                    await Analytics.HandleLoginFail("Social", "ExternalLoginInfo was null");
                }
                string message = string.Empty;
                var isExistingAccount = await AccountService.IsExistingAccount(model.Email);
                if(isExistingAccount)
                {
                    // message = StringLocalizer["The provided email address is already in use."];
                    // don't reveal the above to the world
                    message  = StringLocalizer["Your external login service cannot be registered."];

                    if (!CurrentSite.DisableDbAuth)
                        message += StringLocalizer[" You can still login directly with your stardard username and password, and then register an external login service from the 'My Account' page."];
                }



                return View("ExternalLoginFailure", message);

            }
            else
            {
                Log.LogDebug("ExternalLoginConfirmation called with ModelStateInvalid ");
                model.RegistrationPreamble = CurrentSite.RegistrationPreamble;
                model.RegistrationAgreement = CurrentSite.RegistrationAgreement;
                model.AgreementRequired = !string.IsNullOrWhiteSpace(CurrentSite.RegistrationAgreement);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }


        // GET: /Account/RemainingSessionTime
        [HttpGet]
        [Authorize]
        public virtual async Task<IActionResult> RemainingSessionTime()
        {
            // https://stackoverflow.com/questions/41870309/refresh-user-cookie-ticket-in-asp-net-core-identity
            //SiteUser user = await UserManager.GetUserAsync(User);

            //if (SignInManager.IsSignedIn(User))
            //{
            //    await SignInManager.RefreshSignInAsync(user);

            var result = await RemainingSessionTimeResolver.RemainingSessionTimeInSeconds();

            // answer in seconds
            return new JsonResult(result);
        }


        // GET: /Account/AutoLogoutNotification
        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> AutoLogoutNotification()
        {
            ViewData["Title"] = StringLocalizer["Timed out"];
            await Analytics.HandleLogout("Login Timed Out");
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public virtual IActionResult PendingApproval(Guid userId, bool didSend = false)
        {
            if (AccountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            var model = new PendingNotificationViewModel
            {
                UserId = userId,
                DidSend = didSend
            };

            return View("PendingApproval", model);
        }

        [HttpGet]
        public virtual async Task<IActionResult> TermsOfUse()
        {
            if (!AccountService.IsSignedIn(User) || string.IsNullOrWhiteSpace(CurrentSite.RegistrationAgreement))
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            ViewData["Title"] = StringLocalizer["Registration Agreement Required"];

            var model = new AcceptTermsViewModel
            {
                TermsUpdatedDate = await TimeZoneHelper.ConvertToLocalTime(CurrentSite.TermsUpdatedUtc),
                AgreementRequired = true,
                RegistrationAgreement = CurrentSite.RegistrationAgreement,
                RegistrationPreamble = CurrentSite.RegistrationPreamble
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> TermsOfUse(AcceptTermsViewModel model)
        {
            if (!AccountService.IsSignedIn(User) || string.IsNullOrWhiteSpace(CurrentSite.RegistrationAgreement))
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = StringLocalizer["Registration Agreement Required"];
                model.TermsUpdatedDate = await TimeZoneHelper.ConvertToLocalTime(CurrentSite.TermsUpdatedUtc);
                model.AgreementRequired = true;
                model.RegistrationAgreement = CurrentSite.RegistrationAgreement;
                model.RegistrationPreamble = CurrentSite.RegistrationPreamble;

                return View(model);
            }

            var result = await AccountService.AcceptRegistrationAgreement(User);
            //return Redirect("/");
            if(result)
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public virtual IActionResult EmailConfirmationRequired(Guid userId, bool didSend = false, string returnUrl = null)
        {
            if (AccountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }
            var model = new PendingNotificationViewModel
            {
                UserId = userId,
                DidSend = didSend,
                ReturnUrl = returnUrl


            };

            return View("EmailConfirmationRequired", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> VerifyEmail(Guid userId)
        {
            var info = await AccountService.GetEmailVerificationInfo(userId);

            if(info.User == null)
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            if(info.User.EmailConfirmed)
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            var callbackUrl = Url.Action("ConfirmEmail", "Account",
                            new { userId = info.User.Id.ToString(), code = info.EmailVerificationToken },
                            protocol: HttpContext.Request.Scheme);

            await EmailSender.SendAccountConfirmationEmailAsync(
                            CurrentSite,
                            info.User.Email,
                            StringLocalizer["Confirm your account"],
                            callbackUrl,
                            info.EmailVerificationToken
                            );

            await IpAddressTracker.TackUserIpAddress(CurrentSite.Id, info.User.Id);

            return RedirectToAction("EmailConfirmationRequired", new { userId = info.User.Id, didSend = true });
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> ConfirmEmail(string userId, string code, string returnUrl)
        {
            if (AccountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }
            if (userId == null || code == null)
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            bool? isEmailAlreadyConfirmed = await AccountService.IsEmailConfirmedAsync(userId);

            var result = await AccountService.ConfirmEmailAsync(userId, code);

            if (result.User == null)
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            if(result.IdentityResult.Succeeded)
            {
                if (CurrentSite.RequireApprovalBeforeLogin && !result.User.AccountApproved)
                {
                    await EmailSender.AccountPendingApprovalAdminNotification(CurrentSite, result.User).ConfigureAwait(false);
                    return RedirectToAction("PendingApproval", new { userId = result.User.Id, didSend = true });
                }

                //only send the new account notification email if the user is confirming their email for the first time
                if(isEmailAlreadyConfirmed == false)
                {
                    await EmailSender.NewAccountAdminNotification(CurrentSite, result.User).ConfigureAwait(false);
                }

                if(!string.IsNullOrWhiteSpace(returnUrl))
                {
                    // if we have a return url we should just go ahead and redirect to login
                    this.AlertSuccess(StringLocalizer["Thank you for confirming your email."], true);
                    return RedirectToAction("Login", new { returnUrl });
                }
            }
            else
            {
                this.AlertDanger(StringLocalizer["Oops something went wrong"], true);
            }

            return View();
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> LogOff()
        {
            await AccountService.SignOutAsync();

            if (CurrentSite.SingleBrowserSessions)
            {
                // invalidate all other sign-ins, of which there should be none
                var userid = UserManager.GetUserId(User);
                if (!String.IsNullOrWhiteSpace(userid))
                {
                    var user = await UserManager.FindByIdAsync(userid);
                    if (user != null)
                    {
                        await UserManager.UpdateSecurityStampAsync(user);
                    }
                }
            }

            await Analytics.HandleLogout("User Signed Out");
            return this.RedirectToSiteRoot(CurrentSite);
        }

        // identityserver integration point
        [HttpGet]
        public virtual async Task<IActionResult> Logout(string logoutId)
        {
            var clientId = await IdentityServerIntegration.GetLogoutContextClientIdAsync(logoutId);
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
        public virtual async Task<IActionResult> Logout(IdentityServerLogoutViewModel model)
        {
            await AccountService.SignOutAsync();
            await Analytics.HandleLogout("Federated User Signed Out");
            // set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logoutModel = await IdentityServerIntegration.GetLogoutContextModelAsync(model.LogoutId);
            if(!string.IsNullOrWhiteSpace(logoutModel.PostLogoutRedirectUri))
            {
                return Redirect(logoutModel.PostLogoutRedirectUri);
            }

            return View("LoggedOut", logoutModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<JsonResult> UsernameAvailable(Guid? userId, string userName)
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
            bool available = await AccountService.LoginNameIsAvailable(selectedUserGuid, userName);

            return Json(available);
        }


        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> ForgotPasswordAsync()
        {
            ForgotPasswordViewModel forgotPasswordViewModel = new ForgotPasswordViewModel();
            var recaptchaKeys = await RecaptchaKeysProvider.GetKeys().ConfigureAwait(false);

            if ((CurrentSite.CaptchaOnLogin || CurrentSite.CaptchaOnRegistration) && (!string.IsNullOrEmpty(recaptchaKeys.PublicKey)))
            {
                forgotPasswordViewModel.RecaptchaSiteKey = recaptchaKeys.PublicKey;
                forgotPasswordViewModel.UseInvisibleCaptcha = recaptchaKeys.Invisible;
            }

            if (AccountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            return View(forgotPasswordViewModel);
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var recaptchaKeys = await RecaptchaKeysProvider.GetKeys().ConfigureAwait(false);

                if ((CurrentSite.CaptchaOnLogin || CurrentSite.CaptchaOnRegistration) && (!string.IsNullOrEmpty(recaptchaKeys.PublicKey)))
                {
                    model.RecaptchaSiteKey = recaptchaKeys.PublicKey;
                    model.UseInvisibleCaptcha = recaptchaKeys.Invisible;
                }

                if ((CurrentSite.CaptchaOnLogin || CurrentSite.CaptchaOnRegistration) && (!string.IsNullOrEmpty(recaptchaKeys.PrivateKey)))
                {
                    var captchaResponse = await RecaptchaServerSideValidator.ValidateRecaptcha(Request, CurrentSite.RecaptchaPrivateKey);

                    if (!captchaResponse.Success)
                    {
                        await Analytics.HandleLoginFail("Onsite", "reCAPTCHA Error");
                        ModelState.AddModelError("recaptchaerror", StringLocalizer["reCAPTCHA Error occured. Please try again"]);
                        return View(model);
                    }
                }

                var info = await AccountService.GetPasswordResetInfo(model.Email);
                if (info.User == null || (CurrentSite.RequireConfirmedEmail && !(info.User.EmailConfirmed)))
                {
                    if(info.User != null)
                    {
                        Log.LogWarning(info.User.Email + ": user tried to use pasword recovery but no email was sent because user email is not confirmed and security settings require confirmed email");
                    }
                    //2018-08-21 see below, we now have to wait for sending the email
                    // that delay can be used to determine an account exists
                    // so add a delay here as well
                    var random = new Random();
                    var randomMilliseconds = random.Next(1, 3) * 1000; //between 1 and 3 seconds

                    await Task.Delay(randomMilliseconds);

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
                //EmailSender.SendPasswordResetEmailAsync(
                //    CurrentSite,
                //    model.Email,
                //    StringLocalizer["Reset Password"],
                //    resetUrl).Forget();

                // the above broke in aspnetcore 2.1.x by not awaiting the httpcontext goes out of scope and viewrenderer fails IServiceProvider is disposed

                await EmailSender.SendPasswordResetEmailAsync(
                    CurrentSite,
                    model.Email,
                    StringLocalizer["Reset Password"],
                    resetUrl);


                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public virtual IActionResult ForgotPasswordConfirmation()
        {
            if (AccountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(CurrentSite);
            }

            return View();
        }


        // GET: /Account/SetInitialPassword
        [HttpGet]
        [AllowAnonymous]
        public virtual IActionResult SetInitialPassword(string code = null)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                Log.LogInformation("Set initial password url with no code, redirecting to site root.");
                return this.RedirectToSiteRoot(CurrentSite);
            }

            return View();
        }


        // POST: /Account/SetInitialPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> SetInitialPassword(SetInitialPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await AccountService.ResetPassword(model.Email, model.Password, model.Code);

            if (result.IdentityResult.Succeeded)
            {
                return RedirectToAction("SetInitialPasswordConfirmation", "Account");
            }

            if (result.User == null)
            {
                // User has entered non-existent email.
                // Don't reveal that the user does not exist
                ModelState.AddModelError(string.Empty, StringLocalizer["The password reset token is invalid. Password reset tokens have a short lifespan for security reasons. You will need to use the forgot password link on the login page to get a new reset token sent to your email address."]);
            }
            else
            {
                // typically here if user is trying to set password for some other existing user
                AddErrors(result.IdentityResult);
            }

            return View();
        }

        // GET: /Account/SetInitialPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public virtual IActionResult SetInitialPasswordConfirmation()
        {
            return View();
        }


        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public virtual IActionResult ResetPassword(string code = null)
        {
            if(string.IsNullOrWhiteSpace(code))
            {
                Log.LogInformation("Reset password url with no code, redirecting to site root.");
                return this.RedirectToSiteRoot(CurrentSite);
            }

            return View();
        }



        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await AccountService.ResetPassword(model.Email, model.Password, model.Code);

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
        public virtual IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual IActionResult AccessDenied()
        {
            ViewData["Title"] = StringLocalizer["Oops something went wrong"];
            return View();
        }

        // GET: /Account/SendCode
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        //{
        //    var twoFactorInfo = await accountService.GetTwoFactorInfo();

        //    if (twoFactorInfo.User == null)
        //    {
        //        return View("Error");
        //    }

        //    var factorOptions = twoFactorInfo.UserFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        //    return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}

        //// POST: /Account/SendCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SendCode(SendCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    var twoFactorInfo = await accountService.GetTwoFactorInfo(model.SelectedProvider);
        //    if (twoFactorInfo.User == null)
        //    {
        //        return View("Error");
        //    }

        //    if (string.IsNullOrWhiteSpace(twoFactorInfo.TwoFactorToken))
        //    {
        //        return View("Error");
        //    }

        //    if (model.SelectedProvider == "Email")
        //    {
        //        await emailSender.SendSecurityCodeEmailAsync(
        //            Site,
        //            twoFactorInfo.User.Email,
        //            sr["Security Code"],
        //            twoFactorInfo.TwoFactorToken);
        //    }
        //    //else if (model.SelectedProvider == "Phone")
        //    //{
        //    //    var message = string.Format(sr["Your security code is: {0}"], twoFactorInfo.TwoFactorToken);
        //    //    await smsSender.SendSmsAsync(Site, twoFactorInfo.User.PhoneNumber, message);
        //    //}

        //    return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        //}


        //// GET: /Account/VerifyCode
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        //{
        //    // Require that the user has already logged in via username/password or external login
        //    var user = await accountService.GetTwoFactorAuthenticationUserAsync();
        //    if (user == null)
        //    {
        //        return View("Error");
        //    }
        //    return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}

        //// POST: /Account/VerifyCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // The following code protects for brute force attacks against the two factor codes.
        //    // If a user enters incorrect codes for a specified amount of time then the user account
        //    // will be locked out for a specified amount of time.
        //    var result = await accountService.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
        //    if (result.Succeeded)
        //    {
        //        if (!string.IsNullOrEmpty(model.ReturnUrl))
        //        {
        //            return LocalRedirect(model.ReturnUrl);
        //        }

        //        return this.RedirectToSiteRoot(Site);
        //    }

        //    if (result.IsLockedOut)
        //    {
        //        return await HandleLockout();
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", sr["Invalid code."]);
        //        return View(model);
        //    }
        //}



        #region Helpers


        private bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                if(error.Description == "Invalid token.")
                {
                    ModelState.AddModelError(string.Empty, StringLocalizer["The password reset token is invalid. Password reset tokens have a short lifespan for security reasons. You will need to use the forgot password link on the login page to get a new reset token sent to your email address."]);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
        }

        #endregion
    }
}
