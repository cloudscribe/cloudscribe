// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Validates a secret based on RS256 signed JWT token
    /// </summary>
    public class PrivateKeyJwtSecretValidator : ISecretValidator
    {
        private readonly ILogger _logger;
        private readonly string _audienceUri;

        /// <summary>
        /// Instantiates an instance of private_key_jwt secret validator
        /// </summary>
        public PrivateKeyJwtSecretValidator(IHttpContextAccessor contextAccessor, ILogger<PrivateKeyJwtSecretValidator> logger)
        {
            _audienceUri = string.Concat(contextAccessor.HttpContext.GetIssuerUri().EnsureTrailingSlash(), Constants.ProtocolRoutePaths.Token);
            _logger = logger;
        }

        /// <summary>
        /// Validates a secret
        /// </summary>
        /// <param name="secrets">The stored secrets.</param>
        /// <param name="parsedSecret">The received secret.</param>
        /// <returns>
        /// A validation result
        /// </returns>
        /// <exception cref="System.ArgumentException">ParsedSecret.Credential is not a JWT token</exception>
        public Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret)
        {
            var fail = Task.FromResult(new SecretValidationResult { Success = false });
            var success = Task.FromResult(new SecretValidationResult { Success = true });

            if (parsedSecret.Type != IdentityServerConstants.ParsedSecretTypes.JwtBearer)
            {
                return fail;
            }

            var jwtTokenString = parsedSecret.Credential as string;

            if (jwtTokenString == null)
            {
                _logger.LogError("ParsedSecret.Credential is not a string.");
                return fail;
            }

            var enumeratedSecrets = secrets.ToList().AsReadOnly();

            List<SecurityKey> trustedKeys; 
            try
            {
                trustedKeys = GetTrustedKeys(enumeratedSecrets, jwtTokenString);
            }
            catch (Exception e)
            {
                _logger.LogError("Could not parse assertion as JWT token: {e}", e);
                return fail;
            }

            if (!trustedKeys.Any())
            {
                _logger.LogError("There are no certificates available to validate client assertion.");
                return fail;
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKeys = trustedKeys,
                ValidateIssuerSigningKey = true,

                ValidIssuer = parsedSecret.Id,
                ValidateIssuer = true,

                ValidAudience = _audienceUri,
                ValidateAudience = true,

                RequireSignedTokens = true,
                RequireExpirationTime = true
            };
            try
            {
                SecurityToken token;
                var handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(jwtTokenString, tokenValidationParameters, out token);

                var jwtToken = (JwtSecurityToken)token;

                if (jwtToken.Subject != jwtToken.Issuer)
                {
                    _logger.LogError("Both 'sub' and 'iss' in the client assertion token must have a value of client_id.");
                    return fail;
                }

                return success;
            }
            catch (Exception e)
            {
                _logger.LogError("JWT token validation error: " + e.Message);
                return fail;
            }
        }

        private List<SecurityKey> GetTrustedKeys(IReadOnlyCollection<Secret> secrets, string jwtTokenString)
        {
            var token = new JwtSecurityToken(jwtTokenString);

            var trustedKeys = GetAllTrustedCertificates(secrets)
                                .Select(c => (SecurityKey)new X509SecurityKey(c))
                                .ToList();

            if (!trustedKeys.Any()
                && secrets.Any(s => s.Type == IdentityServerConstants.SecretTypes.X509CertificateThumbprint))
            {
                _logger.LogWarning("Cannot validate client assertion token using only thumbprint. Client must be configured with X509CertificateBase64 secret.");
            }

            return trustedKeys;
        }

        private List<X509Certificate2> GetAllTrustedCertificates(IEnumerable<Secret> secrets)
        {
            return secrets
                .Where(s => s.Type == IdentityServerConstants.SecretTypes.X509CertificateBase64)
                .Select(s => GetCertificateFromString(s.Value))
                .Where(c => c != null)
                .ToList();
        }

        private X509Certificate2 GetCertificateFromString(string value)
        {
            try
            {
                return new X509Certificate2(Convert.FromBase64String(value));
            }
            catch
            {
                _logger.LogWarning("Could not read certificate from string: " + value);
                return null;
            }
        }
    }
}