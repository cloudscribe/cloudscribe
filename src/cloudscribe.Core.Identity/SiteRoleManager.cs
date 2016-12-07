// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-17
// Last Modified:		    2016-12-07
// 
//

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            SiteContext currentSite,
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
        private ISiteContext siteSettings = null;
        private ISiteContext Site
        {
            get
            {
                //if (siteSettings == null) { siteSettings = siteResolver.Resolve(); }
                return siteSettings;
            }
        }

        public async Task<int> CountOfRoles(Guid siteId, string searchInput)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await queries.CountOfRoles(siteId, searchInput, CancellationToken);
        }

        public async Task<IList<ISiteRole>> GetRolesBySite(
            Guid siteId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await queries.GetRolesBySite(siteId, searchInput, pageNumber, pageSize, CancellationToken);

        }

        
        public async Task DeleteUserRolesByRole(Guid roleId)
        {
            var siteId = Site.Id;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            await commands.DeleteUserRolesByRole(siteId, roleId, CancellationToken);
        }

        public async Task DeleteRole(Guid roleId)
        {
            var siteId = Site.Id;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            await commands.DeleteRole(siteId, roleId, CancellationToken);
        }

        //public async Task<bool> RoleExists(Guid siteId, string roleName)
        //{
        //    if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteGuid; }

        //    return await queries.RoleExists(siteId, roleName, CancellationToken);
        //}

        public async Task<IList<IUserInfo>> GetUsersInRole(
            Guid siteId, 
            Guid roleId, 
            string searchInput, 
            int pageNumber, 
            int pageSize)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await queries.GetUsersInRole(siteId, roleId, searchInput, pageNumber, pageSize, CancellationToken);
        }

        public async Task<int> CountUsersInRole(
            Guid siteId, 
            Guid roleId, 
            string searchInput)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await queries.CountUsersInRole(siteId, roleId, searchInput, CancellationToken);
        }

        public async Task<IList<IUserInfo>> GetUsersNotInRole(
            Guid siteId, 
            Guid roleId, 
            string searchInput, 
            int pageNumber, 
            int pageSize)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await queries.GetUsersNotInRole(siteId, roleId, searchInput, pageNumber, pageSize, CancellationToken);
        }

        public async Task<int> CountUsersNotInRole(
            Guid siteId, 
            Guid roleId, 
            string searchInput)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await queries.CountUsersNotInRole(siteId, roleId, searchInput, CancellationToken);
        }

        public async Task AddUserToRole(ISiteUser user, ISiteRole role)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (role == null) { throw new ArgumentNullException(nameof(role)); }
            if(role.SiteId != user.SiteId) { throw new ArgumentException("user and role must have the same siteid"); }

            await commands.AddUserToRole(role.SiteId, role.Id, user.Id, CancellationToken);
            
            user.RolesChanged = true;
            await commands.Update(user, CancellationToken);
            
            
        }

        public async Task RemoveUserFromRole(ISiteUser user, ISiteRole role)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (role == null) { throw new ArgumentNullException(nameof(role)); }

            await commands.RemoveUserFromRole(role.SiteId, role.Id, user.Id, CancellationToken);
            
            user.RolesChanged = true;
            await commands.Update(user, CancellationToken);
           
        }

    }
}
