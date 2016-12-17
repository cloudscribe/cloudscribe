// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Services.Default
{
    /// <summary>
    /// Default token creation service
    /// </summary>
    public class DefaultTokenCreationService : ITokenCreationService
    {
        private readonly IKeyMaterialService _keys;
        private readonly ILogger _logger;

        public DefaultTokenCreationService(IKeyMaterialService keys, ILogger<DefaultTokenCreationService> logger)
        {
            _keys = keys;
            _logger = logger;
        }

        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        /// A protected and serialized security token
        /// </returns>
        public virtual async Task<string> CreateTokenAsync(Token token)
        {
            var header = await CreateHeaderAsync(token);
            var payload = await CreatePayloadAsync(token);

            return await CreateJwtAsync(new JwtSecurityToken(header, payload));
        }

        /// <summary>
        /// Creates the JWT header
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="credential">The credentials.</param>
        /// <returns>The JWT header</returns>
        protected virtual async Task<JwtHeader> CreateHeaderAsync(Token token)
        {
            var credential = await _keys.GetSigningCredentialsAsync();

            if (credential == null)
            {
                throw new InvalidOperationException("No signing credential is configured. Can't create JWT token");
            }

            var header = new JwtHeader(credential);

            // emit x5t claim for backwards compatibility with v4 of MS JWT library
            var x509key = credential.Key as X509SecurityKey;
            if (x509key != null)
            {
                var cert = x509key.Certificate;
                if (DateTime.UtcNow > cert.NotAfter)
                {
                    _logger.LogWarning("Certificate {subjectName} has expired on {expiration}", cert.Subject, cert.NotAfter.ToString());
                }

                header.Add("x5t", Base64Url.Encode(cert.GetCertHash()));
            }

            return header;
        }

        /// <summary>
        /// Creates the JWT payload
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The JWT payload</returns>
        protected virtual Task<JwtPayload> CreatePayloadAsync(Token token)
        {
            var payload = new JwtPayload(
                token.Issuer,
                null,
                null,
                DateTimeHelper.UtcNow,
                DateTimeHelper.UtcNow.AddSeconds(token.Lifetime));

            foreach (var aud in token.Audiences)
            {
                payload.AddClaim(new Claim(JwtClaimTypes.Audience, aud));
            }

            var amrClaims = token.Claims.Where(x => x.Type == JwtClaimTypes.AuthenticationMethod);
            var scopeClaims = token.Claims.Where(x => x.Type == JwtClaimTypes.Scope);
            var jsonClaims = token.Claims.Where(x => x.ValueType == IdentityServerConstants.ClaimValueTypes.Json);

            var normalClaims = token.Claims
                .Except(amrClaims)
                .Except(jsonClaims)
                .Except(scopeClaims);

            payload.AddClaims(normalClaims);

            // scope claims
            if (!scopeClaims.IsNullOrEmpty())
            {
                var scopeValues = scopeClaims.Select(x => x.Value).ToArray();
                payload.Add(JwtClaimTypes.Scope, scopeValues);
            }
            
            // amr claims
            if (!amrClaims.IsNullOrEmpty())
            {
                var amrValues = amrClaims.Select(x => x.Value).Distinct().ToArray();
                payload.Add(JwtClaimTypes.AuthenticationMethod, amrValues);
            }

            // deal with json types
            // calling ToArray() to trigger JSON parsing once and so later 
            // collection identity comparisons work for the anonymous type
            var jsonTokens = jsonClaims.Select(x => new { x.Type, JsonValue = JRaw.Parse(x.Value) }).ToArray();

            var jsonObjects = jsonTokens.Where(x => x.JsonValue.Type == JTokenType.Object).ToArray();
            var jsonObjectGroups = jsonObjects.GroupBy(x => x.Type).ToArray();
            foreach (var group in jsonObjectGroups)
            {
                if (payload.ContainsKey(group.Key))
                {
                    throw new Exception(String.Format("Can't add two claims where one is a JSON object and the other is not a JSON object ({0})", group.Key));
                }

                if (group.Skip(1).Any())
                {
                    // add as array
                    payload.Add(group.Key, group.Select(x => x.JsonValue).ToArray());
                }
                else
                {
                    // add just one
                    payload.Add(group.Key, group.First().JsonValue);
                }
            }

            var jsonArrays = jsonTokens.Where(x => x.JsonValue.Type == JTokenType.Array).ToArray();
            var jsonArrayGroups = jsonArrays.GroupBy(x => x.Type).ToArray();
            foreach (var group in jsonArrayGroups)
            {
                if (payload.ContainsKey(group.Key))
                {
                    throw new Exception(String.Format("Can't add two claims where one is a JSON array and the other is not a JSON array ({0})", group.Key));
                }

                List<JToken> newArr = new List<JToken>();
                foreach (var arrays in group)
                {
                    var arr = (JArray)arrays.JsonValue;
                    newArr.AddRange(arr);
                }

                // add just one array for the group/key/claim type
                payload.Add(group.Key, newArr.ToArray());
            }

            var unsupportedJsonTokens = jsonTokens.Except(jsonObjects).Except(jsonArrays);
            var unsupportedJsonClaimTypes = unsupportedJsonTokens.Select(x => x.Type).Distinct();
            if (unsupportedJsonClaimTypes.Any())
            {
                throw new Exception(String.Format("Unsupported JSON type for claim types: {0}", unsupportedJsonClaimTypes.Aggregate((x, y) => x + ", " + y)));
            }

            return Task.FromResult(payload);
        }

        /// <summary>
        /// Applies the signature to the JWT
        /// </summary>
        /// <param name="jwt">The JWT object.</param>
        /// <returns>The signed JWT</returns>
        protected virtual Task<string> CreateJwtAsync(JwtSecurityToken jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            return Task.FromResult(handler.WriteToken(jwt));
        }
    }
}