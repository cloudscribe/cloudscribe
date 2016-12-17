// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Stores;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CryptoRandom = IdentityModel.CryptoRandom;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerBuilderExtensionsCrypto
    {
        /// <summary>
        /// Sets the signing credential.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="credential">The credential.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddSigningCredential(this IIdentityServerBuilder builder, SigningCredentials credential)
        {
            // todo
            //if (!(credential.Key is AsymmetricSecurityKey) &&
            //    !credential.Key.IsSupportedAlgorithm(SecurityAlgorithms.RsaSha256Signature))
            //{
            //    throw new InvalidOperationException("Signing key is not asymmetric and does not support RS256");
            //}

            builder.Services.AddSingleton<ISigningCredentialStore>(new DefaultSigningCredentialsStore(credential));
            builder.Services.AddSingleton<IValidationKeysStore>(new DefaultValidationKeysStore(new[] { credential.Key }));

            return builder;
        }

        /// <summary>
        /// Sets the signing credential.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="certificate">The certificate.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">X509 certificate does not have a private key.</exception>
        public static IIdentityServerBuilder AddSigningCredential(this IIdentityServerBuilder builder, X509Certificate2 certificate)
        {
            if (certificate == null) throw new ArgumentNullException(nameof(certificate));

            if (!certificate.HasPrivateKey)
            {
                throw new InvalidOperationException("X509 certificate does not have a private key.");
            }

            var credential = new SigningCredentials(new X509SecurityKey(certificate), "RS256");
            return builder.AddSigningCredential(credential);
        }

        /// <summary>
        /// Sets the signing credential.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The name.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">certificate: '{name}'</exception>
        public static IIdentityServerBuilder AddSigningCredential(this IIdentityServerBuilder builder, string name, StoreLocation location = StoreLocation.LocalMachine)
        {
            X509Certificate2 certificate;

            if (location == StoreLocation.LocalMachine)
            {
                certificate = X509.LocalMachine.My.SubjectDistinguishedName.Find(name, validOnly: false).FirstOrDefault();
            }
            else
            {
                certificate = X509.CurrentUser.My.SubjectDistinguishedName.Find(name, validOnly: false).FirstOrDefault();
            }

            if (certificate == null) throw new InvalidOperationException($"certificate: '{name}' not found in certificate store");

            return builder.AddSigningCredential(certificate);
        }

        /// <summary>
        /// Sets the signing credential.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="rsaKey">The RSA key.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">RSA key does not have a private key.</exception>
        public static IIdentityServerBuilder AddSigningCredential(this IIdentityServerBuilder builder, RsaSecurityKey rsaKey)
        {
            if (!rsaKey.HasPrivateKey)
            {
                throw new InvalidOperationException("RSA key does not have a private key.");
            }

            var credential = new SigningCredentials(rsaKey, "RS256");
            return builder.AddSigningCredential(credential);
        }

        /// <summary>
        /// Sets the temporary signing credential.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddTemporarySigningCredential(this IIdentityServerBuilder builder)
        {
            var rsa = RSA.Create();

#if NET452
            if (rsa.KeySize < 2048)
            {
                rsa.Dispose();
                rsa = new RSACryptoServiceProvider(2048);
            }
#endif
            RsaSecurityKey key = null;
#if NET452
            if (rsa is RSACryptoServiceProvider) 
            {
                var parameters = rsa.ExportParameters(includePrivateParameters: true);
                key = new RsaSecurityKey(parameters);
                        
                rsa.Dispose();
            }   
#endif
            if (key == null)
            {
                key = new RsaSecurityKey(rsa);
            }

            key.KeyId = CryptoRandom.CreateUniqueId();
            
            var credential = new SigningCredentials(key, "RS256");
            return builder.AddSigningCredential(credential);
        }

        /// <summary>
        /// Adds the validation keys.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddValidationKeys(this IIdentityServerBuilder builder, params AsymmetricSecurityKey[] keys)
        {
            builder.Services.AddSingleton<IValidationKeysStore>(new DefaultValidationKeysStore(keys));

            return builder;
        }
    }
}