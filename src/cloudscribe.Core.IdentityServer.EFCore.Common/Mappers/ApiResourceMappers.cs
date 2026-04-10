// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using cloudscribe.Core.IdentityServer.EFCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace cloudscribe.Core.IdentityServer.EFCore.Mappers
{
    public static class ApiResourceMappers
    {
        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static IdentityServer4.Models.ApiResource ToModel(this Entities.ApiResource entity)
        {
            if (entity == null) return null;

            return new IdentityServer4.Models.ApiResource
            {
                Name        = entity.Name,
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                Enabled     = entity.Enabled,
                
                UserClaims = entity.UserClaims?.Select(x => x.Type).ToList() 
                             ?? new List<string>(),
                
                Scopes = entity.Scopes?.Select(x => new IdentityServer4.Models.Scope
                {
                    Name                    = x.Name,
                    DisplayName             = x.DisplayName,
                    Description             = x.Description,
                    Required                = x.Required,
                    Emphasize               = x.Emphasize,
                    ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
                    UserClaims              = x.UserClaims?.Select(c => c.Type).ToList() 
                                              ?? new List<string>()
                }).ToList() 
                ?? new List<IdentityServer4.Models.Scope>(),
                
                ApiSecrets = entity.Secrets?.Select(x => new IdentityServer4.Models.Secret
                {
                    Description = x.Description,
                    Value       = x.Value,
                    Expiration  = x.Expiration,
                    Type        = x.Type
                }).ToList() 
                ?? new List<IdentityServer4.Models.Secret>(),
                
                Properties = entity.Properties?.ToDictionary(x => x.Key, x => x.Value) 
                             ?? new Dictionary<string, string>()
            };
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Entities.ApiResource ToEntity(this IdentityServer4.Models.ApiResource model)
        {
            if (model == null) return null;

            return new Entities.ApiResource
            {
                Name        = model.Name,
                DisplayName = model.DisplayName,
                Description = model.Description,
                Enabled     = model.Enabled,
                
                UserClaims = model.UserClaims?.Select(x => new ApiResourceClaim 
                { 
                    Type = x 
                }).ToList() 
                ?? new List<ApiResourceClaim>(),
                
                Scopes = model.Scopes?.Select(x => new ApiScope
                {
                    Name                    = x.Name,
                    DisplayName             = x.DisplayName,
                    Description             = x.Description,
                    Required                = x.Required,
                    Emphasize               = x.Emphasize,
                    ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
                    UserClaims              = x.UserClaims?.Select(c => new ApiScopeClaim 
                    { 
                        Type = c 
                    }).ToList() 
                                              ?? new List<ApiScopeClaim>()
                }).ToList() 
                ?? new List<ApiScope>(),
                
                Secrets = model.ApiSecrets?.Select(x => new ApiSecret
                {
                    Description = x.Description,
                    Value       = x.Value,
                    Expiration  = x.Expiration,
                    Type        = x.Type
                }).ToList() 
                ?? new List<ApiSecret>(),
                
                Properties = model.Properties?.Select(x => new ApiResourceProperty 
                { 
                    Key   = x.Key, 
                    Value = x.Value 
                }).ToList() 
                ?? new List<ApiResourceProperty>()
            };
        }
    }
}
