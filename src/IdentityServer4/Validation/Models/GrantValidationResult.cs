// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Models the result of custom grant validation.
    /// </summary>
    public class GrantValidationResult : ValidationResult
    {
        /// <summary>
        /// Gets or sets the principal which represents the result of the validation.
        /// </summary>
        /// <value>
        /// The principal.
        /// </value>
        public ClaimsPrincipal Subject { get; set; }

        /// <summary>
        /// Custom fields for the token response
        /// </summary>
        public Dictionary<string, object> CustomResponse { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GrantValidationResult"/> class with a given principal.
        /// Warning: the principal needs to include the required claims - it is recommended to use the other constructor that does validation.
        /// </summary>
        public GrantValidationResult(ClaimsPrincipal principal, Dictionary<string, object> customResponse = null)
        {
            IsError = false;

            // TODO: more checks on claims (amr, etc...)
            Subject = principal;
            CustomResponse = customResponse;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrantValidationResult"/> class with an error and description.
        /// </summary>
        /// <param name="error">The error.</param>
        /// /// <param name="errorDescription">The error description.</param>
        public GrantValidationResult(TokenRequestErrors error, string errorDescription = null, Dictionary<string, object> customResponse = null)
        {
            Error = ConvertTokenErrorEnumToString(error);
            ErrorDescription = errorDescription;
            CustomResponse = customResponse;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrantValidationResult"/> class.
        /// </summary>
        /// <param name="subject">The subject claim used to uniquely identifier the user.</param>
        /// <param name="authenticationMethod">The authentication method which describes the custom grant type.</param>
        /// <param name="claims">Additional claims that will be maintained in the principal.</param>
        /// <param name="identityProvider">The identity provider.</param>
        public GrantValidationResult(
            string subject, 
            string authenticationMethod,
            IEnumerable<Claim> claims = null,
            string identityProvider = IdentityServerConstants.LocalIdentityProvider,
            Dictionary<string, object> customResponse = null)
        {
            IsError = false;

            var resultClaims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, subject),
                new Claim(JwtClaimTypes.AuthenticationMethod, authenticationMethod),
                new Claim(JwtClaimTypes.IdentityProvider, identityProvider),
                new Claim(JwtClaimTypes.AuthenticationTime, DateTimeHelper.UtcNow.ToEpochTime().ToString(), ClaimValueTypes.Integer)
            };

            if (!claims.IsNullOrEmpty())
            {
                resultClaims.AddRange(claims);
            }

            var id = new ClaimsIdentity(authenticationMethod);
            id.AddClaims(resultClaims.Distinct(new ClaimComparer()));

            Subject = new ClaimsPrincipal(id);
            CustomResponse = customResponse;
        }

        private string ConvertTokenErrorEnumToString(TokenRequestErrors error)
        {
            if (error == TokenRequestErrors.InvalidClient) return OidcConstants.TokenErrors.InvalidClient;
            if (error == TokenRequestErrors.InvalidGrant) return OidcConstants.TokenErrors.InvalidGrant;
            if (error == TokenRequestErrors.InvalidRequest) return OidcConstants.TokenErrors.InvalidRequest;
            if (error == TokenRequestErrors.InvalidScope) return OidcConstants.TokenErrors.InvalidScope;
            if (error == TokenRequestErrors.UnauthorizedClient) return OidcConstants.TokenErrors.UnauthorizedClient;
            if (error == TokenRequestErrors.UnsupportedGrantType) return OidcConstants.TokenErrors.UnsupportedGrantType;

            throw new InvalidOperationException("invalid token error");
        }
    }
}