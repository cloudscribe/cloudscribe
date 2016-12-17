// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IdentityServer4.Stores.InMemory
{
    /// <summary>
    /// In-memory persisted grant store
    /// </summary>
    public class InMemoryPersistedGrantStore : IPersistedGrantStore
    {
        private readonly ConcurrentDictionary<string, PersistedGrant> _repository = new ConcurrentDictionary<string, PersistedGrant>();

        public Task StoreAsync(PersistedGrant grant)
        {
            _repository[grant.Key] = grant;

            return Task.FromResult(0);
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            PersistedGrant token;
            if (_repository.TryGetValue(key, out token))
            {
                return Task.FromResult(token);
            }

            return Task.FromResult<PersistedGrant>(null);
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var query =
                from item in _repository
                where item.Value.SubjectId == subjectId
                select item.Value;

            var items = query.ToArray().AsEnumerable();
            return Task.FromResult(items);
        }

        public Task RemoveAsync(string key)
        {
            PersistedGrant val;
            _repository.TryRemove(key, out val);

            return Task.FromResult(0);
        }

        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            var query =
                from item in _repository
                where item.Value.ClientId == clientId &&
                    item.Value.SubjectId == subjectId
                select item.Key;

            var keys = query.ToArray();
            foreach (var key in keys)
            {
                PersistedGrant grant;
                _repository.TryRemove(key, out grant);
            }

            return Task.FromResult(0);
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var query =
                from item in _repository
                where item.Value.SubjectId == subjectId &&
                    item.Value.ClientId == clientId &&
                    item.Value.Type == type
                select item.Key;

            var keys = query.ToArray();
            foreach (var key in keys)
            {
                PersistedGrant grant;
                _repository.TryRemove(key, out grant);
            }

            return Task.FromResult(0);
        }
    }
}