// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2015-11-30
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

        #region User

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
                dbContext.Roles.Add(siteRole);
            }
            else
            {
                dbContext.Roles.Update(siteRole);
            }
            
            int rowsAffected = await dbContext.SaveChangesAsync();

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

        public async Task<bool> AddUserToRole(
            int roleId,
            Guid roleGuid,
            int userId,
            Guid userGuid
            )
        {
            UserRole ur = new UserRole();
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
            var query = from x in dbContext.UserRoles
                        where x.UserId == userId
                        select x;

            dbContext.UserRoles.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync();
            return rowsAffected > 0;
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


        public async Task<bool> RoleExists(int siteId, string roleName)
        {
            int count = await dbContext.Roles.CountAsync<SiteRole>(r => r.RoleName == roleName);
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
            return await query.ToListAsync<string>();

        }

        public async Task<int> CountOfRoles(int siteId, string searchInput)
        {
            return await dbContext.Roles.CountAsync<SiteRole>(
                x => x.SiteId == siteId
                && (
                 searchInput == string.Empty
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
                            .Skip(offset)
                            .Take(pageSize)
                            where (
                            x.SiteId == siteId &&
                            (x.DisplayName.Contains(searchInput) || x.RoleName.Contains(searchInput))
                            )
                            orderby x.RoleName ascending
                            select x;

            var items = await listQuery.ToListAsync<ISiteRole>();
            return items;
        }

        #endregion


    }
}
