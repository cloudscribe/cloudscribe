// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace cloudscribe.Core.IdentityServer.EFCore
{
    public class EfConstants
    {
        public class TableNames
        {
            //public const string Scope = "csids_Scopes";
            //public const string ScopeClaim = "csids_ScopeClaims";
            //public const string ScopeSecrets = "csids_ScopeSecrets";

            public const string IdentityResource = "csids_IdentityResources";
            public const string IdentityClaim = "csids_IdentityClaims";
            public const string ApiResource = "csids_ApiResources";
            public const string ApiSecret = "csids_ApiSecrets";
            public const string ApiClaim = "csids_ApiClaims";
            public const string ApiScope = "csids_ApiScopes";
            public const string ApiScopeClaim = "csids_ApiScopeClaims";

            public const string PersistedGrant = "csids_PersistedGrants";
            
            public const string Client = "csids_Clients";
            public const string ClientGrantType = "csids_ClientGrantTypes";
            public const string ClientRedirectUri = "csids_ClientRedirectUris";
            public const string ClientPostLogoutRedirectUri = "csids_ClientPostLogoutRedirectUris";
            public const string ClientScopes = "csids_ClientScopes";
            public const string ClientSecret = "csids_ClientSecrets";
            public const string ClientClaim = "csids_ClientClaims";
            public const string ClientIdPRestriction = "csids_ClientIdPRestrictions";
            public const string ClientCorsOrigin = "csids_ClientCorsOrigins";
        }
    }
}