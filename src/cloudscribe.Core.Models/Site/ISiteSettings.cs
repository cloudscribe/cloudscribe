﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2019-04-20
// 

using System;

namespace cloudscribe.Core.Models
{
    
    public interface ISiteInfo
    {
        Guid Id { get; set; }
        string AliasId { get; set; }
        string SiteName { get; set; }
        string PreferredHostName { get; set; }
        string SiteFolderName { get; set; }
        bool IsServerAdminSite { get; set; }
        string LogoUrl { get; set; }

    }

    
    public interface ISiteSettings : ISiteInfo
    {
        string LdapServer { get; set; }
        string LdapDomain { get; set; }
        int LdapPort { get; set; }
        string LdapRootDN { get; set; }
        string LdapUserDNKey { get; set; }

        string LdapUserDNFormat { get; set; }
        bool LdapUseSsl { get; set; }
        string ConcurrencyStamp { get; set; }
        bool DisableDbAuth { get; set; }
        bool RequireConfirmedEmail { get; set; } 

        // maps to IdentitySignInOptions
        //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNetCore.Identity/SignInOptions.cs
        bool RequireConfirmedPhone { get; set; }

        bool RequireApprovalBeforeLogin { get; set; } 
        string AccountApprovalEmailCsv { get; set; }
           
        /// <summary>
        /// maps to Identity LockoutOptions.MaxFailedAttempts default 5
        /// </summary>
        int MaxInvalidPasswordAttempts { get; set; }

        /// <summary>
        /// min password length is something that should be allowed to be configured
        /// on a site by site basis except in related sites mode where the related site should determine all security settings
        /// It is reasonable that some may want to require longer passwords, but we probably should not let passwords ever be shorter than 6
        /// which is the default in IdentityPasswordOptions
        /// </summary>
        int MinRequiredPasswordLength { get; set; } 

        bool PwdRequireNonAlpha { get; set; }
        bool PwdRequireLowercase { get; set; }
        bool PwdRequireUppercase { get; set; }
        bool PwdRequireDigit { get; set; }

        // typically we are using true for UseEmailForLogin, as such we probably need to override
        // IdentityUserOptions.cs https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNetCore.Identity/UserOptions.cs
        // which by default RequireUniqueEmail = false and we need it to be true if using email for login
        // the problem comes if someone sets up a site not using email for login
        // but later changes to use email for login after accounts with duplicate emails exist
        // therefore I think in all cases we should require unique email

        /// <summary>
        /// This property is npw really "Allow Login With Email
        /// https://security.stackexchange.com/questions/57762/log-in-with-email-is-more-secure-than-a-username
        /// https://ux.stackexchange.com/questions/13647/email-field-or-username-for-logging-in-an-application
        /// </summary>
        bool UseEmailForLogin { get; set; }

        bool AllowUserToChangeEmail {  get; set; }
        
        bool RequiresQuestionAndAnswer { get; set; }
        
        bool AllowNewRegistration { get; set; }
      
        bool AllowPersistentLogin { get; set; }
        
        bool CaptchaOnLogin { get; set; }
        bool CaptchaOnRegistration { get; set; }
        string RecaptchaPrivateKey { get; set; }
        string RecaptchaPublicKey { get; set; }
        bool UseInvisibleRecaptcha { get; set; }
        
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

        string CompanyWebsite { get; set; }

        string LoginInfoBottom { get; set; }
        string LoginInfoTop { get; set; }
        string RegistrationAgreement { get; set; }
        string RegistrationPreamble { get; set; }
        
        string TimeZoneId { get; set; }
        bool SiteIsClosed { get; set; }
        string SiteIsClosedMessage { get; set; }

        string DefaultEmailFromAddress { get; set; }
        string DefaultEmailFromAlias { get; set; }
      
        string SmtpPassword { get; set; }
        int SmtpPort { get; set; }
        string SmtpPreferredEncoding { get; set; }
        bool SmtpRequiresAuth { get; set; }
        string SmtpServer { get; set; }
        string SmtpUser { get; set; }
        bool SmtpUseSsl { get; set; }

        string EmailSenderName { get; set; }
        string EmailApiKey { get; set; }
        string EmailApiEndpoint { get; set; }

        string DkimPublicKey { get; set; }
        string DkimPrivateKey { get; set; } // protected with data protection
        string DkimDomain { get; set; }
        /// <summary>
        /// The selector is used to identify the keys used to attach a token to a piece of email. It does appear in the header 
        /// of the email sent, but isn’t otherwise visible or meaningful to the final recipient. Any time you generate a new key pair 
        /// you need to choose a new selector.
        /// A selector is a string of no more than 63 lower-case alphanumeric characters(a-z or 0-9) followed by a period “.”, followed 
        /// by another string of no more than 63 lower-case alphanumeric characters.
        /// </summary>
        string DkimSelector { get; set; }
        bool SignEmailWithDkim { get; set; }

        string SmsClientId { get; set; }
        string SmsSecureToken { get; set; } //protected with data protection
        string SmsFrom { get; set; }

        string PrivacyPolicy { get; set; }
        string Theme { get; set; } 
        string GoogleAnalyticsProfileId { get; set; }
        
        string FacebookAppId { get; set; }
        string FacebookAppSecret { get; set; }
        string MicrosoftClientId { get; set; }
        string MicrosoftClientSecret { get; set; }
        string GoogleClientId { get; set; }
        string GoogleClientSecret { get; set; }
        string TwitterConsumerKey { get; set; }
        string TwitterConsumerSecret { get; set; }

        string OidConnectDisplayName { get; set; }
        string OidConnectAppId { get; set; }
        string OidConnectAppSecret { get; set; } // protected with data protection
        string OidConnectAuthority { get; set; }
        string OidConnectScopesCsv { get; set; }

        string AddThisDotComUsername { get; set; }
        
        bool IsDataProtected { get; set; }
        DateTime CreatedUtc { get; set; }
        DateTime TermsUpdatedUtc { get; set; }

        string ForcedCulture { get; set; }

        string ForcedUICulture { get; set; }

        DateTime LastModifiedUtc { get; set; }
        string CookiePolicySummary { get; set; }
        bool RequireCookieConsent { get; set; }

        bool Require2FA { get; set; }

        bool ShowSiteNameLink { get; set; }
        string HeaderContent { get; set; }
        string FooterContent { get; set; }

        /// <summary>
        /// false by default, if true a random guid will be generated upon login, saved in SiteUser.BrowserKey and in BrowserKey claim on claimsprincipal.
        /// Site Rules middleware will signout any user whose current BrowserKey claim doesn't match SiteUser.BrowserKey
        /// This way if a user logs in using a new browser, any lingering authenticated session in the previous browser will be signed out.
        /// Newest login wins. This option is for preventing sharing of user accounts.
        /// </summary>
        bool SingleBrowserSessions { get; set; }

        string RegRestrictionTld { get; set; }
        string MaximumInactivityInMinutes { get; set; }
        int PasswordExpiryWarningDays {  get; set; }
        int PasswordExpiresDays { get; set; }
    }
}
