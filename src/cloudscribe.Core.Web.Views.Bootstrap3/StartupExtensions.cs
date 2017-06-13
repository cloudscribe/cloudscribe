using cloudscribe.Core.Web.Views.Bootstrap3;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        [Obsolete("AddEmbeddedBootstrap3ViewsForCloudscribeCore is deprecated, please use AddCloudscribeCoreBootstrap3Views instead.")]
        public static RazorViewEngineOptions AddEmbeddedBootstrap3ViewsForCloudscribeCore(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(Bootstrap3).GetTypeInfo().Assembly,
                    "cloudscribe.Core.Web.Views.Bootstrap3"
                ));

            return options;
        }

        public static RazorViewEngineOptions AddCloudscribeCoreBootstrap3Views(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(Bootstrap3).GetTypeInfo().Assembly,
                    "cloudscribe.Core.Web.Views.Bootstrap3"
                ));

            return options;
        }
    }
}
