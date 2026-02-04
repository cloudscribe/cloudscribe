using cloudscribe.Core.Models;
using Microsoft.Extensions.Logging;
using NetTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.IPService
{
    public partial class BlockedOrPermittedIpService : IBlockedOrPermittedIpService
    {
        private readonly IIpAddressCache _ipCache;
        private readonly IipAddressCommands _iipAddressCommands;
        private readonly ILogger _log;

        public BlockedOrPermittedIpService(
            IIpAddressCache ipCache,
            IipAddressCommands iipAddressCommands,
            ILogger<BlockedOrPermittedIpService> logger)
        {
            _ipCache = ipCache;
            _iipAddressCommands = iipAddressCommands;
            _log = logger;
        }

        public async Task<bool> IsBlockedOrPermittedIpAsync(IPAddress ipAddress, Guid siteId, CancellationToken cancellationToken = default)
        {
            if (siteId == Guid.Empty || ipAddress == null)
            {
                _log.LogWarning("IsBlockedOrPermittedIpAsync called with null IP address or site ID.");
                return false;
            }

            // Load IPs from cache (async)
            var blockedIps = await _ipCache.GetBlockedIpAddressesAsync(siteId, cancellationToken);
            var permittedIps = await _ipCache.GetPermittedIpAddressesAsync(siteId, cancellationToken);

            if (blockedIps.Count == 0 && permittedIps.Count == 0)
            {
                return false;
            }

            // Split into ranges and singles
            var permittedRanges = permittedIps.Where(x => x.IsRange);
            var blockedRanges = blockedIps.Where(x => x.IsRange);
            var blockedSingleIps = blockedIps.Where(x => !x.IsRange).ToList();
            var permittedSingleIps = permittedIps.Where(x => !x.IsRange).ToList();

            ////////// permitted always wins out...

            // individual permitted IPs
            if (permittedSingleIps.Any())
            {
                bool isPermittedByIndividualRule = permittedSingleIps.Any(x => x.IpAddress == ipAddress.ToString());

                if (isPermittedByIndividualRule) return false;
            }

            // Check permitted ranges
            if (permittedRanges.Any())
            {
                if (IsIpInAnyRange(ipAddress, permittedRanges)) return false;
            }

            //////// Having any permitted rules will always prohibit anything NOT in the permitted rules.
            //////// So if we were not permitted by the above checks, we're prohibited now.
            if (permittedSingleIps.Any() || permittedRanges.Any()) return true;


            //////////////////////
            // Just worry about blocked now
            bool isBlocked = false;

            if (blockedSingleIps.Count > 0)
            {
                isBlocked = blockedSingleIps.Any(x => x.IpAddress == ipAddress.ToString());
            }

            if (!isBlocked && blockedRanges.Any())
            {
                isBlocked = IsIpInAnyRange(ipAddress, blockedRanges);
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