// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2015-05-20
// 

using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
    [Authorize]
    public class AccountController : CloudscribeBaseController
    {
        
        public AccountController()
        {
        }

        //public ISiteContext Site
        //{
        //    get {  return HttpContext.GetOwinContext().Get<ISiteContext>(); }
            
        //}

       
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            
            ViewBag.ReturnUrl = returnUrl;
            LoginViewModel model = new LoginViewModel();
            if (Site.SiteSettings.RequireCaptchaOnLogin)
            {
                model.RecaptchaSiteKey = AppSettings.RecaptchaSiteKey;
                if (Site.SiteSettings.RecaptchaPublicKey.Length > 0)
                {
                    model.RecaptchaSiteKey = Site.SiteSettings.RecaptchaPublicKey;
                }
            }

            return View(model);
        }

        
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            if (Site.SiteSettings.RequireCaptchaOnLogin)
            {
                model.RecaptchaSiteKey = AppSettings.RecaptchaSiteKey;
                if (Site.SiteSettings.RecaptchaPublicKey.Length > 0)
                {
                    model.RecaptchaSiteKey = Site.SiteSettings.RecaptchaPublicKey;
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (Site.SiteSettings.RequireCaptchaOnLogin)
            {
                string recpatchaSecretKey = AppSettings.RecaptchaSecretKey;
                if (Site.SiteSettings.RecaptchaPrivateKey.Length > 0)
                {
                    recpatchaSecretKey = Site.SiteSettings.RecaptchaPrivateKey;
                }

                var captchaResponse = await this.ValidateRecaptcha(Request, recpatchaSecretKey);

                if (!captchaResponse.Success)
                {         
                    ModelState.AddModelError("recaptchaerror", "reCAPTCHA Error occured. Please try again");
                    return View(model);
                }
            }


            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await Site.SiteSignInManager.PasswordSignInAsync(
                model.Email, 
                model.Password, 
                model.RememberMe, 
                shouldLockout: false);
            
            switch (result)
            {
                case SignInStatus.Success:
                    //AuthenticationManager.SignIn()
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            // Require that the user has already logged in via username/password or external login
            if (!await Site.SiteSignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await Site.SiteUserManager.FindByIdAsync(await Site.SiteSignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                var code = await Site.SiteUserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await Site.SiteSignInManager.TwoFactorSignInAsync(
                model.Provider, 
                model.Code, 
                isPersistent: model.RememberMe, 
                rememberBrowser: model.RememberBrowser);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Title = "Register";

            ViewBag.SiteName = Site.SiteSettings.SiteName;
            EditUserViewModel model = new EditUserViewModel();
            model.SiteGuid = Site.SiteSettings.SiteGuid;
            if(Site.SiteSettings.RequireCaptchaOnRegistration)
            {
                model.RecaptchaSiteKey = AppSettings.RecaptchaSiteKey;
                if (Site.SiteSettings.RecaptchaPublicKey.Length > 0)
                {
                    model.RecaptchaSiteKey = Site.SiteSettings.RecaptchaPublicKey;
                }
            }
            

            return View(model);
        }

        
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(EditUserViewModel model)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            if (Site.SiteSettings.RequireCaptchaOnRegistration)
            {
                model.RecaptchaSiteKey = AppSettings.RecaptchaSiteKey;
                if (Site.SiteSettings.RecaptchaPublicKey.Length > 0)
                {
                    model.RecaptchaSiteKey = Site.SiteSettings.RecaptchaPublicKey;
                }
            }

            if (ModelState.IsValid)
            {
                if (Site.SiteSettings.RequireCaptchaOnRegistration)
                {
                    string recpatchaSecretKey = AppSettings.RecaptchaSecretKey;
                    if (Site.SiteSettings.RecaptchaPrivateKey.Length > 0)
                    {
                        recpatchaSecretKey = Site.SiteSettings.RecaptchaPrivateKey;
                    }


                    var captchaResponse = await this.ValidateRecaptcha(Request, recpatchaSecretKey);

                    if (!captchaResponse.Success)
                    {
                        //if (captchaResponse.ErrorCodes.Count <= 0)
                        //{
                        //    return View(model);
                        //}

                        ////TODO: log these errors rather than show them in the ui
                        //var error = captchaResponse.ErrorCodes[0].ToLower();
                        //switch (error)
                        //{
                        //    case ("missing-input-secret"):
                        //        ModelState.AddModelError("recaptchaerror", "The secret parameter is missing.");     
                        //        break;
                        //    case ("invalid-input-secret"):
                        //        ModelState.AddModelError("recaptchaerror", "The secret parameter is invalid or malformed.");
                        //        break;
                        //    case ("missing-input-response"):
                        //        ModelState.AddModelError("recaptchaerror", "The response parameter is missing.");
                        //        break;
                        //    case ("invalid-input-response"):
                        //        ModelState.AddModelError("recaptchaerror", "The response parameter is invalid or malformed.");
                        //        break;
                        //    default:
                        //        ModelState.AddModelError("recaptchaerror", "Error occured. Please try again");
                        //        break;
                        //}

                        ModelState.AddModelError("recaptchaerror", "reCAPTCHA Error occured. Please try again");
                        return View(model);
                    }

                }
                
                var user = new SiteUser {
                    UserName = model.LoginName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DisplayName = model.DisplayName 
                };
                if(model.DateOfBirth.HasValue)
                {
                    user.DateOfBirth = model.DateOfBirth.Value;
                }
                

                var result = await Site.SiteUserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await Site.SiteSignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            else
            {
                this.AlertDanger("model was invalid", true);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public JsonResult LoginNameAvailable(int? userId, string loginName)
        {
            // same validation is used when editing or creating a user
            // if editing then the loginname is valid if found attached to the selected user
            // otherwise if found it is not already in use and not available
            int selectedUserId = -1;
            if (userId.HasValue) { selectedUserId = userId.Value; }
            bool available = Site.UserRepository.LoginIsAvailable(
                Site.SiteSettings.SiteId, selectedUserId, loginName);


            return Json(available);
        }

        
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await Site.SiteUserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            return View();
        }

        
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            if (ModelState.IsValid)
            {
                var user = await Site.SiteUserManager.FindByNameAsync(model.Email);
                if (user == null || !(await Site.SiteUserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            return View();
        }

        
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            return code == null ? View("Error") : View();
        }

        
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await Site.SiteUserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await Site.SiteUserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            return View();
        }

        
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            var userId = await Site.SiteSignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await Site.SiteUserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await Site.SiteSignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await Site.SiteSignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
           
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            ViewBag.SiteName = Site.SiteSettings.SiteName;

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new SiteUser { UserName = model.Email, Email = model.Email };
                var result = await Site.SiteUserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await Site.SiteUserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await Site.SiteSignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult SignOut()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            return View();
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}