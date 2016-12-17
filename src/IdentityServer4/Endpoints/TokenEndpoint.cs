// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Endpoints.Results;
using IdentityServer4.Hosting;
using IdentityServer4.Models;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Endpoints
{
    public class TokenEndpoint : IEndpoint
    {
        private readonly ClientSecretValidator _clientValidator;
        private readonly ILogger _logger;
        private readonly ITokenRequestValidator _requestValidator;
        private readonly ITokenResponseGenerator _responseGenerator;

        public TokenEndpoint(ITokenRequestValidator requestValidator, ClientSecretValidator clientValidator, ITokenResponseGenerator responseGenerator, ILogger<TokenEndpoint> logger)
        {
            _requestValidator = requestValidator;
            _clientValidator = clientValidator;
            _responseGenerator = responseGenerator;
            _logger = logger;
        }

        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            _logger.LogTrace("Processing token request.");

            // validate HTTP
            if (context.Request.Method != "POST" || !context.Request.HasFormContentType)
            {
                _logger.LogWarning("Invalid HTTP request for token endpoint");
                return Error(OidcConstants.TokenErrors.InvalidRequest);
            }

            return await ProcessTokenRequestAsync(context);
        }

        private async Task<IEndpointResult> ProcessTokenRequestAsync(HttpContext context)
        {
            _logger.LogDebug("Start token request.");

            // validate client
            var clientResult = await _clientValidator.ValidateAsync(context);

            if (clientResult.Client == null)
            {
                return Error(OidcConstants.TokenErrors.InvalidClient);
            }

            // validate request
            var requestResult = await _requestValidator.ValidateRequestAsync(
                context.Request.Form.AsNameValueCollection(),
                clientResult.Client);

            if (requestResult.IsError)
            {
                return Error(requestResult.Error, requestResult.ErrorDescription, requestResult.CustomResponse);
            }

            // create response
            var response = await _responseGenerator.ProcessAsync(requestResult);

            // return result
            _logger.LogDebug("Token request success.");
            return new TokenResult(response);
        }

        private TokenErrorResult Error(string error, string errorDescription = null, Dictionary<string, object> custom = null)
        {
            var response = new TokenErrorResponse
            {
                Error = error,
                ErrorDescription = errorDescription,
                Custom = custom
            };

            return new TokenErrorResult(response);
        }
    }
}