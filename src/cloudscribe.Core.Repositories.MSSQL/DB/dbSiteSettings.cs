// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2015-01-16
// 
// You must not remove this notice, or any other, from this software.


using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using cloudscribe.DbHelpers.MSSQL;


namespace cloudscribe.Core.Repositories.MSSQL
{
    
    internal static class DBSiteSettings
    {

        public static async Task<int> Create(
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Sites_Insert", 46);
            sph.DefineSqlParameter("@SiteName", SqlDbType.NVarChar, 128, ParameterDirection.Input, siteName);
            sph.DefineSqlParameter("@Skin", SqlDbType.NVarChar, 100, ParameterDirection.Input, skin);
            sph.DefineSqlParameter("@Logo", SqlDbType.NVarChar, 50, ParameterDirection.Input, logo);
            sph.DefineSqlParameter("@Icon", SqlDbType.NVarChar, 50, ParameterDirection.Input, icon);
            sph.DefineSqlParameter("@AllowUserSkins", SqlDbType.Bit, ParameterDirection.Input, allowUserSkins);
            sph.DefineSqlParameter("@AllowNewRegistration", SqlDbType.Bit, ParameterDirection.Input, allowNewRegistration);
            sph.DefineSqlParameter("@UseSecureRegistration", SqlDbType.Bit, ParameterDirection.Input, useSecureRegistration);
            sph.DefineSqlParameter("@UseSSLOnAllPages", SqlDbType.Bit, ParameterDirection.Input, useSslOnAllPages);
            sph.DefineSqlParameter("@DefaultPageKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultPageKeywords);
            sph.DefineSqlParameter("@DefaultPageDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultPageDescription);
            sph.DefineSqlParameter("@DefaultPageEncoding", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultPageEncoding);
            sph.DefineSqlParameter("@DefaultAdditionalMetaTags", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultAdditionalMetaTags);
            sph.DefineSqlParameter("@IsServerAdminSite", SqlDbType.Bit, ParameterDirection.Input, isServerAdminSite);
            sph.DefineSqlParameter("@AllowPageSkins", SqlDbType.Bit, ParameterDirection.Input, allowPageSkins);
            sph.DefineSqlParameter("@AllowHideMenuOnPages", SqlDbType.Bit, ParameterDirection.Input, allowHideMenuOnPages);
            sph.DefineSqlParameter("@UseLdapAuth", SqlDbType.Bit, ParameterDirection.Input, useLdapAuth);
            sph.DefineSqlParameter("@AutoCreateLDAPUserOnFirstLogin", SqlDbType.Bit, ParameterDirection.Input, autoCreateLdapUserOnFirstLogin);
            sph.DefineSqlParameter("@LdapServer", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapServer);
            sph.DefineSqlParameter("@LdapPort", SqlDbType.Int, ParameterDirection.Input, ldapPort);
            sph.DefineSqlParameter("@LdapDomain", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapDomain);
            sph.DefineSqlParameter("@LdapRootDN", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapRootDN);
            sph.DefineSqlParameter("@LdapUserDNKey", SqlDbType.NVarChar, 10, ParameterDirection.Input, ldapUserDNKey);
            sph.DefineSqlParameter("@AllowUserFullNameChange", SqlDbType.Bit, ParameterDirection.Input, allowUserFullNameChange);
            sph.DefineSqlParameter("@UseEmailForLogin", SqlDbType.Bit, ParameterDirection.Input, useEmailForLogin);
            sph.DefineSqlParameter("@ReallyDeleteUsers", SqlDbType.Bit, ParameterDirection.Input, reallyDeleteUsers);
            sph.DefineSqlParameter("@EditorSkin", SqlDbType.NVarChar, 50, ParameterDirection.Input, editorSkin);
            sph.DefineSqlParameter("@DefaultFriendlyUrlPatternEnum", SqlDbType.NVarChar, 50, ParameterDirection.Input, defaultFriendlyUrlPattern);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@EnableMyPageFeature", SqlDbType.Bit, ParameterDirection.Input, enableMyPageFeature);
            sph.DefineSqlParameter("@EditorProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, editorProvider);
            sph.DefineSqlParameter("@DatePickerProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, datePickerProvider);
            sph.DefineSqlParameter("@CaptchaProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, captchaProvider);
            sph.DefineSqlParameter("@RecaptchaPrivateKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, recaptchaPrivateKey);
            sph.DefineSqlParameter("@RecaptchaPublicKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, recaptchaPublicKey);
            sph.DefineSqlParameter("@WordpressAPIKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, wordpressApiKey);
            sph.DefineSqlParameter("@WindowsLiveAppID", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveAppId);
            sph.DefineSqlParameter("@WindowsLiveKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveKey);
            sph.DefineSqlParameter("@AllowOpenIDAuth", SqlDbType.Bit, ParameterDirection.Input, allowOpenIdAuth);
            sph.DefineSqlParameter("@AllowWindowsLiveAuth", SqlDbType.Bit, ParameterDirection.Input, allowWindowsLiveAuth);

            sph.DefineSqlParameter("@GmapApiKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, gmapApiKey);
            sph.DefineSqlParameter("@ApiKeyExtra1", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra1);
            sph.DefineSqlParameter("@ApiKeyExtra2", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra2);
            sph.DefineSqlParameter("@ApiKeyExtra3", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra3);
            sph.DefineSqlParameter("@ApiKeyExtra4", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra4);
            sph.DefineSqlParameter("@ApiKeyExtra5", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra5);
            sph.DefineSqlParameter("@DisableDbAuth", SqlDbType.Bit, ParameterDirection.Input, disableDbAuth);
            object result = await sph.ExecuteScalarAsync();
            int newID = Convert.ToInt32(result);
            return newID;
        }

        public static async Task<bool> Update(
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Sites_Update", 46);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SiteName", SqlDbType.NVarChar, 128, ParameterDirection.Input, siteName);
            sph.DefineSqlParameter("@Skin", SqlDbType.NVarChar, 100, ParameterDirection.Input, skin);
            sph.DefineSqlParameter("@Logo", SqlDbType.NVarChar, 50, ParameterDirection.Input, logo);
            sph.DefineSqlParameter("@Icon", SqlDbType.NVarChar, 50, ParameterDirection.Input, icon);
            sph.DefineSqlParameter("@AllowNewRegistration", SqlDbType.Bit, ParameterDirection.Input, allowNewRegistration);
            sph.DefineSqlParameter("@AllowUserSkins", SqlDbType.Bit, ParameterDirection.Input, allowUserSkins);
            sph.DefineSqlParameter("@UseSecureRegistration", SqlDbType.Bit, ParameterDirection.Input, useSecureRegistration);
            sph.DefineSqlParameter("@UseSSLOnAllPages", SqlDbType.Bit, ParameterDirection.Input, useSslOnAllPages);
            sph.DefineSqlParameter("@DefaultPageKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultPageKeywords);
            sph.DefineSqlParameter("@DefaultPageDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultPageDescription);
            sph.DefineSqlParameter("@DefaultPageEncoding", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultPageEncoding);
            sph.DefineSqlParameter("@DefaultAdditionalMetaTags", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultAdditionalMetaTags);
            sph.DefineSqlParameter("@IsServerAdminSite", SqlDbType.Bit, ParameterDirection.Input, isServerAdminSite);
            sph.DefineSqlParameter("@AllowPageSkins", SqlDbType.Bit, ParameterDirection.Input, allowPageSkins);
            sph.DefineSqlParameter("@AllowHideMenuOnPages", SqlDbType.Bit, ParameterDirection.Input, allowHideMenuOnPages);
            sph.DefineSqlParameter("@UseLdapAuth", SqlDbType.Bit, ParameterDirection.Input, useLdapAuth);
            sph.DefineSqlParameter("@AutoCreateLDAPUserOnFirstLogin", SqlDbType.Bit, ParameterDirection.Input, autoCreateLdapUserOnFirstLogin);
            sph.DefineSqlParameter("@LdapServer", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapServer);
            sph.DefineSqlParameter("@LdapPort", SqlDbType.Int, ParameterDirection.Input, ldapPort);
            sph.DefineSqlParameter("@LdapRootDN", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapRootDN);
            sph.DefineSqlParameter("@LdapUserDNKey", SqlDbType.NVarChar, 10, ParameterDirection.Input, ldapUserDNKey);
            sph.DefineSqlParameter("@AllowUserFullNameChange", SqlDbType.Bit, ParameterDirection.Input, allowUserFullNameChange);
            sph.DefineSqlParameter("@UseEmailForLogin", SqlDbType.Bit, ParameterDirection.Input, useEmailForLogin);
            sph.DefineSqlParameter("@ReallyDeleteUsers", SqlDbType.Bit, ParameterDirection.Input, reallyDeleteUsers);
            sph.DefineSqlParameter("@EditorSkin", SqlDbType.NVarChar, 50, ParameterDirection.Input, editorSkin);
            sph.DefineSqlParameter("@DefaultFriendlyUrlPatternEnum", SqlDbType.NVarChar, 50, ParameterDirection.Input, defaultFriendlyUrlPattern);
            sph.DefineSqlParameter("@EnableMyPageFeature", SqlDbType.Bit, ParameterDirection.Input, enableMyPageFeature);
            sph.DefineSqlParameter("@LdapDomain", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapDomain);
            sph.DefineSqlParameter("@EditorProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, editorProvider);
            sph.DefineSqlParameter("@DatePickerProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, datePickerProvider);
            sph.DefineSqlParameter("@CaptchaProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, captchaProvider);
            sph.DefineSqlParameter("@RecaptchaPrivateKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, recaptchaPrivateKey);
            sph.DefineSqlParameter("@RecaptchaPublicKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, recaptchaPublicKey);
            sph.DefineSqlParameter("@WordpressAPIKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, wordpressApiKey);
            sph.DefineSqlParameter("@WindowsLiveAppID", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveAppId);
            sph.DefineSqlParameter("@WindowsLiveKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveKey);
            sph.DefineSqlParameter("@AllowOpenIDAuth", SqlDbType.Bit, ParameterDirection.Input, allowOpenIdAuth);
            sph.DefineSqlParameter("@AllowWindowsLiveAuth", SqlDbType.Bit, ParameterDirection.Input, allowWindowsLiveAuth);
            sph.DefineSqlParameter("@GmapApiKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, gmapApiKey);
            sph.DefineSqlParameter("@ApiKeyExtra1", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra1);
            sph.DefineSqlParameter("@ApiKeyExtra2", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra2);
            sph.DefineSqlParameter("@ApiKeyExtra3", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra3);
            sph.DefineSqlParameter("@ApiKeyExtra4", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra4);
            sph.DefineSqlParameter("@ApiKeyExtra5", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra5);
            sph.DefineSqlParameter("@DisableDbAuth", SqlDbType.Bit, ParameterDirection.Input, disableDbAuth);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Sites_UpdateExtendedProperties", 12);
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Sites_UpdateRelatedSiteSecurity", 24);
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

        public static bool UpdateRelatedSitesWindowsLive(
            int siteId,
            string windowsLiveAppId,
            string windowsLiveKey
            )
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Sites_SyncRelatedSitesWinLive", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@WindowsLiveAppID", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveAppId);
            sph.DefineSqlParameter("@WindowsLiveKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveKey);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }




        public static async Task<bool> Delete(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Sites_Delete", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
           // sph.ExecuteNonQuery();

            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return (rowsAffected > -1);
        }

        public static async Task<DbDataReader> GetSiteList()
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_SelectAll", 0);
            return await sph.ExecuteReaderAsync();
        }

        public static async Task<DbDataReader> GetSite(string hostName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_SelectOneByHost", 1);
            sph.DefineSqlParameter("@HostName", SqlDbType.NVarChar, 50, ParameterDirection.Input, hostName);
            return await sph.ExecuteReaderAsync();

        }

        public static DbDataReader GetSiteNonAsync(string hostName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_SelectOneByHost", 1);
            sph.DefineSqlParameter("@HostName", SqlDbType.NVarChar, 50, ParameterDirection.Input, hostName);
            return sph.ExecuteReader();

        }


        public static void AddFeature(Guid siteGuid, Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteModuleDefinitions_Insert", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);

            sph.ExecuteNonQuery();

        }

        public static void RemoveFeature(Guid siteGuid, Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteModuleDefinitions_Delete", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.ExecuteNonQuery();
        }


        public static async Task<DbDataReader> GetSite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_SelectOne", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return await sph.ExecuteReaderAsync();
        }

        public static DbDataReader GetSiteNonAsync(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_SelectOne", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static async Task<DbDataReader> GetSite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_SelectOneByGuid", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return await sph.ExecuteReaderAsync();
        }

        

        public static IDataReader GetPageListForAdmin(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Pages_SelectList", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static async Task<int> GetHostCount()
        {
            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_SiteHosts_GetCount",
                null);

            return Convert.ToInt32(result);

        }

        public static async Task<DbDataReader> GetAllHosts()
        {
            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_SiteHosts_SelectAll",
                null);

        }

        public static DbDataReader GetAllHostsNonAsync()
        {
            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_SiteHosts_SelectAll",
                null);

        }

        public static async Task<DbDataReader> GetPageHosts(
            int pageNumber,
            int pageSize)
        {
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteHosts_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync();

        }

        public static async Task<DbDataReader> GetHostList(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteHosts_Select", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return await sph.ExecuteReaderAsync();
        }

        public static async Task<bool> AddHost(Guid siteGuid, int siteId, string hostName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SiteHosts_Insert", 3);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@HostName", SqlDbType.NVarChar, 255, ParameterDirection.Input, hostName);
            int rowsAffected = await sph.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public static async Task<bool> DeleteHost(int hostId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SiteHosts_Delete", 1);
            sph.DefineSqlParameter("@HostID", SqlDbType.Int, ParameterDirection.Input, hostId);
            int rowsAffected = await sph.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }


        public static async Task<int> CountOtherSites(int currentSiteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_CountOtherSites", 1);
            sph.DefineSqlParameter("@CurrentSiteID", SqlDbType.Int, ParameterDirection.Input, currentSiteId);
            object result = await sph.ExecuteScalarAsync();
            return Convert.ToInt32(result);

        }

        public static async Task<DbDataReader> GetPageOfOtherSites(
            int currentSiteId,
            int pageNumber,
            int pageSize)
        {
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_SelectPageOtherSites", 3);
            sph.DefineSqlParameter("@CurrentSiteID", SqlDbType.Int, ParameterDirection.Input, currentSiteId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return await sph.ExecuteReaderAsync();

        }


        public static async Task<int> GetSiteIdByHostName(string hostName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteHosts_SelectSiteIdByHost", 1);
            sph.DefineSqlParameter("@HostName", SqlDbType.NVarChar, 255, ParameterDirection.Input, hostName);
            object result = await sph.ExecuteScalarAsync();
            return Convert.ToInt32(result);

        }

        public static async Task<int> GetSiteIdByFolder(string folderName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteFolders_SelectSiteIdByFolder", 1);
            sph.DefineSqlParameter("@FolderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, folderName);
            object result = await sph.ExecuteScalarAsync();
            return Convert.ToInt32(result);

        }

        public static int GetSiteIdByFolderNonAsync(string folderName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteFolders_SelectSiteIdByFolder", 1);
            sph.DefineSqlParameter("@FolderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, folderName);
            object result = sph.ExecuteScalar();
            return Convert.ToInt32(result);

        }

    }
}
