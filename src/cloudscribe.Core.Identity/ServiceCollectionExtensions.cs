// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-05-07
// Last Modified:			2016-06-19
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Antiforgery.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IdentityBuilder AddCloudscribeIdentity(
            this IServiceCollection services,
            Action<IdentityOptions> setupAction = null
            )
        {
            services.AddSingleton<IOptions<IdentityOptions>, SiteIdentityOptionsResolver>();

            //services.AddIdentity<SiteUser, SiteRole>()
            //    .AddUserStore<UserStore<SiteUser>>()
            //    .AddRoleStore<RoleStore<SiteRole>>()
            //    .AddUserManager<SiteUserManager<SiteUser>>()
            //    .AddRoleManager<SiteRoleManager<SiteRole>>()
            //    .AddDefaultTokenProviders()
            //    ;

            // Services used by identity

            services.AddScoped<IUserClaimsPrincipalFactory<SiteUser>, SiteUserClaimsPrincipalFactory<SiteUser, SiteRole>>();
            services.AddScoped<IPasswordHasher<SiteUser>, SitePasswordHasher<SiteUser>>();
            services.AddScoped<SiteSignInManager<SiteUser>, SiteSignInManager<SiteUser>>();
            services.AddSingleton<SiteAuthCookieValidator, SiteAuthCookieValidator>();
            services.AddScoped<SiteCookieAuthenticationEvents, SiteCookieAuthenticationEvents>();
            services.AddSingleton<IAntiforgeryTokenStore, SiteAntiforgeryTokenStore>();

            services.AddAuthentication(options =>
            {
                // This is the Default value for ExternalCookieAuthenticationScheme
                options.SignInScheme = new IdentityCookieOptions().ExternalCookieAuthenticationScheme;
            });

            // Hosting doesn't add IHttpContextAccessor by default
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Identity services
            services.TryAddSingleton<IdentityMarkerService>();
            services.TryAddScoped<IUserValidator<SiteUser>, UserValidator<SiteUser>>();
            services.TryAddScoped<IPasswordValidator<SiteUser>, PasswordValidator<SiteUser>>();
            //services.TryAddScoped<IPasswordHasher<SiteUser>, PasswordHasher<SiteUser>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IRoleValidator<SiteRole>, RoleValidator<SiteRole>>();
            // No interface for the error describer so we can add errors without rev'ing the interface
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<SiteUser>>();
            //services.TryAddScoped<IUserClaimsPrincipalFactory<SiteUser>, UserClaimsPrincipalFactory<SiteUser, SiteRole>>();
            //services.TryAddScoped<UserManager<SiteUser>, UserManager<SiteUser>>();
            //services.TryAddScoped<SignInManager<SiteUser>, SignInManager<SiteUser>>();
            //services.TryAddScoped<RoleManager<SiteRole>, RoleManager<SiteRole>>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            var builder = new IdentityBuilder(typeof(SiteUser), typeof(SiteRole), services);

            builder.AddUserStore<UserStore<SiteUser>>()
             .AddRoleStore<RoleStore<SiteRole>>()
             .AddUserManager<SiteUserManager<SiteUser>>()
             .AddRoleManager<SiteRoleManager<SiteRole>>()
             ;

            var dataProtectionProviderType = typeof(DataProtectorTokenProvider<SiteUser>);
            var phoneNumberProviderType = typeof(PhoneNumberTokenProvider<SiteUser>);
            var emailTokenProviderType = typeof(EmailTokenProvider<SiteUser>);
            services.Configure<TokenOptions>(options =>
            {
                options.ProviderMap[TokenOptions.DefaultProvider] = new TokenProviderDescriptor(dataProtectionProviderType);
                options.ProviderMap[TokenOptions.DefaultEmailProvider] = new TokenProviderDescriptor(emailTokenProviderType);
                options.ProviderMap[TokenOptions.DefaultPhoneProvider] = new TokenProviderDescriptor(phoneNumberProviderType);
            });
            services.AddTransient(dataProtectionProviderType);
            services.AddTransient(emailTokenProviderType);
            services.AddTransient(phoneNumberProviderType);

            


            return builder;

            //return services;
        }
    }
}
