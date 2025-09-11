using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.FileManager.CoreIntegration;
using Microsoft.Extensions.Configuration;
using cloudscribe.FileManager.Web.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeFileManagerIntegration(
            this IServiceCollection services,
            IConfiguration configuration = null
            )
        {
            services.TryAddScoped<IMediaPathResolver, MediaPathResolver>();
            services.AddCloudscribeFileManager(configuration);

            services.AddScoped<cloudscribe.Versioning.IVersionProvider, VersionProvider>();

            return services;
        }
    }
}
