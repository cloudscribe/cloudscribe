// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-06-23
// 

using cloudscribe.DbHelpers.Sqlite;
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Framework.Logging;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{
    internal class DBSiteSettings
    {
        internal DBSiteSettings(
            string dbConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            connectionString = dbConnectionString;


        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string connectionString;




        public int Create(
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

            byte enableMyPage;
            if (enableMyPageFeature)
            {
                enableMyPage = 1;
            }
            else
            {
                enableMyPage = 0;
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

            sqlCommand.Append(" :SiteName , ");
            sqlCommand.Append(" :Skin , ");
            sqlCommand.Append(" :Logo, ");
            sqlCommand.Append(" :Icon, ");
            sqlCommand.Append(" :AllowNewRegistration, ");
            sqlCommand.Append(" :AllowUserSkins, ");
            sqlCommand.Append(" :UseSecureRegistration, ");
            sqlCommand.Append(" :EnableMyPageFeature, ");
            sqlCommand.Append(" :UseSSLOnAllPages, ");
            sqlCommand.Append(" :DefaultPageKeywords, ");
            sqlCommand.Append(" :DefaultPageDescription, ");
            sqlCommand.Append(" :DefaultPageEncoding, ");
            sqlCommand.Append(" :DefaultAdditionalMetaTags, ");
            sqlCommand.Append(" :IsServerAdminSite, ");
            sqlCommand.Append(" :AllowPageSkins, ");
            sqlCommand.Append(" :AllowHideMenuOnPages, ");

            sqlCommand.Append(" :UseLdapAuth, ");
            sqlCommand.Append(" :AutoCreateLDAPUserOnFirstLogin, ");
            sqlCommand.Append(" :LdapServer, ");
            sqlCommand.Append(" :LdapPort, ");
            sqlCommand.Append(" :LdapDomain, ");
            sqlCommand.Append(" :LdapRootDN, ");
            sqlCommand.Append(" :LdapUserDNKey, ");
            sqlCommand.Append(" :AllowUserFullNameChange, ");
            sqlCommand.Append(" :UseEmailForLogin, ");
            sqlCommand.Append(" :ReallyDeleteUsers, ");
            sqlCommand.Append(" :EditorProvider, ");
            sqlCommand.Append(" :EditorSkin, ");

            sqlCommand.Append(" :DatePickerProvider, ");
            sqlCommand.Append(" :CaptchaProvider, ");
            sqlCommand.Append(" :RecaptchaPrivateKey, ");
            sqlCommand.Append(" :RecaptchaPublicKey, ");
            sqlCommand.Append(" :WordpressAPIKey, ");
            sqlCommand.Append(" :WindowsLiveAppID, ");
            sqlCommand.Append(" :WindowsLiveKey, ");
            sqlCommand.Append(" :AllowOpenIDAuth, ");
            sqlCommand.Append(" :AllowWindowsLiveAuth, ");

            sqlCommand.Append(":GmapApiKey, ");
            sqlCommand.Append(":ApiKeyExtra1, ");
            sqlCommand.Append(":ApiKeyExtra2, ");
            sqlCommand.Append(":ApiKeyExtra3, ");
            sqlCommand.Append(":ApiKeyExtra4, ");
            sqlCommand.Append(":ApiKeyExtra5, ");
            sqlCommand.Append(":DisableDbAuth, ");

            sqlCommand.Append(" :DefaultFriendlyUrlPattern, ");
            sqlCommand.Append(" :SiteGuid ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[46];

            arParams[0] = new SqliteParameter(":SiteName", DbType.String);
            arParams[0].Value = siteName;

            arParams[1] = new SqliteParameter(":IsServerAdminSite", DbType.Int32);
            arParams[1].Value = adminSite;

            arParams[2] = new SqliteParameter(":Skin", DbType.String);
            arParams[2].Value = skin;

            arParams[3] = new SqliteParameter(":Logo", DbType.String);
            arParams[3].Value = logo;

            arParams[4] = new SqliteParameter(":Icon", DbType.String);
            arParams[4].Value = icon;

            arParams[5] = new SqliteParameter(":AllowNewRegistration", DbType.Int32);
            arParams[5].Value = allowNew;

            arParams[6] = new SqliteParameter(":AllowUserSkins", DbType.Int32);
            arParams[6].Value = allowSkins;

            arParams[7] = new SqliteParameter(":UseSecureRegistration", DbType.Int32);
            arParams[7].Value = secure;

            arParams[8] = new SqliteParameter(":EnableMyPageFeature", DbType.Int32);
            arParams[8].Value = enableMyPage;

            arParams[9] = new SqliteParameter(":UseSSLOnAllPages", DbType.Int32);
            arParams[9].Value = ssl;

            arParams[10] = new SqliteParameter(":DefaultPageKeywords", DbType.String);
            arParams[10].Value = defaultPageKeywords;

            arParams[11] = new SqliteParameter(":DefaultPageDescription", DbType.String);
            arParams[11].Value = defaultPageDescription;

            arParams[12] = new SqliteParameter(":DefaultPageEncoding", DbType.String);
            arParams[12].Value = defaultPageEncoding;

            arParams[13] = new SqliteParameter(":DefaultAdditionalMetaTags", DbType.String);
            arParams[13].Value = defaultAdditionalMetaTags;

            arParams[14] = new SqliteParameter(":AllowPageSkins", DbType.Int32);
            arParams[14].Value = pageSkins;

            arParams[15] = new SqliteParameter(":AllowHideMenuOnPages", DbType.Int32);
            arParams[15].Value = allowHide;

            arParams[16] = new SqliteParameter(":UseLdapAuth", DbType.Int32);
            arParams[16].Value = uldapp;

            arParams[17] = new SqliteParameter(":AutoCreateLDAPUserOnFirstLogin", DbType.Int32);
            arParams[17].Value = autoldapp;

            arParams[18] = new SqliteParameter(":LdapServer", DbType.String);
            arParams[18].Value = ldapServer;

            arParams[19] = new SqliteParameter(":LdapPort", DbType.Int32);
            arParams[19].Value = ldapPort;

            arParams[20] = new SqliteParameter(":LdapRootDN", DbType.String);
            arParams[20].Value = ldapRootDN;

            arParams[21] = new SqliteParameter(":LdapUserDNKey", DbType.String);
            arParams[21].Value = ldapUserDNKey;

            arParams[22] = new SqliteParameter(":AllowUserFullNameChange", DbType.Int32);
            arParams[22].Value = allowNameChange;

            arParams[23] = new SqliteParameter(":UseEmailForLogin", DbType.Int32);
            arParams[23].Value = emailForLogin;

            arParams[24] = new SqliteParameter(":ReallyDeleteUsers", DbType.Int32);
            arParams[24].Value = deleteUsers;

            arParams[25] = new SqliteParameter(":EditorSkin", DbType.String);
            arParams[25].Value = editorSkin;

            arParams[26] = new SqliteParameter(":DefaultFriendlyUrlPattern", DbType.String);
            arParams[26].Value = defaultFriendlyUrlPattern;

            arParams[27] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[27].Value = siteGuid.ToString();

            arParams[28] = new SqliteParameter(":LdapDomain", DbType.String);
            arParams[28].Value = ldapDomain;

            arParams[29] = new SqliteParameter(":EditorProvider", DbType.String);
            arParams[29].Value = editorProvider;

            arParams[30] = new SqliteParameter(":DatePickerProvider", DbType.String);
            arParams[30].Value = datePickerProvider;

            arParams[31] = new SqliteParameter(":CaptchaProvider", DbType.String);
            arParams[31].Value = captchaProvider;

            arParams[32] = new SqliteParameter(":RecaptchaPrivateKey", DbType.String);
            arParams[32].Value = recaptchaPrivateKey;

            arParams[33] = new SqliteParameter(":RecaptchaPublicKey", DbType.String);
            arParams[33].Value = recaptchaPublicKey;

            arParams[34] = new SqliteParameter(":WordpressAPIKey", DbType.String);
            arParams[34].Value = wordpressApiKey;

            arParams[35] = new SqliteParameter(":WindowsLiveAppID", DbType.String);
            arParams[35].Value = windowsLiveAppId;

            arParams[36] = new SqliteParameter(":WindowsLiveKey", DbType.String);
            arParams[36].Value = windowsLiveKey;

            arParams[37] = new SqliteParameter(":AllowOpenIDAuth", DbType.Int32);
            arParams[37].Value = oidauth;

            arParams[38] = new SqliteParameter(":AllowWindowsLiveAuth", DbType.Int32);
            arParams[38].Value = winliveauth;

            arParams[39] = new SqliteParameter(":GmapApiKey", DbType.String);
            arParams[39].Value = gmapApiKey;

            arParams[40] = new SqliteParameter(":ApiKeyExtra1", DbType.String);
            arParams[40].Value = apiKeyExtra1;

            arParams[41] = new SqliteParameter(":ApiKeyExtra2", DbType.String);
            arParams[41].Value = apiKeyExtra2;

            arParams[42] = new SqliteParameter(":ApiKeyExtra3", DbType.String);
            arParams[42].Value = apiKeyExtra3;

            arParams[43] = new SqliteParameter(":ApiKeyExtra4", DbType.String);
            arParams[43].Value = apiKeyExtra4;

            arParams[44] = new SqliteParameter(":ApiKeyExtra5", DbType.String);
            arParams[44].Value = apiKeyExtra5;

            arParams[45] = new SqliteParameter(":DisableDbAuth", DbType.Int32);
            arParams[45].Value = intDisableDbAuth;

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }

        public bool Update(
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

            byte enableMy;
            if (enableMyPageFeature)
            {
                enableMy = 1;
            }
            else
            {
                enableMy = 0;
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

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET SiteName = :SiteName, ");
            sqlCommand.Append("IsServerAdminSite = :IsServerAdminSite, ");
            sqlCommand.Append("Skin = :Skin, ");
            sqlCommand.Append("Logo = :Logo, ");
            sqlCommand.Append("Icon = :Icon, ");

            sqlCommand.Append("AllowNewRegistration = :AllowNewRegistration, ");
            sqlCommand.Append("AllowUserSkins = :AllowUserSkins, ");

            sqlCommand.Append("AllowPageSkins = :AllowPageSkins, ");
            sqlCommand.Append("AllowHideMenuOnPages = :AllowHideMenuOnPages, ");

            sqlCommand.Append("UseSecureRegistration = :UseSecureRegistration, ");
            sqlCommand.Append("EnableMyPageFeature = :EnableMyPageFeature, ");
            sqlCommand.Append("UseSSLOnAllPages = :UseSSLOnAllPages, ");

            sqlCommand.Append("UseLdapAuth = :UseLdapAuth, ");
            sqlCommand.Append("AutoCreateLDAPUserOnFirstLogin = :AutoCreateLDAPUserOnFirstLogin, ");
            sqlCommand.Append("LdapServer = :LdapServer, ");
            sqlCommand.Append("LdapPort = :LdapPort, ");
            sqlCommand.Append("LdapDomain = :LdapDomain, ");
            sqlCommand.Append("LdapRootDN = :LdapRootDN, ");
            sqlCommand.Append("LdapUserDNKey = :LdapUserDNKey, ");
            sqlCommand.Append("AllowUserFullNameChange = :AllowUserFullNameChange, ");
            sqlCommand.Append("UseEmailForLogin = :UseEmailForLogin, ");
            sqlCommand.Append("ReallyDeleteUsers = :ReallyDeleteUsers, ");
            sqlCommand.Append("EditorProvider = :EditorProvider, ");
            sqlCommand.Append("EditorSkin = :EditorSkin, ");
            sqlCommand.Append("DefaultFriendlyUrlPatternEnum = :DefaultFriendlyUrlPattern, ");

            sqlCommand.Append("DatePickerProvider = :DatePickerProvider, ");
            sqlCommand.Append("CaptchaProvider = :CaptchaProvider, ");
            sqlCommand.Append("RecaptchaPrivateKey = :RecaptchaPrivateKey, ");
            sqlCommand.Append("RecaptchaPublicKey = :RecaptchaPublicKey, ");
            sqlCommand.Append("WordpressAPIKey = :WordpressAPIKey, ");
            sqlCommand.Append("WindowsLiveAppID = :WindowsLiveAppID, ");
            sqlCommand.Append("WindowsLiveKey = :WindowsLiveKey, ");
            sqlCommand.Append("AllowOpenIDAuth = :AllowOpenIDAuth, ");
            sqlCommand.Append("AllowWindowsLiveAuth = :AllowWindowsLiveAuth, ");
            sqlCommand.Append("DisableDbAuth = :DisableDbAuth, ");

            sqlCommand.Append("DefaultPageKeywords = :DefaultPageKeywords, ");
            sqlCommand.Append("DefaultPageDescription = :DefaultPageDescription, ");
            sqlCommand.Append("DefaultPageEncoding = :DefaultPageEncoding, ");
            sqlCommand.Append("DefaultAdditionalMetaTags = :DefaultAdditionalMetaTags, ");
            sqlCommand.Append("GmapApiKey = :GmapApiKey, ");
            sqlCommand.Append("ApiKeyExtra1 = :ApiKeyExtra1, ");
            sqlCommand.Append("ApiKeyExtra2 = :ApiKeyExtra2, ");
            sqlCommand.Append("ApiKeyExtra3 = :ApiKeyExtra3, ");
            sqlCommand.Append("ApiKeyExtra4 = :ApiKeyExtra4, ");
            sqlCommand.Append("ApiKeyExtra5 = :ApiKeyExtra5 ");

            sqlCommand.Append(" WHERE SiteID = :SiteID ;");

            SqliteParameter[] arParams = new SqliteParameter[46];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":SiteName", DbType.String);
            arParams[1].Value = siteName;

            arParams[2] = new SqliteParameter(":IsServerAdminSite", DbType.Int32);
            arParams[2].Value = adminSite;

            arParams[3] = new SqliteParameter(":Skin", DbType.String);
            arParams[3].Value = skin;

            arParams[4] = new SqliteParameter(":Logo", DbType.String);
            arParams[4].Value = logo;

            arParams[5] = new SqliteParameter(":Icon", DbType.String);
            arParams[5].Value = icon;

            arParams[6] = new SqliteParameter(":AllowNewRegistration", DbType.Int32);
            arParams[6].Value = allowNew;

            arParams[7] = new SqliteParameter(":AllowUserSkins", DbType.Int32);
            arParams[7].Value = allowSkins;

            arParams[8] = new SqliteParameter(":UseSecureRegistration", DbType.Int32);
            arParams[8].Value = secure;

            arParams[9] = new SqliteParameter(":EnableMyPageFeature", DbType.Int32);
            arParams[9].Value = enableMy;

            arParams[10] = new SqliteParameter(":UseSSLOnAllPages", DbType.Int32);
            arParams[10].Value = ssl;

            arParams[11] = new SqliteParameter(":DefaultPageKeywords", DbType.String);
            arParams[11].Value = defaultPageKeywords;

            arParams[12] = new SqliteParameter(":DefaultPageDescription", DbType.String);
            arParams[12].Value = defaultPageDescription;

            arParams[13] = new SqliteParameter(":DefaultPageEncoding", DbType.String);
            arParams[13].Value = defaultPageEncoding;

            arParams[14] = new SqliteParameter(":DefaultAdditionalMetaTags", DbType.String);
            arParams[14].Value = defaultAdditionalMetaTags;

            arParams[15] = new SqliteParameter(":AllowPageSkins", DbType.Int32);
            arParams[15].Value = pageSkins;

            arParams[16] = new SqliteParameter(":AllowHideMenuOnPages", DbType.Int32);
            arParams[16].Value = allowHide;

            arParams[17] = new SqliteParameter(":UseLdapAuth", DbType.Int32);
            arParams[17].Value = uldapp;

            arParams[18] = new SqliteParameter(":AutoCreateLDAPUserOnFirstLogin", DbType.Int32);
            arParams[18].Value = autoldapp;

            arParams[19] = new SqliteParameter(":LdapServer", DbType.String);
            arParams[19].Value = ldapServer;

            arParams[20] = new SqliteParameter(":LdapPort", DbType.Int32);
            arParams[20].Value = ldapPort;

            arParams[21] = new SqliteParameter(":LdapRootDN", DbType.String);
            arParams[21].Value = ldapRootDN;

            arParams[22] = new SqliteParameter(":LdapUserDNKey", DbType.String);
            arParams[22].Value = ldapUserDNKey;

            arParams[23] = new SqliteParameter(":AllowUserFullNameChange", DbType.Int32);
            arParams[23].Value = allowNameChange;

            arParams[24] = new SqliteParameter(":UseEmailForLogin", DbType.Int32);
            arParams[24].Value = emailForLogin;

            arParams[25] = new SqliteParameter(":ReallyDeleteUsers", DbType.Int32);
            arParams[25].Value = deleteUsers;

            arParams[26] = new SqliteParameter(":EditorSkin", DbType.String);
            arParams[26].Value = editorSkin;

            arParams[27] = new SqliteParameter(":DefaultFriendlyUrlPattern", DbType.String);
            arParams[27].Value = defaultFriendlyUrlPattern;

            arParams[28] = new SqliteParameter(":LdapDomain", DbType.String);
            arParams[28].Value = ldapDomain;

            arParams[29] = new SqliteParameter(":EditorProvider", DbType.String);
            arParams[29].Value = editorProvider;

            arParams[30] = new SqliteParameter(":DatePickerProvider", DbType.String);
            arParams[30].Value = datePickerProvider;

            arParams[31] = new SqliteParameter(":CaptchaProvider", DbType.String);
            arParams[31].Value = captchaProvider;

            arParams[32] = new SqliteParameter(":RecaptchaPrivateKey", DbType.String);
            arParams[32].Value = recaptchaPrivateKey;

            arParams[33] = new SqliteParameter(":RecaptchaPublicKey", DbType.String);
            arParams[33].Value = recaptchaPublicKey;

            arParams[34] = new SqliteParameter(":WordpressAPIKey", DbType.String);
            arParams[34].Value = wordpressApiKey;

            arParams[35] = new SqliteParameter(":WindowsLiveAppID", DbType.String);
            arParams[35].Value = windowsLiveAppId;

            arParams[36] = new SqliteParameter(":WindowsLiveKey", DbType.String);
            arParams[36].Value = windowsLiveKey;

            arParams[37] = new SqliteParameter(":AllowOpenIDAuth", DbType.Int32);
            arParams[37].Value = oidauth;

            arParams[38] = new SqliteParameter(":AllowWindowsLiveAuth", DbType.Int32);
            arParams[38].Value = winliveauth;

            arParams[39] = new SqliteParameter(":GmapApiKey", DbType.String);
            arParams[39].Value = gmapApiKey;

            arParams[40] = new SqliteParameter(":ApiKeyExtra1", DbType.String);
            arParams[40].Value = apiKeyExtra1;

            arParams[41] = new SqliteParameter(":ApiKeyExtra2", DbType.String);
            arParams[41].Value = apiKeyExtra2;

            arParams[42] = new SqliteParameter(":ApiKeyExtra3", DbType.String);
            arParams[42].Value = apiKeyExtra3;

            arParams[43] = new SqliteParameter(":ApiKeyExtra4", DbType.String);
            arParams[43].Value = apiKeyExtra4;

            arParams[44] = new SqliteParameter(":ApiKeyExtra5", DbType.String);
            arParams[44].Value = apiKeyExtra5;

            arParams[45] = new SqliteParameter(":DisableDbAuth", DbType.Int32);
            arParams[45].Value = intDisableDbAuth;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
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



            byte allowNew;
            if (allowNewRegistration)
            {
                allowNew = 1;
            }
            else
            {
                allowNew = 0;
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

            sqlCommand.Append("AllowNewRegistration = :AllowNewRegistration, ");
            sqlCommand.Append("UseSecureRegistration = :UseSecureRegistration, ");
            sqlCommand.Append("UseLdapAuth = :UseLdapAuth, ");
            sqlCommand.Append("AutoCreateLDAPUserOnFirstLogin = :AutoCreateLDAPUserOnFirstLogin, ");
            sqlCommand.Append("LdapServer = :LdapServer, ");
            sqlCommand.Append("LdapPort = :LdapPort, ");
            sqlCommand.Append("LdapDomain = :LdapDomain, ");
            sqlCommand.Append("LdapRootDN = :LdapRootDN, ");
            sqlCommand.Append("LdapUserDNKey = :LdapUserDNKey, ");
            sqlCommand.Append("AllowUserFullNameChange = :AllowUserFullNameChange, ");
            sqlCommand.Append("UseEmailForLogin = :UseEmailForLogin, ");
            sqlCommand.Append("AllowOpenIDAuth = :AllowOpenIDAuth, ");
            sqlCommand.Append("AllowWindowsLiveAuth = :AllowWindowsLiveAuth, ");
            sqlCommand.Append("AllowPasswordRetrieval = :AllowPasswordRetrieval, ");
            sqlCommand.Append("AllowPasswordReset = :AllowPasswordReset, ");
            sqlCommand.Append("RequiresQuestionAndAnswer = :RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts = :MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("PasswordAttemptWindowMinutes = :PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("RequiresUniqueEmail = :RequiresUniqueEmail, ");
            sqlCommand.Append("PasswordFormat = :PasswordFormat, ");
            sqlCommand.Append("MinRequiredPasswordLength = :MinRequiredPasswordLength, ");
            sqlCommand.Append("MinReqNonAlphaChars = :MinReqNonAlphaChars, ");
            sqlCommand.Append("PwdStrengthRegex = :PwdStrengthRegex ");

            sqlCommand.Append(" WHERE SiteID <> :SiteID ;");

            SqliteParameter[] arParams = new SqliteParameter[24];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":AllowNewRegistration", DbType.Int32);
            arParams[1].Value = allowNew;

            arParams[2] = new SqliteParameter(":UseSecureRegistration", DbType.Int32);
            arParams[2].Value = secure;

            arParams[3] = new SqliteParameter(":UseLdapAuth", DbType.Int32);
            arParams[3].Value = uldapp;

            arParams[4] = new SqliteParameter(":AutoCreateLDAPUserOnFirstLogin", DbType.Int32);
            arParams[4].Value = autoldapp;

            arParams[5] = new SqliteParameter(":LdapServer", DbType.String);
            arParams[5].Value = ldapServer;

            arParams[6] = new SqliteParameter(":LdapPort", DbType.Int32);
            arParams[6].Value = ldapPort;

            arParams[7] = new SqliteParameter(":LdapRootDN", DbType.String);
            arParams[7].Value = ldapRootDN;

            arParams[8] = new SqliteParameter(":LdapUserDNKey", DbType.String);
            arParams[8].Value = ldapUserDNKey;

            arParams[9] = new SqliteParameter(":AllowUserFullNameChange", DbType.Int32);
            arParams[9].Value = allowNameChange;

            arParams[10] = new SqliteParameter(":UseEmailForLogin", DbType.Int32);
            arParams[10].Value = emailForLogin;

            arParams[11] = new SqliteParameter(":LdapDomain", DbType.String);
            arParams[11].Value = ldapDomain;

            arParams[12] = new SqliteParameter(":AllowOpenIDAuth", DbType.Int32);
            arParams[12].Value = oidauth;

            arParams[13] = new SqliteParameter(":AllowWindowsLiveAuth", DbType.Int32);
            arParams[13].Value = winliveauth;

            arParams[14] = new SqliteParameter(":AllowPasswordRetrieval", DbType.Int32);
            arParams[14].Value = intAllowPasswordRetrieval;

            arParams[15] = new SqliteParameter(":AllowPasswordReset", DbType.Int32);
            arParams[15].Value = intAllowPasswordReset;

            arParams[16] = new SqliteParameter(":RequiresQuestionAndAnswer", DbType.Int32);
            arParams[16].Value = intRequiresQuestionAndAnswer;

            arParams[17] = new SqliteParameter(":MaxInvalidPasswordAttempts", DbType.Int32);
            arParams[17].Value = maxInvalidPasswordAttempts;

            arParams[18] = new SqliteParameter(":PasswordAttemptWindowMinutes", DbType.Int32);
            arParams[18].Value = passwordAttemptWindowMinutes;

            arParams[19] = new SqliteParameter(":RequiresUniqueEmail", DbType.Int32);
            arParams[19].Value = intRequiresUniqueEmail;

            arParams[20] = new SqliteParameter(":PasswordFormat", DbType.Int32);
            arParams[20].Value = passwordFormat;

            arParams[21] = new SqliteParameter(":MinRequiredPasswordLength", DbType.Int32);
            arParams[21].Value = minRequiredPasswordLength;

            arParams[22] = new SqliteParameter(":MinReqNonAlphaChars", DbType.Int32);
            arParams[22].Value = minReqNonAlphaChars;

            arParams[23] = new SqliteParameter(":PwdStrengthRegex", DbType.Object);
            arParams[23].Value = pwdStrengthRegex;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
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
            sqlCommand.Append("WindowsLiveAppID = :WindowsLiveAppID, ");
            sqlCommand.Append("WindowsLiveKey = :WindowsLiveKey ");

            sqlCommand.Append(" WHERE SiteID <> :SiteID ;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":WindowsLiveAppID", DbType.String);
            arParams[1].Value = windowsLiveAppId;

            arParams[2] = new SqliteParameter(":WindowsLiveKey", DbType.String);
            arParams[2].Value = windowsLiveKey;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
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
            String passwordStrengthRegularExpression,
            String defaultEmailFromAddress
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
            sqlCommand.Append("SET AllowPasswordRetrieval = :AllowPasswordRetrieval, ");
            sqlCommand.Append("AllowPasswordReset = :AllowPasswordReset, ");
            sqlCommand.Append("RequiresQuestionAndAnswer = :RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts = :MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("PasswordAttemptWindowMinutes = :PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("RequiresUniqueEmail = :RequiresUniqueEmail, ");
            sqlCommand.Append("PasswordFormat = :PasswordFormat, ");
            sqlCommand.Append("MinRequiredPasswordLength = :MinRequiredPasswordLength, ");
            sqlCommand.Append("MinReqNonAlphaChars = :MinRequiredNonAlphanumericCharacters, ");
            sqlCommand.Append("PwdStrengthRegex = :PasswordStrengthRegularExpression, ");
            sqlCommand.Append("DefaultEmailFromAddress = :DefaultEmailFromAddress ");

            sqlCommand.Append(" WHERE SiteID = :SiteID ;");

            SqliteParameter[] arParams = new SqliteParameter[12];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":AllowPasswordRetrieval", DbType.Int32);
            arParams[1].Value = allowRetrieval;

            arParams[2] = new SqliteParameter(":AllowPasswordReset", DbType.Int32);
            arParams[2].Value = allowReset;

            arParams[3] = new SqliteParameter(":RequiresQuestionAndAnswer", DbType.Int32);
            arParams[3].Value = requiresQA;

            arParams[4] = new SqliteParameter(":MaxInvalidPasswordAttempts", DbType.Int32);
            arParams[4].Value = maxInvalidPasswordAttempts;

            arParams[5] = new SqliteParameter(":PasswordAttemptWindowMinutes", DbType.Int32);
            arParams[5].Value = passwordAttemptWindowMinutes;

            arParams[6] = new SqliteParameter(":RequiresUniqueEmail", DbType.Int32);
            arParams[6].Value = requiresEmail;

            arParams[7] = new SqliteParameter(":PasswordFormat", DbType.Int32);
            arParams[7].Value = passwordFormat;

            arParams[8] = new SqliteParameter(":MinRequiredPasswordLength", DbType.Int32);
            arParams[8].Value = minRequiredPasswordLength;

            arParams[9] = new SqliteParameter(":PasswordStrengthRegularExpression", DbType.Object);
            arParams[9].Value = passwordStrengthRegularExpression;

            arParams[10] = new SqliteParameter(":DefaultEmailFromAddress", DbType.String);
            arParams[10].Value = defaultEmailFromAddress;

            arParams[11] = new SqliteParameter(":MinRequiredNonAlphanumericCharacters", DbType.Int32);
            arParams[11].Value = minRequiredNonAlphanumericCharacters;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public bool Delete(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_UserProperties WHERE UserGuid IN (SELECT UserGuid FROM mp_Users WHERE SiteID = :SiteID);");
            sqlCommand.Append("DELETE FROM mp_UserRoles WHERE UserID IN (SELECT UserID FROM mp_Users WHERE SiteID = :SiteID);");
            sqlCommand.Append("DELETE FROM mp_UserLocation WHERE UserGuid IN (SELECT UserGuid FROM mp_Users WHERE SiteID = :SiteID);");

            sqlCommand.Append("DELETE FROM mp_Users WHERE SiteID = :SiteID; ");

            sqlCommand.Append("DELETE FROM mp_Roles WHERE SiteID = :SiteID; ");
            sqlCommand.Append("DELETE FROM mp_SiteHosts WHERE SiteID = :SiteID; ");


            sqlCommand.Append("DELETE FROM mp_SiteFolders WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = :SiteID);");

            sqlCommand.Append("DELETE FROM mp_SiteSettingsEx WHERE SiteID = :SiteID; ");


            sqlCommand.Append("DELETE FROM mp_RedirectList WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = :SiteID);");
            sqlCommand.Append("DELETE FROM mp_TaskQueue WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = :SiteID);");

            sqlCommand.Append("DELETE FROM mp_Sites ");
            sqlCommand.Append("WHERE HostID = :SiteID  ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }



        //public static bool HasFeature(int siteId, int moduleDefId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT Count(*) FROM mp_SiteModuleDefinitions ");
        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("SiteID = :SiteID ");
        //    sqlCommand.Append(" AND ");
        //    sqlCommand.Append("ModuleDefID = :ModuleDefID ");
        //    sqlCommand.Append(" ;");

        //    SqliteParameter[] arParams = new SqliteParameter[2];

        //    arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new SqliteParameter(":ModuleDefID", DbType.Int32);
        //    arParams[1].Value = moduleDefId;

        //    int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        ConnectionString.GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams));

        //    return (count > 0);

        //}

        //public static bool HasFeature(Guid siteGuid, Guid featureGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT Count(*) FROM mp_SiteModuleDefinitions ");
        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("SiteGuid = :SiteGuid ");
        //    sqlCommand.Append(" AND ");
        //    sqlCommand.Append("FeatureGuid = :FeatureGuid ");
        //    sqlCommand.Append(" ;");

        //    SqliteParameter[] arParams = new SqliteParameter[2];

        //    arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
        //    arParams[0].Value = siteGuid.ToString();

        //    arParams[1] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
        //    arParams[1].Value = featureGuid.ToString();

        //    int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        ConnectionString.GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams));

        //    return (count > 0);

        //}

        //public static void AddFeature(Guid siteGuid, Guid featureGuid)
        //{
        //    if (HasFeature(siteGuid, featureGuid)) return;

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("INSERT INTO mp_SiteModuleDefinitions ");
        //    sqlCommand.Append("( ");
        //    sqlCommand.Append("SiteID, ");
        //    sqlCommand.Append("ModuleDefID, ");
        //    sqlCommand.Append("SiteGuid, ");
        //    sqlCommand.Append("FeatureGuid, ");
        //    sqlCommand.Append("AuthorizedRoles ");
        //    sqlCommand.Append(") ");

        //    sqlCommand.Append("VALUES ");
        //    sqlCommand.Append("( ");
        //    sqlCommand.Append("(SELECT SiteID FROM mp_Sites WHERE SiteGuid = :SiteGuid LIMIT 1), ");
        //    sqlCommand.Append("(SELECT ModuleDefID FROM mp_ModuleDefinitions WHERE Guid = :FeatureGuid LIMIT 1), ");
        //    sqlCommand.Append(":SiteGuid, ");
        //    sqlCommand.Append(":FeatureGuid, ");
        //    sqlCommand.Append("'All Users' ");
        //    sqlCommand.Append(") ;");

        //    SqliteParameter[] arParams = new SqliteParameter[2];

        //    arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
        //    arParams[0].Value = siteGuid.ToString();

        //    arParams[1] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
        //    arParams[1].Value = featureGuid.ToString();

        //    AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public static void RemoveFeature(Guid siteGuid, Guid featureGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_SiteModuleDefinitions ");
        //    sqlCommand.Append("WHERE SiteGuid = :SiteGuid AND FeatureGuid = :FeatureGuid ; ");

        //    SqliteParameter[] arParams = new SqliteParameter[2];

        //    arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
        //    arParams[0].Value = siteGuid.ToString();

        //    arParams[1] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
        //    arParams[1].Value = featureGuid.ToString();

        //    AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public static void RemoveFeature(int siteId, int moduleDefId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_SiteModuleDefinitions ");
        //    sqlCommand.Append("WHERE SiteID = :SiteID AND ModuleDefID = :ModuleDefID ; ");

        //    SqliteParameter[] arParams = new SqliteParameter[2];

        //    arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new SqliteParameter(":ModuleDefID", DbType.Int32);
        //    arParams[1].Value = moduleDefId;

        //    AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        public int GetHostCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SiteHosts ");

            sqlCommand.Append(";");

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                null));
        }

        public DbDataReader GetAllHosts()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append("ORDER BY HostName  ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                null);
        }

        public DbDataReader GetPageHosts(
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_SiteHosts  ");
            //sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ORDER BY HostName ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[0].Value = pageSize;

            arParams[1] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[1].Value = pageLowerBound;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }


        public DbDataReader GetHostList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append("WHERE SiteID = :SiteID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

        public DbDataReader GetHost(string hostName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append("WHERE HostName = :HostName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":HostName", DbType.String);
            arParams[0].Value = hostName;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

        public bool AddHost(Guid siteGuid, int siteId, string hostName)
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
            sqlCommand.Append(":SiteID, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":HostName ");
            sqlCommand.Append(") ;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":HostName", DbType.String);
            arParams[1].Value = hostName;

            arParams[2] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[2].Value = siteGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected > 0;
        }

        public bool DeleteHost(int hostId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE HostID = :HostID  ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":HostID", DbType.Int32);
            arParams[0].Value = hostId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected > 0;
        }

        public DbDataReader GetSiteList()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Sites ");

            sqlCommand.Append("ORDER BY	SiteName ;");
            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString());
        }

        public DbDataReader GetSite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID = :SiteID ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

        public DbDataReader GetSite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteGuid = :SiteGuid ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[0].Value = siteGuid.ToString();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

        public DbDataReader GetSite(string hostName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":HostName", DbType.String);
            arParams[0].Value = hostName;

            int siteId = -1;

            sqlCommand.Append("SELECT mp_SiteHosts.SiteID As SiteID ");
            sqlCommand.Append("FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE mp_SiteHosts.HostName = :HostName ;");

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
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
            sqlCommand.Append("WHERE SiteID = :SiteID OR :SiteID = -1 ");
            sqlCommand.Append("ORDER BY	SiteID ");
            sqlCommand.Append("LIMIT 1 ;");

            arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
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

            sqlCommand.Append("WHERE SiteID = :SiteID ");
            sqlCommand.Append("ORDER BY ParentID,  PageName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }


        public int CountOtherSites(int currentSiteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID <> :CurrentSiteID ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":CurrentSiteID", DbType.Int32);
            arParams[0].Value = currentSiteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

        }

        public DbDataReader GetPageOfOtherSites(
            int currentSiteId,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_Sites  ");
            sqlCommand.Append("WHERE SiteID <> :CurrentSiteID ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("SiteName  ");
            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":CurrentSiteID", DbType.Int32);
            arParams[0].Value = currentSiteId;

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[2].Value = pageLowerBound;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

        public int GetSiteIdByHostName(string hostName)
        {
            int siteId = -1;

            StringBuilder sqlCommand = new StringBuilder();
            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":HostName", DbType.String);
            arParams[0].Value = hostName;

            sqlCommand.Append("SELECT mp_SiteHosts.SiteID As SiteID ");
            sqlCommand.Append("FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE mp_SiteHosts.HostName = :HostName ;");

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
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
                connectionString,
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

        public int GetSiteIdByFolder(string folderName)
        {
            int siteId = -1;

            StringBuilder sqlCommand = new StringBuilder();
            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":FolderName", DbType.String);
            arParams[0].Value = folderName;

            sqlCommand.Append("SELECT COALESCE(s.SiteID, -1) As SiteID ");
            sqlCommand.Append("FROM mp_SiteFolders sf ");
            sqlCommand.Append("JOIN mp_Sites s ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("sf.SiteGuid = s.SiteGuid ");
            sqlCommand.Append("WHERE sf.FolderName = :FolderName ");
            sqlCommand.Append("ORDER BY s.SiteID ");
            sqlCommand.Append(";");

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
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
                connectionString,
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
