// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;

namespace IdentityServer4.Events
{
    /// <summary>
    /// Details base class for issued token events
    /// </summary>
    public class TokenIssuedDetailsBase
    {
        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public string SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        /// <value>
        /// The scopes.
        /// </value>
        public IEnumerable<string> Scopes { get; set; }

        /// <summary>
        /// Gets or sets the lifetime.
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public int Lifetime { get; set; }

        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        public Dictionary<string, object> Claims { get; set; }
    }
}