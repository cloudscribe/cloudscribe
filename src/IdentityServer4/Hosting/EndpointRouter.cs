// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.Hosting
{
    class EndpointRouter : IEndpointRouter
    {
        private readonly Dictionary<string, EndpointName> _pathToNameMap;
        private readonly IdentityServerOptions _options;
        private readonly IEnumerable<EndpointMapping> _mappings;
        private readonly ILogger<EndpointRouter> _logger;

        public EndpointRouter(Dictionary<string, EndpointName> pathToNameMap, IdentityServerOptions options, IEnumerable<EndpointMapping> mappings, ILogger<EndpointRouter> logger)
        {
            _pathToNameMap = pathToNameMap;
            _options = options;
            _mappings = mappings;
            _logger = logger;
        }

        public IEndpoint Find(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach(var key in _pathToNameMap.Keys)
            {
                var path = key.EnsureLeadingSlash();
                if (context.Request.Path.StartsWithSegments(path))
                {
                    var endpointName = _pathToNameMap[key];
                    _logger.LogDebug("Request path {path} matched to endpoint type {endpoint}", context.Request.Path, endpointName);

                    return GetEndpoint(endpointName, context);
                }
            }

            _logger.LogTrace("No endpoint entry found for request path: {path}", context.Request.Path);

            return null;
        }

        private IEndpoint GetEndpoint(EndpointName endpointName, HttpContext context)
        {
            if (_options.Endpoints.IsEndpointEnabled(endpointName))
            {
                var mapping = _mappings.LastOrDefault(x => x.Endpoint == endpointName);
                if (mapping != null)
                {
                    _logger.LogDebug("Mapping found for endpoint: {endpoint}, creating handler: {endpointHandler}", endpointName, mapping.Handler.FullName);
                    return context.RequestServices.GetService(mapping.Handler) as IEndpoint;
                }
                else
                {
                    _logger.LogError("No mapping found for endpoint: {endpoint}", endpointName);
                }
            }
            else
            {
                _logger.LogWarning("{endpoint} endpoint requested, but is diabled in endpoint options.", endpointName);
            }

            return null;
        }
    }
}
