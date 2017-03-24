// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-10-16
// Last Modified:			2017-03-24
// 

using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using cloudscribe.Core.IdentityServerIntegration.Storage;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.EFCore
{
    public class ApiResourceCommands : IApiResourceCommands
    {
        public ApiResourceCommands(
            IConfigurationDbContext context
            )
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            this.context = context;

        }

        private readonly IConfigurationDbContext context;


        public async Task CreateApiResource(string siteId, ApiResource apiResource, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ent = apiResource.ToEntity();
            ent.SiteId = siteId;
            foreach(var s in ent.Scopes)
            {
                s.SiteId = siteId;
            }
            context.ApiResources.Add(ent);
            await context.SaveChangesAsync();
        }

        public async Task UpdateApiResource(string siteId, ApiResource apiResource, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (apiResource == null) return;
            // since the ApiResource doesn't have the ids of the ApiResource entity child objects
            // updating creates duplicate child objects ie Claims and Secrets
            // therefore we need to actually delete the found resource
            // and re-create it - we don't care about the storage ids
            await DeleteApiResource(siteId, apiResource.Name, cancellationToken).ConfigureAwait(false);
            
            await CreateApiResource(siteId, apiResource, cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteApiResource(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var resource = await context.ApiResources.FirstOrDefaultAsync(x => x.SiteId == siteId && x.Name == name);
            if (resource != null)
            {
                context.ApiResources.Remove(resource);
                await context.SaveChangesAsync();
            }
        }
    }
}
