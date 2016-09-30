using cloudscribe.Core.Identity;
using IdentityServer4.Services;
using System.Threading.Tasks;

namespace IdentityServer4.cloudscribeIdentity
{
    public class Integration : IIdentityServerIntegration
    {
        public Integration(
            IIdentityServerInteractionService interaction
            )
        {
            _interaction = interaction;
        }

        private readonly IIdentityServerInteractionService _interaction;

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
