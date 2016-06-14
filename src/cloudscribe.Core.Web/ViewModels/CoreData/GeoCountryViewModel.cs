// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2015-06-14
//

using cloudscribe.Core.Models.Geography;
using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.CoreData
{
    public class GeoCountryViewModel : IGeoCountry
    {
        public Guid Id { get; set; } = Guid.Empty;
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, ErrorMessage = "Name has a maximum length of 255 characters")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "ISOCode2 is required")]
        [StringLength(2, ErrorMessage = "ISOCode2 has a maximum length of 2 characters")]
        public string ISOCode2 { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "ISOCode3 is required")]
        [StringLength(3, ErrorMessage = "ISOCode3 has a maximum length of 3 characters")]
        public string ISOCode3 { get; set; } = string.Empty;
        
        public int ReturnPageNumber { get; set; } = 1;
        
        public static GeoCountryViewModel FromIGeoCountry(IGeoCountry geoCountry)
        {
            GeoCountryViewModel model = new GeoCountryViewModel();
            model.Id = geoCountry.Id;
            model.Name = geoCountry.Name;
            model.ISOCode2 = geoCountry.ISOCode2;
            model.ISOCode3 = geoCountry.ISOCode3;
            return model;
        }
    }
}
