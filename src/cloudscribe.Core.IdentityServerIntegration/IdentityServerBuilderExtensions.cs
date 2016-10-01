// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using cloudscribe.Core.IdentityServerIntegration;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddCloudscribeIdentity<TUser>(this IIdentityServerBuilder builder)
            where TUser : SiteUser
        {
            return builder.AddCloudscribeIdentity<TUser>(AuthenticationScheme.Application + "-");
        }

        public static IIdentityServerBuilder AddCloudscribeIdentity<TUser>(this IIdentityServerBuilder builder, string authenticationScheme)
            where TUser : SiteUser
        {
            builder.Services.Configure<IdentityServerOptions>(options =>
            {
                options.AuthenticationOptions.AuthenticationScheme = authenticationScheme;
            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                //options.Cookies.ApplicationCookie.AuthenticationScheme = authenticationScheme;
                options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
            });

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
