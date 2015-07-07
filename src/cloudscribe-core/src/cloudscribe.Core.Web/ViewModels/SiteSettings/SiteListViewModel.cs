// Author:					Joe Audette
// Created:					2014-11-24
// Last Modified:			2014-11-24
//

using cloudscribe.Core.Models;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SiteListViewModel
    {
        public SiteListViewModel()
        {
            Sites = new List<ISiteInfo>();
            Paging = new PaginationSettings();
        }

        public string Heading { get; set; }
        public List<ISiteInfo> Sites { get; set; }
        public PaginationSettings Paging { get; set; }
    }
}
