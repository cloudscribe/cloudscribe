// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Stores
{
    public class CachingClientStore<T> : IClientStore
        where T : IClientStore
    {
        private readonly IdentityServerOptions _options;
        private readonly ICache<Client> _cache;
        private readonly IClientStore _inner;
        private readonly ILogger<CachingClientStore<T>> _logger;

        public CachingClientStore(IdentityServerOptions options, T inner, ICache<Client> cache, ILogger<CachingClientStore<T>> logger)
        {
            _options = options;
            _inner = inner;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = await _cache.GetAsync(clientId,
                _options.CachingOptions.ClientStoreExpiration,
                () => _inner.FindClientByIdAsync(clientId),
                _logger);

            return client;
        }
    }
}