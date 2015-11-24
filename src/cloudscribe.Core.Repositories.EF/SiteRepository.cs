// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2015-11-24
// 


using cloudscribe.Core.Models;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.EF
{

    public class SiteRepository : ISiteRepository
    {
        public SiteRepository(CoreDbContext dbContext)
        {

            this.dbContext = dbContext;
        }

        private CoreDbContext dbContext;

        public async Task<bool> Save(ISiteSettings site)
        {
            if(site == null) { return false; }

            SiteSettings siteSettings = SiteSettings.FromISiteSettings(site); 
            dbContext.Sites.Add(siteSettings);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<ISiteSettings> Fetch(int siteId)
        {
            SiteSettings item
                = await dbContext.Sites.SingleOrDefaultAsync(x => x.SiteId == siteId);

            return item;
        }

        public ISiteSettings FetchNonAsync(int siteId)
        {
            SiteSettings item
                = dbContext.Sites.SingleOrDefault(x => x.SiteId == siteId);

            return item;
        }

        public async Task<ISiteSettings> Fetch(Guid siteGuid)
        {
            SiteSettings item
                = await dbContext.Sites.SingleOrDefaultAsync(x => x.SiteGuid == siteGuid);

            return item;
        }

        public ISiteSettings FetchNonAsync(Guid siteGuid)
        {
            SiteSettings item
                = dbContext.Sites.SingleOrDefault(x => x.SiteGuid == siteGuid);

            return item;
        }

        public async Task<ISiteSettings> Fetch(string hostName)
        {
            SiteHost host = await dbContext.SiteHosts.FirstOrDefaultAsync(x => x.HostName == hostName);
            if(host == null)
            {
                var query = from s in dbContext.Sites
                            .Take(1)
                            orderby s.SiteId ascending
                            select s;

                return await query.FirstAsync<SiteSettings>();
            }
            
            return await dbContext.Sites.FirstOrDefaultAsync(x => x.SiteId == host.SiteId);


        }

        public ISiteSettings FetchNonAsync(string hostName)
        {
            SiteHost host = dbContext.SiteHosts.FirstOrDefault(x => x.HostName == hostName);
            if (host == null)
            {
                var query = from s in dbContext.Sites
                            .Take(1)
                            orderby s.SiteId ascending
                            select s;

                return query.First<SiteSettings>();
            }

            return dbContext.Sites.FirstOrDefault(x => x.SiteId == host.SiteId);

        }

        public async Task<bool> Delete(int siteId)
        {
            var result = false;
            var itemToRemove = await dbContext.Sites.SingleOrDefaultAsync(x => x.SiteId == siteId);
            if (itemToRemove != null)
            {
                dbContext.Sites.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;

        }


        public async Task<int> GetCount()
        {
            return await dbContext.Sites.CountAsync<SiteSettings>();
        }

        public async Task<List<ISiteInfo>> GetList()
        {
            var query = from x in dbContext.Sites
                        orderby x.SiteName ascending
                        select new SiteInfo
                        { SiteId = x.SiteId,
                          SiteGuid = x.SiteGuid,
                          IsServerAdminSite = x.IsServerAdminSite,
                          PreferredHostName = x.PreferredHostName,
                          SiteFolderName = x.SiteFolderName,
                          SiteName = x.SiteName
                        }
                        ;

            var items = await query.ToListAsync<ISiteInfo>();

            //List<ISiteInfo> result = new List<ISiteInfo>(items); // will this work?

            return items;
        }

        public async Task<int> CountOtherSites(int currentSiteId)
        {
            return await dbContext.Sites.CountAsync<SiteSettings>(x => x.SiteId != currentSiteId);
        }

        public async Task<List<ISiteInfo>> GetPageOtherSites(
            int currentSiteId,
            int pageNumber,
            int pageSize)
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var query = from x in dbContext.Sites 
                        .Skip(offset)
                        .Take(pageSize)
                        where (x.SiteId != currentSiteId)
                        orderby x.SiteName ascending
                        select new SiteInfo
                        {
                            SiteId = x.SiteId,
                            SiteGuid = x.SiteGuid,
                            IsServerAdminSite = x.IsServerAdminSite,
                            PreferredHostName = x.PreferredHostName,
                            SiteFolderName = x.SiteFolderName,
                            SiteName = x.SiteName
                        }
                        ;

            var items = await query.ToListAsync<ISiteInfo>();

            return items;
        }

        public async Task<List<ISiteHost>> GetAllHosts()
        {
            var query = from x in dbContext.SiteHosts
                        orderby x.HostName ascending
                        select x;

            var items = await query.ToListAsync<ISiteHost>();

            //List<ISiteHost> result = new List<ISiteHost>(items); // will this work?

            return items;
        }

        public List<ISiteHost> GetAllHostsNonAsync()
        {
            var query = from x in dbContext.SiteHosts
                        orderby x.HostName ascending
                        select x;

            var items = query.ToList<ISiteHost>();

            //List<ISiteHost> result = new List<ISiteHost>(items); 

            return items;
        }

        public async Task<int> GetHostCount()
        {
            return await dbContext.SiteHosts.CountAsync<SiteHost>();
        }

        public async Task<List<ISiteHost>> GetPageHosts(
            int pageNumber,
            int pageSize)
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var query = from x in dbContext.SiteHosts
                        .Skip(offset)
                        .Take(pageSize)
                        orderby x.HostName ascending
                        select x
                        ;

            var items = await query.ToListAsync<ISiteHost>();

            return items;
        }

        public async Task<List<ISiteHost>> GetSiteHosts(int siteId)
        {
            var query = from x in dbContext.SiteHosts
                        where x.SiteId == siteId
                        orderby x.HostName ascending
                        select x
                        ;

            var items = await query.ToListAsync<ISiteHost>();

            return items;
        }

        public async Task<ISiteHost> GetSiteHost(string hostName)
        {
            var query = from x in dbContext.SiteHosts
                        where x.HostName == hostName
                        orderby x.HostName ascending
                        select x
                        ;

            return await query.FirstOrDefaultAsync<SiteHost>();
            
        }

        public async Task<bool> AddHost(Guid siteGuid, int siteId, string hostName)
        {
            SiteHost host = new SiteHost();
            host.SiteGuid = siteGuid;
            host.SiteId = siteId;
            host.HostName = hostName;

            dbContext.SiteHosts.Add(host);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public async Task<bool> DeleteHost(int hostId)
        {
            var result = false;
            var itemToRemove = await dbContext.SiteHosts.FirstOrDefaultAsync(x => x.HostId == hostId);
            if (itemToRemove != null)
            {
                dbContext.SiteHosts.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;

        }

        public async Task<int> GetSiteIdByHostName(string hostName)
        {
            var query = from x in dbContext.SiteHosts
                        where x.HostName == hostName
                        orderby x.HostName ascending
                        select x
                        ;

            var host = await query.FirstOrDefaultAsync<SiteHost>();

            if(host != null) { return host.SiteId; }

            return -1; // not found
        }

        public async Task<List<SiteFolder>> GetSiteFoldersBySite(Guid siteGuid)
        {
            var query = from x in dbContext.SiteFolders
                        where x.SiteGuid == siteGuid
                        orderby x.FolderName ascending
                        select x
                        ;

            var items = await query.ToListAsync<SiteFolder>();

            return items;

        }

        public async Task<List<SiteFolder>> GetAllSiteFolders()
        {
            var query = from x in dbContext.SiteFolders
                        orderby x.FolderName ascending
                        select x;

            var items = await query.ToListAsync<SiteFolder>();
            
            return items;

        }

        public List<SiteFolder> GetAllSiteFoldersNonAsync()
        {
            var query = from x in dbContext.SiteFolders
                        orderby x.FolderName ascending
                        select x;

            var items = query.ToList<SiteFolder>();

            return items;
        }

        public async Task<int> GetFolderCount()
        {
            return await dbContext.SiteFolders.CountAsync<SiteFolder>();
        }

        public async Task<List<SiteFolder>> GetPageSiteFolders(
            int pageNumber,
            int pageSize)
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var query = from x in dbContext.SiteFolders
                        .Skip(offset)
                        .Take(pageSize)
                        orderby x.FolderName ascending
                        select x
                        ;

            var items = await query.ToListAsync<SiteFolder>();

            return items;

        }

        public async Task<SiteFolder> GetSiteFolder(string folderName)
        {
            var query = from x in dbContext.SiteFolders
                        where x.FolderName == folderName
                        orderby x.FolderName ascending
                        select x
                        ;

            return await query.SingleOrDefaultAsync<SiteFolder>();

        }

        public async Task<bool> Save(SiteFolder siteFolder)
        {
            if (siteFolder == null) { return false; }

            dbContext.SiteFolders.Add(siteFolder);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public async Task<bool> DeleteFolder(Guid guid)
        {
            var result = false;
            var itemToRemove = await dbContext.SiteFolders.FirstOrDefaultAsync(x => x.Guid == guid);
            if (itemToRemove != null)
            {
                dbContext.SiteFolders.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;
        }

        public async Task<int> GetSiteIdByFolder(string folderName)
        {
            var query = from x in dbContext.Sites
                        join y in dbContext.SiteFolders
                        on x.SiteGuid equals y.SiteGuid
                        where y.FolderName == folderName
                        select x.SiteId
                        ;

            return await query.DefaultIfEmpty(-1).FirstOrDefaultAsync<int>();
            
            //return -1; // not found

        }

        public int GetSiteIdByFolderNonAsync(string folderName)
        {
            var query = from x in dbContext.Sites
                        join y in dbContext.SiteFolders
                        on x.SiteGuid equals y.SiteGuid
                        where y.FolderName == folderName
                        select x.SiteId
                        ;

            return query.DefaultIfEmpty(-1).FirstOrDefault<int>();
        }

        public async Task<Guid> GetSiteGuidByFolder(string folderName)
        {
            var query = from x in dbContext.SiteFolders
                        where x.FolderName == folderName 
                        select x
                        ;

            SiteFolder folder = await query.SingleOrDefaultAsync<SiteFolder>();
            if(folder == null) { return Guid.Empty; }
            return folder.SiteGuid;
        }

        public async Task<bool> FolderExists(string folderName)
        {
            Guid found = await GetSiteGuidByFolder(folderName);
            if(found == Guid.Empty) { return false; }
            return true;
        }


        #region IDisposable

        public void Dispose()
        {
            //TODO: ?
        }

        #endregion

    }
}
