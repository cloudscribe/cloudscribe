using cloudscribe.Core.DataProtection;
using cloudscribe.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{

    public class CachingSiteContextResolver : SiteContextResolver
    {
        public CachingSiteContextResolver(
            CacheHelper cacheHelper,
            ISiteQueries siteRepository,
            SiteDataProtector dataProtector,
            IOptions<MultiTenantOptions> multiTenantOptions,
            ILogger<CachingSiteContextResolver> logger,
            IOptions<CachingSiteResolverOptions> cachingOptionsAccessor
            ) :base(siteRepository, dataProtector, multiTenantOptions)
        {
            _cacheHelper = cacheHelper;
            _cachingOptions = cachingOptionsAccessor.Value;
            _log = logger;
        }

        private readonly CacheHelper _cacheHelper;
        private readonly CachingSiteResolverOptions _cachingOptions;
        private readonly ILogger _log;

        private async Task<List<string>> GetAllSiteFoldersFolders()
        {
            var result = await _cacheHelper.GetSiteFoldersFromCache();
            if(result != null)
            {
                return result;
            }
            
            result = await SiteQueries.GetAllSiteFolders();
            await _cacheHelper.AddSiteFoldersToCache(result, _cachingOptions.FolderListCacheDuration);
           
            return result;

        }

        private async Task<string> GetCacheKey(string hostName,string pathStartingSegment)
        {
            if (MultiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                var folders = await GetAllSiteFoldersFolders();

                return folders.Contains(pathStartingSegment) ?  pathStartingSegment : "root";

            }

            return hostName.ToLowerInvariant();
        }

        public override async Task<SiteContext> ResolveSite(
            string hostName,
            string path,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var pathStartingSegment = path.StartingSegment();

            var cacheKey = await GetCacheKey(hostName, pathStartingSegment);
            var result = (SiteContext)_cacheHelper.GetItemFromLocalCache(cacheKey);
            if(result != null)
            {
                // we just got site from cache but check last modified from distributed cache in case site was updated on another node
                var lastMod = await _cacheHelper.GetDistributedCacheTimestamp(result.Id);
                if(lastMod.HasValue)
                {
                    if(result.LastModifiedUtc > lastMod.Value)
                    {
                        await _cacheHelper.SetDistributedCacheTimestamp(result.Id, result.LastModifiedUtc);
                    }

                    if(result.LastModifiedUtc < lastMod.Value)
                    {
                        result = null; // reload below since the one we got is stale
                    }
                }
                else
                {
                    await _cacheHelper.SetDistributedCacheTimestamp(result.Id, result.LastModifiedUtc);
                }
            }

            if(result == null)
            {
                _log.LogTrace($"Site not found in cache with key {cacheKey}");

                result = await base.ResolveSite(hostName, path, cancellationToken);
                if(result != null)
                {
                    _log.LogTrace($"Caching site with key {cacheKey}");

                    await _cacheHelper.SetDistributedCacheTimestamp(result.Id, result.LastModifiedUtc);
                    _cacheHelper.AddToCache(cacheKey, result, _cachingOptions.SiteCacheDuration);   
                }
            }
            else
            {
                _log.LogTrace($"Site was found in cache with key {cacheKey}");
            }


            return result;

        }

        public override async Task<SiteContext> GetById(Guid siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var cacheKey = "site-" + siteId.ToString();

            var result = (SiteContext)_cacheHelper.GetItemFromLocalCache(cacheKey);

            if (result != null)
            {
                // we just got site from cache but check last modified from distributed cache in case site was updated on another node
                var lastMod = await _cacheHelper.GetDistributedCacheTimestamp(result.Id);
                if (lastMod.HasValue)
                {
                    if (result.LastModifiedUtc > lastMod.Value)
                    {
                        await _cacheHelper.SetDistributedCacheTimestamp(result.Id, result.LastModifiedUtc);
                    }

                    if (result.LastModifiedUtc < lastMod.Value)
                    {
                        result = null; // reload below since the one we got is stale
                    }
                }
                else
                {
                    await _cacheHelper.SetDistributedCacheTimestamp(result.Id, result.LastModifiedUtc);
                }

            }

            if (result == null)
            {
                result = await base.GetById(siteId, cancellationToken);
                if (result != null)
                {
                    await _cacheHelper.SetDistributedCacheTimestamp(result.Id, result.LastModifiedUtc);
                    _cacheHelper.AddToCache(cacheKey, result, _cachingOptions.SiteCacheDuration);
                    
                }
            }


            return result;
        }


    }
}
