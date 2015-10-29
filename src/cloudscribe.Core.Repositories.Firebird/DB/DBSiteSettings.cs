// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-08-06
//

using cloudscribe.DbHelpers.Firebird;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Framework.Logging;

namespace cloudscribe.Core.Repositories.Firebird
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

            #region bit conversion

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

            FbParameter[] arParams = new FbParameter[57];

            arParams[0] = new FbParameter(":SiteGuid", FbDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new FbParameter(":SiteName", FbDbType.VarChar, 255);
            arParams[1].Value = siteName;

            arParams[2] = new FbParameter(":Skin", FbDbType.VarChar, 100);
            arParams[2].Value = skin;

            arParams[3] = new FbParameter(":Logo", FbDbType.VarChar, 50);
            arParams[3].Value = logo;

            arParams[4] = new FbParameter(":Icon", FbDbType.VarChar, 50);
            arParams[4].Value = icon;

            arParams[5] = new FbParameter(":AllowUserSkins", FbDbType.Integer);
            arParams[5].Value = allowSkins;

            arParams[6] = new FbParameter(":AllowPageSkins", FbDbType.Integer);
            arParams[6].Value = pageSkins;

            arParams[7] = new FbParameter(":AllowHideMenuOnPages", FbDbType.Integer);
            arParams[7].Value = allowHide;

            arParams[8] = new FbParameter(":AllowNewRegistration", FbDbType.Integer);
            arParams[8].Value = allowNew;

            arParams[9] = new FbParameter(":UseSecureRegistration", FbDbType.Integer);
            arParams[9].Value = secure;

            arParams[10] = new FbParameter(":UseSSLOnAllPages", FbDbType.Integer);
            arParams[10].Value = ssl;

            arParams[11] = new FbParameter(":DefaultPageKeywords", FbDbType.VarChar, 255);
            arParams[11].Value = defaultPageKeywords;

            arParams[12] = new FbParameter(":DefaultPageDescription", FbDbType.VarChar, 255);
            arParams[12].Value = defaultPageDescription;

            arParams[13] = new FbParameter(":DefaultPageEncoding", FbDbType.VarChar, 255);
            arParams[13].Value = defaultPageEncoding;

            arParams[14] = new FbParameter(":DefaultAdditionalMetaTags", FbDbType.VarChar, 255);
            arParams[14].Value = defaultAdditionalMetaTags;

            arParams[15] = new FbParameter(":IsServerAdminSite", FbDbType.Integer);
            arParams[15].Value = adminSite;

            arParams[16] = new FbParameter(":UseLdapAuth", FbDbType.Integer);
            arParams[16].Value = uldapp;

            arParams[17] = new FbParameter(":AutoCreateLDAPUserOnFirstLogin", FbDbType.Integer);
            arParams[17].Value = autoldapp;

            arParams[18] = new FbParameter(":LdapServer", FbDbType.VarChar, 255);
            arParams[18].Value = ldapServer;

            arParams[19] = new FbParameter(":LdapPort", FbDbType.Integer);
            arParams[19].Value = ldapPort;

            arParams[20] = new FbParameter(":LdapDomain", FbDbType.VarChar, 255);
            arParams[20].Value = ldapDomain;

            arParams[21] = new FbParameter(":LdapRootDN", FbDbType.VarChar, 255);
            arParams[21].Value = ldapRootDN;

            arParams[22] = new FbParameter(":LdapUserDNKey", FbDbType.VarChar, 255);
            arParams[22].Value = ldapUserDNKey;

            arParams[23] = new FbParameter(":ReallyDeleteUsers", FbDbType.Integer);
            arParams[23].Value = deleteUsers;

            arParams[24] = new FbParameter(":UseEmailForLogin", FbDbType.Integer);
            arParams[24].Value = emailForLogin;

            arParams[25] = new FbParameter(":AllowUserFullNameChange", FbDbType.Integer);
            arParams[25].Value = allowNameChange;

            arParams[26] = new FbParameter(":EditorSkin", FbDbType.VarChar, 50);
            arParams[26].Value = editorSkin;

            arParams[27] = new FbParameter(":DefaultFriendlyUrlPattern", FbDbType.VarChar, 50);
            arParams[27].Value = defaultFriendlyUrlPattern;

            arParams[28] = new FbParameter(":AllowPasswordRetieval", FbDbType.Integer);
            arParams[28].Value = 1;

            arParams[29] = new FbParameter(":AllowPasswordReset", FbDbType.Integer);
            arParams[29].Value = 1;

            arParams[30] = new FbParameter(":RequiresQuestionAndAnswer", FbDbType.Integer);
            arParams[30].Value = 1;

            arParams[31] = new FbParameter(":MaxInvalidPasswordAttempts", FbDbType.Integer);
            arParams[31].Value = 5;

            arParams[32] = new FbParameter(":PASSWORDATTEMPTWINDOWMINUTES", FbDbType.Integer);
            arParams[32].Value = 5;

            arParams[33] = new FbParameter(":REQUIRESUNIQUEEMAIL", FbDbType.Integer);
            arParams[33].Value = 1;

            arParams[34] = new FbParameter(":PASSWORDFORMAT", FbDbType.Integer);
            arParams[34].Value = 0;

            arParams[35] = new FbParameter(":MINREQUIREDPASSWORDLENGTH", FbDbType.Integer);
            arParams[35].Value = 4;

            arParams[36] = new FbParameter(":MINREQNONALPHACHARS", FbDbType.Integer);
            arParams[36].Value = 0;

            arParams[37] = new FbParameter(":PWDSTRENGTHREGEX", FbDbType.VarChar);
            arParams[37].Value = "";

            arParams[38] = new FbParameter(":DEFAULTEMAILFROMADDRESS", FbDbType.VarChar, 100);
            arParams[38].Value = "noreply@yoursite.com";

            arParams[39] = new FbParameter(":EnableMyPageFeature", FbDbType.Integer);
            arParams[39].Value = enableMy;

            arParams[40] = new FbParameter(":EditorProvider", FbDbType.VarChar, 255);
            arParams[40].Value = editorProvider;

            arParams[41] = new FbParameter(":DatePickerProvider", FbDbType.VarChar, 255);
            arParams[41].Value = datePickerProvider;

            arParams[42] = new FbParameter(":CaptchaProvider", FbDbType.VarChar, 255);
            arParams[42].Value = captchaProvider;

            arParams[43] = new FbParameter(":RecaptchaPrivateKey", FbDbType.VarChar, 255);
            arParams[43].Value = recaptchaPrivateKey;

            arParams[44] = new FbParameter(":RecaptchaPublicKey", FbDbType.VarChar, 255);
            arParams[44].Value = recaptchaPublicKey;

            arParams[45] = new FbParameter(":WordpressAPIKey", FbDbType.VarChar, 255);
            arParams[45].Value = wordpressApiKey;

            arParams[46] = new FbParameter(":WindowsLiveAppID", FbDbType.VarChar, 255);
            arParams[46].Value = windowsLiveAppId;

            arParams[47] = new FbParameter(":WindowsLiveKey", FbDbType.VarChar, 255);
            arParams[47].Value = windowsLiveKey;

            arParams[48] = new FbParameter(":AllowOpenIDAuth", FbDbType.SmallInt);
            arParams[48].Value = oidauth;

            arParams[49] = new FbParameter(":AllowWindowsLiveAuth", FbDbType.SmallInt);
            arParams[49].Value = winliveauth;

            arParams[50] = new FbParameter(":GmapApiKey", FbDbType.VarChar, 255);
            arParams[50].Value = gmapApiKey;

            arParams[51] = new FbParameter(":ApiKeyExtra1", FbDbType.VarChar, 255);
            arParams[51].Value = apiKeyExtra1;

            arParams[52] = new FbParameter(":ApiKeyExtra2", FbDbType.VarChar, 255);
            arParams[52].Value = apiKeyExtra2;

            arParams[53] = new FbParameter(":ApiKeyExtra3", FbDbType.VarChar, 255);
            arParams[53].Value = apiKeyExtra3;

            arParams[54] = new FbParameter(":ApiKeyExtra4", FbDbType.VarChar, 255);
            arParams[54].Value = apiKeyExtra4;

            arParams[55] = new FbParameter(":ApiKeyExtra5", FbDbType.VarChar, 255);
            arParams[55].Value = apiKeyExtra5;

            arParams[56] = new FbParameter(":DisableDbAuth", FbDbType.SmallInt);
            arParams[56].Value = intDisableDbAuth;

            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_SITES_INSERT ("
                + AdoHelper.GetParamString(arParams.Length) + ")",
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

            #region bit conversion

            byte intDisableDbAuth = 0;
            if (disableDbAuth) { intDisableDbAuth = 1; }

            byte oidauth = 0;
            if (allowOpenIdAuth) { oidauth = 1; }

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
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET SiteName = @SiteName, ");
            sqlCommand.Append("IsServerAdminSite = @IsServerAdminSite, ");
            sqlCommand.Append("Skin = @Skin, ");
            sqlCommand.Append("Logo = @Logo, ");
            sqlCommand.Append("Icon = @Icon, ");
            sqlCommand.Append("AllowNewRegistration = @AllowNewRegistration, ");
            sqlCommand.Append("AllowUserSkins = @AllowUserSkins, ");
            sqlCommand.Append("AllowPageSkins = @AllowPageSkins, ");
            sqlCommand.Append("AllowHideMenuOnPages = @AllowHideMenuOnPages, ");
            sqlCommand.Append("UseSecureRegistration = @UseSecureRegistration, ");
            sqlCommand.Append("EnableMyPageFeature = @EnableMyPageFeature, ");
            sqlCommand.Append("UseSSLOnAllPages = @UseSSLOnAllPages, ");
            sqlCommand.Append("UseLdapAuth = @UseLdapAuth, ");
            sqlCommand.Append("AutoCreateLDAPUserOnFirstLogin = @autoCreateLDAPUserOnFirstLogin, ");
            sqlCommand.Append("LdapServer = @LdapServer, ");
            sqlCommand.Append("LdapPort = @LdapPort, ");
            sqlCommand.Append("LdapDomain = @LdapDomain, ");
            sqlCommand.Append("LdapRootDN = @LdapRootDN, ");
            sqlCommand.Append("LdapUserDNKey = @LdapUserDNKey, ");
            sqlCommand.Append("AllowUserFullNameChange = @AllowUserFullNameChange, ");
            sqlCommand.Append("UseEmailForLogin = @UseEmailForLogin, ");
            sqlCommand.Append("ReallyDeleteUsers = @ReallyDeleteUsers, ");
            sqlCommand.Append("EditorSkin = @EditorSkin, ");
            sqlCommand.Append("EditorProvider = @EditorProvider, ");

            sqlCommand.Append("DatePickerProvider = @DatePickerProvider, ");
            sqlCommand.Append("CaptchaProvider = @CaptchaProvider, ");
            sqlCommand.Append("RecaptchaPrivateKey = @RecaptchaPrivateKey, ");
            sqlCommand.Append("RecaptchaPublicKey = @RecaptchaPublicKey, ");
            sqlCommand.Append("WordpressAPIKey = @WordpressAPIKey, ");
            sqlCommand.Append("WindowsLiveAppID = @WindowsLiveAppID, ");
            sqlCommand.Append("WindowsLiveKey = @WindowsLiveKey, ");
            sqlCommand.Append("AllowOpenIDAuth = @AllowOpenIDAuth, ");
            sqlCommand.Append("AllowWindowsLiveAuth = @AllowWindowsLiveAuth, ");
            sqlCommand.Append("GmapApiKey = @GmapApiKey, ");
            sqlCommand.Append("ApiKeyExtra1 = @ApiKeyExtra1, ");
            sqlCommand.Append("ApiKeyExtra2 = @ApiKeyExtra2, ");
            sqlCommand.Append("ApiKeyExtra3 = @ApiKeyExtra3, ");
            sqlCommand.Append("ApiKeyExtra4 = @ApiKeyExtra4, ");
            sqlCommand.Append("ApiKeyExtra5 = @ApiKeyExtra5, ");
            sqlCommand.Append("DisableDbAuth = @DisableDbAuth, ");

            sqlCommand.Append("DefaultFriendlyUrlPatternEnum = @DefaultFriendlyUrlPattern, ");
            sqlCommand.Append("DefaultPageKeywords = @DefaultPageKeywords, ");
            sqlCommand.Append("DefaultPageDescription = @DefaultPageDescription, ");
            sqlCommand.Append("DefaultPageEncoding = @DefaultPageEncoding, ");
            sqlCommand.Append("DefaultAdditionalMetaTags = @DefaultAdditionalMetaTags ");

            sqlCommand.Append(" WHERE SiteID = @SiteID ;");

            FbParameter[] arParams = new FbParameter[46];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SiteName", FbDbType.VarChar, 128);
            arParams[1].Value = siteName;

            arParams[2] = new FbParameter("@IsServerAdminSite", FbDbType.Integer);
            arParams[2].Value = adminSite;

            arParams[3] = new FbParameter("@Skin", FbDbType.VarChar, 100);
            arParams[3].Value = skin;

            arParams[4] = new FbParameter("@Logo", FbDbType.VarChar, 50);
            arParams[4].Value = logo;

            arParams[5] = new FbParameter("@Icon", FbDbType.VarChar, 50);
            arParams[5].Value = icon;

            arParams[6] = new FbParameter("@AllowNewRegistration", FbDbType.Integer);
            arParams[6].Value = allowNew;

            arParams[7] = new FbParameter("@AllowUserSkins", FbDbType.Integer);
            arParams[7].Value = allowSkins;

            arParams[8] = new FbParameter("@UseSecureRegistration", FbDbType.Integer);
            arParams[8].Value = secure;

            arParams[9] = new FbParameter("@EnableMyPageFeature", FbDbType.Integer);
            arParams[9].Value = enableMy;

            arParams[10] = new FbParameter("@UseSSLOnAllPages", FbDbType.Integer);
            arParams[10].Value = ssl;

            arParams[11] = new FbParameter("@DefaultPageKeywords", FbDbType.VarChar, 255);
            arParams[11].Value = defaultPageKeywords;

            arParams[12] = new FbParameter("@DefaultPageDescription", FbDbType.VarChar, 255);
            arParams[12].Value = defaultPageDescription;

            arParams[13] = new FbParameter("@DefaultPageEncoding", FbDbType.VarChar, 255);
            arParams[13].Value = defaultPageEncoding;

            arParams[14] = new FbParameter("@DefaultAdditionalMetaTags", FbDbType.VarChar, 255);
            arParams[14].Value = defaultAdditionalMetaTags;

            arParams[15] = new FbParameter("@AllowPageSkins", FbDbType.Integer);
            arParams[15].Value = pageSkins;

            arParams[16] = new FbParameter("@AllowHideMenuOnPages", FbDbType.Integer);
            arParams[16].Value = allowHide;

            arParams[17] = new FbParameter("@UseLdapAuth", FbDbType.Integer);
            arParams[17].Value = uldapp;

            arParams[18] = new FbParameter("@AutoCreateLDAPUserOnFirstLogin", FbDbType.Integer);
            arParams[18].Value = autoldapp;

            arParams[19] = new FbParameter("@LdapServer", FbDbType.VarChar, 255);
            arParams[19].Value = ldapServer;

            arParams[20] = new FbParameter("@LdapPort", FbDbType.Integer);
            arParams[20].Value = ldapPort;

            arParams[21] = new FbParameter("@LdapRootDN", FbDbType.VarChar, 255);
            arParams[21].Value = ldapRootDN;

            arParams[22] = new FbParameter("@LdapUserDNKey", FbDbType.VarChar, 10);
            arParams[22].Value = ldapUserDNKey;

            arParams[23] = new FbParameter("@AllowUserFullNameChange", FbDbType.Integer);
            arParams[23].Value = allowNameChange;

            arParams[24] = new FbParameter("@UseEmailForLogin", FbDbType.Integer);
            arParams[24].Value = emailForLogin;

            arParams[25] = new FbParameter("@ReallyDeleteUsers", FbDbType.Integer);
            arParams[25].Value = deleteUsers;

            arParams[26] = new FbParameter("@EditorSkin", FbDbType.VarChar, 50);
            arParams[26].Value = editorSkin;

            arParams[27] = new FbParameter("@DefaultFriendlyUrlPattern", FbDbType.VarChar, 50);
            arParams[27].Value = defaultFriendlyUrlPattern;

            arParams[28] = new FbParameter("@LdapDomain", FbDbType.VarChar, 255);
            arParams[28].Value = ldapDomain;

            arParams[29] = new FbParameter("@EditorProvider", FbDbType.VarChar, 255);
            arParams[29].Value = editorProvider;

            arParams[30] = new FbParameter("@DatePickerProvider", FbDbType.VarChar, 255);
            arParams[30].Value = datePickerProvider;

            arParams[31] = new FbParameter("@CaptchaProvider", FbDbType.VarChar, 255);
            arParams[31].Value = captchaProvider;

            arParams[32] = new FbParameter("@RecaptchaPrivateKey", FbDbType.VarChar, 255);
            arParams[32].Value = recaptchaPrivateKey;

            arParams[33] = new FbParameter("@RecaptchaPublicKey", FbDbType.VarChar, 255);
            arParams[33].Value = recaptchaPublicKey;

            arParams[34] = new FbParameter("@WordpressAPIKey", FbDbType.VarChar, 255);
            arParams[34].Value = wordpressApiKey;

            arParams[35] = new FbParameter("@WindowsLiveAppID", FbDbType.VarChar, 255);
            arParams[35].Value = windowsLiveAppId;

            arParams[36] = new FbParameter("@WindowsLiveKey", FbDbType.VarChar, 255);
            arParams[36].Value = windowsLiveKey;

            arParams[37] = new FbParameter("@AllowOpenIDAuth", FbDbType.SmallInt);
            arParams[37].Value = oidauth;

            arParams[38] = new FbParameter("@AllowWindowsLiveAuth", FbDbType.SmallInt);
            arParams[38].Value = winliveauth;

            arParams[39] = new FbParameter("@GmapApiKey", FbDbType.VarChar, 255);
            arParams[39].Value = gmapApiKey;

            arParams[40] = new FbParameter("@ApiKeyExtra1", FbDbType.VarChar, 255);
            arParams[40].Value = apiKeyExtra1;

            arParams[41] = new FbParameter("@ApiKeyExtra2", FbDbType.VarChar, 255);
            arParams[41].Value = apiKeyExtra2;

            arParams[42] = new FbParameter("@ApiKeyExtra3", FbDbType.VarChar, 255);
            arParams[42].Value = apiKeyExtra3;

            arParams[43] = new FbParameter("@ApiKeyExtra4", FbDbType.VarChar, 255);
            arParams[43].Value = apiKeyExtra4;

            arParams[44] = new FbParameter("@ApiKeyExtra5", FbDbType.VarChar, 255);
            arParams[44].Value = apiKeyExtra5;

            arParams[45] = new FbParameter("@DisableDbAuth", FbDbType.SmallInt);
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
            #region bit conversion

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
            if (allowPasswordRetrieval) { intAllowPasswordRetrieval = 1; }
            int intAllowPasswordReset = 0;
            if (allowPasswordReset) { intAllowPasswordReset = 1; }
            int intRequiresQuestionAndAnswer = 0;
            if (requiresQuestionAndAnswer) { intRequiresQuestionAndAnswer = 1; }
            int intRequiresUniqueEmail = 0;
            if (requiresUniqueEmail) { intRequiresUniqueEmail = 1; }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("AllowNewRegistration = @AllowNewRegistration, ");
            sqlCommand.Append("UseSecureRegistration = @UseSecureRegistration, ");
            sqlCommand.Append("UseLdapAuth = @UseLdapAuth, ");
            sqlCommand.Append("AutoCreateLDAPUserOnFirstLogin = @autoCreateLDAPUserOnFirstLogin, ");
            sqlCommand.Append("LdapServer = @LdapServer, ");
            sqlCommand.Append("LdapPort = @LdapPort, ");
            sqlCommand.Append("LdapDomain = @LdapDomain, ");
            sqlCommand.Append("LdapRootDN = @LdapRootDN, ");
            sqlCommand.Append("LdapUserDNKey = @LdapUserDNKey, ");
            sqlCommand.Append("AllowUserFullNameChange = @AllowUserFullNameChange, ");
            sqlCommand.Append("UseEmailForLogin = @UseEmailForLogin, ");
            sqlCommand.Append("AllowOpenIDAuth = @AllowOpenIDAuth, ");
            sqlCommand.Append("AllowWindowsLiveAuth = @AllowWindowsLiveAuth, ");

            sqlCommand.Append("AllowPasswordRetrieval = @AllowPasswordRetrieval, ");
            sqlCommand.Append("AllowPasswordReset = @AllowPasswordReset, ");
            sqlCommand.Append("RequiresQuestionAndAnswer = @RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts = @MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("PasswordAttemptWindowMinutes = @PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("RequiresUniqueEmail = @RequiresUniqueEmail, ");
            sqlCommand.Append("PasswordFormat = @PasswordFormat, ");
            sqlCommand.Append("MinRequiredPasswordLength = @MinRequiredPasswordLength, ");
            sqlCommand.Append("MinReqNonAlphaChars = @MinReqNonAlphaChars, ");
            sqlCommand.Append("PwdStrengthRegex = @PwdStrengthRegex ");

            sqlCommand.Append(" WHERE SiteID <> @SiteID ;");

            FbParameter[] arParams = new FbParameter[24];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@AllowNewRegistration", FbDbType.Integer);
            arParams[1].Value = allowNew;

            arParams[2] = new FbParameter("@UseSecureRegistration", FbDbType.Integer);
            arParams[2].Value = secure;

            arParams[3] = new FbParameter("@UseLdapAuth", FbDbType.Integer);
            arParams[3].Value = uldapp;

            arParams[4] = new FbParameter("@AutoCreateLDAPUserOnFirstLogin", FbDbType.Integer);
            arParams[4].Value = autoldapp;

            arParams[5] = new FbParameter("@LdapServer", FbDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = ldapServer;

            arParams[6] = new FbParameter("@LdapPort", FbDbType.Integer);
            arParams[6].Value = ldapPort;

            arParams[7] = new FbParameter("@LdapRootDN", FbDbType.VarChar, 255);
            arParams[7].Value = ldapRootDN;

            arParams[8] = new FbParameter("@LdapUserDNKey", FbDbType.VarChar, 10);
            arParams[8].Value = ldapUserDNKey;

            arParams[9] = new FbParameter("@AllowUserFullNameChange", FbDbType.Integer);
            arParams[9].Value = allowNameChange;

            arParams[10] = new FbParameter("@UseEmailForLogin", FbDbType.Integer);
            arParams[10].Value = emailForLogin;

            arParams[11] = new FbParameter("@LdapDomain", FbDbType.VarChar, 255);
            arParams[11].Value = ldapDomain;

            arParams[12] = new FbParameter("@AllowOpenIDAuth", FbDbType.SmallInt);
            arParams[12].Value = oidauth;

            arParams[13] = new FbParameter("@AllowWindowsLiveAuth", FbDbType.SmallInt);
            arParams[13].Value = winliveauth;

            arParams[14] = new FbParameter("@AllowPasswordRetrieval", FbDbType.SmallInt);
            arParams[14].Value = intAllowPasswordRetrieval;

            arParams[15] = new FbParameter("@AllowPasswordReset", FbDbType.SmallInt);
            arParams[15].Value = intAllowPasswordReset;

            arParams[16] = new FbParameter("@RequiresQuestionAndAnswer", FbDbType.SmallInt);
            arParams[16].Value = intRequiresQuestionAndAnswer;

            arParams[17] = new FbParameter("@MaxInvalidPasswordAttempts", FbDbType.Integer);
            arParams[17].Value = maxInvalidPasswordAttempts;

            arParams[18] = new FbParameter("@PasswordAttemptWindowMinutes", FbDbType.Integer);
            arParams[18].Value = passwordAttemptWindowMinutes;

            arParams[19] = new FbParameter("@RequiresUniqueEmail", FbDbType.SmallInt);
            arParams[19].Value = intRequiresUniqueEmail;

            arParams[20] = new FbParameter("@PasswordFormat", FbDbType.Integer);
            arParams[20].Value = passwordFormat;

            arParams[21] = new FbParameter("@MinRequiredPasswordLength", FbDbType.Integer);
            arParams[21].Value = minRequiredPasswordLength;

            arParams[22] = new FbParameter("@MinReqNonAlphaChars", FbDbType.Integer);
            arParams[22].Value = minReqNonAlphaChars;

            arParams[23] = new FbParameter("@PwdStrengthRegex", FbDbType.VarChar);
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
            sqlCommand.Append("WindowsLiveAppID = @WindowsLiveAppID, ");
            sqlCommand.Append("WindowsLiveKey = @WindowsLiveKey ");
            sqlCommand.Append("WHERE SiteID <> @SiteID ;");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@WindowsLiveAppID", FbDbType.VarChar, 255);
            arParams[1].Value = windowsLiveAppId;

            arParams[2] = new FbParameter("@WindowsLiveKey", FbDbType.VarChar, 255);
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
            sqlCommand.Append("SET AllowPasswordRetrieval = @AllowPasswordRetrieval, ");
            sqlCommand.Append("AllowPasswordReset = @AllowPasswordReset, ");
            sqlCommand.Append("RequiresQuestionAndAnswer = @RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts = @MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("PasswordAttemptWindowMinutes = @PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("RequiresUniqueEmail = @RequiresUniqueEmail, ");
            sqlCommand.Append("PasswordFormat = @PasswordFormat, ");
            sqlCommand.Append("MinRequiredPasswordLength = @MinRequiredPasswordLength, ");
            sqlCommand.Append("MinReqNonAlphaChars = @MinRequiredNonAlphanumericCharacters, ");
            sqlCommand.Append("PwdStrengthRegex = @PasswordStrengthRegularExpression, ");
            sqlCommand.Append("DefaultEmailFromAddress = @DefaultEmailFromAddress ");

            sqlCommand.Append(" WHERE SiteID = @SiteID ;");

            FbParameter[] arParams = new FbParameter[12];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@AllowPasswordRetrieval", FbDbType.Integer);
            arParams[1].Value = allowRetrieval;

            arParams[2] = new FbParameter("@AllowPasswordReset", FbDbType.Integer);
            arParams[2].Value = allowReset;

            arParams[3] = new FbParameter("@RequiresQuestionAndAnswer", FbDbType.Integer);
            arParams[3].Value = requiresQA;

            arParams[4] = new FbParameter("@MaxInvalidPasswordAttempts", FbDbType.Integer);
            arParams[4].Value = maxInvalidPasswordAttempts;

            arParams[5] = new FbParameter("@PasswordAttemptWindowMinutes", FbDbType.Integer);
            arParams[5].Value = passwordAttemptWindowMinutes;

            arParams[6] = new FbParameter("@RequiresUniqueEmail", FbDbType.Integer);
            arParams[6].Value = requiresEmail;

            arParams[7] = new FbParameter("@PasswordFormat", FbDbType.Integer);
            arParams[7].Value = passwordFormat;

            arParams[8] = new FbParameter("@MinRequiredPasswordLength", FbDbType.Integer);
            arParams[8].Value = minRequiredPasswordLength;

            arParams[9] = new FbParameter("@PasswordStrengthRegularExpression", FbDbType.VarChar);
            arParams[9].Value = passwordStrengthRegularExpression;

            arParams[10] = new FbParameter("@DefaultEmailFromAddress", FbDbType.VarChar, 100);
            arParams[10].Value = defaultEmailFromAddress;

            arParams[11] = new FbParameter("@MinRequiredNonAlphanumericCharacters", FbDbType.Integer);
            arParams[11].Value = minRequiredNonAlphanumericCharacters;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public async Task<bool> Delete(int siteId)
        {
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles WHERE UserID IN (SELECT UserID FROM mp_Users WHERE SiteID = @SiteID);");

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLocation WHERE UserGuid IN (SELECT UserGuid FROM mp_Users WHERE SiteID = @SiteID);");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Users WHERE SiteID = @SiteID; ");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Roles WHERE SiteID = @SiteID; ");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteHosts WHERE SiteID = @SiteID; ");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteSettingsEx WHERE SiteID = @SiteID; ");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteFolders WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID);");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_RedirectList WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID);");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TaskQueue WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID);");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Sites ");
            sqlCommand.Append("WHERE SiteID = @SiteID  ; ");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        //public bool HasFeature(int siteId, int moduleDefId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT Count(*) FROM mp_SiteModuleDefinitions ");
        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("SiteID = @SiteID ");
        //    sqlCommand.Append(" AND ");
        //    sqlCommand.Append("ModuleDefID = @ModuleDefID ");
        //    sqlCommand.Append(" ;");

        //    FbParameter[] arParams = new FbParameter[2];

        //    arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new FbParameter("@ModuleDefID", FbDbType.Integer);
        //    arParams[1].Value = moduleDefId;

        //    int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        ConnectionString.GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams));

        //    return (count > 0);

        //}



        //public static void AddFeature(int siteId, int moduleDefId)
        //{
        //    if (HasFeature(siteId, moduleDefId)) return;

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("INSERT INTO mp_SiteModuleDefinitions ");
        //    sqlCommand.Append("( ");
        //    sqlCommand.Append("SiteID, ");
        //    sqlCommand.Append("ModuleDefID ");
        //    sqlCommand.Append(") ");

        //    sqlCommand.Append("VALUES ");
        //    sqlCommand.Append("( ");
        //    sqlCommand.Append("@SiteID, ");
        //    sqlCommand.Append("@ModuleDefID ");
        //    sqlCommand.Append(") ;");

        //    FbParameter[] arParams = new FbParameter[2];

        //    arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new FbParameter("@ModuleDefID", FbDbType.Integer);
        //    arParams[1].Value = moduleDefId;

        //    AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetWriteConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public static bool HasFeature(Guid siteGuid, Guid featureGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT Count(*) FROM mp_SiteModuleDefinitions ");
        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("SiteGuid = @SiteGuid ");
        //    sqlCommand.Append(" AND ");
        //    sqlCommand.Append("FeatureGuid = @FeatureGuid ");
        //    sqlCommand.Append(" ;");

        //    FbParameter[] arParams = new FbParameter[2];

        //    arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
        //    arParams[0].Value = siteGuid.ToString();

        //    arParams[1] = new FbParameter("@FeatureGuid", FbDbType.Char, 36);
        //    arParams[1].Value = featureGuid.ToString();

        //    int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        ConnectionString.GetReadConnectionString(),
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
        //    sqlCommand.Append("(SELECT FIRST 1 SiteID FROM mp_Sites WHERE SiteGuid = @SiteGuid ), ");
        //    sqlCommand.Append("(SELECT FIRST 1 ModuleDefID FROM mp_ModuleDefinitions WHERE Guid = @FeatureGuid ), ");
        //    sqlCommand.Append("@SiteGuid, ");
        //    sqlCommand.Append("@FeatureGuid, ");
        //    sqlCommand.Append("'All Users' ");
        //    sqlCommand.Append(") ;");

        //    FbParameter[] arParams = new FbParameter[2];

        //    arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
        //    arParams[0].Value = siteGuid.ToString();

        //    arParams[1] = new FbParameter("@FeatureGuid", FbDbType.Char, 36);
        //    arParams[1].Value = featureGuid.ToString();

        //    AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetWriteConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public static void RemoveFeature(Guid siteGuid, Guid featureGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_SiteModuleDefinitions ");
        //    sqlCommand.Append("WHERE SiteGuid = @SiteGuid AND FeatureGuid = @FeatureGuid ; ");

        //    FbParameter[] arParams = new FbParameter[2];

        //    arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
        //    arParams[0].Value = siteGuid.ToString();

        //    arParams[1] = new FbParameter("@FeatureGuid", FbDbType.Char, 36);
        //    arParams[1].Value = featureGuid.ToString();

        //    AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetWriteConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public static void RemoveFeature(int siteId, int moduleDefId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_SiteModuleDefinitions ");
        //    sqlCommand.Append("WHERE SiteID = @SiteID AND ModuleDefID = @ModuleDefID ; ");

        //    FbParameter[] arParams = new FbParameter[2];

        //    arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new FbParameter("@ModuleDefID", FbDbType.Integer);
        //    arParams[1].Value = moduleDefId;

        //    AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetWriteConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_SiteHosts  ");
            //sqlCommand.Append("WHERE   ");
            sqlCommand.Append("ORDER BY HostName  ");
            sqlCommand.Append("	; ");

            //FbParameter[] arParams = new FbParameter[1];

            //arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = countryGuid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                null);

        }


        public async Task<DbDataReader> GetHostList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append("WHERE SiteID = @SiteID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public async Task<DbDataReader> GetHost(int hostId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append("WHERE HostID = @HostID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@HostID", FbDbType.Integer);
            arParams[0].Value = hostId;

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
            sqlCommand.Append("WHERE HostName = @HostName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@HostName", FbDbType.VarChar, 255);
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
            sqlCommand.Append("HostID, ");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("HostName ");
            sqlCommand.Append(") ");

            sqlCommand.Append("VALUES ");
            sqlCommand.Append("( ");
            sqlCommand.Append("NEXT VALUE FOR mp_SiteHosts_seq,");
            sqlCommand.Append("@SiteID, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@HostName ");
            sqlCommand.Append(") ;");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@HostName", FbDbType.VarChar, 255);
            arParams[1].Value = hostName;

            arParams[2] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
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
            sqlCommand.Append("WHERE HostID = @HostID  ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@HostID", FbDbType.Integer);
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

        public async Task<DbDataReader> GetSite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);
        }

        public DbDataReader GetSiteNonAsync(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);
        }

        public async Task<DbDataReader> GetSite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteGuid = @SiteGuid ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);
        }

        public DbDataReader GetSiteNonAsync(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteGuid = @SiteGuid ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);
        }


        public async Task<DbDataReader> GetSite(string hostName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@HostName", FbDbType.VarChar, 255);
            arParams[0].Value = hostName;

            int siteId = -1;

            sqlCommand.Append("SELECT mp_SiteHosts.SiteID ");
            sqlCommand.Append("FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE mp_SiteHosts.HostName = @HostName ;");

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
            sqlCommand.Append("SELECT FIRST 1 * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID = @SiteID OR @SiteID = -1 ");
            sqlCommand.Append("ORDER BY	SiteID ");
            sqlCommand.Append(" ;");

            arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetSiteNonAsync(string hostName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@HostName", FbDbType.VarChar, 255);
            arParams[0].Value = hostName;

            int siteId = -1;

            sqlCommand.Append("SELECT mp_SiteHosts.SiteID ");
            sqlCommand.Append("FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE mp_SiteHosts.HostName = @HostName ;");

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
            sqlCommand.Append("SELECT FIRST 1 * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID = @SiteID OR @SiteID = -1 ");
            sqlCommand.Append("ORDER BY	SiteID ");
            sqlCommand.Append(" ;");

            arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
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

            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append("ORDER BY ParentID,  PageName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
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
            sqlCommand.Append("WHERE SiteID <> @CurrentSiteID ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CurrentSiteID", FbDbType.Integer);
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_Sites  ");
            sqlCommand.Append("WHERE SiteID <> @CurrentSiteID ");
            sqlCommand.Append("ORDER BY  SiteName ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CurrentSiteID", FbDbType.Integer);
            arParams[0].Value = currentSiteId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

        public async Task<int> GetSiteIdByHostName(string hostName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@HostName", FbDbType.VarChar, 255);
            arParams[0].Value = hostName;

            sqlCommand.Append("SELECT FIRST 1 SiteID ");
            sqlCommand.Append("FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE HostName = @HostName ORDER BY SiteID ;");

            int siteId = -1;

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
                sqlCommand.Append("SELECT FIRST 1 SiteID ");
                sqlCommand.Append("FROM	mp_Sites ");
                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append(" ;");

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
            StringBuilder sqlCommand = new StringBuilder();

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@FolderName", FbDbType.VarChar, 255);
            arParams[0].Value = folderName;

            sqlCommand.Append("SELECT FIRST 1 COALESCE(s.SiteID, -1) As SiteID ");
            sqlCommand.Append("FROM mp_SiteFolders sf ");
            sqlCommand.Append("JOIN mp_Sites s ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("sf.SiteGuid = s.SiteGuid ");
            sqlCommand.Append("WHERE sf.FolderName = @FolderName ORDER BY s.SiteID ;");

            int siteId = -1;

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
                sqlCommand.Append("SELECT FIRST 1 SiteID ");
                sqlCommand.Append("FROM	mp_Sites ");
                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append(" ;");

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
            StringBuilder sqlCommand = new StringBuilder();

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@FolderName", FbDbType.VarChar, 255);
            arParams[0].Value = folderName;

            sqlCommand.Append("SELECT FIRST 1 COALESCE(s.SiteID, -1) As SiteID ");
            sqlCommand.Append("FROM mp_SiteFolders sf ");
            sqlCommand.Append("JOIN mp_Sites s ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("sf.SiteGuid = s.SiteGuid ");
            sqlCommand.Append("WHERE sf.FolderName = @FolderName ORDER BY s.SiteID ;");

            int siteId = -1;

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
                sqlCommand.Append("SELECT FIRST 1 SiteID ");
                sqlCommand.Append("FROM	mp_Sites ");
                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append(" ;");

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
