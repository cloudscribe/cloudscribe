// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-17
// Last Modified:		    2016-05-10
// 
//

using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    /// <summary>
    /// extends the standard RoleManager with our custom methods
    /// </summary>
    /// <typeparam name="TRole"></typeparam>
    public class SiteRoleManager<TRole> : RoleManager<TRole> where TRole : SiteRole
    {
        public SiteRoleManager(
            SiteSettings currentSite,
            IUserCommands userCommands,
            IUserQueries userQueries,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IRoleStore<TRole> roleStore,
            IEnumerable<IRoleValidator<TRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<TRole>> logger,
            IHttpContextAccessor contextAccessor
            ) : base(
                roleStore, 
                roleValidators,
                keyNormalizer, 
                errors, 
                logger, 
                contextAccessor)
        {
            if (currentSite == null) { throw new ArgumentNullException(nameof(currentSite)); }
            if (roleStore == null) { throw new ArgumentNullException(nameof(roleStore)); }
            if (userCommands == null) { throw new ArgumentNullException(nameof(userCommands)); }
            commands = userCommands;

            if (userQueries == null) { throw new ArgumentNullException(nameof(userQueries)); }
            queries = userQueries;

            siteSettings = currentSite;
            this.logger = logger;
            multiTenantOptions = multiTenantOptionsAccessor.Value;
            _context = contextAccessor?.HttpContext;

        }

        private readonly HttpContext _context;
        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        private MultiTenantOptions multiTenantOptions;
        private IUserCommands commands;
        private IUserQueries queries;
        private ILogger logger;
        private ISiteSettings siteSettings = null;
        private ISiteSettings Site
        {
            get
            {
                //if (siteSettings == null) { siteSettings = siteResolver.Resolve(); }
                return siteSettings;
            }
        }

        public async Task<int> CountOfRoles(Guid siteGuid, string searchInput)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteGuid = multiTenantOptions.RelatedSiteGuid; }

            return await queries.CountOfRoles(siteGuid, searchInput, CancellationToken);
        }

        public async Task<IList<ISiteRole>> GetRolesBySite(
            Guid siteGuid,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteGuid = multiTenantOptions.RelatedSiteGuid; }

            return await queries.GetRolesBySite(siteGuid, searchInput, pageNumber, pageSize, CancellationToken);

        }

        /// <summary>
        /// TODO: this replicates RoleStore.FindByIdAsync except this takes int as it should
        /// I think there is a way to fix FindByIdAsync so it takes int then get rid of this one
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<ISiteRole> FetchRole(Guid roleGuid)
        {
            return await queries.FetchRole(roleGuid, CancellationToken);
        }

        // again need to consolidate with RoleStore.UpdateAsync
        public async Task SaveRole(ISiteRole role)
        {
            if(role.Id == Guid.Empty)
            {
                // TODO: consider if this is the correct place to generate the id
                role.Id = Guid.NewGuid();
                await commands.CreateRole(role, CancellationToken);
            }
            else
            {
                await commands.UpdateRole(role, CancellationToken);
            }
            
        }

        public async Task DeleteUserRolesByRole(Guid roleGuid)
        {
            await commands.DeleteUserRolesByRole(roleGuid, CancellationToken);
        }

        public async Task DeleteRole(Guid roleGuid)
        {
            await commands.DeleteRole(roleGuid, CancellationToken);
        }

        public async Task<bool> RoleExists(Guid siteGuid, string roleName)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteGuid = multiTenantOptions.RelatedSiteGuid; }

            return await queries.RoleExists(siteGuid, roleName, CancellationToken);
        }

        public async Task<IList<IUserInfo>> GetUsersInRole(Guid siteGuid, Guid roleGuid, string searchInput, int pageNumber, int pageSize)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteGuid = multiTenantOptions.RelatedSiteGuid; }

            return await queries.GetUsersInRole(siteGuid, roleGuid, searchInput, pageNumber, pageSize, CancellationToken);
        }

        public async Task<int> CountUsersInRole(Guid siteGuid, Guid roleGuid, string searchInput)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteGuid = multiTenantOptions.RelatedSiteGuid; }

            return await queries.CountUsersInRole(siteGuid, roleGuid, searchInput, CancellationToken);
        }

        public async Task<IList<IUserInfo>> GetUsersNotInRole(Guid siteGuid, Guid roleGuid, string searchInput, int pageNumber, int pageSize)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteGuid = multiTenantOptions.RelatedSiteGuid; }

            return await queries.GetUsersNotInRole(siteGuid, roleGuid, searchInput, pageNumber, pageSize, CancellationToken);
        }

        public async Task<int> CountUsersNotInRole(Guid siteGuid, Guid roleGuid, string searchInput)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteGuid = multiTenantOptions.RelatedSiteGuid; }

            return await queries.CountUsersNotInRole(siteGuid, roleGuid, searchInput, CancellationToken);
        }

        public async Task AddUserToRole(ISiteUser user, ISiteRole role)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (role == null) { throw new ArgumentNullException(nameof(role)); }
            if(role.SiteId != user.SiteId) { throw new ArgumentException("user and role must have the same siteid"); }

            await commands.AddUserToRole(role.Id, user.Id, CancellationToken);
            
            user.RolesChanged = true;
            await commands.Update(user, CancellationToken);
            
            
        }

        public async Task RemoveUserFromRole(ISiteUser user, ISiteRole role)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (role == null) { throw new ArgumentNullException(nameof(role)); }

            await commands.RemoveUserFromRole(role.Id, user.Id, CancellationToken);
            
            user.RolesChanged = true;
            await commands.Update(user, CancellationToken);
           
        }

    }
}
