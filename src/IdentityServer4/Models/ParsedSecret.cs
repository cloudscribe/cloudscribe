// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Models
{
    /// <summary>
    /// Represents a secret extracted from the OWIN environment
    /// </summary>
    public class ParsedSecret
    {
        /// <summary>
        /// Gets or sets the identifier associated with this secret
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the credential to verify the secret
        /// </summary>
        /// <value>
        /// The credential.
        /// </value>
        public object Credential { get; set; }

        /// <summary>
        /// Gets or sets the type of the secret
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }
    }
}