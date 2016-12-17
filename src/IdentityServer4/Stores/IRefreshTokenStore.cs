// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Threading.Tasks;

namespace IdentityServer4.Stores
{
    /// <summary>
    /// Implements refresh token storage
    /// </summary>
    public interface IRefreshTokenStore
    {
        Task StoreRefreshTokenAsync(string handle, RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenAsync(string refreshTokenHandle);
        Task RemoveRefreshTokenAsync(string refreshTokenHandle);
        Task RemoveRefreshTokensAsync(string subjectId, string clientId);
    }
}