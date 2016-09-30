using cloudscribe.Core.Identity;
using IdentityServer4.cloudscribeIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeIdentityServerIntegration(this IServiceCollection services)
        {
            services.TryAddScoped<IIdentityServerIntegration, Integration>();

            return services;
        }

        public static RazorViewEngineOptions AddEmbeddedViewsForCloudscribeIdentityServerIntegration(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(Integration).GetTypeInfo().Assembly,
                    "IdentityServer4.cloudscribeIdentity"
                ));

            return options;
        }

    }
}
