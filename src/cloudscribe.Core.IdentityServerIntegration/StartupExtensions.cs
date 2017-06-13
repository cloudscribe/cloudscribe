
using cloudscribe.Core.Identity;
using cloudscribe.Core.IdentityServerIntegration;
using cloudscribe.Core.IdentityServerIntegration.Services;
using cloudscribe.Core.Models;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Hosting;
using IdentityServer4.Infrastructure;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;

// https://github.com/IdentityServer/IdentityServer4/issues/19

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IIdentityServerBuilder AddCloudscribeIdentityServerIntegration(this IIdentityServerBuilder builder)   
        {
            builder.Services.AddScoped<IIdentityServerIntegration, CloudscribeIntegration>();

            builder.Services.Configure<IdentityServerOptions>(options =>
            {
                //options.AuthenticationOptions.AuthenticationScheme = AuthenticationScheme.Application;
                options.Authentication.AuthenticationScheme = AuthenticationScheme.Application;
            });

            builder.Services.AddSingleton<IEndpointRouter>(resolver =>
            {
                return new MultiTenantEndpointRouter(CustomConstants.EndpointPathToNameMap,
                    resolver.GetRequiredService<IdentityServerOptions>(),
                    resolver.GetServices<EndpointMapping>(),
                    resolver.GetService<IOptions<MultiTenantOptions>>(),
                    resolver.GetRequiredService<ILogger<MultiTenantEndpointRouter>>());
            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Cookies.ApplicationCookie.AuthenticationScheme = AuthenticationScheme.Application;
                options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
            });

            builder.AddResourceOwnerValidator<ResourceOwnerPasswordValidator<SiteUser>>();
            builder.Services.AddTransient<IProfileService, ProfileService<SiteUser>>();
            builder.Services.AddTransient<IJwtClaimsProcessor<SiteUser>, DefaultJwtClaimsProcessor>();

            builder.Services.AddTransient<ISecurityStampValidator, cloudscribe.Core.IdentityServerIntegration.SecurityStampValidator<SiteUser>>();

            builder.Services.AddScoped<IMatchAuthorizeProtocolRoutePaths, MultiTenantAuthorizeProtocolRouteMatcher>();
            builder.Services.AddScoped<IMatchEndSessionProtocolRoutePaths, MultiTenantEndSessionProtocolRouteMatcher>();

            builder.Services.AddScoped<ApiResourceManager, ApiResourceManager>();
            builder.Services.AddScoped<IdentityResourceManager, IdentityResourceManager>();
            builder.Services.AddScoped<ClientsManager, ClientsManager>();

            //builder.Services.AddTransientDecorator<ICorsPolicyProvider, CorsPolicyProvider>();
            builder.Services.AddTransient<ICorsPathValidator, CorsPathValidator>();

            return builder;
        }



        //public static IServiceCollection AddCloudscribeIdentityServerIntegration(this IServiceCollection services)
        //{
        //    services.AddScoped<IIdentityServerIntegration, Integration>();

        //    return services;
        //}

        //[Obsolete("AddEmbeddedViewsForCloudscribeIdentityServerIntegration is deprecated, please use AddEmbeddedBootstrap3ViewsForCloudscribeCoreIdentityServerIntegration instead.")]
        //public static RazorViewEngineOptions AddEmbeddedViewsForCloudscribeIdentityServerIntegration(this RazorViewEngineOptions options)
        //{
        //    options.AddEmbeddedBootstrap3ViewsForCloudscribeCoreIdentityServerIntegration();
        //    //options.FileProviders.Add(new EmbeddedFileProvider(
        //    //        typeof(CloudscribeIntegration).GetTypeInfo().Assembly,
        //    //        "cloudscribe.Core.IdentityServerIntegration"
        //    //    ));

        //    return options;
        //}

    }
}
