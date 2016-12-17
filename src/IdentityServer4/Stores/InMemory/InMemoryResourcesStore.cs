// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Stores.InMemory
{
    /// <summary>
    /// In-memory resource store
    /// </summary>
    public class InMemoryResourcesStore : IResourceStore
    {
        private readonly IEnumerable<IdentityResource> _identityResources;
        private readonly IEnumerable<ApiResource> _apiResources;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryResourcesStore" /> class.
        /// </summary>
        /// <param name="resources">The resources.</param>
        public InMemoryResourcesStore(IEnumerable<IdentityResource> identityResources = null, IEnumerable<ApiResource> apiResources = null)
        {
            _identityResources = identityResources ?? Enumerable.Empty<IdentityResource>();
            _apiResources = apiResources ?? Enumerable.Empty<ApiResource>();
        }

        public Task<Resources> GetAllResources()
        {
            var result = new Resources(_identityResources, _apiResources);
            return Task.FromResult(result);
        }

        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            var api = from a in _apiResources
                      where a.Name == name
                      select a;
            return Task.FromResult(api.FirstOrDefault());
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> names)
        {
            if (names == null) throw new ArgumentNullException(nameof(names));

            var identity = from i in _identityResources
                           where names.Contains(i.Name)
                           select i;

            return Task.FromResult(identity);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> names)
        {
            if (names == null) throw new ArgumentNullException(nameof(names));

            var api = from a in _apiResources
                      from s in a.Scopes
                      where names.Contains(s.Name)
                      select a;

            return Task.FromResult(api);
        }
    }
}
