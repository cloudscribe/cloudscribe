// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-05-07
// Last Modified:			2017-10-06
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static object SecurityStampValidatorCallback { get; private set; }

        public static IdentityBuilder AddCloudscribeIdentity(
            this IServiceCollection services,
            Action<IdentityOptions> setupAction = null
            )
        {
            services.AddSingleton<IOptionsMonitor<CookieAuthenticationOptions>, SiteCookieAuthenticationOptions>();

            services.AddSingleton<IOptionsMonitor<FacebookOptions>, SiteFacebookOptions>();
            services.AddSingleton<IOptionsMonitor<GoogleOptions>, SiteGoogleOptions>();
            services.AddSingleton<IOptionsMonitor<MicrosoftAccountOptions>, SiteMicrosoftAccountOptions>();
            services.AddSingleton<IOptionsMonitor<TwitterOptions>, SiteTwitterOptions>();
            services.AddSingleton<IOptionsMonitor<OpenIdConnectOptions>, SiteOpenIdConnectOptions>();

            services.TryAddSingleton<IIdentityOptionsFactory, DefaultIdentityOptionsFactory>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
           .AddCookie(IdentityConstants.ApplicationScheme, o =>
            {
                o.LoginPath = new PathString("/Account/Login");             
            })
            .AddCookie(IdentityConstants.ExternalScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.ExternalScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddCookie(IdentityConstants.TwoFactorRememberMeScheme,
                o => o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme
                )
            .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddFacebook(o =>
            {
                o.AppId = "placeholder";
                o.AppSecret = "placeholder";
            })
            .AddGoogle(o =>
            {
                o.ClientId = "placeholder";
                o.ClientSecret = "placeholder";
                o.SignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddMicrosoftAccount(o =>
            {
                o.ClientId = "placeholder";
                o.ClientSecret = "placeholder";
            })
            .AddTwitter(o =>
            {
                o.ConsumerKey = "placeholder";
                o.ConsumerSecret = "placeholder";
            })
            .AddOpenIdConnect(o =>
            {
                o.ClientId = "placeholder";
                o.ClientSecret = "placeholder";
                o.Authority = "https://placeholder.com";
                o.ResponseType = "code id_token";
                o.GetClaimsFromUserInfoEndpoint = true;
                o.SaveTokens = true;

            });

            services.TryAddSingleton<ICaptureOidcTokens, NoopOidcTokenCapture>();
            services.TryAddSingleton<ICookieAuthTicketStoreProvider, NoopCookieAuthTicketStoreProvider>();
            services.AddSingleton<IOptions<IdentityOptions>, SiteIdentityOptionsResolver>();
            services.AddScoped<IUserClaimsPrincipalFactory<SiteUser>, SiteUserClaimsPrincipalFactory<SiteUser, SiteRole>>();
            services.TryAddScoped<IPasswordHasher<SiteUser>, SitePasswordHasher<SiteUser>>();
            services.TryAddScoped<IFallbackPasswordHashValidator<SiteUser>, DefaultFallbackPasswordHashValidator<SiteUser>>();
            services.TryAddScoped<SignInManager<SiteUser>, SignInManager<SiteUser>>();
            services.TryAddScoped<IUserConfirmation<SiteUser>, SiteUserConfirmation<SiteUser>>();
            services.TryAddSingleton<ICookieAuthRedirector, ApiAwareCookieAuthRedirector>();
            services.TryAddScoped<ISocialAuthEmailVerfificationPolicy, DefaultSocialAuthEmailVerfificationPolicy>();
            services.TryAddScoped<ISiteAccountCapabilitiesProvider, DefaultSiteAcountCapabilitiesProvider>();
            services.TryAddScoped<IProcessAccountLoginRules, DefaultAccountLoginRulesProcessor>();

            // Hosting doesn't add IHttpContextAccessor by default
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped<IUserValidator<SiteUser>, UserValidator<SiteUser>>();
            services.TryAddScoped<IPasswordValidator<SiteUser>, PasswordValidator<SiteUser>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IRoleValidator<SiteRole>, RoleValidator<SiteRole>>();
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<SiteUser>>();

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
            var authenticatorProviderType = typeof(AuthenticatorTokenProvider<SiteUser>);
            services.Configure<TokenOptions>(options =>
            {
                options.ProviderMap[TokenOptions.DefaultProvider] = new TokenProviderDescriptor(dataProtectionProviderType);
                options.ProviderMap[TokenOptions.DefaultEmailProvider] = new TokenProviderDescriptor(emailTokenProviderType);
                options.ProviderMap[TokenOptions.DefaultPhoneProvider] = new TokenProviderDescriptor(phoneNumberProviderType);
                options.ProviderMap[TokenOptions.DefaultAuthenticatorProvider] = new TokenProviderDescriptor(authenticatorProviderType);
            });
            services.TryAddTransient(dataProtectionProviderType);
            services.TryAddTransient(emailTokenProviderType);
            services.TryAddTransient(phoneNumberProviderType);
            services.TryAddTransient(authenticatorProviderType);


            services.TryAddScoped<IIdentityServerIntegration, NotIntegratedIdentityServerIntegration>();

            services.AddTransient<SiteAuthCookieEvents>();
            services.TryAddTransient<ISiteAuthCookieEvents, SiteAuthCookieEvents>();
            services.AddTransient<OidcTokenEndpointService>();
            services.AddScoped<cloudscribe.Core.Identity.IOidcHybridFlowHelper, cloudscribe.Core.Identity.HybridFlowHelper>();

            return builder;
        }
    }
}
