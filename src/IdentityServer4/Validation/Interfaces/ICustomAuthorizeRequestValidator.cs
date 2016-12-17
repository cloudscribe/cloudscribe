// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Allows inserting custom validation logic into authorize and token requests
    /// </summary>
    public interface ICustomAuthorizeRequestValidator
    {
        /// <summary>
        /// Custom validation logic for the authorize request.
        /// </summary>
        /// <param name="request">The validated request.</param>
        /// <returns>The validation result</returns>
        // todo
        Task<AuthorizeRequestValidationResult> ValidateAsync(ValidatedAuthorizeRequest request);
    }
}