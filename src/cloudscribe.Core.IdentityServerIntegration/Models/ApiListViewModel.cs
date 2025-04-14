using cloudscribe.Pagination.Models;
using IdentityServer4.Models;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ApiListViewModel
    {
        public ApiListViewModel()
        {
            Apis = new PagedResult<ApiResource>();
        }

        public string SiteId { get; set; } = string.Empty;

        public PagedResult<ApiResource> Apis { get; set; }
    }
}
