// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;

namespace IdentityServer4.Configuration
{
    /// <summary>
    /// Options class to configure discovery endpoint
    /// </summary>
    public class DiscoveryOptions
    {
        /// <summary>
        /// Show endpoints
        /// </summary>
        public bool ShowEndpoints { get; set; } = true;

        /// <summary>
        /// Show signing keys
        /// </summary>
        public bool ShowKeySet { get; set; } = true;

        /// <summary>
        /// Show identity scopes
        /// </summary>
        public bool ShowIdentityScopes { get; set; } = true;

        /// <summary>
        /// Show API scopes
        /// </summary>
        public bool ShowApiScopes { get; set; } = true;

        /// <summary>
        /// Show identity claims
        /// </summary>
        public bool ShowClaims { get; set; } = true;

        /// <summary>
        /// Show response types
        /// </summary>
        public bool ShowResponseTypes { get; set; } = true;

        /// <summary>
        /// Show response modes
        /// </summary>
        public bool ShowResponseModes { get; set; } = true;

        /// <summary>
        /// Show standard grant types
        /// </summary>
        public bool ShowGrantTypes { get; set; } = true;

        /// <summary>
        /// Show custom grant types
        /// </summary>
        public bool ShowExtensionGrantTypes { get; set; } = true;

        /// <summary>
        /// Show token endpoint authentication methods
        /// </summary>
        public bool ShowTokenEndpointAuthenticationMethods { get; set; } = true;

        /// <summary>
        /// Adds custom entries to the discovery document
        /// </summary>
        public Dictionary<string, object> CustomEntries { get; set; } = new Dictionary<string, object>();
    }
}