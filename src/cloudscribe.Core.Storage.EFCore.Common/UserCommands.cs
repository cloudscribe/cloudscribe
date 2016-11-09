// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2016-08-03
// 


using cloudscribe.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public class UserCommands : IUserCommands
    {
        public UserCommands(ICoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private ICoreDbContext dbContext;

        #region User

        public async Task Create(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) { throw new ArgumentException("user can't be null"); }
            if (user.SiteId == Guid.Empty) { throw new ArgumentException("user must have a siteid"); }
            if (user.Id == Guid.Empty) { throw new ArgumentException("user must have a non empty guid for id"); }

            SiteUser siteUser = SiteUser.FromISiteUser(user);
            
            dbContext.Users.Add(siteUser);
            
            int rowsAffected =
                await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false)
                ;

            //if (user.UserGuid == Guid.Empty)
            //{
            //    //user.UserId = siteUser.UserId;
            //    user.UserGuid = siteUser.UserGuid;
            //}

            //return rowsAffected > 0;

        }

        public async Task Update(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentException("user can't be null"); }
            if (user.SiteId == Guid.Empty) { throw new ArgumentException("user must have a siteguid"); }
            if (user.Id == Guid.Empty) { throw new ArgumentException("user must have a non empty guid for id"); }

            SiteUser siteUser = SiteUser.FromISiteUser(user);
            
            bool tracking = dbContext.ChangeTracker.Entries<SiteUser>().Any(x => x.Entity.Id == siteUser.Id);
            if (!tracking)
            {
                dbContext.Users.Update(siteUser);
            }
            
            int rowsAffected =
                await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false)
                ;
           
        }

        //public async Task<bool> Delete(ISiteUser user)
        //{

        //    return await Delete(user.SiteId, user.UserId);
        //    //bool result = await DeleteLoginsByUser(user.SiteId, user.Id, false);
        //    //result = await DeleteClaimsByUser(user.SiteId, user.Id, false);
        //    //result = await DeleteUserRoles(user.UserId, false);

        //    //SiteUser itemToRemove = SiteUser.FromISiteUser(user);
        //    //dbContext.Users.
        //    //dbContext.Users.Remove(itemToRemove);
        //    //int rowsAffected = await dbContext.SaveChangesAsync();
        //    //result = rowsAffected > 0;

        //    //return result;
        //}

        public async Task Delete(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            var itemToRemove = await dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId && x.SiteId == siteId, cancellationToken)
                .ConfigureAwait(false);

            if (itemToRemove != null)
            {
                await DeleteLoginsByUser(itemToRemove.SiteId, itemToRemove.Id, false);
                await DeleteClaimsByUser(itemToRemove.SiteId, itemToRemove.Id, false);
                await DeleteUserRoles(siteId, itemToRemove.Id, false);


                dbContext.Users.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                
            }

            
        }

        public async Task DeleteUsersBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            await DeleteLoginsBySite(siteId);
            await DeleteClaimsBySite(siteId);
            await DeleteUserRolesBySite(siteId);

            var query = from x in dbContext.Users.Where(x => x.SiteId == siteId)
                        select x;

            dbContext.Users.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            
        }

        public async Task FlagAsDeleted(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            var item = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.Id == userId,
                    cancellationToken)
                .ConfigureAwait(false);

            if (item == null) { throw new InvalidOperationException("user not found"); }

            item.IsDeleted = true;

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            
        }

        public async Task FlagAsNotDeleted(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var item = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.Id == userId,
                    cancellationToken)
                    .ConfigureAwait(false);

            if (item == null) { throw new InvalidOperationException("user not found"); }

            item.IsDeleted = false;

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }



        public async Task LockoutAccount(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var item = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.Id == userId
                    , cancellationToken)
                    .ConfigureAwait(false);

            if (item == null) { throw new InvalidOperationException("user not found"); }

            item.IsLockedOut = true;

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
        }

        public async Task UnLockAccount(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

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

        public async Task UpdateFailedPasswordAttemptCount(
            Guid siteId,
            Guid userId,
            int failedPasswordAttemptCount,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var item = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.Id == userId,
                    cancellationToken)
                    .ConfigureAwait(false);

            if (item == null) { throw new InvalidOperationException("user not found"); }

            item.AccessFailedCount = failedPasswordAttemptCount;

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
        }

        public async Task UpdateLastLoginTime(
            Guid siteId,
            Guid userId,
            DateTime lastLoginTime,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var item = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.Id == userId,
                    cancellationToken)
                    .ConfigureAwait(false);

            if (item == null) { throw new InvalidOperationException("user not found"); }

            item.LastLoginUtc = lastLoginTime;

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            
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
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null) { throw new ArgumentException("role cannot be null"); }
            if (role.SiteId == Guid.Empty) { throw new ArgumentException("SiteId must be provided"); }
            if (role.Id == Guid.Empty) { throw new ArgumentException("Id must be provided"); }

            var siteRole = SiteRole.FromISiteRole(role);
            
            if (siteRole.NormalizedRoleName.Length == 0)
            {
                siteRole.NormalizedRoleName = siteRole.RoleName;
            }
            dbContext.Roles.Add(siteRole);
            
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

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

            var siteRole = SiteRole.FromISiteRole(role);
            
            bool tracking = dbContext.ChangeTracker.Entries<SiteRole>().Any(x => x.Entity.Id == siteRole.Id);
            if (!tracking)
            {
                dbContext.Roles.Update(siteRole);
            }
            
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
        }

        public async Task DeleteRole(
            Guid siteId,
            Guid roleId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var itemToRemove = await dbContext.Roles.SingleOrDefaultAsync(
                x => x.Id == roleId,
                cancellationToken)
                .ConfigureAwait(false);

            if (itemToRemove == null) { throw new InvalidOperationException("role not found"); }
            
            dbContext.Roles.Remove(itemToRemove);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
    
        }

        public async Task DeleteRolesBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from r in dbContext.Roles.Where(x => x.SiteId == siteId)
                        select r;

            dbContext.Roles.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

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

            var ur = new UserRole();
            ur.RoleId = roleId;
            ur.UserId = userId;

            dbContext.UserRoles.Add(ur);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task RemoveUserFromRole(
            Guid siteId,
            Guid roleId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var itemToRemove = await dbContext.UserRoles.SingleOrDefaultAsync(
                x => x.UserId == userId && x.RoleId == roleId
                , cancellationToken)
                .ConfigureAwait(false);

            if (itemToRemove == null) { throw new InvalidOperationException("userrole not found"); }
            
            dbContext.UserRoles.Remove(itemToRemove);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
  
        }

        public async Task DeleteUserRoles(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            await DeleteUserRoles(siteId, userId, true, cancellationToken).ConfigureAwait(false) ;
        }

        private async Task DeleteUserRoles(
            Guid siteId,
            Guid userId,
            bool saveChanges,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from x in dbContext.UserRoles
                        where x.UserId == userId
                        select x;

            dbContext.UserRoles.RemoveRange(query);
            if (saveChanges)
            {
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                
            }
            
        }

        public async Task DeleteUserRolesByRole(
            Guid siteId,
            Guid roleId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from x in dbContext.UserRoles
                        where x.RoleId == roleId
                        select x;

            dbContext.UserRoles.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task DeleteUserRolesBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from x in dbContext.UserRoles
                        join y in dbContext.Roles on x.RoleId equals y.Id
                        where y.SiteId == siteId
                        select x;

            dbContext.UserRoles.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }


        #endregion

        #region Claims

        public async Task CreateClaim(
            IUserClaim userClaim,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (userClaim == null) { throw new ArgumentException("userClaim can't be null"); }
            
            var claim = UserClaim.FromIUserClaim(userClaim);
            if (claim.Id == Guid.Empty) throw new ArgumentException("userClaim must have a non empty id");
           
            dbContext.UserClaims.Add(claim);
            
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task UpdateClaim(
            IUserClaim userClaim,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (userClaim == null) { throw new ArgumentException("userClaim can't be null"); }

            var claim = UserClaim.FromIUserClaim(userClaim);
            if (claim.Id == Guid.Empty) throw new ArgumentException("userClaim must have a non empty id");
            
            bool tracking = dbContext.ChangeTracker.Entries<UserClaim>().Any(x => x.Entity.Id == claim.Id);
            if (!tracking)
            {
                dbContext.UserClaims.Update(claim);
            }

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);


        }

        public async Task DeleteClaim(
            Guid siteId,
            Guid claimId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var itemToRemove = await dbContext.UserClaims.SingleOrDefaultAsync(x => x.Id == claimId, cancellationToken);
            if (itemToRemove == null) { throw new InvalidOperationException("claim not found"); }
            
            dbContext.UserClaims.Remove(itemToRemove);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
        }

        public async Task DeleteClaimsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            await DeleteClaimsByUser(siteId, userId, true, cancellationToken);

        }

        private async Task DeleteClaimsByUser(
            Guid siteId,
            Guid userId,
            bool saveChanges,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from x in dbContext.UserClaims
                        where (
                        (siteId == Guid.Empty || x.SiteId == siteId)
                        && x.UserId == userId
                        )
                        select x;

            dbContext.UserClaims.RemoveRange(query);
            if (saveChanges)
            {
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

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

            var query = from x in dbContext.UserClaims
                        where (
                        (siteId == Guid.Empty || x.SiteId == siteId)
                        && (x.UserId == userId && x.ClaimType == claimType)
                        )
                        select x;

            dbContext.UserClaims.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
        }

        public async Task DeleteClaimsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from x in dbContext.UserClaims
                        where x.SiteId == siteId
                        select x;

            dbContext.UserClaims.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
        }

        #endregion

        #region Logins

        public async Task CreateLogin(
            IUserLogin userLogin,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (userLogin == null) { throw new ArgumentException("userLogin can't be null"); }
            if (userLogin.LoginProvider.Length == -1) { throw new ArgumentException("userLogin must have a loginprovider"); }
            if (userLogin.ProviderKey.Length == -1) { throw new ArgumentException("userLogin must have a providerkey"); }
            if (userLogin.UserId == Guid.Empty) { throw new ArgumentException("userLogin must have a user id"); }

            var login = UserLogin.FromIUserLogin(userLogin);

            dbContext.UserLogins.Add(login);

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
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

        public async Task DeleteLoginsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            await DeleteLoginsByUser(siteId, userId, true, cancellationToken);

        }

        private async Task DeleteLoginsByUser(
            Guid siteId,
            Guid userId,
            bool saveChanges,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from l in dbContext.UserLogins
                        where (
                        l.SiteId == siteId
                        && l.UserId == userId
                        )
                        select l;

            dbContext.UserLogins.RemoveRange(query);
            if (saveChanges)
            {
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
 
            }
            

        }

        public async Task DeleteLoginsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from l in dbContext.UserLogins
                        where (l.SiteId == siteId)
                        select l;

            dbContext.UserLogins.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
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

            if (userLocation == null) { throw new ArgumentException("userLocation can't be null"); }

            var ul = UserLocation.FromIUserLocation(userLocation);
            if (ul.Id == Guid.Empty) { ul.Id = Guid.NewGuid(); }
            
            dbContext.UserLocations.Add(ul);
            
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken);
            
        }

        public async Task UpdateUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (userLocation == null) { throw new ArgumentException("userLocation can't be null"); }

            var ul = UserLocation.FromIUserLocation(userLocation);

            cancellationToken.ThrowIfCancellationRequested();

            bool tracking = dbContext.ChangeTracker.Entries<UserLocation>().Any(x => x.Entity.Id == ul.Id);
            if (!tracking)
            {
                dbContext.UserLocations.Update(ul);
            }

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
        }

        public async Task DeleteUserLocation(
            Guid siteId,
            Guid userLocationId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var itemToRemove = await dbContext.UserLocations.SingleOrDefaultAsync(
                x => x.Id == userLocationId
                , cancellationToken)
                .ConfigureAwait(false);

            if (itemToRemove == null) throw new InvalidOperationException("user location not found");
            
            dbContext.UserLocations.Remove(itemToRemove);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
        }

        public async Task DeleteUserLocationsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            
            var query = from l in dbContext.UserLocations
                        where (l.UserId == userId)
                        select l;

            dbContext.UserLocations.RemoveRange(query);


            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
        }

        public async Task DeleteUserLocationsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from l in dbContext.UserLocations
                        where (l.SiteId == siteId)
                        select l;

            dbContext.UserLocations.RemoveRange(query);
            
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

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
