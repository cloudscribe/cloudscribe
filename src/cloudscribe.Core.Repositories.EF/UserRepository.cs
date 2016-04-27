// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2016-01-30
// 


using cloudscribe.Core.Models;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.EF
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(CoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private CoreDbContext dbContext;

        #region User

        public async Task<bool> Save(
            ISiteUser user, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user == null) { return false; }
            //if (user.SiteId == -1) { throw new ArgumentException("user must have a siteid"); }
            if (user.SiteGuid == Guid.Empty) { throw new ArgumentException("user must have a siteguid"); }

            SiteUser siteUser = SiteUser.FromISiteUser(user);
            if(siteUser.UserGuid == Guid.Empty)
            {
                siteUser.UserGuid = Guid.NewGuid(); 
                dbContext.Users.Add(siteUser);
            }
            else
            {
                bool tracking = dbContext.ChangeTracker.Entries<SiteUser>().Any(x => x.Entity.UserGuid == siteUser.UserGuid);
                if (!tracking)
                {
                    dbContext.Users.Update(siteUser);
                }

            }

            int rowsAffected = 
                await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false)
                ;

            if(user.UserGuid == Guid.Empty)
            {
                //user.UserId = siteUser.UserId;
                user.UserGuid = siteUser.UserGuid;
            }

            return rowsAffected > 0;

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

        public async Task<bool> Delete(
            Guid siteGuid, 
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = false;
            var itemToRemove = await dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.UserGuid == userGuid && x.SiteGuid == siteGuid, cancellationToken)
                .ConfigureAwait(false);

            if (itemToRemove != null)
            {
                result = await DeleteLoginsByUser(itemToRemove.SiteGuid, itemToRemove.UserGuid, false);
                result = await DeleteClaimsByUser(itemToRemove.SiteGuid, itemToRemove.UserGuid, false);
                result = await DeleteUserRoles(itemToRemove.UserGuid, false);

                
                dbContext.Users.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                result = rowsAffected > 0;
            }

            return result;

        }

        public async Task<bool> DeleteUsersBySite(
            Guid siteGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            bool result = await DeleteLoginsBySite(siteGuid);
            result = await DeleteClaimsBySite(siteGuid);
            result = await DeleteUserRolesBySite(siteGuid);

            var query = from x in dbContext.Users.Where(x => x.SiteGuid == siteGuid)
                        select x;

            dbContext.Users.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            result = rowsAffected > 0;

            return result;
        }

        public async Task<bool> FlagAsDeleted(
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.UserGuid == userGuid, 
                    cancellationToken)
                .ConfigureAwait(false);

            if(item == null) { return false; }

            item.IsDeleted = true;

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;

        }

        public async Task<bool> FlagAsNotDeleted(
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.UserGuid == userGuid, 
                    cancellationToken)
                    .ConfigureAwait(false);

            if (item == null) { return false; }

            item.IsDeleted = false;

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;

        }

        

        public async Task<bool> LockoutAccount(
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.UserGuid == userGuid
                    ,cancellationToken)
                    .ConfigureAwait(false);

            if (item == null) { return false; }

            item.IsLockedOut = true;
            
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;

        }

        public async Task<bool> UnLockAccount(
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.UserGuid == userGuid, 
                    cancellationToken)
                    .ConfigureAwait(false);

            if (item == null) { return false; }

            item.IsLockedOut = false;
            item.AccessFailedCount = 0;
            
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;
        }

        public async Task<bool> UpdateFailedPasswordAttemptCount(
            Guid userGuid, 
            int failedPasswordAttemptCount,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.UserGuid == userGuid, 
                    cancellationToken)
                    .ConfigureAwait(false);

            if (item == null) { return false; }
            
            item.AccessFailedCount = failedPasswordAttemptCount;
           
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;

        }

        public async Task<bool> UpdateLastLoginTime(
            Guid userGuid,
            DateTime lastLoginTime,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.UserGuid == userGuid, 
                    cancellationToken)
                    .ConfigureAwait(false);

            if (item == null) { return false; }

            item.LastLoginDate = lastLoginTime;

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;
        }

        public int GetCount(Guid siteGuid)
        {
            return dbContext.Users.Count<SiteUser>(x => x.SiteGuid == siteGuid);
        }

        //public async Task<ISiteUser> Fetch(
        //    int siteId, 
        //    int userId, 
        //    CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    cancellationToken.ThrowIfCancellationRequested();
        //    SiteUser item
        //        = await dbContext.Users.AsNoTracking()
        //        .SingleOrDefaultAsync(
        //            x => x.SiteId == siteId && x.UserId == userId
        //            , cancellationToken)
        //            .ConfigureAwait(false);

        //    return item;
        //}

        public async Task<ISiteUser> Fetch(
            Guid siteGuid, 
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            SiteUser item
                = await dbContext.Users.AsNoTracking()
                .SingleOrDefaultAsync(
                    x => x.SiteGuid == siteGuid && x.UserGuid == userGuid
                    , cancellationToken)
                    .ConfigureAwait(false);

            return item;
        }

        
        public async Task<ISiteUser> Fetch(
            Guid siteGuid, 
            string email, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string loweredEmail = email.ToLowerInvariant();
            SiteUser item
                = await dbContext.Users.AsNoTracking()
                .SingleOrDefaultAsync(
                    x => x.SiteGuid == siteGuid && x.NormalizedEmail == loweredEmail
                    , cancellationToken)
                    .ConfigureAwait(false);

            return item;
        }

        public async Task<ISiteUser> FetchByLoginName(
            Guid siteGuid, 
            string userName, 
            bool allowEmailFallback, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string loweredUserName = userName.ToLowerInvariant();

            SiteUser item
                = await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(
                    x => x.SiteGuid == siteGuid
                    && (
                    (x.UserName == userName) 
                    || (allowEmailFallback && x.NormalizedEmail == loweredUserName)
                    ),
                    cancellationToken
                    )
                    .ConfigureAwait(false);

            return item;
        }

        public async Task<List<IUserInfo>> GetByIPAddress(
            Guid siteGuid, 
            string ipv4Address, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from x in dbContext.Users
                        join y in dbContext.UserLocations  
                        on x.SiteGuid equals y.SiteGuid
                        where x.UserGuid == y.UserGuid && y.IpAddress == ipv4Address
                        select x
                        ;

            var items = await query
                .AsNoTracking()
                .ToListAsync<IUserInfo>(cancellationToken)
                .ConfigureAwait(false); 

            return items;

        }

        public async Task<List<IUserInfo>> GetCrossSiteUserListByEmail(
            string email, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            string loweredEmail = email.ToLowerInvariant();

            var query = from c in dbContext.Users
                        where c.NormalizedEmail == loweredEmail
                        orderby c.DisplayName ascending
                        select c;

            var items = await query
                .AsNoTracking()
                .ToListAsync<IUserInfo>(cancellationToken)
                .ConfigureAwait(false); 

            return items;

        }

        public async Task<int> CountUsers(
            Guid siteGuid, 
            string userNameBeginsWith, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbContext.Users.CountAsync<SiteUser>(
                x =>
                (
                    x.SiteGuid == siteGuid
                    && x.IsDeleted == false
                    && x.AccountApproved == true
                    && (
                    userNameBeginsWith == string.Empty
                    || x.DisplayName.StartsWith(userNameBeginsWith)
                    )
                )
                , cancellationToken
                )
                .ConfigureAwait(false);

        }

        public async Task<List<IUserInfo>> GetPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            int offset = (pageSize * pageNumber) - pageSize;

            //IQueryable<IUserInfo> query 
            //    = from x in dbContext.Users

            //            where 
            //            (
            //                x.SiteId == siteId
            //                && x.IsDeleted == false
            //                && x.AccountApproved == true
            //                && (
            //                userNameBeginsWith == string.Empty
            //                || x.DisplayName.StartsWith(userNameBeginsWith)
            //                )
            //            )
            //           // orderby x.DisplayName
            //            select new UserInfo
            //            {
            //                AvatarUrl = x.AvatarUrl,
            //                AccountApproved = x.AccountApproved,
            //                Country = x.Country,
            //                CreatedUtc = x.CreatedUtc,
            //                DateOfBirth = x.DateOfBirth,
            //                DisplayInMemberList = x.DisplayInMemberList,
            //                DisplayName = x.DisplayName,
            //                Email = x.Email,
            //                FirstName = x.FirstName,
            //                Gender = x.Gender,
            //                IsDeleted = x.IsDeleted,
            //                IsLockedOut = x.IsLockedOut,
            //                LastActivityDate = x.LastActivityDate,
            //                LastLoginDate = x.LastLoginDate,
            //                LastName= x.LastName,
            //                PhoneNumber = x.PhoneNumber,
            //                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
            //                SiteGuid = x.SiteGuid,
            //                SiteId = x.SiteId,
            //                State = x.State,
            //                TimeZoneId = x.TimeZoneId,
            //                Trusted = x.Trusted,
            //                UserGuid = x.UserGuid,
            //                UserId = x.UserId,
            //                UserName = x.UserName,
            //                WebSiteUrl = x.WebSiteUrl

            //            };


            //switch (sortMode)
            //{
            //    case 2:
            //        query = query.OrderBy(sl => sl.LastName).ThenBy(s2 => s2.FirstName).AsQueryable();
            //        break;
            //    case 1:
            //        query = query.OrderByDescending(sl => sl.CreatedUtc).AsQueryable();
            //        break;

            //    case 0:
            //    default:
            //        query = query.OrderBy(sl => sl.DisplayName).AsQueryable();
            //        break;
            //}

            //this is pretty ugly, surely there is a better way to dynamically set the order by

            IQueryable<IUserInfo> query;
            switch (sortMode)
            {
                case 2:
                    //query = query.OrderBy(sl => sl.LastName).ThenBy(s2 => s2.FirstName).AsQueryable();
                    query
                = from x in dbContext.Users

                  where
                  (
                      x.SiteGuid == siteGuid
                      && x.IsDeleted == false
                      && x.AccountApproved == true
                      && (
                      userNameBeginsWith == string.Empty
                      || x.DisplayName.StartsWith(userNameBeginsWith)
                      )
                  )
                  orderby x.LastName, x.FirstName
                  select new UserInfo
                  {
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      Country = x.Country,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginDate = x.LastLoginDate,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteGuid = x.SiteGuid,
                     // SiteId = x.SiteId,
                      State = x.State,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserGuid = x.UserGuid,
                     // UserId = x.UserId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };



                    break;
                case 1:
                    //query = query.OrderByDescending(sl => sl.CreatedUtc).AsQueryable();

                    query
                = from x in dbContext.Users

                  where
                  (
                      x.SiteGuid == siteGuid
                      && x.IsDeleted == false
                      && x.AccountApproved == true
                      && (
                      userNameBeginsWith == string.Empty
                      || x.DisplayName.StartsWith(userNameBeginsWith)
                      )
                  )
                  orderby x.CreatedUtc descending
                  select new UserInfo
                  {
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      Country = x.Country,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginDate = x.LastLoginDate,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteGuid = x.SiteGuid,
                     // SiteId = x.SiteId,
                      State = x.State,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserGuid = x.UserGuid,
                     // UserId = x.UserId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


                    break;

                case 0:
                default:
                    //query = query.OrderBy(sl => sl.DisplayName).AsQueryable();

                    query
                = from x in dbContext.Users

                  where
                  (
                      x.SiteGuid == siteGuid
                      && x.IsDeleted == false
                      && x.AccountApproved == true
                      && (
                      userNameBeginsWith == string.Empty
                      || x.DisplayName.StartsWith(userNameBeginsWith)
                      )
                  )
                  orderby x.DisplayName
                  select new UserInfo
                  {
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      Country = x.Country,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginDate = x.LastLoginDate,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteGuid = x.SiteGuid,
                      //SiteId = x.SiteId,
                      State = x.State,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserGuid = x.UserGuid,
                      //UserId = x.UserId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };



                    break;
            }

            
            return await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<IUserInfo>(cancellationToken)
                .ConfigureAwait(false); 
            
            
        }

        public async Task<int> CountUsersForAdminSearch(
            Guid siteGuid, 
            string searchInput, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbContext.Users.CountAsync<SiteUser>(
                x =>
                (
                    x.SiteGuid == siteGuid
                    && (
                    searchInput == string.Empty
                    || x.Email.Contains(searchInput)
                    || x.UserName.Contains(searchInput)
                    || x.FirstName.Contains(searchInput)
                    || x.LastName.Contains(searchInput)
                    || x.DisplayName.Contains(searchInput)
                    )
                )
                ,cancellationToken
                )
                .ConfigureAwait(false);
        }

        public async Task<List<IUserInfo>> GetUserAdminSearchPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in dbContext.Users
                        
                  where
                  (
                      x.SiteGuid == siteGuid
                        && (
                        searchInput == string.Empty
                        || x.Email.Contains(searchInput)
                        || x.UserName.Contains(searchInput)
                        || x.FirstName.Contains(searchInput)
                        || x.LastName.Contains(searchInput)
                        || x.DisplayName.Contains(searchInput)
                        )
                  )
                  select new UserInfo
                  {
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      Country = x.Country,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginDate = x.LastLoginDate,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteGuid = x.SiteGuid,
                     // SiteId = x.SiteId,
                      State = x.State,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserGuid = x.UserGuid,
                    //  UserId = x.UserId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            switch (sortMode)
            {
                case 2:
                    query = query.OrderBy(sl => sl.LastName).ThenBy(s2 => s2.FirstName).AsQueryable();
                    break;
                case 1:
                    query = query.OrderByDescending(sl => sl.CreatedUtc).AsQueryable();
                    break;

                case 0:
                default:
                    query = query.OrderBy(sl => sl.DisplayName).AsQueryable();
                    break;
            }
            
            return await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<IUserInfo>(cancellationToken)
                .ConfigureAwait(false);

            
        }

        public async Task<int> CountLockedByAdmin(
            Guid siteGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbContext.Users.CountAsync<SiteUser>(
                x => x.SiteGuid == siteGuid && x.IsLockedOut == true, 
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IUserInfo>> GetPageLockedByAdmin(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in dbContext.Users
                        
                  where
                  (
                      x.SiteGuid == siteGuid
                      && x.IsLockedOut == true
                  )
                  orderby x.DisplayName
                  select new UserInfo
                  {
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      Country = x.Country,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginDate = x.LastLoginDate,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteGuid = x.SiteGuid,
                     // SiteId = x.SiteId,
                      State = x.State,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserGuid = x.UserGuid,
                      //UserId = x.UserId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };

           
            return await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<IUserInfo>(cancellationToken)
                .ConfigureAwait(false);
            
        }

        public async Task<int> CountFutureLockoutEndDate(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbContext.Users.CountAsync<SiteUser>(
                x => x.SiteGuid == siteGuid
                && x.LockoutEndDateUtc.HasValue
                && x.LockoutEndDateUtc.Value > DateTime.UtcNow
                , 
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IUserInfo>> GetPageFutureLockoutEndDate(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in dbContext.Users

                  where
                  (
                      x.SiteGuid == siteGuid
                        && x.LockoutEndDateUtc.HasValue
                        && x.LockoutEndDateUtc.Value > DateTime.UtcNow
                  )
                  orderby x.DisplayName
                  select new UserInfo
                  {
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      Country = x.Country,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginDate = x.LastLoginDate,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteGuid = x.SiteGuid,
                      //SiteId = x.SiteId,
                      State = x.State,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserGuid = x.UserGuid,
                     // UserId = x.UserId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            return await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<IUserInfo>(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<int> CountUnconfirmedEmail(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbContext.Users.CountAsync<SiteUser>(
                x => x.SiteGuid == siteGuid
                && x.EmailConfirmed == false
                ,
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IUserInfo>> GetPageUnconfirmedEmailUsers(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in dbContext.Users

                  where
                  (
                      x.SiteGuid == siteGuid
                      && x.EmailConfirmed == false
                  )
                  orderby x.DisplayName
                  select new UserInfo
                  {
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      Country = x.Country,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginDate = x.LastLoginDate,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteGuid = x.SiteGuid,
                     // SiteId = x.SiteId,
                      State = x.State,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserGuid = x.UserGuid,
                     // UserId = x.UserId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            return await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<IUserInfo>(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<int> CountUnconfirmedPhone(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbContext.Users.CountAsync<SiteUser>(
                x => x.SiteGuid == siteGuid
                && x.PhoneNumberConfirmed == false
                ,
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IUserInfo>> GetPageUnconfirmedPhoneUsers(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in dbContext.Users

                  where
                  (
                      x.SiteGuid == siteGuid
                      && x.PhoneNumberConfirmed == false
                  )
                  orderby x.DisplayName
                  select new UserInfo
                  {
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      Country = x.Country,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginDate = x.LastLoginDate,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteGuid = x.SiteGuid,
                     // SiteId = x.SiteId,
                      State = x.State,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserGuid = x.UserGuid,
                     // UserId = x.UserId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            return await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<IUserInfo>(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<int> CountNotApprovedUsers(
            Guid siteGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbContext.Users.CountAsync<SiteUser>(
                x => x.SiteGuid == siteGuid && x.AccountApproved == false, 
                cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<List<IUserInfo>> GetNotApprovedUsers(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in dbContext.Users     
                  where
                  (
                      x.SiteGuid == siteGuid
                      && x.AccountApproved == false
                  )
                  orderby x.DisplayName
                  select new UserInfo
                  {
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      Country = x.Country,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginDate = x.LastLoginDate,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteGuid = x.SiteGuid,
                     // SiteId = x.SiteId,
                      State = x.State,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserGuid = x.UserGuid,
                     // UserId = x.UserId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };

           
            return await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<IUserInfo>(cancellationToken)
                .ConfigureAwait(false);
            
        }

        public async Task<bool> EmailExistsInDB(
            Guid siteGuid, 
            string email, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var found = await Fetch(siteGuid, email);
            if(found == null) { return false; }
            return true;

        }

        public async Task<bool> EmailExistsInDB(
            Guid siteGuid, 
            Guid userGuid, 
            string email, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var found = await Fetch(siteGuid, email);
            if (found == null) { return false; }
            if(found.UserGuid != userGuid) { return false; }
            return true;
            
        }

        public bool LoginExistsInDB(Guid siteGuid, string loginName)
        {
            SiteUser item
                = dbContext.Users.SingleOrDefault(
                    x => x.SiteGuid == siteGuid
                    && x.UserName == loginName
                    );

            if(item == null) { return false; }
            return true;
        }


        /// <summary>
        /// available only if the found user matches the passed in one
        /// or if not found
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public async Task<bool> LoginIsAvailable(
            Guid siteGuid, 
            Guid userGuid, 
            string loginName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var found = await FetchByLoginName(siteGuid, loginName, false, cancellationToken);
            if (found == null) { return true; }
            if (found.UserGuid == userGuid) { return true; }
            return false;
        }


        public async Task<string> GetUserNameFromEmail(
            Guid siteGuid, 
            string email, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var found = await Fetch(siteGuid, email, cancellationToken);
            if (found == null) { return string.Empty; }
            return found.UserName;
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
        public async Task<bool> SaveRole(
            ISiteRole role, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (role == null) { return false; }
            //if (role.SiteId == -1) { throw new ArgumentException("SiteId must be provided"); }
            if (role.SiteGuid == Guid.Empty) { throw new ArgumentException("SiteGuid must be provided"); }
            
            SiteRole siteRole = SiteRole.FromISiteRole(role); 
            if(siteRole.RoleGuid == Guid.Empty)
            {
                siteRole.RoleGuid = Guid.NewGuid();
                if(siteRole.RoleName.Length == 0)
                {
                    siteRole.RoleName = siteRole.DisplayName;
                }
                dbContext.Roles.Add(siteRole);
            }
            else
            {
                bool tracking = dbContext.ChangeTracker.Entries<SiteRole>().Any(x => x.Entity.RoleGuid == siteRole.RoleGuid);
                if (!tracking)
                {
                    dbContext.Roles.Update(siteRole);
                }

            }

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            if(role.RoleGuid == Guid.Empty)
            {
                //update the original with the new keys on insert
                //role.RoleId = siteRole.RoleId;
                role.RoleGuid = siteRole.RoleGuid;
            }

            return rowsAffected > 0;

        }

        public async Task<bool> DeleteRole(
            Guid roleGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = false;
            var itemToRemove = await dbContext.Roles.SingleOrDefaultAsync(
                x => x.RoleGuid == roleGuid, 
                cancellationToken)
                .ConfigureAwait(false);

            if (itemToRemove != null)
            {
                dbContext.Roles.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                result = rowsAffected > 0;
            }

            return result;
        }

        public async Task<bool> DeleteRolesBySite(
            Guid siteGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = false;
            var query = from r in dbContext.Roles.Where(x => x.SiteGuid == siteGuid)
                        select r;
            
            dbContext.Roles.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            result = rowsAffected > 0;
            
            return result;
        }

        public async Task<bool> AddUserToRole(
           // int roleId,
            Guid roleGuid,
            //int userId,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            UserRole ur = new UserRole();
            ur.Id = Guid.NewGuid();
            ur.RoleGuid = roleGuid;
            //ur.RoleId = roleId;
            ur.UserGuid = userGuid;
            //ur.UserId = userId;

            dbContext.UserRoles.Add(ur);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;

        }

        public async Task<bool> RemoveUserFromRole(
            Guid roleGuid, 
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = false;
            var itemToRemove = await dbContext.UserRoles.SingleOrDefaultAsync(
                x => x.RoleGuid == roleGuid && x.UserGuid == userGuid, 
                cancellationToken)
                .ConfigureAwait(false);

            if (itemToRemove != null)
            {
                dbContext.UserRoles.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                result = rowsAffected > 0;
            }

            return result;
        }

        public Task<bool> DeleteUserRoles(
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return DeleteUserRoles(userGuid, true, cancellationToken);
        }

        public async Task<bool> DeleteUserRoles(
            Guid userGuid,
            bool saveChanges, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from x in dbContext.UserRoles
                        where x.UserGuid == userGuid
                        select x;

            dbContext.UserRoles.RemoveRange(query);
            if(saveChanges)
            {
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                return rowsAffected > 0;
            }
            return true;
        }

        public async Task<bool> DeleteUserRolesByRole(
            Guid roleGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from x in dbContext.UserRoles
                        where x.RoleGuid == roleGuid
                        select x;

            dbContext.UserRoles.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUserRolesBySite(
            Guid siteGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from x in dbContext.UserRoles
                        join y in dbContext.Roles on x.RoleGuid equals y.RoleGuid
                        where y.SiteGuid == siteGuid
                        select x;

            dbContext.UserRoles.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;
        }


        public async Task<bool> RoleExists(
            Guid siteGuid, 
            string roleName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int count = await dbContext.Roles.CountAsync<SiteRole>(
                r => r.SiteGuid == siteGuid && r.RoleName == roleName
                , cancellationToken)
                .ConfigureAwait(false);

            return count > 0;
        }

        public async Task<ISiteRole> FetchRole(
            Guid roleGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            SiteRole item
                = await dbContext.Roles.SingleOrDefaultAsync(
                    x => x.RoleGuid == roleGuid
                    , cancellationToken)
                    .ConfigureAwait(false);

            return item;
        }

        public async Task<ISiteRole> FetchRole(
            Guid siteGuid, 
            string roleName, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            SiteRole item
                = await dbContext.Roles.SingleOrDefaultAsync(
                    x => x.SiteGuid == siteGuid && x.RoleName == roleName
                    , cancellationToken)
                    .ConfigureAwait(false);

            return item;

        }

        public async Task<List<string>> GetUserRoles(
            Guid siteGuid,
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from x in dbContext.Roles
                        join y in dbContext.UserRoles
                        on x.RoleGuid equals y.RoleGuid
                        where y.UserGuid == userGuid
                        orderby x.RoleName
                        select x.RoleName
                        ;
            return await query
                .AsNoTracking()
                .ToListAsync<string>(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<int> CountOfRoles(
            Guid siteGuid, 
            string searchInput, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbContext.Roles.CountAsync<SiteRole>(
                x => x.SiteGuid.Equals(siteGuid)
                && (
                 (searchInput == "")
                        || x.DisplayName.Contains(searchInput)
                        || x.RoleName.Contains(searchInput)
                ), 
                cancellationToken
                ).ConfigureAwait(false);

        }

        public async Task<IList<ISiteRole>> GetRolesBySite(
            Guid siteGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var listQuery = from x in dbContext.Roles
                            where (
                            x.SiteGuid.Equals(siteGuid) &&
                            (searchInput == "" || x.DisplayName.Contains(searchInput) || x.RoleName.Contains(searchInput))
                            )
                            orderby x.RoleName ascending
                            select new SiteRole {
                                //RoleId = x.RoleId,
                                RoleGuid = x.RoleGuid,
                               // SiteId = x.SiteId,
                                SiteGuid = x.SiteGuid,
                                RoleName = x.RoleName,
                                DisplayName = x.DisplayName,
                                MemberCount = dbContext.UserRoles.Count<UserRole>(u => u.RoleGuid == x.RoleGuid)
                            };

            return await listQuery
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<ISiteRole>(cancellationToken)
                .ConfigureAwait(false);
            
        }


        public async Task<int> CountUsersInRole(
            Guid siteGuid, 
            Guid roleGuid, 
            string searchInput, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from x in dbContext.Users
                        join y in dbContext.UserRoles
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
                        //select x.UserGuid
                        select 1
                        ;

            return await query.CountAsync<int>(cancellationToken).ConfigureAwait(false);

        }


        public async Task<IList<IUserInfo>> GetUsersInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var query = from x in dbContext.Users
                        join y in dbContext.UserRoles
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

          
            return await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<IUserInfo>(cancellationToken)
                .ConfigureAwait(false); 

           

        }

        public async Task<IList<ISiteUser>> GetUsersInRole(
            Guid siteGuid,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from x in dbContext.Users
                        join y in dbContext.UserRoles
                        on x.UserGuid equals y.UserGuid
                        join z in dbContext.Roles
                        on y.RoleGuid equals z.RoleGuid
                        orderby x.DisplayName
                        where 
                            (x.SiteGuid.Equals(siteGuid) && z.RoleName.Equals(roleName))
                            
                        select x
                        ;

            var items = await query
                .AsNoTracking()
                .ToListAsync<ISiteUser>(cancellationToken)
                .ConfigureAwait(false); 

            return items;
        }

        public async Task<int> CountUsersNotInRole(
            Guid siteGuid,
            Guid roleGuid, 
            string searchInput, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from u in dbContext.Users
                        from r in dbContext.Roles
                        join ur in dbContext.UserRoles
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

                        //select u.UserId;
                        select 1;

            return await query.CountAsync<int>(cancellationToken).ConfigureAwait(false);

        }

        public async Task<IList<IUserInfo>> GetUsersNotInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;
            // it took me a lot of tries and googling to figure out how to get this query to work as intended
            // it works but is still logging a warning DefaultIfEmpty() could not be translated and will be evaluated locally

            var query = from u in dbContext.Users
                        from r in dbContext.Roles
                        join ur in dbContext.UserRoles
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
                        
            return await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<IUserInfo>(cancellationToken)
                .ConfigureAwait(false); 
            
        }


        #endregion

        #region Claims

        public async Task<bool> SaveClaim(
            IUserClaim userClaim, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (userClaim == null) { return false; }

            UserClaim claim = UserClaim.FromIUserClaim(userClaim);
            if(claim.Id == Guid.Empty)
            {
                claim.Id = Guid.NewGuid();
                dbContext.UserClaims.Add(claim);
            }
            else
            {
                bool tracking = dbContext.ChangeTracker.Entries<UserClaim>().Any(x => x.Entity.Id == claim.Id);
                if (!tracking)
                {
                    dbContext.UserClaims.Update(claim);
                }

            }

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            if(userClaim.Id == Guid.Empty)
            {
                //update the original on insert
                userClaim.Id = claim.Id;
            }

            return rowsAffected > 0;

        }

        public async Task<bool> DeleteClaim(
            Guid id, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = false;
            var itemToRemove = await dbContext.UserClaims.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (itemToRemove != null)
            {
                dbContext.UserClaims.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                result = rowsAffected > 0;
            }

            return result;

        }

        public async Task<bool> DeleteClaimsByUser(
            Guid siteGuid, 
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await DeleteClaimsByUser(siteGuid, userGuid, true, cancellationToken);

        }

        public async Task<bool> DeleteClaimsByUser(
            Guid siteGuid, 
            Guid userGuid, 
            bool saveChanges, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from x in dbContext.UserClaims
                        where (
                        (siteGuid == Guid.Empty || x.SiteGuid == siteGuid)
                        && x.UserGuid == userGuid
                        )
                        select x;

            dbContext.UserClaims.RemoveRange(query);
            if(saveChanges)
            {
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                return rowsAffected > 0;
            }
            return true;

        }

        public async Task<bool> DeleteClaimByUser(
            Guid siteGuid, 
            Guid userGuid, 
            string claimType, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from x in dbContext.UserClaims
                        where (
                        (siteGuid == Guid.Empty || x.SiteGuid == siteGuid)
                        && (x.UserGuid == userGuid && x.ClaimType == claimType)
                        )
                        select x;

            dbContext.UserClaims.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;

        }

        public async Task<bool> DeleteClaimsBySite(
            Guid siteGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from x in dbContext.UserClaims
                        where x.SiteGuid == siteGuid
                        select x;

            dbContext.UserClaims.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;

        }

        public async Task<IList<IUserClaim>> GetClaimsByUser(
            Guid siteGuid,
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from l in dbContext.UserClaims
                        where l.SiteGuid == siteGuid && l.UserGuid == userGuid

                        select l;
            var items = await query
                .AsNoTracking()
                .ToListAsync<IUserClaim>(cancellationToken)
                .ConfigureAwait(false);
            return items;

        }

        public async Task<IList<ISiteUser>> GetUsersForClaim(
            Guid siteGuid,
            string claimType,
            string claimValue,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from x in dbContext.Users
                        join y in dbContext.UserClaims
                        on x.UserGuid equals y.UserGuid
                        where x.SiteGuid == siteGuid
                        orderby x.DisplayName
                        select x
                        ;

            var items = await query
                .AsNoTracking()
                .ToListAsync<ISiteUser>(cancellationToken)
                .ConfigureAwait(false); 

            return items;
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

            UserLogin login = UserLogin.FromIUserLogin(userLogin);
            
            dbContext.UserLogins.Add(login);
            
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;

        }

        public async Task<IUserLogin> FindLogin(
            Guid siteGuid,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from l in dbContext.UserLogins
                        where (
                        l.SiteGuid == siteGuid
                        && l.LoginProvider == loginProvider
                        && l.ProviderKey == providerKey
                        )
                        select l;

            var items = await query
                .AsNoTracking()
                .SingleOrDefaultAsync<IUserLogin>(cancellationToken)
                .ConfigureAwait(false);

            return items;
        }

        public async Task<bool> DeleteLogin(
            Guid siteGuid,
            Guid userGuid,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from l in dbContext.UserLogins
                        where (
                        l.SiteGuid == siteGuid
                        && l.UserGuid == userGuid
                        && l.LoginProvider == loginProvider
                        && l.ProviderKey == providerKey
                        )
                        select l;

            dbContext.UserLogins.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;

        }

        public Task<bool> DeleteLoginsByUser(
            Guid siteGuid, 
            Guid userGuid, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return DeleteLoginsByUser(siteGuid, userGuid, true, cancellationToken);

        }

        public async Task<bool> DeleteLoginsByUser(
            Guid siteGuid,
            Guid userGuid,
            bool saveChanges, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from l in dbContext.UserLogins
                        where (
                        l.SiteGuid == siteGuid
                        && l.UserGuid == userGuid
                        )
                        select l;

            dbContext.UserLogins.RemoveRange(query);
            if(saveChanges)
            {
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                return rowsAffected > 0;
            }
            return true;

        }

        public async Task<bool> DeleteLoginsBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from l in dbContext.UserLogins
                        where (l.SiteGuid == siteGuid)
                        select l;

            dbContext.UserLogins.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;

        }

        public async Task<IList<IUserLogin>> GetLoginsByUser(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from l in dbContext.UserLogins
                        where (
                        l.SiteGuid == siteGuid
                        && l.UserGuid == userGuid
                        )
                        select l;

            var items = await query
                .AsNoTracking()
                .ToListAsync<IUserLogin>(cancellationToken)
                .ConfigureAwait(false);

            return items;

        }

        #endregion

        #region UserLocation

        public async Task<IUserLocation> FetchLocationByUserAndIpv4Address(
            Guid userGuid,
            long ipv4AddressAsLong,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = from x in dbContext.UserLocations
                        where x.UserGuid == userGuid
                        && x.IpAddressLong == ipv4AddressAsLong
                        select x
                        ;

            return await query
                .AsNoTracking()
                .FirstOrDefaultAsync<UserLocation>(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<bool> AddUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (userLocation == null) { return false; }

            UserLocation ul = UserLocation.FromIUserLocation(userLocation);
            if (ul.RowId == Guid.Empty) { ul.RowId = Guid.NewGuid(); }
            cancellationToken.ThrowIfCancellationRequested();

            dbContext.UserLocations.Add(ul);
            cancellationToken.ThrowIfCancellationRequested();

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken);
       
            return rowsAffected > 0;

        }

        public async Task<bool> UpdateUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (userLocation == null) { return false; }

            UserLocation ul = UserLocation.FromIUserLocation(userLocation);
            
            cancellationToken.ThrowIfCancellationRequested();

            bool tracking = dbContext.ChangeTracker.Entries<UserLocation>().Any(x => x.Entity.RowId == ul.RowId);
            if (!tracking)
            {
                dbContext.UserLocations.Update(ul);
            }

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return rowsAffected > 0;

        }

        public async Task<bool> DeleteUserLocation(
            Guid rowGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = false;
            var itemToRemove = await dbContext.UserLocations.SingleOrDefaultAsync(
                x => x.RowId == rowGuid
                , cancellationToken)
                .ConfigureAwait(false);

            if (itemToRemove != null)
            {
                dbContext.UserLocations.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                result = rowsAffected > 0;
            }

            return result;


        }

        public async Task<bool> DeleteUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = false;
            var itemToRemove = await dbContext.UserLocations.SingleOrDefaultAsync(
                x => x.UserGuid == userGuid
                , cancellationToken)
                .ConfigureAwait(false);

            if (itemToRemove != null)
            {
                dbContext.UserLocations.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                result = rowsAffected > 0;
            }

            return result;

        }

        public async Task<bool> DeleteUserLocationsBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = false;
            var itemToRemove = await dbContext.UserLocations.SingleOrDefaultAsync(
                x => x.SiteGuid == siteGuid
                , cancellationToken)
                .ConfigureAwait(false);

            if (itemToRemove != null)
            {
                dbContext.UserLocations.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                result = rowsAffected > 0;
            }

            return result;


        }

        public Task<int> CountUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return dbContext.UserLocations.CountAsync<UserLocation>(cancellationToken);


        }

        public async Task<IList<IUserLocation>> GetUserLocationsByUser(
            Guid userGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            var query = dbContext.UserLocations
                .OrderBy(x => x.IpAddressLong)
                .Select(p => p)
                .Skip(offset)
                .Take(pageSize)
                .Where(x => x.UserGuid == userGuid)
                ;


            return await query
                .AsNoTracking()
                .ToListAsync<IUserLocation>(cancellationToken)
                .ConfigureAwait(false);

        }

        #endregion

        #region IDisposable Support

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
