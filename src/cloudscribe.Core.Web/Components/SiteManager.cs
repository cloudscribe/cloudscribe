// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-22
// Last Modified:			2016-05-13
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
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
            ISiteCommands siteCommands,
            ISiteQueries siteQueries,
            IUserCommands userCommands,
            IUserQueries userQueries,
            SiteDataProtector dataProtector,
            IHttpContextAccessor contextAccessor,
            ILogger<SiteManager> logger,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<SiteConfigOptions> setupOptionsAccessor
            )
        {

            commands = siteCommands;
            queries = siteQueries;
            this.userCommands = userCommands;
            this.userQueries = userQueries;
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
        private ISiteCommands commands;
        private ISiteQueries queries;
        private IUserQueries userQueries;
        private IUserCommands userCommands;
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
            Guid currentSiteId,
            int pageNumber,
            int pageSize)
        {
            return queries.GetPageOtherSites(currentSiteId, pageNumber, pageSize, CancellationToken);

        }

        public Task<int> CountOtherSites(Guid currentSiteId)
        {
            return queries.CountOtherSites(currentSiteId, CancellationToken);
        }

        //public int GetSiteIdByFolderNonAsync(string folderName)
        //{
        //    return siteRepo.GetSiteIdByFolderNonAsync(folderName);
        //}

        public async Task<ISiteSettings> Fetch(Guid siteId)
        {
            var site = await queries.Fetch(siteId, CancellationToken);
            dataProtector.UnProtect(site);
            return site;
        }

        //public ISiteSettings FetchNonAsync(Guid siteGuid)
        //{
        //    var  site = queries.FetchNonAsync(siteGuid);
        //    dataProtector.UnProtect(site);
        //    return site;
        //}

        //public ISiteSettings FetchNonAsync(string host)
        //{
        //    var site = queries.FetchNonAsync(host);
        //    dataProtector.UnProtect(site);
        //    return site;
        //}

        //public async Task<ISiteSettings> Fetch(Guid siteGuid)
        //{
        //    ISiteSettings site = await siteRepo.Fetch(siteGuid, CancellationToken);
        //    dataProtector.UnProtect(site);
        //    return site;
        //}

        public async Task<ISiteSettings> Fetch(string hostname)
        {
            var site = await queries.Fetch(hostname, CancellationToken);
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
            var matchingSite = await queries.FetchByFolderName(requestedFolderName, CancellationToken);
            if(matchingSite == null) { return true; }
            if(matchingSite.SiteFolderName != requestedFolderName) { return true; }
            if(matchingSite.Id == requestingSite.Id) { return true; }

            return false;

        }

        public async Task<bool> AliasIdIsAvailable(Guid requestingSiteId, string requestedAliasId)
        {
            if (string.IsNullOrWhiteSpace(requestedAliasId)) return false;
            if (requestedAliasId.Length > 36) return false;
            var list = await queries.GetList(CancellationToken).ConfigureAwait(false);
            
            foreach(var s in list)
            {
                if(s.AliasId == requestedAliasId)
                {
                    if ((requestingSiteId == s.Id)) return true;

                    return false;

                }
            }

            return true;

        }

        public async Task Save(ISiteSettings site)
        {
            dataProtector.Protect(site);
            if(site.Id == Guid.Empty)
            {
                site.Id = Guid.NewGuid();
                await commands.Update(site, CancellationToken.None);
            }
            else
            {
                await commands.Update(site, CancellationToken.None);
            }
            
        }

        public async Task Delete(ISiteSettings site)
        {
            // we will need a provider model or something similar here to
            // allow other features and 3rd party features to delete
            // related data when a site is deleted
            // TODO: implement
            // will ProviderModel be available in Core Framework or will we have to use something else
            // a way to use dependency injection?

            // delete users
            await userCommands.DeleteUsersBySite(site.Id, CancellationToken.None); // this also deletes userroles claims logins

            await userCommands.DeleteRolesBySite(site.Id, CancellationToken.None);
            await commands.DeleteHostsBySite(site.Id, CancellationToken.None);
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

            await commands.Delete(site.Id, CancellationToken.None);
        }

        public async Task<SiteSettings> CreateNewSite(bool isServerAdminSite)
        {
            //string templateFolderPath = GetMessageTemplateFolder();
            //string templateFolder = templateFolderPath;

            var newSite = new SiteSettings();
            newSite.Id = Guid.NewGuid();
            newSite.SiteName = "Sample Site";
            newSite.IsServerAdminSite = isServerAdminSite;
            var siteNumber = 1 + await queries.CountOtherSites(Guid.Empty);
            newSite.AliasId = $"tenant-{siteNumber}";

            await CreateNewSite(newSite);

            return newSite;


        }

        public async Task CreateNewSite(ISiteSettings newSite)
        {
            
            if (newSite == null) { throw new ArgumentNullException("you must pass in an instance of ISiteSettings"); }
            //if (newSite.SiteGuid != Guid.Empty) { throw new ArgumentException("newSite should not already have a site guid"); }

            //string templateFolderPath = GetMessageTemplateFolder();
            //string templateFolder = templateFolderPath;

            //SiteSettings newSite = new SiteSettings();
            
            newSite.Theme = setupOptions.DefaultLayout;
            // TODO: more configurable options?
            
            
            await commands.Create(newSite, CancellationToken.None);


            

        }

        public async Task CreateRequiredRolesAndAdminUser(SiteSettings site)
        {
            await EnsureRequiredRoles(site);
            await CreateAdminUser(site);
            
        }

        public async Task CreateAdminUser(ISiteSettings site)
        {

            var adminRole = await userQueries.FetchRole(site.Id, "Administrators", CancellationToken);

            if(adminRole == null)
            {
                throw new InvalidOperationException("Administrators role could nto be found so cannot create admin user");
            }
            

            // if using related sites mode there is a problem if we already have user admin@admin.com
            // and we create another one in the child site with the same email and login so we need to make it different
            // we could just skip creating this user since in related sites mode all users come from the first site
            // but then if the config were changed to not related sites mode there would be no admin user
            // so in related sites mode we create one only as a backup in case settings are changed later
            var countOfSites = await queries.GetCount(CancellationToken);
            var siteDifferentiator = string.Empty;
            if (
                (countOfSites >= 1)
                && (multiTenantOptions.UseRelatedSitesMode)
                )
            {
                //TODO: do we need to replace this logic now that we don't have an integer id?
                //if (site.SiteId > 1)
                //{
                //    siteDifferentiator = site.SiteId.ToInvariantString();
                //}
            }


            var adminUser = InitialData.BuildInitialAdmin();
            adminUser.SiteId = site.Id;
            adminUser.Email = "admin" + siteDifferentiator + "@admin.com";
            adminUser.NormalizedEmail = adminUser.Email.ToUpperInvariant();
            adminUser.UserName = "admin" + siteDifferentiator;
            
            await userCommands.Create(adminUser, CancellationToken.None);
            
            await userCommands.AddUserToRole(
                adminRole.Id,
                adminUser.Id,
                CancellationToken.None);

            
        }

        public async Task EnsureRequiredRoles(ISiteSettings site)
        {
            bool exists = await userQueries.RoleExists(site.Id, "Administrators", CancellationToken);

            if(!exists)
            {
                var adminRole = InitialData.BuildAdminRole();
                adminRole.SiteId = site.Id;
                await userCommands.CreateRole(adminRole, CancellationToken.None);
            }

            exists = await userQueries.RoleExists(site.Id, "Role Administrators", CancellationToken);

            if (!exists)
            {
                var roleAdminRole = InitialData.BuildRoleAdminRole();
                roleAdminRole.SiteId = site.Id;
                await userCommands.CreateRole(roleAdminRole, CancellationToken.None);
                
            }

            exists = await userQueries.RoleExists(site.Id, "Content Administrators", CancellationToken);

            if (!exists)
            {
                var contentAdminRole = InitialData.BuildContentAdminsRole();
                contentAdminRole.SiteId = site.Id;
                await userCommands.CreateRole(contentAdminRole, CancellationToken.None);
            }

            exists = await userQueries.RoleExists(site.Id, "Authenticated Users", CancellationToken);

            if (!exists)
            {
                var authenticatedUserRole = InitialData.BuildAuthenticatedRole();
                authenticatedUserRole.SiteId = site.Id;
                await userCommands.CreateRole(authenticatedUserRole, CancellationToken.None);
            }
            

        }

        

        public Task<ISiteHost> GetSiteHost(string hostName)
        {
            return queries.GetSiteHost(hostName, CancellationToken);
        }

        public Task<List<ISiteHost>> GetSiteHosts(Guid siteId)
        {
            return queries.GetSiteHosts(siteId, CancellationToken);
        }

        public async Task AddHost(Guid siteId, string hostName)
        {
            await commands.AddHost(siteId, hostName, CancellationToken);
        }

        public async Task DeleteHost(Guid hostId)
        {
            await commands.DeleteHost(hostId, CancellationToken);
        }

        public Task<int> GetUserCount(Guid siteId)
        {
            // this is only used on setup controller
            // to make sure admin user was created
            return userQueries.CountUsers(siteId, string.Empty, CancellationToken);
        }

        public Task<int> GetRoleCount(Guid siteId)
        {
            // this is only used on setup controller
            // to make sure admin user and role was created
            return userQueries.CountOfRoles(siteId, string.Empty, CancellationToken);
        }

        public async Task<int> ExistingSiteCount()
        {
            try
            {
                return await queries.GetCount(CancellationToken);
            }
            catch { }
            // errors are expected here before the db is initialized
            // so just return 0 if error here
            return 0;
            
        }

    }
}
