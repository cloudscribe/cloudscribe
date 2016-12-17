// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace IdentityServer4.ResponseHandling
{
    public interface ITokenResponseGenerator
    {
        Task<TokenResponse> ProcessAsync(TokenRequestValidationResult validationResult);
    }
}