// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using IdentityServer4.Validation;
using IdentityServer4.Services;
using IdentityServer4.ResponseHandling;
using Microsoft.Extensions.Logging;
using IdentityServer4.Events;
using IdentityServer4.Hosting;
using IdentityServer4.Endpoints.Results;
using IdentityModel;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.Endpoints
{
    public class UserInfoEndpoint : IEndpoint
    {
        private readonly ILogger _logger;
        private readonly IEventService _events;
        private readonly IUserInfoResponseGenerator _generator;
        private readonly IdentityServerOptions _options;
        private readonly BearerTokenUsageValidator _tokenUsageValidator;
        private readonly ITokenValidator _tokenValidator;

        public UserInfoEndpoint(IdentityServerOptions options, ITokenValidator tokenValidator, IUserInfoResponseGenerator generator, BearerTokenUsageValidator tokenUsageValidator, IEventService events, ILogger<UserInfoEndpoint> logger)
        {
            _options = options;
            _tokenValidator = tokenValidator;
            _tokenUsageValidator = tokenUsageValidator;
            _generator = generator;
            _events = events;
            _logger = logger;
        }

        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            if (context.Request.Method != "GET" && context.Request.Method != "POST")
            {
                _logger.LogWarning("Invalid HTTP method for userinfo endpoint.");
                return new StatusCodeResult(405);
            }

            return await ProcessUserInfoRequestAsync(context);
        }

        private async Task<IEndpointResult> ProcessUserInfoRequestAsync(HttpContext context)
        {
            _logger.LogDebug("Start userinfo request");

            var tokenUsageResult = await _tokenUsageValidator.ValidateAsync(context);
            if (tokenUsageResult.TokenFound == false)
            {
                var error = "No access token found.";

                _logger.LogError(error);
                await RaiseFailureEventAsync(error);
                return Error(OidcConstants.ProtectedResourceErrors.InvalidToken);
            }

            _logger.LogDebug("Token found: {bearerTokenUsageType}", tokenUsageResult.UsageType.ToString());

            var tokenResult = await _tokenValidator.ValidateAccessTokenAsync(
                tokenUsageResult.Token,
                IdentityServerConstants.StandardScopes.OpenId);

            if (tokenResult.IsError)
            {
                _logger.LogError(tokenResult.Error);
                await RaiseFailureEventAsync(tokenResult.Error);
                return Error(tokenResult.Error);
            }

            // pass scopes/claims to profile service
            var claims = tokenResult.Claims.Where(x => !Constants.Filters.ProtocolClaimsFilter.Contains(x.Type));
            var subject = Principal.Create("UserInfo", claims.ToArray());

            if (subject.FindFirst(JwtClaimTypes.Subject) == null)
            {
                var error = "Token contains no sub claim";
                _logger.LogError(error);
                await RaiseFailureEventAsync(error);
                return Error(OidcConstants.ProtectedResourceErrors.InvalidToken);
            }

            var scopes = tokenResult.Claims.Where(c => c.Type == JwtClaimTypes.Scope).Select(c => c.Value);

            var payload = await _generator.ProcessAsync(subject, scopes, tokenResult.Client);

            _logger.LogDebug("End userinfo request");
            await RaiseSuccessEventAsync();

            return new UserInfoResult(payload);
        }

        private IEndpointResult Error(string error, string description = null)
        {
            return new ProtectedResourceErrorResult(error, description);
        }

        private async Task RaiseSuccessEventAsync()
        {
            await _events.RaiseSuccessfulEndpointEventAsync(EventConstants.EndpointNames.UserInfo);
        }

        private async Task RaiseFailureEventAsync(string error)
        {
            if (_options.EventsOptions.RaiseFailureEvents)
            {
                await _events.RaiseFailureEndpointEventAsync(EventConstants.EndpointNames.UserInfo, error);
            }
        }
    }
}