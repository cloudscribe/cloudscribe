// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2014-09-06
// 


using cloudscribe.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace cloudscribe.Core.Repositories.Firebird
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
                    string.Empty, // legacy apiKeyExtra4
                    string.Empty, // legacy apiKeyExtra5
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
                    string.Empty, // legacy apiKeyExtra4
                    string.Empty, // legacy apiKeyExtra5
                    site.DisableDbAuth);

            }

            // settings below stored as key value pairs in mp_SiteSettingsEx


            DBSiteSettingsEx.EnsureSettings();

            DataTable expandoProperties = GetExpandoProperties(passedInSiteId); //-1 on new sites to get the default values
            
            // update a local data table of expando properties if the value changed and mark the row dirty

            SetExpandoProperty(expandoProperties, "AvatarSystem", site.AvatarSystem);
            //SetExpandoProperty(expandoProperties, "AllowUserEditorPreference", site.AllowUserEditorPreference);
            SetExpandoProperty(expandoProperties, "CommentProvider", site.CommentProvider);
            SetExpandoProperty(expandoProperties, "CompanyPublicEmail", site.CompanyPublicEmail);
            SetExpandoProperty(expandoProperties, "CompanyFax", site.CompanyFax);
            SetExpandoProperty(expandoProperties, "CompanyPhone", site.CompanyPhone);
            SetExpandoProperty(expandoProperties, "CompanyCountry", site.CompanyCountry);
            SetExpandoProperty(expandoProperties, "CompanyPostalCode", site.CompanyPostalCode);
            SetExpandoProperty(expandoProperties, "CompanyRegion", site.CompanyRegion);
            SetExpandoProperty(expandoProperties, "CompanyLocality", site.CompanyLocality);
            SetExpandoProperty(expandoProperties, "CompanyStreetAddress", site.CompanyStreetAddress);
            SetExpandoProperty(expandoProperties, "CompanyStreetAddress2", site.CompanyStreetAddress2);
            SetExpandoProperty(expandoProperties, "CompanyName", site.CompanyName);
            SetExpandoProperty(expandoProperties, "CurrencyGuid", site.CurrencyGuid.ToString());
            SetExpandoProperty(expandoProperties, "DefaultStateGuid", site.DefaultStateGuid.ToString());
            SetExpandoProperty(expandoProperties, "DefaultCountryGuid", site.DefaultCountryGuid.ToString());
            SetExpandoProperty(expandoProperties, "DefaultRootPageCreateChildPageRoles", site.DefaultRootPageCreateChildPageRoles);
            SetExpandoProperty(expandoProperties, "DefaultRootPageEditRoles", site.DefaultRootPageEditRoles);
            SetExpandoProperty(expandoProperties, "DefaultRootPageViewRoles", site.DefaultRootPageViewRoles);
            SetExpandoProperty(expandoProperties, "DisqusSiteShortName", site.DisqusSiteShortName);
            SetExpandoProperty(expandoProperties, "EmailAdressesForUserApprovalNotification", site.EmailAdressesForUserApprovalNotification);
            SetExpandoProperty(expandoProperties, "EnableContentWorkflow", site.EnableContentWorkflow.ToString());
            SetExpandoProperty(expandoProperties, "FacebookAppId", site.FacebookAppId);
            SetExpandoProperty(expandoProperties, "ForceContentVersioning", site.ForceContentVersioning.ToString());
            SetExpandoProperty(expandoProperties, "GoogleAnalyticsSettings", site.GoogleAnalyticsSettings);
            SetExpandoProperty(expandoProperties, "GoogleAnalyticsProfileId", site.GoogleAnalyticsProfileId);
            //SetExpandoProperty(expandoProperties, "GoogleAnalyticsPassword", site.GoogleAnalyticsPassword);
            //SetExpandoProperty(expandoProperties, "GoogleAnalyticsEmail", site.GoogleAnalyticsEmail);
            SetExpandoProperty(expandoProperties, "IntenseDebateAccountId", site.IntenseDebateAccountId);
            SetExpandoProperty(expandoProperties, "LoginInfoBottom", site.LoginInfoBottom);
            SetExpandoProperty(expandoProperties, "LoginInfoTop", site.LoginInfoBottom);
            SetExpandoProperty(expandoProperties, "MetaProfile", site.MetaProfile);
            SetExpandoProperty(expandoProperties, "NewsletterEditor", site.NewsletterEditor);
            SetExpandoProperty(expandoProperties, "PasswordRegexWarning", site.PasswordRegexWarning);
            SetExpandoProperty(expandoProperties, "PrivacyPolicyUrl", site.PrivacyPolicyUrl);
            SetExpandoProperty(expandoProperties, "RegistrationAgreement", site.RegistrationAgreement);
            SetExpandoProperty(expandoProperties, "RegistrationPreamble", site.RegistrationPreamble);
            SetExpandoProperty(expandoProperties, "RequireApprovalBeforeLogin", site.RequireApprovalBeforeLogin.ToString());

            // permission roles
            SetExpandoProperty(expandoProperties, "RolesThatCanApproveNewUsers", site.RolesThatCanApproveNewUsers);
            SetExpandoProperty(expandoProperties, "RolesThatCanManageSkins", site.RolesThatCanManageSkins);
            SetExpandoProperty(expandoProperties, "RolesThatCanAssignSkinsToPages", site.RolesThatCanAssignSkinsToPages);
            SetExpandoProperty(expandoProperties, "RolesThatCanDeleteFilesInEditor", site.RolesThatCanDeleteFilesInEditor);
            SetExpandoProperty(expandoProperties, "UserFilesBrowseAndUploadRoles", site.UserFilesBrowseAndUploadRoles);
            SetExpandoProperty(expandoProperties, "GeneralBrowseAndUploadRoles", site.GeneralBrowseAndUploadRoles);
            SetExpandoProperty(expandoProperties, "RolesThatCanEditContentTemplates", site.RolesThatCanEditContentTemplates);
            SetExpandoProperty(expandoProperties, "RolesNotAllowedToEditModuleSettings", site.RolesNotAllowedToEditModuleSettings);
            SetExpandoProperty(expandoProperties, "RolesThatCanLookupUsers", site.RolesThatCanLookupUsers);
            SetExpandoProperty(expandoProperties, "RolesThatCanFullyManageUsers", site.RolesThatCanManageUsers);
            SetExpandoProperty(expandoProperties, "RolesThatCanManageUsers", site.RolesThatCanCreateUsers);
            SetExpandoProperty(expandoProperties, "RolesThatCanViewMemberList", site.RolesThatCanViewMemberList);
            SetExpandoProperty(expandoProperties, "RolesThatCanCreateRootPages", site.RolesThatCanCreateRootPages);
            SetExpandoProperty(expandoProperties, "CommerceReportViewRoles", site.CommerceReportViewRoles);
            SetExpandoProperty(expandoProperties, "SiteRootDraftApprovalRoles", site.SiteRootDraftApprovalRoles);
            SetExpandoProperty(expandoProperties, "SiteRootDraftEditRoles", site.SiteRootDraftEditRoles);
            SetExpandoProperty(expandoProperties, "SiteRootEditRoles", site.SiteRootEditRoles);

            // end roles
            
            SetExpandoProperty(expandoProperties, "SiteIsClosed", site.SiteIsClosed.ToString());
            SetExpandoProperty(expandoProperties, "SiteIsClosedMessage", site.SiteIsClosedMessage);
            SetExpandoProperty(expandoProperties, "SkinVersion", site.SkinVersion.ToString());
            SetExpandoProperty(expandoProperties, "SMTPUseSsl", site.SMTPUseSsl.ToString());
            SetExpandoProperty(expandoProperties, "SMTPRequiresAuthentication", site.SMTPRequiresAuthentication.ToString());
            SetExpandoProperty(expandoProperties, "SMTPServer", site.SMTPServer);
            SetExpandoProperty(expandoProperties, "SMTPPreferredEncoding", site.SMTPPreferredEncoding);
            SetExpandoProperty(expandoProperties, "SMTPPort", site.SMTPPort.ToString(CultureInfo.InvariantCulture));
            SetExpandoProperty(expandoProperties, "SMTPPassword", site.SMTPPassword);
            SetExpandoProperty(expandoProperties, "SMTPUser", site.SMTPUser);
            SetExpandoProperty(expandoProperties, "Slogan", site.Slogan);
            SetExpandoProperty(expandoProperties, "ShowAlternateSearchIfConfigured", site.ShowAlternateSearchIfConfigured.ToString());
            SetExpandoProperty(expandoProperties, "PrimarySearchEngine", site.PrimarySearchEngine);
            SetExpandoProperty(expandoProperties, "GoogleCustomSearchId", site.GoogleCustomSearchId);
            SetExpandoProperty(expandoProperties, "BingAPIId", site.BingAPIId);
            SetExpandoProperty(expandoProperties, "OpenSearchName", site.OpenSearchName);
            SetExpandoProperty(expandoProperties, "RpxNowAdminUrl", site.RpxNowAdminUrl);
            SetExpandoProperty(expandoProperties, "RpxNowApplicationName", site.RpxNowApplicationName);
            SetExpandoProperty(expandoProperties, "RpxNowApiKey", site.RpxNowApiKey);
            //SetExpandoProperty(expandoProperties, "AppLogoForWindowsLive", site.AppLogoForWindowsLive);

            SetExpandoProperty(expandoProperties, "SiteMapSkin", site.SiteMapSkin);
            SetExpandoProperty(expandoProperties, "TimeZoneId", site.TimeZoneId);
            //SetExpandoProperty(expandoProperties, "ShowPasswordStrengthOnRegistration", site.ShowPasswordStrengthOnRegistration);
            //SetExpandoProperty(expandoProperties, "RequireEnterEmailTwiceOnRegistration", site.RequireEnterEmailTwiceOnRegistration);
            SetExpandoProperty(expandoProperties, "RequireCaptchaOnLogin", site.RequireCaptchaOnLogin.ToString());
            SetExpandoProperty(expandoProperties, "RequireCaptchaOnRegistration", site.RequireCaptchaOnRegistration.ToString());
            SetExpandoProperty(expandoProperties, "AllowPersistentLogin", site.AllowPersistentLogin.ToString());
            



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
                    LoadFromReader(reader, site);
                }

            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            LoadExpandoSettings(site);

            return site;

            
        }

        public ISiteSettings Fetch(Guid siteGuid)
        {
            SiteSettings site = new SiteSettings();

            using (IDataReader reader = DBSiteSettings.GetSite(siteGuid))
            {
                if (reader.Read())
                {
                    LoadFromReader(reader, site);
                }

            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            LoadExpandoSettings(site);

            return site;


        }

        public ISiteSettings Fetch(string hostName)
        {
            SiteSettings site = new SiteSettings();

            using (IDataReader reader = DBSiteSettings.GetSite(hostName))
            {
                if (reader.Read())
                {
                    LoadFromReader(reader, site);
                }

            }

            if (site.SiteGuid == Guid.Empty) { return null; }//not found 

            LoadExpandoSettings(site);

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
                    LoadFromReader(reader, site);
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
                    LoadFromReader(reader, site);
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
                    LoadFromReader(reader, host);
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
                    LoadFromReader(reader, host);
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
                    LoadFromReader(reader, host);
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
                    LoadFromReader(reader, siteFolder);
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
                    LoadFromReader(reader, siteFolder);
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
                    LoadFromReader(reader, siteFolder);
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

        private void LoadFromReader(IDataReader reader, ISiteSettings site)
        {

            site.SiteId = Convert.ToInt32(reader["SiteID"]);
            site.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            //site.SiteAlias = reader["SiteAlias"].ToString();
            site.SiteName = reader["SiteName"].ToString();
            site.Skin = reader["Skin"].ToString();
            site.Logo = reader["Logo"].ToString();
            site.Icon = reader["Icon"].ToString();
            site.AllowUserSkins = Convert.ToBoolean(reader["AllowUserSkins"]);
            site.AllowPageSkins = Convert.ToBoolean(reader["AllowPageSkins"]);
            site.AllowHideMenuOnPages = Convert.ToBoolean(reader["AllowHideMenuOnPages"]);
            site.AllowNewRegistration = Convert.ToBoolean(reader["AllowNewRegistration"]);
            site.UseSecureRegistration = Convert.ToBoolean(reader["UseSecureRegistration"]);
            site.UseSslOnAllPages = Convert.ToBoolean(reader["UseSSLOnAllPages"]);

            site.IsServerAdminSite = Convert.ToBoolean(reader["IsServerAdminSite"]);
            site.UseLdapAuth = Convert.ToBoolean(reader["UseLdapAuth"]);
            site.AutoCreateLdapUserOnFirstLogin = Convert.ToBoolean(reader["AutoCreateLdapUserOnFirstLogin"]);

            if (site.SiteLdapSettings == null) { site.SiteLdapSettings = new LdapSettings(); }
            site.SiteLdapSettings.Server = reader["LdapServer"].ToString();
            site.SiteLdapSettings.Port = Convert.ToInt32(reader["LdapPort"]);
            site.SiteLdapSettings.Domain = reader["LdapDomain"].ToString();
            site.SiteLdapSettings.RootDN = reader["LdapRootDN"].ToString();
            site.SiteLdapSettings.UserDNKey = reader["LdapUserDNKey"].ToString();

            site.ReallyDeleteUsers = Convert.ToBoolean(reader["ReallyDeleteUsers"]);
            site.UseEmailForLogin = Convert.ToBoolean(reader["UseEmailForLogin"]);
            site.AllowUserFullNameChange = Convert.ToBoolean(reader["AllowUserFullNameChange"]);
            //site.EditorSkin = reader["EditorSkin"].ToString();
            //site.DefaultFriendlyUrlPatternEnum = reader["DefaultFriendlyUrlPatternEnum"].ToString();
            site.AllowPasswordRetrieval = Convert.ToBoolean(reader["AllowPasswordRetrieval"]);
            site.AllowPasswordReset = Convert.ToBoolean(reader["AllowPasswordReset"]);
            site.RequiresQuestionAndAnswer = Convert.ToBoolean(reader["RequiresQuestionAndAnswer"]);
            site.MaxInvalidPasswordAttempts = Convert.ToInt32(reader["MaxInvalidPasswordAttempts"]);
            site.PasswordAttemptWindowMinutes = Convert.ToInt32(reader["PasswordAttemptWindowMinutes"]);
            //site.RequiresUniqueEmail = Convert.ToBoolean(reader["RequiresUniqueEmail"]);
            site.PasswordFormat = Convert.ToInt32(reader["PasswordFormat"]);
            site.MinRequiredPasswordLength = Convert.ToInt32(reader["MinRequiredPasswordLength"]);
            site.MinRequiredNonAlphanumericCharacters = Convert.ToInt32(reader["MinReqNonAlphaChars"]);
            site.PasswordStrengthRegularExpression = reader["PwdStrengthRegex"].ToString();
            site.DefaultEmailFromAddress = reader["DefaultEmailFromAddress"].ToString();
            //site.EnableMyPageFeature = Convert.ToBoolean(reader["EnableMyPageFeature"]);
            site.EditorProviderName = reader["EditorProvider"].ToString();
            site.CaptchaProvider = reader["CaptchaProvider"].ToString();
            //site.DatePickerProvider = reader["DatePickerProvider"].ToString();
            site.RecaptchaPrivateKey = reader["RecaptchaPrivateKey"].ToString();
            site.RecaptchaPublicKey = reader["RecaptchaPublicKey"].ToString();
            site.WordpressApiKey = reader["WordpressAPIKey"].ToString();
            site.WindowsLiveAppId = reader["WindowsLiveAppID"].ToString();
            site.WindowsLiveKey = reader["WindowsLiveKey"].ToString();
            //site.AllowOpenIDAuth = Convert.ToBoolean(reader["AllowOpenIDAuth"]);
            //site.AllowWindowsLiveAuth = Convert.ToBoolean(reader["AllowWindowsLiveAuth"]);
            site.GmapApiKey = reader["GmapApiKey"].ToString();
            site.AddThisDotComUsername = reader["ApiKeyExtra1"].ToString();
            site.GoogleAnalyticsAccountCode = reader["ApiKeyExtra2"].ToString();
            //site.ApiKeyExtra3 = reader["ApiKeyExtra3"].ToString();
            //site.ApiKeyExtra4 = reader["ApiKeyExtra4"].ToString();
            //site.ApiKeyExtra5 = reader["ApiKeyExtra5"].ToString();
            site.DisableDbAuth = Convert.ToBoolean(reader["DisableDbAuth"]);

        }

        private void LoadFromReader(IDataReader reader, ISiteInfo site)
        {

            site.SiteId = Convert.ToInt32(reader["SiteID"]);
            site.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            site.SiteName = reader["SiteName"].ToString();
        }

        private void LoadFromReader(IDataReader reader, ISiteHost host)
        {

            host.HostId = Convert.ToInt32(reader["HostID"]);
            host.HostName = reader["HostName"].ToString();
            host.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            host.SiteId = Convert.ToInt32(reader["SiteID"]);
        }

        private void LoadFromReader(IDataReader reader, ISiteFolder folder)
        {
            folder.Guid = new Guid(reader["Guid"].ToString());
            folder.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            folder.FolderName = reader["FolderName"].ToString();
        }

        private void LoadExpandoSettings(ISiteSettings site)
        {
            DataTable expandoProperties = GetExpandoProperties(site.SiteId);

            string b = GetExpandoProperty(expandoProperties, "AllowPersistentLogin");
            if (!string.IsNullOrEmpty(b)) { site.AllowPersistentLogin = Convert.ToBoolean(b); }

            site.AvatarSystem = GetExpandoProperty(expandoProperties, "AvatarSystem");
            site.CommentProvider = GetExpandoProperty(expandoProperties, "CommentProvider");
            site.CompanyPublicEmail = GetExpandoProperty(expandoProperties, "CompanyPublicEmail");
            site.CompanyFax = GetExpandoProperty(expandoProperties, "CompanyFax");
            site.CompanyPhone = GetExpandoProperty(expandoProperties, "CompanyPhone");
            site.CompanyCountry = GetExpandoProperty(expandoProperties, "CompanyCountry");
            site.CompanyPostalCode = GetExpandoProperty(expandoProperties, "CompanyPostalCode");
            site.CompanyRegion = GetExpandoProperty(expandoProperties, "CompanyRegion");
            site.CompanyLocality = GetExpandoProperty(expandoProperties, "CompanyLocality");
            site.CompanyStreetAddress = GetExpandoProperty(expandoProperties, "CompanyStreetAddress");
            site.CompanyStreetAddress2 = GetExpandoProperty(expandoProperties, "CompanyStreetAddress2");
            site.CompanyName = GetExpandoProperty(expandoProperties, "CompanyName");

            string g = GetExpandoProperty(expandoProperties, "CurrencyGuid");
            if (g.Length == 36) { site.CurrencyGuid = new Guid(g); }
            g = GetExpandoProperty(expandoProperties, "DefaultStateGuid");
            if (g.Length == 36) { site.DefaultStateGuid = new Guid(g); }
            g = GetExpandoProperty(expandoProperties, "DefaultCountryGuid");
            if (g.Length == 36) { site.DefaultCountryGuid = new Guid(g); }

            site.DefaultRootPageCreateChildPageRoles = GetExpandoProperty(expandoProperties, "DefaultRootPageCreateChildPageRoles");

            site.DefaultRootPageEditRoles = GetExpandoProperty(expandoProperties, "DefaultRootPageEditRoles");
            site.DefaultRootPageViewRoles = GetExpandoProperty(expandoProperties, "DefaultRootPageViewRoles");
            site.DisqusSiteShortName = GetExpandoProperty(expandoProperties, "DisqusSiteShortName");
            site.EmailAdressesForUserApprovalNotification = GetExpandoProperty(expandoProperties, "EmailAdressesForUserApprovalNotification");
            b = GetExpandoProperty(expandoProperties, "EnableContentWorkflow");
            if (!string.IsNullOrEmpty(b)) { site.EnableContentWorkflow = Convert.ToBoolean(b); }
            site.FacebookAppId = GetExpandoProperty(expandoProperties, "FacebookAppId");
            b = GetExpandoProperty(expandoProperties, "ForceContentVersioning");
            if (!string.IsNullOrEmpty(b)) { site.ForceContentVersioning = Convert.ToBoolean(b); }
            site.GoogleAnalyticsSettings = GetExpandoProperty(expandoProperties, "GoogleAnalyticsSettings");
            site.GoogleAnalyticsProfileId = GetExpandoProperty(expandoProperties, "GoogleAnalyticsProfileId");
            //SetExpandoProperty(expandoProperties, "GoogleAnalyticsPassword", site.GoogleAnalyticsPassword);
            //SetExpandoProperty(expandoProperties, "GoogleAnalyticsEmail", site.GoogleAnalyticsEmail);
            site.IntenseDebateAccountId = GetExpandoProperty(expandoProperties, "IntenseDebateAccountId");
            site.LoginInfoBottom = GetExpandoProperty(expandoProperties, "LoginInfoBottom");
            site.LoginInfoBottom = GetExpandoProperty(expandoProperties, "LoginInfoTop");
            site.MetaProfile = GetExpandoProperty(expandoProperties, "MetaProfile");
            site.NewsletterEditor = GetExpandoProperty(expandoProperties, "NewsletterEditor");
            site.PasswordRegexWarning = GetExpandoProperty(expandoProperties, "PasswordRegexWarning");
            site.PrivacyPolicyUrl = GetExpandoProperty(expandoProperties, "PrivacyPolicyUrl");
            site.RegistrationAgreement = GetExpandoProperty(expandoProperties, "RegistrationAgreement");
            site.RegistrationPreamble = GetExpandoProperty(expandoProperties, "RegistrationPreamble");

            b = GetExpandoProperty(expandoProperties, "RequireApprovalBeforeLogin");
            if (!string.IsNullOrEmpty(b)) { site.RequireApprovalBeforeLogin = Convert.ToBoolean(b); }

            // permission roles
            site.RolesThatCanApproveNewUsers = GetExpandoProperty(expandoProperties, "RolesThatCanApproveNewUsers");
            site.RolesThatCanManageSkins = GetExpandoProperty(expandoProperties, "RolesThatCanManageSkins");
            site.RolesThatCanAssignSkinsToPages = GetExpandoProperty(expandoProperties, "RolesThatCanAssignSkinsToPages");
            site.RolesThatCanDeleteFilesInEditor = GetExpandoProperty(expandoProperties, "RolesThatCanDeleteFilesInEditor");
            site.UserFilesBrowseAndUploadRoles = GetExpandoProperty(expandoProperties, "UserFilesBrowseAndUploadRoles");
            site.GeneralBrowseAndUploadRoles = GetExpandoProperty(expandoProperties, "GeneralBrowseAndUploadRoles");
            site.RolesThatCanEditContentTemplates = GetExpandoProperty(expandoProperties, "RolesThatCanEditContentTemplates");
            site.RolesNotAllowedToEditModuleSettings = GetExpandoProperty(expandoProperties, "RolesNotAllowedToEditModuleSettings");
            site.RolesThatCanLookupUsers = GetExpandoProperty(expandoProperties, "RolesThatCanLookupUsers");
            site.RolesThatCanManageUsers = GetExpandoProperty(expandoProperties, "RolesThatCanFullyManageUsers");
            site.RolesThatCanCreateUsers = GetExpandoProperty(expandoProperties, "RolesThatCanManageUsers");
            site.RolesThatCanViewMemberList = GetExpandoProperty(expandoProperties, "RolesThatCanViewMemberList");
            site.RolesThatCanCreateRootPages = GetExpandoProperty(expandoProperties, "RolesThatCanCreateRootPages");
            site.CommerceReportViewRoles = GetExpandoProperty(expandoProperties, "CommerceReportViewRoles");
            site.SiteRootDraftApprovalRoles = GetExpandoProperty(expandoProperties, "SiteRootDraftApprovalRoles");
            site.SiteRootDraftEditRoles = GetExpandoProperty(expandoProperties, "SiteRootDraftEditRoles");
            site.SiteRootEditRoles = GetExpandoProperty(expandoProperties, "SiteRootEditRoles");

            // end roles

            b = GetExpandoProperty(expandoProperties, "SiteIsClosed");
            if (!string.IsNullOrEmpty(b)) { site.SiteIsClosed = Convert.ToBoolean(b); }

            site.SiteIsClosedMessage = GetExpandoProperty(expandoProperties, "SiteIsClosedMessage");

            g = GetExpandoProperty(expandoProperties, "SkinVersion");
            if (g.Length == 36) { site.SkinVersion = new Guid(g); }

            b = GetExpandoProperty(expandoProperties, "SMTPUseSsl");
            if (!string.IsNullOrEmpty(b)) { site.SMTPUseSsl = Convert.ToBoolean(b); }

            b = GetExpandoProperty(expandoProperties, "SMTPRequiresAuthentication");
            if (!string.IsNullOrEmpty(b)) { site.SMTPRequiresAuthentication = Convert.ToBoolean(b); }


            site.SMTPServer = GetExpandoProperty(expandoProperties, "SMTPServer");
            site.SMTPPreferredEncoding = GetExpandoProperty(expandoProperties, "SMTPPreferredEncoding");
            string i = GetExpandoProperty(expandoProperties, "SMTPPort");
            if (!string.IsNullOrEmpty(i)) { site.SMTPPort = Convert.ToInt32(i); }
            site.SMTPPassword = GetExpandoProperty(expandoProperties, "SMTPPassword");
            site.SMTPUser = GetExpandoProperty(expandoProperties, "SMTPUser");
            site.Slogan = GetExpandoProperty(expandoProperties, "Slogan");

            b = GetExpandoProperty(expandoProperties, "ShowAlternateSearchIfConfigured");
            if (!string.IsNullOrEmpty(b)) { site.ShowAlternateSearchIfConfigured = Convert.ToBoolean(b); }

            site.PrimarySearchEngine = GetExpandoProperty(expandoProperties, "PrimarySearchEngine");
            site.GoogleCustomSearchId = GetExpandoProperty(expandoProperties, "GoogleCustomSearchId");
            site.BingAPIId = GetExpandoProperty(expandoProperties, "BingAPIId");
            site.OpenSearchName = GetExpandoProperty(expandoProperties, "OpenSearchName");
            site.RpxNowAdminUrl = GetExpandoProperty(expandoProperties, "RpxNowAdminUrl");
            site.RpxNowApplicationName = GetExpandoProperty(expandoProperties, "RpxNowApplicationName");
            site.RpxNowApiKey = GetExpandoProperty(expandoProperties, "RpxNowApiKey");
            //site.AppLogoForWindowsLive = GetExpandoProperty(expandoProperties, "AppLogoForWindowsLive");

            site.SiteMapSkin = GetExpandoProperty(expandoProperties, "SiteMapSkin");
            site.TimeZoneId = GetExpandoProperty(expandoProperties, "TimeZoneId");
            //SetExpandoProperty(expandoProperties, "ShowPasswordStrengthOnRegistration", site.ShowPasswordStrengthOnRegistration);
            //SetExpandoProperty(expandoProperties, "RequireEnterEmailTwiceOnRegistration", site.RequireEnterEmailTwiceOnRegistration);

            b = GetExpandoProperty(expandoProperties, "RequireCaptchaOnLogin");
            if (!string.IsNullOrEmpty(b)) { site.RequireCaptchaOnLogin = Convert.ToBoolean(b); }

            b = GetExpandoProperty(expandoProperties, "RequireCaptchaOnRegistration");
            if (!string.IsNullOrEmpty(b)) { site.RequireCaptchaOnRegistration = Convert.ToBoolean(b); }







        }

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

        private string GetExpandoProperty(DataTable exapandoProperties, string keyName)
        {
            //EnsureExpandoProperties();

            foreach (DataRow row in exapandoProperties.Rows)
            {
                if (row["KeyName"].ToString().Trim().Equals(keyName, StringComparison.InvariantCulture))
                {
                    return row["KeyValue"].ToString();
                }

            }

            return null;

        }

        private void SetExpandoProperty(DataTable exapandoProperties, string keyName, string keyValue)
        {
            //EnsureExpandoProperties();
            //bool found = false;
            foreach (DataRow row in exapandoProperties.Rows)
            {
                if (row["KeyName"].ToString().Trim().Equals(keyName, StringComparison.InvariantCulture))
                {
                    if (row["KeyValue"].ToString() != keyValue)
                    {
                        row["KeyValue"] = keyValue;
                        row["IsDirty"] = true;
                    }

                    //found = true;
                    break;
                }

            }



        }



        private static DataTable GetExpandoProperties(int siteId)
        {
            if (siteId == -1) { return GetDefaultExpandoProperties(); } //new site

            DataTable dataTable = CreateExpandoTable();
            dataTable.TableName = "expandoProperties";


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

            DataTable dataTable = CreateExpandoTable();

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

        private static DataTable CreateExpandoTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("SiteID", typeof(int));
            dataTable.Columns.Add("KeyName", typeof(string));
            dataTable.Columns.Add("KeyValue", typeof(string));
            dataTable.Columns.Add("GroupName", typeof(string));
            dataTable.Columns.Add("IsDirty", typeof(bool));

            return dataTable;

        }


        #endregion
    }

}
