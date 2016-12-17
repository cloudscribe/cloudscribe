// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Allows inserting custom validation logic into authorize and token requests
    /// </summary>
    public interface ICustomTokenRequestValidator
    {
        /// <summary>
        /// Custom validation logic for the token request.
        /// </summary>
        /// <param name="validationResult">The validation model.</param>
        /// <returns>The validation result</returns>
        Task ValidateAsync(CustomTokenRequestValidationContext context);
    }
}