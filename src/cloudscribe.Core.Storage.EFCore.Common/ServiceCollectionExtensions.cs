using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using Microsoft.Extensions.DependencyInjection;

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


            //graphql likes singletons so these are alternate singleton versions
            // the services can work as any lifetime

            services.AddSingleton<IGeoCommandsSingleton, GeoCommands>();
            services.AddSingleton<IGeoQueriesSingleton, GeoQueries>();

            services.AddSingleton<IUserCommandsSingleton, UserCommands>();
            services.AddSingleton<IUserQueriesSingleton, UserQueries>();

            services.AddSingleton<ISiteCommandsSingleton, SiteCommands>();
            services.AddSingleton<ISiteQueriesSingleton, SiteQueries>();


            return services;
        }
    }
}
