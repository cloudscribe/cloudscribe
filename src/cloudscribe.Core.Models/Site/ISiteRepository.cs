// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2014-08-17
// 

using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Models
{
    public interface ISiteRepository : IDisposable
    {
        ISiteSettings Fetch(int siteId);
        ISiteSettings Fetch(Guid siteGuid);
        ISiteSettings Fetch(string hostName);
        void Save(ISiteSettings site);
        bool Delete(int siteId);
        int GetCount();
        List<ISiteInfo> GetList();
        List<ISiteInfo> GetPageOtherSites(
            int currentSiteId,
            int pageNumber,
            int pageSize,
            out int totalPages);

        List<ISiteHost> GetSiteHosts(int siteId);
        List<ISiteHost> GetAllHosts();
        List<ISiteHost> GetPageHosts(
            int pageNumber,
            int pageSize,
            out int totalPages);
        void AddHost(Guid siteGuid, int siteId, string hostName);
        void DeleteHost(int hostId);
        int GetSiteIdByHostName(string hostName);

        List<SiteFolder> GetBySite(Guid siteGuid);
        List<SiteFolder> GetAllSiteFolders();
        List<SiteFolder> GetPageSiteFolders(
            int pageNumber,
            int pageSize,
            out int totalPages);
        void Save(SiteFolder siteFolder);
        bool DeleteFolder(Guid guid);
        int GetSiteIdByFolder(string folderName);
        Guid GetSiteGuidByFolder(string folderName);
        bool FolderExists(string folderName);

    }
}
