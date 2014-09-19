// Author:					Joe Audette
// Created:					2014-08-30
// Last Modified:			2014-09-06
// 

using cloudscribe.Caching;
using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

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

        public void Save(ISiteSettings site)
        {
            repo.Save(site);


        }


        public ISiteSettings Fetch(int siteId)
        {
            string cachekey = "SiteSettings_" + siteId.ToInvariantString();

            DateTime expiration = DateTime.Now.AddSeconds(AppSettings.CacheDurationInSeconds_SiteSettings);

            try
            {
                ISiteSettings siteSettings = CacheManager.Cache.Get<ISiteSettings>(cachekey, expiration, () =>
                {
                    // This is the anonymous function which gets called if the data is not in the cache.
                    // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
                    ISiteSettings site = repo.Fetch(siteId);
                    return site;
                });

                return siteSettings;
            }
            catch (Exception ex)
            {
                log.Error("failed to get siteSettings from cache so loading it directly", ex);
                
            }

            return repo.Fetch(siteId);
        }

        public ISiteSettings Fetch(Guid siteGuid)
        {


            return repo.Fetch(siteGuid);
        }

        public ISiteSettings Fetch(string hostName)
        {
            if(
                (AppSettings.Cache_Disabled)
                || (AppSettings.CacheDurationInSeconds_SiteSettings == 0)
                )
            {
                return repo.Fetch(hostName);
            }

            string cachekey = "SiteSettings_" + hostName;

            DateTime expiration = DateTime.Now.AddSeconds(AppSettings.CacheDurationInSeconds_SiteSettings);

            try
            {
                ISiteSettings siteSettings = CacheManager.Cache.Get<ISiteSettings>(cachekey, expiration, () =>
                {
                    // This is the anonymous function which gets called if the data is not in the cache.
                    // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
                    ISiteSettings site = repo.Fetch(hostName);
                    return site;
                });

                return siteSettings;
            }
            catch (Exception ex)
            {
                log.Error("failed to get siteSettings from cache so loading it directly", ex);

            }

            

            return repo.Fetch(hostName);

        }


        public bool Delete(int siteId)
        {
            return repo.Delete(siteId);
        }



        public int GetCount()
        {
            return repo.GetCount();
        }

        public List<ISiteInfo> GetList()
        {
            return repo.GetList();
        }

        /// <summary>
        /// pass in -1 for currentSiteId to get all sites
        /// </summary>
        /// <param name="currentSiteId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalPages"></param>
        /// <returns></returns>
        public List<ISiteInfo> GetPageOtherSites(
            int currentSiteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return repo.GetPageOtherSites(currentSiteId, pageNumber, pageSize, out totalPages);
        }

        public List<ISiteHost> GetAllHosts()
        {
            return repo.GetAllHosts();
        }

        public List<ISiteHost> GetPageHosts(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return repo.GetPageHosts(pageNumber, pageSize, out totalPages);
        }

        public List<ISiteHost> GetSiteHosts(int siteId)
        {
            return repo.GetSiteHosts(siteId);
        }

        // TODO make it return either a bool or an instance of ISiteHost
        public void AddHost(Guid siteGuid, int siteId, string hostName)
        {
            repo.AddHost(siteGuid, siteId, hostName);
        }

        public void DeleteHost(int hostId)
        {
            repo.DeleteHost(hostId);
        }

        public int GetSiteIdByHostName(string hostName)
        {
            return repo.GetSiteIdByHostName(hostName);
        }

        public List<SiteFolder> GetBySite(Guid siteGuid)
        {
            return repo.GetBySite(siteGuid);

        }

        public List<SiteFolder> GetAllSiteFolders()
        {
            return repo.GetAllSiteFolders();
        }

        public List<SiteFolder> GetPageSiteFolders(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return repo.GetPageSiteFolders(
                pageNumber,
                pageSize,
                out totalPages);
        }

        public void Save(SiteFolder siteFolder)
        {
            repo.Save(siteFolder);
        }

        public bool DeleteFolder(Guid guid)
        {
            return repo.DeleteFolder(guid);
        }

        public int GetSiteIdByFolder(string folderName)
        {
            return repo.GetSiteIdByFolder(folderName);
        }

        public Guid GetSiteGuidByFolder(string folderName)
        {
            return repo.GetSiteGuidByFolder(folderName);
        }

        public bool FolderExists(string folderName)
        {
            return repo.FolderExists(folderName);
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
