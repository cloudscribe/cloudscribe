// Author:					Joe Audette
// Created:					2014-12-04
// Last Modified:			2014-12-04
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
        }

        public string Heading { get; set; }
        public IList<ISiteRole> SiteRoles { get; set; }

        //TODO: we don't currently have db paging for roles but we might want that
        //public PagingInfo Paging { get; set; }
    }
}
