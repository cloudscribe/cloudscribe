// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Default custom request validator
    /// </summary>
    public class DefaultCustomTokenRequestValidator : ICustomTokenRequestValidator
    {
        public Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            return Task.FromResult(0);
        }
    }
}