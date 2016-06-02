// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-18
// Last Modified:			2016-06-02
// 

using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class MailSettingsViewModel
    {
        [Display(Name = "SiteId")]
        public Guid SiteId { get; set; } = Guid.Empty;

        [Display(Name = "Default Email From Address")]
        [EmailAddress]
        public string DefaultEmailFromAddress { get; set; } = string.Empty;

        [Display(Name = "Default Email From Name")]
        public string DefaultEmailFromAlias { get; set; } = string.Empty;

        [Display(Name = "Smtp User")]
        public string SmtpUser { get; set; } = string.Empty;
        
        [Display(Name = "Smtp Password")]
        public string SmtpPassword { get; set; } = string.Empty;

        [Display(Name = "Smtp Server")]
        public string SmtpServer { get; set; } = string.Empty;

        [Display(Name = "Port")]
        public int SmtpPort { get; set; } = 25;
        
        public bool SmtpRequiresAuth { get; set; } = false;
        
        public bool SmtpUseSsl { get; set; } = false;
        
        [Display(Name = "Encoding - leave blank for ascii")]
        public string SmtpPreferredEncoding { get; set; } = string.Empty;

    }
}
