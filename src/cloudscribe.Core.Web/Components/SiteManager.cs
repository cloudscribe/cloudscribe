// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-22
// Last Modified:			2015-11-18
// 

using cloudscribe.Core.Models;
using Microsoft.Extensions.OptionsModel;
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
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<SetupOptions> setupOptionsAccessor)
        {
            resolver = siteResolver;
            siteRepo = siteRepository;
            userRepo = userRepository;
            multiTenantOptions = multiTenantOptionsAccessor.Value;
            setupOptions = setupOptionsAccessor.Value;
        }

        private MultiTenantOptions multiTenantOptions;
        private SetupOptions setupOptions;
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

        public async Task<ISiteSettings> Fetch(string hostname)
        {
            return await siteRepo.Fetch(hostname);
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
            bool isServerAdminSite)
        {
            //string templateFolderPath = GetMessageTemplateFolder();
            //string templateFolder = templateFolderPath;

            SiteSettings newSite = new SiteSettings();
            newSite.SiteName = "Sample Site";
            newSite.IsServerAdminSite = isServerAdminSite;

            bool result = await CreateNewSite(newSite);

            return newSite;


        }

        public async Task<bool> CreateNewSite(
            ISiteSettings newSite)
        {
            if (siteRepo == null) { throw new ArgumentNullException("you must pass in an instance of ISiteRepository"); }
            if (newSite == null) { throw new ArgumentNullException("you must pass in an instance of ISiteSettings"); }
            if (newSite.SiteGuid != Guid.Empty) { throw new ArgumentException("newSite should not already have a site guid"); }

            //string templateFolderPath = GetMessageTemplateFolder();
            //string templateFolder = templateFolderPath;

            //SiteSettings newSite = new SiteSettings();


            newSite.Layout = setupOptions.DefaultLayout;
            
            newSite.AllowNewRegistration = true;
            newSite.AllowUserFullNameChange = false;
            newSite.AutoCreateLdapUserOnFirstLogin = true;
            newSite.ReallyDeleteUsers = true;
            newSite.LdapPort = 389;
            newSite.LdapRootDN = String.Empty;
            newSite.LdapServer = String.Empty;
            newSite.UseEmailForLogin = true;
            newSite.UseLdapAuth = false;
            newSite.UseSecureRegistration = false;
            newSite.UseSslOnAllPages = setupOptions.SslIsRequiredByWebServer;
           
            
            //0 = clear, 1= hashed, 2= encrypted
            //newSite.PasswordFormat = 1;

            newSite.RequiresQuestionAndAnswer = false;
            newSite.MaxInvalidPasswordAttempts = 10;
            newSite.PasswordAttemptWindowMinutes = 5;
            newSite.MinReqNonAlphaChars = 0;
            newSite.MinRequiredPasswordLength = 7;
            
            
            bool result = await siteRepo.Save(newSite);


            return result;


        }

        public async Task<bool> CreateRequiredRolesAndAdminUser(
            SiteSettings site)
        {



            //SiteRole adminRole = new SiteRole();
            //adminRole.DisplayName = "Admins";
            ////adminRole.DisplayName = "Administrators";
            //adminRole.SiteId = site.SiteId;
            //adminRole.SiteGuid = site.SiteGuid;
            //bool result = await userRepo.SaveRole(adminRole);
            //adminRole.DisplayName = "Administrators";
            //result = await userRepo.SaveRole(adminRole);

            //SiteRole roleAdminRole = new SiteRole();
            //roleAdminRole.DisplayName = "Role Admins";
            //roleAdminRole.SiteId = site.SiteId;
            //roleAdminRole.SiteGuid = site.SiteGuid;
            //result = await userRepo.SaveRole(roleAdminRole);

            //roleAdminRole.DisplayName = "Role Administrators";
            //result = await userRepo.SaveRole(roleAdminRole);

            //SiteRole contentAdminRole = new SiteRole();
            //contentAdminRole.DisplayName = "Content Administrators";
            //contentAdminRole.SiteId = site.SiteId;
            //contentAdminRole.SiteGuid = site.SiteGuid;
            //result = await userRepo.SaveRole(contentAdminRole);

            //SiteRole authenticatedUserRole = new SiteRole();
            //authenticatedUserRole.DisplayName = "Authenticated Users";
            //authenticatedUserRole.SiteId = site.SiteId;
            //authenticatedUserRole.SiteGuid = site.SiteGuid;
            //result = await userRepo.SaveRole(authenticatedUserRole);

            //// if using related sites mode there is a problem if we already have user admin@admin.com
            //// and we create another one in the child site with the same email and login so we need to make it different
            //// we could just skip creating this user since in related sites mode all users come from the first site
            //// but then if the config were changed to not related sites mode there would be no admin user
            //// so in related sites mode we create one only as a backup in case settings are changed later
            //int countOfSites = await siteRepo.GetCount();
            //string siteDifferentiator = string.Empty;
            //if (
            //    (countOfSites >= 1)
            //    && (multiTenantOptions.UseRelatedSitesMode)
            //    )
            //{
            //    if (site.SiteId > 1)
            //    {
            //        siteDifferentiator = site.SiteId.ToInvariantString();
            //    }
            //}


            //SiteUser adminUser = new SiteUser();
            //adminUser.SiteId = site.SiteId;
            //adminUser.SiteGuid = site.SiteGuid;
            //adminUser.Email = "admin" + siteDifferentiator + "@admin.com";
            //adminUser.DisplayName = "Admin";
            //adminUser.UserName = "admin" + siteDifferentiator;

            //adminUser.EmailConfirmed = true;
            //adminUser.AccountApproved = true;

            //// clear text password will be hashed upon login
            //// this format allows migrating from mojoportal
            //adminUser.PasswordHash = "admin||0"; //pwd/salt/format 


            //result = await userRepo.Save(adminUser);




            //result = await userRepo.AddUserToRole(
            //    adminRole.RoleId,
            //    adminRole.RoleGuid,
            //    adminUser.UserId,
            //    adminUser.UserGuid);

            bool result = await EnsureRequiredRoles(site);
            result = await CreateAdminUser(site);

            return result;

        }

        public async Task<bool> CreateAdminUser(ISiteSettings site)
        {

            ISiteRole adminRole = await userRepo.FetchRole(site.SiteId, "Admins");

            if(adminRole == null)
            {
                throw new InvalidOperationException("Admins role could nto be found so cannot create admin user");
            }
            

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


            SiteUser adminUser = new SiteUser();
            adminUser.SiteId = site.SiteId;
            adminUser.SiteGuid = site.SiteGuid;
            adminUser.Email = "admin" + siteDifferentiator + "@admin.com";
            adminUser.DisplayName = "Admin";
            adminUser.UserName = "admin" + siteDifferentiator;

            adminUser.EmailConfirmed = true;
            adminUser.AccountApproved = true;

            // clear text password will be hashed upon login
            // this format allows migrating from mojoportal
            adminUser.PasswordHash = "admin||0"; //pwd/salt/format 


            bool result = await userRepo.Save(adminUser);
            
            result = await userRepo.AddUserToRole(
                adminRole.RoleId,
                adminRole.RoleGuid,
                adminUser.UserId,
                adminUser.UserGuid);

            return result;

        }

        public async Task<bool> EnsureRequiredRoles(ISiteSettings site)
        {
            bool result = true;

            bool exists = await userRepo.RoleExists(site.SiteId, "Admins");

            if(!exists)
            {
                SiteRole adminRole = new SiteRole();
                adminRole.DisplayName = "Admins";
                //adminRole.DisplayName = "Administrators";
                adminRole.SiteId = site.SiteId;
                adminRole.SiteGuid = site.SiteGuid;
                result = await userRepo.SaveRole(adminRole);
                adminRole.DisplayName = "Administrators";
                result = await userRepo.SaveRole(adminRole);
            }

            exists = await userRepo.RoleExists(site.SiteId, "Role Admins");

            if (!exists)
            {
                SiteRole roleAdminRole = new SiteRole();
                roleAdminRole.DisplayName = "Role Admins";
                roleAdminRole.SiteId = site.SiteId;
                roleAdminRole.SiteGuid = site.SiteGuid;
                result = await userRepo.SaveRole(roleAdminRole);

                roleAdminRole.DisplayName = "Role Administrators";
                result = await userRepo.SaveRole(roleAdminRole);
            }

            exists = await userRepo.RoleExists(site.SiteId, "Content Administrators");

            if (!exists)
            {
                SiteRole contentAdminRole = new SiteRole();
                contentAdminRole.DisplayName = "Content Administrators";
                contentAdminRole.SiteId = site.SiteId;
                contentAdminRole.SiteGuid = site.SiteGuid;
                result = await userRepo.SaveRole(contentAdminRole);
            }

            exists = await userRepo.RoleExists(site.SiteId, "Authenticated Users");

            if (!exists)
            {
                SiteRole authenticatedUserRole = new SiteRole();
                authenticatedUserRole.DisplayName = "Authenticated Users";
                authenticatedUserRole.SiteId = site.SiteId;
                authenticatedUserRole.SiteGuid = site.SiteGuid;
                result = await userRepo.SaveRole(authenticatedUserRole);
            }

            

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

        public async Task<int> GetUserCount(int siteId)
        {
            // this is only used on setup controller
            // to make sure admin user was created
            return await userRepo.CountUsers(siteId, string.Empty);
        }

        public async Task<int> GetRoleCount(int siteId)
        {
            // this is only used on setup controller
            // to make sure admin user and role was created
            return await userRepo.CountOfRoles(siteId, string.Empty);
        }

    }
}
