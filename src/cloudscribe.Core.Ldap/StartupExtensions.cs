﻿using cloudscribe.Core.Ldap;
using cloudscribe.Core.Models.Identity;
using cloudscribe.Versioning;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeLdapSupport(
            this IServiceCollection services, 
            IConfiguration config)
        {
            services.AddScoped<ILdapHelper, LdapHelper>();
            services.AddScoped<IVersionProvider, VersionProvider>();

            return services;
        }

    }
}
