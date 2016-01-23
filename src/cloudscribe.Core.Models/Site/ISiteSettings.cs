// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2015-11-12
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

        // TODO use this to force ONLY social logins or ldap option?
        bool DisableDbAuth { get; set; }
        // TODO: implement
        bool UseSecureRegistration { get; set; } // rename to reflect function RequireConfirmedEmail, maps to SignInOptions RequireConfirmedEmail

        //TODO: add this which maps to IdentitySignInOptions
        //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNetCore.Identity/SignInOptions.cs
        //public bool RequireConfirmedPhoneNumber { get; set; }

        bool RequireApprovalBeforeLogin { get; set; } // TODO: implement

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

        /// <summary>
        /// maps to LockoutOptions DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        /// </summary>
        int PasswordAttemptWindowMinutes { get; set; }

        // IdentityPasswordOptions properties are different than what we currently have
        // https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNetCore.Identity/PasswordOptions.cs
        // we should consider removing this since it doesn't map to IdentityPasswordOptions
        int MinReqNonAlphaChars { get; set; }

        // We could make these other options also site settings but to me it seems like a bad idea to do so.
        // following owasp guidelines I think we do not want to let site admins make bad security decisions
        // that lower security below standards, we should only allow them to make more strict settings
        // other IdentityPasswordOptions:
        // public bool RequireNonAlphanumeric { get; set; } = true;
        // public bool RequireLowercase { get; set; } = true;
        // public bool RequireUppercase { get; set; } = true;
        // public bool RequireDigit { get; set; } = true;

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


        bool UseSslOnAllPages { get; set; }
       
        bool RequiresQuestionAndAnswer { get; set; }
        bool ReallyDeleteUsers { get; set; }
        //TODO: implement
        bool AllowNewRegistration { get; set; }
        // TODO: implement
        bool AllowPersistentLogin { get; set; }
        // TODO: implement
        bool AllowUserFullNameChange { get; set; }

        bool CaptchaOnLogin { get; set; }
        bool CaptchaOnRegistration { get; set; }
        string RecaptchaPrivateKey { get; set; }
        string RecaptchaPublicKey { get; set; }
        
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
        string SmtpPassword { get; set; }
        int SmtpPort { get; set; }
        string SmtpPreferredEncoding { get; set; }
        bool SmtpRequiresAuth { get; set; }
        string SmtpServer { get; set; }
        string SmtpUser { get; set; }
        bool SmtpUseSsl { get; set; }
        
        string PrivacyPolicy { get; set; }
        string Layout { get; set; }
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

        string AddThisDotComUsername { get; set; }

        string ApiKeyExtra1 { get; set; } 
        string ApiKeyExtra2 { get; set; } 
        string ApiKeyExtra3 { get; set; }
        string ApiKeyExtra4 { get; set; } 
        string ApiKeyExtra5 { get; set; }

        bool IsDataProtected { get; set; }
        DateTime CreatedUtc { get; set; }
   
    }
}
