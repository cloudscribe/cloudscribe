using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.IpAddresses;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NetTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class PermittedIpService : IPermittedIpService
    {
        private readonly List<BlockedPermittedIpAddressesModel> _permittedIps;
        private readonly IipAddressCommands _iipAddressCommands;
        private SiteContext _currentSite;
        private ILogger _log;
        private readonly IMemoryCache _memoryCache;

        public PermittedIpService(SiteContext currentSite, IipAddressCommands iipAddressCommands, ILogger<PermittedIpService> logger, IMemoryCache memoryCache, CancellationToken cancellationToken = default(CancellationToken))
        {
            _currentSite = currentSite;
            Guid currentSiteId = _currentSite.Id;
            _iipAddressCommands = iipAddressCommands;
            _log = logger;
            _memoryCache = memoryCache;

            PagedResult<BlockedPermittedIpAddressesModel> permittedIpAddresses = _memoryCache.TryGetValue<PagedResult<BlockedPermittedIpAddressesModel>>("PermittedIpAddresses", out permittedIpAddresses) ? permittedIpAddresses : new PagedResult<BlockedPermittedIpAddressesModel>();

            if (permittedIpAddresses == null || permittedIpAddresses.Data.Count <= 0)
            {
                permittedIpAddresses = _iipAddressCommands.GetPermittedIpAddresses(currentSiteId, 1, -1, cancellationToken, true).ConfigureAwait(true).GetAwaiter().GetResult();
                
                _memoryCache.Set("PermittedIpAddresses", permittedIpAddresses);
            }

            if (permittedIpAddresses != null && permittedIpAddresses.Data.Count > 0)
            {
                _permittedIps = permittedIpAddresses.Data ?? new List<BlockedPermittedIpAddressesModel>();
            }
            else
            {
                _permittedIps = new List<BlockedPermittedIpAddressesModel>();
            }
        }

        public Task<bool> AddPermittedIpAddress(BlockedPermittedIpAddressesModel ipAddress, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (ipAddress == null || string.IsNullOrWhiteSpace(ipAddress.IpAddress))
            {
                _log.LogError("IP address cannot be null or empty.");
                throw new ArgumentException("IP address cannot be null or empty.");
            }

            if (_permittedIps.Any(i => i.IpAddress == ipAddress.IpAddress))
            {
                _log.LogWarning($"IP address {ipAddress.IpAddress} is already permitted.");
                throw new ArgumentException($"IP address {ipAddress.IpAddress} is already permitted.");
            }

            try
            {
                _memoryCache.Remove("PermittedIpAddresses");

                Guid currentSiteId = _currentSite.Id;

                return _iipAddressCommands.AddPermittedIpAddress(ipAddress, currentSiteId, cancellationToken).ContinueWith(t => true, cancellationToken);
            }
            catch (Exception e)
            {
                _log.LogError($"Error permitting IP Address: {e}");
                throw new ArgumentException($"Error permitting IP Address: {e}");
            }
        }

        public Task<bool> UpdatePermittedIpAddress(BlockedPermittedIpAddressesModel ipAddress, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (ipAddress == null || string.IsNullOrWhiteSpace(ipAddress.IpAddress))
            {
                _log.LogError($"IP Address cannot be null or empty");
                throw new ArgumentException($"IP Address cannot be null or empty");
            }

            try
            {
                _memoryCache.Remove("PermittedIpAddresses");

                Guid currentSiteId = _currentSite.Id;

                return _iipAddressCommands.UpdatePermittedIpAddress(ipAddress, currentSiteId, cancellationToken).ContinueWith(t => true, cancellationToken);
            }
            catch (Exception e)
            {
                _log.LogError($"Error updating permitted IP Address: {e}");
                throw new ArgumentException($"Error updating permitted IP Address: {e}");
            }
        }

        public async Task<PagedResult<IpAddressesViewModel>> GetPermittedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(siteId.ToString()))
            {
                _log.LogError("Site ID cannot be null or empty.");
                throw new ArgumentException("Site ID cannot be null or empty.");
            }

            PagedResult<BlockedPermittedIpAddressesModel> permittedIpAddresses = new PagedResult<BlockedPermittedIpAddressesModel>();
            PagedResult<IpAddressesViewModel> result = new PagedResult<IpAddressesViewModel>();

            try
            {
                permittedIpAddresses = await _iipAddressCommands.GetPermittedIpAddresses(siteId, pageNumber, pageSize, cancellationToken);

                result = new PagedResult<IpAddressesViewModel>
                {
                    PageNumber = permittedIpAddresses.PageNumber,
                    PageSize = permittedIpAddresses.PageSize,
                    TotalItems = permittedIpAddresses.TotalItems,
                    Data = permittedIpAddresses.Data.Select(x => new IpAddressesViewModel
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
                _log.LogError($"Error retrieving permitted IP addresses: {e}");
                throw new ArgumentException($"Error retrieving permitted IP addresses: {e}");
            }

            return result;
        }

        public async Task<IActionResult> DeletePermittedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (id == Guid.Empty || siteId == Guid.Empty)
            {
                _log.LogError("Invalid ID or Site ID");
                throw new ArgumentException("ID and Site ID cannot be empty.");
            }

            try
            {
                bool result = await _iipAddressCommands.DeletePermittedIpAddress(id, siteId, cancellationToken);

                if (result)
                {
                    _memoryCache.Remove("PermittedIpAddresses");

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
                _log.LogError($"Error deleting permitted IP address: {e}");
                throw new ArgumentException($"Error deleting permitted IP address: {e}");
            }
        }

        public async Task<PagedResult<IpAddressesViewModel>> SearchPermittedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(siteId.ToString()))
            {
                _log.LogError("Site ID cannot be null or empty.");
                throw new ArgumentException("Site ID cannot be null or empty.");
            }

            PagedResult<BlockedPermittedIpAddressesModel> permittedIpAddresses = new PagedResult<BlockedPermittedIpAddressesModel>();
            PagedResult<IpAddressesViewModel> result = new PagedResult<IpAddressesViewModel>();

            try
            {
                permittedIpAddresses = await _iipAddressCommands.SearchPermittedIpAddressesAsync(siteId, pageNumber, pageSize, searchTerm, cancellationToken);

                result = new PagedResult<IpAddressesViewModel>
                {
                    PageNumber = permittedIpAddresses.PageNumber,
                    PageSize = permittedIpAddresses.PageSize,
                    TotalItems = permittedIpAddresses.TotalItems,
                    Data = permittedIpAddresses.Data.Select(x => new IpAddressesViewModel
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
                _log.LogError($"Error retrieving permitted IP addresses for search: {e}");
                throw new ArgumentException($"Error retrieving permitted IP addresses for search: {e}");
            }
        }
    }
}