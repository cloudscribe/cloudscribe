// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-09
// Last Modified:           2016-05-09
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Storage.NoDb;
using Microsoft.Extensions.DependencyInjection;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeCoreNoDbStorage(
            this IServiceCollection services
            )
        {

            services.AddNoDb<SiteSettings>();
            services.AddNoDb<SiteHost>();
            services.AddNoDb<SiteUser>();
            services.AddNoDb<SiteRole>();
            services.AddNoDb<UserRole>();
            services.AddNoDb<UserClaim>();
            services.AddNoDb<UserLogin>();
            services.AddNoDb<UserLocation>();

            services.AddNoDb<GeoCountry>();
            services.AddNoDb<GeoZone>();
            services.AddNoDb<Language>();
            services.AddNoDb<Currency>();

            services.AddScoped<IDataPlatformInfo, DataPlatformInfo>();

            services.AddScoped<IProjectRequestMap, DefaultProjectRequestMap>();
            services.AddScoped<IProjectResolver, DefaultProjectResolver>();
            services.AddScoped<ISiteCommands, SiteCommands>();
            services.AddScoped<ISiteQueries, SiteQueries>();

            services.AddScoped<IUserCommands, UserCommands>();
            services.AddScoped<IUserQueries, UserQueries>();

            services.AddScoped<IGeoCommands, GeoCommands>();
            services.AddScoped<IGeoQueries, GeoQueries>();

            return services;
        }
    }
}
