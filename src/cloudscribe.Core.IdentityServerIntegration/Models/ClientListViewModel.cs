using cloudscribe.Pagination.Models;
using IdentityServer4.Models;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ClientListViewModel
    {
        public ClientListViewModel()
        {
            Clients = new PagedResult<Client>();
        }

        public string SiteId { get; set; } = string.Empty;

        public PagedResult<Client> Clients { get; set; }
    }
}
