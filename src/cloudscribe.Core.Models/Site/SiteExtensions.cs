// Author:					Joe Audette
// Created:					2015-04-06
// Last Modified:			2015-04-06
// 

using cloudscribe.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.Site
{
    public static class SiteExtensions
    {
        public static async Task<bool> EnsureSiteFolder(this ISiteSettings site, ISiteRepository repo)
        {
            bool folderExists = await repo.FolderExists(site.SiteFolderName);

            if(!folderExists)
            {
                List<SiteFolder> siteFolders = await repo.GetSiteFoldersBySite(site.SiteGuid);
                //delete any existing folders before creating a new one
                foreach(SiteFolder f in siteFolders)
                {
                    bool deleted = await repo.DeleteFolder(f.Guid);
                }

                //ensure the current folder mapping
                SiteFolder folder = new SiteFolder();
                folder.FolderName = site.SiteFolderName;
                folder.SiteGuid = site.SiteGuid;
                folderExists = await repo.Save(folder);
            }
            
            return folderExists;
        }

    }
}
