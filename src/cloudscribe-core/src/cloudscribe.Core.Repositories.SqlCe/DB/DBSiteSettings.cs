// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2010-03-10
// Last Modified:			2015-06-15
// 


using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Data.SqlServerCe;
using cloudscribe.DbHelpers.SqlCe;
using Microsoft.Framework.Logging;

namespace cloudscribe.Core.Repositories.SqlCe
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Sites ");
            sqlCommand.Append("(");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("SiteName, ");
            sqlCommand.Append("Skin, ");
            sqlCommand.Append("Logo, ");
            sqlCommand.Append("Icon, ");
            sqlCommand.Append("AllowUserSkins, ");
            sqlCommand.Append("AllowPageSkins, ");
            sqlCommand.Append("AllowHideMenuOnPages, ");
            sqlCommand.Append("AllowNewRegistration, ");
            sqlCommand.Append("UseSecureRegistration, ");
            sqlCommand.Append("UseSSLOnAllPages, ");
            sqlCommand.Append("DefaultPageKeyWords, ");
            sqlCommand.Append("DefaultPageDescription, ");
            sqlCommand.Append("DefaultPageEncoding, ");
            sqlCommand.Append("DefaultAdditionalMetaTags, ");
            sqlCommand.Append("IsServerAdminSite, ");
            sqlCommand.Append("UseLdapAuth, ");
            sqlCommand.Append("AutoCreateLdapUserOnFirstLogin, ");
            sqlCommand.Append("LdapServer, ");
            sqlCommand.Append("LdapPort, ");
            sqlCommand.Append("LdapDomain, ");
            sqlCommand.Append("LdapRootDN, ");
            sqlCommand.Append("LdapUserDNKey, ");
            sqlCommand.Append("ReallyDeleteUsers, ");
            sqlCommand.Append("UseEmailForLogin, ");
            sqlCommand.Append("AllowUserFullNameChange, ");
            sqlCommand.Append("EditorSkin, ");
            sqlCommand.Append("DefaultFriendlyUrlPatternEnum, ");
            sqlCommand.Append("AllowPasswordRetrieval, ");
            sqlCommand.Append("AllowPasswordReset, ");
            sqlCommand.Append("RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("RequiresUniqueEmail, ");
            sqlCommand.Append("PasswordFormat, ");
            sqlCommand.Append("MinRequiredPasswordLength, ");
            sqlCommand.Append("MinReqNonAlphaChars, ");
            sqlCommand.Append("PwdStrengthRegex, ");
            sqlCommand.Append("DefaultEmailFromAddress, ");
            sqlCommand.Append("EnableMyPageFeature, ");
            sqlCommand.Append("EditorProvider, ");
            sqlCommand.Append("CaptchaProvider, ");
            sqlCommand.Append("DatePickerProvider, ");
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
            sqlCommand.Append("DisableDbAuth ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@SiteName, ");
            sqlCommand.Append("@Skin, ");
            sqlCommand.Append("@Logo, ");
            sqlCommand.Append("@Icon, ");
            sqlCommand.Append("@AllowUserSkins, ");
            sqlCommand.Append("@AllowPageSkins, ");
            sqlCommand.Append("@AllowHideMenuOnPages, ");
            sqlCommand.Append("@AllowNewRegistration, ");
            sqlCommand.Append("@UseSecureRegistration, ");
            sqlCommand.Append("@UseSSLOnAllPages, ");
            sqlCommand.Append("@DefaultPageKeyWords, ");
            sqlCommand.Append("@DefaultPageDescription, ");
            sqlCommand.Append("@DefaultPageEncoding, ");
            sqlCommand.Append("@DefaultAdditionalMetaTags, ");
            sqlCommand.Append("@IsServerAdminSite, ");
            sqlCommand.Append("@UseLdapAuth, ");
            sqlCommand.Append("@AutoCreateLdapUserOnFirstLogin, ");
            sqlCommand.Append("@LdapServer, ");
            sqlCommand.Append("@LdapPort, ");
            sqlCommand.Append("@LdapDomain, ");
            sqlCommand.Append("@LdapRootDN, ");
            sqlCommand.Append("@LdapUserDNKey, ");
            sqlCommand.Append("@ReallyDeleteUsers, ");
            sqlCommand.Append("@UseEmailForLogin, ");
            sqlCommand.Append("@AllowUserFullNameChange, ");
            sqlCommand.Append("@EditorSkin, ");
            sqlCommand.Append("@DefaultFriendlyUrlPatternEnum, ");
            sqlCommand.Append("@AllowPasswordRetrieval, ");
            sqlCommand.Append("@AllowPasswordReset, ");
            sqlCommand.Append("@RequiresQuestionAndAnswer, ");
            sqlCommand.Append("@MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("@PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("@RequiresUniqueEmail, ");
            sqlCommand.Append("@PasswordFormat, ");
            sqlCommand.Append("@MinRequiredPasswordLength, ");
            sqlCommand.Append("@MinReqNonAlphaChars, ");
            sqlCommand.Append("@PwdStrengthRegex, ");
            sqlCommand.Append("@DefaultEmailFromAddress, ");
            sqlCommand.Append("@EnableMyPageFeature, ");
            sqlCommand.Append("@EditorProvider, ");
            sqlCommand.Append("@CaptchaProvider, ");
            sqlCommand.Append("@DatePickerProvider, ");
            sqlCommand.Append("@RecaptchaPrivateKey, ");
            sqlCommand.Append("@RecaptchaPublicKey, ");
            sqlCommand.Append("@WordpressAPIKey, ");
            sqlCommand.Append("@WindowsLiveAppID, ");
            sqlCommand.Append("@WindowsLiveKey, ");
            sqlCommand.Append("@AllowOpenIDAuth, ");
            sqlCommand.Append("@AllowWindowsLiveAuth, ");
            sqlCommand.Append("@GmapApiKey, ");
            sqlCommand.Append("@ApiKeyExtra1, ");
            sqlCommand.Append("@ApiKeyExtra2, ");
            sqlCommand.Append("@ApiKeyExtra3, ");
            sqlCommand.Append("@ApiKeyExtra4, ");
            sqlCommand.Append("@ApiKeyExtra5, ");
            sqlCommand.Append("@DisableDbAuth ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[58];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@SiteAlias", SqlDbType.NVarChar);
            arParams[1].Value = string.Empty;

            arParams[2] = new SqlCeParameter("@SiteName", SqlDbType.NVarChar);
            arParams[2].Value = siteName;

            arParams[3] = new SqlCeParameter("@Skin", SqlDbType.NVarChar);
            arParams[3].Value = skin;

            arParams[4] = new SqlCeParameter("@Logo", SqlDbType.NVarChar);
            arParams[4].Value = logo;

            arParams[5] = new SqlCeParameter("@Icon", SqlDbType.NVarChar);
            arParams[5].Value = icon;

            arParams[6] = new SqlCeParameter("@AllowUserSkins", SqlDbType.Bit);
            arParams[6].Value = allowUserSkins;

            arParams[7] = new SqlCeParameter("@AllowPageSkins", SqlDbType.Bit);
            arParams[7].Value = allowPageSkins;

            arParams[8] = new SqlCeParameter("@AllowHideMenuOnPages", SqlDbType.Bit);
            arParams[8].Value = allowHideMenuOnPages;

            arParams[9] = new SqlCeParameter("@AllowNewRegistration", SqlDbType.Bit);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = allowNewRegistration;

            arParams[10] = new SqlCeParameter("@UseSecureRegistration", SqlDbType.Bit);
            arParams[10].Value = useSecureRegistration;

            arParams[11] = new SqlCeParameter("@UseSSLOnAllPages", SqlDbType.Bit);
            arParams[11].Value = useSslOnAllPages;

            arParams[12] = new SqlCeParameter("@DefaultPageKeyWords", SqlDbType.NVarChar);
            arParams[12].Value = defaultPageKeywords;

            arParams[13] = new SqlCeParameter("@DefaultPageDescription", SqlDbType.NVarChar);
            arParams[13].Value = defaultPageDescription;

            arParams[14] = new SqlCeParameter("@DefaultPageEncoding", SqlDbType.NVarChar);
            arParams[14].Value = defaultPageEncoding;

            arParams[15] = new SqlCeParameter("@DefaultAdditionalMetaTags", SqlDbType.NVarChar);
            arParams[15].Value = defaultAdditionalMetaTags;

            arParams[16] = new SqlCeParameter("@IsServerAdminSite", SqlDbType.Bit);
            arParams[16].Value = isServerAdminSite;

            arParams[17] = new SqlCeParameter("@UseLdapAuth", SqlDbType.Bit);
            arParams[17].Value = useLdapAuth;

            arParams[18] = new SqlCeParameter("@AutoCreateLdapUserOnFirstLogin", SqlDbType.Bit);
            arParams[18].Value = autoCreateLdapUserOnFirstLogin;

            arParams[19] = new SqlCeParameter("@LdapServer", SqlDbType.NVarChar);
            arParams[19].Value = ldapServer;

            arParams[20] = new SqlCeParameter("@LdapPort", SqlDbType.Int);
            arParams[20].Value = ldapPort;

            arParams[21] = new SqlCeParameter("@LdapDomain", SqlDbType.NVarChar);
            arParams[21].Value = ldapDomain;

            arParams[22] = new SqlCeParameter("@LdapRootDN", SqlDbType.NVarChar);
            arParams[22].Value = ldapRootDN;

            arParams[23] = new SqlCeParameter("@LdapUserDNKey", SqlDbType.NVarChar);
            arParams[23].Value = ldapUserDNKey;

            arParams[24] = new SqlCeParameter("@ReallyDeleteUsers", SqlDbType.Bit);
            arParams[24].Value = reallyDeleteUsers;

            arParams[25] = new SqlCeParameter("@UseEmailForLogin", SqlDbType.Bit);
            arParams[25].Value = useEmailForLogin;

            arParams[26] = new SqlCeParameter("@AllowUserFullNameChange", SqlDbType.Bit);
            arParams[26].Value = allowUserFullNameChange;

            arParams[27] = new SqlCeParameter("@EditorSkin", SqlDbType.NVarChar);
            arParams[27].Value = editorSkin;

            arParams[28] = new SqlCeParameter("@DefaultFriendlyUrlPatternEnum", SqlDbType.NVarChar);
            arParams[28].Value = defaultFriendlyUrlPattern;

            arParams[29] = new SqlCeParameter("@AllowPasswordRetrieval", SqlDbType.Bit);
            arParams[29].Value = true;

            arParams[30] = new SqlCeParameter("@AllowPasswordReset", SqlDbType.Bit);
            arParams[30].Value = true;

            arParams[31] = new SqlCeParameter("@RequiresQuestionAndAnswer", SqlDbType.Bit);
            arParams[31].Value = true;

            arParams[32] = new SqlCeParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int);
            arParams[32].Value = 5;

            arParams[33] = new SqlCeParameter("@PasswordAttemptWindowMinutes", SqlDbType.Int);
            arParams[33].Value = 5;

            arParams[34] = new SqlCeParameter("@RequiresUniqueEmail", SqlDbType.Bit);
            arParams[34].Value = true;

            arParams[35] = new SqlCeParameter("@PasswordFormat", SqlDbType.Int);
            arParams[35].Value = 0;

            arParams[36] = new SqlCeParameter("@MinRequiredPasswordLength", SqlDbType.Int);
            arParams[36].Value = 7;

            arParams[37] = new SqlCeParameter("@MinReqNonAlphaChars", SqlDbType.Int);
            arParams[37].Value = 0;

            arParams[38] = new SqlCeParameter("@PwdStrengthRegex", SqlDbType.NText);
            arParams[38].Value = string.Empty;

            arParams[39] = new SqlCeParameter("@DefaultEmailFromAddress", SqlDbType.NVarChar);
            arParams[39].Value = string.Empty;

            arParams[40] = new SqlCeParameter("@EnableMyPageFeature", SqlDbType.Bit);
            arParams[40].Value = enableMyPageFeature;

            arParams[41] = new SqlCeParameter("@EditorProvider", SqlDbType.NVarChar);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = editorProvider;

            arParams[42] = new SqlCeParameter("@CaptchaProvider", SqlDbType.NVarChar);
            arParams[42].Value = captchaProvider;

            arParams[43] = new SqlCeParameter("@DatePickerProvider", SqlDbType.NVarChar);
            arParams[43].Value = datePickerProvider;

            arParams[44] = new SqlCeParameter("@RecaptchaPrivateKey", SqlDbType.NVarChar);
            arParams[44].Value = recaptchaPrivateKey;

            arParams[45] = new SqlCeParameter("@RecaptchaPublicKey", SqlDbType.NVarChar);
            arParams[45].Value = recaptchaPublicKey;

            arParams[46] = new SqlCeParameter("@WordpressAPIKey", SqlDbType.NVarChar);
            arParams[46].Value = wordpressApiKey;

            arParams[47] = new SqlCeParameter("@WindowsLiveAppID", SqlDbType.NVarChar);
            arParams[47].Value = windowsLiveAppId;

            arParams[48] = new SqlCeParameter("@WindowsLiveKey", SqlDbType.NVarChar);
            arParams[48].Value = windowsLiveKey;

            arParams[49] = new SqlCeParameter("@AllowOpenIDAuth", SqlDbType.Bit);
            arParams[49].Value = allowOpenIdAuth;

            arParams[50] = new SqlCeParameter("@AllowWindowsLiveAuth", SqlDbType.Bit);
            arParams[50].Value = allowWindowsLiveAuth;

            arParams[51] = new SqlCeParameter("@GmapApiKey", SqlDbType.NVarChar);
            arParams[51].Value = gmapApiKey;

            arParams[52] = new SqlCeParameter("@ApiKeyExtra1", SqlDbType.NVarChar);
            arParams[52].Value = apiKeyExtra1;

            arParams[53] = new SqlCeParameter("@ApiKeyExtra2", SqlDbType.NVarChar);
            arParams[53].Value = apiKeyExtra2;

            arParams[54] = new SqlCeParameter("@ApiKeyExtra3", SqlDbType.NVarChar);
            arParams[54].Value = apiKeyExtra3;

            arParams[55] = new SqlCeParameter("@ApiKeyExtra4", SqlDbType.NVarChar);
            arParams[55].Value = apiKeyExtra4;

            arParams[56] = new SqlCeParameter("@ApiKeyExtra5", SqlDbType.NVarChar);
            arParams[56].Value = apiKeyExtra5;

            arParams[57] = new SqlCeParameter("@DisableDbAuth", SqlDbType.Bit);
            arParams[57].Value = disableDbAuth;

            int newId = Convert.ToInt32(AdoHelper.DoInsertGetIdentitiy(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("SiteName = @SiteName, ");
            sqlCommand.Append("Skin = @Skin, ");
            sqlCommand.Append("Logo = @Logo, ");
            sqlCommand.Append("Icon = @Icon, ");
            sqlCommand.Append("AllowUserSkins = @AllowUserSkins, ");
            sqlCommand.Append("AllowPageSkins = @AllowPageSkins, ");
            sqlCommand.Append("AllowHideMenuOnPages = @AllowHideMenuOnPages, ");
            sqlCommand.Append("AllowNewRegistration = @AllowNewRegistration, ");
            sqlCommand.Append("UseSecureRegistration = @UseSecureRegistration, ");
            sqlCommand.Append("UseSSLOnAllPages = @UseSSLOnAllPages, ");
            sqlCommand.Append("DefaultPageKeyWords = @DefaultPageKeyWords, ");
            sqlCommand.Append("DefaultPageDescription = @DefaultPageDescription, ");
            sqlCommand.Append("DefaultPageEncoding = @DefaultPageEncoding, ");
            sqlCommand.Append("DefaultAdditionalMetaTags = @DefaultAdditionalMetaTags, ");
            sqlCommand.Append("IsServerAdminSite = @IsServerAdminSite, ");
            sqlCommand.Append("UseLdapAuth = @UseLdapAuth, ");
            sqlCommand.Append("AutoCreateLdapUserOnFirstLogin = @AutoCreateLdapUserOnFirstLogin, ");
            sqlCommand.Append("LdapServer = @LdapServer, ");
            sqlCommand.Append("LdapPort = @LdapPort, ");
            sqlCommand.Append("LdapDomain = @LdapDomain, ");
            sqlCommand.Append("LdapRootDN = @LdapRootDN, ");
            sqlCommand.Append("LdapUserDNKey = @LdapUserDNKey, ");
            sqlCommand.Append("ReallyDeleteUsers = @ReallyDeleteUsers, ");
            sqlCommand.Append("UseEmailForLogin = @UseEmailForLogin, ");
            sqlCommand.Append("AllowUserFullNameChange = @AllowUserFullNameChange, ");
            sqlCommand.Append("EditorSkin = @EditorSkin, ");
            sqlCommand.Append("DefaultFriendlyUrlPatternEnum = @DefaultFriendlyUrlPatternEnum, ");
            sqlCommand.Append("EnableMyPageFeature = @EnableMyPageFeature, ");
            sqlCommand.Append("EditorProvider = @EditorProvider, ");
            sqlCommand.Append("CaptchaProvider = @CaptchaProvider, ");
            sqlCommand.Append("DatePickerProvider = @DatePickerProvider, ");
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
            sqlCommand.Append("DisableDbAuth = @DisableDbAuth ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[47];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@SiteName", SqlDbType.NVarChar);
            arParams[1].Value = siteName;

            arParams[2] = new SqlCeParameter("@Skin", SqlDbType.NVarChar);
            arParams[2].Value = skin;

            arParams[3] = new SqlCeParameter("@Logo", SqlDbType.NVarChar);
            arParams[3].Value = logo;

            arParams[4] = new SqlCeParameter("@Icon", SqlDbType.NVarChar);
            arParams[4].Value = icon;

            arParams[5] = new SqlCeParameter("@AllowUserSkins", SqlDbType.Bit);
            arParams[5].Value = allowUserSkins;

            arParams[6] = new SqlCeParameter("@AllowPageSkins", SqlDbType.Bit);
            arParams[6].Value = allowPageSkins;

            arParams[7] = new SqlCeParameter("@AllowHideMenuOnPages", SqlDbType.Bit);
            arParams[7].Value = allowHideMenuOnPages;

            arParams[8] = new SqlCeParameter("@AllowNewRegistration", SqlDbType.Bit);
            arParams[8].Value = allowNewRegistration;

            arParams[9] = new SqlCeParameter("@UseSecureRegistration", SqlDbType.Bit);
            arParams[9].Value = useSecureRegistration;

            arParams[10] = new SqlCeParameter("@UseSSLOnAllPages", SqlDbType.Bit);
            arParams[10].Value = useSslOnAllPages;

            arParams[11] = new SqlCeParameter("@DefaultPageKeyWords", SqlDbType.NVarChar);
            arParams[11].Value = defaultPageKeywords;

            arParams[12] = new SqlCeParameter("@DefaultPageDescription", SqlDbType.NVarChar);
            arParams[12].Value = defaultPageDescription;

            arParams[13] = new SqlCeParameter("@DefaultPageEncoding", SqlDbType.NVarChar);
            arParams[13].Value = defaultPageEncoding;

            arParams[14] = new SqlCeParameter("@DefaultAdditionalMetaTags", SqlDbType.NVarChar);
            arParams[14].Value = defaultAdditionalMetaTags;

            arParams[15] = new SqlCeParameter("@IsServerAdminSite", SqlDbType.Bit);
            arParams[15].Value = isServerAdminSite;

            arParams[16] = new SqlCeParameter("@UseLdapAuth", SqlDbType.Bit);
            arParams[16].Value = useLdapAuth;

            arParams[17] = new SqlCeParameter("@AutoCreateLdapUserOnFirstLogin", SqlDbType.Bit);
            arParams[17].Value = autoCreateLdapUserOnFirstLogin;

            arParams[18] = new SqlCeParameter("@LdapServer", SqlDbType.NVarChar);
            arParams[18].Value = ldapServer;

            arParams[19] = new SqlCeParameter("@LdapPort", SqlDbType.Int);
            arParams[19].Value = ldapPort;

            arParams[20] = new SqlCeParameter("@LdapDomain", SqlDbType.NVarChar);
            arParams[20].Value = ldapDomain;

            arParams[22] = new SqlCeParameter("@LdapRootDN", SqlDbType.NVarChar);
            arParams[22].Value = ldapRootDN;

            arParams[23] = new SqlCeParameter("@LdapUserDNKey", SqlDbType.NVarChar);
            arParams[23].Value = ldapUserDNKey;

            arParams[24] = new SqlCeParameter("@ReallyDeleteUsers", SqlDbType.Bit);
            arParams[24].Value = reallyDeleteUsers;

            arParams[25] = new SqlCeParameter("@UseEmailForLogin", SqlDbType.Bit);
            arParams[25].Value = useEmailForLogin;

            arParams[26] = new SqlCeParameter("@AllowUserFullNameChange", SqlDbType.Bit);
            arParams[26].Value = allowUserFullNameChange;

            arParams[27] = new SqlCeParameter("@EditorSkin", SqlDbType.NVarChar);
            arParams[27].Value = editorSkin;

            arParams[28] = new SqlCeParameter("@DefaultFriendlyUrlPatternEnum", SqlDbType.NVarChar);
            arParams[28].Value = defaultFriendlyUrlPattern;

            arParams[29] = new SqlCeParameter("@EnableMyPageFeature", SqlDbType.Bit);
            arParams[29].Value = enableMyPageFeature;

            arParams[30] = new SqlCeParameter("@EditorProvider", SqlDbType.NVarChar);
            arParams[30].Value = editorProvider;

            arParams[31] = new SqlCeParameter("@CaptchaProvider", SqlDbType.NVarChar);
            arParams[31].Value = captchaProvider;

            arParams[32] = new SqlCeParameter("@DatePickerProvider", SqlDbType.NVarChar);
            arParams[32].Value = datePickerProvider;

            arParams[33] = new SqlCeParameter("@RecaptchaPrivateKey", SqlDbType.NVarChar);
            arParams[33].Value = recaptchaPrivateKey;

            arParams[34] = new SqlCeParameter("@RecaptchaPublicKey", SqlDbType.NVarChar);
            arParams[34].Value = recaptchaPublicKey;

            arParams[35] = new SqlCeParameter("@WordpressAPIKey", SqlDbType.NVarChar);
            arParams[35].Value = wordpressApiKey;

            arParams[36] = new SqlCeParameter("@WindowsLiveAppID", SqlDbType.NVarChar);
            arParams[36].Value = windowsLiveAppId;

            arParams[37] = new SqlCeParameter("@WindowsLiveKey", SqlDbType.NVarChar);
            arParams[37].Value = windowsLiveKey;

            arParams[38] = new SqlCeParameter("@AllowOpenIDAuth", SqlDbType.Bit);
            arParams[38].Value = allowOpenIdAuth;

            arParams[39] = new SqlCeParameter("@AllowWindowsLiveAuth", SqlDbType.Bit);
            arParams[39].Value = allowWindowsLiveAuth;

            arParams[40] = new SqlCeParameter("@GmapApiKey", SqlDbType.NVarChar);
            arParams[40].Value = gmapApiKey;

            arParams[41] = new SqlCeParameter("@ApiKeyExtra1", SqlDbType.NVarChar);
            arParams[41].Value = apiKeyExtra1;

            arParams[42] = new SqlCeParameter("@ApiKeyExtra2", SqlDbType.NVarChar);
            arParams[42].Value = apiKeyExtra2;

            arParams[43] = new SqlCeParameter("@ApiKeyExtra3", SqlDbType.NVarChar);
            arParams[43].Value = apiKeyExtra3;

            arParams[44] = new SqlCeParameter("@ApiKeyExtra4", SqlDbType.NVarChar);
            arParams[44].Value = apiKeyExtra4;

            arParams[45] = new SqlCeParameter("@ApiKeyExtra5", SqlDbType.NVarChar);
            arParams[45].Value = apiKeyExtra5;

            arParams[46] = new SqlCeParameter("@DisableDbAuth", SqlDbType.Bit);
            arParams[46].Value = disableDbAuth;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("AllowPasswordRetrieval = @AllowPasswordRetrieval, ");
            sqlCommand.Append("AllowPasswordReset = @AllowPasswordReset, ");
            sqlCommand.Append("RequiresQuestionAndAnswer = @RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts = @MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("PasswordAttemptWindowMinutes = @PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("RequiresUniqueEmail = @RequiresUniqueEmail, ");
            sqlCommand.Append("PasswordFormat = @PasswordFormat, ");
            sqlCommand.Append("MinRequiredPasswordLength = @MinRequiredPasswordLength, ");
            sqlCommand.Append("MinReqNonAlphaChars = @MinReqNonAlphaChars, ");
            sqlCommand.Append("PwdStrengthRegex = @PwdStrengthRegex, ");
            sqlCommand.Append("DefaultEmailFromAddress = @DefaultEmailFromAddress ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[12];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@AllowPasswordRetrieval", SqlDbType.Bit);
            arParams[1].Value = allowPasswordRetrieval;

            arParams[2] = new SqlCeParameter("@AllowPasswordReset", SqlDbType.Bit);
            arParams[2].Value = allowPasswordReset;

            arParams[3] = new SqlCeParameter("@RequiresQuestionAndAnswer", SqlDbType.Bit);
            arParams[3].Value = requiresQuestionAndAnswer;

            arParams[4] = new SqlCeParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int);
            arParams[4].Value = maxInvalidPasswordAttempts;

            arParams[5] = new SqlCeParameter("@PasswordAttemptWindowMinutes", SqlDbType.Int);
            arParams[5].Value = passwordAttemptWindowMinutes;

            arParams[6] = new SqlCeParameter("@RequiresUniqueEmail", SqlDbType.Bit);
            arParams[6].Value = requiresUniqueEmail;

            arParams[7] = new SqlCeParameter("@PasswordFormat", SqlDbType.Int);
            arParams[7].Value = passwordFormat;

            arParams[8] = new SqlCeParameter("@MinRequiredPasswordLength", SqlDbType.Int);
            arParams[8].Value = minRequiredPasswordLength;

            arParams[9] = new SqlCeParameter("@MinReqNonAlphaChars", SqlDbType.Int);
            arParams[9].Value = minRequiredNonAlphanumericCharacters;

            arParams[10] = new SqlCeParameter("@PwdStrengthRegex", SqlDbType.NText);
            arParams[10].Value = passwordStrengthRegularExpression;

            arParams[11] = new SqlCeParameter("@DefaultEmailFromAddress", SqlDbType.NVarChar);
            arParams[11].Value = defaultEmailFromAddress;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("AllowNewRegistration = @AllowNewRegistration, ");
            sqlCommand.Append("UseSecureRegistration = @UseSecureRegistration, ");
            sqlCommand.Append("UseLdapAuth = @UseLdapAuth, ");
            sqlCommand.Append("AutoCreateLdapUserOnFirstLogin = @AutoCreateLdapUserOnFirstLogin, ");
            sqlCommand.Append("LdapServer = @LdapServer, ");
            sqlCommand.Append("LdapPort = @LdapPort, ");
            sqlCommand.Append("LdapDomain = @LdapDomain, ");
            sqlCommand.Append("LdapRootDN = @LdapRootDN, ");
            sqlCommand.Append("LdapUserDNKey = @LdapUserDNKey, ");
            sqlCommand.Append("UseEmailForLogin = @UseEmailForLogin, ");
            sqlCommand.Append("AllowUserFullNameChange = @AllowUserFullNameChange, ");
            sqlCommand.Append("AllowPasswordRetrieval = @AllowPasswordRetrieval, ");
            sqlCommand.Append("AllowPasswordReset = @AllowPasswordReset, ");
            sqlCommand.Append("RequiresQuestionAndAnswer = @RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts = @MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("PasswordAttemptWindowMinutes = @PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("RequiresUniqueEmail = @RequiresUniqueEmail, ");
            sqlCommand.Append("PasswordFormat = @PasswordFormat, ");
            sqlCommand.Append("MinRequiredPasswordLength = @MinRequiredPasswordLength, ");
            sqlCommand.Append("MinReqNonAlphaChars = @MinReqNonAlphaChars, ");
            sqlCommand.Append("PwdStrengthRegex = @PwdStrengthRegex, ");
            sqlCommand.Append("AllowOpenIDAuth = @AllowOpenIDAuth, ");
            sqlCommand.Append("AllowWindowsLiveAuth = @AllowWindowsLiveAuth ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID <> @SiteID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[24];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@AllowNewRegistration", SqlDbType.Bit);
            arParams[1].Value = allowNewRegistration;

            arParams[2] = new SqlCeParameter("@UseSecureRegistration", SqlDbType.Bit);
            arParams[2].Value = useSecureRegistration;

            arParams[3] = new SqlCeParameter("@UseLdapAuth", SqlDbType.Bit);
            arParams[3].Value = useLdapAuth;

            arParams[4] = new SqlCeParameter("@AutoCreateLdapUserOnFirstLogin", SqlDbType.Bit);
            arParams[4].Value = autoCreateLdapUserOnFirstLogin;

            arParams[5] = new SqlCeParameter("@LdapServer", SqlDbType.NVarChar);
            arParams[5].Value = ldapServer;

            arParams[6] = new SqlCeParameter("@LdapPort", SqlDbType.Int);
            arParams[6].Value = ldapPort;

            arParams[7] = new SqlCeParameter("@LdapDomain", SqlDbType.NVarChar);
            arParams[7].Value = ldapDomain;

            arParams[8] = new SqlCeParameter("@LdapRootDN", SqlDbType.NVarChar);
            arParams[8].Value = ldapRootDN;

            arParams[9] = new SqlCeParameter("@LdapUserDNKey", SqlDbType.NVarChar);
            arParams[9].Value = ldapUserDNKey;

            arParams[10] = new SqlCeParameter("@UseEmailForLogin", SqlDbType.Bit);
            arParams[10].Value = useEmailForLogin;

            arParams[11] = new SqlCeParameter("@AllowUserFullNameChange", SqlDbType.Bit);
            arParams[11].Value = allowUserFullNameChange;

            arParams[12] = new SqlCeParameter("@AllowPasswordRetrieval", SqlDbType.Bit);
            arParams[12].Value = allowPasswordRetrieval;

            arParams[13] = new SqlCeParameter("@AllowPasswordReset", SqlDbType.Bit);
            arParams[13].Value = allowPasswordReset;

            arParams[14] = new SqlCeParameter("@RequiresQuestionAndAnswer", SqlDbType.Bit);
            arParams[14].Value = requiresQuestionAndAnswer;

            arParams[15] = new SqlCeParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int);
            arParams[15].Value = maxInvalidPasswordAttempts;

            arParams[16] = new SqlCeParameter("@PasswordAttemptWindowMinutes", SqlDbType.Int);
            arParams[16].Value = passwordAttemptWindowMinutes;

            arParams[17] = new SqlCeParameter("@RequiresUniqueEmail", SqlDbType.Bit);
            arParams[17].Value = requiresUniqueEmail;

            arParams[18] = new SqlCeParameter("@PasswordFormat", SqlDbType.Int);
            arParams[18].Value = passwordFormat;

            arParams[19] = new SqlCeParameter("@MinRequiredPasswordLength", SqlDbType.Int);
            arParams[19].Value = minRequiredPasswordLength;

            arParams[20] = new SqlCeParameter("@MinReqNonAlphaChars", SqlDbType.Int);
            arParams[20].Value = minReqNonAlphaChars;

            arParams[21] = new SqlCeParameter("@PwdStrengthRegex", SqlDbType.NText);
            arParams[21].Value = pwdStrengthRegex;

            arParams[22] = new SqlCeParameter("@AllowOpenIDAuth", SqlDbType.Bit);
            arParams[22].Value = allowOpenIdAuth;

            arParams[23] = new SqlCeParameter("@AllowWindowsLiveAuth", SqlDbType.Bit);
            arParams[23].Value = allowWindowsLiveAuth;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool UpdateRelatedSitesWindowsLive(
            int siteId,
            string windowsLiveAppId,
            string windowsLiveKey
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("WindowsLiveAppID = @WindowsLiveAppID, ");
            sqlCommand.Append("WindowsLiveKey = @WindowsLiveKey ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID <> @SiteID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@WindowsLiveAppID", SqlDbType.NVarChar);
            arParams[1].Value = windowsLiveAppId;

            arParams[2] = new SqlCeParameter("@WindowsLiveKey", SqlDbType.NVarChar);
            arParams[2].Value = windowsLiveKey;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public bool Delete(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Sites ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public DbDataReader GetSiteList()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("SiteName ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }

        public DbDataReader GetSite(string hostName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@HostName", SqlDbType.NVarChar, 255);
            arParams[0].Value = hostName;

            int siteId = -1;

            sqlCommand.Append("SELECT SiteID ");
            sqlCommand.Append("FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE HostName = @HostName ;");

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    siteId = Convert.ToInt32(reader["SiteID"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP (1) * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID = @SiteID OR @SiteID = -1 ");
            sqlCommand.Append("ORDER BY	SiteID ");
            sqlCommand.Append(" ;");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public Guid GetSiteGuidFromID(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT SiteGuid ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            Guid siteGuid = Guid.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
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

        //private static bool HasFeature(Guid siteGuid, Guid featureGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT Count(*) FROM mp_SiteModuleDefinitions ");
        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("SiteGuid = @SiteGuid ");
        //    sqlCommand.Append(" AND ");
        //    sqlCommand.Append("FeatureGuid = @FeatureGuid ");
        //    sqlCommand.Append(" ;");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
        //    arParams[0].Value = siteGuid;

        //    arParams[1] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
        //    arParams[1].Value = featureGuid;

        //    int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        ConnectionString.GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams));

        //    return (count > 0);

        //}

        //public static void AddFeature(Guid siteGuid, Guid featureGuid)
        //{
        //    if (HasFeature(siteGuid, featureGuid)) return;

        //    int siteId = GetSiteId(siteGuid);
        //    int moduleDefId = GetFeatureId(featureGuid);

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
        //    sqlCommand.Append("@SiteID, ");
        //    sqlCommand.Append("@ModuleDefID, ");
        //    sqlCommand.Append("@SiteGuid, ");
        //    sqlCommand.Append("@FeatureGuid, ");
        //    sqlCommand.Append("'All Users' ");
        //    sqlCommand.Append(") ;");

        //    SqlCeParameter[] arParams = new SqlCeParameter[4];

        //    arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
        //    arParams[0].Value = siteGuid;

        //    arParams[1] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
        //    arParams[1].Value = featureGuid;

        //    arParams[2] = new SqlCeParameter("@SiteID", SqlDbType.Int);
        //    arParams[2].Value = siteId;

        //    arParams[3] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
        //    arParams[3].Value = moduleDefId;

        //    AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        private int GetSiteId(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(1) COALESCE(SiteID, -1) FROM mp_Sites WHERE SiteGuid = @SiteGuid ");
            sqlCommand.Append(" ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = siteGuid;

            int id = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return id;

        }

        private int GetFeatureId(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(1) COALESCE(ModuleDefID, -1) FROM mp_ModuleDefinitions WHERE Guid = @FeatureGuid ");
            sqlCommand.Append(" ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = featureGuid;

            int id = Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return id;

        }

        //public static void RemoveFeature(Guid siteGuid, Guid featureGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_SiteModuleDefinitions ");
        //    sqlCommand.Append("WHERE SiteGuid = @SiteGuid AND FeatureGuid = @FeatureGuid ; ");

        //    SqlCeParameter[] arParams = new SqlCeParameter[2];

        //    arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
        //    arParams[0].Value = siteGuid;

        //    arParams[1] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
        //    arParams[1].Value = featureGuid;

        //    AdoHelper.ExecuteNonQuery(
        //        ConnectionString.GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        public DbDataReader GetSite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetSite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteGuid = @SiteGuid ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = siteGuid;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
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

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public int GetHostCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append(";");


            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
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
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }

        public DbDataReader GetPageHosts(
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_SiteHosts  ");
            //sqlCommand.Append("WHERE   ");

            sqlCommand.Append("ORDER BY HostName  ");

            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(";");

            //SqlCeParameter[] arParams = new SqlCeParameter[1];

            //arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            //arParams[0].Value = applicationId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }

        public DbDataReader GetHostList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append("WHERE SiteID = @SiteID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public DbDataReader GetHost(string hostName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append("WHERE HostName = @HostName ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@HostName", SqlDbType.NVarChar, 255);
            arParams[0].Value = hostName;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
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
            sqlCommand.Append("@SiteID, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@HostName ");
            sqlCommand.Append(") ;");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@HostName", SqlDbType.NVarChar, 255);
            arParams[1].Value = hostName;

            arParams[2] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Value = siteGuid;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected > 0;

        }

        public bool DeleteHost(int hostId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE HostID = @HostID  ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@HostID", SqlDbType.Int);
            arParams[0].Value = hostId;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected > 0;
        }

        public int CountOtherSites(int currentSiteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteID <> @CurrentSiteID   ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@CurrentSiteID", SqlDbType.Int);
            arParams[0].Value = currentSiteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public DbDataReader GetPageOfOtherSites(
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites  ");
            sqlCommand.Append("WHERE SiteID <> @CurrentSiteID   ");

            sqlCommand.Append("ORDER BY SiteName  ");


            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@CurrentSiteID", SqlDbType.Int);
            arParams[0].Value = currentSiteId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public int GetSiteIdByHostName(string hostName)
        {
            int siteId = -1;

            StringBuilder sqlCommand = new StringBuilder();

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@HostName", SqlDbType.NVarChar, 255);
            arParams[0].Value = hostName;

            sqlCommand.Append("SELECT SiteID ");
            sqlCommand.Append("FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE HostName = @HostName ;");

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
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
                sqlCommand.Append("SELECT TOP (1) SiteID ");
                sqlCommand.Append("FROM	mp_Sites ");
                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append(" ;");

                using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
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

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FolderName", SqlDbType.NVarChar, 255);
            arParams[0].Value = folderName;

            sqlCommand.Append("SELECT COALESCE(s.SiteID, -1) AS SiteID ");
            sqlCommand.Append("FROM mp_SiteFolders sf ");
            sqlCommand.Append("JOIN mp_Sites s ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("sf.SiteGuid = s.SiteGuid ");
            sqlCommand.Append("WHERE sf.FolderName = @FolderName ");
            sqlCommand.Append("ORDER BY s.SiteID ");
            sqlCommand.Append(";");

            using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    siteId = Convert.ToInt32(reader[0]);
                }
            }

            if (siteId == -1)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("SELECT TOP (1) SiteID ");
                sqlCommand.Append("FROM	mp_Sites ");
                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append(" ;");

                using (DbDataReader reader = AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null))
                {
                    if (reader.Read())
                    {
                        siteId = Convert.ToInt32(reader[0]);
                    }
                }

            }

            return siteId;
        }

    }
}
