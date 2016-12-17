// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores.Serialization;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Stores
{
    /// <summary>
    /// Default refresh token store.
    /// </summary>
    /// <seealso cref="IdentityServer4.Stores.DefaultGrantStore{IdentityServer4.Models.RefreshToken}" />
    /// <seealso cref="IdentityServer4.Stores.IRefreshTokenStore" />
    public class DefaultRefreshTokenStore : DefaultGrantStore<RefreshToken>, IRefreshTokenStore
    {
        public DefaultRefreshTokenStore(
            IPersistedGrantStore store, 
            PersistentGrantSerializer serializer, 
            ILogger<DefaultRefreshTokenStore> logger) 
            : base(Constants.PersistedGrantTypes.RefreshToken, store, serializer, logger)
        {
        }

        public Task StoreRefreshTokenAsync(string handle, RefreshToken refreshToken)
        {
            return StoreItemAsync(handle, refreshToken, refreshToken.ClientId, refreshToken.SubjectId, refreshToken.CreationTime, refreshToken.Lifetime);
        }

        public Task<RefreshToken> GetRefreshTokenAsync(string refreshTokenHandle)
        {
            return GetItemAsync(refreshTokenHandle);
        }

        public Task RemoveRefreshTokenAsync(string refreshTokenHandle)
        {
            return RemoveItemAsync(refreshTokenHandle);
        }

        public Task RemoveRefreshTokensAsync(string subjectId, string clientId)
        {
            return RemoveAllAsync(subjectId, clientId);
        }
    }
}