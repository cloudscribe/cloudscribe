// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Configuration.DependencyInjection;
using IdentityServer4.Endpoints;
using IdentityServer4.Events;
using IdentityServer4.Hosting;
using IdentityServer4.Infrastructure;
using IdentityServer4.Models;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Services.Default;
using IdentityServer4.Stores;
using IdentityServer4.Stores.Serialization;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerBuilderExtensionsCore
    {
        /// <summary>
        /// Adds the required platform services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddRequiredPlatformServices(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddAuthentication();
            builder.Services.AddOptions();

            builder.Services.AddSingleton(
                resolver => resolver.GetRequiredService<IOptions<IdentityServerOptions>>().Value);

            return builder;
        }

        /// <summary>
        /// Adds the default endpoints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddDefaultEndpoints(this IIdentityServerBuilder builder)
        {
            builder.Services.AddSingleton<IEndpointRouter>(resolver =>
            {
                return new EndpointRouter(Constants.EndpointPathToNameMap,
                    resolver.GetRequiredService<IdentityServerOptions>(),
                    resolver.GetServices<EndpointMapping>(),
                    resolver.GetRequiredService<ILogger<EndpointRouter>>());
            });

            builder.AddEndpoint<AuthorizeEndpoint>(EndpointName.Authorize);
            builder.AddEndpoint<CheckSessionEndpoint>(EndpointName.CheckSession);
            builder.AddEndpoint<DiscoveryEndpoint>(EndpointName.Discovery);
            builder.AddEndpoint<EndSessionEndpoint>(EndpointName.EndSession);
            builder.AddEndpoint<IntrospectionEndpoint>(EndpointName.Introspection);
            builder.AddEndpoint<RevocationEndpoint>(EndpointName.Revocation);
            builder.AddEndpoint<TokenEndpoint>(EndpointName.Token);
            builder.AddEndpoint<UserInfoEndpoint>(EndpointName.UserInfo);

            builder.Services.AddScoped<IMatchAuthorizeProtocolRoutePaths, DefaultAuthorizeProtocolRouteMatcher>();
            builder.Services.AddScoped<IMatchEndSessionProtocolRoutePaths, DefaultEndSessionProtocolRouteMatcher>();
            
            return builder;
        }

        /// <summary>
        /// Adds the endpoint.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddEndpoint<T>(this IIdentityServerBuilder builder, EndpointName endpoint)
            where T : class, IEndpoint
        {
            builder.Services.AddTransient<T>();
            builder.Services.AddSingleton(new EndpointMapping { Endpoint = endpoint, Handler = typeof(T) });

            return builder;
        }

        /// <summary>
        /// Adds the core services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddCoreServices(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<ApiSecretValidator>();
            builder.Services.AddTransient<SecretParser>();
            builder.Services.AddTransient<ClientSecretValidator>();
            builder.Services.AddTransient<SecretValidator>();
            builder.Services.AddTransient<ScopeValidator>();
            builder.Services.AddTransient<ExtensionGrantValidator>();
            builder.Services.AddTransient<BearerTokenUsageValidator>();
            builder.Services.AddTransient<PersistentGrantSerializer>();
            builder.Services.AddTransient<EventServiceHelper>();
            builder.Services.AddTransient<ReturnUrlParser>();

            builder.Services.AddTransient<IReturnUrlParser, OidcReturnUrlParser>();
            builder.Services.AddTransient<ISessionIdService, DefaultSessionIdService>();
            builder.Services.AddTransient<IClientSessionService, DefaultClientSessionService>();
            builder.Services.AddTransient(typeof(MessageCookie<>));
            builder.Services.AddScoped<AuthenticationHandler>();

            builder.Services.AddCors();
            builder.Services.AddTransient<ICorsPathValidator, DefaultCorsPathValidator>();
            builder.Services.AddTransientDecorator<ICorsPolicyProvider, CorsPolicyProvider>();

            return builder;
        }

        /// <summary>
        /// Adds the pluggable services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddPluggableServices(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IPersistedGrantService, DefaultPersistedGrantService>();
            builder.Services.TryAddTransient<IKeyMaterialService, DefaultKeyMaterialService>();
            builder.Services.TryAddTransient<IEventService, DefaultEventService>();
            builder.Services.TryAddTransient<ITokenService, DefaultTokenService>();
            builder.Services.TryAddTransient<ITokenCreationService, DefaultTokenCreationService>();
            builder.Services.TryAddTransient<IClaimsService, DefaultClaimsService>();
            builder.Services.TryAddTransient<IRefreshTokenService, DefaultRefreshTokenService>();
            builder.Services.TryAddTransient<IConsentService, DefaultConsentService>();
            builder.Services.TryAddTransient<ICorsPolicyService, DefaultCorsPolicyService>();
            builder.Services.TryAddTransient<IProfileService, DefaultProfileService>();
            builder.Services.TryAddTransient(typeof(IMessageStore<>), typeof(CookieMessageStore<>));
            builder.Services.TryAddTransient<IIdentityServerInteractionService, DefaultIdentityServerInteractionService>();
            builder.Services.TryAddTransient<IAuthorizationCodeStore, DefaultAuthorizationCodeStore>();
            builder.Services.TryAddTransient<IRefreshTokenStore, DefaultRefreshTokenStore>();
            builder.Services.TryAddTransient<IReferenceTokenStore, DefaultReferenceTokenStore>();
            builder.Services.TryAddTransient<IUserConsentStore, DefaultUserConsentStore>();

            return builder;
        }

        /// <summary>
        /// Adds the validators.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddValidators(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IEndSessionRequestValidator, EndSessionRequestValidator>();
            builder.Services.TryAddTransient<ITokenRevocationRequestValidator, TokenRevocationRequestValidator>();
            builder.Services.TryAddTransient<IAuthorizeRequestValidator, AuthorizeRequestValidator>();
            builder.Services.TryAddTransient<ITokenRequestValidator, TokenRequestValidator>();
            builder.Services.TryAddTransient<IRedirectUriValidator, StrictRedirectUriValidator>();
            builder.Services.TryAddTransient<ITokenValidator, TokenValidator>();
            builder.Services.TryAddTransient<IIntrospectionRequestValidator, IntrospectionRequestValidator>();
            builder.Services.TryAddTransient<IResourceOwnerPasswordValidator, NotSupportedResouceOwnerPasswordValidator>();
            builder.Services.TryAddTransient<ICustomTokenValidator, DefaultCustomTokenValidator>();
            builder.Services.TryAddTransient<ICustomAuthorizeRequestValidator, DefaultCustomAuthorizeRequestValidator>();
            builder.Services.TryAddTransient<ICustomTokenRequestValidator, DefaultCustomTokenRequestValidator>();

            return builder;
        }

        /// <summary>
        /// Adds the response generators.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddResponseGenerators(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<ITokenResponseGenerator, TokenResponseGenerator>();
            builder.Services.TryAddTransient<IUserInfoResponseGenerator, UserInfoResponseGenerator>();
            builder.Services.TryAddTransient<IIntrospectionResponseGenerator, IntrospectionResponseGenerator>();
            builder.Services.TryAddTransient<IAuthorizeInteractionResponseGenerator, AuthorizeInteractionResponseGenerator>();
            builder.Services.TryAddTransient<IAuthorizeResponseGenerator, AuthorizeResponseGenerator>();

            return builder;
        }

        /// <summary>
        /// Adds the default secret parsers.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddDefaultSecretParsers(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<ISecretParser, BasicAuthenticationSecretParser>();
            builder.Services.AddTransient<ISecretParser, PostBodySecretParser>();

            return builder;
        }

        /// <summary>
        /// Adds the default secret validators.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddDefaultSecretValidators(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<ISecretValidator, HashedSharedSecretValidator>();

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
            var registration = services.FirstOrDefault(x => x.ServiceType == typeof(TService));
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
}