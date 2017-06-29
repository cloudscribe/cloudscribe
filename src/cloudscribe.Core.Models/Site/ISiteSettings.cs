// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2017-06-29
// 

using System;

namespace cloudscribe.Core.Models
{
    
    // lighter base version for lists
    public interface ISiteInfo
    {
        Guid Id { get; set; }
        string AliasId { get; set; }
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

        string ConcurrencyStamp { get; set; }

        
        bool DisableDbAuth { get; set; }
    
        bool RequireConfirmedEmail { get; set; } 

        // maps to IdentitySignInOptions
        //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNetCore.Identity/SignInOptions.cs
        bool RequireConfirmedPhone { get; set; }

        bool RequireApprovalBeforeLogin { get; set; } 
        string AccountApprovalEmailCsv { get; set; }

        //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNetCore.Identity/LockoutOptions.cs

        // should we make this configurable per site:

        /// <value>
        /// True if a newly created user can be locked out, otherwise false.
        /// </value>
        /// <remarks>
        /// Defaults to true.
        /// </remarks>
        //public bool AllowedForNewUsers { get; set; } = true;

            
        /// <summary>
        /// maps to Identity LockoutOptions.MaxFailedAttempts default 5
        /// </summary>
        int MaxInvalidPasswordAttempts { get; set; }

        
        // This is from IdentityOptions.cs
        // should this be configurable per site or not? I lean towards not
        /// <summary>
        /// Gets or sets the <see cref="TimeSpan"/> after which security stamps are re-validated.
        /// </summary>
        /// <value>
        /// The <see cref="TimeSpan"/> after which security stamps are re-validated.
        /// </value>
        //public TimeSpan SecurityStampValidationInterval { get; set; } = TimeSpan.FromMinutes(30);



        /// <summary>
        /// min password length is something that should be allowed to be configured
        /// on a site by site basis except in related sites mode where the related site should determine all security settings
        /// It is reasonable that some may want to require longer passwords, but we probably should not let passwords ever be shorter than 6
        /// which is the default in IdentityPasswordOptions
        /// </summary>
        int MinRequiredPasswordLength { get; set; } // maps to IdentityPasswordOptions public int RequiredLength { get; set; } = 6;


        // typically we are using true for UseEmailForLogin, as such we probably need to override
        // IdentityUserOptions.cs https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNetCore.Identity/UserOptions.cs
        // which by default RequireUniqueEmail = false and we need it to be true if using email for login
        // the problem comes if someone sets up a site not using email for login
        // but later changes to use email for login after accounts with duplicate emails exist
        // therefore I think in all cases we should require unique email

        bool UseEmailForLogin { get; set; }
        
        bool RequiresQuestionAndAnswer { get; set; }
        bool ReallyDeleteUsers { get; set; }
        //TODO: implement
        bool AllowNewRegistration { get; set; }
      
        bool AllowPersistentLogin { get; set; }
        
        bool CaptchaOnLogin { get; set; }
        bool CaptchaOnRegistration { get; set; }
        string RecaptchaPrivateKey { get; set; }
        string RecaptchaPublicKey { get; set; }
        bool UseInvisibleRecaptcha { get; set; }
        
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
        string DefaultEmailFromAlias { get; set; }
      
        string SmtpPassword { get; set; }
        int SmtpPort { get; set; }
        string SmtpPreferredEncoding { get; set; }
        bool SmtpRequiresAuth { get; set; }
        string SmtpServer { get; set; }
        string SmtpUser { get; set; }
        bool SmtpUseSsl { get; set; }

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
        
        //social login stuff
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

        string AddThisDotComUsername { get; set; }

        
        bool IsDataProtected { get; set; }
        DateTime CreatedUtc { get; set; }
        DateTime TermsUpdatedUtc { get; set; }

        string ForcedCulture { get; set; }

        string ForcedUICulture { get; set; }


        // TODO: drop
        //bool AllowUserFullNameChange { get; set; }
        //string ApiKeyExtra1 { get; set; }
        //string ApiKeyExtra2 { get; set; }
        //string ApiKeyExtra3 { get; set; }
        //string ApiKeyExtra4 { get; set; }
        //string ApiKeyExtra5 { get; set; }
        //bool UseSslOnAllPages { get; set; }
        //int MinReqNonAlphaChars { get; set; }
        //int PasswordAttemptWindowMinutes { get; set; }

    }
}
