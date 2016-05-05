using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Components.Editor;
using cloudscribe.Core.Web.Components.Messaging;
using cloudscribe.Web.Common.Razor;
using cloudscribe.Web.Pagination;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web
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
            services.AddMultitenancy<SiteSettings, CachingSiteResolver>();
            services.AddSingleton<IOptions<IdentityOptions>, SiteIdentityOptionsProvider>();

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

            services.AddSingleton<IOptions<IdentityOptions>, SiteIdentityOptionsProvider>();

            services.AddScoped<IUserClaimsPrincipalFactory<SiteUser>, SiteUserClaimsPrincipalFactory<SiteUser, SiteRole>>();
            services.AddScoped<IPasswordHasher<SiteUser>, SitePasswordHasher<SiteUser>>();
            services.AddScoped<SiteSignInManager<SiteUser>, SiteSignInManager<SiteUser>>();
            services.AddScoped<SiteAuthCookieValidator, SiteAuthCookieValidator>();
            services.AddScoped<SiteCookieAuthenticationEvents, SiteCookieAuthenticationEvents>();



            services.AddTransient<IBuildPaginationLinks, PaginationLinkBuilder>();

            services.AddTransient<IEmailTemplateService, HardCodedEmailTemplateService>();
            services.AddTransient<ISiteMessageEmailSender, SiteEmailMessageSender>();
            services.AddTransient<ISmsSender, SiteSmsSender>();

            services.AddSingleton<IThemeListBuilder, SiteThemeListBuilder>();




            return services;
        }
    }
}
