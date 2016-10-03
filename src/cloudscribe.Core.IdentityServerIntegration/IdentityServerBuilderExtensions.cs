

using cloudscribe.Core.Identity;
using cloudscribe.Core.IdentityServerIntegration;
using cloudscribe.Core.Models;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Configuration.DependencyInjection;
using IdentityServer4.Endpoints;
using IdentityServer4.Events;
using IdentityServer4.Hosting;
using IdentityServer4.Models;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Validation;
using IdentityServer4.Services;
using IdentityServer4.Services.Default;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerBuilderExtensions
    {
        

        public static IIdentityServerBuilder AddCloudscribeIdentity<TUser>(this IIdentityServerBuilder builder)
            where TUser : SiteUser
        {
            return builder.AddCloudscribeIdentity<TUser>(AuthenticationScheme.Application);
        }

        public static IIdentityServerBuilder AddCloudscribeIdentity<TUser>(this IIdentityServerBuilder builder, string authenticationScheme)
            where TUser : SiteUser
        {
            builder.Services.Configure<IdentityServerOptions>(options =>
            {
                options.AuthenticationOptions.AuthenticationScheme = authenticationScheme;
            });

            builder.Services.AddSingleton<IEndpointRouter>(resolver =>
            {
                return new MultiTenantEndpointRouter(CustomConstants.EndpointPathToNameMap,
                    resolver.GetRequiredService<IdentityServerOptions>(),
                    resolver.GetServices<EndpointMapping>(),
                    resolver.GetService<IOptions<MultiTenantOptions>>(),
                    resolver.GetRequiredService<ILogger<MultiTenantEndpointRouter>>());
            });

            //builder.Services.Configure<IdentityOptions>(options =>
            //{
            //    //options.Cookies.ApplicationCookie.AuthenticationScheme = authenticationScheme;
            //    options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
            //    options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
            //    options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
            //});

            builder.AddResourceOwnerValidator<ResourceOwnerPasswordValidator<TUser>>();
            builder.Services.AddTransient<IProfileService, ProfileService<TUser>>();

            builder.Services.AddTransient<ISecurityStampValidator, cloudscribe.Core.IdentityServerIntegration.SecurityStampValidator<TUser>>();

            //builder.Services.AddSingleton<IdentityServerOptions>(container => {
            //    var tenant = container.Resolve<IHttpContextAccessor>().HttpContext.GetCurrentRequesrtTenant();
            //    return CreateFooFromTenant(tenant);
            //});

            return builder;
        }

    }
}
