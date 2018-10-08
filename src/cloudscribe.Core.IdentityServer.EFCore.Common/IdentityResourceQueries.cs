// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-12-05
// Last Modified:			2018-10-08
// 

using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using cloudscribe.Core.IdentityServerIntegration.Storage;
using cloudscribe.Pagination.Models;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.EFCore
{
    public class IdentityResourceQueries : IIdentityResourceQueries, IIdentityResourceQueriesSingleton
    {
        public IdentityResourceQueries(
            IConfigurationDbContextFactory contextFactory
            )
        {
            _contextFactory = contextFactory;
        }

        private readonly IConfigurationDbContextFactory _contextFactory;

        public async Task<bool> IdentityResourceExists(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var resource = await FetchIdentityResource(siteId, name, cancellationToken).ConfigureAwait(false);
            return (resource != null);
        }

        public async Task<IdentityResource> FetchIdentityResource(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var context = _contextFactory.CreateContext())
            {
                IQueryable<Entities.IdentityResource> query = context.IdentityResources
                .AsNoTracking()
                .Include(x => x.UserClaims);

                query = query.Where(x => x.SiteId == siteId && x.Name == name);
                var ent = await query.SingleOrDefaultAsync();

                return ent.ToModel();
            }
            
        }

        public async Task<int> CountIdentityResources(string siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var context = _contextFactory.CreateContext())
            {
                return await context.IdentityResources
                .Where(x => x.SiteId == siteId)
                .CountAsync();
            }
            
        }

        public async Task<PagedResult<IdentityResource>> GetIdentityResources(
            string siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            using (var context = _contextFactory.CreateContext())
            {
                var list = await context.IdentityResources
                .AsNoTracking()
                .Where(x => x.SiteId == siteId)
                .OrderBy(x => x.Name)
                .Skip(offset)
                .Take(pageSize).ToListAsync();

                var result = new PagedResult<IdentityResource>();

                var model = list.Select(x => x.ToModel());
                result.Data = model.ToList();
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await CountIdentityResources(siteId, cancellationToken).ConfigureAwait(false);

                return result;
            }
            
        }
    }
}
