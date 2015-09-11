// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2015-09-11
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
        
        //[Required(ErrorMessageResourceName = "SiteNameRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //[Display(Name = "SiteName", ResourceType = typeof(CommonResources))]
        public string SiteName { get; set; } = string.Empty;
        


        //[RequiredWithConfig(RequiredKey = "SloganRequired", ErrorMessageResourceName = "SiteNameRequired", ErrorMessageResourceType = typeof(CommonResources))]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Slogan Length Error")]
        //[Display(Name = "SiteSlogan", ResourceType = typeof(CommonResources))]
        public string Slogan { get; set; } = string.Empty;
        

        //[Display(Name = "CompanyName", ResourceType = typeof(CommonResources))]
        public string CompanyName { get; set; } = string.Empty;
        
        //[Display(Name = "CompanyAddress1", ResourceType = typeof(CommonResources))]
        public string CompanyStreetAddress { get; set; } = string.Empty;
        
        //[Display(Name = "CompanyAddress2", ResourceType = typeof(CommonResources))]
        public string CompanyStreetAddress2 { get; set; } = string.Empty;

        //[Display(Name = "CompanyLocality", ResourceType = typeof(CommonResources))]
        public string CompanyLocality { get; set; } = string.Empty;
        
        //[Display(Name = "CompanyCountry", ResourceType = typeof(CommonResources))]
        public string CompanyCountry { get; set; } = string.Empty;


        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }
        
        //[Display(Name = "CompanyRegion", ResourceType = typeof(CommonResources))]
        public string CompanyRegion { get; set; } = string.Empty;
        
        //[Display(Name = "CompanyPostalCode", ResourceType = typeof(CommonResources))]
        public string CompanyPostalCode { get; set; } = string.Empty;
        
        //[Display(Name = "CompanyPhone", ResourceType = typeof(CommonResources))]
        public string CompanyPhone { get; set; } = string.Empty;
        
        //[Display(Name = "CompanyFax", ResourceType = typeof(CommonResources))]
        public string CompanyFax { get; set; } = string.Empty;
        
        [EmailAddress]
        //[Display(Name = "CompanyPublicEmail", ResourceType = typeof(CommonResources))]
        public string CompanyPublicEmail { get; set; } = string.Empty;

    }
}
