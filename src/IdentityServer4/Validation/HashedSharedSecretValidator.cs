// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Validates a shared secret stored in SHA256 or SHA512
    /// </summary>
    public class HashedSharedSecretValidator : ISecretValidator
    {
        private readonly ILogger _logger;

        public HashedSharedSecretValidator(ILogger<HashedSharedSecretValidator> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Validates a secret
        /// </summary>
        /// <param name="secrets">The stored secrets.</param>
        /// <param name="parsedSecret">The received secret.</param>
        /// <returns>
        /// A validation result
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Id or cedential</exception>
        public Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret)
        {
            var fail = Task.FromResult(new SecretValidationResult { Success = false });
            var success = Task.FromResult(new SecretValidationResult { Success = true });

            if (parsedSecret.Type != IdentityServerConstants.ParsedSecretTypes.SharedSecret)
            {
                _logger.LogDebug("Hashed shared secret validator cannot process {type}", parsedSecret.Type ?? "null");
                return fail;
            }

            var sharedSecret = parsedSecret.Credential as string;

            if (parsedSecret.Id.IsMissing() || sharedSecret.IsMissing())
            {
                throw new ArgumentException("Id or Credential is missing.");
            }

            var secretSha256 = sharedSecret.Sha256();
            var secretSha512 = sharedSecret.Sha512();

            foreach (var secret in secrets)
            {
                var secretDescription = string.IsNullOrEmpty(secret.Description) ? "no description" : secret.Description;

                // this validator is only applicable to shared secrets
                if (secret.Type != IdentityServerConstants.SecretTypes.SharedSecret)
                {
                    _logger.LogDebug("Skipping secret: {description}, secret is not of type {type}.", secretDescription, IdentityServerConstants.SecretTypes.SharedSecret);
                    continue;
                }

                bool isValid = false;
                byte[] secretBytes;

                try
                {
                    secretBytes = Convert.FromBase64String(secret.Value);
                }
                catch (FormatException)
                {
                    _logger.LogInformation("Secret: {description} uses invalid hashing algorithm.", secretDescription);
                    return fail;
                }

                if (secretBytes.Length == 32)
                {
                    isValid = TimeConstantComparer.IsEqual(secret.Value, secretSha256);
                }
                else if (secretBytes.Length == 64)
                {
                    isValid = TimeConstantComparer.IsEqual(secret.Value, secretSha512);
                }
                else
                {
                    _logger.LogInformation("Secret: {description} uses invalid hashing algorithm.", secretDescription);
                    return fail;
                }

                if (isValid)
                {
                    return success;
                }
            }

            _logger.LogDebug("No matching hashed secret found.");
            return fail;
        }
    }
}