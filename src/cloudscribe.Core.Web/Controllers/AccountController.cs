// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2018-03-07
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
            IIdentityServerIntegration identityServerIntegration,
            IStringLocalizer<CloudscribeCore> localizer,
            IRecaptchaKeysProvider recaptchaKeysProvider,
            IHandleCustomRegistration customRegistration,
            IHandleAccountAnalytics analyticsHandler,
            ILogger<AccountController> logger
            )
        {
            _accountService = accountService;
            _currentSite = currentSite; 
            _identityServerIntegration = identityServerIntegration;
            _emailSender = emailSender;
            _ipAddressTracker = ipAddressTracker;
            _sr = localizer;
            _log = logger;
            _recaptchaKeysProvider = recaptchaKeysProvider;
            _timeZoneHelper = timeZoneHelper;
            _customRegistration = customRegistration;
            _analytics = analyticsHandler;
        }

        private readonly IAccountService _accountService;
        private readonly ISiteContext _currentSite;
        private readonly IIdentityServerIntegration _identityServerIntegration;
        private readonly ISiteMessageEmailSender _emailSender;
        private IpAddressTracker _ipAddressTracker;
        private ILogger _log;
        private IStringLocalizer _sr;
        private IRecaptchaKeysProvider _recaptchaKeysProvider;
        private SiteTimeZoneService _timeZoneHelper;
        private IHandleCustomRegistration _customRegistration;
        private IHandleAccountAnalytics _analytics;

        private async Task<IActionResult> HandleLoginSuccess(UserLoginResult result, string returnUrl)
        {
            _analytics.HandleLoginSuccess(result).Forget();

            if (result.User != null)
            {
                await _ipAddressTracker.TackUserIpAddress(_currentSite.Id, result.User.Id);
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

            return this.RedirectToSiteRoot(_currentSite);
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
            _analytics.HandleLoginNotAllowed(result).Forget();

            if (result.User != null)
            {      
                await _ipAddressTracker.TackUserIpAddress(_currentSite.Id, result.User.Id);

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

                        _emailSender.SendAccountConfirmationEmailAsync(
                            _currentSite,
                            result.User.Email,
                            _sr["Confirm your account"],
                            callbackUrl).Forget();


                        this.AlertSuccess(_sr["Please check your email inbox, we just sent you a link that you need to click to confirm your account"], true);
                    }
                   
                    return RedirectToAction("EmailConfirmationRequired", new { userId = result.User.Id, didSend = true });
                }

                if (result.NeedsAccountApproval)
                {
                    var timeSpan = DateTime.UtcNow - result.User.CreatedUtc;
                    if (timeSpan.TotalDays < 1)
                    {
                        // account was just created so send notification to approver
                        _emailSender.AccountPendingApprovalAdminNotification(_currentSite, result.User).Forget();
                    }

                    return RedirectToAction("PendingApproval", new { userId = result.User.Id, didSend = true });
                }
            }

            return this.RedirectToSiteRoot(_currentSite);
        }

        private async Task<IActionResult> HandleRequiresTwoFactor(UserLoginResult result, string returnUrl, bool rememberMe)
        {
            _analytics.HandleRequiresTwoFactor(result).Forget();

            if (result.User != null)
            {
                await _ipAddressTracker.TackUserIpAddress(_currentSite.Id, result.User.Id);

                _log.LogWarning($"redirecting from login for {result.User.Email} because 2 factor not configured yet for account");
            }

            return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, rememberMe });

            //return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        private async Task<IActionResult> HandleLockout(UserLoginResult result = null)
        {
            _analytics.HandleLockout(result).Forget();

            ViewData["Title"] = _sr["Locked out"];

            if (result != null && result.User != null)
            {
                await _ipAddressTracker.TackUserIpAddress(_currentSite.Id, result.User.Id);

                _log.LogWarning($"redirecting to lockout page for {result.User.Email} because account is locked");
            }

            return View("Lockout");
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (_accountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(_currentSite);
            }

            returnUrl = _identityServerIntegration.EnsureFolderSegmentIfNeeded(_currentSite, returnUrl);
            //identityserver integration point
            var idProvider = await _identityServerIntegration.GetAuthorizationContextAsync(returnUrl);

            if (!string.IsNullOrEmpty(idProvider))
            {
                // if IdP is passed, then bypass showing the login screen
                return ExternalLogin(idProvider, returnUrl);
            }

            ViewData["Title"] = _sr["Log In"];
            ViewData["ReturnUrl"] = returnUrl;

            var model = new LoginViewModel();

            var recaptchaKeys = await _recaptchaKeysProvider.GetKeys().ConfigureAwait(false);
            
            if ((_currentSite.CaptchaOnLogin)&& (!string.IsNullOrEmpty(recaptchaKeys.PublicKey)))
            {
                model.RecaptchaSiteKey = recaptchaKeys.PublicKey;
                model.UseInvisibleCaptcha = recaptchaKeys.Invisible;
            }
            model.UseEmailForLogin = _currentSite.UseEmailForLogin;
            model.LoginInfoTop = _currentSite.LoginInfoTop;
            model.LoginInfoBottom = _currentSite.LoginInfoBottom;
            var externalSchemes = await _accountService.GetExternalAuthenticationSchemes();
            model.ExternalAuthenticationList = externalSchemes.ToList();
            // don't disable db auth if there are no social auth providers configured
            model.DisableDbAuth = _currentSite.DisableDbAuth && _currentSite.HasAnySocialAuthEnabled();

            return View(model);
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["Title"] = _sr["Log In"];
            ViewData["ReturnUrl"] = returnUrl;

            _analytics.HandleLoginSubmit("Onsite").Forget();

            var recaptchaKeys = await _recaptchaKeysProvider.GetKeys().ConfigureAwait(false);
            if ((_currentSite.CaptchaOnLogin) && (!string.IsNullOrEmpty(recaptchaKeys.PublicKey)))
            {
                model.RecaptchaSiteKey = recaptchaKeys.PublicKey;
                model.UseInvisibleCaptcha = recaptchaKeys.Invisible;
            }
            model.UseEmailForLogin = _currentSite.UseEmailForLogin;
            model.LoginInfoTop = _currentSite.LoginInfoTop;
            model.LoginInfoBottom = _currentSite.LoginInfoBottom;
            model.ExternalAuthenticationList = await _accountService.GetExternalAuthenticationSchemes();
            // don't disable db auth if there are no social auth providers configured
            model.DisableDbAuth = _currentSite.DisableDbAuth && _currentSite.HasAnySocialAuthEnabled();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.Where(k => ModelState[k].Errors.Count > 0).Select(k => new { propertyName = k, errorMessage = ModelState[k].Errors[0].ErrorMessage });
                var trackedError = errors.FirstOrDefault().errorMessage;
                _analytics.HandleLoginFail("Onsite", trackedError).Forget();

                return View(model);
            }

            if ((_currentSite.CaptchaOnLogin) && (!string.IsNullOrEmpty(recaptchaKeys.PrivateKey)))
            {
                var recpatchaSecretKey = recaptchaKeys.PrivateKey;
                var captchaResponse = await this.ValidateRecaptcha(Request, recpatchaSecretKey);

                if (!captchaResponse.Success)
                {
                    _analytics.HandleLoginFail("Onsite", "reCAPTCHA Error").Forget();

                    ModelState.AddModelError("recaptchaerror", _sr["reCAPTCHA Error occured. Please try again"]);
                    return View(model);
                }
            }

            var result = await _accountService.TryLogin(model);
            if (result.SignInResult.Succeeded)
            {
                return await HandleLoginSuccess(result, returnUrl);
            }

            foreach (var reason in result.RejectReasons)
            {
                //these reasons are not meant to be shown in the ui
                // but we can log them so admin will see failed attempts in the log along with reasons
                _log.LogWarning(reason);
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
                _analytics.HandleLoginFail("Onsite", _sr["Invalid login attempt."]).Forget();

                _log.LogInformation($"login did not succeed for {model.Email}");
                ModelState.AddModelError(string.Empty, _sr["Invalid login attempt."]);
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            ViewData["Title"] = _sr["Two-factor authentication"];

            // Ensure the user has gone through the username & password screen first
            var user = await _accountService.GetTwoFactorAuthenticationUserAsync();

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
            ViewData["Title"] = _sr["Two-factor authentication"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var result = await _accountService.Try2FaLogin(model, rememberMe);

            if (result.SignInResult.Succeeded)
            {
                return await HandleLoginSuccess(result, returnUrl);
            }

            foreach (var reason in result.RejectReasons)
            {
                //these reasons are not meant to be shown in the ui
                // but we can log them so admin will see failed attempts in the log along with reasons
                _log.LogWarning(reason);
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
                _analytics.HandleLoginFail("Onsite", _sr["Invalid authenticator code."]).Forget();


                ModelState.AddModelError(string.Empty, _sr["Invalid authenticator code."]);
                return View(model);
            }


        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            ViewData["Title"] = _sr["Recovery code verification"];

            // Ensure the user has gone through the username & password screen first
            var user = await _accountService.GetTwoFactorAuthenticationUserAsync();
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
            ViewData["Title"] = _sr["Recovery code verification"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var result = await _accountService.TryLoginWithRecoveryCode(model);

            if (result.SignInResult.Succeeded)
            {
                return await HandleLoginSuccess(result, returnUrl);
            }

            foreach (var reason in result.RejectReasons)
            {
                //these reasons are not meant to be shown in the ui
                // but we can log them so admin will see failed attempts in the log along with reasons
                _log.LogWarning(reason);
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
                _analytics.HandleLoginFail("Onsite", _sr["Invalid recovery code entered."]).Forget();


                ModelState.AddModelError(string.Empty, _sr["Invalid recovery code entered."]);
                return View(model);
            }


        }


        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            if(_accountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(_currentSite);
            }
            if(!_currentSite.AllowNewRegistration)
            {
                return new StatusCodeResult(404);
            }
            // login is equivalent to register for new social auth users
            // if db auth is disabled just redirect
            if(_currentSite.DisableDbAuth && _currentSite.HasAnySocialAuthEnabled())
            {
                return RedirectToAction("Login");
            }

            ViewData["Title"] = _sr["Register"];

            returnUrl = _identityServerIntegration.EnsureFolderSegmentIfNeeded(_currentSite, returnUrl);
            //identityserver integration point
            var idProvider = await _identityServerIntegration.GetAuthorizationContextAsync(returnUrl);

            if (!string.IsNullOrEmpty(idProvider))
            {
                // if IdP is passed, then bypass showing the login screen
                return ExternalLogin(idProvider, returnUrl);
            }
            ViewData["ReturnUrl"] = returnUrl;

            var model = new RegisterViewModel
            {
                SiteId = _currentSite.Id
            };

            if ((_currentSite.CaptchaOnRegistration)&& (!string.IsNullOrWhiteSpace(_currentSite.RecaptchaPublicKey)))
            {
                model.RecaptchaSiteKey = _currentSite.RecaptchaPublicKey;
                model.UseInvisibleCaptcha = _currentSite.UseInvisibleRecaptcha;
            }
            model.UseEmailForLogin = _currentSite.UseEmailForLogin;
            model.RegistrationPreamble = _currentSite.RegistrationPreamble;
            model.RegistrationAgreement = _currentSite.RegistrationAgreement;
            model.AgreementRequired = !string.IsNullOrWhiteSpace(_currentSite.RegistrationAgreement);
            model.ExternalAuthenticationList = await _accountService.GetExternalAuthenticationSchemes();

            var viewName = await _customRegistration.GetRegisterViewName(_currentSite, HttpContext);

            await _customRegistration.HandleRegisterGet(
                _currentSite,
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
            ViewData["Title"] = _sr["Register"];

            ViewData["ReturnUrl"] = returnUrl;

            _analytics.HandleRegisterSubmit("Onsite").Forget();

            if ((_currentSite.CaptchaOnRegistration) && (!string.IsNullOrWhiteSpace(_currentSite.RecaptchaPublicKey)))
            {
                model.RecaptchaSiteKey = _currentSite.RecaptchaPublicKey;
                model.UseInvisibleCaptcha = _currentSite.UseInvisibleRecaptcha;
            }
            model.UseEmailForLogin = _currentSite.UseEmailForLogin;
            model.RegistrationPreamble = _currentSite.RegistrationPreamble;
            model.RegistrationAgreement = _currentSite.RegistrationAgreement;
            model.AgreementRequired = !string.IsNullOrWhiteSpace(_currentSite.RegistrationAgreement);
            model.ExternalAuthenticationList = await _accountService.GetExternalAuthenticationSchemes();

            bool isValid = ModelState.IsValid;
            
            bool customDataIsValid = await _customRegistration.HandleRegisterValidation(
                _currentSite,
                model,
                HttpContext,
                ViewData,
                ModelState);

            if (isValid && customDataIsValid)
            {
                if ((_currentSite.CaptchaOnRegistration) && (!string.IsNullOrWhiteSpace(_currentSite.RecaptchaPublicKey)))
                {
                    string recpatchaSecretKey = _currentSite.RecaptchaPrivateKey;

                    var captchaResponse = await this.ValidateRecaptcha(Request, recpatchaSecretKey);

                    if (!captchaResponse.Success)
                    {
                        ModelState.AddModelError("recaptchaerror", _sr["reCAPTCHA Error occured. Please try again"]);
                        isValid = false;
                    }
                }

                if (!string.IsNullOrWhiteSpace(_currentSite.RegistrationAgreement))
                {
                    if (!model.AgreeToTerms)
                    {
                        ModelState.AddModelError("agreementerror", _sr["You must agree to the terms"]);
                        isValid = false;
                    }
                }

                var viewName = await _customRegistration.GetRegisterViewName(_currentSite, HttpContext);

                if (!isValid || !customDataIsValid)
                {
                    return View(viewName, model);
                }
                
                var result = await _accountService.TryRegister(model, ModelState, HttpContext, _customRegistration);

                if (result.SignInResult.Succeeded || (result.SignInResult.IsNotAllowed && result.User != null))
                {
                    await _customRegistration.HandleRegisterPostSuccess(
                        _currentSite,
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
                    _log.LogWarning(reason);
                }

                if (result.SignInResult.IsNotAllowed)
                {
                    return await HandleLoginNotAllowed(result);
                }

                var te = result.RejectReasons.FirstOrDefault();
                if(string.IsNullOrEmpty(te)) { te = "unknown"; }

                _analytics.HandleRegisterFail("Onsite", te).Forget();

                _log.LogInformation($"registration did not succeed for {model.Email}");
               // ModelState.AddModelError(string.Empty, sr["Invalid registration attempt."]);
                return View(viewName, model);           
            }

            var errors = ModelState.Keys.Where(k => ModelState[k].Errors.Count > 0).Select(k => new { propertyName = k, errorMessage = ModelState[k].Errors[0].ErrorMessage });
            var trackedError = errors.FirstOrDefault().errorMessage;
            _analytics.HandleRegisterFail("Onsite", trackedError).Forget();

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            _log.LogDebug("ExternalLogin called for " + provider + " with returnurl " + returnUrl);

            _analytics.HandleLoginSubmit(provider).Forget();

            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _accountService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            _log.LogDebug("ExternalLoginCallback called with returnurl " + returnUrl);

            if (remoteError != null)
            {
                var errormessage = string.Format(_sr["Error from external provider: {0}"], remoteError);

                _analytics.HandleLoginFail("Social", errormessage).Forget();

                ModelState.AddModelError("providererror", errormessage);
                return RedirectToAction("Login");
            }

            var result = await _accountService.TryExternalLogin();

            if (result.SignInResult.Succeeded)
            {
                if (result.User != null)
                {
                    if (result.MustAcceptTerms)
                    {
                        await _analytics.HandleLoginSuccess(result);
                        
                        return RedirectToAction("TermsOfUse");
                    }
                }
                return await HandleLoginSuccess(result, returnUrl);
            }

            foreach (var reason in result.RejectReasons)
            {
                // these reasons are not meant to be shown in the ui
                // but we can log them so admin will see failed attempts in the log along with reasons
                _log.LogWarning(reason);
            }

            if (result.SignInResult.IsNotAllowed)
            {
                return await HandleLoginNotAllowed(result);
            }

            if (result.ExternalLoginInfo == null)
            {
                _log.LogDebug("ExternalLoginCallback redirecting to login because GetExternalLoginInfoAsync returned null ");
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
            var model = new ExternalLoginConfirmationViewModel
            {
                Email = email,
                RegistrationPreamble = _currentSite.RegistrationPreamble,
                RegistrationAgreement = _currentSite.RegistrationAgreement,
                AgreementRequired = !string.IsNullOrWhiteSpace(_currentSite.RegistrationAgreement)
            };
            return View("ExternalLoginConfirmation", model);
        }

        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            // this is posted if the SiteUser has not been created yet 
            _log.LogDebug("ExternalLoginConfirmation called with returnurl " + returnUrl);

            if (_accountService.IsSignedIn(User)) // this should be false
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                var result = await _accountService.TryExternalLogin(model.Email, model.AgreeToTerms);
                if (result.SignInResult.Succeeded)
                {
                    return await HandleLoginSuccess(result, returnUrl);
                }

                foreach (var reason in result.RejectReasons)
                {
                    // these reasons are not meant to be shown in the ui
                    // but we can log them so admin will see failed attempts in the log along with reasons
                    _log.LogWarning(reason);
                }

                if (result.SignInResult.IsNotAllowed)
                {
                    return await HandleLoginNotAllowed(result);
                }

                if (result.ExternalLoginInfo == null)
                {
                    _log.LogWarning("ExternalLoginInfo was null");

                    _analytics.HandleLoginFail("Social", "ExternalLoginInfo was null").Forget();

                    return View("ExternalLoginFailure");
                }

            }
            else
            {
                _log.LogDebug("ExternalLoginConfirmation called with ModelStateInvalid ");
                model.RegistrationPreamble = _currentSite.RegistrationPreamble;
                model.RegistrationAgreement = _currentSite.RegistrationAgreement;
                model.AgreementRequired = !string.IsNullOrWhiteSpace(_currentSite.RegistrationAgreement);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PendingApproval(Guid userId, bool didSend = false)
        {
            if (_accountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(_currentSite);
            }

            var model = new PendingNotificationViewModel
            {
                UserId = userId,
                DidSend = didSend
            };

            return View("PendingApproval", model);
        }

        [HttpGet]
        public async Task<IActionResult> TermsOfUse()
        {
            if (!_accountService.IsSignedIn(User) || string.IsNullOrWhiteSpace(_currentSite.RegistrationAgreement))
            {
                return this.RedirectToSiteRoot(_currentSite);
            }

            ViewData["Title"] = _sr["Registration Agreement Required"];

            var model = new AcceptTermsViewModel
            {
                TermsUpdatedDate = await _timeZoneHelper.ConvertToLocalTime(_currentSite.TermsUpdatedUtc),
                AgreementRequired = true,
                RegistrationAgreement = _currentSite.RegistrationAgreement,
                RegistrationPreamble = _currentSite.RegistrationPreamble
            };

            return View(model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TermsOfUse(AcceptTermsViewModel model)
        {
            if (!_accountService.IsSignedIn(User) || string.IsNullOrWhiteSpace(_currentSite.RegistrationAgreement))
            {
                return this.RedirectToSiteRoot(_currentSite);
            }

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = _sr["Registration Agreement Required"];
                model.TermsUpdatedDate = await _timeZoneHelper.ConvertToLocalTime(_currentSite.TermsUpdatedUtc);
                model.AgreementRequired = true;
                model.RegistrationAgreement = _currentSite.RegistrationAgreement;
                model.RegistrationPreamble = _currentSite.RegistrationPreamble;

                return View(model);
            }

            var result = await _accountService.AcceptRegistrationAgreement(User);
            //return Redirect("/");
            if(result)
            {
                return this.RedirectToSiteRoot(_currentSite);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult EmailConfirmationRequired(Guid userId, bool didSend = false)
        {
            if (_accountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(_currentSite);
            }
            var model = new PendingNotificationViewModel
            {
                UserId = userId,
                DidSend = didSend
            };

            return View("EmailConfirmationRequired", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyEmail(Guid userId)
        {
            var info = await _accountService.GetEmailVerificationInfo(userId);
            
            if(info.User == null)
            {
                return this.RedirectToSiteRoot(_currentSite);
            }

            if(info.User.EmailConfirmed)
            {
                return this.RedirectToSiteRoot(_currentSite);
            }

            var callbackUrl = Url.Action("ConfirmEmail", "Account",
                            new { userId = info.User.Id.ToString(), code = info.EmailVerificationToken },
                            protocol: HttpContext.Request.Scheme);

            _emailSender.SendAccountConfirmationEmailAsync(
                            _currentSite,
                            info.User.Email,
                            _sr["Confirm your account"],
                            callbackUrl).Forget();

            await _ipAddressTracker.TackUserIpAddress(_currentSite.Id, info.User.Id);

            return RedirectToAction("EmailConfirmationRequired", new { userId = info.User.Id, didSend = true });
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (_accountService.IsSignedIn(User))
            {
                return this.RedirectToSiteRoot(_currentSite);
            }
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var result = await _accountService.ConfirmEmailAsync(userId, code);
            
            if (result.User == null)
            {
                return View("Error");
            }
            
            if(result.IdentityResult.Succeeded)
            {
                if (_currentSite.RequireApprovalBeforeLogin && !result.User.AccountApproved)
                {
                    await _emailSender.AccountPendingApprovalAdminNotification(_currentSite, result.User).ConfigureAwait(false);
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
            await _accountService.SignOutAsync();
            //return Redirect("/");
            return this.RedirectToSiteRoot(_currentSite);
        }

        // identityserver integration point
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var clientId = await _identityServerIntegration.GetLogoutContextClientIdAsync(logoutId);
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
            await _accountService.SignOutAsync();
            // set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logoutModel = await _identityServerIntegration.GetLogoutContextModelAsync(model.LogoutId);
            
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
            bool available = await _accountService.LoginNameIsAvailable(selectedUserGuid, userName);

            
            
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
                var info = await _accountService.GetPasswordResetInfo(model.Email);
                if (info.User == null || (_currentSite.RequireConfirmedEmail && !(info.User.EmailConfirmed)))
                {
                    if(info.User != null)
                    {
                        _log.LogWarning(info.User.Email + ": user tried to use pasword recovery but no email was sent because user email is not confirmed and security settings require confirmed email");
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
                _emailSender.SendPasswordResetEmailAsync(
                    _currentSite,
                    model.Email,
                    _sr["Reset Password"],
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

            var result = await _accountService.ResetPassword(model.Email, model.Password, model.Code);

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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            ViewData["Title"] = _sr["Oops something went wrong"];
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
