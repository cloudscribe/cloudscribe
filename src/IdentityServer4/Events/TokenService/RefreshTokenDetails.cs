// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Events
{
    /// <summary>
    /// Details class for refresh token issued events
    /// </summary>
    public class RefreshTokenDetails : TokenIssuedDetailsBase
    {
        /// <summary>
        /// Gets or sets the handle identifier.
        /// </summary>
        /// <value>
        /// The handle identifier.
        /// </value>
        public string HandleId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }
    }
}