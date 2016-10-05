using cloudscribe.Core.IdentityServer.EFCore;
using cloudscribe.Core.IdentityServer.EFCore.DbContexts;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using cloudscribe.Core.IdentityServer.EFCore.Services;
using cloudscribe.Core.IdentityServer.EFCore.Stores;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CloudscribeIdentityServerIntegrationEFCoreStorage
    {
        public static async Task InitializeDatabaseAsync(
            IServiceProvider serviceProvider
            //,IEnumerable<Client> initialClients = null,
            //IEnumerable<Scope> initialScopes = null
            )
        {
            using (var serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var grantContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                await grantContext.Database.MigrateAsync();

                var configContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                await configContext.Database.MigrateAsync();

                // we can't do this because we don't have a tenantid to tag the data with
                //if ((initialClients != null) && (!configContext.Clients.Any()))
                //{
                //    foreach (var client in initialClients)
                //    {
                //        configContext.Clients.Add(client.ToEntity());
                //    }
                //    configContext.SaveChanges();
                //}

                //if ((initialScopes != null) && (!configContext.Scopes.Any()))
                //{
                //    foreach (var scope in initialScopes)
                //    {
                //        configContext.Scopes.Add(scope.ToEntity());
                //    }
                //    configContext.SaveChanges();
                //}

            }

        }
    }
}
