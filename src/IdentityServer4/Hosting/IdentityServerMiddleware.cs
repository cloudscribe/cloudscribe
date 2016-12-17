// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.Hosting
{
    public class IdentityServerMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public IdentityServerMiddleware(RequestDelegate next, ILogger<IdentityServerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IEndpointRouter router)
        {
            try
            {
                var endpoint = router.Find(context);
                if (endpoint != null)
                {
                    _logger.LogInformation("Invoking IdentityServer endpoint: {endpointType} for {url}", endpoint.GetType().FullName, context.Request.Path.ToString());

                    var result = await endpoint.ProcessAsync(context);

                    if (result != null)
                    {
                        _logger.LogTrace("Invoking result: {type}", result.GetType().FullName);
                        await result.ExecuteAsync(context);
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Unhandled exception: {exception}", ex.ToString());
                throw;
            }

            await _next(context);
        }
    }
}