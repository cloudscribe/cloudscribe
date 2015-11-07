// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-09
// Last Modified:			2015-11-07
// 

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using cloudscribe.Core.Models.Logging;

namespace cloudscribe.Core.Models.DataExtensions
{
    public static class ModelDataExtensions
    {


        public static void LoadFromReader(this ISiteRole role, DbDataReader reader)
        {
            role.RoleId = Convert.ToInt32(reader["RoleID"]);
            role.SiteId = Convert.ToInt32(reader["SiteID"]);
            role.RoleName = reader["RoleName"].ToString();
            role.DisplayName = reader["DisplayName"].ToString();
            role.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            role.RoleGuid = new Guid(reader["RoleGuid"].ToString());
        }

        public static void LoadFromReader(this IUserLogin userLogin, DbDataReader reader)
        {
            userLogin.SiteId = Convert.ToInt32(reader["SiteId"]);
            userLogin.LoginProvider = reader["LoginProvider"].ToString();
            userLogin.ProviderKey = reader["ProviderKey"].ToString();
            userLogin.UserId = reader["UserId"].ToString();
            userLogin.ProviderDisplayName = reader["ProviderDisplayName"].ToString();
        }

        public static void LoadFromReader(this IUserClaim userClaim, DbDataReader reader)
        {
            userClaim.SiteId = Convert.ToInt32(reader["SiteId"]);
            userClaim.Id = Convert.ToInt32(reader["Id"]);
            userClaim.UserId = reader["UserId"].ToString();
            userClaim.ClaimType = reader["ClaimType"].ToString();
            userClaim.ClaimValue = reader["ClaimValue"].ToString();
        }

        public static void LoadFromReader(this IUserInfo user, DbDataReader reader)
        {
            user.UserId = Convert.ToInt32(reader["UserID"], CultureInfo.InvariantCulture);
            if (reader["UserGuid"] != DBNull.Value)
            {
                user.UserGuid = new Guid(reader["UserGuid"].ToString());
            }
            user.SiteId = Convert.ToInt32(reader["SiteID"], CultureInfo.InvariantCulture);
            user.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            user.DisplayName = reader["Name"].ToString();
            user.UserName = reader["LoginName"].ToString();
            user.Email = reader["Email"].ToString();
            user.FirstName = reader["FirstName"].ToString();
            user.LastName = reader["LastName"].ToString();

            user.Gender = reader["Gender"].ToString();

            if (reader["AccountApproved"] != DBNull.Value)
            {
                user.AccountApproved = Convert.ToBoolean(reader["AccountApproved"]);
            }

           
            if (reader["Trusted"] != DBNull.Value)
            {
                user.Trusted = Convert.ToBoolean(reader["Trusted"]);
            }
            if (reader["DisplayInMemberList"] != DBNull.Value)
            {
                user.DisplayInMemberList = Convert.ToBoolean(reader["DisplayInMemberList"]);
            }
            user.WebSiteUrl = reader["WebSiteURL"].ToString();
            user.Country = reader["Country"].ToString();
            user.State = reader["State"].ToString();
           
            user.AvatarUrl = reader["AvatarUrl"].ToString();

            if (reader["DateCreated"] != DBNull.Value)
            {
                user.CreatedUtc = Convert.ToDateTime(reader["DateCreated"]);
            }

            if (reader["IsDeleted"] != DBNull.Value)
            {
                user.IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
            }
            if (reader["LastActivityDate"] != DBNull.Value)
            {
                user.LastActivityDate = Convert.ToDateTime(reader["LastActivityDate"]);
            }
            if (reader["LastLoginDate"] != DBNull.Value)
            {
                user.LastLoginDate = Convert.ToDateTime(reader["LastLoginDate"]);
            }

            if (reader["IsLockedOut"] != DBNull.Value)
            {
                user.IsLockedOut = Convert.ToBoolean(reader["IsLockedOut"]);
            }
            
            user.TimeZoneId = reader["TimeZoneId"].ToString();

            if (reader["DateOfBirth"] != DBNull.Value)
            {
                user.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
            }

            user.PhoneNumber = reader["PhoneNumber"].ToString();
            user.PhoneNumberConfirmed = Convert.ToBoolean(reader["PhoneNumberConfirmed"]);
        }

        public static void LoadFromReader(this SiteUser user, DbDataReader reader)
        {
            user.UserId = Convert.ToInt32(reader["UserID"], CultureInfo.InvariantCulture);
            user.SiteId = Convert.ToInt32(reader["SiteID"], CultureInfo.InvariantCulture);
            user.DisplayName = reader["Name"].ToString();
            user.UserName = reader["LoginName"].ToString();

            user.Email = reader["Email"].ToString();
            user.LoweredEmail = reader["LoweredEmail"].ToString();
            
            user.Gender = reader["Gender"].ToString();

            if (reader["AccountApproved"] != DBNull.Value)
            {
                user.AccountApproved = Convert.ToBoolean(reader["AccountApproved"]);
            }

            if (reader["RegisterConfirmGuid"] != DBNull.Value)
            {
                user.RegisterConfirmGuid = new Guid(reader["RegisterConfirmGuid"].ToString());
            }
            
            if (reader["Trusted"] != DBNull.Value)
            {
                user.Trusted = Convert.ToBoolean(reader["Trusted"]);
            }
            if (reader["DisplayInMemberList"] != DBNull.Value)
            {
                user.DisplayInMemberList = Convert.ToBoolean(reader["DisplayInMemberList"]);
            }
            user.WebSiteUrl = reader["WebSiteURL"].ToString();
            user.Country = reader["Country"].ToString();
            user.State = reader["State"].ToString();
            
            user.AvatarUrl = reader["AvatarUrl"].ToString();

            user.Signature = reader["Signature"].ToString();
            if (reader["DateCreated"] != DBNull.Value)
            {
                user.CreatedUtc = Convert.ToDateTime(reader["DateCreated"]);
            }
            if (reader["UserGuid"] != DBNull.Value)
            {
                user.UserGuid = new Guid(reader["UserGuid"].ToString());
            }
            
            if (reader["IsDeleted"] != DBNull.Value)
            {
                user.IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
            }
            if (reader["LastActivityDate"] != DBNull.Value)
            {
                user.LastActivityDate = Convert.ToDateTime(reader["LastActivityDate"]);
            }
            if (reader["LastLoginDate"] != DBNull.Value)
            {
                user.LastLoginDate = Convert.ToDateTime(reader["LastLoginDate"]);
            }
            if (reader["LastPasswordChangedDate"] != DBNull.Value)
            {
                user.LastPasswordChangedDate = Convert.ToDateTime(reader["LastPasswordChangedDate"]);
            }
            if (reader["LastLockoutDate"] != DBNull.Value)
            {
                user.LastLockoutDate = Convert.ToDateTime(reader["LastLockoutDate"]);
            }
            if (reader["FailedPasswordAttemptCount"] != DBNull.Value)
            {
                user.FailedPasswordAttemptCount = Convert.ToInt32(reader["FailedPasswordAttemptCount"]);
            }
            if (reader["FailedPwdAttemptWindowStart"] != DBNull.Value)
            {
                user.FailedPasswordAttemptWindowStart = Convert.ToDateTime(reader["FailedPwdAttemptWindowStart"]);
            }
            if (reader["FailedPwdAnswerAttemptCount"] != DBNull.Value)
            {
                user.FailedPasswordAnswerAttemptCount = Convert.ToInt32(reader["FailedPwdAnswerAttemptCount"]);
            }
            if (reader["FailedPwdAnswerWindowStart"] != DBNull.Value)
            {
                user.FailedPasswordAnswerAttemptWindowStart = Convert.ToDateTime(reader["FailedPwdAnswerWindowStart"]);
            }
            if (reader["IsLockedOut"] != DBNull.Value)
            {
                user.IsLockedOut = Convert.ToBoolean(reader["IsLockedOut"]);
            }
            
            user.Comment = reader["Comment"].ToString();
           
            user.SiteGuid = new Guid(reader["SiteGuid"].ToString());

            

            user.FirstName = reader["FirstName"].ToString();
            user.LastName = reader["LastName"].ToString();

            user.MustChangePwd = Convert.ToBoolean(reader["MustChangePwd"]);
            user.NewEmail = reader["NewEmail"].ToString();
            
            user.EmailChangeGuid = new Guid(reader["EmailChangeGuid"].ToString());
            user.TimeZoneId = reader["TimeZoneId"].ToString();
            user.PasswordResetGuid = new Guid(reader["PasswordResetGuid"].ToString());
            user.RolesChanged = Convert.ToBoolean(reader["RolesChanged"]);
            user.AuthorBio = reader["AuthorBio"].ToString();
            if (reader["DateOfBirth"] != DBNull.Value)
            {
                user.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
            }

            user.EmailConfirmed = Convert.ToBoolean(reader["EmailConfirmed"]);

            user.SecurityStamp = reader["SecurityStamp"].ToString();
            user.PhoneNumber = reader["PhoneNumber"].ToString();
            user.PhoneNumberConfirmed = Convert.ToBoolean(reader["PhoneNumberConfirmed"]);
            user.TwoFactorEnabled = Convert.ToBoolean(reader["TwoFactorEnabled"]);
            if (reader["LockoutEndDateUtc"] != DBNull.Value)
            {
                user.LockoutEndDateUtc = Convert.ToDateTime(reader["LockoutEndDateUtc"]);
            }

            user.PasswordHash = reader["PasswordHash"].ToString();
            
        }

        public static void LoadFromReader(this ISiteInfo site, DbDataReader reader)
        {
            site.SiteId = Convert.ToInt32(reader["SiteID"]);
            site.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            site.SiteName = reader["SiteName"].ToString();
            site.SiteFolderName = reader["ApiKeyExtra4"].ToString();
            site.PreferredHostName = reader["ApiKeyExtra5"].ToString();
            site.IsServerAdminSite = Convert.ToBoolean(reader["IsServerAdminSite"]);
        }

        public static void LoadFromReader(this ISiteSettings site, DbDataReader reader)
        {
            site.SiteId = Convert.ToInt32(reader["SiteID"]);
            site.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            
            site.SiteName = reader["SiteName"].ToString();
            site.Layout = reader["Skin"].ToString();
            site.AllowNewRegistration = Convert.ToBoolean(reader["AllowNewRegistration"]);
            site.UseSecureRegistration = Convert.ToBoolean(reader["UseSecureRegistration"]);
            site.UseSslOnAllPages = Convert.ToBoolean(reader["UseSSLOnAllPages"]);

            site.IsServerAdminSite = Convert.ToBoolean(reader["IsServerAdminSite"]);
            site.UseLdapAuth = Convert.ToBoolean(reader["UseLdapAuth"]);
            site.AutoCreateLdapUserOnFirstLogin = Convert.ToBoolean(reader["AutoCreateLdapUserOnFirstLogin"]);
            
            site.LdapServer = reader["LdapServer"].ToString();
            site.LdapPort = Convert.ToInt32(reader["LdapPort"]);
            site.LdapDomain = reader["LdapDomain"].ToString();
            site.LdapRootDN = reader["LdapRootDN"].ToString();
            site.LdapUserDNKey = reader["LdapUserDNKey"].ToString();

            site.ReallyDeleteUsers = Convert.ToBoolean(reader["ReallyDeleteUsers"]);
            site.UseEmailForLogin = Convert.ToBoolean(reader["UseEmailForLogin"]);
            site.AllowUserFullNameChange = Convert.ToBoolean(reader["AllowUserFullNameChange"]);
            
            site.RequiresQuestionAndAnswer = Convert.ToBoolean(reader["RequiresQuestionAndAnswer"]);
            site.MaxInvalidPasswordAttempts = Convert.ToInt32(reader["MaxInvalidPasswordAttempts"]);
            site.PasswordAttemptWindowMinutes = Convert.ToInt32(reader["PasswordAttemptWindowMinutes"]);
           
            site.MinRequiredPasswordLength = Convert.ToInt32(reader["MinRequiredPasswordLength"]);
            site.MinReqNonAlphaChars = Convert.ToInt32(reader["MinReqNonAlphaChars"]);
            
            site.DefaultEmailFromAddress = reader["DefaultEmailFromAddress"].ToString();
            site.RecaptchaPrivateKey = reader["RecaptchaPrivateKey"].ToString();
            site.RecaptchaPublicKey = reader["RecaptchaPublicKey"].ToString();
            
            site.AddThisDotComUsername = reader["AddThisDotComUsername"].ToString();
            site.ApiKeyExtra1 = reader["ApiKeyExtra1"].ToString();
            site.ApiKeyExtra2 = reader["ApiKeyExtra2"].ToString();
            site.ApiKeyExtra3 = reader["ApiKeyExtra3"].ToString();
            site.ApiKeyExtra4 = reader["ApiKeyExtra4"].ToString();
            site.ApiKeyExtra5 = reader["ApiKeyExtra5"].ToString();
            
            site.DisableDbAuth = Convert.ToBoolean(reader["DisableDbAuth"]);
            
            site.RequiresQuestionAndAnswer = Convert.ToBoolean(reader["RequiresQuestionAndAnswer"]);
            site.AllowDbFallbackWithLdap = Convert.ToBoolean(reader["AllowDbFallbackWithLdap"]);
            site.EmailLdapDbFallback = Convert.ToBoolean(reader["EmailLdapDbFallback"]);
            site.AllowPersistentLogin = Convert.ToBoolean(reader["AllowPersistentLogin"]);
            site.CaptchaOnLogin = Convert.ToBoolean(reader["CaptchaOnLogin"]);
            site.CaptchaOnRegistration = Convert.ToBoolean(reader["CaptchaOnRegistration"]);
            site.SiteIsClosed = Convert.ToBoolean(reader["SiteIsClosed"]);
            site.SiteIsClosedMessage = reader["SiteIsClosedMessage"].ToString();
            site.PrivacyPolicy = reader["PrivacyPolicy"].ToString();
            site.TimeZoneId = reader["TimeZoneId"].ToString();
            site.GoogleAnalyticsProfileId = reader["GoogleAnalyticsProfileId"].ToString();
            site.CompanyName = reader["CompanyName"].ToString();
            site.CompanyStreetAddress = reader["CompanyStreetAddress"].ToString();
            site.CompanyStreetAddress2 = reader["CompanyStreetAddress2"].ToString();
            site.CompanyRegion = reader["CompanyRegion"].ToString();
            site.CompanyLocality = reader["CompanyLocality"].ToString();
            site.CompanyCountry = reader["CompanyCountry"].ToString();
            site.CompanyPostalCode = reader["CompanyPostalCode"].ToString();
            site.CompanyPublicEmail = reader["CompanyPublicEmail"].ToString();
            site.CompanyPhone = reader["CompanyPhone"].ToString();
            site.CompanyFax = reader["CompanyFax"].ToString();
            site.FacebookAppId = reader["FacebookAppId"].ToString();
            site.FacebookAppSecret = reader["FacebookAppSecret"].ToString();
            site.GoogleClientId = reader["GoogleClientId"].ToString();
            site.GoogleClientSecret = reader["GoogleClientSecret"].ToString();
            site.TwitterConsumerKey = reader["TwitterConsumerKey"].ToString();
            site.TwitterConsumerSecret = reader["TwitterConsumerSecret"].ToString();
            site.MicrosoftClientId = reader["MicrosoftClientId"].ToString();
            site.MicrosoftClientSecret = reader["MicrosoftClientSecret"].ToString();
            site.PreferredHostName = reader["PreferredHostName"].ToString();
            site.SiteFolderName = reader["SiteFolderName"].ToString();
            site.AddThisDotComUsername = reader["AddThisDotComUsername"].ToString();
            site.LoginInfoTop = reader["LoginInfoTop"].ToString();
            site.LoginInfoBottom = reader["LoginInfoBottom"].ToString();
            site.RegistrationAgreement = reader["RegistrationAgreement"].ToString();
            site.RegistrationPreamble = reader["RegistrationPreamble"].ToString();
            site.SmtpServer = reader["SmtpServer"].ToString();
            site.SmtpPort = Convert.ToInt32(reader["SmtpPort"]);
            site.SmtpUser = reader["SmtpUser"].ToString();
            site.SmtpPassword = reader["SmtpPassword"].ToString();
            site.SmtpPreferredEncoding = reader["SmtpPreferredEncoding"].ToString();
            site.SmtpRequiresAuth = Convert.ToBoolean(reader["SmtpRequiresAuth"]);
            site.SmtpUseSsl = Convert.ToBoolean(reader["SmtpUseSsl"]);

        }


        public static void LoadFromReader(this ISiteFolder folder, DbDataReader reader)
        {
            folder.Guid = new Guid(reader["Guid"].ToString());
            folder.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            folder.FolderName = reader["FolderName"].ToString();
        }

        public static void LoadFromReader(this ISiteHost host, DbDataReader reader)
        {
            host.HostId = Convert.ToInt32(reader["HostID"]);
            host.HostName = reader["HostName"].ToString();
            host.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            host.SiteId = Convert.ToInt32(reader["SiteID"]);
        }

        public static void LoadExpandoSettings(this ISiteSettings site, List<ExpandoSetting> expandoProperties)
        {
            //string b = GetExpandoProperty(expandoProperties, "AllowPersistentLogin");
            //if (!string.IsNullOrEmpty(b)) { site.AllowPersistentLogin = Convert.ToBoolean(b); }

            //site.AvatarSystem = GetExpandoProperty(expandoProperties, "AvatarSystem");
            //site.CommentProvider = GetExpandoProperty(expandoProperties, "CommentProvider");
            //site.CompanyPublicEmail = GetExpandoProperty(expandoProperties, "CompanyPublicEmail");
            //site.CompanyFax = GetExpandoProperty(expandoProperties, "CompanyFax");
            //site.CompanyPhone = GetExpandoProperty(expandoProperties, "CompanyPhone");
            //site.CompanyCountry = GetExpandoProperty(expandoProperties, "CompanyCountry");
            //site.CompanyPostalCode = GetExpandoProperty(expandoProperties, "CompanyPostalCode");
            //site.CompanyRegion = GetExpandoProperty(expandoProperties, "CompanyRegion");
            //site.CompanyLocality = GetExpandoProperty(expandoProperties, "CompanyLocality");
            //site.CompanyStreetAddress = GetExpandoProperty(expandoProperties, "CompanyStreetAddress");
            //site.CompanyStreetAddress2 = GetExpandoProperty(expandoProperties, "CompanyStreetAddress2");
            //site.CompanyName = GetExpandoProperty(expandoProperties, "CompanyName");

            //string g = GetExpandoProperty(expandoProperties, "CurrencyGuid");
            //if ((g != null) && (g.Length == 36)) { site.CurrencyGuid = new Guid(g); }

            //g = GetExpandoProperty(expandoProperties, "DefaultStateGuid");
            //if ((g != null) && (g.Length == 36)) { site.DefaultStateGuid = new Guid(g); }

            //g = GetExpandoProperty(expandoProperties, "DefaultCountryGuid");
            //if ((g != null) && (g.Length == 36)) { site.DefaultCountryGuid = new Guid(g); }

            //site.DefaultRootPageCreateChildPageRoles = GetExpandoProperty(expandoProperties, "DefaultRootPageCreateChildPageRoles");

            //site.DefaultRootPageEditRoles = GetExpandoProperty(expandoProperties, "DefaultRootPageEditRoles");
            //site.DefaultRootPageViewRoles = GetExpandoProperty(expandoProperties, "DefaultRootPageViewRoles");
            //site.DisqusSiteShortName = GetExpandoProperty(expandoProperties, "DisqusSiteShortName");
            //site.EmailAdressesForUserApprovalNotification = GetExpandoProperty(expandoProperties, "EmailAdressesForUserApprovalNotification");
            //b = GetExpandoProperty(expandoProperties, "EnableContentWorkflow");
            //if (!string.IsNullOrEmpty(b)) { site.EnableContentWorkflow = Convert.ToBoolean(b); }
            
            //b = GetExpandoProperty(expandoProperties, "ForceContentVersioning");
            //if (!string.IsNullOrEmpty(b)) { site.ForceContentVersioning = Convert.ToBoolean(b); }
            //site.GoogleAnalyticsSettings = GetExpandoProperty(expandoProperties, "GoogleAnalyticsSettings");
            //site.GoogleAnalyticsProfileId = GetExpandoProperty(expandoProperties, "GoogleAnalyticsProfileId");
            ////SetExpandoProperty(expandoProperties, "GoogleAnalyticsPassword", site.GoogleAnalyticsPassword);
            ////SetExpandoProperty(expandoProperties, "GoogleAnalyticsEmail", site.GoogleAnalyticsEmail);
            //site.IntenseDebateAccountId = GetExpandoProperty(expandoProperties, "IntenseDebateAccountId");
            //site.LoginInfoBottom = GetExpandoProperty(expandoProperties, "LoginInfoBottom");
            //site.LoginInfoTop = GetExpandoProperty(expandoProperties, "LoginInfoTop");
            //site.MetaProfile = GetExpandoProperty(expandoProperties, "MetaProfile");
            //site.NewsletterEditor = GetExpandoProperty(expandoProperties, "NewsletterEditor");
            //site.PasswordRegexWarning = GetExpandoProperty(expandoProperties, "PasswordRegexWarning");
            //site.PrivacyPolicy = GetExpandoProperty(expandoProperties, "PrivacyPolicyUrl");
            //site.RegistrationAgreement = GetExpandoProperty(expandoProperties, "RegistrationAgreement");
            //site.RegistrationPreamble = GetExpandoProperty(expandoProperties, "RegistrationPreamble");

            //b = GetExpandoProperty(expandoProperties, "RequireApprovalBeforeLogin");
            //if (!string.IsNullOrEmpty(b)) { site.RequireApprovalBeforeLogin = Convert.ToBoolean(b); }

            //// permission roles
            //site.RolesThatCanApproveNewUsers = GetExpandoProperty(expandoProperties, "RolesThatCanApproveNewUsers");
            //site.RolesThatCanManageSkins = GetExpandoProperty(expandoProperties, "RolesThatCanManageSkins");
            //site.RolesThatCanAssignSkinsToPages = GetExpandoProperty(expandoProperties, "RolesThatCanAssignSkinsToPages");
            //site.RolesThatCanDeleteFilesInEditor = GetExpandoProperty(expandoProperties, "RolesThatCanDeleteFilesInEditor");
            //site.UserFilesBrowseAndUploadRoles = GetExpandoProperty(expandoProperties, "UserFilesBrowseAndUploadRoles");
            //site.GeneralBrowseAndUploadRoles = GetExpandoProperty(expandoProperties, "GeneralBrowseAndUploadRoles");
            //site.RolesThatCanEditContentTemplates = GetExpandoProperty(expandoProperties, "RolesThatCanEditContentTemplates");
            //site.RolesNotAllowedToEditModuleSettings = GetExpandoProperty(expandoProperties, "RolesNotAllowedToEditModuleSettings");
            //site.RolesThatCanLookupUsers = GetExpandoProperty(expandoProperties, "RolesThatCanLookupUsers");
            //site.RolesThatCanManageUsers = GetExpandoProperty(expandoProperties, "RolesThatCanFullyManageUsers");
            //site.RolesThatCanCreateUsers = GetExpandoProperty(expandoProperties, "RolesThatCanManageUsers");
            //site.RolesThatCanViewMemberList = GetExpandoProperty(expandoProperties, "RolesThatCanViewMemberList");
            //site.RolesThatCanCreateRootPages = GetExpandoProperty(expandoProperties, "RolesThatCanCreateRootPages");
            //site.CommerceReportViewRoles = GetExpandoProperty(expandoProperties, "CommerceReportViewRoles");
            //site.SiteRootDraftApprovalRoles = GetExpandoProperty(expandoProperties, "SiteRootDraftApprovalRoles");
            //site.SiteRootDraftEditRoles = GetExpandoProperty(expandoProperties, "SiteRootDraftEditRoles");
            //site.SiteRootEditRoles = GetExpandoProperty(expandoProperties, "SiteRootEditRoles");

            //// end roles

            //b = GetExpandoProperty(expandoProperties, "SiteIsClosed");
            //if (!string.IsNullOrEmpty(b)) { site.SiteIsClosed = Convert.ToBoolean(b); }

            //site.SiteIsClosedMessage = GetExpandoProperty(expandoProperties, "SiteIsClosedMessage");

            //g = GetExpandoProperty(expandoProperties, "SkinVersion");
            //if (!string.IsNullOrEmpty(g))
            //{
            //    if (g.Length == 36) { site.SkinVersion = new Guid(g); }
            //}

            //b = GetExpandoProperty(expandoProperties, "SMTPUseSsl");
            //if (!string.IsNullOrEmpty(b)) { site.SmtpUseSsl = Convert.ToBoolean(b); }

            //b = GetExpandoProperty(expandoProperties, "SMTPRequiresAuthentication");
            //if (!string.IsNullOrEmpty(b)) { site.SmtpRequiresAuth = Convert.ToBoolean(b); }


            //site.SmtpServer = GetExpandoProperty(expandoProperties, "SMTPServer");
            //site.SmtpPreferredEncoding = GetExpandoProperty(expandoProperties, "SMTPPreferredEncoding");
            //string i = GetExpandoProperty(expandoProperties, "SMTPPort");
            //if (!string.IsNullOrEmpty(i)) { site.SmtpPort = Convert.ToInt32(i); }
            //site.SmtpPassword = GetExpandoProperty(expandoProperties, "SMTPPassword");
            //site.SmtpUser = GetExpandoProperty(expandoProperties, "SMTPUser");
            //site.Slogan = GetExpandoProperty(expandoProperties, "Slogan");

            //b = GetExpandoProperty(expandoProperties, "ShowAlternateSearchIfConfigured");
            //if (!string.IsNullOrEmpty(b)) { site.ShowAlternateSearchIfConfigured = Convert.ToBoolean(b); }

            //site.PrimarySearchEngine = GetExpandoProperty(expandoProperties, "PrimarySearchEngine");
            //site.GoogleCustomSearchId = GetExpandoProperty(expandoProperties, "GoogleCustomSearchId");
            //site.BingAPIId = GetExpandoProperty(expandoProperties, "BingAPIId");
            //site.OpenSearchName = GetExpandoProperty(expandoProperties, "OpenSearchName");
            //site.RpxNowAdminUrl = GetExpandoProperty(expandoProperties, "RpxNowAdminUrl");
            //site.RpxNowApplicationName = GetExpandoProperty(expandoProperties, "RpxNowApplicationName");
            //site.RpxNowApiKey = GetExpandoProperty(expandoProperties, "RpxNowApiKey");
            ////site.AppLogoForWindowsLive = GetExpandoProperty(expandoProperties, "AppLogoForWindowsLive");

            //site.SiteMapSkin = GetExpandoProperty(expandoProperties, "SiteMapSkin");
            //site.TimeZoneId = GetExpandoProperty(expandoProperties, "TimeZoneId");
            ////SetExpandoProperty(expandoProperties, "ShowPasswordStrengthOnRegistration", site.ShowPasswordStrengthOnRegistration);
            ////SetExpandoProperty(expandoProperties, "RequireEnterEmailTwiceOnRegistration", site.RequireEnterEmailTwiceOnRegistration);

            //b = GetExpandoProperty(expandoProperties, "RequireCaptchaOnLogin");
            //if (!string.IsNullOrEmpty(b)) { site.CaptchaOnLogin = Convert.ToBoolean(b); }

            //b = GetExpandoProperty(expandoProperties, "RequireCaptchaOnRegistration");
            //if (!string.IsNullOrEmpty(b)) { site.CaptchaOnRegistration = Convert.ToBoolean(b); }

            //site.FacebookAppId = GetExpandoProperty(expandoProperties, "FacebookAppId");
            //site.FacebookAppSecret = GetExpandoProperty(expandoProperties, "FacebookAppSecret");

            //site.TwitterConsumerKey = GetExpandoProperty(expandoProperties, "TwitterConsumerKey");
            //site.TwitterConsumerSecret = GetExpandoProperty(expandoProperties, "TwitterConsumerSecret");

            //site.GoogleClientId = GetExpandoProperty(expandoProperties, "GoogleClientId");
            //site.GoogleClientSecret = GetExpandoProperty(expandoProperties, "GoogleClientSecret");


        }

        public static void SetExpandoSettings(this ISiteSettings site, List<ExpandoSetting> expandoProperties)
        {
            //SetExpandoProperty(expandoProperties, "AvatarSystem", site.AvatarSystem);
            //SetExpandoProperty(expandoProperties, "AllowUserEditorPreference", site.AllowUserEditorPreference);
            //SetExpandoProperty(expandoProperties, "CommentProvider", site.CommentProvider);
            //SetExpandoProperty(expandoProperties, "CompanyPublicEmail", site.CompanyPublicEmail);
            //SetExpandoProperty(expandoProperties, "CompanyFax", site.CompanyFax);
            //SetExpandoProperty(expandoProperties, "CompanyPhone", site.CompanyPhone);
            //SetExpandoProperty(expandoProperties, "CompanyCountry", site.CompanyCountry);
            //SetExpandoProperty(expandoProperties, "CompanyPostalCode", site.CompanyPostalCode);
            //SetExpandoProperty(expandoProperties, "CompanyRegion", site.CompanyRegion);
            //SetExpandoProperty(expandoProperties, "CompanyLocality", site.CompanyLocality);
            //SetExpandoProperty(expandoProperties, "CompanyStreetAddress", site.CompanyStreetAddress);
            //SetExpandoProperty(expandoProperties, "CompanyStreetAddress2", site.CompanyStreetAddress2);
            //SetExpandoProperty(expandoProperties, "CompanyName", site.CompanyName);
            //SetExpandoProperty(expandoProperties, "CurrencyGuid", site.CurrencyGuid.ToString());
            //SetExpandoProperty(expandoProperties, "DefaultStateGuid", site.DefaultStateGuid.ToString());
            //SetExpandoProperty(expandoProperties, "DefaultCountryGuid", site.DefaultCountryGuid.ToString());
            //SetExpandoProperty(expandoProperties, "DefaultRootPageCreateChildPageRoles", site.DefaultRootPageCreateChildPageRoles);
            //SetExpandoProperty(expandoProperties, "DefaultRootPageEditRoles", site.DefaultRootPageEditRoles);
            //SetExpandoProperty(expandoProperties, "DefaultRootPageViewRoles", site.DefaultRootPageViewRoles);
            //SetExpandoProperty(expandoProperties, "DisqusSiteShortName", site.DisqusSiteShortName);
            //SetExpandoProperty(expandoProperties, "EmailAdressesForUserApprovalNotification", site.EmailAdressesForUserApprovalNotification);
            //SetExpandoProperty(expandoProperties, "EnableContentWorkflow", site.EnableContentWorkflow.ToString());
            
            //SetExpandoProperty(expandoProperties, "ForceContentVersioning", site.ForceContentVersioning.ToString());
            //SetExpandoProperty(expandoProperties, "GoogleAnalyticsSettings", site.GoogleAnalyticsSettings);
            //SetExpandoProperty(expandoProperties, "GoogleAnalyticsProfileId", site.GoogleAnalyticsProfileId);
            //SetExpandoProperty(expandoProperties, "GoogleAnalyticsPassword", site.GoogleAnalyticsPassword);
            //SetExpandoProperty(expandoProperties, "GoogleAnalyticsEmail", site.GoogleAnalyticsEmail);
            //SetExpandoProperty(expandoProperties, "IntenseDebateAccountId", site.IntenseDebateAccountId);
            //SetExpandoProperty(expandoProperties, "LoginInfoBottom", site.LoginInfoBottom);
            //SetExpandoProperty(expandoProperties, "LoginInfoTop", site.LoginInfoTop);
            //SetExpandoProperty(expandoProperties, "MetaProfile", site.MetaProfile);
            //SetExpandoProperty(expandoProperties, "NewsletterEditor", site.NewsletterEditor);
            //SetExpandoProperty(expandoProperties, "PasswordRegexWarning", site.PasswordRegexWarning);
            //SetExpandoProperty(expandoProperties, "PrivacyPolicyUrl", site.PrivacyPolicy);
            //SetExpandoProperty(expandoProperties, "RegistrationAgreement", site.RegistrationAgreement);
            //SetExpandoProperty(expandoProperties, "RegistrationPreamble", site.RegistrationPreamble);
            //SetExpandoProperty(expandoProperties, "RequireApprovalBeforeLogin", site.RequireApprovalBeforeLogin.ToString());

            // permission roles
            //SetExpandoProperty(expandoProperties, "RolesThatCanApproveNewUsers", site.RolesThatCanApproveNewUsers);
           // SetExpandoProperty(expandoProperties, "RolesThatCanManageSkins", site.RolesThatCanManageSkins);
            //SetExpandoProperty(expandoProperties, "RolesThatCanAssignSkinsToPages", site.RolesThatCanAssignSkinsToPages);
            //SetExpandoProperty(expandoProperties, "RolesThatCanDeleteFilesInEditor", site.RolesThatCanDeleteFilesInEditor);
            //SetExpandoProperty(expandoProperties, "UserFilesBrowseAndUploadRoles", site.UserFilesBrowseAndUploadRoles);
            //SetExpandoProperty(expandoProperties, "GeneralBrowseAndUploadRoles", site.GeneralBrowseAndUploadRoles);
            //SetExpandoProperty(expandoProperties, "RolesThatCanEditContentTemplates", site.RolesThatCanEditContentTemplates);
            //SetExpandoProperty(expandoProperties, "RolesNotAllowedToEditModuleSettings", site.RolesNotAllowedToEditModuleSettings);
            //SetExpandoProperty(expandoProperties, "RolesThatCanLookupUsers", site.RolesThatCanLookupUsers);
            //SetExpandoProperty(expandoProperties, "RolesThatCanFullyManageUsers", site.RolesThatCanManageUsers);
            //SetExpandoProperty(expandoProperties, "RolesThatCanManageUsers", site.RolesThatCanCreateUsers);
            //SetExpandoProperty(expandoProperties, "RolesThatCanViewMemberList", site.RolesThatCanViewMemberList);
            //SetExpandoProperty(expandoProperties, "RolesThatCanCreateRootPages", site.RolesThatCanCreateRootPages);
            //SetExpandoProperty(expandoProperties, "CommerceReportViewRoles", site.CommerceReportViewRoles);
            //SetExpandoProperty(expandoProperties, "SiteRootDraftApprovalRoles", site.SiteRootDraftApprovalRoles);
            //SetExpandoProperty(expandoProperties, "SiteRootDraftEditRoles", site.SiteRootDraftEditRoles);
            //SetExpandoProperty(expandoProperties, "SiteRootEditRoles", site.SiteRootEditRoles);

            // end roles

            //SetExpandoProperty(expandoProperties, "SiteIsClosed", site.SiteIsClosed.ToString());
            //SetExpandoProperty(expandoProperties, "SiteIsClosedMessage", site.SiteIsClosedMessage);
            //SetExpandoProperty(expandoProperties, "SkinVersion", site.SkinVersion.ToString());
            //SetExpandoProperty(expandoProperties, "SMTPUseSsl", site.SmtpUseSsl.ToString());
            //SetExpandoProperty(expandoProperties, "SMTPRequiresAuthentication", site.SmtpRequiresAuth.ToString());
            //SetExpandoProperty(expandoProperties, "SMTPServer", site.SmtpServer);
            //SetExpandoProperty(expandoProperties, "SMTPPreferredEncoding", site.SmtpPreferredEncoding);
            //SetExpandoProperty(expandoProperties, "SMTPPort", site.SmtpPort.ToString(CultureInfo.InvariantCulture));
            //SetExpandoProperty(expandoProperties, "SMTPPassword", site.SmtpPassword);
            //SetExpandoProperty(expandoProperties, "SMTPUser", site.SmtpUser);
            //SetExpandoProperty(expandoProperties, "Slogan", site.Slogan);
            //SetExpandoProperty(expandoProperties, "ShowAlternateSearchIfConfigured", site.ShowAlternateSearchIfConfigured.ToString());
            //SetExpandoProperty(expandoProperties, "PrimarySearchEngine", site.PrimarySearchEngine);
            //SetExpandoProperty(expandoProperties, "GoogleCustomSearchId", site.GoogleCustomSearchId);
            //SetExpandoProperty(expandoProperties, "BingAPIId", site.BingAPIId);
            //SetExpandoProperty(expandoProperties, "OpenSearchName", site.OpenSearchName);
            //SetExpandoProperty(expandoProperties, "RpxNowAdminUrl", site.RpxNowAdminUrl);
            //SetExpandoProperty(expandoProperties, "RpxNowApplicationName", site.RpxNowApplicationName);
            //SetExpandoProperty(expandoProperties, "RpxNowApiKey", site.RpxNowApiKey);
            //SetExpandoProperty(expandoProperties, "AppLogoForWindowsLive", site.AppLogoForWindowsLive);

            //SetExpandoProperty(expandoProperties, "SiteMapSkin", site.SiteMapSkin);
            //SetExpandoProperty(expandoProperties, "TimeZoneId", site.TimeZoneId);
            //SetExpandoProperty(expandoProperties, "ShowPasswordStrengthOnRegistration", site.ShowPasswordStrengthOnRegistration);
            //SetExpandoProperty(expandoProperties, "RequireEnterEmailTwiceOnRegistration", site.RequireEnterEmailTwiceOnRegistration);
            //SetExpandoProperty(expandoProperties, "RequireCaptchaOnLogin", site.CaptchaOnLogin.ToString());
            //SetExpandoProperty(expandoProperties, "RequireCaptchaOnRegistration", site.CaptchaOnRegistration.ToString());
            //SetExpandoProperty(expandoProperties, "AllowPersistentLogin", site.AllowPersistentLogin.ToString());

            //SetExpandoProperty(expandoProperties, "FacebookAppId", site.FacebookAppId);
            //SetExpandoProperty(expandoProperties, "FacebookAppSecret", site.FacebookAppSecret);

            //SetExpandoProperty(expandoProperties, "TwitterConsumerKey", site.TwitterConsumerKey);
            //SetExpandoProperty(expandoProperties, "TwitterConsumerSecret", site.TwitterConsumerSecret);

            //SetExpandoProperty(expandoProperties, "GoogleClientId", site.GoogleClientId);
            //SetExpandoProperty(expandoProperties, "GoogleClientSecret", site.GoogleClientSecret);

        }

        private static string GetExpandoProperty(List<ExpandoSetting> exapandoProperties, string keyName)
        {
            //EnsureExpandoProperties();

            foreach (ExpandoSetting s in exapandoProperties)
            {
                if (s.KeyName.Trim().Equals(keyName, StringComparison.CurrentCulture))
                {
                    return s.KeyValue;
                }

            }

            return null;

        }

        private static void SetExpandoProperty(List<ExpandoSetting> exapandoProperties, string keyName, string keyValue)
        {
            
            foreach (ExpandoSetting s in exapandoProperties)
            {
                if (s.KeyName.Trim().Equals(keyName, StringComparison.CurrentCulture))
                {
                    if (s.KeyValue != keyValue)
                    {
                        s.KeyValue = keyValue;
                        s.IsDirty = true;
                    };
                    break;
                }

            }



        }

        public static void LoadFromReader(this ILogItem logItem, DbDataReader reader)
        {
            logItem.Id = Convert.ToInt32(reader["ID"]);
            logItem.LogDateUtc = Convert.ToDateTime(reader["LogDate"]);
            logItem.IpAddress = reader["IpAddress"].ToString();
            logItem.Culture = reader["Culture"].ToString();
            logItem.Url = reader["Url"].ToString();
            logItem.ShortUrl = reader["ShortUrl"].ToString();
            logItem.Thread = reader["Thread"].ToString();
            logItem.LogLevel = reader["LogLevel"].ToString();
            logItem.Logger = reader["Logger"].ToString();
            logItem.Message = reader["Message"].ToString();

        }

        //public static DataTable CreateExpandoTable()
        //{
        //    DataTable dataTable = new DataTable();
        //    dataTable.TableName = "expandoProperties";
        //    dataTable.Columns.Add("SiteID", typeof(int));
        //    dataTable.Columns.Add("KeyName", typeof(string));
        //    dataTable.Columns.Add("KeyValue", typeof(string));
        //    dataTable.Columns.Add("GroupName", typeof(string));
        //    dataTable.Columns.Add("IsDirty", typeof(bool));

        //    return dataTable;

        //}
    }
}
