using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.Core.Identity
{
    public interface IIdentityServerIntegration
    {
        Task<IdentityServerLoggedOutViewModel> GetLogoutContextModelAsync(string logoutId);
        
        Task<string> GetAuthorizationContextAsync(string returnUrl);

        Task<string> GetLogoutContextClientIdAsync(string logoutId);
    }

    public class IdentityServerLogoutViewModel
    {
        public string LogoutId { get; set; }
    }

    public class IdentityServerLoggedOutViewModel
    {
        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
        public string SignOutIframeUrl { get; set; }
    }

    public class NotIntegratedIdentityServerIntegration : IIdentityServerIntegration
    {
        public Task<string> GetAuthorizationContextAsync(string returnUrl)
        {
            return Task.FromResult("");
        }

        public Task<string> GetLogoutContextClientIdAsync(string logoutId)
        {
            return Task.FromResult("");
        }

        public Task<IdentityServerLoggedOutViewModel> GetLogoutContextModelAsync(string logoutId)
        {
            throw new NotImplementedException();
        }
    }


}
