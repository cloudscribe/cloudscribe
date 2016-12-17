// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Stores
{
    public static class IResourceStoreExtensions
    {
        public static async Task<Resources> FindResourcesByScopeAsync(this IResourceStore store, IEnumerable<string> scopeNames)
        {
            var identity = await store.FindIdentityResourcesByScopeAsync(scopeNames);
            var apiResources = await store.FindApiResourcesByScopeAsync(scopeNames);

            var apis = new List<ApiResource>();
            foreach (var apiResource in apiResources)
            {
                apis.Add(apiResource.CloneWithScopes(apiResource.Scopes.Where(x => scopeNames.Contains(x.Name))));
            }

            var resources = new Resources(identity, apis)
            {
                OfflineAccess = scopeNames.Contains(IdentityServerConstants.StandardScopes.OfflineAccess)
            };
            return resources;
        }

        public async static Task<Resources> FindEnabledResourcesByScopeAsync(this IResourceStore store, IEnumerable<string> scopeNames)
        {
            return (await store.FindResourcesByScopeAsync(scopeNames)).FilterEnabled();
        }

        public static async Task<Resources> GetAllEnabledResourcesAsync(this IResourceStore store)
        {
            var resources = await store.GetAllResources();
            return resources.FilterEnabled();
        }

        public async static Task<IEnumerable<IdentityResource>> FindEnabledIdentityResourcesByScopeAsync(this IResourceStore store, IEnumerable<string> scopeNames)
        {
            return (await store.FindIdentityResourcesByScopeAsync(scopeNames)).Where(x => x.Enabled).ToArray();
        }
    }
}