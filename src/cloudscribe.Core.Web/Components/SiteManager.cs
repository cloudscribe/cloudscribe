// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-22
// Last Modified:			2016-02-04
// 

using cloudscribe.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.DataProtection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteManager
    {

        public SiteManager(
            SiteSettings currentSite,
            ISiteRepository siteRepository,
            IUserRepository userRepository,
            SiteDataProtector dataProtector,
            IHttpContextAccessor contextAccessor,
            ILogger<SiteManager> logger,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<SiteConfigOptions> setupOptionsAccessor
            )
        {
            
            siteRepo = siteRepository;
            userRepo = userRepository;
            multiTenantOptions = multiTenantOptionsAccessor.Value;
            setupOptions = setupOptionsAccessor.Value;
            _context = contextAccessor?.HttpContext;
            this.dataProtector = dataProtector;
            log = logger;

            //resolver = siteResolver;
            siteSettings = currentSite;
        }

        private readonly HttpContext _context;
        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;
        private ILogger log;
        private SiteDataProtector dataProtector;
        
        private MultiTenantOptions multiTenantOptions;
        private SiteConfigOptions setupOptions;
        private ISiteRepository siteRepo;
        private IUserRepository userRepo;
        private ISiteSettings siteSettings = null;
        private ISiteSettings Site
        {
            get
            {
                //if (siteSettings == null) { siteSettings = resolver.Resolve(); }
                return siteSettings;
            }
        }

        public ISiteSettings CurrentSite
        {
            get { return Site; }
        }
        
           

        public Task<List<ISiteInfo>> GetPageOtherSites(
            int currentSiteId,
            int pageNumber,
            int pageSize)
        {
            return siteRepo.GetPageOtherSites(currentSiteId, pageNumber, pageSize, CancellationToken);

        }

        public Task<int> CountOtherSites(int currentSiteId)
        {
            return siteRepo.CountOtherSites(currentSiteId, CancellationToken);
        }

        //public int GetSiteIdByFolderNonAsync(string folderName)
        //{
        //    return siteRepo.GetSiteIdByFolderNonAsync(folderName);
        //}

        public async Task<ISiteSettings> Fetch(Guid siteGuid)
        {
            ISiteSettings site = await siteRepo.Fetch(siteGuid, CancellationToken);
            dataProtector.UnProtect(site);
            return site;
        }

        public ISiteSettings FetchNonAsync(int siteId)
        {
            ISiteSettings  site = siteRepo.FetchNonAsync(siteId);
            dataProtector.UnProtect(site);
            return site;
        }

        public ISiteSettings FetchNonAsync(string host)
        {
            ISiteSettings site = siteRepo.FetchNonAsync(host);
            dataProtector.UnProtect(site);
            return site;
        }

        public async Task<ISiteSettings> Fetch(int siteId)
        {
            ISiteSettings site = await siteRepo.Fetch(siteId, CancellationToken);
            dataProtector.UnProtect(site);
            return site;
        }

        public async Task<ISiteSettings> Fetch(string hostname)
        {
            ISiteSettings site = await siteRepo.Fetch(hostname, CancellationToken);
            dataProtector.UnProtect(site);
            return site;
        }

        /// <summary>
        /// returns true if the folder is not in use or is in use only on the passed in ISiteSettings
        /// </summary>
        /// <param name=""></param>
        /// <param name="requestedFolderName"></param>
        /// <returns></returns>
        public async Task<bool> FolderNameIsAvailable(ISiteSettings requestingSite, string requestedFolderName)
        {
            var matchingSite = await siteRepo.FetchByFolderName(requestedFolderName, CancellationToken);
            if(matchingSite == null) { return true; }
            if(matchingSite.SiteFolderName != requestedFolderName) { return true; }
            if(matchingSite.SiteGuid == requestingSite.SiteGuid) { return true; }

            return false;

        }

        public async Task<bool> Save(ISiteSettings site)
        {
            dataProtector.Protect(site);
            return await siteRepo.Save(site, CancellationToken.None);
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
            bool resultStep = await userRepo.DeleteUsersBySite(site.SiteId, CancellationToken.None); // this also deletes userroles claims logins

            resultStep = await userRepo.DeleteRolesBySite(site.SiteId, CancellationToken.None);
            resultStep = await siteRepo.DeleteHostsBySite(site.SiteId, CancellationToken.None);
            //resultStep = await siteRepo.DeleteFoldersBySite(site.SiteGuid, CancellationToken.None);


            // the below method deletes a lot of things by siteid including the following tables
            // Exec mp_Sites_Delete
            // mp_UserRoles
            // mp_UserProperties
            // mp_UserLocation
            // mp_Users
            // mp_Roles
            // mp_SiteHosts
            // mp_SiteFolders
            // mp_SiteSettingsEx
            // mp_Sites

            return await siteRepo.Delete(site.SiteId, CancellationToken.None);
        }

        public async Task<SiteSettings> CreateNewSite(bool isServerAdminSite)
        {
            //string templateFolderPath = GetMessageTemplateFolder();
            //string templateFolder = templateFolderPath;

            SiteSettings newSite = new SiteSettings();
            newSite.SiteName = "Sample Site";
            newSite.IsServerAdminSite = isServerAdminSite;

            bool result = await CreateNewSite(newSite);

            return newSite;


        }

        public async Task<bool> CreateNewSite(ISiteSettings newSite)
        {
            if (siteRepo == null) { throw new ArgumentNullException("you must pass in an instance of ISiteRepository"); }
            if (newSite == null) { throw new ArgumentNullException("you must pass in an instance of ISiteSettings"); }
            if (newSite.SiteGuid != Guid.Empty) { throw new ArgumentException("newSite should not already have a site guid"); }

            //string templateFolderPath = GetMessageTemplateFolder();
            //string templateFolder = templateFolderPath;

            //SiteSettings newSite = new SiteSettings();
            
            newSite.Layout = setupOptions.DefaultLayout;
            // TODO: more configurable options?
            
            
            bool result = await siteRepo.Save(newSite, CancellationToken.None);


            return result;

        }

        public async Task<bool> CreateRequiredRolesAndAdminUser(SiteSettings site)
        {
            bool result = await EnsureRequiredRoles(site);
            result = await CreateAdminUser(site);

            return result;

        }

        public async Task<bool> CreateAdminUser(ISiteSettings site)
        {

            ISiteRole adminRole = await userRepo.FetchRole(site.SiteId, "Admins", CancellationToken);

            if(adminRole == null)
            {
                throw new InvalidOperationException("Admins role could nto be found so cannot create admin user");
            }
            

            // if using related sites mode there is a problem if we already have user admin@admin.com
            // and we create another one in the child site with the same email and login so we need to make it different
            // we could just skip creating this user since in related sites mode all users come from the first site
            // but then if the config were changed to not related sites mode there would be no admin user
            // so in related sites mode we create one only as a backup in case settings are changed later
            int countOfSites = await siteRepo.GetCount(CancellationToken);
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
            adminUser.NormalizedEmail = adminUser.Email;
            adminUser.DisplayName = "Admin";
            adminUser.UserName = "admin" + siteDifferentiator;

            adminUser.EmailConfirmed = true;
            adminUser.AccountApproved = true;

            // clear text password will be hashed upon login
            // this format allows migrating from mojoportal
            adminUser.PasswordHash = "admin||0"; //pwd/salt/format 


            bool result = await userRepo.Save(adminUser, CancellationToken.None);
            
            result = await userRepo.AddUserToRole(
                adminRole.RoleId,
                adminRole.RoleGuid,
                adminUser.UserId,
                adminUser.UserGuid,
                CancellationToken.None);

            return result;

        }

        public async Task<bool> EnsureRequiredRoles(ISiteSettings site)
        {
            bool result = true;

            bool exists = await userRepo.RoleExists(site.SiteId, "Admins", CancellationToken);

            if(!exists)
            {
                SiteRole adminRole = new SiteRole();
                adminRole.DisplayName = "Admins";
                //adminRole.DisplayName = "Administrators";
                adminRole.SiteId = site.SiteId;
                adminRole.SiteGuid = site.SiteGuid;
                result = await userRepo.SaveRole(adminRole, CancellationToken.None);
                adminRole.DisplayName = "Administrators";
                result = await userRepo.SaveRole(adminRole, CancellationToken.None);
            }

            exists = await userRepo.RoleExists(site.SiteId, "Role Admins", CancellationToken);

            if (!exists)
            {
                SiteRole roleAdminRole = new SiteRole();
                roleAdminRole.DisplayName = "Role Admins";
                roleAdminRole.SiteId = site.SiteId;
                roleAdminRole.SiteGuid = site.SiteGuid;
                result = await userRepo.SaveRole(roleAdminRole, CancellationToken.None);

                roleAdminRole.DisplayName = "Role Administrators";
                result = await userRepo.SaveRole(roleAdminRole, CancellationToken.None);
            }

            exists = await userRepo.RoleExists(site.SiteId, "Content Administrators", CancellationToken);

            if (!exists)
            {
                SiteRole contentAdminRole = new SiteRole();
                contentAdminRole.DisplayName = "Content Administrators";
                contentAdminRole.SiteId = site.SiteId;
                contentAdminRole.SiteGuid = site.SiteGuid;
                result = await userRepo.SaveRole(contentAdminRole, CancellationToken.None);
            }

            exists = await userRepo.RoleExists(site.SiteId, "Authenticated Users", CancellationToken);

            if (!exists)
            {
                SiteRole authenticatedUserRole = new SiteRole();
                authenticatedUserRole.DisplayName = "Authenticated Users";
                authenticatedUserRole.SiteId = site.SiteId;
                authenticatedUserRole.SiteGuid = site.SiteGuid;
                result = await userRepo.SaveRole(authenticatedUserRole, CancellationToken.None);
            }

            

            return result;

        }




        //public async Task<ISiteFolder> GetSiteFolder(string folderName)
        //{
        //    return await siteRepo.GetSiteFolder(folderName, CancellationToken);
        //}

        //public async Task<bool> EnsureSiteFolder(ISiteSettings site)
        //{
        //    bool folderExists = await siteRepo.FolderExists(site.SiteFolderName, CancellationToken);

        //    if (!folderExists)
        //    {
        //        List<ISiteFolder> siteFolders = await siteRepo.GetSiteFoldersBySite(site.SiteGuid, CancellationToken);
        //        //delete any existing folders before creating a new one
        //        foreach (ISiteFolder f in siteFolders)
        //        {
        //            bool deleted = await siteRepo.DeleteFolder(f.Guid, CancellationToken);
        //        }

        //        //ensure the current folder mapping
        //        SiteFolder folder = new SiteFolder();
        //        folder.FolderName = site.SiteFolderName;
        //        folder.SiteGuid = site.SiteGuid;
        //        folderExists = await siteRepo.Save(folder, CancellationToken);
        //    }

        //    return folderExists;
        //}

        public Task<ISiteHost> GetSiteHost(string hostName)
        {
            return siteRepo.GetSiteHost(hostName, CancellationToken);
        }

        public Task<List<ISiteHost>> GetSiteHosts(int siteId)
        {
            return siteRepo.GetSiteHosts(siteId, CancellationToken);
        }

        public Task<bool> AddHost(Guid siteGuid, int siteId, string hostName)
        {
            return siteRepo.AddHost(siteGuid, siteId, hostName, CancellationToken);
        }

        public Task<bool> DeleteHost(int hostId)
        {
            return siteRepo.DeleteHost(hostId, CancellationToken);
        }

        public Task<int> GetUserCount(int siteId)
        {
            // this is only used on setup controller
            // to make sure admin user was created
            return userRepo.CountUsers(siteId, string.Empty, CancellationToken);
        }

        public Task<int> GetRoleCount(int siteId)
        {
            // this is only used on setup controller
            // to make sure admin user and role was created
            return userRepo.CountOfRoles(siteId, string.Empty, CancellationToken);
        }

        public Task<int> ExistingSiteCount()
        {
            try
            {
                return siteRepo.GetCount(CancellationToken);
            }
            catch { }
            // errors are expected here before the db is initialized
            // so just return 0 if error here
            return Task.FromResult(0);
            
        }

    }
}
