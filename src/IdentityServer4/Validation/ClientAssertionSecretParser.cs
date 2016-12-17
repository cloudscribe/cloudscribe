// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Parses a POST body for secrets
    /// </summary>
    public class ClientAssertionSecretParser : ISecretParser
    {
        private readonly IdentityServerOptions _options;
        private readonly ILogger _logger;

        public ClientAssertionSecretParser(IdentityServerOptions options, ILogger<ClientAssertionSecretParser> logger)
        {
            _options = options;
            _logger = logger;
        }

        public string AuthenticationMethod => OidcConstants.EndpointAuthenticationMethods.PrivateKeyJwt;

        /// <summary>
        /// Tries to find a JWT client assertion token in the request body that can be used for authentication
        /// Used for "private_key_jwt" client authentication method as defined in http://openid.net/specs/openid-connect-core-1_0.html#ClientAuthentication
        /// </summary>
        /// <param name="context">The HTTP context</param>
        /// <returns>
        /// A parsed secret
        /// </returns>
        public Task<ParsedSecret> ParseAsync(HttpContext context)
        {
            _logger.LogDebug("Start parsing for JWT client assertion in post body");

            if (!context.Request.HasFormContentType)
            {
                _logger.LogDebug("Content type is not a form");
                return Task.FromResult<ParsedSecret>(null);
            }

            var body = context.Request.Form;

            if (body != null)
            {
                var clientId = body[OidcConstants.TokenRequest.ClientId].FirstOrDefault();
                var clientAssertionType = body[OidcConstants.TokenRequest.ClientAssertionType].FirstOrDefault();
                var clientAssertion = body[OidcConstants.TokenRequest.ClientAssertion].FirstOrDefault();
                
                if (clientAssertion.IsPresent()
                    && clientAssertionType == OidcConstants.ClientAssertionTypes.JwtBearer)
                {
                    if (clientAssertion.Length > _options.InputLengthRestrictions.Jwt)
                    {
                        _logger.LogError("Client assertion token exceeds maximum lenght.");
                        return Task.FromResult<ParsedSecret>(null);
                    }

                    if (!clientId.IsPresent())
                    {
                        // actual "client_id" form field is optional since the value is always present inside the token
                        clientId = GetClientIdFromToken(clientAssertion);
                        if (!clientId.IsPresent())
                        {
                            return null;
                        }
                    }
                    
                    if (clientId.Length > _options.InputLengthRestrictions.ClientId)
                    {
                        _logger.LogError("Client ID exceeds maximum lenght.");
                        return Task.FromResult<ParsedSecret>(null);
                    }

                    var parsedSecret = new ParsedSecret
                    {
                        Id = clientId,
                        Credential = clientAssertion,
                        Type = IdentityServerConstants.ParsedSecretTypes.JwtBearer
                    };

                    return Task.FromResult<ParsedSecret>(parsedSecret);
                }
            }

            _logger.LogDebug("No JWT client assertion found in post body");
            return Task.FromResult<ParsedSecret>(null);
        }

        private string GetClientIdFromToken(string token)
        {
            try
            {
                var jwt = new JwtSecurityToken(token);
                return jwt.Issuer;
            }
            catch (Exception e)
            {
                _logger.LogWarning("Could not parse client assertion", e);
                return null;
            }
        }
    }
}