// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Service for validating a received secret against a stored secret
    /// </summary>
    public interface ISecretValidator
    {
        /// <summary>
        /// Validates a secret
        /// </summary>
        /// <param name="secrets">The stored secrets.</param>
        /// <param name="parsedSecret">The received secret.</param>
        /// <returns>A validation result</returns>
        Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret);
    }
}