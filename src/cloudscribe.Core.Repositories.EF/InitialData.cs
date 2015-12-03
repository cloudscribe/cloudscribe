// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-03
// Last Modified:			2015-12-03
// 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//using Microsoft.AspNet.Identity;
using Microsoft.Data.Entity;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;

namespace cloudscribe.Core.Repositories.EF
{
    public static class InitialData
    {

        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<CoreDbContext>();

                if (await db.Database.EnsureCreatedAsync())
                {
                    await EnsureData(serviceProvider);
 
                }
            }
        }

        private static async Task EnsureData(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<CoreDbContext>();
                int count = await db.Countries.CountAsync<GeoCountry>();
                if(count == 0)
                {
                    foreach(GeoCountry c in cloudscribe.Core.Models.InitialData.BuildCountryList())
                    {
                        db.Countries.Add(c);
                    }
                }

                count = await db.States.CountAsync<GeoZone>();
                if (count == 0)
                {
                    foreach (GeoZone c in cloudscribe.Core.Models.InitialData.BuildStateList())
                    {
                        db.States.Add(c);
                    }
                }

                count = await db.Languages.CountAsync<Language>();
                if (count == 0)
                {
                    foreach (Language c in cloudscribe.Core.Models.InitialData.BuildLanguageList())
                    {
                        db.Languages.Add(c);
                    }
                }

                count = await db.Currencies.CountAsync<Currency>();
                if (count == 0)
                {
                    foreach (Currency c in cloudscribe.Core.Models.InitialData.BuildCurrencyList())
                    {
                        db.Currencies.Add(c);
                    }
                }

                // go ahead and save
                int rowsAffected = await db.SaveChangesAsync();

                count = await db.Sites.CountAsync<SiteSettings>();
                if (count == 0)
                {
                    // TODO: create first site

                    // TODO: ensure roles

                    // TODO: ensure admin user
                }
            }

           
        }

        
    }
}
