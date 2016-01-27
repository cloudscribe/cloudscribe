// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-27
//

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.pgsql
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

            // possibly will change this later to have NpgSqlFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(Npgsql.NpgsqlFactory.Instance);
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
            sqlCommand.Append("INSERT INTO mp_sites (");
            sqlCommand.Append("siteguid, ");

            sqlCommand.Append("sitename, ");
            sqlCommand.Append("skin, ");
            sqlCommand.Append("allownewregistration, ");
            sqlCommand.Append("usesecureregistration, ");
            sqlCommand.Append("isserveradminsite, ");
            sqlCommand.Append("useldapauth, ");
            sqlCommand.Append("autocreateldapuseronfirstlogin, ");
            sqlCommand.Append("ldapserver, ");
            sqlCommand.Append("ldapport, ");
            sqlCommand.Append("ldapdomain, ");
            sqlCommand.Append("ldaprootdn, ");
            sqlCommand.Append("ldapuserdnkey, ");
            sqlCommand.Append("reallydeleteusers, ");
            sqlCommand.Append("useemailforlogin, ");
            sqlCommand.Append("disabledbauth, ");
            sqlCommand.Append("recaptchaprivatekey, ");
            sqlCommand.Append("recaptchapublickey, ");
            sqlCommand.Append("requiresquestionandanswer, ");
            sqlCommand.Append("maxinvalidpasswordattempts, ");
            sqlCommand.Append("minrequiredpasswordlength, ");
            sqlCommand.Append("defaultemailfromaddress, ");
            sqlCommand.Append("allowdbfallbackwithldap, ");
            sqlCommand.Append("emailldapdbfallback, ");
            sqlCommand.Append("allowpersistentlogin, ");
            sqlCommand.Append("captchaonlogin, ");
            sqlCommand.Append("captchaonregistration, ");
            sqlCommand.Append("siteisclosed, ");
            sqlCommand.Append("siteisclosedmessage, ");
            sqlCommand.Append("privacypolicy, ");
            sqlCommand.Append("timezoneid, ");
            sqlCommand.Append("googleanalyticsprofileid, ");
            sqlCommand.Append("companyname, ");
            sqlCommand.Append("companystreetaddress, ");
            sqlCommand.Append("companystreetaddress2, ");
            sqlCommand.Append("companyregion, ");
            sqlCommand.Append("companylocality, ");
            sqlCommand.Append("companycountry, ");
            sqlCommand.Append("companypostalcode, ");
            sqlCommand.Append("companypublicemail, ");
            sqlCommand.Append("companyphone, ");
            sqlCommand.Append("companyfax, ");
            sqlCommand.Append("facebookappid, ");
            sqlCommand.Append("facebookappsecret, ");
            sqlCommand.Append("googleclientid, ");
            sqlCommand.Append("googleclientsecret, ");
            sqlCommand.Append("twitterconsumerkey, ");
            sqlCommand.Append("twitterconsumersecret, ");
            sqlCommand.Append("microsoftclientid, ");
            sqlCommand.Append("microsoftclientsecret, ");
            sqlCommand.Append("preferredhostname, ");
            sqlCommand.Append("sitefoldername, ");
            sqlCommand.Append("addthisdotcomusername, ");
            sqlCommand.Append("logininfotop, ");
            sqlCommand.Append("logininfobottom, ");
            sqlCommand.Append("registrationagreement, ");
            sqlCommand.Append("registrationpreamble, ");
            sqlCommand.Append("smtpserver, ");
            sqlCommand.Append("smtpport, ");
            sqlCommand.Append("smtpuser, ");
            sqlCommand.Append("smtppassword, ");
            sqlCommand.Append("smtppreferredencoding, ");
            sqlCommand.Append("smtprequiresauth, ");
            sqlCommand.Append("smtpusessl, ");
            sqlCommand.Append("isdataprotected, ");
            sqlCommand.Append("createdutc, ");

            sqlCommand.Append("requireconfirmedphone, ");
            sqlCommand.Append("defaultemailfromalias, ");
            sqlCommand.Append("accountapprovalemailcsv, ");
            sqlCommand.Append("dkimpublickey, ");
            sqlCommand.Append("dkimprivatekey, ");
            sqlCommand.Append("dkimdomain, ");
            sqlCommand.Append("dkimselector, ");
            sqlCommand.Append("signemailwithdkim, ");
            sqlCommand.Append("oidconnectappid, ");
            sqlCommand.Append("oidconnectappsecret, ");
            sqlCommand.Append("smsclientid, ");
            sqlCommand.Append("smssecuretoken, ");
            sqlCommand.Append("smsfrom, ");

            sqlCommand.Append("requireapprovalbeforelogin ");


            sqlCommand.Append(") ");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":siteguid, ");

            sqlCommand.Append(":sitename, ");
            sqlCommand.Append(":skin, ");
            sqlCommand.Append(":allownewregistration, ");
            sqlCommand.Append(":usesecureregistration, ");
            sqlCommand.Append(":isserveradminsite, ");
            sqlCommand.Append(":useldapauth, ");
            sqlCommand.Append(":autocreateldapuseronfirstlogin, ");
            sqlCommand.Append(":ldapserver, ");
            sqlCommand.Append(":ldapport, ");
            sqlCommand.Append(":ldapdomain, ");
            sqlCommand.Append(":ldaprootdn, ");
            sqlCommand.Append(":ldapuserdnkey, ");
            sqlCommand.Append(":reallydeleteusers, ");
            sqlCommand.Append(":useemailforlogin, ");
            sqlCommand.Append(":disabledbauth, ");
            sqlCommand.Append(":recaptchaprivatekey, ");
            sqlCommand.Append(":recaptchapublickey, ");
            sqlCommand.Append(":requiresquestionandanswer, ");
            sqlCommand.Append(":maxinvalidpasswordattempts, ");
            sqlCommand.Append(":minrequiredpasswordlength, ");
            sqlCommand.Append(":defaultemailfromaddress, ");
            sqlCommand.Append(":allowdbfallbackwithldap, ");
            sqlCommand.Append(":emailldapdbfallback, ");
            sqlCommand.Append(":allowpersistentlogin, ");
            sqlCommand.Append(":captchaonlogin, ");
            sqlCommand.Append(":captchaonregistration, ");
            sqlCommand.Append(":siteisclosed, ");
            sqlCommand.Append(":siteisclosedmessage, ");
            sqlCommand.Append(":privacypolicy, ");
            sqlCommand.Append(":timezoneid, ");
            sqlCommand.Append(":googleanalyticsprofileid, ");
            sqlCommand.Append(":companyname, ");
            sqlCommand.Append(":companystreetaddress, ");
            sqlCommand.Append(":companystreetaddress2, ");
            sqlCommand.Append(":companyregion, ");
            sqlCommand.Append(":companylocality, ");
            sqlCommand.Append(":companycountry, ");
            sqlCommand.Append(":companypostalcode, ");
            sqlCommand.Append(":companypublicemail, ");
            sqlCommand.Append(":companyphone, ");
            sqlCommand.Append(":companyfax, ");
            sqlCommand.Append(":facebookappid, ");
            sqlCommand.Append(":facebookappsecret, ");
            sqlCommand.Append(":googleclientid, ");
            sqlCommand.Append(":googleclientsecret, ");
            sqlCommand.Append(":twitterconsumerkey, ");
            sqlCommand.Append(":twitterconsumersecret, ");
            sqlCommand.Append(":microsoftclientid, ");
            sqlCommand.Append(":microsoftclientsecret, ");
            sqlCommand.Append(":preferredhostname, ");
            sqlCommand.Append(":sitefoldername, ");
            sqlCommand.Append(":addthisdotcomusername, ");
            sqlCommand.Append(":logininfotop, ");
            sqlCommand.Append(":logininfobottom, ");
            sqlCommand.Append(":registrationagreement, ");
            sqlCommand.Append(":registrationpreamble, ");
            sqlCommand.Append(":smtpserver, ");
            sqlCommand.Append(":smtpport, ");
            sqlCommand.Append(":smtpuser, ");
            sqlCommand.Append(":smtppassword, ");
            sqlCommand.Append(":smtppreferredencoding, ");
            sqlCommand.Append(":smtprequiresauth, ");
            sqlCommand.Append(":smtpusessl, ");
            sqlCommand.Append(":isdataprotected, ");
            sqlCommand.Append(":createdutc, ");

            sqlCommand.Append(":requireconfirmedphone, ");
            sqlCommand.Append(":defaultemailfromalias, ");
            sqlCommand.Append(":accountapprovalemailcsv, ");
            sqlCommand.Append(":dkimpublickey, ");
            sqlCommand.Append(":dkimprivatekey, ");
            sqlCommand.Append(":dkimdomain, ");
            sqlCommand.Append(":dkimselector, ");
            sqlCommand.Append(":signemailwithdkim, ");
            sqlCommand.Append(":oidconnectappid, ");
            sqlCommand.Append(":oidconnectappsecret, ");
            sqlCommand.Append(":smsclientid, ");
            sqlCommand.Append(":smssecuretoken, ");
            sqlCommand.Append(":smsfrom, ");

            sqlCommand.Append(":requireapprovalbeforelogin ");

            sqlCommand.Append(") ");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_sites_siteid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[80];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter("sitename", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = siteName;

            arParams[2] = new NpgsqlParameter("skin", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[2].Value = skin;
            
            arParams[3] = new NpgsqlParameter("allownewregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[3].Value = allowNewRegistration;

            arParams[4] = new NpgsqlParameter("usesecureregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Value = useSecureRegistration;
            
            arParams[5] = new NpgsqlParameter("isserveradminsite", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Value = isServerAdminSite;

            arParams[6] = new NpgsqlParameter("useldapauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Value = useLdapAuth;

            arParams[7] = new NpgsqlParameter("autocreateldapuseronfirstlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Value = autoCreateLdapUserOnFirstLogin;

            arParams[8] = new NpgsqlParameter("ldapserver", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[8].Value = ldapServer;

            arParams[9] = new NpgsqlParameter("ldapport", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[9].Value = ldapPort;

            arParams[10] = new NpgsqlParameter("ldapdomain", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[10].Value = ldapDomain;

            arParams[11] = new NpgsqlParameter("ldaprootdn", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[11].Value = ldapRootDN;

            arParams[12] = new NpgsqlParameter("ldapuserdnkey", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[12].Value = ldapUserDNKey;

            arParams[13] = new NpgsqlParameter("reallydeleteusers", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[13].Value = reallyDeleteUsers;

            arParams[14] = new NpgsqlParameter("useemailforlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[14].Value = useEmailForLogin;
            
            arParams[15] = new NpgsqlParameter("recaptchaprivatekey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[15].Value = recaptchaPrivateKey;

            arParams[16] = new NpgsqlParameter("recaptchapublickey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[16].Value = recaptchaPublicKey;
            
            arParams[17] = new NpgsqlParameter("disabledbauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[17].Value = disableDbAuth;

            arParams[18] = new NpgsqlParameter("requiresquestionandanswer", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[18].Value = requiresQuestionAndAnswer;

            arParams[19] = new NpgsqlParameter("maxinvalidpasswordattempts", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[19].Value = maxInvalidPasswordAttempts;
            
            arParams[20] = new NpgsqlParameter("minrequiredpasswordlength", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[20].Value = minRequiredPasswordLength;
            
            arParams[21] = new NpgsqlParameter("defaultemailfromaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[21].Value = defaultEmailFromAddress;

            arParams[22] = new NpgsqlParameter("allowdbfallbackwithldap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[22].Value = allowDbFallbackWithLdap;

            arParams[23] = new NpgsqlParameter("emailldapdbfallback", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[23].Value = emailLdapDbFallback;

            arParams[24] = new NpgsqlParameter("allowpersistentlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[24].Value = allowPersistentLogin;

            arParams[25] = new NpgsqlParameter("captchaonlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[25].Value = captchaOnLogin;

            arParams[26] = new NpgsqlParameter("captchaonregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[26].Value = captchaOnRegistration;

            arParams[27] = new NpgsqlParameter("siteisclosed", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[27].Value = siteIsClosed;

            arParams[28] = new NpgsqlParameter("siteisclosedmessage", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[28].Value = siteIsClosedMessage;

            arParams[29] = new NpgsqlParameter("privacypolicy", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[29].Value = privacyPolicy;

            arParams[30] = new NpgsqlParameter("timezoneid", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[30].Value = timeZoneId;

            arParams[31] = new NpgsqlParameter("googleanalyticsprofileid", NpgsqlTypes.NpgsqlDbType.Varchar, 25);
            arParams[31].Value = googleAnalyticsProfileId;

            arParams[32] = new NpgsqlParameter("companyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[32].Value = companyName;

            arParams[33] = new NpgsqlParameter("companystreetaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[33].Value = companyStreetAddress;

            arParams[34] = new NpgsqlParameter("companystreetaddress2", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[34].Value = companyStreetAddress2;

            arParams[35] = new NpgsqlParameter("companyregion", NpgsqlTypes.NpgsqlDbType.Varchar, 200);
            arParams[35].Value = companyRegion;

            arParams[36] = new NpgsqlParameter("companylocality", NpgsqlTypes.NpgsqlDbType.Varchar, 200);
            arParams[36].Value = companyLocality;

            arParams[37] = new NpgsqlParameter("companycountry", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[37].Value = companyCountry;

            arParams[38] = new NpgsqlParameter("companypostalcode", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[38].Value = companyPostalCode;

            arParams[39] = new NpgsqlParameter("companypublicemail", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[39].Value = companyPublicEmail;

            arParams[40] = new NpgsqlParameter("companyphone", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[40].Value = companyPhone;

            arParams[41] = new NpgsqlParameter("companyfax", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[41].Value = companyFax;

            arParams[42] = new NpgsqlParameter("facebookappid", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[42].Value = facebookAppId;

            arParams[43] = new NpgsqlParameter("facebookappsecret", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[43].Value = facebookAppSecret;

            arParams[44] = new NpgsqlParameter("googleclientid", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[44].Value = googleClientId;

            arParams[45] = new NpgsqlParameter("googleclientsecret", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[45].Value = googleClientSecret;

            arParams[46] = new NpgsqlParameter("twitterconsumerkey", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[46].Value = twitterConsumerKey;

            arParams[47] = new NpgsqlParameter("twitterconsumersecret", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[47].Value = twitterConsumerSecret;

            arParams[48] = new NpgsqlParameter("microsoftclientid", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[48].Value = microsoftClientId;

            arParams[49] = new NpgsqlParameter("microsoftclientsecret", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[49].Value = microsoftClientSecret;

            arParams[50] = new NpgsqlParameter("preferredhostname", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[50].Value = preferredHostName;

            arParams[51] = new NpgsqlParameter("sitefoldername", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[51].Value = siteFolderName;

            arParams[52] = new NpgsqlParameter("addthisdotcomusername", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[52].Value = addThisDotComUsername;

            arParams[53] = new NpgsqlParameter("logininfotop", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[53].Value = loginInfoTop;

            arParams[54] = new NpgsqlParameter("logininfobottom", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[54].Value = loginInfoBottom;

            arParams[55] = new NpgsqlParameter("registrationagreement", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[55].Value = registrationAgreement;

            arParams[56] = new NpgsqlParameter("registrationpreamble", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[56].Value = registrationPreamble;

            arParams[57] = new NpgsqlParameter("smtpserver", NpgsqlTypes.NpgsqlDbType.Varchar, 200);
            arParams[57].Value = smtpServer;

            arParams[58] = new NpgsqlParameter("smtpport", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[58].Value = smtpPort;

            arParams[59] = new NpgsqlParameter("smtpuser", NpgsqlTypes.NpgsqlDbType.Varchar, 500);
            arParams[59].Value = smtpUser;

            arParams[60] = new NpgsqlParameter("smtppassword", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[60].Value = smtpPassword;

            arParams[61] = new NpgsqlParameter("smtppreferredencoding", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[61].Value = smtpPreferredEncoding;

            arParams[62] = new NpgsqlParameter("smtprequiresauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[62].Value = smtpRequiresAuth;

            arParams[63] = new NpgsqlParameter("smtpusessl", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[63].Value = smtpUseSsl;

            arParams[64] = new NpgsqlParameter("requireapprovalbeforelogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[64].Value = requireApprovalBeforeLogin;

            arParams[65] = new NpgsqlParameter("isdataprotected", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[65].Value = isDataProtected;

            arParams[66] = new NpgsqlParameter("createdutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[66].Value = createdUtc;

            arParams[67] = new NpgsqlParameter("requireconfirmedphone", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[67].Value = requireConfirmedPhone;

            arParams[68] = new NpgsqlParameter("defaultemailfromalias", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[68].Value = defaultEmailFromAlias;

            arParams[69] = new NpgsqlParameter("accountapprovalemailcsv", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[69].Value = accountApprovalEmailCsv;

            arParams[70] = new NpgsqlParameter("dkimpublickey", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[70].Value = dkimPublicKey;

            arParams[71] = new NpgsqlParameter("dkimprivatekey", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[71].Value = dkimPrivateKey;

            arParams[72] = new NpgsqlParameter("dkimdomain", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[72].Value = dkimDomain;

            arParams[73] = new NpgsqlParameter("dkimselector", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[73].Value = dkimSelector;

            arParams[74] = new NpgsqlParameter("signemailwithdkim", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[74].Value = signEmailWithDkim;

            arParams[75] = new NpgsqlParameter("oidconnectappId", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[75].Value = oidConnectAppId;

            arParams[76] = new NpgsqlParameter("oidconnectappsecret", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[76].Value = oidConnectAppSecret;

            arParams[77] = new NpgsqlParameter("smsclientid", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[77].Value = smsClientId;

            arParams[78] = new NpgsqlParameter("smssecuretoken", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[78].Value = smsSecureToken;

            arParams[79] = new NpgsqlParameter("smsfrom", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[79].Value = smsFrom;

            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
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
            sqlCommand.Append("UPDATE mp_sites ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("sitename = :sitename, ");
            sqlCommand.Append("skin = :skin, ");
            sqlCommand.Append("allownewregistration = :allownewregistration, ");
            sqlCommand.Append("usesecureregistration = :usesecureregistration, ");
            sqlCommand.Append("isserveradminsite = :isserveradminsite, ");
            sqlCommand.Append("useldapauth = :useldapauth, ");
            sqlCommand.Append("autocreateldapuseronfirstlogin = :autocreateldapuseronfirstlogin, ");
            sqlCommand.Append("ldapserver = :ldapserver, ");
            sqlCommand.Append("ldapport = :ldapport, ");
            sqlCommand.Append("ldapdomain = :ldapdomain, ");
            sqlCommand.Append("ldaprootdn = :ldaprootdn, ");
            sqlCommand.Append("ldapuserdnkey = :ldapuserdnkey, ");
            sqlCommand.Append("reallydeleteusers = :reallydeleteusers, ");
            sqlCommand.Append("useemailforlogin = :useemailforlogin, ");
            sqlCommand.Append("disabledbauth = :disabledbauth, ");
            sqlCommand.Append("recaptchaprivatekey = :recaptchaprivatekey, ");
            sqlCommand.Append("recaptchapublickey = :recaptchapublickey, ");
            sqlCommand.Append("requiresquestionandanswer = :requiresquestionandanswer, ");
            sqlCommand.Append("maxinvalidpasswordattempts = :maxinvalidpasswordattempts, ");
            sqlCommand.Append("minrequiredpasswordlength = :minrequiredpasswordlength, ");
            sqlCommand.Append("defaultemailfromaddress = :defaultemailfromaddress, ");
            sqlCommand.Append("allowdbfallbackwithldap = :allowdbfallbackwithldap, ");
            sqlCommand.Append("emailldapdbfallback = :emailldapdbfallback, ");
            sqlCommand.Append("allowpersistentlogin = :allowpersistentlogin, ");
            sqlCommand.Append("captchaonlogin = :captchaonlogin, ");
            sqlCommand.Append("captchaonregistration = :captchaonregistration,");
            sqlCommand.Append("siteisclosed = :siteisclosed, ");
            sqlCommand.Append("siteisclosedmessage = :siteisclosedmessage, ");
            sqlCommand.Append("privacypolicy = :privacypolicy, ");
            sqlCommand.Append("timezoneid = :timezoneid, ");
            sqlCommand.Append("googleanalyticsprofileid = :googleanalyticsprofileid, ");
            sqlCommand.Append("companyname = :companyname, ");
            sqlCommand.Append("companystreetaddress = :companystreetaddress, ");
            sqlCommand.Append("companystreetaddress2 = :companystreetaddress2, ");
            sqlCommand.Append("companyregion = :companyregion, ");
            sqlCommand.Append("companylocality = :companylocality, ");
            sqlCommand.Append("companycountry = :companycountry, ");
            sqlCommand.Append("companypostalcode = :companypostalcode, ");
            sqlCommand.Append("companypublicemail = :companypublicemail, ");
            sqlCommand.Append("companyphone = :companyphone, ");
            sqlCommand.Append("companyfax = :companyfax, ");
            sqlCommand.Append("facebookappid = :facebookappid, ");
            sqlCommand.Append("facebookappsecret = :facebookappsecret, ");
            sqlCommand.Append("googleclientid = :googleclientid, ");
            sqlCommand.Append("googleclientsecret = :googleclientsecret, ");
            sqlCommand.Append("twitterconsumerkey = :twitterconsumerkey, ");
            sqlCommand.Append("twitterconsumersecret = :twitterconsumersecret, ");
            sqlCommand.Append("microsoftclientid = :microsoftclientid, ");
            sqlCommand.Append("microsoftclientsecret = :microsoftclientsecret, ");
            sqlCommand.Append("preferredhostname = :preferredhostname, ");
            sqlCommand.Append("sitefoldername = :sitefoldername, ");
            sqlCommand.Append("addthisdotcomusername = :addthisdotcomusername, ");
            sqlCommand.Append("logininfotop = :logininfotop, ");
            sqlCommand.Append("logininfobottom = :logininfobottom, ");
            sqlCommand.Append("registrationagreement = :registrationagreement, ");
            sqlCommand.Append("registrationpreamble = :registrationpreamble, ");
            sqlCommand.Append("smtpserver = :smtpserver, ");
            sqlCommand.Append("smtpport = :smtpport, ");
            sqlCommand.Append("smtpuser = :smtpuser, ");
            sqlCommand.Append("smtppassword = :smtppassword, ");
            sqlCommand.Append("smtppreferredencoding = :smtppreferredencoding, ");
            sqlCommand.Append("smtprequiresauth = :smtprequiresauth, ");
            sqlCommand.Append("smtpusessl = :smtpusessl, ");
            sqlCommand.Append("isdataprotected = :isdataprotected, ");

            sqlCommand.Append("requireconfirmedphone = :requireconfirmedphone, ");
            sqlCommand.Append("defaultemailfromalias = :defaultemailfromalias, ");
            sqlCommand.Append("accountapprovalemailcsv = :accountapprovalemailcsv, ");
            sqlCommand.Append("dkimpublickey = :dkimpublickey, ");
            sqlCommand.Append("dkimprivatekey = :dkimprivatekey, ");
            sqlCommand.Append("dkimdomain = :dkimdomain, ");
            sqlCommand.Append("dkimselector = :dkimselector, ");
            sqlCommand.Append("signemailwithdkim = :signemailwithdkim, ");
            sqlCommand.Append("oidconnectappid = :oidconnectappid, ");
            sqlCommand.Append("oidconnectappsecret = :oidconnectappsecret, ");
            sqlCommand.Append("smsclientid = :smsclientid, ");
            sqlCommand.Append("smssecuretoken = :smssecuretoken, ");
            sqlCommand.Append("smsfrom = :smsfrom, ");

            sqlCommand.Append("requireapprovalbeforelogin = :requireapprovalbeforelogin ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[79];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("sitename", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = siteName;

            arParams[2] = new NpgsqlParameter("skin", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[2].Value = skin;
            
            arParams[3] = new NpgsqlParameter("allownewregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[3].Value = allowNewRegistration;

            arParams[4] = new NpgsqlParameter("usesecureregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Value = useSecureRegistration;
            
            arParams[5] = new NpgsqlParameter("isserveradminsite", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Value = isServerAdminSite;

            arParams[6] = new NpgsqlParameter("useldapauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Value = useLdapAuth;

            arParams[7] = new NpgsqlParameter("autocreateldapuseronfirstlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Value = autoCreateLdapUserOnFirstLogin;

            arParams[8] = new NpgsqlParameter("ldapserver", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[8].Value = ldapServer;

            arParams[9] = new NpgsqlParameter("ldapport", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[9].Value = ldapPort;

            arParams[10] = new NpgsqlParameter("ldapdomain", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[10].Value = ldapDomain;

            arParams[11] = new NpgsqlParameter("ldaprootdn", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[11].Value = ldapRootDN;

            arParams[12] = new NpgsqlParameter("ldapuserdnkey", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[12].Value = ldapUserDNKey;

            arParams[13] = new NpgsqlParameter("reallydeleteusers", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[13].Value = reallyDeleteUsers;

            arParams[14] = new NpgsqlParameter("useemailforlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[14].Value = useEmailForLogin;
            
            arParams[15] = new NpgsqlParameter("recaptchaprivatekey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[15].Value = recaptchaPrivateKey;

            arParams[16] = new NpgsqlParameter("recaptchapublickey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[16].Value = recaptchaPublicKey;
            
            arParams[17] = new NpgsqlParameter("disabledbauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[17].Value = disableDbAuth;

            arParams[18] = new NpgsqlParameter("requiresquestionandanswer", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[18].Value = requiresQuestionAndAnswer;

            arParams[19] = new NpgsqlParameter("maxinvalidpasswordattempts", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[19].Value = maxInvalidPasswordAttempts;
            
            arParams[20] = new NpgsqlParameter("minrequiredpasswordlength", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[20].Value = minRequiredPasswordLength;
            
            arParams[21] = new NpgsqlParameter("defaultemailfromaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[21].Value = defaultEmailFromAddress;

            arParams[22] = new NpgsqlParameter("allowdbfallbackwithldap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[22].Value = allowDbFallbackWithLdap;

            arParams[23] = new NpgsqlParameter("emailldapdbfallback", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[23].Value = emailLdapDbFallback;

            arParams[24] = new NpgsqlParameter("allowpersistentlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[24].Value = allowPersistentLogin;

            arParams[25] = new NpgsqlParameter("captchaonlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[25].Value = captchaOnLogin;

            arParams[26] = new NpgsqlParameter("captchaonregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[26].Value = captchaOnRegistration;

            arParams[27] = new NpgsqlParameter("siteisclosed", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[27].Value = siteIsClosed;

            arParams[28] = new NpgsqlParameter("siteisclosedmessage", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[28].Value = siteIsClosedMessage;

            arParams[29] = new NpgsqlParameter("privacypolicy", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[29].Value = privacyPolicy;

            arParams[30] = new NpgsqlParameter("timezoneid", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[30].Value = timeZoneId;

            arParams[31] = new NpgsqlParameter("googleanalyticsprofileid", NpgsqlTypes.NpgsqlDbType.Varchar, 25);
            arParams[31].Value = googleAnalyticsProfileId;

            arParams[32] = new NpgsqlParameter("companyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[32].Value = companyName;

            arParams[33] = new NpgsqlParameter("companystreetaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[33].Value = companyStreetAddress;

            arParams[34] = new NpgsqlParameter("companystreetaddress2", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[34].Value = companyStreetAddress2;

            arParams[35] = new NpgsqlParameter("companyregion", NpgsqlTypes.NpgsqlDbType.Varchar, 200);
            arParams[35].Value = companyRegion;

            arParams[36] = new NpgsqlParameter("companylocality", NpgsqlTypes.NpgsqlDbType.Varchar, 200);
            arParams[36].Value = companyLocality;

            arParams[37] = new NpgsqlParameter("companycountry", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[37].Value = companyCountry;

            arParams[38] = new NpgsqlParameter("companypostalcode", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[38].Value = companyPostalCode;

            arParams[39] = new NpgsqlParameter("companypublicemail", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[39].Value = companyPublicEmail;

            arParams[40] = new NpgsqlParameter("companyphone", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[40].Value = companyPhone;

            arParams[41] = new NpgsqlParameter("companyfax", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[41].Value = companyFax;

            arParams[42] = new NpgsqlParameter("facebookappid", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[42].Value = facebookAppId;

            arParams[43] = new NpgsqlParameter("facebookappsecret", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[43].Value = facebookAppSecret;

            arParams[44] = new NpgsqlParameter("googleclientid", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[44].Value = googleClientId;

            arParams[45] = new NpgsqlParameter("googleclientsecret", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[45].Value = googleClientSecret;

            arParams[46] = new NpgsqlParameter("twitterconsumerkey", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[46].Value = twitterConsumerKey;

            arParams[47] = new NpgsqlParameter("twitterconsumersecret", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[47].Value = twitterConsumerSecret;

            arParams[48] = new NpgsqlParameter("microsoftclientid", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[48].Value = microsoftClientId;

            arParams[49] = new NpgsqlParameter("microsoftclientsecret", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[49].Value = microsoftClientSecret;

            arParams[50] = new NpgsqlParameter("preferredhostname", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[50].Value = preferredHostName;

            arParams[51] = new NpgsqlParameter("sitefoldername", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[51].Value = siteFolderName;

            arParams[52] = new NpgsqlParameter("addthisdotcomusername", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[52].Value = addThisDotComUsername;

            arParams[53] = new NpgsqlParameter("logininfotop", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[53].Value = loginInfoTop;

            arParams[54] = new NpgsqlParameter("logininfobottom", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[54].Value = loginInfoBottom;

            arParams[55] = new NpgsqlParameter("registrationagreement", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[55].Value = registrationAgreement;

            arParams[56] = new NpgsqlParameter("registrationpreamble", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[56].Value = registrationPreamble;

            arParams[57] = new NpgsqlParameter("smtpserver", NpgsqlTypes.NpgsqlDbType.Varchar, 200);
            arParams[57].Value = smtpServer;

            arParams[58] = new NpgsqlParameter("smtpport", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[58].Value = smtpPort;

            arParams[59] = new NpgsqlParameter("smtpuser", NpgsqlTypes.NpgsqlDbType.Varchar, 500);
            arParams[59].Value = smtpUser;

            arParams[60] = new NpgsqlParameter("smtppassword", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[60].Value = smtpPassword;

            arParams[61] = new NpgsqlParameter("smtppreferredencoding", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[61].Value = smtpPreferredEncoding;

            arParams[62] = new NpgsqlParameter("smtprequiresauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[62].Value = smtpRequiresAuth;

            arParams[63] = new NpgsqlParameter("smtpusessl", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[63].Value = smtpUseSsl;

            arParams[64] = new NpgsqlParameter("requireapprovalbeforelogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[64].Value = requireApprovalBeforeLogin;

            arParams[65] = new NpgsqlParameter("isdataprotected", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[65].Value = isDataProtected;

            arParams[66] = new NpgsqlParameter("requireconfirmedphone", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[66].Value = requireConfirmedPhone;

            arParams[67] = new NpgsqlParameter("defaultemailfromalias", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[67].Value = defaultEmailFromAlias;

            arParams[68] = new NpgsqlParameter("accountapprovalemailcsv", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[68].Value = accountApprovalEmailCsv;

            arParams[69] = new NpgsqlParameter("dkimpublickey", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[69].Value = dkimPublicKey;

            arParams[70] = new NpgsqlParameter("dkimprivatekey", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[70].Value = dkimPrivateKey;

            arParams[71] = new NpgsqlParameter("dkimdomain", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[71].Value = dkimDomain;

            arParams[72] = new NpgsqlParameter("dkimselector", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[72].Value = dkimSelector;

            arParams[73] = new NpgsqlParameter("signemailwithdkim", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[73].Value = signEmailWithDkim;

            arParams[74] = new NpgsqlParameter("oidconnectappId", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[74].Value = oidConnectAppId;

            arParams[75] = new NpgsqlParameter("oidconnectappsecret", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[75].Value = oidConnectAppSecret;

            arParams[76] = new NpgsqlParameter("smsclientid", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[76].Value = smsClientId;

            arParams[77] = new NpgsqlParameter("smssecuretoken", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[77].Value = smsSecureToken;

            arParams[78] = new NpgsqlParameter("smsfrom", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[78].Value = smsFrom;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

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
            sqlCommand.Append("UPDATE mp_sites ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("usesecureregistration = :usesecureregistration, ");

            sqlCommand.Append("useldapauth = :useldapauth, ");
            sqlCommand.Append("autocreateldapuseronfirstlogin = :autocreateldapuseronfirstlogin, ");
            sqlCommand.Append("ldapserver = :ldapserver, ");
            sqlCommand.Append("ldapport = :ldapport, ");
            sqlCommand.Append("ldapdomain = :ldapdomain, ");
            sqlCommand.Append("ldaprootdn = :ldaprootdn, ");
            sqlCommand.Append("ldapuserdnkey = :ldapuserdnkey, ");

            sqlCommand.Append("useemailforlogin = :useemailforlogin, ");
            sqlCommand.Append("allowuserfullnamechange = :allowuserfullnamechange, ");

            sqlCommand.Append("allowpasswordretrieval = :allowpasswordretrieval, ");
            sqlCommand.Append("allowpasswordreset = :allowpasswordreset, ");
            sqlCommand.Append("requiresquestionandanswer = :requiresquestionandanswer, ");
            sqlCommand.Append("maxinvalidpasswordattempts = :maxinvalidpasswordattempts, ");
            sqlCommand.Append("passwordattemptwindowminutes = :passwordattemptwindowminutes, ");
            sqlCommand.Append("requiresuniqueemail = :requiresuniqueemail, ");
            sqlCommand.Append("passwordformat = :passwordformat, ");
            sqlCommand.Append("minrequiredpasswordlength = :minrequiredpasswordlength, ");
            sqlCommand.Append("minreqnonalphachars = :minreqnonalphachars, ");
            sqlCommand.Append("pwdstrengthregex = :pwdstrengthregex, ");

            sqlCommand.Append("allowopenidauth = :allowopenidauth, ");
            sqlCommand.Append("allowwindowsliveauth = :allowwindowsliveauth ");


            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid <> :siteid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[24];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("allownewregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[1].Value = allowNewRegistration;

            arParams[2] = new NpgsqlParameter("usesecureregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[2].Value = useSecureRegistration;

            arParams[3] = new NpgsqlParameter("useldapauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[3].Value = useLdapAuth;

            arParams[4] = new NpgsqlParameter("autocreateldapuseronfirstlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Value = autoCreateLdapUserOnFirstLogin;

            arParams[5] = new NpgsqlParameter("ldapserver", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Value = ldapServer;

            arParams[6] = new NpgsqlParameter("ldapport", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[6].Value = ldapPort;

            arParams[7] = new NpgsqlParameter("ldapdomain", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[7].Value = ldapDomain;

            arParams[8] = new NpgsqlParameter("ldaprootdn", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[8].Value = ldapRootDN;

            arParams[9] = new NpgsqlParameter("ldapuserdnkey", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[9].Value = ldapUserDNKey;

            arParams[10] = new NpgsqlParameter("useemailforlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[10].Value = useEmailForLogin;

            arParams[11] = new NpgsqlParameter("allowuserfullnamechange", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[11].Value = allowUserFullNameChange;

            arParams[12] = new NpgsqlParameter("allowopenidauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[12].Value = allowOpenIdAuth;

            arParams[13] = new NpgsqlParameter("allowwindowsliveauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[13].Value = allowWindowsLiveAuth;

            arParams[14] = new NpgsqlParameter("allowpasswordretrieval", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[14].Value = allowPasswordRetrieval;

            arParams[15] = new NpgsqlParameter("allowpasswordreset", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[15].Value = allowPasswordReset;

            arParams[16] = new NpgsqlParameter("requiresquestionandanswer", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[16].Value = requiresQuestionAndAnswer;

            arParams[17] = new NpgsqlParameter("maxinvalidpasswordattempts", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[17].Value = maxInvalidPasswordAttempts;

            arParams[18] = new NpgsqlParameter("passwordattemptwindowminutes", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[18].Value = passwordAttemptWindowMinutes;

            arParams[19] = new NpgsqlParameter("requiresuniqueemail", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[19].Value = requiresUniqueEmail;

            arParams[20] = new NpgsqlParameter("passwordformat", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[20].Value = passwordFormat;

            arParams[21] = new NpgsqlParameter("minrequiredpasswordlength", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[21].Value = minRequiredPasswordLength;

            arParams[22] = new NpgsqlParameter("minreqnonalphachars", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[22].Value = minReqNonAlphaChars;

            arParams[23] = new NpgsqlParameter("pwdstrengthregex", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[23].Value = pwdStrengthRegex;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
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
            sqlCommand.Append("UPDATE mp_sites ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("windowsliveappid = :windowsliveappid, ");
            sqlCommand.Append("windowslivekey = :windowslivekey ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid <> :siteid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("windowsliveappid", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = windowsLiveAppId;

            arParams[2] = new NpgsqlParameter("windowslivekey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Value = windowsLiveKey;


            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        //public bool UpdateExtendedProperties(
        //    int siteId,
        //    bool allowPasswordRetrieval,
        //    bool allowPasswordReset,
        //    bool requiresQuestionAndAnswer,
        //    int maxInvalidPasswordAttempts,
        //    int passwordAttemptWindowMinutes,
        //    bool requiresUniqueEmail,
        //    int passwordFormat,
        //    int minRequiredPasswordLength,
        //    int minRequiredNonAlphanumericCharacters,
        //    String passwordStrengthRegularExpression,
        //    String defaultEmailFromAddress
        //    )
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[12];

        //    arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new NpgsqlParameter("allowpasswordretrieval", NpgsqlTypes.NpgsqlDbType.Boolean);
        //    arParams[1].Value = allowPasswordRetrieval;

        //    arParams[2] = new NpgsqlParameter("allowpasswordreset", NpgsqlTypes.NpgsqlDbType.Boolean);
        //    arParams[2].Value = allowPasswordReset;

        //    arParams[3] = new NpgsqlParameter("requiresquestionandanswer", NpgsqlTypes.NpgsqlDbType.Boolean);
        //    arParams[3].Value = requiresQuestionAndAnswer;

        //    arParams[4] = new NpgsqlParameter("maxinvalidpasswordattempts", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[4].Value = maxInvalidPasswordAttempts;

        //    arParams[5] = new NpgsqlParameter("passwordattemptwindowminutes", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[5].Value = passwordAttemptWindowMinutes;

        //    arParams[6] = new NpgsqlParameter("requiresuniqueemail", NpgsqlTypes.NpgsqlDbType.Boolean);
        //    arParams[6].Value = requiresUniqueEmail;

        //    arParams[7] = new NpgsqlParameter("passwordformat", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[7].Value = passwordFormat;

        //    arParams[8] = new NpgsqlParameter("minrequiredpasswordlength", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[8].Value = minRequiredPasswordLength;

        //    arParams[9] = new NpgsqlParameter("minrequirednonalphanumericcharacters", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[9].Value = minRequiredNonAlphanumericCharacters;

        //    arParams[10] = new NpgsqlParameter("passwordstrengthregularexpression", NpgsqlTypes.NpgsqlDbType.Text);
        //    arParams[10].Value = passwordStrengthRegularExpression;

        //    arParams[11] = new NpgsqlParameter("defaultemailfromaddress", NpgsqlTypes.NpgsqlDbType.Text, 100);
        //    arParams[11].Value = defaultEmailFromAddress;

        //    int rowsAffected = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        writeConnectionString,
        //        CommandType.StoredProcedure,
        //        "mp_sites_updateextendedproperties(:siteid,:allowpasswordretrieval,:allowpasswordreset,:requiresquestionandanswer,:maxinvalidpasswordattempts,:passwordattemptwindowminutes,:requiresuniqueemail,:passwordformat,:minrequiredpasswordlength,:minrequirednonalphanumericcharacters,:passwordstrengthregularexpression,:defaultemailfromaddress)",
        //        arParams));

        //    return (rowsAffected > -1);

        //}



        public async Task<bool> Delete(
            int siteId,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();


            sqlCommand.Append("DELETE FROM mp_userproperties WHERE userguid IN (SELECT userguid FROM mp_users WHERE siteid = :siteid);");
            sqlCommand.Append("DELETE FROM mp_userroles WHERE userid IN (SELECT userid FROM mp_users WHERE siteid = :siteid);");
            sqlCommand.Append("DELETE FROM mp_userlocation WHERE userguid IN (SELECT userguid FROM mp_users WHERE siteid = :siteid);");


            sqlCommand.Append("DELETE FROM mp_users WHERE siteid = :siteid; ");

            sqlCommand.Append("DELETE FROM mp_roles WHERE siteid = :siteid; ");
            sqlCommand.Append("DELETE FROM mp_sitehosts WHERE siteid = :siteid; ");

            sqlCommand.Append("DELETE FROM mp_sitefolders WHERE siteguid IN (SELECT siteguid FROM mp_sites WHERE siteid = :siteid);");

            sqlCommand.Append("DELETE FROM mp_sitesettingsex WHERE siteid = :siteid; ");

            
            sqlCommand.Append("DELETE FROM mp_sites ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > 0);

        }

        public async Task<DbDataReader> GetSiteList(CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sites ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("sitename ");
            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null, 
                cancellationToken);

        }

        public async Task<DbDataReader> GetSite(
            string hostName,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("hostname", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[0].Value = hostName;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sites ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = COALESCE( ");
            sqlCommand.Append("(select siteid from mp_sitehosts where hostname = :hostname limit 1), ");
            sqlCommand.Append("(select siteid from mp_sites order by siteid limit 1) ");
            sqlCommand.Append(") ");
            sqlCommand.Append(";");
            
            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public DbDataReader GetSiteNonAsync(string hostName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("hostname", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[0].Value = hostName;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sites ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = COALESCE( ");
            sqlCommand.Append("(select siteid from mp_sitehosts where hostname = :hostname limit 1), ");
            sqlCommand.Append("(select siteid from mp_sites order by siteid limit 1) ");
            sqlCommand.Append(") ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }


        public async Task<DbDataReader> GetSite(
            int siteId,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sites ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");
            
            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);
        }

        public DbDataReader GetSiteNonAsync(int siteId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sites ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");
            
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = siteGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sites ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append(";");
            
            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);
        }

        public DbDataReader GetSiteNonAsync(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = siteGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sites ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append(";");
            
            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public async Task<int> GetHostCount(CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_sitehosts ");
            sqlCommand.Append(";");

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null,
                cancellationToken);

            return Convert.ToInt32(result);

        }

        public async Task<DbDataReader> GetHost(
            string hostName,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sitehosts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("hostname = :hostname  ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("hostname", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = hostName;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);
        }

        public async Task<DbDataReader> GetAllHosts(CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sitehosts ");
            sqlCommand.Append("ORDER BY hostname  ");
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
            sqlCommand.Append("FROM	mp_sitehosts ");
            sqlCommand.Append("ORDER BY hostname  ");
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = pageSize;

            arParams[1] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_sitehosts  ");
            //sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ORDER BY hostname  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }


        public async Task<DbDataReader> GetHostList(
            int siteId,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sitehosts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");
            
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("hostname", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[1].Value = hostName;

            arParams[2] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Value = siteGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_sitehosts (");
            sqlCommand.Append("siteid, ");
            sqlCommand.Append("hostname, ");
            sqlCommand.Append("siteguid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":siteid, ");
            sqlCommand.Append(":hostname, ");
            sqlCommand.Append(":siteguid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");
            
            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteHost(
            int hostId,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("hostid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = hostId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_sitehosts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("hostid = :hostid ");
            sqlCommand.Append(";");
            
            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteHostsBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_sitehosts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");
            
            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return rowsAffected > 0;
        }


        public async Task<int> CountOtherSites(
            int currentSiteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_sites ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid <> :currentsiteid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("currentsiteid", NpgsqlTypes.NpgsqlDbType.Integer);
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
           
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("currentsiteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = currentSiteId;

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_sites  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid <> :currentsiteid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("sitename  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

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
            int siteId = -1;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT siteid ");
            sqlCommand.Append("FROM	mp_sitehosts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("hostname = :hostname ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("hostname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[0].Value = hostName;

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
                sqlCommand.Append("SELECT siteid ");
                sqlCommand.Append("FROM	mp_sites ");

                sqlCommand.Append("ORDER BY	siteid ");
                sqlCommand.Append("LIMIT 1 ;");

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
            int siteId = -1;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COALESCE(s.siteid, -1) AS siteid ");
            sqlCommand.Append("FROM	mp_sitefolders sf ");

            sqlCommand.Append("JOIN mp_sites s ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("sf.siteguid = s.siteguid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sf.foldername = :foldername ");
            sqlCommand.Append("ORDER BY s.siteid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[0].Value = folderName;

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
                sqlCommand.Append("SELECT siteid ");
                sqlCommand.Append("FROM	mp_sites ");

                sqlCommand.Append("ORDER BY	siteid ");
                sqlCommand.Append("LIMIT 1 ;");

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
            int siteId = -1;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COALESCE(s.siteid, -1) AS siteid ");
            sqlCommand.Append("FROM	mp_sitefolders sf ");

            sqlCommand.Append("JOIN mp_sites s ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("sf.siteguid = s.siteguid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sf.foldername = :foldername ");
            sqlCommand.Append("ORDER BY s.siteid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[0].Value = folderName;

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
                sqlCommand.Append("SELECT siteid ");
                sqlCommand.Append("FROM	mp_sites ");

                sqlCommand.Append("ORDER BY	siteid ");
                sqlCommand.Append("LIMIT 1 ;");

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
