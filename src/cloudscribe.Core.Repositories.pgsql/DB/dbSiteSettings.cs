// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-01-15
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Text;
using Npgsql;
using cloudscribe.DbHelpers.pgsql;

namespace cloudscribe.Core.Repositories.pgsql
{
    
    internal static class DBSiteSettings
    {
        
        public static int Create(
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
            sqlCommand.Append("INSERT INTO mp_sites (");
            sqlCommand.Append("siteguid, ");
    
            sqlCommand.Append("sitename, ");
            sqlCommand.Append("skin, ");
            sqlCommand.Append("logo, ");
            sqlCommand.Append("icon, ");
            sqlCommand.Append("allowuserskins, ");
            sqlCommand.Append("allowpageskins, ");
            sqlCommand.Append("allowhidemenuonpages, ");
            sqlCommand.Append("allownewregistration, ");
            sqlCommand.Append("usesecureregistration, ");
            sqlCommand.Append("usesslonallpages, ");
            sqlCommand.Append("defaultpagekeywords, ");
            sqlCommand.Append("defaultpagedescription, ");
            sqlCommand.Append("defaultpageencoding, ");
            sqlCommand.Append("defaultadditionalmetatags, ");
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
            sqlCommand.Append("editorskin, ");
            sqlCommand.Append("defaultfriendlyurlpatternenum, ");
            sqlCommand.Append("disabledbauth, ");
            
            sqlCommand.Append("enablemypagefeature, ");
            sqlCommand.Append("editorprovider, ");
            sqlCommand.Append("captchaprovider, ");
            sqlCommand.Append("datepickerprovider, ");
            sqlCommand.Append("recaptchaprivatekey, ");
            sqlCommand.Append("recaptchapublickey, ");
            sqlCommand.Append("wordpressapikey, ");
            sqlCommand.Append("windowsliveappid, ");
            sqlCommand.Append("windowslivekey, ");
            sqlCommand.Append("allowopenidauth, ");
            sqlCommand.Append("allowwindowsliveauth, ");
            sqlCommand.Append("gmapapikey, ");
            sqlCommand.Append("apikeyextra1, ");
            sqlCommand.Append("apikeyextra2, ");
            sqlCommand.Append("apikeyextra3, ");
            sqlCommand.Append("apikeyextra4, ");
            sqlCommand.Append("apikeyextra5 )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":siteguid, ");
     
            sqlCommand.Append(":sitename, ");
            sqlCommand.Append(":skin, ");
            sqlCommand.Append(":logo, ");
            sqlCommand.Append(":icon, ");
            sqlCommand.Append(":allowuserskins, ");
            sqlCommand.Append(":allowpageskins, ");
            sqlCommand.Append(":allowhidemenuonpages, ");
            sqlCommand.Append(":allownewregistration, ");
            sqlCommand.Append(":usesecureregistration, ");
            sqlCommand.Append(":usesslonallpages, ");
            sqlCommand.Append(":defaultpagekeywords, ");
            sqlCommand.Append(":defaultpagedescription, ");
            sqlCommand.Append(":defaultpageencoding, ");
            sqlCommand.Append(":defaultadditionalmetatags, ");
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
            sqlCommand.Append(":editorskin, ");
            sqlCommand.Append(":defaultfriendlyurlpatternenum, ");
            sqlCommand.Append(":disabledbauth, ");
            
            sqlCommand.Append(":enablemypagefeature, ");
            sqlCommand.Append(":editorprovider, ");
            sqlCommand.Append(":captchaprovider, ");
            sqlCommand.Append(":datepickerprovider, ");
            sqlCommand.Append(":recaptchaprivatekey, ");
            sqlCommand.Append(":recaptchapublickey, ");
            sqlCommand.Append(":wordpressapikey, ");
            sqlCommand.Append(":windowsliveappid, ");
            sqlCommand.Append(":windowslivekey, ");
            sqlCommand.Append(":allowopenidauth, ");
            sqlCommand.Append(":allowwindowsliveauth, ");
            sqlCommand.Append(":gmapapikey, ");
            sqlCommand.Append(":apikeyextra1, ");
            sqlCommand.Append(":apikeyextra2, ");
            sqlCommand.Append(":apikeyextra3, ");
            sqlCommand.Append(":apikeyextra4, ");
            sqlCommand.Append(":apikeyextra5 )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_sites_siteid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[46];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter("sitename", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = siteName;

            arParams[2] = new NpgsqlParameter("skin", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[2].Value = skin;

            arParams[3] = new NpgsqlParameter("logo", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[3].Value = logo;

            arParams[4] = new NpgsqlParameter("icon", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[4].Value = icon;

            arParams[5] = new NpgsqlParameter("allowuserskins", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Value = allowUserSkins;

            arParams[6] = new NpgsqlParameter("allowpageskins", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Value = allowPageSkins;

            arParams[7] = new NpgsqlParameter("allowhidemenuonpages", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Value = allowHideMenuOnPages;

            arParams[8] = new NpgsqlParameter("allownewregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Value = allowNewRegistration;

            arParams[9] = new NpgsqlParameter("usesecureregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[9].Value = useSecureRegistration;

            arParams[10] = new NpgsqlParameter("usesslonallpages", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[10].Value = useSslOnAllPages;

            arParams[11] = new NpgsqlParameter("defaultpagekeywords", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[11].Value = defaultPageKeywords;

            arParams[12] = new NpgsqlParameter("defaultpagedescription", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[12].Value = defaultPageDescription;

            arParams[13] = new NpgsqlParameter("defaultpageencoding", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[13].Value = defaultPageEncoding;

            arParams[14] = new NpgsqlParameter("defaultadditionalmetatags", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[14].Value = defaultAdditionalMetaTags;

            arParams[15] = new NpgsqlParameter("isserveradminsite", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[15].Value = isServerAdminSite;

            arParams[16] = new NpgsqlParameter("useldapauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[16].Value = useLdapAuth;

            arParams[17] = new NpgsqlParameter("autocreateldapuseronfirstlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[17].Value = autoCreateLdapUserOnFirstLogin;

            arParams[18] = new NpgsqlParameter("ldapserver", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[18].Value = ldapServer;

            arParams[19] = new NpgsqlParameter("ldapport", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[19].Value = ldapPort;

            arParams[20] = new NpgsqlParameter("ldapdomain", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[20].Value = ldapDomain;

            arParams[21] = new NpgsqlParameter("ldaprootdn", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[21].Value = ldapRootDN;

            arParams[22] = new NpgsqlParameter("ldapuserdnkey", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[22].Value = ldapUserDNKey;

            arParams[23] = new NpgsqlParameter("reallydeleteusers", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[23].Value = reallyDeleteUsers;

            arParams[24] = new NpgsqlParameter("useemailforlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[24].Value = useEmailForLogin;

            arParams[25] = new NpgsqlParameter("allowuserfullnamechange", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[25].Value = allowUserFullNameChange;

            arParams[26] = new NpgsqlParameter("editorskin", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[26].Value = editorSkin;

            arParams[27] = new NpgsqlParameter("defaultfriendlyurlpatternenum", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[27].Value = defaultFriendlyUrlPattern;

            arParams[28] = new NpgsqlParameter("enablemypagefeature", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[28].Value = enableMyPageFeature;

            arParams[29] = new NpgsqlParameter("editorprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[29].Value = editorProvider;

            arParams[30] = new NpgsqlParameter("captchaprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[30].Value = captchaProvider;

            arParams[31] = new NpgsqlParameter("datepickerprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[31].Value = datePickerProvider;

            arParams[32] = new NpgsqlParameter("recaptchaprivatekey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[32].Value = recaptchaPrivateKey;

            arParams[33] = new NpgsqlParameter("recaptchapublickey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[33].Value = recaptchaPublicKey;

            arParams[34] = new NpgsqlParameter("wordpressapikey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[34].Value = wordpressApiKey;

            arParams[35] = new NpgsqlParameter("windowsliveappid", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[35].Value = windowsLiveAppId;

            arParams[36] = new NpgsqlParameter("windowslivekey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[36].Value = windowsLiveKey;

            arParams[37] = new NpgsqlParameter("allowopenidauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[37].Value = allowOpenIdAuth;

            arParams[38] = new NpgsqlParameter("allowwindowsliveauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[38].Value = allowWindowsLiveAuth;

            arParams[39] = new NpgsqlParameter("gmapapikey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[39].Value = gmapApiKey;

            arParams[40] = new NpgsqlParameter("apikeyextra1", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[40].Value = apiKeyExtra1;

            arParams[41] = new NpgsqlParameter("apikeyextra2", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[41].Value = apiKeyExtra2;

            arParams[42] = new NpgsqlParameter("apikeyextra3", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[42].Value = apiKeyExtra3;

            arParams[43] = new NpgsqlParameter("apikeyextra4", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[43].Value = apiKeyExtra4;

            arParams[44] = new NpgsqlParameter("apikeyextra5", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[44].Value = apiKeyExtra5;

            arParams[45] = new NpgsqlParameter("disabledbauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[45].Value = disableDbAuth;

            int newID = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newID;

        }

        public static bool Update(
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
            sqlCommand.Append("UPDATE mp_sites ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("sitename = :sitename, ");
            sqlCommand.Append("skin = :skin, ");
            sqlCommand.Append("logo = :logo, ");
            sqlCommand.Append("icon = :icon, ");
            sqlCommand.Append("allowuserskins = :allowuserskins, ");
            sqlCommand.Append("allowpageskins = :allowpageskins, ");
            sqlCommand.Append("allowhidemenuonpages = :allowhidemenuonpages, ");
            sqlCommand.Append("allownewregistration = :allownewregistration, ");
            sqlCommand.Append("usesecureregistration = :usesecureregistration, ");
            sqlCommand.Append("usesslonallpages = :usesslonallpages, ");
            sqlCommand.Append("defaultpagekeywords = :defaultpagekeywords, ");
            sqlCommand.Append("defaultpagedescription = :defaultpagedescription, ");
            sqlCommand.Append("defaultpageencoding = :defaultpageencoding, ");
            sqlCommand.Append("defaultadditionalmetatags = :defaultadditionalmetatags, ");
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
            sqlCommand.Append("editorskin = :editorskin, ");
            sqlCommand.Append("defaultfriendlyurlpatternenum = :defaultfriendlyurlpatternenum, ");
            sqlCommand.Append("disabledbauth = :disabledbauth, ");
            sqlCommand.Append("enablemypagefeature = :enablemypagefeature, ");
            sqlCommand.Append("editorprovider = :editorprovider, ");
            sqlCommand.Append("captchaprovider = :captchaprovider, ");
            sqlCommand.Append("datepickerprovider = :datepickerprovider, ");
            sqlCommand.Append("recaptchaprivatekey = :recaptchaprivatekey, ");
            sqlCommand.Append("recaptchapublickey = :recaptchapublickey, ");
            sqlCommand.Append("wordpressapikey = :wordpressapikey, ");
            sqlCommand.Append("windowsliveappid = :windowsliveappid, ");
            sqlCommand.Append("windowslivekey = :windowslivekey, ");
            sqlCommand.Append("allowopenidauth = :allowopenidauth, ");
            sqlCommand.Append("allowwindowsliveauth = :allowwindowsliveauth, ");
            sqlCommand.Append("gmapapikey = :gmapapikey, ");
            sqlCommand.Append("apikeyextra1 = :apikeyextra1, ");
            sqlCommand.Append("apikeyextra2 = :apikeyextra2, ");
            sqlCommand.Append("apikeyextra3 = :apikeyextra3, ");
            sqlCommand.Append("apikeyextra4 = :apikeyextra4, ");
            sqlCommand.Append("apikeyextra5 = :apikeyextra5 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[46];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("sitename", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = siteName;

            arParams[2] = new NpgsqlParameter("skin", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[2].Value = skin;

            arParams[3] = new NpgsqlParameter("logo", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[3].Value = logo;

            arParams[4] = new NpgsqlParameter("icon", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[4].Value = icon;

            arParams[5] = new NpgsqlParameter("allowuserskins", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Value = allowUserSkins;

            arParams[6] = new NpgsqlParameter("allowpageskins", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Value = allowPageSkins;

            arParams[7] = new NpgsqlParameter("allowhidemenuonpages", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Value = allowHideMenuOnPages;

            arParams[8] = new NpgsqlParameter("allownewregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Value = allowNewRegistration;

            arParams[9] = new NpgsqlParameter("usesecureregistration", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[9].Value = useSecureRegistration;

            arParams[10] = new NpgsqlParameter("usesslonallpages", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[10].Value = useSslOnAllPages;

            arParams[11] = new NpgsqlParameter("defaultpagekeywords", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[11].Value = defaultPageKeywords;

            arParams[12] = new NpgsqlParameter("defaultpagedescription", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[12].Value = defaultPageDescription;

            arParams[13] = new NpgsqlParameter("defaultpageencoding", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[13].Value = defaultPageEncoding;

            arParams[14] = new NpgsqlParameter("defaultadditionalmetatags", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[14].Value = defaultAdditionalMetaTags;

            arParams[15] = new NpgsqlParameter("isserveradminsite", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[15].Value = isServerAdminSite;

            arParams[16] = new NpgsqlParameter("useldapauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[16].Value = useLdapAuth;

            arParams[17] = new NpgsqlParameter("autocreateldapuseronfirstlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[17].Value = autoCreateLdapUserOnFirstLogin;

            arParams[18] = new NpgsqlParameter("ldapserver", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[18].Value = ldapServer;

            arParams[19] = new NpgsqlParameter("ldapport", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[19].Value = ldapPort;

            arParams[20] = new NpgsqlParameter("ldapdomain", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[20].Value = ldapDomain;

            arParams[21] = new NpgsqlParameter("ldaprootdn", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[21].Value = ldapRootDN;

            arParams[22] = new NpgsqlParameter("ldapuserdnkey", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[22].Value = ldapUserDNKey;

            arParams[23] = new NpgsqlParameter("reallydeleteusers", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[23].Value = reallyDeleteUsers;

            arParams[24] = new NpgsqlParameter("useemailforlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[24].Value = useEmailForLogin;

            arParams[25] = new NpgsqlParameter("allowuserfullnamechange", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[25].Value = allowUserFullNameChange;

            arParams[26] = new NpgsqlParameter("editorskin", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[26].Value = editorSkin;

            arParams[27] = new NpgsqlParameter("defaultfriendlyurlpatternenum", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[27].Value = defaultFriendlyUrlPattern;

            arParams[28] = new NpgsqlParameter("enablemypagefeature", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[28].Value = enableMyPageFeature;

            arParams[29] = new NpgsqlParameter("editorprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[29].Value = editorProvider;

            arParams[30] = new NpgsqlParameter("captchaprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[30].Value = captchaProvider;

            arParams[31] = new NpgsqlParameter("datepickerprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[31].Value = datePickerProvider;

            arParams[32] = new NpgsqlParameter("recaptchaprivatekey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[32].Value = recaptchaPrivateKey;

            arParams[33] = new NpgsqlParameter("recaptchapublickey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[33].Value = recaptchaPublicKey;

            arParams[34] = new NpgsqlParameter("wordpressapikey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[34].Value = wordpressApiKey;

            arParams[35] = new NpgsqlParameter("windowsliveappid", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[35].Value = windowsLiveAppId;

            arParams[36] = new NpgsqlParameter("windowslivekey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[36].Value = windowsLiveKey;

            arParams[37] = new NpgsqlParameter("allowopenidauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[37].Value = allowOpenIdAuth;

            arParams[38] = new NpgsqlParameter("allowwindowsliveauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[38].Value = allowWindowsLiveAuth;

            arParams[39] = new NpgsqlParameter("gmapapikey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[39].Value = gmapApiKey;

            arParams[40] = new NpgsqlParameter("apikeyextra1", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[40].Value = apiKeyExtra1;

            arParams[41] = new NpgsqlParameter("apikeyextra2", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[41].Value = apiKeyExtra2;

            arParams[42] = new NpgsqlParameter("apikeyextra3", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[42].Value = apiKeyExtra3;

            arParams[43] = new NpgsqlParameter("apikeyextra4", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[43].Value = apiKeyExtra4;

            arParams[44] = new NpgsqlParameter("apikeyextra5", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[44].Value = apiKeyExtra5;

            arParams[45] = new NpgsqlParameter("disabledbauth", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[45].Value = disableDbAuth;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateRelatedSites(
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
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateRelatedSitesWindowsLive(
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
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static bool UpdateExtendedProperties(
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[12];
            
            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("allowpasswordretrieval", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[1].Value = allowPasswordRetrieval;

            arParams[2] = new NpgsqlParameter("allowpasswordreset", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[2].Value = allowPasswordReset;

            arParams[3] = new NpgsqlParameter("requiresquestionandanswer", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[3].Value = requiresQuestionAndAnswer;

            arParams[4] = new NpgsqlParameter("maxinvalidpasswordattempts", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Value = maxInvalidPasswordAttempts;

            arParams[5] = new NpgsqlParameter("passwordattemptwindowminutes", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Value = passwordAttemptWindowMinutes;

            arParams[6] = new NpgsqlParameter("requiresuniqueemail", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Value = requiresUniqueEmail;

            arParams[7] = new NpgsqlParameter("passwordformat", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[7].Value = passwordFormat;

            arParams[8] = new NpgsqlParameter("minrequiredpasswordlength", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[8].Value = minRequiredPasswordLength;

            arParams[9] = new NpgsqlParameter("minrequirednonalphanumericcharacters", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[9].Value = minRequiredNonAlphanumericCharacters;

            arParams[10] = new NpgsqlParameter("passwordstrengthregularexpression", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[10].Value = passwordStrengthRegularExpression;

            arParams[11] = new NpgsqlParameter("defaultemailfromaddress", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[11].Value = defaultEmailFromAddress;

            int rowsAffected = Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_sites_updateextendedproperties(:siteid,:allowpasswordretrieval,:allowpasswordreset,:requiresquestionandanswer,:maxinvalidpasswordattempts,:passwordattemptwindowminutes,:requiresuniqueemail,:passwordformat,:minrequiredpasswordlength,:minrequirednonalphanumericcharacters,:passwordstrengthregularexpression,:defaultemailfromaddress)",
                arParams));

            return (rowsAffected > -1);

        }

        

        public static bool Delete(int siteId)
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

            sqlCommand.Append("DELETE FROM mp_redirectlist WHERE siteguid IN (SELECT siteguid FROM mp_sites WHERE siteid = :siteid);");
            sqlCommand.Append("DELETE FROM mp_taskqueue WHERE siteguid IN (SELECT siteguid FROM mp_sites WHERE siteid = :siteid);");
            
            sqlCommand.Append("DELETE FROM mp_sites ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");

            int rowsAffected = AdoHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static IDataReader GetSiteList()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sites ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("sitename ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }

        public static IDataReader GetSite(string hostName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("hostname", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[0].Value = hostName;
           
            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_sites_selectonebyhostv2(:hostname)",
                arParams);
        }


        public static IDataReader GetSite(int siteId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;
            
            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_sites_selectonev2(:siteid)",
                arParams);
        }

        public static IDataReader GetSite(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = siteGuid.ToString();
            
            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_sites_selectonebyguidv2(:siteguid)",
                arParams);
        }

        public static int GetHostCount()
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_sitehosts ");
            sqlCommand.Append(";");

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));


        }

        public static IDataReader GetAllHosts()
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sitehosts ");
            sqlCommand.Append("ORDER BY hostname  ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);


        }

        public static IDataReader GetPageHosts(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetHostCount();

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }



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

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        
        public static IDataReader GetHostList(int siteId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;
            
            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_sitehosts_select(:siteid)",
                arParams);
        }

        public static void AddHost(Guid siteGuid, int siteId, string hostName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("hostname", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[1].Value = hostName;

            arParams[2] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Value = siteGuid.ToString();
            
            AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_sitehosts_insert(:siteid,:hostname,:siteguid)",
                arParams);
        }

        public static void DeleteHost(int hostId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("hostid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = hostId;
           
            AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_sitehosts_delete(:hostid)",
                arParams);
        }


        public static int CountOtherSites(int currentSiteId)
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

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        public static IDataReader GetPageOfOtherSites(
            int currentSiteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = CountOtherSites(currentSiteId);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

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

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetSiteIdByHostName(string hostName)
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

            using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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

                using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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

        public static int GetSiteIdByFolder(string folderName)
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

            using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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

                using (IDataReader reader = AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
