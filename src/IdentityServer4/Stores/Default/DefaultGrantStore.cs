// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Stores.Serialization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.Stores
{
    /// <summary>
    /// Base class for persisting grants using the IPersistedGrantStore.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultGrantStore<T>
    {
        private readonly string _grantType;
        private readonly ILogger _logger;
        private readonly IPersistedGrantStore _store;
        private readonly PersistentGrantSerializer _serializer;

        protected DefaultGrantStore(string grantType,
            IPersistedGrantStore store,
            PersistentGrantSerializer serializer,
            ILogger logger)
        {
            if (grantType.IsMissing()) throw new ArgumentNullException(nameof(grantType));

            _grantType = grantType;
            _store = store;
            _serializer = serializer;
            _logger = logger;
        }

        const string KeySeparator = ":";

        protected string GetHashedKey(string value)
        {
            return (value + KeySeparator + _grantType).Sha256();
        }

        protected async Task<T> GetItemAsync(string key)
        {
            key = GetHashedKey(key);

            var grant = await _store.GetAsync(key);
            if (grant != null && grant.Type == _grantType)
            {
                return _serializer.Deserialize<T>(grant.Data);
            }

            return default(T);
        }

        protected async Task StoreItemAsync(string key, T item, string clientId, string subjectId, DateTime created, int lifetime)
        {
            key = GetHashedKey(key);

            var json = _serializer.Serialize(item);

            var grant = new PersistedGrant()
            {
                Key = key,
                Type = _grantType,
                ClientId = clientId,
                SubjectId = subjectId,
                CreationTime = created,
                Expiration = created.AddSeconds(lifetime),
                Data = json,
            };

            await _store.StoreAsync(grant);
        }

        protected async Task RemoveItemAsync(string key)
        {
            key = GetHashedKey(key);
            await _store.RemoveAsync(key);
        }

        protected async Task RemoveAllAsync(string subjectId, string clientId)
        {
            await _store.RemoveAllAsync(subjectId, clientId, _grantType);
        }
    }
}