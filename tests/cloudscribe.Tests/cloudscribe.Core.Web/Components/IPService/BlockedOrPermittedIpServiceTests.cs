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
            // Mock IIpAddressCache instead of the old dependencies
            var mockCache = new Mock<IIpAddressCache>();
            
            mockCache.Setup(x => x.GetBlockedIpAddressesAsync(It.IsAny<Guid>(), It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(blocked ?? new List<BlockedPermittedIpAddressesModel>());
            
            mockCache.Setup(x => x.GetPermittedIpAddressesAsync(It.IsAny<Guid>(), It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(permitted ?? new List<BlockedPermittedIpAddressesModel>());

            // Mock IipAddressCommands for CRUD operations
            var mockIpCommands = new Mock<IipAddressCommands>();

            // Mock ILogger
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<BlockedOrPermittedIpService>>();

            return new BlockedOrPermittedIpService(mockCache.Object, mockIpCommands.Object, mockLogger.Object);
        }

        /// <summary>
        /// In all below - returning true means you ARE blocked - jk
        /// </summary>

        [Fact]
        public async Task ReturnsFalse_WhenNoBlockedOrPermitted()
        {
            BlockedOrPermittedIpService service = CreateService();
            Assert.False(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.3.4"), _siteId));
        } 

        [Fact]
        public async Task ReturnsTrue_WhenIpIsBlocked()
        {
            List<BlockedPermittedIpAddressesModel> blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false }
            };
            BlockedOrPermittedIpService service = CreateService(blocked: blocked);
            Assert.True(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.3.4"), _siteId));
        }

        [Fact]
        public async Task ReturnsFalse_WhenIpIsPermitted()
        {
            List<BlockedPermittedIpAddressesModel> permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false }
            };
            BlockedOrPermittedIpService service = CreateService(permitted: permitted);
            Assert.False(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.3.4"), _siteId));
        }

        [Fact]
        public async Task ReturnsTrue_WhenIpIsNotInPermittedSet()
        {
            List<BlockedPermittedIpAddressesModel> permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.5", SiteId = _siteId, IsRange = false }
            };
            BlockedOrPermittedIpService service = CreateService(permitted: permitted);
            Assert.True(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.3.6"), _siteId));
        }


        [Fact]
        public async Task ReturnsTrue_WhenIpIsInBlockedRange()
        {
            List<BlockedPermittedIpAddressesModel> blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0/24", SiteId = _siteId, IsRange = true }
            };
            BlockedOrPermittedIpService service = CreateService(blocked: blocked);
            Assert.True(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.3.100"), _siteId));
        }

        [Fact]
        public async Task ReturnsFalse_WhenIpIsOutsideBlockedRange()
        {
            List<BlockedPermittedIpAddressesModel> blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0/24", SiteId = _siteId, IsRange = true }
            };
            BlockedOrPermittedIpService service = CreateService(blocked: blocked);
            Assert.False(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.99.100"), _siteId));
        }

        [Fact]
        public async Task ReturnsFalse_WhenIpIsInPermittedRange()
        {
            List<BlockedPermittedIpAddressesModel> permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0/24", SiteId = _siteId, IsRange = true }
            };
            BlockedOrPermittedIpService service = CreateService(permitted: permitted);
            Assert.False(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.3.100"), _siteId));
        }

        [Fact]
        public async Task ReturnsTrue_WhenIpIsOutsidePermittedRange()
        {
            List<BlockedPermittedIpAddressesModel> permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0/24", SiteId = _siteId, IsRange = true }
            };
            BlockedOrPermittedIpService service = CreateService(permitted: permitted);
            Assert.True(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.5.100"), _siteId));
        }

        [Fact]
        public async Task ReturnsFalse_WhenIpIsBothBlockedAndPermitted_PermittedTakesPriority()
        {
            List<BlockedPermittedIpAddressesModel> blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false }
            };
            List<BlockedPermittedIpAddressesModel> permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false }
            };
            BlockedOrPermittedIpService service = CreateService(blocked: blocked, permitted: permitted);
            Assert.False(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.3.4"), _siteId));
        }

        [Fact]
        public async Task ReturnsFalse_WhenIpIsInBothBlockedAndPermittedRanges_PermittedTakesPriority()
        {
            List<BlockedPermittedIpAddressesModel> blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0/24", SiteId = _siteId, IsRange = true }
            };
            List<BlockedPermittedIpAddressesModel> permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0/24", SiteId = _siteId, IsRange = true }
            };
            BlockedOrPermittedIpService service = CreateService(blocked: blocked, permitted: permitted);
            Assert.False(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.3.100"), _siteId));
        }

        /// <summary>
        /// testing a mix of single IP and range: being permitted always overrides being blocked
        /// </summary>
        [Fact]
        public async Task ReturnsFalse_WhenUserIsWithinPermittedRangeEvenIfSpecificallyBlocked()
        {

            List<BlockedPermittedIpAddressesModel> blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.100", SiteId = _siteId, IsRange = false }
            };
            List<BlockedPermittedIpAddressesModel> permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0 - 1.2.3.100", SiteId = _siteId, IsRange = true }
            };
            BlockedOrPermittedIpService service = CreateService(blocked: blocked, permitted: permitted);
            Assert.False(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.3.100"), _siteId));
        }

        /// <summary>
        /// test a complicated mix of types here
        /// </summary>
        [Fact]
        public async Task ReturnsFalse_WhenIpIsInMultiplePermittedRanges()
        {
            List<BlockedPermittedIpAddressesModel> permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0 - 1.2.3.50", SiteId = _siteId, IsRange = true },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.40 - 1.2.3.60", SiteId = _siteId, IsRange = true },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.8", SiteId = _siteId, IsRange = false }
            };
            List<BlockedPermittedIpAddressesModel> blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0 - 1.2.3.90", SiteId = _siteId, IsRange = true }
            };

            BlockedOrPermittedIpService service = CreateService(blocked: blocked, permitted: permitted);
            Assert.False(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.3.41"), _siteId));
        }

        /// <summary>
        /// test a complicated mix of types here
        /// </summary>
        [Fact]
        public async Task ReturnsTrue_WhenIpIsInBlockedRangeAndWeHaveMultiplePermittedRanges()
        {
            List<BlockedPermittedIpAddressesModel> permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0 - 1.2.3.50", SiteId = _siteId, IsRange = true },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.40 - 1.2.3.60", SiteId = _siteId, IsRange = true },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.8", SiteId = _siteId, IsRange = false }
            };
            List<BlockedPermittedIpAddressesModel> blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0 - 1.2.3.90", SiteId = _siteId, IsRange = true }
            };

            BlockedOrPermittedIpService service = CreateService(blocked: blocked, permitted: permitted);
            Assert.True(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.3.61"), _siteId));
        }

        /// <summary>
        /// test a complicated mix of types here
        /// </summary>
        [Fact]
        public async Task ReturnsTrue_WhenIpIsNotInPermittedRangesAndOutsideOfBlockedRanges()
        {
            List<BlockedPermittedIpAddressesModel> permitted = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0 - 1.2.3.50", SiteId = _siteId, IsRange = true },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.40 - 1.2.3.60", SiteId = _siteId, IsRange = true },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.8", SiteId = _siteId, IsRange = false }
            };
            List<BlockedPermittedIpAddressesModel> blocked = new List<BlockedPermittedIpAddressesModel>
            {
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.4", SiteId = _siteId, IsRange = false },
                new BlockedPermittedIpAddressesModel { IpAddress = "1.2.3.0 - 1.2.3.90", SiteId = _siteId, IsRange = true }
            };

            BlockedOrPermittedIpService service = CreateService(blocked: blocked, permitted: permitted);
            Assert.True(await service.IsBlockedOrPermittedIpAsync(IPAddress.Parse("1.2.3.99"), _siteId));
        }
    }
}
