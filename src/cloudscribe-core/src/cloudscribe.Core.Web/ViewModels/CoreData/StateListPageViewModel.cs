// Author:					Joe Audette
// Created:					2014-11-16
// Last Modified:			2015-06-10
//

using cloudscribe.Core.Models.Geography;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.CoreData
{
    public class StateListPageViewModel
    {
        public StateListPageViewModel()
        {
            Country = new GeoCountryViewModel();
            States = new List<IGeoZone>();
            Paging = new PaginationSettings();

        }

        public string Heading { get; set; }
        public GeoCountryViewModel Country { get; set; }
        public List<IGeoZone> States { get; set; }
        public PaginationSettings Paging { get; set; }

        private int returnPageNumber = 1;

        public int ReturnPageNumber
        {
            get { return returnPageNumber; }
            set { returnPageNumber = value; }
        }

        private int countryListReturnPageNumber = 1;

        public int CountryListReturnPageNumber
        {
            get { return countryListReturnPageNumber; }
            set { countryListReturnPageNumber = value; }
        }

    }
}
