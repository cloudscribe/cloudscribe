using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.FileManager.CoreIntegration;
using Microsoft.Extensions.Configuration;
using cloudscribe.FileManager.Web.Models;
using cloudscribe.FileManager.Web;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeFileManagerIntegration(
            this IServiceCollection services,
            IConfigurationRoot configuration = null
            )
        {
            services.TryAddScoped<IMediaPathResolver, MediaPathResolver>();
            services.AddCloudscribeFileManager(configuration);

            return services;
        }
    }
}
