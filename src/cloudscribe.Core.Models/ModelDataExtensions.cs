// Author:					Joe Audette
// Created:					2014-12-09
// Last Modified:			2014-12-09
// 

using cloudscribe.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace cloudscribe.Core.Models.DataExtensions
{
    public static class ModelDataExtensions
    {
        

        public static void LoadFromReader(this ISiteRole role, IDataReader reader)
        {
            role.RoleId = Convert.ToInt32(reader["RoleID"]);
            role.SiteId = Convert.ToInt32(reader["SiteID"]);
            role.RoleName = reader["RoleName"].ToString();
            role.DisplayName = reader["DisplayName"].ToString();
            role.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            role.RoleGuid = new Guid(reader["RoleGuid"].ToString());
        }

        public static void LoadFromReader(this IUserLogin userLogin, IDataReader reader)
        {
            userLogin.LoginProvider = reader["LoginProvider"].ToString();
            userLogin.ProviderKey = reader["ProviderKey"].ToString();
            userLogin.UserId = reader["UserId"].ToString();
        }

        public static void LoadFromReader(this IUserClaim userClaim, IDataReader reader)
        {
            userClaim.Id = Convert.ToInt32(reader["Id"]);
            userClaim.UserId = reader["UserId"].ToString();
            userClaim.ClaimType = reader["ClaimType"].ToString();
            userClaim.ClaimValue = reader["ClaimValue"].ToString();
        }

        public static void LoadFromReader(this IUserInfo user, IDataReader reader)
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

            if (reader["ProfileApproved"] != DBNull.Value)
            {
                user.ProfileApproved = Convert.ToBoolean(reader["ProfileApproved"]);
            }

            if (reader["ApprovedForForums"] != DBNull.Value)
            {
                user.ApprovedForLogin = Convert.ToBoolean(reader["ApprovedForForums"]);
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
            user.TotalPosts = Convert.ToInt32(reader["TotalPosts"], CultureInfo.InvariantCulture);
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

            if (reader["TotalRevenue"] != DBNull.Value)
            {
                user.TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"]);
            }

            user.TimeZoneId = reader["TimeZoneId"].ToString();

            if (reader["DateOfBirth"] != DBNull.Value)
            {
                user.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
            }

            user.PhoneNumber = reader["PhoneNumber"].ToString();
            user.PhoneNumberConfirmed = Convert.ToBoolean(reader["PhoneNumberConfirmed"]);
        }

        public static void LoadFromReader(this SiteUser user, IDataReader reader)
        {
            user.UserId = Convert.ToInt32(reader["UserID"], CultureInfo.InvariantCulture);
            user.SiteId = Convert.ToInt32(reader["SiteID"], CultureInfo.InvariantCulture);
            user.DisplayName = reader["Name"].ToString();
            user.UserName = reader["LoginName"].ToString();
            
            user.Email = reader["Email"].ToString();
            user.LoweredEmail = reader["LoweredEmail"].ToString();
            user.PasswordQuestion = reader["PasswordQuestion"].ToString();
            user.PasswordAnswer = reader["PasswordAnswer"].ToString();
            user.Gender = reader["Gender"].ToString();

            if (reader["ProfileApproved"] != DBNull.Value)
            {
                user.ProfileApproved = Convert.ToBoolean(reader["ProfileApproved"]);
            }

            if (reader["RegisterConfirmGuid"] != DBNull.Value)
            {
                user.RegisterConfirmGuid = new Guid(reader["RegisterConfirmGuid"].ToString());
            }
            if (reader["ApprovedForForums"] != DBNull.Value)
            {
                user.ApprovedForLogin = Convert.ToBoolean(reader["ApprovedForForums"]);
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

            //legacy fields
            //user.Occupation = reader["Occupation"].ToString();
            //user.Interests = reader["Interests"].ToString();
            //user.MSN = reader["MSN"].ToString();
            //user.Yahoo = reader["Yahoo"].ToString();
            //user.AIM = reader["AIM"].ToString();
            //user.ICQ = reader["ICQ"].ToString();
            //user.TimeOffsetHours = Convert.ToInt32(reader["TimeOffsetHours"]);

            user.TotalPosts = Convert.ToInt32(reader["TotalPosts"], CultureInfo.InvariantCulture);
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
            user.Skin = reader["Skin"].ToString();
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
            user.MobilePin = reader["MobilePIN"].ToString();

            user.Comment = reader["Comment"].ToString();
            user.OpenIdUri = reader["OpenIDURI"].ToString();
            user.WindowsLiveId = reader["WindowsLiveID"].ToString();
            user.SiteGuid = new Guid(reader["SiteGuid"].ToString());

            if (reader["TotalRevenue"] != DBNull.Value)
            {
                user.TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"]);
            }

            user.FirstName = reader["FirstName"].ToString();
            user.LastName = reader["LastName"].ToString();

            user.MustChangePwd = Convert.ToBoolean(reader["MustChangePwd"]);
            user.NewEmail = reader["NewEmail"].ToString();
            user.EditorPreference = reader["EditorPreference"].ToString();
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

            user.Password = reader["Pwd"].ToString();
            user.PasswordFormat = Convert.ToInt32(reader["PwdFormat"]);
            user.PasswordHash = reader["PasswordHash"].ToString();
            user.PasswordSalt = reader["PasswordSalt"].ToString();

            if (user.PasswordHash.Length == 0)
            {

                user.PasswordHash =
                    user.Password + "|"
                    + user.PasswordSalt + "|"
                    + user.PasswordFormat.ToString(CultureInfo.InvariantCulture)
                    ;

            }

        }

        public static void LoadFromReader(this ISiteInfo site, IDataReader reader)
        {
            site.SiteId = Convert.ToInt32(reader["SiteID"]);
            site.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            site.SiteName = reader["SiteName"].ToString();
            site.SiteFolderName = reader["ApiKeyExtra4"].ToString();
            site.PreferredHostName = reader["ApiKeyExtra5"].ToString();
            site.IsServerAdminSite = Convert.ToBoolean(reader["IsServerAdminSite"]);
        }

        public static void LoadFromReader(this ISiteSettings site, IDataReader reader)
        {
            site.SiteId = Convert.ToInt32(reader["SiteID"]);
            site.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            //site.SiteAlias = reader["SiteAlias"].ToString();
            site.SiteName = reader["SiteName"].ToString();
            site.Skin = reader["Skin"].ToString();
            site.Logo = reader["Logo"].ToString();
            site.Icon = reader["Icon"].ToString();
            site.AllowUserSkins = Convert.ToBoolean(reader["AllowUserSkins"]);
            site.AllowPageSkins = Convert.ToBoolean(reader["AllowPageSkins"]);
            site.AllowHideMenuOnPages = Convert.ToBoolean(reader["AllowHideMenuOnPages"]);
            site.AllowNewRegistration = Convert.ToBoolean(reader["AllowNewRegistration"]);
            site.UseSecureRegistration = Convert.ToBoolean(reader["UseSecureRegistration"]);
            site.UseSslOnAllPages = Convert.ToBoolean(reader["UseSSLOnAllPages"]);

            site.IsServerAdminSite = Convert.ToBoolean(reader["IsServerAdminSite"]);
            site.UseLdapAuth = Convert.ToBoolean(reader["UseLdapAuth"]);
            site.AutoCreateLdapUserOnFirstLogin = Convert.ToBoolean(reader["AutoCreateLdapUserOnFirstLogin"]);

            if (site.SiteLdapSettings == null) { site.SiteLdapSettings = new LdapSettings(); }
            site.SiteLdapSettings.Server = reader["LdapServer"].ToString();
            site.SiteLdapSettings.Port = Convert.ToInt32(reader["LdapPort"]);
            site.SiteLdapSettings.Domain = reader["LdapDomain"].ToString();
            site.SiteLdapSettings.RootDN = reader["LdapRootDN"].ToString();
            site.SiteLdapSettings.UserDNKey = reader["LdapUserDNKey"].ToString();

            site.ReallyDeleteUsers = Convert.ToBoolean(reader["ReallyDeleteUsers"]);
            site.UseEmailForLogin = Convert.ToBoolean(reader["UseEmailForLogin"]);
            site.AllowUserFullNameChange = Convert.ToBoolean(reader["AllowUserFullNameChange"]);
            //site.EditorSkin = reader["EditorSkin"].ToString();
            //site.DefaultFriendlyUrlPatternEnum = reader["DefaultFriendlyUrlPatternEnum"].ToString();
            site.AllowPasswordRetrieval = Convert.ToBoolean(reader["AllowPasswordRetrieval"]);
            site.AllowPasswordReset = Convert.ToBoolean(reader["AllowPasswordReset"]);
            site.RequiresQuestionAndAnswer = Convert.ToBoolean(reader["RequiresQuestionAndAnswer"]);
            site.MaxInvalidPasswordAttempts = Convert.ToInt32(reader["MaxInvalidPasswordAttempts"]);
            site.PasswordAttemptWindowMinutes = Convert.ToInt32(reader["PasswordAttemptWindowMinutes"]);
            //site.RequiresUniqueEmail = Convert.ToBoolean(reader["RequiresUniqueEmail"]);
            site.PasswordFormat = Convert.ToInt32(reader["PasswordFormat"]);
            site.MinRequiredPasswordLength = Convert.ToInt32(reader["MinRequiredPasswordLength"]);
            site.MinRequiredNonAlphanumericCharacters = Convert.ToInt32(reader["MinReqNonAlphaChars"]);
            site.PasswordStrengthRegularExpression = reader["PwdStrengthRegex"].ToString();
            site.DefaultEmailFromAddress = reader["DefaultEmailFromAddress"].ToString();
            //site.EnableMyPageFeature = Convert.ToBoolean(reader["EnableMyPageFeature"]);
            site.EditorProviderName = reader["EditorProvider"].ToString();
            site.CaptchaProvider = reader["CaptchaProvider"].ToString();
            //site.DatePickerProvider = reader["DatePickerProvider"].ToString();
            site.RecaptchaPrivateKey = reader["RecaptchaPrivateKey"].ToString();
            site.RecaptchaPublicKey = reader["RecaptchaPublicKey"].ToString();
            site.WordpressApiKey = reader["WordpressAPIKey"].ToString();
            site.WindowsLiveAppId = reader["WindowsLiveAppID"].ToString();
            site.WindowsLiveKey = reader["WindowsLiveKey"].ToString();
            //site.AllowOpenIDAuth = Convert.ToBoolean(reader["AllowOpenIDAuth"]);
            //site.AllowWindowsLiveAuth = Convert.ToBoolean(reader["AllowWindowsLiveAuth"]);
            site.GmapApiKey = reader["GmapApiKey"].ToString();
            site.AddThisDotComUsername = reader["ApiKeyExtra1"].ToString();
            site.GoogleAnalyticsAccountCode = reader["ApiKeyExtra2"].ToString();
            //site.ApiKeyExtra3 = reader["ApiKeyExtra3"].ToString();
            site.SiteFolderName = reader["ApiKeyExtra4"].ToString();
            site.PreferredHostName = reader["ApiKeyExtra5"].ToString();
            site.DisableDbAuth = Convert.ToBoolean(reader["DisableDbAuth"]);

        }


        public static void LoadFromReader(this ISiteFolder folder, IDataReader reader)
        {
            folder.Guid = new Guid(reader["Guid"].ToString());
            folder.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            folder.FolderName = reader["FolderName"].ToString();
        }

        public static void LoadFromReader(this ISiteHost host, IDataReader reader)
        {
            host.HostId = Convert.ToInt32(reader["HostID"]);
            host.HostName = reader["HostName"].ToString();
            host.SiteGuid = new Guid(reader["SiteGuid"].ToString());
            host.SiteId = Convert.ToInt32(reader["SiteID"]);
        }

        public static void LoadExpandoSettings(this ISiteSettings site, DataTable expandoProperties)
        {
            string b = GetExpandoProperty(expandoProperties, "AllowPersistentLogin");
            if (!string.IsNullOrEmpty(b)) { site.AllowPersistentLogin = Convert.ToBoolean(b); }

            site.AvatarSystem = GetExpandoProperty(expandoProperties, "AvatarSystem");
            site.CommentProvider = GetExpandoProperty(expandoProperties, "CommentProvider");
            site.CompanyPublicEmail = GetExpandoProperty(expandoProperties, "CompanyPublicEmail");
            site.CompanyFax = GetExpandoProperty(expandoProperties, "CompanyFax");
            site.CompanyPhone = GetExpandoProperty(expandoProperties, "CompanyPhone");
            site.CompanyCountry = GetExpandoProperty(expandoProperties, "CompanyCountry");
            site.CompanyPostalCode = GetExpandoProperty(expandoProperties, "CompanyPostalCode");
            site.CompanyRegion = GetExpandoProperty(expandoProperties, "CompanyRegion");
            site.CompanyLocality = GetExpandoProperty(expandoProperties, "CompanyLocality");
            site.CompanyStreetAddress = GetExpandoProperty(expandoProperties, "CompanyStreetAddress");
            site.CompanyStreetAddress2 = GetExpandoProperty(expandoProperties, "CompanyStreetAddress2");
            site.CompanyName = GetExpandoProperty(expandoProperties, "CompanyName");

            string g = GetExpandoProperty(expandoProperties, "CurrencyGuid");
            if (g.Length == 36) { site.CurrencyGuid = new Guid(g); }
            g = GetExpandoProperty(expandoProperties, "DefaultStateGuid");
            if (g.Length == 36) { site.DefaultStateGuid = new Guid(g); }
            g = GetExpandoProperty(expandoProperties, "DefaultCountryGuid");
            if (g.Length == 36) { site.DefaultCountryGuid = new Guid(g); }

            site.DefaultRootPageCreateChildPageRoles = GetExpandoProperty(expandoProperties, "DefaultRootPageCreateChildPageRoles");

            site.DefaultRootPageEditRoles = GetExpandoProperty(expandoProperties, "DefaultRootPageEditRoles");
            site.DefaultRootPageViewRoles = GetExpandoProperty(expandoProperties, "DefaultRootPageViewRoles");
            site.DisqusSiteShortName = GetExpandoProperty(expandoProperties, "DisqusSiteShortName");
            site.EmailAdressesForUserApprovalNotification = GetExpandoProperty(expandoProperties, "EmailAdressesForUserApprovalNotification");
            b = GetExpandoProperty(expandoProperties, "EnableContentWorkflow");
            if (!string.IsNullOrEmpty(b)) { site.EnableContentWorkflow = Convert.ToBoolean(b); }
            site.FacebookAppId = GetExpandoProperty(expandoProperties, "FacebookAppId");
            b = GetExpandoProperty(expandoProperties, "ForceContentVersioning");
            if (!string.IsNullOrEmpty(b)) { site.ForceContentVersioning = Convert.ToBoolean(b); }
            site.GoogleAnalyticsSettings = GetExpandoProperty(expandoProperties, "GoogleAnalyticsSettings");
            site.GoogleAnalyticsProfileId = GetExpandoProperty(expandoProperties, "GoogleAnalyticsProfileId");
            //SetExpandoProperty(expandoProperties, "GoogleAnalyticsPassword", site.GoogleAnalyticsPassword);
            //SetExpandoProperty(expandoProperties, "GoogleAnalyticsEmail", site.GoogleAnalyticsEmail);
            site.IntenseDebateAccountId = GetExpandoProperty(expandoProperties, "IntenseDebateAccountId");
            site.LoginInfoBottom = GetExpandoProperty(expandoProperties, "LoginInfoBottom");
            site.LoginInfoBottom = GetExpandoProperty(expandoProperties, "LoginInfoTop");
            site.MetaProfile = GetExpandoProperty(expandoProperties, "MetaProfile");
            site.NewsletterEditor = GetExpandoProperty(expandoProperties, "NewsletterEditor");
            site.PasswordRegexWarning = GetExpandoProperty(expandoProperties, "PasswordRegexWarning");
            site.PrivacyPolicyUrl = GetExpandoProperty(expandoProperties, "PrivacyPolicyUrl");
            site.RegistrationAgreement = GetExpandoProperty(expandoProperties, "RegistrationAgreement");
            site.RegistrationPreamble = GetExpandoProperty(expandoProperties, "RegistrationPreamble");

            b = GetExpandoProperty(expandoProperties, "RequireApprovalBeforeLogin");
            if (!string.IsNullOrEmpty(b)) { site.RequireApprovalBeforeLogin = Convert.ToBoolean(b); }

            // permission roles
            site.RolesThatCanApproveNewUsers = GetExpandoProperty(expandoProperties, "RolesThatCanApproveNewUsers");
            site.RolesThatCanManageSkins = GetExpandoProperty(expandoProperties, "RolesThatCanManageSkins");
            site.RolesThatCanAssignSkinsToPages = GetExpandoProperty(expandoProperties, "RolesThatCanAssignSkinsToPages");
            site.RolesThatCanDeleteFilesInEditor = GetExpandoProperty(expandoProperties, "RolesThatCanDeleteFilesInEditor");
            site.UserFilesBrowseAndUploadRoles = GetExpandoProperty(expandoProperties, "UserFilesBrowseAndUploadRoles");
            site.GeneralBrowseAndUploadRoles = GetExpandoProperty(expandoProperties, "GeneralBrowseAndUploadRoles");
            site.RolesThatCanEditContentTemplates = GetExpandoProperty(expandoProperties, "RolesThatCanEditContentTemplates");
            site.RolesNotAllowedToEditModuleSettings = GetExpandoProperty(expandoProperties, "RolesNotAllowedToEditModuleSettings");
            site.RolesThatCanLookupUsers = GetExpandoProperty(expandoProperties, "RolesThatCanLookupUsers");
            site.RolesThatCanManageUsers = GetExpandoProperty(expandoProperties, "RolesThatCanFullyManageUsers");
            site.RolesThatCanCreateUsers = GetExpandoProperty(expandoProperties, "RolesThatCanManageUsers");
            site.RolesThatCanViewMemberList = GetExpandoProperty(expandoProperties, "RolesThatCanViewMemberList");
            site.RolesThatCanCreateRootPages = GetExpandoProperty(expandoProperties, "RolesThatCanCreateRootPages");
            site.CommerceReportViewRoles = GetExpandoProperty(expandoProperties, "CommerceReportViewRoles");
            site.SiteRootDraftApprovalRoles = GetExpandoProperty(expandoProperties, "SiteRootDraftApprovalRoles");
            site.SiteRootDraftEditRoles = GetExpandoProperty(expandoProperties, "SiteRootDraftEditRoles");
            site.SiteRootEditRoles = GetExpandoProperty(expandoProperties, "SiteRootEditRoles");

            // end roles

            b = GetExpandoProperty(expandoProperties, "SiteIsClosed");
            if (!string.IsNullOrEmpty(b)) { site.SiteIsClosed = Convert.ToBoolean(b); }

            site.SiteIsClosedMessage = GetExpandoProperty(expandoProperties, "SiteIsClosedMessage");

            g = GetExpandoProperty(expandoProperties, "SkinVersion");
            if (!string.IsNullOrEmpty(g))
            {
                if (g.Length == 36) { site.SkinVersion = new Guid(g); }
            }

            b = GetExpandoProperty(expandoProperties, "SMTPUseSsl");
            if (!string.IsNullOrEmpty(b)) { site.SMTPUseSsl = Convert.ToBoolean(b); }

            b = GetExpandoProperty(expandoProperties, "SMTPRequiresAuthentication");
            if (!string.IsNullOrEmpty(b)) { site.SMTPRequiresAuthentication = Convert.ToBoolean(b); }


            site.SMTPServer = GetExpandoProperty(expandoProperties, "SMTPServer");
            site.SMTPPreferredEncoding = GetExpandoProperty(expandoProperties, "SMTPPreferredEncoding");
            string i = GetExpandoProperty(expandoProperties, "SMTPPort");
            if (!string.IsNullOrEmpty(i)) { site.SMTPPort = Convert.ToInt32(i); }
            site.SMTPPassword = GetExpandoProperty(expandoProperties, "SMTPPassword");
            site.SMTPUser = GetExpandoProperty(expandoProperties, "SMTPUser");
            site.Slogan = GetExpandoProperty(expandoProperties, "Slogan");

            b = GetExpandoProperty(expandoProperties, "ShowAlternateSearchIfConfigured");
            if (!string.IsNullOrEmpty(b)) { site.ShowAlternateSearchIfConfigured = Convert.ToBoolean(b); }

            site.PrimarySearchEngine = GetExpandoProperty(expandoProperties, "PrimarySearchEngine");
            site.GoogleCustomSearchId = GetExpandoProperty(expandoProperties, "GoogleCustomSearchId");
            site.BingAPIId = GetExpandoProperty(expandoProperties, "BingAPIId");
            site.OpenSearchName = GetExpandoProperty(expandoProperties, "OpenSearchName");
            site.RpxNowAdminUrl = GetExpandoProperty(expandoProperties, "RpxNowAdminUrl");
            site.RpxNowApplicationName = GetExpandoProperty(expandoProperties, "RpxNowApplicationName");
            site.RpxNowApiKey = GetExpandoProperty(expandoProperties, "RpxNowApiKey");
            //site.AppLogoForWindowsLive = GetExpandoProperty(expandoProperties, "AppLogoForWindowsLive");

            site.SiteMapSkin = GetExpandoProperty(expandoProperties, "SiteMapSkin");
            site.TimeZoneId = GetExpandoProperty(expandoProperties, "TimeZoneId");
            //SetExpandoProperty(expandoProperties, "ShowPasswordStrengthOnRegistration", site.ShowPasswordStrengthOnRegistration);
            //SetExpandoProperty(expandoProperties, "RequireEnterEmailTwiceOnRegistration", site.RequireEnterEmailTwiceOnRegistration);

            b = GetExpandoProperty(expandoProperties, "RequireCaptchaOnLogin");
            if (!string.IsNullOrEmpty(b)) { site.RequireCaptchaOnLogin = Convert.ToBoolean(b); }

            b = GetExpandoProperty(expandoProperties, "RequireCaptchaOnRegistration");
            if (!string.IsNullOrEmpty(b)) { site.RequireCaptchaOnRegistration = Convert.ToBoolean(b); }




        }

        public static void SetExpandoSettings(this ISiteSettings site, DataTable expandoProperties)
        {
            SetExpandoProperty(expandoProperties, "AvatarSystem", site.AvatarSystem);
            //SetExpandoProperty(expandoProperties, "AllowUserEditorPreference", site.AllowUserEditorPreference);
            SetExpandoProperty(expandoProperties, "CommentProvider", site.CommentProvider);
            SetExpandoProperty(expandoProperties, "CompanyPublicEmail", site.CompanyPublicEmail);
            SetExpandoProperty(expandoProperties, "CompanyFax", site.CompanyFax);
            SetExpandoProperty(expandoProperties, "CompanyPhone", site.CompanyPhone);
            SetExpandoProperty(expandoProperties, "CompanyCountry", site.CompanyCountry);
            SetExpandoProperty(expandoProperties, "CompanyPostalCode", site.CompanyPostalCode);
            SetExpandoProperty(expandoProperties, "CompanyRegion", site.CompanyRegion);
            SetExpandoProperty(expandoProperties, "CompanyLocality", site.CompanyLocality);
            SetExpandoProperty(expandoProperties, "CompanyStreetAddress", site.CompanyStreetAddress);
            SetExpandoProperty(expandoProperties, "CompanyStreetAddress2", site.CompanyStreetAddress2);
            SetExpandoProperty(expandoProperties, "CompanyName", site.CompanyName);
            SetExpandoProperty(expandoProperties, "CurrencyGuid", site.CurrencyGuid.ToString());
            SetExpandoProperty(expandoProperties, "DefaultStateGuid", site.DefaultStateGuid.ToString());
            SetExpandoProperty(expandoProperties, "DefaultCountryGuid", site.DefaultCountryGuid.ToString());
            SetExpandoProperty(expandoProperties, "DefaultRootPageCreateChildPageRoles", site.DefaultRootPageCreateChildPageRoles);
            SetExpandoProperty(expandoProperties, "DefaultRootPageEditRoles", site.DefaultRootPageEditRoles);
            SetExpandoProperty(expandoProperties, "DefaultRootPageViewRoles", site.DefaultRootPageViewRoles);
            SetExpandoProperty(expandoProperties, "DisqusSiteShortName", site.DisqusSiteShortName);
            SetExpandoProperty(expandoProperties, "EmailAdressesForUserApprovalNotification", site.EmailAdressesForUserApprovalNotification);
            SetExpandoProperty(expandoProperties, "EnableContentWorkflow", site.EnableContentWorkflow.ToString());
            SetExpandoProperty(expandoProperties, "FacebookAppId", site.FacebookAppId);
            SetExpandoProperty(expandoProperties, "ForceContentVersioning", site.ForceContentVersioning.ToString());
            SetExpandoProperty(expandoProperties, "GoogleAnalyticsSettings", site.GoogleAnalyticsSettings);
            SetExpandoProperty(expandoProperties, "GoogleAnalyticsProfileId", site.GoogleAnalyticsProfileId);
            //SetExpandoProperty(expandoProperties, "GoogleAnalyticsPassword", site.GoogleAnalyticsPassword);
            //SetExpandoProperty(expandoProperties, "GoogleAnalyticsEmail", site.GoogleAnalyticsEmail);
            SetExpandoProperty(expandoProperties, "IntenseDebateAccountId", site.IntenseDebateAccountId);
            SetExpandoProperty(expandoProperties, "LoginInfoBottom", site.LoginInfoBottom);
            SetExpandoProperty(expandoProperties, "LoginInfoTop", site.LoginInfoBottom);
            SetExpandoProperty(expandoProperties, "MetaProfile", site.MetaProfile);
            SetExpandoProperty(expandoProperties, "NewsletterEditor", site.NewsletterEditor);
            SetExpandoProperty(expandoProperties, "PasswordRegexWarning", site.PasswordRegexWarning);
            SetExpandoProperty(expandoProperties, "PrivacyPolicyUrl", site.PrivacyPolicyUrl);
            SetExpandoProperty(expandoProperties, "RegistrationAgreement", site.RegistrationAgreement);
            SetExpandoProperty(expandoProperties, "RegistrationPreamble", site.RegistrationPreamble);
            SetExpandoProperty(expandoProperties, "RequireApprovalBeforeLogin", site.RequireApprovalBeforeLogin.ToString());

            // permission roles
            SetExpandoProperty(expandoProperties, "RolesThatCanApproveNewUsers", site.RolesThatCanApproveNewUsers);
            SetExpandoProperty(expandoProperties, "RolesThatCanManageSkins", site.RolesThatCanManageSkins);
            SetExpandoProperty(expandoProperties, "RolesThatCanAssignSkinsToPages", site.RolesThatCanAssignSkinsToPages);
            SetExpandoProperty(expandoProperties, "RolesThatCanDeleteFilesInEditor", site.RolesThatCanDeleteFilesInEditor);
            SetExpandoProperty(expandoProperties, "UserFilesBrowseAndUploadRoles", site.UserFilesBrowseAndUploadRoles);
            SetExpandoProperty(expandoProperties, "GeneralBrowseAndUploadRoles", site.GeneralBrowseAndUploadRoles);
            SetExpandoProperty(expandoProperties, "RolesThatCanEditContentTemplates", site.RolesThatCanEditContentTemplates);
            SetExpandoProperty(expandoProperties, "RolesNotAllowedToEditModuleSettings", site.RolesNotAllowedToEditModuleSettings);
            SetExpandoProperty(expandoProperties, "RolesThatCanLookupUsers", site.RolesThatCanLookupUsers);
            SetExpandoProperty(expandoProperties, "RolesThatCanFullyManageUsers", site.RolesThatCanManageUsers);
            SetExpandoProperty(expandoProperties, "RolesThatCanManageUsers", site.RolesThatCanCreateUsers);
            SetExpandoProperty(expandoProperties, "RolesThatCanViewMemberList", site.RolesThatCanViewMemberList);
            SetExpandoProperty(expandoProperties, "RolesThatCanCreateRootPages", site.RolesThatCanCreateRootPages);
            SetExpandoProperty(expandoProperties, "CommerceReportViewRoles", site.CommerceReportViewRoles);
            SetExpandoProperty(expandoProperties, "SiteRootDraftApprovalRoles", site.SiteRootDraftApprovalRoles);
            SetExpandoProperty(expandoProperties, "SiteRootDraftEditRoles", site.SiteRootDraftEditRoles);
            SetExpandoProperty(expandoProperties, "SiteRootEditRoles", site.SiteRootEditRoles);

            // end roles

            SetExpandoProperty(expandoProperties, "SiteIsClosed", site.SiteIsClosed.ToString());
            SetExpandoProperty(expandoProperties, "SiteIsClosedMessage", site.SiteIsClosedMessage);
            SetExpandoProperty(expandoProperties, "SkinVersion", site.SkinVersion.ToString());
            SetExpandoProperty(expandoProperties, "SMTPUseSsl", site.SMTPUseSsl.ToString());
            SetExpandoProperty(expandoProperties, "SMTPRequiresAuthentication", site.SMTPRequiresAuthentication.ToString());
            SetExpandoProperty(expandoProperties, "SMTPServer", site.SMTPServer);
            SetExpandoProperty(expandoProperties, "SMTPPreferredEncoding", site.SMTPPreferredEncoding);
            SetExpandoProperty(expandoProperties, "SMTPPort", site.SMTPPort.ToString(CultureInfo.InvariantCulture));
            SetExpandoProperty(expandoProperties, "SMTPPassword", site.SMTPPassword);
            SetExpandoProperty(expandoProperties, "SMTPUser", site.SMTPUser);
            SetExpandoProperty(expandoProperties, "Slogan", site.Slogan);
            SetExpandoProperty(expandoProperties, "ShowAlternateSearchIfConfigured", site.ShowAlternateSearchIfConfigured.ToString());
            SetExpandoProperty(expandoProperties, "PrimarySearchEngine", site.PrimarySearchEngine);
            SetExpandoProperty(expandoProperties, "GoogleCustomSearchId", site.GoogleCustomSearchId);
            SetExpandoProperty(expandoProperties, "BingAPIId", site.BingAPIId);
            SetExpandoProperty(expandoProperties, "OpenSearchName", site.OpenSearchName);
            SetExpandoProperty(expandoProperties, "RpxNowAdminUrl", site.RpxNowAdminUrl);
            SetExpandoProperty(expandoProperties, "RpxNowApplicationName", site.RpxNowApplicationName);
            SetExpandoProperty(expandoProperties, "RpxNowApiKey", site.RpxNowApiKey);
            //SetExpandoProperty(expandoProperties, "AppLogoForWindowsLive", site.AppLogoForWindowsLive);

            SetExpandoProperty(expandoProperties, "SiteMapSkin", site.SiteMapSkin);
            SetExpandoProperty(expandoProperties, "TimeZoneId", site.TimeZoneId);
            //SetExpandoProperty(expandoProperties, "ShowPasswordStrengthOnRegistration", site.ShowPasswordStrengthOnRegistration);
            //SetExpandoProperty(expandoProperties, "RequireEnterEmailTwiceOnRegistration", site.RequireEnterEmailTwiceOnRegistration);
            SetExpandoProperty(expandoProperties, "RequireCaptchaOnLogin", site.RequireCaptchaOnLogin.ToString());
            SetExpandoProperty(expandoProperties, "RequireCaptchaOnRegistration", site.RequireCaptchaOnRegistration.ToString());
            SetExpandoProperty(expandoProperties, "AllowPersistentLogin", site.AllowPersistentLogin.ToString());
            
        }

        private static string GetExpandoProperty(DataTable exapandoProperties, string keyName)
        {
            //EnsureExpandoProperties();

            foreach (DataRow row in exapandoProperties.Rows)
            {
                if (row["KeyName"].ToString().Trim().Equals(keyName, StringComparison.InvariantCulture))
                {
                    return row["KeyValue"].ToString();
                }

            }

            return null;

        }

        private static void SetExpandoProperty(DataTable exapandoProperties, string keyName, string keyValue)
        {
            //EnsureExpandoProperties();
            //bool found = false;
            foreach (DataRow row in exapandoProperties.Rows)
            {
                if (row["KeyName"].ToString().Trim().Equals(keyName, StringComparison.InvariantCulture))
                {
                    if (row["KeyValue"].ToString() != keyValue)
                    {
                        row["KeyValue"] = keyValue;
                        row["IsDirty"] = true;
                    }

                    //found = true;
                    break;
                }

            }



        }

        public static DataTable CreateExpandoTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.TableName = "expandoProperties";
            dataTable.Columns.Add("SiteID", typeof(int));
            dataTable.Columns.Add("KeyName", typeof(string));
            dataTable.Columns.Add("KeyValue", typeof(string));
            dataTable.Columns.Add("GroupName", typeof(string));
            dataTable.Columns.Add("IsDirty", typeof(bool));

            return dataTable;

        }
    }
}
