// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Services.InMemory
{
    /// <summary>
    /// CORS policy service that configures the allowed origins from a list of clients' redirect URLs.
    /// </summary>
    public class InMemoryCorsPolicyService : ICorsPolicyService
    {
        private readonly ILogger<InMemoryCorsPolicyService> _logger;
        private readonly IEnumerable<Client> _clients;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCorsPolicyService"/> class.
        /// </summary>
        /// <param name="clients">The clients.</param>
        public InMemoryCorsPolicyService(ILogger<InMemoryCorsPolicyService> logger, IEnumerable<Client> clients)
        {
            _logger = logger;
            _clients = clients ?? Enumerable.Empty<Client>();
        }

        /// <summary>
        /// Determines whether origin is allowed.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <returns></returns>
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            var query =
                from client in _clients
                from url in client.AllowedCorsOrigins
                select url.GetOrigin();

            var result = query.Contains(origin, StringComparer.OrdinalIgnoreCase);

            if (result)
            {
                _logger.LogDebug("Client list checked and origin: {0} is allowed", origin);
            }
            else
            {
                _logger.LogDebug("Client list checked and origin: {0} is not allowed", origin);
            }
            
            return Task.FromResult(result);
        }
    }
}
