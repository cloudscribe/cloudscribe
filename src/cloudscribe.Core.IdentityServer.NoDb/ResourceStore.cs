// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-10-12
// Last Modified:           2016-12-06
// 

using cloudscribe.Core.Models;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using NoDb;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.NoDb
{
    public class ResourceStore : IResourceStore
    {
        public ResourceStore(
            IHttpContextAccessor contextAccessor,
            IBasicQueries<ApiResource> apiQueries,
            IBasicQueries<IdentityResource> identityResourceQueries
            )
        {
            _contextAccessor = contextAccessor;
            _apiQueries = apiQueries;
            _identityResourceQueries = identityResourceQueries;
        }

        private IBasicQueries<ApiResource> _apiQueries;
        private IBasicQueries<IdentityResource> _identityResourceQueries;
        private IHttpContextAccessor _contextAccessor;

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var allApis = await GetAllApis().ConfigureAwait(false);
            return allApis.Where(x => x.Name == name).FirstOrDefault();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames != null && scopeNames.Any())
            {
                var all = await GetAllApis().ConfigureAwait(false);
                var apis = all.Where(x => scopeNames.Contains(x.Name));
                return apis;
            }

            return new List<ApiResource>();
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames != null && scopeNames.Any())
            {
                var all = await GetAllIdentityResources().ConfigureAwait(false);
                var found = all.Where(x => scopeNames.Contains(x.Name));
                return found;
            }

            return new List<IdentityResource>();
        }

        public async Task<Resources> GetAllResources()
        {
            var ids = await GetAllIdentityResources().ConfigureAwait(false);
            var apis = await GetAllApis().ConfigureAwait(false);
            var result = new Resources(ids, apis);
            return result;
        }
        
        private async Task<IEnumerable<ApiResource>> GetAllApis()
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null) return new List<ApiResource>();
            //TODO: cache
            var allApis = await _apiQueries.GetAllAsync(site.Id.ToString()).ConfigureAwait(false);

            return allApis;
        }

        private async Task<IEnumerable<IdentityResource>> GetAllIdentityResources()
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null) return new List<IdentityResource>();
            //TODO: cache
            var all = await _identityResourceQueries.GetAllAsync(site.Id.ToString()).ConfigureAwait(false);

            return all;
        }
    }
}
