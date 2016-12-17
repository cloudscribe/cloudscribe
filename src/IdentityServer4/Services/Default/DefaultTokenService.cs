// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Services.Default
{
    /// <summary>
    /// Default token service
    /// </summary>
    public class DefaultTokenService : ITokenService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        protected readonly IHttpContextAccessor _context;

        /// <summary>
        /// The claims provider
        /// </summary>
        protected readonly IClaimsService _claimsProvider;

        /// <summary>
        /// The reference token store
        /// </summary>
        protected readonly IReferenceTokenStore _referenceTokenStore;

        /// <summary>
        /// The signing service
        /// </summary>
        protected readonly ITokenCreationService _creationService;

        /// <summary>
        /// The events service
        /// </summary>
        protected readonly IEventService _events;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTokenService" /> class. This overloaded constructor is deprecated and will be removed in 3.0.0.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="claimsProvider">The claims provider.</param>
        /// <param name="referenceTokenStore">The reference token store.</param>
        /// <param name="creationService">The signing service.</param>
        /// <param name="events">The events service.</param>
        /// <param name="logger">The logger.</param>
        public DefaultTokenService(IHttpContextAccessor context, IClaimsService claimsProvider, IReferenceTokenStore referenceTokenStore, ITokenCreationService creationService, IEventService events, ILogger<DefaultTokenService> logger)
        {
            _logger = logger;
            _context = context;
            _claimsProvider = claimsProvider;
            _referenceTokenStore = referenceTokenStore;
            _creationService = creationService;
            _events = events;
        }

        /// <summary>
        /// Creates an identity token.
        /// </summary>
        /// <param name="request">The token creation request.</param>
        /// <returns>
        /// An identity token
        /// </returns>
        public virtual async Task<Token> CreateIdentityTokenAsync(TokenCreationRequest request)
        {
            _logger.LogTrace("Creating identity token");
            request.Validate();

            // host provided claims
            var claims = new List<Claim>();

            // if nonce was sent, must be mirrored in id token
            if (request.Nonce.IsPresent())
            {
                claims.Add(new Claim(JwtClaimTypes.Nonce, request.Nonce));
            }

            // add iat claim
            claims.Add(new Claim(JwtClaimTypes.IssuedAt, DateTimeHelper.UtcNow.ToEpochTime().ToString(), ClaimValueTypes.Integer));

            // add at_hash claim
            if (request.AccessTokenToHash.IsPresent())
            {
                claims.Add(new Claim(JwtClaimTypes.AccessTokenHash, HashAdditionalData(request.AccessTokenToHash)));
            }

            // add c_hash claim
            if (request.AuthorizationCodeToHash.IsPresent())
            {
                claims.Add(new Claim(JwtClaimTypes.AuthorizationCodeHash, HashAdditionalData(request.AuthorizationCodeToHash)));
            }

            // add sid if present
            if (request.ValidatedRequest.SessionId.IsPresent())
            {
                claims.Add(new Claim(JwtClaimTypes.SessionId, request.ValidatedRequest.SessionId));
            }

            claims.AddRange(await _claimsProvider.GetIdentityTokenClaimsAsync(
                request.Subject,
                request.Client,
                request.Resources,
                request.IncludeAllIdentityClaims,
                request.ValidatedRequest));

            var issuer = _context.HttpContext.GetIssuerUri();

            var token = new Token(OidcConstants.TokenTypes.IdentityToken)
            {
                Audiences = { request.Client.ClientId },
                Issuer = issuer,
                Lifetime = request.Client.IdentityTokenLifetime,
                Claims = claims.Distinct(new ClaimComparer()).ToList(),
                ClientId = request.Client.ClientId,
                AccessTokenType = request.Client.AccessTokenType
            };

            return token;
        }

        /// <summary>
        /// Creates an access token.
        /// </summary>
        /// <param name="request">The token creation request.</param>
        /// <returns>
        /// An access token
        /// </returns>
        public virtual async Task<Token> CreateAccessTokenAsync(TokenCreationRequest request)
        {
            _logger.LogTrace("Creating access token");
            request.Validate();

            var claims = new List<Claim>();
            claims.AddRange(await _claimsProvider.GetAccessTokenClaimsAsync(
                request.Subject,
                request.Client,
                request.Resources,
                request.ValidatedRequest));

            if (request.Client.IncludeJwtId)
            {
                claims.Add(new Claim(JwtClaimTypes.JwtId, CryptoRandom.CreateUniqueId()));
            }

            var issuer = _context.HttpContext.GetIssuerUri();
            var token = new Token(OidcConstants.TokenTypes.AccessToken)
            {
                Audiences = { string.Format(Constants.AccessTokenAudience, issuer.EnsureTrailingSlash()) },
                Issuer = issuer,
                Lifetime = request.Client.AccessTokenLifetime,
                Claims = claims.Distinct(new ClaimComparer()).ToList(),
                ClientId = request.Client.ClientId,
                AccessTokenType = request.Client.AccessTokenType
            };

            foreach(var api in request.Resources.ApiResources)
            {
                if (api.Name.IsPresent())
                {
                    token.Audiences.Add(api.Name);
                }
            }

            return token;
        }

        /// <summary>
        /// Creates a serialized and protected security token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        /// A security token in serialized form
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Invalid token type.</exception>
        public virtual async Task<string> CreateSecurityTokenAsync(Token token)
        {
            string tokenResult;

            if (token.Type == OidcConstants.TokenTypes.AccessToken)
            {
                if (token.AccessTokenType == AccessTokenType.Jwt)
                {
                    _logger.LogTrace("Creating JWT access token");

                    tokenResult = await _creationService.CreateTokenAsync(token);
                }
                else
                {
                    _logger.LogTrace("Creating reference access token");

                    var handle = CryptoRandom.CreateUniqueId();
                    await _referenceTokenStore.StoreReferenceTokenAsync(handle, token);

                    tokenResult = handle;
                }
            }
            else if (token.Type == OidcConstants.TokenTypes.IdentityToken)
            {
                _logger.LogTrace("Creating JWT identity token");

                tokenResult = await _creationService.CreateTokenAsync(token);
            }
            else
            {
                throw new InvalidOperationException("Invalid token type.");
            }

            await _events.RaiseTokenIssuedEventAsync(token, tokenResult);
            return tokenResult;
        }

        /// <summary>
        /// Hashes an additional data (e.g. for c_hash or at_hash).
        /// </summary>
        /// <param name="tokenToHash">The token to hash.</param>
        /// <returns></returns>
        protected virtual string HashAdditionalData(string tokenToHash)
        {
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(tokenToHash));

                var leftPart = new byte[16];
                Array.Copy(hash, leftPart, 16);

                return Base64Url.Encode(leftPart);
            }
        }
    }
}