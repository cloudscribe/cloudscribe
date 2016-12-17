// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Events
{
    /// <summary>
    /// Details class for refresh token refresh events
    /// </summary>
    public class RefreshTokenRefreshDetails
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the old handle.
        /// </summary>
        /// <value>
        /// The old handle.
        /// </value>
        public string OldHandle { get; set; }

        /// <summary>
        /// Gets or sets the new handle.
        /// </summary>
        /// <value>
        /// The new handle.
        /// </value>
        public string NewHandle { get; set; }

        /// <summary>
        /// Gets or sets the lifetime.
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public int Lifetime { get; set; }
    }
}
