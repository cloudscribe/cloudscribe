// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    public class BearerTokenUsageValidator
    {
        private readonly ILogger<BearerTokenUsageValidator> _logger;

        public BearerTokenUsageValidator(ILogger<BearerTokenUsageValidator> logger)
        {
            _logger = logger;
        }

        public async Task<BearerTokenUsageValidationResult> ValidateAsync(HttpContext context)
        {
            _logger.LogInformation("ValidateAsync: Locating bearer token");

            var result = ValidateAuthorizationHeader(context);
            if (result.TokenFound)
            {
                _logger.LogDebug("Bearer token found in header");
                return result;
            }

            if (context.Request.HasFormContentType)
            {
                result = await ValidatePostBodyAsync(context);
                if (result.TokenFound)
                {
                    _logger.LogDebug("Bearer token found in body");
                    return result;
                }
            }

            _logger.LogDebug("Bearer token not found");
            return new BearerTokenUsageValidationResult();
        }

        public BearerTokenUsageValidationResult ValidateAuthorizationHeader(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authorizationHeader.IsPresent())
            {
                _logger.LogTrace("Authorization header value found");

                var header = authorizationHeader.Trim();
                if (header.StartsWith(OidcConstants.AuthenticationSchemes.AuthorizationHeaderBearer))
                {
                    _logger.LogTrace("Authorization scheme is bearer");

                    var value = header.Substring(OidcConstants.AuthenticationSchemes.AuthorizationHeaderBearer.Length).Trim();
                    if (value.IsPresent())
                    {
                        return new BearerTokenUsageValidationResult
                        {
                            TokenFound = true,
                            Token = value,
                            UsageType = BearerTokenUsageType.AuthorizationHeader
                        };
                    }
                }
            }

            return new BearerTokenUsageValidationResult();
        }

        public Task<BearerTokenUsageValidationResult> ValidatePostBodyAsync(HttpContext context)
        {
            var token = context.Request.Form["access_token"].FirstOrDefault();
            if (token.IsPresent())
            {
                return Task.FromResult(new BearerTokenUsageValidationResult
                {
                    TokenFound = true,
                    Token = token,
                    UsageType = BearerTokenUsageType.PostBody
                });
            }

            return Task.FromResult(new BearerTokenUsageValidationResult());
        }
    }
}