// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;

namespace IdentityServer4.Events
{
    /// <summary>
    /// Details class for access token issued events
    /// </summary>
    public class AccessTokenIssuedDetails : TokenIssuedDetailsBase
    {
        /// <summary>
        /// Gets or sets the type of the access token.
        /// </summary>
        /// <value>
        /// The type of the token.
        /// </value>
        public AccessTokenType TokenType { get; set; }

        /// <summary>
        /// Gets or sets the type of the reference token handle.
        /// </summary>
        /// <value>
        /// The type of the token.
        /// </value>
        public string ReferenceTokenHandle { get; set; }
    }
}