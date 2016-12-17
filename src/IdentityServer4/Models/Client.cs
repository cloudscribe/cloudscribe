// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System;
using IdentityServer4.Extensions;
using IdentityModel;

namespace IdentityServer4.Models
{
    /// <summary>
    /// Models an OpenID Connect or OAuth2 client
    /// </summary>
    public class Client
    {
        // setting grant types should be atomic
        private IEnumerable<string> _allowedGrantTypes = GrantTypes.Implicit;

        /// <summary>
        /// Specifies if client is enabled (defaults to true)
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Unique ID of the client
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the protocol type.
        /// </summary>
        /// <value>
        /// The protocol type.
        /// </value>
        public string ProtocolType { get; set; } = IdentityServerConstants.ProtocolTypes.OpenIdConnect;

        /// <summary>
        /// Client secrets - only relevant for flows that require a secret
        /// </summary>
        public ICollection<Secret> ClientSecrets { get; set; } = new HashSet<Secret>();

        /// <summary>
        /// If set to false, no client secret is needed to request tokens at the token endpoint (defaults to true)
        /// </summary>
        public bool RequireClientSecret { get; set; } = true;

        /// <summary>
        /// Client display name (used for logging and consent screen)
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// URI to further information about client (used on consent screen)
        /// </summary>
        public string ClientUri { get; set; }
        
        /// <summary>
        /// URI to client logo (used on consent screen)
        /// </summary>
        public string LogoUri { get; set; }

        /// <summary>
        /// Specifies whether a consent screen is required (defaults to true)
        /// </summary>
        public bool RequireConsent { get; set; } = true;

        /// <summary>
        /// Specifies whether user can choose to store consent decisions (defaults to true)
        /// </summary>
        public bool AllowRememberConsent { get; set; } = true;

        /// <summary>
        /// Specifies the allowed grant types (legal combinations of AuthorizationCode, Implicit, Hybrid, ResourceOwner, ClientCredentials). Defaults to Implicit.
        /// </summary>
        public IEnumerable<string> AllowedGrantTypes
        {
            get { return _allowedGrantTypes; }
            set
            {
                ValidateGrantTypes(value);
                _allowedGrantTypes = value.ToArray();
            }
        }

        /// <summary>
        /// Specifies whether a proof key is required for authorization code based token requests
        /// </summary>
        public bool RequirePkce { get; set; } = false;

        /// <summary>
        /// Specifies whether a proof key can be sent using plain method (not recommended and default to false)
        /// </summary>
        public bool AllowPlainTextPkce { get; set; } = false;

        /// <summary>
        /// Controls whether access tokens are transmitted via the browser for this client (defaults to true).
        /// This can prevent accidental leakage of access tokens when multiple response types are allowed.
        /// </summary>
        /// <value>
        /// <c>true</c> if access tokens can be transmitted via the browser; otherwise, <c>false</c>.
        /// </value>
        public bool AllowAccessTokensViaBrowser { get; set; } = false;

        /// <summary>
        /// Specifies allowed URIs to return tokens or authorization codes to
        /// </summary>
        public ICollection<string> RedirectUris { get; set; } = new HashSet<string>();

        /// <summary>
        /// Specifies allowed URIs to redirect to after logout
        /// </summary>
        public ICollection<string> PostLogoutRedirectUris { get; set; } = new HashSet<string>();
        
        /// <summary>
        /// Specifies logout URI at client for HTTP based logout.
        /// </summary>
        public string LogoutUri { get; set; }

        /// <summary>
        /// Specifies is the user's session id should be sent to the LogoutUri. Defaults to true.
        /// </summary>
        public bool LogoutSessionRequired { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [allow offline access].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow offline access]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowOfflineAccess { get; set; } = false;

        /// <summary>
        /// Specifies the api scopes that the client is allowed to request. If empty, the client can't access any scope
        /// </summary>
        public ICollection<string> AllowedScopes { get; set; } = new HashSet<string>();

        /// <summary>
        /// Lifetime of identity token in seconds (defaults to 300 seconds / 5 minutes)
        /// </summary>
        public int IdentityTokenLifetime { get; set; } = 300;

        /// <summary>
        /// Lifetime of access token in seconds (defaults to 3600 seconds / 1 hour)
        /// </summary>
        public int AccessTokenLifetime { get; set; } = 3600;

        /// <summary>
        /// Lifetime of authorization code in seconds (defaults to 300 seconds / 5 minutes)
        /// </summary>
        public int AuthorizationCodeLifetime { get; set; } = 300;

        /// <summary>
        /// Maximum lifetime of a refresh token in seconds. Defaults to 2592000 seconds / 30 days
        /// </summary>
        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;

        /// <summary>
        /// Sliding lifetime of a refresh token in seconds. Defaults to 1296000 seconds / 15 days
        /// </summary>
        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;

        /// <summary>
        /// ReUse: the refresh token handle will stay the same when refreshing tokens
        /// OneTime: the refresh token handle will be updated when refreshing tokens
        /// </summary>
        public TokenUsage RefreshTokenUsage { get; set; } = TokenUsage.OneTimeOnly;

        /// <summary>
        /// Gets or sets a value indicating whether the access token (and its claims) should be updated on a refresh token request.
        /// </summary>
        /// <value>
        /// <c>true</c> if the token should be updated; otherwise, <c>false</c>.
        /// </value>
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; } = false;

        /// <summary>
        /// Absolute: the refresh token will expire on a fixed point in time (specified by the AbsoluteRefreshTokenLifetime)
        /// Sliding: when refreshing the token, the lifetime of the refresh token will be renewed (by the amount specified in SlidingRefreshTokenLifetime). The lifetime will not exceed AbsoluteRefreshTokenLifetime.
        /// </summary>        
        public TokenExpiration RefreshTokenExpiration { get; set; } = TokenExpiration.Absolute;

        /// <summary>
        /// Specifies whether the access token is a reference token or a self contained JWT token (defaults to Jwt).
        /// </summary>
        public AccessTokenType AccessTokenType { get; set; } = AccessTokenType.Jwt;

        /// <summary>
        /// Gets or sets a value indicating whether the local login is allowed for this client. Defaults to <c>true</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if local logins are enabled; otherwise, <c>false</c>.
        /// </value>
        public bool EnableLocalLogin { get; set; } = true;

        /// <summary>
        /// Specifies which external IdPs can be used with this client (if list is empty all IdPs are allowed). Defaults to empty.
        /// </summary>
        public ICollection<string> IdentityProviderRestrictions { get; set; } = new HashSet<string>();

        /// <summary>
        /// Gets or sets a value indicating whether JWT access tokens should include an identifier
        /// </summary>
        /// <value>
        /// <c>true</c> to add an id; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeJwtId { get; set; } = false;

        /// <summary>
        /// Allows settings claims for the client (will be included in the access token).
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        public ICollection<Claim> Claims { get; set; } = new HashSet<Claim>(new ClaimComparer());

        /// <summary>
        /// Gets or sets a value indicating whether client claims should be always included in the access tokens - or only for client credentials flow.
        /// </summary>
        /// <value>
        /// <c>true</c> if claims should always be sent; otherwise, <c>false</c>.
        /// </value>
        public bool AlwaysSendClientClaims { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether all client claims should be prefixed.
        /// </summary>
        /// <value>
        /// <c>true</c> if client claims should be prefixed; otherwise, <c>false</c>.
        /// </value>
        public bool PrefixClientClaims { get; set; } = true;

        /// <summary>
        /// Gets or sets the allowed CORS origins for JavaScript clients.
        /// </summary>
        /// <value>
        /// The allowed CORS origins.
        /// </value>
        public ICollection<string> AllowedCorsOrigins { get; set; } = new HashSet<string>();

        public void ValidateGrantTypes(IEnumerable<string> grantTypes)
        {
            // must set at least one grant type
            if (grantTypes.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Grant types list is empty");
            }

            // spaces are not allowed in grant types
            // todo: check for other characters?
            foreach (var type in grantTypes)
            {
                if (type.Contains(' '))
                {
                    throw new InvalidOperationException("Grant types cannot contain spaces");
                }
            }

            // single grant type, seems to be fine
            if (grantTypes.Count() == 1) return;

            // don't allow duplicate grant types
            if (grantTypes.Count() != grantTypes.Distinct().Count())
            {
                throw new InvalidOperationException("Grant types list contains duplicate values");
            }
            
            // would allow response_type downgrade attack from code to token
            DisallowGrantTypeCombination(GrantType.Implicit, GrantType.AuthorizationCode, grantTypes);
            DisallowGrantTypeCombination(GrantType.Implicit, GrantType.Hybrid, grantTypes);
            
            DisallowGrantTypeCombination(GrantType.AuthorizationCode, GrantType.Hybrid, grantTypes);
            
            return;
        }

        private void DisallowGrantTypeCombination(string value1, string value2, IEnumerable<string> grantTypes)
        {
            if (grantTypes.Contains(value1, StringComparer.Ordinal) &&
                grantTypes.Contains(value2, StringComparer.Ordinal))
            {
                throw new InvalidOperationException($"Grant types list cannot contain both {value1} and {value2}");
            }
        }
    }
}