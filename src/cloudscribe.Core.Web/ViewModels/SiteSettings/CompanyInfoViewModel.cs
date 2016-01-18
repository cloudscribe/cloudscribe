// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2015-11-09
// 

//using cloudscribe.Configuration.DataAnnotations;
//using cloudscribe.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Mvc.Rendering;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class CompanyInfoViewModel
    {
        public CompanyInfoViewModel()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
        }

        [Display(Name = "SiteId")]
        public int SiteId { get; set; } = -1;
        
        [Display(Name = "SiteGuid")]
        public Guid SiteGuid { get; set; } = Guid.Empty;
        
        
        
        //[Display(Name = "CompanyName", ResourceType = typeof(CommonResources))]
        [Display (Name = "Name")]
        public string CompanyName { get; set; } = string.Empty;

        //[Display(Name = "CompanyAddress1", ResourceType = typeof(CommonResources))]
        [Display(Name = "Address1")]
        public string CompanyStreetAddress { get; set; } = string.Empty;

        //[Display(Name = "CompanyAddress2", ResourceType = typeof(CommonResources))]
        [Display(Name = "Address2")]
        public string CompanyStreetAddress2 { get; set; } = string.Empty;

        //[Display(Name = "CompanyLocality", ResourceType = typeof(CommonResources))]
        [Display(Name = "City")]
        public string CompanyLocality { get; set; } = string.Empty;

        //[Display(Name = "CompanyCountry", ResourceType = typeof(CommonResources))]
        [Display(Name = "Country")]
        public string CompanyCountry { get; set; } = string.Empty;


        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }

        //[Display(Name = "CompanyRegion", ResourceType = typeof(CommonResources))]
        [Display(Name = "State/Region")]
        public string CompanyRegion { get; set; } = string.Empty;

        //[Display(Name = "CompanyPostalCode", ResourceType = typeof(CommonResources))]
        [Display(Name = "Postal Code")]
        public string CompanyPostalCode { get; set; } = string.Empty;

        //[Display(Name = "CompanyPhone", ResourceType = typeof(CommonResources))]
        [Display(Name = "Phone")]
        public string CompanyPhone { get; set; } = string.Empty;

        //[Display(Name = "CompanyFax", ResourceType = typeof(CommonResources))]
        [Display(Name = "Fax")]
        public string CompanyFax { get; set; } = string.Empty;
        
        [EmailAddress]
        //[Display(Name = "CompanyPublicEmail", ResourceType = typeof(CommonResources))]
        [Display(Name = "Public Email")]
        public string CompanyPublicEmail { get; set; } = string.Empty;

    }
}
