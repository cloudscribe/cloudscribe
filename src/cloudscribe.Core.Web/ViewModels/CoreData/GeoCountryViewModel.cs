// Author:					Joe Audette
// Created:					2014-11-15
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
    public class GeoCountryViewModel : IGeoCountry
    {
        
        private Guid guid = Guid.Empty;

        [Display(Name = "Guid")]
        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }


        private string name = string.Empty;

        [Display(Name = "Name", ResourceType = typeof(CommonResources))]
        [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(CommonResources))]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string iSOCode2 = string.Empty;

        [Display(Name = "ISOCode2", ResourceType = typeof(CommonResources))]
        [Required(ErrorMessageResourceName = "ISOCode2Required", ErrorMessageResourceType = typeof(CommonResources))]
        public string ISOCode2
        {
            get { return iSOCode2; }
            set { iSOCode2 = value; }
        }


        private string iSOCode3 = string.Empty;

        [Display(Name = "ISOCode3", ResourceType = typeof(CommonResources))]
        [Required(ErrorMessageResourceName = "ISOCode3Required", ErrorMessageResourceType = typeof(CommonResources))]
        public string ISOCode3
        {
            get { return iSOCode3; }
            set { iSOCode3 = value; }
        }

        public static GeoCountryViewModel FromIGeoCountry(IGeoCountry geoCountry)
        {
            GeoCountryViewModel model = new GeoCountryViewModel();
            model.Guid = geoCountry.Guid;
            model.Name = geoCountry.Name;
            model.ISOCode2 = geoCountry.ISOCode2;
            model.ISOCode3 = geoCountry.ISOCode3;

            return model;

        }

    }
}
