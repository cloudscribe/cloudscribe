// Author:					Joe Audette
// Created:					2014-11-17
// Last Modified:			2014-11-17
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
    public class GeoZoneViewModel : IGeoZone
    {
        public GeoZoneViewModel()
        {
            Country = new GeoCountryViewModel();
        }

        private string heading = string.Empty;

        public string Heading 
        {
            get { return heading; }
            set { heading = value; }
        }

        public GeoCountryViewModel Country { get; set; }

        private Guid guid = Guid.Empty;

        [Display(Name = "Guid")]
        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        private Guid countryGuid = Guid.Empty;

        [Display(Name = "CountryGuid")]
        public Guid CountryGuid
        {
            get { return countryGuid; }
            set { countryGuid = value; }
        }

        private string name = string.Empty;

        [Display(Name = "Name", ResourceType = typeof(CommonResources))]
        [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(CommonResources))]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string code = string.Empty;

        [Display(Name = "Code", ResourceType = typeof(CommonResources))]
        [Required(ErrorMessageResourceName = "CodeRequired", ErrorMessageResourceType = typeof(CommonResources))]
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        private int returnPageNumber = 1;

        public int ReturnPageNumber
        {
            get { return returnPageNumber; }
            set { returnPageNumber = value; }
        }

        public static GeoZoneViewModel FromIGeoZone(IGeoZone geoZone)
        {
            GeoZoneViewModel model = new GeoZoneViewModel();
            model.Guid = geoZone.Guid;
            model.CountryGuid = geoZone.CountryGuid;
            model.Name = geoZone.Name;
            model.Code = geoZone.Code;
            
            return model;

        }

    }
}
