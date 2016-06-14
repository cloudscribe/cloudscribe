// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-17
// Last Modified:			2015-06-14
//

using cloudscribe.Core.Models.Geography;
using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.CoreData
{
    public class GeoZoneViewModel : IGeoZone
    {
        public string CountryName { get; set; } = string.Empty;
        
        public Guid Id { get; set; } = Guid.Empty;
        public Guid CountryId { get; set; } = Guid.Empty;
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, ErrorMessage = "Name has a maximum length of 255 characters")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Code is required")]
        [StringLength(255, ErrorMessage = "Code has a maximum length of 255 characters")]
        public string Code { get; set; } = string.Empty;
        
        public int ReturnPageNumber { get; set; } = 1;     
        public int CountryListReturnPageNumber { get; set; } = 1;
        
        public static GeoZoneViewModel FromIGeoZone(IGeoZone geoZone)
        {
            GeoZoneViewModel model = new GeoZoneViewModel();
            model.Id = geoZone.Id;
            model.CountryId = geoZone.CountryId;
            model.Name = geoZone.Name;
            model.Code = geoZone.Code;
            return model;
        }
    }
}
