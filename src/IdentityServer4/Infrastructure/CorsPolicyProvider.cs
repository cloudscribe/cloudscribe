// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors.Infrastructure;
using IdentityServer4.Configuration;
using IdentityServer4.Configuration.DependencyInjection;
using IdentityServer4.Services;
using IdentityServer4.Infrastructure;

namespace IdentityServer4
{
    internal class CorsPolicyProvider : ICorsPolicyProvider
    {
        private readonly ICorsPolicyService _corsPolicyService;
        private readonly ILogger<CorsPolicyProvider> _logger;
        private readonly ICorsPolicyProvider _inner;
        private readonly IdentityServerOptions _options;
        private readonly ICorsPathValidator _corsPathValidator;

        public CorsPolicyProvider(
            ILogger<CorsPolicyProvider> logger,
            Decorator<ICorsPolicyProvider> inner,
            IdentityServerOptions options,
            ICorsPolicyService corsPolicyService,
            ICorsPathValidator corsPathValidator
            )
        {
            _logger = logger;
            _inner = inner.Instance;
            _options = options;
            _corsPolicyService = corsPolicyService;
            _corsPathValidator = corsPathValidator;
        }

        public Task<CorsPolicy> GetPolicyAsync(HttpContext context, string policyName)
        {
            if (_options.CorsOptions.CorsPolicyName == policyName)
            {
                return ProcessAsync(context);
            }
            else
            {
                return _inner.GetPolicyAsync(context, policyName);
            }
        }

        async Task<CorsPolicy> ProcessAsync(HttpContext context)
        {
            var origin = context.Request.GetCorsOrigin();
            if (origin != null)
            {
                var path = context.Request.Path;
                if (IsPathAllowed(path))
                {
                    _logger.LogDebug("CORS request made for path: {path} from origin: {origin}", path, origin);

                    if (await _corsPolicyService.IsOriginAllowedAsync(origin))
                    {
                        _logger.LogDebug("CorsPolicyService allowed origin: {origin}", origin);
                        return Allow(origin);
                    }
                    else
                    {
                        _logger.LogWarning("CorsPolicyService did not allow origin: {origin}", origin);
                    }
                }
                else
                {
                    _logger.LogDebug("CORS request made for path: {path} from origin: {origin} but rejected because invalid CORS path", path, origin);
                }
            }

            return null;
        }

        private CorsPolicy Allow(string origin)
        {
            var policyBuilder = new CorsPolicyBuilder();

            var policy = policyBuilder
                .WithOrigins(origin)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .Build();

            return policy;
        }

        private bool IsPathAllowed(PathString path)
        {
            return _corsPathValidator.IsPathAllowed(path, _options.CorsOptions);
        }
    }
}
