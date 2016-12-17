// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    public class ApiSecretValidator
    {
        private readonly ILogger _logger;
        private readonly IResourceStore _resources;
        private readonly IEventService _events;
        private readonly SecretParser _parser;
        private readonly SecretValidator _validator;

        public ApiSecretValidator(IResourceStore resources, SecretParser parsers, SecretValidator validator, IEventService events, ILogger<ApiSecretValidator> logger)
        {
            _resources = resources;
            _parser = parsers;
            _validator = validator;
            _events = events;
            _logger = logger;
        }

        public async Task<ApiSecretValidationResult> ValidateAsync(HttpContext context)
        {
            _logger.LogTrace("Start API validation");

            var fail = new ApiSecretValidationResult
            {
                IsError = true
            };

            var parsedSecret = await _parser.ParseAsync(context);
            if (parsedSecret == null)
            {
                await RaiseFailureEvent("unknown", "No API id or secret found");

                _logger.LogError("No scope secret found");
                return fail;
            }

            // load API resource
            var api = await _resources.FindApiResourceAsync(parsedSecret.Id);
            if (api == null)
            {
                await RaiseFailureEvent(parsedSecret.Id, "Unknown API resource");

                _logger.LogError("No API resource with that name found. aborting");
                return fail;
            }

            if (api.Enabled == false)
            {
                await RaiseFailureEvent(parsedSecret.Id, "API resource not enabled");

                _logger.LogError("API resource not enabled. aborting.");
                return fail;
            }

            var result = await _validator.ValidateAsync(parsedSecret, api.ApiSecrets);
            if (result.Success)
            {
                _logger.LogDebug("API resource validation success");

                var success = new ApiSecretValidationResult
                {
                    IsError = false,
                    Resource = api
                };

                await RaiseSuccessEvent(api.Name);
                return success;
            }

            await RaiseFailureEvent(api.Name, "Invalid API secret");
            _logger.LogError("API validation failed.");

            return fail;
        }

        private async Task RaiseSuccessEvent(string clientId)
        {
            // TODO: API secret validation (not client)
            await _events.RaiseSuccessfulClientAuthenticationEventAsync(clientId, EventConstants.ClientTypes.Scope);
        }

        private async Task RaiseFailureEvent(string clientId, string message)
        {
            // TODO: API secret validation (not client)
            await _events.RaiseFailureClientAuthenticationEventAsync(message, clientId, EventConstants.ClientTypes.Scope);
        }
    }
}