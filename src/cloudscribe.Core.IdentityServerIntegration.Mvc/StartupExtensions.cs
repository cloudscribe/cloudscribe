using cloudscribe.Core.IdentityServerIntegration.Mvc;
using cloudscribe.Web.Common.Setup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IIdentityServerBuilder AddCloudscribeIdentityServerIntegrationMvc(this IIdentityServerBuilder builder)
        {
            builder.AddCloudscribeIdentityServerIntegrationCommon();

            builder.Services.AddScoped<IVersionProvider, VersionProvider>();

            return builder;
        }
    }
}
