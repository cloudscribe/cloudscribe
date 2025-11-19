using cloudscribe.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.IPService
{
    /// <summary>
    /// Provides caching for IP address blocking and permitting rules.
    /// Manages tenant-aware cache keys to prevent cross-tenant data leakage.
    /// </summary>
    public interface IIpAddressCache
    {
        /// <summary>
        /// Gets the list of blocked IP addresses for a specific site, using cache when available.
        /// </summary>
        /// <param name="siteId">The site identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of blocked IP address models</returns>
        Task<List<BlockedPermittedIpAddressesModel>> GetBlockedIpAddressesAsync(Guid siteId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the list of permitted IP addresses for a specific site, using cache when available.
        /// </summary>
        /// <param name="siteId">The site identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of permitted IP address models</returns>
        Task<List<BlockedPermittedIpAddressesModel>> GetPermittedIpAddressesAsync(Guid siteId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Invalidates the blocked IP addresses cache for a specific site.
        /// Call this when blocked IPs are added, updated, or deleted.
        /// </summary>
        /// <param name="siteId">The site identifier</param>
        void InvalidateBlockedIps(Guid siteId);

        /// <summary>
        /// Invalidates the permitted IP addresses cache for a specific site.
        /// Call this when permitted IPs are added, updated, or deleted.
        /// </summary>
        /// <param name="siteId">The site identifier</param>
        void InvalidatePermittedIps(Guid siteId);

        /// <summary>
        /// Invalidates both blocked and permitted IP caches for a specific site.
        /// </summary>
        /// <param name="siteId">The site identifier</param>
        void InvalidateAll(Guid siteId);
    }
}
