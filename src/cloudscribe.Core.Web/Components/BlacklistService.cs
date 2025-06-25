using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.IpAddresses;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class BlacklistService : IBlacklistService
    {
        private readonly List<BlackWhiteListedIpAddressesModel> _blacklistedIps;
        private readonly IipAddressCommands _iipAddressCommands;
        private SiteContext _currentSite;
        private ILogger _log;
        private readonly IMemoryCache _memoryCache;

        public BlacklistService(SiteContext currentSite, IipAddressCommands iipAddressCommands, ILogger<BlacklistService> logger, IMemoryCache memoryCache, CancellationToken cancellationToken = default(CancellationToken))
        {
            _currentSite = currentSite;
            Guid currentSiteId = _currentSite.Id;
            _iipAddressCommands = iipAddressCommands;
            _log = logger;
            _memoryCache = memoryCache;

            PagedResult<BlackWhiteListedIpAddressesModel> blacklistedIpAddresses = _memoryCache.TryGetValue<PagedResult<BlackWhiteListedIpAddressesModel>>("BlacklistedIpAddresses", out blacklistedIpAddresses) ? blacklistedIpAddresses : new PagedResult<BlackWhiteListedIpAddressesModel>();

            if (blacklistedIpAddresses == null || blacklistedIpAddresses.Data.Count <= 0)
            {
                blacklistedIpAddresses = _iipAddressCommands.GetBlacklistedIpAddresses(currentSiteId, 1, -1, cancellationToken, true).ConfigureAwait(true).GetAwaiter().GetResult();

                _memoryCache.Set("BlacklistedIpAddresses", blacklistedIpAddresses);
            }

            if (blacklistedIpAddresses != null && blacklistedIpAddresses.Data.Count > 0)
            {
                _blacklistedIps = blacklistedIpAddresses.Data ?? new List<BlackWhiteListedIpAddressesModel>();
            }
            else
            {
                _blacklistedIps = new List<BlackWhiteListedIpAddressesModel>();
            }
        }

        public bool IsBlacklisted(IPAddress ipAddress, Guid siteId)
        {
            if (_blacklistedIps == null || _blacklistedIps.Count <= 0 || siteId == Guid.Empty)
            {
                return false;
            }
            return _blacklistedIps.Any(x => x.IpAddress == ipAddress.ToString() && x.SiteId == siteId);
        }

        public Task<bool> AddBlacklistedIpAddress(BlackWhiteListedIpAddressesModel ipAddress, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (ipAddress == null || string.IsNullOrWhiteSpace(ipAddress.IpAddress))
            {
                _log.LogError("IP address cannot be null or empty.");
                throw new ArgumentException("IP address cannot be null or empty.");
            }

            if (_blacklistedIps.Any(i => i.IpAddress == ipAddress.IpAddress))
            {
                _log.LogWarning($"IP address {ipAddress.IpAddress} is already blacklisted.");
                throw new ArgumentException($"IP address {ipAddress.IpAddress} is already blacklisted.");
            }

            try
            {
                _memoryCache.Remove("BlacklistedIpAddresses");

                return _iipAddressCommands.AddBlacklistedIpAddress(ipAddress, cancellationToken);
            }
            catch (Exception e)
            {
                _log.LogError($"Error blacklisting IP Address: {e}");
                throw new ArgumentException($"Error blacklisting IP Address: {e}");
            }
        }

        public async Task<PagedResult<IpAddressesViewModel>> GetBlacklistedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(siteId.ToString()))
            {
                _log.LogError("Site ID cannot be null or empty.");
                throw new ArgumentException("Site ID cannot be null or empty.");
            }

            PagedResult<BlackWhiteListedIpAddressesModel> blacklistedIpAddresses = new PagedResult<BlackWhiteListedIpAddressesModel>();
            PagedResult<IpAddressesViewModel> result = new PagedResult<IpAddressesViewModel>();

            try
            {
                blacklistedIpAddresses = await _iipAddressCommands.GetBlacklistedIpAddresses(siteId, pageNumber, pageSize, cancellationToken);

                result = new PagedResult<IpAddressesViewModel>
                {
                    PageNumber = blacklistedIpAddresses.PageNumber,
                    PageSize = blacklistedIpAddresses.PageSize,
                    TotalItems = blacklistedIpAddresses.TotalItems,
                    Data = blacklistedIpAddresses.Data.Select(x => new IpAddressesViewModel
                    {
                        Id = x.Id,
                        IpAddress = x.IpAddress,
                        SiteId = x.SiteId,
                        CreatedDate = x.CreatedDate,
                        LastUpdated = x.LastUpdated,
                        IsWhitelisted = x.IsWhitelisted,
                        Reason = x.Reason
                    }).ToList()
                };
            }
            catch (Exception e)
            {
                _log.LogError($"Error retrieving blacklisted IP addresses: {e}");
                throw new ArgumentException($"Error retrieving blacklisted IP addresses: {e}");
            }

            return result;
        }

        public Task<bool> UpdateBlacklistedIpAddress(BlackWhiteListedIpAddressesModel ipAddress, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (ipAddress == null || string.IsNullOrWhiteSpace(ipAddress.IpAddress))
            {
                _log.LogError($"IP Address cannot be null or empty");
                throw new ArgumentException($"IP Address cannot be null or empty");
            }

            try
            {
                _memoryCache.Remove("BlacklistedIpAddresses");

                return _iipAddressCommands.UpdateBlacklistedIpAddress(ipAddress, cancellationToken).ContinueWith(t => true, cancellationToken);
            }
            catch (Exception e)
            {
                _log.LogError($"Error updating blacklisted IP Address: {e}");
                throw new ArgumentException($"Error updating blacklisted IP Address: {e}");
            }
        }

        public async Task<IActionResult> DeleteBlacklistedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (id == Guid.Empty || siteId == Guid.Empty)
            {
                _log.LogError("Invalid ID or Site ID");
                throw new ArgumentException("ID and Site ID cannot be empty.");
            }

            try
            {
                bool result = await _iipAddressCommands.DeleteBlacklistedIpAddress(id, siteId, cancellationToken);

                if (result)
                {
                    _memoryCache.Remove("BlacklistedIpAddresses");

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
                _log.LogError($"Error deleting blacklisted IP address: {e}");
                throw new ArgumentException($"Error deleting blacklisted IP address: {e}");
            }
        }

        public async Task<PagedResult<IpAddressesViewModel>> SearchBlacklistedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(siteId.ToString()))
            {
                _log.LogError("Site ID cannot be null or empty.");
                throw new ArgumentException("Site ID cannot be null or empty.");
            }

            PagedResult<BlackWhiteListedIpAddressesModel> blacklistedIpAddresses = new PagedResult<BlackWhiteListedIpAddressesModel>();
            PagedResult<IpAddressesViewModel> result = new PagedResult<IpAddressesViewModel>();

            try
            {
                blacklistedIpAddresses = await _iipAddressCommands.SearchBlacklistedIpAddressesAsync(siteId, pageNumber, pageSize, searchTerm, cancellationToken);

                result = new PagedResult<IpAddressesViewModel>
                {
                    PageNumber = blacklistedIpAddresses.PageNumber,
                    PageSize = blacklistedIpAddresses.PageSize,
                    TotalItems = blacklistedIpAddresses.TotalItems,
                    Data = blacklistedIpAddresses.Data.Select(x => new IpAddressesViewModel
                    {
                        Id = x.Id,
                        IpAddress = x.IpAddress,
                        SiteId = x.SiteId,
                        CreatedDate = x.CreatedDate,
                        LastUpdated = x.LastUpdated,
                        IsWhitelisted = x.IsWhitelisted,
                        Reason = x.Reason
                    }).ToList()
                };

                return result;
            }
            catch (Exception e)
            {
                _log.LogError($"Error retrieving blacklisted IP addresses for search: {e}");
                throw new ArgumentException($"Error retrieving blacklisted IP addresses for search: {e}");
            }
        }
    }
}