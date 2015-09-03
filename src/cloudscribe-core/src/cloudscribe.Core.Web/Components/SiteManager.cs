// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-22
// Last Modified:			2015-08-05
// 

using cloudscribe.Core.Models;
using Microsoft.Framework.OptionsModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteManager
    {

        public SiteManager(
            ISiteResolver siteResolver, 
            ISiteRepository siteRepository,
            IUserRepository userRepository,
            IOptions<MultiTenantOptions> multiTenantOptions)
        {
            resolver = siteResolver;
            siteRepo = siteRepository;
            userRepo = userRepository;
            this.multiTenantOptions = multiTenantOptions.Options;
        }

        private MultiTenantOptions multiTenantOptions;
        private ISiteResolver resolver;
        private ISiteRepository siteRepo;
        private IUserRepository userRepo;
        private ISiteSettings siteSettings = null;
        private ISiteSettings Site
        {
            get
            {
                if (siteSettings == null) { siteSettings = resolver.Resolve(); }
                return siteSettings;
            }
        }

        public ISiteSettings CurrentSite
        {
            get { return Site; }
        }



        public async Task<List<ISiteInfo>> GetPageOtherSites(
            int currentSiteId,
            int pageNumber,
            int pageSize)
        {
            return await siteRepo.GetPageOtherSites(currentSiteId, pageNumber, pageSize);

        }

        public async Task<int> CountOtherSites(int currentSiteId)
        {
            return await siteRepo.CountOtherSites(currentSiteId);
        }

        public async Task<ISiteSettings> Fetch(Guid siteGuid)
        {
            return await siteRepo.Fetch(siteGuid);
        }

        public async Task<ISiteSettings> Fetch(int siteId)
        {
            return await siteRepo.Fetch(siteId);
        }

        public async Task<bool> Save(ISiteSettings site)
        {
            return await siteRepo.Save(site);
        }

        public async Task<bool> Delete(ISiteSettings site)
        {
            // we will need a provider model or something similar here to
            // allow other features and 3rd party features to delete
            // related data when a site is deleted
            // TODO: implement
            // will ProviderModel be available in Core Framework or will we have to use something else
            // a way to use dependency injection?

            // delete users
            bool resultStep = await userRepo.DeleteClaimsBySite(site.SiteId);
            resultStep = await userRepo.DeleteLoginsBySite(site.SiteId);


            // the below method deletes a lot of things by siteid including the following tables
            // Exec mp_Sites_Delete
            // mp_UserRoles
            // mp_UserProperties
            // mp_UserLocation
            // mp_Users
            // mp_Roles
            // mp_SiteHosts
            // mp_SiteFolders
            // mp_RedirectList
            // mp_TaskQueue
            // mp_SiteSettingsEx
            // mp_Sites

            return await siteRepo.Delete(site.SiteId);
        }

        public async Task<SiteSettings> CreateNewSite(
            ConfigHelper config,
            bool isServerAdminSite)
        {
            //string templateFolderPath = GetMessageTemplateFolder();
            //string templateFolder = templateFolderPath;

            SiteSettings newSite = new SiteSettings();
            newSite.SiteName = "Sample Site";
            newSite.IsServerAdminSite = isServerAdminSite;

            bool result = await CreateNewSite(config, newSite);

            return newSite;


        }

        public async Task<bool> CreateNewSite(
            ConfigHelper config,
            ISiteSettings newSite)
        {
            if (siteRepo == null) { throw new ArgumentNullException("you must pass in an instance of ISiteRepository"); }
            if (newSite == null) { throw new ArgumentNullException("you must pass in an instance of ISiteSettings"); }
            if (newSite.SiteGuid != Guid.Empty) { throw new ArgumentException("newSite should not already have a site guid"); }

            //string templateFolderPath = GetMessageTemplateFolder();
            //string templateFolder = templateFolderPath;

            //SiteSettings newSite = new SiteSettings();


            newSite.Skin = config.DefaultInitialSkin();

            //newSite.Logo = GetMessageTemplate(templateFolder, "InitialSiteLogoContent.config");

            newSite.AllowHideMenuOnPages = false;
            newSite.AllowNewRegistration = true;
            newSite.AllowPageSkins = false;
            newSite.AllowUserFullNameChange = false;
            newSite.AllowUserSkins = false;
            newSite.AutoCreateLdapUserOnFirstLogin = true;
            //newSite.DefaultFriendlyUrlPattern = SiteSettings.FriendlyUrlPattern.PageNameWithDotASPX;
            //newSite.EditorSkin = SiteSettings.ContentEditorSkin.normal;
            //newSite.EncryptPasswords = false;
            newSite.Icon = String.Empty;
            newSite.ReallyDeleteUsers = true;
            newSite.SiteLdapSettings.Port = 389;
            newSite.SiteLdapSettings.RootDN = String.Empty;
            newSite.SiteLdapSettings.Server = String.Empty;
            newSite.UseEmailForLogin = true;
            newSite.UseLdapAuth = false;
            newSite.UseSecureRegistration = false;
            newSite.UseSslOnAllPages = config.SslIsRequiredByWebServer();
            //newSite.CreateInitialDataOnCreate = false;

            newSite.AllowPasswordReset = true;
            newSite.AllowPasswordRetrieval = true;
            //0 = clear, 1= hashed, 2= encrypted
            newSite.PasswordFormat = 1;

            newSite.RequiresQuestionAndAnswer = true;
            newSite.MaxInvalidPasswordAttempts = 10;
            newSite.PasswordAttemptWindowMinutes = 5;
            //newSite.RequiresUniqueEmail = true;
            newSite.MinRequiredNonAlphanumericCharacters = 0;
            newSite.MinRequiredPasswordLength = 7;
            newSite.PasswordStrengthRegularExpression = String.Empty;
            //newSite.DefaultEmailFromAddress = GetMessageTemplate(templateFolder, "InitialEmailFromContent.config");

            bool result = await siteRepo.Save(newSite);


            return result;


        }

        public async Task<bool> CreateRequiredRolesAndAdminUser(
            SiteSettings site,
            ConfigHelper config)
        {

            SiteRole adminRole = new SiteRole();
            adminRole.DisplayName = "Admins";
            //adminRole.DisplayName = "Administrators";
            adminRole.SiteId = site.SiteId;
            adminRole.SiteGuid = site.SiteGuid;
            bool result = await userRepo.SaveRole(adminRole);
            adminRole.DisplayName = "Administrators";
            result = await userRepo.SaveRole(adminRole);

            SiteRole roleAdminRole = new SiteRole();
            roleAdminRole.DisplayName = "Role Admins";
            roleAdminRole.SiteId = site.SiteId;
            roleAdminRole.SiteGuid = site.SiteGuid;
            result = await userRepo.SaveRole(roleAdminRole);

            roleAdminRole.DisplayName = "Role Administrators";
            result = await userRepo.SaveRole(roleAdminRole);

            SiteRole contentAdminRole = new SiteRole();
            contentAdminRole.DisplayName = "Content Administrators";
            contentAdminRole.SiteId = site.SiteId;
            contentAdminRole.SiteGuid = site.SiteGuid;
            result = await userRepo.SaveRole(contentAdminRole);

            SiteRole authenticatedUserRole = new SiteRole();
            authenticatedUserRole.DisplayName = "Authenticated Users";
            authenticatedUserRole.SiteId = site.SiteId;
            authenticatedUserRole.SiteGuid = site.SiteGuid;
            result = await userRepo.SaveRole(authenticatedUserRole);

            //SiteRole newsletterAdminRole = new SiteRole();
            //newsletterAdminRole.DisplayName = "Newsletter Administrators";
            //newsletterAdminRole.SiteId = site.SiteId;
            //newsletterAdminRole.SiteGuid = site.SiteGuid;
            //userRepository.SaveRole(newsletterAdminRole);

            // if using related sites mode there is a problem if we already have user admin@admin.com
            // and we create another one in the child site with the same email and login so we need to make it different
            // we could just skip creating this user since in related sites mode all users come from the first site
            // but then if the config were changed to not related sites mode there would be no admin user
            // so in related sites mode we create one only as a backup in case settings are changed later
            int countOfSites = await siteRepo.GetCount();
            string siteDifferentiator = string.Empty;
            if (
                (countOfSites >= 1)
                && (multiTenantOptions.UseRelatedSitesMode)
                )
            {
                if (site.SiteId > 1)
                {
                    siteDifferentiator = site.SiteId.ToInvariantString();
                }
            }

            //mojoMembershipProvider membership = Membership.Provider as mojoMembershipProvider;
            //bool overridRelatedSiteMode = true;
            SiteUser adminUser = new SiteUser();
            adminUser.SiteId = site.SiteId;
            adminUser.SiteGuid = site.SiteGuid;
            adminUser.Email = "admin" + siteDifferentiator + "@admin.com";
            adminUser.DisplayName = "Admin";
            adminUser.UserName = "admin" + siteDifferentiator;

            adminUser.EmailConfirmed = true;
            adminUser.ApprovedForLogin = true;
            adminUser.Password = "admin";
            adminUser.PasswordFormat = 0;

            //if (membership != null)
            //{
            //    adminUser.Password = membership.EncodePassword(site, adminUser, "admin");
            //}

            adminUser.PasswordQuestion = "What is your user name?";
            adminUser.PasswordAnswer = "admin";

            result = await userRepo.Save(adminUser);

            //siteUserManager.AddPassword(adminUser.UserGuid.ToString(), "admin");

            //siteUserManager.Create(adminUser, "admin");
            //var result = siteUserManager.CreateAsync(adminUser, "admin");
            //if (result.Succeeded)
            //{
            //}
            result = await userRepo.AddUserToRole(
                adminRole.RoleId,
                adminRole.RoleGuid,
                adminUser.UserId,
                adminUser.UserGuid);

            return result;

        }


        public async Task<SiteFolder> GetSiteFolder(string folderName)
        {
            return await siteRepo.GetSiteFolder(folderName);
        }

        public async Task<bool> EnsureSiteFolder(ISiteSettings site)
        {
            bool folderExists = await siteRepo.FolderExists(site.SiteFolderName);

            if (!folderExists)
            {
                List<SiteFolder> siteFolders = await siteRepo.GetSiteFoldersBySite(site.SiteGuid);
                //delete any existing folders before creating a new one
                foreach (SiteFolder f in siteFolders)
                {
                    bool deleted = await siteRepo.DeleteFolder(f.Guid);
                }

                //ensure the current folder mapping
                SiteFolder folder = new SiteFolder();
                folder.FolderName = site.SiteFolderName;
                folder.SiteGuid = site.SiteGuid;
                folderExists = await siteRepo.Save(folder);
            }

            return folderExists;
        }

        public async Task<ISiteHost> GetSiteHost(string hostName)
        {
            return await siteRepo.GetSiteHost(hostName);
        }

        public async Task<List<ISiteHost>> GetSiteHosts(int siteId)
        {
            return await siteRepo.GetSiteHosts(siteId);
        }

        public async Task<bool> AddHost(Guid siteGuid, int siteId, string hostName)
        {
            return await siteRepo.AddHost(siteGuid, siteId, hostName);
        }

        public async Task<bool> DeleteHost(int hostId)
        {
            return await siteRepo.DeleteHost(hostId);
        }

    }
}
