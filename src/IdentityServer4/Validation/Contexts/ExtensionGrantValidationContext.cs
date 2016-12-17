// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Class describing the resource owner password validation request
    /// </summary>
    public class ExtensionGrantValidationContext
    {
        public ValidatedTokenRequest Request { get; set; }
        public GrantValidationResult Result { get; set; } = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
    }
}