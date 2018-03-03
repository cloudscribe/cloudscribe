// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2018-03-02
// 

using System;

namespace cloudscribe.Core.Models
{
    //[Serializable()]
    public class SiteSettings : SiteInfo, ISiteSettings, ISiteContext
    {

        public SiteSettings()
        {
            this.Id = Guid.NewGuid();
        }


        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();


        private string theme = string.Empty;
        public string Theme
        {
            get { return theme ?? string.Empty; }
            set { theme = value; }
        }
        
        
        public bool AllowNewRegistration { get; set; } = true;
        public bool RequireConfirmedEmail { get; set; } = false;
        public bool RequireConfirmedPhone { get; set; } = false;

        public bool UseLdapAuth { get; set; } = false;
        public bool AllowDbFallbackWithLdap { get; set; } = false;
        public bool EmailLdapDbFallback { get; set; } = false;
        public bool AutoCreateLdapUserOnFirstLogin { get; set; } = true;
        
        private string ldapServer = string.Empty;
        public string LdapServer
        {
            get { return ldapServer ?? string.Empty; }
            set { ldapServer = value; }
        }

        private string ldapDomain = string.Empty;
        public string LdapDomain
        {
            get { return ldapDomain ?? string.Empty; }
            set { ldapDomain = value; }
        }

        public int LdapPort { get; set; } = 389;

        private string ldapRootDN = string.Empty;
        public string LdapRootDN
        {
            get { return ldapRootDN ?? string.Empty; }
            set { ldapRootDN = value; }
        }

        private string ldapUserDNKey = "CN";
        public string LdapUserDNKey
        {
            get { return ldapUserDNKey ?? string.Empty; }
            set { ldapUserDNKey = value; }
        }
        
        
        public bool ReallyDeleteUsers { get; set; } = true;
        public bool UseEmailForLogin { get; set; } = true;
        public bool DisableDbAuth { get; set; } = false;
       
        public bool RequiresQuestionAndAnswer { get; set; } = false;
        public bool RequireApprovalBeforeLogin { get; set; } = false;

        private string accountApprovalEmailCsv = string.Empty;
        public string AccountApprovalEmailCsv
        {
            get { return accountApprovalEmailCsv ?? string.Empty; }
            set { accountApprovalEmailCsv = value; }
        }

        
        public int MaxInvalidPasswordAttempts { get; set; } = 5;
        public int MinRequiredPasswordLength { get; set; } = 7;
        public bool PwdRequireNonAlpha { get; set; } = true;
        public bool PwdRequireLowercase { get; set; } = true;
        public bool PwdRequireUppercase { get; set; } = true;
        public bool PwdRequireDigit { get; set; } = true;



        public bool AllowPersistentLogin { get; set; } = true;
        public bool CaptchaOnRegistration { get; set; } = false;
        public bool CaptchaOnLogin { get; set; } = false;
        

        private string recaptchaPrivateKey = string.Empty;
        public string RecaptchaPrivateKey
        {
            get { return recaptchaPrivateKey ?? string.Empty; }
            set { recaptchaPrivateKey = value; }
        }

        private string recaptchaPublicKey = string.Empty;
        public string RecaptchaPublicKey
        {
            get { return recaptchaPublicKey ?? string.Empty; }
            set { recaptchaPublicKey = value; }
        }

        public bool UseInvisibleRecaptcha { get; set; } = false;

        private string facebookAppId = string.Empty;
        public string FacebookAppId
        {
            get { return facebookAppId ?? string.Empty; }
            set { facebookAppId = value; }
        }

        private string facebookAppSecret = string.Empty;
        public string FacebookAppSecret
        {
            get { return facebookAppSecret ?? string.Empty; }
            set { facebookAppSecret = value; }
        }

        private string microsoftClientId = string.Empty;
        public string MicrosoftClientId
        {
            get { return microsoftClientId ?? string.Empty; }
            set { microsoftClientId = value; }
        }

        private string microsoftClientSecret = string.Empty;
        public string MicrosoftClientSecret
        {
            get { return microsoftClientSecret ?? string.Empty; }
            set { microsoftClientSecret = value; }
        }

        private string googleClientId = string.Empty;
        public string GoogleClientId
        {
            get { return googleClientId ?? string.Empty; }
            set { googleClientId = value; }
        }

        private string googleClientSecret = string.Empty;
        public string GoogleClientSecret
        {
            get { return googleClientSecret ?? string.Empty; }
            set { googleClientSecret = value; }
        }

        private string twitterConsumerKey = string.Empty;
        public string TwitterConsumerKey
        {
            get { return twitterConsumerKey ?? string.Empty; }
            set { twitterConsumerKey = value; }
        }

        private string twitterConsumerSecret = string.Empty;
        public string TwitterConsumerSecret
        {
            get { return twitterConsumerSecret ?? string.Empty; }
            set { twitterConsumerSecret = value; }
        }

        private string oidConnectAppId = string.Empty;
        public string OidConnectAppId
        {
            get { return oidConnectAppId ?? string.Empty; }
            set { oidConnectAppId = value; }
        }

        private string oidConnectAppSecret = string.Empty; // protected with data protection
        public string OidConnectAppSecret
        {
            get { return oidConnectAppSecret ?? string.Empty; }
            set { oidConnectAppSecret = value; }
        }

        private string oidConnectAuthority = string.Empty;
        public string OidConnectAuthority
        {
            get { return oidConnectAuthority ?? string.Empty; }
            set { oidConnectAuthority = value; }
        }

        private string oidConnectDisplayName = string.Empty;
        public string OidConnectDisplayName
        {
            get { return oidConnectDisplayName ?? string.Empty; }
            set { oidConnectDisplayName = value; }
        }

        private string addThisDotComUsername = string.Empty;
        public string AddThisDotComUsername
        {
            get { return addThisDotComUsername ?? string.Empty; }
            set { addThisDotComUsername = value; }
        }

        

        private string timeZoneId = "America/New_York";
        public string TimeZoneId
        {
            get { return timeZoneId ?? "America/New_York"; }
            set { timeZoneId = value; }
        }

        private string companyName = string.Empty;
        public string CompanyName
        {
            get { return companyName ?? string.Empty; }
            set { companyName = value; }
        }

        private string companyStreetAddress = string.Empty;
        public string CompanyStreetAddress
        {
            get { return companyStreetAddress ?? string.Empty; }
            set { companyStreetAddress = value; }
        }

        private string companyStreetAddress2 = string.Empty;
        public string CompanyStreetAddress2
        {
            get { return companyStreetAddress2 ?? string.Empty; }
            set { companyStreetAddress2 = value; }
        }

        private string companyLocality = string.Empty;
        public string CompanyLocality
        {
            get { return companyLocality ?? string.Empty; }
            set { companyLocality = value; }
        }

        private string companyRegion = string.Empty;
        public string CompanyRegion
        {
            get { return companyRegion ?? string.Empty; }
            set { companyRegion = value; }
        }

        private string companyPostalCode = string.Empty;
        public string CompanyPostalCode
        {
            get { return companyPostalCode ?? string.Empty; }
            set { companyPostalCode = value; }
        }

        private string companyCountry = string.Empty;
        public string CompanyCountry
        {
            get { return companyCountry ?? string.Empty; }
            set { companyCountry = value; }
        }

        private string companyPhone = string.Empty;
        public string CompanyPhone
        {
            get { return companyPhone ?? string.Empty; }
            set { companyPhone = value; }
        }

        private string companyFax = string.Empty;
        public string CompanyFax
        {
            get { return companyFax ?? string.Empty; }
            set { companyFax = value; }
        }

        private string companyPublicEmail = string.Empty;
        public string CompanyPublicEmail
        {
            get { return companyPublicEmail ?? string.Empty; }
            set { companyPublicEmail = value; }
        }

        public string CompanyWebsite { get; set; }

        private string defaultEmailFromAddress = string.Empty;
        public string DefaultEmailFromAddress
        {
            get { return defaultEmailFromAddress ?? string.Empty; }
            set { defaultEmailFromAddress = value; }
        }

        private string defaultEmailFromAlias = string.Empty;
        public string DefaultEmailFromAlias
        {
            get { return defaultEmailFromAlias ?? string.Empty; }
            set { defaultEmailFromAlias = value; }
        }

        private string smtpUser = string.Empty;
        public string SmtpUser
        {
            get { return smtpUser ?? string.Empty; }
            set { smtpUser = value; }
        }

        private string smtpPassword = string.Empty;
        public string SmtpPassword
        {
            get { return smtpPassword ?? string.Empty; }
            set { smtpPassword = value; }
        }
        
        public int SmtpPort { get; set; } = 25;

        private string smtpPreferredEncoding = string.Empty;
        public string SmtpPreferredEncoding
        {
            get { return smtpPreferredEncoding ?? string.Empty; }
            set { smtpPreferredEncoding = value; }
        }

        private string smtpServer = string.Empty;
        public string SmtpServer
        {
            get { return smtpServer ?? string.Empty; }
            set { smtpServer = value; }
        }

        public bool SmtpRequiresAuth { get; set; } = false;
        public bool SmtpUseSsl { get; set; } = false;

        private string dkimPublicKey = string.Empty;
        public string DkimPublicKey
        {
            get { return dkimPublicKey ?? string.Empty; }
            set { dkimPublicKey = value; }
        }

        private string dkimPrivateKey = string.Empty; // protected with data protection
        public string DkimPrivateKey
        {
            get { return dkimPrivateKey ?? string.Empty; }
            set { dkimPrivateKey = value; }
        }

        private string dkimDomain = string.Empty;
        public string DkimDomain
        {
            get { return dkimDomain ?? string.Empty; }
            set { dkimDomain = value; }
        }

        private string dkimSelector = string.Empty;
        public string DkimSelector
        {
            get { return dkimSelector ?? string.Empty; }
            set { dkimSelector = value; }
        }

        public bool SignEmailWithDkim { get; set; } = false;

        public string EmailSenderName { get; set; } = "SmtpMailSender";
        public string EmailApiKey { get; set; }
        public string EmailApiEndpoint { get; set; }


        private string smsClientId = string.Empty;
        public string SmsClientId
        {
            get { return smsClientId ?? string.Empty; }
            set { smsClientId = value; }
        }

        private string smsSecureToken = string.Empty; //protected with data protection
        public string SmsSecureToken
        {
            get { return smsSecureToken ?? string.Empty; }
            set { smsSecureToken = value; }
        }

        private string smsFrom = string.Empty;
        public string SmsFrom
        {
            get { return smsFrom ?? string.Empty; }
            set { smsFrom = value; }
        }

        
        private string googleAnalyticsProfileId = string.Empty;
        public string GoogleAnalyticsProfileId
        {
            get { return googleAnalyticsProfileId ?? string.Empty; }
            set { googleAnalyticsProfileId = value; }
        }

        private string registrationAgreement = string.Empty;
        public string RegistrationAgreement
        {
            get { return registrationAgreement ?? string.Empty; }
            set { registrationAgreement = value; }
        }

        private string registrationPreamble = string.Empty;
        public string RegistrationPreamble
        {
            get { return registrationPreamble ?? string.Empty; }
            set { registrationPreamble = value; }
        }

        private string loginInfoTop = string.Empty;
        public string LoginInfoTop
        {
            get { return loginInfoTop ?? string.Empty; }
            set { loginInfoTop = value; }
        }

        private string loginInfoBottom = string.Empty;
        public string LoginInfoBottom
        {
            get { return loginInfoBottom ?? string.Empty; }
            set { loginInfoBottom = value; }
        }

        
        public bool SiteIsClosed { get; set; } = false;

        private string siteIsClosedMessage = string.Empty;
        public string SiteIsClosedMessage
        {
            get { return siteIsClosedMessage ?? string.Empty; }
            set { siteIsClosedMessage = value; }
        }

        private string privacyPolicy = string.Empty;
        public string PrivacyPolicy
        {
            get { return privacyPolicy ?? string.Empty; }
            set { privacyPolicy = value; }
        }

        public bool IsDataProtected { get; set; } = false;

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime TermsUpdatedUtc { get; set; } = DateTime.UtcNow;

        public string ForcedCulture { get; set; }

        public string ForcedUICulture { get; set; }

        //public bool UseSslOnAllPages { get; set; } = false;

        //public int PasswordAttemptWindowMinutes { get; set; } = 5;
        //public int MinReqNonAlphaChars { get; set; } = 0;

        //public bool RequireEnterEmailTwiceOnRegistration { get; set; } = false;

        //public bool AllowUserFullNameChange { get; set; } = true;

        //private string apiKeyExtra1 = string.Empty;
        //public string ApiKeyExtra1
        //{
        //    get { return apiKeyExtra1 ?? string.Empty; }
        //    set { apiKeyExtra1 = value; }
        //}

        //private string apiKeyExtra2 = string.Empty;
        //public string ApiKeyExtra2
        //{
        //    get { return apiKeyExtra2 ?? string.Empty; }
        //    set { apiKeyExtra2 = value; }
        //}

        //private string apiKeyExtra3 = string.Empty;
        //public string ApiKeyExtra3
        //{
        //    get { return apiKeyExtra3 ?? string.Empty; }
        //    set { apiKeyExtra3 = value; }
        //}

        //private string apiKeyExtra4 = string.Empty;
        //public string ApiKeyExtra4
        //{
        //    get { return apiKeyExtra4 ?? string.Empty; }
        //    set { apiKeyExtra4 = value; }
        //}

        //private string apiKeyExtra5 = string.Empty;
        //public string ApiKeyExtra5
        //{
        //    get { return apiKeyExtra5 ?? string.Empty; }
        //    set { apiKeyExtra5 = value; }
        //}


        public static SiteSettings FromISiteSettings(ISiteSettings i)
        {
            if(i == null) { return null; }

            SiteSettings s = new SiteSettings();
            s.ConcurrencyStamp = i.ConcurrencyStamp;
            s.AccountApprovalEmailCsv = i.AccountApprovalEmailCsv;
            s.AddThisDotComUsername = i.AddThisDotComUsername;
            s.AllowDbFallbackWithLdap = i.AllowDbFallbackWithLdap;
            s.AllowNewRegistration = i.AllowNewRegistration;
            s.AllowPersistentLogin = i.AllowPersistentLogin;
            s.AutoCreateLdapUserOnFirstLogin = i.AutoCreateLdapUserOnFirstLogin;
            s.CaptchaOnLogin = i.CaptchaOnLogin;
            s.CaptchaOnRegistration = i.CaptchaOnRegistration;
            s.CompanyCountry = i.CompanyCountry;
            s.CompanyFax = i.CompanyFax;
            s.CompanyLocality = i.CompanyLocality;
            s.CompanyName = i.CompanyName;
            s.CompanyPhone = i.CompanyPhone;
            s.CompanyPostalCode = i.CompanyPostalCode;
            s.CompanyPublicEmail = i.CompanyPublicEmail;
            s.CompanyWebsite = i.CompanyWebsite;
            s.CompanyRegion = i.CompanyRegion;
            s.CompanyStreetAddress = i.CompanyStreetAddress;
            s.CompanyStreetAddress2 = i.CompanyStreetAddress2;
            s.CreatedUtc = i.CreatedUtc;
            s.DefaultEmailFromAddress = i.DefaultEmailFromAddress;
            s.DefaultEmailFromAlias = i.DefaultEmailFromAlias;
            s.DisableDbAuth = i.DisableDbAuth;
            s.DkimPublicKey = i.DkimPublicKey;
            s.DkimPrivateKey = i.DkimPrivateKey;
            s.DkimDomain = i.DkimDomain;
            s.DkimSelector = i.DkimSelector;
            s.EmailLdapDbFallback = i.EmailLdapDbFallback;
            s.ForcedCulture = i.ForcedCulture;
            s.ForcedUICulture = i.ForcedUICulture;
            s.FacebookAppId = i.FacebookAppId;
            s.FacebookAppSecret = i.FacebookAppSecret;
            s.GoogleAnalyticsProfileId = i.GoogleAnalyticsProfileId;
            s.GoogleClientId = i.GoogleClientId;
            s.GoogleClientSecret = i.GoogleClientSecret;
            s.IsDataProtected = i.IsDataProtected;
            s.IsServerAdminSite = i.IsServerAdminSite;
            s.Theme = i.Theme;
            s.LdapDomain = i.LdapDomain;
            s.LdapPort = i.LdapPort;
            s.LdapRootDN = i.LdapRootDN;
            s.LdapServer = i.LdapServer;
            s.LdapUserDNKey = i.LdapUserDNKey;
            s.LoginInfoBottom = i.LoginInfoBottom;
            s.LoginInfoTop = i.LoginInfoTop;
            s.MaxInvalidPasswordAttempts = i.MaxInvalidPasswordAttempts;
            s.MicrosoftClientId = i.MicrosoftClientId;
            s.MicrosoftClientSecret = i.MicrosoftClientSecret;
            s.MinRequiredPasswordLength = i.MinRequiredPasswordLength;
            s.OidConnectAppId = i.OidConnectAppId;
            s.OidConnectAppSecret = i.OidConnectAppSecret;
            s.OidConnectAuthority = i.OidConnectAuthority;
            s.OidConnectDisplayName = i.OidConnectDisplayName;
            s.PreferredHostName = i.PreferredHostName;
            s.PrivacyPolicy = i.PrivacyPolicy;

            s.PwdRequireDigit = i.PwdRequireDigit;
            s.PwdRequireLowercase = i.PwdRequireLowercase;
            s.PwdRequireNonAlpha = i.PwdRequireNonAlpha;
            s.PwdRequireUppercase = i.PwdRequireUppercase;

            s.ReallyDeleteUsers = i.ReallyDeleteUsers;
            s.RecaptchaPrivateKey = i.RecaptchaPrivateKey;
            s.RecaptchaPublicKey = i.RecaptchaPublicKey;
            s.RegistrationAgreement = i.RegistrationAgreement;
            s.RegistrationPreamble = i.RegistrationPreamble;
            s.RequireApprovalBeforeLogin = i.RequireApprovalBeforeLogin;
            s.RequireConfirmedEmail = i.RequireConfirmedEmail;
            s.RequireConfirmedPhone = i.RequireConfirmedPhone;
            s.RequiresQuestionAndAnswer = i.RequiresQuestionAndAnswer;
            s.SignEmailWithDkim = i.SignEmailWithDkim;
            s.SiteFolderName = i.SiteFolderName;
            s.Id = i.Id;
            s.AliasId = i.AliasId;
            s.SiteIsClosed = i.SiteIsClosed;
            s.SiteIsClosedMessage = i.SiteIsClosedMessage;
            s.SiteName = i.SiteName;
            s.SmsClientId = i.SmsClientId;
            s.SmsSecureToken = i.SmsSecureToken;
            s.SmsFrom = i.SmsFrom;
            s.SmtpPassword = i.SmtpPassword;
            s.SmtpPort = i.SmtpPort;
            s.SmtpPreferredEncoding = i.SmtpPreferredEncoding;
            s.SmtpRequiresAuth = i.SmtpRequiresAuth;
            s.SmtpServer = i.SmtpServer;
            s.SmtpUser = i.SmtpUser;
            s.SmtpUseSsl = i.SmtpUseSsl;
            s.TimeZoneId = i.TimeZoneId;
            s.TwitterConsumerKey = i.TwitterConsumerKey;
            s.TwitterConsumerSecret = i.TwitterConsumerSecret;
            s.UseInvisibleRecaptcha = i.UseInvisibleRecaptcha;
            s.UseEmailForLogin = i.UseEmailForLogin;
            s.UseLdapAuth = i.UseLdapAuth;
            s.TermsUpdatedUtc = i.TermsUpdatedUtc;
            s.EmailApiEndpoint = i.EmailApiEndpoint;
            s.EmailApiKey = i.EmailApiKey;
            s.EmailSenderName = i.EmailSenderName;
           
            return s;
        }

    }
}
