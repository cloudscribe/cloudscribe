// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Stores
{
    /// <summary>
    /// Resource retrieval
    /// </summary>
    public interface IResourceStore
    {
        /// <summary>
        /// Gets identity resources by scope name.
        /// </summary>
        Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames);
        
        /// <summary>
        /// Gets API resources by scope name.
        /// </summary>
        Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames);

        /// <summary>
        /// Finds the API resource by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Task<ApiResource> FindApiResourceAsync(string name);

        /// <summary>
        /// Gets all resources.
        /// </summary>
        Task<Resources> GetAllResources();
    }
}