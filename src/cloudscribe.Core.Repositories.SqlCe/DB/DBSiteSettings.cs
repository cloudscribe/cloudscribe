// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2010-03-10
// Last Modified:			2015-11-06
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
            string siteName,
            string skin,
            bool allowNewRegistration,
            bool useSecureRegistration,
            bool useSslOnAllPages,
            bool isServerAdminSite,
            bool useLdapAuth,
            bool autoCreateLdapUserOnFirstLogin,
            string ldapServer,
            int ldapPort,
            string ldapDomain,
            string ldapRootDN,
            string ldapUserDNKey,
            bool allowUserFullNameChange,
            bool useEmailForLogin,
            bool reallyDeleteUsers,
            string recaptchaPrivateKey,
            string recaptchaPublicKey,
            string apiKeyExtra1,
            string apiKeyExtra2,
            string apiKeyExtra3,
            string apiKeyExtra4,
            string apiKeyExtra5,
            bool disableDbAuth,

            bool requiresQuestionAndAnswer,
            int maxInvalidPasswordAttempts,
            int passwordAttemptWindowMinutes,
            int minRequiredPasswordLength,
            int minReqNonAlphaChars,
            string defaultEmailFromAddress,
            bool allowDbFallbackWithLdap,
            bool emailLdapDbFallback,
            bool allowPersistentLogin,
            bool captchaOnLogin,
            bool captchaOnRegistration,
            bool siteIsClosed,
            string siteIsClosedMessage,
            string privacyPolicy,
            string timeZoneId,
            string googleAnalyticsProfileId,
            string companyName,
            string companyStreetAddress,
            string companyStreetAddress2,
            string companyRegion,
            string companyLocality,
            string companyCountry,
            string companyPostalCode,
            string companyPublicEmail,
            string companyPhone,
            string companyFax,
            string facebookAppId,
            string facebookAppSecret,
            string googleClientId,
            string googleClientSecret,
            string twitterConsumerKey,
            string twitterConsumerSecret,
            string microsoftClientId,
            string microsoftClientSecret,
            string preferredHostName,
            string siteFolderName,
            string addThisDotComUsername,
            string loginInfoTop,
            string loginInfoBottom,
            string registrationAgreement,
            string registrationPreamble,
            string smtpServer,
            int smtpPort,
            string smtpUser,
            string smtpPassword,
            string smtpPreferredEncoding,
            bool smtpRequiresAuth,
            bool smtpUseSsl,
            bool requireApprovalBeforeLogin

            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Sites ");
            sqlCommand.Append("(");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("SiteName, ");
            sqlCommand.Append("Skin, ");
            sqlCommand.Append("AllowNewRegistration, ");
            sqlCommand.Append("UseSecureRegistration, ");
            sqlCommand.Append("UseSSLOnAllPages, ");
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
            sqlCommand.Append("RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("MinRequiredPasswordLength, ");
            sqlCommand.Append("MinReqNonAlphaChars, ");
            sqlCommand.Append("DefaultEmailFromAddress, ");
            sqlCommand.Append("RecaptchaPrivateKey, ");
            sqlCommand.Append("RecaptchaPublicKey, ");
            sqlCommand.Append("ApiKeyExtra1, ");
            sqlCommand.Append("ApiKeyExtra2, ");
            sqlCommand.Append("ApiKeyExtra3, ");
            sqlCommand.Append("ApiKeyExtra4, ");
            sqlCommand.Append("ApiKeyExtra5, ");
            sqlCommand.Append("DisableDbAuth, ");

            sqlCommand.Append("AllowDbFallbackWithLdap, ");
            sqlCommand.Append("EmailLdapDbFallback, ");
            sqlCommand.Append("AllowPersistentLogin, ");
            sqlCommand.Append("CaptchaOnLogin, ");
            sqlCommand.Append("CaptchaOnRegistration, ");
            sqlCommand.Append("SiteIsClosed, ");
            sqlCommand.Append("SiteIsClosedMessage, ");
            sqlCommand.Append("PrivacyPolicy, ");
            sqlCommand.Append("TimeZoneId, ");
            sqlCommand.Append("GoogleAnalyticsProfileId, ");
            sqlCommand.Append("CompanyName, ");
            sqlCommand.Append("CompanyStreetAddress, ");
            sqlCommand.Append("CompanyStreetAddress2, ");
            sqlCommand.Append("CompanyRegion, ");
            sqlCommand.Append("CompanyLocality, ");
            sqlCommand.Append("CompanyCountry, ");
            sqlCommand.Append("CompanyPostalCode, ");
            sqlCommand.Append("CompanyPublicEmail, ");
            sqlCommand.Append("CompanyPhone, ");
            sqlCommand.Append("CompanyFax, ");
            sqlCommand.Append("FacebookAppId, ");
            sqlCommand.Append("FacebookAppSecret, ");
            sqlCommand.Append("GoogleClientId, ");
            sqlCommand.Append("GoogleClientSecret, ");
            sqlCommand.Append("TwitterConsumerKey, ");
            sqlCommand.Append("TwitterConsumerSecret, ");
            sqlCommand.Append("MicrosoftClientId, ");
            sqlCommand.Append("MicrosoftClientSecret, ");
            sqlCommand.Append("PreferredHostName, ");
            sqlCommand.Append("SiteFolderName, ");
            sqlCommand.Append("AddThisDotComUsername, ");
            sqlCommand.Append("LoginInfoTop, ");
            sqlCommand.Append("LoginInfoBottom, ");
            sqlCommand.Append("RegistrationAgreement, ");
            sqlCommand.Append("RegistrationPreamble, ");
            sqlCommand.Append("SmtpServer, ");
            sqlCommand.Append("SmtpPort, ");
            sqlCommand.Append("SmtpUser, ");
            sqlCommand.Append("SmtpPassword, ");
            sqlCommand.Append("SmtpPreferredEncoding, ");
            sqlCommand.Append("SmtpRequiresAuth, ");
            sqlCommand.Append("SmtpUseSsl, ");
            sqlCommand.Append("RequireApprovalBeforeLogin ");



            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@SiteName, ");
            sqlCommand.Append("@Skin, ");
            sqlCommand.Append("@AllowNewRegistration, ");
            sqlCommand.Append("@UseSecureRegistration, ");
            sqlCommand.Append("@UseSSLOnAllPages, ");
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
            sqlCommand.Append("@RequiresQuestionAndAnswer, ");
            sqlCommand.Append("@MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("@PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("@MinRequiredPasswordLength, ");
            sqlCommand.Append("@MinReqNonAlphaChars, ");
            sqlCommand.Append("@DefaultEmailFromAddress, ");
            sqlCommand.Append("@RecaptchaPrivateKey, ");
            sqlCommand.Append("@RecaptchaPublicKey, ");
            sqlCommand.Append("@ApiKeyExtra1, ");
            sqlCommand.Append("@ApiKeyExtra2, ");
            sqlCommand.Append("@ApiKeyExtra3, ");
            sqlCommand.Append("@ApiKeyExtra4, ");
            sqlCommand.Append("@ApiKeyExtra5, ");
            sqlCommand.Append("@DisableDbAuth, ");

            sqlCommand.Append("@AllowDbFallbackWithLdap, ");
            sqlCommand.Append("@EmailLdapDbFallback, ");
            sqlCommand.Append("@AllowPersistentLogin, ");
            sqlCommand.Append("@CaptchaOnLogin, ");
            sqlCommand.Append("@CaptchaOnRegistration, ");
            sqlCommand.Append("@SiteIsClosed, ");
            sqlCommand.Append("@SiteIsClosedMessage, ");
            sqlCommand.Append("@PrivacyPolicy, ");
            sqlCommand.Append("@TimeZoneId, ");
            sqlCommand.Append("@GoogleAnalyticsProfileId, ");
            sqlCommand.Append("@CompanyName, ");
            sqlCommand.Append("@CompanyStreetAddress, ");
            sqlCommand.Append("@CompanyStreetAddress2, ");
            sqlCommand.Append("@CompanyRegion, ");
            sqlCommand.Append("@CompanyLocality, ");
            sqlCommand.Append("@CompanyCountry, ");
            sqlCommand.Append("@CompanyPostalCode, ");
            sqlCommand.Append("@CompanyPublicEmail, ");
            sqlCommand.Append("@CompanyPhone, ");
            sqlCommand.Append("@CompanyFax, ");
            sqlCommand.Append("@FacebookAppId, ");
            sqlCommand.Append("@FacebookAppSecret, ");
            sqlCommand.Append("@GoogleClientId, ");
            sqlCommand.Append("@GoogleClientSecret, ");
            sqlCommand.Append("@TwitterConsumerKey, ");
            sqlCommand.Append("@TwitterConsumerSecret, ");
            sqlCommand.Append("@MicrosoftClientId, ");
            sqlCommand.Append("@MicrosoftClientSecret, ");
            sqlCommand.Append("@PreferredHostName, ");
            sqlCommand.Append("@SiteFolderName, ");
            sqlCommand.Append("@AddThisDotComUsername, ");
            sqlCommand.Append("@LoginInfoTop, ");
            sqlCommand.Append("@LoginInfoBottom, ");
            sqlCommand.Append("@RegistrationAgreement, ");
            sqlCommand.Append("@RegistrationPreamble, ");
            sqlCommand.Append("@SmtpServer, ");
            sqlCommand.Append("@SmtpPort, ");
            sqlCommand.Append("@SmtpUser, ");
            sqlCommand.Append("@SmtpPassword, ");
            sqlCommand.Append("@SmtpPreferredEncoding, ");
            sqlCommand.Append("@SmtpRequiresAuth, ");
            sqlCommand.Append("@SmtpUseSsl, ");
            sqlCommand.Append("@RequireApprovalBeforeLogin ");

            
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[74];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = siteGuid;
            
            arParams[1] = new SqlCeParameter("@SiteName", SqlDbType.NVarChar);
            arParams[1].Value = siteName;

            arParams[2] = new SqlCeParameter("@Skin", SqlDbType.NVarChar);
            arParams[2].Value = skin;
            
            arParams[3] = new SqlCeParameter("@AllowNewRegistration", SqlDbType.Bit);
            arParams[3].Value = allowNewRegistration;

            arParams[4] = new SqlCeParameter("@UseSecureRegistration", SqlDbType.Bit);
            arParams[4].Value = useSecureRegistration;

            arParams[5] = new SqlCeParameter("@UseSSLOnAllPages", SqlDbType.Bit);
            arParams[5].Value = useSslOnAllPages;
            
            arParams[6] = new SqlCeParameter("@IsServerAdminSite", SqlDbType.Bit);
            arParams[6].Value = isServerAdminSite;

            arParams[7] = new SqlCeParameter("@UseLdapAuth", SqlDbType.Bit);
            arParams[7].Value = useLdapAuth;

            arParams[8] = new SqlCeParameter("@AutoCreateLdapUserOnFirstLogin", SqlDbType.Bit);
            arParams[8].Value = autoCreateLdapUserOnFirstLogin;

            arParams[9] = new SqlCeParameter("@LdapServer", SqlDbType.NVarChar);
            arParams[9].Value = ldapServer;

            arParams[10] = new SqlCeParameter("@LdapPort", SqlDbType.Int);
            arParams[10].Value = ldapPort;

            arParams[11] = new SqlCeParameter("@LdapDomain", SqlDbType.NVarChar);
            arParams[11].Value = ldapDomain;

            arParams[12] = new SqlCeParameter("@LdapRootDN", SqlDbType.NVarChar);
            arParams[12].Value = ldapRootDN;

            arParams[13] = new SqlCeParameter("@LdapUserDNKey", SqlDbType.NVarChar);
            arParams[13].Value = ldapUserDNKey;

            arParams[14] = new SqlCeParameter("@ReallyDeleteUsers", SqlDbType.Bit);
            arParams[14].Value = reallyDeleteUsers;

            arParams[15] = new SqlCeParameter("@UseEmailForLogin", SqlDbType.Bit);
            arParams[15].Value = useEmailForLogin;

            arParams[16] = new SqlCeParameter("@AllowUserFullNameChange", SqlDbType.Bit);
            arParams[16].Value = allowUserFullNameChange;
            
            arParams[17] = new SqlCeParameter("@RequiresQuestionAndAnswer", SqlDbType.Bit);
            arParams[17].Value = requiresQuestionAndAnswer;

            arParams[18] = new SqlCeParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int);
            arParams[18].Value = maxInvalidPasswordAttempts;

            arParams[19] = new SqlCeParameter("@PasswordAttemptWindowMinutes", SqlDbType.Int);
            arParams[19].Value = passwordAttemptWindowMinutes;
            
            arParams[20] = new SqlCeParameter("@MinRequiredPasswordLength", SqlDbType.Int);
            arParams[20].Value = minRequiredPasswordLength;

            arParams[21] = new SqlCeParameter("@MinReqNonAlphaChars", SqlDbType.Int);
            arParams[21].Value = minReqNonAlphaChars;
            
            arParams[22] = new SqlCeParameter("@DefaultEmailFromAddress", SqlDbType.NVarChar);
            arParams[22].Value = defaultEmailFromAddress;
            
            arParams[23] = new SqlCeParameter("@RecaptchaPrivateKey", SqlDbType.NVarChar);
            arParams[23].Value = recaptchaPrivateKey;

            arParams[24] = new SqlCeParameter("@RecaptchaPublicKey", SqlDbType.NVarChar);
            arParams[24].Value = recaptchaPublicKey;
            
            arParams[25] = new SqlCeParameter("@ApiKeyExtra1", SqlDbType.NVarChar);
            arParams[25].Value = apiKeyExtra1;

            arParams[26] = new SqlCeParameter("@ApiKeyExtra2", SqlDbType.NVarChar);
            arParams[26].Value = apiKeyExtra2;

            arParams[27] = new SqlCeParameter("@ApiKeyExtra3", SqlDbType.NVarChar);
            arParams[27].Value = apiKeyExtra3;

            arParams[28] = new SqlCeParameter("@ApiKeyExtra4", SqlDbType.NVarChar);
            arParams[28].Value = apiKeyExtra4;

            arParams[29] = new SqlCeParameter("@ApiKeyExtra5", SqlDbType.NVarChar);
            arParams[29].Value = apiKeyExtra5;

            arParams[30] = new SqlCeParameter("@DisableDbAuth", SqlDbType.Bit);
            arParams[30].Value = disableDbAuth;

            arParams[31] = new SqlCeParameter("@AllowDbFallbackWithLdap", SqlDbType.Bit);
            arParams[31].Value = allowDbFallbackWithLdap;

            arParams[32] = new SqlCeParameter("@EmailLdapDbFallback", SqlDbType.Bit);
            arParams[32].Value = emailLdapDbFallback;

            arParams[33] = new SqlCeParameter("@AllowPersistentLogin", SqlDbType.Bit);
            arParams[33].Value = allowPersistentLogin;

            arParams[34] = new SqlCeParameter("@CaptchaOnLogin", SqlDbType.Bit);
            arParams[34].Value = captchaOnLogin;

            arParams[35] = new SqlCeParameter("@CaptchaOnRegistration", SqlDbType.Bit);
            arParams[35].Value = captchaOnRegistration;

            arParams[36] = new SqlCeParameter("@SiteIsClosed", SqlDbType.Bit);
            arParams[36].Value = siteIsClosed;

            arParams[37] = new SqlCeParameter("@SiteIsClosedMessage", SqlDbType.NText);
            arParams[37].Value = siteIsClosedMessage;

            arParams[38] = new SqlCeParameter("@PrivacyPolicy", SqlDbType.NText);
            arParams[38].Value = privacyPolicy;

            arParams[39] = new SqlCeParameter("@TimeZoneId", SqlDbType.NVarChar, 50);
            arParams[39].Value = timeZoneId;

            arParams[40] = new SqlCeParameter("@GoogleAnalyticsProfileId", SqlDbType.NVarChar, 25);
            arParams[40].Value = googleAnalyticsProfileId;

            arParams[41] = new SqlCeParameter("@CompanyName", SqlDbType.NVarChar, 255);
            arParams[41].Value = companyName;

            arParams[42] = new SqlCeParameter("@CompanyStreetAddress", SqlDbType.NVarChar, 250);
            arParams[42].Value = companyStreetAddress;

            arParams[43] = new SqlCeParameter("@CompanyStreetAddress2", SqlDbType.NVarChar, 250);
            arParams[43].Value = companyStreetAddress2;

            arParams[44] = new SqlCeParameter("@CompanyRegion", SqlDbType.NVarChar, 200);
            arParams[44].Value = companyRegion;

            arParams[45] = new SqlCeParameter("@CompanyLocality", SqlDbType.NVarChar, 200);
            arParams[45].Value = companyLocality;

            arParams[46] = new SqlCeParameter("@CompanyCountry", SqlDbType.NVarChar, 10);
            arParams[46].Value = companyCountry;

            arParams[47] = new SqlCeParameter("@CompanyPostalCode", SqlDbType.NVarChar, 20);
            arParams[47].Value = companyPostalCode;

            arParams[48] = new SqlCeParameter("@CompanyPublicEmail", SqlDbType.NVarChar, 100);
            arParams[48].Value = companyPublicEmail;

            arParams[49] = new SqlCeParameter("@CompanyPhone", SqlDbType.NVarChar, 20);
            arParams[49].Value = companyPhone;

            arParams[50] = new SqlCeParameter("@CompanyFax", SqlDbType.NVarChar, 20);
            arParams[50].Value = companyFax;

            arParams[51] = new SqlCeParameter("@FacebookAppId", SqlDbType.NVarChar, 100);
            arParams[51].Value = facebookAppId;

            arParams[52] = new SqlCeParameter("@FacebookAppSecret", SqlDbType.NVarChar, 100);
            arParams[52].Value = facebookAppSecret;

            arParams[53] = new SqlCeParameter("@GoogleClientId", SqlDbType.NVarChar, 100);
            arParams[53].Value = googleClientId;

            arParams[54] = new SqlCeParameter("@GoogleClientSecret", SqlDbType.NVarChar, 100);
            arParams[54].Value = googleClientSecret;

            arParams[55] = new SqlCeParameter("@TwitterConsumerKey", SqlDbType.NVarChar, 100);
            arParams[55].Value = twitterConsumerKey;

            arParams[56] = new SqlCeParameter("@TwitterConsumerSecret", SqlDbType.NVarChar, 100);
            arParams[56].Value = twitterConsumerSecret;

            arParams[57] = new SqlCeParameter("@MicrosoftClientId", SqlDbType.NVarChar, 100);
            arParams[57].Value = microsoftClientId;

            arParams[58] = new SqlCeParameter("@MicrosoftClientSecret", SqlDbType.NVarChar, 100);
            arParams[58].Value = microsoftClientSecret;

            arParams[59] = new SqlCeParameter("@PreferredHostName", SqlDbType.NVarChar, 250);
            arParams[59].Value = preferredHostName;

            arParams[60] = new SqlCeParameter("@SiteFolderName", SqlDbType.NVarChar, 50);
            arParams[60].Value = siteFolderName;

            arParams[61] = new SqlCeParameter("@AddThisDotComUsername", SqlDbType.NVarChar, 50);
            arParams[61].Value = addThisDotComUsername;

            arParams[62] = new SqlCeParameter("@LoginInfoTop", SqlDbType.NText);
            arParams[62].Value = loginInfoTop;

            arParams[63] = new SqlCeParameter("@LoginInfoBottom", SqlDbType.NText);
            arParams[63].Value = loginInfoBottom;

            arParams[64] = new SqlCeParameter("@RegistrationAgreement", SqlDbType.NText);
            arParams[64].Value = registrationAgreement;

            arParams[65] = new SqlCeParameter("@RegistrationPreamble", SqlDbType.NText);
            arParams[65].Value = registrationPreamble;

            arParams[66] = new SqlCeParameter("@SmtpServer", SqlDbType.NVarChar, 200);
            arParams[66].Value = smtpServer;

            arParams[67] = new SqlCeParameter("@SmtpPort", SqlDbType.Int);
            arParams[67].Value = smtpPort;

            arParams[68] = new SqlCeParameter("@SmtpUser", SqlDbType.NVarChar, 500);
            arParams[68].Value = smtpUser;

            arParams[69] = new SqlCeParameter("@SmtpPassword", SqlDbType.NVarChar, 500);
            arParams[69].Value = smtpPassword;

            arParams[70] = new SqlCeParameter("@SmtpPreferredEncoding", SqlDbType.NVarChar, 20);
            arParams[70].Value = smtpPreferredEncoding;

            arParams[71] = new SqlCeParameter("@SmtpRequiresAuth", SqlDbType.Bit);
            arParams[71].Value = smtpRequiresAuth;

            arParams[72] = new SqlCeParameter("@SmtpUseSsl", SqlDbType.Bit);
            arParams[72].Value = smtpUseSsl;

            arParams[73] = new SqlCeParameter("@RequireApprovalBeforeLogin", SqlDbType.Bit);
            arParams[73].Value = requireApprovalBeforeLogin;



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
            bool allowNewRegistration,
            bool useSecureRegistration,
            bool useSslOnAllPages,
            bool isServerAdminSite,
            bool useLdapAuth,
            bool autoCreateLdapUserOnFirstLogin,
            string ldapServer,
            int ldapPort,
            string ldapDomain,
            string ldapRootDN,
            string ldapUserDNKey,
            bool allowUserFullNameChange,
            bool useEmailForLogin,
            bool reallyDeleteUsers,
            string recaptchaPrivateKey,
            string recaptchaPublicKey,
            string apiKeyExtra1,
            string apiKeyExtra2,
            string apiKeyExtra3,
            string apiKeyExtra4,
            string apiKeyExtra5,
            bool disableDbAuth,

            bool requiresQuestionAndAnswer,
            int maxInvalidPasswordAttempts,
            int passwordAttemptWindowMinutes,
            int minRequiredPasswordLength,
            int minReqNonAlphaChars,
            string defaultEmailFromAddress,
            bool allowDbFallbackWithLdap,
            bool emailLdapDbFallback,
            bool allowPersistentLogin,
            bool captchaOnLogin,
            bool captchaOnRegistration,
            bool siteIsClosed,
            string siteIsClosedMessage,
            string privacyPolicy,
            string timeZoneId,
            string googleAnalyticsProfileId,
            string companyName,
            string companyStreetAddress,
            string companyStreetAddress2,
            string companyRegion,
            string companyLocality,
            string companyCountry,
            string companyPostalCode,
            string companyPublicEmail,
            string companyPhone,
            string companyFax,
            string facebookAppId,
            string facebookAppSecret,
            string googleClientId,
            string googleClientSecret,
            string twitterConsumerKey,
            string twitterConsumerSecret,
            string microsoftClientId,
            string microsoftClientSecret,
            string preferredHostName,
            string siteFolderName,
            string addThisDotComUsername,
            string loginInfoTop,
            string loginInfoBottom,
            string registrationAgreement,
            string registrationPreamble,
            string smtpServer,
            int smtpPort,
            string smtpUser,
            string smtpPassword,
            string smtpPreferredEncoding,
            bool smtpRequiresAuth,
            bool smtpUseSsl,
            bool requireApprovalBeforeLogin
        
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("SiteName = @SiteName, ");
            sqlCommand.Append("Skin = @Skin, ");
            sqlCommand.Append("AllowNewRegistration = @AllowNewRegistration, ");
            sqlCommand.Append("UseSecureRegistration = @UseSecureRegistration, ");
            sqlCommand.Append("UseSSLOnAllPages = @UseSSLOnAllPages, ");
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
            sqlCommand.Append("RecaptchaPrivateKey = @RecaptchaPrivateKey, ");
            sqlCommand.Append("RecaptchaPublicKey = @RecaptchaPublicKey, ");
            sqlCommand.Append("ApiKeyExtra1 = @ApiKeyExtra1, ");
            sqlCommand.Append("ApiKeyExtra2 = @ApiKeyExtra2, ");
            sqlCommand.Append("ApiKeyExtra3 = @ApiKeyExtra3, ");
            sqlCommand.Append("ApiKeyExtra4 = @ApiKeyExtra4, ");
            sqlCommand.Append("ApiKeyExtra5 = @ApiKeyExtra5, ");
            sqlCommand.Append("DisableDbAuth = @DisableDbAuth, ");
            sqlCommand.Append("RequiresQuestionAndAnswer = @RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts = @MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("PasswordAttemptWindowMinutes = @PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("MinRequiredPasswordLength = @MinRequiredPasswordLength, ");
            sqlCommand.Append("MinReqNonAlphaChars = @MinReqNonAlphaChars, ");

            sqlCommand.Append("AllowDbFallbackWithLdap = @AllowDbFallbackWithLdap, ");
            sqlCommand.Append("EmailLdapDbFallback = @EmailLdapDbFallback, ");
            sqlCommand.Append("AllowPersistentLogin = @AllowPersistentLogin, ");
            sqlCommand.Append("CaptchaOnLogin = @CaptchaOnLogin, ");
            sqlCommand.Append("CaptchaOnRegistration = @CaptchaOnRegistration,");
            sqlCommand.Append("SiteIsClosed = @SiteIsClosed, ");
            sqlCommand.Append("SiteIsClosedMessage = @SiteIsClosedMessage, ");
            sqlCommand.Append("PrivacyPolicy = @PrivacyPolicy, ");
            sqlCommand.Append("TimeZoneId = @TimeZoneId, ");
            sqlCommand.Append("GoogleAnalyticsProfileId = @GoogleAnalyticsProfileId, ");
            sqlCommand.Append("CompanyName = @CompanyName, ");
            sqlCommand.Append("CompanyStreetAddress = @CompanyStreetAddress, ");
            sqlCommand.Append("CompanyStreetAddress2 = @CompanyStreetAddress2, ");
            sqlCommand.Append("CompanyRegion = @CompanyRegion, ");
            sqlCommand.Append("CompanyLocality = @CompanyLocality, ");
            sqlCommand.Append("CompanyCountry = @CompanyCountry, ");
            sqlCommand.Append("CompanyPostalCode = @CompanyPostalCode, ");
            sqlCommand.Append("CompanyPublicEmail = @CompanyPublicEmail, ");
            sqlCommand.Append("CompanyPhone = @CompanyPhone, ");
            sqlCommand.Append("CompanyFax = @CompanyFax, ");
            sqlCommand.Append("FacebookAppId = @FacebookAppId, ");
            sqlCommand.Append("FacebookAppSecret = @FacebookAppSecret, ");
            sqlCommand.Append("GoogleClientId = @GoogleClientId, ");
            sqlCommand.Append("GoogleClientSecret = @GoogleClientSecret, ");
            sqlCommand.Append("TwitterConsumerKey = @TwitterConsumerKey, ");
            sqlCommand.Append("TwitterConsumerSecret = @TwitterConsumerSecret, ");
            sqlCommand.Append("MicrosoftClientId = @MicrosoftClientId, ");
            sqlCommand.Append("MicrosoftClientSecret = @MicrosoftClientSecret, ");
            sqlCommand.Append("PreferredHostName = @PreferredHostName, ");
            sqlCommand.Append("SiteFolderName = @SiteFolderName, ");
            sqlCommand.Append("AddThisDotComUsername = @AddThisDotComUsername, ");
            sqlCommand.Append("LoginInfoTop = @LoginInfoTop, ");
            sqlCommand.Append("LoginInfoBottom = @LoginInfoBottom, ");
            sqlCommand.Append("RegistrationAgreement = @RegistrationAgreement, ");
            sqlCommand.Append("RegistrationPreamble = @RegistrationPreamble, ");
            sqlCommand.Append("SmtpServer = @SmtpServer, ");
            sqlCommand.Append("SmtpPort = @SmtpPort, ");
            sqlCommand.Append("SmtpUser = @SmtpUser, ");
            sqlCommand.Append("SmtpPassword = @SmtpPassword, ");
            sqlCommand.Append("SmtpPreferredEncoding = @SmtpPreferredEncoding, ");
            sqlCommand.Append("SmtpRequiresAuth = @SmtpRequiresAuth, ");
            sqlCommand.Append("SmtpUseSsl = @SmtpUseSsl, ");
            sqlCommand.Append("RequireApprovalBeforeLogin = @RequireApprovalBeforeLogin ");


            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[74];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@SiteName", SqlDbType.NVarChar);
            arParams[1].Value = siteName;

            arParams[2] = new SqlCeParameter("@Skin", SqlDbType.NVarChar);
            arParams[2].Value = skin;
            
            arParams[3] = new SqlCeParameter("@AllowNewRegistration", SqlDbType.Bit);
            arParams[3].Value = allowNewRegistration;

            arParams[4] = new SqlCeParameter("@UseSecureRegistration", SqlDbType.Bit);
            arParams[4].Value = useSecureRegistration;

            arParams[5] = new SqlCeParameter("@UseSSLOnAllPages", SqlDbType.Bit);
            arParams[5].Value = useSslOnAllPages;
            
            arParams[6] = new SqlCeParameter("@IsServerAdminSite", SqlDbType.Bit);
            arParams[6].Value = isServerAdminSite;

            arParams[7] = new SqlCeParameter("@UseLdapAuth", SqlDbType.Bit);
            arParams[7].Value = useLdapAuth;

            arParams[8] = new SqlCeParameter("@AutoCreateLdapUserOnFirstLogin", SqlDbType.Bit);
            arParams[8].Value = autoCreateLdapUserOnFirstLogin;

            arParams[9] = new SqlCeParameter("@LdapServer", SqlDbType.NVarChar);
            arParams[9].Value = ldapServer;

            arParams[10] = new SqlCeParameter("@LdapPort", SqlDbType.Int);
            arParams[10].Value = ldapPort;

            arParams[11] = new SqlCeParameter("@LdapDomain", SqlDbType.NVarChar);
            arParams[11].Value = ldapDomain;

            arParams[12] = new SqlCeParameter("@LdapRootDN", SqlDbType.NVarChar);
            arParams[12].Value = ldapRootDN;

            arParams[13] = new SqlCeParameter("@LdapUserDNKey", SqlDbType.NVarChar);
            arParams[13].Value = ldapUserDNKey;

            arParams[14] = new SqlCeParameter("@ReallyDeleteUsers", SqlDbType.Bit);
            arParams[14].Value = reallyDeleteUsers;

            arParams[15] = new SqlCeParameter("@UseEmailForLogin", SqlDbType.Bit);
            arParams[15].Value = useEmailForLogin;

            arParams[16] = new SqlCeParameter("@AllowUserFullNameChange", SqlDbType.Bit);
            arParams[16].Value = allowUserFullNameChange;
            
            arParams[17] = new SqlCeParameter("@RecaptchaPrivateKey", SqlDbType.NVarChar);
            arParams[17].Value = recaptchaPrivateKey;

            arParams[18] = new SqlCeParameter("@RecaptchaPublicKey", SqlDbType.NVarChar);
            arParams[18].Value = recaptchaPublicKey;
            
            arParams[19] = new SqlCeParameter("@ApiKeyExtra1", SqlDbType.NVarChar);
            arParams[19].Value = apiKeyExtra1;

            arParams[20] = new SqlCeParameter("@ApiKeyExtra2", SqlDbType.NVarChar);
            arParams[20].Value = apiKeyExtra2;

            arParams[21] = new SqlCeParameter("@ApiKeyExtra3", SqlDbType.NVarChar);
            arParams[21].Value = apiKeyExtra3;

            arParams[22] = new SqlCeParameter("@ApiKeyExtra4", SqlDbType.NVarChar);
            arParams[22].Value = apiKeyExtra4;

            arParams[23] = new SqlCeParameter("@ApiKeyExtra5", SqlDbType.NVarChar);
            arParams[23].Value = apiKeyExtra5;

            arParams[24] = new SqlCeParameter("@DisableDbAuth", SqlDbType.Bit);
            arParams[24].Value = disableDbAuth;

            arParams[25] = new SqlCeParameter("@AllowDbFallbackWithLdap", SqlDbType.Bit);
            arParams[25].Value = allowDbFallbackWithLdap;

            arParams[26] = new SqlCeParameter("@EmailLdapDbFallback", SqlDbType.Bit);
            arParams[26].Value = emailLdapDbFallback;

            arParams[27] = new SqlCeParameter("@AllowPersistentLogin", SqlDbType.Bit);
            arParams[27].Value = allowPersistentLogin;

            arParams[28] = new SqlCeParameter("@CaptchaOnLogin", SqlDbType.Bit);
            arParams[28].Value = captchaOnLogin;

            arParams[29] = new SqlCeParameter("@CaptchaOnRegistration", SqlDbType.Bit);
            arParams[29].Value = captchaOnRegistration;

            arParams[30] = new SqlCeParameter("@SiteIsClosed", SqlDbType.Bit);
            arParams[30].Value = siteIsClosed;

            arParams[31] = new SqlCeParameter("@SiteIsClosedMessage", SqlDbType.NText);
            arParams[31].Value = siteIsClosedMessage;

            arParams[32] = new SqlCeParameter("@PrivacyPolicy", SqlDbType.NText);
            arParams[32].Value = privacyPolicy;

            arParams[33] = new SqlCeParameter("@TimeZoneId", SqlDbType.NVarChar, 50);
            arParams[33].Value = timeZoneId;

            arParams[34] = new SqlCeParameter("@GoogleAnalyticsProfileId", SqlDbType.NVarChar, 25);
            arParams[34].Value = googleAnalyticsProfileId;
        
            arParams[35] = new SqlCeParameter("@CompanyName", SqlDbType.NVarChar, 255);
            arParams[35].Value = companyName;

            arParams[36] = new SqlCeParameter("@CompanyStreetAddress", SqlDbType.NVarChar, 250);
            arParams[36].Value = companyStreetAddress;

            arParams[37] = new SqlCeParameter("@CompanyStreetAddress2", SqlDbType.NVarChar, 250);
            arParams[37].Value = companyStreetAddress2;

            arParams[38] = new SqlCeParameter("@CompanyRegion", SqlDbType.NVarChar, 200);
            arParams[38].Value = companyRegion;

            arParams[39] = new SqlCeParameter("@CompanyLocality", SqlDbType.NVarChar, 200);
            arParams[39].Value = companyLocality;

            arParams[40] = new SqlCeParameter("@CompanyCountry", SqlDbType.NVarChar, 10);
            arParams[40].Value = companyCountry;

            arParams[41] = new SqlCeParameter("@CompanyPostalCode", SqlDbType.NVarChar, 20);
            arParams[41].Value = companyPostalCode;

            arParams[42] = new SqlCeParameter("@CompanyPublicEmail", SqlDbType.NVarChar, 100);
            arParams[42].Value = companyPublicEmail;

            arParams[43] = new SqlCeParameter("@CompanyPhone", SqlDbType.NVarChar, 20);
            arParams[43].Value = companyPhone;

            arParams[44] = new SqlCeParameter("@CompanyFax", SqlDbType.NVarChar, 20);
            arParams[44].Value = companyFax;

            arParams[45] = new SqlCeParameter("@FacebookAppId", SqlDbType.NVarChar, 100);
            arParams[45].Value = facebookAppId;

            arParams[46] = new SqlCeParameter("@FacebookAppSecret", SqlDbType.NVarChar, 100);
            arParams[46].Value = facebookAppSecret;

            arParams[47] = new SqlCeParameter("@GoogleClientId", SqlDbType.NVarChar, 100);
            arParams[47].Value = googleClientId;

            arParams[48] = new SqlCeParameter("@GoogleClientSecret", SqlDbType.NVarChar, 100);
            arParams[48].Value = googleClientSecret;

            arParams[49] = new SqlCeParameter("@TwitterConsumerKey", SqlDbType.NVarChar, 100);
            arParams[49].Value = twitterConsumerKey;

            arParams[50] = new SqlCeParameter("@TwitterConsumerSecret", SqlDbType.NVarChar, 100);
            arParams[50].Value = twitterConsumerSecret;

            arParams[51] = new SqlCeParameter("@MicrosoftClientId", SqlDbType.NVarChar, 100);
            arParams[51].Value = microsoftClientId;

            arParams[52] = new SqlCeParameter("@MicrosoftClientSecret", SqlDbType.NVarChar, 100);
            arParams[52].Value = microsoftClientSecret;

            arParams[53] = new SqlCeParameter("@PreferredHostName", SqlDbType.NVarChar, 250);
            arParams[53].Value = preferredHostName;

            arParams[54] = new SqlCeParameter("@SiteFolderName", SqlDbType.NVarChar, 50);
            arParams[54].Value = siteFolderName;

            arParams[55] = new SqlCeParameter("@AddThisDotComUsername", SqlDbType.NVarChar, 50);
            arParams[55].Value = addThisDotComUsername;

            arParams[56] = new SqlCeParameter("@LoginInfoTop", SqlDbType.NText);
            arParams[56].Value = loginInfoTop;

            arParams[57] = new SqlCeParameter("@LoginInfoBottom", SqlDbType.NText);
            arParams[57].Value = loginInfoBottom;

            arParams[58] = new SqlCeParameter("@RegistrationAgreement", SqlDbType.NText);
            arParams[58].Value = registrationAgreement;

            arParams[59] = new SqlCeParameter("@RegistrationPreamble", SqlDbType.NText);
            arParams[59].Value = registrationPreamble;

            arParams[60] = new SqlCeParameter("@SmtpServer", SqlDbType.NVarChar, 200);
            arParams[60].Value = smtpServer;

            arParams[61] = new SqlCeParameter("@SmtpPort", SqlDbType.Int);
            arParams[61].Value = smtpPort;

            arParams[62] = new SqlCeParameter("@SmtpUser", SqlDbType.NVarChar, 500);
            arParams[62].Value = smtpUser;

            arParams[63] = new SqlCeParameter("@SmtpPassword", SqlDbType.NVarChar, 500);
            arParams[63].Value = smtpPassword;

            arParams[64] = new SqlCeParameter("@SmtpPreferredEncoding", SqlDbType.NVarChar, 20);
            arParams[64].Value = smtpPreferredEncoding;

            arParams[65] = new SqlCeParameter("@SmtpRequiresAuth", SqlDbType.Bit);
            arParams[65].Value = smtpRequiresAuth;

            arParams[66] = new SqlCeParameter("@SmtpUseSsl", SqlDbType.Bit);
            arParams[66].Value = smtpUseSsl;

            arParams[67] = new SqlCeParameter("@RequireApprovalBeforeLogin", SqlDbType.Bit);
            arParams[67].Value = requireApprovalBeforeLogin;
            
            arParams[68] = new SqlCeParameter("@RequiresQuestionAndAnswer", SqlDbType.Bit);
            arParams[68].Value = requiresQuestionAndAnswer;

            arParams[69] = new SqlCeParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int);
            arParams[69].Value = maxInvalidPasswordAttempts;

            arParams[70] = new SqlCeParameter("@PasswordAttemptWindowMinutes", SqlDbType.Int);
            arParams[70].Value = passwordAttemptWindowMinutes;

            arParams[71] = new SqlCeParameter("@MinRequiredPasswordLength", SqlDbType.Int);
            arParams[71].Value = minRequiredPasswordLength;

            arParams[72] = new SqlCeParameter("@MinReqNonAlphaChars", SqlDbType.Int);
            arParams[72].Value = minReqNonAlphaChars;

            arParams[73] = new SqlCeParameter("@DefaultEmailFromAddress", SqlDbType.NVarChar);
            arParams[73].Value = defaultEmailFromAddress;

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
