// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-10-16
// Last Modified:			2018-10-08
// 

using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using cloudscribe.Core.IdentityServerIntegration.Storage;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.EFCore
{
    public class IdentityResourceCommands : IIdentityResourceCommands, IIdentityResourceCommandsSingleton
    {
        public IdentityResourceCommands(
            IConfigurationDbContextFactory contextFactory
            )
        {
            _contextFactory = contextFactory;

        }

        private readonly IConfigurationDbContextFactory _contextFactory;


        public async Task CreateIdentityResource(string siteId, IdentityResource identityResource, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ent = identityResource.ToEntity();
            ent.SiteId = siteId;

            using (var context = _contextFactory.CreateContext())
            {
                context.IdentityResources.Add(ent);
                await context.SaveChangesAsync();
            }
    
        }

        public async Task UpdateIdentityResource(string siteId, IdentityResource identityResource, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (identityResource == null) return;
            // since the ApiResource doesn't have the ids of the ApiResource entity child objects
            // updating creates duplicate child objects ie Claims and Secrets
            // therefore we need to actually delete the found resource
            // and re-create it - we don't care about the storage ids
            await DeleteIdentityResource(siteId, identityResource.Name, cancellationToken).ConfigureAwait(false);

            await CreateIdentityResource(siteId, identityResource, cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteIdentityResource(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var context = _contextFactory.CreateContext())
            {
                var resource = await context.IdentityResources.FirstOrDefaultAsync(x => x.SiteId == siteId && x.Name == name);
                if (resource != null)
                {
                    context.IdentityResources.Remove(resource);
                    await context.SaveChangesAsync();
                }
            }
            
        }
    }
}
