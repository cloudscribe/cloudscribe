// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// A service for parsing secrets found on the environment
    /// </summary>
    public interface ISecretParser
    {
        /// <summary>
        /// Tries to find a secret on the environment that can be used for authentication
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>A parsed secret</returns>
        Task<ParsedSecret> ParseAsync(HttpContext context);

        /// <summary>
        /// Returns the authentication method name that this parser implements
        /// </summary>
        string AuthenticationMethod { get; }
    }
}