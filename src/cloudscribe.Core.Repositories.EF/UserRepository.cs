// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2015-11-24
// 


using cloudscribe.Core.Models;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.EF
{
    public class UserRepository
    {
        public UserRepository(CoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private CoreDbContext dbContext;


        public async Task<bool> Save(ISiteUser user)
        {
            if (user == null) { return false; }
            if (user.SiteId == -1) { throw new ArgumentException("user must have a siteid"); }
            if (user.SiteGuid == Guid.Empty) { throw new ArgumentException("user must have a siteguid"); }

            SiteUser siteUser = SiteUser.FromISiteUser(user);
            dbContext.Users.Add(siteUser);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public async Task<bool> Delete(int userId)
        {
            var result = false;
            var itemToRemove = await dbContext.Users.SingleOrDefaultAsync(x => x.UserId == userId);
            if (itemToRemove != null)
            {
                dbContext.Users.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;

        }

        public async Task<bool> FlagAsDeleted(int userId)
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(x => x.UserId == userId);

            if(item == null) { return false; }

            item.IsDeleted = true;

            dbContext.Users.Update(item);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public async Task<bool> FlagAsNotDeleted(int userId)
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(x => x.UserId == userId);

            if (item == null) { return false; }

            item.IsDeleted = false;

            dbContext.Users.Update(item);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public async Task<bool> ConfirmRegistration(Guid registrationGuid)
        {
            if (registrationGuid == Guid.Empty)
            {
                return false;
            }

            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(x => x.RegisterConfirmGuid == registrationGuid);

            if (item == null) { return false; }

            item.IsLockedOut = false;
            item.RegisterConfirmGuid = Guid.Empty;

            dbContext.Users.Update(item);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public async Task<bool> LockoutAccount(Guid userGuid)
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(x => x.UserGuid == userGuid);

            if (item == null) { return false; }

            item.IsLockedOut = true;
            item.LastLockoutDate = DateTime.UtcNow;

            dbContext.Users.Update(item);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public async Task<bool> UnLockAccount(Guid userGuid)
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(x => x.UserGuid == userGuid);

            if (item == null) { return false; }

            item.IsLockedOut = false;
            item.FailedPasswordAttemptCount = 0;
            item.FailedPasswordAnswerAttemptCount = 0;

            dbContext.Users.Update(item);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> UpdateFailedPasswordAttemptCount(Guid userGuid, int failedPasswordAttemptCount)
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(x => x.UserGuid == userGuid);

            if (item == null) { return false; }
            
            item.FailedPasswordAttemptCount = failedPasswordAttemptCount;
           
            dbContext.Users.Update(item);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public int GetCount(int siteId)
        {
            return dbContext.Users.Count<SiteUser>(x => x.SiteId == siteId);
        }

        public async Task<ISiteUser> Fetch(int siteId, int userId)
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(x => x.SiteId == siteId && x.UserId == userId);

            return item;
        }

        public async Task<ISiteUser> Fetch(int siteId, Guid userGuid)
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(x => x.SiteId == siteId && x.UserGuid == userGuid);

            return item;
        }

        public async Task<ISiteUser> FetchByConfirmationGuid(int siteId, Guid confirmGuid)
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(x => x.SiteId == siteId && x.RegisterConfirmGuid == confirmGuid);

            return item;
        }

        public async Task<ISiteUser> Fetch(int siteId, string email)
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(x => x.SiteId == siteId && x.LoweredEmail == email.ToLowerInvariant());

            return item;
        }

        public async Task<ISiteUser> FetchByLoginName(int siteId, string userName, bool allowEmailFallback)
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.SiteId == siteId 
                    && (
                    (x.UserName == userName) 
                    || (allowEmailFallback && x.LoweredEmail == userName.ToLowerInvariant())
                    )
                    );

            return item;
        }

        public async Task<List<IUserInfo>> GetByIPAddress(Guid siteGuid, string ipv4Address)
        {
            //var query = from x in dbContext.Users
            //            join y in dbContext.UserLocations  // we don't have a model class for UserLocation yet
            //            on x.SiteGuid equals y.SiteGuid
            //            where y.FolderName == folderName
            //            select x.SiteId
            //            ;

            throw new NotImplementedException("need to implement model class for UserLocation to make this work");


        }

        public async Task<List<IUserInfo>> GetCrossSiteUserListByEmail(string email)
        {
            var query = from c in dbContext.Users
                        where c.LoweredEmail == email.ToLowerInvariant()
                        orderby c.DisplayName ascending
                        select c;

            var items = await query.ToListAsync<IUserInfo>(); // not sure this will work IUserInfo from SiteUser may need to map it in select

            return items;

        }

        public async Task<int> CountUsers(int siteId, string userNameBeginsWith)
        {
            return await dbContext.Users.CountAsync<SiteUser>(
                x => 
                (
                    x.SiteId == siteId
                    && x.IsDeleted == false
                    && x.AccountApproved == true
                    && (
                    userNameBeginsWith == string.Empty 
                    || x.DisplayName.StartsWith(userNameBeginsWith)
                    )
                )
                );

        }

        public async Task<List<IUserInfo>> GetPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode)
        {
            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query 
                = from x in dbContext.Users
                        .Skip(offset)
                        .Take(pageSize)
                        where 
                        (
                            x.SiteId == siteId
                            && x.IsDeleted == false
                            && x.AccountApproved == true
                            && (
                            userNameBeginsWith == string.Empty
                            || x.DisplayName.StartsWith(userNameBeginsWith)
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
                            LastActivityDate = x.LastActivityDate,
                            LastLoginDate = x.LastLoginDate,
                            LastName= x.LastName,
                            PhoneNumber = x.PhoneNumber,
                            PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                            SiteGuid = x.SiteGuid,
                            SiteId = x.SiteId,
                            State = x.State,
                            TimeZoneId = x.TimeZoneId,
                            Trusted = x.Trusted,
                            UserGuid = x.UserGuid,
                            UserId = x.UserId,
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
      
            var items = await query.ToListAsync<IUserInfo>(); 
            
            return items;
        }

        public async Task<int> CountUsersForAdminSearch(int siteId, string searchInput)
        {
            return await dbContext.Users.CountAsync<SiteUser>(
                x =>
                (
                    x.SiteId == siteId
                    && (
                    searchInput == string.Empty
                    || x.Email.Contains(searchInput)
                    || x.UserName.Contains(searchInput)
                    || x.FirstName.Contains(searchInput)
                    || x.LastName.Contains(searchInput)
                    || x.DisplayName.Contains(searchInput)
                    )
                )
                );
        }

        public async Task<List<IUserInfo>> GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode)
        {
            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in dbContext.Users
                        .Skip(offset)
                        .Take(pageSize)
                  where
                  (
                      x.SiteId == siteId
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
                      LastActivityDate = x.LastActivityDate,
                      LastLoginDate = x.LastLoginDate,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteGuid = x.SiteGuid,
                      SiteId = x.SiteId,
                      State = x.State,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserGuid = x.UserGuid,
                      UserId = x.UserId,
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

            var items = await query.ToListAsync<IUserInfo>();

            return items;
        }

        public async Task<int> CountLockedOutUsers(int siteId)
        {
            return await dbContext.Users.CountAsync<SiteUser>(x => x.SiteId == siteId && x.IsLockedOut == true);
        }

        public async Task<List<IUserInfo>> GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in dbContext.Users
                        .Skip(offset)
                        .Take(pageSize)
                  where
                  (
                      x.SiteId == siteId
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
                      LastActivityDate = x.LastActivityDate,
                      LastLoginDate = x.LastLoginDate,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteGuid = x.SiteGuid,
                      SiteId = x.SiteId,
                      State = x.State,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserGuid = x.UserGuid,
                      UserId = x.UserId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };

            var items = await query.ToListAsync<IUserInfo>();

            return items;
        }

        public async Task<int> CountNotApprovedUsers(int siteId)
        {
            return await dbContext.Users.CountAsync<SiteUser>(x => x.SiteId == siteId && x.AccountApproved == false);

        }

        public async Task<List<IUserInfo>> GetNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in dbContext.Users
                        .Skip(offset)
                        .Take(pageSize)
                  where
                  (
                      x.SiteId == siteId
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
                      LastActivityDate = x.LastActivityDate,
                      LastLoginDate = x.LastLoginDate,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteGuid = x.SiteGuid,
                      SiteId = x.SiteId,
                      State = x.State,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserGuid = x.UserGuid,
                      UserId = x.UserId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };

            var items = await query.ToListAsync<IUserInfo>();

            return items;
        }

        public async Task<bool> EmailExistsInDB(int siteId, string email)
        {
            var found = await Fetch(siteId, email);
            if(found == null) { return false; }
            return true;

        }

        public async Task<bool> EmailExistsInDB(int siteId, int userId, string email)
        { 
            var found = await Fetch(siteId, email);
            if (found == null) { return false; }
            if(found.UserId != userId) { return false; }
            return true;
            
        }

        public bool LoginExistsInDB(int siteId, string loginName)
        {
            SiteUser item
                = dbContext.Users.SingleOrDefault(
                    x => x.SiteId == siteId
                    && x.UserName == loginName
                    );

            if(item == null) { return false; }
            return true;
        }




    }
}
