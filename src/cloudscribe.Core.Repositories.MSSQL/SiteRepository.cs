// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2016-01-27
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.DataExtensions;
using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MSSQL
{

    public sealed class SiteRepository : ISiteRepository
    {
        public SiteRepository(
            IOptions<MSSQLConnectionOptions> connectionOptions,
            ILoggerFactory loggerFactory
            )
        {
            if (connectionOptions == null) { throw new ArgumentNullException(nameof(connectionOptions)); }
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }

            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(typeof(SiteRepository).FullName);

            readConnectionString = connectionOptions.Value.ReadConnectionString;
            writeConnectionString = connectionOptions.Value.WriteConnectionString;
            
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

        public async Task<bool> Save(ISiteSettings site, CancellationToken cancellationToken = default(CancellationToken))
        {
            int passedInSiteId = site.SiteId;
            bool result = false;

            cancellationToken.ThrowIfCancellationRequested();

            if (site.SiteId == -1) // new site
            {
                site.SiteGuid = Guid.NewGuid();

                site.SiteId = await dbSiteSettings.Create(
                    site.SiteGuid,
                    site.SiteName,
                    site.Layout,
                    site.AllowNewRegistration,
                    site.RequireConfirmedEmail,
                    site.IsServerAdminSite,
                    site.UseLdapAuth,
                    site.AutoCreateLdapUserOnFirstLogin,
                    site.LdapServer,
                    site.LdapPort,
                    site.LdapDomain,
                    site.LdapRootDN,
                    site.LdapUserDNKey,
                    site.UseEmailForLogin,
                    site.ReallyDeleteUsers,
                    site.RecaptchaPrivateKey,
                    site.RecaptchaPublicKey,
                    site.DisableDbAuth,
                    site.RequiresQuestionAndAnswer,
                    site.MaxInvalidPasswordAttempts,
                    site.MinRequiredPasswordLength,
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
                    site.RequireApprovalBeforeLogin,
                    site.IsDataProtected,
                    site.CreatedUtc,

                    site.RequireConfirmedPhone,
                    site.DefaultEmailFromAlias,
                    site.AccountApprovalEmailCsv,
                    site.DkimPublicKey,
                    site.DkimPrivateKey,
                    site.DkimDomain,
                    site.DkimSelector,
                    site.SignEmailWithDkim,
                    site.OidConnectAppId,
                    site.OidConnectAppSecret,
                    site.SmsClientId,
                    site.SmsSecureToken,
                    site.SmsFrom,

                    cancellationToken
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
                    site.RequireConfirmedEmail,
                    site.IsServerAdminSite,
                    site.UseLdapAuth,
                    site.AutoCreateLdapUserOnFirstLogin,
                    site.LdapServer,
                    site.LdapPort,
                    site.LdapDomain,
                    site.LdapRootDN,
                    site.LdapUserDNKey,
                    site.UseEmailForLogin,
                    site.ReallyDeleteUsers,
                    site.RecaptchaPrivateKey,
                    site.RecaptchaPublicKey,
                    site.DisableDbAuth,

                    site.RequiresQuestionAndAnswer,
                    site.MaxInvalidPasswordAttempts,
                    site.MinRequiredPasswordLength,
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
                    site.RequireApprovalBeforeLogin,
                    site.IsDataProtected,

                    site.RequireConfirmedPhone,
                    site.DefaultEmailFromAlias,
                    site.AccountApprovalEmailCsv,
                    site.DkimPublicKey,
                    site.DkimPrivateKey,
                    site.DkimDomain,
                    site.DkimSelector,
                    site.SignEmailWithDkim,
                    site.OidConnectAppId,
                    site.OidConnectAppSecret,
                    site.SmsClientId,
                    site.SmsSecureToken,
                    site.SmsFrom,

                    cancellationToken
                    );

            }

            if (!result) { return result; }

            // settings below stored as key value pairs in mp_SiteSettingsEx


            //bool nextResult = await dbSiteSettingsEx.EnsureSettings();

            //List<ExpandoSetting> expandoProperties = GetExpandoProperties(passedInSiteId); //-1 on new sites to get the default values

            // update a local data table of expando properties if the value changed and mark the row dirty
            //site.SetExpandoSettings(expandoProperties);
            // finally update the database only with properties in the table marked as dirty
            //SaveExpandoProperties(site.SiteId, site.SiteGuid, expandoProperties);

            return result;
        }


        public async Task<ISiteSettings> Fetch(
            int siteId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            SiteSettings site = new SiteSettings();

            using (DbDataReader reader = await dbSiteSettings.GetSite(siteId, cancellationToken))
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

        public async Task<ISiteSettings> Fetch(
            Guid siteGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            SiteSettings site = new SiteSettings();

            using (DbDataReader reader = await dbSiteSettings.GetSite(siteGuid, cancellationToken))
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

        public async Task<ISiteSettings> Fetch(
            string hostName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            SiteSettings site = new SiteSettings();

            using (DbDataReader reader = await dbSiteSettings.GetSite(hostName, cancellationToken))
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


        public async Task<bool> Delete(
            int siteId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteSettings.Delete(siteId, cancellationToken);
        }



        public async Task<int> GetCount(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteSettings.CountOtherSites(-1, cancellationToken);
        }

        public async Task<List<ISiteInfo>> GetList(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<ISiteInfo> sites = new List<ISiteInfo>();
            using (DbDataReader reader = await dbSiteSettings.GetSiteList(cancellationToken))
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

        public async Task<int> CountOtherSites(
            int currentSiteId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteSettings.CountOtherSites(currentSiteId, cancellationToken);
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
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<ISiteInfo> sites = new List<ISiteInfo>();

            using (DbDataReader reader = await dbSiteSettings.GetPageOfOtherSites(
                currentSiteId, 
                pageNumber, 
                pageSize,
                cancellationToken))
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

        public async Task<List<ISiteHost>> GetAllHosts(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<ISiteHost> hosts = new List<ISiteHost>();
            using (DbDataReader reader = await dbSiteSettings.GetAllHosts(cancellationToken))
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

        public async Task<int> GetHostCount(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteSettings.GetHostCount(cancellationToken);
        }

        public async Task<List<ISiteHost>> GetPageHosts(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<ISiteHost> hosts = new List<ISiteHost>();
            using (DbDataReader reader = await dbSiteSettings.GetPageHosts(pageNumber, pageSize, cancellationToken))
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

        public async Task<List<ISiteHost>> GetSiteHosts(
            int siteId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<ISiteHost> hosts = new List<ISiteHost>();
            using (DbDataReader reader = await dbSiteSettings.GetHostList(siteId, cancellationToken))
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

        public async Task<ISiteHost> GetSiteHost(
            string hostName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (DbDataReader reader = await dbSiteSettings.GetHost(hostName, cancellationToken))
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

        public async Task<bool> AddHost(
            Guid siteGuid, 
            int siteId, 
            string hostName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteSettings.AddHost(siteGuid, siteId, hostName, cancellationToken);
        }

        public async Task<bool> DeleteHost(
            int hostId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteSettings.DeleteHost(hostId, cancellationToken);
        }

        public async Task<bool> DeleteHostsBySite(
            int siteId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteSettings.DeleteHostsBySite(siteId, cancellationToken);
        }

        public async Task<int> GetSiteIdByHostName(
            string hostName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteSettings.GetSiteIdByHostName(hostName, cancellationToken);
        }

        public async Task<List<ISiteFolder>> GetSiteFoldersBySite(
            Guid siteGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<ISiteFolder> siteFolderList
                = new List<ISiteFolder>();

            using (DbDataReader reader = await dbSiteFolder.GetBySite(siteGuid, cancellationToken))
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

        public async Task<List<ISiteFolder>> GetAllSiteFolders(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<ISiteFolder> siteFolderList
                = new List<ISiteFolder>();

            using (DbDataReader reader = await dbSiteFolder.GetAll(cancellationToken))
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

        public List<ISiteFolder> GetAllSiteFoldersNonAsync()
        {
            List<ISiteFolder> siteFolderList
                = new List<ISiteFolder>();

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

        public async Task<int> GetFolderCount(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteFolder.GetFolderCount(cancellationToken);
        }

        public async Task<List<ISiteFolder>> GetPageSiteFolders(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<ISiteFolder> siteFolderList
                = new List<ISiteFolder>();

            using (DbDataReader reader = await dbSiteFolder.GetPage(pageNumber, pageSize, cancellationToken))
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

        public async Task<ISiteFolder> GetSiteFolder(
            string folderName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (DbDataReader reader = await dbSiteFolder.GetOne(folderName, cancellationToken))
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


        public async Task<bool> Save(
            ISiteFolder siteFolder, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (siteFolder == null) { return false; }
            cancellationToken.ThrowIfCancellationRequested();

            if (siteFolder.Guid == Guid.Empty)
            {
                siteFolder.Guid = Guid.NewGuid();

                return await dbSiteFolder.Add(
                    siteFolder.Guid,
                    siteFolder.SiteGuid,
                    siteFolder.FolderName,
                    cancellationToken);
            }
            else
            {
                return await dbSiteFolder.Update(
                    siteFolder.Guid,
                    siteFolder.SiteGuid,
                    siteFolder.FolderName,
                    cancellationToken);

            }
        }

        public async Task<bool> DeleteFolder(
            Guid guid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteFolder.Delete(guid, cancellationToken);
        }

        public async Task<bool> DeleteFoldersBySite(
            Guid siteGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteFolder.DeleteFoldersBySite(siteGuid, cancellationToken);
        }

        public async Task<int> GetSiteIdByFolder(
            string folderName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteSettings.GetSiteIdByFolder(folderName, cancellationToken);
        }

        public int GetSiteIdByFolderNonAsync(string folderName)
        {
            return dbSiteSettings.GetSiteIdByFolderNonAsync(folderName);
        }

        public async Task<Guid> GetSiteGuidByFolder(
            string folderName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteFolder.GetSiteGuid(folderName, cancellationToken);
        }

        public async Task<bool> FolderExists(
            string folderName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await dbSiteFolder.Exists(folderName, cancellationToken);
        }

        //TODO: this is not part of ISiteSettings
        // this method should be moved to AppSettings class
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


        //    return result;

        //}


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
