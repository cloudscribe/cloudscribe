// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Events
{
    /// <summary>
    /// Details class for authorization code issued events
    /// </summary>
    public class AuthorizationCodeDetails : TokenIssuedDetailsBase
    {
        /// <summary>
        /// Gets or sets the handle identifier.
        /// </summary>
        /// <value>
        /// The handle identifier.
        /// </value>
        public string HandleId { get; set; }

        /// <summary>
        /// Gets or sets the redirect URI.
        /// </summary>
        /// <value>
        /// The redirect URI.
        /// </value>
        public string RedirectUri { get; set; }
    }
}