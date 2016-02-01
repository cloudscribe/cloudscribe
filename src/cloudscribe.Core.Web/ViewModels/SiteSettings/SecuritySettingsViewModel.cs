// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-19
// Last Modified:			2016-02-01
// 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Mvc.Rendering;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SecuritySettingsViewModel
    {
        [Display(Name = "SiteId")]
        public int SiteId { get; set; } = -1;

        [Display(Name = "SiteGuid")]
        public Guid SiteGuid { get; set; } = Guid.Empty;

        [Display(Name = "Use Email For Login")]
        public bool UseEmailForLogin { get; set; }

        [Display(Name = "Allow New Registrations")]
        public bool AllowNewRegistration { get; set; }

        [Display(Name = "Require Confirmed Email")]
        public bool RequireConfirmedEmail { get; set; }

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

        //[Display(Name = "Minimum Password Length")]
        //public int MinRequiredPasswordLength { get; set; }
        
        //[Display(Name = "Minimum Password Non Alphabetic Characters")]
        //public int MinReqNonAlphaChars { get; set; }

        //[Display(Name = "Max Invalid Password Attempts")]
        //public int MaxInvalidPasswordAttempts { get; set; }

        //[Display(Name = "Password Attempt Window Minutes")]
        //public int PasswordAttemptWindowMinutes { get; set; }
        
        //[Display(Name = "Require Security Question and Answer")]
        //public bool RequiresQuestionAndAnswer { get; set; }

        [Display(Name = "Really Delete Users")]
        public bool ReallyDeleteUsers { get; set; }

        //[Display(Name = "Allow Full Name Changes")]
        //public bool AllowUserFullNameChange { get; set; }

    }
}
