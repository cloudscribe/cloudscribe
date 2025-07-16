using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.IpAddresses;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public interface IBlockedIpService
    {
        Task<bool> AddBlockedIpAddress(BlockedPermittedIpAddressesModel ipAddress, Guid currentSiteId, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<bool> UpdateBlockedIpAddress(BlockedPermittedIpAddressesModel ipAddress, Guid currentSiteId, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<PagedResult<IpAddressesViewModel>> GetBlockedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken));

        Task<IActionResult> DeleteBlockedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedResult<IpAddressesViewModel>> SearchBlockedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken));
    }
}