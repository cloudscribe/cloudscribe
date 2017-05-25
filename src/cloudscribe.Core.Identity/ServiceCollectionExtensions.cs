// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-05-07
// Last Modified:			2016-11-26
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Antiforgery.Internal;
using Microsoft.AspNetCore.Authentication.Cookies;
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

            
            // Services used by identity

            services.TryAddScoped<IUserClaimsPrincipalFactory<SiteUser>, SiteUserClaimsPrincipalFactory<SiteUser, SiteRole>>();
            services.TryAddScoped<IPasswordHasher<SiteUser>, SitePasswordHasher<SiteUser>>();
            services.TryAddScoped<SiteSignInManager<SiteUser>, SiteSignInManager<SiteUser>>();
            services.TryAddSingleton<SiteAuthCookieValidator, SiteAuthCookieValidator>();
            services.TryAddScoped<SiteCookieAuthenticationEvents, SiteCookieAuthenticationEvents>();
            services.TryAddScoped<ISocialAuthEmailVerfificationPolicy, DefaultSocialAuthEmailVerfificationPolicy>();

            services.TryAddScoped<ISiteAcountCapabilitiesProvider, DefaultSiteAcountCapabilitiesProvider>();
            services.TryAddScoped<IProcessAccountLoginRules, DefaultAccountLoginRulesProcessor>();

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

            services.AddScoped<UserEvents, UserEvents>();

            services.TryAddScoped<ICustomClaimProvider, DoNothingCustomClaimProvider>();

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
            services.TryAddTransient(dataProtectionProviderType);
            services.TryAddTransient(emailTokenProviderType);
            services.TryAddTransient(phoneNumberProviderType);


            services.TryAddScoped<IIdentityServerIntegration, NotIntegratedIdentityServerIntegration>();

            return builder;

            //return services;
        }

        

    }
}
