// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerBuilderExtensionsAdditional
    {
        /// <summary>
        /// Adds the extension grant validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddExtensionGrantValidator<T>(this IIdentityServerBuilder builder)
            where T : class, IExtensionGrantValidator
        {
            builder.Services.AddTransient<IExtensionGrantValidator, T>();

            return builder;
        }

        /// <summary>
        /// Adds the resource owner validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddResourceOwnerValidator<T>(this IIdentityServerBuilder builder)
           where T : class, IResourceOwnerPasswordValidator
        {
            builder.Services.AddTransient<IResourceOwnerPasswordValidator, T>();

            return builder;
        }

        /// <summary>
        /// Adds the profile service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddProfileService<T>(this IIdentityServerBuilder builder)
           where T : class, IProfileService
        {
            builder.Services.AddTransient<IProfileService, T>();

            return builder;
        }

        /// <summary>
        /// Adds the secret parser.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddSecretParser<T>(this IIdentityServerBuilder builder)
            where T : class, ISecretParser
        {
            builder.Services.AddTransient<ISecretParser, T>();

            return builder;
        }

        /// <summary>
        /// Adds the secret validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddSecretValidator<T>(this IIdentityServerBuilder builder)
            where T : class, ISecretValidator
        {
            builder.Services.AddTransient<ISecretValidator, T>();

            return builder;
        }

        /// <summary>
        /// Adds the client store cache.
        /// </summary>
        /// <typeparam name="T">The type of the concrete client store class that is registered in DI.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddClientStoreCache<T>(this IIdentityServerBuilder builder)
            where T : IClientStore
        {
            builder.Services.AddTransient<IClientStore, CachingClientStore<T>>();
            return builder;
        }
        
        /// <summary>
        /// Adds the client store cache.
        /// </summary>
        /// <typeparam name="T">The type of the concrete scope store class that is registered in DI.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddResourceStoreCache<T>(this IIdentityServerBuilder builder)
            where T : IResourceStore
        {
            builder.Services.AddTransient<IResourceStore, CachingResourceStore<T>>();
            return builder;
        }

        /// <summary>
        /// Adds the authorize interaction response generator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddAuthorizeInteractionResponseGenerator<T>(this IIdentityServerBuilder builder)
            where T: class, IAuthorizeInteractionResponseGenerator
        {
            builder.Services.AddTransient<IAuthorizeInteractionResponseGenerator, T>();

            return builder;
        }

        /// <summary>
        /// Adds the custom authorize request validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddCustomAuthorizeRequestValidator<T>(this IIdentityServerBuilder builder)
           where T : class, ICustomAuthorizeRequestValidator
        {
            builder.Services.AddTransient<ICustomAuthorizeRequestValidator, T>();

            return builder;
        }
    }
}