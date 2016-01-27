// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2010-03-10
// Last Modified:			2016-01-27
// 


using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Data.SqlServerCe;
using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;

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

            // possibly will change this later to have SqlCeProviderFactory/DbProviderFactory injected
            AdoHelper = new SqlCeHelper(SqlCeProviderFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string connectionString;
        private SqlCeHelper AdoHelper;

        public int Create(
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
            string smsFrom

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
            sqlCommand.Append("RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("MinRequiredPasswordLength, ");
            sqlCommand.Append("DefaultEmailFromAddress, ");
            sqlCommand.Append("RecaptchaPrivateKey, ");
            sqlCommand.Append("RecaptchaPublicKey, ");
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

            sqlCommand.Append("RequireApprovalBeforeLogin ");
            
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@SiteName, ");
            sqlCommand.Append("@Skin, ");
            sqlCommand.Append("@AllowNewRegistration, ");
            sqlCommand.Append("@UseSecureRegistration, ");
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
            sqlCommand.Append("@RequiresQuestionAndAnswer, ");
            sqlCommand.Append("@MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("@MinRequiredPasswordLength, ");
            sqlCommand.Append("@DefaultEmailFromAddress, ");
            sqlCommand.Append("@RecaptchaPrivateKey, ");
            sqlCommand.Append("@RecaptchaPublicKey, ");
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
            sqlCommand.Append("@IsDataProtected, ");
            sqlCommand.Append("@CreatedUtc, ");

            sqlCommand.Append("@RequireConfirmedPhone, ");
            sqlCommand.Append("@DefaultEmailFromAlias, ");
            sqlCommand.Append("@AccountApprovalEmailCsv, ");
            sqlCommand.Append("@DkimPublicKey, ");
            sqlCommand.Append("@DkimPrivateKey, ");
            sqlCommand.Append("@DkimDomain, ");
            sqlCommand.Append("@DkimSelector, ");
            sqlCommand.Append("@SignEmailWithDkim, ");
            sqlCommand.Append("@OidConnectAppId, ");
            sqlCommand.Append("@OidConnectAppSecret, ");
            sqlCommand.Append("@SmsClientId, ");
            sqlCommand.Append("@SmsSecureToken, ");
            sqlCommand.Append("@SmsFrom, ");

            sqlCommand.Append("@RequireApprovalBeforeLogin ");

            
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[80];

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
            
            arParams[5] = new SqlCeParameter("@IsServerAdminSite", SqlDbType.Bit);
            arParams[5].Value = isServerAdminSite;

            arParams[6] = new SqlCeParameter("@UseLdapAuth", SqlDbType.Bit);
            arParams[6].Value = useLdapAuth;

            arParams[7] = new SqlCeParameter("@AutoCreateLdapUserOnFirstLogin", SqlDbType.Bit);
            arParams[7].Value = autoCreateLdapUserOnFirstLogin;

            arParams[8] = new SqlCeParameter("@LdapServer", SqlDbType.NVarChar);
            arParams[8].Value = ldapServer;

            arParams[9] = new SqlCeParameter("@LdapPort", SqlDbType.Int);
            arParams[9].Value = ldapPort;

            arParams[10] = new SqlCeParameter("@LdapDomain", SqlDbType.NVarChar);
            arParams[10].Value = ldapDomain;

            arParams[11] = new SqlCeParameter("@LdapRootDN", SqlDbType.NVarChar);
            arParams[11].Value = ldapRootDN;

            arParams[12] = new SqlCeParameter("@LdapUserDNKey", SqlDbType.NVarChar);
            arParams[12].Value = ldapUserDNKey;

            arParams[13] = new SqlCeParameter("@ReallyDeleteUsers", SqlDbType.Bit);
            arParams[13].Value = reallyDeleteUsers;

            arParams[14] = new SqlCeParameter("@UseEmailForLogin", SqlDbType.Bit);
            arParams[14].Value = useEmailForLogin;
            
            arParams[15] = new SqlCeParameter("@RequiresQuestionAndAnswer", SqlDbType.Bit);
            arParams[15].Value = requiresQuestionAndAnswer;

            arParams[16] = new SqlCeParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int);
            arParams[16].Value = maxInvalidPasswordAttempts;
            
            arParams[17] = new SqlCeParameter("@MinRequiredPasswordLength", SqlDbType.Int);
            arParams[17].Value = minRequiredPasswordLength;
            
            arParams[18] = new SqlCeParameter("@DefaultEmailFromAddress", SqlDbType.NVarChar);
            arParams[18].Value = defaultEmailFromAddress;
            
            arParams[19] = new SqlCeParameter("@RecaptchaPrivateKey", SqlDbType.NVarChar);
            arParams[19].Value = recaptchaPrivateKey;

            arParams[20] = new SqlCeParameter("@RecaptchaPublicKey", SqlDbType.NVarChar);
            arParams[20].Value = recaptchaPublicKey;
            
            arParams[21] = new SqlCeParameter("@DisableDbAuth", SqlDbType.Bit);
            arParams[21].Value = disableDbAuth;

            arParams[22] = new SqlCeParameter("@AllowDbFallbackWithLdap", SqlDbType.Bit);
            arParams[22].Value = allowDbFallbackWithLdap;

            arParams[23] = new SqlCeParameter("@EmailLdapDbFallback", SqlDbType.Bit);
            arParams[23].Value = emailLdapDbFallback;

            arParams[24] = new SqlCeParameter("@AllowPersistentLogin", SqlDbType.Bit);
            arParams[24].Value = allowPersistentLogin;

            arParams[25] = new SqlCeParameter("@CaptchaOnLogin", SqlDbType.Bit);
            arParams[25].Value = captchaOnLogin;

            arParams[26] = new SqlCeParameter("@CaptchaOnRegistration", SqlDbType.Bit);
            arParams[26].Value = captchaOnRegistration;

            arParams[27] = new SqlCeParameter("@SiteIsClosed", SqlDbType.Bit);
            arParams[27].Value = siteIsClosed;

            arParams[28] = new SqlCeParameter("@SiteIsClosedMessage", SqlDbType.NText);
            arParams[28].Value = siteIsClosedMessage;

            arParams[29] = new SqlCeParameter("@PrivacyPolicy", SqlDbType.NText);
            arParams[29].Value = privacyPolicy;

            arParams[30] = new SqlCeParameter("@TimeZoneId", SqlDbType.NVarChar, 50);
            arParams[30].Value = timeZoneId;

            arParams[31] = new SqlCeParameter("@GoogleAnalyticsProfileId", SqlDbType.NVarChar, 25);
            arParams[31].Value = googleAnalyticsProfileId;

            arParams[32] = new SqlCeParameter("@CompanyName", SqlDbType.NVarChar, 255);
            arParams[32].Value = companyName;

            arParams[33] = new SqlCeParameter("@CompanyStreetAddress", SqlDbType.NVarChar, 250);
            arParams[33].Value = companyStreetAddress;

            arParams[34] = new SqlCeParameter("@CompanyStreetAddress2", SqlDbType.NVarChar, 250);
            arParams[34].Value = companyStreetAddress2;

            arParams[35] = new SqlCeParameter("@CompanyRegion", SqlDbType.NVarChar, 200);
            arParams[35].Value = companyRegion;

            arParams[36] = new SqlCeParameter("@CompanyLocality", SqlDbType.NVarChar, 200);
            arParams[36].Value = companyLocality;

            arParams[37] = new SqlCeParameter("@CompanyCountry", SqlDbType.NVarChar, 10);
            arParams[37].Value = companyCountry;

            arParams[38] = new SqlCeParameter("@CompanyPostalCode", SqlDbType.NVarChar, 20);
            arParams[38].Value = companyPostalCode;

            arParams[39] = new SqlCeParameter("@CompanyPublicEmail", SqlDbType.NVarChar, 100);
            arParams[39].Value = companyPublicEmail;

            arParams[40] = new SqlCeParameter("@CompanyPhone", SqlDbType.NVarChar, 20);
            arParams[40].Value = companyPhone;

            arParams[41] = new SqlCeParameter("@CompanyFax", SqlDbType.NVarChar, 20);
            arParams[41].Value = companyFax;

            arParams[42] = new SqlCeParameter("@FacebookAppId", SqlDbType.NVarChar, 100);
            arParams[42].Value = facebookAppId;

            arParams[43] = new SqlCeParameter("@FacebookAppSecret", SqlDbType.NText);
            arParams[43].Value = facebookAppSecret;

            arParams[44] = new SqlCeParameter("@GoogleClientId", SqlDbType.NVarChar, 100);
            arParams[44].Value = googleClientId;

            arParams[45] = new SqlCeParameter("@GoogleClientSecret", SqlDbType.NText);
            arParams[45].Value = googleClientSecret;

            arParams[46] = new SqlCeParameter("@TwitterConsumerKey", SqlDbType.NVarChar, 100);
            arParams[46].Value = twitterConsumerKey;

            arParams[47] = new SqlCeParameter("@TwitterConsumerSecret", SqlDbType.NText);
            arParams[47].Value = twitterConsumerSecret;

            arParams[48] = new SqlCeParameter("@MicrosoftClientId", SqlDbType.NVarChar, 100);
            arParams[48].Value = microsoftClientId;

            arParams[49] = new SqlCeParameter("@MicrosoftClientSecret", SqlDbType.NText);
            arParams[49].Value = microsoftClientSecret;

            arParams[50] = new SqlCeParameter("@PreferredHostName", SqlDbType.NVarChar, 250);
            arParams[50].Value = preferredHostName;

            arParams[51] = new SqlCeParameter("@SiteFolderName", SqlDbType.NVarChar, 50);
            arParams[51].Value = siteFolderName;

            arParams[52] = new SqlCeParameter("@AddThisDotComUsername", SqlDbType.NVarChar, 50);
            arParams[52].Value = addThisDotComUsername;

            arParams[53] = new SqlCeParameter("@LoginInfoTop", SqlDbType.NText);
            arParams[53].Value = loginInfoTop;

            arParams[54] = new SqlCeParameter("@LoginInfoBottom", SqlDbType.NText);
            arParams[54].Value = loginInfoBottom;

            arParams[55] = new SqlCeParameter("@RegistrationAgreement", SqlDbType.NText);
            arParams[55].Value = registrationAgreement;

            arParams[56] = new SqlCeParameter("@RegistrationPreamble", SqlDbType.NText);
            arParams[56].Value = registrationPreamble;

            arParams[57] = new SqlCeParameter("@SmtpServer", SqlDbType.NVarChar, 200);
            arParams[57].Value = smtpServer;

            arParams[58] = new SqlCeParameter("@SmtpPort", SqlDbType.Int);
            arParams[58].Value = smtpPort;

            arParams[59] = new SqlCeParameter("@SmtpUser", SqlDbType.NVarChar, 500);
            arParams[59].Value = smtpUser;

            arParams[60] = new SqlCeParameter("@SmtpPassword", SqlDbType.NText);
            arParams[60].Value = smtpPassword;

            arParams[61] = new SqlCeParameter("@SmtpPreferredEncoding", SqlDbType.NVarChar, 20);
            arParams[61].Value = smtpPreferredEncoding;

            arParams[62] = new SqlCeParameter("@SmtpRequiresAuth", SqlDbType.Bit);
            arParams[62].Value = smtpRequiresAuth;

            arParams[63] = new SqlCeParameter("@SmtpUseSsl", SqlDbType.Bit);
            arParams[63].Value = smtpUseSsl;

            arParams[64] = new SqlCeParameter("@RequireApprovalBeforeLogin", SqlDbType.Bit);
            arParams[64].Value = requireApprovalBeforeLogin;

            arParams[65] = new SqlCeParameter("@IsDataProtected", SqlDbType.Bit);
            arParams[65].Value = isDataProtected;

            arParams[66] = new SqlCeParameter("@CreatedUtc", SqlDbType.DateTime);
            arParams[66].Value = createdUtc;

            arParams[67] = new SqlCeParameter("@RequireConfirmedPhone", SqlDbType.Bit);
            arParams[67].Value = requireConfirmedPhone;

            arParams[68] = new SqlCeParameter("@DefaultEmailFromAlias", SqlDbType.NVarChar, 100);
            arParams[68].Value = defaultEmailFromAddress;

            arParams[69] = new SqlCeParameter("@AccountApprovalEmailCsv", SqlDbType.NText);
            arParams[69].Value = accountApprovalEmailCsv;

            arParams[70] = new SqlCeParameter("@DkimPublicKey", SqlDbType.NText);
            arParams[70].Value = dkimPublicKey;

            arParams[71] = new SqlCeParameter("@DkimPrivateKey", SqlDbType.NText);
            arParams[71].Value = dkimPrivateKey;

            arParams[72] = new SqlCeParameter("@DkimDomain", SqlDbType.NVarChar, 255);
            arParams[72].Value = dkimDomain;

            arParams[73] = new SqlCeParameter("@DkimSelector", SqlDbType.NVarChar, 128);
            arParams[73].Value = dkimSelector;

            arParams[74] = new SqlCeParameter("@SignEmailWithDkim", SqlDbType.Bit);
            arParams[74].Value = signEmailWithDkim;

            arParams[75] = new SqlCeParameter("@OidConnectAppId", SqlDbType.NVarChar, 255);
            arParams[75].Value = oidConnectAppId;

            arParams[76] = new SqlCeParameter("@OidConnectAppSecret", SqlDbType.NText);
            arParams[76].Value = oidConnectAppSecret;

            arParams[77] = new SqlCeParameter("@SmsClientId", SqlDbType.NVarChar, 255);
            arParams[77].Value = smsClientId;

            arParams[78] = new SqlCeParameter("@SmsSecureToken", SqlDbType.NText);
            arParams[78].Value = smsSecureToken;

            arParams[79] = new SqlCeParameter("@SmsFrom", SqlDbType.NVarChar, 255);
            arParams[79].Value = smsFrom;


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
            string smsFrom


            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Sites ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("SiteName = @SiteName, ");
            sqlCommand.Append("Skin = @Skin, ");
            sqlCommand.Append("AllowNewRegistration = @AllowNewRegistration, ");
            sqlCommand.Append("UseSecureRegistration = @UseSecureRegistration, ");
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
            sqlCommand.Append("RecaptchaPrivateKey = @RecaptchaPrivateKey, ");
            sqlCommand.Append("RecaptchaPublicKey = @RecaptchaPublicKey, ");
            sqlCommand.Append("DisableDbAuth = @DisableDbAuth, ");
            sqlCommand.Append("RequiresQuestionAndAnswer = @RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts = @MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("MinRequiredPasswordLength = @MinRequiredPasswordLength, ");
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
            sqlCommand.Append("IsDataProtected = @IsDataProtected, ");

            sqlCommand.Append("RequireConfirmedPhone = @RequireConfirmedPhone, ");
            sqlCommand.Append("DefaultEmailFromAlias = @DefaultEmailFromAlias, ");
            sqlCommand.Append("AccountApprovalEmailCsv = @AccountApprovalEmailCsv, ");
            sqlCommand.Append("DkimPublicKey = @DkimPublicKey, ");
            sqlCommand.Append("DkimPrivateKey = @DkimPrivateKey, ");
            sqlCommand.Append("DkimDomain = @DkimDomain, ");
            sqlCommand.Append("DkimSelector = @DkimSelector, ");
            sqlCommand.Append("SignEmailWithDkim = @SignEmailWithDkim, ");
            sqlCommand.Append("OidConnectAppId = @OidConnectAppId, ");
            sqlCommand.Append("OidConnectAppSecret = @OidConnectAppSecret, ");
            sqlCommand.Append("SmsClientId = @SmsClientId, ");
            sqlCommand.Append("SmsSecureToken = @SmsSecureToken, ");
            sqlCommand.Append("SmsFrom = @SmsFrom, ");

            sqlCommand.Append("RequireApprovalBeforeLogin = @RequireApprovalBeforeLogin ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[79];

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
            
            arParams[5] = new SqlCeParameter("@IsServerAdminSite", SqlDbType.Bit);
            arParams[5].Value = isServerAdminSite;

            arParams[6] = new SqlCeParameter("@UseLdapAuth", SqlDbType.Bit);
            arParams[6].Value = useLdapAuth;

            arParams[7] = new SqlCeParameter("@AutoCreateLdapUserOnFirstLogin", SqlDbType.Bit);
            arParams[7].Value = autoCreateLdapUserOnFirstLogin;

            arParams[8] = new SqlCeParameter("@LdapServer", SqlDbType.NVarChar);
            arParams[8].Value = ldapServer;

            arParams[9] = new SqlCeParameter("@LdapPort", SqlDbType.Int);
            arParams[9].Value = ldapPort;

            arParams[10] = new SqlCeParameter("@LdapDomain", SqlDbType.NVarChar);
            arParams[10].Value = ldapDomain;

            arParams[11] = new SqlCeParameter("@LdapRootDN", SqlDbType.NVarChar);
            arParams[11].Value = ldapRootDN;

            arParams[12] = new SqlCeParameter("@LdapUserDNKey", SqlDbType.NVarChar);
            arParams[12].Value = ldapUserDNKey;

            arParams[13] = new SqlCeParameter("@ReallyDeleteUsers", SqlDbType.Bit);
            arParams[13].Value = reallyDeleteUsers;

            arParams[14] = new SqlCeParameter("@UseEmailForLogin", SqlDbType.Bit);
            arParams[14].Value = useEmailForLogin;
            
            arParams[15] = new SqlCeParameter("@RecaptchaPrivateKey", SqlDbType.NVarChar);
            arParams[15].Value = recaptchaPrivateKey;

            arParams[16] = new SqlCeParameter("@RecaptchaPublicKey", SqlDbType.NVarChar);
            arParams[16].Value = recaptchaPublicKey;
            
            arParams[17] = new SqlCeParameter("@DisableDbAuth", SqlDbType.Bit);
            arParams[17].Value = disableDbAuth;

            arParams[18] = new SqlCeParameter("@AllowDbFallbackWithLdap", SqlDbType.Bit);
            arParams[18].Value = allowDbFallbackWithLdap;

            arParams[19] = new SqlCeParameter("@EmailLdapDbFallback", SqlDbType.Bit);
            arParams[19].Value = emailLdapDbFallback;

            arParams[20] = new SqlCeParameter("@AllowPersistentLogin", SqlDbType.Bit);
            arParams[20].Value = allowPersistentLogin;

            arParams[21] = new SqlCeParameter("@CaptchaOnLogin", SqlDbType.Bit);
            arParams[21].Value = captchaOnLogin;

            arParams[22] = new SqlCeParameter("@CaptchaOnRegistration", SqlDbType.Bit);
            arParams[22].Value = captchaOnRegistration;

            arParams[23] = new SqlCeParameter("@SiteIsClosed", SqlDbType.Bit);
            arParams[23].Value = siteIsClosed;

            arParams[24] = new SqlCeParameter("@SiteIsClosedMessage", SqlDbType.NText);
            arParams[24].Value = siteIsClosedMessage;

            arParams[25] = new SqlCeParameter("@PrivacyPolicy", SqlDbType.NText);
            arParams[25].Value = privacyPolicy;

            arParams[26] = new SqlCeParameter("@TimeZoneId", SqlDbType.NVarChar, 50);
            arParams[26].Value = timeZoneId;

            arParams[27] = new SqlCeParameter("@GoogleAnalyticsProfileId", SqlDbType.NVarChar, 25);
            arParams[27].Value = googleAnalyticsProfileId;
        
            arParams[28] = new SqlCeParameter("@CompanyName", SqlDbType.NVarChar, 255);
            arParams[28].Value = companyName;

            arParams[29] = new SqlCeParameter("@CompanyStreetAddress", SqlDbType.NVarChar, 250);
            arParams[29].Value = companyStreetAddress;

            arParams[30] = new SqlCeParameter("@CompanyStreetAddress2", SqlDbType.NVarChar, 250);
            arParams[30].Value = companyStreetAddress2;

            arParams[31] = new SqlCeParameter("@CompanyRegion", SqlDbType.NVarChar, 200);
            arParams[31].Value = companyRegion;

            arParams[32] = new SqlCeParameter("@CompanyLocality", SqlDbType.NVarChar, 200);
            arParams[32].Value = companyLocality;

            arParams[33] = new SqlCeParameter("@CompanyCountry", SqlDbType.NVarChar, 10);
            arParams[33].Value = companyCountry;

            arParams[34] = new SqlCeParameter("@CompanyPostalCode", SqlDbType.NVarChar, 20);
            arParams[34].Value = companyPostalCode;

            arParams[35] = new SqlCeParameter("@CompanyPublicEmail", SqlDbType.NVarChar, 100);
            arParams[35].Value = companyPublicEmail;

            arParams[36] = new SqlCeParameter("@CompanyPhone", SqlDbType.NVarChar, 20);
            arParams[36].Value = companyPhone;

            arParams[37] = new SqlCeParameter("@CompanyFax", SqlDbType.NVarChar, 20);
            arParams[37].Value = companyFax;

            arParams[38] = new SqlCeParameter("@FacebookAppId", SqlDbType.NVarChar, 100);
            arParams[38].Value = facebookAppId;

            arParams[39] = new SqlCeParameter("@FacebookAppSecret", SqlDbType.NText);
            arParams[39].Value = facebookAppSecret;

            arParams[40] = new SqlCeParameter("@GoogleClientId", SqlDbType.NVarChar, 100);
            arParams[40].Value = googleClientId;

            arParams[41] = new SqlCeParameter("@GoogleClientSecret", SqlDbType.NText);
            arParams[41].Value = googleClientSecret;

            arParams[42] = new SqlCeParameter("@TwitterConsumerKey", SqlDbType.NVarChar, 100);
            arParams[42].Value = twitterConsumerKey;

            arParams[43] = new SqlCeParameter("@TwitterConsumerSecret", SqlDbType.NText);
            arParams[43].Value = twitterConsumerSecret;

            arParams[44] = new SqlCeParameter("@MicrosoftClientId", SqlDbType.NVarChar, 100);
            arParams[44].Value = microsoftClientId;

            arParams[45] = new SqlCeParameter("@MicrosoftClientSecret", SqlDbType.NText);
            arParams[45].Value = microsoftClientSecret;

            arParams[46] = new SqlCeParameter("@PreferredHostName", SqlDbType.NVarChar, 250);
            arParams[46].Value = preferredHostName;

            arParams[47] = new SqlCeParameter("@SiteFolderName", SqlDbType.NVarChar, 50);
            arParams[47].Value = siteFolderName;

            arParams[48] = new SqlCeParameter("@AddThisDotComUsername", SqlDbType.NVarChar, 50);
            arParams[48].Value = addThisDotComUsername;

            arParams[49] = new SqlCeParameter("@LoginInfoTop", SqlDbType.NText);
            arParams[49].Value = loginInfoTop;

            arParams[50] = new SqlCeParameter("@LoginInfoBottom", SqlDbType.NText);
            arParams[50].Value = loginInfoBottom;

            arParams[51] = new SqlCeParameter("@RegistrationAgreement", SqlDbType.NText);
            arParams[51].Value = registrationAgreement;

            arParams[52] = new SqlCeParameter("@RegistrationPreamble", SqlDbType.NText);
            arParams[52].Value = registrationPreamble;

            arParams[53] = new SqlCeParameter("@SmtpServer", SqlDbType.NVarChar, 200);
            arParams[53].Value = smtpServer;

            arParams[54] = new SqlCeParameter("@SmtpPort", SqlDbType.Int);
            arParams[54].Value = smtpPort;

            arParams[55] = new SqlCeParameter("@SmtpUser", SqlDbType.NVarChar, 500);
            arParams[55].Value = smtpUser;

            arParams[56] = new SqlCeParameter("@SmtpPassword", SqlDbType.NText);
            arParams[56].Value = smtpPassword;

            arParams[57] = new SqlCeParameter("@SmtpPreferredEncoding", SqlDbType.NVarChar, 20);
            arParams[57].Value = smtpPreferredEncoding;

            arParams[58] = new SqlCeParameter("@SmtpRequiresAuth", SqlDbType.Bit);
            arParams[58].Value = smtpRequiresAuth;

            arParams[59] = new SqlCeParameter("@SmtpUseSsl", SqlDbType.Bit);
            arParams[59].Value = smtpUseSsl;

            arParams[60] = new SqlCeParameter("@RequireApprovalBeforeLogin", SqlDbType.Bit);
            arParams[60].Value = requireApprovalBeforeLogin;
            
            arParams[61] = new SqlCeParameter("@RequiresQuestionAndAnswer", SqlDbType.Bit);
            arParams[61].Value = requiresQuestionAndAnswer;

            arParams[62] = new SqlCeParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int);
            arParams[62].Value = maxInvalidPasswordAttempts;
            
            arParams[63] = new SqlCeParameter("@MinRequiredPasswordLength", SqlDbType.Int);
            arParams[63].Value = minRequiredPasswordLength;
            
            arParams[64] = new SqlCeParameter("@DefaultEmailFromAddress", SqlDbType.NVarChar);
            arParams[64].Value = defaultEmailFromAddress;

            arParams[65] = new SqlCeParameter("@IsDataProtected", SqlDbType.Bit);
            arParams[65].Value = isDataProtected;

            arParams[66] = new SqlCeParameter("@RequireConfirmedPhone", SqlDbType.Bit);
            arParams[66].Value = requireConfirmedPhone;

            arParams[67] = new SqlCeParameter("@DefaultEmailFromAlias", SqlDbType.NVarChar, 100);
            arParams[67].Value = defaultEmailFromAddress;

            arParams[68] = new SqlCeParameter("@AccountApprovalEmailCsv", SqlDbType.NText);
            arParams[68].Value = accountApprovalEmailCsv;

            arParams[69] = new SqlCeParameter("@DkimPublicKey", SqlDbType.NText);
            arParams[69].Value = dkimPublicKey;

            arParams[70] = new SqlCeParameter("@DkimPrivateKey", SqlDbType.NText);
            arParams[70].Value = dkimPrivateKey;

            arParams[71] = new SqlCeParameter("@DkimDomain", SqlDbType.NVarChar, 255);
            arParams[71].Value = dkimDomain;

            arParams[72] = new SqlCeParameter("@DkimSelector", SqlDbType.NVarChar, 128);
            arParams[72].Value = dkimSelector;

            arParams[73] = new SqlCeParameter("@SignEmailWithDkim", SqlDbType.Bit);
            arParams[73].Value = signEmailWithDkim;

            arParams[74] = new SqlCeParameter("@OidConnectAppId", SqlDbType.NVarChar, 255);
            arParams[74].Value = oidConnectAppId;

            arParams[75] = new SqlCeParameter("@OidConnectAppSecret", SqlDbType.NText);
            arParams[75].Value = oidConnectAppSecret;

            arParams[76] = new SqlCeParameter("@SmsClientId", SqlDbType.NVarChar, 255);
            arParams[76].Value = smsClientId;

            arParams[77] = new SqlCeParameter("@SmsSecureToken", SqlDbType.NText);
            arParams[77].Value = smsSecureToken;

            arParams[78] = new SqlCeParameter("@SmsFrom", SqlDbType.NVarChar, 255);
            arParams[78].Value = smsFrom;

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

        public bool DeleteHostsBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE SiteID = @SiteID  ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Value = siteId;

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
