// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-17
// Last Modified:			2015-06-10
//

using cloudscribe.Core.Models.Geography;
//using cloudscribe.Resources;
using System;
using System.ComponentModel.DataAnnotations;

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

        //[Display(Name = "Name", ResourceType = typeof(CommonResources))]
        //[Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(CommonResources))]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string code = string.Empty;

        //[Display(Name = "Code", ResourceType = typeof(CommonResources))]
        //[Required(ErrorMessageResourceName = "CodeRequired", ErrorMessageResourceType = typeof(CommonResources))]
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

        private int countryListReturnPageNumber = 1;

        public int CountryListReturnPageNumber
        {
            get { return countryListReturnPageNumber; }
            set { countryListReturnPageNumber = value; }
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
