// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores.Serialization;
using Microsoft.Extensions.Logging;
using IdentityServer4.Extensions;
using System;

namespace IdentityServer4.Stores
{
    /// <summary>
    /// Default user consent store.
    /// </summary>
    /// <seealso cref="IdentityServer4.Stores.DefaultGrantStore{IdentityServer4.Models.Consent}" />
    /// <seealso cref="IdentityServer4.Stores.IUserConsentStore" />
    public class DefaultUserConsentStore : DefaultGrantStore<Consent>, IUserConsentStore
    {
        public DefaultUserConsentStore(
            IPersistedGrantStore store, 
            PersistentGrantSerializer serializer, 
            ILogger<DefaultUserConsentStore> logger) 
            : base(Constants.PersistedGrantTypes.UserConsent, store, serializer, logger)
        {
        }

        string GetConsentKey(string subjectId, string clientId)
        {
            return subjectId + "|" + clientId;
        }

        /// <summary>
        /// Stores the user consent asynchronous.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        public Task StoreUserConsentAsync(Consent consent)
        {
            var key = GetConsentKey(consent.ClientId, consent.SubjectId);
            return StoreItemAsync(key, consent, consent.ClientId, consent.SubjectId, DateTimeHelper.UtcNow, Int32.MaxValue);
        }

        /// <summary>
        /// Gets the user consent asynchronous.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public Task<Consent> GetUserConsentAsync(string subjectId, string clientId)
        {
            var key = GetConsentKey(clientId, subjectId);
            return GetItemAsync(key);
        }

        /// <summary>
        /// Removes the user consent asynchronous.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public Task RemoveUserConsentAsync(string subjectId, string clientId)
        {
            var key = GetConsentKey(clientId, subjectId);
            return RemoveItemAsync(key);
        }
    }
}