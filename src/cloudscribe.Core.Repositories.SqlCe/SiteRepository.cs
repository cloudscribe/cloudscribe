// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2015-01-16
// 


using cloudscribe.Core.Models;
using cloudscribe.Core.Models.DataExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.SqlCe
{
    //disable warning about not really being async
    // we know it is not, and for SqlCe there is probably no benefit to making it really async
#pragma warning disable 1998

    public sealed class SiteRepository : ISiteRepository
    {
        public SiteRepository()
        { }

        #region ISiteRepository

        public async Task<bool> Save(ISiteSettings site)
        {
            int passedInSiteId = site.SiteId;
            bool result = false;

            if (site.SiteId == -1) // new site
            {
                site.SiteGuid = Guid.NewGuid(); 

                site.SiteId = DBSiteSettings.Create(
                    site.SiteGuid,
                    site.SiteName,
                    site.Skin,
                    site.Logo,
                    site.Icon,
                    site.AllowNewRegistration,
                    site.AllowUserSkins,
                    site.AllowPageSkins,
                    site.AllowHideMenuOnPages, 
                    site.UseSecureRegistration,
                    site.UseSslOnAllPages,
                    string.Empty, // legacy defaultPageKeywords
                    string.Empty, // legacy defaultPageDescription
                    string.Empty, // legacy defaultPageEncoding
                    string.Empty, // legacy defaultAdditionalMetaTag
                    site.IsServerAdminSite,
                    site.UseLdapAuth,
                    site.AutoCreateLdapUserOnFirstLogin,
                    site.SiteLdapSettings.Server,
                    site.SiteLdapSettings.Port,
                    site.SiteLdapSettings.Domain,
                    site.SiteLdapSettings.RootDN,
                    site.SiteLdapSettings.UserDNKey,
                    site.AllowUserFullNameChange,
                    site.UseEmailForLogin,
                    site.ReallyDeleteUsers,
                    string.Empty, // legacy site.EditorSkin,
                    string.Empty, // legacy site.DefaultFriendlyUrlPatternEnum,
                    false, // legacy site.EnableMyPageFeature,
                    site.EditorProviderName,
                    string.Empty, // legacy site.DatePickerProvider,
                    site.CaptchaProvider,
                    site.RecaptchaPrivateKey,
                    site.RecaptchaPublicKey,
                    site.WordpressApiKey,
                    site.WindowsLiveAppId,
                    site.WindowsLiveKey,
                    site.AllowOpenIdAuth,
                    false, //legacy site.AllowWindowsLiveAuth,
                    site.GmapApiKey,
                    site.AddThisDotComUsername, //apiKeyExtra2
                    site.GoogleAnalyticsAccountCode, //apiKeyExtra2
                    string.Empty, //legacy apiKeyExtra3
                    site.SiteFolderName, // legacy apiKeyExtra4
                    site.PreferredHostName, // legacy apiKeyExtra5
                    site.DisableDbAuth);

                result = site.SiteId > -1;
    
            }
            else
            {
                result = DBSiteSettings.Update(
                    site.SiteId,
                    site.SiteName,
                    site.Skin,
                    site.Logo,
                    site.Icon,
                    site.AllowNewRegistration,
                    site.AllowUserSkins,
                    site.AllowPageSkins,
                    site.AllowHideMenuOnPages,
                    site.UseSecureRegistration,
                    site.UseSslOnAllPages,
                    string.Empty, // legacy defaultPageKeywords
                    string.Empty, // legacy defaultPageDescription
                    string.Empty, // legacy defaultPageEncoding
                    string.Empty, // legacy defaultAdditionalMetaTag
                    site.IsServerAdminSite,
                    site.UseLdapAuth,
                    site.AutoCreateLdapUserOnFirstLogin,
                    site.SiteLdapSettings.Server,
                    site.SiteLdapSettings.Port,
                    site.SiteLdapSettings.Domain,
                    site.SiteLdapSettings.RootDN,
                    site.SiteLdapSettings.UserDNKey,
                    site.AllowUserFullNameChange,
                    site.UseEmailForLogin,
                    site.ReallyDeleteUsers,
                    string.Empty, // legacy site.EditorSkin,
                    string.Empty, // legacy site.DefaultFriendlyUrlPatternEnum,
                    false, // legacy site.EnableMyPageFeature,
                    site.EditorProviderName,
                    string.Empty, // legacy site.DatePickerProvider,
                    site.CaptchaProvider,
                    site.RecaptchaPrivateKey,
                    site.RecaptchaPublicKey,
                    site.WordpressApiKey,
                    site.WindowsLiveAppId,
                    site.WindowsLiveKey,
                    site.AllowOpenIdAuth,
                    false, //legacy site.AllowWindowsLiveAuth,
                    site.GmapApiKey,
                    site.AddThisDotComUsername, //apiKeyExtra2
                    site.GoogleAnalyticsAccountCode, //apiKeyExtra2
                    string.Empty, //legacy apiKeyExtra3
                    site.SiteFolderName, // legacy apiKeyExtra4
                    site.PreferredHostName, // legacy apiKeyExtra5
                    site.DisableDbAuth);

            }

            if (!result) { return result; }

            // settings below stored as key value pairs in mp_SiteSettingsEx


            DBSiteSettingsEx.EnsureSettings();

            DataTable expandoProperties = GetExpandoProperties(passedInSiteId); //-1 on new sites to get the default values
            
            // update a local data table of expando properties if the value changed and mark the row dirty
            site.SetExpandoSettings(expandoProperties);
            // finally update the database only with properties in the table marked as dirty
            SaveExpandoProperties(site.SiteId, site.SiteGuid, expandoProperties);

            return result;

        }

        
        public async Task<ISiteSettings> Fetch(int siteId)
        {
            SiteSettings site = new SiteSettings();

            using (IDataReader reader = DBSiteSettings.GetSite(siteId))
            {
                if (reader.Read())
                {
                    site.LoadFromReader(reader);
                }
            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            DataTable expandoProperties = GetExpandoProperties(site.SiteId);
            site.LoadExpandoSettings(expandoProperties);

            return site;

        }

        public ISiteSettings FetchNonAsync(int siteId)
        {
            SiteSettings site = new SiteSettings();

            using (IDataReader reader = DBSiteSettings.GetSite(siteId))
            {
                if (reader.Read())
                {
                    site.LoadFromReader(reader);
                }
            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            DataTable expandoProperties = GetExpandoProperties(site.SiteId);
            site.LoadExpandoSettings(expandoProperties);

            return site;

        }

        public async Task<ISiteSettings> Fetch(Guid siteGuid)
        {
            SiteSettings site = new SiteSettings();

            using (IDataReader reader = DBSiteSettings.GetSite(siteGuid))
            {
                if (reader.Read())
                {
                    site.LoadFromReader(reader);
                }

            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            DataTable expandoProperties = GetExpandoProperties(site.SiteId);
            site.LoadExpandoSettings(expandoProperties);

            return site;


        }

        public async Task<ISiteSettings> Fetch(string hostName)
        {
            SiteSettings site = new SiteSettings();

            using (IDataReader reader = DBSiteSettings.GetSite(hostName))
            {
                if (reader.Read())
                {
                    site.LoadFromReader(reader);
                }
            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            DataTable expandoProperties = GetExpandoProperties(site.SiteId);
            site.LoadExpandoSettings(expandoProperties);

            return site;
        }

        public ISiteSettings FetchNonAsync(string hostName)
        {
            SiteSettings site = new SiteSettings();

            using (IDataReader reader = DBSiteSettings.GetSite(hostName))
            {
                if (reader.Read())
                {
                    site.LoadFromReader(reader);
                }
            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            DataTable expandoProperties = GetExpandoProperties(site.SiteId);
            site.LoadExpandoSettings(expandoProperties);

            return site;
        }

        
        public async Task<bool> Delete(int siteId)
        {
            return DBSiteSettings.Delete(siteId);
        }


        
        public async Task<int> GetCount()
        {
            return DBSiteSettings.CountOtherSites(-1);
        }

        public async Task<List<ISiteInfo>> GetList()
        {
            List<ISiteInfo> sites = new List<ISiteInfo>();
            using (IDataReader reader = DBSiteSettings.GetSiteList())
            {
                while (reader.Read())
                {
                    SiteInfo site = new SiteInfo();
                    site.LoadFromReader(reader);
                    sites.Add(site);
                }

            }

            return sites;
        }

        public async Task<int> CountOtherSites(int currentSiteId)
        {
            return DBSiteSettings.CountOtherSites(currentSiteId);
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
            List<ISiteInfo> sites = new List<ISiteInfo>();
            using (IDataReader reader = DBSiteSettings.GetPageOfOtherSites(currentSiteId, pageNumber, pageSize))
            {
                while (reader.Read())
                {
                    SiteInfo site = new SiteInfo();
                    site.LoadFromReader(reader);
                    sites.Add(site);
                }
            }

            return sites;
        }

        public async Task<List<ISiteHost>> GetAllHosts()
        {
            List<ISiteHost> hosts = new List<ISiteHost>();
            using (IDataReader reader = DBSiteSettings.GetAllHosts())
            {
                while (reader.Read())
                {
                    SiteHost host = new SiteHost();
                    host.LoadFromReader(reader);
                    hosts.Add(host);
                }

            }

            return hosts;
        }

        public List<ISiteHost> GetAllHostsNonAsync()
        {
            List<ISiteHost> hosts = new List<ISiteHost>();
            using (IDataReader reader = DBSiteSettings.GetAllHosts())
            {
                while (reader.Read())
                {
                    SiteHost host = new SiteHost();
                    host.LoadFromReader(reader);
                    hosts.Add(host);
                }

            }

            return hosts;
        }

        public async Task<int> GetHostCount()
        {
            return DBSiteSettings.GetHostCount();
        }

        public async Task<List<ISiteHost>> GetPageHosts(
            int pageNumber,
            int pageSize)
        {
            List<ISiteHost> hosts = new List<ISiteHost>();
            using (IDataReader reader = DBSiteSettings.GetPageHosts(pageNumber, pageSize))
            {
                while (reader.Read())
                {
                    SiteHost host = new SiteHost();
                    host.LoadFromReader(reader);
                    hosts.Add(host);
                }

            }

            return hosts;
        }

        public async Task<List<ISiteHost>> GetSiteHosts(int siteId)
        {
            List<ISiteHost> hosts = new List<ISiteHost>();
            using (IDataReader reader = DBSiteSettings.GetHostList(siteId))
            {
                while (reader.Read())
                {
                    SiteHost host = new SiteHost();
                    host.LoadFromReader(reader);
                    hosts.Add(host);
                }

            }

            return hosts;
        }

        public async Task<ISiteHost> GetSiteHost(string hostName)
        {
            using (IDataReader reader = DBSiteSettings.GetHost(hostName))
            {
                while (reader.Read())
                {
                    SiteHost host = new SiteHost();
                    host.LoadFromReader(reader);
                    return host;
                }

            }

            return null;
        }

        public async Task<bool> AddHost(Guid siteGuid, int siteId, string hostName)
        {
            return DBSiteSettings.AddHost(siteGuid, siteId, hostName);
        }

        public async Task<bool> DeleteHost(int hostId)
        {
            return DBSiteSettings.DeleteHost(hostId);
        }

        public async Task<int> GetSiteIdByHostName(string hostName)
        {
            return DBSiteSettings.GetSiteIdByHostName(hostName);
        }

        public async Task<List<SiteFolder>> GetSiteFoldersBySite(Guid siteGuid)
        {
            List<SiteFolder> siteFolderList
                = new List<SiteFolder>();

            using (IDataReader reader = DBSiteFolder.GetBySite(siteGuid))
            {
                while (reader.Read())
                {
                    SiteFolder siteFolder = new SiteFolder();
                    siteFolder.LoadFromReader(reader);
                    siteFolderList.Add(siteFolder);
                }
            }

            return siteFolderList;

        }

        public async Task<SiteFolder> GetSiteFolder(string folderName)
        {
            using (IDataReader reader = DBSiteFolder.GetOne(folderName))
            {
                if (reader.Read())
                {
                    SiteFolder siteFolder = new SiteFolder();
                    siteFolder.LoadFromReader(reader);
                    return siteFolder;
                }
            }

            return null;
        }

        public async Task<List<SiteFolder>> GetAllSiteFolders()
        {
            List<SiteFolder> siteFolderList
                = new List<SiteFolder>();

            using (IDataReader reader = DBSiteFolder.GetAll())
            {
                while (reader.Read())
                {
                    SiteFolder siteFolder = new SiteFolder();
                    siteFolder.LoadFromReader(reader);
                    siteFolderList.Add(siteFolder);
                }
            }

            return siteFolderList;

        }

        public List<SiteFolder> GetAllSiteFoldersNonAsync()
        {
            List<SiteFolder> siteFolderList
                = new List<SiteFolder>();

            using (IDataReader reader = DBSiteFolder.GetAll())
            {
                while (reader.Read())
                {
                    SiteFolder siteFolder = new SiteFolder();
                    siteFolder.LoadFromReader(reader);
                    siteFolderList.Add(siteFolder);
                }
            }

            return siteFolderList;

        }

        public async Task<int> GetFolderCount()
        {
            return DBSiteFolder.GetFolderCount();
        }

        public async Task<List<SiteFolder>> GetPageSiteFolders(
            int pageNumber,
            int pageSize)
        {
            List<SiteFolder> siteFolderList
                = new List<SiteFolder>();

            using (DbDataReader reader = DBSiteFolder.GetPage(pageNumber, pageSize))
            {
                while (reader.Read())
                {
                    SiteFolder siteFolder = new SiteFolder();
                    siteFolder.LoadFromReader(reader);
                    siteFolderList.Add(siteFolder);
                }
            }

            return siteFolderList;

        }
       

        public async Task<bool> Save(SiteFolder siteFolder)
        {
            if (siteFolder == null) { return false; }

            if (siteFolder.Guid == Guid.Empty)
            {
                siteFolder.Guid = Guid.NewGuid();

                return DBSiteFolder.Add(
                    siteFolder.Guid,
                    siteFolder.SiteGuid,
                    siteFolder.FolderName);
            }
            else
            {
                return DBSiteFolder.Update(
                    siteFolder.Guid,
                    siteFolder.SiteGuid,
                    siteFolder.FolderName);

            }
        }

        public async Task<bool> DeleteFolder(Guid guid)
        {
            return DBSiteFolder.Delete(guid);
        }

        public async Task<int> GetSiteIdByFolder(string folderName)
        {
            return DBSiteSettings.GetSiteIdByFolder(folderName);
        }

        public int GetSiteIdByFolderNonAsync(string folderName)
        {
            return DBSiteSettings.GetSiteIdByFolder(folderName);
        }

        public async Task<Guid> GetSiteGuidByFolder(string folderName)
        {
            return DBSiteFolder.GetSiteGuid(folderName);
        }

        public async Task<bool> FolderExists(string folderName)
        {
            return DBSiteFolder.Exists(folderName);
        }

        


        #endregion

        #region IDisposable

        public void Dispose()
        {

        }

        #endregion

        #region private methods


        private void SaveExpandoProperties(int siteId, Guid siteGuid, DataTable exapandoProperties)
        {
            // process the dirty rows as updates

            foreach (DataRow row in exapandoProperties.Rows)
            {
                bool isDirty = Convert.ToBoolean(row["IsDirty"]);
                if (isDirty)
                {
                    DBSiteSettingsEx.SaveExpandoProperty(
                        siteId,
                        siteGuid,
                        row["GroupName"].ToString(),
                        row["KeyName"].ToString(),
                        row["KeyValue"].ToString());

                }

            }

        }

        
        private static DataTable GetExpandoProperties(int siteId)
        {
            if (siteId == -1) { return GetDefaultExpandoProperties(); } //new site

            DataTable dataTable = ModelDataExtensions.CreateExpandoTable();
           

            using (IDataReader reader = DBSiteSettingsEx.GetSiteSettingsExList(siteId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["SiteID"] = reader["SiteID"];
                    row["KeyName"] = reader["KeyName"];
                    row["KeyValue"] = reader["KeyValue"];
                    row["GroupName"] = reader["GroupName"];

                    row["IsDirty"] = false;

                    dataTable.Rows.Add(row);

                }
            }

            return dataTable;
        }

        private static DataTable GetDefaultExpandoProperties()
        {

            DataTable dataTable = ModelDataExtensions.CreateExpandoTable();

            using (IDataReader reader = DBSiteSettingsEx.GetDefaultExpandoSettings())
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["SiteID"] = -1;
                    row["KeyName"] = reader["KeyName"];
                    row["KeyValue"] = reader["DefaultValue"];
                    row["GroupName"] = reader["GroupName"];

                    row["IsDirty"] = false;

                    dataTable.Rows.Add(row);

                }
            }


            return dataTable;
        }

        


        #endregion
    }

#pragma warning restore 1998

}
