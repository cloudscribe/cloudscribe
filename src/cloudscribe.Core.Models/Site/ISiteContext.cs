﻿using System;

namespace cloudscribe.Core.Models
{
    /// <summary>
    /// read only immutable version of ISiteSettings
    /// </summary>
    public interface ISiteContext : ILdapSettings
    {
        // from ISiteInfo but without setters
        Guid Id { get; }
        string AliasId { get; }
        string SiteName { get; }
        string PreferredHostName { get; }
        string SiteFolderName { get; }
        bool IsServerAdminSite { get; }

        // from ISiteSettings but without setters
       
        bool DisableDbAuth { get; }
        bool RequireConfirmedEmail { get; } 
        bool RequireConfirmedPhone { get; }
        bool RequireApprovalBeforeLogin { get; } 
        string AccountApprovalEmailCsv { get; }
        int MaxInvalidPasswordAttempts { get; }
        int MinRequiredPasswordLength { get; }

        bool PwdRequireNonAlpha { get; }
        bool PwdRequireLowercase { get; }
        bool PwdRequireUppercase { get; }
        bool PwdRequireDigit { get; }

        bool UseEmailForLogin { get; }
        bool AllowUserToChangeEmail { get; }

        bool RequiresQuestionAndAnswer { get; }
        bool AllowNewRegistration { get; }
        bool AllowPersistentLogin { get; }
        bool CaptchaOnLogin { get; }
        bool CaptchaOnRegistration { get; }
        string RecaptchaPrivateKey { get; }
        string RecaptchaPublicKey { get; }
        bool UseInvisibleRecaptcha { get; }

        string CompanyCountry { get; }
        string CompanyFax { get; }
        string CompanyLocality { get; }
        string CompanyName { get; }
        string CompanyPhone { get; }
        string CompanyPostalCode { get; }
        string CompanyPublicEmail { get; }
        string CompanyRegion { get; }
        string CompanyStreetAddress { get; }
        string CompanyStreetAddress2 { get; }
        string CompanyWebsite { get; }
        string LoginInfoBottom { get;  }
        string LoginInfoTop { get; }
        string RegistrationAgreement { get; }
        string RegistrationPreamble { get; }
        string TimeZoneId { get; }
        bool SiteIsClosed { get; }
        string SiteIsClosedMessage { get; }
        string DefaultEmailFromAddress { get; }
        string DefaultEmailFromAlias { get; }
        string SmtpPassword { get; }
        int SmtpPort { get; }
        string SmtpPreferredEncoding { get; }
        bool SmtpRequiresAuth { get; }
        string SmtpServer { get; }
        string SmtpUser { get; }
        bool SmtpUseSsl { get; }
        string DkimPublicKey { get; }
        string DkimPrivateKey { get; } 
        string DkimDomain { get; }
        string DkimSelector { get; }
        bool SignEmailWithDkim { get; }
        string EmailSenderName { get; }
        string EmailApiKey { get; }
        string EmailApiEndpoint { get; }

        string SmsClientId { get; }
        string SmsSecureToken { get; } 
        string SmsFrom { get; }
        string PrivacyPolicy { get; }
        string Theme { get; }
        string GoogleAnalyticsProfileId { get; }

        string FacebookAppId { get; }
        string FacebookAppSecret { get; }
        string MicrosoftClientId { get; }
        string MicrosoftClientSecret { get; }
        string GoogleClientId { get; }
        string GoogleClientSecret { get; }
        string TwitterConsumerKey { get; }
        string TwitterConsumerSecret { get; }
        string OidConnectDisplayName { get; }
        string OidConnectAppId { get;  }
        string OidConnectAppSecret { get; }
        string OidConnectAuthority { get; }

        string OidConnectScopesCsv { get; }
        string AddThisDotComUsername { get; }
        DateTime CreatedUtc { get; }
        DateTime TermsUpdatedUtc { get; }

        string ForcedCulture { get; }

        string ForcedUICulture { get; }

        DateTime LastModifiedUtc { get;  }
        string CookiePolicySummary { get; }
        bool RequireCookieConsent { get;  }

        bool Require2FA { get; }

        bool ShowSiteNameLink { get; }
        string HeaderContent { get; }
        string FooterContent { get; }
        string LogoUrl { get; }

        bool SingleBrowserSessions { get; }
        string MaximumInactivityInMinutes { get; }
        string RegRestrictionTld { get; }
        int PasswordExpiryWarningDays { get; }
        int PasswordExpiresDays { get; }
    }
}
