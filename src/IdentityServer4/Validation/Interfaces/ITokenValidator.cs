// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    public interface ITokenValidator
    {
        Task<TokenValidationResult> ValidateAccessTokenAsync(string token, string expectedScope = null);
        Task<TokenValidationResult> ValidateIdentityTokenAsync(string token, string clientId = null, bool validateLifetime = true);
    }
}