// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Validation
{
    /// <summary>
    /// Validation result for secrets
    /// </summary>
    public class SecretValidationResult : ValidationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the secret validation was successful.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }
    }
}