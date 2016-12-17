// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Threading.Tasks;

namespace IdentityServer4.Stores
{
    public static class IClientStoreExtensions
    {
        public static async Task<Client> FindEnabledClientByIdAsync(this IClientStore store, string clientId)
        {
            var client = await store.FindClientByIdAsync(clientId);
            if (client != null && client.Enabled == true) return client;

            return null;
        }
    }
}