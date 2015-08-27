// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2014-11-24
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
    public interface ISiteSettings : ISiteInfo
    {
        
        bool AllowDbFallbackWithLdap { get; set; }
        bool AllowEmailLoginWithLdapDbFallback { get; set; }
        bool AllowHideMenuOnPages { get; set; }
        bool AllowNewRegistration { get; set; }
        bool AllowOpenIdAuth { get; set; }
        bool AllowPageSkins { get; set; }
        bool AllowPasswordReset { get; set; }
        bool AllowPasswordRetrieval { get; set; }
        bool AllowPersistentLogin { get; set; }
        bool AllowUserFullNameChange { get; set; }
        bool AllowUserSkins { get; set; }
        bool AutoCreateLdapUserOnFirstLogin { get; set; }
        string AvatarSystem { get; set; }
        
        
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
        Guid CurrencyGuid { get; set; }
        Guid DefaultCountryGuid { get; set; }
        string DefaultEmailFromAddress { get; set; }
        string DefaultFromEmailAlias { get; set; }
        
        Guid DefaultStateGuid { get; set; }
        bool DisableDbAuth { get; set; }
        
        string EditorProviderName { get; set; }
        string EmailAdressesForUserApprovalNotification { get; set; }
        bool EnableContentWorkflow { get; set; }
        
        bool ForceContentVersioning { get; set; }
        string GeneralBrowseAndUploadRoles { get; set; }
        string GmapApiKey { get; set; }
        string GoogleAnalyticsAccountCode { get; set; }
        //string GoogleAnalyticsEmail { get; set; }
        //string GoogleAnalyticsPassword { get; set; }
        string GoogleAnalyticsProfileId { get; set; }
        string GoogleAnalyticsSettings { get; set; }
        string GoogleCustomSearchId { get; set; }
        string Icon { get; set; }
        string IntenseDebateAccountId { get; set; }

        string LoginInfoBottom { get; set; }
        string LoginInfoTop { get; set; }
        string Logo { get; set; }
        int MaxInvalidPasswordAttempts { get; set; }
        string MetaProfile { get; set; }
        int MinRequiredNonAlphanumericCharacters { get; set; }
        int MinRequiredPasswordLength { get; set; }
        string MobileSkin { get; set; }
        string NewsletterEditor { get; set; }
        string OpenSearchName { get; set; }
        int PasswordAttemptWindowMinutes { get; set; }
        int PasswordFormat { get; set; }
        string PasswordRegexWarning { get; set; }
        string PasswordStrengthRegularExpression { get; set; }

        string PrimarySearchEngine { get; set; }
        string PrivacyPolicyUrl { get; set; }
        bool ReallyDeleteUsers { get; set; }
        string RecaptchaPrivateKey { get; set; }
        string RecaptchaPublicKey { get; set; }
        string RegistrationAgreement { get; set; }
        string RegistrationPreamble { get; set; }
        bool RequireApprovalBeforeLogin { get; set; }
        bool RequireCaptchaOnLogin { get; set; }
        bool RequireCaptchaOnRegistration { get; set; }
        bool RequireEnterEmailTwiceOnRegistration { get; set; }
        bool RequirePasswordChangeOnResetRecover { get; set; }
        bool RequiresQuestionAndAnswer { get; set; }


        //probably much of this should be moved to a new apppermissions table
        //bool RequiresUniqueEmail { get; set; }
        string RolesNotAllowedToEditModuleSettings { get; set; }
        string RolesThatCanApproveNewUsers { get; set; }
        string RolesThatCanAssignSkinsToPages { get; set; }
        string RolesThatCanCreateRootPages { get; set; }
        string RolesThatCanCreateUsers { get; set; }
        string RolesThatCanDeleteFilesInEditor { get; set; }
        string RolesThatCanEditContentTemplates { get; set; }
        //string RolesThatCanEditGoogleAnalyticsQueries { get; set; }
        string RolesThatCanLookupUsers { get; set; }
        string RolesThatCanManageSkins { get; set; }
        string RolesThatCanManageUsers { get; set; }
        //string RolesThatCanViewGoogleAnalytics { get; set; }
        string RolesThatCanViewMemberList { get; set; }
        string SiteRootDraftApprovalRoles { get; set; }
        string SiteRootDraftEditRoles { get; set; }
        string SiteRootEditRoles { get; set; }
        string DefaultRootPageCreateChildPageRoles { get; set; }
        string DefaultRootPageEditRoles { get; set; }
        string DefaultRootPageViewRoles { get; set; }
        string CommerceReportViewRoles { get; set; }



        bool ShowAlternateSearchIfConfigured { get; set; }
        //bool ShowPasswordStrengthOnRegistration { get; set; }


        bool SiteIsClosed { get; set; }
        string SiteIsClosedMessage { get; set; }
        LdapSettings SiteLdapSettings { get; set; }
        string SiteMapSkin { get; set; }

        
        string Skin { get; set; }
        Guid SkinVersion { get; set; }
        string Slogan { get; set; }
        string SMTPPassword { get; set; }
        int SMTPPort { get; set; }
        string SMTPPreferredEncoding { get; set; }
        bool SMTPRequiresAuthentication { get; set; }
        string SMTPServer { get; set; }
        string SMTPUser { get; set; }
        bool SMTPUseSsl { get; set; }
        string TimeZoneId { get; set; }
        bool UseEmailForLogin { get; set; }
        bool UseLdapAuth { get; set; }
        string UserFilesBrowseAndUploadRoles { get; set; }
        bool UseSecureRegistration { get; set; }
        bool UseSslOnAllPages { get; set; }


        string BingAPIId { get; set; }
        string CaptchaProvider { get; set; } //prob will only use recaptcha
        string CommentProvider { get; set; }

        //social login stuff
        string FacebookAppId { get; set; }
        string FacebookAppSecret { get; set; }
        string MicrosoftClientId { get; set; }
        string MicrosoftClientSecret { get; set; }
        string GoogleClientId { get; set; }
        string GoogleClientSecret { get; set; }
        string TwitterConsumerKey { get; set; }
        string TwitterConsumerSecret { get; set; }

        string WordpressApiKey { get; set; }
        string DisqusSiteShortName { get; set; }
        string AddThisDotComUsername { get; set; }

        // these probobly won't be used
        string RpxNowAdminUrl { get; set; }
        string RpxNowApiKey { get; set; }
        string RpxNowApplicationName { get; set; }
    }
}
