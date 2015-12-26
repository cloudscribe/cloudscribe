// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-26
// Last Modified:			2015-12-26
// 


using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Logging.EF
{
    public static class DbInitializer
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<LoggingDbContext>();
                bool didCreatedDb = await db.Database.EnsureCreatedAsync();
                if(!didCreatedDb)
                {
                    await db.Database.MigrateAsync();
                }
                
            }
        }
    }
}
