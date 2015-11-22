// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2015-11-22
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
        
        public string DefaultEmailFromAddress { get; set; } = string.Empty;
        //public string DefaultFromEmailAlias { get; set; } = string.Empty;
        public string Layout { get; set; } = string.Empty;
        
        public bool AllowNewRegistration { get; set; } = true;
        public bool UseSecureRegistration { get; set; } = false;
        public bool UseSslOnAllPages { get; set; } = false;
        
        public bool UseLdapAuth { get; set; } = false;
        public bool AllowDbFallbackWithLdap { get; set; } = false;
        public bool EmailLdapDbFallback { get; set; } = false;
        public bool AutoCreateLdapUserOnFirstLogin { get; set; } = true;

        public string LdapServer { get; set; } = string.Empty;
        public string LdapDomain { get; set; } = string.Empty;
        public int LdapPort { get; set; } = 389;
        public string LdapRootDN { get; set; } = string.Empty;
        public string LdapUserDNKey { get; set; } = "CN";
        
        public bool AllowUserFullNameChange { get; set; } = true;
        public bool ReallyDeleteUsers { get; set; } = true;
        public bool UseEmailForLogin { get; set; } = true;
        public bool DisableDbAuth { get; set; } = false;
       
        public bool RequiresQuestionAndAnswer { get; set; } = false;
        public bool RequireApprovalBeforeLogin { get; set; } = false;
        
        public int MaxInvalidPasswordAttempts { get; set; } = 10;
        public int PasswordAttemptWindowMinutes { get; set; } = 5;
        
        public int MinRequiredPasswordLength { get; set; } = 7;    
        public int MinReqNonAlphaChars { get; set; } = 0;
        
        public bool AllowPersistentLogin { get; set; } = true;
        public bool CaptchaOnRegistration { get; set; } = false;
        public bool CaptchaOnLogin { get; set; } = false;
        //public bool RequireEnterEmailTwiceOnRegistration { get; set; } = false;
         
        public string RecaptchaPrivateKey { get; set; } = string.Empty;
        public string RecaptchaPublicKey { get; set; } = string.Empty;
        
        public string FacebookAppId { get; set; } = string.Empty;
        public string FacebookAppSecret { get; set; } = string.Empty;

        public string MicrosoftClientId { get; set; } = string.Empty;
        public string MicrosoftClientSecret { get; set; } = string.Empty;

        public string GoogleClientId { get; set; } = string.Empty;
        public string GoogleClientSecret { get; set; } = string.Empty;

        public string TwitterConsumerKey { get; set; } = string.Empty;
        public string TwitterConsumerSecret { get; set; } = string.Empty;
        
        public string AddThisDotComUsername { get; set; } = string.Empty;

        public string ApiKeyExtra1 { get; set; } = string.Empty;
        public string ApiKeyExtra2 { get; set; } = string.Empty;
        public string ApiKeyExtra3 { get; set; } = string.Empty;
        public string ApiKeyExtra4 { get; set; } = string.Empty;
        public string ApiKeyExtra5 { get; set; } = string.Empty;
        
        public string TimeZoneId { get; set; } = "Eastern Standard Time";
        
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
        
        public string SmtpUser { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 25;  
        public string SmtpPreferredEncoding { get; set; } = string.Empty;
        public string SmtpServer { get; set; } = string.Empty;
        public bool SmtpRequiresAuth { get; set; } = false;
        public bool SmtpUseSsl { get; set; } = false;
        
        public string GoogleAnalyticsProfileId { get; set; } = string.Empty;
        
        public string RegistrationAgreement { get; set; } = string.Empty; 
        public string RegistrationPreamble { get; set; } = string.Empty;
        public string LoginInfoTop { get; set; } = string.Empty;
        public string LoginInfoBottom { get; set; } = string.Empty;
        public bool SiteIsClosed { get; set; } = false;
        public string SiteIsClosedMessage { get; set; } = string.Empty;
        public string PrivacyPolicy { get; set; } = string.Empty;

        public static SiteSettings FromISiteSettings(ISiteSettings i)
        {
            if(i == null) { return null; }

            SiteSettings s = new SiteSettings();
            s.AddThisDotComUsername = i.AddThisDotComUsername;
            s.AllowDbFallbackWithLdap = i.AllowDbFallbackWithLdap;
            s.AllowNewRegistration = i.AllowNewRegistration;
            s.AllowPersistentLogin = i.AllowPersistentLogin;
            s.AllowUserFullNameChange = i.AllowUserFullNameChange;
            s.ApiKeyExtra1 = i.ApiKeyExtra1;
            s.ApiKeyExtra2 = i.ApiKeyExtra2;
            s.ApiKeyExtra3 = i.ApiKeyExtra3;
            s.ApiKeyExtra4 = i.ApiKeyExtra4;
            s.ApiKeyExtra5 = i.ApiKeyExtra5;
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
            s.CompanyRegion = i.CompanyRegion;
            s.CompanyStreetAddress = i.CompanyStreetAddress;
            s.CompanyStreetAddress2 = i.CompanyStreetAddress2;
            s.DefaultEmailFromAddress = i.DefaultEmailFromAddress;
            s.DisableDbAuth = i.DisableDbAuth;
            s.EmailLdapDbFallback = i.EmailLdapDbFallback;
            s.FacebookAppId = i.FacebookAppId;
            s.FacebookAppSecret = i.FacebookAppSecret;
            s.GoogleAnalyticsProfileId = i.GoogleAnalyticsProfileId;
            s.GoogleClientId = i.GoogleClientId;
            s.GoogleClientSecret = i.GoogleClientSecret;
            s.IsServerAdminSite = i.IsServerAdminSite;
            s.Layout = i.Layout;
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
            s.MinReqNonAlphaChars = i.MinReqNonAlphaChars;
            s.MinRequiredPasswordLength = i.MinRequiredPasswordLength;
            s.PasswordAttemptWindowMinutes = i.PasswordAttemptWindowMinutes;
            s.PreferredHostName = i.PreferredHostName;
            s.PrivacyPolicy = i.PrivacyPolicy;
            s.ReallyDeleteUsers = i.ReallyDeleteUsers;
            s.RecaptchaPrivateKey = i.RecaptchaPrivateKey;
            s.RecaptchaPublicKey = i.RecaptchaPublicKey;
            s.RegistrationAgreement = i.RegistrationAgreement;
            s.RegistrationPreamble = i.RegistrationPreamble;
            s.RequireApprovalBeforeLogin = i.RequireApprovalBeforeLogin;
            s.RequiresQuestionAndAnswer = i.RequiresQuestionAndAnswer;
            s.SiteFolderName = i.SiteFolderName;
            s.SiteGuid = i.SiteGuid;
            s.SiteId = i.SiteId;
            s.SiteIsClosed = i.SiteIsClosed;
            s.SiteIsClosedMessage = i.SiteIsClosedMessage;
            s.SiteName = i.SiteName;
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
            s.UseEmailForLogin = i.UseEmailForLogin;
            s.UseLdapAuth = i.UseLdapAuth;
            s.UseSecureRegistration = i.UseSecureRegistration;
            s.UseSslOnAllPages = i.UseSslOnAllPages;
            


            return s;
        }

    }
}
