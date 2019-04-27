// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-26
// Last Modified:           2019-04-27
// 

using cloudscribe.Core.Models;
using NoDb;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    public class UserCommands : IUserCommands, IUserCommandsSingleton
    {
        public UserCommands(
            //IProjectResolver projectResolver,
            IBasicCommands<SiteUser> userCommands,
            IBasicCommands<SiteRole> roleCommands,
            IBasicCommands<UserRole> userRoleCommands,
            IBasicCommands<UserClaim> claimCommands,
            IBasicCommands<UserLogin> loginCommands,
            IBasicCommands<UserToken> tokenCommands,
            IBasicCommands<UserLocation> locationCommands,
            IBasicQueries<SiteUser> userQueries,
            IBasicQueries<SiteRole> roleQueries,
            IBasicQueries<UserRole> userRoleQueries,
            IBasicQueries<UserClaim> claimQueries,
            IBasicQueries<UserToken> tokenQueries,
            IBasicQueries<UserLocation> locationQueries,
            IStoragePathResolver<UserLogin> loginPathResolver,
            IStoragePathResolver<UserToken> tokenPathResolver
            )
        {
            
            _userCommands = userCommands;
            _roleCommands = roleCommands;
            _userRoleCommands = userRoleCommands;
            _claimCommands = claimCommands;
            _loginCommands = loginCommands;
            _locationCommands = locationCommands;
            _loginPathResolver = loginPathResolver;

            _userQueries = userQueries;
            _roleQueries = roleQueries;
            _userRoleQueries = userRoleQueries;
            _claimQueries = claimQueries;
            _locationQueries = locationQueries;
            _tokenQueries = tokenQueries;

            _tokenCommands = tokenCommands;
            _tokenPathResolver = tokenPathResolver;

        }

        //private IProjectResolver projectResolver;
        private IBasicCommands<SiteUser> _userCommands;
        private IBasicCommands<SiteRole> _roleCommands;
        private IBasicCommands<UserRole> _userRoleCommands;
        private IBasicCommands<UserClaim> _claimCommands;
        private IBasicCommands<UserLogin> _loginCommands;
        private IBasicCommands<UserLocation> _locationCommands;
        private IStoragePathResolver<UserLogin> _loginPathResolver;
        private IBasicCommands<UserToken> _tokenCommands;
        private IStoragePathResolver<UserToken> _tokenPathResolver;

        private IBasicQueries<SiteUser> _userQueries;
        private IBasicQueries<SiteRole> _roleQueries;
        private IBasicQueries<UserRole> _userRoleQueries;
        private IBasicQueries<UserClaim> _claimQueries;
        private IBasicQueries<UserLocation> _locationQueries;
        private readonly IBasicQueries<UserToken> _tokenQueries;

        //protected string projectId;

        //private async Task EnsureProjectId()
        //{
        //    if (string.IsNullOrEmpty(projectId))
        //    {
        //        projectId = await projectResolver.ResolveProjectId().ConfigureAwait(false);
        //    }

        //}

        #region User

        public async Task Create(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user == null) { throw new ArgumentException("user cannot be null"); }
            if (user.SiteId == Guid.Empty) { throw new ArgumentException("user must have a siteid"); }
            if (user.Id == Guid.Empty) { throw new ArgumentException("user must have a non-empty id"); }

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = user.SiteId.ToString();

            var siteUser = SiteUser.FromISiteUser(user);
            
            await _userCommands.CreateAsync(
                projectId,
                siteUser.Id.ToString(),
                siteUser,
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task Update(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user == null) { throw new ArgumentException("user cannot be null"); }
            if (user.SiteId == Guid.Empty) { throw new ArgumentException("user must have a siteid"); }
            if (user.Id == Guid.Empty) { throw new ArgumentException("user must have a non-empty id"); }

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = user.SiteId.ToString();

            var siteUser = SiteUser.FromISiteUser(user);
            await _userCommands.UpdateAsync(
                    projectId,
                    siteUser.Id.ToString(),
                    siteUser,
                    cancellationToken).ConfigureAwait(false);

        }

        public async Task Delete(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            var projectId = siteId.ToString();

            await _userCommands.DeleteAsync(projectId, userId.ToString(), cancellationToken).ConfigureAwait(false);
            
            // try catch here is because NoDb throws an error if the folder doesn't exist and it may not exist 
            // for these if there are no social logins for any user, or claims, or tokens
            try
            {
                await DeleteUserRoles(siteId, userId);
            }
            catch { }


            try
            {
                await DeleteClaimsByUser(siteId, userId);
            }
            catch { }
            try
            {
                await DeleteLoginsByUser(siteId, userId);
            }
            catch { }
            try
            {
                await DeleteTokensByUser(siteId, userId);
            }
            catch { }
           
            
            

        }

        public async Task DeleteUsersBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var projectId = siteId.ToString();

            await DeleteLoginsBySite(siteId);
            await DeleteClaimsBySite(siteId);
            await DeleteUserRolesBySite(siteId);
            await DeleteTokensBySite(siteId);

            var all = await _userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var users = all.ToList().AsQueryable();

            var query = from x in users.Where(x => x.SiteId == siteId)
                        select x;

            foreach(var u in query)
            {
                await _userCommands.DeleteAsync(
                    projectId,
                    u.Id.ToString(),
                    cancellationToken).ConfigureAwait(false);
            }

        }

        //public async Task FlagAsDeleted(
        //    Guid siteId,
        //    Guid userId,
        //    CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    ThrowIfDisposed();
        //    cancellationToken.ThrowIfCancellationRequested();

        //    //await EnsureProjectId().ConfigureAwait(false);
        //    var projectId = siteId.ToString();

        //    var item
        //        = await userQueries.FetchAsync(
        //            projectId,
        //            userId.ToString(),
        //            cancellationToken).ConfigureAwait(false);

        //    if (item == null) { throw new InvalidOperationException("user not found"); }

        //    item.IsDeleted = true;

        //    await userCommands.UpdateAsync(
        //            projectId,
        //            item.Id.ToString(),
        //            item,
        //            cancellationToken).ConfigureAwait(false);
            
        //}

        //public async Task FlagAsNotDeleted(
        //    Guid siteId,
        //    Guid userId,
        //    CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    ThrowIfDisposed();
        //    cancellationToken.ThrowIfCancellationRequested();

        //    //await EnsureProjectId().ConfigureAwait(false);
        //    var projectId = siteId.ToString();

        //    var item
        //        = await userQueries.FetchAsync(
        //            projectId,
        //            userId.ToString(),
        //            cancellationToken).ConfigureAwait(false);

        //    if (item == null) { throw new InvalidOperationException("user not found"); }

        //    item.IsDeleted = false;

        //    await userCommands.UpdateAsync(
        //            projectId,
        //            item.Id.ToString(),
        //            item,
        //            cancellationToken).ConfigureAwait(false);

        //}

        public async Task LockoutAccount(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var item
                = await _userQueries.FetchAsync(
                    projectId,
                    userId.ToString(),
                    cancellationToken).ConfigureAwait(false);

            if (item == null) { throw new InvalidOperationException("user not found"); }

            item.IsLockedOut = true;

            await _userCommands.UpdateAsync(
                    projectId,
                    item.Id.ToString(),
                    item,
                    cancellationToken).ConfigureAwait(false);

        }

        public async Task UnLockAccount(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var item
                = await _userQueries.FetchAsync(
                    projectId,
                    userId.ToString(),
                    cancellationToken).ConfigureAwait(false);

            if (item == null) { throw new InvalidOperationException("user not found"); }

            item.IsLockedOut = true;
            item.AccessFailedCount = 0;

            await _userCommands.UpdateAsync(
                    projectId,
                    item.Id.ToString(),
                    item,
                    cancellationToken).ConfigureAwait(false);

        }

        public async Task UpdateFailedPasswordAttemptCount(
            Guid siteId,
            Guid userId,
            int failedPasswordAttemptCount,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var item
                = await _userQueries.FetchAsync(
                    projectId,
                    userId.ToString(),
                    cancellationToken).ConfigureAwait(false);

            if (item == null) { throw new InvalidOperationException("user not found"); }

            item.AccessFailedCount = failedPasswordAttemptCount;

            await _userCommands.UpdateAsync(
                    projectId,
                    item.Id.ToString(),
                    item,
                    cancellationToken).ConfigureAwait(false);

        }

        public async Task UpdateLastLoginTime(
            Guid siteId,
            Guid userId,
            DateTime lastLoginTime,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var item
                = await _userQueries.FetchAsync(
                    projectId,
                    userId.ToString(),
                    cancellationToken).ConfigureAwait(false);

            if (item == null) { throw new InvalidOperationException("user not found"); }

            item.LastLoginUtc = lastLoginTime;

            await _userCommands.UpdateAsync(
                    projectId,
                    item.Id.ToString(),
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
            if (role.SiteId == Guid.Empty) { throw new ArgumentException("SiteId must be provided"); }
            if (role.Id == Guid.Empty) { throw new ArgumentException("Id must be provided"); }

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = role.SiteId.ToString();

            var siteRole = SiteRole.FromISiteRole(role);
            
            if (siteRole.NormalizedRoleName.Length == 0)
            {
                siteRole.NormalizedRoleName = siteRole.RoleName;
            }
            await _roleCommands.CreateAsync(
                projectId,
                siteRole.Id.ToString(),
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
            if (role.SiteId == Guid.Empty) { throw new ArgumentException("SiteId must be provided"); }
            if (role.Id == Guid.Empty) { throw new ArgumentException("Id must be provided"); }

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = role.SiteId.ToString();

            var siteRole = SiteRole.FromISiteRole(role);

            await _roleCommands.UpdateAsync(
                    projectId,
                    siteRole.Id.ToString(),
                    siteRole,
                    cancellationToken).ConfigureAwait(false);
            
        }

        public async Task DeleteRole(
            Guid siteId,
            Guid roleId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (roleId == Guid.Empty) { throw new ArgumentException("Id must be provided"); }

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            await _roleCommands.DeleteAsync(
                projectId,
                roleId.ToString(),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteRolesBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId == Guid.Empty) { throw new ArgumentException("siteId must be provided"); }

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var all = await _roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x => x.SiteId == siteId);

            foreach (var item in filtered)
            {
                await _roleCommands.DeleteAsync(
                    projectId,
                    item.Id.ToString(),
                    cancellationToken).ConfigureAwait(false);

            }

            
        }

        public async Task AddUserToRole(
            Guid siteId,
            Guid roleId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (roleId == Guid.Empty) { throw new ArgumentException("roleId must be provided"); }
            if (userId == Guid.Empty) { throw new ArgumentException("userId must be provided"); }

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            UserRole ur = new UserRole();
            ur.RoleId = roleId;
            ur.UserId = userId;

            var key = userId.ToString() + "~" + roleId.ToString();

            await _userRoleCommands.CreateAsync(
                projectId,
                key,
                ur,
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task RemoveUserFromRole(
            Guid siteId,
            Guid roleId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (roleId == Guid.Empty) { throw new ArgumentException("roleId must be provided"); }
            if (userId == Guid.Empty) { throw new ArgumentException("userId must be provided"); }

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var key = userId.ToString() + "~" + roleId.ToString();

            await _userRoleCommands.DeleteAsync(
                projectId,
                key,
                cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteUserRoles(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (userId == Guid.Empty) { throw new ArgumentException("userId must be provided"); }

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var all = await _userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(
                x => x.UserId == userId
            );

            foreach (var item in filtered)
            {
                var key = item.UserId.ToString() + "~" + item.RoleId.ToString();

                await _roleCommands.DeleteAsync(
                    projectId,
                    key,
                    cancellationToken).ConfigureAwait(false);
  
            }

        }

        public async Task DeleteUserRolesByRole(
            Guid siteId,
            Guid roleId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (roleId == Guid.Empty) { throw new ArgumentException("roleId must be provided"); }

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var all = await _userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(
                x => x.RoleId == roleId
            );

            foreach (var item in filtered)
            {
                var key = item.UserId.ToString() + "~" + item.RoleId.ToString();

                await _roleCommands.DeleteAsync(
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

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allRoles = await _roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var siteRoles = allRoles.Where(
                x => x.SiteId == siteId
            );

            var allUserRoles = await _userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in allUserRoles
                        join y in siteRoles on x.RoleId equals y.Id
                        select x;

            foreach (var item in query)
            {
                var key = item.UserId.ToString() + "~" + item.RoleId.ToString();

                await _roleCommands.DeleteAsync(
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
            if (userClaim.SiteId == Guid.Empty) { throw new ArgumentException("userClaim must have n SiteId"); }
            if (userClaim.UserId == Guid.Empty) { throw new ArgumentException("userClaim must have a UserId"); }

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = userClaim.SiteId.ToString();

            var claim = UserClaim.FromIUserClaim(userClaim);
            
            await _claimCommands.CreateAsync(
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
            if (userClaim.SiteId == Guid.Empty) { throw new ArgumentException("userClaim must have n SiteId"); }
            if (userClaim.UserId == Guid.Empty) { throw new ArgumentException("userClaim must have a UserId"); }

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = userClaim.SiteId.ToString();

            var claim = UserClaim.FromIUserClaim(userClaim);
            await _claimCommands.UpdateAsync(
                    projectId,
                    claim.Id.ToString(),
                    claim,
                    cancellationToken).ConfigureAwait(false);

        }

        public async Task DeleteClaim(
            Guid siteId,
            Guid claimId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (claimId == Guid.Empty) { throw new ArgumentException("id must be provided"); }

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            await _claimCommands.DeleteAsync(
                projectId,
                claimId.ToString(),
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

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var all = await _claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(
                x => x.UserId == userId
                && x.SiteId == siteId
            );

            foreach (var item in filtered)
            {
                await _claimCommands.DeleteAsync(
                    projectId,
                    item.Id.ToString(),
                    cancellationToken).ConfigureAwait(false);   
            }
            
        }

        public async Task DeleteClaimByUser(
            Guid siteId,
            Guid userId,
            string claimType,
            string claimValue,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId == Guid.Empty) { throw new ArgumentException("SiteId must be provided"); }
            if (userId == Guid.Empty) { throw new ArgumentException("UserId must be provided"); }
            if (string.IsNullOrWhiteSpace(claimType)) throw new ArgumentException("claimType must be provided");

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var all = await _claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(
                x => x.UserId == userId
                && x.SiteId == siteId
                && x.ClaimType == claimType
                && x.ClaimValue == claimValue
            );

            foreach (var item in filtered)
            {
                await _claimCommands.DeleteAsync(
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

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var all = await _claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x =>
                 x.SiteId == siteId
            );

            foreach (var item in filtered)
            {
                await _claimCommands.DeleteAsync(
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
            if (userLogin.UserId == Guid.Empty) throw new ArgumentException("UserId must be provided");
            if (userLogin.SiteId == Guid.Empty) throw new ArgumentException("SiteId must be provided");

            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = userLogin.SiteId.ToString();

            var login = UserLogin.FromIUserLogin(userLogin);

            // this will be a tricky one for queries because the key consists of 4 columns
            // TODO: review this and whether we really need all the  parts of the key in EF
            // http://www.jerriepelser.com/blog/using-aspnet-oauth-providers-without-identity
            // ProviderKey is the unique key associated with the login on that service
            var key = login.UserId.ToString()
                + "~" + login.SiteId.ToString()
                + "~" + login.LoginProvider
                + "~" + login.ProviderKey;

            await _loginCommands.CreateAsync(
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

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var key = userId.ToString()
                + "~" + siteId.ToString()
                + "~" + loginProvider
                + "~" + providerKey;

            await _loginCommands.DeleteAsync(
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

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var folderPath = await _loginPathResolver.ResolvePath(projectId).ConfigureAwait(false);

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
                await _loginCommands.DeleteAsync(
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

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var folderPath = await _loginPathResolver.ResolvePath(projectId).ConfigureAwait(false);

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
                await _loginCommands.DeleteAsync(
                    projectId,
                    foundFileKey,
                    cancellationToken).ConfigureAwait(false);

            }

        }



        #endregion

        #region UserToken

        public async Task CreateToken(
            IUserToken userToken,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (userToken == null) { throw new ArgumentException("userToken can't be null"); }
            if (userToken.LoginProvider.Length == -1) { throw new ArgumentException("userToken must have a loginprovider"); }
            if (userToken.Name.Length == -1) { throw new ArgumentException("userToken must have a Name"); }
            if (userToken.UserId == Guid.Empty) { throw new ArgumentException("userToken must have a user id"); }

            var projectId = userToken.SiteId.ToString();

            var token = UserToken.FromIUserToken(userToken);

            // this will be a tricky one for queries because the key consists of 4 columns
            // TODO: review this and whether we really need all the  parts of the key in EF
            // http://www.jerriepelser.com/blog/using-aspnet-oauth-providers-without-identity
            // ProviderKey is the unique key associated with the login on that service
            var key = token.UserId.ToString()
                + "~" + token.SiteId.ToString()
                + "~" + token.LoginProvider
                + "~" + token.Name;

            await _tokenCommands.CreateAsync(
                projectId,
                key,
                token,
                cancellationToken).ConfigureAwait(false);


        }

        public async Task UpdateToken(
            IUserToken userToken,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (userToken == null) { throw new ArgumentException("userToken can't be null"); }
            if (userToken.LoginProvider.Length == -1) { throw new ArgumentException("userToken must have a loginprovider"); }
            if (userToken.Name.Length == -1) { throw new ArgumentException("userToken must have a Name"); }
            if (userToken.UserId == Guid.Empty) { throw new ArgumentException("userToken must have a user id"); }

            var projectId = userToken.SiteId.ToString();

            var token = UserToken.FromIUserToken(userToken);

            // this will be a tricky one for queries because the key consists of 4 columns
            // TODO: review this and whether we really need all the  parts of the key in EF
            // http://www.jerriepelser.com/blog/using-aspnet-oauth-providers-without-identity
            // ProviderKey is the unique key associated with the login on that service
            var key = token.UserId.ToString()
                + "~" + token.SiteId.ToString()
                + "~" + token.LoginProvider
                + "~" + token.Name;

            await _tokenCommands.UpdateAsync(
                projectId,
                key,
                token,
                cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteToken(
            Guid siteId,
            Guid userId,
            string loginProvider,
            string name,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            
            var projectId = siteId.ToString();

            var key = userId.ToString()
                + "~" + siteId.ToString()
                + "~" + loginProvider
                + "~" + name;

            await _tokenCommands.DeleteAsync(
                projectId,
                key,
                cancellationToken).ConfigureAwait(false);

        }

        public async Task DeleteTokensByProvider(
            Guid siteId,
            Guid userId,
            string loginProvider,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var projectId = siteId.ToString();

            var all = await _tokenQueries.GetAllAsync(projectId);

            var matching = all.Where(x => x.SiteId == siteId && x.UserId == userId && x.LoginProvider == loginProvider);

            foreach(var t in matching)
            {
                var key = t.UserId.ToString()
               + "~" + t.SiteId.ToString()
               + "~" + t.LoginProvider
               + "~" + t.Name;

                await _tokenCommands.DeleteAsync(
                    projectId,
                    key,
                    cancellationToken).ConfigureAwait(false);
            }

            

        }

        public async Task DeleteTokensBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId == Guid.Empty) throw new ArgumentException("siteId must be provided");

            var projectId = siteId.ToString();

            var folderPath = await _tokenPathResolver.ResolvePath(projectId).ConfigureAwait(false);

            // understand structure of key which is the filename
            //var key = login.UserGuid.ToString()
            //    + "~" + login.SiteGuid.ToString()
            //    + "~" + login.LoginProvider
            //    + "~" + login.Name;

            var matchPattern = "*~" + siteId.ToString() + "~*";

            var dir = new DirectoryInfo(folderPath);
            var matches = dir.GetFiles(matchPattern);

            foreach (var match in matches)
            {
                var foundFileKey = Path.GetFileNameWithoutExtension(match.Name);
                await _tokenCommands.DeleteAsync(
                    projectId,
                    foundFileKey,
                    cancellationToken).ConfigureAwait(false);

            }
        }

        public async Task DeleteTokensByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId == Guid.Empty) throw new ArgumentException("siteId must be provided");
            if (userId == Guid.Empty) throw new ArgumentException("userId must be provided");

            var projectId = siteId.ToString();

            var folderPath = await _tokenPathResolver.ResolvePath(projectId).ConfigureAwait(false);

            // understand structure of key which is the filename
            //var key = login.UserGuid.ToString()
            //    + "~" + login.SiteGuid.ToString()
            //    + "~" + login.LoginProvider
            //    + "~" + login.Name;

            var matchPattern = userId.ToString() +
                "~" + siteId.ToString()
                + "~*";

            var dir = new DirectoryInfo(folderPath);
            var matches = dir.GetFiles(matchPattern);

            foreach (var match in matches)
            {
                var foundFileKey = Path.GetFileNameWithoutExtension(match.Name);
                await _tokenCommands.DeleteAsync(
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
            if (userLocation.Id == Guid.Empty) throw new ArgumentException("Id must not be empty guid");
            if (userLocation.UserId == Guid.Empty) throw new ArgumentException("UserId must not be empty guid");
            if (userLocation.SiteId == Guid.Empty) throw new ArgumentException("SiteId must not be empty guid");

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = userLocation.SiteId.ToString();

            var ul = UserLocation.FromIUserLocation(userLocation);
           
            await _locationCommands.CreateAsync(
                projectId,
                ul.Id.ToString(),
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
            if (userLocation.Id == Guid.Empty) throw new ArgumentException("Id must not be empty guid");
            if (userLocation.UserId == Guid.Empty) throw new ArgumentException("UserId must not be empty guid");
            if (userLocation.SiteId == Guid.Empty) throw new ArgumentException("SiteId must not be empty guid");

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = userLocation.SiteId.ToString();

            var ul = UserLocation.FromIUserLocation(userLocation);

            await _locationCommands.UpdateAsync(
                    projectId,
                    ul.Id.ToString(),
                    ul,
                    cancellationToken).ConfigureAwait(false);
            
        }

        public async Task DeleteUserLocation(
            Guid siteId,
            Guid userLocationId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            await _locationCommands.DeleteAsync(
                projectId,
                userLocationId.ToString(),
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task DeleteUserLocationsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (userId == Guid.Empty) throw new ArgumentException("userId must not be empty guid");

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var all = await _locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x => x.UserId == userId);
            foreach (var loc in filtered)
            {
                await _locationCommands.DeleteAsync(
                    projectId,
                    loc.Id.ToString(),
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

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var all = await _locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x => x.SiteId == siteId);
            foreach (var loc in filtered)
            {
                await _locationCommands.DeleteAsync(
                    projectId,
                    loc.Id.ToString(),
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
