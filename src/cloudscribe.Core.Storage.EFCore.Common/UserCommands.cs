// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2019-04-27
// 


using cloudscribe.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public class UserCommands : IUserCommands, IUserCommandsSingleton
    {
        public UserCommands(ICoreDbContextFactory coreDbContextFactory)
        {
            _contextFactory = coreDbContextFactory; 
        }

        private readonly ICoreDbContextFactory _contextFactory;

        

        #region User

        public async Task Create(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) { throw new ArgumentException("user can't be null"); }
            if (user.SiteId == Guid.Empty) { throw new ArgumentException("user must have a siteid"); }
            if (user.Id == Guid.Empty) { throw new ArgumentException("user must have a non empty guid for id"); }

            SiteUser siteUser = SiteUser.FromISiteUser(user);

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.Users.Add(siteUser);

                int rowsAffected =
                    await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false)
                    ;
            }
            
        }

        public async Task Update(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentException("user can't be null"); }
            if (user.SiteId == Guid.Empty) { throw new ArgumentException("user must have a siteguid"); }
            if (user.Id == Guid.Empty) { throw new ArgumentException("user must have a non empty guid for id"); }

            SiteUser siteUser = SiteUser.FromISiteUser(user);

            using (var dbContext = _contextFactory.CreateContext())
            {
                bool tracking = dbContext.ChangeTracker.Entries<SiteUser>().Any(x => x.Entity.Id == siteUser.Id);
                if (!tracking)
                {
                    dbContext.Users.Update(siteUser);
                }
                else
                {
                    var tracked = dbContext.ChangeTracker.Entries<SiteUser>().FirstOrDefault(x => x.Entity.Id == siteUser.Id);
                    var s = tracked.State;
                    if (s == EntityState.Unchanged)
                    {
                        tracked.State = EntityState.Detached;
                        try
                        {
                            dbContext.Users.Update(siteUser);
                        }
                        catch (Exception)
                        { }
                    }
                }

                int rowsAffected =
                    await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false)
                    ;
            }

        }

       

        public async Task Delete(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var itemToRemove = await dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId && x.SiteId == siteId, cancellationToken)
                .ConfigureAwait(false);

                if (itemToRemove != null)
                {
                    DeleteLoginsByUser(itemToRemove.SiteId, itemToRemove.Id, dbContext);
                    DeleteClaimsByUser(itemToRemove.SiteId, itemToRemove.Id, dbContext);
                    DeleteUserRoles(siteId, itemToRemove.Id, dbContext);
                    DeleteTokensByUser(itemToRemove.SiteId, itemToRemove.Id, dbContext);
                    
                    dbContext.Users.Remove(itemToRemove);
                    int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);
                }
            }
  
        }

        public async Task DeleteUsersBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            
            cancellationToken.ThrowIfCancellationRequested();
            
            await DeleteLoginsBySite(siteId);
            await DeleteClaimsBySite(siteId);
            await DeleteUserRolesBySite(siteId);
            await DeleteTokensBySite(siteId);

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.Users.Where(x => x.SiteId == siteId)
                            select x;

                dbContext.Users.RemoveRange(query);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        
        public async Task LockoutAccount(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var item = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.Id == userId
                    , cancellationToken)
                    .ConfigureAwait(false);

                if (item == null) { throw new InvalidOperationException("user not found"); }

                item.IsLockedOut = true;

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task UnLockAccount(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var item = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.Id == userId,
                    cancellationToken)
                    .ConfigureAwait(false);

                if (item == null) { throw new InvalidOperationException("user not found"); }

                item.IsLockedOut = false;
                item.AccessFailedCount = 0;

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task UpdateFailedPasswordAttemptCount(
            Guid siteId,
            Guid userId,
            int failedPasswordAttemptCount,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var item = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.Id == userId,
                    cancellationToken)
                    .ConfigureAwait(false);

                if (item == null) { throw new InvalidOperationException("user not found"); }

                item.AccessFailedCount = failedPasswordAttemptCount;

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task UpdateLastLoginTime(
            Guid siteId,
            Guid userId,
            DateTime lastLoginTime,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var item = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.Id == userId,
                    cancellationToken)
                    .ConfigureAwait(false);

                if (item == null) { throw new InvalidOperationException("user not found"); }

                item.LastLoginUtc = lastLoginTime;

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }



        #endregion

        #region Roles

        /// <summary>
        /// Persists a new instance of Role. Returns true on success.
        /// when a role is created displayname corresponds to rolename
        /// but rolename can never change since it is used in a cookies and coded
        /// into security checks in some cases
        /// so subsequent changes to rolename really only effect displayname
        /// ie for localization or customization
        /// to really change a rolename you can delete the role and create a new one with the desired name
        /// some specific required system roles (Admin, Content Administrators) 
        /// are also not allowed to be deleted
        /// </summary>
        /// <returns></returns>
        public async Task CreateRole(
            ISiteRole role,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null) { throw new ArgumentException("role cannot be null"); }
            if (role.SiteId == Guid.Empty) { throw new ArgumentException("SiteId must be provided"); }
            if (role.Id == Guid.Empty) { throw new ArgumentException("Id must be provided"); }
            
            var siteRole = SiteRole.FromISiteRole(role);
            
            if (siteRole.NormalizedRoleName.Length == 0)
            {
                siteRole.NormalizedRoleName = siteRole.RoleName;
            }

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.Roles.Add(siteRole);

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task UpdateRole(
            ISiteRole role,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) { throw new ArgumentException("role cannot be null"); }
            if (role.SiteId == Guid.Empty) { throw new ArgumentException("SiteId must be provided"); }
            if (role.Id == Guid.Empty) { throw new ArgumentException("Id must be provided"); }

            var siteRole = SiteRole.FromISiteRole(role);

            using (var dbContext = _contextFactory.CreateContext())
            {
                bool tracking = dbContext.ChangeTracker.Entries<SiteRole>().Any(x => x.Entity.Id == siteRole.Id);
                if (!tracking)
                {
                    dbContext.Roles.Update(siteRole);
                }

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
 
        }

        public async Task DeleteRole(
            Guid siteId,
            Guid roleId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var itemToRemove = await dbContext.Roles.SingleOrDefaultAsync(
                x => x.Id == roleId,
                cancellationToken)
                .ConfigureAwait(false);

                if (itemToRemove == null) { throw new InvalidOperationException("role not found"); }

                dbContext.Roles.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task DeleteRolesBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from r in dbContext.Roles.Where(x => x.SiteId == siteId)
                            select r;

                dbContext.Roles.RemoveRange(query);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task AddUserToRole(
            Guid siteId,
            Guid roleId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var ur = new UserRole();
            ur.RoleId = roleId;
            ur.UserId = userId;

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.UserRoles.Add(ur);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task RemoveUserFromRole(
            Guid siteId,
            Guid roleId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var itemToRemove = await dbContext.UserRoles.SingleOrDefaultAsync(
                x => x.UserId == userId && x.RoleId == roleId
                , cancellationToken)
                .ConfigureAwait(false);

                if (itemToRemove == null) { throw new InvalidOperationException("userrole not found"); }

                dbContext.UserRoles.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task DeleteUserRoles(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var db = _contextFactory.CreateContext())
            {
                DeleteUserRoles(siteId, userId, db);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
                
        }

        private void DeleteUserRoles(
            Guid siteId,
            Guid userId,
            ICoreDbContext dbContext)
        {
            var query = from x in dbContext.UserRoles
                        where x.UserId == userId
                        select x;

            dbContext.UserRoles.RemoveRange(query);
               
        }

        //private async Task DeleteUserRoles(
        //    Guid siteId,
        //    Guid userId,
        //    bool saveChanges,
        //    CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    using (var dbContext = _contextFactory.CreateContext())
        //    {
        //        var query = from x in dbContext.UserRoles
        //                    where x.UserId == userId
        //                    select x;

        //        dbContext.UserRoles.RemoveRange(query);
        //        if (saveChanges)
        //        {
        //            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
        //                .ConfigureAwait(false);


        //        }
        //    }

        //}

        public async Task DeleteUserRolesByRole(
            Guid siteId,
            Guid roleId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.UserRoles
                            where x.RoleId == roleId
                            select x;

                dbContext.UserRoles.RemoveRange(query);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task DeleteUserRolesBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.UserRoles
                            join y in dbContext.Roles on x.RoleId equals y.Id
                            where y.SiteId == siteId
                            select x;

                dbContext.UserRoles.RemoveRange(query);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }


        #endregion

        #region Claims

        public async Task CreateClaim(
            IUserClaim userClaim,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userClaim == null) { throw new ArgumentException("userClaim can't be null"); }
            
            var claim = UserClaim.FromIUserClaim(userClaim);
            if (claim.Id == Guid.Empty) throw new ArgumentException("userClaim must have a non empty id");

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.UserClaims.Add(claim);

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task UpdateClaim(
            IUserClaim userClaim,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userClaim == null) { throw new ArgumentException("userClaim can't be null"); }

            var claim = UserClaim.FromIUserClaim(userClaim);
            if (claim.Id == Guid.Empty) throw new ArgumentException("userClaim must have a non empty id");

            using (var dbContext = _contextFactory.CreateContext())
            {
                bool tracking = dbContext.ChangeTracker.Entries<UserClaim>().Any(x => x.Entity.Id == claim.Id);
                if (!tracking)
                {
                    dbContext.UserClaims.Update(claim);
                }

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task DeleteClaim(
            Guid siteId,
            Guid claimId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var itemToRemove = await dbContext.UserClaims.SingleOrDefaultAsync(x => x.Id == claimId, cancellationToken);
                if (itemToRemove == null) { throw new InvalidOperationException("claim not found"); }

                dbContext.UserClaims.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task DeleteClaimsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var db = _contextFactory.CreateContext())
            {
                DeleteClaimsByUser(siteId, userId, db);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
            
        }

        private void DeleteClaimsByUser(
            Guid siteId,
            Guid userId,
            ICoreDbContext dbContext)
        {
            
            var query = from x in dbContext.UserClaims
                        where (
                        (siteId == Guid.Empty || x.SiteId == siteId)
                        && x.UserId == userId
                        )
                        select x;

            dbContext.UserClaims.RemoveRange(query);
               
        }

        public async Task DeleteClaimByUser(
            Guid siteId,
            Guid userId,
            string claimType,
            string claimValue,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.UserClaims
                            where (
                            (siteId == Guid.Empty || x.SiteId == siteId)
                            && (x.UserId == userId && x.ClaimType == claimType && x.ClaimValue == claimValue)
                            )
                            select x;

                dbContext.UserClaims.RemoveRange(query);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task DeleteClaimsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.UserClaims
                            where x.SiteId == siteId
                            select x;

                dbContext.UserClaims.RemoveRange(query);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        #endregion

        #region Logins

        public async Task CreateLogin(
            IUserLogin userLogin,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userLogin == null) { throw new ArgumentException("userLogin can't be null"); }
            if (userLogin.LoginProvider.Length == -1) { throw new ArgumentException("userLogin must have a loginprovider"); }
            if (userLogin.ProviderKey.Length == -1) { throw new ArgumentException("userLogin must have a providerkey"); }
            if (userLogin.UserId == Guid.Empty) { throw new ArgumentException("userLogin must have a user id"); }

            var login = UserLogin.FromIUserLogin(userLogin);

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.UserLogins.Add(login);

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }
        
        public async Task DeleteLogin(
            Guid siteId,
            Guid userId,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from l in dbContext.UserLogins
                            where (
                            l.SiteId == siteId
                            && l.UserId == userId
                            && l.LoginProvider == loginProvider
                            && l.ProviderKey == providerKey
                            )
                            select l;

                dbContext.UserLogins.RemoveRange(query);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
 
        }

        public async Task DeleteLoginsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var db = _contextFactory.CreateContext())
            {
                DeleteLoginsByUser(siteId, userId, db);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
            
        }

        private void DeleteLoginsByUser(
            Guid siteId,
            Guid userId,
            ICoreDbContext dbContext)
        {
            
            var query = from l in dbContext.UserLogins
                        where (
                        l.SiteId == siteId
                        && l.UserId == userId
                        )
                        select l;

            dbContext.UserLogins.RemoveRange(query);
                
        }

        public async Task DeleteLoginsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from l in dbContext.UserLogins
                            where (l.SiteId == siteId)
                            select l;

                dbContext.UserLogins.RemoveRange(query);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        #endregion

        #region UserToken

        public async Task CreateToken(
            IUserToken userToken,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userToken == null) { throw new ArgumentException("userToken can't be null"); }
            if (userToken.LoginProvider.Length == -1) { throw new ArgumentException("userToken must have a loginprovider"); }
            if (userToken.Name.Length == -1) { throw new ArgumentException("userToken must have a Name"); }
            if (userToken.UserId == Guid.Empty) { throw new ArgumentException("userToken must have a user id"); }

            var token = UserToken.FromIUserToken(userToken);

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.UserTokens.Add(token);

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task UpdateToken(
            IUserToken userToken,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userToken == null) { throw new ArgumentException("userToken can't be null"); }
            if (userToken.LoginProvider.Length == -1) { throw new ArgumentException("userToken must have a loginprovider"); }
            if (userToken.Name.Length == -1) { throw new ArgumentException("userToken must have a Name"); }
            if (userToken.UserId == Guid.Empty) { throw new ArgumentException("userToken must have a user id"); }

            var token = UserToken.FromIUserToken(userToken);

            using (var dbContext = _contextFactory.CreateContext())
            {
                bool tracking = dbContext.ChangeTracker.Entries<UserToken>().Any(x =>
                    x.Entity.SiteId == token.SiteId
                    && x.Entity.UserId == token.UserId
                    && x.Entity.LoginProvider == token.LoginProvider
                    && x.Entity.Name == token.Name
                    );

                    if (!tracking)
                    {
                        dbContext.UserTokens.Update(token);
                    }

                    int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);
            }
 
        }

        public async Task DeleteTokensByProvider(
            Guid siteId,
            Guid userId,
            string loginProvider,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from l in dbContext.UserTokens
                            where (
                            l.SiteId == siteId
                            && l.UserId == userId
                            && l.LoginProvider == loginProvider
                            )
                            select l;

                dbContext.UserTokens.RemoveRange(query);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task DeleteToken(
            Guid siteId,
            Guid userId,
            string loginProvider,
            string name,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from l in dbContext.UserTokens
                            where (
                            l.SiteId == siteId
                            && l.UserId == userId
                            && l.LoginProvider == loginProvider
                            && l.Name == name
                            )
                            select l;

                dbContext.UserTokens.RemoveRange(query);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task DeleteTokensBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from l in dbContext.UserTokens
                            where (l.SiteId == siteId)
                            select l;

                dbContext.UserTokens.RemoveRange(query);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task DeleteTokensByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var db = _contextFactory.CreateContext())
            {
                DeleteTokensByUser(siteId, userId, db);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
            
        }

        private void DeleteTokensByUser(
            Guid siteId,
            Guid userId,
           ICoreDbContext dbContext)
        {
            
            var query = from l in dbContext.UserTokens
                        where (
                        l.SiteId == siteId
                        && l.UserId == userId
                        )
                        select l;

            dbContext.UserTokens.RemoveRange(query);
               
        }

        #endregion

        #region UserLocation

        public async Task AddUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userLocation == null) { throw new ArgumentException("userLocation can't be null"); }

            var ul = UserLocation.FromIUserLocation(userLocation);
            if (ul.Id == Guid.Empty) { ul.Id = Guid.NewGuid(); }

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.UserLocations.Add(ul);

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken);
            }

        }

        public async Task UpdateUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userLocation == null) { throw new ArgumentException("userLocation can't be null"); }

            var ul = UserLocation.FromIUserLocation(userLocation);

            using (var dbContext = _contextFactory.CreateContext())
            {
                bool tracking = dbContext.ChangeTracker.Entries<UserLocation>().Any(x => x.Entity.Id == ul.Id);
                if (!tracking)
                {
                    dbContext.UserLocations.Update(ul);
                }

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task DeleteUserLocation(
            Guid siteId,
            Guid userLocationId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var itemToRemove = await dbContext.UserLocations.SingleOrDefaultAsync(
                x => x.Id == userLocationId
                , cancellationToken)
                .ConfigureAwait(false);

                if (itemToRemove == null) throw new InvalidOperationException("user location not found");

                dbContext.UserLocations.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
 
        }

        public async Task DeleteUserLocationsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from l in dbContext.UserLocations
                            where (l.UserId == userId)
                            select l;

                dbContext.UserLocations.RemoveRange(query);


                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task DeleteUserLocationsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from l in dbContext.UserLocations
                            where (l.SiteId == siteId)
                            select l;

                dbContext.UserLocations.RemoveRange(query);

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        
        #endregion

        
    }
}
