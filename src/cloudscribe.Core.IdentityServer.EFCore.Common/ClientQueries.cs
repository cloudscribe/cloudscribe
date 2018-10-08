// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-10-19
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
    public class ClientQueries : IClientQueries, IClientQueriesSingleton
    {
        public ClientQueries(
            IConfigurationDbContextFactory contextFactory
            )
        {
            _contextFactory = contextFactory;
        }

        private readonly IConfigurationDbContextFactory _contextFactory;

        public async Task<bool> ClientExists(string siteId, string clientId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var client = await FetchClient(siteId, clientId, cancellationToken).ConfigureAwait(false);
            return (client != null);
        }

        public async Task<Client> FetchClient(string siteId, string clientId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var context = _contextFactory.CreateContext())
            {
                var client = await context.Clients
                .AsNoTracking()
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.Properties)
                .Include(x => x.AllowedCorsOrigins)
                .FirstOrDefaultAsync(x => x.SiteId == siteId && x.ClientId == clientId);

                var model = client.ToModel();

                return model;
            }

                
        }

        public async Task<int> CountClients(string siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var context = _contextFactory.CreateContext())
            {
                return await context.Clients.AsNoTracking().Where(x => x.SiteId == siteId).CountAsync(cancellationToken).ConfigureAwait(false);
            }
            
        }

        public async Task<PagedResult<Client>> GetClients(
            string siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            using (var context = _contextFactory.CreateContext())
            {
                var data = await context.Clients
                .AsNoTracking()
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .Where(x => x.SiteId == siteId)
                .OrderBy(x => x.ClientName)
                .Skip(offset)
                .Take(pageSize).ToListAsync().ConfigureAwait(false);

                var result = new PagedResult<Client>();
                result.Data = data.Select(x => x.ToModel()).ToList();
                result.TotalItems = await CountClients(siteId, cancellationToken).ConfigureAwait(false);
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;

                return result;
            }

            
        }

    }
}
