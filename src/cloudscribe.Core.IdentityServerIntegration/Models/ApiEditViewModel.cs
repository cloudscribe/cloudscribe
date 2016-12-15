using IdentityServer4.Models;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ApiEditViewModel
    {
        public ApiEditViewModel()
        {
            NewApi = new ApiItemViewModel();
            NewApiClaim = new NewApiClaimViewModel();
            NewApiSecret = new NewApiSecretViewModel();
            NewScope = new NewApiScopeViewModel();
        }

        public string SiteId { get; set; } = string.Empty;

        public ApiResource CurrentApi { get; set; } = null;

        public ApiItemViewModel NewApi { get; set; }

        public NewApiClaimViewModel NewApiClaim { get; set; }

        public NewApiSecretViewModel NewApiSecret { get; set; }

        public NewApiScopeViewModel NewScope { get; set; }
    }
}
