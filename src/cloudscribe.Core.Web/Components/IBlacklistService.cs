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
    public interface IBlacklistService
    {
        bool IsBlacklisted(IPAddress ipAddress, Guid siteId);
        
        Task<bool> AddBlacklistedIpAddress(BlackWhiteListedIpAddressesModel ipAddress, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<bool> UpdateBlacklistedIpAddress(BlackWhiteListedIpAddressesModel ipAddress, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<PagedResult<IpAddressesViewModel>> GetBlacklistedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken));

        Task<IActionResult> DeleteBlacklistedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedResult<IpAddressesViewModel>> SearchBlacklistedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken));
    }
}