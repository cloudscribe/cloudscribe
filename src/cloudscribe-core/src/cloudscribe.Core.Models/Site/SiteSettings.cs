// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2014-08-16
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

        private string defaultEmailFromAddress = string.Empty;

        public string DefaultEmailFromAddress
        {
            get { return defaultEmailFromAddress; }
            set { defaultEmailFromAddress = value; }
        }

        private string defaultFromEmailAlias = string.Empty;

        public string DefaultFromEmailAlias
        {
            get { return defaultFromEmailAlias; }
            set { defaultFromEmailAlias = value; }
        }

        private string skin = string.Empty;

        public string Skin
        {
            get { return skin; }
            set { skin = value; }
        }

        private string mobileSkin = string.Empty;

        public string MobileSkin
        {
            get { return mobileSkin; }
            set { mobileSkin = value; }
        }

        private string editorProviderName = "CKEditorProvider";// CKEditorProvider TinyMCEProvider

        public string EditorProviderName
        {
            get { return editorProviderName; }
            set { editorProviderName = value; }
        }

        //public ContentEditorSkin EditorSkin
        //{
        //    get { return editorSkin; }
        //    set { editorSkin = value; }
        //}

        private string logo = string.Empty;

        public string Logo
        {
            get { return logo; }
            set { logo = value; }
        }

        private string icon = string.Empty;

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        private bool allowUserSkins = false;

        public bool AllowUserSkins
        {
            get { return allowUserSkins; }
            set { allowUserSkins = value; }
        }

        private bool allowPageSkins = false;

        public bool AllowPageSkins
        {
            get { return allowPageSkins; }
            set { allowPageSkins = value; }
        }

        private bool allowHideMenuOnPages = false;

        public bool AllowHideMenuOnPages
        {
            get { return allowHideMenuOnPages; }
            set { allowHideMenuOnPages = value; }
        }

        private bool allowNewRegistration = true;

        public bool AllowNewRegistration
        {
            get { return allowNewRegistration; }
            set { allowNewRegistration = value; }
        }

        private bool useSecureRegistration = false;

        public bool UseSecureRegistration
        {
            get { return useSecureRegistration; }
            set { useSecureRegistration = value; }
        }

        private bool useSSLOnAllPages = false;

        public bool UseSslOnAllPages
        {
            get { return useSSLOnAllPages; }
            set { useSSLOnAllPages = value; }
        }

        // moved to siteinfo base class
        //private bool isServerAdminSite = false;

        //public bool IsServerAdminSite
        //{
        //    get { return isServerAdminSite; }
        //    set { isServerAdminSite = value; }
        //}

        private bool useLdapAuth = false;

        public bool UseLdapAuth
        {
            get { return useLdapAuth; }
            set { useLdapAuth = value; }
        }

        private bool allowDbFallbackWithLdap = false;

        public bool AllowDbFallbackWithLdap
        {
            get { return allowDbFallbackWithLdap; }
            set { allowDbFallbackWithLdap = value; }
        }

        private bool allowEmailLoginWithLdapDbFallback = false;

        public bool AllowEmailLoginWithLdapDbFallback
        {
            get { return allowEmailLoginWithLdapDbFallback; }
            set { allowEmailLoginWithLdapDbFallback = value; }
        }

        private bool autoCreateLDAPUserOnFirstLogin = true;

        public bool AutoCreateLdapUserOnFirstLogin
        {
            get { return autoCreateLDAPUserOnFirstLogin; }
            set { autoCreateLDAPUserOnFirstLogin = value; }
        }

        //[NonSerialized]
        private LdapSettings ldapSettings = new LdapSettings();

        public LdapSettings SiteLdapSettings
        {
            get { return ldapSettings; }
            set { ldapSettings = value; }
        }

        private bool allowUserFullNameChange = true;

        public bool AllowUserFullNameChange
        {
            get { return allowUserFullNameChange; }
            set { allowUserFullNameChange = value; }
        }

        private bool reallyDeleteUsers = true;

        public bool ReallyDeleteUsers
        {
            get { return reallyDeleteUsers; }
            set { reallyDeleteUsers = value; }
        }

        private bool useEmailForLogin = true;

        public bool UseEmailForLogin
        {
            get { return useEmailForLogin; }
            set { useEmailForLogin = value; }
        }

        private bool disableDbAuth = false;

        public bool DisableDbAuth
        {
            get { return disableDbAuth; }
            set { disableDbAuth = value; }
        }

        private bool allowPasswordRetrieval = true;

        public bool AllowPasswordRetrieval
        {
            get { return allowPasswordRetrieval; }
            set { allowPasswordRetrieval = value; }
        }

        private bool allowPasswordReset = false;

        public bool AllowPasswordReset
        {
            get { return allowPasswordReset; }
            set { allowPasswordReset = value; }
        }

        private bool requirePasswordChangeOnResetRecover = false;

        public bool RequirePasswordChangeOnResetRecover
        {
            get { return requirePasswordChangeOnResetRecover; }
            set { requirePasswordChangeOnResetRecover = value; }
        }

        private bool requiresQuestionAndAnswer;

        public bool RequiresQuestionAndAnswer
        {
            get { return requiresQuestionAndAnswer; }
            set { requiresQuestionAndAnswer = value; }
        }

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

        private int maxInvalidPasswordAttempts = 10;

        public int MaxInvalidPasswordAttempts
        {
            get { return maxInvalidPasswordAttempts; }
            set { maxInvalidPasswordAttempts = value; }
        }

        private int passwordAttemptWindowMinutes = 5;

        public int PasswordAttemptWindowMinutes
        {
            get { return passwordAttemptWindowMinutes; }
            set { passwordAttemptWindowMinutes = value; }
        }

        private int passwordFormat = 0;
        /// <summary>
        /// Clear = 0, Hashed = 1, Encrypted = 2, corresponding to MembershipPasswordFormat Enum
        /// </summary>
        public int PasswordFormat
        {
            get { return passwordFormat; }
            set { passwordFormat = value; }
        }

        private int minRequiredPasswordLength = 7;

        public int MinRequiredPasswordLength
        {
            get { return minRequiredPasswordLength; }
            set { minRequiredPasswordLength = value; }
        }

        private int minRequiredNonAlphanumericCharacters = 0;

        public int MinRequiredNonAlphanumericCharacters
        {
            get { return minRequiredNonAlphanumericCharacters; }
            set { minRequiredNonAlphanumericCharacters = value; }
        }

        private string passwordStrengthRegularExpression = string.Empty;

        public string PasswordStrengthRegularExpression
        {
            get { return passwordStrengthRegularExpression; }
            set { passwordStrengthRegularExpression = value; }
        }

        private bool allowPersistentLogin = true;

        public bool AllowPersistentLogin
        {
            get { return allowPersistentLogin; }
            set { allowPersistentLogin = value; }
        }

        private bool requireCaptchaOnRegistration = false;

        public bool RequireCaptchaOnRegistration
        {
            get { return requireCaptchaOnRegistration; }
            set { requireCaptchaOnRegistration = value; }
        }

        private bool requireCaptchaOnLogin = false;

        public bool RequireCaptchaOnLogin
        {
            get { return requireCaptchaOnLogin; }
            set { requireCaptchaOnLogin = value; }
        }

        private bool requireEnterEmailTwiceOnRegistration = false;

        public bool RequireEnterEmailTwiceOnRegistration
        {
            get { return requireEnterEmailTwiceOnRegistration; }
            set { requireEnterEmailTwiceOnRegistration = value; }
        }

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

        private string captchaProvider = "SubkismetCaptchaProvider";

        public string CaptchaProvider
        {
            get { return captchaProvider; }
            set { captchaProvider = value; }
        }

        private string recaptchaPrivateKey = string.Empty;

        public string RecaptchaPrivateKey
        {
            get { return recaptchaPrivateKey; }
            set { recaptchaPrivateKey = value; }
        }

        private string recaptchaPublicKey = string.Empty;

        public string RecaptchaPublicKey
        {
            get { return recaptchaPublicKey; }
            set { recaptchaPublicKey = value; }
        }

        private string wordpressAPIKey = string.Empty;

        public string WordpressApiKey
        {
            get { return wordpressAPIKey; }
            set { wordpressAPIKey = value; }
        }

        private string windowsLiveAppID = string.Empty;

        public string WindowsLiveAppId
        {
            get { return windowsLiveAppID; }
            set { windowsLiveAppID = value; }
        }

        private string windowsLiveKey = string.Empty;

        public string WindowsLiveKey
        {
            get { return windowsLiveKey; }
            set { windowsLiveKey = value; }
        }

        private bool allowOpenIDAuth = false;

        public bool AllowOpenIdAuth
        {
            get { return allowOpenIDAuth; }
            set { allowOpenIDAuth = value; }
        }

        private bool allowWindowsLiveAuth = false;

        public bool AllowWindowsLiveAuth
        {
            get { return allowWindowsLiveAuth; }
            set { allowWindowsLiveAuth = value; }
        }

        private string gmapApiKey = string.Empty;

        public string GmapApiKey
        {
            get { return gmapApiKey; }
            set { gmapApiKey = value; }
        }

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

        private string timeZoneId = "Eastern Standard Time";

        public string TimeZoneId
        {
            get { return timeZoneId; }
            set { timeZoneId = value; }
        }


        private string siteMapSkin = string.Empty;

        public string SiteMapSkin
        {
            get { return siteMapSkin; }
            set { siteMapSkin = value; }
        }


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



        private string rpxNowApiKey = string.Empty;

        public string RpxNowApiKey
        {
            get { return rpxNowApiKey; }
            set { rpxNowApiKey = value; }
        }

        private string rpxNowApplicationName = string.Empty;

        public string RpxNowApplicationName
        {
            get { return rpxNowApplicationName; }
            set { rpxNowApplicationName = value; }
        }

        private string rpxNowAdminUrl = string.Empty;

        public string RpxNowAdminUrl
        {
            get { return rpxNowAdminUrl; }
            set { rpxNowAdminUrl = value; }
        }


        private string openSearchName = string.Empty;

        public string OpenSearchName
        {
            get { return openSearchName; }
            set { openSearchName = value; }
        }

        private string bingAPIId = string.Empty;

        public string BingAPIId
        {
            get { return bingAPIId; }
            set { bingAPIId = value; }
        }

        private string googleCustomSearchId = string.Empty;

        public string GoogleCustomSearchId
        {
            get { return googleCustomSearchId; }
            set { googleCustomSearchId = value; }
        }

        private string primarySearchEngine = "internal";

        public string PrimarySearchEngine
        {
            get { return primarySearchEngine; }
            set { primarySearchEngine = value; }
        }

        private bool showAlternateSearchIfConfigured = false;

        public bool ShowAlternateSearchIfConfigured
        {
            get { return showAlternateSearchIfConfigured; }
            set { showAlternateSearchIfConfigured = value; }
        }

        private string slogan = string.Empty;

        public string Slogan
        {
            get { return slogan; }
            set { slogan = value; }
        }

        private string companyName = string.Empty;

        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        private string companyStreetAddress = string.Empty;

        public string CompanyStreetAddress
        {
            get { return companyStreetAddress; }
            set { companyStreetAddress = value; }
        }

        private string companyStreetAddress2 = string.Empty;

        public string CompanyStreetAddress2
        {
            get { return companyStreetAddress2; }
            set { companyStreetAddress2 = value; }
        }

        private string companyLocality = string.Empty;

        public string CompanyLocality
        {
            get { return companyLocality; }
            set { companyLocality = value; }
        }

        private string companyRegion = string.Empty;

        public string CompanyRegion
        {
            get { return companyRegion; }
            set { companyRegion = value; }
        }

        private string companyPostalCode = string.Empty;

        public string CompanyPostalCode
        {
            get { return companyPostalCode; }
            set { companyPostalCode = value; }
        }

        private string companyCountry = string.Empty;

        public string CompanyCountry
        {
            get { return companyCountry; }
            set { companyCountry = value; }
        }

        private string companyPhone = string.Empty;

        public string CompanyPhone
        {
            get { return companyPhone; }
            set { companyPhone = value; }
        }

        private string companyFax = string.Empty;

        public string CompanyFax
        {
            get { return companyFax; }
            set { companyFax = value; }
        }

        private string companyPublicEmail = string.Empty;

        public string CompanyPublicEmail
        {
            get { return companyPublicEmail; }
            set { companyPublicEmail = value; }
        }



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

        private string privacyPolicyUrl = "/privacy.aspx";

        public string PrivacyPolicyUrl
        {
            get { return privacyPolicyUrl; }
            set { privacyPolicyUrl = value; }
        }

        private string sMTPUser = string.Empty;

        public string SMTPUser
        {
            get { return sMTPUser; }
            set { sMTPUser = value; }
        }

        private string sMTPPassword = string.Empty;

        public string SMTPPassword
        {
            get { return sMTPPassword; }
            set { sMTPPassword = value; }
        }

        private int sMTPPort = 25;

        public int SMTPPort
        {
            get { return sMTPPort; }
            set { sMTPPort = value; }
        }

        private string sMTPPreferredEncoding = string.Empty;

        public string SMTPPreferredEncoding
        {
            get { return sMTPPreferredEncoding; }
            set { sMTPPreferredEncoding = value; }
        }

        private string sMTPServer = string.Empty;

        public string SMTPServer
        {
            get { return sMTPServer; }
            set { sMTPServer = value; }
        }


        private bool sMTPRequiresAuthentication = false;

        public bool SMTPRequiresAuthentication
        {
            get { return sMTPRequiresAuthentication; }
            set { sMTPRequiresAuthentication = value; }
        }

        private bool sMTPUseSsl = false;

        public bool SMTPUseSsl
        {
            get { return sMTPUseSsl; }
            set { sMTPUseSsl = value; }
        }

        private bool allowUserEditorPreference = false;

        public bool AllowUserEditorPreference
        {
            get { return allowUserEditorPreference; }
            set { allowUserEditorPreference = value; }
        }

        private string siteRootEditRoles = string.Empty;

        public string SiteRootEditRoles
        {
            get { return siteRootEditRoles; }
            set { siteRootEditRoles = value; }
        }

        private string siteRootDraftEditRoles = string.Empty;

        public string SiteRootDraftEditRoles
        {
            get { return siteRootDraftEditRoles; }
            set { siteRootDraftEditRoles = value; }
        }

        // added 2013-04-24 for 3 level workflow
        private string siteRootDraftApprovalRoles = string.Empty;
        public string SiteRootDraftApprovalRoles
        {
            get { return siteRootDraftApprovalRoles; }
            set { siteRootDraftApprovalRoles = value; }
        }

        private string commerceReportViewRoles = string.Empty;

        public string CommerceReportViewRoles
        {
            get { return commerceReportViewRoles; }
            set { commerceReportViewRoles = value; }
        }

        private string rolesThatCanCreateRootPages = "Content Administrators;";

        public string RolesThatCanCreateRootPages
        {
            get { return rolesThatCanCreateRootPages; }
            set { rolesThatCanCreateRootPages = value; }
        }

        private string rolesThatCanViewMemberList = "Authenticated Users;";

        public string RolesThatCanViewMemberList
        {
            get { return rolesThatCanViewMemberList; }
            set { rolesThatCanViewMemberList = value; }
        }

        private string rolesThatCanCreateUsers = string.Empty;//RolesThatCanManageUsers

        public string RolesThatCanCreateUsers
        {
            get { return rolesThatCanCreateUsers; }
            set { rolesThatCanCreateUsers = value; }
        }

        private string rolesThatCanManageUsers = string.Empty;//RolesThatCanFullyManageUsers

        public string RolesThatCanManageUsers
        {
            get { return rolesThatCanManageUsers; }
            set { rolesThatCanManageUsers = value; }
        }


        private string rolesThatCanLookupUsers = string.Empty;

        public string RolesThatCanLookupUsers
        {
            get { return rolesThatCanLookupUsers; }
            set { rolesThatCanLookupUsers = value; }
        }

        private string rolesNotAllowedToEditModuleSettings = string.Empty;

        public string RolesNotAllowedToEditModuleSettings
        {
            get { return rolesNotAllowedToEditModuleSettings; }
            set { rolesNotAllowedToEditModuleSettings = value; }
        }

        private string rolesThatCanEditContentTemplates = string.Empty;

        public string RolesThatCanEditContentTemplates
        {
            get { return rolesThatCanEditContentTemplates; }
            set { rolesThatCanEditContentTemplates = value; }
        }

        private string generalBrowseAndUploadRoles = string.Empty;

        public string GeneralBrowseAndUploadRoles
        {
            get { return generalBrowseAndUploadRoles; }
            set { generalBrowseAndUploadRoles = value; }
        }

        private string userFilesBrowseAndUploadRoles = string.Empty;

        public string UserFilesBrowseAndUploadRoles
        {
            get { return userFilesBrowseAndUploadRoles; }
            set { userFilesBrowseAndUploadRoles = value; }
        }

        private string rolesThatCanDeleteFilesInEditor = string.Empty;

        public string RolesThatCanDeleteFilesInEditor
        {
            get { return rolesThatCanDeleteFilesInEditor; }
            set { rolesThatCanDeleteFilesInEditor = value; }
        }

        private string rolesThatCanAssignSkinsToPages = string.Empty;

        public string RolesThatCanAssignSkinsToPages
        {
            get { return rolesThatCanAssignSkinsToPages; }
            set { rolesThatCanAssignSkinsToPages = value; }
        }

        private string rolesThatCanManageSkins = string.Empty;

        public string RolesThatCanManageSkins
        {
            get { return rolesThatCanManageSkins; }
            set { rolesThatCanManageSkins = value; }
        }

        private string defaultRootPageViewRoles = "All Users;";

        public string DefaultRootPageViewRoles
        {
            get { return defaultRootPageViewRoles; }
            set { defaultRootPageViewRoles = value; }
        }

        private string defaultRootPageEditRoles = string.Empty;

        public string DefaultRootPageEditRoles
        {
            get { return defaultRootPageEditRoles; }
            set { defaultRootPageEditRoles = value; }
        }

        private string defaultRootPageCreateChildPageRoles = string.Empty;

        public string DefaultRootPageCreateChildPageRoles
        {
            get { return defaultRootPageCreateChildPageRoles; }
            set { defaultRootPageCreateChildPageRoles = value; }
        }

        private Guid skinVersion = Guid.Empty;

        public Guid SkinVersion
        {
            get { return skinVersion; }
            set { skinVersion = value; }
        }



        private string avatarSystem = "gravatar";

        public string AvatarSystem
        {
            get { return avatarSystem; }
            set { avatarSystem = value; }
        }

        private string commentProvider = "intensedebate";

        public string CommentProvider
        {
            get { return commentProvider; }
            set { commentProvider = value; }
        }

        private string facebookAppId = string.Empty;

        public string FacebookAppId
        {
            get { return facebookAppId; }
            set { facebookAppId = value; }
        }

        private string intenseDebateAccountId = string.Empty;

        public string IntenseDebateAccountId
        {
            get { return intenseDebateAccountId; }
            set { intenseDebateAccountId = value; }
        }

        private string disqusSiteShortName = string.Empty;

        public string DisqusSiteShortName
        {
            get { return disqusSiteShortName; }
            set { disqusSiteShortName = value; ; }
        }


        private string metaProfile = string.Empty;
        /// <summary>
        /// if you are using vocabularies such as Dublin Core in your page meta data, you can specify the profile which will be added to the head element
        /// http://dublincore.org/documents/dcq-html/
        /// ie for Dublin Core you would put http://dublincore.org/documents/dcq-html/ 
        /// if using multiple vocabularies you can separe the urls by white space
        /// </summary>
        public string MetaProfile
        {
            get { return metaProfile; }
            set { metaProfile = value; }
        }

        private string newsletterEditor = "TinyMCEProvider";

        public string NewsletterEditor
        {
            get { return newsletterEditor; }
            set { newsletterEditor = value; }
        }

        private Guid defaultCountryGuid = new Guid("a71d6727-61e7-4282-9fcb-526d1e7bc24f"); //US

        public Guid DefaultCountryGuid
        {
            get { return defaultCountryGuid; }
            set { defaultCountryGuid = value; }
        }

        private Guid defaultStateGuid = Guid.Empty;

        public Guid DefaultStateGuid
        {
            get { return defaultStateGuid; }
            set { defaultStateGuid = value; }
        }

        private Guid currencyGuid = new Guid("ff2dde1b-e7d7-4c3a-9ab4-6474345e0f31"); //USD

        public Guid CurrencyGuid
        {
            get { return currencyGuid; }
            set { currencyGuid = value; }
        }


        //public Currency GetCurrency()
        //{
        //    if (currency == null) { currency = new Currency(CurrencyGuid); }
        //    return currency;
        //}

        private bool forceContentVersioning = false;

        public bool ForceContentVersioning
        {
            get { return forceContentVersioning; }
            set { forceContentVersioning = value; }
        }

        private bool enableContentWorkflow = false;

        public bool EnableContentWorkflow
        {
            get { return enableContentWorkflow; }
            set { enableContentWorkflow = value; }
        }


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

        private string googleAnalyticsProfileId = string.Empty;

        public string GoogleAnalyticsProfileId
        {
            get { return googleAnalyticsProfileId; }
            set { googleAnalyticsProfileId = value; }
        }

        private string googleAnalyticsSettings = string.Empty;

        public string GoogleAnalyticsSettings
        {
            get { return googleAnalyticsSettings; }
            set { googleAnalyticsSettings = value; }
        }

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

        private bool requireApprovalBeforeLogin = false;

        public bool RequireApprovalBeforeLogin
        {
            get { return requireApprovalBeforeLogin; }
            set { requireApprovalBeforeLogin = value; }
        }



        private string emailAdressesForUserApprovalNotification = string.Empty;

        public string EmailAdressesForUserApprovalNotification
        {
            get { return emailAdressesForUserApprovalNotification; }
            set { emailAdressesForUserApprovalNotification = value; }
        }


        private string rolesThatCanApproveNewUsers = string.Empty;

        public string RolesThatCanApproveNewUsers
        {
            get { return rolesThatCanApproveNewUsers; }
            set { rolesThatCanApproveNewUsers = value; }
        }

        private string passwordRegexWarning = string.Empty;

        public string PasswordRegexWarning
        {
            get { return passwordRegexWarning; }
            set { passwordRegexWarning = value; }
        }

        private string registrationAgreement = string.Empty;

        public string RegistrationAgreement
        {
            get { return registrationAgreement; }
            set { registrationAgreement = value; }
        }

        private string registrationPreamble = string.Empty;

        public string RegistrationPreamble
        {
            get { return registrationPreamble; }
            set { registrationPreamble = value; }
        }

        private string loginInfoTop = string.Empty;

        public string LoginInfoTop
        {
            get { return loginInfoTop; }
            set { loginInfoTop = value; }
        }


        private string loginInfoBottom = string.Empty;

        public string LoginInfoBottom
        {
            get { return loginInfoBottom; }
            set { loginInfoBottom = value; }
        }

        private bool siteIsClosed = false;

        public bool SiteIsClosed
        {
            get { return siteIsClosed; }
            set { siteIsClosed = value; }
        }

        private string siteIsClosedMessage = string.Empty;

        public string SiteIsClosedMessage
        {
            get { return siteIsClosedMessage; }
            set { siteIsClosedMessage = value; }
        }

        


    }
}
