// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-10-12
// Last Modified:           2016-10-12
// 

using IdentityServer4.Models;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CloudscribeIdentityServerIntegrationNoDbStorage
    {
        public static async Task InitializeDatabaseAsync(
            IServiceProvider serviceProvider
            , string siteId = null
            , IEnumerable<Client> initialClients = null,
            IEnumerable<Scope> initialScopes = null
            )
        {
            using (var serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var scopeCommands = serviceScope.ServiceProvider.GetRequiredService<IBasicCommands<Scope>>();
                var scopeQueries = serviceScope.ServiceProvider.GetRequiredService<IBasicQueries<Scope>>();
                var clientCommands = serviceScope.ServiceProvider.GetRequiredService<IBasicCommands<Client>>();
                var clientQueries = serviceScope.ServiceProvider.GetRequiredService<IBasicQueries<Client>>();

                if (!string.IsNullOrEmpty(siteId))
                {
                    if (initialClients != null) 
                    {
                        var allClients = await clientQueries.GetAllAsync(siteId).ConfigureAwait(false) ;
                        if(allClients.ToList().Count == 0)
                        {
                            foreach (var client in initialClients)
                            {
                                await clientCommands.CreateAsync(siteId, client.ClientId, client).ConfigureAwait(false);  
                            }
                            
                        }
                        
                    }

                    if (initialScopes != null) 
                    {
                        var allScopes = await scopeQueries.GetAllAsync(siteId).ConfigureAwait(false);
                        if (allScopes.ToList().Count == 0)
                        {
                            foreach (var scope in initialScopes)
                            {
                                await scopeCommands.CreateAsync(siteId, scope.Name, scope).ConfigureAwait(false);
                            }
                        }
 
                    }
                }


            }

        }

    }
}
