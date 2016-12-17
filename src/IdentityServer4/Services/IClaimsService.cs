// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Services
{
    /// <summary>
    /// The claims service is responsible for determining which claims to include in tokens
    /// </summary>
    public interface IClaimsService
    {
        /// <summary>
        /// Returns claims for an identity token
        /// </summary>
        /// <param name="subject">The subject</param>
        /// <param name="client">The client</param>
        /// <param name="scopes">The requested scopes</param>
        /// <param name="includeAllIdentityClaims">Specifies if all claims should be included in the token, or if the userinfo endpoint can be used to retrieve them</param>
        /// <param name="request">The raw request</param>
        /// <returns>Claims for the identity token</returns>
        Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ClaimsPrincipal subject, Client client, Resources resources, bool includeAllIdentityClaims, ValidatedRequest request);

        /// <summary>
        /// Returns claims for an access token.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="client">The client.</param>
        /// <param name="scopes">The requested scopes.</param>
        /// <param name="request">The raw request.</param>
        /// <returns>Claims for the access token</returns>
        Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject, Client client, Resources resources, ValidatedRequest request);
    }
}