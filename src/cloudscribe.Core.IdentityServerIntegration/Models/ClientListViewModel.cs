using cloudscribe.Web.Pagination;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ClientListViewModel
    {
        public ClientListViewModel()
        {
            Clients = new List<Client>();
            Paging = new PaginationSettings();
        }

        public string SiteId { get; set; } = string.Empty;

        public IList<Client> Clients { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
