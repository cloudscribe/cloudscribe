// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2015-08-04
// 

using System;

namespace cloudscribe.Core.Models
{
    //[Serializable()]
    public class SiteSettings : SiteInfo, ISiteSettings
    {

        public SiteSettings()
        {

        }

        //// for future use
        //private string apiKeyExtra4 = string.Empty;

        // maps To PreferredHostName as of 2008-05-22
        private string apiKeyExtra5 = string.Empty;

        // moved to SiteInfo base class
        ///// <summary>
        ///// In case multiple host names map to your site and you want to force a particular one.
        ///// For example I want to force urls with hostname mojoportal.com to www.mojoportal.com,
        ///// because my SSL certificate matches www.mojoportal.com but not mojoportal.com
        ///// </summary>
        //public string PreferredHostName
        //{
        //    get { return apiKeyExtra5; }
        //    set { apiKeyExtra5 = value; }
        //}

        
        public string DefaultEmailFromAddress { get; set; } = string.Empty;
        public string DefaultFromEmailAlias { get; set; } = string.Empty;
        public string Skin { get; set; } = string.Empty;
        public string MobileSkin { get; set; } = string.Empty;
        public string EditorProviderName { get; set; } = "CKEditorProvider";


        //public ContentEditorSkin EditorSkin
        //{
        //    get { return editorSkin; }
        //    set { editorSkin = value; }
        //}

       
        public string Logo { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool AllowUserSkins { get; set; } = false;
        public bool AllowPageSkins { get; set; } = false;
        public bool AllowHideMenuOnPages { get; set; } = false;
        public bool AllowNewRegistration { get; set; } = true;
        public bool UseSecureRegistration { get; set; } = false;
        public bool UseSslOnAllPages { get; set; } = false;
        

        // moved to siteinfo base class
        //private bool isServerAdminSite = false;

        //public bool IsServerAdminSite
        //{
        //    get { return isServerAdminSite; }
        //    set { isServerAdminSite = value; }
        //}

        
        public bool UseLdapAuth { get; set; } = false;
        public bool AllowDbFallbackWithLdap { get; set; } = false;
        public bool AllowEmailLoginWithLdapDbFallback { get; set; } = false;
        public bool AutoCreateLdapUserOnFirstLogin { get; set; } = true;
        

        //[NonSerialized]
        private LdapSettings ldapSettings = new LdapSettings();

        public LdapSettings SiteLdapSettings
        {
            get { return ldapSettings; }
            set { ldapSettings = value; }
        }

        
        public bool AllowUserFullNameChange { get; set; } = true;
        public bool ReallyDeleteUsers { get; set; } = true;
        public bool UseEmailForLogin { get; set; } = true;
        public bool DisableDbAuth { get; set; } = false;
        public bool AllowPasswordRetrieval { get; set; } = true;
        public bool AllowPasswordReset { get; set; } = false;
        public bool RequirePasswordChangeOnResetRecover { get; set; } = false;
        public bool RequiresQuestionAndAnswer { get; set; } = false;
        

        //private bool requiresUniqueEmail;

        //public bool RequiresUniqueEmail
        //{
        //    // I'm not exposing this in the UI because it really needs to
        //    // always be true with the current design if email is used for login
        //    // we could expose this in scenario is loginname for login
        //    // but if someone starts that way and changes it things could get inconsistent
        //    get { return requiresUniqueEmail; }
        //    set { requiresUniqueEmail = value; }
        //}

        
        public int MaxInvalidPasswordAttempts { get; set; } = 10;
        public int PasswordAttemptWindowMinutes { get; set; } = 5;
        

        /// <summary>
        /// legacy field from mojoportal
        /// 
        /// Clear = 0, Hashed = 1, Encrypted = 2, corresponding to MembershipPasswordFormat Enum
        /// </summary>
        public int PasswordFormat { get; set; } = 0;

        public int MinRequiredPasswordLength { get; set; } = 7;    
        public int MinRequiredNonAlphanumericCharacters { get; set; } = 0;
        public string PasswordStrengthRegularExpression { get; set; } = string.Empty;
        public bool AllowPersistentLogin { get; set; } = true;
        public bool RequireCaptchaOnRegistration { get; set; } = false;
        public bool RequireCaptchaOnLogin { get; set; } = false;
        public bool RequireEnterEmailTwiceOnRegistration { get; set; } = false;
         

        // moved to siteinfo base class
        //private string siteFolderName = string.Empty;

        //public string SiteFolderName
        //{
        //    get { return siteFolderName; }
        //    set { siteFolderName = value; }
        //}

        //public string DatePickerProvider
        //{
        //    get { return datePickerProvider; }
        //    set { datePickerProvider = value; }
        //}

        
        public string CaptchaProvider { get; set; } = "recaptcha";
        public string RecaptchaPrivateKey { get; set; } = string.Empty;
        public string RecaptchaPublicKey { get; set; } = string.Empty;
        
        public string WordpressApiKey { get; set; } = string.Empty;


        public string WindowsLiveAppId { get; set; } = string.Empty;
        public string WindowsLiveKey { get; set; } = string.Empty;
        
        public bool AllowOpenIdAuth { get; set; } = false;
        
        public bool AllowWindowsLiveAuth { get; set; } = false;
        
        public string GmapApiKey { get; set; } = string.Empty;


        // AddThisDotComUsername maps to apiKeyExtra1
        private string apiKeyExtra1 = string.Empty;

        public string AddThisDotComUsername
        {
            get { return apiKeyExtra1; }
            set { apiKeyExtra1 = value; }
        }

        //GoogleAnalyticsAccountCode
        private string apiKeyExtra2 = string.Empty;

        public string GoogleAnalyticsAccountCode
        {
            get { return apiKeyExtra2; }
            set { apiKeyExtra2 = value; }
        }


        public string TimeZoneId { get; set; } = "Eastern Standard Time";
        
        public string SiteMapSkin { get; set; } = string.Empty;



        //public string AppLogoForWindowsLive
        //{
        //    get
        //    {
        //        string result = GetExpandoProperty("AppLogoForWindowsLive");
        //        if (result != null) { return result; }
        //        return "/Data/logos/mojomoonprint.jpg";
        //    }
        //    set { SetExpandoProperty("AppLogoForWindowsLive", value); }
        //}

            

        public string RpxNowApiKey { get; set; } = string.Empty;
        public string RpxNowApplicationName { get; set; } = string.Empty;
        public string RpxNowAdminUrl { get; set; } = string.Empty;
        
        public string OpenSearchName { get; set; } = string.Empty;
        
        public string BingAPIId { get; set; } = string.Empty;
        public string GoogleCustomSearchId { get; set; } = string.Empty;


        private string primarySearchEngine = "internal";

        public string PrimarySearchEngine
        {
            get { return primarySearchEngine; }
            set { primarySearchEngine = value; }
        }

        
        public bool ShowAlternateSearchIfConfigured { get; set; } = false;
        
        public string Slogan { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyStreetAddress { get; set; } = string.Empty;
        public string CompanyStreetAddress2 { get; set; } = string.Empty;
        public string CompanyLocality { get; set; } = string.Empty;
        public string CompanyRegion { get; set; } = string.Empty;
        public string CompanyPostalCode { get; set; } = string.Empty; 
        public string CompanyCountry { get; set; } = string.Empty;
        public string CompanyPhone { get; set; } = string.Empty;
        public string CompanyFax { get; set; } = string.Empty;
        public string CompanyPublicEmail { get; set; } = string.Empty;




        //public bool EnableWoopra
        //{
        //    get
        //    {
        //        string sBool = GetExpandoProperty("EnableWoopra");

        //        if ((sBool != null) && (sBool.Length > 0))
        //        {
        //            return Convert.ToBoolean(sBool);
        //        }

        //        return false;
        //    }
        //    set { SetExpandoProperty("EnableWoopra", value.ToString()); }
        //}

        

        public string PrivacyPolicyUrl { get; set; } = string.Empty;
        public string SMTPUser { get; set; } = string.Empty;
        public string SMTPPassword { get; set; } = string.Empty;
        public int SMTPPort { get; set; } = 25;  
        public string SMTPPreferredEncoding { get; set; } = string.Empty;
        public string SMTPServer { get; set; } = string.Empty;
        public bool SMTPRequiresAuthentication { get; set; } = false;
        public bool SMTPUseSsl { get; set; } = false;
        
        public bool AllowUserEditorPreference { get; set; } = false;
        public string SiteRootEditRoles { get; set; } = string.Empty;
        public string SiteRootDraftEditRoles { get; set; } = string.Empty;
        public string SiteRootDraftApprovalRoles { get; set; } = string.Empty;
        public string CommerceReportViewRoles { get; set; } = string.Empty;
        public string RolesThatCanCreateRootPages { get; set; } = "Content Administrators;";
        public string RolesThatCanViewMemberList { get; set; } = "Authenticated Users;";
        public string RolesThatCanCreateUsers { get; set; } = string.Empty; //RolesThatCanManageUsers
        public string RolesThatCanManageUsers { get; set; } = string.Empty; //RolesThatCanFullyManageUsers
        public string RolesThatCanLookupUsers { get; set; } = string.Empty;
        public string RolesNotAllowedToEditModuleSettings { get; set; } = string.Empty; 
        public string RolesThatCanEditContentTemplates { get; set; } = string.Empty; 
        public string GeneralBrowseAndUploadRoles { get; set; } = string.Empty;
        public string UserFilesBrowseAndUploadRoles { get; set; } = string.Empty;
        public string RolesThatCanDeleteFilesInEditor { get; set; } = string.Empty;
        public string RolesThatCanAssignSkinsToPages { get; set; } = string.Empty;
        public string RolesThatCanManageSkins { get; set; } = string.Empty;
        public string DefaultRootPageViewRoles { get; set; } = "All Users;";
        public string DefaultRootPageEditRoles { get; set; } = string.Empty;
        public string DefaultRootPageCreateChildPageRoles { get; set; } = string.Empty;
        public Guid SkinVersion { get; set; } = Guid.Empty;
        public string AvatarSystem { get; set; } = "gravatar";
        public string CommentProvider { get; set; } = "intensedebate";
        public string FacebookAppId { get; set; } = string.Empty;
        public string IntenseDebateAccountId { get; set; } = string.Empty;
        public string DisqusSiteShortName { get; set; } = string.Empty;



        
        /// <summary>
        /// if you are using vocabularies such as Dublin Core in your page meta data, you can specify the profile which will be added to the head element
        /// http://dublincore.org/documents/dcq-html/
        /// ie for Dublin Core you would put http://dublincore.org/documents/dcq-html/ 
        /// if using multiple vocabularies you can separe the urls by white space
        /// </summary>
        public string MetaProfile { get; set; } = string.Empty;
        public string NewsletterEditor { get; set; } = "TinyMCEProvider";
        public Guid DefaultCountryGuid { get; set; } = new Guid("a71d6727-61e7-4282-9fcb-526d1e7bc24f"); //US
        public Guid DefaultStateGuid { get; set; } = Guid.Empty;
        public Guid CurrencyGuid { get; set; } = new Guid("ff2dde1b-e7d7-4c3a-9ab4-6474345e0f31"); //USD
        
        public bool ForceContentVersioning { get; set; } = false;
        public bool EnableContentWorkflow { get; set; } = false;



        //public string GoogleAnalyticsEmail
        //{
        //    get
        //    {
        //        string result = GetExpandoProperty("GoogleAnalyticsEmail");
        //        if (result != null) { return result; }
        //        return string.Empty;
        //    }
        //    set { SetExpandoProperty("GoogleAnalyticsEmail", value); }
        //}


        //public string GoogleAnalyticsPassword
        //{
        //    get
        //    {
        //        string result = GetExpandoProperty("GoogleAnalyticsPassword");
        //        if (result != null) { return result; }
        //        return string.Empty;
        //    }
        //    set { SetExpandoProperty("GoogleAnalyticsPassword", value); }
        //}


        public string GoogleAnalyticsProfileId { get; set; } = string.Empty;
        public string GoogleAnalyticsSettings { get; set; } = string.Empty;


        //public string RolesThatCanViewGoogleAnalytics
        //{
        //    get
        //    {
        //        string result = GetExpandoProperty("RolesThatCanViewGoogleAnalytics");
        //        if (result != null) { return result; }
        //        return "Admins;Content Administrators;";
        //    }
        //    set { SetExpandoProperty("RolesThatCanViewGoogleAnalytics", value); }
        //}

        //public string RolesThatCanEditGoogleAnalyticsQueries
        //{
        //    get
        //    {
        //        string result = GetExpandoProperty("RolesThatCanEditGoogleAnalyticsQueries");
        //        if (result != null) { return result; }
        //        return "Admins;Content Administrators;";
        //    }
        //    set { SetExpandoProperty("RolesThatCanEditGoogleAnalyticsQueries", value); }
        //}

        
        public bool RequireApprovalBeforeLogin { get; set; } = false;
        public string EmailAdressesForUserApprovalNotification { get; set; } = string.Empty;
        public string RolesThatCanApproveNewUsers { get; set; } = string.Empty;
        public string PasswordRegexWarning { get; set; } = string.Empty;
        public string RegistrationAgreement { get; set; } = string.Empty; 
        public string RegistrationPreamble { get; set; } = string.Empty;
        public string LoginInfoTop { get; set; } = string.Empty;
        public string LoginInfoBottom { get; set; } = string.Empty;
        public bool SiteIsClosed { get; set; } = false;
        public string SiteIsClosedMessage { get; set; } = string.Empty;
        

    }
}
