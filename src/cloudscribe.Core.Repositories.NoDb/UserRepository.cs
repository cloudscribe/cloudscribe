// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-26
// Last Modified:           2016-05-07
// 

using cloudscribe.Core.Models;
using NoDb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.NoDb
{
    
    public class UserRepository
    {
        public UserRepository(
            IProjectResolver projectResolver,
            IBasicCommands<SiteUser> userCommands,
            IBasicQueries<SiteUser> userQueries,
            IBasicCommands<SiteRole> roleCommands,
            IBasicQueries<SiteRole> roleQueries,
            IBasicCommands<UserRole> userRoleCommands,
            IBasicQueries<UserRole> userRoleQueries,
            IBasicCommands<UserClaim> claimCommands,
            IBasicQueries<UserClaim> claimQueries,
            IBasicCommands<UserLogin> loginCommands,
            IBasicQueries<UserLogin> loginQueries,
            IBasicCommands<UserLocation> locationCommands,
            IBasicQueries<UserLocation> locationQueries,
            IStoragePathResolver<UserLogin> loginPathResolver
            )
        {
            this.projectResolver = projectResolver;
            this.userCommands = userCommands;
            this.userQueries = userQueries;
            this.roleCommands = roleCommands;
            this.roleQueries = roleQueries;
            this.userRoleCommands = userRoleCommands;
            this.userRoleQueries = userRoleQueries;
            this.claimCommands = claimCommands;
            this.claimQueries = claimQueries;
            this.loginCommands = loginCommands;
            this.locationCommands = locationCommands;
            this.locationQueries = locationQueries;
            this.loginPathResolver = loginPathResolver;
            this.loginQueries = loginQueries;
        }

        private IProjectResolver projectResolver;
        private IBasicCommands<SiteUser> userCommands;
        private IBasicQueries<SiteUser> userQueries;
        private IBasicCommands<SiteRole> roleCommands;
        private IBasicQueries<SiteRole> roleQueries;
        private IBasicCommands<UserRole> userRoleCommands;
        private IBasicQueries<UserRole> userRoleQueries;
        private IBasicCommands<UserClaim> claimCommands;
        private IBasicQueries<UserClaim> claimQueries;
        private IBasicCommands<UserLogin> loginCommands;
        private IBasicQueries<UserLogin> loginQueries;
        private IBasicCommands<UserLocation> locationCommands;
        private IBasicQueries<UserLocation> locationQueries;
        private IStoragePathResolver<UserLogin> loginPathResolver;

        protected string projectId;

        private async Task EnsureProjectId()
        {
            if(string.IsNullOrEmpty(projectId))
            {
                await projectResolver.ResolveProjectId().ConfigureAwait(false);
            }

        }

        #region User

        public async Task<bool> Save(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = false;
            if (user == null) { return result; }
            
            if (user.SiteGuid == Guid.Empty) { throw new ArgumentException("user must have a siteguid"); }

            await EnsureProjectId().ConfigureAwait(false);

            SiteUser siteUser = SiteUser.FromISiteUser(user);
            if (siteUser.UserGuid == Guid.Empty)
            {
                siteUser.UserGuid = Guid.NewGuid();
                
                result = await userCommands.CreateAsync(
                    projectId, 
                    siteUser.UserGuid.ToString(), 
                    siteUser, 
                    cancellationToken).ConfigureAwait(false);
            }
            else
            {
                result = await userCommands.UpdateAsync(
                    projectId, 
                    siteUser.UserGuid.ToString(), 
                    siteUser, 
                    cancellationToken).ConfigureAwait(false);

            }
            
            return result;

        }

        public async Task<bool> Delete(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = false;

            await EnsureProjectId().ConfigureAwait(false);

            result = await userCommands.DeleteAsync(projectId, userGuid.ToString(), cancellationToken).ConfigureAwait(false);
            
            return result;

        }

        //public async Task<bool> DeleteUsersBySite(
        //    Guid siteGuid,
        //    CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    bool result = await DeleteLoginsBySite(siteGuid);
        //    result = await DeleteClaimsBySite(siteGuid);
        //    result = await DeleteUserRolesBySite(siteGuid);

        //    var query = from x in dbContext.Users.Where(x => x.SiteGuid == siteGuid)
        //                select x;

        //    dbContext.Users.RemoveRange(query);
        //    int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
        //        .ConfigureAwait(false);

        //    result = rowsAffected > 0;

        //    return result;
        //}

        public async Task<bool> FlagAsDeleted(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var result = false;

            await EnsureProjectId().ConfigureAwait(false);

            var item
                = await userQueries.FetchAsync(
                    projectId,
                    userGuid.ToString(),
                    cancellationToken).ConfigureAwait(false);

            if (item == null) { return result; }

            item.IsDeleted = true;

            result = await userCommands.UpdateAsync(
                    projectId,
                    item.UserGuid.ToString(),
                    item,
                    cancellationToken).ConfigureAwait(false);

            return result;

        }

        #endregion

        #region Roles

        public async Task<bool> SaveRole(
            ISiteRole role,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var result = false;

            if (role == null) { return result; }

            await EnsureProjectId().ConfigureAwait(false);
            
            if (role.SiteGuid == Guid.Empty) { throw new ArgumentException("SiteGuid must be provided"); }

            SiteRole siteRole = SiteRole.FromISiteRole(role);
            if (siteRole.RoleGuid == Guid.Empty)
            {
                siteRole.RoleGuid = Guid.NewGuid();
                if (siteRole.RoleName.Length == 0)
                {
                    siteRole.RoleName = siteRole.DisplayName;
                }
                result = await roleCommands.CreateAsync(
                    projectId,
                    siteRole.RoleGuid.ToString(),
                    siteRole,
                    cancellationToken).ConfigureAwait(false);
            }
            else
            {
                result = await roleCommands.UpdateAsync(
                    projectId,
                    siteRole.RoleGuid.ToString(),
                    siteRole,
                    cancellationToken).ConfigureAwait(false);

            }

            return result;

        }

        public async Task<bool> DeleteRole(
            Guid roleGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            
            await EnsureProjectId().ConfigureAwait(false);

            return await roleCommands.DeleteAsync(
                projectId,
                roleGuid.ToString(),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> DeleteRolesBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var result = false;
            var all = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x => x.SiteGuid == siteGuid);

            foreach (var item in filtered)
            {
                var tmpResult = await roleCommands.DeleteAsync(
                    projectId,
                    item.RoleGuid.ToString(),
                    cancellationToken).ConfigureAwait(false);

                if (tmpResult) result = true;
            }

            return result;
            
        }

        public async Task<bool> AddUserToRole(
            Guid roleGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            UserRole ur = new UserRole();
            ur.RoleGuid = roleGuid;
            ur.UserGuid = userGuid;

            var key = userGuid.ToString() + "~" + roleGuid.ToString();

            var result = await userRoleCommands.CreateAsync(
                projectId,
                key,
                ur,
                cancellationToken).ConfigureAwait(false);
            
            return result;

        }

        public async Task<bool> RemoveUserFromRole(
            Guid roleGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);
            
            var key = userGuid.ToString() + "~" + roleGuid.ToString();

            return await userRoleCommands.DeleteAsync(
                projectId,
                key,
                cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> DeleteUserRoles(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var result = false;
            var all = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(
                x => x.UserGuid == userGuid 
            );

            foreach (var item in filtered)
            {
                var key = item.UserGuid.ToString() + "~" + item.RoleGuid.ToString();

                var tmpResult = await roleCommands.DeleteAsync(
                    projectId,
                    key,
                    cancellationToken).ConfigureAwait(false);

                if (tmpResult) result = true;
            }

            return result;

        }

        public async Task<bool> DeleteUserRolesByRole(
            Guid roleGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var result = false;
            var all = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(
                x => x.RoleGuid == roleGuid
            );

            foreach (var item in filtered)
            {
                var key = item.UserGuid.ToString() + "~" + item.RoleGuid.ToString();

                var tmpResult = await roleCommands.DeleteAsync(
                    projectId,
                    key,
                    cancellationToken).ConfigureAwait(false);

                if (tmpResult) result = true;
            }

            return result;
        }

        public async Task<bool> DeleteUserRolesBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);
            
            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var siteRoles = allRoles.Where(
                x => x.SiteGuid == siteGuid
            );

            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            
            var result = false;
            var query = from x in allUserRoles
                        join y in siteRoles on x.RoleGuid equals y.RoleGuid
                        select x;

            foreach (var item in query)
            {
                var key = item.UserGuid.ToString() + "~" + item.RoleGuid.ToString();

                var tmpResult = await roleCommands.DeleteAsync(
                    projectId,
                    key,
                    cancellationToken).ConfigureAwait(false);

                if (tmpResult) result = true;
            }

            return result;
            
        }

        public async Task<bool> RoleExists(
            Guid siteGuid,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteGuid == siteGuid
                && x.RoleName == roleName
            );
            
            return filteredRoles.ToList().Count > 0;
        }

        public async Task<ISiteRole> FetchRole(
            Guid roleGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            return await roleQueries.FetchAsync(
                projectId,
                roleGuid.ToString(),
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task<ISiteRole> FetchRole(
            Guid siteGuid,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteGuid == siteGuid
                && x.RoleName == roleName
            );

            return filteredRoles.FirstOrDefault();

        }

        public async Task<List<string>> GetUserRoles(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteGuid == siteGuid
            );

            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in filteredRoles
                        join y in allUserRoles
                        on x.RoleGuid equals y.RoleGuid
                        where y.UserGuid == userGuid
                        orderby x.RoleName
                        select x.RoleName
                        ;

            return query.ToList<string>();

        }

        public async Task<int> CountOfRoles(
            Guid siteGuid,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteGuid == siteGuid
                && (
                 (searchInput == "")
                        || x.DisplayName.Contains(searchInput)
                        || x.RoleName.Contains(searchInput)
                )
            );

            return filteredRoles.ToList().Count;
            
        }

        public async Task<IList<ISiteRole>> GetRolesBySite(
            Guid siteGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteGuid == siteGuid
                && (
                 (searchInput == "")
                        || x.DisplayName.Contains(searchInput)
                        || x.RoleName.Contains(searchInput)
                )
            );

            int offset = (pageSize * pageNumber) - pageSize;

            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var listQuery = from x in filteredRoles
                            orderby x.RoleName ascending
                            select x;

            return listQuery
                .Skip(offset)
                .Take(pageSize)
                .ToList<ISiteRole>()
                ;

        }

        public async Task<int> CountUsersInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in allUsers
                        join y in allUserRoles
                        on x.UserGuid equals y.UserGuid
                        where (
                            (x.SiteGuid.Equals(siteGuid) && y.RoleGuid.Equals(roleGuid))
                            && (
                                (searchInput == "")
                                || x.Email.Contains(searchInput)
                                || x.DisplayName.Contains(searchInput)
                                || x.UserName.Contains(searchInput)
                                || x.FirstName.Contains(searchInput)
                                || x.LastName.Contains(searchInput)
                            )
                            )
                        
                        select x
                        ;

            return query.ToList().Count();

        }

        public async Task<IList<IUserInfo>> GetUsersInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            

            var query = from x in allUsers
                        join y in allUserRoles
                        on x.UserGuid equals y.UserGuid
                        orderby x.DisplayName
                        where (
                            (x.SiteGuid.Equals(siteGuid) && y.RoleGuid.Equals(roleGuid))
                            && (
                                (searchInput == "")
                                || x.Email.Contains(searchInput)
                                || x.DisplayName.Contains(searchInput)
                                || x.UserName.Contains(searchInput)
                                || x.FirstName.Contains(searchInput)
                                || x.LastName.Contains(searchInput)
                            )
                            )
                        select x
                        ;

            int offset = (pageSize * pageNumber) - pageSize;

            return query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;

        }

        public async Task<IList<ISiteUser>> GetUsersInRole(
            Guid siteGuid,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var siteRoles = allRoles.Where(x => x.SiteGuid == siteGuid);

            var query = from x in allUsers
                        join y in allUserRoles
                        on x.UserGuid equals y.UserGuid
                        join z in siteRoles
                        on y.RoleGuid equals z.RoleGuid
                        orderby x.DisplayName
                        where
                            (x.SiteGuid.Equals(siteGuid) && z.RoleName.Equals(roleName))

                        select x
                        ;

            var items = query
                .ToList<ISiteUser>()
                ;

            return items;
        }

        public async Task<int> CountUsersNotInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var siteRoles = allRoles.Where(x => x.SiteGuid == siteGuid);

            var query = from u in allUsers
                        from r in siteRoles
                        join ur in allUserRoles
                        on new { r.RoleGuid, u.UserGuid } equals new { ur.RoleGuid, ur.UserGuid } into t
                        from t2 in t.DefaultIfEmpty()
                        where (
                        u.SiteGuid == siteGuid
                        && r.SiteGuid == siteGuid
                        && r.RoleGuid == roleGuid
                        && (
                                (searchInput == "")
                                || u.Email.Contains(searchInput)
                                || u.DisplayName.Contains(searchInput)
                                || u.UserName.Contains(searchInput)
                                || u.FirstName.Contains(searchInput)
                                || u.LastName.Contains(searchInput)
                            )

                        && t2 == null
                        )

                        select u;

            return query.ToList().Count;

        }

        public async Task<IList<IUserInfo>> GetUsersNotInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var siteRoles = allRoles.Where(x => x.SiteGuid == siteGuid);
            
            var query = from u in allUsers
                        from r in siteRoles
                        join ur in allUserRoles
                        on new { r.RoleGuid, u.UserGuid } equals new { ur.RoleGuid, ur.UserGuid } into t
                        from t2 in t.DefaultIfEmpty()
                        where (
                        u.SiteGuid == siteGuid
                        && r.SiteGuid == siteGuid
                        && r.RoleGuid == roleGuid
                        && (
                                (searchInput == "")
                                || u.Email.Contains(searchInput)
                                || u.DisplayName.Contains(searchInput)
                                || u.UserName.Contains(searchInput)
                                || u.FirstName.Contains(searchInput)
                                || u.LastName.Contains(searchInput)
                            )

                        && t2 == null
                        )
                        orderby u.DisplayName
                        select u;

            int offset = (pageSize * pageNumber) - pageSize;

            return query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;

        }


        #endregion

        #region Claims

        public async Task<bool> SaveClaim(
            IUserClaim userClaim,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var result = false;

            if (userClaim == null) { return result; }

            await EnsureProjectId().ConfigureAwait(false);

            UserClaim claim = UserClaim.FromIUserClaim(userClaim);
            if (claim.Id == Guid.Empty)
            {
                claim.Id = Guid.NewGuid();
                result = await claimCommands.CreateAsync(
                    projectId,
                    claim.Id.ToString(),
                    claim,
                    cancellationToken).ConfigureAwait(false);
            }
            else
            {
                result = await claimCommands.UpdateAsync(
                    projectId,
                    claim.Id.ToString(),
                    claim,
                    cancellationToken).ConfigureAwait(false);

            }
            
            return result;

        }

        public async Task<bool> DeleteClaim(
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            
            await EnsureProjectId().ConfigureAwait(false);

            return await claimCommands.DeleteAsync(
                projectId,
                id.ToString(),
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task<bool> DeleteClaimsByUser(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);
            var result = false;
            var all = await claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(
                x => x.UserGuid == userGuid
                && x.SiteGuid == siteGuid
            );

            foreach (var item in filtered)
            {
                var tmpResult = await claimCommands.DeleteAsync(
                    projectId,
                    item.Id.ToString(),
                    cancellationToken).ConfigureAwait(false);

                if (tmpResult) result = true;
            }

            return result;

        }

        public async Task<bool> DeleteClaimByUser(
            Guid siteGuid,
            Guid userGuid,
            string claimType,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);
            var result = false;
            var all = await claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(
                x => x.UserGuid == userGuid
                && x.SiteGuid == siteGuid
                && x.ClaimType == claimType
            );

            foreach (var item in filtered)
            {
                var tmpResult = await claimCommands.DeleteAsync(
                    projectId,
                    item.Id.ToString(),
                    cancellationToken).ConfigureAwait(false);

                if (tmpResult) result = true;
            }

            return result;

        }

        public async Task<bool> DeleteClaimsBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);
            var result = false;
            var all = await claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x =>
                 x.SiteGuid == siteGuid 
            );

            foreach (var item in filtered)
            {
                var tmpResult = await claimCommands.DeleteAsync(
                    projectId,
                    item.Id.ToString(),
                    cancellationToken).ConfigureAwait(false);

                if (tmpResult) result = true;
            }

            return result;

        }

        public async Task<IList<IUserClaim>> GetClaimsByUser(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);
           
            var all = await claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x =>
                 x.SiteGuid == siteGuid
                 && x.UserGuid == userGuid
            );

            return filtered as IList<IUserClaim>;

        }


        public async Task<IList<ISiteUser>> GetUsersForClaim(
            Guid siteGuid,
            string claimType,
            string claimValue,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allClaims = await claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredClaims = allClaims.Where(x =>
                 x.SiteGuid == siteGuid
                 && x.ClaimType == claimType
                 && x.ClaimValue == claimValue
            );

            var allUsers = await userQueries.GetAllAsync(
                projectId,
                cancellationToken).ConfigureAwait(false);

            var query = from x in allUsers
                        join y in filteredClaims
                        on x.UserGuid equals y.UserGuid
                        where x.SiteGuid == siteGuid
                        orderby x.DisplayName
                        select x
                        ;

            return query.ToList() as IList<ISiteUser>;
            
            
        }


        #endregion

        #region Logins

        public async Task<bool> CreateLogin(
            IUserLogin userLogin,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (userLogin == null) { return false; }
            if (userLogin.LoginProvider.Length == -1) { return false; }
            if (userLogin.ProviderKey.Length == -1) { return false; }
            if (userLogin.UserGuid == Guid.Empty) { return false; }

            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            UserLogin login = UserLogin.FromIUserLogin(userLogin);

            var result = false;
            // this will be a tricky one for queries because the key consists of 4 columns
            // TODO: review this and whether we really need all the  parts of the key in EF
            // http://www.jerriepelser.com/blog/using-aspnet-oauth-providers-without-identity
            // ProviderKey is the unique key associated with the login on that service
            var key = login.UserGuid.ToString()
                + "~" + login.SiteGuid.ToString()
                + "~" + login.LoginProvider
                + "~" + login.ProviderKey;

            result = await loginCommands.CreateAsync(
                projectId,
                key,
                login,
                cancellationToken).ConfigureAwait(false);

            return result;

        }

        public async Task<IUserLogin> FindLogin(
            Guid siteGuid,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var folderPath = await loginPathResolver.ResolvePath(projectId).ConfigureAwait(false);

            if (!Directory.Exists(folderPath)) return null;

            // understand structure of key which is the filename
            //var key = login.UserGuid.ToString()
            //    + "~" + login.SiteGuid.ToString()
            //    + "~" + login.LoginProvider
            //    + "~" + login.ProviderKey;

            var matchPattern = "*~" + siteGuid.ToString()
                + "~" + loginProvider
                + "~" + providerKey;

            var dir = new DirectoryInfo(folderPath);
            var matches = dir.GetFiles(matchPattern);
            var foundFileKey = string.Empty;
            foreach(var match in matches)
            {
                foundFileKey = Path.GetFileNameWithoutExtension(match.Name);
                break; // should only be one so we won't keep interating
            }
            
            if(!string.IsNullOrEmpty(foundFileKey))
            {
                return await loginQueries.FetchAsync(
                    projectId,
                    foundFileKey,
                    cancellationToken).ConfigureAwait(false);
            }

            return null;
        }

        public async Task<bool> DeleteLogin(
            Guid siteGuid,
            Guid userGuid,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var key = userGuid.ToString()
                + "~" + siteGuid.ToString()
                + "~" + loginProvider
                + "~" + providerKey;

            return await loginCommands.DeleteAsync(
                projectId,
                key,
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task<bool> DeleteLoginsByUser(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            
            await EnsureProjectId().ConfigureAwait(false);

            var folderPath = await loginPathResolver.ResolvePath(projectId).ConfigureAwait(false);

            // understand structure of key which is the filename
            //var key = login.UserGuid.ToString()
            //    + "~" + login.SiteGuid.ToString()
            //    + "~" + login.LoginProvider
            //    + "~" + login.ProviderKey;

            var matchPattern = userGuid.ToString() +
                "~" + siteGuid.ToString()
                + "~*";

            var dir = new DirectoryInfo(folderPath);
            var matches = dir.GetFiles(matchPattern);

            var result = false;
            foreach (var match in matches)
            {
                var foundFileKey = Path.GetFileNameWithoutExtension(match.Name);
                var tempResult = await loginCommands.DeleteAsync(
                    projectId,
                    foundFileKey,
                    cancellationToken).ConfigureAwait(false);

                if (tempResult) result = true;
            }

            
            return result;

        }

        public async Task<bool> DeleteLoginsBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var folderPath = await loginPathResolver.ResolvePath(projectId).ConfigureAwait(false);

            // understand structure of key which is the filename
            //var key = login.UserGuid.ToString()
            //    + "~" + login.SiteGuid.ToString()
            //    + "~" + login.LoginProvider
            //    + "~" + login.ProviderKey;

            var matchPattern =  "*~" + siteGuid.ToString() + "~*";

            var dir = new DirectoryInfo(folderPath);
            var matches = dir.GetFiles(matchPattern);

            var result = false;
            foreach (var match in matches)
            {
                var foundFileKey = Path.GetFileNameWithoutExtension(match.Name);
                var tempResult = await loginCommands.DeleteAsync(
                    projectId,
                    foundFileKey,
                    cancellationToken).ConfigureAwait(false);

                if (tempResult) result = true;
            }


            return result;


        }

        public async Task<IList<IUserLogin>> GetLoginsByUser(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var folderPath = await loginPathResolver.ResolvePath(projectId).ConfigureAwait(false);

            // understand structure of key which is the filename
            //var key = login.UserGuid.ToString()
            //    + "~" + login.SiteGuid.ToString()
            //    + "~" + login.LoginProvider
            //    + "~" + login.ProviderKey;

            var matchPattern = "*~" + siteGuid.ToString() + "~*";

            var dir = new DirectoryInfo(folderPath);
            var matches = dir.GetFiles(matchPattern);

            var result = new List<IUserLogin>();
            foreach (var match in matches)
            {
                var foundFileKey = Path.GetFileNameWithoutExtension(match.Name);
                var tempResult = await loginQueries.FetchAsync(
                    projectId,
                    foundFileKey,
                    cancellationToken).ConfigureAwait(false);

                if (tempResult != null) result.Add(tempResult);
            }


            return result;


        }

        #endregion

        #region UserLocation

        public async Task<IUserLocation> FetchLocationByUserAndIpv4Address(
            Guid userGuid,
            long ipv4AddressAsLong,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var all = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            
            var query = from x in all
                        where x.UserGuid == userGuid
                        && x.IpAddressLong == ipv4AddressAsLong
                        select x
                        ;

            return query.FirstOrDefault<UserLocation>();

        }

        public async Task<bool> AddUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            var result = false;
            
            if (userLocation == null) { return result; }

            await EnsureProjectId().ConfigureAwait(false);

            UserLocation ul = UserLocation.FromIUserLocation(userLocation);
            if (ul.RowId == Guid.Empty)
            {
                ul.RowId = Guid.NewGuid();
                result = await locationCommands.CreateAsync(
                    projectId,
                    ul.RowId.ToString(),
                    ul,
                    cancellationToken).ConfigureAwait(false);
            }
            else
            {
                result = await locationCommands.UpdateAsync(
                    projectId,
                    ul.RowId.ToString(),
                    ul,
                    cancellationToken).ConfigureAwait(false);
            }
            
            
            return result;

        }

        public async Task<bool> UpdateUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            var result = false;

            if (userLocation == null) { return result; }

            await EnsureProjectId().ConfigureAwait(false);

            UserLocation ul = UserLocation.FromIUserLocation(userLocation);

            result = await locationCommands.UpdateAsync(
                    projectId,
                    ul.RowId.ToString(),
                    ul,
                    cancellationToken).ConfigureAwait(false);

            return result;

        }

        public async Task<bool> DeleteUserLocation(
            Guid rowGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            var result = false;

            await EnsureProjectId().ConfigureAwait(false);

            result = await locationCommands.DeleteAsync(
                projectId,
                rowGuid.ToString(),
                cancellationToken).ConfigureAwait(false);
            
            return result;
            
        }

        public async Task<bool> DeleteUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            var result = false;

            await EnsureProjectId().ConfigureAwait(false);

            var all = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x => x.UserGuid == userGuid);
            foreach(var loc in filtered)
            {
                var tmpResult = await locationCommands.DeleteAsync(
                    projectId,
                    loc.RowId.ToString(),
                    cancellationToken).ConfigureAwait(false);

                if (tmpResult) result = true;
            }
            
            return result;

        }

        public async Task<bool> DeleteUserLocationsBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            var result = false;

            await EnsureProjectId().ConfigureAwait(false);

            var all = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x => x.SiteGuid == siteGuid);
            foreach (var loc in filtered)
            {
                var tmpResult = await locationCommands.DeleteAsync(
                    projectId,
                    loc.RowId.ToString(),
                    cancellationToken).ConfigureAwait(false);

                if (tmpResult) result = true;
            }

            return result;
            
        }

        public async Task<int> CountUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            var result = 0;

            await EnsureProjectId().ConfigureAwait(false);

            var all = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x => x.UserGuid == userGuid);
            result = filtered.ToList().Count;

            return result;

        }

        public async Task<IList<IUserLocation>> GetUserLocationsByUser(
            Guid userGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            
            await EnsureProjectId().ConfigureAwait(false);

            int offset = (pageSize * pageNumber) - pageSize;

            var all = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            
            var query = all
                .OrderBy(x => x.IpAddressLong)
                .Select(p => p)
                .Skip(offset)
                .Take(pageSize)
                .Where(x => x.UserGuid == userGuid)
                ;

            return  query.ToList() as IList<IUserLocation>;

        }


        #endregion

        #region IDisposable Support

        private void ThrowIfDisposed()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SiteRoleStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
