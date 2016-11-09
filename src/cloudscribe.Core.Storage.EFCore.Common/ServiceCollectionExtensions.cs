using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public static  class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeCoreEFCommon(
            this IServiceCollection services
            )
        {
            
            services.AddScoped<ISiteCommands, SiteCommands>();
            services.AddScoped<ISiteQueries, SiteQueries>();

            services.AddScoped<IUserCommands, UserCommands>();
            services.AddScoped<IUserQueries, UserQueries>();

            services.AddScoped<IGeoCommands, GeoCommands>();
            services.AddScoped<IGeoQueries, GeoQueries>();

            services.TryAddScoped<ICoreTableNames, CoreTableNames>();

            return services;
        }
    }
}
