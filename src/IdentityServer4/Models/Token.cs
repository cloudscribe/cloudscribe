// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer4.Models
{
    /// <summary>
    /// Models a token.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        public Token()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="tokenType">Type of the token.</param>
        public Token(string tokenType)
        {
            Type = tokenType;
        }

        /// <summary>
        /// Gets or sets the audiences.
        /// </summary>
        /// <value>
        /// The audiences.
        /// </value>
        public ICollection<string> Audiences { get; set; } = new HashSet<string>();
        
        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        /// <value>
        /// The issuer.
        /// </value>
        public string Issuer { get; set; }
        
        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        public DateTime CreationTime { get; set; } = DateTimeHelper.UtcNow;
        
        /// <summary>
        /// Gets or sets the lifetime.
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public int Lifetime { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; } = OidcConstants.TokenTypes.AccessToken;

        /// <summary>
        /// Gets or sets the ID of the client.
        /// </summary>
        /// <value>
        /// The ID of the client.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the type of access token of the client
        /// </summary>
        /// <value>
        /// The access token type specified by the client.
        /// </value>
        public AccessTokenType AccessTokenType { get; set; }

        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        public List<Claim> Claims { get; set; } = new List<Claim>();

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; } = 4;

        /// <summary>
        /// Gets the subject identifier.
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public string SubjectId => Claims.Where(x => x.Type == JwtClaimTypes.Subject).Select(x => x.Value).SingleOrDefault();
      
        /// <summary>
        /// Gets the scopes.
        /// </summary>
        /// <value>
        /// The scopes.
        /// </value>
        public IEnumerable<string> Scopes => Claims.Where(x => x.Type == JwtClaimTypes.Scope).Select(x => x.Value);
    }
}