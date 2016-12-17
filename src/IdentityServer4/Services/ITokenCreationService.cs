// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Threading.Tasks;

namespace IdentityServer4.Services
{
    /// <summary>
    /// Logic for signing security tokens
    /// </summary>
    public interface ITokenCreationService
    {
        /// <summary>
        /// Signs the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>A protected and serialized security token</returns>
        Task<string> CreateTokenAsync(Token token);
    }
}