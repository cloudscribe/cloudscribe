// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace cloudscribe.Core.IdentityServer.EFCore.Mappers
{
    public static class ClientMappers
    {
        public static IdentityServer4.Models.Client ToModel(this Entities.Client entity)
        {
            if (entity == null) return null;

            return new IdentityServer4.Models.Client
            {
                // Basic properties
                ClientId                           = entity.ClientId,
                ClientName                         = entity.ClientName,
                Description                        = entity.Description,
                Enabled                            = entity.Enabled,
                ProtocolType                       = entity.ProtocolType,
                
                // Client authentication
                RequireClientSecret                = entity.RequireClientSecret,
                ClientUri                          = entity.ClientUri,
                LogoUri                            = entity.LogoUri,
                
                // Consent properties
                RequireConsent                     = entity.RequireConsent,
                AllowRememberConsent               = entity.AllowRememberConsent,
                AlwaysIncludeUserClaimsInIdToken   = entity.AlwaysIncludeUserClaimsInIdToken,
                
                // PKCE properties
                RequirePkce                        = entity.RequirePkce,
                AllowPlainTextPkce                 = entity.AllowPlainTextPkce,
                AllowAccessTokensViaBrowser        = entity.AllowAccessTokensViaBrowser,
                
                // Logout properties
                FrontChannelLogoutUri              = entity.FrontChannelLogoutUri,
                FrontChannelLogoutSessionRequired  = entity.FrontChannelLogoutSessionRequired,
                BackChannelLogoutUri               = entity.BackChannelLogoutUri,
                BackChannelLogoutSessionRequired   = entity.BackChannelLogoutSessionRequired,
                
                // Token properties
                AllowOfflineAccess                 = entity.AllowOfflineAccess,
                IdentityTokenLifetime              = entity.IdentityTokenLifetime,
                AccessTokenLifetime                = entity.AccessTokenLifetime,
                AuthorizationCodeLifetime          = entity.AuthorizationCodeLifetime,
                ConsentLifetime                    = entity.ConsentLifetime,
                AbsoluteRefreshTokenLifetime       = entity.AbsoluteRefreshTokenLifetime,
                SlidingRefreshTokenLifetime        = entity.SlidingRefreshTokenLifetime,
                RefreshTokenUsage                  = (IdentityServer4.Models.TokenUsage)entity.RefreshTokenUsage,
                UpdateAccessTokenClaimsOnRefresh   = entity.UpdateAccessTokenClaimsOnRefresh,
                RefreshTokenExpiration             = (IdentityServer4.Models.TokenExpiration)entity.RefreshTokenExpiration,
                AccessTokenType                    = (IdentityServer4.Models.AccessTokenType)entity.AccessTokenType,
                
                // Login properties
                EnableLocalLogin                   = entity.EnableLocalLogin,
                IncludeJwtId                       = entity.IncludeJwtId,
                
                // Claims properties
                AlwaysSendClientClaims             = entity.AlwaysSendClientClaims,
                ClientClaimsPrefix                 = entity.ClientClaimsPrefix,
                PairWiseSubjectSalt                = entity.PairWiseSubjectSalt,
                
                // Device flow
                UserSsoLifetime                    = entity.UserSsoLifetime,
                UserCodeType                       = entity.UserCodeType,
                DeviceCodeLifetime                 = entity.DeviceCodeLifetime,
                
                // Collection 1: ClientSecrets
                ClientSecrets = entity.ClientSecrets?.Select(x => new IdentityServer4.Models.Secret
                {
                    Description = x.Description,
                    Value       = x.Value,
                    Expiration  = x.Expiration,
                    Type        = x.Type
                }).ToList() 
                ?? new List<IdentityServer4.Models.Secret>(),
                
                // Collection 2: AllowedGrantTypes
                AllowedGrantTypes = entity.AllowedGrantTypes?.Select(x => x.GrantType).ToList() 
                                    ?? new List<string>(),
                
                // Collection 3: RedirectUris
                RedirectUris = entity.RedirectUris?.Select(x => x.RedirectUri).ToList() 
                               ?? new List<string>(),
                
                // Collection 4: PostLogoutRedirectUris
                PostLogoutRedirectUris = entity.PostLogoutRedirectUris?.Select(x => x.PostLogoutRedirectUri).ToList() 
                                         ?? new List<string>(),
                
                // Collection 5: AllowedScopes
                AllowedScopes = entity.AllowedScopes?.Select(x => x.Scope).ToList() 
                                ?? new List<string>(),
                
                // Collection 6: Claims
                Claims = entity.Claims?.Select(x => new Claim(x.Type, x.Value)).ToList() 
                         ?? new List<Claim>(),
                
                // Collection 7: IdentityProviderRestrictions
                IdentityProviderRestrictions = entity.IdentityProviderRestrictions?.Select(x => x.Provider).ToList() 
                                               ?? new List<string>(),
                
                // Collection 8: AllowedCorsOrigins
                AllowedCorsOrigins = entity.AllowedCorsOrigins?.Select(x => x.Origin).ToList() 
                                     ?? new List<string>(),
                
                // Collection 9: Properties
                Properties = entity.Properties?.ToDictionary(x => x.Key, x => x.Value) 
                             ?? new Dictionary<string, string>()
            };
        }

        public static Entities.Client ToEntity(this IdentityServer4.Models.Client model)
        {
            if (model == null) return null;

            return new Entities.Client
            {
                // Basic properties
                ClientId                           = model.ClientId,
                ClientName                         = model.ClientName,
                Description                        = model.Description,
                Enabled                            = model.Enabled,
                ProtocolType                       = model.ProtocolType,
                
                // Client authentication
                RequireClientSecret                = model.RequireClientSecret,
                ClientUri                          = model.ClientUri,
                LogoUri                            = model.LogoUri,
                
                // Consent properties
                RequireConsent                     = model.RequireConsent,
                AllowRememberConsent               = model.AllowRememberConsent,
                AlwaysIncludeUserClaimsInIdToken   = model.AlwaysIncludeUserClaimsInIdToken,
                
                // PKCE properties
                RequirePkce                        = model.RequirePkce,
                AllowPlainTextPkce                 = model.AllowPlainTextPkce,
                AllowAccessTokensViaBrowser        = model.AllowAccessTokensViaBrowser,
                
                // Logout properties
                FrontChannelLogoutUri              = model.FrontChannelLogoutUri,
                FrontChannelLogoutSessionRequired  = model.FrontChannelLogoutSessionRequired,
                BackChannelLogoutUri               = model.BackChannelLogoutUri,
                BackChannelLogoutSessionRequired   = model.BackChannelLogoutSessionRequired,
                
                // Token properties
                AllowOfflineAccess                 = model.AllowOfflineAccess,
                IdentityTokenLifetime              = model.IdentityTokenLifetime,
                AccessTokenLifetime                = model.AccessTokenLifetime,
                AuthorizationCodeLifetime          = model.AuthorizationCodeLifetime,
                ConsentLifetime                    = model.ConsentLifetime,
                AbsoluteRefreshTokenLifetime       = model.AbsoluteRefreshTokenLifetime,
                SlidingRefreshTokenLifetime        = model.SlidingRefreshTokenLifetime,
                RefreshTokenUsage                  = (int)model.RefreshTokenUsage,
                UpdateAccessTokenClaimsOnRefresh   = model.UpdateAccessTokenClaimsOnRefresh,
                RefreshTokenExpiration             = (int)model.RefreshTokenExpiration,
                AccessTokenType                    = (int)model.AccessTokenType,
                
                // Login properties
                EnableLocalLogin                   = model.EnableLocalLogin,
                IncludeJwtId                       = model.IncludeJwtId,
                
                // Claims properties
                AlwaysSendClientClaims             = model.AlwaysSendClientClaims,
                ClientClaimsPrefix                 = model.ClientClaimsPrefix,
                PairWiseSubjectSalt                = model.PairWiseSubjectSalt,
                
                // Device flow
                UserSsoLifetime                    = model.UserSsoLifetime,
                UserCodeType                       = model.UserCodeType,
                DeviceCodeLifetime                 = model.DeviceCodeLifetime,
                
                // Collection 1: ClientSecrets
                ClientSecrets = model.ClientSecrets?.Select(x => new Entities.ClientSecret
                {
                    Description = x.Description,
                    Value       = x.Value,
                    Expiration  = x.Expiration,
                    Type        = x.Type
                }).ToList() 
                ?? new List<Entities.ClientSecret>(),
                
                // Collection 2: AllowedGrantTypes
                AllowedGrantTypes = model.AllowedGrantTypes?.Select(x => new Entities.ClientGrantType 
                { 
                    GrantType = x 
                }).ToList() 
                ?? new List<Entities.ClientGrantType>(),
                
                // Collection 3: RedirectUris
                RedirectUris = model.RedirectUris?.Select(x => new Entities.ClientRedirectUri 
                { 
                    RedirectUri = x 
                }).ToList() 
                ?? new List<Entities.ClientRedirectUri>(),
                
                // Collection 4: PostLogoutRedirectUris
                PostLogoutRedirectUris = model.PostLogoutRedirectUris?.Select(x => new Entities.ClientPostLogoutRedirectUri 
                { 
                    PostLogoutRedirectUri = x 
                }).ToList() 
                ?? new List<Entities.ClientPostLogoutRedirectUri>(),
                
                // Collection 5: AllowedScopes
                AllowedScopes = model.AllowedScopes?.Select(x => new Entities.ClientScope 
                { 
                    Scope = x 
                }).ToList() 
                ?? new List<Entities.ClientScope>(),
                
                // Collection 6: Claims
                Claims = model.Claims?.Select(x => new Entities.ClientClaim 
                { 
                    Type  = x.Type, 
                    Value = x.Value 
                }).ToList() 
                ?? new List<Entities.ClientClaim>(),
                
                // Collection 7: IdentityProviderRestrictions
                IdentityProviderRestrictions = model.IdentityProviderRestrictions?.Select(x => new Entities.ClientIdPRestriction 
                { 
                    Provider = x 
                }).ToList() 
                ?? new List<Entities.ClientIdPRestriction>(),
                
                // Collection 8: AllowedCorsOrigins
                AllowedCorsOrigins = model.AllowedCorsOrigins?.Select(x => new Entities.ClientCorsOrigin 
                { 
                    Origin = x 
                }).ToList() 
                ?? new List<Entities.ClientCorsOrigin>(),
                
                // Collection 9: Properties
                Properties = model.Properties?.Select(x => new Entities.ClientProperty 
                { 
                    Key   = x.Key, 
                    Value = x.Value 
                }).ToList() 
                ?? new List<Entities.ClientProperty>()
            };
        }
    }
}
