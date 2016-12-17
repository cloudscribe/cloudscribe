// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer4.Validation
{
    /// <summary>
    /// Modles the validation result of access tokens and id tokens.
    /// </summary>
    public class TokenValidationResult : ValidationResult
    {
        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        public IEnumerable<Claim> Claims { get; set; }
        
        /// <summary>
        /// Gets or sets the JWT.
        /// </summary>
        /// <value>
        /// The JWT.
        /// </value>
        public string Jwt { get; set; }
        
        /// <summary>
        /// Gets or sets the reference token.
        /// </summary>
        /// <value>
        /// The reference token.
        /// </value>
        public Token ReferenceToken { get; set; }
        
        /// <summary>
        /// Gets or sets the reference token identifier.
        /// </summary>
        /// <value>
        /// The reference token identifier.
        /// </value>
        public string ReferenceTokenId { get; set; }
        
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public Client Client { get; set; }
    }
}