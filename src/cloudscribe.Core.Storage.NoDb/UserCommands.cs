// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-26
// Last Modified:           2016-05-09
// 

using cloudscribe.Core.Models;
using NoDb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    public class UserCommands
    {
        public UserCommands(
            IProjectResolver projectResolver,
            IBasicCommands<SiteUser> userCommands,
            IBasicCommands<SiteRole> roleCommands,
            IBasicCommands<UserRole> userRoleCommands,
            IBasicCommands<UserClaim> claimCommands,
            IBasicCommands<UserLogin> loginCommands,
            IBasicCommands<UserLocation> locationCommands,
            IBasicQueries<SiteUser> userQueries,
            IBasicQueries<SiteRole> roleQueries,
            IBasicQueries<UserRole> userRoleQueries,
            IBasicQueries<UserClaim> claimQueries,
            IBasicQueries<UserLocation> locationQueries,
            IStoragePathResolver<UserLogin> loginPathResolver
            )
        {
            this.projectResolver = projectResolver;
            this.userCommands = userCommands;
            this.roleCommands = roleCommands;
            this.userRoleCommands = userRoleCommands;
            this.claimCommands = claimCommands;
            this.loginCommands = loginCommands;
            this.locationCommands = locationCommands;
            this.loginPathResolver = loginPathResolver;

            this.userQueries = userQueries;
            this.roleQueries = roleQueries;
            this.userRoleQueries = userRoleQueries;
            this.claimQueries = claimQueries;
            this.locationQueries = locationQueries;

        }

        private IProjectResolver projectResolver;
        private IBasicCommands<SiteUser> userCommands;
        private IBasicCommands<SiteRole> roleCommands;
        private IBasicCommands<UserRole> userRoleCommands;
        private IBasicCommands<UserClaim> claimCommands;
        private IBasicCommands<UserLogin> loginCommands;
        private IBasicCommands<UserLocation> locationCommands;
        private IStoragePathResolver<UserLogin> loginPathResolver;

        private IBasicQueries<SiteUser> userQueries;
        private IBasicQueries<SiteRole> roleQueries;
        private IBasicQueries<UserRole> userRoleQueries;
        private IBasicQueries<UserClaim> claimQueries;
        private IBasicQueries<UserLocation> locationQueries;

        protected string projectId;

        private async Task EnsureProjectId()
        {
            if (string.IsNullOrEmpty(projectId))
            {
                await projectResolver.ResolveProjectId().ConfigureAwait(false);
            }

        }

        #region User

        public async Task Create(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user == null) { throw new ArgumentException("user cannot be null"); }
            if (user.SiteGuid == Guid.Empty) { throw new ArgumentException("user must have a siteid"); }
            if (user.UserGuid == Guid.Empty) { throw new ArgumentException("user must have a non-empty id"); }

            await EnsureProjectId().ConfigureAwait(false);

            var siteUser = SiteUser.FromISiteUser(user);
            
            await userCommands.CreateAsync(
                projectId,
                siteUser.UserGuid.ToString(),
                siteUser,
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task Update(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user == null) { throw new ArgumentException("user cannot be null"); }
            if (user.SiteGuid == Guid.Empty) { throw new ArgumentException("user must have a siteid"); }
            if (user.UserGuid == Guid.Empty) { throw new ArgumentException("user must have a non-empty id"); }

            await EnsureProjectId().ConfigureAwait(false);
 
            var siteUser = SiteUser.FromISiteUser(user);
            await userCommands.UpdateAsync(
                    projectId,
                    siteUser.UserGuid.ToString(),
                    siteUser,
                    cancellationToken).ConfigureAwait(false);

        }

        public async Task Delete(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            
            await EnsureProjectId().ConfigureAwait(false);

            await userCommands.DeleteAsync(projectId, userGuid.ToString(), cancellationToken).ConfigureAwait(false);
 
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

        public async Task FlagAsDeleted(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            
            await EnsureProjectId().ConfigureAwait(false);

            var item
                = await userQueries.FetchAsync(
                    projectId,
                    userGuid.ToString(),
                    cancellationToken).ConfigureAwait(false);

            if (item == null) { throw new InvalidOperationException("user not found"); }

            item.IsDeleted = true;

            await userCommands.UpdateAsync(
                    projectId,
                    item.UserGuid.ToString(),
                    item,
                    cancellationToken).ConfigureAwait(false);
            
        }

        #endregion

        #region Roles

        public async Task CreateRole(
            ISiteRole role,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null) { throw new ArgumentException("role cannot be null"); }
            if (role.SiteGuid == Guid.Empty) { throw new ArgumentException("SiteId must be provided"); }
            if (role.RoleGuid == Guid.Empty) { throw new ArgumentException("Id must be provided"); }

            await EnsureProjectId().ConfigureAwait(false);
            
            var siteRole = SiteRole.FromISiteRole(role);
            
            if (siteRole.RoleName.Length == 0)
            {
                siteRole.RoleName = siteRole.DisplayName;
            }
            await roleCommands.CreateAsync(
                projectId,
                siteRole.RoleGuid.ToString(),
                siteRole,
                cancellationToken).ConfigureAwait(false);
            

        }

        public async Task UpdateRole(
            ISiteRole role,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null) { throw new ArgumentException("role cannot be null"); }
            if (role.SiteGuid == Guid.Empty) { throw new ArgumentException("SiteId must be provided"); }
            if (role.RoleGuid == Guid.Empty) { throw new ArgumentException("Id must be provided"); }

            await EnsureProjectId().ConfigureAwait(false);
            
            var siteRole = SiteRole.FromISiteRole(role);

            await roleCommands.UpdateAsync(
                    projectId,
                    siteRole.RoleGuid.ToString(),
                    siteRole,
                    cancellationToken).ConfigureAwait(false);
            
        }

        public async Task DeleteRole(
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (id == Guid.Empty) { throw new ArgumentException("Id must be provided"); }

            await EnsureProjectId().ConfigureAwait(false);

            await roleCommands.DeleteAsync(
                projectId,
                id.ToString(),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteRolesBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId == Guid.Empty) { throw new ArgumentException("siteId must be provided"); }

            await EnsureProjectId().ConfigureAwait(false);
            
            var all = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x => x.SiteGuid == siteId);

            foreach (var item in filtered)
            {
                await roleCommands.DeleteAsync(
                    projectId,
                    item.RoleGuid.ToString(),
                    cancellationToken).ConfigureAwait(false);

            }

            
        }

        public async Task AddUserToRole(
            Guid roleId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (roleId == Guid.Empty) { throw new ArgumentException("roleId must be provided"); }
            if (userId == Guid.Empty) { throw new ArgumentException("userId must be provided"); }

            await EnsureProjectId().ConfigureAwait(false);

            UserRole ur = new UserRole();
            ur.RoleGuid = roleId;
            ur.UserGuid = userId;

            var key = userId.ToString() + "~" + roleId.ToString();

            await userRoleCommands.CreateAsync(
                projectId,
                key,
                ur,
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task RemoveUserFromRole(
            Guid roleId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (roleId == Guid.Empty) { throw new ArgumentException("roleId must be provided"); }
            if (userId == Guid.Empty) { throw new ArgumentException("userId must be provided"); }

            await EnsureProjectId().ConfigureAwait(false);

            var key = userId.ToString() + "~" + roleId.ToString();

            await userRoleCommands.DeleteAsync(
                projectId,
                key,
                cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteUserRoles(
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (userId == Guid.Empty) { throw new ArgumentException("userId must be provided"); }

            await EnsureProjectId().ConfigureAwait(false);

            var all = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(
                x => x.UserGuid == userId
            );

            foreach (var item in filtered)
            {
                var key = item.UserGuid.ToString() + "~" + item.RoleGuid.ToString();

                await roleCommands.DeleteAsync(
                    projectId,
                    key,
                    cancellationToken).ConfigureAwait(false);
  
            }

        }

        public async Task DeleteUserRolesByRole(
            Guid roleId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (roleId == Guid.Empty) { throw new ArgumentException("roleId must be provided"); }

            await EnsureProjectId().ConfigureAwait(false);
            
            var all = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(
                x => x.RoleGuid == roleId
            );

            foreach (var item in filtered)
            {
                var key = item.UserGuid.ToString() + "~" + item.RoleGuid.ToString();

                await roleCommands.DeleteAsync(
                    projectId,
                    key,
                    cancellationToken).ConfigureAwait(false);
 
            }

        }

        public async Task DeleteUserRolesBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId == Guid.Empty) { throw new ArgumentException("siteId must be provided"); }

            await EnsureProjectId().ConfigureAwait(false);

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var siteRoles = allRoles.Where(
                x => x.SiteGuid == siteId
            );

            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in allUserRoles
                        join y in siteRoles on x.RoleGuid equals y.RoleGuid
                        select x;

            foreach (var item in query)
            {
                var key = item.UserGuid.ToString() + "~" + item.RoleGuid.ToString();

                await roleCommands.DeleteAsync(
                    projectId,
                    key,
                    cancellationToken).ConfigureAwait(false);

            }


        }




        #endregion

        #region Claims

        public async Task CreateClaim(
            IUserClaim userClaim,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            
            if (userClaim == null) { throw new ArgumentException("userClaim cannnot be null"); }
            if (userClaim.Id == Guid.Empty) { throw new ArgumentException("userClaim must have an id"); }
            if (userClaim.SiteGuid == Guid.Empty) { throw new ArgumentException("userClaim must have n SiteId"); }
            if (userClaim.UserGuid == Guid.Empty) { throw new ArgumentException("userClaim must have a UserId"); }

            await EnsureProjectId().ConfigureAwait(false);

            var claim = UserClaim.FromIUserClaim(userClaim);
            
            await claimCommands.CreateAsync(
                projectId,
                claim.Id.ToString(),
                claim,
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task UpdateClaim(
            IUserClaim userClaim,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            
            if (userClaim == null) { throw new ArgumentException("userClaim cannnot be null"); }
            if (userClaim.Id == Guid.Empty) { throw new ArgumentException("userClaim must have an id"); }
            if (userClaim.SiteGuid == Guid.Empty) { throw new ArgumentException("userClaim must have n SiteId"); }
            if (userClaim.UserGuid == Guid.Empty) { throw new ArgumentException("userClaim must have a UserId"); }

            await EnsureProjectId().ConfigureAwait(false);

            var claim = UserClaim.FromIUserClaim(userClaim);
            await claimCommands.UpdateAsync(
                    projectId,
                    claim.Id.ToString(),
                    claim,
                    cancellationToken).ConfigureAwait(false);

        }

        public async Task DeleteClaim(
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (id == Guid.Empty) { throw new ArgumentException("id must be provided"); }

            await EnsureProjectId().ConfigureAwait(false);

            await claimCommands.DeleteAsync(
                projectId,
                id.ToString(),
                cancellationToken).ConfigureAwait(false);

        }

        public async Task DeleteClaimsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId == Guid.Empty) { throw new ArgumentException("SiteId must be provided"); }
            if (userId == Guid.Empty) { throw new ArgumentException("UserId must be provided"); }

            await EnsureProjectId().ConfigureAwait(false);

            var all = await claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(
                x => x.UserGuid == userId
                && x.SiteGuid == siteId
            );

            foreach (var item in filtered)
            {
                await claimCommands.DeleteAsync(
                    projectId,
                    item.Id.ToString(),
                    cancellationToken).ConfigureAwait(false);   
            }
            
        }

        public async Task DeleteClaimByUser(
            Guid siteId,
            Guid userId,
            string claimType,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId == Guid.Empty) { throw new ArgumentException("SiteId must be provided"); }
            if (userId == Guid.Empty) { throw new ArgumentException("UserId must be provided"); }
            if (string.IsNullOrWhiteSpace(claimType)) throw new ArgumentException("claimType must be provided");

            await EnsureProjectId().ConfigureAwait(false);

            var all = await claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(
                x => x.UserGuid == userId
                && x.SiteGuid == siteId
                && x.ClaimType == claimType
            );

            foreach (var item in filtered)
            {
                await claimCommands.DeleteAsync(
                    projectId,
                    item.Id.ToString(),
                    cancellationToken).ConfigureAwait(false);
            }

        }

        public async Task DeleteClaimsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId == Guid.Empty) { throw new ArgumentException("SiteId must be provided"); }

            await EnsureProjectId().ConfigureAwait(false);

            var all = await claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x =>
                 x.SiteGuid == siteId
            );

            foreach (var item in filtered)
            {
                await claimCommands.DeleteAsync(
                    projectId,
                    item.Id.ToString(),
                    cancellationToken).ConfigureAwait(false);     
            }
            
        }





        #endregion

        #region Logins

        public async Task CreateLogin(
            IUserLogin userLogin,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (userLogin == null) throw new ArgumentException("userLogin can't be null");
            if (userLogin.LoginProvider.Length == -1) throw new ArgumentException("LoginProvider must be provided");
            if (userLogin.ProviderKey.Length == -1) throw new ArgumentException("ProviderKey must be provided");
            if (userLogin.UserGuid == Guid.Empty) throw new ArgumentException("UserId must be provided");
            if (userLogin.SiteGuid == Guid.Empty) throw new ArgumentException("SiteId must be provided");

            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var login = UserLogin.FromIUserLogin(userLogin);

            // this will be a tricky one for queries because the key consists of 4 columns
            // TODO: review this and whether we really need all the  parts of the key in EF
            // http://www.jerriepelser.com/blog/using-aspnet-oauth-providers-without-identity
            // ProviderKey is the unique key associated with the login on that service
            var key = login.UserGuid.ToString()
                + "~" + login.SiteGuid.ToString()
                + "~" + login.LoginProvider
                + "~" + login.ProviderKey;

            await loginCommands.CreateAsync(
                projectId,
                key,
                login,
                cancellationToken).ConfigureAwait(false);
            
        }

        
        public async Task DeleteLogin(
            Guid siteId,
            Guid userId,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId == Guid.Empty) throw new ArgumentException("siteId must be provided");
            if (userId == Guid.Empty) throw new ArgumentException("userId must be provided");
            if (string.IsNullOrWhiteSpace(loginProvider)) throw new ArgumentException("loginProvider not valid");
            if (string.IsNullOrWhiteSpace(providerKey)) throw new ArgumentException("providerKey not valid");

            await EnsureProjectId().ConfigureAwait(false);

            var key = userId.ToString()
                + "~" + siteId.ToString()
                + "~" + loginProvider
                + "~" + providerKey;

            await loginCommands.DeleteAsync(
                projectId,
                key,
                cancellationToken).ConfigureAwait(false);

        }

        public async Task DeleteLoginsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId == Guid.Empty) throw new ArgumentException("siteId must be provided");
            if (userId == Guid.Empty) throw new ArgumentException("userId must be provided");

            await EnsureProjectId().ConfigureAwait(false);

            var folderPath = await loginPathResolver.ResolvePath(projectId).ConfigureAwait(false);

            // understand structure of key which is the filename
            //var key = login.UserGuid.ToString()
            //    + "~" + login.SiteGuid.ToString()
            //    + "~" + login.LoginProvider
            //    + "~" + login.ProviderKey;

            var matchPattern = userId.ToString() +
                "~" + siteId.ToString()
                + "~*";

            var dir = new DirectoryInfo(folderPath);
            var matches = dir.GetFiles(matchPattern);

            foreach (var match in matches)
            {
                var foundFileKey = Path.GetFileNameWithoutExtension(match.Name);
                await loginCommands.DeleteAsync(
                    projectId,
                    foundFileKey,
                    cancellationToken).ConfigureAwait(false);
                
            }
            
        }

        public async Task DeleteLoginsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId == Guid.Empty) throw new ArgumentException("siteId must be provided");

            await EnsureProjectId().ConfigureAwait(false);

            var folderPath = await loginPathResolver.ResolvePath(projectId).ConfigureAwait(false);

            // understand structure of key which is the filename
            //var key = login.UserGuid.ToString()
            //    + "~" + login.SiteGuid.ToString()
            //    + "~" + login.LoginProvider
            //    + "~" + login.ProviderKey;

            var matchPattern = "*~" + siteId.ToString() + "~*";

            var dir = new DirectoryInfo(folderPath);
            var matches = dir.GetFiles(matchPattern);

            foreach (var match in matches)
            {
                var foundFileKey = Path.GetFileNameWithoutExtension(match.Name);
                await loginCommands.DeleteAsync(
                    projectId,
                    foundFileKey,
                    cancellationToken).ConfigureAwait(false);

            }

        }



        #endregion

        #region UserLocation
        
        public async Task AddUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            
            if (userLocation == null) throw new ArgumentException("userLocation must not be null");
            if (userLocation.RowId == Guid.Empty) throw new ArgumentException("Id must not be empty guid");
            if (userLocation.UserGuid == Guid.Empty) throw new ArgumentException("UserId must not be empty guid");
            if (userLocation.SiteGuid == Guid.Empty) throw new ArgumentException("SiteId must not be empty guid");

            await EnsureProjectId().ConfigureAwait(false);

            var ul = UserLocation.FromIUserLocation(userLocation);
           
            await locationCommands.CreateAsync(
                projectId,
                ul.RowId.ToString(),
                ul,
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task UpdateUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (userLocation == null) throw new ArgumentException("userLocation must not be null");
            if (userLocation.RowId == Guid.Empty) throw new ArgumentException("Id must not be empty guid");
            if (userLocation.UserGuid == Guid.Empty) throw new ArgumentException("UserId must not be empty guid");
            if (userLocation.SiteGuid == Guid.Empty) throw new ArgumentException("SiteId must not be empty guid");
            
            await EnsureProjectId().ConfigureAwait(false);

            var ul = UserLocation.FromIUserLocation(userLocation);

            await locationCommands.UpdateAsync(
                    projectId,
                    ul.RowId.ToString(),
                    ul,
                    cancellationToken).ConfigureAwait(false);
            
        }

        public async Task DeleteUserLocation(
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
           
            await EnsureProjectId().ConfigureAwait(false);

            await locationCommands.DeleteAsync(
                projectId,
                id.ToString(),
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task DeleteUserLocationsByUser(
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (userId == Guid.Empty) throw new ArgumentException("userId must not be empty guid");
            
            await EnsureProjectId().ConfigureAwait(false);

            var all = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x => x.UserGuid == userId);
            foreach (var loc in filtered)
            {
                await locationCommands.DeleteAsync(
                    projectId,
                    loc.RowId.ToString(),
                    cancellationToken).ConfigureAwait(false);
                
            }
            
        }

        public async Task DeleteUserLocationsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId == Guid.Empty) throw new ArgumentException("userId must not be empty guid");
            
            await EnsureProjectId().ConfigureAwait(false);

            var all = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x => x.SiteGuid == siteId);
            foreach (var loc in filtered)
            {
                await locationCommands.DeleteAsync(
                    projectId,
                    loc.RowId.ToString(),
                    cancellationToken).ConfigureAwait(false);
            }

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
