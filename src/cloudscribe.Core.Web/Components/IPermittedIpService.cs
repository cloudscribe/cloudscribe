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
    public interface IPermittedIpService
    {
        Task<bool> AddPermittedIpAddress(BlockedPermittedIpAddressesModel ipAddress, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<bool> UpdatePermittedIpAddress(BlockedPermittedIpAddressesModel ipAddress, CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedResult<IpAddressesViewModel>> GetPermittedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<IActionResult> DeletePermittedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedResult<IpAddressesViewModel>> SearchPermittedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken));
    }
}