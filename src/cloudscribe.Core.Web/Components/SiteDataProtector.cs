// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-18
// Last Modified:			2016-01-27
// 


using cloudscribe.Core.Models;
using Microsoft.AspNet.DataProtection;
using Microsoft.Extensions.Logging;
using System;

namespace cloudscribe.Core.Web.Components
{
    public class SiteDataProtector
    {
        public SiteDataProtector(
            IDataProtectionProvider dataProtectionProvider,
            ILogger<SiteDataProtector> logger)
        {
            rawProtector = dataProtectionProvider.CreateProtector("cloudscribe.Core.Models.SiteSettings");
            log = logger;
        }

        private ILogger log;
        private IDataProtector rawProtector = null;
        private IPersistedDataProtector dataProtector
        {
            get { return rawProtector as IPersistedDataProtector; }
        }

        public void Protect(ISiteSettings site)
        {
            if (site == null) { throw new ArgumentNullException("you must pass in an implementation of ISiteSettings"); }
            if (site.IsDataProtected) { return; }
            if (dataProtector == null) { return; }

            if (site.FacebookAppSecret.Length > 0)
            {
                try
                {
                    site.FacebookAppSecret = dataProtector.PersistentProtect(site.FacebookAppSecret);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.GoogleClientSecret.Length > 0)
            {
                try
                {
                    site.GoogleClientSecret = dataProtector.PersistentProtect(site.GoogleClientSecret);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.MicrosoftClientSecret.Length > 0)
            {
                try
                {
                    site.MicrosoftClientSecret = dataProtector.PersistentProtect(site.MicrosoftClientSecret);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.TwitterConsumerSecret.Length > 0)
            {
                try
                {
                    site.TwitterConsumerSecret = dataProtector.PersistentProtect(site.TwitterConsumerSecret);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.SmtpPassword.Length > 0)
            {
                try
                {
                    site.SmtpPassword = dataProtector.PersistentProtect(site.SmtpPassword);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.OidConnectAppSecret.Length > 0)
            {
                try
                {
                    site.OidConnectAppSecret = dataProtector.PersistentProtect(site.OidConnectAppSecret);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.DkimPrivateKey.Length > 0)
            {
                try
                {
                    site.DkimPrivateKey = dataProtector.PersistentProtect(site.DkimPrivateKey);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.SmsSecureToken.Length > 0)
            {
                try
                {
                    site.SmsSecureToken = dataProtector.PersistentProtect(site.SmsSecureToken);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            site.IsDataProtected = true;

            
        }

        public void UnProtect(ISiteSettings site)
        {
            bool requiresMigration = false;
            bool wasRevoked = false;
            if (site == null) { throw new ArgumentNullException("you must pass in an implementation of ISiteSettings"); }
            if (!site.IsDataProtected) { return; }

            if (site.FacebookAppSecret.Length > 0)
            {
                try
                {
                    site.FacebookAppSecret = dataProtector.PersistentUnprotect(site.FacebookAppSecret, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }
                catch (FormatException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.GoogleClientSecret.Length > 0)
            {
                try
                {
                    site.GoogleClientSecret = dataProtector.PersistentUnprotect(site.GoogleClientSecret, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }
                catch (FormatException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.MicrosoftClientSecret.Length > 0)
            {
                try
                {
                    site.MicrosoftClientSecret = dataProtector.PersistentUnprotect(site.MicrosoftClientSecret, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }
                catch (FormatException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.TwitterConsumerSecret.Length > 0)
            {
                try
                {
                    site.TwitterConsumerSecret = dataProtector.PersistentUnprotect(site.TwitterConsumerSecret, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }
                catch (FormatException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.SmtpPassword.Length > 0)
            {
                try
                {
                    site.SmtpPassword = dataProtector.PersistentUnprotect(site.SmtpPassword, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }
                catch (FormatException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.OidConnectAppSecret.Length > 0)
            {
                try
                {
                    site.OidConnectAppSecret = dataProtector.PersistentUnprotect(site.OidConnectAppSecret, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }
                catch (FormatException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.SmsSecureToken.Length > 0)
            {
                try
                {
                    site.SmsSecureToken = dataProtector.PersistentUnprotect(site.SmsSecureToken, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }
                catch (FormatException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            if (site.DkimPrivateKey.Length > 0)
            {
                try
                {
                    site.DkimPrivateKey = dataProtector.PersistentUnprotect(site.DkimPrivateKey, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    log.LogError("data protection error", ex);
                }
                catch (FormatException ex)
                {
                    log.LogError("data protection error", ex);
                }

            }

            site.IsDataProtected = false;

            if (requiresMigration || wasRevoked)
            {
                log.LogWarning("DataProtection key wasRevoked or requires migration, save site settings for " + site.SiteName + " to protect with a new key");
            }

            
        }


    }
}
