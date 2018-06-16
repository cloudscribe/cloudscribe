// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-17
// Last Modified:		    2018-06-16
// 
//

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.EventHandlers;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
            IHttpContextAccessor contextAccessor,
            IEnumerable<IHandleUserAddedToRole> userAddedToRoleHandlers,
            IEnumerable<IHandleUserRemovedFromRole> userRemovedFromRoleHandlers
            ) : base(
                roleStore, 
                roleValidators,
                keyNormalizer, 
                errors, 
                logger)
        {
            if (roleStore == null) { throw new ArgumentNullException(nameof(roleStore)); }
            _commands = userCommands ?? throw new ArgumentNullException(nameof(userCommands));
            _queries = userQueries ?? throw new ArgumentNullException(nameof(userQueries));
            _siteSettings = currentSite ?? throw new ArgumentNullException(nameof(currentSite));
            _log = logger;
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _context = contextAccessor?.HttpContext;
            _userAddedToRoleHandlers = userAddedToRoleHandlers;
            _userRemovedFromRoleHandlers = userRemovedFromRoleHandlers;

        }

        private readonly HttpContext _context;
        //private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        private MultiTenantOptions _multiTenantOptions;
        private IUserCommands _commands;
        private IUserQueries _queries;
        private ILogger _log;
        private readonly IEnumerable<IHandleUserAddedToRole> _userAddedToRoleHandlers;
        private readonly IEnumerable<IHandleUserRemovedFromRole> _userRemovedFromRoleHandlers;


        private ISiteContext _siteSettings = null;

        private ISiteContext Site
        {
            get
            {
                //if (siteSettings == null) { siteSettings = siteResolver.Resolve(); }
                return _siteSettings;
            }
        }

        public async Task<int> CountOfRoles(Guid siteId, string searchInput)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return await _queries.CountOfRoles(siteId, searchInput, CancellationToken);
        }

        public async Task<PagedResult<ISiteRole>> GetRolesBySite(
            Guid siteId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return await _queries.GetRolesBySite(siteId, searchInput, pageNumber, pageSize, CancellationToken);

        }

        
        public async Task DeleteUserRolesByRole(Guid roleId)
        {
            var siteId = Site.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            await _commands.DeleteUserRolesByRole(siteId, roleId, CancellationToken);
        }

        public async Task DeleteRole(Guid roleId)
        {
            var siteId = Site.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            await _commands.DeleteRole(siteId, roleId, CancellationToken);
        }

        //public async Task<bool> RoleExists(Guid siteId, string roleName)
        //{
        //    if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteGuid; }

        //    return await queries.RoleExists(siteId, roleName, CancellationToken);
        //}

        public async Task<PagedResult<IUserInfo>> GetUsersInRole(
            Guid siteId, 
            Guid roleId, 
            string searchInput, 
            int pageNumber, 
            int pageSize)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return await _queries.GetUsersInRole(siteId, roleId, searchInput, pageNumber, pageSize, CancellationToken);
        }

        public async Task<int> CountUsersInRole(
            Guid siteId, 
            Guid roleId, 
            string searchInput)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return await _queries.CountUsersInRole(siteId, roleId, searchInput, CancellationToken);
        }

        public async Task<PagedResult<IUserInfo>> GetUsersNotInRole(
            Guid siteId, 
            Guid roleId, 
            string searchInput, 
            int pageNumber, 
            int pageSize)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return await _queries.GetUsersNotInRole(siteId, roleId, searchInput, pageNumber, pageSize, CancellationToken);
        }

        public async Task<int> CountUsersNotInRole(
            Guid siteId, 
            Guid roleId, 
            string searchInput)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return await _queries.CountUsersNotInRole(siteId, roleId, searchInput, CancellationToken);
        }

        public async Task AddUserToRole(ISiteUser user, ISiteRole role)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (role == null) { throw new ArgumentNullException(nameof(role)); }
            if(role.SiteId != user.SiteId) { throw new ArgumentException("user and role must have the same siteid"); }

            await _commands.AddUserToRole(role.SiteId, role.Id, user.Id, CancellationToken);
            
            user.RolesChanged = true;
            await _commands.Update(user, CancellationToken);

            foreach(var handler in _userAddedToRoleHandlers)
            {
                try
                {
                    await handler.Handle(user, role);
                }
                catch(Exception ex)
                {
                    _log.LogError($"{ex.Message}-{ex.StackTrace}");
                }
            }
            
            
        }

        public async Task RemoveUserFromRole(ISiteUser user, ISiteRole role)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (role == null) { throw new ArgumentNullException(nameof(role)); }

            await _commands.RemoveUserFromRole(role.SiteId, role.Id, user.Id, CancellationToken);
            
            user.RolesChanged = true;
            await _commands.Update(user, CancellationToken);

            foreach (var handler in _userRemovedFromRoleHandlers)
            {
                try
                {
                    await handler.Handle(user, role);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message}-{ex.StackTrace}");
                }
            }

        }

    }
}
