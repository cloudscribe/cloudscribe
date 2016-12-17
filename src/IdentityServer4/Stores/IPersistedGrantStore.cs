// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Stores
{
    /// <summary>
    /// Interface for persisting any type of grant.
    /// </summary>
    public interface IPersistedGrantStore
    {
        Task StoreAsync(PersistedGrant grant);

        Task<PersistedGrant> GetAsync(string key);
        Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId);

        Task RemoveAsync(string key);
        Task RemoveAllAsync(string subjectId, string clientId);
        Task RemoveAllAsync(string subjectId, string clientId, string type);
    }
}
