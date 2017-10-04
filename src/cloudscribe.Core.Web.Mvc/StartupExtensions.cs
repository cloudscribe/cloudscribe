using cloudscribe.Core.Web.Mvc;
using cloudscribe.Web.Common.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeCoreMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCloudscribeCoreCommon(configuration);
            services.AddScoped<IVersionProvider, ControllerVersionInfo>();


            return services;
        }
    }
}
