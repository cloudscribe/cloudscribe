// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-02-02
// Last Modified:			2016-06-14
// 

using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SmsSettingsViewModel
    {  
        public Guid SiteId { get; set; } = Guid.Empty;
        [StringLength(100, ErrorMessage = "SMS From has a maximum length of 100 characters")]
        public string SmsFrom { get; set; } = string.Empty;
        [StringLength(255, ErrorMessage = "SMS ClientId From has a maximum length of 255 characters")]
        public string SmsClientId { get; set; } = string.Empty;
        
        public string SmsSecureToken { get; set; } = string.Empty;
    }
}
