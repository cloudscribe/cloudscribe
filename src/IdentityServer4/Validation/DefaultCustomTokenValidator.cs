// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Default custom token validator
    /// </summary>
    public class DefaultCustomTokenValidator : ICustomTokenValidator
    {
        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// The user service
        /// </summary>
        protected readonly IProfileService Profile;

        /// <summary>
        /// The client store
        /// </summary>
        protected readonly IClientStore Clients;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCustomTokenValidator"/> class.
        /// </summary>
        /// <param name="profile">The profile service</param>
        /// <param name="clients">The client store.</param>
        /// <param name="logger">The logger</param>
        public DefaultCustomTokenValidator(IProfileService profile, IClientStore clients, ILogger<DefaultCustomTokenValidator> logger)
        {
            Logger = logger;
            Profile = profile;
            Clients = clients;
        }

        /// <summary>
        /// Custom validation logic for access tokens.
        /// </summary>
        /// <param name="result">The validation result so far.</param>
        /// <returns>
        /// The validation result
        /// </returns>
        public virtual async Task<TokenValidationResult> ValidateAccessTokenAsync(TokenValidationResult result)
        {
            if (result.IsError)
            {
                return result;
            }

            // make sure user is still active (if sub claim is present)
            var subClaim = result.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject);
            if (subClaim != null)
            {
                var principal = Principal.Create("tokenvalidator", result.Claims.ToArray());

                if (result.ReferenceTokenId.IsPresent())
                {
                    principal.Identities.First().AddClaim(new Claim(JwtClaimTypes.ReferenceTokenId, result.ReferenceTokenId));
                }

                var isActiveCtx = new IsActiveContext(principal, result.Client, IdentityServerConstants.ProfileIsActiveCallers.AccessTokenValidation);
                await Profile.IsActiveAsync(isActiveCtx);
                
                if (isActiveCtx.IsActive == false)
                {
                    Logger.LogError("User marked as not active: {subject}", subClaim.Value);

                    result.IsError = true;
                    result.Error = OidcConstants.ProtectedResourceErrors.InvalidToken;
                    result.Claims = null;

                    return result;
                }
            }

            // make sure client is still active (if client_id claim is present)
            var clientClaim = result.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.ClientId);
            if (clientClaim != null)
            {
                var client = await Clients.FindEnabledClientByIdAsync(clientClaim.Value);
                if (client == null)
                {
                    Logger.LogError("Client deleted or disabled: {clientId}", clientClaim.Value);

                    result.IsError = true;
                    result.Error = OidcConstants.ProtectedResourceErrors.InvalidToken;
                    result.Claims = null;

                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Custom validation logic for identity tokens.
        /// </summary>
        /// <param name="result">The validation result so far.</param>
        /// <returns>
        /// The validation result
        /// </returns>
        public virtual async Task<TokenValidationResult> ValidateIdentityTokenAsync(TokenValidationResult result)
        {
            // make sure user is still active (if sub claim is present)
            var subClaim = result.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject);
            if (subClaim != null)
            {
                var principal = Principal.Create("tokenvalidator", result.Claims.ToArray());

                var isActiveCtx = new IsActiveContext(principal, result.Client, IdentityServerConstants.ProfileIsActiveCallers.IdentityTokenValidation);
                await Profile.IsActiveAsync(isActiveCtx);
                
                if (isActiveCtx.IsActive == false)
                {
                    Logger.LogError("User marked as not active: {subject}", subClaim.Value);

                    result.IsError = true;
                    result.Error = OidcConstants.ProtectedResourceErrors.InvalidToken;
                    result.Claims = null;

                    return result;
                }
            }

            return result;
        }
    }
}