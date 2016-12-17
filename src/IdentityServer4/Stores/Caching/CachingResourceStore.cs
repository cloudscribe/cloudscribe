// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;

namespace IdentityServer4.Stores
{
    public class CachingResourceStore<T> : IResourceStore
        where T : IResourceStore
    {
        const string AllKey = "__all__";

        private readonly IdentityServerOptions _options;
        private readonly ICache<IEnumerable<IdentityResource>> _identityCache;
        private readonly ICache<IEnumerable<ApiResource>> _apiByScopeCache;
        private readonly ICache<ApiResource> _apiCache;
        private readonly ICache<Resources> _allCache;
        private readonly IResourceStore _inner;
        private readonly ILogger<CachingResourceStore<T>> _logger;

        public CachingResourceStore(IdentityServerOptions options, T inner, 
            ICache<IEnumerable<IdentityResource>> identityCache, 
            ICache<IEnumerable<ApiResource>> apiByScopeCache,
            ICache<ApiResource> apiCache,
            ICache<Resources> allCache,
            ILogger<CachingResourceStore<T>> logger)
        {
            _options = options;
            _inner = inner;
            _identityCache = identityCache;
            _apiByScopeCache = apiByScopeCache;
            _apiCache = apiCache;
            _allCache = allCache;
            _logger = logger;
        }

        private string GetKey(IEnumerable<string> names)
        {
            if (names == null || !names.Any()) return "";
            return names.OrderBy(x => x).Aggregate((x, y) => x + "," + y);
        }

        public async Task<Resources> GetAllResources()
        {
            var key = AllKey;

            var all = await _allCache.GetAsync(key,
                _options.CachingOptions.ScopeStoreExpiration,
                () => _inner.GetAllResources(),
                _logger);

            return all;
        }

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var api = await _apiCache.GetAsync(name,
                _options.CachingOptions.ScopeStoreExpiration,
                () => _inner.FindApiResourceAsync(name),
                _logger);

            return api;
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> names)
        {
            var key = GetKey(names);

            var identities = await _identityCache.GetAsync(key,
                _options.CachingOptions.ScopeStoreExpiration,
                () => _inner.FindIdentityResourcesByScopeAsync(names),
                _logger);

            return identities;
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> names)
        {
            var key = GetKey(names);

            var apis = await _apiByScopeCache.GetAsync(key,
                _options.CachingOptions.ScopeStoreExpiration,
                () => _inner.FindApiResourcesByScopeAsync(names),
                _logger);

            return apis;
        }
    }
}