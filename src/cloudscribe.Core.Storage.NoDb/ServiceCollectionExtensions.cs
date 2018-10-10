// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-09
// Last Modified:           2017-10-06
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Storage.NoDb;
using NoDb;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeCoreNoDbStorage(
            this IServiceCollection services,
            bool useSingletons = false
            )
        {
            if(useSingletons)
            {
                //graphql likes singletons so these are alternate singleton versions
                // the services can work as any lifetime

                services.AddSingleton<IBasicQueries<SiteSettings>, CrossProjectQueries<SiteSettings>>();
                services.AddSingleton<IBasicQueries<SiteHost>, CrossProjectQueries<SiteHost>>();

                services.AddNoDbSingleton<SiteSettings>();
                services.AddNoDbSingleton<SiteHost>();

                services.AddNoDbSingleton<SiteUser>();
                services.AddNoDbSingleton<SiteRole>();
                services.AddNoDbSingleton<UserRole>();
                services.AddNoDbSingleton<UserClaim>();
                services.AddNoDbSingleton<UserLogin>();
                services.AddNoDbSingleton<UserLocation>();
                services.AddNoDbSingleton<UserToken>();

                services.AddNoDbSingleton<GeoCountry>();
                services.AddNoDbSingleton<GeoZone>();
                
                services.AddSingleton<ISiteCommandsSingleton, SiteCommands>();
                services.AddSingleton<ISiteQueriesSingleton, SiteQueries>();

                services.AddSingleton<IUserCommandsSingleton, UserCommands>();
                services.AddSingleton<IUserQueriesSingleton, UserQueries>();

                services.AddSingleton<IGeoCommandsSingleton, GeoCommands>();
                services.AddSingleton<IGeoQueriesSingleton, GeoQueries>();
            }
            else
            {
                services.AddScoped<IBasicQueries<SiteSettings>, CrossProjectQueries<SiteSettings>>();
                services.AddScoped<IBasicQueries<SiteHost>, CrossProjectQueries<SiteHost>>();

                services.AddNoDb<SiteSettings>();
                services.AddNoDb<SiteHost>();

                services.AddNoDb<SiteUser>();
                services.AddNoDb<SiteRole>();
                services.AddNoDb<UserRole>();
                services.AddNoDb<UserClaim>();
                services.AddNoDb<UserLogin>();
                services.AddNoDb<UserLocation>();
                services.AddNoDb<UserToken>();

                services.AddNoDb<GeoCountry>();
                services.AddNoDb<GeoZone>();
                
                
            }

            services.AddScoped<ISiteCommands, SiteCommands>();
            services.AddScoped<ISiteQueries, SiteQueries>();

            services.AddScoped<IUserCommands, UserCommands>();
            services.AddScoped<IUserQueries, UserQueries>();

            services.AddScoped<IGeoCommands, GeoCommands>();
            services.AddScoped<IGeoQueries, GeoQueries>();

            services.AddScoped<IDataPlatformInfo, DataPlatformInfo>();
            
            return services;
        }
    }
}
