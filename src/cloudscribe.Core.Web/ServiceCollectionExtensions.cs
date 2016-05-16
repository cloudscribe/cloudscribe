// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-05-07
// Last Modified:			2016-05-08
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Setup;
using cloudscribe.Core.Web;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Components.Editor;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Core.Web.Navigation;
using cloudscribe.Web.Common.Razor;
using cloudscribe.Web.Navigation;
using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeCore(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<MultiTenantOptions>(configuration.GetSection("MultiTenantOptions"));
            services.Configure<SiteConfigOptions>(configuration.GetSection("SiteConfigOptions"));
            
            services.Configure<UIOptions>(configuration.GetSection("UIOptions"));
            services.Configure<CkeditorOptions>(configuration.GetSection("CkeditorOptions"));
            

            services.AddScoped<ITimeZoneResolver, RequestTimeZoneResolver>();
            //services.AddMultitenancy<SiteSettings, SiteResolver>();
            services.Configure<CachingSiteResolverOptions>(configuration.GetSection("CachingSiteResolverOptions"));
            services.AddMultitenancy<SiteSettings, CachingSiteResolver>();
            services.AddSingleton<IOptions<IdentityOptions>, SiteIdentityOptionsResolver>();

            services.AddScoped<SiteManager, SiteManager>();
            
            services.AddScoped<GeoDataManager, GeoDataManager>();
            services.AddScoped<SystemInfoManager, SystemInfoManager>();
            services.AddScoped<IpAddressTracker, IpAddressTracker>();

            services.AddScoped<SiteDataProtector>();
            
            services.AddIdentity<SiteUser, SiteRole>()
                .AddUserStore<UserStore<SiteUser>>()
                .AddRoleStore<RoleStore<SiteRole>>()
                .AddUserManager<SiteUserManager<SiteUser>>()
                .AddRoleManager<SiteRoleManager<SiteRole>>();

            services.AddSingleton<IOptions<IdentityOptions>, SiteIdentityOptionsResolver>();

            services.AddScoped<IUserClaimsPrincipalFactory<SiteUser>, SiteUserClaimsPrincipalFactory<SiteUser, SiteRole>>();
            services.AddScoped<IPasswordHasher<SiteUser>, SitePasswordHasher<SiteUser>>();
            services.AddScoped<SiteSignInManager<SiteUser>, SiteSignInManager<SiteUser>>();
            services.AddScoped<SiteAuthCookieValidator, SiteAuthCookieValidator>();
            services.AddScoped<SiteCookieAuthenticationEvents, SiteCookieAuthenticationEvents>();
            services.AddSingleton<IAntiforgeryTokenStore, SiteAntiforgeryTokenStore>();
            
            services.AddCloudscribePagination();

            services.AddScoped<IVersionProviderFactory, VersionProviderFactory>();
            services.AddScoped<IVersionProvider, CloudscribeCoreVersionProvider>();

            services.AddTransient<IEmailTemplateService, HardCodedEmailTemplateService>();
            services.AddTransient<ISiteMessageEmailSender, SiteEmailMessageSender>();
            services.AddTransient<ISmsSender, SiteSmsSender>();

            services.AddSingleton<IThemeListBuilder, SiteThemeListBuilder>();
            services.AddSingleton<IRazorViewEngine, CoreViewEngine>();

            services.AddScoped<INodeUrlPrefixProvider, FolderTenantNodeUrlPrefixProvider>();
            services.AddCloudscribeNavigation(configuration);

            return services;
        }
    }
}
