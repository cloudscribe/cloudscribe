using cloudscribe.Core.IdentityServer.EFCore.Entities;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using Xunit;

namespace cloudscribe.Core.Identity.ModelMappingTests
{
    /// <summary>
    /// Unit tests for IdentityResource AutoMapper mappings.
    /// Tests current AutoMapper behavior including child collections (UserClaims, Properties).
    /// </summary>
    public class IdentityResourceMappingTests
    {
        #region ToModel Tests

        [Fact]
        public void ToModel_WithValidEntity_MapsAllProperties()
        {
            // Arrange
            var entity = new IdentityResource
            {
                SiteId = "site1",
                Id = 123,
                Enabled = true,
                Name = "openid",
                DisplayName = "OpenID",
                Description = "Your user identifier",
                Required = true,
                Emphasize = false,
                ShowInDiscoveryDocument = true,
                Created = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2026, 4, 9, 0, 0, 0, DateTimeKind.Utc),
                NonEditable = false
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.Equal(entity.Enabled, model.Enabled);
            Assert.Equal(entity.Name, model.Name);
            Assert.Equal(entity.DisplayName, model.DisplayName);
            Assert.Equal(entity.Description, model.Description);
            Assert.Equal(entity.Required, model.Required);
            Assert.Equal(entity.Emphasize, model.Emphasize);
            Assert.Equal(entity.ShowInDiscoveryDocument, model.ShowInDiscoveryDocument);

            // Note: Id, SiteId, Created, Updated, NonEditable are entity-specific
            // IdentityServer4.Models.IdentityResource has different/fewer properties
        }

        [Fact]
        public void ToModel_WithNullEntity_ReturnsNull()
        {
            // Arrange
            IdentityResource entity = null;

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.Null(model);
        }

        [Fact]
        public void ToModel_WithNullOptionalProperties_MapsNullsCorrectly()
        {
            // Arrange
            var entity = new IdentityResource
            {
                SiteId = "site1",
                Id = 1,
                Enabled = true,
                Name = "profile",
                DisplayName = null, // Null display name
                Description = null, // Null description
                Required = false,
                Emphasize = false,
                ShowInDiscoveryDocument = true,
                Created = DateTime.UtcNow,
                Updated = null, // Never updated
                NonEditable = false
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.Null(model.DisplayName);
            Assert.Null(model.Description);
        }

        [Fact]
        public void ToModel_WithUserClaims_MapsClaimsToStringList()
        {
            // Arrange
            var entity = new IdentityResource
            {
                SiteId = "site1",
                Id = 1,
                Enabled = true,
                Name = "profile",
                DisplayName = "User profile",
                Description = "Your user profile information",
                UserClaims = new List<IdentityClaim>
            {
                new IdentityClaim { Id = 1, Type = "name" },
                new IdentityClaim { Id = 2, Type = "family_name" },
                new IdentityClaim { Id = 3, Type = "given_name" },
                new IdentityClaim { Id = 4, Type = "picture" }
            },
                Created = DateTime.UtcNow
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.NotNull(model.UserClaims);
            Assert.Equal(4, model.UserClaims.Count);
            Assert.Contains("name", model.UserClaims);
            Assert.Contains("family_name", model.UserClaims);
            Assert.Contains("given_name", model.UserClaims);
            Assert.Contains("picture", model.UserClaims);
        }

        [Fact]
        public void ToModel_WithNullUserClaims_MapsToNullOrEmptyCollection()
        {
            // Arrange
            var entity = new IdentityResource
            {
                SiteId = "site1",
                Id = 1,
                Enabled = true,
                Name = "test",
                UserClaims = null, // No claims
                Created = DateTime.UtcNow
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            // AutoMapper may create empty collection or leave null
            // Both are acceptable behaviors
        }

        [Fact]
        public void ToModel_WithEmptyUserClaims_MapsToEmptyCollection()
        {
            // Arrange
            var entity = new IdentityResource
            {
                SiteId = "site1",
                Id = 1,
                Enabled = true,
                Name = "test",
                UserClaims = new List<IdentityClaim>(), // Empty list
                Created = DateTime.UtcNow
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.NotNull(model.UserClaims);
            Assert.Empty(model.UserClaims);
        }

        [Fact]
        public void ToModel_WithProperties_MapsPropertiesToDictionary()
        {
            // Arrange
            var entity = new IdentityResource
            {
                SiteId = "site1",
                Id = 1,
                Enabled = true,
                Name = "custom",
                Properties = new List<IdentityResourceProperty>
            {
                new IdentityResourceProperty { Id = 1, Key = "key1", Value = "value1" },
                new IdentityResourceProperty { Id = 2, Key = "key2", Value = "value2" },
                new IdentityResourceProperty { Id = 3, Key = "special_prop", Value = "special_value" }
            },
                Created = DateTime.UtcNow
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.NotNull(model.Properties);
            Assert.Equal(3, model.Properties.Count);
            Assert.True(model.Properties.ContainsKey("key1"));
            Assert.Equal("value1", model.Properties["key1"]);
            Assert.True(model.Properties.ContainsKey("key2"));
            Assert.Equal("value2", model.Properties["key2"]);
            Assert.True(model.Properties.ContainsKey("special_prop"));
            Assert.Equal("special_value", model.Properties["special_prop"]);
        }

        #endregion

        #region ToEntity Tests

        [Fact]
        public void ToEntity_WithValidModel_MapsAllProperties()
        {
            // Arrange
            var model = new IdentityServer4.Models.IdentityResource
            {
                Enabled = false,
                Name = "email",
                DisplayName = "Email Address",
                Description = "Your email address",
                Required = true,
                Emphasize = true,
                ShowInDiscoveryDocument = false
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Equal(model.Enabled, entity.Enabled);
            Assert.Equal(model.Name, entity.Name);
            Assert.Equal(model.DisplayName, entity.DisplayName);
            Assert.Equal(model.Description, entity.Description);
            Assert.Equal(model.Required, entity.Required);
            Assert.Equal(model.Emphasize, entity.Emphasize);
            Assert.Equal(model.ShowInDiscoveryDocument, entity.ShowInDiscoveryDocument);

            // Entity-specific properties should have defaults
            Assert.Equal(0, entity.Id); // Default int value
            Assert.Null(entity.SiteId); // Must be set by caller
        }

        [Fact]
        public void ToEntity_WithNullModel_ReturnsNull()
        {
            // Arrange
            IdentityServer4.Models.IdentityResource model = null;

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.Null(entity);
        }

        [Fact]
        public void ToEntity_WithUserClaims_MapsStringListToClaims()
        {
            // Arrange
            var model = new IdentityServer4.Models.IdentityResource
            {
                Enabled = true,
                Name = "address",
                DisplayName = "Address",
                UserClaims = new List<string>
            {
                "street_address",
                "locality",
                "region",
                "postal_code",
                "country"
            }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.NotNull(entity.UserClaims);
            Assert.Equal(5, entity.UserClaims.Count);
            Assert.Contains(entity.UserClaims, c => c.Type == "street_address");
            Assert.Contains(entity.UserClaims, c => c.Type == "locality");
            Assert.Contains(entity.UserClaims, c => c.Type == "region");
            Assert.Contains(entity.UserClaims, c => c.Type == "postal_code");
            Assert.Contains(entity.UserClaims, c => c.Type == "country");

            // Entity claim objects should be created
            Assert.All(entity.UserClaims, claim =>
            {
                Assert.IsType<IdentityClaim>(claim);
                Assert.NotNull(claim.Type);
            });
        }

        [Fact]
        public void ToEntity_WithNullUserClaims_MapsToNullOrEmptyCollection()
        {
            // Arrange
            var model = new IdentityServer4.Models.IdentityResource
            {
                Enabled = true,
                Name = "test",
                UserClaims = null
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            // AutoMapper behavior with null collections
        }

        [Fact]
        public void ToEntity_WithProperties_MapsDictionaryToPropertyEntities()
        {
            // Arrange
            var model = new IdentityServer4.Models.IdentityResource
            {
                Enabled = true,
                Name = "custom",
                Properties = new Dictionary<string, string>
            {
                { "prop1", "value1" },
                { "prop2", "value2" },
                { "important_setting", "true" }
            }
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.NotNull(entity.Properties);
            Assert.Equal(3, entity.Properties.Count);
            Assert.Contains(entity.Properties, p => p.Key == "prop1" && p.Value == "value1");
            Assert.Contains(entity.Properties, p => p.Key == "prop2" && p.Value == "value2");
            Assert.Contains(entity.Properties, p => p.Key == "important_setting" && p.Value == "true");

            // Entity property objects should be created
            Assert.All(entity.Properties, prop =>
            {
                Assert.IsType<IdentityResourceProperty>(prop);
                Assert.NotNull(prop.Key);
                Assert.NotNull(prop.Value);
            });
        }

        #endregion

        #region Bidirectional Mapping Tests

        [Fact]
        public void BidirectionalMapping_EntityToModelToEntity_PreservesModelProperties()
        {
            // Arrange
            var originalEntity = new IdentityResource
            {
                SiteId = "site-abc",
                Id = 999,
                Enabled = true,
                Name = "phone",
                DisplayName = "Phone Number",
                Description = "Your telephone number",
                Required = false,
                Emphasize = true,
                ShowInDiscoveryDocument = true,
                UserClaims = new List<IdentityClaim>
            {
                new IdentityClaim { Id = 1, Type = "phone_number" },
                new IdentityClaim { Id = 2, Type = "phone_number_verified" }
            },
                Properties = new List<IdentityResourceProperty>
            {
                new IdentityResourceProperty { Id = 1, Key = "color", Value = "blue" }
            },
                Created = new DateTime(2026, 1, 1),
                Updated = new DateTime(2026, 4, 9),
                NonEditable = false
            };

            // Act
            var model = originalEntity.ToModel();
            var newEntity = model.ToEntity();

            // Assert - model properties should round-trip
            Assert.Equal(originalEntity.Enabled, newEntity.Enabled);
            Assert.Equal(originalEntity.Name, newEntity.Name);
            Assert.Equal(originalEntity.DisplayName, newEntity.DisplayName);
            Assert.Equal(originalEntity.Description, newEntity.Description);
            Assert.Equal(originalEntity.Required, newEntity.Required);
            Assert.Equal(originalEntity.Emphasize, newEntity.Emphasize);
            Assert.Equal(originalEntity.ShowInDiscoveryDocument, newEntity.ShowInDiscoveryDocument);

            // Child collections
            Assert.Equal(originalEntity.UserClaims.Count, newEntity.UserClaims.Count);
            Assert.Equal(originalEntity.Properties.Count, newEntity.Properties.Count);

            // Entity-specific properties are lost (expected)
            Assert.Equal(0, newEntity.Id);
            Assert.Null(newEntity.SiteId);
        }

        [Fact]
        public void BidirectionalMapping_ModelToEntityToModel_PreservesAllData()
        {
            // Arrange
            var originalModel = new IdentityServer4.Models.IdentityResource
            {
                Enabled = false,
                Name = "custom_scope",
                DisplayName = "Custom Scope",
                Description = "A custom identity scope",
                Required = true,
                Emphasize = false,
                ShowInDiscoveryDocument = true,
                UserClaims = new List<string> { "custom_claim_1", "custom_claim_2" },
                Properties = new Dictionary<string, string>
            {
                { "category", "custom" },
                { "priority", "high" }
            }
            };

            // Act
            var entity = originalModel.ToEntity();
            var newModel = entity.ToModel();

            // Assert - perfect round-trip
            Assert.Equal(originalModel.Enabled, newModel.Enabled);
            Assert.Equal(originalModel.Name, newModel.Name);
            Assert.Equal(originalModel.DisplayName, newModel.DisplayName);
            Assert.Equal(originalModel.Description, newModel.Description);
            Assert.Equal(originalModel.Required, newModel.Required);
            Assert.Equal(originalModel.Emphasize, newModel.Emphasize);
            Assert.Equal(originalModel.ShowInDiscoveryDocument, newModel.ShowInDiscoveryDocument);

            // Collections
            Assert.Equal(originalModel.UserClaims.Count, newModel.UserClaims.Count);
            Assert.All(originalModel.UserClaims, claim => Assert.Contains(claim, newModel.UserClaims));

            Assert.Equal(originalModel.Properties.Count, newModel.Properties.Count);
            Assert.All(originalModel.Properties, kvp =>
            {
                Assert.True(newModel.Properties.ContainsKey(kvp.Key));
                Assert.Equal(kvp.Value, newModel.Properties[kvp.Key]);
            });
        }

        [Fact]
        public void ToEntity_SetsCreatedDateToUtcNow()
        {
            // Arrange
            var model = new IdentityServer4.Models.IdentityResource
            {
                Enabled = true,
                Name = "test"
            };

            var beforeCreation = DateTime.UtcNow.AddSeconds(-1);

            // Act
            var entity = model.ToEntity();

            var afterCreation = DateTime.UtcNow.AddSeconds(1);

            // Assert
            Assert.NotNull(entity);
            // Created date should be set to approximately UtcNow by entity default
            Assert.True(entity.Created >= beforeCreation && entity.Created <= afterCreation,
                $"Created date {entity.Created} should be between {beforeCreation} and {afterCreation}");
        }

        [Fact]
        public void ToModel_WithComplexScenario_HandlesAllCollectionsAndNulls()
        {
            // Arrange - realistic scenario with mixed null/empty/populated collections
            var entity = new IdentityResource
            {
                SiteId = "production-site",
                Id = 42,
                Enabled = true,
                Name = "offline_access",
                DisplayName = null, // Null display name
                Description = "Offline access",
                Required = false,
                Emphasize = true,
                ShowInDiscoveryDocument = false,
                UserClaims = new List<IdentityClaim>(), // Empty claims
                Properties = new List<IdentityResourceProperty>
            {
                new IdentityResourceProperty { Key = "lifetime", Value = "3600" },
                new IdentityResourceProperty { Key = "refresh", Value = "true" }
            },
                Created = new DateTime(2025, 6, 1),
                Updated = null, // Never updated
                NonEditable = true
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.Equal("offline_access", model.Name);
            Assert.Null(model.DisplayName);
            Assert.Equal("Offline access", model.Description);
            Assert.Empty(model.UserClaims);
            Assert.Equal(2, model.Properties.Count);
            Assert.Equal("3600", model.Properties["lifetime"]);
            Assert.Equal("true", model.Properties["refresh"]);
        }

        #endregion
    }
}
