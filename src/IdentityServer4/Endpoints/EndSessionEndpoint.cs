// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Endpoints.Results;
using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Specialized;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.Endpoints
{
    class EndSessionEndpoint : IEndpoint
    {
        private readonly IEndSessionRequestValidator _endSessionRequestValidator;
        private readonly ILogger<EndSessionEndpoint> _logger;
        private readonly IMatchEndSessionProtocolRoutePaths _pathMatcher;

        public EndSessionEndpoint(
            IEndSessionRequestValidator endSessionRequestValidator,
            IMatchEndSessionProtocolRoutePaths pathMatcher,
            ILogger<EndSessionEndpoint> logger)
        {
            _endSessionRequestValidator = endSessionRequestValidator;
            _pathMatcher = pathMatcher;
            _logger = logger;
        }

        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            if (_pathMatcher.IsEndSessionPath(context.Request.Path))
            {
                return await ProcessSignoutAsync(context);
            }

            if (_pathMatcher.IsEndSessionCallbackPath(context.Request.Path))
            {
                return await ProcessSignoutCallbackAsync(context);
            }

            _logger.LogWarning("Invalid request path to end session endpoint");
            return new StatusCodeResult(HttpStatusCode.NotFound);
        }

        private async Task<IEndpointResult> ProcessSignoutAsync(HttpContext context)
        {
            NameValueCollection parameters = null;
            if (context.Request.Method == "GET")
            {
                parameters = context.Request.Query.AsNameValueCollection();
            }
            else if (context.Request.Method == "POST")
            {
                parameters = context.Request.Form.AsNameValueCollection();
            }
            else
            {
                _logger.LogWarning("Invalid HTTP method for end session endpoint.");
                return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }

            var user = await context.GetIdentityServerUserAsync();

            _logger.LogDebug("Processing signout request for {subjectId}", user?.GetSubjectId() ?? "anonymous");

            var result = await _endSessionRequestValidator.ValidateAsync(parameters, user);
            
            if (result.IsError)
            {
                _logger.LogError("Error processing end session request {error}", result.Error);
            }
            else
            {
                _logger.LogDebug("Success validating end session request from {clientId}", result.ValidatedRequest?.Client?.ClientId);
            }

            return new EndSessionResult(result);
        }

        private async Task<IEndpointResult> ProcessSignoutCallbackAsync(HttpContext context)
        {
            if (context.Request.Method != "GET")
            {
                _logger.LogWarning("Invalid HTTP method for end session callback endpoint.");
                return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }

            _logger.LogDebug("Processing signout callback request");

            var parameters = context.Request.Query.AsNameValueCollection();
            var result = await _endSessionRequestValidator.ValidateCallbackAsync(parameters);

            if (result.IsError == false)
            {
                _logger.LogInformation("Successful signout callback. Client logout iframe urls: {urls}", result.ClientLogoutUrls);
            }

            return new EndSessionCallbackResult(result);
        }
    }
}
