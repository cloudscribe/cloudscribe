// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace IdentityServer4.Services.Default
{
    public class DefaultCache<T> : ICache<T>
        where T : class
    {
        const string KeySeparator = ":";

        readonly IMemoryCache _cache;

        public DefaultCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        string GetKey(string key)
        {
            return typeof(T).FullName + KeySeparator + key;
        }

        public Task<T> GetAsync(string key)
        {
            key = GetKey(key);
            var item = _cache.Get<T>(key);
            return Task.FromResult(item);
        }

        public Task SetAsync(string key, T item, TimeSpan expiration)
        {
            key = GetKey(key);
            _cache.Set(key, item, expiration);
            return Task.FromResult(0);
        }
    }
}