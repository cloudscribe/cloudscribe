// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-19
// Last Modified:			2019-04-20
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

        [Display(Name = "Email addresses (csv) to notify of new users")]
        public string AccountApprovalEmailCsv { get; set; } = string.Empty;

        [Display(Name = "Allow Persistent Login")]
        public bool AllowPersistentLogin { get; set; }

        [Display(Name = "Only Use Social Authentication")]
        public bool DisableDbAuth { get; set; }
        
        public bool HasAnySocialAuthEnabled { get; set; } = false;

        public bool Require2FA { get; set; }

        public bool SingleBrowserSessions { get; set; }

        //LDAP

        //public bool UseLdapAuth { get; set; } = false;
        //public bool AllowDbFallbackWithLdap { get; set; } = false;
        //public bool EmailLdapDbFallback { get; set; } = false;
        //public bool AutoCreateLdapUserOnFirstLogin { get; set; } = true;


        public string LdapServer { get; set; }

        public string LdapDomain { get; set; }

        public int LdapPort { get; set; } = 389;

        public string LdapRootDN { get; set; }

        public string LdapUserDNKey { get; set; } = "CN";

        public string LdapUserDNFormat { get; set; } = "username@LDAPDOMAIN"; // or LDAPDOMAIN\username

        public bool LdapUseSsl { get; set; }


        public string LdapTestUsername { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string LdapTestPassword { get; set; }

    }
}
