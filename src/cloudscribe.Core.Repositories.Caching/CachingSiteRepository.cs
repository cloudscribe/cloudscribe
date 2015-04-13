// Author:					Joe Audette
// Created:					2014-08-30
// Last Modified:			2015-04-13
// 

using cloudscribe.Caching;
using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.Caching
{

    /// <summary>
    /// a caching wrapper for ISiteRepository, you must pass in an implementation of ISiteRepository
    /// which will be used internally. 
    /// db hits will be reduced by caching but memory use (or other resource like Azure cache)
    /// will be increased.
    /// 
    /// SiteSettings is an object used for most requests so it makes sense to cache it instead of looking
    /// it up on each request in most environments
    /// </summary>
    public sealed class CachingSiteRepository : ISiteRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CachingSiteRepository));

        private ISiteRepository repo = null;

        public CachingSiteRepository(ISiteRepository implementation)
        {
            if ((implementation == null)||(implementation is CachingSiteRepository))
            { 
                throw new ArgumentException("you must pass in an implementation of ISiteRepository"); 
            }

            repo = implementation;
        }


        #region ISiteRepository

        public async Task<bool> Save(ISiteSettings site)
        {
            return await repo.Save(site);
        }


        public async Task<ISiteSettings> Fetch(int siteId)
        {
            //string cachekey = "SiteSettings_" + siteId.ToInvariantString();

            //DateTime expiration = DateTime.Now.AddSeconds(AppSettings.CacheDurationInSeconds_SiteSettings);

            //try
            //{
            //    ISiteSettings siteSettings = CacheManager.Cache.Get<ISiteSettings>(cachekey, expiration, () =>
            //    {
            //        // This is the anonymous function which gets called if the data is not in the cache.
            //        // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
            //        ISiteSettings site = repo.Fetch(siteId);
            //        return site;
            //    });

            //    return siteSettings;
            //}
            //catch (Exception ex)
            //{
            //    log.Error("failed to get siteSettings from cache so loading it directly", ex);
                
            //}

            return await repo.Fetch(siteId);
        }

        public ISiteSettings FetchNonAsync(int siteId)
        {
            return repo.FetchNonAsync(siteId);
        }

        public async Task<ISiteSettings> Fetch(Guid siteGuid)
        {
            return await repo.Fetch(siteGuid);
        }

        public async Task<ISiteSettings> Fetch(string hostName)
        {
            if(
                (AppSettings.Cache_Disabled)
                || (AppSettings.CacheDurationInSeconds_SiteSettings == 0)
                )
            {
                return await repo.Fetch(hostName);
            }

            //string cachekey = "SiteSettings_" + hostName;

            //DateTime expiration = DateTime.Now.AddSeconds(AppSettings.CacheDurationInSeconds_SiteSettings);

            //try
            //{
            //    ISiteSettings siteSettings = CacheManager.Cache.Get<ISiteSettings>(cachekey, expiration, () =>
            //    {
            //        // This is the anonymous function which gets called if the data is not in the cache.
            //        // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
            //        ISiteSettings site = repo.Fetch(hostName);
            //        return site;
            //    });

            //    return siteSettings;
            //}
            //catch (Exception ex)
            //{
            //    log.Error("failed to get siteSettings from cache so loading it directly", ex);

            //}

            

            return await repo.Fetch(hostName);

        }

        public ISiteSettings FetchNonAsync(string hostName)
        {
            if (
                (AppSettings.Cache_Disabled)
                || (AppSettings.CacheDurationInSeconds_SiteSettings == 0)
                )
            {
                return repo.FetchNonAsync(hostName);
            }

            string cachekey = "SiteSettings_" + hostName;

            DateTime expiration = DateTime.Now.AddSeconds(AppSettings.CacheDurationInSeconds_SiteSettings);

            try
            {
                ISiteSettings siteSettings = CacheManager.Cache.Get<ISiteSettings>(cachekey, expiration, () =>
                {
                    // This is the anonymous function which gets called if the data is not in the cache.
                    // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
                    ISiteSettings site = repo.FetchNonAsync(hostName);
                    return site;
                });

                return siteSettings;
            }
            catch (Exception ex)
            {
                log.Error("failed to get siteSettings from cache so loading it directly", ex);

            }



            return repo.FetchNonAsync(hostName);

        }


        public async Task<bool> Delete(int siteId)
        {
            return await repo.Delete(siteId);
        }



        public async Task<int> GetCount()
        {
            return await repo.GetCount();
        }

        public async Task<List<ISiteInfo>> GetList()
        {
            return await repo.GetList();
        }

        public async Task<int> CountOtherSites(int currentSiteId)
        {
            return await repo.CountOtherSites(currentSiteId);
        }

        /// <summary>
        /// pass in -1 for currentSiteId to get all sites
        /// </summary>
        /// <param name="currentSiteId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<ISiteInfo>> GetPageOtherSites(
            int currentSiteId,
            int pageNumber,
            int pageSize)
        {
            return await repo.GetPageOtherSites(currentSiteId, pageNumber, pageSize);
        }

        public async Task<List<ISiteHost>> GetAllHosts()
        {
            return await repo.GetAllHosts();
        }

        public List<ISiteHost> GetAllHostsNonAsync()
        {
            return repo.GetAllHostsNonAsync();
        }

        public async Task<int> GetHostCount()
        {
            return await repo.GetHostCount();
        }

        public async Task<List<ISiteHost>> GetPageHosts(
            int pageNumber,
            int pageSize)
        {
            return await repo.GetPageHosts(pageNumber, pageSize);
        }

        public async Task<List<ISiteHost>> GetSiteHosts(int siteId)
        {
            return await repo.GetSiteHosts(siteId);
        }

        public async Task<ISiteHost> GetSiteHost(string hostName)
        {
            return await repo.GetSiteHost(hostName);
        }


        public async Task<bool> AddHost(Guid siteGuid, int siteId, string hostName)
        {
            return await repo.AddHost(siteGuid, siteId, hostName);
        }

        public async Task<bool> DeleteHost(int hostId)
        {
            return await repo.DeleteHost(hostId);
        }

        public async Task<int> GetSiteIdByHostName(string hostName)
        {
            return await repo.GetSiteIdByHostName(hostName);
        }

        public async Task<List<SiteFolder>> GetSiteFoldersBySite(Guid siteGuid)
        {
            return await repo.GetSiteFoldersBySite(siteGuid);

        }

        public async Task<SiteFolder> GetSiteFolder(string folderName)
        {
            return await repo.GetSiteFolder(folderName);
        }

        public async Task<List<SiteFolder>> GetAllSiteFolders()
        {
            return await repo.GetAllSiteFolders();
        }

        public List<SiteFolder> GetAllSiteFoldersNonAsync()
        {
            return repo.GetAllSiteFoldersNonAsync();
        }

        public async Task<int> GetFolderCount()
        {
            return await repo.GetFolderCount();
        }

        public async Task<List<SiteFolder>> GetPageSiteFolders(
            int pageNumber,
            int pageSize)
        {
            return await repo.GetPageSiteFolders(
                pageNumber,
                pageSize);
        }

        public async Task<bool> Save(SiteFolder siteFolder)
        {
            return await repo.Save(siteFolder);
        }

        public async Task<bool> DeleteFolder(Guid guid)
        {
            return await repo.DeleteFolder(guid);
        }

        public async Task<int> GetSiteIdByFolder(string folderName)
        {
            return await repo.GetSiteIdByFolder(folderName);
        }

        public int GetSiteIdByFolderNonAsync(string folderName)
        {
            return repo.GetSiteIdByFolderNonAsync(folderName);
        }

        public async Task<Guid> GetSiteGuidByFolder(string folderName)
        {
            return await repo.GetSiteGuidByFolder(folderName);
            
        }

        public async Task<bool> FolderExists(string folderName)
        {
            return await repo.FolderExists(folderName);
        }

        //public bool IsAllowedFolder(string folderName)
        //{
        //    bool result = true;

        //    //TODO: wrap in AppSettings class to avoid dependency on System.Configuration here

        //    //if (ConfigurationManager.AppSettings["DisallowedVirtualFolderNames"] != null)
        //    //{
        //    //    string[] disallowedNames
        //    //        = ConfigurationManager.AppSettings["DisallowedVirtualFolderNames"].Split(new char[] { ';' });

        //    //    foreach (string disallowedName in disallowedNames)
        //    //    {
        //    //        if (string.Equals(folderName, disallowedName, StringComparison.InvariantCultureIgnoreCase)) result = false;
        //    //    }

        //    //}


        //    return repo.IsAllowedFolder(folderName); 

        //}


        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);

        }

        public void Dispose(bool disposeManaged)
        {
            repo.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
