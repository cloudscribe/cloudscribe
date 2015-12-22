// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2015-12-22
// 


using cloudscribe.Core.Models;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> Save(ISiteUser user)
        {
            if (user == null) { return false; }
            if (user.SiteId == -1) { throw new ArgumentException("user must have a siteid"); }
            if (user.SiteGuid == Guid.Empty) { throw new ArgumentException("user must have a siteguid"); }

            SiteUser siteUser = SiteUser.FromISiteUser(user);
            if(siteUser.UserId == -1)
            {
                siteUser.UserId = 0; //EF needs it to be zero in order to generate
                dbContext.Users.Add(siteUser);
            }
            else
            {
                bool tracking = dbContext.ChangeTracker.Entries<SiteUser>().Any(x => x.Entity.UserId == siteUser.UserId);
                if (!tracking)
                {
                    dbContext.Users.Update(siteUser);
                }

            }

            int rowsAffected = await dbContext.SaveChangesAsync();

            if(user.UserId == -1)
            {
                user.UserId = siteUser.UserId;
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

        public async Task<bool> Delete(int siteId, int userId)
        {
            var result = false;
            var itemToRemove = await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId && x.SiteId == siteId);
            if (itemToRemove != null)
            {
                result = await DeleteLoginsByUser(itemToRemove.SiteId, itemToRemove.Id, false);
                result = await DeleteClaimsByUser(itemToRemove.SiteId, itemToRemove.Id, false);
                result = await DeleteUserRoles(itemToRemove.UserId, false);

                
                dbContext.Users.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;

        }

        public async Task<bool> DeleteUsersBySite(int siteId)
        {
            bool result = await DeleteLoginsBySite(siteId);
            result = await DeleteClaimsBySite(siteId);
            result = await DeleteUserRolesBySite(siteId);

            var query = from x in dbContext.Users.Where(x => x.SiteId == siteId)
                        select x;

            dbContext.Users.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync();
            result = rowsAffected > 0;

            return result;
        }

        public async Task<bool> FlagAsDeleted(int userId)
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(x => x.UserId == userId);

            if(item == null) { return false; }

            item.IsDeleted = true;

            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public async Task<bool> FlagAsNotDeleted(int userId)
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(x => x.UserId == userId);

            if (item == null) { return false; }

            item.IsDeleted = false;

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

            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> UpdateFailedPasswordAttemptCount(Guid userGuid, int failedPasswordAttemptCount)
        {
            SiteUser item
                = await dbContext.Users.SingleOrDefaultAsync(x => x.UserGuid == userGuid);

            if (item == null) { return false; }
            
            item.FailedPasswordAttemptCount = failedPasswordAttemptCount;
           
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
                = await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.SiteId == siteId && x.UserId == userId);

            return item;
        }

        public async Task<ISiteUser> Fetch(int siteId, Guid userGuid)
        {
            SiteUser item
                = await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.SiteId == siteId && x.UserGuid == userGuid);

            return item;
        }

        public async Task<ISiteUser> FetchByConfirmationGuid(int siteId, Guid confirmGuid)
        {
            SiteUser item
                = await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.SiteId == siteId && x.RegisterConfirmGuid == confirmGuid);

            return item;
        }

        public async Task<ISiteUser> Fetch(int siteId, string email)
        {
            string loweredEmail = email.ToLowerInvariant();
            SiteUser item
                = await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.SiteId == siteId && x.LoweredEmail == loweredEmail);

            return item;
        }

        public async Task<ISiteUser> FetchByLoginName(int siteId, string userName, bool allowEmailFallback)
        {
            string loweredUserName = userName.ToLowerInvariant();

            SiteUser item
                = await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(
                    x => x.SiteId == siteId 
                    && (
                    (x.UserName == userName) 
                    || (allowEmailFallback && x.LoweredEmail == loweredUserName)
                    )
                    );

            return item;
        }

        public async Task<List<IUserInfo>> GetByIPAddress(Guid siteGuid, string ipv4Address)
        {
            var query = from x in dbContext.Users
                        join y in dbContext.UserLocations  
                        on x.SiteGuid equals y.SiteGuid
                        where x.UserGuid == y.UserGuid && y.IpAddress == ipv4Address
                        select x
                        ;

            var items = await query.AsNoTracking().ToListAsync<IUserInfo>(); 

            return items;

        }

        public async Task<List<IUserInfo>> GetCrossSiteUserListByEmail(string email)
        {
            string loweredEmail = email.ToLowerInvariant();

            var query = from c in dbContext.Users
                        where c.LoweredEmail == loweredEmail
                        orderby c.DisplayName ascending
                        select c;

            var items = await query.AsNoTracking().ToListAsync<IUserInfo>(); 

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
                      x.SiteId == siteId
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



                    break;
                case 1:
                    //query = query.OrderByDescending(sl => sl.CreatedUtc).AsQueryable();

                    query
                = from x in dbContext.Users

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


                    break;

                case 0:
                default:
                    //query = query.OrderBy(sl => sl.DisplayName).AsQueryable();

                    query
                = from x in dbContext.Users

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



                    break;
            }

            
            return await query.AsNoTracking().Skip(offset).Take(pageSize).ToListAsync<IUserInfo>(); 
            
            
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
            
            return await query.AsNoTracking().Skip(offset).Take(pageSize).ToListAsync<IUserInfo>();

            
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

           
            return await query.AsNoTracking().Skip(offset).Take(pageSize).ToListAsync<IUserInfo>();
            
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

           
            return await query.AsNoTracking().Skip(offset).Take(pageSize).ToListAsync<IUserInfo>();
            
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


        /// <summary>
        /// available only if the found user matches the passed in one
        /// or if not found
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public async Task<bool> LoginIsAvailable(int siteId, int userId, string loginName)
        {
            var found = await FetchByLoginName(siteId, loginName, false);
            if (found == null) { return true; }
            if (found.UserId == userId) { return true; }
            return false;
        }


        public async Task<string> GetUserNameFromEmail(int siteId, string email)
        {
            var found = await Fetch(siteId, email);
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
        public async Task<bool> SaveRole(ISiteRole role)
        {
            if (role == null) { return false; }
            if (role.SiteId == -1) { throw new ArgumentException("SiteId must be provided"); }
            if (role.SiteGuid == Guid.Empty) { throw new ArgumentException("SiteGuid must be provided"); }
            
            SiteRole siteRole = SiteRole.FromISiteRole(role); 
            if(siteRole.RoleId == -1)
            {
                siteRole.RoleId = 0;
                if(siteRole.RoleName.Length == 0)
                {
                    siteRole.RoleName = siteRole.DisplayName;
                }
                dbContext.Roles.Add(siteRole);
            }
            else
            {
                bool tracking = dbContext.ChangeTracker.Entries<SiteRole>().Any(x => x.Entity.RoleId == siteRole.RoleId);
                if (!tracking)
                {
                    dbContext.Roles.Update(siteRole);
                }

            }

            int rowsAffected = await dbContext.SaveChangesAsync();

            if(role.RoleId == -1)
            {
                //update the original with the new keys on insert
                role.RoleId = siteRole.RoleId;
                role.RoleGuid = siteRole.RoleGuid;
            }

            return rowsAffected > 0;

        }

        public async Task<bool> DeleteRole(int roleId)
        {
            var result = false;
            var itemToRemove = await dbContext.Roles.SingleOrDefaultAsync(x => x.RoleId == roleId);
            if (itemToRemove != null)
            {
                dbContext.Roles.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;
        }

        public async Task<bool> DeleteRolesBySite(int siteId)
        {
            var result = false;
            var query = from r in dbContext.Roles.Where(x => x.SiteId == siteId)
                        select r;
            
            dbContext.Roles.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync();
            result = rowsAffected > 0;
            
            return result;
        }

        public async Task<bool> AddUserToRole(
            int roleId,
            Guid roleGuid,
            int userId,
            Guid userGuid
            )
        {
            UserRole ur = new UserRole();
            ur.Id = 0;
            ur.RoleGuid = roleGuid;
            ur.RoleId = roleId;
            ur.UserGuid = userGuid;
            ur.UserId = userId;

            dbContext.UserRoles.Add(ur);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public async Task<bool> RemoveUserFromRole(int roleId, int userId)
        {
            var result = false;
            var itemToRemove = await dbContext.UserRoles.SingleOrDefaultAsync(
                x => x.RoleId == roleId && x.UserId == userId);
            if (itemToRemove != null)
            {
                dbContext.UserRoles.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;
        }

        public async Task<bool> DeleteUserRoles(int userId)
        {
            return await DeleteUserRoles(userId, true);
        }

        public async Task<bool> DeleteUserRoles(int userId, bool saveChanges)
        {
            var query = from x in dbContext.UserRoles
                        where x.UserId == userId
                        select x;

            dbContext.UserRoles.RemoveRange(query);
            if(saveChanges)
            {
                int rowsAffected = await dbContext.SaveChangesAsync();
                return rowsAffected > 0;
            }
            return true;
        }

        public async Task<bool> DeleteUserRolesByRole(int roleId)
        {
            var query = from x in dbContext.UserRoles
                        where x.RoleId == roleId
                        select x;

            dbContext.UserRoles.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUserRolesBySite(int siteId)
        {
            var query = from x in dbContext.UserRoles
                        join y in dbContext.Roles on x.RoleId equals y.RoleId
                        where y.SiteId == siteId
                        select x;

            dbContext.UserRoles.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync();
            return rowsAffected > 0;
        }


        public async Task<bool> RoleExists(int siteId, string roleName)
        {
            int count = await dbContext.Roles.CountAsync<SiteRole>(r => r.SiteId == siteId && r.RoleName == roleName);
            return count > 0;
        }

        public async Task<ISiteRole> FetchRole(int roleId)
        {
            SiteRole item
                = await dbContext.Roles.SingleOrDefaultAsync(x => x.RoleId == roleId);

            return item;
        }

        public async Task<ISiteRole> FetchRole(int siteId, string roleName)
        {
            SiteRole item
                = await dbContext.Roles.SingleOrDefaultAsync(
                    x => x.SiteId == siteId && x.RoleName == roleName);

            return item;

        }

        public async Task<List<string>> GetUserRoles(int siteId, int userId)
        {
            var query = from x in dbContext.Roles
                        join y in dbContext.UserRoles
                        on x.RoleId equals y.RoleId
                        where y.UserId == userId
                        orderby x.RoleName
                        select x.RoleName
                        ;
            return await query.AsNoTracking().ToListAsync<string>();

        }

        public async Task<int> CountOfRoles(int siteId, string searchInput)
        {
            return await dbContext.Roles.CountAsync<SiteRole>(
                x => x.SiteId.Equals(siteId)
                && (
                 (searchInput == "")
                        || x.DisplayName.Contains(searchInput)
                        || x.RoleName.Contains(searchInput)
                )
                );

        }

        public async Task<IList<ISiteRole>> GetRolesBySite(
            int siteId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var listQuery = from x in dbContext.Roles
                            where (
                            x.SiteId.Equals(siteId) &&
                            (searchInput == "" || x.DisplayName.Contains(searchInput) || x.RoleName.Contains(searchInput))
                            )
                            orderby x.RoleName ascending
                            select new SiteRole {
                                RoleId = x.RoleId,
                                RoleGuid = x.RoleGuid,
                                SiteId = x.SiteId,
                                SiteGuid = x.SiteGuid,
                                RoleName = x.RoleName,
                                DisplayName = x.DisplayName,
                                MemberCount = dbContext.UserRoles.Count<UserRole>(u => u.RoleId == x.RoleId)
                            };

            return await listQuery.AsNoTracking().Skip(offset).Take(pageSize).ToListAsync<ISiteRole>();
            
        }


        public async Task<int> CountUsersInRole(int siteId, int roleId, string searchInput)
        {
            var query = from x in dbContext.Users
                        join y in dbContext.UserRoles
                        on x.UserId equals y.UserId
                        where (
                            (x.SiteId.Equals(siteId) && y.RoleId.Equals(roleId))
                            && (
                                (searchInput == "")
                                || x.Email.Contains(searchInput)
                                || x.DisplayName.Contains(searchInput)
                                || x.UserName.Contains(searchInput)
                                || x.FirstName.Contains(searchInput)
                                || x.LastName.Contains(searchInput)
                            )
                            )
                        select x.UserId
                        ;

            return await query.CountAsync<int>();

        }


        public async Task<IList<IUserInfo>> GetUsersInRole(
            int siteId,
            int roleId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var query = from x in dbContext.Users
                        join y in dbContext.UserRoles
                        on x.UserId equals y.UserId
                        orderby x.DisplayName 
                        where (
                            (x.SiteId.Equals(siteId) && y.RoleId.Equals(roleId))
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

          
            return await query.AsNoTracking().Skip(offset).Take(pageSize).ToListAsync<IUserInfo>(); 

           

        }

        public async Task<IList<ISiteUser>> GetUsersInRole(
            int siteId,
            string roleName)
        {
            var query = from x in dbContext.Users
                        join y in dbContext.UserRoles
                        on x.UserId equals y.UserId
                        join z in dbContext.Roles
                        on y.RoleId equals z.RoleId
                        orderby x.DisplayName
                        where 
                            (x.SiteId.Equals(siteId) && z.RoleName.Equals(roleName))
                            
                        select x
                        ;

            var items = await query.AsNoTracking().ToListAsync<ISiteUser>(); 

            return items;
        }

        public async Task<int> CountUsersNotInRole(int siteId, int roleId, string searchInput)
        {
            var query = from u in dbContext.Users
                        from r in dbContext.Roles
                        join ur in dbContext.UserRoles
                        on new { r.RoleId, u.UserId } equals new { ur.RoleId, ur.UserId } into t
                        from t2 in t.DefaultIfEmpty()
                        where (
                        u.SiteId == siteId
                        && r.SiteId == siteId
                        && r.RoleId == roleId
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
   
                        select u.UserId;

            return await query.CountAsync<int>();

        }

        public async Task<IList<IUserInfo>> GetUsersNotInRole(
            int siteId,
            int roleId,
            string searchInput,
            int pageNumber,
            int pageSize)
        {
            int offset = (pageSize * pageNumber) - pageSize;
            // it took me a lot of tries to figure out how to do this query
            // it works but is still logging a warning DefaultIfEmpty() could not be translated and will be evaluated locally
            var query = from u in dbContext.Users
                        from r in dbContext.Roles
                        join ur in dbContext.UserRoles
                        on new { r.RoleId, u.UserId } equals new { ur.RoleId, ur.UserId } into t
                        from t2 in t.DefaultIfEmpty()
                        where (
                        u.SiteId == siteId
                        && r.SiteId == siteId
                        && r.RoleId == roleId
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
                        
            return await query.AsNoTracking().Skip(offset).Take(pageSize).ToListAsync<IUserInfo>(); 
            
        }


        #endregion

        #region Claims

        public async Task<bool> SaveClaim(IUserClaim userClaim)
        {
            if (userClaim == null) { return false; }

            UserClaim claim = UserClaim.FromIUserClaim(userClaim);
            if(claim.Id == -1)
            {
                claim.Id = 0;
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

            int rowsAffected = await dbContext.SaveChangesAsync();
            if(userClaim.Id == -1)
            {
                //update the original on insert
                userClaim.Id = claim.Id;
            }

            return rowsAffected > 0;

        }

        public async Task<bool> DeleteClaim(int id)
        {
            var result = false;
            var itemToRemove = await dbContext.UserClaims.SingleOrDefaultAsync(x => x.Id == id);
            if (itemToRemove != null)
            {
                dbContext.UserClaims.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;

        }

        public async Task<bool> DeleteClaimsByUser(int siteId, string userId)
        {
            return await DeleteClaimsByUser(siteId, userId, true);

        }

        public async Task<bool> DeleteClaimsByUser(int siteId, string userId, bool saveChanges)
        {
            var query = from x in dbContext.UserClaims
                        where (
                        (siteId == -1 || x.SiteId == siteId)
                        && x.UserId == userId
                        )
                        select x;

            dbContext.UserClaims.RemoveRange(query);
            if(saveChanges)
            {
                int rowsAffected = await dbContext.SaveChangesAsync();
                return rowsAffected > 0;
            }
            return true;

        }

        public async Task<bool> DeleteClaimByUser(int siteId, string userId, string claimType)
        {
            var query = from x in dbContext.UserClaims
                        where (
                        (siteId == -1 || x.SiteId == siteId)
                        && (x.UserId == userId && x.ClaimType == claimType)
                        )
                        select x;

            dbContext.UserClaims.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync();
            return rowsAffected > 0;

        }

        public async Task<bool> DeleteClaimsBySite(int siteId)
        {
            var query = from x in dbContext.UserClaims
                        where x.SiteId == siteId
                        select x;

            dbContext.UserClaims.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync();
            return rowsAffected > 0;

        }

        public async Task<IList<IUserClaim>> GetClaimsByUser(int siteId, string userId)
        {
            var query = from l in dbContext.UserClaims
                        where l.SiteId == siteId && l.UserId == userId
                        
                        select l;
            var items = await query.AsNoTracking().ToListAsync<IUserClaim>();
            return items;

        }

        public async Task<IList<ISiteUser>> GetUsersForClaim(
            int siteId,
            string claimType,
            string claimValue)
        {
            var query = from x in dbContext.Users
                        join y in dbContext.UserClaims
                        on x.UserGuid.ToString() equals y.UserId
                        where x.SiteId == siteId 
                        orderby x.DisplayName
                        select x
                        ;

            var items = await query.AsNoTracking().ToListAsync<ISiteUser>(); 

            return items;
        }


        #endregion

        #region Logins

        public async Task<bool> CreateLogin(IUserLogin userLogin)
        {
            if (userLogin == null) { return false; }
            if (userLogin.LoginProvider.Length == -1) { return false; }
            if (userLogin.ProviderKey.Length == -1) { return false; }
            if (userLogin.UserId.Length == -1) { return false; }

            UserLogin login = UserLogin.FromIUserLogin(userLogin);
            
            dbContext.UserLogins.Add(login);
            
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public async Task<IUserLogin> FindLogin(
            int siteId,
            string loginProvider,
            string providerKey)
        {
            var query = from l in dbContext.UserLogins
                        where (
                        l.SiteId == siteId
                        && l.LoginProvider == loginProvider
                        && l.ProviderKey == providerKey
                        )
                        select l;

            var items = await query.SingleOrDefaultAsync<IUserLogin>();

            return items;
        }

        public async Task<bool> DeleteLogin(
            int siteId,
            string loginProvider,
            string providerKey,
            string userId)
        {
            var query = from l in dbContext.UserLogins
                        where (
                        l.SiteId == siteId
                        && l.LoginProvider == loginProvider
                        && l.ProviderKey == providerKey
                        )
                        select l;

            dbContext.UserLogins.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync();
            return rowsAffected > 0;

        }

        public async Task<bool> DeleteLoginsByUser(int siteId, string userId)
        {
            return await DeleteLoginsByUser(siteId, userId, true);

        }

        public async Task<bool> DeleteLoginsByUser(int siteId, string userId, bool saveChanges)
        {
            var query = from l in dbContext.UserLogins
                        where (
                        l.SiteId == siteId
                        && l.UserId == userId
                        )
                        select l;

            dbContext.UserLogins.RemoveRange(query);
            if(saveChanges)
            {
                int rowsAffected = await dbContext.SaveChangesAsync();
                return rowsAffected > 0;
            }
            return true;

        }

        public async Task<bool> DeleteLoginsBySite(int siteId)
        {
            var query = from l in dbContext.UserLogins
                        where (l.SiteId == siteId)
                        select l;

            dbContext.UserLogins.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync();
            return rowsAffected > 0;

        }

        public async Task<IList<IUserLogin>> GetLoginsByUser(
            int siteId,
            string userId)
        {
            var query = from l in dbContext.UserLogins
                        where (
                        l.SiteId == siteId
                        && l.UserId == userId
                        )
                        select l;

            var items = await query.AsNoTracking().ToListAsync<IUserLogin>();

            return items;

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
