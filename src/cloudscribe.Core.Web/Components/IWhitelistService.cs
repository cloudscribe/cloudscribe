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
    public interface IWhitelistService
    {
        bool IsWhitelisted(IPAddress ipAddress, Guid siteId);
        
        Task<bool> AddWhitelistedIpAddress(BlackWhiteListedIpAddressesModel ipAddress, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<bool> UpdateWhitelistedIpAddress(BlackWhiteListedIpAddressesModel ipAddress, CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedResult<IpAddressesViewModel>> GetWhitelistedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<IActionResult> DeleteWhitelistedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedResult<IpAddressesViewModel>> SearchWhitelistedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken));
    }
}