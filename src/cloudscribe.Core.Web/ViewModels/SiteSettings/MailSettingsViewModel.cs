// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-18
// Last Modified:			2016-01-18
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class MailSettingsViewModel
    {
        [Display(Name = "SiteId")]
        public int SiteId { get; set; } = -1;

        [Display(Name = "SiteGuid")]
        public Guid SiteGuid { get; set; } = Guid.Empty;

        //[Display(Name = "DefaultEmailFromAddress", ResourceType = typeof(CommonResources))]
        [Display(Name = "Default Email From Address")]
        [EmailAddress]
        public string DefaultEmailFromAddress { get; set; } = string.Empty;

        //[Display(Name = "DefaultEmailFromAddress", ResourceType = typeof(CommonResources))]
        [Display(Name = "Default Email From Name")]
        public string DefaultEmailFromAlias { get; set; } = string.Empty;

        //[Display(Name = "SmtpUser", ResourceType = typeof(CommonResources))]
        [Display(Name = "Smtp User")]
        public string SmtpUser { get; set; } = string.Empty;

        //[Display(Name = "SmtpPassword", ResourceType = typeof(CommonResources))]
        
        [Display(Name = "Password")]
        public string SmtpPassword { get; set; } = string.Empty;

        //[Display(Name = "SmtpServer", ResourceType = typeof(CommonResources))]
        [Display(Name = "Smtp Server")]
        public string SmtpServer { get; set; } = string.Empty;

        //[Display(Name = "SmtpPort", ResourceType = typeof(CommonResources))]
        [Display(Name = "Port")]
        public int SmtpPort { get; set; } = 25;

        //[Display(Name = "SmtpRequiresAuth", ResourceType = typeof(CommonResources))]
        
        public bool SmtpRequiresAuth { get; set; } = false;

        //[Display(Name = "SmtpUseSsl", ResourceType = typeof(CommonResources))]
        public bool SmtpUseSsl { get; set; } = false;

        //[Display(Name = "SmtpPreferredEncoding", ResourceType = typeof(CommonResources))]
        [Display(Name = "Encoding - leave blank for ascii")]
        public string SmtpPreferredEncoding { get; set; } = string.Empty;



    }
}
