// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-12-05
// Last Modified:			2016-12-05
// 

using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using cloudscribe.Core.IdentityServerIntegration.Storage;
using cloudscribe.Core.Models.Generic;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.EFCore
{
    public class ApiResourceQueries : IApiResourceQueries
    {
        public ApiResourceQueries(
            IConfigurationDbContext context
            )
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            this.context = context;
        }

        private readonly IConfigurationDbContext context;

        public async Task<bool> ApiResourceExists(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var resource = await FetchApiResource(siteId, name, cancellationToken).ConfigureAwait(false);
            return (resource != null);
        }

        public async Task<ApiResource> FetchApiResource(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<Entities.ApiResource> query = context.ApiResources
                .AsNoTracking()
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims);

            query = query.Where(x => x.SiteId == siteId && x.Name == name);
            var ent = await query.SingleOrDefaultAsync();

            return ent.ToModel();

        }

        public async Task<int> CountApiResources(string siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await context.ApiResources
                .Where(x => x.SiteId == siteId)
                .CountAsync();
        }

        public async Task<PagedResult<ApiResource>> GetApiResources(
            string siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            int offset = (pageSize * pageNumber) - pageSize;

            var list = await context.ApiResources
                .AsNoTracking()
                .Where(x => x.SiteId == siteId)
                .OrderBy(x => x.Name)
                .Skip(offset)
                .Take(pageSize).ToListAsync();

            var result = new PagedResult<ApiResource>();
            result.TotalItems = await CountApiResources(siteId, cancellationToken).ConfigureAwait(false);
            var model = list.Select(x => x.ToModel());
            result.Data = model.ToList();

            return result;

        }
    }
}
