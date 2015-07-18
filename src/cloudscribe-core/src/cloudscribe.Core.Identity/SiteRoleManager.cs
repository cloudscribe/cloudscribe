// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-17
// Last Modified:		    2015-07-17
// 
//

using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
            ISiteResolver siteResolver,
            IUserRepository userRepository,
            IRoleStore<TRole> roleStore,
            IEnumerable<IRoleValidator<TRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<TRole>> logger,
            IHttpContextAccessor contextAccessor
            ) : base(
                roleStore, 
                roleValidators, 
                new UseOriginalLookupNormalizer(), //bypass the uppercasenormalizer passed in
                errors, 
                logger, 
                contextAccessor)
        {

            this.siteResolver = siteResolver;
            userRepo = userRepository;
            this.logger = logger;
            
        }

        private ISiteResolver siteResolver;
        private IUserRepository userRepo;
        private ILogger logger;
        private ISiteSettings siteSettings = null;
        private ISiteSettings Site
        {
            get
            {
                if (siteSettings == null) { siteSettings = siteResolver.Resolve(); }
                return siteSettings;
            }
        }

        public async Task<int> CountOfRoles(int siteId, string searchInput)
        {
            return await userRepo.CountOfRoles(siteId, searchInput);
        }

        public async Task<IList<ISiteRole>> GetRolesBySite(
            int siteId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            return await userRepo.GetRolesBySite(siteId, searchInput, pageNumber, pageSize);

        }

        /// <summary>
        /// TODO: this replicates RoleStore.FindByIdAsync except this takes int as it should
        /// I think there is a way to fix FindByIdAsync so it takes int then get rid of this one
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<ISiteRole> FetchRole(int roleId)
        {
            return await userRepo.FetchRole(roleId);
        }

        // again need to consolidate with RoleStore.UpdateAsync
        public async Task<bool> SaveRole(ISiteRole role)
        {
            return await userRepo.SaveRole(role);
        }

        public async Task<bool> DeleteUserRolesByRole(int roleId)
        {
            return await userRepo.DeleteUserRolesByRole(roleId);
        }

        public async Task<bool> DeleteRole(int roleId)
        {
            return await userRepo.DeleteRole(roleId);
        }

        public async Task<IList<IUserInfo>> GetUsersInRole(int siteId, int roleId, string searchInput, int pageNumber, int pageSize)
        {
            return await userRepo.GetUsersInRole(siteId, roleId, searchInput, pageNumber, pageSize);
        }

        public async Task<int> CountUsersInRole(int siteId, int roleId, string searchInput)
        {
            return await userRepo.CountUsersInRole(siteId, roleId, searchInput);
        }

        public async Task<IList<IUserInfo>> GetUsersNotInRole(int siteId, int roleId, string searchInput, int pageNumber, int pageSize)
        {
            return await userRepo.GetUsersNotInRole(siteId, roleId, searchInput, pageNumber, pageSize);
        }

        public async Task<int> CountUsersNotInRole(int siteId, int roleId, string searchInput)
        {
            return await userRepo.CountUsersNotInRole(siteId, roleId, searchInput);
        }

    }
}
