using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ClientItemViewModel
    {
        public ClientItemViewModel()
        {

        }

        public ClientItemViewModel(string siteId, Client client)
        {
            if (string.IsNullOrEmpty(siteId)) throw new ArgumentException("SiteId must be provided");
            if (client == null) throw new ArgumentNullException("client must be provided");

            SiteId = siteId;
            ClientId = client.ClientId;
            ClientName = client.ClientName;
            AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime;
            AccessTokenLifetime = client.AccessTokenLifetime;
            AccessTokenType = client.AccessTokenType;
            //AllowAccessToAllScopes = client.AllowAccessToAllScopes;
            AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser;
            AllowRememberConsent = client.AllowRememberConsent;
            AlwaysSendClientClaims = client.AlwaysSendClientClaims;
            AuthorizationCodeLifetime = client.AuthorizationCodeLifetime;
            ClientUri = client.ClientUri;
            Enabled = client.Enabled;
            EnableLocalLogin = client.EnableLocalLogin;
            IdentityTokenLifetime = client.IdentityTokenLifetime;
            IncludeJwtId = client.IncludeJwtId;
            LogoUri = client.LogoUri;
            LogoutSessionRequired = client.LogoutSessionRequired;
            LogoutUri = client.LogoutUri;
            PrefixClientClaims = client.PrefixClientClaims;
            RequireClientSecret = client.RequireClientSecret;
            RequireConsent = client.RequireConsent;
            RequirePkce = client.RequirePkce;
            SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime;
            UpdateAccessTokenClaimsOnRefresh = client.UpdateAccessTokenClaimsOnRefresh;
            RefreshTokenExpiration = client.RefreshTokenExpiration;
            RefreshTokenUsage = client.RefreshTokenUsage;

            Client = client;

        }

        //TODO: localize error messages

        public Client Client { get; private set; } = null;

        [Required]
        public string SiteId { get; set; }

        [Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string ClientId { get; set; }

        [Required]
        public string ClientName { get; set; }

        public AccessTokenType AccessTokenType { get; set; }

        public TokenExpiration RefreshTokenExpiration { get; set; }
        public TokenUsage RefreshTokenUsage { get; set; }

        public int AbsoluteRefreshTokenLifetime { get; set; }
        public int AccessTokenLifetime { get; set; }
        
        public bool AllowAccessToAllScopes { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }

        public bool AllowRememberConsent { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public int AuthorizationCodeLifetime { get; set; }

        public string ClientUri { get; set; }
        public bool Enabled { get; set; }
        public bool EnableLocalLogin { get; set; }

        public int IdentityTokenLifetime { get; set; }
        public bool IncludeJwtId { get; set; }
        public string LogoUri { get; set; }
        public bool LogoutSessionRequired { get; set; }
        public string LogoutUri { get; set; }

        public bool PrefixClientClaims { get; set; }

        public bool RequireClientSecret { get; set; }
        public bool RequireConsent { get; set; }
        public bool RequirePkce { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

        

    }
}
