// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;

namespace cloudscribe.Core.IdentityServer.EFCore.Entities
{
    public class Client
    {
        public int Id { get; set; }

        public string SiteId { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public bool Enabled { get; set; }
        public List<ClientSecret> ClientSecrets { get; set; }
        public bool RequireClientSecret { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public bool RequireConsent { get; set; }
        public bool AllowRememberConsent { get; set; }
        public List<ClientGrantType> AllowedGrantTypes { get; set; }
        public bool RequirePkce { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public List<ClientRedirectUri> RedirectUris { get; set; }
        public List<ClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; }
        public string LogoutUri { get; set; }
        public bool LogoutSessionRequired { get; set; }
        public bool AllowAccessToAllScopes { get; set; }
        public List<ClientScope> AllowedScopes { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public int AccessTokenLifetime { get; set; }
        public int AuthorizationCodeLifetime { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }
        public int RefreshTokenUsage { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public int AccessTokenType { get; set; }
        public bool EnableLocalLogin { get; set; }
        public List<ClientIdPRestriction> IdentityProviderRestrictions { get; set; }
        public bool IncludeJwtId { get; set; }
        public List<ClientClaim> Claims { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public bool PrefixClientClaims { get; set; }
        public List<ClientCorsOrigin> AllowedCorsOrigins { get; set; }
        public bool PublicClient { get; set; }
        public bool AllowPromptNone { get; set; }
    }
}