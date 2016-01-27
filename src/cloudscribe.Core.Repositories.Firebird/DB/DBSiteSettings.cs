// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-27
//

using cloudscribe.DbHelpers;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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

            // possibly will change this later to have FirebirdClientFactory/DbProviderFactory injected
            AdoHelper = new FirebirdHelper(FirebirdClientFactory.Instance);
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;
        private FirebirdHelper AdoHelper;

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
            
            FbParameter[] arParams = new FbParameter[80];

            arParams[0] = new FbParameter(":SiteGuid", FbDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new FbParameter(":SiteName", FbDbType.VarChar, 255);
            arParams[1].Value = siteName;

            arParams[2] = new FbParameter(":Skin", FbDbType.VarChar, 100);
            arParams[2].Value = skin;
            
            arParams[3] = new FbParameter(":AllowNewRegistration", FbDbType.Integer);
            arParams[3].Value = allowNewRegistration ? 1 : 0;

            arParams[4] = new FbParameter(":UseSecureRegistration", FbDbType.Integer);
            arParams[4].Value = useSecureRegistration ? 1 : 0;
            
            arParams[5] = new FbParameter(":IsServerAdminSite", FbDbType.Integer);
            arParams[5].Value = isServerAdminSite ? 1 : 0;

            arParams[6] = new FbParameter(":UseLdapAuth", FbDbType.Integer);
            arParams[6].Value = useLdapAuth ? 1 : 0;

            arParams[7] = new FbParameter(":AutoCreateLDAPUserOnFirstLogin", FbDbType.Integer);
            arParams[7].Value = autoCreateLdapUserOnFirstLogin ? 1 : 0;

            arParams[8] = new FbParameter(":LdapServer", FbDbType.VarChar, 255);
            arParams[8].Value = ldapServer;

            arParams[9] = new FbParameter(":LdapPort", FbDbType.Integer);
            arParams[9].Value = ldapPort;

            arParams[10] = new FbParameter(":LdapDomain", FbDbType.VarChar, 255);
            arParams[10].Value = ldapDomain;

            arParams[11] = new FbParameter(":LdapRootDN", FbDbType.VarChar, 255);
            arParams[11].Value = ldapRootDN;

            arParams[12] = new FbParameter(":LdapUserDNKey", FbDbType.VarChar, 255);
            arParams[12].Value = ldapUserDNKey;

            arParams[13] = new FbParameter(":ReallyDeleteUsers", FbDbType.SmallInt);
            arParams[13].Value = reallyDeleteUsers ? 1 : 0;

            arParams[14] = new FbParameter(":UseEmailForLogin", FbDbType.SmallInt);
            arParams[14].Value = useEmailForLogin ? 1 : 0;
            
            arParams[15] = new FbParameter(":RequiresQuestionAndAnswer", FbDbType.Integer);
            arParams[15].Value = requiresQuestionAndAnswer;

            arParams[16] = new FbParameter(":MaxInvalidPasswordAttempts", FbDbType.Integer);
            arParams[16].Value = maxInvalidPasswordAttempts;
            
            arParams[17] = new FbParameter(":MINREQUIREDPASSWORDLENGTH", FbDbType.Integer);
            arParams[17].Value = minRequiredPasswordLength;
            
            arParams[18] = new FbParameter(":DEFAULTEMAILFROMADDRESS", FbDbType.VarChar, 100);
            arParams[18].Value = "noreply@yoursite.com";
            
            arParams[19] = new FbParameter(":RecaptchaPrivateKey", FbDbType.VarChar, 255);
            arParams[19].Value = recaptchaPrivateKey;

            arParams[20] = new FbParameter(":RecaptchaPublicKey", FbDbType.VarChar, 255);
            arParams[20].Value = recaptchaPublicKey;
            
            arParams[21] = new FbParameter(":DisableDbAuth", FbDbType.SmallInt);
            arParams[21].Value = disableDbAuth ? 1 : 0; 

            arParams[22] = new FbParameter(":AllowDbFallbackWithLdap", FbDbType.SmallInt);
            arParams[22].Value = allowDbFallbackWithLdap ? 1 : 0;

            arParams[23] = new FbParameter(":EmailLdapDbFallback", FbDbType.SmallInt);
            arParams[23].Value = emailLdapDbFallback ? 1 : 0;

            arParams[24] = new FbParameter(":AllowPersistentLogin", FbDbType.SmallInt);
            arParams[24].Value = allowPersistentLogin ? 1 : 0;

            arParams[25] = new FbParameter(":CaptchaOnLogin", FbDbType.SmallInt);
            arParams[25].Value = captchaOnLogin ? 1 : 0;

            arParams[26] = new FbParameter(":CaptchaOnRegistration", FbDbType.SmallInt);
            arParams[26].Value = captchaOnRegistration ? 1 : 0;

            arParams[27] = new FbParameter(":SiteIsClosed", FbDbType.SmallInt);
            arParams[27].Value = siteIsClosed ? 1 : 0;

            arParams[28] = new FbParameter(":SiteIsClosedMessage", FbDbType.VarChar);
            arParams[28].Value = siteIsClosedMessage;

            arParams[29] = new FbParameter(":PrivacyPolicy", FbDbType.VarChar);
            arParams[29].Value = privacyPolicy;

            arParams[30] = new FbParameter(":TimeZoneId", FbDbType.VarChar, 50);
            arParams[30].Value = timeZoneId;

            arParams[31] = new FbParameter(":GoogleAnalyticsProfileId", FbDbType.VarChar, 25);
            arParams[31].Value = timeZoneId;

            arParams[32] = new FbParameter(":CompanyName", FbDbType.VarChar, 255);
            arParams[32].Value = companyName;

            arParams[33] = new FbParameter(":CompanyStreetAddress", FbDbType.VarChar, 250);
            arParams[33].Value = companyStreetAddress;

            arParams[34] = new FbParameter(":CompanyStreetAddress2", FbDbType.VarChar, 250);
            arParams[34].Value = companyStreetAddress2;

            arParams[35] = new FbParameter(":CompanyRegion", FbDbType.VarChar, 200);
            arParams[35].Value = companyRegion;

            arParams[36] = new FbParameter(":CompanyLocality", FbDbType.VarChar, 200);
            arParams[36].Value = companyLocality;

            arParams[37] = new FbParameter(":CompanyCountry", FbDbType.VarChar, 10);
            arParams[37].Value = companyCountry;

            arParams[38] = new FbParameter(":CompanyPostalCode", FbDbType.VarChar, 20);
            arParams[38].Value = companyPostalCode;

            arParams[39] = new FbParameter(":CompanyPublicEmail", FbDbType.VarChar, 100);
            arParams[39].Value = companyPublicEmail;

            arParams[40] = new FbParameter(":CompanyPhone", FbDbType.VarChar, 20);
            arParams[40].Value = companyPhone;

            arParams[41] = new FbParameter(":CompanyFax", FbDbType.VarChar, 20);
            arParams[41].Value = companyFax;

            arParams[42] = new FbParameter(":FacebookAppId", FbDbType.VarChar, 100);
            arParams[42].Value = facebookAppId;

            arParams[43] = new FbParameter(":FacebookAppSecret", FbDbType.VarChar);
            arParams[43].Value = facebookAppSecret;

            arParams[44] = new FbParameter(":GoogleClientId", FbDbType.VarChar, 100);
            arParams[44].Value = googleClientId;

            arParams[45] = new FbParameter(":GoogleClientSecret", FbDbType.VarChar);
            arParams[45].Value = googleClientSecret;

            arParams[46] = new FbParameter(":TwitterConsumerKey", FbDbType.VarChar, 100);
            arParams[46].Value = twitterConsumerKey;

            arParams[47] = new FbParameter(":TwitterConsumerSecret", FbDbType.VarChar);
            arParams[47].Value = twitterConsumerSecret;

            arParams[48] = new FbParameter(":MicrosoftClientId", FbDbType.VarChar, 100);
            arParams[48].Value = microsoftClientId;

            arParams[49] = new FbParameter(":MicrosoftClientSecret", FbDbType.VarChar);
            arParams[49].Value = microsoftClientSecret;

            arParams[50] = new FbParameter(":PreferredHostName", FbDbType.VarChar, 250);
            arParams[50].Value = preferredHostName;

            arParams[51] = new FbParameter(":SiteFolderName", FbDbType.VarChar, 50);
            arParams[51].Value = siteFolderName;

            arParams[52] = new FbParameter(":AddThisDotComUsername", FbDbType.VarChar, 50);
            arParams[52].Value = addThisDotComUsername;

            arParams[53] = new FbParameter(":LoginInfoTop", FbDbType.VarChar);
            arParams[53].Value = loginInfoTop;

            arParams[54] = new FbParameter(":LoginInfoBottom", FbDbType.VarChar);
            arParams[54].Value = loginInfoBottom;

            arParams[55] = new FbParameter(":RegistrationAgreement", FbDbType.VarChar);
            arParams[55].Value = registrationAgreement;

            arParams[56] = new FbParameter(":RegistrationPreamble", FbDbType.VarChar);
            arParams[56].Value = registrationPreamble;

            arParams[57] = new FbParameter(":SMTPServer", FbDbType.VarChar, 200);
            arParams[57].Value = smtpServer;

            arParams[58] = new FbParameter(":SMTPPort", FbDbType.Integer);
            arParams[58].Value = smtpPort;

            arParams[59] = new FbParameter(":SMTPUser", FbDbType.VarChar, 500);
            arParams[59].Value = smtpUser;

            arParams[60] = new FbParameter(":SMTPPassword", FbDbType.VarChar);
            arParams[60].Value = smtpPassword;

            arParams[61] = new FbParameter(":SMTPPreferredEncoding", FbDbType.VarChar, 500);
            arParams[61].Value = smtpPreferredEncoding;

            arParams[62] = new FbParameter(":SMTPRequiresAuth", FbDbType.SmallInt);
            arParams[62].Value = smtpRequiresAuth ? 1 : 0;

            arParams[63] = new FbParameter(":SMTPUseSsl", FbDbType.SmallInt);
            arParams[63].Value = smtpUseSsl ? 1 : 0;

            arParams[64] = new FbParameter(":RequireApprovalBeforeLogin", FbDbType.SmallInt);
            arParams[64].Value = requireApprovalBeforeLogin ? 1 : 0;

            arParams[65] = new FbParameter(":IsDataProtected", FbDbType.SmallInt);
            arParams[65].Value = isDataProtected ? 1 : 0;

            arParams[66] = new FbParameter(":CreatedUtc", FbDbType.TimeStamp);
            arParams[66].Value = createdUtc;

            arParams[67] = new FbParameter(":RequireConfirmedPhone", FbDbType.SmallInt);
            arParams[67].Value = requireConfirmedPhone ? 1 : 0;

            arParams[68] = new FbParameter(":DefaultEmailFromAlias", FbDbType.VarChar, 100);
            arParams[68].Value = defaultEmailFromAlias;

            arParams[69] = new FbParameter(":AccountApprovalEmailCsv", FbDbType.VarChar);
            arParams[69].Value = accountApprovalEmailCsv;

            arParams[70] = new FbParameter(":DkimPublicKey", FbDbType.VarChar);
            arParams[70].Value = dkimPublicKey;

            arParams[71] = new FbParameter(":DkimPrivateKey", FbDbType.VarChar);
            arParams[71].Value = dkimPrivateKey;

            arParams[72] = new FbParameter(":DkimDomain", FbDbType.VarChar, 255);
            arParams[72].Value = dkimDomain;

            arParams[73] = new FbParameter(":DkimSelector", FbDbType.VarChar, 128);
            arParams[73].Value = dkimSelector;

            arParams[74] = new FbParameter(":SignEmailWithDkim", FbDbType.SmallInt);
            arParams[74].Value = signEmailWithDkim ? 1 : 0;

            arParams[75] = new FbParameter(":OidConnectAppId", FbDbType.VarChar, 255);
            arParams[75].Value = oidConnectAppId;

            arParams[76] = new FbParameter(":OidConnectAppSecret", FbDbType.VarChar);
            arParams[76].Value = oidConnectAppSecret;

            arParams[77] = new FbParameter(":SmsClientId", FbDbType.VarChar, 255);
            arParams[77].Value = smsClientId;

            arParams[78] = new FbParameter(":SmsSecureToken", FbDbType.VarChar);
            arParams[78].Value = smsSecureToken;

            arParams[79] = new FbParameter(":SmsFrom", FbDbType.VarChar, 255);
            arParams[79].Value = smsFrom;


            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_SITES_INSERT ("
                + AdoHelper.GetParamString(arParams.Length) + ")",
                true,
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
            sqlCommand.Append("SET SiteName = @SiteName, ");
            sqlCommand.Append("IsServerAdminSite = @IsServerAdminSite, ");
            sqlCommand.Append("Skin = @Skin, ");
            sqlCommand.Append("AllowNewRegistration = @AllowNewRegistration, ");  
            sqlCommand.Append("UseSecureRegistration = @UseSecureRegistration, ");
            sqlCommand.Append("UseLdapAuth = @UseLdapAuth, ");
            sqlCommand.Append("AutoCreateLDAPUserOnFirstLogin = @autoCreateLDAPUserOnFirstLogin, ");
            sqlCommand.Append("LdapServer = @LdapServer, ");
            sqlCommand.Append("LdapPort = @LdapPort, ");
            sqlCommand.Append("LdapDomain = @LdapDomain, ");
            sqlCommand.Append("LdapRootDN = @LdapRootDN, ");
            sqlCommand.Append("LdapUserDNKey = @LdapUserDNKey, ");
            sqlCommand.Append("UseEmailForLogin = @UseEmailForLogin, ");
            sqlCommand.Append("ReallyDeleteUsers = @ReallyDeleteUsers, ");
            sqlCommand.Append("RecaptchaPrivateKey = @RecaptchaPrivateKey, ");
            sqlCommand.Append("RecaptchaPublicKey = @RecaptchaPublicKey, ");
            sqlCommand.Append("DisableDbAuth = @DisableDbAuth, ");

            sqlCommand.Append("RequiresQuestionAndAnswer = @RequiresQuestionAndAnswer, ");
            sqlCommand.Append("MaxInvalidPasswordAttempts = @MaxInvalidPasswordAttempts, ");
            sqlCommand.Append("MinRequiredPasswordLength = @MinRequiredPasswordLength, ");
            sqlCommand.Append("DefaultEmailFromAddress = @DefaultEmailFromAddress, ");
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

            sqlCommand.Append(" WHERE SiteID = @SiteID ;");

            FbParameter[] arParams = new FbParameter[80];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SiteName", FbDbType.VarChar, 128);
            arParams[1].Value = siteName;

            arParams[2] = new FbParameter("@IsServerAdminSite", FbDbType.SmallInt);
            arParams[2].Value = isServerAdminSite ? 1 : 0;

            arParams[3] = new FbParameter("@Skin", FbDbType.VarChar, 100);
            arParams[3].Value = skin;
            
            arParams[4] = new FbParameter("@AllowNewRegistration", FbDbType.SmallInt);
            arParams[4].Value = allowNewRegistration ? 1 : 0;
            
            arParams[5] = new FbParameter("@UseSecureRegistration", FbDbType.SmallInt);
            arParams[5].Value = useSecureRegistration ? 1 : 0;
            
            arParams[6] = new FbParameter("@UseLdapAuth", FbDbType.SmallInt);
            arParams[6].Value = useLdapAuth ? 1 : 0;

            arParams[7] = new FbParameter("@AutoCreateLDAPUserOnFirstLogin", FbDbType.SmallInt);
            arParams[7].Value = autoCreateLdapUserOnFirstLogin ? 1 : 0;

            arParams[8] = new FbParameter("@LdapServer", FbDbType.VarChar, 255);
            arParams[8].Value = ldapServer;

            arParams[9] = new FbParameter("@LdapPort", FbDbType.Integer);
            arParams[9].Value = ldapPort;

            arParams[10] = new FbParameter("@LdapRootDN", FbDbType.VarChar, 255);
            arParams[10].Value = ldapRootDN;

            arParams[11] = new FbParameter("@LdapUserDNKey", FbDbType.VarChar, 10);
            arParams[11].Value = ldapUserDNKey;
            
            arParams[12] = new FbParameter("@UseEmailForLogin", FbDbType.SmallInt);
            arParams[12].Value = useEmailForLogin ? 1 : 0;

            arParams[13] = new FbParameter("@ReallyDeleteUsers", FbDbType.SmallInt);
            arParams[13].Value = reallyDeleteUsers ? 1 : 0;
            
            arParams[14] = new FbParameter("@LdapDomain", FbDbType.VarChar, 255);
            arParams[14].Value = ldapDomain;
            
            arParams[15] = new FbParameter("@RecaptchaPrivateKey", FbDbType.VarChar, 255);
            arParams[15].Value = recaptchaPrivateKey;

            arParams[16] = new FbParameter("@RecaptchaPublicKey", FbDbType.VarChar, 255);
            arParams[16].Value = recaptchaPublicKey;
            
            arParams[17] = new FbParameter("@DisableDbAuth", FbDbType.SmallInt);
            arParams[17].Value = disableDbAuth ? 1 : 0;

            arParams[18] = new FbParameter("@AllowDbFallbackWithLdap", FbDbType.SmallInt);
            arParams[18].Value = allowDbFallbackWithLdap ? 1 : 0;

            arParams[19] = new FbParameter("@EmailLdapDbFallback", FbDbType.SmallInt);
            arParams[19].Value = emailLdapDbFallback ? 1 : 0;

            arParams[20] = new FbParameter("@AllowPersistentLogin", FbDbType.SmallInt);
            arParams[20].Value = allowPersistentLogin ? 1 : 0;

            arParams[21] = new FbParameter("@CaptchaOnLogin", FbDbType.SmallInt);
            arParams[21].Value = captchaOnLogin ? 1 : 0;

            arParams[22] = new FbParameter("@CaptchaOnRegistration", FbDbType.SmallInt);
            arParams[22].Value = captchaOnRegistration ? 1 : 0;

            arParams[23] = new FbParameter("@SiteIsClosed", FbDbType.SmallInt);
            arParams[23].Value = siteIsClosed ? 1 : 0;

            arParams[24] = new FbParameter("@SiteIsClosedMessage", FbDbType.VarChar);
            arParams[24].Value = siteIsClosedMessage;

            arParams[25] = new FbParameter("@PrivacyPolicy", FbDbType.VarChar);
            arParams[25].Value = privacyPolicy;

            arParams[26] = new FbParameter("@TimeZoneId", FbDbType.VarChar, 50);
            arParams[26].Value = timeZoneId;

            arParams[27] = new FbParameter("@GoogleAnalyticsProfileId", FbDbType.VarChar, 25);
            arParams[27].Value = timeZoneId;

            arParams[28] = new FbParameter("@CompanyName", FbDbType.VarChar, 255);
            arParams[28].Value = companyName;

            arParams[29] = new FbParameter("@CompanyStreetAddress", FbDbType.VarChar, 250);
            arParams[29].Value = companyStreetAddress;

            arParams[30] = new FbParameter("@CompanyStreetAddress2", FbDbType.VarChar, 250);
            arParams[30].Value = companyStreetAddress2;

            arParams[31] = new FbParameter("@CompanyRegion", FbDbType.VarChar, 200);
            arParams[31].Value = companyRegion;

            arParams[32] = new FbParameter("@CompanyLocality", FbDbType.VarChar, 200);
            arParams[32].Value = companyLocality;

            arParams[33] = new FbParameter("@CompanyCountry", FbDbType.VarChar, 10);
            arParams[33].Value = companyCountry;

            arParams[34] = new FbParameter("@CompanyPostalCode", FbDbType.VarChar, 20);
            arParams[34].Value = companyPostalCode;

            arParams[35] = new FbParameter("@CompanyPublicEmail", FbDbType.VarChar, 100);
            arParams[35].Value = companyPublicEmail;

            arParams[36] = new FbParameter("@CompanyPhone", FbDbType.VarChar, 20);
            arParams[36].Value = companyPhone;

            arParams[37] = new FbParameter("@CompanyFax", FbDbType.VarChar, 20);
            arParams[37].Value = companyFax;

            arParams[38] = new FbParameter("@FacebookAppId", FbDbType.VarChar, 100);
            arParams[38].Value = facebookAppId;

            arParams[39] = new FbParameter("@FacebookAppSecret", FbDbType.VarChar);
            arParams[39].Value = facebookAppSecret;

            arParams[40] = new FbParameter("@GoogleClientId", FbDbType.VarChar, 100);
            arParams[40].Value = googleClientId;

            arParams[41] = new FbParameter("@GoogleClientSecret", FbDbType.VarChar);
            arParams[41].Value = googleClientSecret;

            arParams[42] = new FbParameter("@TwitterConsumerKey", FbDbType.VarChar, 100);
            arParams[42].Value = twitterConsumerKey;

            arParams[43] = new FbParameter("@TwitterConsumerSecret", FbDbType.VarChar);
            arParams[43].Value = twitterConsumerSecret;

            arParams[44] = new FbParameter("@MicrosoftClientId", FbDbType.VarChar, 100);
            arParams[44].Value = microsoftClientId;

            arParams[45] = new FbParameter("@MicrosoftClientSecret", FbDbType.VarChar);
            arParams[45].Value = microsoftClientSecret;

            arParams[46] = new FbParameter("@PreferredHostName", FbDbType.VarChar, 250);
            arParams[46].Value = preferredHostName;

            arParams[47] = new FbParameter("@SiteFolderName", FbDbType.VarChar, 50);
            arParams[47].Value = siteFolderName;

            arParams[48] = new FbParameter("@AddThisDotComUsername", FbDbType.VarChar, 50);
            arParams[48].Value = addThisDotComUsername;

            arParams[49] = new FbParameter("@LoginInfoTop", FbDbType.VarChar);
            arParams[49].Value = loginInfoTop;

            arParams[50] = new FbParameter("@LoginInfoBottom", FbDbType.VarChar);
            arParams[50].Value = loginInfoBottom;

            arParams[51] = new FbParameter("@RegistrationAgreement", FbDbType.VarChar);
            arParams[51].Value = registrationAgreement;

            arParams[52] = new FbParameter("@RegistrationPreamble", FbDbType.VarChar);
            arParams[52].Value = registrationPreamble;

            arParams[53] = new FbParameter("@SMTPServer", FbDbType.VarChar, 200);
            arParams[53].Value = smtpServer;

            arParams[54] = new FbParameter("@SMTPPort", FbDbType.Integer);
            arParams[54].Value = smtpPort;

            arParams[55] = new FbParameter("@SMTPUser", FbDbType.VarChar, 500);
            arParams[55].Value = smtpUser;

            arParams[56] = new FbParameter("@SMTPPassword", FbDbType.VarChar);
            arParams[56].Value = smtpPassword;

            arParams[57] = new FbParameter("@SMTPPreferredEncoding", FbDbType.VarChar, 500);
            arParams[57].Value = smtpPreferredEncoding;

            arParams[58] = new FbParameter("@SMTPRequiresAuth", FbDbType.SmallInt);
            arParams[58].Value = smtpRequiresAuth ? 1 : 0;

            arParams[59] = new FbParameter("@SMTPUseSsl", FbDbType.SmallInt);
            arParams[59].Value = smtpUseSsl ? 1 : 0;

            arParams[60] = new FbParameter("@RequireApprovalBeforeLogin", FbDbType.SmallInt);
            arParams[60].Value = requireApprovalBeforeLogin ? 1 : 0;

            arParams[61] = new FbParameter("@RequiresQuestionAndAnswer", FbDbType.SmallInt);
            arParams[61].Value = requiresQuestionAndAnswer ? 1 : 0;

            arParams[62] = new FbParameter("@MaxInvalidPasswordAttempts", FbDbType.Integer);
            arParams[62].Value = maxInvalidPasswordAttempts;
            
            arParams[63] = new FbParameter("@MinRequiredPasswordLength", FbDbType.Integer);
            arParams[63].Value = minRequiredPasswordLength;
            
            arParams[64] = new FbParameter("@DefaultEmailFromAddress", FbDbType.VarChar, 100);
            arParams[64].Value = defaultEmailFromAddress;

            arParams[65] = new FbParameter("@IsDataProtected", FbDbType.SmallInt);
            arParams[65].Value = isDataProtected ? 1 : 0;

            arParams[67] = new FbParameter("@RequireConfirmedPhone", FbDbType.SmallInt);
            arParams[67].Value = requireConfirmedPhone ? 1 : 0;

            arParams[68] = new FbParameter("@DefaultEmailFromAlias", FbDbType.VarChar, 100);
            arParams[68].Value = defaultEmailFromAlias;

            arParams[69] = new FbParameter("@AccountApprovalEmailCsv", FbDbType.VarChar);
            arParams[69].Value = accountApprovalEmailCsv;

            arParams[70] = new FbParameter("@DkimPublicKey", FbDbType.VarChar);
            arParams[70].Value = dkimPublicKey;

            arParams[71] = new FbParameter("@DkimPrivateKey", FbDbType.VarChar);
            arParams[71].Value = dkimPrivateKey;

            arParams[72] = new FbParameter("@DkimDomain", FbDbType.VarChar, 255);
            arParams[72].Value = dkimDomain;

            arParams[73] = new FbParameter("@DkimSelector", FbDbType.VarChar, 128);
            arParams[73].Value = dkimSelector;

            arParams[74] = new FbParameter("@SignEmailWithDkim", FbDbType.SmallInt);
            arParams[74].Value = signEmailWithDkim ? 1 : 0;

            arParams[75] = new FbParameter("@OidConnectAppId", FbDbType.VarChar, 255);
            arParams[75].Value = oidConnectAppId;

            arParams[76] = new FbParameter("@OidConnectAppSecret", FbDbType.VarChar);
            arParams[76].Value = oidConnectAppSecret;

            arParams[77] = new FbParameter("@SmsClientId", FbDbType.VarChar, 255);
            arParams[77].Value = smsClientId;

            arParams[78] = new FbParameter("@SmsSecureToken", FbDbType.VarChar);
            arParams[78].Value = smsSecureToken;

            arParams[79] = new FbParameter("@SmsFrom", FbDbType.VarChar, 255);
            arParams[79].Value = smsFrom;


            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams);

            return (rowsAffected > 0);

        }

        public async Task<bool> Delete(
            int siteId,
            CancellationToken cancellationToken)
        {
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles WHERE UserID IN (SELECT UserID FROM mp_Users WHERE SiteID = @SiteID);");

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLocation WHERE UserGuid IN (SELECT UserGuid FROM mp_Users WHERE SiteID = @SiteID);");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Users WHERE SiteID = @SiteID; ");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Roles WHERE SiteID = @SiteID; ");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteHosts WHERE SiteID = @SiteID; ");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteSettingsEx WHERE SiteID = @SiteID; ");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteFolders WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID);");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Sites ");
            sqlCommand.Append("WHERE SiteID = @SiteID  ; ");

            rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

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

        public async Task<int> GetHostCount(CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SiteHosts ");
            sqlCommand.Append(";");

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
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
                CommandType.Text,
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
                CommandType.Text,
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
                CommandType.Text,
                sqlCommand.ToString(),
                null,
                cancellationToken);

        }


        public async Task<DbDataReader> GetHostList(
            int siteId,
            CancellationToken cancellationToken)
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
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetHost(
            int hostId,
            CancellationToken cancellationToken)
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
                CommandType.Text,
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
            sqlCommand.Append("WHERE HostName = @HostName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@HostName", FbDbType.VarChar, 255);
            arParams[0].Value = hostName;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
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
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
            sqlCommand.Append("WHERE HostID = @HostID  ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@HostID", FbDbType.Integer);
            arParams[0].Value = hostId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
                arParams,
                cancellationToken);

            return rowsAffected > 0;

        }

        public async Task<bool> DeleteHostsBySite(int siteId, CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE SiteID = @SiteID  ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Value = siteId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                true,
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
                CommandType.Text,
                sqlCommand.ToString(),
                null,
                cancellationToken);
        }

        public async Task<DbDataReader> GetSite(
            int siteId,
            CancellationToken cancellationToken)
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
            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
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
            sqlCommand.Append("WHERE SiteGuid = @SiteGuid ");
            sqlCommand.Append("ORDER BY	SiteName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.VarChar, 36);
            arParams[0].Value = siteGuid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);
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
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }


        public async Task<DbDataReader> GetSite(
            string hostName,
            CancellationToken cancellationToken)
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
                CommandType.Text,
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
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

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

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
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
            sqlCommand.Append("WHERE SiteID <> @CurrentSiteID ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CurrentSiteID", FbDbType.Integer);
            arParams[0].Value = currentSiteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
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
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<int> GetSiteIdByHostName(
            string hostName,
            CancellationToken cancellationToken)
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
                CommandType.Text,
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
                sqlCommand.Append("SELECT FIRST 1 SiteID ");
                sqlCommand.Append("FROM	mp_Sites ");
                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append(" ;");

                using (DbDataReader reader = await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
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
                CommandType.Text,
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
                sqlCommand.Append("SELECT FIRST 1 SiteID ");
                sqlCommand.Append("FROM	mp_Sites ");
                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append(" ;");

                using (DbDataReader reader = await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
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
                sqlCommand.Append("SELECT FIRST 1 SiteID ");
                sqlCommand.Append("FROM	mp_Sites ");
                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append(" ;");

                using (DbDataReader reader = AdoHelper.ExecuteReader(
                readConnectionString,
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

    }
}
