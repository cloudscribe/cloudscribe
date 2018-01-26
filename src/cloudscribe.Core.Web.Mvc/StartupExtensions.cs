using cloudscribe.Core.Web.Mvc;
using cloudscribe.Core.Web.Mvc.Components;
using cloudscribe.Web.Common.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

            services.TryAddScoped<IDecideErrorResponseType, DefaultErrorResponseTypeDecider>();


            return services;
        }
    }
}
