using IdentityServer4.Models;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ScopeEditViewModel
    {
        public ScopeEditViewModel()
        {

        }

        public string SiteId { get; set; } = string.Empty;

        public Scope CurrentScope { get; set; } = null;
    }
}
