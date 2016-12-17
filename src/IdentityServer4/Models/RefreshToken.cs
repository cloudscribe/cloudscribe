// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer4.Models
{
    /// <summary>
    /// Models a refresh token.
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the life time.
        /// </summary>
        /// <value>
        /// The life time.
        /// </value>
        public int Lifetime { get; set; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public Token AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the original subject that requiested the token.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public ClaimsPrincipal Subject => IdentityServerPrincipal.FromSubjectId(SubjectId, AccessToken.Claims);

        /// <summary>
        /// Gets or sets the version number.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; } = 4;

        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId => AccessToken.ClientId;

        /// <summary>
        /// Gets the subject identifier.
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public string SubjectId => AccessToken.SubjectId;

        /// <summary>
        /// Gets the scopes.
        /// </summary>
        /// <value>
        /// The scopes.
        /// </value>
        public IEnumerable<string> Scopes => AccessToken.Scopes;
    }
}