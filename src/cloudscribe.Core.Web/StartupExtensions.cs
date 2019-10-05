// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-05-07
// Last Modified:			2018-06-19
// 


using cloudscribe.Core.DataProtection;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Identity;
using cloudscribe.Core.Web.Analytics;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Core.Web.Design;
using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Core.Web.Mvc.Components;
using cloudscribe.Core.Web.Navigation;
using cloudscribe.Email;
using cloudscribe.Email.ElasticEmail;
using cloudscribe.Email.Mailgun;
using cloudscribe.Email.SendGrid;
using cloudscribe.Email.Smtp;
using cloudscribe.Web.Common.Components;
using cloudscribe.Web.Common.Models;
using cloudscribe.Web.Common.Razor;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.Navigation.Caching;
using cloudscribe.Web.SiteMap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NodaTime;
using NodaTime.TimeZones;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeCoreMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCloudscribeCoreCommon(configuration);
            //services.AddScoped<IVersionProvider, ControllerVersionInfo>();

            services.TryAddScoped<IDecideErrorResponseType, DefaultErrorResponseTypeDecider>();


            return services;
        }

        


        public static IServiceCollection AddCloudscribeCoreCommon(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options =>
            {
                options.ConstraintMap.Add("sitefolder", typeof(SiteFolderRouteConstraint));
            });

            services.AddMemoryCache();
            // this uses memorycache with try add so a different implementation of IDistributedCache can be added beofre or after this and override it
            services.AddDistributedMemoryCache();


            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddOptions();
            
            services.Configure<MultiTenantOptions>(configuration.GetSection("MultiTenantOptions"));
            services.Configure<NewUserOptions>(configuration.GetSection("NewUserOptions"));

            services.Configure<RecaptchaKeys>(configuration.GetSection("RecaptchaKeys"));
            services.Configure<SiteConfigOptions>(configuration.GetSection("SiteConfigOptions"));
            services.Configure<UIOptions>(configuration.GetSection("UIOptions"));

            services.Configure<CustomSocialAuthSchemes>(configuration.GetSection("CustomSocialAuthSchemes"));

            services.Configure<CoreIconConfig>(configuration.GetSection("CoreIconConfig"));
            services.Configure<CoreThemeConfig>(configuration.GetSection("CoreThemeConfig"));
            services.TryAddScoped<ICoreThemeHelper, CoreThemeHelper>();

            services.Configure<CachingSiteResolverOptions>(configuration.GetSection("CachingSiteResolverOptions"));


            
            //services.TryAddScoped<ISiteContextResolver, SiteContextResolver>();
            services.TryAddScoped<ISiteContextResolver, CachingSiteContextResolver>();

            //services.AddMultitenancy<SiteContext, CachingSiteResolver>();
            services.AddMultitenancy<SiteContext, SiteResolver>();

            services.AddScoped<CacheHelper, CacheHelper>();

            services.AddScoped<SiteEvents, SiteEvents>();
            services.AddScoped<SiteManager, SiteManager>();
            services.TryAddScoped<IAccountService, AccountService>();
            services.AddScoped<GeoDataManager, GeoDataManager>();
            services.AddScoped<SystemInfoManager, SystemInfoManager>();
            services.AddScoped<IpAddressTracker, IpAddressTracker>();
            services.AddScoped<SiteTimeZoneService, SiteTimeZoneService>();

            services.AddScoped<SiteDataProtector>();

            services.TryAddScoped<ICkeditorOptionsResolver, SiteCkeditorOptionsResolver>();
            //TODO: remove in a future version
            // services.AddScoped<cloudscribe.Web.Common.ITimeZoneIdResolver, RequestTimeZoneIdResolver>();

            services.TryAddSingleton<IDateTimeZoneProvider>(new DateTimeZoneCache(TzdbDateTimeZoneSource.Default));
            services.AddScoped<cloudscribe.DateTimeUtils.ITimeZoneIdResolver, SiteTimeZoneIdResolver>();
            services.TryAddScoped<cloudscribe.DateTimeUtils.ITimeZoneHelper, cloudscribe.DateTimeUtils.TimeZoneHelper>();

            services.TryAddScoped<IHandleCustomRegistration, NoRegistrationCustomization>();
            services.TryAddScoped<IHandleCustomUserInfo, NoUserInfoCustomization>();
            services.TryAddScoped<IHandleCustomUserInfoAdmin, NoUserEditCustomization>();

            services.TryAddScoped<IHandleAccountAnalytics, GoogleAccountAnalytics>();


            //services.TryAddScoped<IViewRendererRouteProvider, SiteViewRendererRouteProvider>();
            services.AddCloudscribeCommmon(configuration);
            
            services.AddCloudscribePagination();

            services.AddScoped<cloudscribe.Versioning.IVersionProviderFactory, cloudscribe.Versioning.VersionProviderFactory>();
            services.AddScoped<cloudscribe.Versioning.IVersionProvider, CloudscribeCoreVersionProvider>();
            services.AddScoped<cloudscribe.Versioning.IVersionProvider, DataStorageVersionInfo>();
            services.AddScoped<cloudscribe.Versioning.IVersionProvider, IdentityVersionInfo>();

            services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));
            services.TryAddScoped<ISmtpOptionsProvider, SiteSmtpOptionsResolver>();
            services.TryAddScoped<IEmailSenderResolver, SiteEmailSenderResolver>();

            services.AddScoped<ISiteMessageEmailSender, SiteEmailMessageSender>();

            //services.AddTransient<ISiteMessageEmailSender, FakeSiteEmailSender>();
            services.TryAddScoped<ISendGridOptionsProvider, SiteSendGridOptionsProvider>();
            services.TryAddScoped<IMailgunOptionsProvider, SiteMailgunOptionsProvider>();
            services.TryAddScoped<IElasticEmailOptionsProvider, SiteElasticEmailOptionsProvider>();
            services.AddCloudscribeEmailSenders(configuration);
            
            services.TryAddSingleton<IThemeListBuilder, SiteThemeListBuilder>();
            //services.AddSingleton<IRazorViewEngine, CoreViewEngine>();
            services.TryAddScoped<ViewRenderer>();

            services.AddSingleton<IOptions<NavigationOptions>, SiteNavigationOptionsResolver>();
            services.AddScoped<ITreeCacheKeyResolver, SiteNavigationCacheKeyResolver>();
            //services.AddScoped<INodeUrlPrefixProvider, FolderTenantNodeUrlPrefixProvider>();
            services.AddCloudscribeNavigation(configuration);

            services.AddScoped<ISiteMapNodeService, NavigationTreeSiteMapNodeService>();

            // Identity ***
            services.TryAddScoped<ISiteAccountCapabilitiesProvider, SiteAccountCapabilitiesProvider>();
            services.AddCloudscribeIdentity();

            services.AddScoped<IUserContextResolver, UserContextResolver>();
            services.AddScoped<ISiteIdResolver, SiteIdResolver>();

            services.TryAddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.TryAddScoped<IRecaptchaKeysProvider, SiteRecaptchaKeysProvider>();

            services.TryAddScoped<INewUserDisplayNameResolver, DefaultNewUserDisplayNameResolver>();

            services.AddCloudscribeFileManagerIntegration(configuration);

            services.AddScoped<IGuardNeededRoles, AdministratorsRoleGuard>();

            services.TryAddScoped<ILdapHelper, NotImplementedLdapHelper>();

            services.TryAddScoped<ILdapSslCertificateValidator, AlwaysValidLdapSslCertificateValidator>();

            return services;
        }

        /// <summary>
        /// This method adds an embedded file provider to the RazorViewOptions to be able to load the cloudscribe Core related views.
        /// If you download and install the views below your view folder you don't need this method and you can customize the views.
        /// You can get the views from https://github.com/joeaudette/cloudscribe/tree/master/src/cloudscribe.Core.Web/Views
        /// </summary>
        /// <param name="options"></param>
        /// <returns>RazorViewEngineOptions</returns>
        //[Obsolete("AddEmbeddedViewsForCloudscribeCore is deprecated, please use AddEmbeddedBootstrap3ViewsForCloudscribeCore instead.")]
        //public static RazorViewEngineOptions AddEmbeddedViewsForCloudscribeCore(this RazorViewEngineOptions options)
        //{
        //    //options.FileProviders.Add(new EmbeddedFileProvider(
        //    //        typeof(SiteManager).GetTypeInfo().Assembly,
        //    //        "cloudscribe.Core.Web"
        //    //    ));
        //    options.AddEmbeddedBootstrap3ViewsForCloudscribeCore();

        //    return options;
        //}

        
        
        /// this strategy to support views under /Sys really is a relic from mvc 5 not really needed now
        public static RazorViewEngineOptions AddCloudscribeViewLocationFormats(this RazorViewEngineOptions options)
        {
            options.ViewLocationFormats.Add("/Views/Sys/{1}/{0}" + RazorViewEngine.ViewExtension);
            options.ViewLocationFormats.Add("/Views/Sys/Shared/{0}" + RazorViewEngine.ViewExtension);

            options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Sys/{1}/{0}" + RazorViewEngine.ViewExtension);
            options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Sys/Shared/{0}" + RazorViewEngine.ViewExtension);
            options.AreaViewLocationFormats.Add("/Views/Sys/Shared/{0}" + RazorViewEngine.ViewExtension);

            return options;
        }

        public static AuthorizationOptions AddCloudscribeCoreDefaultPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy(
                    "ServerAdminPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("ServerAdmins");
                    });

            options.AddPolicy(
                "CoreDataPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("ServerAdmins");
                });

            options.AddPolicy(
                "AdminMenuPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("ServerAdmins", "Administrators", "Role Administrators", "Content Administrators");
                });

            options.AddPolicy(
                "AdminPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("ServerAdmins", "Administrators");
                });

            options.AddPolicy(
                "UserManagementPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("ServerAdmins", "Administrators");
                });

            options.AddPolicy(
                "UserLookupPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("ServerAdmins", "Administrators");
                });

            options.AddPolicy(
                "RoleAdminPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("Role Administrators", "Administrators");
                });

            options.AddPolicy(
                "RoleLookupPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("Role Administrators", "Administrators", "Content Administrators");
                });

            return options;
        }

        [Obsolete("This method is deprecated, you should use services.AddCloudscribeCoreMvc instead.")]
        public static IServiceCollection AddCloudscribeCore(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddCloudscribeCoreCommon(configuration);

            return services;
        }

    }
}
