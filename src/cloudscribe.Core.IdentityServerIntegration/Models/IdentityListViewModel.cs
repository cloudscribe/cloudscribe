using cloudscribe.Pagination.Models;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class IdentityListViewModel
    {
        public IdentityListViewModel()
        {
            IdentityResources = new PagedResult<IdentityResource>();
            //Paging = new PaginationSettings();
        }

        public string SiteId { get; set; } = string.Empty;

        public PagedResult<IdentityResource> IdentityResources { get; set; }

        //public PaginationSettings Paging { get; set; }
    }
}
