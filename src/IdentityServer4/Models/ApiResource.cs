// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Models
{
    /// <summary>
    /// Models a web API resource.
    /// </summary>
    public class ApiResource
    {
        public ApiResource()
        {
        }

        public ApiResource(string name)
            : this(name, name, null)
        {
        }

        public ApiResource(string name, string displayName)
            : this(name, displayName, null)
        {
        }

        public ApiResource(string name, IEnumerable<string> claimTypes)
            : this(name, name, claimTypes)
        {
        }

        public ApiResource(string name, string displayName, IEnumerable<string> claimTypes)
        {
            if (name.IsMissing()) throw new ArgumentNullException(nameof(name));

            Name = name;
            DisplayName = displayName;

            Scopes.Add(new Scope(name, displayName));

            if (!claimTypes.IsNullOrEmpty())
            {
                foreach (var type in claimTypes)
                {
                    UserClaims.Add(type);
                }
            }
        }

        /// <summary>
        /// Indicates if this API is enabled. Defaults to true.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// The unique name of the API. This value is used for authentication with introspection and will be added to the audience of the outgoing access token.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display name of the API resource.
        /// </summary>
        public string DisplayName { get; set; }
        
        /// <summary>
        /// Description of the API resource.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The API secret is used for the introspection endpoint. The API can authenticate with introspection using the API name and secret.
        /// </summary>
        public ICollection<Secret> ApiSecrets { get; set; } = new HashSet<Secret>();

        /// <summary>
        /// List of accociated user claims that should be included in the access token.
        /// </summary>
        public ICollection<string> UserClaims { get; set; } = new HashSet<string>();

        /// <summary>
        /// An API must have at least one scope. Each scope can have different settings.
        /// </summary>
        public ICollection<Scope> Scopes { get; set; } = new HashSet<Scope>();

        internal ApiResource CloneWithScopes(IEnumerable<Scope> scopes)
        {
            return new ApiResource()
            {
                Enabled = Enabled,
                Name = Name,
                ApiSecrets = ApiSecrets,
                Scopes = new HashSet<Scope>(scopes.ToArray()),
                UserClaims = UserClaims,
            };
        }
    }
}
