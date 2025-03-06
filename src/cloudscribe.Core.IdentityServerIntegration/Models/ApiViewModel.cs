// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ApiViewModel
    {
        public static ApiViewModel GetOfflineAccess(bool check)
        {
            return new ApiViewModel
            {
                Name = IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = "Offline Access",
                Description = "Access to your applications and resources, even when you are offline",
                Checked = check
            };
        }

        private ApiViewModel()
        {
        }

        public ApiViewModel(ApiResource scope, bool check)
        {
            Name = scope.Name;
            DisplayName = scope.DisplayName;
            Description = scope.Description;
            Checked = check;
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Checked { get; set; }
    }
}
