// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using IdentityServer4.Services;
using System.Linq;
using cloudscribe.Core.Models;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.Core.IdentityServer.EFCore.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IConfigurationDbContext context;
        private IHttpContextAccessor _contextAccessor;

        public CorsPolicyService(
            IHttpContextAccessor contextAccessor,
            IConfigurationDbContext context
            )
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            this.context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null) return false;
            var _siteId = site.Id.ToString();

            var origins = await context.Clients
                .AsNoTracking()
                .Where(x => x.SiteId == _siteId)
                .SelectMany(x => x.AllowedCorsOrigins.Select(y => y.Origin)).ToListAsync();

            var distinctOrigins = origins.Where(x => x != null).Distinct();

            var isAllowed = distinctOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase);

            return isAllowed;
        }
    }
}