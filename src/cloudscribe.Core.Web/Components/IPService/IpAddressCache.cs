using cloudscribe.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.IPService
{
    public class IpAddressCache : IIpAddressCache
    {
        private readonly IMemoryCache _cache;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<IpAddressCache> _logger;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

        public IpAddressCache(
            IMemoryCache cache,
            IServiceProvider serviceProvider,
            ILogger<IpAddressCache> logger)
        {
            _cache = cache;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<List<BlockedPermittedIpAddressesModel>> GetBlockedIpAddressesAsync(
            Guid siteId, 
            CancellationToken cancellationToken = default)
        {
            string cacheKey = GetBlockedCacheKey(siteId);
            
            if (!_cache.TryGetValue(cacheKey, out List<BlockedPermittedIpAddressesModel> ips))
            {
                _logger.LogDebug("Cache miss for blocked IPs, site {SiteId}", siteId);
                
                using (var scope = _serviceProvider.CreateScope())
                {
                    var commands = scope.ServiceProvider.GetRequiredService<IipAddressCommands>();
                    
                    var result = await commands.GetBlockedIpAddresses(
                        siteId, 
                        pageNumber: 1, 
                        pageSize: -1, 
                        cancellationToken: cancellationToken, 
                        IsFromService: true);
                    
                    ips = result?.Data ?? new List<BlockedPermittedIpAddressesModel>();
                    
                    _cache.Set(cacheKey, ips, CacheDuration);
                }
            }
            
            return ips;
        }

        public async Task<List<BlockedPermittedIpAddressesModel>> GetPermittedIpAddressesAsync(
            Guid siteId, 
            CancellationToken cancellationToken = default)
        {
            string cacheKey = GetPermittedCacheKey(siteId);
            
            if (!_cache.TryGetValue(cacheKey, out List<BlockedPermittedIpAddressesModel> ips))
            {
                _logger.LogDebug("Cache miss for permitted IPs, site {SiteId}", siteId);
                
                using (var scope = _serviceProvider.CreateScope())
                {
                    var commands = scope.ServiceProvider.GetRequiredService<IipAddressCommands>();
                    
                    var result = await commands.GetPermittedIpAddresses(
                        siteId, 
                        pageNumber: 1, 
                        pageSize: -1, 
                        cancellationToken: cancellationToken, 
                        IsFromService: true);
                    
                    ips = result?.Data ?? new List<BlockedPermittedIpAddressesModel>();
                    
                    _cache.Set(cacheKey, ips, CacheDuration);
                }
            }
            
            return ips;
        }

        public void InvalidateBlockedIps(Guid siteId)
        {
            string cacheKey = GetBlockedCacheKey(siteId);
            _cache.Remove(cacheKey);
            _logger.LogInformation("Invalidated blocked IPs cache for site {SiteId}", siteId);
        }

        public void InvalidatePermittedIps(Guid siteId)
        {
            string cacheKey = GetPermittedCacheKey(siteId);
            _cache.Remove(cacheKey);
            _logger.LogInformation("Invalidated permitted IPs cache for site {SiteId}", siteId);
        }

        public void InvalidateAll(Guid siteId)
        {
            InvalidateBlockedIps(siteId);
            InvalidatePermittedIps(siteId);
        }

        private static string GetBlockedCacheKey(Guid siteId) => $"BlockedIpAddresses_{siteId}";
        private static string GetPermittedCacheKey(Guid siteId) => $"PermittedIpAddresses_{siteId}";
    }
}
