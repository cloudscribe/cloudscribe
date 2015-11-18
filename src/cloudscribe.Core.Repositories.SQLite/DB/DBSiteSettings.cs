// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-11-18
// 

using cloudscribe.DbHelpers.SQLite;
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
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
            sqlCommand.Append("INSERT INTO mp_Sites ( ");
            sqlCommand.Append("SiteName, ");
            sqlCommand.Append("Skin, "); 
            sqlCommand.Append("AllowNewRegistration, ");
            sqlCommand.Append("UseSecureRegistration, ");
            sqlCommand.Append("UseSSLOnAllPages, ");
            sqlCommand.Append("IsServerAdminSite, ");
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
            sqlCommand.Append("RecaptchaPrivateKey, ");
            sqlCommand.Append("RecaptchaPublicKey, ");
            sqlCommand.Append("ApiKeyExtra1, ");
            sqlCommand.Append("ApiKeyExtra2, ");
            sqlCommand.Append("ApiKeyExtra3, ");
            sqlCommand.Append("ApiKeyExtra4, ");
            sqlCommand.Append("ApiKeyExtra5, ");
            sqlCommand.Append("DisableDbAuth, ");

            sqlCommand.Append("RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("MinRequiredPasswordLength, ");
            sqlCommand.Append("MinReqNonAlphaChars, ");
            sqlCommand.Append("DefaultEmailFromAddress, ");
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
            sqlCommand.Append("RequireApprovalBeforeLogin, ");

            sqlCommand.Append("SiteGuid ");


            sqlCommand.Append("  )");


            sqlCommand.Append("VALUES (");

            sqlCommand.Append(" :SiteName , ");
            sqlCommand.Append(" :Skin , ");
            sqlCommand.Append(" :AllowNewRegistration, ");
            sqlCommand.Append(" :UseSecureRegistration, ");
            sqlCommand.Append(" :UseSSLOnAllPages, ");
            sqlCommand.Append(" :IsServerAdminSite, ");
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
            sqlCommand.Append(" :RecaptchaPrivateKey, ");
            sqlCommand.Append(" :RecaptchaPublicKey, ");
            sqlCommand.Append(":ApiKeyExtra1, ");
            sqlCommand.Append(":ApiKeyExtra2, ");
            sqlCommand.Append(":ApiKeyExtra3, ");
            sqlCommand.Append(":ApiKeyExtra4, ");
            sqlCommand.Append(":ApiKeyExtra5, ");
            sqlCommand.Append(":DisableDbAuth, ");

            sqlCommand.Append(":RequiresQuestionAndAnswer, ");
            sqlCommand.Append(":MaxInvalidPasswordAttempts, ");
            sqlCommand.Append(":PasswordAttemptWindowMinutes, ");
            sqlCommand.Append(":MinRequiredPasswordLength, ");
            sqlCommand.Append(":MinReqNonAlphaChars, ");
            sqlCommand.Append(":DefaultEmailFromAddress, ");
            sqlCommand.Append(":AllowDbFallbackWithLdap, ");
            sqlCommand.Append(":EmailLdapDbFallback, ");
            sqlCommand.Append(":AllowPersistentLogin, ");
            sqlCommand.Append(":CaptchaOnLogin, ");
            sqlCommand.Append(":CaptchaOnRegistration, ");
            sqlCommand.Append(":SiteIsClosed, ");
            sqlCommand.Append(":SiteIsClosedMessage, ");
            sqlCommand.Append(":PrivacyPolicy, ");
            sqlCommand.Append(":TimeZoneId, ");
            sqlCommand.Append(":GoogleAnalyticsProfileId, ");
            sqlCommand.Append(":CompanyName, ");
            sqlCommand.Append(":CompanyStreetAddress, ");
            sqlCommand.Append(":CompanyStreetAddress2, ");
            sqlCommand.Append(":CompanyRegion, ");
            sqlCommand.Append(":CompanyLocality, ");
            sqlCommand.Append(":CompanyCountry, ");
            sqlCommand.Append(":CompanyPostalCode, ");
            sqlCommand.Append(":CompanyPublicEmail, ");
            sqlCommand.Append(":CompanyPhone, ");
            sqlCommand.Append(":CompanyFax, ");
            sqlCommand.Append(":FacebookAppId, ");
            sqlCommand.Append(":FacebookAppSecret, ");
            sqlCommand.Append(":GoogleClientId, ");
            sqlCommand.Append(":GoogleClientSecret, ");
            sqlCommand.Append(":TwitterConsumerKey, ");
            sqlCommand.Append(":TwitterConsumerSecret, ");
            sqlCommand.Append(":MicrosoftClientId, ");
            sqlCommand.Append(":MicrosoftClientSecret, ");
            sqlCommand.Append(":PreferredHostName, ");
            sqlCommand.Append(":SiteFolderName, ");
            sqlCommand.Append(":AddThisDotComUsername, ");
            sqlCommand.Append(":LoginInfoTop, ");
            sqlCommand.Append(":LoginInfoBottom, ");
            sqlCommand.Append(":RegistrationAgreement, ");
            sqlCommand.Append(":RegistrationPreamble, ");
            sqlCommand.Append(":SmtpServer, ");
            sqlCommand.Append(":SmtpPort, ");
            sqlCommand.Append(":SmtpUser, ");
            sqlCommand.Append(":SmtpPassword, ");
            sqlCommand.Append(":SmtpPreferredEncoding, ");
            sqlCommand.Append(":SmtpRequiresAuth, ");
            sqlCommand.Append(":SmtpUseSsl, ");
            sqlCommand.Append(":RequireApprovalBeforeLogin, ");

            sqlCommand.Append(" :SiteGuid ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[74];

            arParams[0] = new SqliteParameter(":SiteName", DbType.String);
            arParams[0].Value = siteName;

            arParams[1] = new SqliteParameter(":IsServerAdminSite", DbType.Int32);
            arParams[1].Value = isServerAdminSite ? 1 : 0;

            arParams[2] = new SqliteParameter(":Skin", DbType.String);
            arParams[2].Value = skin;
            
            arParams[3] = new SqliteParameter(":AllowNewRegistration", DbType.Int32);
            arParams[3].Value = allowNewRegistration ? 1 : 0;
            
            arParams[4] = new SqliteParameter(":UseSecureRegistration", DbType.Int32);
            arParams[4].Value = useSecureRegistration ? 1 : 0;
            
            arParams[5] = new SqliteParameter(":UseSSLOnAllPages", DbType.Int32);
            arParams[5].Value = useSslOnAllPages ? 1 : 0;
            
            arParams[6] = new SqliteParameter(":UseLdapAuth", DbType.Int32);
            arParams[6].Value = useLdapAuth ? 1 : 0;

            arParams[7] = new SqliteParameter(":AutoCreateLDAPUserOnFirstLogin", DbType.Int32);
            arParams[7].Value = autoCreateLdapUserOnFirstLogin ? 1 : 0;

            arParams[8] = new SqliteParameter(":LdapServer", DbType.String);
            arParams[8].Value = ldapServer;

            arParams[9] = new SqliteParameter(":LdapPort", DbType.Int32);
            arParams[9].Value = ldapPort;

            arParams[10] = new SqliteParameter(":LdapRootDN", DbType.String);
            arParams[10].Value = ldapRootDN;

            arParams[11] = new SqliteParameter(":LdapUserDNKey", DbType.String);
            arParams[11].Value = ldapUserDNKey;

            arParams[12] = new SqliteParameter(":AllowUserFullNameChange", DbType.Int32);
            arParams[12].Value = allowUserFullNameChange ? 1 : 0;

            arParams[13] = new SqliteParameter(":UseEmailForLogin", DbType.Int32);
            arParams[13].Value = useEmailForLogin ? 1 : 0;

            arParams[14] = new SqliteParameter(":ReallyDeleteUsers", DbType.Int32);
            arParams[14].Value = reallyDeleteUsers ? 1 : 0;
            
            arParams[15] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[15].Value = siteGuid.ToString();

            arParams[16] = new SqliteParameter(":LdapDomain", DbType.String);
            arParams[16].Value = ldapDomain;
            
            arParams[17] = new SqliteParameter(":RecaptchaPrivateKey", DbType.String);
            arParams[17].Value = recaptchaPrivateKey;

            arParams[18] = new SqliteParameter(":RecaptchaPublicKey", DbType.String);
            arParams[18].Value = recaptchaPublicKey;
            
            arParams[19] = new SqliteParameter(":ApiKeyExtra1", DbType.String);
            arParams[19].Value = apiKeyExtra1;

            arParams[20] = new SqliteParameter(":ApiKeyExtra2", DbType.String);
            arParams[20].Value = apiKeyExtra2;

            arParams[21] = new SqliteParameter(":ApiKeyExtra3", DbType.String);
            arParams[21].Value = apiKeyExtra3;

            arParams[22] = new SqliteParameter(":ApiKeyExtra4", DbType.String);
            arParams[22].Value = apiKeyExtra4;

            arParams[23] = new SqliteParameter(":ApiKeyExtra5", DbType.String);
            arParams[23].Value = apiKeyExtra5;

            arParams[24] = new SqliteParameter(":DisableDbAuth", DbType.Int32);
            arParams[24].Value = disableDbAuth ? 1 : 0;

            arParams[25] = new SqliteParameter(":RequiresQuestionAndAnswer", DbType.Int32);
            arParams[25].Value = requiresQuestionAndAnswer ? 1 : 0;

            arParams[26] = new SqliteParameter(":MaxInvalidPasswordAttempts", DbType.Int32);
            arParams[26].Value = maxInvalidPasswordAttempts;

            arParams[27] = new SqliteParameter(":PasswordAttemptWindowMinutes", DbType.Int32);
            arParams[27].Value = passwordAttemptWindowMinutes;

            arParams[28] = new SqliteParameter(":MinRequiredPasswordLength", DbType.Int32);
            arParams[28].Value = minRequiredPasswordLength;

            arParams[29] = new SqliteParameter(":MinReqNonAlphaChars", DbType.Int32);
            arParams[29].Value = minReqNonAlphaChars;

            arParams[30] = new SqliteParameter(":DefaultEmailFromAddress", DbType.String);
            arParams[30].Value = defaultEmailFromAddress;

            arParams[31] = new SqliteParameter(":AllowDbFallbackWithLdap", DbType.Int32);
            arParams[31].Value = allowDbFallbackWithLdap ? 1 : 0;

            arParams[32] = new SqliteParameter(":EmailLdapDbFallback", DbType.Int32);
            arParams[32].Value = emailLdapDbFallback ? 1 : 0;

            arParams[33] = new SqliteParameter(":AllowPersistentLogin", DbType.Int32);
            arParams[33].Value = allowPersistentLogin ? 1 : 0;

            arParams[34] = new SqliteParameter(":CaptchaOnLogin", DbType.Int32);
            arParams[34].Value = captchaOnLogin ? 1 : 0;

            arParams[35] = new SqliteParameter(":CaptchaOnRegistration", DbType.Int32);
            arParams[35].Value = captchaOnRegistration ? 1 : 0;

            arParams[36] = new SqliteParameter(":SiteIsClosed", DbType.Int32);
            arParams[36].Value = siteIsClosed ? 1 : 0;

            arParams[37] = new SqliteParameter(":SiteIsClosedMessage", DbType.Object);
            arParams[37].Value = siteIsClosedMessage;

            arParams[38] = new SqliteParameter(":PrivacyPolicy", DbType.Object);
            arParams[38].Value = privacyPolicy;

            arParams[39] = new SqliteParameter(":TimeZoneId", DbType.String);
            arParams[39].Value = timeZoneId;

            arParams[40] = new SqliteParameter(":GoogleAnalyticsProfileId", DbType.String);
            arParams[40].Value = googleAnalyticsProfileId;

            arParams[41] = new SqliteParameter(":CompanyName", DbType.String);
            arParams[41].Value = companyName;

            arParams[42] = new SqliteParameter(":CompanyStreetAddress", DbType.String);
            arParams[42].Value = companyStreetAddress;

            arParams[43] = new SqliteParameter(":CompanyStreetAddress2", DbType.String);
            arParams[43].Value = companyStreetAddress2;

            arParams[44] = new SqliteParameter(":CompanyRegion", DbType.String);
            arParams[44].Value = companyRegion;

            arParams[45] = new SqliteParameter(":CompanyLocality", DbType.String);
            arParams[45].Value = companyLocality;

            arParams[46] = new SqliteParameter(":CompanyCountry", DbType.String);
            arParams[46].Value = companyCountry;

            arParams[47] = new SqliteParameter(":CompanyPostalCode", DbType.String);
            arParams[47].Value = companyPostalCode;

            arParams[48] = new SqliteParameter(":CompanyPublicEmail", DbType.String);
            arParams[48].Value = companyPublicEmail;

            arParams[49] = new SqliteParameter(":CompanyPhone", DbType.String);
            arParams[49].Value = companyPhone;

            arParams[50] = new SqliteParameter(":CompanyFax", DbType.String);
            arParams[50].Value = companyFax;

            arParams[51] = new SqliteParameter(":FacebookAppId", DbType.String);
            arParams[51].Value = facebookAppId;

            arParams[52] = new SqliteParameter(":FacebookAppSecret", DbType.String);
            arParams[52].Value = facebookAppSecret;

            arParams[53] = new SqliteParameter(":GoogleClientId", DbType.String);
            arParams[53].Value = googleClientId;

            arParams[54] = new SqliteParameter(":GoogleClientSecret", DbType.String);
            arParams[54].Value = googleClientSecret;

            arParams[55] = new SqliteParameter(":TwitterConsumerKey", DbType.String);
            arParams[55].Value = twitterConsumerKey;

            arParams[56] = new SqliteParameter(":TwitterConsumerSecret", DbType.String);
            arParams[56].Value = twitterConsumerSecret;

            arParams[57] = new SqliteParameter(":MicrosoftClientId", DbType.String);
            arParams[57].Value = microsoftClientId;

            arParams[58] = new SqliteParameter(":MicrosoftClientSecret", DbType.String);
            arParams[58].Value = microsoftClientSecret;

            arParams[59] = new SqliteParameter(":PreferredHostName", DbType.String);
            arParams[59].Value = preferredHostName;

            arParams[60] = new SqliteParameter(":SiteFolderName", DbType.String);
            arParams[60].Value = siteFolderName;

            arParams[61] = new SqliteParameter(":AddThisDotComUsername", DbType.String);
            arParams[61].Value = addThisDotComUsername;

            arParams[62] = new SqliteParameter(":LoginInfoTop", DbType.Object);
            arParams[62].Value = loginInfoTop;

            arParams[63] = new SqliteParameter(":LoginInfoBottom", DbType.Object);
            arParams[63].Value = loginInfoBottom;

            arParams[64] = new SqliteParameter(":RegistrationAgreement", DbType.Object);
            arParams[64].Value = registrationAgreement;

            arParams[65] = new SqliteParameter(":RegistrationPreamble", DbType.Object);
            arParams[65].Value =registrationPreamble;

            arParams[66] = new SqliteParameter(":SmtpServer", DbType.String);
            arParams[66].Value = smtpServer;

            arParams[67] = new SqliteParameter(":SmtpPort", DbType.Int32);
            arParams[67].Value = smtpPort;

            arParams[68] = new SqliteParameter(":SmtpUser", DbType.String);
            arParams[68].Value = smtpUser;

            arParams[69] = new SqliteParameter(":SmtpPassword", DbType.String);
            arParams[69].Value = smtpPassword;

            arParams[70] = new SqliteParameter(":SmtpPreferredEncoding", DbType.String);
            arParams[70].Value = smtpPreferredEncoding;

            arParams[71] = new SqliteParameter(":SmtpRequiresAuth", DbType.Int32);
            arParams[71].Value = smtpRequiresAuth ? 1 : 0;

            arParams[72] = new SqliteParameter(":SmtpUseSsl", DbType.Int32);
            arParams[72].Value = smtpUseSsl ? 1 : 0;

            arParams[73] = new SqliteParameter(":RequireApprovalBeforeLogin", DbType.Int32);
            arParams[73].Value = requireApprovalBeforeLogin ? 1 : 0;



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
            sqlCommand.Append("SET SiteName = :SiteName, ");
            sqlCommand.Append("IsServerAdminSite = :IsServerAdminSite, ");
            sqlCommand.Append("Skin = :Skin, ");
            sqlCommand.Append("AllowNewRegistration = :AllowNewRegistration, ");
            sqlCommand.Append("UseSecureRegistration = :UseSecureRegistration, ");
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
            sqlCommand.Append("RecaptchaPrivateKey = :RecaptchaPrivateKey, ");
            sqlCommand.Append("RecaptchaPublicKey = :RecaptchaPublicKey, "); 
            sqlCommand.Append("DisableDbAuth = :DisableDbAuth, ");
            sqlCommand.Append("ApiKeyExtra1 = :ApiKeyExtra1, ");
            sqlCommand.Append("ApiKeyExtra2 = :ApiKeyExtra2, ");
            sqlCommand.Append("ApiKeyExtra3 = :ApiKeyExtra3, ");
            sqlCommand.Append("ApiKeyExtra4 = :ApiKeyExtra4, ");
            sqlCommand.Append("ApiKeyExtra5 = :ApiKeyExtra5, ");

            sqlCommand.Append("RequiresQuestionAndAnswer = :RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts = :MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("PasswordAttemptWindowMinutes = :PasswordAttemptWindowMinutes, ");
            sqlCommand.Append("MinRequiredPasswordLength = :MinRequiredPasswordLength, ");
            sqlCommand.Append("MinReqNonAlphaChars = :MinReqNonAlphaChars, ");
            sqlCommand.Append("DefaultEmailFromAddress = :DefaultEmailFromAddress, ");
            sqlCommand.Append("AllowDbFallbackWithLdap = :AllowDbFallbackWithLdap, ");
            sqlCommand.Append("EmailLdapDbFallback = :EmailLdapDbFallback, ");
            sqlCommand.Append("AllowPersistentLogin = :AllowPersistentLogin, ");
            sqlCommand.Append("CaptchaOnLogin = :CaptchaOnLogin, ");
            sqlCommand.Append("CaptchaOnRegistration = :CaptchaOnRegistration,");
            sqlCommand.Append("SiteIsClosed = :SiteIsClosed, ");
            sqlCommand.Append("SiteIsClosedMessage = :SiteIsClosedMessage, ");
            sqlCommand.Append("PrivacyPolicy = :PrivacyPolicy, ");
            sqlCommand.Append("TimeZoneId = :TimeZoneId, ");
            sqlCommand.Append("GoogleAnalyticsProfileId = :GoogleAnalyticsProfileId, ");
            sqlCommand.Append("CompanyName = :CompanyName, ");
            sqlCommand.Append("CompanyStreetAddress = :CompanyStreetAddress, ");
            sqlCommand.Append("CompanyStreetAddress2 = :CompanyStreetAddress2, ");
            sqlCommand.Append("CompanyRegion = :CompanyRegion, ");
            sqlCommand.Append("CompanyLocality = :CompanyLocality, ");
            sqlCommand.Append("CompanyCountry = :CompanyCountry, ");
            sqlCommand.Append("CompanyPostalCode = :CompanyPostalCode, ");
            sqlCommand.Append("CompanyPublicEmail = :CompanyPublicEmail, ");
            sqlCommand.Append("CompanyPhone = :CompanyPhone, ");
            sqlCommand.Append("CompanyFax = :CompanyFax, ");
            sqlCommand.Append("FacebookAppId = :FacebookAppId, ");
            sqlCommand.Append("FacebookAppSecret = :FacebookAppSecret, ");
            sqlCommand.Append("GoogleClientId = :GoogleClientId, ");
            sqlCommand.Append("GoogleClientSecret = :GoogleClientSecret, ");
            sqlCommand.Append("TwitterConsumerKey = :TwitterConsumerKey, ");
            sqlCommand.Append("TwitterConsumerSecret = :TwitterConsumerSecret, ");
            sqlCommand.Append("MicrosoftClientId = :MicrosoftClientId, ");
            sqlCommand.Append("MicrosoftClientSecret = :MicrosoftClientSecret, ");
            sqlCommand.Append("PreferredHostName = :PreferredHostName, ");
            sqlCommand.Append("SiteFolderName = :SiteFolderName, ");
            sqlCommand.Append("AddThisDotComUsername = :AddThisDotComUsername, ");
            sqlCommand.Append("LoginInfoTop = :LoginInfoTop, ");
            sqlCommand.Append("LoginInfoBottom = :LoginInfoBottom, ");
            sqlCommand.Append("RegistrationAgreement = :RegistrationAgreement, ");
            sqlCommand.Append("RegistrationPreamble = :RegistrationPreamble, ");
            sqlCommand.Append("SmtpServer = :SmtpServer, ");
            sqlCommand.Append("SmtpPort = :SmtpPort, ");
            sqlCommand.Append("SmtpUser = :SmtpUser, ");
            sqlCommand.Append("SmtpPassword = :SmtpPassword, ");
            sqlCommand.Append("SmtpPreferredEncoding = :SmtpPreferredEncoding, ");
            sqlCommand.Append("SmtpRequiresAuth = :SmtpRequiresAuth, ");
            sqlCommand.Append("SmtpUseSsl = :SmtpUseSsl, ");
            sqlCommand.Append("RequireApprovalBeforeLogin = :RequireApprovalBeforeLogin ");

            sqlCommand.Append(" WHERE SiteID = :SiteID ;");

            SqliteParameter[] arParams = new SqliteParameter[74];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":SiteName", DbType.String);
            arParams[1].Value = siteName;

            arParams[2] = new SqliteParameter(":IsServerAdminSite", DbType.Int32);
            arParams[2].Value = isServerAdminSite ? 1 : 0;

            arParams[3] = new SqliteParameter(":Skin", DbType.String);
            arParams[3].Value = skin;
            
            arParams[4] = new SqliteParameter(":AllowNewRegistration", DbType.Int32);
            arParams[4].Value = allowNewRegistration ? 1 : 0;
            
            arParams[5] = new SqliteParameter(":UseSecureRegistration", DbType.Int32);
            arParams[5].Value = useSecureRegistration ? 1 : 0;
            
            arParams[6] = new SqliteParameter(":UseSSLOnAllPages", DbType.Int32);
            arParams[6].Value = useSslOnAllPages ? 1 : 0;
            
            arParams[7] = new SqliteParameter(":UseLdapAuth", DbType.Int32);
            arParams[7].Value = useLdapAuth ? 1 : 0;

            arParams[8] = new SqliteParameter(":AutoCreateLDAPUserOnFirstLogin", DbType.Int32);
            arParams[8].Value = autoCreateLdapUserOnFirstLogin ? 1 : 0;

            arParams[9] = new SqliteParameter(":LdapServer", DbType.String);
            arParams[9].Value = ldapServer;

            arParams[10] = new SqliteParameter(":LdapPort", DbType.Int32);
            arParams[10].Value = ldapPort;

            arParams[11] = new SqliteParameter(":LdapRootDN", DbType.String);
            arParams[11].Value = ldapRootDN;

            arParams[12] = new SqliteParameter(":LdapUserDNKey", DbType.String);
            arParams[12].Value = ldapUserDNKey;

            arParams[13] = new SqliteParameter(":AllowUserFullNameChange", DbType.Int32);
            arParams[13].Value = allowUserFullNameChange ? 1 : 0;

            arParams[14] = new SqliteParameter(":UseEmailForLogin", DbType.Int32);
            arParams[14].Value = useEmailForLogin ? 1 : 0;

            arParams[15] = new SqliteParameter(":ReallyDeleteUsers", DbType.Int32);
            arParams[15].Value = reallyDeleteUsers ? 1 : 0;
            
            arParams[16] = new SqliteParameter(":LdapDomain", DbType.String);
            arParams[16].Value = ldapDomain;
            
            arParams[17] = new SqliteParameter(":RecaptchaPrivateKey", DbType.String);
            arParams[17].Value = recaptchaPrivateKey;

            arParams[18] = new SqliteParameter(":RecaptchaPublicKey", DbType.String);
            arParams[18].Value = recaptchaPublicKey;
            
            arParams[19] = new SqliteParameter(":ApiKeyExtra1", DbType.String);
            arParams[19].Value = apiKeyExtra1;

            arParams[20] = new SqliteParameter(":ApiKeyExtra2", DbType.String);
            arParams[20].Value = apiKeyExtra2;

            arParams[21] = new SqliteParameter(":ApiKeyExtra3", DbType.String);
            arParams[21].Value = apiKeyExtra3;

            arParams[22] = new SqliteParameter(":ApiKeyExtra4", DbType.String);
            arParams[22].Value = apiKeyExtra4;

            arParams[23] = new SqliteParameter(":ApiKeyExtra5", DbType.String);
            arParams[23].Value = apiKeyExtra5;

            arParams[24] = new SqliteParameter(":DisableDbAuth", DbType.Int32);
            arParams[24].Value = disableDbAuth ? 1 : 0;

            arParams[25] = new SqliteParameter(":RequiresQuestionAndAnswer", DbType.Int32);
            arParams[25].Value = requiresQuestionAndAnswer ? 1 : 0;

            arParams[26] = new SqliteParameter(":MaxInvalidPasswordAttempts", DbType.Int32);
            arParams[26].Value = maxInvalidPasswordAttempts;

            arParams[27] = new SqliteParameter(":PasswordAttemptWindowMinutes", DbType.Int32);
            arParams[27].Value = passwordAttemptWindowMinutes;

            arParams[28] = new SqliteParameter(":MinRequiredPasswordLength", DbType.Int32);
            arParams[28].Value = minRequiredPasswordLength;

            arParams[29] = new SqliteParameter(":MinReqNonAlphaChars", DbType.Int32);
            arParams[29].Value = minReqNonAlphaChars;

            arParams[30] = new SqliteParameter(":DefaultEmailFromAddress", DbType.String);
            arParams[30].Value = defaultEmailFromAddress;

            arParams[31] = new SqliteParameter(":AllowDbFallbackWithLdap", DbType.Int32);
            arParams[31].Value = allowDbFallbackWithLdap ? 1 : 0;

            arParams[32] = new SqliteParameter(":EmailLdapDbFallback", DbType.Int32);
            arParams[32].Value = emailLdapDbFallback ? 1 : 0;

            arParams[33] = new SqliteParameter(":AllowPersistentLogin", DbType.Int32);
            arParams[33].Value = allowPersistentLogin ? 1 : 0;

            arParams[34] = new SqliteParameter(":CaptchaOnLogin", DbType.Int32);
            arParams[34].Value = captchaOnLogin ? 1 : 0;

            arParams[35] = new SqliteParameter(":CaptchaOnRegistration", DbType.Int32);
            arParams[35].Value = captchaOnRegistration ? 1 : 0;

            arParams[36] = new SqliteParameter(":SiteIsClosed", DbType.Int32);
            arParams[36].Value = siteIsClosed ? 1 : 0;

            arParams[37] = new SqliteParameter(":SiteIsClosedMessage", DbType.Object);
            arParams[37].Value = siteIsClosedMessage;

            arParams[38] = new SqliteParameter(":PrivacyPolicy", DbType.Object);
            arParams[38].Value = privacyPolicy;

            arParams[39] = new SqliteParameter(":TimeZoneId", DbType.String);
            arParams[39].Value = timeZoneId;

            arParams[40] = new SqliteParameter(":GoogleAnalyticsProfileId", DbType.String);
            arParams[40].Value = googleAnalyticsProfileId;

            arParams[41] = new SqliteParameter(":CompanyName", DbType.String);
            arParams[41].Value = companyName;

            arParams[42] = new SqliteParameter(":CompanyStreetAddress", DbType.String);
            arParams[42].Value = companyStreetAddress;

            arParams[43] = new SqliteParameter(":CompanyStreetAddress2", DbType.String);
            arParams[43].Value = companyStreetAddress2;

            arParams[44] = new SqliteParameter(":CompanyRegion", DbType.String);
            arParams[44].Value = companyRegion;

            arParams[45] = new SqliteParameter(":CompanyLocality", DbType.String);
            arParams[45].Value = companyLocality;

            arParams[46] = new SqliteParameter(":CompanyCountry", DbType.String);
            arParams[46].Value = companyCountry;

            arParams[47] = new SqliteParameter(":CompanyPostalCode", DbType.String);
            arParams[47].Value = companyPostalCode;

            arParams[48] = new SqliteParameter(":CompanyPublicEmail", DbType.String);
            arParams[48].Value = companyPublicEmail;

            arParams[49] = new SqliteParameter(":CompanyPhone", DbType.String);
            arParams[49].Value = companyPhone;

            arParams[50] = new SqliteParameter(":CompanyFax", DbType.String);
            arParams[50].Value = companyFax;

            arParams[51] = new SqliteParameter(":FacebookAppId", DbType.String);
            arParams[51].Value = facebookAppId;

            arParams[52] = new SqliteParameter(":FacebookAppSecret", DbType.String);
            arParams[52].Value = facebookAppSecret;

            arParams[53] = new SqliteParameter(":GoogleClientId", DbType.String);
            arParams[53].Value = googleClientId;

            arParams[54] = new SqliteParameter(":GoogleClientSecret", DbType.String);
            arParams[54].Value = googleClientSecret;

            arParams[55] = new SqliteParameter(":TwitterConsumerKey", DbType.String);
            arParams[55].Value = twitterConsumerKey;

            arParams[56] = new SqliteParameter(":TwitterConsumerSecret", DbType.String);
            arParams[56].Value = twitterConsumerSecret;

            arParams[57] = new SqliteParameter(":MicrosoftClientId", DbType.String);
            arParams[57].Value = microsoftClientId;

            arParams[58] = new SqliteParameter(":MicrosoftClientSecret", DbType.String);
            arParams[58].Value = microsoftClientSecret;

            arParams[59] = new SqliteParameter(":PreferredHostName", DbType.String);
            arParams[59].Value = preferredHostName;

            arParams[60] = new SqliteParameter(":SiteFolderName", DbType.String);
            arParams[60].Value = siteFolderName;

            arParams[61] = new SqliteParameter(":AddThisDotComUsername", DbType.String);
            arParams[61].Value = addThisDotComUsername;

            arParams[62] = new SqliteParameter(":LoginInfoTop", DbType.Object);
            arParams[62].Value = loginInfoTop;

            arParams[63] = new SqliteParameter(":LoginInfoBottom", DbType.Object);
            arParams[63].Value = loginInfoBottom;

            arParams[64] = new SqliteParameter(":RegistrationAgreement", DbType.Object);
            arParams[64].Value = registrationAgreement;

            arParams[65] = new SqliteParameter(":RegistrationPreamble", DbType.Object);
            arParams[65].Value = registrationPreamble;

            arParams[66] = new SqliteParameter(":SmtpServer", DbType.String);
            arParams[66].Value = smtpServer;

            arParams[67] = new SqliteParameter(":SmtpPort", DbType.Int32);
            arParams[67].Value = smtpPort;

            arParams[68] = new SqliteParameter(":SmtpUser", DbType.String);
            arParams[68].Value = smtpUser;

            arParams[69] = new SqliteParameter(":SmtpPassword", DbType.String);
            arParams[69].Value = smtpPassword;

            arParams[70] = new SqliteParameter(":SmtpPreferredEncoding", DbType.String);
            arParams[70].Value = smtpPreferredEncoding;

            arParams[71] = new SqliteParameter(":SmtpRequiresAuth", DbType.Int32);
            arParams[71].Value = smtpRequiresAuth ? 1 : 0;

            arParams[72] = new SqliteParameter(":SmtpUseSsl", DbType.Int32);
            arParams[72].Value = smtpUseSsl ? 1 : 0;

            arParams[73] = new SqliteParameter(":RequireApprovalBeforeLogin", DbType.Int32);
            arParams[73].Value = requireApprovalBeforeLogin ? 1 : 0;

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
