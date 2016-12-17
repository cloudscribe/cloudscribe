// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Services;
using System.Threading.Tasks;
using IdentityServer4.Events;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Stores;
using IdentityServer4.Models;

namespace IdentityServer4.Validation
{
    public class ClientSecretValidator
    {
        private readonly ILogger _logger;
        private readonly IClientStore _clients;
        private readonly IEventService _events;
        private readonly SecretValidator _validator;
        private readonly SecretParser _parser;

        public ClientSecretValidator(IClientStore clients, SecretParser parser, SecretValidator validator, IEventService events, ILogger<ClientSecretValidator> logger)
        {
            _clients = clients;
            _parser = parser;
            _validator = validator;
            _events = events;
            _logger = logger;
        }

        public async Task<ClientSecretValidationResult> ValidateAsync(HttpContext context)
        {
            _logger.LogDebug("Start client validation");

            var fail = new ClientSecretValidationResult
            {
                IsError = true
            };

            var parsedSecret = await _parser.ParseAsync(context);
            if (parsedSecret == null)
            {
                await RaiseFailureEvent("unknown", "No client id found");

                _logger.LogError("No client identifier found");
                return fail;
            }

            // load client
            var client = await _clients.FindEnabledClientByIdAsync(parsedSecret.Id);
            if (client == null)
            {
                await RaiseFailureEvent(parsedSecret.Id, "Unknown client");

                _logger.LogError("No client with id '{clientId}' found. aborting", parsedSecret.Id);
                return fail;
            }

            if (!client.RequireClientSecret || client.IsImplicitOnly())
            {
                _logger.LogDebug("Public Client - skipping secret validation success");
            }
            else
            {
                var result = await _validator.ValidateAsync(parsedSecret, client.ClientSecrets);
                if (result.Success == false)
                {
                    await RaiseFailureEvent(client.ClientId, "Invalid client secret");
                    _logger.LogError("Client secret validation failed for client: {clientId}.", client.ClientId);

                    return fail;
                }
            }

            _logger.LogDebug("Client validation success");

            var success = new ClientSecretValidationResult
            {
                IsError = false,
                Client = client
            };

            await RaiseSuccessEvent(client.ClientId);
            return success;
        }

        private async Task RaiseSuccessEvent(string clientId)
        {
            await _events.RaiseSuccessfulClientAuthenticationEventAsync(clientId, EventConstants.ClientTypes.Client);
        }

        private async Task RaiseFailureEvent(string clientId, string message)
        {
            await _events.RaiseFailureClientAuthenticationEventAsync(message, clientId, EventConstants.ClientTypes.Client);
        }
    }
}