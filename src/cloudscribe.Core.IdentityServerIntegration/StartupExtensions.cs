﻿using cloudscribe.Core.Identity;
using cloudscribe.Core.IdentityServerIntegration;
using cloudscribe.Core.IdentityServerIntegration.Services;
using cloudscribe.Core.IdentityServerIntegration.Configuration;
using cloudscribe.Core.IdentityServerIntegration.Hosting;
using cloudscribe.Core.Models;
using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Hosting;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using cloudscribe.Core.IdentityServerIntegration.Handlers;

// https://github.com/IdentityServer/IdentityServer4/issues/19

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IIdentityServerBuilder AddCloudscribeIdentityServerIntegrationMvc(this IIdentityServerBuilder builder)
        {
            builder.AddCloudscribeIdentityServerIntegrationCommon();

            return builder;
        }



        public static IIdentityServerBuilder AddIdentityServerConfiguredForCloudscribe(this IServiceCollection services)
        {
            var builder = services.AddIdentityServerBuilder();

            builder
                .AddRequiredPlatformServices()
                .AddCookieAuthenticationForCloudscribe()
                .AddCoreServices()
                .AddDefaultEndpoints()
                .AddPluggableServices()
                .AddValidators()
                .AddResponseGenerators()
                .AddDefaultSecretParsers()
                .AddDefaultSecretValidators();

           
            return new IdentityServerBuilder(services);
            
        }

        public static IIdentityServerBuilder AddCookieAuthenticationForCloudscribe(this IIdentityServerBuilder builder)
        {
            builder.Services.AddAuthentication();

            builder.Services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, ConfigureInternalCookieOptions>();
            builder.Services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureInternalCookieOptions>();
            builder.Services.AddTransientDecorator<IAuthenticationService, IdentityServerAuthenticationService>();
            builder.Services.AddTransientDecorator<IAuthenticationHandlerProvider, FederatedSignoutAuthenticationHandlerProvider>();

            return builder;
        }

        public static IIdentityServerBuilder AddIdentityServerConfiguredForCloudscribe(this IServiceCollection services, Action<IdentityServerOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddIdentityServerConfiguredForCloudscribe();
        }

        public static IIdentityServerBuilder AddCloudscribeIdentityServerIntegrationCommon(this IIdentityServerBuilder builder)   
        {

            builder.Services.AddSingleton<IIdentityOptionsFactory, IdentityOptionsFactory>();
            builder.Services.AddScoped<ICustomClaimProvider, IdentityServerCustomClaimsProvider>();

            builder.Services.AddScoped<IIdentityServerIntegration, CloudscribeIntegration>();

            builder.Services.Configure<IdentityServerOptions>(options =>
            {
                //TODO: are the options set here actually used?
                // how to make this handle folder tenants?
                options.UserInteraction.ErrorUrl = "/oops/error";
                options.UserInteraction.LogoutUrl = "/account/logout";
                options.UserInteraction.LoginUrl = "/account/login";
                options.UserInteraction.LoginReturnUrlParameter = "returnUrl";
            });

            builder.Services.Configure<SecurityStampValidatorOptions>(opts =>
            {
                opts.OnRefreshingPrincipal = SecurityStampValidatorCallback.UpdatePrincipal;
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
                
                options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
               
            });

            builder.AddResourceOwnerValidator<ResourceOwnerPasswordValidator<SiteUser>>();
            builder.Services.AddTransient<IProfileService, ProfileService<SiteUser>>();
            
            builder.Services.AddScoped<ApiResourceManager, ApiResourceManager>();
            builder.Services.AddScoped<IdentityResourceManager, IdentityResourceManager>();
            builder.Services.AddScoped<ClientsManager, ClientsManager>();

            builder.Services.AddScoped<cloudscribe.Versioning.IVersionProvider, IntegrationVersionProvider>();
            builder.Services.AddScoped<cloudscribe.Versioning.IVersionProvider, StorageVersionProvider>();

            builder.Services.AddScoped<IHandlePasswordValidation<SiteUser>, NoopHandlePasswordValidation<SiteUser>>();

            return builder;
        }

        internal static void AddTransientDecorator<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddDecorator<TService>();
            services.AddTransient<TService, TImplementation>();
        }

        internal static void AddDecorator<TService>(this IServiceCollection services)
        {
            var registration = services.LastOrDefault(x => x.ServiceType == typeof(TService));
            if (registration == null)
            {
                throw new InvalidOperationException("Service type: " + typeof(TService).Name + " not registered.");
            }
            if (services.Any(x => x.ServiceType == typeof(Decorator<TService>)))
            {
                throw new InvalidOperationException("Decorator already registered for type: " + typeof(TService).Name + ".");
            }

            services.Remove(registration);

            if (registration.ImplementationInstance != null)
            {
                var type = registration.ImplementationInstance.GetType();
                var innerType = typeof(Decorator<,>).MakeGenericType(typeof(TService), type);
                services.Add(new ServiceDescriptor(typeof(Decorator<TService>), innerType, ServiceLifetime.Transient));
                services.Add(new ServiceDescriptor(type, registration.ImplementationInstance));
            }
            else if (registration.ImplementationFactory != null)
            {
                services.Add(new ServiceDescriptor(typeof(Decorator<TService>), provider =>
                {
                    return new DisposableDecorator<TService>((TService)registration.ImplementationFactory(provider));
                }, registration.Lifetime));
            }
            else
            {
                var type = registration.ImplementationType;
                var innerType = typeof(Decorator<,>).MakeGenericType(typeof(TService), registration.ImplementationType);
                services.Add(new ServiceDescriptor(typeof(Decorator<TService>), innerType, ServiceLifetime.Transient));
                services.Add(new ServiceDescriptor(type, type, registration.Lifetime));
            }
        }

    }

    internal class Decorator<TService>
    {
        public TService Instance { get; set; }

        public Decorator(TService instance)
        {
            Instance = instance;
        }
    }

    internal class Decorator<TService, TImpl> : Decorator<TService>
        where TImpl : class, TService
    {
        public Decorator(TImpl instance) : base(instance)
        {
        }
    }

    internal class DisposableDecorator<TService> : Decorator<TService>, IDisposable
    {
        public DisposableDecorator(TService instance) : base(instance)
        {
        }

        public void Dispose()
        {
            (Instance as IDisposable)?.Dispose();
        }
    }
}
