
using cloudscribe.Web.Pagination;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ScopeListViewModel
    {
        public ScopeListViewModel()
        {
            Scopes = new List<Scope>();
            Paging = new PaginationSettings();
        }

        public Guid SiteId { get; set; } = Guid.Empty;

        public IList<Scope> Scopes { get; set; }

        public PaginationSettings Paging { get; set; }

    }
}
