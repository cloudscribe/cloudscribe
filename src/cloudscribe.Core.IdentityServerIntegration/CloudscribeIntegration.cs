using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class CloudscribeIntegration : IIdentityServerIntegration
    {
        public CloudscribeIntegration(
            IIdentityServerInteractionService interaction
            )
        {
            _interaction = interaction;
        }

        private readonly IIdentityServerInteractionService _interaction;

        public string EnsureFolderSegmentIfNeeded(ISiteContext site, string returnUrl)
        {
            // only adjust if the return url is an endpoint url
            if (!IsEndpointReturnUrl(returnUrl)) return returnUrl;
            if (site == null) return returnUrl;
            if (string.IsNullOrEmpty(site.SiteFolderName)) return returnUrl;
            var folderSegment = "/" + site.SiteFolderName;
            if (returnUrl.StartsWith(folderSegment)) return returnUrl;

            return folderSegment + returnUrl;
        }

        private bool IsEndpointReturnUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;

            if (url.StartsWith("/" + CustomConstants.ProtocolRoutePaths.Authorize)) return true;
            if (url.StartsWith("/" + CustomConstants.ProtocolRoutePaths.AuthorizeAfterConsent)) return true;
            if (url.StartsWith("/" + CustomConstants.ProtocolRoutePaths.AuthorizeAfterLogin)) return true;
            if (url.StartsWith("/" + CustomConstants.ProtocolRoutePaths.CheckSession)) return true;
            if (url.StartsWith("/" + CustomConstants.ProtocolRoutePaths.DiscoveryConfiguration)) return true;
            if (url.StartsWith("/" + CustomConstants.ProtocolRoutePaths.EndSession)) return true;
            if (url.StartsWith("/" + CustomConstants.ProtocolRoutePaths.EndSessionCallback)) return true;
            if (url.StartsWith("/" + CustomConstants.ProtocolRoutePaths.Introspection)) return true;
            if (url.StartsWith("/" + CustomConstants.ProtocolRoutePaths.Revocation)) return true;
            if (url.StartsWith("/" + CustomConstants.ProtocolRoutePaths.Token)) return true;
            if (url.StartsWith("/" + CustomConstants.ProtocolRoutePaths.UserInfo)) return true;
            
            return false;
        }

        public async Task<string> GetAuthorizationContextAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            return (context?.IdP != null) ? context.IdP : string.Empty;
            
        }

        public async Task<IdentityServerLoggedOutViewModel> GetLogoutContextModelAsync(string logoutId)
        {
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new IdentityServerLoggedOutViewModel
            {
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = logout?.ClientId,
                SignOutIframeUrl = logout?.SignOutIFrameUrl
            };

            return vm;
        }

        public async Task<string> GetLogoutContextClientIdAsync(string logoutId)
        {
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            return (context?.ClientId != null) ? context.ClientId : string.Empty;

        }
    }
}
