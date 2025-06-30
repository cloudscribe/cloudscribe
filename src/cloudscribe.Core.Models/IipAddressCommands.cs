using cloudscribe.Pagination.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public interface IipAddressCommands
    {
        Task<PagedResult<BlockedPermittedIpAddressesModel>> GetPermittedIpAddresses(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken), bool? IsFromService = false);
        Task<bool> AddPermittedIpAddress(BlockedPermittedIpAddressesModel blockedPermittedIpAddressesModel, CancellationToken cancellationToken);
        Task<bool> UpdatePermittedIpAddress(BlockedPermittedIpAddressesModel blockedPermittedIpAddressesModel, CancellationToken cancellationToken);
        Task<bool> DeletePermittedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken));
        Task<PagedResult<BlockedPermittedIpAddressesModel>> SearchPermittedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedResult<BlockedPermittedIpAddressesModel>> GetBlockedIpAddresses(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken), bool? IsFromService = false);
        Task<bool> AddBlockedIpAddress(BlockedPermittedIpAddressesModel blockedPermittedIpAddressesModel, CancellationToken cancellationToken);
        Task<bool> UpdateBlockedIpAddress(BlockedPermittedIpAddressesModel blockedPermittedIpAddressesModel, CancellationToken cancellationToken);
        Task<bool> DeleteBlockedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken));
        Task<PagedResult<BlockedPermittedIpAddressesModel>> SearchBlockedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken));
    }
}