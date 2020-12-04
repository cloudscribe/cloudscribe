// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2019-04-20
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
        
        public bool RequireCookieConsent { get; set; } = true;
        
        public string CookiePolicySummary { get; set; } = "To ensure you get the best experience, this website uses cookies.";
        
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        
        public string Theme { get; set; }

        public bool AllowNewRegistration { get; set; } = true;
        
        public bool RequireConfirmedEmail { get; set; } = false;
        
        public bool RequireConfirmedPhone { get; set; } = false;

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

        /// <summary>
        /// This property is now really Allow login with email
        /// https://security.stackexchange.com/questions/57762/log-in-with-email-is-more-secure-than-a-username
        /// https://ux.stackexchange.com/questions/13647/email-field-or-username-for-logging-in-an-application
        /// </summary>
        public bool UseEmailForLogin { get; set; } = true;

        public bool AllowUserToChangeEmail { get; set; } = true;

        public bool DisableDbAuth { get; set; } = false;
       
        public bool RequiresQuestionAndAnswer { get; set; } = false;

        public bool RequireApprovalBeforeLogin { get; set; } = false;
        
        public string AccountApprovalEmailCsv { get; set; }
        
        public int MaxInvalidPasswordAttempts { get; set; } = 5;
        
        public int MinRequiredPasswordLength { get; set; } = 7;
        
        public bool PwdRequireNonAlpha { get; set; } = true;
        
        public bool PwdRequireLowercase { get; set; } = true;
        
        public bool PwdRequireUppercase { get; set; } = true;
       
        public bool PwdRequireDigit { get; set; } = true;
        
        public bool AllowPersistentLogin { get; set; } = true;
        
        public bool CaptchaOnRegistration { get; set; } = false;
        
        public bool CaptchaOnLogin { get; set; } = false;
        
        public string RecaptchaPrivateKey { get; set; }

        public string RecaptchaPublicKey { get; set; }

        public bool UseInvisibleRecaptcha { get; set; } = false;
   
        public string FacebookAppId { get; set; }

        public string FacebookAppSecret { get; set; }

        public string MicrosoftClientId { get; set; }

        public string MicrosoftClientSecret { get; set; }

        public string GoogleClientId { get; set; }

        public string GoogleClientSecret { get; set; }

        public string TwitterConsumerKey { get; set; }

        public string TwitterConsumerSecret { get; set; }

        public string OidConnectAppId { get; set; }

        // protected with data protection
        public string OidConnectAppSecret { get; set; }

        public string OidConnectAuthority { get; set; }

        public string OidConnectDisplayName { get; set; }

        public string OidConnectScopesCsv { get; set; }

        public string AddThisDotComUsername { get; set; }
        
        public string TimeZoneId { get; set; } = "America/New_York";
        
        public string CompanyName { get; set; }
        
        public string CompanyStreetAddress { get; set; }
        
        public string CompanyStreetAddress2 { get; set; }
        
        public string CompanyLocality { get; set; }
        
        public string CompanyRegion { get; set; }
       
        public string CompanyPostalCode { get; set; }
        
        public string CompanyCountry { get; set; }
        
        public string CompanyPhone { get; set; }
        
        public string CompanyFax { get; set; }
       
        public string CompanyPublicEmail { get; set; }
        
        public string CompanyWebsite { get; set; }

        public string DefaultEmailFromAddress { get; set; }
        
        public string DefaultEmailFromAlias { get; set; }
        
        public string SmtpUser { get; set; }
        
        public string SmtpPassword { get; set; }
        
        public int SmtpPort { get; set; } = 25;

        public string SmtpPreferredEncoding { get; set; }
        
        public string SmtpServer { get; set; }
        
        public bool SmtpRequiresAuth { get; set; } = false;

        public bool SmtpUseSsl { get; set; } = false;

        public string DkimPublicKey { get; set; }
        
        // protected with data protection
        public string DkimPrivateKey { get; set; }
       
        public string DkimDomain { get; set; }
        
        public string DkimSelector { get; set; }
        
        public bool SignEmailWithDkim { get; set; } = false;

        public string EmailSenderName { get; set; } = "SmtpMailSender";

        public string EmailApiKey { get; set; }

        public string EmailApiEndpoint { get; set; }
        
        public string SmsClientId { get; set; }
        
        //protected with data protection
        public string SmsSecureToken { get; set; }
        
        public string SmsFrom { get; set; }
        
        public string GoogleAnalyticsProfileId { get; set; }
        
        public string RegistrationAgreement { get; set; }
        
        public string RegistrationPreamble { get; set; }
        
        public string LoginInfoTop { get; set; }
        
        public string LoginInfoBottom { get; set; }
        
        public bool SiteIsClosed { get; set; } = false;
        
        public string SiteIsClosedMessage { get; set; }
        
        public string PrivacyPolicy { get; set; }
       
        public bool IsDataProtected { get; set; } = false;
        

        public DateTime TermsUpdatedUtc { get; set; } = DateTime.UtcNow;

        public string ForcedCulture { get; set; }

        public string ForcedUICulture { get; set; }

        public bool Require2FA { get; set; }

        public bool ShowSiteNameLink { get; set; } = true;

        public string HeaderContent { get; set; }

        public string FooterContent { get; set; }

        public bool SingleBrowserSessions { get; set; }



        public static SiteSettings FromISiteSettings(ISiteSettings i)
        {
            if(i == null) { return null; }

            // can't we just   return (SiteSettings)i   here..??  - jimk

            SiteSettings s = new SiteSettings
            {
                ConcurrencyStamp           = i.ConcurrencyStamp,
                AccountApprovalEmailCsv    = i.AccountApprovalEmailCsv,
                AddThisDotComUsername      = i.AddThisDotComUsername,
               // AllowDbFallbackWithLdap  = i.AllowDbFallbackWithLdap,
                AllowNewRegistration       = i.AllowNewRegistration,
                AllowPersistentLogin       = i.AllowPersistentLogin,
                //AutoCreateLdapUserOnFirstLogin = i.AutoCreateLdapUserOnFirstLogin,
                CaptchaOnLogin             = i.CaptchaOnLogin,
                CaptchaOnRegistration      = i.CaptchaOnRegistration,
                CompanyCountry             = i.CompanyCountry,
                CompanyFax                 = i.CompanyFax,
                CompanyLocality            = i.CompanyLocality,
                CompanyName                = i.CompanyName,
                CompanyPhone               = i.CompanyPhone,
                CompanyPostalCode          = i.CompanyPostalCode,
                CompanyPublicEmail         = i.CompanyPublicEmail,
                CompanyWebsite             = i.CompanyWebsite,
                CompanyRegion              = i.CompanyRegion,
                CompanyStreetAddress       = i.CompanyStreetAddress,
                CompanyStreetAddress2      = i.CompanyStreetAddress2,
                CookiePolicySummary        = i.CookiePolicySummary,
                CreatedUtc                 = i.CreatedUtc,
                DefaultEmailFromAddress    = i.DefaultEmailFromAddress,
                DefaultEmailFromAlias      = i.DefaultEmailFromAlias,
                DisableDbAuth              = i.DisableDbAuth,
                DkimPublicKey              = i.DkimPublicKey,
                DkimPrivateKey             = i.DkimPrivateKey,
                DkimDomain                 = i.DkimDomain,
                DkimSelector               = i.DkimSelector,
                //EmailLdapDbFallback      = i.EmailLdapDbFallback,
                FooterContent              = i.FooterContent,
                ForcedCulture              = i.ForcedCulture,
                ForcedUICulture            = i.ForcedUICulture,
                FacebookAppId              = i.FacebookAppId,
                FacebookAppSecret          = i.FacebookAppSecret,
                GoogleAnalyticsProfileId   = i.GoogleAnalyticsProfileId,
                GoogleClientId             = i.GoogleClientId,
                GoogleClientSecret         = i.GoogleClientSecret,
                HeaderContent              = i.HeaderContent,
                IsDataProtected            = i.IsDataProtected,
                IsServerAdminSite          = i.IsServerAdminSite,
                Theme                      = i.Theme,
                LastModifiedUtc            = i.LastModifiedUtc,
                LdapDomain                 = i.LdapDomain,
                LdapPort                   = i.LdapPort,
                LdapRootDN                 = i.LdapRootDN,
                LdapServer                 = i.LdapServer,
                LdapUserDNKey              = i.LdapUserDNKey,
                LdapUserDNFormat           = i.LdapUserDNFormat,
                LdapUseSsl                 = i.LdapUseSsl,
                LoginInfoBottom            = i.LoginInfoBottom,
                LoginInfoTop               = i.LoginInfoTop,
                LogoUrl                    = i.LogoUrl,
                MaxInvalidPasswordAttempts = i.MaxInvalidPasswordAttempts,
                MicrosoftClientId          = i.MicrosoftClientId,
                MicrosoftClientSecret      = i.MicrosoftClientSecret,
                MinRequiredPasswordLength  = i.MinRequiredPasswordLength,
                OidConnectAppId            = i.OidConnectAppId,
                OidConnectAppSecret        = i.OidConnectAppSecret,
                OidConnectAuthority        = i.OidConnectAuthority,
                OidConnectDisplayName      = i.OidConnectDisplayName,
                OidConnectScopesCsv        = i.OidConnectScopesCsv,
                PreferredHostName          = i.PreferredHostName,
                PrivacyPolicy              = i.PrivacyPolicy,

                PwdRequireDigit            = i.PwdRequireDigit,
                PwdRequireLowercase        = i.PwdRequireLowercase,
                PwdRequireNonAlpha         = i.PwdRequireNonAlpha,
                PwdRequireUppercase        = i.PwdRequireUppercase,
                
                RecaptchaPrivateKey        = i.RecaptchaPrivateKey,
                RecaptchaPublicKey         = i.RecaptchaPublicKey,
                RegistrationAgreement      = i.RegistrationAgreement,
                RegistrationPreamble       = i.RegistrationPreamble,
                Require2FA                 = i.Require2FA,
                RequireApprovalBeforeLogin = i.RequireApprovalBeforeLogin,
                RequireConfirmedEmail      = i.RequireConfirmedEmail,
                RequireConfirmedPhone      = i.RequireConfirmedPhone,
                RequiresQuestionAndAnswer  = i.RequiresQuestionAndAnswer,
                RequireCookieConsent       = i.RequireCookieConsent,
                SignEmailWithDkim          = i.SignEmailWithDkim,
                SingleBrowserSessions      = i.SingleBrowserSessions,
                SiteFolderName             = i.SiteFolderName,
                Id                         = i.Id,
                AliasId                    = i.AliasId,
                ShowSiteNameLink           = i.ShowSiteNameLink,
                SiteIsClosed               = i.SiteIsClosed,
                SiteIsClosedMessage        = i.SiteIsClosedMessage,
                SiteName                   = i.SiteName,
                SmsClientId                = i.SmsClientId,
                SmsSecureToken             = i.SmsSecureToken,
                SmsFrom                    = i.SmsFrom,
                SmtpPassword               = i.SmtpPassword,
                SmtpPort                   = i.SmtpPort,
                SmtpPreferredEncoding      = i.SmtpPreferredEncoding,
                SmtpRequiresAuth           = i.SmtpRequiresAuth,
                SmtpServer                 = i.SmtpServer,
                SmtpUser                   = i.SmtpUser,
                SmtpUseSsl                 = i.SmtpUseSsl,
                TimeZoneId                 = i.TimeZoneId,
                TwitterConsumerKey         = i.TwitterConsumerKey,
                TwitterConsumerSecret      = i.TwitterConsumerSecret,
                UseInvisibleRecaptcha      = i.UseInvisibleRecaptcha,
                UseEmailForLogin           = i.UseEmailForLogin,
                AllowUserToChangeEmail     = i.AllowUserToChangeEmail,
                //UseLdapAuth              = i.UseLdapAuth,
                TermsUpdatedUtc            = i.TermsUpdatedUtc,
                EmailApiEndpoint           = i.EmailApiEndpoint,
                EmailApiKey                = i.EmailApiKey,
                EmailSenderName            = i.EmailSenderName
            };

            return s;
        }
    }
}
