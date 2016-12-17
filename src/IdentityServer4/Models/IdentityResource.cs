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
    /// Models a user identity resource.
    /// </summary>
    public class IdentityResource
    {
        public IdentityResource()
        {
        }

        public IdentityResource(string name, IEnumerable<string> claimTypes)
            : this(name, name, claimTypes)
        {
        }

        public IdentityResource(string name, string displayName, IEnumerable<string> claimTypes)
        {
            if (name.IsMissing()) throw new ArgumentNullException(nameof(name));
            if (claimTypes.IsNullOrEmpty()) throw new ArgumentException("Must provide at least one claim type", nameof(claimTypes));

            Name = name;
            DisplayName = displayName;

            foreach(var type in claimTypes)
            {
                UserClaims.Add(type);
            }
        }

        /// <summary>
        /// Indicates if this resource is enabled and can be requested. Defaults to true.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// The unique name of the identity resource. This is the value a client will use for the scope parameter in the authorize request.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display name. This value will be used e.g. on the consent screen.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Description. This value will be used e.g. on the consent screen.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Specifies whether the user can de-select the scope on the consent screen (if the consent screen wants to implement such a feature). Defaults to false.
        /// </summary>
        public bool Required { get; set; } = false;

        /// <summary>
        /// Specifies whether the consent screen will emphasize this scope (if the consent screen wants to implement such a feature). 
        /// Use this setting for sensitive or important scopes. Defaults to false.
        /// </summary>
        public bool Emphasize { get; set; } = false;

        /// <summary>
        /// Specifies whether this scope is shown in the discovery document. Defaults to true.
        /// </summary>
        public bool ShowInDiscoveryDocument { get; set; } = true;
        
        /// <summary>
        /// List of associated user claims that should be included in the identity token.
        /// </summary>
        public ICollection<string> UserClaims { get; set; } = new HashSet<string>();
    }
}
