// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ScopeViewModel
    {
        public ScopeViewModel(Scope scope, bool check)
        {
            Name = scope.Name;
            DisplayName = scope.DisplayName;
            Description = scope.Description;
            Emphasize = scope.Emphasize;
            Required = scope.Required;
            Checked = check || scope.Required;
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Emphasize { get; set; }
        public bool Required { get; set; }
        public bool Checked { get; set; }
    }
}
