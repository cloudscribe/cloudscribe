using cloudscribe.Core.IdentityServer.EFCore.Entities;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using System.Security.Claims;
using Xunit;
using EntityClient = cloudscribe.Core.IdentityServer.EFCore.Entities.Client;

namespace cloudscribe.Core.Identity.ModelMappingTests
{
    /// <summary>
    /// Tests for Client mapping between IdentityServer4 models and EF Core entities.
    /// These tests verify current AutoMapper behavior before replacing with manual mapping.
    /// 
    /// Complexity: High
    /// - 60+ main properties (booleans, strings, ints, DateTimes)
    /// - 10 child collections:
    ///   1. ClientSecrets (Secret with Description, Value, Expiration, Type)
    ///   2. AllowedGrantTypes (string list)
    ///   3. RedirectUris (string list)
    ///   4. PostLogoutRedirectUris (string list)
    ///   5. AllowedScopes (string list)
    ///   6. Claims (Claim with Type, Value)
    ///   7. IdentityProviderRestrictions (string list)
    ///   8. AllowedCorsOrigins (string list)
    ///   9. Properties (dictionary)
    ///   10. Various other collections and complex mappings
    /// - Bidirectional mapping (entity ↔ model)
    /// </summary>
    public class ClientMappingTests
    {
        #region Entity to Model Mapping (ToModel)

        [Fact]
        public void ToModel_WithValidEntity_MapsAllCoreProperties()
        {
            // Arrange
            var entity = new EntityClient
            {
                Id = 100,
                SiteId = "site-123",
                Enabled = true,
                ClientId = "test-client-id",
                ProtocolType = "oidc",
                RequireClientSecret = true,
                ClientName = "Test Client",
                Description = "A test client application",
                ClientUri = "https://client.example.com",
                LogoUri = "https://client.example.com/logo.png",
                RequireConsent = false,
                AllowRememberConsent = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                RequirePkce = true,
                AllowPlainTextPkce = false,
                AllowAccessTokensViaBrowser = true,
                FrontChannelLogoutUri = "https://client.example.com/front-logout",
                FrontChannelLogoutSessionRequired = false,
                BackChannelLogoutUri = "https://client.example.com/back-logout",
                BackChannelLogoutSessionRequired = true,
                AllowOfflineAccess = true,
                IdentityTokenLifetime = 600,
                AccessTokenLifetime = 7200,
                AuthorizationCodeLifetime = 600,
                ConsentLifetime = 1800,
                AbsoluteRefreshTokenLifetime = 5184000,
                SlidingRefreshTokenLifetime = 2592000,
                RefreshTokenUsage = 1,
                UpdateAccessTokenClaimsOnRefresh = true,
                RefreshTokenExpiration = 0,
                AccessTokenType = 0,
                EnableLocalLogin = false,
                IncludeJwtId = true,
                AlwaysSendClientClaims = false,
                ClientClaimsPrefix = "custom_prefix_",
                PairWiseSubjectSalt = "salt-value",
                Created = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc),
                LastAccessed = new DateTime(2024, 12, 1, 14, 22, 33, DateTimeKind.Utc),
                UserSsoLifetime = 3600,
                UserCodeType = "Numeric",
                DeviceCodeLifetime = 600,
                NonEditable = false
            };

            // Act
            var model = entity.ToModel();

            // Assert - Core properties
            Assert.NotNull(model);
            Assert.Equal("test-client-id", model.ClientId);
            Assert.Equal("oidc", model.ProtocolType);
            Assert.True(model.Enabled);
            Assert.True(model.RequireClientSecret);
            Assert.Equal("Test Client", model.ClientName);
            Assert.Equal("A test client application", model.Description);
            Assert.Equal("https://client.example.com", model.ClientUri);
            Assert.Equal("https://client.example.com/logo.png", model.LogoUri);
            Assert.False(model.RequireConsent);
            Assert.True(model.AllowRememberConsent);
            Assert.True(model.AlwaysIncludeUserClaimsInIdToken);
            Assert.True(model.RequirePkce);
            Assert.False(model.AllowPlainTextPkce);
            Assert.True(model.AllowAccessTokensViaBrowser);
            Assert.Equal("https://client.example.com/front-logout", model.FrontChannelLogoutUri);
            Assert.False(model.FrontChannelLogoutSessionRequired);
            Assert.Equal("https://client.example.com/back-logout", model.BackChannelLogoutUri);
            Assert.True(model.BackChannelLogoutSessionRequired);
            Assert.True(model.AllowOfflineAccess);
        }

        [Fact]
        public void ToModel_WithValidEntity_MapsAllTokenLifetimeProperties()
        {
            // Arrange
            var entity = new EntityClient
            {
                ClientId = "lifetime-client",
                IdentityTokenLifetime = 600,
                AccessTokenLifetime = 7200,
                AuthorizationCodeLifetime = 600,
                ConsentLifetime = 1800,
                AbsoluteRefreshTokenLifetime = 5184000,
                SlidingRefreshTokenLifetime = 2592000,
                RefreshTokenUsage = 1,
                UpdateAccessTokenClaimsOnRefresh = true,
                RefreshTokenExpiration = 0,
                AccessTokenType = 0
            };

            // Act
            var model = entity.ToModel();

            // Assert - Token lifetime properties
            Assert.Equal(600, model.IdentityTokenLifetime);
            Assert.Equal(7200, model.AccessTokenLifetime);
            Assert.Equal(600, model.AuthorizationCodeLifetime);
            Assert.Equal(1800, model.ConsentLifetime);
            Assert.Equal(5184000, model.AbsoluteRefreshTokenLifetime);
            Assert.Equal(2592000, model.SlidingRefreshTokenLifetime);
            Assert.Equal(IdentityServer4.Models.TokenUsage.OneTimeOnly, model.RefreshTokenUsage);
            Assert.True(model.UpdateAccessTokenClaimsOnRefresh);
            // AutoMapper maps int 0 from entity but IdentityServer4.Models.Client constructor sets default to Sliding (1)
            Assert.Equal(IdentityServer4.Models.TokenExpiration.Sliding, model.RefreshTokenExpiration);
            Assert.Equal(IdentityServer4.Models.AccessTokenType.Jwt, model.AccessTokenType);
        }

        [Fact]
        public void ToModel_WithValidEntity_MapsTimestampAndAdvancedProperties()
        {
            // Arrange
            var entity = new EntityClient
            {
                ClientId = "timestamp-client",
                UserSsoLifetime = 7200,
                UserCodeType = "Numeric",
                DeviceCodeLifetime = 900,
                EnableLocalLogin = false,
                IncludeJwtId = true,
                AlwaysSendClientClaims = true,
                ClientClaimsPrefix = "my_prefix_",
                PairWiseSubjectSalt = "unique-salt"
            };

            // Act
            var model = entity.ToModel();

            // Assert - Advanced properties
            Assert.Equal(7200, model.UserSsoLifetime);
            Assert.Equal("Numeric", model.UserCodeType);
            Assert.Equal(900, model.DeviceCodeLifetime);
            Assert.False(model.EnableLocalLogin);
            Assert.True(model.IncludeJwtId);
            Assert.True(model.AlwaysSendClientClaims);
            Assert.Equal("my_prefix_", model.ClientClaimsPrefix);
            Assert.Equal("unique-salt", model.PairWiseSubjectSalt);
        }

        [Fact]
        public void ToModel_WithClientSecrets_MapsSecretsCollection()
        {
            // Arrange
            var entity = new EntityClient
            {
                ClientId = "secrets-client",
                ClientSecrets = new List<ClientSecret>
                {
                    new ClientSecret
                    {
                        Id = 1,
                        Description = "Primary secret",
                        Value = "hashed-secret-1",
                        Expiration = new DateTime(2025, 12, 31, 23, 59, 59, DateTimeKind.Utc),
                        Type = "SharedSecret"
                    },
                    new ClientSecret
                    {
                        Id = 2,
                        Description = "Backup secret",
                        Value = "hashed-secret-2",
                        Expiration = null,
                        Type = "SharedSecret"
                    }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.ClientSecrets);
            Assert.Equal(2, model.ClientSecrets.Count);

            var secret1 = model.ClientSecrets.ElementAt(0);
            Assert.Equal("Primary secret", secret1.Description);
            Assert.Equal("hashed-secret-1", secret1.Value);
            Assert.Equal(new DateTime(2025, 12, 31, 23, 59, 59, DateTimeKind.Utc), secret1.Expiration);
            Assert.Equal("SharedSecret", secret1.Type);

            var secret2 = model.ClientSecrets.ElementAt(1);
            Assert.Equal("Backup secret", secret2.Description);
            Assert.Equal("hashed-secret-2", secret2.Value);
            Assert.Null(secret2.Expiration);
        }

        [Fact]
        public void ToModel_WithAllowedGrantTypes_MapsGrantTypesCollection()
        {
            // Arrange
            var entity = new EntityClient
            {
                ClientId = "grant-types-client",
                AllowedGrantTypes = new List<ClientGrantType>
                {
                    new ClientGrantType { Id = 1, GrantType = "authorization_code" },
                    new ClientGrantType { Id = 2, GrantType = "client_credentials" },
                    new ClientGrantType { Id = 3, GrantType = "refresh_token" }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.AllowedGrantTypes);
            Assert.Equal(3, model.AllowedGrantTypes.Count);
            Assert.Contains("authorization_code", model.AllowedGrantTypes);
            Assert.Contains("client_credentials", model.AllowedGrantTypes);
            Assert.Contains("refresh_token", model.AllowedGrantTypes);
        }

        [Fact]
        public void ToModel_WithRedirectUris_MapsRedirectUrisCollection()
        {
            // Arrange
            var entity = new EntityClient
            {
                ClientId = "redirect-client",
                RedirectUris = new List<ClientRedirectUri>
                {
                    new ClientRedirectUri { Id = 1, RedirectUri = "https://app.example.com/callback" },
                    new ClientRedirectUri { Id = 2, RedirectUri = "https://app.example.com/signin-oidc" }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.RedirectUris);
            Assert.Equal(2, model.RedirectUris.Count);
            Assert.Contains("https://app.example.com/callback", model.RedirectUris);
            Assert.Contains("https://app.example.com/signin-oidc", model.RedirectUris);
        }

        [Fact]
        public void ToModel_WithPostLogoutRedirectUris_MapsPostLogoutRedirectUrisCollection()
        {
            // Arrange
            var entity = new EntityClient
            {
                ClientId = "logout-client",
                PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>
                {
                    new ClientPostLogoutRedirectUri { Id = 1, PostLogoutRedirectUri = "https://app.example.com/" },
                    new ClientPostLogoutRedirectUri { Id = 2, PostLogoutRedirectUri = "https://app.example.com/signout-complete" }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.PostLogoutRedirectUris);
            Assert.Equal(2, model.PostLogoutRedirectUris.Count);
            Assert.Contains("https://app.example.com/", model.PostLogoutRedirectUris);
            Assert.Contains("https://app.example.com/signout-complete", model.PostLogoutRedirectUris);
        }

        [Fact]
        public void ToModel_WithAllowedScopes_MapsScopesCollection()
        {
            // Arrange
            var entity = new EntityClient
            {
                ClientId = "scopes-client",
                AllowedScopes = new List<ClientScope>
                {
                    new ClientScope { Id = 1, Scope = "openid" },
                    new ClientScope { Id = 2, Scope = "profile" },
                    new ClientScope { Id = 3, Scope = "email" },
                    new ClientScope { Id = 4, Scope = "api1" }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.AllowedScopes);
            Assert.Equal(4, model.AllowedScopes.Count);
            Assert.Contains("openid", model.AllowedScopes);
            Assert.Contains("profile", model.AllowedScopes);
            Assert.Contains("email", model.AllowedScopes);
            Assert.Contains("api1", model.AllowedScopes);
        }

        [Fact]
        public void ToModel_WithClaims_MapsClaimsCollection()
        {
            // Arrange
            var entity = new EntityClient
            {
                ClientId = "claims-client",
                Claims = new List<ClientClaim>
                {
                    new ClientClaim { Id = 1, Type = "client_type", Value = "spa" },
                    new ClientClaim { Id = 2, Type = "environment", Value = "production" },
                    new ClientClaim { Id = 3, Type = "tier", Value = "premium" }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.Claims);
            Assert.Equal(3, model.Claims.Count);
            Assert.Contains(model.Claims, c => c.Type == "client_type" && c.Value == "spa");
            Assert.Contains(model.Claims, c => c.Type == "environment" && c.Value == "production");
            Assert.Contains(model.Claims, c => c.Type == "tier" && c.Value == "premium");
        }

        [Fact]
        public void ToModel_WithIdentityProviderRestrictions_MapsRestrictionsCollection()
        {
            // Arrange
            var entity = new EntityClient
            {
                ClientId = "idp-restricted-client",
                IdentityProviderRestrictions = new List<ClientIdPRestriction>
                {
                    new ClientIdPRestriction { Id = 1, Provider = "Google" },
                    new ClientIdPRestriction { Id = 2, Provider = "Facebook" }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.IdentityProviderRestrictions);
            Assert.Equal(2, model.IdentityProviderRestrictions.Count);
            Assert.Contains("Google", model.IdentityProviderRestrictions);
            Assert.Contains("Facebook", model.IdentityProviderRestrictions);
        }

        [Fact]
        public void ToModel_WithAllowedCorsOrigins_MapsCorsOriginsCollection()
        {
            // Arrange
            var entity = new EntityClient
            {
                ClientId = "cors-client",
                AllowedCorsOrigins = new List<ClientCorsOrigin>
                {
                    new ClientCorsOrigin { Id = 1, Origin = "https://app.example.com" },
                    new ClientCorsOrigin { Id = 2, Origin = "https://admin.example.com" },
                    new ClientCorsOrigin { Id = 3, Origin = "https://mobile.example.com" }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.AllowedCorsOrigins);
            Assert.Equal(3, model.AllowedCorsOrigins.Count);
            Assert.Contains("https://app.example.com", model.AllowedCorsOrigins);
            Assert.Contains("https://admin.example.com", model.AllowedCorsOrigins);
            Assert.Contains("https://mobile.example.com", model.AllowedCorsOrigins);
        }

        [Fact]
        public void ToModel_WithProperties_MapsPropertiesDictionary()
        {
            // Arrange
            var entity = new EntityClient
            {
                ClientId = "props-client",
                Properties = new List<ClientProperty>
                {
                    new ClientProperty { Id = 1, Key = "custom_setting_1", Value = "value1" },
                    new ClientProperty { Id = 2, Key = "custom_setting_2", Value = "value2" },
                    new ClientProperty { Id = 3, Key = "deployment_region", Value = "us-west" }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.Properties);
            Assert.Equal(3, model.Properties.Count);
            Assert.Equal("value1", model.Properties["custom_setting_1"]);
            Assert.Equal("value2", model.Properties["custom_setting_2"]);
            Assert.Equal("us-west", model.Properties["deployment_region"]);
        }

        [Fact]
        public void ToModel_WithAllCollections_MapsEverything()
        {
            // Arrange
            var entity = new EntityClient
            {
                Id = 999,
                SiteId = "comprehensive-site",
                ClientId = "comprehensive-client",
                ClientName = "Comprehensive Client",
                Enabled = true,
                ClientSecrets = new List<ClientSecret>
                {
                    new ClientSecret { Value = "secret1", Type = "SharedSecret" }
                },
                AllowedGrantTypes = new List<ClientGrantType>
                {
                    new ClientGrantType { GrantType = "authorization_code" }
                },
                RedirectUris = new List<ClientRedirectUri>
                {
                    new ClientRedirectUri { RedirectUri = "https://example.com/callback" }
                },
                PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>
                {
                    new ClientPostLogoutRedirectUri { PostLogoutRedirectUri = "https://example.com/logout" }
                },
                AllowedScopes = new List<ClientScope>
                {
                    new ClientScope { Scope = "openid" }
                },
                Claims = new List<ClientClaim>
                {
                    new ClientClaim { Type = "claim_type", Value = "claim_value" }
                },
                IdentityProviderRestrictions = new List<ClientIdPRestriction>
                {
                    new ClientIdPRestriction { Provider = "Google" }
                },
                AllowedCorsOrigins = new List<ClientCorsOrigin>
                {
                    new ClientCorsOrigin { Origin = "https://example.com" }
                },
                Properties = new List<ClientProperty>
                {
                    new ClientProperty { Key = "key1", Value = "value1" }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.Equal("comprehensive-client", model.ClientId);
            Assert.Single(model.ClientSecrets);
            Assert.Single(model.AllowedGrantTypes);
            Assert.Single(model.RedirectUris);
            Assert.Single(model.PostLogoutRedirectUris);
            Assert.Single(model.AllowedScopes);
            Assert.Single(model.Claims);
            Assert.Single(model.IdentityProviderRestrictions);
            Assert.Single(model.AllowedCorsOrigins);
            Assert.Single(model.Properties);
        }

        [Fact]
        public void ToModel_WithNullEntity_ReturnsNull()
        {
            // Act
            var model = ((EntityClient)null).ToModel();

            // Assert
            Assert.Null(model);
        }

        [Fact]
        public void ToModel_WithEmptyCollections_MapsEmptyCollections()
        {
            // Arrange
            var entity = new EntityClient
            {
                ClientId = "empty-collections-client",
                ClientSecrets = new List<ClientSecret>(),
                AllowedGrantTypes = new List<ClientGrantType>(),
                RedirectUris = new List<ClientRedirectUri>(),
                PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>(),
                AllowedScopes = new List<ClientScope>(),
                Claims = new List<ClientClaim>(),
                IdentityProviderRestrictions = new List<ClientIdPRestriction>(),
                AllowedCorsOrigins = new List<ClientCorsOrigin>(),
                Properties = new List<ClientProperty>()
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.NotNull(model.ClientSecrets);
            Assert.NotNull(model.AllowedGrantTypes);
            Assert.NotNull(model.RedirectUris);
            Assert.NotNull(model.PostLogoutRedirectUris);
            Assert.NotNull(model.AllowedScopes);
            Assert.NotNull(model.Claims);
            Assert.NotNull(model.IdentityProviderRestrictions);
            Assert.NotNull(model.AllowedCorsOrigins);
            Assert.NotNull(model.Properties);
            Assert.Empty(model.ClientSecrets);
            Assert.Empty(model.AllowedGrantTypes);
            Assert.Empty(model.RedirectUris);
            Assert.Empty(model.PostLogoutRedirectUris);
            Assert.Empty(model.AllowedScopes);
            Assert.Empty(model.Claims);
            Assert.Empty(model.IdentityProviderRestrictions);
            Assert.Empty(model.AllowedCorsOrigins);
            Assert.Empty(model.Properties);
        }

        [Fact]
        public void ToModel_WithNullCollections_HandlesGracefully()
        {
            // Arrange
            var entity = new EntityClient
            {
                ClientId = "null-collections-client",
                ClientSecrets = null,
                AllowedGrantTypes = null,
                RedirectUris = null,
                PostLogoutRedirectUris = null,
                AllowedScopes = null,
                Claims = null,
                IdentityProviderRestrictions = null,
                AllowedCorsOrigins = null,
                Properties = null
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.Equal("null-collections-client", model.ClientId);
            // AutoMapper behavior: null collections may become null or empty depending on configuration
            // We're documenting current behavior here
        }

        #endregion

        #region Model to Entity Mapping (ToEntity)

        [Fact]
        public void ToEntity_WithValidModel_MapsAllCoreProperties()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-client-id",
                ProtocolType = "oidc",
                Enabled = false,
                RequireClientSecret = false,
                ClientName = "Model Client",
                Description = "A client from model",
                ClientUri = "https://model.example.com",
                LogoUri = "https://model.example.com/logo.png",
                RequireConsent = true,
                AllowRememberConsent = false,
                AlwaysIncludeUserClaimsInIdToken = false,
                RequirePkce = false,
                AllowPlainTextPkce = true,
                AllowAccessTokensViaBrowser = false,
                FrontChannelLogoutUri = "https://model.example.com/front-logout",
                FrontChannelLogoutSessionRequired = true,
                BackChannelLogoutUri = "https://model.example.com/back-logout",
                BackChannelLogoutSessionRequired = false,
                AllowOfflineAccess = false,
                EnableLocalLogin = true,
                IncludeJwtId = false,
                AlwaysSendClientClaims = true,
                ClientClaimsPrefix = "model_",
                PairWiseSubjectSalt = "model-salt"
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Equal("model-client-id", entity.ClientId);
            Assert.Equal("oidc", entity.ProtocolType);
            Assert.False(entity.Enabled);
            Assert.False(entity.RequireClientSecret);
            Assert.Equal("Model Client", entity.ClientName);
            Assert.Equal("A client from model", entity.Description);
            Assert.Equal("https://model.example.com", entity.ClientUri);
            Assert.Equal("https://model.example.com/logo.png", entity.LogoUri);
            Assert.True(entity.RequireConsent);
            Assert.False(entity.AllowRememberConsent);
            Assert.False(entity.AlwaysIncludeUserClaimsInIdToken);
            Assert.False(entity.RequirePkce);
            Assert.True(entity.AllowPlainTextPkce);
            Assert.False(entity.AllowAccessTokensViaBrowser);
            Assert.Equal("https://model.example.com/front-logout", entity.FrontChannelLogoutUri);
            Assert.True(entity.FrontChannelLogoutSessionRequired);
            Assert.Equal("https://model.example.com/back-logout", entity.BackChannelLogoutUri);
            Assert.False(entity.BackChannelLogoutSessionRequired);
            Assert.False(entity.AllowOfflineAccess);
            Assert.True(entity.EnableLocalLogin);
            Assert.False(entity.IncludeJwtId);
            Assert.True(entity.AlwaysSendClientClaims);
            Assert.Equal("model_", entity.ClientClaimsPrefix);
            Assert.Equal("model-salt", entity.PairWiseSubjectSalt);
        }

        [Fact]
        public void ToEntity_WithValidModel_MapsTokenLifetimeProperties()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-lifetime-client",
                IdentityTokenLifetime = 1200,
                AccessTokenLifetime = 14400,
                AuthorizationCodeLifetime = 1200,
                ConsentLifetime = 3600,
                AbsoluteRefreshTokenLifetime = 10368000,
                SlidingRefreshTokenLifetime = 5184000,
                RefreshTokenUsage = 0,
                UpdateAccessTokenClaimsOnRefresh = false,
                RefreshTokenExpiration = IdentityServer4.Models.TokenExpiration.Sliding,
                AccessTokenType = IdentityServer4.Models.AccessTokenType.Reference
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.Equal(1200, entity.IdentityTokenLifetime);
            Assert.Equal(14400, entity.AccessTokenLifetime);
            Assert.Equal(1200, entity.AuthorizationCodeLifetime);
            Assert.Equal(3600, entity.ConsentLifetime);
            Assert.Equal(10368000, entity.AbsoluteRefreshTokenLifetime);
            Assert.Equal(5184000, entity.SlidingRefreshTokenLifetime);
            Assert.Equal(0, entity.RefreshTokenUsage);
            Assert.False(entity.UpdateAccessTokenClaimsOnRefresh);
            Assert.Equal((int)IdentityServer4.Models.TokenExpiration.Sliding, entity.RefreshTokenExpiration);
            Assert.Equal((int)IdentityServer4.Models.AccessTokenType.Reference, entity.AccessTokenType);
        }

        [Fact]
        public void ToEntity_WithValidModel_MapsTimestampAndAdvancedProperties()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-timestamp-client",
                UserSsoLifetime = 14400,
                UserCodeType = "Alphanumeric",
                DeviceCodeLifetime = 1200
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.Equal(14400, entity.UserSsoLifetime);
            Assert.Equal("Alphanumeric", entity.UserCodeType);
            Assert.Equal(1200, entity.DeviceCodeLifetime);
        }

        [Fact]
        public void ToEntity_WithClientSecrets_MapsSecretsCollection()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-secrets-client",
                ClientSecrets = new List<IdentityServer4.Models.Secret>
                {
                    new IdentityServer4.Models.Secret
                    {
                        Description = "Model secret 1",
                        Value = "model-hash-1",
                        Expiration = new DateTime(2026, 6, 30, 23, 59, 59, DateTimeKind.Utc),
                        Type = "SharedSecret"
                    },
                    new IdentityServer4.Models.Secret
                    {
                        Description = "Model secret 2",
                        Value = "model-hash-2",
                        Expiration = null,
                        Type = "X509Thumbprint"
                    }
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.ClientSecrets);
            Assert.Equal(2, entity.ClientSecrets.Count);

            var secret1 = entity.ClientSecrets[0];
            Assert.Equal("Model secret 1", secret1.Description);
            Assert.Equal("model-hash-1", secret1.Value);
            Assert.Equal(new DateTime(2026, 6, 30, 23, 59, 59, DateTimeKind.Utc), secret1.Expiration);
            Assert.Equal("SharedSecret", secret1.Type);

            var secret2 = entity.ClientSecrets[1];
            Assert.Equal("Model secret 2", secret2.Description);
            Assert.Equal("model-hash-2", secret2.Value);
            Assert.Null(secret2.Expiration);
            Assert.Equal("X509Thumbprint", secret2.Type);
        }

        [Fact]
        public void ToEntity_WithAllowedGrantTypes_MapsGrantTypesCollection()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-grant-types-client",
                // IdentityServer4 does not allow mixing authorization_code with implicit
                // Use a valid combination instead
                AllowedGrantTypes = new List<string>
                {
                    "client_credentials",
                    "password",
                    "refresh_token"
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.AllowedGrantTypes);
            Assert.Equal(3, entity.AllowedGrantTypes.Count);
            Assert.Contains(entity.AllowedGrantTypes, g => g.GrantType == "client_credentials");
            Assert.Contains(entity.AllowedGrantTypes, g => g.GrantType == "password");
            Assert.Contains(entity.AllowedGrantTypes, g => g.GrantType == "refresh_token");
        }

        [Fact]
        public void ToEntity_WithRedirectUris_MapsRedirectUrisCollection()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-redirect-client",
                RedirectUris = new List<string>
                {
                    "https://model.example.com/callback1",
                    "https://model.example.com/callback2"
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.RedirectUris);
            Assert.Equal(2, entity.RedirectUris.Count);
            Assert.Contains(entity.RedirectUris, r => r.RedirectUri == "https://model.example.com/callback1");
            Assert.Contains(entity.RedirectUris, r => r.RedirectUri == "https://model.example.com/callback2");
        }

        [Fact]
        public void ToEntity_WithPostLogoutRedirectUris_MapsPostLogoutRedirectUrisCollection()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-logout-client",
                PostLogoutRedirectUris = new List<string>
                {
                    "https://model.example.com/",
                    "https://model.example.com/logout-complete"
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.PostLogoutRedirectUris);
            Assert.Equal(2, entity.PostLogoutRedirectUris.Count);
            Assert.Contains(entity.PostLogoutRedirectUris, p => p.PostLogoutRedirectUri == "https://model.example.com/");
            Assert.Contains(entity.PostLogoutRedirectUris, p => p.PostLogoutRedirectUri == "https://model.example.com/logout-complete");
        }

        [Fact]
        public void ToEntity_WithAllowedScopes_MapsScopesCollection()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-scopes-client",
                AllowedScopes = new List<string>
                {
                    "openid",
                    "profile",
                    "email",
                    "api1",
                    "api2"
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.AllowedScopes);
            Assert.Equal(5, entity.AllowedScopes.Count);
            Assert.Contains(entity.AllowedScopes, s => s.Scope == "openid");
            Assert.Contains(entity.AllowedScopes, s => s.Scope == "profile");
            Assert.Contains(entity.AllowedScopes, s => s.Scope == "email");
            Assert.Contains(entity.AllowedScopes, s => s.Scope == "api1");
            Assert.Contains(entity.AllowedScopes, s => s.Scope == "api2");
        }

        [Fact]
        public void ToEntity_WithClaims_MapsClaimsCollection()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-claims-client",
                Claims = new List<Claim>
                {
                    new Claim("app_name", "MyApp"),
                    new Claim("version", "2.0"),
                    new Claim("tier", "enterprise")
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.Claims);
            Assert.Equal(3, entity.Claims.Count);
            Assert.Contains(entity.Claims, c => c.Type == "app_name" && c.Value == "MyApp");
            Assert.Contains(entity.Claims, c => c.Type == "version" && c.Value == "2.0");
            Assert.Contains(entity.Claims, c => c.Type == "tier" && c.Value == "enterprise");
        }

        [Fact]
        public void ToEntity_WithIdentityProviderRestrictions_MapsRestrictionsCollection()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-idp-restricted-client",
                IdentityProviderRestrictions = new List<string>
                {
                    "Microsoft",
                    "GitHub"
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.IdentityProviderRestrictions);
            Assert.Equal(2, entity.IdentityProviderRestrictions.Count);
            Assert.Contains(entity.IdentityProviderRestrictions, i => i.Provider == "Microsoft");
            Assert.Contains(entity.IdentityProviderRestrictions, i => i.Provider == "GitHub");
        }

        [Fact]
        public void ToEntity_WithAllowedCorsOrigins_MapsCorsOriginsCollection()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-cors-client",
                AllowedCorsOrigins = new List<string>
                {
                    "https://frontend.example.com",
                    "https://api.example.com"
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.AllowedCorsOrigins);
            Assert.Equal(2, entity.AllowedCorsOrigins.Count);
            Assert.Contains(entity.AllowedCorsOrigins, c => c.Origin == "https://frontend.example.com");
            Assert.Contains(entity.AllowedCorsOrigins, c => c.Origin == "https://api.example.com");
        }

        [Fact]
        public void ToEntity_WithProperties_MapsPropertiesDictionary()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-props-client",
                Properties = new Dictionary<string, string>
                {
                    { "model_prop_1", "value1" },
                    { "model_prop_2", "value2" },
                    { "environment", "staging" }
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.Properties);
            Assert.Equal(3, entity.Properties.Count);
            Assert.Contains(entity.Properties, p => p.Key == "model_prop_1" && p.Value == "value1");
            Assert.Contains(entity.Properties, p => p.Key == "model_prop_2" && p.Value == "value2");
            Assert.Contains(entity.Properties, p => p.Key == "environment" && p.Value == "staging");
        }

        [Fact]
        public void ToEntity_WithAllCollections_MapsEverything()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-comprehensive-client",
                ClientName = "Model Comprehensive Client",
                Enabled = true,
                ClientSecrets = new List<IdentityServer4.Models.Secret>
                {
                    new IdentityServer4.Models.Secret { Value = "model-secret", Type = "SharedSecret" }
                },
                AllowedGrantTypes = new List<string> { "authorization_code" },
                RedirectUris = new List<string> { "https://example.com/callback" },
                PostLogoutRedirectUris = new List<string> { "https://example.com/logout" },
                AllowedScopes = new List<string> { "openid" },
                Claims = new List<Claim> { new Claim("claim_type", "claim_value") },
                IdentityProviderRestrictions = new List<string> { "Google" },
                AllowedCorsOrigins = new List<string> { "https://example.com" },
                Properties = new Dictionary<string, string> { { "key1", "value1" } }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Equal("model-comprehensive-client", entity.ClientId);
            Assert.Single(entity.ClientSecrets);
            Assert.Single(entity.AllowedGrantTypes);
            Assert.Single(entity.RedirectUris);
            Assert.Single(entity.PostLogoutRedirectUris);
            Assert.Single(entity.AllowedScopes);
            Assert.Single(entity.Claims);
            Assert.Single(entity.IdentityProviderRestrictions);
            Assert.Single(entity.AllowedCorsOrigins);
            Assert.Single(entity.Properties);
        }

        [Fact]
        public void ToEntity_WithNullModel_ReturnsNull()
        {
            // Act
            var entity = ((IdentityServer4.Models.Client)null).ToEntity();

            // Assert
            Assert.Null(entity);
        }

        [Fact]
        public void ToEntity_WithEmptyCollections_MapsEmptyCollections()
        {
            // Arrange
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-empty-collections-client",
                ClientSecrets = new List<IdentityServer4.Models.Secret>(),
                AllowedGrantTypes = new List<string>(),
                RedirectUris = new List<string>(),
                PostLogoutRedirectUris = new List<string>(),
                AllowedScopes = new List<string>(),
                Claims = new List<Claim>(),
                IdentityProviderRestrictions = new List<string>(),
                AllowedCorsOrigins = new List<string>(),
                Properties = new Dictionary<string, string>()
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.NotNull(entity.ClientSecrets);
            Assert.NotNull(entity.AllowedGrantTypes);
            Assert.NotNull(entity.RedirectUris);
            Assert.NotNull(entity.PostLogoutRedirectUris);
            Assert.NotNull(entity.AllowedScopes);
            Assert.NotNull(entity.Claims);
            Assert.NotNull(entity.IdentityProviderRestrictions);
            Assert.NotNull(entity.AllowedCorsOrigins);
            Assert.NotNull(entity.Properties);
            Assert.Empty(entity.ClientSecrets);
            Assert.Empty(entity.AllowedGrantTypes);
            Assert.Empty(entity.RedirectUris);
            Assert.Empty(entity.PostLogoutRedirectUris);
            Assert.Empty(entity.AllowedScopes);
            Assert.Empty(entity.Claims);
            Assert.Empty(entity.IdentityProviderRestrictions);
            Assert.Empty(entity.AllowedCorsOrigins);
            Assert.Empty(entity.Properties);
        }

        [Fact]
        public void ToEntity_WithNullCollections_HandlesGracefully()
        {
            // Arrange
            // Note: IdentityServer4.Models.Client validates AllowedGrantTypes and throws if null
            // So we test with empty collections instead, which is the realistic scenario
            var model = new IdentityServer4.Models.Client
            {
                ClientId = "model-null-collections-client",
                ClientSecrets = new List<IdentityServer4.Models.Secret>(),
                AllowedGrantTypes = new List<string>(), // Cannot be null - IS4 validates
                RedirectUris = new List<string>(),
                PostLogoutRedirectUris = new List<string>(),
                AllowedScopes = new List<string>(),
                Claims = new List<Claim>(),
                IdentityProviderRestrictions = new List<string>(),
                AllowedCorsOrigins = new List<string>(),
                Properties = new Dictionary<string, string>()
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Equal("model-null-collections-client", entity.ClientId);
            // AutoMapper behavior: empty collections map to empty entity collections
            // We're documenting current behavior here
        }

        #endregion

        #region Bidirectional Mapping Tests

        [Fact]
        public void BidirectionalMapping_EntityToModelToEntity_PreservesMainProperties()
        {
            // Arrange
            var originalEntity = new EntityClient
            {
                Id = 555,
                SiteId = "bidirectional-site",
                ClientId = "bidirectional-client",
                ClientName = "Bidirectional Client",
                Enabled = true,
                RequireClientSecret = true,
                ProtocolType = "oidc",
                ClientUri = "https://bi.example.com",
                AccessTokenLifetime = 3600,
                IdentityTokenLifetime = 300,
                Created = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            // Act
            var model = originalEntity.ToModel();
            var resultEntity = model.ToEntity();

            // Assert
            Assert.Equal(originalEntity.ClientId, resultEntity.ClientId);
            Assert.Equal(originalEntity.ClientName, resultEntity.ClientName);
            Assert.Equal(originalEntity.Enabled, resultEntity.Enabled);
            Assert.Equal(originalEntity.RequireClientSecret, resultEntity.RequireClientSecret);
            Assert.Equal(originalEntity.ProtocolType, resultEntity.ProtocolType);
            Assert.Equal(originalEntity.ClientUri, resultEntity.ClientUri);
            Assert.Equal(originalEntity.AccessTokenLifetime, resultEntity.AccessTokenLifetime);
            Assert.Equal(originalEntity.IdentityTokenLifetime, resultEntity.IdentityTokenLifetime);
            // Note: Created is entity-only property, not on IdentityServer4 model, so it's not preserved in bidirectional mapping
        }

        [Fact]
        public void BidirectionalMapping_EntityToModelToEntity_PreservesCollections()
        {
            // Arrange
            var originalEntity = new EntityClient
            {
                ClientId = "collections-roundtrip",
                ClientSecrets = new List<ClientSecret>
                {
                    new ClientSecret { Value = "secret1", Type = "SharedSecret" }
                },
                AllowedGrantTypes = new List<ClientGrantType>
                {
                    new ClientGrantType { GrantType = "authorization_code" }
                },
                RedirectUris = new List<ClientRedirectUri>
                {
                    new ClientRedirectUri { RedirectUri = "https://example.com/callback" }
                },
                AllowedScopes = new List<ClientScope>
                {
                    new ClientScope { Scope = "openid" }
                },
                Claims = new List<ClientClaim>
                {
                    new ClientClaim { Type = "claim_type", Value = "claim_value" }
                }
            };

            // Act
            var model = originalEntity.ToModel();
            var resultEntity = model.ToEntity();

            // Assert
            Assert.Single(resultEntity.ClientSecrets);
            Assert.Equal("secret1", resultEntity.ClientSecrets[0].Value);

            Assert.Single(resultEntity.AllowedGrantTypes);
            Assert.Equal("authorization_code", resultEntity.AllowedGrantTypes[0].GrantType);

            Assert.Single(resultEntity.RedirectUris);
            Assert.Equal("https://example.com/callback", resultEntity.RedirectUris[0].RedirectUri);

            Assert.Single(resultEntity.AllowedScopes);
            Assert.Equal("openid", resultEntity.AllowedScopes[0].Scope);

            Assert.Single(resultEntity.Claims);
            Assert.Equal("claim_type", resultEntity.Claims[0].Type);
            Assert.Equal("claim_value", resultEntity.Claims[0].Value);
        }

        [Fact]
        public void BidirectionalMapping_ModelToEntityToModel_PreservesMainProperties()
        {
            // Arrange
            var originalModel = new IdentityServer4.Models.Client
            {
                ClientId = "model-roundtrip",
                ClientName = "Model Roundtrip Client",
                Enabled = false,
                RequireClientSecret = false,
                ProtocolType = "oidc",
                ClientUri = "https://model-roundtrip.example.com",
                AccessTokenLifetime = 7200,
                IdentityTokenLifetime = 600
            };

            // Act
            var entity = originalModel.ToEntity();
            var resultModel = entity.ToModel();

            // Assert
            Assert.Equal(originalModel.ClientId, resultModel.ClientId);
            Assert.Equal(originalModel.ClientName, resultModel.ClientName);
            Assert.Equal(originalModel.Enabled, resultModel.Enabled);
            Assert.Equal(originalModel.RequireClientSecret, resultModel.RequireClientSecret);
            Assert.Equal(originalModel.ProtocolType, resultModel.ProtocolType);
            Assert.Equal(originalModel.ClientUri, resultModel.ClientUri);
            Assert.Equal(originalModel.AccessTokenLifetime, resultModel.AccessTokenLifetime);
            Assert.Equal(originalModel.IdentityTokenLifetime, resultModel.IdentityTokenLifetime);
        }

        [Fact]
        public void BidirectionalMapping_ModelToEntityToModel_PreservesCollections()
        {
            // Arrange
            var originalModel = new IdentityServer4.Models.Client
            {
                ClientId = "model-collections-roundtrip",
                ClientSecrets = new List<IdentityServer4.Models.Secret>
                {
                    new IdentityServer4.Models.Secret { Value = "model-secret", Type = "SharedSecret" }
                },
                AllowedGrantTypes = new List<string> { "client_credentials" },
                RedirectUris = new List<string> { "https://model.example.com/callback" },
                AllowedScopes = new List<string> { "api1" },
                Claims = new List<Claim> { new Claim("model_claim", "model_value") }
            };

            // Act
            var entity = originalModel.ToEntity();
            var resultModel = entity.ToModel();

            // Assert
            Assert.Single(resultModel.ClientSecrets);
            Assert.Equal("model-secret", resultModel.ClientSecrets.ElementAt(0).Value);

            Assert.Single(resultModel.AllowedGrantTypes);
            Assert.Contains("client_credentials", resultModel.AllowedGrantTypes);

            Assert.Single(resultModel.RedirectUris);
            Assert.Contains("https://model.example.com/callback", resultModel.RedirectUris);

            Assert.Single(resultModel.AllowedScopes);
            Assert.Contains("api1", resultModel.AllowedScopes);

            Assert.Single(resultModel.Claims);
            Assert.Contains(resultModel.Claims, c => c.Type == "model_claim" && c.Value == "model_value");
        }

        #endregion
    }
}
