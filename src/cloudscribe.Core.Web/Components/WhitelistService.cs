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
    public class WhitelistService : IWhitelistService
    {
        private readonly List<BlackWhiteListedIpAddressesModel> _whitelistedIps;
        private readonly IipAddressCommands _iipAddressCommands;
        private SiteContext _currentSite;
        private ILogger _log;
        private readonly IMemoryCache _memoryCache;

        public WhitelistService(SiteContext currentSite, IipAddressCommands iipAddressCommands, ILogger<WhitelistService> logger, IMemoryCache memoryCache, CancellationToken cancellationToken = default(CancellationToken))
        {
            _currentSite = currentSite;
            Guid currentSiteId = _currentSite.Id;
            _iipAddressCommands = iipAddressCommands;
            _log = logger;
            _memoryCache = memoryCache;

            PagedResult<BlackWhiteListedIpAddressesModel> whitelistedIpAddresses = _memoryCache.TryGetValue<PagedResult<BlackWhiteListedIpAddressesModel>>("WhitelistedIpAddresses", out whitelistedIpAddresses) ? whitelistedIpAddresses : new PagedResult<BlackWhiteListedIpAddressesModel>();

            if (whitelistedIpAddresses == null || whitelistedIpAddresses.Data.Count <= 0)
            {
                whitelistedIpAddresses = _iipAddressCommands.GetWhitelistedIpAddresses(currentSiteId, 1, -1, cancellationToken, true).ConfigureAwait(true).GetAwaiter().GetResult();
                
                _memoryCache.Set("WhitelistedIpAddresses", whitelistedIpAddresses);
            }

            if (whitelistedIpAddresses != null && whitelistedIpAddresses.Data.Count > 0)
            {
                _whitelistedIps = whitelistedIpAddresses.Data ?? new List<BlackWhiteListedIpAddressesModel>();
            }
            else
            {
                _whitelistedIps = new List<BlackWhiteListedIpAddressesModel>();
            }
        }

        public bool IsWhitelisted(IPAddress ipAddress, Guid siteId)
        {
            if (_whitelistedIps == null || _whitelistedIps.Count <= 0 || siteId == Guid.Empty)
            {
                return true;
            }
            return _whitelistedIps.Any(x => x.IpAddress == ipAddress.ToString() && x.SiteId == siteId);
        }

        public Task<bool> AddWhitelistedIpAddress(BlackWhiteListedIpAddressesModel ipAddress, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (ipAddress == null || string.IsNullOrWhiteSpace(ipAddress.IpAddress))
            {
                _log.LogError("IP address cannot be null or empty.");
                throw new ArgumentException("IP address cannot be null or empty.");
            }

            if (_whitelistedIps.Any(i => i.IpAddress == ipAddress.IpAddress))
            {
                _log.LogWarning($"IP address {ipAddress.IpAddress} is already whitelisted.");
                throw new ArgumentException($"IP address {ipAddress.IpAddress} is already whitelisted.");
            }

            try
            {
                _memoryCache.Remove("WhitelistedIpAddresses");

                return _iipAddressCommands.AddWhitelistedIpAddress(ipAddress, cancellationToken).ContinueWith(t => true, cancellationToken);
            }
            catch (Exception e)
            {
                _log.LogError($"Error whitelisting IP Address: {e}");
                throw new ArgumentException($"Error whitelisting IP Address: {e}");
            }
        }

        public Task<bool> UpdateWhitelistedIpAddress(BlackWhiteListedIpAddressesModel ipAddress, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (ipAddress == null || string.IsNullOrWhiteSpace(ipAddress.IpAddress))
            {
                _log.LogError($"IP Address cannot be null or empty");
                throw new ArgumentException($"IP Address cannot be null or empty");
            }

            try
            {
                _memoryCache.Remove("WhitelistedIpAddresses");

                return _iipAddressCommands.UpdateWhitelistedIpAddress(ipAddress, cancellationToken).ContinueWith(t => true, cancellationToken);
            }
            catch (Exception e)
            {
                _log.LogError($"Error updating whitelisted IP Address: {e}");
                throw new ArgumentException($"Error updating whitelisted IP Address: {e}");
            }
        }

        public async Task<PagedResult<IpAddressesViewModel>> GetWhitelistedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(siteId.ToString()))
            {
                _log.LogError("Site ID cannot be null or empty.");
                throw new ArgumentException("Site ID cannot be null or empty.");
            }

            PagedResult<BlackWhiteListedIpAddressesModel> whitelistedIpAddresses = new PagedResult<BlackWhiteListedIpAddressesModel>();
            PagedResult<IpAddressesViewModel> result = new PagedResult<IpAddressesViewModel>();

            try
            {
                whitelistedIpAddresses = await _iipAddressCommands.GetWhitelistedIpAddresses(siteId, pageNumber, pageSize, cancellationToken);

                result = new PagedResult<IpAddressesViewModel>
                {
                    PageNumber = whitelistedIpAddresses.PageNumber,
                    PageSize = whitelistedIpAddresses.PageSize,
                    TotalItems = whitelistedIpAddresses.TotalItems,
                    Data = whitelistedIpAddresses.Data.Select(x => new IpAddressesViewModel
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
                _log.LogError($"Error retrieving whitelisted IP addresses: {e}");
                throw new ArgumentException($"Error retrieving whitelisted IP addresses: {e}");
            }

            return result;
        }

        public async Task<IActionResult> DeleteWhitelistedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (id == Guid.Empty || siteId == Guid.Empty)
            {
                _log.LogError("Invalid ID or Site ID");
                throw new ArgumentException("ID and Site ID cannot be empty.");
            }

            try
            {
                bool result = await _iipAddressCommands.DeleteWhitelistedIpAddress(id, siteId, cancellationToken);

                if (result)
                {
                    _memoryCache.Remove("WhitelistedIpAddresses");

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
                _log.LogError($"Error deleting whitelisted IP address: {e}");
                throw new ArgumentException($"Error deleting whitelisted IP address: {e}");
            }
        }

        public async Task<PagedResult<IpAddressesViewModel>> SearchWhitelistedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(siteId.ToString()))
            {
                _log.LogError("Site ID cannot be null or empty.");
                throw new ArgumentException("Site ID cannot be null or empty.");
            }

            PagedResult<BlackWhiteListedIpAddressesModel> whitelistedIpAddresses = new PagedResult<BlackWhiteListedIpAddressesModel>();
            PagedResult<IpAddressesViewModel> result = new PagedResult<IpAddressesViewModel>();

            try
            {
                whitelistedIpAddresses = await _iipAddressCommands.SearchWhitelistedIpAddressesAsync(siteId, pageNumber, pageSize, searchTerm, cancellationToken);

                result = new PagedResult<IpAddressesViewModel>
                {
                    PageNumber = whitelistedIpAddresses.PageNumber,
                    PageSize = whitelistedIpAddresses.PageSize,
                    TotalItems = whitelistedIpAddresses.TotalItems,
                    Data = whitelistedIpAddresses.Data.Select(x => new IpAddressesViewModel
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
                _log.LogError($"Error retrieving whitelisted IP addresses for search: {e}");
                throw new ArgumentException($"Error retrieving whitelisted IP addresses for search: {e}");
            }
        }
    }
}