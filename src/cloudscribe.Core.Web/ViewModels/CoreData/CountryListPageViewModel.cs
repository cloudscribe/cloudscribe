// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2015-03-27
//

using cloudscribe.Core.Models.Geography;
using System.Collections.Generic;

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

        private bool useModals = false;

        public bool UseModals
        {
            get { return useModals; }
            set { useModals = value; }
        }
    }
}
