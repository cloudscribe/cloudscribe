// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using IdentityServer4.Stores;
using IdentityServer4.Stores.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.Services.Default
{
    /// <summary>
    /// Default persisted grant service
    /// </summary>
    public class DefaultPersistedGrantService : IPersistedGrantService
    {
        private readonly ILogger<DefaultPersistedGrantService> _logger;
        private readonly IPersistedGrantStore _store;
        private readonly PersistentGrantSerializer _serializer;

        public DefaultPersistedGrantService(IPersistedGrantStore store, 
            PersistentGrantSerializer serializer,
            ILogger<DefaultPersistedGrantService> logger)
        {
            _store = store;
            _serializer = serializer;
            _logger = logger;
        }
        
        public async Task<IEnumerable<Consent>> GetAllGrantsAsync(string subjectId)
        {
            var grants = await _store.GetAllAsync(subjectId);

            var consents = grants.Where(x => x.Type == Constants.PersistedGrantTypes.UserConsent)
                .Select(x => _serializer.Deserialize<Consent>(x.Data));

            var codes = grants.Where(x => x.Type == Constants.PersistedGrantTypes.AuthorizationCode)
                .Select(x => _serializer.Deserialize<AuthorizationCode>(x.Data))
                .Select(x => new Consent
                {
                    ClientId = x.ClientId,
                    SubjectId = subjectId,
                    Scopes = x.RequestedScopes
                });

            var refresh = grants.Where(x => x.Type == Constants.PersistedGrantTypes.RefreshToken)
                .Select(x => _serializer.Deserialize<RefreshToken>(x.Data))
                .Select(x => new Consent
                {
                    ClientId = x.ClientId,
                    SubjectId = subjectId,
                    Scopes = x.Scopes
                });

            var access = grants.Where(x => x.Type == Constants.PersistedGrantTypes.ReferenceToken)
                .Select(x => _serializer.Deserialize<Token>(x.Data))
                .Select(x => new Consent
                {
                    ClientId = x.ClientId,
                    SubjectId = subjectId,
                    Scopes = x.Scopes
                });

            consents = Join(consents, codes);
            consents = Join(consents, refresh);
            consents = Join(consents, access);

            return consents.ToArray();
        }

        IEnumerable<Consent> Join(IEnumerable<Consent> first, IEnumerable<Consent> second)
        {
            var query =
                from f in first
                join s in second on f.ClientId equals s.ClientId
                let scopes = f.Scopes.Union(s.Scopes).Distinct()
                select new Consent
                {
                    ClientId = f.ClientId,
                    SubjectId = f.SubjectId,
                    Scopes = scopes
                };
            return query;
        }

        public Task RemoveAllGrantsAsync(string subjectId, string clientId)
        {
            return _store.RemoveAllAsync(subjectId, clientId);
        }
    }
}