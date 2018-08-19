// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-19
// Last Modified:			2018-86-18
// 

using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SecuritySettingsViewModel
    {
        
        [Display(Name = "SiteId")]
        public Guid SiteId { get; set; } = Guid.Empty;

        [Display(Name = "Use Email For Login")]
        public bool UseEmailForLogin { get; set; }

        [Display(Name = "Allow New Registrations")]
        public bool AllowNewRegistration { get; set; }

        public bool EmailIsConfigured { get; set; } = false;

        [Display(Name = "Require Confirmed Email")]
        public bool RequireConfirmedEmail { get; set; }

        public bool SmsIsConfigured { get; set; } = false;

        [Display(Name = "Require Confirmed Phone")]
        public bool RequireConfirmedPhone { get; set; }

        [Display(Name = "Require Admin Account Approval")]
        public bool RequireApprovalBeforeLogin { get; set; }

        [Display(Name = "Email addresses (csv) to notify of new unapproved users")]
        public string AccountApprovalEmailCsv { get; set; } = string.Empty;

        [Display(Name = "Allow Persistent Login")]
        public bool AllowPersistentLogin { get; set; }

        [Display(Name = "Only Use Social Authentication")]
        public bool DisableDbAuth { get; set; }
        
        public bool HasAnySocialAuthEnabled { get; set; } = false;

    }
}
