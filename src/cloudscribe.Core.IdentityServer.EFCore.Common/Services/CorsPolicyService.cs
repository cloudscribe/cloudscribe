// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using IdentityServer4.Services;
using System.Linq;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;

namespace cloudscribe.Core.IdentityServer.EFCore.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IConfigurationDbContext context;

        public CorsPolicyService(IConfigurationDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            this.context = context;
        }

        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            var origins = context.Clients.SelectMany(x => x.AllowedCorsOrigins.Select(y => y.Origin)).ToList();

            var distinctOrigins = origins.Where(x => x != null).Distinct();

            var isAllowed = distinctOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase);

            return Task.FromResult(isAllowed);
        }
    }
}