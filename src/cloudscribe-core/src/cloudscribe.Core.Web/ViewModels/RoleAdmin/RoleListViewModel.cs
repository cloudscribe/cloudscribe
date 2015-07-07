// Author:					Joe Audette
// Created:					2014-12-04
// Last Modified:			2015-01-05
//

using cloudscribe.Core.Models;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.RoleAdmin
{
    public class RoleListViewModel
    {
        public RoleListViewModel()
        {
            SiteRoles = new List<ISiteRole>();
            Paging = new PaginationSettings();
        }

        public string Heading { get; set; }
        public IList<ISiteRole> SiteRoles { get; set; }
        public PaginationSettings Paging { get; set; }

        //TODO: we don't currently have db paging for roles but we might want that
        //public PagingInfo Paging { get; set; }
    }
}
