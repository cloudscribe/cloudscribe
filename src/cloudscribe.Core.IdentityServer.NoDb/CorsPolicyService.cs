// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-10-12
// Last Modified:           2016-10-25
// 

using cloudscribe.Core.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using NoDb;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.NoDb
{
    public class CorsPolicyService : ICorsPolicyService
    {
        public CorsPolicyService(
            IHttpContextAccessor contextAccessor,
            IBasicQueries<Client> queries
            )
        {
            _queries = queries;
            this.contextAccessor = contextAccessor;
        }

        private IBasicQueries<Client> _queries;
        private IHttpContextAccessor contextAccessor;

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            var site = contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null) return false;
            //TODO: cache
            var clients = await _queries.GetAllAsync(site.Id.ToString()).ConfigureAwait(false);

            var origins = clients.SelectMany(x => x.AllowedCorsOrigins.Select(y => y)).ToList();

            var distinctOrigins = origins.Where(x => x != null).Distinct();

            var isAllowed = distinctOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase);

            return isAllowed;
        }
    }
}
