using cloudscribe.Core.IdentityServer.EFCore.Entities;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using Xunit;

namespace cloudscribe.Core.Identity.ModelMappingTests
{
    /// <summary>
    /// Tests for ApiResource mapping between IdentityServer4 models and EF Core entities.
    /// These tests verify current AutoMapper behavior before replacing with manual mapping.
    /// 
    /// Complexity: Medium
    /// - 10 main properties
    /// - 4 child collections: Secrets (ApiSecret), Scopes (ApiScope), UserClaims (string list), Properties (dictionary)
    /// - Nested child: ApiScopeClaim within ApiScope
    /// - Bidirectional mapping (entity ↔ model)
    /// </summary>
    public class ApiResourceMappingTests
    {
        #region Entity to Model Mapping (ToModel)

        [Fact]
        public void ToModel_WithValidEntity_MapsAllProperties()
        {
            // Arrange
            var entity = new ApiResource
            {
                Id = 123,
                SiteId = "site-456",
                Enabled = true,
                Name = "test-api",
                DisplayName = "Test API",
                Description = "A test API resource"
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.Equal("test-api", model.Name);
            Assert.Equal("Test API", model.DisplayName);
            Assert.Equal("A test API resource", model.Description);
            Assert.True(model.Enabled);
        }

        [Fact]
        public void ToModel_WithSecrets_MapsSecretsCollection()
        {
            // Arrange
            var entity = new ApiResource
            {
                Name = "test-api",
                Secrets = new List<ApiSecret>
                {
                    new ApiSecret
                    {
                        Id = 1,
                        Description = "Primary secret",
                        Value = "secret-hash-1",
                        Expiration = new DateTime(2025, 12, 31, 23, 59, 59, DateTimeKind.Utc),
                        Type = "SharedSecret"
                    },
                    new ApiSecret
                    {
                        Id = 2,
                        Description = "Backup secret",
                        Value = "secret-hash-2",
                        Expiration = null,
                        Type = "SharedSecret"
                    }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.ApiSecrets);
            Assert.Equal(2, model.ApiSecrets.Count);

            var secret1 = model.ApiSecrets.ElementAt(0);
            Assert.Equal("Primary secret", secret1.Description);
            Assert.Equal("secret-hash-1", secret1.Value);
            Assert.Equal(new DateTime(2025, 12, 31, 23, 59, 59, DateTimeKind.Utc), secret1.Expiration);
            Assert.Equal("SharedSecret", secret1.Type);

            var secret2 = model.ApiSecrets.ElementAt(1);
            Assert.Equal("Backup secret", secret2.Description);
            Assert.Equal("secret-hash-2", secret2.Value);
            Assert.Null(secret2.Expiration);
            Assert.Equal("SharedSecret", secret2.Type);
        }

        [Fact]
        public void ToModel_WithScopes_MapsScopesCollectionWithNestedClaims()
        {
            // Arrange
            var entity = new ApiResource
            {
                Name = "test-api",
                Scopes = new List<ApiScope>
                {
                    new ApiScope
                    {
                        Id = 1,
                        Name = "api.read",
                        DisplayName = "Read Access",
                        Description = "Read-only access to the API",
                        Required = false,
                        Emphasize = false,
                        ShowInDiscoveryDocument = true,
                        UserClaims = new List<ApiScopeClaim>
                        {
                            new ApiScopeClaim { Id = 1, Type = "role" },
                            new ApiScopeClaim { Id = 2, Type = "email" }
                        }
                    },
                    new ApiScope
                    {
                        Id = 2,
                        Name = "api.write",
                        DisplayName = "Write Access",
                        Description = "Full access to the API",
                        Required = true,
                        Emphasize = true,
                        ShowInDiscoveryDocument = true,
                        UserClaims = new List<ApiScopeClaim>
                        {
                            new ApiScopeClaim { Id = 3, Type = "admin" }
                        }
                    }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.Scopes);
            Assert.Equal(2, model.Scopes.Count);

            var scope1 = model.Scopes.ElementAt(0);
            Assert.Equal("api.read", scope1.Name);
            Assert.Equal("Read Access", scope1.DisplayName);
            Assert.Equal("Read-only access to the API", scope1.Description);
            Assert.False(scope1.Required);
            Assert.False(scope1.Emphasize);
            Assert.True(scope1.ShowInDiscoveryDocument);
            Assert.NotNull(scope1.UserClaims);
            Assert.Equal(2, scope1.UserClaims.Count);
            Assert.Contains("role", scope1.UserClaims);
            Assert.Contains("email", scope1.UserClaims);

            var scope2 = model.Scopes.ElementAt(1);
            Assert.Equal("api.write", scope2.Name);
            Assert.Equal("Write Access", scope2.DisplayName);
            Assert.Equal("Full access to the API", scope2.Description);
            Assert.True(scope2.Required);
            Assert.True(scope2.Emphasize);
            Assert.True(scope2.ShowInDiscoveryDocument);
            Assert.NotNull(scope2.UserClaims);
            Assert.Single(scope2.UserClaims);
            Assert.Contains("admin", scope2.UserClaims);
        }

        [Fact]
        public void ToModel_WithUserClaims_MapsUserClaimsCollection()
        {
            // Arrange
            var entity = new ApiResource
            {
                Name = "test-api",
                UserClaims = new List<ApiResourceClaim>
                {
                    new ApiResourceClaim { Id = 1, Type = "name" },
                    new ApiResourceClaim { Id = 2, Type = "email" },
                    new ApiResourceClaim { Id = 3, Type = "role" }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.UserClaims);
            Assert.Equal(3, model.UserClaims.Count);
            Assert.Contains("name", model.UserClaims);
            Assert.Contains("email", model.UserClaims);
            Assert.Contains("role", model.UserClaims);
        }

        [Fact]
        public void ToModel_WithProperties_MapsPropertiesDictionary()
        {
            // Arrange
            var entity = new ApiResource
            {
                Name = "test-api",
                Properties = new List<ApiResourceProperty>
                {
                    new ApiResourceProperty { Id = 1, Key = "environment", Value = "production" },
                    new ApiResourceProperty { Id = 2, Key = "tier", Value = "premium" },
                    new ApiResourceProperty { Id = 3, Key = "region", Value = "us-west" }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.Properties);
            Assert.Equal(3, model.Properties.Count);
            Assert.Equal("production", model.Properties["environment"]);
            Assert.Equal("premium", model.Properties["tier"]);
            Assert.Equal("us-west", model.Properties["region"]);
        }

        [Fact]
        public void ToModel_WithAllChildCollections_MapsEverything()
        {
            // Arrange
            var entity = new ApiResource
            {
                Id = 999,
                Name = "comprehensive-api",
                DisplayName = "Comprehensive API",
                Enabled = true,
                Secrets = new List<ApiSecret>
                {
                    new ApiSecret { Value = "secret1", Type = "SharedSecret" }
                },
                Scopes = new List<ApiScope>
                {
                    new ApiScope 
                    { 
                        Name = "scope1", 
                        UserClaims = new List<ApiScopeClaim> 
                        { 
                            new ApiScopeClaim { Type = "scope-claim" } 
                        } 
                    }
                },
                UserClaims = new List<ApiResourceClaim>
                {
                    new ApiResourceClaim { Type = "api-claim" }
                },
                Properties = new List<ApiResourceProperty>
                {
                    new ApiResourceProperty { Key = "key1", Value = "value1" }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.Equal("comprehensive-api", model.Name);
            Assert.Single(model.ApiSecrets);
            Assert.Single(model.Scopes);
            Assert.Single(model.UserClaims);
            Assert.Single(model.Properties);
            Assert.Equal("secret1", model.ApiSecrets.ElementAt(0).Value);
            Assert.Equal("scope1", model.Scopes.ElementAt(0).Name);
            Assert.Contains("api-claim", model.UserClaims);
            Assert.Equal("value1", model.Properties["key1"]);
        }

        [Fact]
        public void ToModel_WithNullEntity_ReturnsNull()
        {
            // Act
            var model = ((ApiResource)null).ToModel();

            // Assert
            Assert.Null(model);
        }

        [Fact]
        public void ToModel_WithEmptyCollections_MapsEmptyCollections()
        {
            // Arrange
            var entity = new ApiResource
            {
                Name = "empty-api",
                Secrets = new List<ApiSecret>(),
                Scopes = new List<ApiScope>(),
                UserClaims = new List<ApiResourceClaim>(),
                Properties = new List<ApiResourceProperty>()
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.NotNull(model.ApiSecrets);
            Assert.NotNull(model.Scopes);
            Assert.NotNull(model.UserClaims);
            Assert.NotNull(model.Properties);
            Assert.Empty(model.ApiSecrets);
            Assert.Empty(model.Scopes);
            Assert.Empty(model.UserClaims);
            Assert.Empty(model.Properties);
        }

        [Fact]
        public void ToModel_WithNullCollections_HandlesGracefully()
        {
            // Arrange
            var entity = new ApiResource
            {
                Name = "null-collections-api",
                Secrets = null,
                Scopes = null,
                UserClaims = null,
                Properties = null
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.Equal("null-collections-api", model.Name);
            // AutoMapper behavior: null collections may become null or empty depending on configuration
            // We're documenting current behavior here
        }

        #endregion

        #region Model to Entity Mapping (ToEntity)

        [Fact]
        public void ToEntity_WithValidModel_MapsAllProperties()
        {
            // Arrange
            var model = new IdentityServer4.Models.ApiResource
            {
                Enabled = true,
                Name = "model-api",
                DisplayName = "Model API",
                Description = "An API from a model"
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Equal("model-api", entity.Name);
            Assert.Equal("Model API", entity.DisplayName);
            Assert.Equal("An API from a model", entity.Description);
            Assert.True(entity.Enabled);
        }

        [Fact]
        public void ToEntity_WithApiSecrets_MapsSecretsCollection()
        {
            // Arrange
            var model = new IdentityServer4.Models.ApiResource
            {
                Name = "secret-api",
                ApiSecrets = new List<IdentityServer4.Models.Secret>
                {
                    new IdentityServer4.Models.Secret
                    {
                        Description = "First secret",
                        Value = "hashed-secret-1",
                        Expiration = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        Type = "SharedSecret"
                    },
                    new IdentityServer4.Models.Secret
                    {
                        Description = "Second secret",
                        Value = "hashed-secret-2",
                        Expiration = null,
                        Type = "X509Thumbprint"
                    }
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.Secrets);
            Assert.Equal(2, entity.Secrets.Count);

            var secret1 = entity.Secrets[0];
            Assert.Equal("First secret", secret1.Description);
            Assert.Equal("hashed-secret-1", secret1.Value);
            Assert.Equal(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), secret1.Expiration);
            Assert.Equal("SharedSecret", secret1.Type);

            var secret2 = entity.Secrets[1];
            Assert.Equal("Second secret", secret2.Description);
            Assert.Equal("hashed-secret-2", secret2.Value);
            Assert.Null(secret2.Expiration);
            Assert.Equal("X509Thumbprint", secret2.Type);
        }

        [Fact]
        public void ToEntity_WithScopes_MapsScopesCollectionWithNestedClaims()
        {
            // Arrange
            var model = new IdentityServer4.Models.ApiResource
            {
                Name = "scoped-api",
                Scopes = new List<IdentityServer4.Models.Scope>
                {
                    new IdentityServer4.Models.Scope
                    {
                        Name = "read",
                        DisplayName = "Read Scope",
                        Description = "Allows reading",
                        Required = true,
                        Emphasize = true,
                        ShowInDiscoveryDocument = true,
                        UserClaims = new List<string> { "sub", "name", "email" }
                    },
                    new IdentityServer4.Models.Scope
                    {
                        Name = "write",
                        DisplayName = "Write Scope",
                        Description = "Allows writing",
                        Required = false,
                        Emphasize = false,
                        ShowInDiscoveryDocument = false,
                        UserClaims = new List<string> { "sub", "admin_role" }
                    }
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.Scopes);
            Assert.Equal(2, entity.Scopes.Count);

            var scope1 = entity.Scopes[0];
            Assert.Equal("read", scope1.Name);
            Assert.Equal("Read Scope", scope1.DisplayName);
            Assert.Equal("Allows reading", scope1.Description);
            Assert.True(scope1.Required);
            Assert.True(scope1.Emphasize);
            Assert.True(scope1.ShowInDiscoveryDocument);
            Assert.NotNull(scope1.UserClaims);
            Assert.Equal(3, scope1.UserClaims.Count);
            Assert.Contains(scope1.UserClaims, c => c.Type == "sub");
            Assert.Contains(scope1.UserClaims, c => c.Type == "name");
            Assert.Contains(scope1.UserClaims, c => c.Type == "email");

            var scope2 = entity.Scopes[1];
            Assert.Equal("write", scope2.Name);
            Assert.Equal("Write Scope", scope2.DisplayName);
            Assert.Equal("Allows writing", scope2.Description);
            Assert.False(scope2.Required);
            Assert.False(scope2.Emphasize);
            Assert.False(scope2.ShowInDiscoveryDocument);
            Assert.NotNull(scope2.UserClaims);
            Assert.Equal(2, scope2.UserClaims.Count);
            Assert.Contains(scope2.UserClaims, c => c.Type == "sub");
            Assert.Contains(scope2.UserClaims, c => c.Type == "admin_role");
        }

        [Fact]
        public void ToEntity_WithUserClaims_MapsUserClaimsCollection()
        {
            // Arrange
            var model = new IdentityServer4.Models.ApiResource
            {
                Name = "claims-api",
                UserClaims = new List<string> { "sub", "name", "email", "role", "custom_claim" }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.UserClaims);
            Assert.Equal(5, entity.UserClaims.Count);
            Assert.Contains(entity.UserClaims, c => c.Type == "sub");
            Assert.Contains(entity.UserClaims, c => c.Type == "name");
            Assert.Contains(entity.UserClaims, c => c.Type == "email");
            Assert.Contains(entity.UserClaims, c => c.Type == "role");
            Assert.Contains(entity.UserClaims, c => c.Type == "custom_claim");
        }

        [Fact]
        public void ToEntity_WithProperties_MapsPropertiesDictionary()
        {
            // Arrange
            var model = new IdentityServer4.Models.ApiResource
            {
                Name = "props-api",
                Properties = new Dictionary<string, string>
                {
                    { "custom_property_1", "value1" },
                    { "custom_property_2", "value2" },
                    { "deployment_env", "staging" }
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.Properties);
            Assert.Equal(3, entity.Properties.Count);
            Assert.Contains(entity.Properties, p => p.Key == "custom_property_1" && p.Value == "value1");
            Assert.Contains(entity.Properties, p => p.Key == "custom_property_2" && p.Value == "value2");
            Assert.Contains(entity.Properties, p => p.Key == "deployment_env" && p.Value == "staging");
        }

        [Fact]
        public void ToEntity_WithAllChildCollections_MapsEverything()
        {
            // Arrange
            var model = new IdentityServer4.Models.ApiResource
            {
                Name = "full-api",
                DisplayName = "Full API Resource",
                Enabled = true,
                ApiSecrets = new List<IdentityServer4.Models.Secret>
                {
                    new IdentityServer4.Models.Secret { Value = "secret-value", Type = "SharedSecret" }
                },
                Scopes = new List<IdentityServer4.Models.Scope>
                {
                    new IdentityServer4.Models.Scope 
                    { 
                        Name = "full.scope", 
                        UserClaims = new List<string> { "scope-claim-type" } 
                    }
                },
                UserClaims = new List<string> { "resource-claim-type" },
                Properties = new Dictionary<string, string>
                {
                    { "prop-key", "prop-value" }
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Equal("full-api", entity.Name);
            Assert.Single(entity.Secrets);
            Assert.Single(entity.Scopes);
            Assert.Single(entity.UserClaims);
            Assert.Single(entity.Properties);
            Assert.Equal("secret-value", entity.Secrets[0].Value);
            Assert.Equal("full.scope", entity.Scopes[0].Name);
            Assert.Equal("scope-claim-type", entity.Scopes[0].UserClaims[0].Type);
            Assert.Equal("resource-claim-type", entity.UserClaims[0].Type);
            Assert.Equal("prop-key", entity.Properties[0].Key);
            Assert.Equal("prop-value", entity.Properties[0].Value);
        }

        [Fact]
        public void ToEntity_WithNullModel_ReturnsNull()
        {
            // Act
            var entity = ((IdentityServer4.Models.ApiResource)null).ToEntity();

            // Assert
            Assert.Null(entity);
        }

        [Fact]
        public void ToEntity_WithEmptyCollections_MapsEmptyCollections()
        {
            // Arrange
            var model = new IdentityServer4.Models.ApiResource
            {
                Name = "empty-model-api",
                ApiSecrets = new List<IdentityServer4.Models.Secret>(),
                Scopes = new List<IdentityServer4.Models.Scope>(),
                UserClaims = new List<string>(),
                Properties = new Dictionary<string, string>()
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.NotNull(entity.Secrets);
            Assert.NotNull(entity.Scopes);
            Assert.NotNull(entity.UserClaims);
            Assert.NotNull(entity.Properties);
            Assert.Empty(entity.Secrets);
            Assert.Empty(entity.Scopes);
            Assert.Empty(entity.UserClaims);
            Assert.Empty(entity.Properties);
        }

        [Fact]
        public void ToEntity_WithNullCollections_HandlesGracefully()
        {
            // Arrange
            var model = new IdentityServer4.Models.ApiResource
            {
                Name = "null-model-api",
                ApiSecrets = null,
                Scopes = null,
                UserClaims = null,
                Properties = null
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Equal("null-model-api", entity.Name);
            // AutoMapper behavior: null collections may become null or empty depending on configuration
            // We're documenting current behavior here
        }

        #endregion

        #region Bidirectional Mapping Tests

        [Fact]
        public void BidirectionalMapping_EntityToModelToEntity_PreservesData()
        {
            // Arrange
            var originalEntity = new ApiResource
            {
                Id = 789,
                SiteId = "site-abc",
                Name = "bidirectional-api",
                DisplayName = "Bidirectional API",
                Description = "Testing roundtrip mapping",
                Enabled = true,
                Secrets = new List<ApiSecret>
                {
                    new ApiSecret { Value = "test-secret", Type = "SharedSecret" }
                },
                Scopes = new List<ApiScope>
                {
                    new ApiScope 
                    { 
                        Name = "test.scope",
                        UserClaims = new List<ApiScopeClaim> { new ApiScopeClaim { Type = "claim1" } }
                    }
                },
                UserClaims = new List<ApiResourceClaim>
                {
                    new ApiResourceClaim { Type = "claim2" }
                },
                Properties = new List<ApiResourceProperty>
                {
                    new ApiResourceProperty { Key = "key1", Value = "value1" }
                }
            };

            // Act
            var model = originalEntity.ToModel();
            var resultEntity = model.ToEntity();

            // Assert - Main properties
            Assert.Equal(originalEntity.Name, resultEntity.Name);
            Assert.Equal(originalEntity.DisplayName, resultEntity.DisplayName);
            Assert.Equal(originalEntity.Description, resultEntity.Description);
            Assert.Equal(originalEntity.Enabled, resultEntity.Enabled);

            // Assert - Collections
            Assert.Single(resultEntity.Secrets);
            Assert.Equal(originalEntity.Secrets[0].Value, resultEntity.Secrets[0].Value);

            Assert.Single(resultEntity.Scopes);
            Assert.Equal(originalEntity.Scopes[0].Name, resultEntity.Scopes[0].Name);
            Assert.Single(resultEntity.Scopes[0].UserClaims);
            Assert.Equal("claim1", resultEntity.Scopes[0].UserClaims[0].Type);

            Assert.Single(resultEntity.UserClaims);
            Assert.Equal("claim2", resultEntity.UserClaims[0].Type);

            Assert.Single(resultEntity.Properties);
            Assert.Equal("key1", resultEntity.Properties[0].Key);
            Assert.Equal("value1", resultEntity.Properties[0].Value);
        }

        [Fact]
        public void BidirectionalMapping_ModelToEntityToModel_PreservesData()
        {
            // Arrange
            var originalModel = new IdentityServer4.Models.ApiResource
            {
                Name = "roundtrip-api",
                DisplayName = "Roundtrip API",
                Description = "Testing model-first roundtrip",
                Enabled = false,
                ApiSecrets = new List<IdentityServer4.Models.Secret>
                {
                    new IdentityServer4.Models.Secret { Value = "model-secret", Type = "SharedSecret" }
                },
                Scopes = new List<IdentityServer4.Models.Scope>
                {
                    new IdentityServer4.Models.Scope 
                    { 
                        Name = "model.scope",
                        UserClaims = new List<string> { "model-claim" }
                    }
                },
                UserClaims = new List<string> { "user-claim" },
                Properties = new Dictionary<string, string>
                {
                    { "model-key", "model-value" }
                }
            };

            // Act
            var entity = originalModel.ToEntity();
            var resultModel = entity.ToModel();

            // Assert - Main properties
            Assert.Equal(originalModel.Name, resultModel.Name);
            Assert.Equal(originalModel.DisplayName, resultModel.DisplayName);
            Assert.Equal(originalModel.Description, resultModel.Description);
            Assert.Equal(originalModel.Enabled, resultModel.Enabled);

            // Assert - Collections
            Assert.Single(resultModel.ApiSecrets);
            Assert.Equal(originalModel.ApiSecrets.ElementAt(0).Value, resultModel.ApiSecrets.ElementAt(0).Value);

            Assert.Single(resultModel.Scopes);
            Assert.Equal(originalModel.Scopes.ElementAt(0).Name, resultModel.Scopes.ElementAt(0).Name);
            Assert.Single(resultModel.Scopes.ElementAt(0).UserClaims);
            Assert.Contains("model-claim", resultModel.Scopes.ElementAt(0).UserClaims);

            Assert.Single(resultModel.UserClaims);
            Assert.Contains("user-claim", resultModel.UserClaims);

            Assert.Single(resultModel.Properties);
            Assert.Equal("model-value", resultModel.Properties["model-key"]);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void ToModel_WithScopeContainingEmptyUserClaims_MapsCorrectly()
        {
            // Arrange
            var entity = new ApiResource
            {
                Name = "scope-empty-claims-api",
                Scopes = new List<ApiScope>
                {
                    new ApiScope
                    {
                        Name = "empty-scope",
                        UserClaims = new List<ApiScopeClaim>()
                    }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model.Scopes);
            Assert.Single(model.Scopes);
            Assert.NotNull(model.Scopes.ElementAt(0).UserClaims);
            Assert.Empty(model.Scopes.ElementAt(0).UserClaims);
        }

        [Fact]
        public void ToEntity_WithScopeContainingEmptyUserClaims_MapsCorrectly()
        {
            // Arrange
            var model = new IdentityServer4.Models.ApiResource
            {
                Name = "scope-empty-claims-model",
                Scopes = new List<IdentityServer4.Models.Scope>
                {
                    new IdentityServer4.Models.Scope
                    {
                        Name = "empty-scope",
                        UserClaims = new List<string>()
                    }
                }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity.Scopes);
            Assert.Single(entity.Scopes);
            Assert.NotNull(entity.Scopes[0].UserClaims);
            Assert.Empty(entity.Scopes[0].UserClaims);
        }

        [Fact]
        public void ToModel_WithMultipleScopesHavingDifferentClaimCounts_MapsCorrectly()
        {
            // Arrange
            var entity = new ApiResource
            {
                Name = "multi-scope-api",
                Scopes = new List<ApiScope>
                {
                    new ApiScope 
                    { 
                        Name = "scope-no-claims",
                        UserClaims = new List<ApiScopeClaim>()
                    },
                    new ApiScope 
                    { 
                        Name = "scope-one-claim",
                        UserClaims = new List<ApiScopeClaim> { new ApiScopeClaim { Type = "claim1" } }
                    },
                    new ApiScope 
                    { 
                        Name = "scope-many-claims",
                        UserClaims = new List<ApiScopeClaim> 
                        { 
                            new ApiScopeClaim { Type = "claim2" },
                            new ApiScopeClaim { Type = "claim3" },
                            new ApiScopeClaim { Type = "claim4" }
                        }
                    }
                }
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.Equal(3, model.Scopes.Count);
            Assert.Empty(model.Scopes.ElementAt(0).UserClaims);
            Assert.Single(model.Scopes.ElementAt(1).UserClaims);
            Assert.Equal(3, model.Scopes.ElementAt(2).UserClaims.Count);
            Assert.Contains("claim1", model.Scopes.ElementAt(1).UserClaims);
            Assert.Contains("claim2", model.Scopes.ElementAt(2).UserClaims);
            Assert.Contains("claim3", model.Scopes.ElementAt(2).UserClaims);
            Assert.Contains("claim4", model.Scopes.ElementAt(2).UserClaims);
        }

        #endregion
    }
}
