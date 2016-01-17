// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2015-11-12
// 

using System;

namespace cloudscribe.Core.Models
{
    // this interface is based on mojoportal and is what it is for historical reasons
    // and for compatibility with mojoportal data, but refactored a bit for cloudscribe
    // and the only concrete repositories we ship use mojoportal data access code and schema

    // lighter base version for lists
    public interface ISiteInfo
    {
        int SiteId { get; set; }
        Guid SiteGuid { get; set; }
        string SiteName { get; set; }
        string PreferredHostName { get; set; }
        string SiteFolderName { get; set; }
        bool IsServerAdminSite { get; set; }

    }

    
    public interface ISiteSettings : ISiteInfo
    {
        //TODO: implement ldap auth middleware or helper for accountcontroller
        bool UseLdapAuth { get; set; }
        bool AllowDbFallbackWithLdap { get; set; }
        bool EmailLdapDbFallback { get; set; }
        bool AutoCreateLdapUserOnFirstLogin { get; set; }
        string LdapServer { get; set; }
        string LdapDomain { get; set; }
        int LdapPort { get; set; }
        string LdapRootDN { get; set; }
        string LdapUserDNKey { get; set; }

        // TODO use this to force ONLY social logins or ldap option?
        bool DisableDbAuth { get; set; }
        bool UseSecureRegistration { get; set; } // rename to reflect function RequireConfirmedEmailAddress
        bool RequireApprovalBeforeLogin { get; set; } // TODO: implement
        int MaxInvalidPasswordAttempts { get; set; }
        int MinReqNonAlphaChars { get; set; }
        int MinRequiredPasswordLength { get; set; }
        int PasswordAttemptWindowMinutes { get; set; }
        bool UseEmailForLogin { get; set; }
        bool UseSslOnAllPages { get; set; }
       
        bool RequiresQuestionAndAnswer { get; set; }
        bool ReallyDeleteUsers { get; set; }
        bool AllowNewRegistration { get; set; }
        
        bool AllowPersistentLogin { get; set; }
        bool AllowUserFullNameChange { get; set; }

        bool CaptchaOnLogin { get; set; }
        bool CaptchaOnRegistration { get; set; }
        string RecaptchaPrivateKey { get; set; }
        string RecaptchaPublicKey { get; set; }
        
        //company info
        string CompanyCountry { get; set; }
        string CompanyFax { get; set; }
        string CompanyLocality { get; set; }
        string CompanyName { get; set; }
        string CompanyPhone { get; set; }
        string CompanyPostalCode { get; set; }
        string CompanyPublicEmail { get; set; }
        string CompanyRegion { get; set; }
        string CompanyStreetAddress { get; set; }
        string CompanyStreetAddress2 { get; set; }
        
        string LoginInfoBottom { get; set; }
        string LoginInfoTop { get; set; }
        string RegistrationAgreement { get; set; }
        string RegistrationPreamble { get; set; }
        
        string TimeZoneId { get; set; }
        bool SiteIsClosed { get; set; }
        string SiteIsClosedMessage { get; set; }

        string DefaultEmailFromAddress { get; set; }
        string SmtpPassword { get; set; }
        int SmtpPort { get; set; }
        string SmtpPreferredEncoding { get; set; }
        bool SmtpRequiresAuth { get; set; }
        string SmtpServer { get; set; }
        string SmtpUser { get; set; }
        bool SmtpUseSsl { get; set; }
        
        string PrivacyPolicy { get; set; }
        string Layout { get; set; }
        string GoogleAnalyticsProfileId { get; set; }
        
        //social login stuff
        string FacebookAppId { get; set; }
        string FacebookAppSecret { get; set; }
        string MicrosoftClientId { get; set; }
        string MicrosoftClientSecret { get; set; }
        string GoogleClientId { get; set; }
        string GoogleClientSecret { get; set; }
        string TwitterConsumerKey { get; set; }
        string TwitterConsumerSecret { get; set; }

        string AddThisDotComUsername { get; set; }

        string ApiKeyExtra1 { get; set; } 
        string ApiKeyExtra2 { get; set; } 
        string ApiKeyExtra3 { get; set; }
        string ApiKeyExtra4 { get; set; } 
        string ApiKeyExtra5 { get; set; }

        //bool IsDataProtected { get; set; }
        //DateTime CreatedUtc { get; set; }
   
    }
}
