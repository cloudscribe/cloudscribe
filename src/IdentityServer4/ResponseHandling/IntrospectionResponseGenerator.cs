// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace IdentityServer4.ResponseHandling
{
    public class IntrospectionResponseGenerator : IIntrospectionResponseGenerator
    {
        private readonly ILogger _logger;

        public IntrospectionResponseGenerator(ILogger<IntrospectionResponseGenerator> logger)
        {
            _logger = logger;
        }

        public Task<Dictionary<string, object>> ProcessAsync(IntrospectionRequestValidationResult validationResult, ApiResource apiResource)
        {
            _logger.LogTrace("Creating introspection response");

            if (validationResult.IsActive == false)
            {
                _logger.LogDebug("Creating introspection response for inactive token.");

                var response = new Dictionary<string, object>();
                response.Add("active", false);
                return Task.FromResult(response);
            }
            else
            { 
                _logger.LogDebug("Creating introspection response for active token.");

                var response = validationResult.Claims.Where(c => c.Type != JwtClaimTypes.Scope).ToClaimsDictionary();

                var allowedScopes = apiResource.Scopes.Select(x => x.Name);
                var scopes = validationResult.Claims.Where(c => c.Type == JwtClaimTypes.Scope).Select(x => x.Value);
                scopes = scopes.Where(x => allowedScopes.Contains(x));
                response.Add("scope", scopes.ToArray());

                response.Add("active", true);

                return Task.FromResult(response);
            }
        }
    }
}