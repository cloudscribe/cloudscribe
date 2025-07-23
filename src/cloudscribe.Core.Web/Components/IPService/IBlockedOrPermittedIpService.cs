using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.IpAddresses;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.IPService
{
    public interface IBlockedOrPermittedIpService
    {
        bool IsBlockedOrPermittedIp(IPAddress ipAddress, Guid siteId);
        Task<bool> AddBlockedIpAddress(BlockedPermittedIpAddressesModel ipAddress, Guid currentSiteId, CancellationToken cancellationToken = default);

        Task<bool> UpdateBlockedIpAddress(BlockedPermittedIpAddressesModel ipAddress, Guid currentSiteId, CancellationToken cancellationToken = default);

        Task<PagedResult<IpAddressesViewModel>> GetBlockedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        Task<IActionResult> DeleteBlockedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default);

        Task<PagedResult<IpAddressesViewModel>> SearchBlockedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default);

        Task<bool> AddPermittedIpAddress(BlockedPermittedIpAddressesModel ipAddress, CancellationToken cancellationToken = default);

        Task<bool> UpdatePermittedIpAddress(BlockedPermittedIpAddressesModel ipAddress, CancellationToken cancellationToken = default);

        Task<PagedResult<IpAddressesViewModel>> GetPermittedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        Task<IActionResult> DeletePermittedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default);

        Task<PagedResult<IpAddressesViewModel>> SearchPermittedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default);
    }
}