using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NetTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace cloudscribe.Core.Web.Components.IPService
{
    public partial class BlockedOrPermittedIpService : IBlockedOrPermittedIpService
    {
        private List<BlockedPermittedIpAddressesModel> _blockedIps, _permittedIps;
        private IEnumerable<BlockedPermittedIpAddressesModel> permittedRanges, blockedRanges;
        private readonly IipAddressCommands _iipAddressCommands;
        private SiteContext _currentSite;
        private ILogger _log;
        private readonly IMemoryCache _memoryCache;

        public BlockedOrPermittedIpService(SiteContext currentSite, IipAddressCommands iipAddressCommands, ILogger<BlockedOrPermittedIpService> logger, IMemoryCache memoryCache, CancellationToken cancellationToken = default(CancellationToken))
        {
            _currentSite = currentSite;
            Guid currentSiteId = _currentSite.Id;
            _iipAddressCommands = iipAddressCommands;
            _log = logger;
            _memoryCache = memoryCache;

            PagedResult<BlockedPermittedIpAddressesModel> blockedIpAddresses = _memoryCache.TryGetValue<PagedResult<BlockedPermittedIpAddressesModel>>("BlockedIpAddresses", out blockedIpAddresses) ? blockedIpAddresses : new PagedResult<BlockedPermittedIpAddressesModel>();

            if (blockedIpAddresses == null || blockedIpAddresses.Data.Count <= 0)
            {
                blockedIpAddresses = _iipAddressCommands.GetBlockedIpAddresses(currentSiteId, 1, -1, cancellationToken, true).ConfigureAwait(true).GetAwaiter().GetResult();

                _memoryCache.Set("BlockedIpAddresses", blockedIpAddresses);
            }

            if (blockedIpAddresses != null && blockedIpAddresses.Data.Count > 0)
            {
                _blockedIps = blockedIpAddresses.Data ?? new List<BlockedPermittedIpAddressesModel>();
            }
            else
            {
                _blockedIps = new List<BlockedPermittedIpAddressesModel>();
            }

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

        public bool IsBlockedOrPermittedIp(IPAddress ipAddress, Guid siteId)
        {
            if (siteId == Guid.Empty || ipAddress == null)
            {
                _log.LogWarning("IsBlockedOrPermittedIp called with null IP address or site ID.");
                return false;
            }

            if (_blockedIps.Count <= 0 && _permittedIps.Count <= 0)
            {
                _log.LogWarning("Blocked or permitted IP lists are not initialized.");
                return false;
            }

            permittedRanges = _permittedIps.Where(x => x.SiteId == siteId && x.IsRange == true);
            blockedRanges = _blockedIps.Where(x => x.SiteId == siteId && x.IsRange == true);
            _blockedIps = _blockedIps.Where(x => x.SiteId == siteId && x.IsRange == false).ToList();
            _permittedIps = _permittedIps.Where(x => x.SiteId == siteId && x.IsRange == false).ToList();

            bool isBlocked = false;

            if (_blockedIps.Count > 0)
            {
                isBlocked = _blockedIps.Any(x => x.IpAddress == ipAddress.ToString() && x.SiteId == siteId && x.IsRange == false);

                if (isBlocked)
                {
                    if (_permittedIps.Count > 0)
                    {
                        isBlocked = !_permittedIps.Any(x => x.IpAddress == ipAddress.ToString() && x.SiteId == siteId && x.IsRange == false);

                        if (isBlocked)
                        {
                            isBlocked = !IsIpInAnyRange(ipAddress, permittedRanges);
                        }
                    }
                    else
                    {
                        if (permittedRanges.Any())
                        {
                            isBlocked = !IsIpInAnyRange(ipAddress, permittedRanges);
                        }
                    }
                }
                else
                {
                    if (blockedRanges.Any())
                    {
                        isBlocked = IsIpInAnyRange(ipAddress, blockedRanges);

                        if (isBlocked)
                        {
                            if (_permittedIps.Count > 0)
                            {
                                isBlocked = !_permittedIps.Any(x => x.IpAddress == ipAddress.ToString() && x.SiteId == siteId && x.IsRange == false);

                                if (isBlocked)
                                {
                                    isBlocked = !IsIpInAnyRange(ipAddress, permittedRanges);
                                }
                            }
                        }
                        else if (_permittedIps.Count > 0)
                        {
                            isBlocked = !_permittedIps.Any(x => x.IpAddress == ipAddress.ToString() && x.SiteId == siteId && x.IsRange == false);

                            if (!isBlocked)
                            {
                                if (permittedRanges.Any())
                                {
                                    isBlocked = !IsIpInAnyRange(ipAddress, permittedRanges);
                                }
                            }
                            else
                            {
                                isBlocked = true;
                            }
                        }
                        else
                        {
                            if (permittedRanges.Any())
                            {
                                isBlocked = !IsIpInAnyRange(ipAddress, permittedRanges);
                            }
                            else
                            {
                                isBlocked = false;
                            }
                        }
                    }
                }
            }
            else if(blockedRanges.Any())
            {
                isBlocked = IsIpInAnyRange(ipAddress, blockedRanges);

                if (isBlocked)
                {
                    if (_permittedIps.Count > 0)
                    {
                        isBlocked = !_permittedIps.Any(x => x.IpAddress == ipAddress.ToString() && x.SiteId == siteId && x.IsRange == false);

                        if (isBlocked)
                        {
                            isBlocked = !IsIpInAnyRange(ipAddress, permittedRanges);
                        }
                    }
                    else
                    {
                        if (permittedRanges.Any())
                        {
                            isBlocked = !IsIpInAnyRange(ipAddress, permittedRanges);
                        }
                    }
                }
            }
            else
            {
                if (_permittedIps.Count > 0)
                {
                    isBlocked = !_permittedIps.Any(x => x.IpAddress == ipAddress.ToString() && x.SiteId == siteId && x.IsRange == false);

                    if (!isBlocked)
                    {
                        if (permittedRanges.Any())
                        {
                            isBlocked = !IsIpInAnyRange(ipAddress, permittedRanges);
                        }
                    }
                    else
                    {
                        isBlocked = true;
                    }
                }
                else
                {
                    if (permittedRanges.Any())
                    {
                        isBlocked = !IsIpInAnyRange(ipAddress, permittedRanges);
                    }
                    else
                    {
                        isBlocked = false;
                    }
                }
            }

            return isBlocked;
        }

        private bool IsIpInAnyRange(IPAddress ipAddress, IEnumerable<BlockedPermittedIpAddressesModel> ranges)
        {
            foreach (var range in ranges)
            {
                IPAddressRange ipRange = IPAddressRange.Parse(range.IpAddress);

                if (ipRange.Contains(ipAddress))
                {
                    return true;
                }
            }

            return false;
        }
    }
}