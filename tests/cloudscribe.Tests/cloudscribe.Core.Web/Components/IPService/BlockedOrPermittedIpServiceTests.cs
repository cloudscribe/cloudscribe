using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;
using Moq;
using System.Net;
using Xunit;

namespace cloudscribe.Core.Web.Components.IPService.Tests
{
    public class BlockedOrPermittedIpServiceTests
    {
        private Guid _siteId = Guid.NewGuid();

        private BlockedOrPermittedIpService CreateService(
            List<BlockedPermittedIpAddressesModel>? blocked = null,
            List<BlockedPermittedIpAddressesModel>? permitted = null)
        {
            // Mock ISiteContext
            var siteSettings = new SiteSettings { Id = _siteId };
            var siteContext = new SiteContext(siteSettings);

            // Mock IipAddressCommands
            var mockIpCommands = new Mock<IipAddressCommands>();
            mockIpCommands.Setup(x => x.GetBlockedIpAddresses(_siteId, 1, -1, It.IsAny<System.Threading.CancellationToken>(), true))
                .ReturnsAsync(new PagedResult<BlockedPermittedIpAddressesModel> { Data = blocked ?? new List<BlockedPermittedIpAddressesModel>() });
            mockIpCommands.Setup(x => x.GetPermittedIpAddresses(_siteId, 1, -1, It.IsAny<System.Threading.CancellationToken>(), true))
                .ReturnsAsync(new PagedResult<BlockedPermittedIpAddressesModel> { Data = permitted ?? new List<BlockedPermittedIpAddressesModel>() });

            // Mock ILogger
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<BlockedOrPermittedIpService>>();

            // Mock IMemoryCache (no caching for tests)
            var mockCache = new Mock<Microsoft.Extensions.Caching.Memory.IMemoryCache>();
            object? cacheValue = null;
            mockCache.Setup(m => m.TryGetValue(It.IsAny<object>(), out cacheValue)).Returns(false);
            mockCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<Microsoft.Extensions.Caching.Memory.ICacheEntry>());

            return new BlockedOrPermittedIpService(siteContext, mockIpCommands.Object, mockLogger.Object, mockCache.Object);
        }

        /// <summary>
        /// In all below - returning true means you ARE blocked - jk
        /// </summary>

        [Fact]
        public void ReturnsFalse_WhenNoBlockedOrPermitted()
        {
            var service = CreateService();
            Assert.False(service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.3.4"), _siteId));
        } 

        [Fact]
        public void ReturnsTrue_WhenIpIsBlocked()
        {
            var blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false }
            };
            var service = CreateService(blocked: blocked);
            Assert.True(service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.3.4"), _siteId));
        } 

        [Fact]
        public void ReturnsFalse_WhenIpIsPermitted()
        {
            var permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false }
            };
            var service = CreateService(permitted: permitted);
            Assert.False(service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.3.4"), _siteId));
        }

        [Fact]
        public void ReturnsTrue_WhenIpIsNotInPermittedSet()
        {
            var permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.5", SiteId = _siteId, IsRange = false }
            };
            var service = CreateService(permitted: permitted);
            Assert.True(service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.3.6"), _siteId));
        }


        [Fact]
        public void ReturnsTrue_WhenIpIsInBlockedRange()
        {
            var blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0/24", SiteId = _siteId, IsRange = true }
            };
            var service = CreateService(blocked: blocked);
            Assert.True(service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.3.100"), _siteId));
        }

        [Fact]
        public void ReturnsFalse_WhenIpIsOutsideBlockedRange()
        {
            var blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0/24", SiteId = _siteId, IsRange = true }
            };
            var service = CreateService(blocked: blocked);
            Assert.False(service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.99.100"), _siteId));
        }

        [Fact]
        public void ReturnsFalse_WhenIpIsInPermittedRange()
        {
            var permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0/24", SiteId = _siteId, IsRange = true }
            };
            var service = CreateService(permitted: permitted);
            Assert.False(service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.3.100"), _siteId));
        }

        [Fact]
        public void ReturnsTree_WhenIpIsOutsidePermittedRange()
        {
            var permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0/24", SiteId = _siteId, IsRange = true }
            };
            var service = CreateService(permitted: permitted);
            Assert.True(service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.5.100"), _siteId));
        }

        [Fact]
        public void ReturnsFalse_WhenIpIsBothBlockedAndPermitted_PermittedTakesPriority()
        {
            var blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false }
            };
            var permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false }
            };
            var service = CreateService(blocked: blocked, permitted: permitted);
            Assert.False(service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.3.4"), _siteId));
        } 

        [Fact]
        public void ReturnsFalse_WhenIpIsInBothBlockedAndPermittedRanges_PermittedTakesPriority()
        {
            var blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0/24", SiteId = _siteId, IsRange = true }
            };
            var permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0/24", SiteId = _siteId, IsRange = true }
            };
            var service = CreateService(blocked: blocked, permitted: permitted);
            Assert.False(service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.3.100"), _siteId));
        }

        /// <summary>
        /// testing a mix of single IP and range: being permitted always overrides being blocked
        /// </summary>
        [Fact]
        public void ReturnsFalse_WhenUserIsWithinPermittedRangeEvenIfSpecificallyBlocked()
        {

            var blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.100", SiteId = _siteId, IsRange = false }
            };
            var permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0 - 1.2.3.100", SiteId = _siteId, IsRange = true }
            };
            var service = CreateService(blocked: blocked, permitted: permitted);
            Assert.False(service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.3.100"), _siteId));
        }

        /// <summary>
        /// test a complicated mix of types here
        /// </summary>
        [Fact]
        public void ReturnsCorrectly_WhenIpIsInMixtureOfPermittedRanges()
        {
            var permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0 - 1.2.3.50", SiteId = _siteId, IsRange = true },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.40 - 1.2.3.60", SiteId = _siteId, IsRange = true },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.8", SiteId = _siteId, IsRange = false }
            };
            var blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0 - 1.2.3.90", SiteId = _siteId, IsRange = true }
            };

            var service = CreateService(permitted: permitted);
            Assert.False(service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.3.41"), _siteId));
            Assert.True (service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.3.61"), _siteId));
            Assert.True (service.IsBlockedOrPermittedIp(IPAddress.Parse("1.2.3.99"), _siteId));
        }
    }
}
