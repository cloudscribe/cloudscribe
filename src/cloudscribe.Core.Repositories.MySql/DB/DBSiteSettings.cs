// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-27
// 
 
using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Threading;
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

            // possibly will change this later to have MySqlClientFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(MySqlClientFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private AdoHelper AdoHelper;

        public async Task<int> Create(
            Guid siteGuid,
            string siteName,
            string skin,
            bool allowNewRegistration,
            bool useSecureRegistration,
            bool isServerAdminSite,
            bool useLdapAuth,
            bool autoCreateLdapUserOnFirstLogin,
            string ldapServer,
            int ldapPort,
            string ldapDomain,
            string ldapRootDN,
            string ldapUserDNKey,
            bool useEmailForLogin,
            bool reallyDeleteUsers,
            string recaptchaPrivateKey,
            string recaptchaPublicKey,
            bool disableDbAuth,

            bool requiresQuestionAndAnswer,
            int maxInvalidPasswordAttempts,
            int minRequiredPasswordLength,
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
            bool requireApprovalBeforeLogin,
            bool isDataProtected,
            DateTime createdUtc,

            bool requireConfirmedPhone,
            string defaultEmailFromAlias,
            string accountApprovalEmailCsv,
            string dkimPublicKey,
            string dkimPrivateKey,
            string dkimDomain,
            string dkimSelector,
            bool signEmailWithDkim,
            string oidConnectAppId,
            string oidConnectAppSecret,
            string smsClientId,
            string smsSecureToken,
            string smsFrom,

            CancellationToken cancellationToken
            )
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Sites ( ");
            sqlCommand.Append("SiteName, ");
            sqlCommand.Append("Skin, ");
            sqlCommand.Append("AllowNewRegistration, ");
            sqlCommand.Append("UseSecureRegistration, ");
            sqlCommand.Append("IsServerAdminSite, ");
            sqlCommand.Append("UseLdapAuth, ");
            sqlCommand.Append("AutoCreateLDAPUserOnFirstLogin, ");
            sqlCommand.Append("LdapServer, ");
            sqlCommand.Append("LdapPort, ");
            sqlCommand.Append("LdapDomain, ");
            sqlCommand.Append("LdapRootDN, ");
            sqlCommand.Append("LdapUserDNKey, ");
            sqlCommand.Append("UseEmailForLogin, ");
            sqlCommand.Append("ReallyDeleteUsers, ");
            sqlCommand.Append("RecaptchaPrivateKey, ");
            sqlCommand.Append("RecaptchaPublicKey, ");
            sqlCommand.Append("DisableDbAuth, ");

            sqlCommand.Append("RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("MinRequiredPasswordLength, ");
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
            sqlCommand.Append("IsDataProtected, ");
            sqlCommand.Append("CreatedUtc, ");

            sqlCommand.Append("RequireConfirmedPhone, ");
            sqlCommand.Append("DefaultEmailFromAlias, ");
            sqlCommand.Append("AccountApprovalEmailCsv, ");
            sqlCommand.Append("DkimPublicKey, ");
            sqlCommand.Append("DkimPrivateKey, ");
            sqlCommand.Append("DkimDomain, ");
            sqlCommand.Append("DkimSelector, ");
            sqlCommand.Append("SignEmailWithDkim, ");
            sqlCommand.Append("OidConnectAppId, ");
            sqlCommand.Append("OidConnectAppSecret, ");
            sqlCommand.Append("SmsClientId, ");
            sqlCommand.Append("SmsSecureToken, ");
            sqlCommand.Append("SmsFrom, ");

            sqlCommand.Append("SiteGuid ");
            sqlCommand.Append("  )");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(" ?SiteName , ");
            sqlCommand.Append(" ?Skin , ");
            sqlCommand.Append(" ?AllowNewRegistration, ");
            sqlCommand.Append(" ?UseSecureRegistration, ");  
            sqlCommand.Append(" ?IsServerAdminSite, ");    
            sqlCommand.Append(" ?UseLdapAuth, ");
            sqlCommand.Append(" ?AutoCreateLDAPUserOnFirstLogin, ");
            sqlCommand.Append(" ?LdapServer, ");
            sqlCommand.Append(" ?LdapPort, ");
            sqlCommand.Append(" ?LdapDomain, ");
            sqlCommand.Append(" ?LdapRootDN, ");
            sqlCommand.Append(" ?LdapUserDNKey, ");
            sqlCommand.Append(" ?UseEmailForLogin, ");
            sqlCommand.Append(" ?ReallyDeleteUsers, ");      
            sqlCommand.Append(" ?RecaptchaPrivateKey, ");
            sqlCommand.Append(" ?RecaptchaPublicKey, ");   
            sqlCommand.Append("?DisableDbAuth, ");

            sqlCommand.Append("?RequiresQuestionAndAnswer, ");
            sqlCommand.Append("?MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("?MinRequiredPasswordLength, ");
            sqlCommand.Append("?DefaultEmailFromAddress, ");
            sqlCommand.Append("?AllowDbFallbackWithLdap, ");
            sqlCommand.Append("?EmailLdapDbFallback, ");
            sqlCommand.Append("?AllowPersistentLogin, ");
            sqlCommand.Append("?CaptchaOnLogin, ");
            sqlCommand.Append("?CaptchaOnRegistration, ");
            sqlCommand.Append("?SiteIsClosed, ");
            sqlCommand.Append("?SiteIsClosedMessage, ");
            sqlCommand.Append("?PrivacyPolicy, ");
            sqlCommand.Append("?TimeZoneId, ");
            sqlCommand.Append("?GoogleAnalyticsProfileId, ");
            sqlCommand.Append("?CompanyName, ");
            sqlCommand.Append("?CompanyStreetAddress, ");
            sqlCommand.Append("?CompanyStreetAddress2, ");
            sqlCommand.Append("?CompanyRegion, ");
            sqlCommand.Append("?CompanyLocality, ");
            sqlCommand.Append("?CompanyCountry, ");
            sqlCommand.Append("?CompanyPostalCode, ");
            sqlCommand.Append("?CompanyPublicEmail, ");
            sqlCommand.Append("?CompanyPhone, ");
            sqlCommand.Append("?CompanyFax, ");
            sqlCommand.Append("?FacebookAppId, ");
            sqlCommand.Append("?FacebookAppSecret, ");
            sqlCommand.Append("?GoogleClientId, ");
            sqlCommand.Append("?GoogleClientSecret, ");
            sqlCommand.Append("?TwitterConsumerKey, ");
            sqlCommand.Append("?TwitterConsumerSecret, ");
            sqlCommand.Append("?MicrosoftClientId, ");
            sqlCommand.Append("?MicrosoftClientSecret, ");
            sqlCommand.Append("?PreferredHostName, ");
            sqlCommand.Append("?SiteFolderName, ");
            sqlCommand.Append("?AddThisDotComUsername, ");
            sqlCommand.Append("?LoginInfoTop, ");
            sqlCommand.Append("?LoginInfoBottom, ");
            sqlCommand.Append("?RegistrationAgreement, ");
            sqlCommand.Append("?RegistrationPreamble, ");
            sqlCommand.Append("?SmtpServer, ");
            sqlCommand.Append("?SmtpPort, ");
            sqlCommand.Append("?SmtpUser, ");
            sqlCommand.Append("?SmtpPassword, ");
            sqlCommand.Append("?SmtpPreferredEncoding, ");
            sqlCommand.Append("?SmtpRequiresAuth, ");
            sqlCommand.Append("?SmtpUseSsl, ");
            sqlCommand.Append("?RequireApprovalBeforeLogin, ");
            sqlCommand.Append("?IsDataProtected, ");
            sqlCommand.Append("?CreatedUtc, ");

            sqlCommand.Append("?RequireConfirmedPhone, ");
            sqlCommand.Append("?DefaultEmailFromAlias, ");
            sqlCommand.Append("?AccountApprovalEmailCsv, ");
            sqlCommand.Append("?DkimPublicKey, ");
            sqlCommand.Append("?DkimPrivateKey, ");
            sqlCommand.Append("?DkimDomain, ");
            sqlCommand.Append("?DkimSelector, ");
            sqlCommand.Append("?SignEmailWithDkim, ");
            sqlCommand.Append("?OidConnectAppId, ");
            sqlCommand.Append("?OidConnectAppSecret, ");
            sqlCommand.Append("?SmsClientId, ");
            sqlCommand.Append("?SmsSecureToken, ");
            sqlCommand.Append("?SmsFrom, ");

            sqlCommand.Append(" ?SiteGuid ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[80];

            arParams[0] = new MySqlParameter("?SiteName", MySqlDbType.VarChar, 255);
            arParams[0].Value = siteName;

            arParams[1] = new MySqlParameter("?IsServerAdminSite", MySqlDbType.Int32);
            arParams[1].Value = isServerAdminSite ? 1 : 0;

            arParams[2] = new MySqlParameter("?Skin", MySqlDbType.VarChar, 100);
            arParams[2].Value = skin;
            
            arParams[3] = new MySqlParameter("?AllowNewRegistration", MySqlDbType.Int32);
            arParams[3].Value = allowNewRegistration ? 1 : 0;
            
            arParams[4] = new MySqlParameter("?UseSecureRegistration", MySqlDbType.Int32);
            arParams[4].Value = useSecureRegistration ? 1 : 0;
            
            arParams[5] = new MySqlParameter("?AutoCreateLDAPUserOnFirstLogin", MySqlDbType.Int32);
            arParams[5].Value = autoCreateLdapUserOnFirstLogin ? 1 : 0;

            arParams[6] = new MySqlParameter("?LdapServer", MySqlDbType.VarChar, 255);
            arParams[6].Value = ldapServer;

            arParams[7] = new MySqlParameter("?LdapPort", MySqlDbType.Int32);
            arParams[7].Value = ldapPort;

            arParams[8] = new MySqlParameter("?LdapRootDN", MySqlDbType.VarChar, 255);
            arParams[8].Value = ldapRootDN;

            arParams[9] = new MySqlParameter("?LdapUserDNKey", MySqlDbType.VarChar, 255);
            arParams[9].Value = ldapUserDNKey;
            
            arParams[10] = new MySqlParameter("?UseEmailForLogin", MySqlDbType.Int32);
            arParams[10].Value = useEmailForLogin ? 1 : 0;

            arParams[11] = new MySqlParameter("?ReallyDeleteUsers", MySqlDbType.Int32);
            arParams[11].Value = reallyDeleteUsers ? 0 : 1;
            
            arParams[12] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[12].Value = siteGuid.ToString();

            arParams[13] = new MySqlParameter("?LdapDomain", MySqlDbType.VarChar, 255);
            arParams[13].Value = ldapDomain;
            
            arParams[14] = new MySqlParameter("?RecaptchaPrivateKey", MySqlDbType.VarChar, 255);
            arParams[14].Value = recaptchaPrivateKey;

            arParams[15] = new MySqlParameter("?RecaptchaPublicKey", MySqlDbType.VarChar, 255);
            arParams[15].Value = recaptchaPublicKey;
            
            arParams[16] = new MySqlParameter("?DisableDbAuth", MySqlDbType.Int32);
            arParams[16].Value = disableDbAuth ? 1 : 0;

            arParams[17] = new MySqlParameter("?RequiresQuestionAndAnswer", MySqlDbType.Int16);
            arParams[17].Value = requiresQuestionAndAnswer ? 1 : 0;

            arParams[18] = new MySqlParameter("?MaxInvalidPasswordAttempts", MySqlDbType.Int32);
            arParams[18].Value = maxInvalidPasswordAttempts;
            
            arParams[19] = new MySqlParameter("?MinRequiredPasswordLength", MySqlDbType.Int32);
            arParams[19].Value = minRequiredPasswordLength;
            
            arParams[20] = new MySqlParameter("?DefaultEmailFromAddress", MySqlDbType.VarChar, 100);
            arParams[20].Value = defaultEmailFromAddress;

            arParams[21] = new MySqlParameter("?AllowDbFallbackWithLdap", MySqlDbType.Int16);
            arParams[21].Value = allowDbFallbackWithLdap ? 1 : 0;

            arParams[22] = new MySqlParameter("?EmailLdapDbFallback", MySqlDbType.Int16);
            arParams[22].Value = emailLdapDbFallback ? 1 : 0;

            arParams[23] = new MySqlParameter("?AllowPersistentLogin", MySqlDbType.Int16);
            arParams[23].Value = allowPersistentLogin ? 1 : 0;

            arParams[24] = new MySqlParameter("?CaptchaOnLogin", MySqlDbType.Int16);
            arParams[24].Value = captchaOnLogin ? 1 : 0;

            arParams[25] = new MySqlParameter("?CaptchaOnRegistration", MySqlDbType.Int16);
            arParams[25].Value = captchaOnRegistration ? 1 : 0;

            arParams[26] = new MySqlParameter("?SiteIsClosed", MySqlDbType.Int16);
            arParams[26].Value = siteIsClosed ? 1 : 0;

            arParams[27] = new MySqlParameter("?SiteIsClosedMessage", MySqlDbType.Text);
            arParams[27].Value = siteIsClosedMessage;

            arParams[28] = new MySqlParameter("?PrivacyPolicy", MySqlDbType.Text);
            arParams[28].Value = privacyPolicy;

            arParams[29] = new MySqlParameter("?TimeZoneId", MySqlDbType.VarChar, 50);
            arParams[29].Value = timeZoneId;

            arParams[30] = new MySqlParameter("?GoogleAnalyticsProfileId", MySqlDbType.VarChar, 50);
            arParams[30].Value = googleAnalyticsProfileId;

            arParams[31] = new MySqlParameter("?CompanyName", MySqlDbType.VarChar, 255);
            arParams[31].Value = companyName;

            arParams[32] = new MySqlParameter("?CompanyStreetAddress", MySqlDbType.VarChar, 250);
            arParams[32].Value = companyStreetAddress;

            arParams[33] = new MySqlParameter("?CompanyStreetAddress2", MySqlDbType.VarChar, 250);
            arParams[33].Value = companyStreetAddress2;

            arParams[34] = new MySqlParameter("?CompanyRegion", MySqlDbType.VarChar, 200);
            arParams[34].Value = companyRegion;

            arParams[35] = new MySqlParameter("?CompanyLocality", MySqlDbType.VarChar, 200);
            arParams[35].Value = companyLocality;

            arParams[36] = new MySqlParameter("?CompanyCountry", MySqlDbType.VarChar, 10);
            arParams[36].Value = companyCountry;

            arParams[37] = new MySqlParameter("?CompanyPostalCode", MySqlDbType.VarChar, 20);
            arParams[37].Value = companyPostalCode;

            arParams[38] = new MySqlParameter("?CompanyPublicEmail", MySqlDbType.VarChar, 100);
            arParams[38].Value = companyPublicEmail;

            arParams[39] = new MySqlParameter("?CompanyPhone", MySqlDbType.VarChar, 20);
            arParams[39].Value = companyPhone;

            arParams[40] = new MySqlParameter("?CompanyFax", MySqlDbType.VarChar, 20);
            arParams[40].Value = companyFax;

            arParams[41] = new MySqlParameter("?FacebookAppId", MySqlDbType.VarChar, 100);
            arParams[41].Value = facebookAppId;

            arParams[42] = new MySqlParameter("?FacebookAppSecret", MySqlDbType.Text);
            arParams[42].Value = facebookAppSecret;

            arParams[43] = new MySqlParameter("?GoogleClientId", MySqlDbType.VarChar, 100);
            arParams[43].Value = googleClientId;

            arParams[44] = new MySqlParameter("?GoogleClientSecret", MySqlDbType.Text);
            arParams[44].Value = googleClientSecret;

            arParams[45] = new MySqlParameter("?TwitterConsumerKey", MySqlDbType.VarChar, 100);
            arParams[45].Value = twitterConsumerKey;

            arParams[46] = new MySqlParameter("?TwitterConsumerSecret", MySqlDbType.Text);
            arParams[46].Value = twitterConsumerSecret;

            arParams[47] = new MySqlParameter("?MicrosoftClientId", MySqlDbType.VarChar, 100);
            arParams[47].Value = microsoftClientId;

            arParams[48] = new MySqlParameter("?MicrosoftClientSecret", MySqlDbType.Text);
            arParams[48].Value = microsoftClientSecret;

            arParams[49] = new MySqlParameter("?PreferredHostName", MySqlDbType.VarChar, 250);
            arParams[49].Value = preferredHostName;

            arParams[50] = new MySqlParameter("?SiteFolderName", MySqlDbType.VarChar, 50);
            arParams[50].Value = siteFolderName;

            arParams[51] = new MySqlParameter("?AddThisDotComUsername", MySqlDbType.VarChar, 50);
            arParams[51].Value = addThisDotComUsername;

            arParams[52] = new MySqlParameter("?LoginInfoTop", MySqlDbType.Text);
            arParams[52].Value = loginInfoTop;

            arParams[53] = new MySqlParameter("?LoginInfoBottom", MySqlDbType.Text);
            arParams[53].Value = loginInfoBottom;

            arParams[54] = new MySqlParameter("?RegistrationAgreement", MySqlDbType.Text);
            arParams[54].Value = registrationAgreement;

            arParams[55] = new MySqlParameter("?RegistrationPreamble", MySqlDbType.Text);
            arParams[55].Value = registrationPreamble;

            arParams[56] = new MySqlParameter("?SmtpServer", MySqlDbType.VarChar, 200);
            arParams[56].Value = smtpServer;

            arParams[57] = new MySqlParameter("?SmtpPort", MySqlDbType.Int32);
            arParams[57].Value = smtpPort;

            arParams[58] = new MySqlParameter("?SmtpUser", MySqlDbType.VarChar, 500);
            arParams[58].Value = smtpUser;

            arParams[59] = new MySqlParameter("?SmtpPassword", MySqlDbType.Text);
            arParams[59].Value = smtpPassword;

            arParams[60] = new MySqlParameter("?SmtpPreferredEncoding", MySqlDbType.VarChar, 20);
            arParams[60].Value = smtpPreferredEncoding;

            arParams[61] = new MySqlParameter("?SmtpRequiresAuth", MySqlDbType.Int16);
            arParams[61].Value = smtpRequiresAuth ? 1 : 0;

            arParams[62] = new MySqlParameter("?SmtpUseSsl", MySqlDbType.Int16);
            arParams[62].Value = smtpUseSsl ? 1 : 0;

            arParams[63] = new MySqlParameter("?RequireApprovalBeforeLogin", MySqlDbType.Int16);
            arParams[63].Value = requireApprovalBeforeLogin ? 1 : 0;

            arParams[64] = new MySqlParameter("?IsDataProtected", MySqlDbType.Int16);
            arParams[64].Value = isDataProtected ? 1 : 0;

            arParams[65] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[65].Value = createdUtc;

            arParams[66] = new MySqlParameter("?RequireConfirmedPhone", MySqlDbType.Int16);
            arParams[66].Value = requireConfirmedPhone ? 1 : 0;

            arParams[67] = new MySqlParameter("?DefaultEmailFromAlias", MySqlDbType.VarChar, 100);
            arParams[67].Value = defaultEmailFromAddress;

            arParams[68] = new MySqlParameter("?AccountApprovalEmailCsv", MySqlDbType.Text);
            arParams[68].Value = accountApprovalEmailCsv;

            arParams[69] = new MySqlParameter("?DkimPublicKey", MySqlDbType.Text);
            arParams[69].Value = dkimPublicKey;

            arParams[70] = new MySqlParameter("?DkimPrivateKey", MySqlDbType.Text);
            arParams[70].Value = dkimPrivateKey;

            arParams[71] = new MySqlParameter("?DkimDomain", MySqlDbType.VarChar, 255);
            arParams[71].Value = dkimDomain;

            arParams[72] = new MySqlParameter("?DkimSelector", MySqlDbType.VarChar, 128);
            arParams[72].Value = dkimSelector;

            arParams[73] = new MySqlParameter("?SignEmailWithDkim", MySqlDbType.Int16);
            arParams[73].Value = signEmailWithDkim ? 1 : 0;

            arParams[74] = new MySqlParameter("?OidConnectAppId", MySqlDbType.VarChar, 255);
            arParams[74].Value = oidConnectAppId;

            arParams[75] = new MySqlParameter("?OidConnectAppSecret", MySqlDbType.Text);
            arParams[75].Value = oidConnectAppSecret;

            arParams[76] = new MySqlParameter("?SmsClientId", MySqlDbType.VarChar, 255);
            arParams[76].Value = smsClientId;

            arParams[77] = new MySqlParameter("?SmsSecureToken", MySqlDbType.Text);
            arParams[77].Value = smsSecureToken;

            arParams[78] = new MySqlParameter("?SmsFrom", MySqlDbType.VarChar, 255);
            arParams[78].Value = smsFrom;

            arParams[79] = new MySqlParameter("?UseLdapAuth", MySqlDbType.Int32);
            arParams[79].Value = useLdapAuth ? 1 : 0;


            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int newID = Convert.ToInt32(result);

            return newID;

        }

        public async Task<bool> Update(
            int siteId,
            string siteName,
            string skin,
            bool allowNewRegistration,
            bool useSecureRegistration,
            bool isServerAdminSite,
            bool useLdapAuth,
            bool autoCreateLdapUserOnFirstLogin,
            string ldapServer,
            int ldapPort,
            string ldapDomain,
            string ldapRootDN,
            string ldapUserDNKey,
            bool useEmailForLogin,
            bool reallyDeleteUsers,
            string recaptchaPrivateKey,
            string recaptchaPublicKey,
            bool disableDbAuth,

            bool requiresQuestionAndAnswer,
            int maxInvalidPasswordAttempts,
            int minRequiredPasswordLength,
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
            bool requireApprovalBeforeLogin,
            bool isDataProtected,

            bool requireConfirmedPhone,
            string defaultEmailFromAlias,
            string accountApprovalEmailCsv,
            string dkimPublicKey,
            string dkimPrivateKey,
            string dkimDomain,
            string dkimSelector,
            bool signEmailWithDkim,
            string oidConnectAppId,
            string oidConnectAppSecret,
            string smsClientId,
            string smsSecureToken,
            string smsFrom,

            CancellationToken cancellationToken
            )
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET SiteName = ?SiteName, ");
            sqlCommand.Append("IsServerAdminSite = ?IsServerAdminSite, ");
            sqlCommand.Append("Skin = ?Skin, ");
            sqlCommand.Append("AllowNewRegistration = ?AllowNewRegistration, ");
            sqlCommand.Append("UseSecureRegistration = ?UseSecureRegistration, ");
            sqlCommand.Append("UseLdapAuth = ?UseLdapAuth, ");
            sqlCommand.Append("AutoCreateLDAPUserOnFirstLogin = ?AutoCreateLDAPUserOnFirstLogin, ");
            sqlCommand.Append("LdapServer = ?LdapServer, ");
            sqlCommand.Append("LdapPort = ?LdapPort, ");
            sqlCommand.Append("LdapDomain = ?LdapDomain, ");
            sqlCommand.Append("LdapRootDN = ?LdapRootDN, ");
            sqlCommand.Append("LdapUserDNKey = ?LdapUserDNKey, ");
            sqlCommand.Append("UseEmailForLogin = ?UseEmailForLogin, ");
            sqlCommand.Append("ReallyDeleteUsers = ?ReallyDeleteUsers, ");
            sqlCommand.Append("RecaptchaPrivateKey = ?RecaptchaPrivateKey, ");
            sqlCommand.Append("RecaptchaPublicKey = ?RecaptchaPublicKey, ");
            sqlCommand.Append("DisableDbAuth = ?DisableDbAuth, ");

            sqlCommand.Append("RequiresQuestionAndAnswer = ?RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts = ?MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("MinRequiredPasswordLength = ?MinRequiredPasswordLength, ");
            sqlCommand.Append("DefaultEmailFromAddress = ?DefaultEmailFromAddress, ");
            sqlCommand.Append("AllowDbFallbackWithLdap = ?AllowDbFallbackWithLdap, ");
            sqlCommand.Append("EmailLdapDbFallback = ?EmailLdapDbFallback, ");
            sqlCommand.Append("AllowPersistentLogin = ?AllowPersistentLogin, ");
            sqlCommand.Append("CaptchaOnLogin = ?CaptchaOnLogin, ");
            sqlCommand.Append("CaptchaOnRegistration = ?CaptchaOnRegistration,");
            sqlCommand.Append("SiteIsClosed = ?SiteIsClosed, ");
            sqlCommand.Append("SiteIsClosedMessage = ?SiteIsClosedMessage, ");
            sqlCommand.Append("PrivacyPolicy = ?PrivacyPolicy, ");
            sqlCommand.Append("TimeZoneId = ?TimeZoneId, ");
            sqlCommand.Append("GoogleAnalyticsProfileId = ?GoogleAnalyticsProfileId, ");
            sqlCommand.Append("CompanyName = ?CompanyName, ");
            sqlCommand.Append("CompanyStreetAddress = ?CompanyStreetAddress, ");
            sqlCommand.Append("CompanyStreetAddress2 = ?CompanyStreetAddress2, ");
            sqlCommand.Append("CompanyRegion = ?CompanyRegion, ");
            sqlCommand.Append("CompanyLocality = ?CompanyLocality, ");
            sqlCommand.Append("CompanyCountry = ?CompanyCountry, ");
            sqlCommand.Append("CompanyPostalCode = ?CompanyPostalCode, ");
            sqlCommand.Append("CompanyPublicEmail = ?CompanyPublicEmail, ");
            sqlCommand.Append("CompanyPhone = ?CompanyPhone, ");
            sqlCommand.Append("CompanyFax = ?CompanyFax, ");
            sqlCommand.Append("FacebookAppId = ?FacebookAppId, ");
            sqlCommand.Append("FacebookAppSecret = ?FacebookAppSecret, ");
            sqlCommand.Append("GoogleClientId = ?GoogleClientId, ");
            sqlCommand.Append("GoogleClientSecret = ?GoogleClientSecret, ");
            sqlCommand.Append("TwitterConsumerKey = ?TwitterConsumerKey, ");
            sqlCommand.Append("TwitterConsumerSecret = ?TwitterConsumerSecret, ");
            sqlCommand.Append("MicrosoftClientId = ?MicrosoftClientId, ");
            sqlCommand.Append("MicrosoftClientSecret = ?MicrosoftClientSecret, ");
            sqlCommand.Append("PreferredHostName = ?PreferredHostName, ");
            sqlCommand.Append("SiteFolderName = ?SiteFolderName, ");
            sqlCommand.Append("AddThisDotComUsername = ?AddThisDotComUsername, ");
            sqlCommand.Append("LoginInfoTop = ?LoginInfoTop, ");
            sqlCommand.Append("LoginInfoBottom = ?LoginInfoBottom, ");
            sqlCommand.Append("RegistrationAgreement = ?RegistrationAgreement, ");
            sqlCommand.Append("RegistrationPreamble = ?RegistrationPreamble, ");
            sqlCommand.Append("SmtpServer = ?SmtpServer, ");
            sqlCommand.Append("SmtpPort = ?SmtpPort, ");
            sqlCommand.Append("SmtpUser = ?SmtpUser, ");
            sqlCommand.Append("SmtpPassword = ?SmtpPassword, ");
            sqlCommand.Append("SmtpPreferredEncoding = ?SmtpPreferredEncoding, ");
            sqlCommand.Append("SmtpRequiresAuth = ?SmtpRequiresAuth, ");
            sqlCommand.Append("SmtpUseSsl = ?SmtpUseSsl, ");
            sqlCommand.Append("IsDataProtected = ?IsDataProtected, ");

            sqlCommand.Append("RequireConfirmedPhone = ?RequireConfirmedPhone, ");
            sqlCommand.Append("DefaultEmailFromAlias = ?DefaultEmailFromAlias, ");
            sqlCommand.Append("AccountApprovalEmailCsv = ?AccountApprovalEmailCsv, ");
            sqlCommand.Append("DkimPublicKey = ?DkimPublicKey, ");
            sqlCommand.Append("DkimPrivateKey = ?DkimPrivateKey, ");
            sqlCommand.Append("DkimDomain = ?DkimDomain, ");
            sqlCommand.Append("DkimSelector = ?DkimSelector, ");
            sqlCommand.Append("SignEmailWithDkim = ?SignEmailWithDkim, ");
            sqlCommand.Append("OidConnectAppId = ?OidConnectAppId, ");
            sqlCommand.Append("OidConnectAppSecret = ?OidConnectAppSecret, ");
            sqlCommand.Append("SmsClientId = ?SmsClientId, ");
            sqlCommand.Append("SmsSecureToken = ?SmsSecureToken, ");
            sqlCommand.Append("SmsFrom = ?SmsFrom, ");
            
            sqlCommand.Append("RequireApprovalBeforeLogin = ?RequireApprovalBeforeLogin ");
            
            sqlCommand.Append(" WHERE SiteID = ?SiteID ;");

            MySqlParameter[] arParams = new MySqlParameter[79];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?SiteName", MySqlDbType.VarChar, 128);
            arParams[1].Value = siteName;

            arParams[2] = new MySqlParameter("?IsServerAdminSite", MySqlDbType.Int32);
            arParams[2].Value = isServerAdminSite ? 1 : 0;

            arParams[3] = new MySqlParameter("?Skin", MySqlDbType.VarChar, 100);
            arParams[3].Value = skin;
            
            arParams[4] = new MySqlParameter("?AllowNewRegistration", MySqlDbType.Int32);
            arParams[4].Value = allowNewRegistration ? 1 : 0;
            
            arParams[5] = new MySqlParameter("?UseSecureRegistration", MySqlDbType.Int32);
            arParams[5].Value = useSecureRegistration ? 1 : 0;
            
            arParams[6] = new MySqlParameter("?UseLdapAuth", MySqlDbType.Int32);
            arParams[6].Value = useLdapAuth ? 1 : 0;

            arParams[7] = new MySqlParameter("?AutoCreateLDAPUserOnFirstLogin", MySqlDbType.Int32);
            arParams[7].Value = autoCreateLdapUserOnFirstLogin ? 1 : 0;

            arParams[8] = new MySqlParameter("?LdapServer", MySqlDbType.VarChar, 255);
            arParams[8].Value = ldapServer;

            arParams[9] = new MySqlParameter("?LdapPort", MySqlDbType.Int32);
            arParams[9].Value = ldapPort;

            arParams[10] = new MySqlParameter("?LdapRootDN", MySqlDbType.VarChar, 255);
            arParams[10].Value = ldapRootDN;

            arParams[11] = new MySqlParameter("?LdapUserDNKey", MySqlDbType.VarChar, 10);
            arParams[11].Value = ldapUserDNKey;
            
            arParams[12] = new MySqlParameter("?UseEmailForLogin", MySqlDbType.Int32);
            arParams[12].Value = useEmailForLogin ? 1 :0;

            arParams[13] = new MySqlParameter("?ReallyDeleteUsers", MySqlDbType.Int32);
            arParams[13].Value = reallyDeleteUsers ? 1 : 0;
            
            arParams[14] = new MySqlParameter("?LdapDomain", MySqlDbType.VarChar, 255);
            arParams[14].Value = ldapDomain;
            
            arParams[15] = new MySqlParameter("?RecaptchaPrivateKey", MySqlDbType.VarChar, 255);
            arParams[15].Value = recaptchaPrivateKey;

            arParams[16] = new MySqlParameter("?RecaptchaPublicKey", MySqlDbType.VarChar, 255);
            arParams[16].Value = recaptchaPublicKey;
            
            arParams[17] = new MySqlParameter("?DisableDbAuth", MySqlDbType.Int32);
            arParams[17].Value = disableDbAuth ? 1 : 0;

            arParams[18] = new MySqlParameter("?RequiresQuestionAndAnswer", MySqlDbType.Int16);
            arParams[18].Value = requiresQuestionAndAnswer ? 1 : 0;

            arParams[19] = new MySqlParameter("?MaxInvalidPasswordAttempts", MySqlDbType.Int32);
            arParams[19].Value = maxInvalidPasswordAttempts;
            
            arParams[20] = new MySqlParameter("?MinRequiredPasswordLength", MySqlDbType.Int32);
            arParams[20].Value = minRequiredPasswordLength;
            
            arParams[21] = new MySqlParameter("?DefaultEmailFromAddress", MySqlDbType.VarChar, 100);
            arParams[21].Value = defaultEmailFromAddress;

            arParams[22] = new MySqlParameter("?AllowDbFallbackWithLdap", MySqlDbType.Int16);
            arParams[22].Value = allowDbFallbackWithLdap ? 1 : 0;

            arParams[23] = new MySqlParameter("?EmailLdapDbFallback", MySqlDbType.Int16);
            arParams[23].Value = emailLdapDbFallback ? 1 : 0;

            arParams[24] = new MySqlParameter("?AllowPersistentLogin", MySqlDbType.Int16);
            arParams[24].Value = allowPersistentLogin ? 1 : 0;

            arParams[25] = new MySqlParameter("?CaptchaOnLogin", MySqlDbType.Int16);
            arParams[25].Value = captchaOnLogin ? 1 : 0;

            arParams[26] = new MySqlParameter("?CaptchaOnRegistration", MySqlDbType.Int16);
            arParams[26].Value = captchaOnRegistration ? 1 : 0;

            arParams[27] = new MySqlParameter("?SiteIsClosed", MySqlDbType.Int16);
            arParams[27].Value = siteIsClosed ? 1 : 0;

            arParams[28] = new MySqlParameter("?SiteIsClosedMessage", MySqlDbType.Text);
            arParams[28].Value = siteIsClosedMessage;

            arParams[29] = new MySqlParameter("?PrivacyPolicy", MySqlDbType.Text);
            arParams[29].Value = privacyPolicy;

            arParams[30] = new MySqlParameter("?TimeZoneId", MySqlDbType.VarChar, 50);
            arParams[30].Value = timeZoneId;

            arParams[31] = new MySqlParameter("?GoogleAnalyticsProfileId", MySqlDbType.VarChar, 50);
            arParams[31].Value = googleAnalyticsProfileId;

            arParams[32] = new MySqlParameter("?CompanyName", MySqlDbType.VarChar, 255);
            arParams[32].Value = companyName;

            arParams[33] = new MySqlParameter("?CompanyStreetAddress", MySqlDbType.VarChar, 250);
            arParams[33].Value = companyStreetAddress;

            arParams[34] = new MySqlParameter("?CompanyStreetAddress2", MySqlDbType.VarChar, 250);
            arParams[34].Value = companyStreetAddress2;

            arParams[35] = new MySqlParameter("?CompanyRegion", MySqlDbType.VarChar, 200);
            arParams[35].Value = companyRegion;

            arParams[36] = new MySqlParameter("?CompanyLocality", MySqlDbType.VarChar, 200);
            arParams[36].Value = companyLocality;

            arParams[37] = new MySqlParameter("?CompanyCountry", MySqlDbType.VarChar, 10);
            arParams[37].Value = companyCountry;

            arParams[38] = new MySqlParameter("?CompanyPostalCode", MySqlDbType.VarChar, 20);
            arParams[38].Value = companyPostalCode;

            arParams[39] = new MySqlParameter("?CompanyPublicEmail", MySqlDbType.VarChar, 100);
            arParams[39].Value = companyPublicEmail;

            arParams[40] = new MySqlParameter("?CompanyPhone", MySqlDbType.VarChar, 20);
            arParams[40].Value = companyPhone;

            arParams[41] = new MySqlParameter("?CompanyFax", MySqlDbType.VarChar, 20);
            arParams[41].Value = companyFax;

            arParams[42] = new MySqlParameter("?FacebookAppId", MySqlDbType.VarChar, 100);
            arParams[42].Value = facebookAppId;

            arParams[43] = new MySqlParameter("?FacebookAppSecret", MySqlDbType.Text);
            arParams[43].Value = facebookAppSecret;

            arParams[44] = new MySqlParameter("?GoogleClientId", MySqlDbType.VarChar, 100);
            arParams[44].Value = googleClientId;

            arParams[45] = new MySqlParameter("?GoogleClientSecret", MySqlDbType.Text);
            arParams[45].Value = googleClientSecret;

            arParams[46] = new MySqlParameter("?TwitterConsumerKey", MySqlDbType.VarChar, 100);
            arParams[46].Value = twitterConsumerKey;

            arParams[47] = new MySqlParameter("?TwitterConsumerSecret", MySqlDbType.Text);
            arParams[47].Value = twitterConsumerSecret;

            arParams[48] = new MySqlParameter("?MicrosoftClientId", MySqlDbType.VarChar, 100);
            arParams[48].Value = microsoftClientId;

            arParams[49] = new MySqlParameter("?MicrosoftClientSecret", MySqlDbType.Text);
            arParams[49].Value = microsoftClientSecret;

            arParams[50] = new MySqlParameter("?PreferredHostName", MySqlDbType.VarChar, 250);
            arParams[50].Value = preferredHostName;

            arParams[51] = new MySqlParameter("?SiteFolderName", MySqlDbType.VarChar, 50);
            arParams[51].Value = siteFolderName;

            arParams[52] = new MySqlParameter("?AddThisDotComUsername", MySqlDbType.VarChar, 50);
            arParams[52].Value = addThisDotComUsername;

            arParams[53] = new MySqlParameter("?LoginInfoTop", MySqlDbType.Text);
            arParams[53].Value = loginInfoTop;

            arParams[54] = new MySqlParameter("?LoginInfoBottom", MySqlDbType.Text);
            arParams[54].Value = loginInfoBottom;

            arParams[55] = new MySqlParameter("?RegistrationAgreement", MySqlDbType.Text);
            arParams[55].Value = registrationAgreement;

            arParams[56] = new MySqlParameter("?RegistrationPreamble", MySqlDbType.Text);
            arParams[56].Value = registrationPreamble;

            arParams[57] = new MySqlParameter("?SmtpServer", MySqlDbType.VarChar, 200);
            arParams[57].Value = smtpServer;

            arParams[58] = new MySqlParameter("?SmtpPort", MySqlDbType.Int32);
            arParams[58].Value = smtpPort;

            arParams[59] = new MySqlParameter("?SmtpUser", MySqlDbType.VarChar, 500);
            arParams[59].Value = smtpUser;

            arParams[60] = new MySqlParameter("?SmtpPassword", MySqlDbType.Text);
            arParams[60].Value = smtpPassword;

            arParams[61] = new MySqlParameter("?SmtpPreferredEncoding", MySqlDbType.VarChar, 20);
            arParams[61].Value = smtpPreferredEncoding;

            arParams[62] = new MySqlParameter("?SmtpRequiresAuth", MySqlDbType.Int16);
            arParams[62].Value = smtpRequiresAuth ? 1 : 0;

            arParams[63] = new MySqlParameter("?SmtpUseSsl", MySqlDbType.Int16);
            arParams[63].Value = smtpUseSsl ? 1 : 0;

            arParams[64] = new MySqlParameter("?RequireApprovalBeforeLogin", MySqlDbType.Int16);
            arParams[64].Value = requireApprovalBeforeLogin ? 1 : 0;

            arParams[65] = new MySqlParameter("?IsDataProtected", MySqlDbType.Int16);
            arParams[65].Value = isDataProtected ? 1 : 0;

            arParams[66] = new MySqlParameter("?RequireConfirmedPhone", MySqlDbType.Int16);
            arParams[66].Value = requireConfirmedPhone ? 1 : 0;

            arParams[67] = new MySqlParameter("?DefaultEmailFromAlias", MySqlDbType.VarChar, 100);
            arParams[67].Value = defaultEmailFromAddress;

            arParams[68] = new MySqlParameter("?AccountApprovalEmailCsv", MySqlDbType.Text);
            arParams[68].Value = accountApprovalEmailCsv;

            arParams[69] = new MySqlParameter("?DkimPublicKey", MySqlDbType.Text);
            arParams[69].Value = dkimPublicKey;

            arParams[70] = new MySqlParameter("?DkimPrivateKey", MySqlDbType.Text);
            arParams[70].Value = dkimPrivateKey;

            arParams[71] = new MySqlParameter("?DkimDomain", MySqlDbType.VarChar, 255);
            arParams[71].Value = dkimDomain;

            arParams[72] = new MySqlParameter("?DkimSelector", MySqlDbType.VarChar, 128);
            arParams[72].Value = dkimSelector;

            arParams[73] = new MySqlParameter("?SignEmailWithDkim", MySqlDbType.Int16);
            arParams[73].Value = signEmailWithDkim ? 1 : 0;

            arParams[74] = new MySqlParameter("?OidConnectAppId", MySqlDbType.VarChar, 255);
            arParams[74].Value = oidConnectAppId;

            arParams[75] = new MySqlParameter("?OidConnectAppSecret", MySqlDbType.Text);
            arParams[75].Value = oidConnectAppSecret;

            arParams[76] = new MySqlParameter("?SmsClientId", MySqlDbType.VarChar, 255);
            arParams[76].Value = smsClientId;

            arParams[77] = new MySqlParameter("?SmsSecureToken", MySqlDbType.Text);
            arParams[77].Value = smsSecureToken;

            arParams[78] = new MySqlParameter("?SmsFrom", MySqlDbType.VarChar, 255);
            arParams[78].Value = smsFrom;


            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

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

        public async Task<bool> Delete(
            int siteId,
            CancellationToken cancellationToken)
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
                arParams,
                cancellationToken);

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

        public async Task<int> GetHostCount(CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append(";");

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                sqlCommand.ToString(),
                null,
                cancellationToken);

            return Convert.ToInt32(result);
        }

        public async Task<DbDataReader> GetAllHosts(CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append("ORDER BY HostName  ");
            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                null,
                cancellationToken);
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
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
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
                arParams,
                cancellationToken);
        }


        public async Task<DbDataReader> GetHostList(
            int siteId,
            CancellationToken cancellationToken)
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
                arParams,
                cancellationToken);


        }

        public async Task<DbDataReader> GetHost(
            string hostName,
            CancellationToken cancellationToken)
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
                arParams,
                cancellationToken);
        }

        public async Task<bool> AddHost(
            Guid siteGuid, 
            int siteId, 
            string hostName,
            CancellationToken cancellationToken)
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
                arParams,
                cancellationToken);

            return rowsAffected > 0;

        }

        public async Task<bool> DeleteHost(
            int hostId,
            CancellationToken cancellationToken)
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
                arParams,
                cancellationToken);

            return rowsAffected > 0;

        }

        public async Task<bool> DeleteHostsBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE SiteID = ?SiteID  ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return rowsAffected > 0;

        }

        public async Task<DbDataReader> GetSiteList(CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Sites ");

            sqlCommand.Append("ORDER BY	SiteName ;");
            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                sqlCommand.ToString(),
                null,
                cancellationToken);
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

        public async Task<DbDataReader> GetSite(
            int siteId,
            CancellationToken cancellationToken)
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
                arParams,
                cancellationToken);
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

        public async Task<DbDataReader> GetSite(
            Guid siteGuid,
            CancellationToken cancellationToken)
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
                arParams,
                cancellationToken);
        }

        public DbDataReader GetSiteNonAsync(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Sites ");
            sqlCommand.Append("WHERE SiteGuid = ?SiteGuid ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);
        }



        public async Task<DbDataReader> GetSite(
            string hostName,
            CancellationToken cancellationToken)
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
                arParams,
                cancellationToken))
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
                arParams,
                cancellationToken);

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

        public async Task<int> CountOtherSites(
            int currentSiteId,
            CancellationToken cancellationToken)
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
                arParams,
                cancellationToken);

            return Convert.ToInt32(result);

        }

        public async Task<DbDataReader> GetPageOfOtherSites(
            int currentSiteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
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
                arParams,
                cancellationToken);

        }


        public async Task<int> GetSiteIdByHostName(
            string hostName,
            CancellationToken cancellationToken)
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
                arParams,
                cancellationToken))
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
                null,
                cancellationToken))
                {
                    if (reader.Read())
                    {
                        siteId = Convert.ToInt32(reader["SiteID"]);
                    }
                }

            }

            return siteId;

        }

        public async Task<int> GetSiteIdByFolder(
            string folderName,
            CancellationToken cancellationToken)
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
                arParams,
                cancellationToken))
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
                null,
                cancellationToken))
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
