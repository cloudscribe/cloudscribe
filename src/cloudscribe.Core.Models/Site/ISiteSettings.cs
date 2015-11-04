// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2015-11-05
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

    // the full monty
    // need to review each and consider removing some
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

        //LdapSettings SiteLdapSettings { get; set; }


        // TODO use this to force ONLY social logins or ldap option?
        bool DisableDbAuth { get; set; }
        bool UseSecureRegistration { get; set; } // rename to reflect function RequireConfirmedEmailAddress
        bool RequireApprovalBeforeLogin { get; set; } // TODO: implement
        int MaxInvalidPasswordAttempts { get; set; }
        int MinReqNonAlphaChars { get; set; }
        int MinRequiredPasswordLength { get; set; }
        int PasswordAttemptWindowMinutes { get; set; }
        //int PasswordFormat { get; set; } // only used for migration from mojoportal
        bool UseEmailForLogin { get; set; }
        
        bool UseSslOnAllPages { get; set; }
        //string PasswordRegexWarning { get; set; }
       // string PasswordStrengthRegularExpression { get; set; }
        //bool RequireEnterEmailTwiceOnRegistration { get; set; }
        bool RequirePasswordChangeOnResetRecover { get; set; }
        bool RequiresQuestionAndAnswer { get; set; }
        bool ReallyDeleteUsers { get; set; }
        bool AllowNewRegistration { get; set; }
        //bool AllowOpenIdAuth { get; set; } //remove
        
        //bool AllowPasswordReset { get; set; } //remove
        //bool AllowPasswordRetrieval { get; set; } //remove
        bool AllowPersistentLogin { get; set; }
        bool AllowUserFullNameChange { get; set; }

        bool CaptchaOnLogin { get; set; }
        bool CaptchaOnRegistration { get; set; }
        string RecaptchaPrivateKey { get; set; }
        string RecaptchaPublicKey { get; set; }

        //probably much of this should be moved to a new apppermissions table
        //bool RequiresUniqueEmail { get; set; }

        //string RolesThatCanApproveNewUsers { get; set; }
        //string RolesThatCanCreateUsers { get; set; }
        //string RolesThatCanEditGoogleAnalyticsQueries { get; set; }
        //string RolesThatCanLookupUsers { get; set; }
        //string RolesThatCanManageUsers { get; set; }
        //string RolesThatCanViewGoogleAnalytics { get; set; }
        //string RolesThatCanViewMemberList { get; set; }

        //string UserFilesBrowseAndUploadRoles { get; set; }
        //string GeneralBrowseAndUploadRoles { get; set; }
        //string RolesThatCanDeleteFilesInEditor { get; set; }
        
        

        //company info
        //string Slogan { get; set; }
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
        //Guid CurrencyGuid { get; set; }
        //Guid DefaultCountryGuid { get; set; }
        string DefaultEmailFromAddress { get; set; }
        //string DefaultFromEmailAlias { get; set; }
       // Guid DefaultStateGuid { get; set; }
        
        string LoginInfoBottom { get; set; }
        string LoginInfoTop { get; set; }
        string RegistrationAgreement { get; set; }
        string RegistrationPreamble { get; set; }
        
        string TimeZoneId { get; set; }
        bool SiteIsClosed { get; set; }
        string SiteIsClosedMessage { get; set; }
        
        string SmtpPassword { get; set; }
        int SmtpPort { get; set; }
        string SmtpPreferredEncoding { get; set; }
        bool SmtpRequiresAuth { get; set; }
        string SmtpServer { get; set; }
        string SmtpUser { get; set; }
        bool SmtpUseSsl { get; set; }
        
        
        //string OpenSearchName { get; set; }
        //string PrimarySearchEngine { get; set; }
        string PrivacyPolicy { get; set; }
        
        //string MetaProfile { get; set; }
        //string Icon { get; set; }
        //string Logo { get; set; }
        string Layout { get; set; }
        //string MobileSkin { get; set; }
        string SiteMapSkin { get; set; }
        Guid SkinVersion { get; set; }
        //bool AllowUserSkins { get; set; }


        //string GoogleAnalyticsAccountCode { get; set; }
        //string GoogleAnalyticsEmail { get; set; }
        //string GoogleAnalyticsPassword { get; set; }
        string GoogleAnalyticsProfileId { get; set; }
        //string GoogleAnalyticsSettings { get; set; }
        //string GoogleCustomSearchId { get; set; }
        //string BingAPIId { get; set; }
        //string GmapApiKey { get; set; }
        //string CaptchaProvider { get; set; } //remove
        //string CommentProvider { get; set; } //remove
        //string IntenseDebateAccountId { get; set; }
        //string AvatarSystem { get; set; }

        //social login stuff
        string FacebookAppId { get; set; }
        string FacebookAppSecret { get; set; }
        string MicrosoftClientId { get; set; }
        string MicrosoftClientSecret { get; set; }
        string GoogleClientId { get; set; }
        string GoogleClientSecret { get; set; }
        string TwitterConsumerKey { get; set; }
        string TwitterConsumerSecret { get; set; }

        //string WordpressApiKey { get; set; }
        //string DisqusSiteShortName { get; set; }
        string AddThisDotComUsername { get; set; }

        string ApiKeyExtra1 { get; set; } 
        string ApiKeyExtra2 { get; set; } 
        string ApiKeyExtra3 { get; set; }
        string ApiKeyExtra4 { get; set; } 
        string ApiKeyExtra5 { get; set; }

        
        //string NewsletterEditor { get; set; } // remove or move to cms application settings

        // cms stuff to remove or move to cms application settings
        //bool AllowHideMenuOnPages { get; set; } 
        //bool AllowPageSkins { get; set; } 
        //string EditorProviderName { get; set; } 
        //string EmailAdressesForUserApprovalNotification { get; set; }
        //bool EnableContentWorkflow { get; set; }
        //bool ForceContentVersioning { get; set; }
        //string RolesNotAllowedToEditModuleSettings { get; set; }
        //string RolesThatCanAssignSkinsToPages { get; set; }
        //string RolesThatCanCreateRootPages { get; set; }
        //string RolesThatCanEditContentTemplates { get; set; }
        //string RolesThatCanManageSkins { get; set; }
        //string SiteRootDraftApprovalRoles { get; set; }
        //string SiteRootDraftEditRoles { get; set; }
        //string SiteRootEditRoles { get; set; }
        //string DefaultRootPageCreateChildPageRoles { get; set; }
        //string DefaultRootPageEditRoles { get; set; }
        //string DefaultRootPageViewRoles { get; set; }

        

        // these probobly won't be used
        //string RpxNowAdminUrl { get; set; }
        //string RpxNowApiKey { get; set; }
        //string RpxNowApplicationName { get; set; }
        //string CommerceReportViewRoles { get; set; }

        //bool ShowAlternateSearchIfConfigured { get; set; }
        //bool ShowPasswordStrengthOnRegistration { get; set; }
    }
}
