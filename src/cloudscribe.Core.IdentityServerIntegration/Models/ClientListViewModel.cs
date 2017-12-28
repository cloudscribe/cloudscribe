using cloudscribe.Pagination.Models;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ClientListViewModel
    {
        public ClientListViewModel()
        {
            Clients = new PagedResult<Client>();
            //Paging = new PaginationSettings();
        }

        public string SiteId { get; set; } = string.Empty;

        public PagedResult<Client> Clients { get; set; }

        //public PaginationSettings Paging { get; set; }
    }
}
