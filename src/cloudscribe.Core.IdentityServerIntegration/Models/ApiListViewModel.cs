
using cloudscribe.Web.Pagination;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ApiListViewModel
    {
        public ApiListViewModel()
        {
            Apis = new List<ApiResource>();
            Paging = new PaginationSettings();
        }

        public string SiteId { get; set; } = string.Empty;

        public IList<ApiResource> Apis { get; set; }

        public PaginationSettings Paging { get; set; }

    }
}
