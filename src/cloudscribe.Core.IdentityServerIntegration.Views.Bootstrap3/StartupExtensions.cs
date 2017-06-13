using cloudscribe.Core.IdentityServerIntegration.Views.Bootstrap3;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        [Obsolete("AddEmbeddedBootstrap3ViewsForCloudscribeCoreIdentityServerIntegration is deprecated, please use AddCloudscribeCoreIdentityServerIntegrationBootstrap3Views instead.")]
        public static RazorViewEngineOptions AddEmbeddedBootstrap3ViewsForCloudscribeCoreIdentityServerIntegration(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(Bootstrap3).GetTypeInfo().Assembly,
                    "cloudscribe.Core.IdentityServerIntegration.Views.Bootstrap3"
                ));

            return options;
        }

        public static RazorViewEngineOptions AddCloudscribeCoreIdentityServerIntegrationBootstrap3Views(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(Bootstrap3).GetTypeInfo().Assembly,
                    "cloudscribe.Core.IdentityServerIntegration.Views.Bootstrap3"
                ));

            return options;
        }
    }
}
