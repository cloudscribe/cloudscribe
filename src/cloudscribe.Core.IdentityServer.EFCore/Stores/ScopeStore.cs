// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.Core.IdentityServer.EFCore.Stores
{
    public class ScopeStore : IScopeStore
    {
        private readonly IConfigurationDbContext context;
        private string _siteId;

        public ScopeStore(
            SiteContext site,
            IConfigurationDbContext context
            )
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _siteId = site.Id.ToString();
            this.context = context;
        }

        public async Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            IQueryable<Entities.Scope> scopes = context.Scopes
                .AsNoTracking()
                .Include(x => x.Claims)
                .Include(x => x.ScopeSecrets);

            if (scopeNames != null && scopeNames.Any())
            {
                scopes = scopes.Where(x => x.SiteId == _siteId && scopeNames.Contains(x.Name));
            }

            var foundScopes = await scopes.ToListAsync();
            var model = foundScopes.Select(x => x.ToModel());

            return model;
        }

        public async Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        {
            IQueryable<Entities.Scope> scopes = context.Scopes
                .AsNoTracking()
                .Include(x => x.Claims)
                .Include(x => x.ScopeSecrets);

            if (publicOnly)
            {
                scopes = scopes.Where(x => x.SiteId == _siteId && x.ShowInDiscoveryDocument);
            }
            else
            {
                scopes = scopes.Where(x => x.SiteId == _siteId);
            }

            var foundScopes = await scopes.ToListAsync();
            var model = foundScopes.Select(x => x.ToModel());

            return model;
        }
    }
}