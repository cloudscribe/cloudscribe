// Author:					Joe Audette
// Created:					2014-11-16
// Last Modified:			2014-11-16
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using cloudscribe.Resources;
using cloudscribe.Configuration.DataAnnotations;
using cloudscribe.Core.Models.Geography;
using System.Web.Mvc;

namespace cloudscribe.Core.Web.ViewModels.CoreData
{
    public class StateListPageViewModel
    {
        public StateListPageViewModel()
        {
            Country = new GeoCountryViewModel();
            States = new List<IGeoZone>();
            Paging = new PagingInfo();
            
        }

        public string Heading { get; set; }
        public GeoCountryViewModel Country { get; set; }
        public List<IGeoZone> States { get; set; }
        public PagingInfo Paging { get; set; }

    }
}
