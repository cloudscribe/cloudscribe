// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-11-18
//

using cloudscribe.DbHelpers.pgsql;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
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
        }

        private ILoggerFactory logFactory;
        private string readConnectionString;
        private string writeConnectionString;

        public async Task<int> Create(
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
            sqlCommand.Append("INSERT INTO mp_sites (");
            sqlCommand.Append("siteguid, ");

            sqlCommand.Append("sitename, ");
            sqlCommand.Append("skin, ");
            sqlCommand.Append("allownewregistration, ");
            sqlCommand.Append("usesecureregistration, ");
            sqlCommand.Append("usesslonallpages, ");
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
            sqlCommand.Append("allowuserfullnamechange, ");
            sqlCommand.Append("disabledbauth, ");
            sqlCommand.Append("recaptchaprivatekey, ");
            sqlCommand.Append("recaptchapublickey, ");
            sqlCommand.Append("apikeyextra1, ");
            sqlCommand.Append("apikeyextra2, ");
            sqlCommand.Append("apikeyextra3, ");
            sqlCommand.Append("apikeyextra4, ");
            sqlCommand.Append("apikeyextra5, ");

            sqlCommand.Append("requiresquestionandanswer, ");
            sqlCommand.Append("maxinvalidpasswordattempts, ");
            sqlCommand.Append("passwordattemptwindowminutes, ");
            sqlCommand.Append("minrequiredpasswordlength, ");
            sqlCommand.Append("minreqnonalphachars, ");
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
            sqlCommand.Append("requireapprovalbeforelogin ");


            sqlCommand.Append(") ");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":siteguid, ");

            sqlCommand.Append(":sitename, ");
            sqlCommand.Append(":skin, ");
            sqlCommand.Append(":allownewregistration, ");
            sqlCommand.Append(":usesecureregistration, ");
            sqlCommand.Append(":usesslonallpages, ");
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
            sqlCommand.Append(":allowuserfullnamechange, ");
            sqlCommand.Append(":disabledbauth, ");
            sqlCommand.Append(":recaptchaprivatekey, ");
            sqlCommand.Append(":recaptchapublickey, ");
            sqlCommand.Append(":apikeyextra1, ");
            sqlCommand.Append(":apikeyextra2, ");
            sqlCommand.Append(":apikeyextra3, ");
            sqlCommand.Append(":apikeyextra4, ");
            sqlCommand.Append(":apikeyextra5, ");

            sqlCommand.Append(":requiresquestionandanswer, ");
            sqlCommand.Append(":maxinvalidpasswordattempts, ");
            sqlCommand.Append(":passwordattemptwindowminutes, ");
            sqlCommand.Append(":minrequiredpasswordlength, ");
            sqlCommand.Append(":minreqnonalphachars, ");
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
            sqlCommand.Append(":requireapprovalbeforelogin ");

            sqlCommand.Append(") ");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_sites_siteid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[74];

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

            arParams[5] = new NpgsqlParameter("usesslonallpages", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Value = useSslOnAllPages;
            
            arParams[6] = new NpgsqlParameter("isserveradminsite", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Value = isServerAdminSite;

            arParams[7] = new NpgsqlParameter("useldapauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Value = useLdapAuth;

            arParams[8] = new NpgsqlParameter("autocreateldapuseronfirstlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Value = autoCreateLdapUserOnFirstLogin;

            arParams[9] = new NpgsqlParameter("ldapserver", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[9].Value = ldapServer;

            arParams[10] = new NpgsqlParameter("ldapport", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[10].Value = ldapPort;

            arParams[11] = new NpgsqlParameter("ldapdomain", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[11].Value = ldapDomain;

            arParams[12] = new NpgsqlParameter("ldaprootdn", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[12].Value = ldapRootDN;

            arParams[13] = new NpgsqlParameter("ldapuserdnkey", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[13].Value = ldapUserDNKey;

            arParams[14] = new NpgsqlParameter("reallydeleteusers", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[14].Value = reallyDeleteUsers;

            arParams[15] = new NpgsqlParameter("useemailforlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[15].Value = useEmailForLogin;

            arParams[16] = new NpgsqlParameter("allowuserfullnamechange", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[16].Value = allowUserFullNameChange;
            
            arParams[17] = new NpgsqlParameter("recaptchaprivatekey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[17].Value = recaptchaPrivateKey;

            arParams[18] = new NpgsqlParameter("recaptchapublickey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[18].Value = recaptchaPublicKey;
            
            arParams[19] = new NpgsqlParameter("apikeyextra1", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[19].Value = apiKeyExtra1;

            arParams[20] = new NpgsqlParameter("apikeyextra2", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[20].Value = apiKeyExtra2;

            arParams[21] = new NpgsqlParameter("apikeyextra3", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[21].Value = apiKeyExtra3;

            arParams[22] = new NpgsqlParameter("apikeyextra4", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[22].Value = apiKeyExtra4;

            arParams[23] = new NpgsqlParameter("apikeyextra5", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[23].Value = apiKeyExtra5;

            arParams[24] = new NpgsqlParameter("disabledbauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[24].Value = disableDbAuth;

            arParams[25] = new NpgsqlParameter("requiresquestionandanswer", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[25].Value = requiresQuestionAndAnswer;

            arParams[26] = new NpgsqlParameter("maxinvalidpasswordattempts", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[26].Value = maxInvalidPasswordAttempts;

            arParams[27] = new NpgsqlParameter("passwordattemptwindowminutes", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[27].Value = passwordAttemptWindowMinutes;

            arParams[28] = new NpgsqlParameter("minrequiredpasswordlength", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[28].Value = minRequiredPasswordLength;

            arParams[29] = new NpgsqlParameter("minreqnonalphachars", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[29].Value = minReqNonAlphaChars;

            arParams[30] = new NpgsqlParameter("defaultemailfromaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[30].Value = defaultEmailFromAddress;

            arParams[31] = new NpgsqlParameter("allowdbfallbackwithldap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[31].Value = allowDbFallbackWithLdap;

            arParams[32] = new NpgsqlParameter("emailldapdbfallback", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[32].Value = emailLdapDbFallback;

            arParams[33] = new NpgsqlParameter("allowpersistentlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[33].Value = allowPersistentLogin;

            arParams[34] = new NpgsqlParameter("captchaonlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[34].Value = captchaOnLogin;

            arParams[35] = new NpgsqlParameter("captchaonregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[35].Value = captchaOnRegistration;

            arParams[36] = new NpgsqlParameter("siteisclosed", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[36].Value = siteIsClosed;

            arParams[37] = new NpgsqlParameter("siteisclosedmessage", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[37].Value = siteIsClosedMessage;

            arParams[38] = new NpgsqlParameter("privacypolicy", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[38].Value = privacyPolicy;

            arParams[39] = new NpgsqlParameter("timezoneid", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[39].Value = timeZoneId;

            arParams[40] = new NpgsqlParameter("googleanalyticsprofileid", NpgsqlTypes.NpgsqlDbType.Varchar, 25);
            arParams[40].Value = googleAnalyticsProfileId;

            arParams[41] = new NpgsqlParameter("companyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[41].Value = companyName;

            arParams[42] = new NpgsqlParameter("companystreetaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[42].Value = companyStreetAddress;

            arParams[43] = new NpgsqlParameter("companystreetaddress2", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[43].Value = companyStreetAddress2;

            arParams[44] = new NpgsqlParameter("companyregion", NpgsqlTypes.NpgsqlDbType.Varchar, 200);
            arParams[44].Value = companyRegion;

            arParams[45] = new NpgsqlParameter("companylocality", NpgsqlTypes.NpgsqlDbType.Varchar, 200);
            arParams[45].Value = companyLocality;

            arParams[46] = new NpgsqlParameter("companycountry", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[46].Value = companyCountry;

            arParams[47] = new NpgsqlParameter("companypostalcode", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[47].Value = companyPostalCode;

            arParams[48] = new NpgsqlParameter("companypublicemail", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[48].Value = companyPublicEmail;

            arParams[49] = new NpgsqlParameter("companyphone", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[49].Value = companyPhone;

            arParams[50] = new NpgsqlParameter("companyfax", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[50].Value = companyFax;

            arParams[51] = new NpgsqlParameter("facebookappid", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[51].Value = facebookAppId;

            arParams[52] = new NpgsqlParameter("facebookappsecret", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[52].Value = facebookAppSecret;

            arParams[53] = new NpgsqlParameter("googleclientid", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[53].Value = googleClientId;

            arParams[54] = new NpgsqlParameter("googleclientsecret", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[54].Value = googleClientSecret;

            arParams[55] = new NpgsqlParameter("twitterconsumerkey", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[55].Value = twitterConsumerKey;

            arParams[56] = new NpgsqlParameter("twitterconsumersecret", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[56].Value = twitterConsumerSecret;

            arParams[57] = new NpgsqlParameter("microsoftclientid", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[57].Value = microsoftClientId;

            arParams[58] = new NpgsqlParameter("microsoftclientsecret", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[58].Value = microsoftClientSecret;

            arParams[59] = new NpgsqlParameter("preferredhostname", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[59].Value = preferredHostName;

            arParams[60] = new NpgsqlParameter("sitefoldername", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[60].Value = siteFolderName;

            arParams[61] = new NpgsqlParameter("addthisdotcomusername", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[61].Value = addThisDotComUsername;

            arParams[62] = new NpgsqlParameter("logininfotop", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[62].Value = loginInfoTop;

            arParams[63] = new NpgsqlParameter("logininfobottom", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[63].Value = loginInfoBottom;

            arParams[64] = new NpgsqlParameter("registrationagreement", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[64].Value = registrationAgreement;

            arParams[65] = new NpgsqlParameter("registrationpreamble", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[65].Value = registrationPreamble;

            arParams[66] = new NpgsqlParameter("smtpserver", NpgsqlTypes.NpgsqlDbType.Varchar, 200);
            arParams[66].Value = smtpServer;

            arParams[67] = new NpgsqlParameter("smtpport", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[67].Value = smtpPort;

            arParams[68] = new NpgsqlParameter("smtpuser", NpgsqlTypes.NpgsqlDbType.Varchar, 500);
            arParams[68].Value = smtpUser;

            arParams[69] = new NpgsqlParameter("smtppassword", NpgsqlTypes.NpgsqlDbType.Varchar, 500);
            arParams[69].Value = smtpPassword;

            arParams[70] = new NpgsqlParameter("smtppreferredencoding", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[70].Value = smtpPreferredEncoding;

            arParams[71] = new NpgsqlParameter("smtprequiresauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[71].Value = smtpRequiresAuth;

            arParams[72] = new NpgsqlParameter("smtpusessl", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[72].Value = smtpUseSsl;

            arParams[73] = new NpgsqlParameter("requireapprovalbeforelogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[73].Value = requireApprovalBeforeLogin;



            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            int newID = Convert.ToInt32(result);

            return newID;

        }

        public async Task<bool> Update(
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
            sqlCommand.Append("UPDATE mp_sites ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("sitename = :sitename, ");
            sqlCommand.Append("skin = :skin, ");
            sqlCommand.Append("allownewregistration = :allownewregistration, ");
            sqlCommand.Append("usesecureregistration = :usesecureregistration, ");
            sqlCommand.Append("usesslonallpages = :usesslonallpages, ");
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
            sqlCommand.Append("allowuserfullnamechange = :allowuserfullnamechange, ");
            sqlCommand.Append("disabledbauth = :disabledbauth, ");
            sqlCommand.Append("recaptchaprivatekey = :recaptchaprivatekey, ");
            sqlCommand.Append("recaptchapublickey = :recaptchapublickey, ");
            sqlCommand.Append("apikeyextra1 = :apikeyextra1, ");
            sqlCommand.Append("apikeyextra2 = :apikeyextra2, ");
            sqlCommand.Append("apikeyextra3 = :apikeyextra3, ");
            sqlCommand.Append("apikeyextra4 = :apikeyextra4, ");
            sqlCommand.Append("apikeyextra5 = :apikeyextra5, ");

            sqlCommand.Append("requiresquestionandanswer = :requiresquestionandanswer, ");
            sqlCommand.Append("maxinvalidpasswordattempts = :maxinvalidpasswordattempts, ");
            sqlCommand.Append("passwordattemptwindowminutes = :passwordattemptwindowminutes, ");
            sqlCommand.Append("minrequiredpasswordlength = :minrequiredpasswordlength, ");
            sqlCommand.Append("minreqnonalphachars = :minreqnonalphachars, ");
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
            sqlCommand.Append("requireapprovalbeforelogin = :requireapprovalbeforelogin ");



            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[74];

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

            arParams[5] = new NpgsqlParameter("usesslonallpages", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Value = useSslOnAllPages;
            
            arParams[6] = new NpgsqlParameter("isserveradminsite", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Value = isServerAdminSite;

            arParams[7] = new NpgsqlParameter("useldapauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Value = useLdapAuth;

            arParams[8] = new NpgsqlParameter("autocreateldapuseronfirstlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Value = autoCreateLdapUserOnFirstLogin;

            arParams[9] = new NpgsqlParameter("ldapserver", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[9].Value = ldapServer;

            arParams[10] = new NpgsqlParameter("ldapport", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[10].Value = ldapPort;

            arParams[11] = new NpgsqlParameter("ldapdomain", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[11].Value = ldapDomain;

            arParams[12] = new NpgsqlParameter("ldaprootdn", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[12].Value = ldapRootDN;

            arParams[13] = new NpgsqlParameter("ldapuserdnkey", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[13].Value = ldapUserDNKey;

            arParams[14] = new NpgsqlParameter("reallydeleteusers", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[14].Value = reallyDeleteUsers;

            arParams[15] = new NpgsqlParameter("useemailforlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[15].Value = useEmailForLogin;

            arParams[16] = new NpgsqlParameter("allowuserfullnamechange", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[16].Value = allowUserFullNameChange;
            
            arParams[17] = new NpgsqlParameter("recaptchaprivatekey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[17].Value = recaptchaPrivateKey;

            arParams[18] = new NpgsqlParameter("recaptchapublickey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[18].Value = recaptchaPublicKey;
            
            arParams[19] = new NpgsqlParameter("apikeyextra1", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[19].Value = apiKeyExtra1;

            arParams[20] = new NpgsqlParameter("apikeyextra2", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[20].Value = apiKeyExtra2;

            arParams[21] = new NpgsqlParameter("apikeyextra3", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[21].Value = apiKeyExtra3;

            arParams[22] = new NpgsqlParameter("apikeyextra4", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[22].Value = apiKeyExtra4;

            arParams[23] = new NpgsqlParameter("apikeyextra5", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[23].Value = apiKeyExtra5;

            arParams[24] = new NpgsqlParameter("disabledbauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[24].Value = disableDbAuth;

            arParams[25] = new NpgsqlParameter("requiresquestionandanswer", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[25].Value = requiresQuestionAndAnswer;

            arParams[26] = new NpgsqlParameter("maxinvalidpasswordattempts", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[26].Value = maxInvalidPasswordAttempts;

            arParams[27] = new NpgsqlParameter("passwordattemptwindowminutes", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[27].Value = passwordAttemptWindowMinutes;

            arParams[28] = new NpgsqlParameter("minrequiredpasswordlength", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[28].Value = minRequiredPasswordLength;

            arParams[29] = new NpgsqlParameter("minreqnonalphachars", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[29].Value = minReqNonAlphaChars;

            arParams[30] = new NpgsqlParameter("defaultemailfromaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[30].Value = defaultEmailFromAddress;

            arParams[31] = new NpgsqlParameter("allowdbfallbackwithldap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[31].Value = allowDbFallbackWithLdap;

            arParams[32] = new NpgsqlParameter("emailldapdbfallback", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[32].Value = emailLdapDbFallback;

            arParams[33] = new NpgsqlParameter("allowpersistentlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[33].Value = allowPersistentLogin;

            arParams[34] = new NpgsqlParameter("captchaonlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[34].Value = captchaOnLogin;

            arParams[35] = new NpgsqlParameter("captchaonregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[35].Value = captchaOnRegistration;

            arParams[36] = new NpgsqlParameter("siteisclosed", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[36].Value = siteIsClosed;

            arParams[37] = new NpgsqlParameter("siteisclosedmessage", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[37].Value = siteIsClosedMessage;

            arParams[38] = new NpgsqlParameter("privacypolicy", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[38].Value = privacyPolicy;

            arParams[39] = new NpgsqlParameter("timezoneid", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[39].Value = timeZoneId;

            arParams[40] = new NpgsqlParameter("googleanalyticsprofileid", NpgsqlTypes.NpgsqlDbType.Varchar, 25);
            arParams[40].Value = googleAnalyticsProfileId;

            arParams[41] = new NpgsqlParameter("companyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[41].Value = companyName;

            arParams[42] = new NpgsqlParameter("companystreetaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[42].Value = companyStreetAddress;

            arParams[43] = new NpgsqlParameter("companystreetaddress2", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[43].Value = companyStreetAddress2;

            arParams[44] = new NpgsqlParameter("companyregion", NpgsqlTypes.NpgsqlDbType.Varchar, 200);
            arParams[44].Value = companyRegion;

            arParams[45] = new NpgsqlParameter("companylocality", NpgsqlTypes.NpgsqlDbType.Varchar, 200);
            arParams[45].Value = companyLocality;

            arParams[46] = new NpgsqlParameter("companycountry", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[46].Value = companyCountry;

            arParams[47] = new NpgsqlParameter("companypostalcode", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[47].Value = companyPostalCode;

            arParams[48] = new NpgsqlParameter("companypublicemail", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[48].Value = companyPublicEmail;

            arParams[49] = new NpgsqlParameter("companyphone", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[49].Value = companyPhone;

            arParams[50] = new NpgsqlParameter("companyfax", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[50].Value = companyFax;

            arParams[51] = new NpgsqlParameter("facebookappid", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[51].Value = facebookAppId;

            arParams[52] = new NpgsqlParameter("facebookappsecret", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[52].Value = facebookAppSecret;

            arParams[53] = new NpgsqlParameter("googleclientid", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[53].Value = googleClientId;

            arParams[54] = new NpgsqlParameter("googleclientsecret", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[54].Value = googleClientSecret;

            arParams[55] = new NpgsqlParameter("twitterconsumerkey", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[55].Value = twitterConsumerKey;

            arParams[56] = new NpgsqlParameter("twitterconsumersecret", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[56].Value = twitterConsumerSecret;

            arParams[57] = new NpgsqlParameter("microsoftclientid", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[57].Value = microsoftClientId;

            arParams[58] = new NpgsqlParameter("microsoftclientsecret", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[58].Value = microsoftClientSecret;

            arParams[59] = new NpgsqlParameter("preferredhostname", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[59].Value = preferredHostName;

            arParams[60] = new NpgsqlParameter("sitefoldername", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[60].Value = siteFolderName;

            arParams[61] = new NpgsqlParameter("addthisdotcomusername", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[61].Value = addThisDotComUsername;

            arParams[62] = new NpgsqlParameter("logininfotop", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[62].Value = loginInfoTop;

            arParams[63] = new NpgsqlParameter("logininfobottom", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[63].Value = loginInfoBottom;

            arParams[64] = new NpgsqlParameter("registrationagreement", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[64].Value = registrationAgreement;

            arParams[65] = new NpgsqlParameter("registrationpreamble", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[65].Value = registrationPreamble;

            arParams[66] = new NpgsqlParameter("smtpserver", NpgsqlTypes.NpgsqlDbType.Varchar, 200);
            arParams[66].Value = smtpServer;

            arParams[67] = new NpgsqlParameter("smtpport", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[67].Value = smtpPort;

            arParams[68] = new NpgsqlParameter("smtpuser", NpgsqlTypes.NpgsqlDbType.Varchar, 500);
            arParams[68].Value = smtpUser;

            arParams[69] = new NpgsqlParameter("smtppassword", NpgsqlTypes.NpgsqlDbType.Varchar, 500);
            arParams[69].Value = smtpPassword;

            arParams[70] = new NpgsqlParameter("smtppreferredencoding", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[70].Value = smtpPreferredEncoding;

            arParams[71] = new NpgsqlParameter("smtprequiresauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[71].Value = smtpRequiresAuth;

            arParams[72] = new NpgsqlParameter("smtpusessl", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[72].Value = smtpUseSsl;

            arParams[73] = new NpgsqlParameter("requireapprovalbeforelogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[73].Value = requireApprovalBeforeLogin;



            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
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



        public async Task<bool> Delete(int siteId)
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
                arParams);

            return (rowsAffected > 0);

        }

        public async Task<DbDataReader> GetSiteList()
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
                null);

        }

        public async Task<DbDataReader> GetSite(string hostName)
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

            //return await AdoHelper.ExecuteReaderAsync(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sites_selectonebyhostv2(:hostname)",
            //    arParams);

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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

            //return AdoHelper.ExecuteReader(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sites_selectonebyhostv2(:hostname)",
            //    arParams);

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }


        public async Task<DbDataReader> GetSite(int siteId)
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

            //return await AdoHelper.ExecuteReaderAsync(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sites_selectonev2(:siteid)",
            //    arParams);

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
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

            //return AdoHelper.ExecuteReader(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sites_selectonev2(:siteid)",
            //    arParams);

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public async Task<DbDataReader> GetSite(Guid siteGuid)
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

            //return await AdoHelper.ExecuteReaderAsync(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sites_selectonebyguidv2(:siteguid)",
            //    arParams);

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
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

            //return AdoHelper.ExecuteReader(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sites_selectonebyguidv2(:siteguid)",
            //    arParams);

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public async Task<int> GetHostCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_sitehosts ");
            sqlCommand.Append(";");

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null);

            return Convert.ToInt32(result);

        }

        public async Task<DbDataReader> GetHost(string hostName)
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
                arParams);
        }

        public async Task<DbDataReader> GetAllHosts()
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
                null);

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
                arParams);

        }


        public async Task<DbDataReader> GetHostList(int siteId)
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

            //return await AdoHelper.ExecuteReaderAsync(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sitehosts_select(:siteid)",
            //    arParams);

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public async Task<bool> AddHost(Guid siteGuid, int siteId, string hostName)
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

            //int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sitehosts_insert(:siteid,:hostname,:siteguid)",
            //    arParams);

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteHost(int hostId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("hostid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = hostId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_sitehosts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("hostid = :hostid ");
            sqlCommand.Append(";");

            //int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_sitehosts_delete(:hostid)",
            //    arParams);

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected > 0;
        }


        public async Task<int> CountOtherSites(int currentSiteId)
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
                arParams);

            return Convert.ToInt32(result);
        }

        public async Task<DbDataReader> GetPageOfOtherSites(
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
                arParams);

        }

        public async Task<int> GetSiteIdByHostName(string hostName)
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

                using (DbDataReader reader = await AdoHelper.ExecuteReaderAsync(
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

        public async Task<int> GetSiteIdByFolder(string folderName)
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

                using (DbDataReader reader = await AdoHelper.ExecuteReaderAsync(
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
