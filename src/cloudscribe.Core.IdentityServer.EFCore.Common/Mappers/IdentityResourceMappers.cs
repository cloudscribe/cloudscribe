// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using cloudscribe.Core.IdentityServer.EFCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace cloudscribe.Core.IdentityServer.EFCore.Mappers
{
    public static class IdentityResourceMappers
    {
        public static IdentityServer4.Models.IdentityResource ToModel(this IdentityResource resource)
        {
            if (resource == null) return null;

            return new IdentityServer4.Models.IdentityResource
            {
                Name                    = resource.Name,
                DisplayName             = resource.DisplayName,
                Description             = resource.Description,
                Enabled                 = resource.Enabled,
                Required                = resource.Required,
                Emphasize               = resource.Emphasize,
                ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                
                UserClaims = resource.UserClaims?.Select(x => x.Type).ToList() 
                             ?? new List<string>(),
                
                Properties = resource.Properties?.ToDictionary(x => x.Key, x => x.Value) 
                             ?? new Dictionary<string, string>()
            };
        }

        public static IdentityResource ToEntity(this IdentityServer4.Models.IdentityResource resource)
        {
            if (resource == null) return null;

            return new IdentityResource
            {
                Name                    = resource.Name,
                DisplayName             = resource.DisplayName,
                Description             = resource.Description,
                Enabled                 = resource.Enabled,
                Required                = resource.Required,
                Emphasize               = resource.Emphasize,
                ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                
                UserClaims = resource.UserClaims?.Select(x => new IdentityClaim 
                { 
                    Type = x 
                }).ToList() 
                ?? new List<IdentityClaim>(),
                
                Properties = resource.Properties?.Select(x => new IdentityResourceProperty 
                { 
                    Key = x.Key, 
                    Value = x.Value 
                }).ToList() 
                ?? new List<IdentityResourceProperty>()
            };
        }
    }
}
