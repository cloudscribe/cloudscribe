using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
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
            , string siteId = null
            , IEnumerable<Client> initialClients = null,
            IEnumerable<Scope> initialScopes = null
            )
        {
            using (var serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var grantContext = serviceScope.ServiceProvider.GetRequiredService<IPersistedGrantDbContext>();
                try
                {
                    await grantContext.Database.MigrateAsync();
                }
                catch(System.NotImplementedException)
                {
                    grantContext.Database.Migrate();
                }
                

                var configContext = serviceScope.ServiceProvider.GetRequiredService<IConfigurationDbContext>();
                try
                {
                    await configContext.Database.MigrateAsync();
                }
                catch (System.NotImplementedException)
                {
                    configContext.Database.Migrate();
                }
                

                if(!string.IsNullOrEmpty(siteId))
                {
                    if ((initialClients != null) && (!configContext.Clients.Any(x => x.SiteId == siteId)))
                    {
                        foreach (var client in initialClients)
                        {
                            var c = client.ToEntity();
                            c.SiteId = siteId;
                            configContext.Clients.Add(c);
                        }
                        await configContext.SaveChangesAsync();
                    }

                    if ((initialScopes != null) && (!configContext.Scopes.Any(x => x.SiteId == siteId)))
                    {
                        foreach (var scope in initialScopes)
                        {
                            var s = scope.ToEntity();
                            s.SiteId = siteId;
                            configContext.Scopes.Add(s);
                        }
                        await configContext.SaveChangesAsync();
                    }
                }


            }

        }
    }
}
