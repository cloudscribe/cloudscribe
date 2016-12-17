// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;

namespace IdentityServer4.Configuration
{
    /// <summary>
    /// The IdentityServerOptions class is the top level container for all configuration settings of IdentityServer.
    /// </summary>
    public class IdentityServerOptions
    {
        /// <summary>
        /// Gets or sets the display name of the site used in standard views.
        /// </summary>
        /// <value>
        /// Display name of the site used in standard views.
        /// </value>
        public string SiteName { get; set; } = Constants.IdentityServerName;

        /// <summary>
        /// Gets or sets the unique name of this server instance, e.g. https://myissuer.com
        /// </summary>
        /// <value>
        /// Unique name of this server instance, e.g. https://myissuer.com
        /// </value>
        public string IssuerUri { get; set; }

        // todo
        /// <summary>
        /// Gets or sets a value indicating whether SSL is required for IdentityServer. Defaults to `true`.
        /// </summary>
        /// <value>
        ///   <c>true</c> if SSL is required; otherwise, <c>false</c>.
        /// </value>
        //public bool RequireSsl { get; set; } = true;

        /// <summary>
        /// Gets or sets the endpoint configuration.
        /// </summary>
        /// <value>
        /// The endpoints configuration.
        /// </value>
        public EndpointsOptions Endpoints { get; set; } = new EndpointsOptions();

        /// <summary>
        /// Gets or sets the discovery endpoint configuration.
        /// </summary>
        /// <value>
        /// The discovery endpoint configuration.
        /// </value>
        public DiscoveryOptions DiscoveryOptions { get; set; } = new DiscoveryOptions();

        /// <summary>
        /// Gets or sets the authentication options.
        /// </summary>
        /// <value>
        /// The authentication options.
        /// </value>
        public AuthenticationOptions AuthenticationOptions { get; set; } = new AuthenticationOptions();

        /// <summary>
        /// Gets or sets the protocol logout urls.
        /// </summary>
        /// <value>
        /// The protocol logout urls.
        /// </value>
        public List<string> ProtocolLogoutUrls { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the events options.
        /// </summary>
        /// <value>
        /// The events options.
        /// </value>
        public EventsOptions EventsOptions { get; set; } = new EventsOptions();

        /// <summary>
        /// Gets or sets the max input length restrictions.
        /// </summary>
        /// <value>
        /// The length restrictions.
        /// </value>
        public InputLengthRestrictions InputLengthRestrictions { get; set; } = new InputLengthRestrictions();

        /// <summary>
        /// Gets or sets the options for the user interaction.
        /// </summary>
        /// <value>
        /// The user interaction options.
        /// </value>
        public UserInteractionOptions UserInteractionOptions { get; set; } = new UserInteractionOptions();

        /// <summary>
        /// Gets or sets the caching options.
        /// </summary>
        /// <value>
        /// The caching options.
        /// </value>
        public CachingOptions CachingOptions { get; set; } = new CachingOptions();

        /// <summary>
        /// Gets or sets the cors options.
        /// </summary>
        /// <value>
        /// The cors options.
        /// </value>
        public CorsOptions CorsOptions { get; set; } = new CorsOptions();
    }
}