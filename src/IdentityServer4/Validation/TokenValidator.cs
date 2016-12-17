// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Logging;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using IdentityServer4.Stores;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.Validation
{
    public class TokenValidator : ITokenValidator
    {
        private readonly ILogger _logger;
        private readonly IdentityServerOptions _options;
        private readonly IHttpContextAccessor _context;
        private readonly IReferenceTokenStore _referenceTokenStore;
        private readonly ICustomTokenValidator _customValidator;
        private readonly IClientStore _clients;
        private readonly IKeyMaterialService _keys;

        private readonly TokenValidationLog _log;
        
        public TokenValidator(IdentityServerOptions options, IHttpContextAccessor context, IClientStore clients, IReferenceTokenStore referenceTokenStore, ICustomTokenValidator customValidator, IKeyMaterialService keys, ILogger<TokenValidator> logger)
        {
            _options = options;
            _context = context;
            _clients = clients;
            _referenceTokenStore = referenceTokenStore;
            _customValidator = customValidator;
            _keys = keys;
            _logger = logger;

            _log = new TokenValidationLog();
        }
        
        public virtual async Task<TokenValidationResult> ValidateIdentityTokenAsync(string token, string clientId = null, bool validateLifetime = true)
        {
            _logger.LogDebug("Start identity token validation");

            if (token.Length > _options.InputLengthRestrictions.Jwt)
            {
                _logger.LogError("JWT too long");
                return Invalid(OidcConstants.ProtectedResourceErrors.InvalidToken);
            }

            if (clientId.IsMissing())
            {
                clientId = GetClientIdFromJwt(token);

                if (clientId.IsMissing())
                {
                    _logger.LogError("No clientId supplied, can't find id in identity token.");
                    return Invalid(OidcConstants.ProtectedResourceErrors.InvalidToken);
                }
            }

            _log.ClientId = clientId;
            _log.ValidateLifetime = validateLifetime;

            var client = await _clients.FindEnabledClientByIdAsync(clientId);
            if (client == null)
            {
                _logger.LogError("Unknown or diabled client: {clientId}.", clientId);
                return Invalid(OidcConstants.ProtectedResourceErrors.InvalidToken);
            }

            _log.ClientName = client.ClientName;
            _logger.LogDebug("Client found: {clientId} / {clientName}", client.ClientId, client.ClientName);

            var keys = await _keys.GetValidationKeysAsync();
            var result = await ValidateJwtAsync(token, clientId, keys, validateLifetime);

            result.Client = client;

            if (result.IsError)
            {
                LogError("Error validating JWT");
                return result;
            }

            _log.Claims = result.Claims.ToClaimsDictionary();

            _logger.LogDebug("Calling into custom token validator: {type}", _customValidator.GetType().FullName);
            var customResult = await _customValidator.ValidateIdentityTokenAsync(result);

            if (customResult.IsError)
            {
                LogError("Custom validator failed: " + (customResult.Error ?? "unknown"));
                return customResult;
            }

            _log.Claims = customResult.Claims.ToClaimsDictionary();

            LogSuccess();
            return customResult;
        }

        public virtual async Task<TokenValidationResult> ValidateAccessTokenAsync(string token, string expectedScope = null)
        {
            _logger.LogTrace("Start access token validation");

            _log.ExpectedScope = expectedScope;
            _log.ValidateLifetime = true;

            TokenValidationResult result;

            if (token.Contains("."))
            {
                if (token.Length > _options.InputLengthRestrictions.Jwt)
                {
                    _logger.LogError("JWT too long");

                    return new TokenValidationResult
                    {
                        IsError = true,
                        Error = OidcConstants.ProtectedResourceErrors.InvalidToken,
                        ErrorDescription = "Token too long"
                    };
                }

                _log.AccessTokenType = AccessTokenType.Jwt.ToString();
                result = await ValidateJwtAsync(
                    token,
                    string.Format(Constants.AccessTokenAudience, _context.HttpContext.GetIssuerUri().EnsureTrailingSlash()),
                    await _keys.GetValidationKeysAsync());
            }
            else
            {
                if (token.Length > _options.InputLengthRestrictions.TokenHandle)
                {
                    _logger.LogError("token handle too long");

                    return new TokenValidationResult
                    {
                        IsError = true,
                        Error = OidcConstants.ProtectedResourceErrors.InvalidToken,
                        ErrorDescription = "Token too long"
                    };
                }

                _log.AccessTokenType = AccessTokenType.Reference.ToString();
                result = await ValidateReferenceAccessTokenAsync(token);
            }

            _log.Claims = result.Claims.ToClaimsDictionary();

            if (result.IsError)
            {
                return result;
            }

            if (expectedScope.IsPresent())
            {
                var scope = result.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Scope && c.Value == expectedScope);
                if (scope == null)
                {
                    LogError(string.Format("Checking for expected scope {0} failed", expectedScope));
                    return Invalid(OidcConstants.ProtectedResourceErrors.InsufficientScope);
                }
            }

            var customResult = await _customValidator.ValidateAccessTokenAsync(result);

            if (customResult.IsError)
            {
                LogError("Custom validator failed: " + (customResult.Error ?? "unknown"));
                return customResult;
            }

            // add claims again after custom validation
            _log.Claims = customResult.Claims.ToClaimsDictionary();

            LogSuccess();
            return customResult;
        }

        private async Task<TokenValidationResult> ValidateJwtAsync(string jwt, string audience, IEnumerable<SecurityKey> validationKeys, bool validateLifetime = true)
        {
            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();

            var parameters = new TokenValidationParameters
            {
                ValidIssuer = _context.HttpContext.GetIssuerUri(),
                IssuerSigningKeys = validationKeys,
                ValidateLifetime = validateLifetime,
                ValidAudience = audience
            };

            try
            {
                SecurityToken jwtToken;
                var id = handler.ValidateToken(jwt, parameters, out jwtToken);

                // if access token contains an ID, log it
                var jwtId = id.FindFirst(JwtClaimTypes.JwtId);
                if (jwtId != null)
                {
                    _log.JwtId = jwtId.Value;
                }

                // load the client that belongs to the client_id claim
                Client client = null;
                var clientId = id.FindFirst(JwtClaimTypes.ClientId);
                if (clientId != null)
                {
                    client = await _clients.FindEnabledClientByIdAsync(clientId.Value);
                    if (client == null)
                    {
                        throw new InvalidOperationException("Client does not exist anymore.");
                    }
                }

                return new TokenValidationResult
                {
                    IsError = false,

                    Claims = id.Claims,
                    Client = client,
                    Jwt = jwt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("JWT token validation error: {exception}", ex.ToString());
                return Invalid(OidcConstants.ProtectedResourceErrors.InvalidToken);
            }
        }

        private async Task<TokenValidationResult> ValidateReferenceAccessTokenAsync(string tokenHandle)
        {
            _log.TokenHandle = tokenHandle;
            var token = await _referenceTokenStore.GetReferenceTokenAsync(tokenHandle);

            if (token == null)
            {
                LogError("Token handle not found in token handle store.");
                return Invalid(OidcConstants.ProtectedResourceErrors.InvalidToken);
            }

            // TODO: review
            //if (token.Type != OidcConstants.TokenTypes.AccessToken)
            //{
            //    LogError("Token handle does not resolve to an access token - but instead to: " + token.Type);

            //    await _tokenHandles.RemoveAsync(tokenHandle);
            //    return Invalid(OidcConstants.ProtectedResourceErrors.InvalidToken);
            //}

            if (DateTimeHelper.UtcNow >= token.CreationTime.AddSeconds(token.Lifetime))
            {
                LogError("Token expired.");

                await _referenceTokenStore.RemoveReferenceTokenAsync(tokenHandle);
                return Invalid(OidcConstants.ProtectedResourceErrors.ExpiredToken);
            }

            // load the client that is defined in the token
            Client client = null;
            if (token.ClientId != null)
            {
                client = await _clients.FindEnabledClientByIdAsync(token.ClientId);
            }

            if (client == null)
            {
                LogError($"Client deleted or disabled: {token.ClientId}");
                return Invalid(OidcConstants.ProtectedResourceErrors.InvalidToken);
            }

            return new TokenValidationResult
            {
                IsError = false,

                Client = client,
                Claims = ReferenceTokenToClaims(token),
                ReferenceToken = token,
                ReferenceTokenId = tokenHandle
            };
        }

        private IEnumerable<Claim> ReferenceTokenToClaims(Token token)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Issuer, token.Issuer),
                new Claim(JwtClaimTypes.NotBefore, token.CreationTime.ToEpochTime().ToString(), ClaimValueTypes.Integer),
                new Claim(JwtClaimTypes.Expiration, token.CreationTime.AddSeconds(token.Lifetime).ToEpochTime().ToString(), ClaimValueTypes.Integer)
            };

            foreach (var aud in token.Audiences)
            {
                claims.Add(new Claim(JwtClaimTypes.Audience, aud));
            }

            claims.AddRange(token.Claims);
            return claims;
        }

        private string GetClientIdFromJwt(string token)
        {
            try
            {
                var jwt = new JwtSecurityToken(token);
                var clientId = jwt.Audiences.FirstOrDefault();

                return clientId;
            }
            catch (Exception ex)
            {
                _logger.LogError("Malformed JWT token: {exception}", ex.ToString());
                return null;
            }
        }

        private TokenValidationResult Invalid(string error)
        {
            return new TokenValidationResult
            {
                IsError = true,
                Error = error
            };
        }

        private void LogError(string message)
        {
            _logger.LogError(message +"\n{logMessage}", _log);
        }

        private void LogSuccess()
        {
            _logger.LogDebug("Token validation success\n{logMessage}", _log);
        }
    }
}