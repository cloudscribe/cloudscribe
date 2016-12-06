// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-10-12
// Last Modified:           2016-12-06
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
            IEnumerable<ApiResource> initialApiResources = null,
            IEnumerable<IdentityResource> initialIdentityResources = null
            )
        {
            using (var serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var apiResourceCommands = serviceScope.ServiceProvider.GetRequiredService<IBasicCommands<ApiResource>>();
                var apiResourceQueries = serviceScope.ServiceProvider.GetRequiredService<IBasicQueries<ApiResource>>();
                var identityResourceCommands = serviceScope.ServiceProvider.GetRequiredService<IBasicCommands<IdentityResource>>();
                var identityResourceQueries = serviceScope.ServiceProvider.GetRequiredService<IBasicQueries<IdentityResource>>();
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

                    if (initialApiResources != null) 
                    {
                        var allApis = await apiResourceQueries.GetAllAsync(siteId).ConfigureAwait(false);
                        if (allApis.ToList().Count == 0)
                        {
                            foreach (var api in initialApiResources)
                            {
                                await apiResourceCommands.CreateAsync(siteId, api.Name, api).ConfigureAwait(false);
                            }
                        }
 
                    }

                    if (initialIdentityResources != null)
                    {
                        var all = await identityResourceQueries.GetAllAsync(siteId).ConfigureAwait(false);
                        if (all.ToList().Count == 0)
                        {
                            foreach (var res in initialIdentityResources)
                            {
                                await identityResourceCommands.CreateAsync(siteId, res.Name, res).ConfigureAwait(false);
                            }
                        }

                    }
                }


            }

        }

    }
}
