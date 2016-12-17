// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Models a validated request to the token endpoint.
    /// </summary>
    public class ValidatedTokenRequest : ValidatedRequest
    {
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public Client Client { get; set; }
        
        /// <summary>
        /// Gets or sets the type of the grant.
        /// </summary>
        /// <value>
        /// The type of the grant.
        /// </value>
        public string GrantType { get; set; }
        
        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        /// <value>
        /// The scopes.
        /// </value>
        public IEnumerable<string> Scopes { get; set; }

        /// <summary>
        /// Gets or sets the username used in the request.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
        
        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        public RefreshToken RefreshToken { get; set; }
        
        /// <summary>
        /// Gets or sets the refresh token handle.
        /// </summary>
        /// <value>
        /// The refresh token handle.
        /// </value>
        public string RefreshTokenHandle { get; set; }

        /// <summary>
        /// Gets or sets the authorization code.
        /// </summary>
        /// <value>
        /// The authorization code.
        /// </value>
        public AuthorizationCode AuthorizationCode { get; set; }

        /// <summary>
        /// Gets or sets the authorization code handle.
        /// </summary>
        /// <value>
        /// The authorization code handle.
        /// </value>
        public string AuthorizationCodeHandle { get; set; }

        /// <summary>
        /// Gets or sets the code verifier.
        /// </summary>
        /// <value>
        /// The code verifier.
        /// </value>
        public string CodeVerifier { get; set; }
    }
}