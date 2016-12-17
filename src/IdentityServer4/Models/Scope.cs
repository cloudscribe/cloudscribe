// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using System;
using System.Collections.Generic;

namespace IdentityServer4.Models
{
    /// <summary>
    /// Models access to an API resource
    /// </summary>
    public class Scope
    {
        public Scope()
        {
        }

        public Scope(string name)
            : this(name, name, null)
        {
        }

        public Scope(string name, string displayName)
            : this(name, displayName, null)
        {
        }

        public Scope(string name, IEnumerable<string> claimTypes)
            : this(name, name, claimTypes)
        {
        }

        public Scope(string name, string displayName, IEnumerable<string> claimTypes)
        {
            if (name.IsMissing()) throw new ArgumentNullException(nameof(name));

            Name = name;
            DisplayName = displayName;

            if (!claimTypes.IsNullOrEmpty())
            {
                foreach (var type in claimTypes)
                {
                    UserClaims.Add(type);
                }
            }
        }

        /// <summary>
        /// Name of the scope. This is the value a client will use to request the scope.
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
        /// Specifies whether the user can de-select the scope on the consent screen. Defaults to false.
        /// </summary>
        public bool Required { get; set; } = false;

        /// <summary>
        /// Specifies whether the consent screen will emphasize this scope. Use this setting for sensitive or important scopes. Defaults to false.
        /// </summary>
        public bool Emphasize { get; set; } = false;

        /// <summary>
        /// Specifies whether this scope is shown in the discovery document. Defaults to true.
        /// </summary>
        public bool ShowInDiscoveryDocument { get; set; } = true;

        /// <summary>
        /// List of user claims that should be included in the access token.
        /// </summary>
        public ICollection<string> UserClaims { get; set; } = new HashSet<string>();
    }
}