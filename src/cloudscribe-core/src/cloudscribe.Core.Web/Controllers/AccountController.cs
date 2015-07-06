// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2015-07-06
// 

using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using cloudscribe.Messaging;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace cloudscribe.Core.Web.Controllers
{
    [Authorize]
    public class AccountController : CloudscribeBaseController
    {

        public AccountController(
            ISiteResolver siteResolver,
            IUserRepository userRepository,
            UserManager<SiteUser> userManager,
            SignInManager<SiteUser> signInManager,
            IConfiguration configuration)
        {
            Site = siteResolver.Resolve();
            userRepo = userRepository;
            UserManager = userManager;
            SignInManager = signInManager;
            config = configuration;
        }

        //private ISiteResolver resolver;
        private ISiteSettings Site;
        private IUserRepository userRepo;
        private IConfiguration config;
        public UserManager<SiteUser> UserManager { get; private set; }
        public SignInManager<SiteUser> SignInManager { get; private set; }

        //public ISiteContext Site
        //{
        //    get {  return HttpContext.GetOwinContext().Get<ISiteContext>(); }

        //}


        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {

            ViewBag.SiteName = Site.SiteName;

            ViewBag.ReturnUrl = returnUrl;
            LoginViewModel model = new LoginViewModel();
            if (Site.RequireCaptchaOnLogin)
            {
                model.RecaptchaSiteKey = config.RecaptchaSiteKey();
                if (Site.RecaptchaPublicKey.Length > 0)
                {
                    model.RecaptchaSiteKey = Site.RecaptchaPublicKey;
                }
            }

            return View(model);
        }


        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.SiteName = Site.SiteName;

            if (Site.RequireCaptchaOnLogin)
            {
                model.RecaptchaSiteKey = config.RecaptchaSiteKey();
                if (Site.RecaptchaPublicKey.Length > 0)
                {
                    model.RecaptchaSiteKey = Site.RecaptchaPublicKey;
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (Site.RequireCaptchaOnLogin)
            {
                string recpatchaSecretKey = config.RecaptchaSecretKey();
                if (Site.RecaptchaPrivateKey.Length > 0)
                {
                    recpatchaSecretKey = Site.RecaptchaPrivateKey;
                }

                //var captchaResponse = await this.ValidateRecaptcha(Request, recpatchaSecretKey);

                //if (!captchaResponse.Success)
                //{
                //    ModelState.AddModelError("recaptchaerror", "reCAPTCHA Error occured. Please try again");
                //    return View(model);
                //}
            }


            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure : false);

            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        //AuthenticationManager.SignIn()
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}

            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }





        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewBag.Title = "Register";

            ViewBag.SiteName = Site.SiteName;
            EditUserViewModel model = new EditUserViewModel();
            model.SiteGuid = Site.SiteGuid;
            if (Site.RequireCaptchaOnRegistration)
            {
                model.RecaptchaSiteKey = config.RecaptchaSiteKey();
                if (Site.RecaptchaPublicKey.Length > 0)
                {
                    model.RecaptchaSiteKey = Site.RecaptchaPublicKey;
                }
            }


            return View(model);
        }


        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(EditUserViewModel model)
        {
            ViewBag.SiteName = Site.SiteName;

            if (Site.RequireCaptchaOnRegistration)
            {
                model.RecaptchaSiteKey = config.RecaptchaSiteKey();
                if (Site.RecaptchaPublicKey.Length > 0)
                {
                    model.RecaptchaSiteKey = Site.RecaptchaPublicKey;
                }
            }

            if (ModelState.IsValid)
            {
                if (Site.RequireCaptchaOnRegistration)
                {
                    string recpatchaSecretKey = config.RecaptchaSecretKey();
                    if (Site.RecaptchaPrivateKey.Length > 0)
                    {
                        recpatchaSecretKey = Site.RecaptchaPrivateKey;
                    }


                    //var captchaResponse = await this.ValidateRecaptcha(Request, recpatchaSecretKey);

                    //if (!captchaResponse.Success)
                    //{
                    //    //if (captchaResponse.ErrorCodes.Count <= 0)
                    //    //{
                    //    //    return View(model);
                    //    //}

                    //    ////TODO: log these errors rather than show them in the ui
                    //    //var error = captchaResponse.ErrorCodes[0].ToLower();
                    //    //switch (error)
                    //    //{
                    //    //    case ("missing-input-secret"):
                    //    //        ModelState.AddModelError("recaptchaerror", "The secret parameter is missing.");     
                    //    //        break;
                    //    //    case ("invalid-input-secret"):
                    //    //        ModelState.AddModelError("recaptchaerror", "The secret parameter is invalid or malformed.");
                    //    //        break;
                    //    //    case ("missing-input-response"):
                    //    //        ModelState.AddModelError("recaptchaerror", "The response parameter is missing.");
                    //    //        break;
                    //    //    case ("invalid-input-response"):
                    //    //        ModelState.AddModelError("recaptchaerror", "The response parameter is invalid or malformed.");
                    //    //        break;
                    //    //    default:
                    //    //        ModelState.AddModelError("recaptchaerror", "Error occured. Please try again");
                    //    //        break;
                    //    //}

                    //    ModelState.AddModelError("recaptchaerror", "reCAPTCHA Error occured. Please try again");
                    //    return View(model);
                    //}

                }

                var user = new SiteUser
                {
                    UserName = model.LoginName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DisplayName = model.DisplayName
                };
                if (model.DateOfBirth.HasValue)
                {
                    user.DateOfBirth = model.DateOfBirth.Value;
                }


                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Context.Request.Scheme);
                    //await MessageServices.SendEmailAsync(model.Email, "Confirm your account",
                    //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    await SignInManager.SignInAsync(user, isPersistent: false);
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

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogOff()
        {
            SignInManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //[HttpGet]
        //public ActionResult SignOut()
        //{
        //    AuthenticationManager.SignOut();
        //    return RedirectToAction("Index", "Home");
        //}

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            ViewBag.SiteName = Site.SiteName;

            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            ViewBag.SiteName = Site.SiteName;

            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = info.LoginProvider;
                var email = info.ExternalPrincipal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
            }

        }

        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {

            if (User.IsSignedIn())
            {
                return RedirectToAction("Index", "Manage");
            }

            ViewBag.SiteName = Site.SiteName;
            
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await SignInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new SiteUser {
                    SiteGuid = Site.SiteGuid,
                    SiteId = Site.SiteId,
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        public JsonResult LoginNameAvailable(int? userId, string loginName)
        {
            // same validation is used when editing or creating a user
            // if editing then the loginname is valid if found attached to the selected user
            // otherwise if found it is not already in use and not available
            int selectedUserId = -1;
            if (userId.HasValue) { selectedUserId = userId.Value; }
            bool available = userRepo.LoginIsAvailable(
                Site.SiteId, selectedUserId, loginName);


            return Json(available);
        }


        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            ViewBag.SiteName = Site.SiteName;

            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }


        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            ViewBag.SiteName = Site.SiteName;

            return View();
        }


        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            ViewBag.SiteName = Site.SiteName;

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                // var code = await UserManager.GeneratePasswordResetTokenAsync(user);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Context.Request.Scheme);
                // await MessageServices.SendEmailAsync(model.Email, "Reset Password",
                //    "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                // return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            ViewBag.SiteName = Site.SiteName;

            return View();
        }


        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            ViewBag.SiteName = Site.SiteName;

            return code == null ? View("Error") : View();
        }


        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            ViewBag.SiteName = Site.SiteName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }


        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            ViewBag.SiteName = Site.SiteName;

            return View();
        }





        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            ViewBag.SiteName = Site.SiteName;

            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }


        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            ViewBag.SiteName = Site.SiteName;

            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            // Generate the token and send it
            var code = await UserManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }

            var message = "Your security code is: " + code;
            if (model.SelectedProvider == "Email")
            {
                await MessageServices.SendEmailAsync(await UserManager.GetEmailAsync(user), "Security Code", message);
            }
            else if (model.SelectedProvider == "Phone")
            {
                await MessageServices.SendSmsAsync(await UserManager.GetPhoneNumberAsync(user), message);
            }

            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }


        // GET: /Account/VerifyCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            ViewBag.SiteName = Site.SiteName;

            // Require that the user has already logged in via username/password or external login
            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
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
            ViewBag.SiteName = Site.SiteName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError("", "Invalid code.");
                return View(model);
            }

        }






        #region Helpers


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        //private async Task<ApplicationUser> GetCurrentUserAsync()
        //{
        //    return await UserManager.FindByIdAsync(Context.User.GetUserId());
        //}

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}