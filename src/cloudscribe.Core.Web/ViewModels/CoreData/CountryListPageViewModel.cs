// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2014-11-15
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cloudscribe.Core.Models.Geography;

namespace cloudscribe.Core.Web.ViewModels.CoreData
{
    public class CountryListPageViewModel
    {
        public CountryListPageViewModel()
        {
            Paging = new PagingInfo();
            
        }

        public string Heading { get; set; }
        public List<IGeoCountry> Countries { get; set; }
        public PagingInfo Paging { get; set; }
    }
}
