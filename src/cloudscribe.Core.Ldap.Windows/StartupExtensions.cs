using cloudscribe.Core.Ldap.Windows;
using cloudscribe.Core.Models.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeLdapWindowsSupport(
            this IServiceCollection services,
            IConfiguration config)
        {


            services.AddScoped<ILdapHelper, LdapHelper>();


            return services;
        }

    }
}
