using cloudscribe.Web.StaticFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Microsoft.AspNetCore.Builder
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeWebStaticFiles(
           this IServiceCollection services
           )
        {
            services.AddScoped<cloudscribe.Versioning.IVersionProvider, VersionProvider>();

            return services;
        }


        public static IApplicationBuilder UseCloudscribeCommonStaticFiles(this IApplicationBuilder builder)
        {

            builder.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new EmbeddedFileResolvingProvider(typeof(EmbeddedFileResolvingProvider).GetTypeInfo().Assembly, "cloudscribe.Web.StaticFiles")
                ,
                RequestPath = new PathString("/cr")
            });

            return builder;
        }
    }

    
}
