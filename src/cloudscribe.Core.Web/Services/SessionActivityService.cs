using cloudscribe.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Services
{
    public class SessionActivityService : ISessionActivityService
    {
        private readonly IMemoryCache _cache;
        private readonly ISiteQueries _siteQueries;
        private readonly ILogger<SessionActivityService> _logger;

        public SessionActivityService(
            IMemoryCache cache,
            ISiteQueries siteQueries,
            ILogger<SessionActivityService> logger)
        {
            _cache = cache;
            _siteQueries = siteQueries;
            _logger = logger;
        }

        public void UpdateActivity(string userId, string siteId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(siteId))
            {
                return;
            }

            var key = $"session_{siteId}_{userId}";

            // Get site's configured timeout
            // Parse the siteId as GUID and fetch the site
            ISiteSettings site = null;
            if (Guid.TryParse(siteId, out var siteGuid))
            {
                // Using GetAwaiter().GetResult() to avoid async/await in sync method
                site = _siteQueries.Fetch(siteGuid, CancellationToken.None).GetAwaiter().GetResult();
            }

            // IMPORTANT: If MaximumInactivityInMinutes is null, empty, or zero, 
            // auto-logout is disabled - don't track anything
            if (site != null &&
                !string.IsNullOrWhiteSpace(site.MaximumInactivityInMinutes) &&
                double.TryParse(site.MaximumInactivityInMinutes, out var minutes) &&
                minutes > 0)
            {
                var expiryTime = DateTime.UtcNow.AddMinutes(minutes);

                // Store in cache with absolute expiry slightly longer than session
                // This ensures cache entries clean themselves up
                _cache.Set(key, expiryTime, TimeSpan.FromMinutes(minutes + 5));

                _logger.LogDebug($"Updated session activity for user {userId} in site {siteId}: expires at {expiryTime}");
            }
            else
            {
                // Auto-logout is disabled for this site
                // Remove any existing cache entry to ensure no tracking
                _cache.Remove(key);
                
                if (site != null)
                {
                    _logger.LogDebug($"Auto-logout disabled for site {siteId} - removed session tracking for user {userId}");
                }
            }
        }

        public DateTime? GetSessionExpiry(string userId, string siteId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(siteId))
            {
                return null;
            }

            var key = $"session_{siteId}_{userId}";
            if (_cache.TryGetValue<DateTime>(key, out var expiry))
            {
                return expiry;
            }
            return null;
        }

        public void RemoveSession(string userId, string siteId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(siteId))
            {
                return;
            }

            var key = $"session_{siteId}_{userId}";
            _cache.Remove(key);
            _logger.LogDebug($"Removed session tracking for user {userId} in site {siteId}");
        }
    }
}