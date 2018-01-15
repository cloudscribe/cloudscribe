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
            AlwaysIncludeUserClaimsInIdToken = client.AlwaysIncludeUserClaimsInIdToken;
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
            FrontChannelLogoutSessionRequired = client.FrontChannelLogoutSessionRequired;
            FrontChannelLogoutUri = client.FrontChannelLogoutUri;
            BackChannelLogoutSessionRequired = client.BackChannelLogoutSessionRequired;
            BackChannelLogoutUri = client.BackChannelLogoutUri;
            //Consider making client claims prefix value configurable
            //https://github.com/IdentityServer/IdentityServer4/issues/1534
            //PrefixClientClaims = client.PrefixClientClaims;
            ClientClaimsPrefix = client.ClientClaimsPrefix;

            PairWiseSubjectSalt = client.PairWiseSubjectSalt;
            RequireClientSecret = client.RequireClientSecret;
            RequireConsent = client.RequireConsent;
            RequirePkce = client.RequirePkce;
            SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime;
            UpdateAccessTokenClaimsOnRefresh = client.UpdateAccessTokenClaimsOnRefresh;
            RefreshTokenExpiration = client.RefreshTokenExpiration;
            RefreshTokenUsage = client.RefreshTokenUsage;
            AllowOfflineAccess = client.AllowOfflineAccess;

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

        //https://github.com/IdentityServer/IdentityServer4/issues/499
        //public bool AllowAccessToAllScopes { get; set; }

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
        public bool FrontChannelLogoutSessionRequired { get; set; }
        public string FrontChannelLogoutUri { get; set; }

        public bool BackChannelLogoutSessionRequired { get; set; }
        public string BackChannelLogoutUri { get; set; }

        //public bool PrefixClientClaims { get; set; }
        public string ClientClaimsPrefix { get; set; }

        public bool RequireClientSecret { get; set; }
        public bool RequireConsent { get; set; }
        public bool RequirePkce { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

        /// <summary>
        /// When requesting both an id token and access token, should the user claims always be added to the id token instead of requring the client to use the userinfo endpoint.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; } = false;

        /// <summary>
        /// Gets or sets a salt value used in pair-wise subjectId generation for users of this client.
        /// </summary>
        public string PairWiseSubjectSalt { get; set; }

        public bool AllowOfflineAccess { get; set; }


        //these are not implemented in the UI but could be if somone really needs it



        /// <summary>
        /// Specifies whether a proof key can be sent using plain method (not recommended and defaults to <c>false</c>.)
        /// </summary>
        //public bool AllowPlainTextPkce { get; set; } = false;





    }
}
