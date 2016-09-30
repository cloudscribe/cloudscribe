using cloudscribe.Core.Identity;
using IdentityServer4.cloudscribeIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeIdentityServerIntegration(this IServiceCollection services)
        {
            services.TryAddScoped<IIdentityServerIntegration, Integration>();

            return services;
        }
    }
}
