// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using IdentityModel;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Parses a POST body for secrets
    /// </summary>
    public class PostBodySecretParser : ISecretParser
    {
        private readonly ILogger _logger;
        private readonly IdentityServerOptions _options;

        /// <summary>
        /// Creates the parser with options
        /// </summary>
        /// <param name="options">IdentityServer options</param>
        /// <param name="logger">Logger</param>
        public PostBodySecretParser(IdentityServerOptions options, ILogger<PostBodySecretParser> logger)
        {
            _logger = logger;
            _options = options;
        }

        public string AuthenticationMethod => OidcConstants.EndpointAuthenticationMethods.PostBody;

        /// <summary>
        /// Tries to find a secret on the environment that can be used for authentication
        /// </summary>
        /// <returns>
        /// A parsed secret
        /// </returns>
        public Task<ParsedSecret> ParseAsync(HttpContext context)
        {
            _logger.LogDebug("Start parsing for secret in post body");

            if (!context.Request.HasFormContentType)
            {
                _logger.LogDebug("Content type is not a form");
                return Task.FromResult<ParsedSecret>(null);
            }

            var body = context.Request.Form;

            if (body != null)
            {
                var id = body["client_id"].FirstOrDefault();
                var secret = body["client_secret"].FirstOrDefault();

                // client id must be present
                if (id.IsPresent())
                {
                    if (id.Length > _options.InputLengthRestrictions.ClientId)
                    {
                        _logger.LogError("Client ID exceeds maximum lenght.");
                        return Task.FromResult<ParsedSecret>(null);
                    }

                    if (secret.IsPresent())
                    {
                        if (secret.Length > _options.InputLengthRestrictions.ClientSecret)
                        {
                            _logger.LogError("Client secret exceeds maximum lenght.");
                            return Task.FromResult<ParsedSecret>(null);
                        }

                        return Task.FromResult(new ParsedSecret
                        {
                            Id = id,
                            Credential = secret,
                            Type = IdentityServerConstants.ParsedSecretTypes.SharedSecret
                        });
                    }
                    else
                    {
                        // client secret is optional
                        _logger.LogDebug("client id without secret found");

                        return Task.FromResult(new ParsedSecret
                        {
                            Id = id,
                            Type = IdentityServerConstants.ParsedSecretTypes.NoSecret
                        });
                    }
                }
            }

            _logger.LogDebug("No secret in post body found");
            return Task.FromResult<ParsedSecret>(null);
        }
    }
}