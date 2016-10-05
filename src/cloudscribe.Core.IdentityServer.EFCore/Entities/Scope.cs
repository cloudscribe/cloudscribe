// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;

namespace cloudscribe.Core.IdentityServer.EFCore.Entities
{
    public class Scope
    {
        public int Id { get; set; }

        public string SiteId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public int Type { get; set; }
        public List<ScopeClaim> Claims { get; set; }
        public bool IncludeAllClaimsForUser { get; set; }
        public string ClaimsRule { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public List<ScopeSecret> ScopeSecrets { get; set; }
        public bool AllowUnrestrictedIntrospection { get; set; }

    }
}