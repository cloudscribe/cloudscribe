// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Services
{
    /// <summary>
    /// Implements refresh token creation and validation
    /// </summary>
    public interface IRefreshTokenService
    {
        /// <summary>
        /// Creates the refresh token.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="client">The client.</param>
        /// <returns>
        /// The refresh token handle
        /// </returns>
        Task<string> CreateRefreshTokenAsync(ClaimsPrincipal subject, Token accessToken, Client client);

        /// <summary>
        /// Updates the refresh token.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="client">The client.</param>
        /// <returns>The refresh token handle</returns>
        Task<string> UpdateRefreshTokenAsync(string handle, RefreshToken refreshToken, Client client);
    }
}