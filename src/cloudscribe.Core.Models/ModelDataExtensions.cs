// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-09
// Last Modified:			2016-05-17
// 

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;


namespace cloudscribe.Core.Models.DataExtensions
{
    public static class ModelDataExtensions
    {
        public static void LoadFromReader(this ISiteRole role, DbDataReader reader)
        {
            role.NormalizedRoleName = reader["RoleName"].ToString();
            role.RoleName = reader["DisplayName"].ToString();
            role.SiteId = new Guid(reader["SiteGuid"].ToString());
            role.Id = new Guid(reader["RoleGuid"].ToString());
        }

        public static void LoadFromReader(this IUserLogin userLogin, DbDataReader reader)
        {
            userLogin.SiteId = new Guid(reader["SiteGuid"].ToString());
            userLogin.LoginProvider = reader["LoginProvider"].ToString();
            userLogin.ProviderKey = reader["ProviderKey"].ToString();
            userLogin.UserId = new Guid(reader["UserId"].ToString());
            userLogin.ProviderDisplayName = reader["ProviderDisplayName"].ToString();
        }

        public static void LoadFromReader(this IUserClaim userClaim, DbDataReader reader)
        {
            userClaim.SiteId = new Guid(reader["SiteId"].ToString());
            userClaim.Id = new Guid(reader["Id"].ToString());
            userClaim.UserId = new Guid(reader["UserId"].ToString());
            userClaim.ClaimType = reader["ClaimType"].ToString();
            userClaim.ClaimValue = reader["ClaimValue"].ToString();
        }

        public static void LoadFromReader(this IUserInfo user, DbDataReader reader)
        {
           //TODO: this got broken by the change to require id passed in the constructor of SuteUser
            //if (reader["UserGuid"] != DBNull.Value)
            //{
            //    user.Id = new Guid(reader["UserGuid"].ToString());
            //}
            
            user.SiteId = new Guid(reader["SiteGuid"].ToString());
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
            
            if (reader["DisplayInMemberList"] != DBNull.Value)
            {
                user.DisplayInMemberList = Convert.ToBoolean(reader["DisplayInMemberList"]);
            }
            user.WebSiteUrl = reader["WebSiteURL"].ToString();
            //user.Country = reader["Country"].ToString();
            //user.State = reader["State"].ToString();
           
            user.AvatarUrl = reader["AvatarUrl"].ToString();

            if (reader["DateCreated"] != DBNull.Value)
            {
                user.CreatedUtc = Convert.ToDateTime(reader["DateCreated"]);
            }

            //if (reader["IsDeleted"] != DBNull.Value)
            //{
            //    user.IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
            //}
            
            if (reader["LastLoginDate"] != DBNull.Value)
            {
                user.LastLoginUtc = Convert.ToDateTime(reader["LastLoginDate"]);
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
            //TODO: this is broken because SiteUser must nnow have id in consrtructor
            //if (reader["UserGuid"] != DBNull.Value)
            //{
            //    user.Id = new Guid(reader["UserGuid"].ToString());
            //} 

            user.DisplayName = reader["Name"].ToString();
            user.UserName = reader["LoginName"].ToString();

            user.Email = reader["Email"].ToString();
            user.NormalizedEmail = reader["LoweredEmail"].ToString();
            
            user.Gender = reader["Gender"].ToString();

            if (reader["AccountApproved"] != DBNull.Value)
            {
                user.AccountApproved = Convert.ToBoolean(reader["AccountApproved"]);
            }
            
            
            if (reader["DisplayInMemberList"] != DBNull.Value)
            {
                user.DisplayInMemberList = Convert.ToBoolean(reader["DisplayInMemberList"]);
            }
            user.WebSiteUrl = reader["WebSiteURL"].ToString();
            //user.Country = reader["Country"].ToString();
            //user.State = reader["State"].ToString();
            
            user.AvatarUrl = reader["AvatarUrl"].ToString();

            user.Signature = reader["Signature"].ToString();
            if (reader["DateCreated"] != DBNull.Value)
            {
                user.CreatedUtc = Convert.ToDateTime(reader["DateCreated"]);
            }
            
            
            //if (reader["IsDeleted"] != DBNull.Value)
            //{
            //    user.IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
            //}
            
            if (reader["LastLoginDate"] != DBNull.Value)
            {
                user.LastLoginUtc = Convert.ToDateTime(reader["LastLoginDate"]);
            }
            if (reader["LastPasswordChangedDate"] != DBNull.Value)
            {
                user.LastPasswordChangeUtc = Convert.ToDateTime(reader["LastPasswordChangedDate"]);
            }
            
            if (reader["FailedPasswordAttemptCount"] != DBNull.Value)
            {
                user.AccessFailedCount = Convert.ToInt32(reader["FailedPasswordAttemptCount"]);
            }
           
            
            
            if (reader["IsLockedOut"] != DBNull.Value)
            {
                user.IsLockedOut = Convert.ToBoolean(reader["IsLockedOut"]);
            }
            
            user.Comment = reader["Comment"].ToString();
           
            user.SiteId = new Guid(reader["SiteGuid"].ToString());
            
            user.FirstName = reader["FirstName"].ToString();
            user.LastName = reader["LastName"].ToString();

            user.MustChangePwd = Convert.ToBoolean(reader["MustChangePwd"]);
            user.NewEmail = reader["NewEmail"].ToString();
            
            
            user.TimeZoneId = reader["TimeZoneId"].ToString();
            
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

            if (reader["EmailConfirmSentUtc"] != DBNull.Value)
            {
                user.EmailConfirmSentUtc = Convert.ToDateTime(reader["EmailConfirmSentUtc"]);
            }

            if (reader["AgreementAcceptedUtc"] != DBNull.Value)
            {
                user.AgreementAcceptedUtc = Convert.ToDateTime(reader["AgreementAcceptedUtc"]);
            }

            user.PasswordHash = reader["PasswordHash"].ToString();
            user.NormalizedUserName = reader["NormalizedUserName"].ToString();
            user.NewEmailApproved = Convert.ToBoolean(reader["NewEmailApproved"]);
            user.CanAutoLockout = Convert.ToBoolean(reader["CanAutoLockout"]);

        }

        public static void LoadFromReader(this ISiteInfo site, DbDataReader reader)
        {
            
            site.Id = new Guid(reader["SiteGuid"].ToString());
            site.SiteName = reader["SiteName"].ToString();
            site.SiteFolderName = reader["SiteFolderName"].ToString();
            site.PreferredHostName = reader["PreferredHostName"].ToString();
            site.IsServerAdminSite = Convert.ToBoolean(reader["IsServerAdminSite"]);
        }

        public static void LoadFromReader(this ISiteSettings site, DbDataReader reader)
        {
            
            site.Id = new Guid(reader["SiteGuid"].ToString());
            
            site.SiteName = reader["SiteName"].ToString();
            site.Theme = reader["Skin"].ToString();
            site.AllowNewRegistration = Convert.ToBoolean(reader["AllowNewRegistration"]);
            site.RequireConfirmedEmail = Convert.ToBoolean(reader["UseSecureRegistration"]);
            
            site.IsServerAdminSite = Convert.ToBoolean(reader["IsServerAdminSite"]);
            
            
            site.LdapServer = reader["LdapServer"].ToString();
            site.LdapPort = Convert.ToInt32(reader["LdapPort"]);
            site.LdapDomain = reader["LdapDomain"].ToString();
            site.LdapRootDN = reader["LdapRootDN"].ToString();
            site.LdapUserDNKey = reader["LdapUserDNKey"].ToString();
            site.LdapUserDNFormat = reader["LdapUserDNFormat"].ToString();

            //site.ReallyDeleteUsers = Convert.ToBoolean(reader["ReallyDeleteUsers"]);
            site.UseEmailForLogin = Convert.ToBoolean(reader["UseEmailForLogin"]);
            site.AllowUserToChangeEmail = Convert.ToBoolean(reader["AllowUserToChangeEmail"]);
            
            site.RequiresQuestionAndAnswer = Convert.ToBoolean(reader["RequiresQuestionAndAnswer"]);
            site.MaxInvalidPasswordAttempts = Convert.ToInt32(reader["MaxInvalidPasswordAttempts"]);
            
            site.MinRequiredPasswordLength = Convert.ToInt32(reader["MinRequiredPasswordLength"]);
            
            site.DefaultEmailFromAddress = reader["DefaultEmailFromAddress"].ToString();
            site.RecaptchaPrivateKey = reader["RecaptchaPrivateKey"].ToString();
            site.RecaptchaPublicKey = reader["RecaptchaPublicKey"].ToString();
            
            site.AddThisDotComUsername = reader["AddThisDotComUsername"].ToString();
            
            site.DisableDbAuth = Convert.ToBoolean(reader["DisableDbAuth"]);
            
            site.RequireApprovalBeforeLogin = Convert.ToBoolean(reader["RequireApprovalBeforeLogin"]);
            
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

            site.IsDataProtected = Convert.ToBoolean(reader["IsDataProtected"]);
            if(reader["CreatedUtc"] != DBNull.Value)
            {
                site.CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]);
            }

            site.RequireConfirmedPhone = Convert.ToBoolean(reader["RequireConfirmedPhone"]);
            site.DefaultEmailFromAlias = reader["DefaultEmailFromAlias"].ToString();
            site.AccountApprovalEmailCsv = reader["AccountApprovalEmailCsv"].ToString();
            site.DkimPublicKey = reader["DkimPublicKey"].ToString();
            site.DkimPrivateKey = reader["DkimPrivateKey"].ToString();
            site.DkimDomain = reader["DkimDomain"].ToString();
            site.DkimSelector = reader["DkimSelector"].ToString();
            site.SignEmailWithDkim = Convert.ToBoolean(reader["SignEmailWithDkim"]);
            site.OidConnectAppId = reader["OidConnectAppId"].ToString();
            site.OidConnectAppSecret = reader["OidConnectAppSecret"].ToString();
            site.OidConnectAuthority = reader["OidConnectAuthority"].ToString();
            site.SmsClientId = reader["SmsClientId"].ToString();
            site.SmsSecureToken = reader["SmsSecureToken"].ToString();
            site.SmsFrom = reader["SmsFrom"].ToString();


        }


        //public static void LoadFromReader(this ISiteFolder folder, DbDataReader reader)
        //{
        //    folder.Guid = new Guid(reader["Guid"].ToString());
        //    folder.SiteGuid = new Guid(reader["SiteGuid"].ToString());
        //    folder.FolderName = reader["FolderName"].ToString();
        //}

        public static void LoadFromReader(this ISiteHost host, DbDataReader reader)
        {
            host.Id = new Guid(reader["HostGuid"].ToString());
            host.HostName = reader["HostName"].ToString();
            host.SiteId = new Guid(reader["SiteGuid"].ToString());
           
        }

        public static void LoadFromReader(this IUserLocation location, DbDataReader reader)
        {
            location.Id = new Guid(reader["RowID"].ToString());
            location.UserId = new Guid(reader["UserGuid"].ToString());
            location.SiteId = new Guid(reader["SiteGuid"].ToString());
            location.IpAddress = reader["IPAddress"].ToString();
            location.IpAddressLong = Convert.ToInt64(reader["IPAddressLong"]);
            location.HostName = reader["Hostname"].ToString();
            location.Longitude = Convert.ToDouble(reader["Longitude"]);
            location.Latitude = Convert.ToDouble(reader["Latitude"]);
            location.Isp = reader["ISP"].ToString();
            location.Continent = reader["Continent"].ToString();
            location.Country = reader["Country"].ToString();
            location.Region = reader["Region"].ToString();
            location.City = reader["City"].ToString();
            location.TimeZone = reader["TimeZone"].ToString();
            location.CaptureCount = Convert.ToInt32(reader["CaptureCount"]);
            location.FirstCaptureUtc = Convert.ToDateTime(reader["FirstCaptureUTC"]);
            location.LastCaptureUtc = Convert.ToDateTime(reader["LastCaptureUTC"]);
        }

        public static void LoadExpandoSettings(this ISiteSettings site, List<ExpandoSetting> expandoProperties)
        {
            // this may go away

            //string b = GetExpandoProperty(expandoProperties, "AllowPersistentLogin");
            //if (!string.IsNullOrEmpty(b)) { site.AllowPersistentLogin = Convert.ToBoolean(b); }

            //site.AvatarSystem = GetExpandoProperty(expandoProperties, "AvatarSystem");
            //site.CommentProvider = GetExpandoProperty(expandoProperties, "CommentProvider");
            

        }

        public static void SetExpandoSettings(this ISiteSettings site, List<ExpandoSetting> expandoProperties)
        {
            //SetExpandoProperty(expandoProperties, "AvatarSystem", site.AvatarSystem);
            //SetExpandoProperty(expandoProperties, "AllowUserEditorPreference", site.AllowUserEditorPreference);
            
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

        

        
    }
}
