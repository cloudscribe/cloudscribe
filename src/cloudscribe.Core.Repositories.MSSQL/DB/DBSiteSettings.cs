// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-27
// 


using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;


namespace cloudscribe.Core.Repositories.MSSQL
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

            // possibly will change this later to have SqlClientFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(SqlClientFactory.Instance);
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

            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Sites_Insert", 
                80);

            sph.DefineSqlParameter("@SiteName", SqlDbType.NVarChar, 128, ParameterDirection.Input, siteName);
            sph.DefineSqlParameter("@Skin", SqlDbType.NVarChar, 100, ParameterDirection.Input, skin); 
            sph.DefineSqlParameter("@AllowNewRegistration", SqlDbType.Bit, ParameterDirection.Input, allowNewRegistration);
            sph.DefineSqlParameter("@UseSecureRegistration", SqlDbType.Bit, ParameterDirection.Input, useSecureRegistration);
            sph.DefineSqlParameter("@IsServerAdminSite", SqlDbType.Bit, ParameterDirection.Input, isServerAdminSite);
            sph.DefineSqlParameter("@UseLdapAuth", SqlDbType.Bit, ParameterDirection.Input, useLdapAuth);
            sph.DefineSqlParameter("@AutoCreateLDAPUserOnFirstLogin", SqlDbType.Bit, ParameterDirection.Input, autoCreateLdapUserOnFirstLogin);
            sph.DefineSqlParameter("@LdapServer", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapServer);
            sph.DefineSqlParameter("@LdapPort", SqlDbType.Int, ParameterDirection.Input, ldapPort);
            sph.DefineSqlParameter("@LdapDomain", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapDomain);
            sph.DefineSqlParameter("@LdapRootDN", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapRootDN);
            sph.DefineSqlParameter("@LdapUserDNKey", SqlDbType.NVarChar, 10, ParameterDirection.Input, ldapUserDNKey);
            sph.DefineSqlParameter("@UseEmailForLogin", SqlDbType.Bit, ParameterDirection.Input, useEmailForLogin);
            sph.DefineSqlParameter("@ReallyDeleteUsers", SqlDbType.Bit, ParameterDirection.Input, reallyDeleteUsers);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@RecaptchaPrivateKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, recaptchaPrivateKey);
            sph.DefineSqlParameter("@RecaptchaPublicKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, recaptchaPublicKey);
            sph.DefineSqlParameter("@DisableDbAuth", SqlDbType.Bit, ParameterDirection.Input, disableDbAuth);
            sph.DefineSqlParameter("@RequiresQuestionAndAnswer", SqlDbType.Bit, ParameterDirection.Input, requiresQuestionAndAnswer);
            sph.DefineSqlParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int, ParameterDirection.Input, maxInvalidPasswordAttempts);
            sph.DefineSqlParameter("@MinRequiredPasswordLength", SqlDbType.Int, ParameterDirection.Input, minRequiredPasswordLength);
            sph.DefineSqlParameter("@DefaultEmailFromAddress", SqlDbType.NVarChar, 100, ParameterDirection.Input, defaultEmailFromAddress);
            sph.DefineSqlParameter("@AllowDbFallbackWithLdap", SqlDbType.Bit, ParameterDirection.Input, allowDbFallbackWithLdap);
            sph.DefineSqlParameter("@EmailLdapDbFallback", SqlDbType.Bit, ParameterDirection.Input, emailLdapDbFallback);
            sph.DefineSqlParameter("@AllowPersistentLogin", SqlDbType.Bit, ParameterDirection.Input, allowPersistentLogin);
            sph.DefineSqlParameter("@CaptchaOnLogin", SqlDbType.Bit, ParameterDirection.Input, captchaOnLogin);
            sph.DefineSqlParameter("@CaptchaOnRegistration", SqlDbType.Bit, ParameterDirection.Input, captchaOnRegistration);
            sph.DefineSqlParameter("@SiteIsClosed", SqlDbType.Bit, ParameterDirection.Input, siteIsClosed);
            sph.DefineSqlParameter("@SiteIsClosedMessage", SqlDbType.NVarChar, -1, ParameterDirection.Input, siteIsClosedMessage);
            sph.DefineSqlParameter("@PrivacyPolicy", SqlDbType.NVarChar, -1, ParameterDirection.Input, privacyPolicy);
            sph.DefineSqlParameter("@TimeZoneId", SqlDbType.NVarChar, 50, ParameterDirection.Input, timeZoneId);
            sph.DefineSqlParameter("@GoogleAnalyticsProfileId", SqlDbType.NVarChar, 25, ParameterDirection.Input, googleAnalyticsProfileId);
            sph.DefineSqlParameter("@CompanyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, companyName);
            sph.DefineSqlParameter("@CompanyStreetAddress", SqlDbType.NVarChar, 250, ParameterDirection.Input, companyStreetAddress);
            sph.DefineSqlParameter("@CompanyStreetAddress2", SqlDbType.NVarChar, 250, ParameterDirection.Input, companyStreetAddress2);
            sph.DefineSqlParameter("@CompanyRegion", SqlDbType.NVarChar, 200, ParameterDirection.Input, companyRegion);
            sph.DefineSqlParameter("@CompanyLocality", SqlDbType.NVarChar, 200, ParameterDirection.Input, companyLocality);
            sph.DefineSqlParameter("@CompanyCountry", SqlDbType.NVarChar, 10, ParameterDirection.Input, companyCountry);
            sph.DefineSqlParameter("@CompanyPostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, companyPostalCode);
            sph.DefineSqlParameter("@CompanyPublicEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, companyPublicEmail);
            sph.DefineSqlParameter("@CompanyPhone", SqlDbType.NVarChar, 20, ParameterDirection.Input, companyPhone);
            sph.DefineSqlParameter("@CompanyFax", SqlDbType.NVarChar, 20, ParameterDirection.Input, companyFax);
            sph.DefineSqlParameter("@FacebookAppId", SqlDbType.NVarChar, 100, ParameterDirection.Input, facebookAppId);
            sph.DefineSqlParameter("@FacebookAppSecret", SqlDbType.NVarChar, -1, ParameterDirection.Input, facebookAppSecret);
            sph.DefineSqlParameter("@GoogleClientId", SqlDbType.NVarChar, 100, ParameterDirection.Input, googleClientId);
            sph.DefineSqlParameter("@GoogleClientSecret", SqlDbType.NVarChar, -1, ParameterDirection.Input, googleClientSecret);
            sph.DefineSqlParameter("@TwitterConsumerKey", SqlDbType.NVarChar, 100, ParameterDirection.Input, twitterConsumerKey);
            sph.DefineSqlParameter("@TwitterConsumerSecret", SqlDbType.NVarChar, -1, ParameterDirection.Input, twitterConsumerSecret);
            sph.DefineSqlParameter("@MicrosoftClientId", SqlDbType.NVarChar, 100, ParameterDirection.Input, microsoftClientId);
            sph.DefineSqlParameter("@MicrosoftClientSecret", SqlDbType.NVarChar, -1, ParameterDirection.Input, microsoftClientSecret);
            sph.DefineSqlParameter("@PreferredHostName", SqlDbType.NVarChar, 250, ParameterDirection.Input, preferredHostName);
            sph.DefineSqlParameter("@SiteFolderName", SqlDbType.NVarChar, 50, ParameterDirection.Input, siteFolderName);
            sph.DefineSqlParameter("@AddThisDotComUsername", SqlDbType.NVarChar, 50, ParameterDirection.Input, addThisDotComUsername);
            sph.DefineSqlParameter("@LoginInfoTop", SqlDbType.NVarChar, -1, ParameterDirection.Input, loginInfoTop);
            sph.DefineSqlParameter("@LoginInfoBottom", SqlDbType.NVarChar, -1, ParameterDirection.Input, loginInfoBottom);
            sph.DefineSqlParameter("@RegistrationAgreement", SqlDbType.NVarChar, -1, ParameterDirection.Input, registrationAgreement);
            sph.DefineSqlParameter("@RegistrationPreamble", SqlDbType.NVarChar, -1, ParameterDirection.Input, registrationPreamble);
            sph.DefineSqlParameter("@SmtpServer", SqlDbType.NVarChar, 200, ParameterDirection.Input, smtpServer);
            sph.DefineSqlParameter("@SmtpPort", SqlDbType.Int, ParameterDirection.Input, smtpPort);
            sph.DefineSqlParameter("@SmtpUser", SqlDbType.NVarChar, 500, ParameterDirection.Input, smtpUser);
            sph.DefineSqlParameter("@SmtpPassword", SqlDbType.NVarChar, -1, ParameterDirection.Input, smtpPassword);
            sph.DefineSqlParameter("@SmtpPreferredEncoding", SqlDbType.NVarChar, 20, ParameterDirection.Input, smtpPreferredEncoding);
            sph.DefineSqlParameter("@SmtpRequiresAuth", SqlDbType.Bit, ParameterDirection.Input, smtpRequiresAuth);
            sph.DefineSqlParameter("@SmtpUseSsl", SqlDbType.Bit, ParameterDirection.Input, smtpUseSsl);
            sph.DefineSqlParameter("@RequireApprovalBeforeLogin", SqlDbType.Bit, ParameterDirection.Input, requireApprovalBeforeLogin);
            sph.DefineSqlParameter("@IsDataProtected", SqlDbType.Bit, ParameterDirection.Input, isDataProtected);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);

            sph.DefineSqlParameter("@RequireConfirmedPhone", SqlDbType.Bit, ParameterDirection.Input, requireConfirmedPhone);
            sph.DefineSqlParameter("@DefaultEmailFromAlias", SqlDbType.NVarChar, 100, ParameterDirection.Input, defaultEmailFromAlias);
            sph.DefineSqlParameter("@AccountApprovalEmailCsv", SqlDbType.NVarChar, -1, ParameterDirection.Input, accountApprovalEmailCsv);
            sph.DefineSqlParameter("@DkimPublicKey", SqlDbType.NVarChar, 100, ParameterDirection.Input, dkimPublicKey);
            sph.DefineSqlParameter("@DkimPrivateKey", SqlDbType.NVarChar, -1, ParameterDirection.Input, dkimPrivateKey);
            sph.DefineSqlParameter("@DkimDomain", SqlDbType.NVarChar, 255, ParameterDirection.Input, dkimDomain);
            sph.DefineSqlParameter("@DkimSelector", SqlDbType.NVarChar, 128, ParameterDirection.Input, dkimSelector);
            sph.DefineSqlParameter("@SignEmailWithDkim", SqlDbType.Bit, ParameterDirection.Input, signEmailWithDkim);
            sph.DefineSqlParameter("@OidConnectAppId", SqlDbType.NVarChar, 100, ParameterDirection.Input, oidConnectAppId);
            sph.DefineSqlParameter("@OidConnectAppSecret", SqlDbType.NVarChar, -1, ParameterDirection.Input, oidConnectAppSecret);
            sph.DefineSqlParameter("@SmsClientId", SqlDbType.NVarChar, 100, ParameterDirection.Input, smsClientId);
            sph.DefineSqlParameter("@SmsSecureToken", SqlDbType.NVarChar, -1, ParameterDirection.Input, smsSecureToken);
            sph.DefineSqlParameter("@SmsFrom", SqlDbType.NVarChar, 100, ParameterDirection.Input, smsFrom);



            //-9 + 13 = 4


            object result = await sph.ExecuteScalarAsync(cancellationToken);
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

            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Sites_Update", 
                79);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SiteName", SqlDbType.NVarChar, 128, ParameterDirection.Input, siteName);
            sph.DefineSqlParameter("@Skin", SqlDbType.NVarChar, 100, ParameterDirection.Input, skin);
            sph.DefineSqlParameter("@AllowNewRegistration", SqlDbType.Bit, ParameterDirection.Input, allowNewRegistration);
            sph.DefineSqlParameter("@UseSecureRegistration", SqlDbType.Bit, ParameterDirection.Input, useSecureRegistration);
            sph.DefineSqlParameter("@IsServerAdminSite", SqlDbType.Bit, ParameterDirection.Input, isServerAdminSite);
            sph.DefineSqlParameter("@UseLdapAuth", SqlDbType.Bit, ParameterDirection.Input, useLdapAuth);
            sph.DefineSqlParameter("@AutoCreateLDAPUserOnFirstLogin", SqlDbType.Bit, ParameterDirection.Input, autoCreateLdapUserOnFirstLogin);
            sph.DefineSqlParameter("@LdapServer", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapServer);
            sph.DefineSqlParameter("@LdapPort", SqlDbType.Int, ParameterDirection.Input, ldapPort);
            sph.DefineSqlParameter("@LdapRootDN", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapRootDN);
            sph.DefineSqlParameter("@LdapUserDNKey", SqlDbType.NVarChar, 10, ParameterDirection.Input, ldapUserDNKey);
            sph.DefineSqlParameter("@UseEmailForLogin", SqlDbType.Bit, ParameterDirection.Input, useEmailForLogin);
            sph.DefineSqlParameter("@ReallyDeleteUsers", SqlDbType.Bit, ParameterDirection.Input, reallyDeleteUsers);  
            sph.DefineSqlParameter("@LdapDomain", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapDomain);
            sph.DefineSqlParameter("@RecaptchaPrivateKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, recaptchaPrivateKey);
            sph.DefineSqlParameter("@RecaptchaPublicKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, recaptchaPublicKey);
            sph.DefineSqlParameter("@DisableDbAuth", SqlDbType.Bit, ParameterDirection.Input, disableDbAuth);
            sph.DefineSqlParameter("@RequiresQuestionAndAnswer", SqlDbType.Bit, ParameterDirection.Input, requiresQuestionAndAnswer);
            sph.DefineSqlParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int, ParameterDirection.Input, maxInvalidPasswordAttempts);
            sph.DefineSqlParameter("@MinRequiredPasswordLength", SqlDbType.Int, ParameterDirection.Input, minRequiredPasswordLength);
            sph.DefineSqlParameter("@DefaultEmailFromAddress", SqlDbType.NVarChar, 100, ParameterDirection.Input, defaultEmailFromAddress);
            sph.DefineSqlParameter("@AllowDbFallbackWithLdap", SqlDbType.Bit, ParameterDirection.Input, allowDbFallbackWithLdap);
            sph.DefineSqlParameter("@EmailLdapDbFallback", SqlDbType.Bit, ParameterDirection.Input, emailLdapDbFallback);
            sph.DefineSqlParameter("@AllowPersistentLogin", SqlDbType.Bit, ParameterDirection.Input, allowPersistentLogin);
            sph.DefineSqlParameter("@CaptchaOnLogin", SqlDbType.Bit, ParameterDirection.Input, captchaOnLogin);
            sph.DefineSqlParameter("@CaptchaOnRegistration", SqlDbType.Bit, ParameterDirection.Input, captchaOnRegistration);
            sph.DefineSqlParameter("@SiteIsClosed", SqlDbType.Bit, ParameterDirection.Input, siteIsClosed);
            sph.DefineSqlParameter("@SiteIsClosedMessage", SqlDbType.NVarChar, -1, ParameterDirection.Input, siteIsClosedMessage);
            sph.DefineSqlParameter("@PrivacyPolicy", SqlDbType.NVarChar, -1, ParameterDirection.Input, privacyPolicy);
            sph.DefineSqlParameter("@TimeZoneId", SqlDbType.NVarChar, 50, ParameterDirection.Input, timeZoneId);
            sph.DefineSqlParameter("@GoogleAnalyticsProfileId", SqlDbType.NVarChar, 25, ParameterDirection.Input, googleAnalyticsProfileId);
            sph.DefineSqlParameter("@CompanyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, companyName);
            sph.DefineSqlParameter("@CompanyStreetAddress", SqlDbType.NVarChar, 250, ParameterDirection.Input, companyStreetAddress);
            sph.DefineSqlParameter("@CompanyStreetAddress2", SqlDbType.NVarChar, 250, ParameterDirection.Input, companyStreetAddress2);
            sph.DefineSqlParameter("@CompanyRegion", SqlDbType.NVarChar, 200, ParameterDirection.Input, companyRegion);
            sph.DefineSqlParameter("@CompanyLocality", SqlDbType.NVarChar, 200, ParameterDirection.Input, companyLocality);
            sph.DefineSqlParameter("@CompanyCountry", SqlDbType.NVarChar, 10, ParameterDirection.Input, companyCountry);
            sph.DefineSqlParameter("@CompanyPostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, companyPostalCode);
            sph.DefineSqlParameter("@CompanyPublicEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, companyPublicEmail);
            sph.DefineSqlParameter("@CompanyPhone", SqlDbType.NVarChar, 20, ParameterDirection.Input, companyPhone);
            sph.DefineSqlParameter("@CompanyFax", SqlDbType.NVarChar, 20, ParameterDirection.Input, companyFax);
            sph.DefineSqlParameter("@FacebookAppId", SqlDbType.NVarChar, 100, ParameterDirection.Input, facebookAppId);
            sph.DefineSqlParameter("@FacebookAppSecret", SqlDbType.NVarChar, -1, ParameterDirection.Input, facebookAppSecret);
            sph.DefineSqlParameter("@GoogleClientId", SqlDbType.NVarChar, 100, ParameterDirection.Input, googleClientId);
            sph.DefineSqlParameter("@GoogleClientSecret", SqlDbType.NVarChar, -1, ParameterDirection.Input, googleClientSecret);
            sph.DefineSqlParameter("@TwitterConsumerKey", SqlDbType.NVarChar, 100, ParameterDirection.Input, twitterConsumerKey);
            sph.DefineSqlParameter("@TwitterConsumerSecret", SqlDbType.NVarChar, -1, ParameterDirection.Input, twitterConsumerSecret);
            sph.DefineSqlParameter("@MicrosoftClientId", SqlDbType.NVarChar, 100, ParameterDirection.Input, microsoftClientId);
            sph.DefineSqlParameter("@MicrosoftClientSecret", SqlDbType.NVarChar, -1, ParameterDirection.Input, microsoftClientSecret);
            sph.DefineSqlParameter("@PreferredHostName", SqlDbType.NVarChar, 250, ParameterDirection.Input, preferredHostName);
            sph.DefineSqlParameter("@SiteFolderName", SqlDbType.NVarChar, 50, ParameterDirection.Input, siteFolderName);
            sph.DefineSqlParameter("@AddThisDotComUsername", SqlDbType.NVarChar, 50, ParameterDirection.Input, addThisDotComUsername);
            sph.DefineSqlParameter("@LoginInfoTop", SqlDbType.NVarChar, -1, ParameterDirection.Input, loginInfoTop);
            sph.DefineSqlParameter("@LoginInfoBottom", SqlDbType.NVarChar, -1, ParameterDirection.Input, loginInfoBottom);
            sph.DefineSqlParameter("@RegistrationAgreement", SqlDbType.NVarChar, -1, ParameterDirection.Input, registrationAgreement);
            sph.DefineSqlParameter("@RegistrationPreamble", SqlDbType.NVarChar, -1, ParameterDirection.Input, registrationPreamble);
            sph.DefineSqlParameter("@SmtpServer", SqlDbType.NVarChar, 200, ParameterDirection.Input, smtpServer);
            sph.DefineSqlParameter("@SmtpPort", SqlDbType.Int, ParameterDirection.Input, smtpPort);
            sph.DefineSqlParameter("@SmtpUser", SqlDbType.NVarChar, 500, ParameterDirection.Input, smtpUser);
            sph.DefineSqlParameter("@SmtpPassword", SqlDbType.NVarChar, -1, ParameterDirection.Input, smtpPassword);
            sph.DefineSqlParameter("@SmtpPreferredEncoding", SqlDbType.NVarChar, 20, ParameterDirection.Input, smtpPreferredEncoding);
            sph.DefineSqlParameter("@SmtpRequiresAuth", SqlDbType.Bit, ParameterDirection.Input, smtpRequiresAuth);
            sph.DefineSqlParameter("@SmtpUseSsl", SqlDbType.Bit, ParameterDirection.Input, smtpUseSsl);
            sph.DefineSqlParameter("@RequireApprovalBeforeLogin", SqlDbType.Bit, ParameterDirection.Input, requireApprovalBeforeLogin);
            sph.DefineSqlParameter("@IsDataProtected", SqlDbType.Bit, ParameterDirection.Input, isDataProtected);

            sph.DefineSqlParameter("@RequireConfirmedPhone", SqlDbType.Bit, ParameterDirection.Input, requireConfirmedPhone);
            sph.DefineSqlParameter("@DefaultEmailFromAlias", SqlDbType.NVarChar, 100, ParameterDirection.Input, defaultEmailFromAlias);
            sph.DefineSqlParameter("@AccountApprovalEmailCsv", SqlDbType.NVarChar, -1, ParameterDirection.Input, accountApprovalEmailCsv);
            sph.DefineSqlParameter("@DkimPublicKey", SqlDbType.NVarChar, 100, ParameterDirection.Input, dkimPublicKey);
            sph.DefineSqlParameter("@DkimPrivateKey", SqlDbType.NVarChar, -1, ParameterDirection.Input, dkimPrivateKey);
            sph.DefineSqlParameter("@DkimDomain", SqlDbType.NVarChar, 255, ParameterDirection.Input, dkimDomain);
            sph.DefineSqlParameter("@DkimSelector", SqlDbType.NVarChar, 128, ParameterDirection.Input, dkimSelector);
            sph.DefineSqlParameter("@SignEmailWithDkim", SqlDbType.Bit, ParameterDirection.Input, signEmailWithDkim);
            sph.DefineSqlParameter("@OidConnectAppId", SqlDbType.NVarChar, 100, ParameterDirection.Input, oidConnectAppId);
            sph.DefineSqlParameter("@OidConnectAppSecret", SqlDbType.NVarChar, -1, ParameterDirection.Input, oidConnectAppSecret);
            sph.DefineSqlParameter("@SmsClientId", SqlDbType.NVarChar, 100, ParameterDirection.Input, smsClientId);
            sph.DefineSqlParameter("@SmsSecureToken", SqlDbType.NVarChar, -1, ParameterDirection.Input, smsSecureToken);
            sph.DefineSqlParameter("@SmsFrom", SqlDbType.NVarChar, 100, ParameterDirection.Input, smsFrom);

            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
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
            string passwordStrengthRegularExpression,
            string defaultEmailFromAddress
            )
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Sites_UpdateExtendedProperties", 
                12);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@AllowPasswordRetrieval", SqlDbType.Bit, ParameterDirection.Input, allowPasswordRetrieval);
            sph.DefineSqlParameter("@AllowPasswordReset", SqlDbType.Bit, ParameterDirection.Input, allowPasswordReset);
            sph.DefineSqlParameter("@RequiresQuestionAndAnswer", SqlDbType.Bit, ParameterDirection.Input, requiresQuestionAndAnswer);
            sph.DefineSqlParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int, ParameterDirection.Input, maxInvalidPasswordAttempts);
            sph.DefineSqlParameter("@PasswordAttemptWindowMinutes", SqlDbType.Int, ParameterDirection.Input, passwordAttemptWindowMinutes);
            sph.DefineSqlParameter("@RequiresUniqueEmail", SqlDbType.Bit, ParameterDirection.Input, requiresUniqueEmail);
            sph.DefineSqlParameter("@PasswordFormat", SqlDbType.Int, ParameterDirection.Input, passwordFormat);
            sph.DefineSqlParameter("@MinRequiredPasswordLength", SqlDbType.Int, ParameterDirection.Input, minRequiredPasswordLength);
            sph.DefineSqlParameter("@MinRequiredNonAlphanumericCharacters", SqlDbType.Int, ParameterDirection.Input, minRequiredNonAlphanumericCharacters);
            sph.DefineSqlParameter("@PasswordStrengthRegularExpression", SqlDbType.NVarChar, -1, ParameterDirection.Input, passwordStrengthRegularExpression);
            sph.DefineSqlParameter("@DefaultEmailFromAddress", SqlDbType.NVarChar, 100, ParameterDirection.Input, defaultEmailFromAddress);
            int rowsAffected = sph.ExecuteNonQuery();
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

            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Sites_UpdateRelatedSiteSecurity", 
                24);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@AllowNewRegistration", SqlDbType.Bit, ParameterDirection.Input, allowNewRegistration);
            sph.DefineSqlParameter("@UseSecureRegistration", SqlDbType.Bit, ParameterDirection.Input, useSecureRegistration);
            sph.DefineSqlParameter("@UseLdapAuth", SqlDbType.Bit, ParameterDirection.Input, useLdapAuth);
            sph.DefineSqlParameter("@AutoCreateLDAPUserOnFirstLogin", SqlDbType.Bit, ParameterDirection.Input, autoCreateLdapUserOnFirstLogin);
            sph.DefineSqlParameter("@LdapServer", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapServer);
            sph.DefineSqlParameter("@LdapDomain", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapDomain);
            sph.DefineSqlParameter("@LdapPort", SqlDbType.Int, ParameterDirection.Input, ldapPort);
            sph.DefineSqlParameter("@LdapRootDN", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapRootDN);
            sph.DefineSqlParameter("@LdapUserDNKey", SqlDbType.NVarChar, 10, ParameterDirection.Input, ldapUserDNKey);
            sph.DefineSqlParameter("@AllowUserFullNameChange", SqlDbType.Bit, ParameterDirection.Input, allowUserFullNameChange);
            sph.DefineSqlParameter("@UseEmailForLogin", SqlDbType.Bit, ParameterDirection.Input, useEmailForLogin);
            sph.DefineSqlParameter("@AllowOpenIDAuth", SqlDbType.Bit, ParameterDirection.Input, allowOpenIdAuth);
            sph.DefineSqlParameter("@AllowWindowsLiveAuth", SqlDbType.Bit, ParameterDirection.Input, allowWindowsLiveAuth);
            sph.DefineSqlParameter("@AllowPasswordRetrieval", SqlDbType.Bit, ParameterDirection.Input, allowPasswordRetrieval);
            sph.DefineSqlParameter("@AllowPasswordReset", SqlDbType.Bit, ParameterDirection.Input, allowPasswordReset);
            sph.DefineSqlParameter("@RequiresQuestionAndAnswer", SqlDbType.Bit, ParameterDirection.Input, requiresQuestionAndAnswer);
            sph.DefineSqlParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int, ParameterDirection.Input, maxInvalidPasswordAttempts);
            sph.DefineSqlParameter("@PasswordAttemptWindowMinutes", SqlDbType.Int, ParameterDirection.Input, passwordAttemptWindowMinutes);
            sph.DefineSqlParameter("@RequiresUniqueEmail", SqlDbType.Bit, ParameterDirection.Input, requiresUniqueEmail);
            sph.DefineSqlParameter("@PasswordFormat", SqlDbType.Int, ParameterDirection.Input, passwordFormat);
            sph.DefineSqlParameter("@MinRequiredPasswordLength", SqlDbType.Int, ParameterDirection.Input, minRequiredPasswordLength);
            sph.DefineSqlParameter("@MinReqNonAlphaChars", SqlDbType.Int, ParameterDirection.Input, minReqNonAlphaChars);
            sph.DefineSqlParameter("@PwdStrengthRegex", SqlDbType.NVarChar, -1, ParameterDirection.Input, pwdStrengthRegex);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public bool UpdateRelatedSitesWindowsLive(
            int siteId,
            string windowsLiveAppId,
            string windowsLiveKey
            )
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Sites_SyncRelatedSitesWinLive", 
                3);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@WindowsLiveAppID", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveAppId);
            sph.DefineSqlParameter("@WindowsLiveKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveKey);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }




        public async Task<bool> Delete(
            int siteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_Sites_Delete", 
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return (rowsAffected > -1);
        }

        public async Task<DbDataReader> GetSiteList(CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Sites_SelectAll", 
                0);

            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public async Task<DbDataReader> GetSite(string hostName, CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Sites_SelectOneByHost", 
                1);

            sph.DefineSqlParameter("@HostName", SqlDbType.NVarChar, 50, ParameterDirection.Input, hostName);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }

        public DbDataReader GetSiteNonAsync(string hostName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Sites_SelectOneByHost", 
                1);

            sph.DefineSqlParameter("@HostName", SqlDbType.NVarChar, 50, ParameterDirection.Input, hostName);
            return sph.ExecuteReader();

        }


        //public void AddFeature(Guid siteGuid, Guid featureGuid)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_SiteModuleDefinitions_Insert", 
        //        2);

        //    sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
        //    sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);

        //    sph.ExecuteNonQuery();

        //}

        //public void RemoveFeature(Guid siteGuid, Guid featureGuid)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        writeConnectionString, 
        //        "mp_SiteModuleDefinitions_Delete", 
        //        2);

        //    sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
        //    sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
        //    sph.ExecuteNonQuery();
        //}


        public async Task<DbDataReader> GetSite(
            int siteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Sites_SelectOne", 
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public DbDataReader GetSiteNonAsync(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Sites_SelectOne", 
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public async Task<DbDataReader> GetSite(
            Guid siteGuid,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Sites_SelectOneByGuid", 
                1);

            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public DbDataReader GetSiteNonAsync(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString,
                "mp_Sites_SelectOneByGuid",
                1);

            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();
        }



        //public DbDataReader GetPageListForAdmin(int siteId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(
        //        logFactory,
        //        readConnectionString, 
        //        "mp_Pages_SelectList", 
        //        1);

        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    return sph.ExecuteReader();
        //}

        public async Task<int> GetHostCount(CancellationToken cancellationToken)
        {
            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_SiteHosts_GetCount",
                null,
                cancellationToken);

            return Convert.ToInt32(result);

        }

        public async Task<DbDataReader> GetAllHosts(CancellationToken cancellationToken)
        {
            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_SiteHosts_SelectAll",
                null,
                cancellationToken);

        }

        public DbDataReader GetAllHostsNonAsync()
        {
            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_SiteHosts_SelectAll",
                null);

        }

        public async Task<DbDataReader> GetPageHosts(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_SiteHosts_SelectPage", 
                2);

            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }

        public async Task<DbDataReader> GetHostList(
            int siteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_SiteHosts_Select", 
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public async Task<DbDataReader> GetHost(
            string hostName,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_SiteHosts_SelectOneByHost", 
                1);

            sph.DefineSqlParameter("@HostName", SqlDbType.NVarChar, 255, ParameterDirection.Input, hostName);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public async Task<DbDataReader> GetHost(
            int hostId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_SiteHosts_SelectOne", 
                1);

            sph.DefineSqlParameter("@HostID", SqlDbType.Int, ParameterDirection.Input, hostId);
            return await sph.ExecuteReaderAsync(cancellationToken);
        }

        public async Task<bool> AddHost(
            Guid siteGuid, 
            int siteId, 
            string hostName,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_SiteHosts_Insert", 
                3);

            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@HostName", SqlDbType.NVarChar, 255, ParameterDirection.Input, hostName);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteHost(
            int hostId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_SiteHosts_Delete", 
                1);

            sph.DefineSqlParameter("@HostID", SqlDbType.Int, ParameterDirection.Input, hostId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteHostsBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString,
                "mp_SiteHosts_DeleteBySite",
                1);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = await sph.ExecuteNonQueryAsync(cancellationToken);
            return rowsAffected > 0;
        }


        public async Task<int> CountOtherSites(
            int currentSiteId,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Sites_CountOtherSites", 
                1);

            sph.DefineSqlParameter("@CurrentSiteID", SqlDbType.Int, ParameterDirection.Input, currentSiteId);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(result);

        }

        public async Task<DbDataReader> GetPageOfOtherSites(
            int currentSiteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_Sites_SelectPageOtherSites", 
                3);

            sph.DefineSqlParameter("@CurrentSiteID", SqlDbType.Int, ParameterDirection.Input, currentSiteId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync(cancellationToken);

        }


        public async Task<int> GetSiteIdByHostName(
            string hostName,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_SiteHosts_SelectSiteIdByHost", 
                1);

            sph.DefineSqlParameter("@HostName", SqlDbType.NVarChar, 255, ParameterDirection.Input, hostName);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(result);

        }

        public async Task<int> GetSiteIdByFolder(
            string folderName,
            CancellationToken cancellationToken)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_SiteFolders_SelectSiteIdByFolder", 
                1);

            sph.DefineSqlParameter("@FolderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, folderName);
            object result = await sph.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(result);

        }

        public int GetSiteIdByFolderNonAsync(string folderName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_SiteFolders_SelectSiteIdByFolder", 
                1);

            sph.DefineSqlParameter("@FolderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, folderName);
            object result = sph.ExecuteScalar();
            return Convert.ToInt32(result);

        }

    }
}
