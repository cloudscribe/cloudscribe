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
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace cloudscribe.Core.IdentityServer.EFCore.Stores
{
    public class ResourceStore : IResourceStore
    {
        private readonly IConfigurationDbContext _context;
        private IHttpContextAccessor _contextAccessor;
        private readonly ILogger<ResourceStore> _logger;

        public ResourceStore(
            IHttpContextAccessor contextAccessor,
            IConfigurationDbContext context, 
            ILogger<ResourceStore> logger
            )
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _contextAccessor = contextAccessor;
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null) return null;
            var _siteId = site.Id.ToString();

            var query =
                from apiResource in _context.ApiResources
                where apiResource.Name == name && apiResource.SiteId == _siteId
                select apiResource;

            var apis = query
                .AsNoTracking()
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                    .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims);

            var api = await apis.FirstOrDefaultAsync();

            if (api != null)
            {
                _logger.LogDebug("Found {api} API resource in database", name);
            }
            else
            {
                _logger.LogDebug("Did not find {api} API resource in database", name);
            }

            return api.ToModel();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null) return null;
            var _siteId = site.Id.ToString();

            var names = scopeNames.ToArray();

            //var query =
            //    from api in _context.ApiResources
            //    let scopes = api.Scopes.Select(x => x.Name)
            //    where api.SiteId == _siteId && scopes.Intersect(names).Any()
            //    select api;
            var query =
                from api in _context.ApiResources
                where api.SiteId == _siteId && api.Scopes.Where(x => names.Contains(x.Name)).Any()
                select api;

            var apis = query
                .AsNoTracking()
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                    .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims);

            var results = await apis.ToArrayAsync();
            var models = results.Select(x => x.ToModel()).ToArray();

            _logger.LogDebug("Found {scopes} API scopes in database", models.SelectMany(x => x.Scopes).Select(x => x.Name));

            return models.AsEnumerable();
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null) return null;
            var _siteId = site.Id.ToString();

            var scopes = scopeNames.ToArray();

            var query =
                from identityResource in _context.IdentityResources
                .AsNoTracking()
                where identityResource.SiteId == _siteId && scopes.Contains(identityResource.Name)
                select identityResource
                ;

            var resources = query
                .Include(x => x.UserClaims);

            var results = await resources.ToArrayAsync();

            _logger.LogDebug("Found {scopes} identity scopes in database", results.Select(x => x.Name));

            return results.Select(x => x.ToModel()).ToArray().AsEnumerable();
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null) return null;
            var _siteId = site.Id.ToString();

            var identity = _context.IdentityResources
              .AsNoTracking()
              .Include(x => x.UserClaims)
              .Where(x => x.SiteId == _siteId)
              ;

            var apis = _context.ApiResources
                .AsNoTracking()
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                    .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims)
                .Where(x => x.SiteId == _siteId)
                ;

            //var result = new Resources(
            //    identity.ToArray().Select(x => x.ToModel()).AsEnumerable(),
            //    apis.ToArray().Select(x => x.ToModel()).AsEnumerable());

            var i = await identity.ToArrayAsync();
            var a = await apis.ToArrayAsync();

            var result = new Resources(
                i.Select(x => x.ToModel()).AsEnumerable(),
                a.Select(x => x.ToModel()).AsEnumerable());

            _logger.LogDebug("Found {scopes} as all scopes in database", result.IdentityResources.Select(x => x.Name).Union(result.ApiResources.SelectMany(x => x.Scopes).Select(x => x.Name)));

            return result;
        }
    }
}
