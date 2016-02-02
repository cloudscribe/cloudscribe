// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-02-02
// Last Modified:			2016-02-02
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SmsSettingsViewModel
    {
        [Display(Name = "SiteId")]
        public int SiteId { get; set; } = -1;

        [Display(Name = "SiteGuid")]
        public Guid SiteGuid { get; set; } = Guid.Empty;

        [Display(Name = "Sms From")]
        public string SmsFrom { get; set; } = string.Empty;

        //[Display(Name = "SmtpPassword", ResourceType = typeof(CommonResources))]

        [Display(Name = "Sms Client Id")]
        public string SmsClientId { get; set; } = string.Empty;

        //[Display(Name = "SmtpServer", ResourceType = typeof(CommonResources))]
        [Display(Name = "Sms Secure Token")]
        public string SmsSecureToken { get; set; } = string.Empty;
    }
}
