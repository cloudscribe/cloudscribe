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

namespace cloudscribe.Core.Web.Components
{
    public class BlockedOrPermittedIpService : IBlockedOrPermittedIpService
    {
        private readonly List<BlockedPermittedIpAddressesModel> _blockedIps, _permittedIps;
        private readonly IipAddressCommands _iipAddressCommands;
        private SiteContext _currentSite;
        private ILogger _log;
        private readonly IMemoryCache _memoryCache;

        public BlockedOrPermittedIpService(SiteContext currentSite, IipAddressCommands iipAddressCommands, ILogger<BlockedIpService> logger, IMemoryCache memoryCache, CancellationToken cancellationToken = default(CancellationToken))
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
            if (siteId == Guid.Empty)
            {
                return false;
            }

            bool isBlocked = false;

            if (_blockedIps == null || _blockedIps.Count <= 0)
            {
                if (_permittedIps != null && _permittedIps.Count > 0)
                {
                    // if there are no blocked ips, but there are permitted ips, then the ip is blocked unless permitted
                    if(_permittedIps.Any(x => x.IpAddress == ipAddress.ToString() && x.SiteId == siteId && x.IsRange == false))
                    {
                        isBlocked = false;
                    }
                    else
                    {
                        isBlocked = true;
                    }
                }
                else
                {
                    // if there are no blocked or permitted ips, then we do not block the ip
                    isBlocked = false;
                }
            } else if (_blockedIps.Any(x => x.IpAddress == ipAddress.ToString() && x.SiteId == siteId && x.IsRange == false))
            {
                //blocked ip specifically mentioned
                isBlocked = true;
            }
            else if (_permittedIps.Any(x => x.IpAddress == ipAddress.ToString() && x.SiteId == siteId && x.IsRange == false))
            {
                //permitted ip specifically mentioned
                isBlocked = false;
            }
            else
            {
                //check for ranges
                List<BlockedPermittedIpAddressesModel> blockedIpRanges = _blockedIps.Where(x => x.SiteId == siteId && x.IsRange == true).ToList();

                foreach (var range in blockedIpRanges)
                {
                    IPAddressRange ipRange = IPAddressRange.Parse(range.IpAddress);

                    if (ipRange.Contains(ipAddress))
                    {
                        isBlocked = true;
                    }
                }
            }

            return isBlocked;
        }
    }
}