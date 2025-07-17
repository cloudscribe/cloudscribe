using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.IpAddresses;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.IPService
{
    public partial class BlockedOrPermittedIpService : IBlockedOrPermittedIpService
    {
        public Task<bool> AddBlockedIpAddress(BlockedPermittedIpAddressesModel ipAddress, Guid currentSiteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (ipAddress == null || string.IsNullOrWhiteSpace(ipAddress.IpAddress))
            {
                _log.LogError("IP address cannot be null or empty.");
                throw new ArgumentException("IP address cannot be null or empty.");
            }

            if (_blockedIps.Any(i => i.IpAddress == ipAddress.IpAddress))
            {
                _log.LogWarning($"IP address {ipAddress.IpAddress} is already blocked.");
                throw new ArgumentException($"IP address {ipAddress.IpAddress} is already blocked.");
            }

            try
            {
                _memoryCache.Remove("BlockedIpAddresses");

                return _iipAddressCommands.AddBlockedIpAddress(ipAddress, currentSiteId, cancellationToken);
            }
            catch (Exception e)
            {
                _log.LogError($"Error blocking IP Address: {e}");
                throw new ArgumentException($"Error blocking IP Address: {e}");
            }
        }

        public async Task<PagedResult<IpAddressesViewModel>> GetBlockedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(siteId.ToString()))
            {
                _log.LogError("Site ID cannot be null or empty.");
                throw new ArgumentException("Site ID cannot be null or empty.");
            }

            PagedResult<BlockedPermittedIpAddressesModel> blockedIpAddresses = new PagedResult<BlockedPermittedIpAddressesModel>();
            PagedResult<IpAddressesViewModel> result = new PagedResult<IpAddressesViewModel>();

            try
            {
                blockedIpAddresses = await _iipAddressCommands.GetBlockedIpAddresses(siteId, pageNumber, pageSize, cancellationToken);

                result = new PagedResult<IpAddressesViewModel>
                {
                    PageNumber = blockedIpAddresses.PageNumber,
                    PageSize = blockedIpAddresses.PageSize,
                    TotalItems = blockedIpAddresses.TotalItems,
                    Data = blockedIpAddresses.Data.Select(x => new IpAddressesViewModel
                    {
                        Id = x.Id,
                        IpAddress = x.IpAddress,
                        SiteId = x.SiteId,
                        CreatedDate = x.CreatedDate,
                        LastUpdated = x.LastUpdated,
                        IsPermitted = x.IsPermitted,
                        Reason = x.Reason,
                        IsRange = x.IsRange
                    }).ToList()
                };
            }
            catch (Exception e)
            {
                _log.LogError($"Error retrieving blocked IP addresses: {e}");
                throw new ArgumentException($"Error retrieving blocked IP addresses: {e}");
            }

            return result;
        }

        public Task<bool> UpdateBlockedIpAddress(BlockedPermittedIpAddressesModel ipAddress, Guid currentSiteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (ipAddress == null || string.IsNullOrWhiteSpace(ipAddress.IpAddress))
            {
                _log.LogError($"IP Address cannot be null or empty");
                throw new ArgumentException($"IP Address cannot be null or empty");
            }

            try
            {
                _memoryCache.Remove("BlockedIpAddresses");

                return _iipAddressCommands.UpdateBlockedIpAddress(ipAddress, currentSiteId, cancellationToken).ContinueWith(t => true, cancellationToken);
            }
            catch (Exception e)
            {
                _log.LogError($"Error updating blocked IP Address: {e}");
                throw new ArgumentException($"Error updating blocked IP Address: {e}");
            }
        }

        public async Task<IActionResult> DeleteBlockedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (id == Guid.Empty || siteId == Guid.Empty)
            {
                _log.LogError("Invalid ID or Site ID");
                throw new ArgumentException("ID and Site ID cannot be empty.");
            }

            try
            {
                bool result = await _iipAddressCommands.DeleteBlockedIpAddress(id, siteId, cancellationToken);

                if (result)
                {
                    _memoryCache.Remove("BlockedIpAddresses");

                    return new OkResult();
                }
                else
                {
                    _log.LogWarning($"IP address with ID {id} not found or could not be deleted.");
                    throw new ArgumentException($"IP address with ID {id} not found or could not be deleted.");
                }
            }
            catch (Exception e)
            {
                _log.LogError($"Error deleting blocked IP address: {e}");
                throw new ArgumentException($"Error deleting blocked IP address: {e}");
            }
        }

        public async Task<PagedResult<IpAddressesViewModel>> SearchBlockedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(siteId.ToString()))
            {
                _log.LogError("Site ID cannot be null or empty.");
                throw new ArgumentException("Site ID cannot be null or empty.");
            }

            PagedResult<BlockedPermittedIpAddressesModel> blockedIpAddresses = new PagedResult<BlockedPermittedIpAddressesModel>();
            PagedResult<IpAddressesViewModel> result = new PagedResult<IpAddressesViewModel>();

            try
            {
                blockedIpAddresses = await _iipAddressCommands.SearchBlockedIpAddressesAsync(siteId, pageNumber, pageSize, searchTerm, cancellationToken);

                result = new PagedResult<IpAddressesViewModel>
                {
                    PageNumber = blockedIpAddresses.PageNumber,
                    PageSize = blockedIpAddresses.PageSize,
                    TotalItems = blockedIpAddresses.TotalItems,
                    Data = blockedIpAddresses.Data.Select(x => new IpAddressesViewModel
                    {
                        Id = x.Id,
                        IpAddress = x.IpAddress,
                        SiteId = x.SiteId,
                        CreatedDate = x.CreatedDate,
                        LastUpdated = x.LastUpdated,
                        IsPermitted = x.IsPermitted,
                        Reason = x.Reason
                    }).ToList()
                };

                return result;
            }
            catch (Exception e)
            {
                _log.LogError($"Error retrieving blocked IP addresses for search: {e}");
                throw new ArgumentException($"Error retrieving blocked IP addresses for search: {e}");
            }
        }
    }
}