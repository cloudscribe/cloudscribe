using cloudscribe.Pagination.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public interface IipAddressCommands
    {
        Task<PagedResult<BlackWhiteListedIpAddressesModel>> GetWhitelistedIpAddresses(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken), bool? IsFromService = false);
        Task<bool> AddWhitelistedIpAddress(BlackWhiteListedIpAddressesModel blackWhiteListedIpAddressesModel, CancellationToken cancellationToken);
        Task<bool> UpdateWhitelistedIpAddress(BlackWhiteListedIpAddressesModel blackWhiteListedIpAddressesModel, CancellationToken cancellationToken);
        Task<bool> DeleteWhitelistedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken));
        Task<PagedResult<BlackWhiteListedIpAddressesModel>> SearchWhitelistedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedResult<BlackWhiteListedIpAddressesModel>> GetBlacklistedIpAddresses(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken), bool? IsFromService = false);
        Task<bool> AddBlacklistedIpAddress(BlackWhiteListedIpAddressesModel blackWhiteListedIpAddressesModel, CancellationToken cancellationToken);
        Task<bool> UpdateBlacklistedIpAddress(BlackWhiteListedIpAddressesModel blackWhiteListedIpAddressesModel, CancellationToken cancellationToken);
        Task<bool> DeleteBlacklistedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken));
        Task<PagedResult<BlackWhiteListedIpAddressesModel>> SearchBlacklistedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken));
    }
}