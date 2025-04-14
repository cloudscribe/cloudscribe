using cloudscribe.Pagination.Models;
using IdentityServer4.Models;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class IdentityListViewModel
    {
        public IdentityListViewModel()
        {
            IdentityResources = new PagedResult<IdentityResource>();
        }

        public string SiteId { get; set; } = string.Empty;

        public PagedResult<IdentityResource> IdentityResources { get; set; }
    }
}
