// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-18
// Last Modified:			2018-06-22
// 


using cloudscribe.Core.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using System;

namespace cloudscribe.Core.DataProtection
{
    public class SiteDataProtector
    {
        public SiteDataProtector(
            IDataProtectionProvider dataProtectionProvider,
            ILogger<SiteDataProtector> logger)
        {
            _rawProtector = dataProtectionProvider.CreateProtector("cloudscribe.Core.Models.SiteSettings");
            _log = logger;
        }

        private ILogger _log;
        private IDataProtector _rawProtector = null;
        private IPersistedDataProtector DataProtector
        {
            get { return _rawProtector as IPersistedDataProtector; }
        }

        public void Protect(ISiteSettings site)
        {
            if (site == null) { throw new ArgumentNullException("you must pass in an implementation of ISiteSettings"); }
            if (site.IsDataProtected) { return; }
            if (DataProtector == null) { return; }

            var countOfProtectedItems = 0;

            if (!string.IsNullOrWhiteSpace(site.FacebookAppSecret))
            {
                try
                {
                    site.FacebookAppSecret = DataProtector.PersistentProtect(site.FacebookAppSecret);
                    countOfProtectedItems += 1;
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error: {ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.GoogleClientSecret))
            {
                try
                {
                    site.GoogleClientSecret = DataProtector.PersistentProtect(site.GoogleClientSecret);
                    countOfProtectedItems += 1;
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error: {ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.MicrosoftClientSecret))
            {
                try
                {
                    site.MicrosoftClientSecret = DataProtector.PersistentProtect(site.MicrosoftClientSecret);
                    countOfProtectedItems += 1;
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error: {ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.TwitterConsumerSecret))
            {
                try
                {
                    site.TwitterConsumerSecret = DataProtector.PersistentProtect(site.TwitterConsumerSecret);
                    countOfProtectedItems += 1;
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error: {ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.SmtpPassword))
            {
                try
                {
                    site.SmtpPassword = DataProtector.PersistentProtect(site.SmtpPassword);
                    countOfProtectedItems += 1;
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error: {ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.EmailApiKey))
            {
                try
                {
                    site.EmailApiKey = DataProtector.PersistentProtect(site.EmailApiKey);
                    countOfProtectedItems += 1;
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error: {ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.OidConnectAppSecret))
            {
                try
                {
                    site.OidConnectAppSecret = DataProtector.PersistentProtect(site.OidConnectAppSecret);
                    countOfProtectedItems += 1;
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error: {ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.DkimPrivateKey))
            {
                try
                {
                    site.DkimPrivateKey = DataProtector.PersistentProtect(site.DkimPrivateKey);
                    countOfProtectedItems += 1;
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error: {ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.SmsSecureToken))
            {
                try
                {
                    site.SmsSecureToken = DataProtector.PersistentProtect(site.SmsSecureToken);
                    countOfProtectedItems += 1;
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error: {ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            site.IsDataProtected = countOfProtectedItems > 0;

            
        }

        public void UnProtect(ISiteSettings site)
        {
            bool requiresMigration = false;
            bool wasRevoked = false;
            if (site == null) { throw new ArgumentNullException("you must pass in an implementation of ISiteSettings"); }
            if (!site.IsDataProtected) { return; }

            if (!string.IsNullOrWhiteSpace(site.FacebookAppSecret))
            {
                try
                {
                    site.FacebookAppSecret = DataProtector.PersistentUnprotect(site.FacebookAppSecret, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }
                catch (FormatException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.GoogleClientSecret))
            {
                try
                {
                    site.GoogleClientSecret = DataProtector.PersistentUnprotect(site.GoogleClientSecret, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }
                catch (FormatException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.MicrosoftClientSecret))
            {
                try
                {
                    site.MicrosoftClientSecret = DataProtector.PersistentUnprotect(site.MicrosoftClientSecret, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }
                catch (FormatException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.TwitterConsumerSecret))
            {
                try
                {
                    site.TwitterConsumerSecret = DataProtector.PersistentUnprotect(site.TwitterConsumerSecret, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }
                catch (FormatException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.SmtpPassword))
            {
                try
                {
                    site.SmtpPassword = DataProtector.PersistentUnprotect(site.SmtpPassword, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }
                catch (FormatException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.EmailApiKey))
            {
                try
                {
                    site.EmailApiKey = DataProtector.PersistentUnprotect(site.EmailApiKey, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }
                catch (FormatException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.OidConnectAppSecret))
            {
                try
                {
                    site.OidConnectAppSecret = DataProtector.PersistentUnprotect(site.OidConnectAppSecret, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }
                catch (FormatException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }
                

            }

            if (!string.IsNullOrWhiteSpace(site.SmsSecureToken))
            {
                try
                {
                    site.SmsSecureToken = DataProtector.PersistentUnprotect(site.SmsSecureToken, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }
                catch (FormatException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            if (!string.IsNullOrWhiteSpace(site.DkimPrivateKey))
            {
                try
                {
                    site.DkimPrivateKey = DataProtector.PersistentUnprotect(site.DkimPrivateKey, out requiresMigration, out wasRevoked);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }
                catch (FormatException ex)
                {
                    _log.LogError($"data protection error:{ex.Message} stacktrace: {ex.StackTrace}");
                }

            }

            site.IsDataProtected = false;

            if (requiresMigration || wasRevoked)
            {
                _log.LogDebug($"DataProtection key was Revoked or requires migration, save site settings for {site.SiteName} to protect with a new key");
            }

            
        }


    }
}
