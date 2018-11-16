using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using Microsoft.Extensions.DependencyInjection;

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public static  class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeCoreEFCommon(
            this IServiceCollection services,
            bool useSingletonLifetime = false
            )
        {
            // there are some scenarios where we want to use singleton
            // such as console apps used in scheduled tasks or cron jobs
            // these implementations can be used as singleton or scoped
            if(useSingletonLifetime)
            {
                services.AddSingleton<ISiteCommands, SiteCommands>();
                services.AddSingleton<ISiteQueries, SiteQueries>();

                services.AddSingleton<IUserCommands, UserCommands>();
                services.AddSingleton<IUserQueries, UserQueries>();

                services.AddSingleton<IGeoCommands, GeoCommands>();
                services.AddSingleton<IGeoQueries, GeoQueries>();
            }
            else
            {
                services.AddScoped<ISiteCommands, SiteCommands>();
                services.AddScoped<ISiteQueries, SiteQueries>();

                services.AddScoped<IUserCommands, UserCommands>();
                services.AddScoped<IUserQueries, UserQueries>();

                services.AddScoped<IGeoCommands, GeoCommands>();
                services.AddScoped<IGeoQueries, GeoQueries>();
            }
            
            // graphql likes singletons so these are alternate singleton versions
            // we use different interfaces for this case because grpahql runs in the same web app
            // where other services expect scoped
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
