// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2016-06-14
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
        
        public Guid SiteId { get; set; } = Guid.Empty;
                
        [StringLength(250, ErrorMessage = "Company name has a maximum length of 250 characters")]
        public string CompanyName { get; set; } = string.Empty;
        [StringLength(250, ErrorMessage = "Address 1 has a maximum length of 250 characters")]
        public string CompanyStreetAddress { get; set; } = string.Empty;
        [StringLength(250, ErrorMessage = "Address 2 has a maximum length of 250 characters")]
        public string CompanyStreetAddress2 { get; set; } = string.Empty;
        [StringLength(200, ErrorMessage = "City has a maximum length of 200 characters")]
        public string CompanyLocality { get; set; } = string.Empty;

        public string CompanyCountry { get; set; } = string.Empty;
        
        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }
        [StringLength(200, ErrorMessage = "State/Region has a maximum length of 200 characters")]
        public string CompanyRegion { get; set; } = string.Empty;
        [StringLength(20, ErrorMessage = "Zip/Postal code has a maximum length of 20 characters")]
        public string CompanyPostalCode { get; set; } = string.Empty;
        [StringLength(20, ErrorMessage = "Phone has a maximum length of 20 characters")]
        public string CompanyPhone { get; set; } = string.Empty;
        [StringLength(20, ErrorMessage = "Fax has a maximum length of 20 characters")]
        public string CompanyFax { get; set; } = string.Empty;
        
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        [StringLength(100, ErrorMessage = "Email has a maximum length of 100 characters")]
        public string CompanyPublicEmail { get; set; } = string.Empty;

    }
}
