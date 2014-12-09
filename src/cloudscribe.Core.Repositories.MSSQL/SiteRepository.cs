// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2014-12-09
// 


using cloudscribe.Core.Models;
using cloudscribe.Core.Models.DataExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace cloudscribe.Core.Repositories.MSSQL
{

    public sealed class SiteRepository : ISiteRepository
    {
        public SiteRepository()
        { }

        #region ISiteRepository

        public void Save(ISiteSettings site)
        {
            int passedInSiteId = site.SiteId;
            

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
    
            }
            else
            {
                DBSiteSettings.Update(
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

            // settings below stored as key value pairs in mp_SiteSettingsEx


            DBSiteSettingsEx.EnsureSettings();

            DataTable expandoProperties = GetExpandoProperties(passedInSiteId); //-1 on new sites to get the default values
            
            // update a local data table of expando properties if the value changed and mark the row dirty
            site.SetExpandoSettings(expandoProperties);
            // finally update the database only with properties in the table marked as dirty
            SaveExpandoProperties(site.SiteId, site.SiteGuid, expandoProperties);
            

        }

        
        public ISiteSettings Fetch(int siteId)
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

        public ISiteSettings Fetch(Guid siteGuid)
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

        public ISiteSettings Fetch(string hostName)
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

        
        public bool Delete(int siteId)
        {
            return DBSiteSettings.Delete(siteId);
        }


        
        public int GetCount()
        {
            return DBSiteSettings.CountOtherSites(-1);
        }

        public List<ISiteInfo> GetList()
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
            List<ISiteInfo> sites = new List<ISiteInfo>();
            totalPages = 1;
            using (IDataReader reader = DBSiteSettings.GetPageOfOtherSites(currentSiteId, pageNumber, pageSize, out totalPages))
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

        public List<ISiteHost> GetAllHosts()
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

        public List<ISiteHost> GetPageHosts(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            List<ISiteHost> hosts = new List<ISiteHost>();
            using (IDataReader reader = DBSiteSettings.GetPageHosts(pageNumber, pageSize, out totalPages))
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

        public List<ISiteHost> GetSiteHosts(int siteId)
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

        // TODO make it return either a bool or an instance of ISiteHost
        public void AddHost(Guid siteGuid, int siteId, string hostName)
        {
            DBSiteSettings.AddHost(siteGuid, siteId, hostName);
        }

        public void DeleteHost(int hostId)
        {
            DBSiteSettings.DeleteHost(hostId);
        }

        public int GetSiteIdByHostName(string hostName)
        {
            return DBSiteSettings.GetSiteIdByHostName(hostName);
        }

        public List<SiteFolder> GetBySite(Guid siteGuid)
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

        public List<SiteFolder> GetAllSiteFolders()
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

        public List<SiteFolder> GetPageSiteFolders(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            List<SiteFolder> siteFolderList
                = new List<SiteFolder>();

            using (IDataReader reader = DBSiteFolder.GetPage(pageNumber, pageSize, out totalPages))
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

       

        public void Save(SiteFolder siteFolder)
        {
            if (siteFolder == null) { return; }

            if (siteFolder.Guid == Guid.Empty)
            {
                siteFolder.Guid = Guid.NewGuid();

                DBSiteFolder.Add(
                    siteFolder.Guid,
                    siteFolder.SiteGuid,
                    siteFolder.FolderName);
            }
            else
            {
                DBSiteFolder.Update(
                    siteFolder.Guid,
                    siteFolder.SiteGuid,
                    siteFolder.FolderName);

            }
        }

        public bool DeleteFolder(Guid guid)
        {
            return DBSiteFolder.Delete(guid);
        }

        public int GetSiteIdByFolder(string folderName)
        {
            return DBSiteSettings.GetSiteIdByFolder(folderName);
        }

        public Guid GetSiteGuidByFolder(string folderName)
        {
            return DBSiteFolder.GetSiteGuid(folderName);
        }

        public bool FolderExists(string folderName)
        {
            return DBSiteFolder.Exists(folderName);
        }

        //TODO: this is not part of ISiteSettings
        // this method should be moved to AppSettings class
        public bool IsAllowedFolder(string folderName)
        {
            bool result = true;

            //TODO: wrap in AppSettings class to avoid dependency on System.Configuration here

            //if (ConfigurationManager.AppSettings["DisallowedVirtualFolderNames"] != null)
            //{
            //    string[] disallowedNames
            //        = ConfigurationManager.AppSettings["DisallowedVirtualFolderNames"].Split(new char[] { ';' });

            //    foreach (string disallowedName in disallowedNames)
            //    {
            //        if (string.Equals(folderName, disallowedName, StringComparison.InvariantCultureIgnoreCase)) result = false;
            //    }

            //}


            return result;

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

}
