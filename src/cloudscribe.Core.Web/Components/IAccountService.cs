using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Core.Web.ViewModels.Account;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public interface IAccountService
    {
        Task<bool> AcceptRegistrationAgreement(ClaimsPrincipal principal);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string returnUrl = null);
        Task<VerifyEmailResult> ConfirmEmailAsync(string userId, string code);
        Task<VerifyEmailInfo> GetEmailVerificationInfo(Guid userId);
        Task<List<AuthenticationScheme>> GetExternalAuthenticationSchemes();
        Task<ResetPasswordInfo> GetPasswordResetInfo(string email);
        Task<IUserContext> GetTwoFactorAuthenticationUserAsync();
        Task<TwoFactorInfo> GetTwoFactorInfo(string provider = null);
        Task HandleUserRolesChanged(ClaimsPrincipal principal);
        bool IsSignedIn(ClaimsPrincipal user);
        Task<bool> LoginNameIsAvailable(Guid userId, string loginName);
        Task<ResetPasswordResult> ResetPassword(string email, string password, string resetCode);
        Task SignOutAsync();
        Task<UserLoginResult> Try2FaLogin(LoginWith2faViewModel model, bool rememberMe);
        Task<UserLoginResult> TryExternalLogin(string providedEmail = "", bool? didAcceptTerms = null);
        Task<UserLoginResult> TryLogin(LoginViewModel model);
        Task<UserLoginResult> TryLoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model);
        Task<UserLoginResult> TryRegister(RegisterViewModel model, ModelStateDictionary modelState, HttpContext httpContext, IHandleCustomRegistration customRegistration);
        Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool rememberMe, bool rememberBrowser);
        Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string code);
        Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool rememberMe, bool rememberBrowser);
    }
}