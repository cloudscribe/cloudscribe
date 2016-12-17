// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Default custom request validator
    /// </summary>
    public class DefaultCustomAuthorizeRequestValidator : ICustomAuthorizeRequestValidator
    {
        /// <summary>
        /// Custom validation logic for the authorize request.
        /// </summary>
        /// <param name="request">The validated request.</param>
        /// <returns>
        /// The validation result
        /// </returns>
        // todo - maybe split up in two interfaces - semantics for tokens have changed because of custom response fields
        public Task<AuthorizeRequestValidationResult> ValidateAsync(ValidatedAuthorizeRequest request)
        {
            return Task.FromResult(new AuthorizeRequestValidationResult
            {
                IsError = false
            });
        }
    }
}