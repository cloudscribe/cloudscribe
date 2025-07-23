// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-10-19
// Last Modified:			2018-10-08
// 

using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using cloudscribe.Core.IdentityServerIntegration.Storage;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.EFCore
{
    public class ClientCommands : IClientCommands, IClientCommandsSingleton
    {
        public ClientCommands(
            IConfigurationDbContextFactory contextFactory
            )
        {
            _contextFactory = contextFactory;
        }

        private readonly IConfigurationDbContextFactory _contextFactory;


        public async Task UpdateClient(string siteId, Client client, CancellationToken cancellationToken = default)
        {
            if (client == null) return;
            cancellationToken.ThrowIfCancellationRequested();

            using (var context = _contextFactory.CreateContext())
            {
                await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

                try
                {
                    await DeleteClient(siteId, client.ClientId, context, cancellationToken).ConfigureAwait(false);
                    await CreateClient(siteId, client, context, cancellationToken).ConfigureAwait(false);

                    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                    throw;
                }
            }
        }


        public async Task CreateClient(string siteId, Client client, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var ent = client.ToEntity();
            ent.SiteId = siteId;

            using (var context = _contextFactory.CreateContext())
            {
                await CreateClient(siteId, client, context, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task CreateClient(string siteId, Client client, IConfigurationDbContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var ent = client.ToEntity();
            ent.SiteId = siteId;

            context.Clients.Add(ent);
        }


        public async Task DeleteClient(string siteId, string clientId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var context = _contextFactory.CreateContext())
            {
                await DeleteClient(siteId, clientId, context, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task DeleteClient(string siteId, string clientId, IConfigurationDbContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var client = context.Clients
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .FirstOrDefault(x => x.SiteId == siteId && x.ClientId == clientId);

            if (client != null)
            {
                context.Clients.Remove(client);
            }
        }


    }
}
