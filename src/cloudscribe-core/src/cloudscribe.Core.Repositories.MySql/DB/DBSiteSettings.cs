// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-06-13
// 
// You must not remove this notice, or any other, from this software.
// 


using cloudscribe.DbHelpers.MySql;
using Microsoft.Framework.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MySql
{
    internal class DBSiteSettings
    {
        internal DBSiteSettings(
            string dbReadConnectionString,
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;

        public async Task<int> Create(
            Guid siteGuid,
            String siteName,
            String skin,
            String logo,
            String icon,
            bool allowNewRegistration,
            bool allowUserSkins,
            bool allowPageSkins,
            bool allowHideMenuOnPages,
            bool useSecureRegistration,
            bool useSslOnAllPages,
            String defaultPageKeywords,
            String defaultPageDescription,
            String defaultPageEncoding,
            String defaultAdditionalMetaTags,
            bool isServerAdminSite,
            bool useLdapAuth,
            bool autoCreateLdapUserOnFirstLogin,
            String ldapServer,
            int ldapPort,
            String ldapDomain,
            String ldapRootDN,
            String ldapUserDNKey,
            bool allowUserFullNameChange,
            bool useEmailForLogin,
            bool reallyDeleteUsers,
            String editorSkin,
            String defaultFriendlyUrlPattern,
            bool enableMyPageFeature,
            string editorProvider,
            string datePickerProvider,
            string captchaProvider,
            string recaptchaPrivateKey,
            string recaptchaPublicKey,
            string wordpressApiKey,
            string windowsLiveAppId,
            string windowsLiveKey,
            bool allowOpenIdAuth,
            bool allowWindowsLiveAuth,
            string gmapApiKey,
            string apiKeyExtra1,
            string apiKeyExtra2,
            string apiKeyExtra3,
            string apiKeyExtra4,
            string apiKeyExtra5,
            bool disableDbAuth)
        {

            #region bool conversion

            byte intDisableDbAuth = 0;
            if (disableDbAuth) { intDisableDbAuth = 1; }

            byte oidauth;
            if (allowOpenIdAuth)
            {
                oidauth = 1;
            }
            else
            {
                oidauth = 0;
            }

            byte winliveauth;
            if (allowWindowsLiveAuth)
            {
                winliveauth = 1;
            }
            else
            {
                winliveauth = 0;
            }

            byte uldapp;
            if (useLdapAuth)
            {
                uldapp = 1;
            }
            else
            {
                uldapp = 0;
            }

            byte autoldapp;
            if (autoCreateLdapUserOnFirstLogin)
            {
                autoldapp = 1;
            }
            else
            {
                autoldapp = 0;
            }

            byte allowNameChange;
            if (allowUserFullNameChange)
            {
                allowNameChange = 1;
            }
            else
            {
                allowNameChange = 0;
            }

            byte emailForLogin;
            if (useEmailForLogin)
            {
                emailForLogin = 1;
            }
            else
            {
                emailForLogin = 0;
            }

            byte deleteUsers;
            if (reallyDeleteUsers)
            {
                deleteUsers = 1;
            }
            else
            {
                deleteUsers = 0;
            }

            byte allowNew;
            if (allowNewRegistration)
            {
                allowNew = 1;
            }
            else
            {
                allowNew = 0;
            }

            byte allowSkins;
            if (allowUserSkins)
            {
                allowSkins = 1;
            }
            else
            {
                allowSkins = 0;
            }

            byte secure;
            if (useSecureRegistration)
            {
                secure = 1;
            }
            else
            {
                secure = 0;
            }

            byte ssl;
            if (useSslOnAllPages)
            {
                ssl = 1;
            }
            else
            {
                ssl = 0;
            }

            byte adminSite;
            if (isServerAdminSite)
            {
                adminSite = 1;
            }
            else
            {
                adminSite = 0;
            }

            byte pageSkins;
            if (allowPageSkins)
            {
                pageSkins = 1;
            }
            else
            {
                pageSkins = 0;
            }

            byte allowHide;
            if (allowHideMenuOnPages)
            {
                allowHide = 1;
            }
            else
            {
                allowHide = 0;
            }

            byte enableMy;
            if (enableMyPageFeature)
            {
                enableMy = 1;
            }
            else
            {
                enableMy = 0;
            }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Sites ( ");
            sqlCommand.Append("SiteName, ");
            sqlCommand.Append("Skin, ");
            sqlCommand.Append("Logo, ");
            sqlCommand.Append("Icon, ");
            sqlCommand.Append("AllowNewRegistration, ");
            sqlCommand.Append("AllowUserSkins, ");
            sqlCommand.Append("UseSecureRegistration, ");
            sqlCommand.Append("EnableMyPageFeature, ");
            sqlCommand.Append("UseSSLOnAllPages, ");
            sqlCommand.Append("DefaultPageKeywords, ");
            sqlCommand.Append("DefaultPageDescription, ");
            sqlCommand.Append("DefaultPageEncoding, ");
            sqlCommand.Append("DefaultAdditionalMetaTags, ");
            sqlCommand.Append("IsServerAdminSite, ");
            sqlCommand.Append("AllowPageSkins, ");
            sqlCommand.Append("AllowHideMenuOnPages, ");

            sqlCommand.Append("UseLdapAuth, ");
            sqlCommand.Append("AutoCreateLDAPUserOnFirstLogin, ");
            sqlCommand.Append("LdapServer, ");
            sqlCommand.Append("LdapPort, ");
            sqlCommand.Append("LdapDomain, ");
            sqlCommand.Append("LdapRootDN, ");
            sqlCommand.Append("LdapUserDNKey, ");
            sqlCommand.Append("AllowUserFullNameChange, ");
            sqlCommand.Append("UseEmailForLogin, ");
            sqlCommand.Append("ReallyDeleteUsers, ");
            sqlCommand.Append("EditorProvider, ");
            sqlCommand.Append("EditorSkin, ");

            sqlCommand.Append("DatePickerProvider, ");
            sqlCommand.Append("CaptchaProvider, ");
            sqlCommand.Append("RecaptchaPrivateKey, ");
            sqlCommand.Append("RecaptchaPublicKey, ");
            sqlCommand.Append("WordpressAPIKey, ");
            sqlCommand.Append("WindowsLiveAppID, ");
            sqlCommand.Append("WindowsLiveKey, ");
            sqlCommand.Append("AllowOpenIDAuth, ");
            sqlCommand.Append("AllowWindowsLiveAuth, ");
            sqlCommand.Append("GmapApiKey, ");
            sqlCommand.Append("ApiKeyExtra1, ");
            sqlCommand.Append("ApiKeyExtra2, ");
            sqlCommand.Append("ApiKeyExtra3, ");
            sqlCommand.Append("ApiKeyExtra4, ");
            sqlCommand.Append("ApiKeyExtra5, ");
            sqlCommand.Append("DisableDbAuth, ");

            sqlCommand.Append("DefaultFriendlyUrlPatternEnum, ");
            sqlCommand.Append("SiteGuid ");
            sqlCommand.Append("  )");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(" ?SiteName , ");
            sqlCommand.Append(" ?Skin , ");
            sqlCommand.Append(" ?Logo, ");
            sqlCommand.Append(" ?Icon, ");
            sqlCommand.Append(" ?AllowNewRegistration, ");
            sqlCommand.Append(" ?AllowUserSkins, ");
            sqlCommand.Append(" ?UseSecureRegistration, ");
            sqlCommand.Append(" ?EnableMyPageFeature, ");
            sqlCommand.Append(" ?UseSSLOnAllPages, ");
            sqlCommand.Append(" ?DefaultPageKeywords, ");
            sqlCommand.Append(" ?DefaultPageDescription, ");
            sqlCommand.Append(" ?DefaultPageEncoding, ");
            sqlCommand.Append(" ?DefaultAdditionalMetaTags, ");
            sqlCommand.Append(" ?IsServerAdminSite, ");
            sqlCommand.Append(" ?AllowPageSkins, ");
            sqlCommand.Append(" ?AllowHideMenuOnPages, ");

            sqlCommand.Append(" ?UseLdapAuth, ");
            sqlCommand.Append(" ?AutoCreateLDAPUserOnFirstLogin, ");
            sqlCommand.Append(" ?LdapServer, ");
            sqlCommand.Append(" ?LdapPort, ");
            sqlCommand.Append(" ?LdapDomain, ");
            sqlCommand.Append(" ?LdapRootDN, ");
            sqlCommand.Append(" ?LdapUserDNKey, ");
            sqlCommand.Append(" ?AllowUserFullNameChange, ");
            sqlCommand.Append(" ?UseEmailForLogin, ");
            sqlCommand.Append(" ?ReallyDeleteUsers, ");
            sqlCommand.Append(" ?EditorProvider, ");
            sqlCommand.Append(" ?EditorSkin, ");

            sqlCommand.Append(" ?DatePickerProvider, ");
            sqlCommand.Append(" ?CaptchaProvider, ");
            sqlCommand.Append(" ?RecaptchaPrivateKey, ");
            sqlCommand.Append(" ?RecaptchaPublicKey, ");
            sqlCommand.Append(" ?WordpressAPIKey, ");
            sqlCommand.Append(" ?WindowsLiveAppID, ");
            sqlCommand.Append(" ?WindowsLiveKey, ");
            sqlCommand.Append(" ?AllowOpenIDAuth, ");
            sqlCommand.Append("?AllowWindowsLiveAuth, ");
            sqlCommand.Append("?GmapApiKey, ");
            sqlCommand.Append("?ApiKeyExtra1, ");
            sqlCommand.Append("?ApiKeyExtra2, ");
            sqlCommand.Append("?ApiKeyExtra3, ");
            sqlCommand.Append("?ApiKeyExtra4, ");
            sqlCommand.Append("?ApiKeyExtra5, ");
            sqlCommand.Append("?DisableDbAuth, ");

            sqlCommand.Append(" ?DefaultFriendlyUrlPattern, ");
            sqlCommand.Append(" ?SiteGuid ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[46];

            arParams[0] = new MySqlParameter("?SiteName", MySqlDbType.VarChar, 255);
            arParams[0].Value = siteName;

            arParams[1] = new MySqlParameter("?IsServerAdminSite", MySqlDbType.Int32);
            arParams[1].Value = adminSite;

            arParams[2] = new MySqlParameter("?Skin", MySqlDbType.VarChar, 100);
            arParams[2].Value = skin;

            arParams[3] = new MySqlParameter("?Logo", MySqlDbType.VarChar, 50);
            arParams[3].Value = logo;

            arParams[4] = new MySqlParameter("?Icon", MySqlDbType.VarChar, 50);
            arParams[4].Value = icon;

            arParams[5] = new MySqlParameter("?AllowNewRegistration", MySqlDbType.Int32);
            arParams[5].Value = allowNew;

            arParams[6] = new MySqlParameter("?AllowUserSkins", MySqlDbType.Int32);
            arParams[6].Value = allowSkins;

            arParams[7] = new MySqlParameter("?UseSecureRegistration", MySqlDbType.Int32);
            arParams[7].Value = secure;

            arParams[8] = new MySqlParameter("?EnableMyPageFeature", MySqlDbType.Int32);
            arParams[8].Value = enableMy;

            arParams[9] = new MySqlParameter("?UseSSLOnAllPages", MySqlDbType.Int32);
            arParams[9].Value = ssl;

            arParams[10] = new MySqlParameter("?DefaultPageKeywords", MySqlDbType.VarChar, 255);
            arParams[10].Value = defaultPageKeywords;

            arParams[11] = new MySqlParameter("?DefaultPageDescription", MySqlDbType.VarChar, 255);
            arParams[11].Value = defaultPageDescription;

            arParams[12] = new MySqlParameter("?DefaultPageEncoding", MySqlDbType.VarChar, 255);
            arParams[12].Value = defaultPageEncoding;

            arParams[13] = new MySqlParameter("?DefaultAdditionalMetaTags", MySqlDbType.VarChar, 255);
            arParams[13].Value = defaultAdditionalMetaTags;

            arParams[14] = new MySqlParameter("?AllowPageSkins", MySqlDbType.Int32);
            arParams[14].Value = pageSkins;

            arParams[15] = new MySqlParameter("?AllowHideMenuOnPages", MySqlDbType.Int32);
            arParams[15].Value = allowHide;

            arParams[16] = new MySqlParameter("?UseLdapAuth", MySqlDbType.Int32);
            arParams[16].Value = uldapp;

            arParams[17] = new MySqlParameter("?AutoCreateLDAPUserOnFirstLogin", MySqlDbType.Int32);
            arParams[17].Value = autoldapp;

            arParams[18] = new MySqlParameter("?LdapServer", MySqlDbType.VarChar, 255);
            arParams[18].Value = ldapServer;

            arParams[19] = new MySqlParameter("?LdapPort", MySqlDbType.Int32);
            arParams[19].Value = ldapPort;

            arParams[20] = new MySqlParameter("?LdapRootDN", MySqlDbType.VarChar, 255);
            arParams[20].Value = ldapRootDN;

            arParams[21] = new MySqlParameter("?LdapUserDNKey", MySqlDbType.VarChar, 255);
            arParams[21].Value = ldapUserDNKey;

            arParams[22] = new MySqlParameter("?AllowUserFullNameChange", MySqlDbType.Int32);
            arParams[22].Value = allowNameChange;

            arParams[23] = new MySqlParameter("?UseEmailForLogin", MySqlDbType.Int32);
            arParams[23].Value = emailForLogin;

            arParams[24] = new MySqlParameter("?ReallyDeleteUsers", MySqlDbType.Int32);
            arParams[24].Value = deleteUsers;

            arParams[25] = new MySqlParameter("?EditorSkin", MySqlDbType.VarChar, 50);
            arParams[25].Value = editorSkin;

            arParams[26] = new MySqlParameter("?DefaultFriendlyUrlPattern", MySqlDbType.VarChar, 50);
            arParams[26].Value = defaultFriendlyUrlPattern;

            arParams[27] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[27].Value = siteGuid.ToString();

            arParams[28] = new MySqlParameter("?LdapDomain", MySqlDbType.VarChar, 255);
            arParams[28].Value = ldapDomain;

            arParams[29] = new MySqlParameter("?EditorProvider", MySqlDbType.VarChar, 255);
            arParams[29].Value = editorProvider;

            arParams[30] = new MySqlParameter("?DatePickerProvider", MySqlDbType.VarChar, 255);
            arParams[30].Value = datePickerProvider;

            arParams[31] = new MySqlParameter("?CaptchaProvider", MySqlDbType.VarChar, 255);
            arParams[31].Value = captchaProvider;

            arParams[32] = new MySqlParameter("?RecaptchaPrivateKey", MySqlDbType.VarChar, 255);
            arParams[32].Value = recaptchaPrivateKey;

            arParams[33] = new MySqlParameter("?RecaptchaPublicKey", MySqlDbType.VarChar, 255);
            arParams[33].Value = recaptchaPublicKey;

            arParams[34] = new MySqlParameter("?WordpressAPIKey", MySqlDbType.VarChar, 255);
            arParams[34].Value = wordpressApiKey;

            arParams[35] = new MySqlParameter("?WindowsLiveAppID", MySqlDbType.VarChar, 255);
            arParams[35].Value = windowsLiveAppId;

            arParams[36] = new MySqlParameter("?WindowsLiveKey", MySqlDbType.VarChar, 255);
            arParams[36].Value = windowsLiveKey;

            arParams[37] = new MySqlParameter("?AllowOpenIDAuth", MySqlDbType.Int32);
            arParams[37].Value = oidauth;

            arParams[38] = new MySqlParameter("?AllowWindowsLiveAuth", MySqlDbType.Int32);
            arParams[38].Value = winliveauth;

            arParams[39] = new MySqlParameter("?GmapApiKey", MySqlDbType.VarChar, 255);
            arParams[39].Value = gmapApiKey;

            arParams[40] = new MySqlParameter("?ApiKeyExtra1", MySqlDbType.VarChar, 255);
            arParams[40].Value = apiKeyExtra1;

            arParams[41] = new MySqlParameter("?ApiKeyExtra2", MySqlDbType.VarChar, 255);
            arParams[41].Value = apiKeyExtra2;

            arParams[42] = new MySqlParameter("?ApiKeyExtra3", MySqlDbType.VarChar, 255);
            arParams[42].Value = apiKeyExtra3;

            arParams[43] = new MySqlParameter("?ApiKeyExtra4", MySqlDbType.VarChar, 255);
            arParams[43].Value = apiKeyExtra4;

            arParams[44] = new MySqlParameter("?ApiKeyExtra5", MySqlDbType.VarChar, 255);
            arParams[44].Value = apiKeyExtra5;

            arParams[45] = new MySqlParameter("?DisableDbAuth", MySqlDbType.Int32);
            arParams[45].Value = intDisableDbAuth;

            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            int newID = Convert.ToInt32(result);

            return newID;


        }

        public async Task<bool> Update(
            int siteId,
            string siteName,
            string skin,
            string logo,
            string icon,
            bool allowNewRegistration,
            bool allowUserSkins,
            bool allowPageSkins,
            bool allowHideMenuOnPages,
            bool useSecureRegistration,
            bool useSslOnAllPages,
            string defaultPageKeywords,
            string defaultPageDescription,
            string defaultPageEncoding,
            string defaultAdditionalMetaTags,
            bool isServerAdminSite,
            bool useLdapAuth,
            bool autoCreateLdapUserOnFirstLogin,
            string ldapServer,
            int ldapPort,
            String ldapDomain,
            string ldapRootDN,
            string ldapUserDNKey,
            bool allowUserFullNameChange,
            bool useEmailForLogin,
            bool reallyDeleteUsers,
            String editorSkin,
            String defaultFriendlyUrlPattern,
            bool enableMyPageFeature,
            string editorProvider,
            string datePickerProvider,
            string captchaProvider,
            string recaptchaPrivateKey,
            string recaptchaPublicKey,
            string wordpressApiKey,
            string windowsLiveAppId,
            string windowsLiveKey,
            bool allowOpenIdAuth,
            bool allowWindowsLiveAuth,
            string gmapApiKey,
            string apiKeyExtra1,
            string apiKeyExtra2,
            string apiKeyExtra3,
            string apiKeyExtra4,
            string apiKeyExtra5,
            bool disableDbAuth)
        {

            #region bool conversion

            byte intDisableDbAuth = 0;
            if (disableDbAuth) { intDisableDbAuth = 1; }

            byte oidauth = 0;
            if (allowOpenIdAuth) { oidauth = 1; }


            byte winliveauth = 0;
            if (allowWindowsLiveAuth)
            {
                winliveauth = 1;
            }


            byte uldapp = 0;
            if (useLdapAuth)
            {
                uldapp = 1;
            }


            byte autoldapp = 0;
            if (autoCreateLdapUserOnFirstLogin)
            {
                autoldapp = 1;
            }


            byte allowNameChange = 0;
            if (allowUserFullNameChange)
            {
                allowNameChange = 1;
            }


            byte emailForLogin = 0;
            if (useEmailForLogin)
            {
                emailForLogin = 1;
            }


            byte deleteUsers = 0;
            if (reallyDeleteUsers)
            {
                deleteUsers = 1;
            }


            byte allowNew = 0;
            if (allowNewRegistration)
            {
                allowNew = 1;
            }


            byte allowSkins = 0;
            if (allowUserSkins)
            {
                allowSkins = 1;
            }


            byte secure = 0;
            if (useSecureRegistration)
            {
                secure = 1;
            }


            byte ssl = 0;
            if (useSslOnAllPages)
            {
                ssl = 1;
            }


            byte adminSite = 0;
            if (isServerAdminSite)
            {
                adminSite = 1;
            }


            byte pageSkins = 0;
            if (allowPageSkins)
            {
                pageSkins = 1;
            }

            byte allowHide = 0;
            if (allowHideMenuOnPages)
            {
                allowHide = 1;
            }


            byte enableMy = 0;
            if (enableMyPageFeature)
            {
                enableMy = 1;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET SiteName = ?SiteName, ");
            sqlCommand.Append("IsServerAdminSite = ?IsServerAdminSite, ");
            sqlCommand.Append("Skin = ?Skin, ");
            sqlCommand.Append("Logo = ?Logo, ");
            sqlCommand.Append("Icon = ?Icon, ");

            sqlCommand.Append("AllowNewRegistration = ?AllowNewRegistration, ");
            sqlCommand.Append("AllowUserSkins = ?AllowUserSkins, ");

            sqlCommand.Append("AllowPageSkins = ?AllowPageSkins, ");
            sqlCommand.Append("AllowHideMenuOnPages = ?AllowHideMenuOnPages, ");


            sqlCommand.Append("UseSecureRegistration = ?UseSecureRegistration, ");
            sqlCommand.Append("EnableMyPageFeature = ?EnableMyPageFeature, ");
            sqlCommand.Append("UseSSLOnAllPages = ?UseSSLOnAllPages, ");


            sqlCommand.Append("UseLdapAuth = ?UseLdapAuth, ");
            sqlCommand.Append("AutoCreateLDAPUserOnFirstLogin = ?AutoCreateLDAPUserOnFirstLogin, ");
            sqlCommand.Append("LdapServer = ?LdapServer, ");
            sqlCommand.Append("LdapPort = ?LdapPort, ");
            sqlCommand.Append("LdapDomain = ?LdapDomain, ");
            sqlCommand.Append("LdapRootDN = ?LdapRootDN, ");
            sqlCommand.Append("LdapUserDNKey = ?LdapUserDNKey, ");
            sqlCommand.Append("AllowUserFullNameChange = ?AllowUserFullNameChange, ");
            sqlCommand.Append("UseEmailForLogin = ?UseEmailForLogin, ");
            sqlCommand.Append("ReallyDeleteUsers = ?ReallyDeleteUsers, ");
            sqlCommand.Append("EditorSkin = ?EditorSkin, ");
            sqlCommand.Append("EditorProvider = ?EditorProvider, ");
            sqlCommand.Append("DefaultFriendlyUrlPatternEnum = ?DefaultFriendlyUrlPattern, ");

            sqlCommand.Append("DatePickerProvider = ?DatePickerProvider, ");
            sqlCommand.Append("CaptchaProvider = ?CaptchaProvider, ");
            sqlCommand.Append("RecaptchaPrivateKey = ?RecaptchaPrivateKey, ");
            sqlCommand.Append("RecaptchaPublicKey = ?RecaptchaPublicKey, ");
            sqlCommand.Append("WordpressAPIKey = ?WordpressAPIKey, ");
            sqlCommand.Append("WindowsLiveAppID = ?WindowsLiveAppID, ");
            sqlCommand.Append("WindowsLiveKey = ?WindowsLiveKey, ");
            sqlCommand.Append("AllowOpenIDAuth = ?AllowOpenIDAuth, ");
            sqlCommand.Append("AllowWindowsLiveAuth = ?AllowWindowsLiveAuth, ");
            sqlCommand.Append("GmapApiKey = ?GmapApiKey, ");
            sqlCommand.Append("ApiKeyExtra1 = ?ApiKeyExtra1, ");
            sqlCommand.Append("ApiKeyExtra2 = ?ApiKeyExtra2, ");
            sqlCommand.Append("ApiKeyExtra3 = ?ApiKeyExtra3, ");
            sqlCommand.Append("ApiKeyExtra4 = ?ApiKeyExtra4, ");
            sqlCommand.Append("ApiKeyExtra5 = ?ApiKeyExtra5, ");
            sqlCommand.Append("DisableDbAuth = ?DisableDbAuth, ");

            sqlCommand.Append("DefaultPageKeywords = ?DefaultPageKeywords, ");
            sqlCommand.Append("DefaultPageDescription = ?DefaultPageDescription, ");
            sqlCommand.Append("DefaultPageEncoding = ?DefaultPageEncoding, ");
            sqlCommand.Append("DefaultAdditionalMetaTags = ?DefaultAdditionalMetaTags ");

            sqlCommand.Append(" WHERE SiteID = ?SiteID ;");

            MySqlParameter[] arParams = new MySqlParameter[46];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?SiteName", MySqlDbType.VarChar, 128);
            arParams[1].Value = siteName;

            arParams[2] = new MySqlParameter("?IsServerAdminSite", MySqlDbType.Int32);
            arParams[2].Value = adminSite;

            arParams[3] = new MySqlParameter("?Skin", MySqlDbType.VarChar, 100);
            arParams[3].Value = skin;

            arParams[4] = new MySqlParameter("?Logo", MySqlDbType.VarChar, 50);
            arParams[4].Value = logo;

            arParams[5] = new MySqlParameter("?Icon", MySqlDbType.VarChar, 50);
            arParams[5].Value = icon;

            arParams[6] = new MySqlParameter("?AllowNewRegistration", MySqlDbType.Int32);
            arParams[6].Value = allowNew;

            arParams[7] = new MySqlParameter("?AllowUserSkins", MySqlDbType.Int32);
            arParams[7].Value = allowSkins;

            arParams[8] = new MySqlParameter("?UseSecureRegistration", MySqlDbType.Int32);
            arParams[8].Value = secure;

            arParams[9] = new MySqlParameter("?EnableMyPageFeature", MySqlDbType.Int32);
            arParams[9].Value = enableMy;

            arParams[10] = new MySqlParameter("?UseSSLOnAllPages", MySqlDbType.Int32);
            arParams[10].Value = ssl;

            arParams[11] = new MySqlParameter("?DefaultPageKeywords", MySqlDbType.VarChar, 255);
            arParams[11].Value = defaultPageKeywords;

            arParams[12] = new MySqlParameter("?DefaultPageDescription", MySqlDbType.VarChar, 255);
            arParams[12].Value = defaultPageDescription;

            arParams[13] = new MySqlParameter("?DefaultPageEncoding", MySqlDbType.VarChar, 255);
            arParams[13].Value = defaultPageEncoding;

            arParams[14] = new MySqlParameter("?DefaultAdditionalMetaTags", MySqlDbType.VarChar, 255);
            arParams[14].Value = defaultAdditionalMetaTags;

            arParams[15] = new MySqlParameter("?AllowPageSkins", MySqlDbType.Int32);
            arParams[15].Value = pageSkins;

            arParams[16] = new MySqlParameter("?AllowHideMenuOnPages", MySqlDbType.Int32);
            arParams[16].Value = allowHide;

            arParams[17] = new MySqlParameter("?UseLdapAuth", MySqlDbType.Int32);
            arParams[17].Value = uldapp;

            arParams[18] = new MySqlParameter("?AutoCreateLDAPUserOnFirstLogin", MySqlDbType.Int32);
            arParams[18].Value = autoldapp;

            arParams[19] = new MySqlParameter("?LdapServer", MySqlDbType.VarChar, 255);
            arParams[19].Value = ldapServer;

            arParams[20] = new MySqlParameter("?LdapPort", MySqlDbType.Int32);
            arParams[20].Value = ldapPort;

            arParams[21] = new MySqlParameter("?LdapRootDN", MySqlDbType.VarChar, 255);
            arParams[21].Value = ldapRootDN;

            arParams[22] = new MySqlParameter("?LdapUserDNKey", MySqlDbType.VarChar, 10);
            arParams[22].Value = ldapUserDNKey;

            arParams[23] = new MySqlParameter("?AllowUserFullNameChange", MySqlDbType.Int32);
            arParams[23].Value = allowNameChange;

            arParams[24] = new MySqlParameter("?UseEmailForLogin", MySqlDbType.Int32);
            arParams[24].Value = emailForLogin;

            arParams[25] = new MySqlParameter("?ReallyDeleteUsers", MySqlDbType.Int32);
            arParams[25].Value = deleteUsers;

            arParams[26] = new MySqlParameter("?EditorSkin", MySqlDbType.VarChar, 50);
            arParams[26].Value = editorSkin;

            arParams[27] = new MySqlParameter("?DefaultFriendlyUrlPattern", MySqlDbType.VarChar, 50);
            arParams[27].Value = defaultFriendlyUrlPattern;

            arParams[28] = new MySqlParameter("?LdapDomain", MySqlDbType.VarChar, 255);
            arParams[28].Value = ldapDomain;

            arParams[29] = new MySqlParameter("?EditorProvider", MySqlDbType.VarChar, 255);
            arParams[29].Value = editorProvider;

            arParams[30] = new MySqlParameter("?DatePickerProvider", MySqlDbType.VarChar, 255);
            arParams[30].Value = datePickerProvider;

            arParams[31] = new MySqlParameter("?CaptchaProvider", MySqlDbType.VarChar, 255);
            arParams[31].Value = captchaProvider;

            arParams[32] = new MySqlParameter("?RecaptchaPrivateKey", MySqlDbType.VarChar, 255);
            arParams[32].Value = recaptchaPrivateKey;

            arParams[33] = new MySqlParameter("?RecaptchaPublicKey", MySqlDbType.VarChar, 255);
            arParams[33].Value = recaptchaPublicKey;

            arParams[34] = new MySqlParameter("?WordpressAPIKey", MySqlDbType.VarChar, 255);
            arParams[34].Value = wordpressApiKey;

            arParams[35] = new MySqlParameter("?WindowsLiveAppID", MySqlDbType.VarChar, 255);
            arParams[35].Value = windowsLiveAppId;

            arParams[36] = new MySqlParameter("?WindowsLiveKey", MySqlDbType.VarChar, 255);
            arParams[36].Value = windowsLiveKey;

            arParams[37] = new MySqlParameter("?AllowOpenIDAuth", MySqlDbType.Int32);
            arParams[37].Value = oidauth;

            arParams[38] = new MySqlParameter("?AllowWindowsLiveAuth", MySqlDbType.Int32);
            arParams[38].Value = winliveauth;

            arParams[39] = new MySqlParameter("?GmapApiKey", MySqlDbType.VarChar, 255);
            arParams[39].Value = gmapApiKey;

            arParams[40] = new MySqlParameter("?ApiKeyExtra1", MySqlDbType.VarChar, 255);
            arParams[40].Value = apiKeyExtra1;

            arParams[41] = new MySqlParameter("?ApiKeyExtra2", MySqlDbType.VarChar, 255);
            arParams[41].Value = apiKeyExtra2;

            arParams[42] = new MySqlParameter("?ApiKeyExtra3", MySqlDbType.VarChar, 255);
            arParams[42].Value = apiKeyExtra3;

            arParams[43] = new MySqlParameter("?ApiKeyExtra4", MySqlDbType.VarChar, 255);
            arParams[43].Value = apiKeyExtra4;

            arParams[44] = new MySqlParameter("?ApiKeyExtra5", MySqlDbType.VarChar, 255);
            arParams[44].Value = apiKeyExtra5;

            arParams[45] = new MySqlParameter("?DisableDbAuth", MySqlDbType.Int32);
            arParams[45].Value = intDisableDbAuth;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public bool UpdateRelatedSites(
            int siteId,
            bool allowNewRegistration,
            bool useSecureRegistration,
            bool useLdapAuth,
            bool autoCreateLdapUserOnFirstLogin,
            string ldapServer,
            string ldapDomain,
            int ldapPort,
            string ldapRootDN,
            string ldapUserDNKey,
            bool allowUserFullNameChange,
            bool useEmailForLogin,
            bool allowOpenIdAuth,
            bool allowWindowsLiveAuth,
            bool allowPasswordRetrieval,
            bool allowPasswordReset,
            bool requiresQuestionAndAnswer,
            int maxInvalidPasswordAttempts,
            int passwordAttemptWindowMinutes,
            bool requiresUniqueEmail,
            int passwordFormat,
            int minRequiredPasswordLength,
            int minReqNonAlphaChars,
            string pwdStrengthRegex
            )
        {

            #region bool conversion

            byte oidauth = 0;
            if (allowOpenIdAuth)
            {
                oidauth = 1;
            }


            byte winliveauth = 0;
            if (allowWindowsLiveAuth)
            {
                winliveauth = 1;
            }


            byte uldapp = 0;
            if (useLdapAuth)
            {
                uldapp = 1;
            }


            byte autoldapp = 0;
            if (autoCreateLdapUserOnFirstLogin)
            {
                autoldapp = 1;
            }


            byte allowNameChange = 0;
            if (allowUserFullNameChange)
            {
                allowNameChange = 1;
            }


            byte emailForLogin = 0;
            if (useEmailForLogin)
            {
                emailForLogin = 1;
            }

            byte allowNew = 0;
            if (allowNewRegistration)
            {
                allowNew = 1;
            }


            byte secure = 0;
            if (useSecureRegistration)
            {
                secure = 1;
            }

            int intAllowPasswordRetrieval = 0;
            if (allowPasswordRetrieval)
            {
                intAllowPasswordRetrieval = 1;
            }

            int intAllowPasswordReset = 0;
            if (allowPasswordReset)
            {
                intAllowPasswordReset = 1;
            }

            int intRequiresQuestionAndAnswer = 0;
            if (requiresQuestionAndAnswer)
            {
                intRequiresQuestionAndAnswer = 1;
            }

            int intRequiresUniqueEmail = 0;
            if (requiresUniqueEmail)
            {
                intRequiresUniqueEmail = 1;
            }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET ");

            sqlCommand.Append("AllowNewRegistration = ?AllowNewRegistration, ");
            sqlCommand.Append("UseSecureRegistration = ?UseSecureRegistration, ");
            sqlCommand.Append("UseLdapAuth = ?UseLdapAuth, ");
            sqlCommand.Append("AutoCreateLDAPUserOnFirstLogin = ?AutoCreateLDAPUserOnFirstLogin, ");
            sqlCommand.Append("LdapServer = ?LdapServer, ");
            sqlCommand.Append("LdapPort = ?LdapPort, ");
            sqlCommand.Append("LdapDomain = ?LdapDomain, ");
            sqlCommand.Append("LdapRootDN = ?LdapRootDN, ");
            sqlCommand.Append("LdapUserDNKey = ?LdapUserDNKey, ");
            sqlCommand.Append("AllowUserFullNameChange = ?AllowUserFullNameChange, ");
            sqlCommand.Append("UseEmailForLogin = ?UseEmailForLogin, ");
            sqlCommand.Append("AllowOpenIDAuth = ?AllowOpenIDAuth, ");
            sqlCommand.Append("AllowWindowsLiveAuth = ?AllowWindowsLiveAuth, ");
            sqlCommand.Append("AllowPasswordRetrieval = ?AllowPasswordRetrieval, ");
            sqlCommand.Append("AllowPasswordReset = ?AllowPasswordReset, ");
            sqlCommand.Append("RequiresQuestionAndAnswer = ?RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts = ?MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("PasswordAttemptWindowMinutes = ?PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("RequiresUniqueEmail = ?RequiresUniqueEmail, ");
            sqlCommand.Append("PasswordFormat = ?PasswordFormat, ");
            sqlCommand.Append("MinRequiredPasswordLength = ?MinRequiredPasswordLength, ");
            sqlCommand.Append("MinReqNonAlphaChars = ?MinReqNonAlphaChars, ");
            sqlCommand.Append("PwdStrengthRegex = ?PwdStrengthRegex ");


            sqlCommand.Append(" WHERE SiteID <> ?SiteID ;");

            MySqlParameter[] arParams = new MySqlParameter[24];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?AllowNewRegistration", MySqlDbType.Int32);
            arParams[1].Value = allowNew;

            arParams[2] = new MySqlParameter("?UseSecureRegistration", MySqlDbType.Int32);
            arParams[2].Value = secure;

            arParams[3] = new MySqlParameter("?UseLdapAuth", MySqlDbType.Int32);
            arParams[3].Value = uldapp;

            arParams[4] = new MySqlParameter("?AutoCreateLDAPUserOnFirstLogin", MySqlDbType.Int32);
            arParams[4].Value = autoldapp;

            arParams[5] = new MySqlParameter("?LdapServer", MySqlDbType.VarChar, 255);
            arParams[5].Value = ldapServer;

            arParams[6] = new MySqlParameter("?LdapPort", MySqlDbType.Int32);
            arParams[6].Value = ldapPort;

            arParams[7] = new MySqlParameter("?LdapRootDN", MySqlDbType.VarChar, 255);
            arParams[7].Value = ldapRootDN;

            arParams[8] = new MySqlParameter("?LdapUserDNKey", MySqlDbType.VarChar, 10);
            arParams[8].Value = ldapUserDNKey;

            arParams[9] = new MySqlParameter("?AllowUserFullNameChange", MySqlDbType.Int32);
            arParams[9].Value = allowNameChange;

            arParams[10] = new MySqlParameter("?UseEmailForLogin", MySqlDbType.Int32);
            arParams[10].Value = emailForLogin;

            arParams[11] = new MySqlParameter("?LdapDomain", MySqlDbType.VarChar, 255);
            arParams[11].Value = ldapDomain;

            arParams[12] = new MySqlParameter("?AllowOpenIDAuth", MySqlDbType.Int32);
            arParams[12].Value = oidauth;

            arParams[13] = new MySqlParameter("?AllowWindowsLiveAuth", MySqlDbType.Int32);
            arParams[13].Value = winliveauth;

            arParams[14] = new MySqlParameter("?AllowPasswordRetrieval", MySqlDbType.Int32);
            arParams[14].Value = intAllowPasswordRetrieval;

            arParams[15] = new MySqlParameter("?AllowPasswordReset", MySqlDbType.Int32);
            arParams[15].Value = intAllowPasswordReset;

            arParams[16] = new MySqlParameter("?RequiresQuestionAndAnswer", MySqlDbType.Int32);
            arParams[16].Value = intRequiresQuestionAndAnswer;

            arParams[17] = new MySqlParameter("?MaxInvalidPasswordAttempts", MySqlDbType.Int32);
            arParams[17].Value = maxInvalidPasswordAttempts;

            arParams[18] = new MySqlParameter("?PasswordAttemptWindowMinutes", MySqlDbType.Int32);
            arParams[18].Value = passwordAttemptWindowMinutes;

            arParams[19] = new MySqlParameter("?RequiresUniqueEmail", MySqlDbType.Int32);
            arParams[19].Value = intRequiresUniqueEmail;

            arParams[20] = new MySqlParameter("?PasswordFormat", MySqlDbType.Int32);
            arParams[20].Value = passwordFormat;

            arParams[21] = new MySqlParameter("?MinRequiredPasswordLength", MySqlDbType.Int32);
            arParams[21].Value = minRequiredPasswordLength;

            arParams[22] = new MySqlParameter("?MinReqNonAlphaChars", MySqlDbType.Int32);
            arParams[22].Value = minReqNonAlphaChars;

            arParams[23] = new MySqlParameter("?PwdStrengthRegex", MySqlDbType.Text);
            arParams[23].Value = pwdStrengthRegex;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);


            return (rowsAffected > 0);


        }

        public bool UpdateRelatedSitesWindowsLive(
            int siteId,
            string windowsLiveAppId,
            string windowsLiveKey
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("WindowsLiveAppID = ?WindowsLiveAppID, ");
            sqlCommand.Append("WindowsLiveKey = ?WindowsLiveKey ");

            sqlCommand.Append(" WHERE SiteID <> ?SiteID ;");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?WindowsLiveAppID", MySqlDbType.VarChar, 255);
            arParams[1].Value = windowsLiveAppId;

            arParams[2] = new MySqlParameter("?WindowsLiveKey", MySqlDbType.VarChar, 255);
            arParams[2].Value = windowsLiveKey;


            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);


            return (rowsAffected > 0);

        }

        public bool UpdateExtendedProperties(
            int siteId,
            bool allowPasswordRetrieval,
            bool allowPasswordReset,
            bool requiresQuestionAndAnswer,
            int maxInvalidPasswordAttempts,
            int passwordAttemptWindowMinutes,
            bool requiresUniqueEmail,
            int passwordFormat,
            int minRequiredPasswordLength,
            int minRequiredNonAlphanumericCharacters,
            string passwordStrengthRegularExpression,
            string defaultEmailFromAddress
            )
        {
            #region bit conversion

            byte allowRetrieval;
            if (allowPasswordRetrieval)
            {
                allowRetrieval = 1;
            }
            else
            {
                allowRetrieval = 0;
            }

            byte allowReset;
            if (allowPasswordReset)
            {
                allowReset = 1;
            }
            else
            {
                allowReset = 0;
            }

            byte requiresQA;
            if (requiresQuestionAndAnswer)
            {
                requiresQA = 1;
            }
            else
            {
                requiresQA = 0;
            }

            byte requiresEmail;
            if (requiresUniqueEmail)
            {
                requiresEmail = 1;
            }
            else
            {
                requiresEmail = 0;
            }

            #endregion

            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET AllowPasswordRetrieval = ?AllowPasswordRetrieval, ");
            sqlCommand.Append("AllowPasswordReset = ?AllowPasswordReset, ");
            sqlCommand.Append("RequiresQuestionAndAnswer = ?RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts = ?MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("PasswordAttemptWindowMinutes = ?PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("RequiresUniqueEmail = ?RequiresUniqueEmail, ");
            sqlCommand.Append("PasswordFormat = ?PasswordFormat, ");
            sqlCommand.Append("MinRequiredPasswordLength = ?MinRequiredPasswordLength, ");
            sqlCommand.Append("MinReqNonAlphaChars = ?MinRequiredNonAlphanumericCharacters, ");
            sqlCommand.Append("PwdStrengthRegex = ?PasswordStrengthRegularExpression, ");
            sqlCommand.Append("DefaultEmailFromAddress = ?DefaultEmailFromAddress ");


            sqlCommand.Append(" WHERE SiteID = ?SiteID ;");

            MySqlParameter[] arParams = new MySqlParameter[12];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?AllowPasswordRetrieval", MySqlDbType.Int32);
            arParams[1].Value = allowRetrieval;

            arParams[2] = new MySqlParameter("?AllowPasswordReset", MySqlDbType.Int32);
            arParams[2].Value = allowReset;

            arParams[3] = new MySqlParameter("?RequiresQuestionAndAnswer", MySqlDbType.Int32);
            arParams[3].Value = requiresQA;

            arParams[4] = new MySqlParameter("?MaxInvalidPasswordAttempts", MySqlDbType.Int32);
            arParams[4].Value = maxInvalidPasswordAttempts;

            arParams[5] = new MySqlParameter("?PasswordAttemptWindowMinutes", MySqlDbType.Int32);
            arParams[5].Value = passwordAttemptWindowMinutes;

            arParams[6] = new MySqlParameter("?RequiresUniqueEmail", MySqlDbType.Int32);
            arParams[6].Value = requiresEmail;

            arParams[7] = new MySqlParameter("?PasswordFormat", MySqlDbType.Int32);
            arParams[7].Value = passwordFormat;

            arParams[8] = new MySqlParameter("?MinRequiredPasswordLength", MySqlDbType.Int32);
            arParams[8].Value = minRequiredPasswordLength;

            arParams[9] = new MySqlParameter("?PasswordStrengthRegularExpression", MySqlDbType.Text);
            arParams[9].Value = passwordStrengthRegularExpression;

            arParams[10] = new MySqlParameter("?DefaultEmailFromAddress", MySqlDbType.VarChar, 100);
            arParams[10].Value = defaultEmailFromAddress;

            arParams[11] = new MySqlParameter("?MinRequiredNonAlphanumericCharacters", MySqlDbType.Int32);
            arParams[11].Value = minRequiredNonAlphanumericCharacters;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public async Task<bool> Delete(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_UserProperties WHERE UserGuid IN (SELECT UserGuid FROM mp_Users WHERE SiteID = ?SiteID);");
            sqlCommand.Append("DELETE FROM mp_UserRoles WHERE UserID IN (SELECT UserID FROM mp_Users WHERE SiteID = ?SiteID);");
            sqlCommand.Append("DELETE FROM mp_UserLocation WHERE UserGuid IN (SELECT UserGuid FROM mp_Users WHERE SiteID = ?SiteID);");

            sqlCommand.Append("DELETE FROM mp_Users WHERE SiteID = ?SiteID; ");

            sqlCommand.Append("DELETE FROM mp_Roles WHERE SiteID = ?SiteID; ");
            sqlCommand.Append("DELETE FROM mp_SiteHosts WHERE SiteID = ?SiteID; ");


            sqlCommand.Append("DELETE FROM mp_SiteFolders WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID);");
            sqlCommand.Append("DELETE FROM mp_SiteSettingsEx WHERE SiteID = ?SiteID; ");

            sqlCommand.Append("DELETE FROM mp_Sites ");
            sqlCommand.Append("WHERE SiteID = ?SiteID  ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        //public static bool HasFeature(int siteId, int moduleDefId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT Count(*) FROM mp_SiteModuleDefinitions ");
        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("SiteID = ?SiteID ");
        //    sqlCommand.Append(" AND ");
        //    sqlCommand.Append("ModuleDefID = ?ModuleDefID ");
        //    sqlCommand.Append(" ;");

        //    MySqlParameter[] arParams = new MySqlParameter[2];

        //    arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
        //    arParams[1].Value = moduleDefId;

        //    int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        ConnectionString.GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams));

        //    return (count > 0);

        //}

        public async Task<int> GetHostCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append(";");

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                sqlCommand.ToString(),
                null);

            return Convert.ToInt32(result);
        }

        public async Task<DbDataReader> GetAllHosts()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append("ORDER BY HostName  ");
            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                null);
        }

        public DbDataReader GetAllHostsNonAsync()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append("ORDER BY HostName  ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                null);
        }

        public async Task<DbDataReader> GetPageHosts(
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetHostCount();

            //if (pageSize > 0) totalPages = totalRows / pageSize;

            //if (totalRows <= pageSize)
            //{
            //    totalPages = 1;
            //}
            //else
            //{
            //    int remainder;
            //    Math.DivRem(totalRows, pageSize, out remainder);
            //    if (remainder > 0)
            //    {
            //        totalPages += 1;
            //    }
            //}

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_SiteHosts  ");
            //sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ORDER BY HostName  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[0].Value = pageSize;

            arParams[1] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[1].Value = pageLowerBound;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);
        }


        public async Task<DbDataReader> GetHostList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append("WHERE SiteID = ?SiteID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);


        }

        public async Task<DbDataReader> GetHost(string hostName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append("WHERE HostName = ?HostName ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?HostName", MySqlDbType.VarChar, 255);
            arParams[0].Value = hostName;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);
        }

        public async Task<bool> AddHost(Guid siteGuid, int siteId, string hostName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("INSERT INTO mp_SiteHosts ");
            sqlCommand.Append("( ");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("HostName ");
            sqlCommand.Append(") ");

            sqlCommand.Append("VALUES ");
            sqlCommand.Append("( ");
            sqlCommand.Append("?SiteID, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?HostName ");
            sqlCommand.Append(") ;");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?HostName", MySqlDbType.VarChar, 255);
            arParams[1].Value = hostName;

            arParams[2] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[2].Value = siteGuid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected > 0;

        }

        public async Task<bool> DeleteHost(int hostId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE HostID = ?HostID  ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?HostID", MySqlDbType.Int32);
            arParams[0].Value = hostId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected > 0;

        }

        public async Task<DbDataReader> GetSiteList()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Sites ");

            sqlCommand.Append("ORDER BY	SiteName ;");
            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString());
        }

        public int GetFirstSiteID()
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COALESCE(SiteID, -1) ");

            sqlCommand.Append("FROM	mp_Sites ");

            sqlCommand.Append("ORDER BY	SiteID LIMIT 1 ;");

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                sqlCommand.ToString()));
        }

        public Guid GetSiteGuidFromID(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT SiteGuid ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID = ?SiteID ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            Guid siteGuid = Guid.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams))
            {

                if (reader.Read())
                {
                    siteGuid = new Guid(reader["SiteGuid"].ToString());
                }
            }

            return siteGuid;
        }

        public int GetSiteIDFromGuid(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT SiteID ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteGuid = ?SiteGuid ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            int siteID = -1;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams))
            {

                if (reader.Read())
                {
                    siteID = Convert.ToInt32(reader["SiteID"].ToString(), CultureInfo.InvariantCulture);
                }
            }

            return siteID;
        }

        public async Task<DbDataReader> GetSite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID = ?SiteID ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public DbDataReader GetSiteNonAsync(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID = ?SiteID ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public async Task<DbDataReader> GetSite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteGuid = ?SiteGuid ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);
        }



        public async Task<DbDataReader> GetSite(string hostName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?HostName", MySqlDbType.VarChar, 255);
            arParams[0].Value = hostName;

            int siteId = -1;

            sqlCommand.Append("SELECT mp_SiteHosts.SiteID ");
            sqlCommand.Append("FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE mp_SiteHosts.HostName = ?HostName ;");

            using (DbDataReader reader = await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    siteId = Convert.ToInt32(reader["SiteID"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID = ?SiteID OR ?SiteID = -1 ");
            sqlCommand.Append("ORDER BY	SiteID ");
            sqlCommand.Append("LIMIT 1 ;");

            arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetSiteNonAsync(string hostName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?HostName", MySqlDbType.VarChar, 255);
            arParams[0].Value = hostName;

            int siteId = -1;

            sqlCommand.Append("SELECT mp_SiteHosts.SiteID ");
            sqlCommand.Append("FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE mp_SiteHosts.HostName = ?HostName ;");

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    siteId = Convert.ToInt32(reader["SiteID"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID = ?SiteID OR ?SiteID = -1 ");
            sqlCommand.Append("ORDER BY	SiteID ");
            sqlCommand.Append("LIMIT 1 ;");

            arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }



        public DbDataReader GetPageListForAdmin(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("PageID, ");
            sqlCommand.Append("ParentID, ");
            sqlCommand.Append("PageOrder, ");
            sqlCommand.Append("PageName ");

            sqlCommand.Append("FROM	mp_Pages ");

            sqlCommand.Append("WHERE SiteID = ?SiteID ");
            sqlCommand.Append("ORDER BY ParentID,  PageName ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);
        }

        public async Task<int> CountOtherSites(int currentSiteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID <> ?CurrentSiteID ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CurrentSiteID", MySqlDbType.Int32);
            arParams[0].Value = currentSiteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

            return Convert.ToInt32(result);

        }

        public async Task<DbDataReader> GetPageOfOtherSites(
            int currentSiteId,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = CountOtherSites(currentSiteId);

            //if (pageSize > 0) totalPages = totalRows / pageSize;

            //if (totalRows <= pageSize)
            //{
            //    totalPages = 1;
            //}
            //else
            //{
            //    int remainder;
            //    Math.DivRem(totalRows, pageSize, out remainder);
            //    if (remainder > 0)
            //    {
            //        totalPages += 1;
            //    }
            //}

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_Sites  ");
            sqlCommand.Append("WHERE SiteID <> ?CurrentSiteID ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("SiteName  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?CurrentSiteID", MySqlDbType.Int32);
            arParams[0].Value = currentSiteId;

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[2].Value = pageLowerBound;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }


        public async Task<int> GetSiteIdByHostName(string hostName)
        {
            int siteId = -1;

            StringBuilder sqlCommand = new StringBuilder();

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?HostName", MySqlDbType.VarChar, 255);
            arParams[0].Value = hostName;

            sqlCommand.Append("SELECT SiteID ");
            sqlCommand.Append("FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE HostName = ?HostName ;");

            using (DbDataReader reader = await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    siteId = Convert.ToInt32(reader["SiteID"]);
                }
            }

            if (siteId == -1)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("SELECT SiteID ");
                sqlCommand.Append("FROM	mp_Sites ");

                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append("LIMIT 1 ;");

                using (DbDataReader reader = await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                null))
                {
                    if (reader.Read())
                    {
                        siteId = Convert.ToInt32(reader["SiteID"]);
                    }
                }

            }

            return siteId;

        }

        public async Task<int> GetSiteIdByFolder(string folderName)
        {
            int siteId = -1;

            StringBuilder sqlCommand = new StringBuilder();

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FolderName", MySqlDbType.VarChar, 255);
            arParams[0].Value = folderName;

            sqlCommand.Append("SELECT COALESCE(s.SiteID, -1) AS SiteID ");
            sqlCommand.Append("FROM mp_SiteFolders sf ");
            sqlCommand.Append("JOIN mp_Sites s ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("sf.SiteGuid = s.SiteGuid ");
            sqlCommand.Append("WHERE sf.FolderName = ?FolderName ");
            sqlCommand.Append("ORDER BY s.SiteID ");
            sqlCommand.Append(";");

            using (DbDataReader reader = await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    siteId = Convert.ToInt32(reader["SiteID"]);
                }
            }

            if (siteId == -1)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("SELECT SiteID ");
                sqlCommand.Append("FROM	mp_Sites ");

                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append("LIMIT 1 ;");

                using (DbDataReader reader = await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                null))
                {
                    if (reader.Read())
                    {
                        siteId = Convert.ToInt32(reader["SiteID"]);
                    }
                }

            }

            return siteId;

        }

        public int GetSiteIdByFolderNonAsync(string folderName)
        {
            int siteId = -1;

            StringBuilder sqlCommand = new StringBuilder();

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FolderName", MySqlDbType.VarChar, 255);
            arParams[0].Value = folderName;

            sqlCommand.Append("SELECT COALESCE(s.SiteID, -1) AS SiteID ");
            sqlCommand.Append("FROM mp_SiteFolders sf ");
            sqlCommand.Append("JOIN mp_Sites s ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("sf.SiteGuid = s.SiteGuid ");
            sqlCommand.Append("WHERE sf.FolderName = ?FolderName ");
            sqlCommand.Append("ORDER BY s.SiteID ");
            sqlCommand.Append(";");

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    siteId = Convert.ToInt32(reader["SiteID"]);
                }
            }

            if (siteId == -1)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("SELECT SiteID ");
                sqlCommand.Append("FROM	mp_Sites ");

                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append("LIMIT 1 ;");

                using (DbDataReader reader = AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                null))
                {
                    if (reader.Read())
                    {
                        siteId = Convert.ToInt32(reader["SiteID"]);
                    }
                }

            }

            return siteId;

        }

    }
}
