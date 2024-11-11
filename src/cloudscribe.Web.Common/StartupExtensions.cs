﻿using cloudscribe.Web.Common;
using cloudscribe.Web.Common.Analytics;
using cloudscribe.Web.Common.Analytics.GA4;
using cloudscribe.Web.Common.Components;
using cloudscribe.Web.Common.Helpers;
//using cloudscribe.Web.Common.Http;
using cloudscribe.Web.Common.Models;
using cloudscribe.Web.Common.Razor;
using cloudscribe.Web.Common.Recaptcha;
using cloudscribe.Web.Common.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeCommmon(
            this IServiceCollection services,
            IConfiguration configuration = null)
        {
            //services.TryAddSingleton<IDateTimeZoneProvider>(new DateTimeZoneCache(TzdbDateTimeZoneSource.Default));
            //services.TryAddSingleton<IHttpClientProvider, DefaultHttpClientProvider>();

            services.AddHttpClient();
            services.AddHttpClient("google-analytics", c =>
            {
                c.BaseAddress = new Uri("https://www.google-analytics.com/");
            });
            services.AddHttpClient<IRecaptchaValidationService, RecaptchaValidationService>(client =>
            {
                client.BaseAddress = new Uri("https://www.google.com/");

            });

            services.TryAddScoped<IRecaptchaServerSideValidator, RecaptchaServerSideValidator>();

            //services.TryAddScoped<ITimeZoneHelper, TimeZoneHelper>();
            services.TryAddScoped<IResourceHelper, ResourceHelper>();

            //services.TryAddScoped<ITimeZoneIdResolver, GmtTimeZoneIdResolver>();
            services.TryAddScoped<ITinyMceEditorOptionsResolver, DefaultTinyMceEditorOptionsResolver>();

            if (configuration != null)
            {
                services.Configure<TinyMceEditorOptions>(configuration.GetSection("TinyMceEditorOptions"));
                services.Configure<BannerImageMap>(configuration.GetSection("BannerImageMap"));
                services.TryAddScoped<IBannerService, ConfigBannerService>();

                services.Configure<GoogleAnalyticsOptions>(configuration.GetSection("GoogleAnalyticsOptions"));
                services.Configure<GoogleAnalyticsGA4Options>(configuration.GetSection("GoogleAnalyticsGA4Options"));

            }
            else
            {
                services.Configure<TinyMceEditorOptions>(c =>
                {  });
            }

            services.AddScoped<GoogleAnalyticsApiService>();
            services.AddScoped<GoogleAnalyticsHelper>();
            services.AddScoped<GoogleAnalyticsGA4Helper>();

            services.AddScoped<cloudscribe.Versioning.IVersionProvider, CommonVersionProvider>();

            services.TryAddScoped<IViewRendererRouteProvider, DefaultViewRendererRouteProvider>();
            services.TryAddScoped<ViewRenderer>();

            return services;
        }

        //public static RazorViewEngineOptions AddCloudscribeCommonEmbeddedViews(this RazorViewEngineOptions options)
        //{
        //    options.FileProviders.Add(new EmbeddedFileProvider(
        //           typeof(CloudscribeCommonResources).GetTypeInfo().Assembly,
        //           "cloudscribe.Web.Common"
        //       ));

        //    return options;
        //}

        //public static IApplicationBuilder UseCloudscribeCommonStaticFiles(this IApplicationBuilder builder)
        //{

        //    builder.UseStaticFiles(new StaticFileOptions()
        //    {
        //        FileProvider = new EmbeddedFileResolvingProvider(typeof(CloudscribeCommonResources).GetTypeInfo().Assembly,"cloudscribe.Web.Common")
        //        , RequestPath = new PathString("/cr")
        //    });

        //    return builder;
        //}
    }
}
