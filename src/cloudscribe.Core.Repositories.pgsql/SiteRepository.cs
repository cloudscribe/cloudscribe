// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2015-11-18
// 


using cloudscribe.Core.Models;
using cloudscribe.Core.Models.DataExtensions;
using cloudscribe.DbHelpers.pgsql;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.pgsql
{

    public sealed class SiteRepository : ISiteRepository
    {
        public SiteRepository(
            IOptions<PostgreSqlConnectionOptions> configuration,
            ILoggerFactory loggerFactory)
        {
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }

            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(typeof(SiteRepository).FullName);

            readConnectionString = configuration.Value.ReadConnectionString;
            writeConnectionString = configuration.Value.WriteConnectionString;

            dbSiteSettings = new DBSiteSettings(readConnectionString, writeConnectionString, logFactory);
            dbSiteSettingsEx = new DBSiteSettingsEx(readConnectionString, writeConnectionString, logFactory);
            dbSiteFolder = new DBSiteFolder(readConnectionString, writeConnectionString, logFactory);

        }

        private ILoggerFactory logFactory;
        private ILogger log;
        private string readConnectionString;
        private string writeConnectionString;
        private DBSiteSettings dbSiteSettings;
        private DBSiteSettingsEx dbSiteSettingsEx;
        private DBSiteFolder dbSiteFolder;

        #region ISiteRepository

        public async Task<bool> Save(ISiteSettings site)
        {
            int passedInSiteId = site.SiteId;
            bool result = false;

            if (site.SiteId == -1) // new site
            {
                site.SiteGuid = Guid.NewGuid();

                site.SiteId = await dbSiteSettings.Create(
                    site.SiteGuid,
                    site.SiteName,
                    site.Layout,
                    site.AllowNewRegistration,
                    site.UseSecureRegistration,
                    site.UseSslOnAllPages,
                    site.IsServerAdminSite,
                    site.UseLdapAuth,
                    site.AutoCreateLdapUserOnFirstLogin,
                    site.LdapServer,
                    site.LdapPort,
                    site.LdapDomain,
                    site.LdapRootDN,
                    site.LdapUserDNKey,
                    site.AllowUserFullNameChange,
                    site.UseEmailForLogin,
                    site.ReallyDeleteUsers,
                    site.RecaptchaPrivateKey,
                    site.RecaptchaPublicKey,
                    site.ApiKeyExtra1,
                    site.ApiKeyExtra2,
                    site.ApiKeyExtra3,
                    site.ApiKeyExtra4,
                    site.ApiKeyExtra5,
                    site.DisableDbAuth,

                    site.RequiresQuestionAndAnswer,
                    site.MaxInvalidPasswordAttempts,
                    site.PasswordAttemptWindowMinutes,
                    site.MinRequiredPasswordLength,
                    site.MinReqNonAlphaChars,
                    site.DefaultEmailFromAddress,
                    site.AllowDbFallbackWithLdap,
                    site.EmailLdapDbFallback,
                    site.AllowPersistentLogin,
                    site.CaptchaOnLogin,
                    site.CaptchaOnRegistration,
                    site.SiteIsClosed,
                    site.SiteIsClosedMessage,
                    site.PrivacyPolicy,
                    site.TimeZoneId,
                    site.GoogleAnalyticsProfileId,
                    site.CompanyName,
                    site.CompanyStreetAddress,
                    site.CompanyStreetAddress2,
                    site.CompanyRegion,
                    site.CompanyLocality,
                    site.CompanyCountry,
                    site.CompanyPostalCode,
                    site.CompanyPublicEmail,
                    site.CompanyPhone,
                    site.CompanyFax,
                    site.FacebookAppId,
                    site.FacebookAppSecret,
                    site.GoogleClientId,
                    site.GoogleClientSecret,
                    site.TwitterConsumerKey,
                    site.TwitterConsumerSecret,
                    site.MicrosoftClientId,
                    site.MicrosoftClientSecret,
                    site.PreferredHostName,
                    site.SiteFolderName,
                    site.AddThisDotComUsername,
                    site.LoginInfoTop,
                    site.LoginInfoBottom,
                    site.RegistrationAgreement,
                    site.RegistrationPreamble,
                    site.SmtpServer,
                    site.SmtpPort,
                    site.SmtpUser,
                    site.SmtpPassword,
                    site.SmtpPreferredEncoding,
                    site.SmtpRequiresAuth,
                    site.SmtpUseSsl,
                    site.RequireApprovalBeforeLogin

                    );

                result = site.SiteId > -1;

            }
            else
            {
                result = await dbSiteSettings.Update(
                    site.SiteId,
                    site.SiteName,
                    site.Layout,  
                    site.AllowNewRegistration,
                    site.UseSecureRegistration,
                    site.UseSslOnAllPages,
                    site.IsServerAdminSite,
                    site.UseLdapAuth,
                    site.AutoCreateLdapUserOnFirstLogin,
                    site.LdapServer,
                    site.LdapPort,
                    site.LdapDomain,
                    site.LdapRootDN,
                    site.LdapUserDNKey,
                    site.AllowUserFullNameChange,
                    site.UseEmailForLogin,
                    site.ReallyDeleteUsers,
                    site.RecaptchaPrivateKey,
                    site.RecaptchaPublicKey,
                    site.ApiKeyExtra1,
                    site.ApiKeyExtra2,
                    site.ApiKeyExtra3,
                    site.ApiKeyExtra4,
                    site.ApiKeyExtra5,
                    site.DisableDbAuth,

                    site.RequiresQuestionAndAnswer,
                    site.MaxInvalidPasswordAttempts,
                    site.PasswordAttemptWindowMinutes,
                    site.MinRequiredPasswordLength,
                    site.MinReqNonAlphaChars,
                    site.DefaultEmailFromAddress,
                    site.AllowDbFallbackWithLdap,
                    site.EmailLdapDbFallback,
                    site.AllowPersistentLogin,
                    site.CaptchaOnLogin,
                    site.CaptchaOnRegistration,
                    site.SiteIsClosed,
                    site.SiteIsClosedMessage,
                    site.PrivacyPolicy,
                    site.TimeZoneId,
                    site.GoogleAnalyticsProfileId,
                    site.CompanyName,
                    site.CompanyStreetAddress,
                    site.CompanyStreetAddress2,
                    site.CompanyRegion,
                    site.CompanyLocality,
                    site.CompanyCountry,
                    site.CompanyPostalCode,
                    site.CompanyPublicEmail,
                    site.CompanyPhone,
                    site.CompanyFax,
                    site.FacebookAppId,
                    site.FacebookAppSecret,
                    site.GoogleClientId,
                    site.GoogleClientSecret,
                    site.TwitterConsumerKey,
                    site.TwitterConsumerSecret,
                    site.MicrosoftClientId,
                    site.MicrosoftClientSecret,
                    site.PreferredHostName,
                    site.SiteFolderName,
                    site.AddThisDotComUsername,
                    site.LoginInfoTop,
                    site.LoginInfoBottom,
                    site.RegistrationAgreement,
                    site.RegistrationPreamble,
                    site.SmtpServer,
                    site.SmtpPort,
                    site.SmtpUser,
                    site.SmtpPassword,
                    site.SmtpPreferredEncoding,
                    site.SmtpRequiresAuth,
                    site.SmtpUseSsl,
                    site.RequireApprovalBeforeLogin


                    );

            }

            if (!result) { return result; }

            // settings below stored as key value pairs in mp_SiteSettingsEx


            //bool nextResult = await dbSiteSettingsEx.EnsureSettings();

           // List<ExpandoSetting> expandoProperties = GetExpandoProperties(passedInSiteId); //-1 on new sites to get the default values

            // update a local data table of expando properties if the value changed and mark the row dirty
            //site.SetExpandoSettings(expandoProperties);
            // finally update the database only with properties in the table marked as dirty
            //SaveExpandoProperties(site.SiteId, site.SiteGuid, expandoProperties);

            return result;

        }


        public async Task<ISiteSettings> Fetch(int siteId)
        {
            SiteSettings site = new SiteSettings();

            using (DbDataReader reader = await dbSiteSettings.GetSite(siteId))
            {
                if (reader.Read())
                {
                    site.LoadFromReader(reader);
                }
            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            //List<ExpandoSetting> expandoProperties = GetExpandoProperties(site.SiteId);
            //site.LoadExpandoSettings(expandoProperties);

            return site;

        }

        public ISiteSettings FetchNonAsync(int siteId)
        {
            SiteSettings site = new SiteSettings();

            using (DbDataReader reader = dbSiteSettings.GetSiteNonAsync(siteId))
            {
                if (reader.Read())
                {
                    site.LoadFromReader(reader);
                }
            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            //List<ExpandoSetting> expandoProperties = GetExpandoProperties(site.SiteId);
            //site.LoadExpandoSettings(expandoProperties);

            return site;

        }

        public async Task<ISiteSettings> Fetch(Guid siteGuid)
        {
            SiteSettings site = new SiteSettings();

            using (DbDataReader reader = await dbSiteSettings.GetSite(siteGuid))
            {
                if (reader.Read())
                {
                    site.LoadFromReader(reader);
                }

            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            //List<ExpandoSetting> expandoProperties = GetExpandoProperties(site.SiteId);
            //site.LoadExpandoSettings(expandoProperties);

            return site;


        }

        public ISiteSettings FetchNonAsync(Guid siteGuid)
        {
            SiteSettings site = new SiteSettings();

            using (DbDataReader reader = dbSiteSettings.GetSiteNonAsync(siteGuid))
            {
                if (reader.Read())
                {
                    site.LoadFromReader(reader);
                }

            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            //List<ExpandoSetting> expandoProperties = GetExpandoProperties(site.SiteId);
            //site.LoadExpandoSettings(expandoProperties);

            return site;


        }

        public async Task<ISiteSettings> Fetch(string hostName)
        {
            SiteSettings site = new SiteSettings();

            using (DbDataReader reader = await dbSiteSettings.GetSite(hostName))
            {
                if (reader.Read())
                {
                    site.LoadFromReader(reader);
                }
            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            //List<ExpandoSetting> expandoProperties = GetExpandoProperties(site.SiteId);
            //site.LoadExpandoSettings(expandoProperties);

            return site;
        }

        public ISiteSettings FetchNonAsync(string hostName)
        {
            SiteSettings site = new SiteSettings();

            using (DbDataReader reader = dbSiteSettings.GetSiteNonAsync(hostName))
            {
                if (reader.Read())
                {
                    site.LoadFromReader(reader);
                }
            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            //List<ExpandoSetting> expandoProperties = GetExpandoProperties(site.SiteId);
            //site.LoadExpandoSettings(expandoProperties);

            return site;
        }


        public async Task<bool> Delete(int siteId)
        {
            return await dbSiteSettings.Delete(siteId);
        }



        public async Task<int> GetCount()
        {
            return await dbSiteSettings.CountOtherSites(-1);
        }

        public async Task<List<ISiteInfo>> GetList()
        {
            List<ISiteInfo> sites = new List<ISiteInfo>();
            using (DbDataReader reader = await dbSiteSettings.GetSiteList())
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
            return await dbSiteSettings.CountOtherSites(currentSiteId);
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

            using (DbDataReader reader = await dbSiteSettings.GetPageOfOtherSites(currentSiteId, pageNumber, pageSize))
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

        public async Task<ISiteHost> GetSiteHost(string hostName)
        {
            using (DbDataReader reader = await dbSiteSettings.GetHost(hostName))
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

        public async Task<List<ISiteHost>> GetAllHosts()
        {
            List<ISiteHost> hosts = new List<ISiteHost>();
            using (DbDataReader reader = await dbSiteSettings.GetAllHosts())
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
            using (DbDataReader reader = dbSiteSettings.GetAllHostsNonAsync())
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
            return await dbSiteSettings.GetHostCount();
        }

        public async Task<List<ISiteHost>> GetPageHosts(
            int pageNumber,
            int pageSize)
        {
            List<ISiteHost> hosts = new List<ISiteHost>();
            using (DbDataReader reader = await dbSiteSettings.GetPageHosts(pageNumber, pageSize))
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
            using (DbDataReader reader = await dbSiteSettings.GetHostList(siteId))
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


        public async Task<bool> AddHost(Guid siteGuid, int siteId, string hostName)
        {
            return await dbSiteSettings.AddHost(siteGuid, siteId, hostName);
        }

        public async Task<bool> DeleteHost(int hostId)
        {
            return await dbSiteSettings.DeleteHost(hostId);
        }

        public async Task<int> GetSiteIdByHostName(string hostName)
        {
            return await dbSiteSettings.GetSiteIdByHostName(hostName);
        }

        public async Task<List<SiteFolder>> GetSiteFoldersBySite(Guid siteGuid)
        {
            List<SiteFolder> siteFolderList
                = new List<SiteFolder>();

            using (DbDataReader reader = await dbSiteFolder.GetBySite(siteGuid))
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

        public async Task<List<SiteFolder>> GetAllSiteFolders()
        {
            List<SiteFolder> siteFolderList
                = new List<SiteFolder>();

            using (DbDataReader reader = await dbSiteFolder.GetAll())
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

            using (DbDataReader reader = dbSiteFolder.GetAllNonAsync())
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
            using (DbDataReader reader = await dbSiteFolder.GetOne(folderName))
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

        public async Task<int> GetFolderCount()
        {
            return await dbSiteFolder.GetFolderCount();
        }

        public async Task<List<SiteFolder>> GetPageSiteFolders(
            int pageNumber,
            int pageSize)
        {
            List<SiteFolder> siteFolderList
                = new List<SiteFolder>();

            using (DbDataReader reader = await dbSiteFolder.GetPage(pageNumber, pageSize))
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

                return await dbSiteFolder.Add(
                    siteFolder.Guid,
                    siteFolder.SiteGuid,
                    siteFolder.FolderName);
            }
            else
            {
                return await dbSiteFolder.Update(
                    siteFolder.Guid,
                    siteFolder.SiteGuid,
                    siteFolder.FolderName);

            }
        }

        public async Task<bool> DeleteFolder(Guid guid)
        {
            return await dbSiteFolder.Delete(guid);
        }

        public async Task<int> GetSiteIdByFolder(string folderName)
        {
            return await dbSiteSettings.GetSiteIdByFolder(folderName);
        }

        public int GetSiteIdByFolderNonAsync(string folderName)
        {
            return dbSiteSettings.GetSiteIdByFolderNonAsync(folderName);
        }

        public async Task<Guid> GetSiteGuidByFolder(string folderName)
        {
            return await dbSiteFolder.GetSiteGuid(folderName);
        }

        public async Task<bool> FolderExists(string folderName)
        {
            return await dbSiteFolder.Exists(folderName);
        }




        #endregion

        #region IDisposable

        public void Dispose()
        {

        }

        #endregion

        #region private methods



        private void SaveExpandoProperties(int siteId, Guid siteGuid, List<ExpandoSetting> exapandoProperties)
        {
            // process the dirty rows as updates

            foreach (ExpandoSetting s in exapandoProperties)
            {
                if (s.IsDirty)
                {
                    dbSiteSettingsEx.SaveExpandoProperty(
                        siteId,
                        siteGuid,
                        s.GroupName,
                        s.KeyName,
                        s.KeyValue);

                }

            }

        }


        private List<ExpandoSetting> GetExpandoProperties(int siteId)
        {
            if (siteId == -1) { return GetDefaultExpandoProperties(); } //new site

            List<ExpandoSetting> settings = new List<ExpandoSetting>();

            using (DbDataReader reader = dbSiteSettingsEx.GetSiteSettingsExList(siteId))
            {
                while (reader.Read())
                {
                    ExpandoSetting s = new ExpandoSetting();
                    s.SiteId = Convert.ToInt32(reader["SiteID"]);
                    s.KeyName = reader["KeyName"].ToString();
                    s.KeyValue = reader["KeyValue"].ToString();
                    s.GroupName = reader["GroupName"].ToString();
                    s.IsDirty = false;

                    settings.Add(s);

                }
            }

            return settings;
        }

        private List<ExpandoSetting> GetDefaultExpandoProperties()
        {

            List<ExpandoSetting> settings = new List<ExpandoSetting>();

            using (DbDataReader reader = dbSiteSettingsEx.GetDefaultExpandoSettings())
            {
                while (reader.Read())
                {
                    ExpandoSetting s = new ExpandoSetting();
                    s.SiteId = -1;
                    s.KeyName = reader["KeyName"].ToString();
                    s.KeyValue = reader["DefaultValue"].ToString();
                    s.GroupName = reader["GroupName"].ToString();
                    s.IsDirty = false;

                    settings.Add(s);

                }
            }


            return settings;
        }




        #endregion
    }

}
