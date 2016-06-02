// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2016-06-02
// 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public Guid SiteId { get; set; } = Guid.Empty;
                
        [Display (Name = "Name")]
        public string CompanyName { get; set; } = string.Empty;

        [Display(Name = "Address1")]
        public string CompanyStreetAddress { get; set; } = string.Empty;

        [Display(Name = "Address2")]
        public string CompanyStreetAddress2 { get; set; } = string.Empty;

        [Display(Name = "City")]
        public string CompanyLocality { get; set; } = string.Empty;

        [Display(Name = "Country")]
        public string CompanyCountry { get; set; } = string.Empty;
        
        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }

        [Display(Name = "State/Region")]
        public string CompanyRegion { get; set; } = string.Empty;

        [Display(Name = "Postal Code")]
        public string CompanyPostalCode { get; set; } = string.Empty;

        [Display(Name = "Phone")]
        public string CompanyPhone { get; set; } = string.Empty;

        [Display(Name = "Fax")]
        public string CompanyFax { get; set; } = string.Empty;
        
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        [Display(Name = "Public Email")]
        public string CompanyPublicEmail { get; set; } = string.Empty;

    }
}
