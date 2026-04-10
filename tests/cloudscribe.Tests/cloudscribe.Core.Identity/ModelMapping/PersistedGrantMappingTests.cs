using cloudscribe.Core.IdentityServer.EFCore.Entities;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using Xunit;

namespace cloudscribe.Core.Identity.ModelMappingTests
{
    /// <summary>
    /// Unit tests for PersistedGrant AutoMapper mappings.
    /// Tests current AutoMapper behavior to ensure no regressions when migrating to manual mapping.
    /// </summary>
    public class PersistedGrantMappingTests
    {
        #region ToModel Tests

        [Fact]
        public void ToModel_WithValidEntity_MapsAllProperties()
        {
            // Arrange
            var entity = new PersistedGrant
            {
                SiteId = "site1",
                Key = "test-key-123",
                Type = "authorization_code",
                SubjectId = "user-456",
                ClientId = "client-789",
                CreationTime = new DateTime(2026, 4, 9, 10, 30, 0, DateTimeKind.Utc),
                Expiration = new DateTime(2026, 4, 9, 11, 30, 0, DateTimeKind.Utc),
                Data = "encrypted-grant-data"
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.Equal(entity.Key, model.Key);
            Assert.Equal(entity.Type, model.Type);
            Assert.Equal(entity.SubjectId, model.SubjectId);
            Assert.Equal(entity.ClientId, model.ClientId);
            Assert.Equal(entity.CreationTime, model.CreationTime);
            Assert.Equal(entity.Expiration, model.Expiration);
            Assert.Equal(entity.Data, model.Data);

            // Note: SiteId is entity-specific and should NOT be in model
            // IdentityServer4.Models.PersistedGrant does not have SiteId property
        }

        [Fact]
        public void ToModel_WithNullEntity_ReturnsNull()
        {
            // Arrange
            PersistedGrant entity = null;

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.Null(model);
        }

        [Fact]
        public void ToModel_WithNullExpiration_MapsNullCorrectly()
        {
            // Arrange
            var entity = new PersistedGrant
            {
                SiteId = "site1",
                Key = "test-key",
                Type = "refresh_token",
                SubjectId = "user1",
                ClientId = "client1",
                CreationTime = DateTime.UtcNow,
                Expiration = null, // No expiration
                Data = "data"
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.Null(model.Expiration);
        }

        [Fact]
        public void ToModel_WithEmptyStrings_MapsEmptyStringsCorrectly()
        {
            // Arrange
            var entity = new PersistedGrant
            {
                SiteId = string.Empty,
                Key = string.Empty,
                Type = string.Empty,
                SubjectId = string.Empty,
                ClientId = string.Empty,
                CreationTime = DateTime.UtcNow,
                Expiration = null,
                Data = string.Empty
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.Equal(string.Empty, model.Key);
            Assert.Equal(string.Empty, model.Type);
            Assert.Equal(string.Empty, model.SubjectId);
            Assert.Equal(string.Empty, model.ClientId);
            Assert.Equal(string.Empty, model.Data);
        }

        [Fact]
        public void ToModel_WithMinDateTime_MapsCorrectly()
        {
            // Arrange
            var entity = new PersistedGrant
            {
                SiteId = "site1",
                Key = "test-key",
                Type = "code",
                SubjectId = "user1",
                ClientId = "client1",
                CreationTime = DateTime.MinValue,
                Expiration = DateTime.MinValue,
                Data = "data"
            };

            // Act
            var model = entity.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.Equal(DateTime.MinValue, model.CreationTime);
            Assert.Equal(DateTime.MinValue, model.Expiration);
        }

        #endregion

        #region ToEntity Tests

        [Fact]
        public void ToEntity_WithValidModel_MapsAllProperties()
        {
            // Arrange
            var model = new IdentityServer4.Models.PersistedGrant
            {
                Key = "model-key-123",
                Type = "reference_token",
                SubjectId = "model-user-456",
                ClientId = "model-client-789",
                CreationTime = new DateTime(2026, 4, 9, 12, 0, 0, DateTimeKind.Utc),
                Expiration = new DateTime(2026, 4, 9, 13, 0, 0, DateTimeKind.Utc),
                Data = "model-encrypted-data"
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Equal(model.Key, entity.Key);
            Assert.Equal(model.Type, entity.Type);
            Assert.Equal(model.SubjectId, entity.SubjectId);
            Assert.Equal(model.ClientId, entity.ClientId);
            Assert.Equal(model.CreationTime, entity.CreationTime);
            Assert.Equal(model.Expiration, entity.Expiration);
            Assert.Equal(model.Data, entity.Data);

            // SiteId should be null/default when mapping from model
            // Caller must set SiteId explicitly
            Assert.Null(entity.SiteId);
        }

        [Fact]
        public void ToEntity_WithNullModel_ReturnsNull()
        {
            // Arrange
            IdentityServer4.Models.PersistedGrant model = null;

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.Null(entity);
        }

        [Fact]
        public void ToEntity_WithNullExpiration_MapsNullCorrectly()
        {
            // Arrange
            var model = new IdentityServer4.Models.PersistedGrant
            {
                Key = "key1",
                Type = "type1",
                SubjectId = "sub1",
                ClientId = "client1",
                CreationTime = DateTime.UtcNow,
                Expiration = null,
                Data = "data"
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Null(entity.Expiration);
        }

        #endregion

        #region UpdateEntity Tests

        [Fact]
        public void UpdateEntity_WithValidModel_UpdatesAllProperties()
        {
            // Arrange
            var existingEntity = new PersistedGrant
            {
                SiteId = "original-site",
                Key = "original-key",
                Type = "original-type",
                SubjectId = "original-subject",
                ClientId = "original-client",
                CreationTime = DateTime.UtcNow.AddHours(-2),
                Expiration = DateTime.UtcNow.AddHours(-1),
                Data = "original-data"
            };

            var updateModel = new IdentityServer4.Models.PersistedGrant
            {
                Key = "updated-key",
                Type = "updated-type",
                SubjectId = "updated-subject",
                ClientId = "updated-client",
                CreationTime = DateTime.UtcNow.AddHours(-1),
                Expiration = DateTime.UtcNow.AddHours(1),
                Data = "updated-data"
            };

            // Act
            updateModel.UpdateEntity(existingEntity);

            // Assert - all properties should be updated
            Assert.Equal("updated-key", existingEntity.Key);
            Assert.Equal("updated-type", existingEntity.Type);
            Assert.Equal("updated-subject", existingEntity.SubjectId);
            Assert.Equal("updated-client", existingEntity.ClientId);
            Assert.Equal(updateModel.CreationTime, existingEntity.CreationTime);
            Assert.Equal(updateModel.Expiration, existingEntity.Expiration);
            Assert.Equal("updated-data", existingEntity.Data);

            // SiteId should be preserved (not updated by model)
            Assert.Equal("original-site", existingEntity.SiteId);
        }

        [Fact]
        public void UpdateEntity_WithNullExpiration_UpdatesToNull()
        {
            // Arrange
            var existingEntity = new PersistedGrant
            {
                SiteId = "site1",
                Key = "key1",
                Type = "type1",
                SubjectId = "sub1",
                ClientId = "client1",
                CreationTime = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddHours(1), // Has expiration
                Data = "data"
            };

            var updateModel = new IdentityServer4.Models.PersistedGrant
            {
                Key = "key1",
                Type = "type1",
                SubjectId = "sub1",
                ClientId = "client1",
                CreationTime = DateTime.UtcNow,
                Expiration = null, // Removing expiration
                Data = "data"
            };

            // Act
            updateModel.UpdateEntity(existingEntity);

            // Assert
            Assert.Null(existingEntity.Expiration);
        }

        [Fact]
        public void UpdateEntity_PreservesSiteId()
        {
            // Arrange
            var existingEntity = new PersistedGrant
            {
                SiteId = "important-site-id",
                Key = "key1",
                Type = "type1",
                SubjectId = "sub1",
                ClientId = "client1",
                CreationTime = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddHours(1),
                Data = "data"
            };

            var updateModel = new IdentityServer4.Models.PersistedGrant
            {
                Key = "new-key",
                Type = "new-type",
                SubjectId = "new-sub",
                ClientId = "new-client",
                CreationTime = DateTime.UtcNow.AddHours(-1),
                Expiration = DateTime.UtcNow.AddHours(2),
                Data = "new-data"
            };

            // Act
            updateModel.UpdateEntity(existingEntity);

            // Assert - SiteId must be preserved for multi-tenancy
            Assert.Equal("important-site-id", existingEntity.SiteId);
        }

        #endregion

        #region Bidirectional Mapping Tests

        [Fact]
        public void BidirectionalMapping_EntityToModelToEntity_PreservesData()
        {
            // Arrange
            var originalEntity = new PersistedGrant
            {
                SiteId = "site-123",
                Key = "bidirectional-key",
                Type = "authorization_code",
                SubjectId = "user-abc",
                ClientId = "client-xyz",
                CreationTime = new DateTime(2026, 4, 9, 14, 0, 0, DateTimeKind.Utc),
                Expiration = new DateTime(2026, 4, 9, 15, 0, 0, DateTimeKind.Utc),
                Data = "bidirectional-test-data"
            };

            // Act
            var model = originalEntity.ToModel();
            var newEntity = model.ToEntity();

            // Assert - all model properties should round-trip
            Assert.Equal(originalEntity.Key, newEntity.Key);
            Assert.Equal(originalEntity.Type, newEntity.Type);
            Assert.Equal(originalEntity.SubjectId, newEntity.SubjectId);
            Assert.Equal(originalEntity.ClientId, newEntity.ClientId);
            Assert.Equal(originalEntity.CreationTime, newEntity.CreationTime);
            Assert.Equal(originalEntity.Expiration, newEntity.Expiration);
            Assert.Equal(originalEntity.Data, newEntity.Data);

            // Note: SiteId is lost in round-trip (expected behavior)
            Assert.Null(newEntity.SiteId);
        }

        [Fact]
        public void BidirectionalMapping_ModelToEntityToModel_PreservesData()
        {
            // Arrange
            var originalModel = new IdentityServer4.Models.PersistedGrant
            {
                Key = "model-roundtrip-key",
                Type = "refresh_token",
                SubjectId = "model-user",
                ClientId = "model-client",
                CreationTime = new DateTime(2026, 4, 9, 16, 0, 0, DateTimeKind.Utc),
                Expiration = new DateTime(2026, 4, 9, 17, 0, 0, DateTimeKind.Utc),
                Data = "model-roundtrip-data"
            };

            // Act
            var entity = originalModel.ToEntity();
            var newModel = entity.ToModel();

            // Assert - all properties should round-trip perfectly
            Assert.Equal(originalModel.Key, newModel.Key);
            Assert.Equal(originalModel.Type, newModel.Type);
            Assert.Equal(originalModel.SubjectId, newModel.SubjectId);
            Assert.Equal(originalModel.ClientId, newModel.ClientId);
            Assert.Equal(originalModel.CreationTime, newModel.CreationTime);
            Assert.Equal(originalModel.Expiration, newModel.Expiration);
            Assert.Equal(originalModel.Data, newModel.Data);
        }

        #endregion
    }
}