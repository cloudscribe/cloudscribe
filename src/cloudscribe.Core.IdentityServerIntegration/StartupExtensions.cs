
using cloudscribe.Core.Identity;
using cloudscribe.Core.IdentityServerIntegration;
using cloudscribe.Core.IdentityServerIntegration.Services;
using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Setup;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Endpoints;
using IdentityServer4.Hosting;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

// https://github.com/IdentityServer/IdentityServer4/issues/19

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        [Obsolete("this method is deprected, please use .AddCloudscribeIdentityServerIntegrationMvc instead.")]
        public static IIdentityServerBuilder AddCloudscribeIdentityServerIntegration(this IIdentityServerBuilder builder)
        {
            builder.AddCloudscribeIdentityServerIntegrationCommon();

            return builder;
        }
        public static IIdentityServerBuilder AddCloudscribeIdentityServerIntegrationCommon(this IIdentityServerBuilder builder)   
        {
            builder.Services.AddScoped<IIdentityServerIntegration, CloudscribeIntegration>();

            builder.Services.Configure<IdentityServerOptions>(options =>
            {
               options.Authentication.AuthenticationScheme = AuthenticationScheme.Application;
               
 
            });

            builder.Services.AddSingleton<IEndpointRouter>(resolver =>
            {
                return new MultiTenantEndpointRouter(
                    resolver.GetService<IEnumerable<Endpoint>>(),
                    resolver.GetRequiredService<IdentityServerOptions>(),
                    resolver.GetService<IOptions<MultiTenantOptions>>(),
                    resolver.GetRequiredService<ILogger<MultiTenantEndpointRouter>>());
            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // commented out 2017-07-25 breaking change in 2.0
                //options.Cookies.ApplicationCookie.AuthenticationScheme = AuthenticationScheme.Application;
                options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
            });

            builder.AddResourceOwnerValidator<ResourceOwnerPasswordValidator<SiteUser>>();
            builder.Services.AddTransient<IProfileService, ProfileService<SiteUser>>();
            builder.Services.AddTransient<IJwtClaimsProcessor<SiteUser>, DefaultJwtClaimsProcessor>();

            //builder.Services.AddTransient<ISecurityStampValidator, cloudscribe.Core.IdentityServerIntegration.SecurityStampValidator<SiteUser>>();

            builder.Services.AddScoped<IMatchAuthorizeProtocolRoutePaths, MultiTenantAuthorizeProtocolRouteMatcher>();
            builder.Services.AddScoped<IMatchEndSessionProtocolRoutePaths, MultiTenantEndSessionProtocolRouteMatcher>();

            builder.Services.AddScoped<ApiResourceManager, ApiResourceManager>();
            builder.Services.AddScoped<IdentityResourceManager, IdentityResourceManager>();
            builder.Services.AddScoped<ClientsManager, ClientsManager>();

            //builder.Services.AddTransientDecorator<ICorsPolicyProvider, CorsPolicyProvider>();
            builder.Services.AddTransient<ICorsPathValidator, CorsPathValidator>();

            builder.Services.AddScoped<IVersionProvider, IntegrationVersionProvider>();
            builder.Services.AddScoped<IVersionProvider, StorageVersionProvider>();

            return builder;
        }



        

    }
}
