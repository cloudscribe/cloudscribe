﻿// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:           2020-12-17 - jk
// 


using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public class UserQueries : IUserQueries, IUserQueriesSingleton
    {
        public UserQueries(ICoreDbContextFactory coreDbContextFactory)
        {
            _contextFactory = coreDbContextFactory;
        }

        private readonly ICoreDbContextFactory _contextFactory;

        #region User

        public async Task<ISiteUser> Fetch(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var item
               = await dbContext.Users
               .AsNoTracking()
               .SingleOrDefaultAsync(
                   x => x.SiteId == siteId && x.Id == userId
                   , cancellationToken)
                   .ConfigureAwait(false);

                return item;

            }

               
        }


        public async Task<ISiteUser> Fetch(
            Guid siteId,
            string email,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var item = await dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    x => x.SiteId == siteId && x.NormalizedEmail == email
                    , cancellationToken)
                    .ConfigureAwait(false);

                return item;
            }


            
        }


        public async Task<List<ISiteUser>> GetAllUsersForSite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var list
                = await dbContext.Users
                .AsNoTracking()
                .Where(x => x.SiteId == siteId)
                .ToListAsync<ISiteUser>(cancellationToken).ConfigureAwait(false);

                return list;
            }
        }

        public async Task<List<ISiteUser>> GetAllApprovedUsersForSite(
          Guid siteId,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var list
                = await dbContext.Users
                .AsNoTracking()
                .Where(x => x.SiteId == siteId && x.AccountApproved == true)
                .ToListAsync<ISiteUser>(cancellationToken).ConfigureAwait(false);

                return list;
            }
        }

        public async Task<List<ISiteUser>> GetUsers(
            Guid siteId,
            List<Guid> userIds,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var list
                = await dbContext.Users
                .AsNoTracking()
                .Where(x => x.SiteId == siteId && userIds.Contains(x.Id))
                .ToListAsync<ISiteUser>(cancellationToken).ConfigureAwait(false);

                return list;
            }

            
        }

        public async Task<ISiteUser> FetchByLoginName(
            Guid siteId,
            string userName,
            bool allowEmailFallback,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var item = await dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    x => x.SiteId == siteId
                    && (
                    (x.NormalizedUserName == userName)
                    || (allowEmailFallback && x.NormalizedEmail == userName)
                    ),
                    cancellationToken
                    )
                    .ConfigureAwait(false);

                return item;
            }
        }


        public async Task<ISiteUser> FetchByLoginNameCaseInsensitive(
            Guid siteId,
            string userName,
            bool allowEmailFallback,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var item = await dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    x => x.SiteId == siteId
                    && (
                    (x.NormalizedUserName == userName.ToUpper())
                    || (allowEmailFallback && x.NormalizedEmail == userName.ToUpper())
                    || x.UserName.ToUpper() == userName.ToUpper()
                    ),
                    cancellationToken
                    )
                    .ConfigureAwait(false);

                return item;
            }
        }

        public async Task<List<IUserInfo>> GetByIPAddress(
            Guid siteId,
            string ipv4Address,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.Users
                            join y in dbContext.UserLocations
                            on x.SiteId equals y.SiteId
                            where x.Id == y.UserId && y.IpAddress == ipv4Address
                            select x
                        ;

                var items = await query
                    .AsNoTracking()
                    .ToListAsync<IUserInfo>(cancellationToken)
                    .ConfigureAwait(false);

                return items;
            }

        }

        public async Task<List<IUserInfo>> GetCrossSiteUserListByEmail(
            string email,
            CancellationToken cancellationToken = default(CancellationToken))
        {


            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from c in dbContext.Users
                            where c.NormalizedEmail == email
                            orderby c.DisplayName ascending
                            select c;

                var items = await query
                    .AsNoTracking()
                    .ToListAsync<IUserInfo>(cancellationToken)
                    .ConfigureAwait(false);

                return items;
            }

            

        }

        public async Task<int> CountUsers(
            Guid siteId,
            string userNameBeginsWith,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Users.CountAsync<SiteUser>(
                x =>
                (
                    x.SiteId == siteId
                    && x.AccountApproved == true
                    && (
                    userNameBeginsWith == string.Empty
                    || x.DisplayName.ToUpper().StartsWith(userNameBeginsWith)
                    )
                )
                , cancellationToken
                )
                .ConfigureAwait(false);
            }

        }

        public async Task<PagedResult<IUserInfo>> GetPage(
            Guid siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;
            
            userNameBeginsWith = userNameBeginsWith.ToUpper();

            using (var dbContext = _contextFactory.CreateContext())
            {
                IQueryable<IUserInfo> query;
                switch (sortMode)
                {
                    case 2:
                        query
                    = from x in dbContext.Users

                      where
                      (
                          x.SiteId == siteId
                          && x.AccountApproved == true
                          && (
                          userNameBeginsWith == string.Empty
                          || x.DisplayName.ToUpper().StartsWith(userNameBeginsWith)
                          )
                      )
                      orderby x.LastName, x.FirstName
                      select x;
                        break;
                    case 1:
                        query
                    = from x in dbContext.Users

                      where
                      (
                          x.SiteId == siteId
                          && x.AccountApproved == true
                          && (
                          userNameBeginsWith == string.Empty
                          || x.DisplayName.ToUpper().StartsWith(userNameBeginsWith)
                          )
                      )
                      orderby x.CreatedUtc descending
                      select x;
                        break;

                    case 0:
                    default:
                        query
                    = from x in dbContext.Users

                      where
                      (
                          x.SiteId == siteId
                          && x.AccountApproved == true
                          && (
                          userNameBeginsWith == string.Empty
                          || x.DisplayName.ToUpper().StartsWith(userNameBeginsWith)
                          )
                      )
                      orderby x.DisplayName
                      select x;
                        break;
                }


                var data = await query
                    .AsSingleQuery()
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync<IUserInfo>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<IUserInfo>();
                result.Data = data;
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await CountUsers(siteId, userNameBeginsWith, cancellationToken).ConfigureAwait(false);
                return result;
            }

        }

        public async Task<int> CountUsersForAdminSearch(
            Guid siteId,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (searchInput == null) searchInput = string.Empty;

            //allows user to enter multiple words (e.g. to allow full name search)
            var searchTerms = searchInput.Trim().ToUpper().Split(" ");

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = dbContext.Users.Where(x => x.SiteId == siteId);
                foreach (var term in searchTerms)
                {
                    if (!string.IsNullOrWhiteSpace(term))
                    {
                        query = query.Where(x =>
                                                x.NormalizedEmail.Contains(term)
                                                || x.NormalizedUserName.Contains(term)
                                                || (x.FirstName != null && x.FirstName.ToUpper().Contains(term))
                                                || (x.LastName != null && x.LastName.ToUpper().Contains(term))
                                                || x.DisplayName.ToUpper().Contains(term)
                         );
                    }
                }

                query = query.Distinct();
                return await query.CountAsync<SiteUser>().ConfigureAwait(false);
            }
        }

        public async Task<PagedResult<IUserInfo>> GetUserAdminSearchPage(
            Guid siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (searchInput == null) searchInput = string.Empty;

            //allows user to enter multiple words (e.g. to allow full name search)
            var searchTerms = searchInput.Trim().ToUpper().Split(" ");

            int offset = (pageSize * pageNumber) - pageSize;
            
            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = dbContext.Users.Where(x => x.SiteId == siteId);
                foreach (var term in searchTerms)
                {
                    if (!string.IsNullOrWhiteSpace(term))
                    {
                        // Note each term is already in upper case
                        query = query.Where(x =>
                                                    x.NormalizedEmail.Contains(term)
                                                 || x.NormalizedUserName.Contains(term)
                                                 || (x.FirstName != null && x.FirstName.ToUpper().Contains(term))
                                                 || (x.LastName != null && x.LastName.ToUpper().Contains(term))
                                                 || x.DisplayName.ToUpper().Contains(term)
                         );
                    }
                }

                query = query.Distinct();

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

                var data = await query
                    .AsSingleQuery()
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync<IUserInfo>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<IUserInfo>();
                result.Data = data;
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await CountUsersForAdminSearch(siteId, searchInput, cancellationToken).ConfigureAwait(false);
                return result;
            }
        }

        public async Task<int> CountLockedByAdmin(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Users.CountAsync<SiteUser>(
               x => x.SiteId == siteId && x.IsLockedOut == true,
               cancellationToken)
               .ConfigureAwait(false);
            }

           
        }

        public async Task<PagedResult<IUserInfo>> GetPageLockedByAdmin(
            Guid siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            using (var dbContext = _contextFactory.CreateContext())
            {
                IQueryable<IUserInfo> query
                = from x in dbContext.Users

                  where
                  (
                      x.SiteId == siteId
                      && x.IsLockedOut == true
                  )
                  orderby x.DisplayName
                  select x;

                var data = await query
                    .AsSingleQuery()
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync<IUserInfo>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<IUserInfo>();
                result.Data = data;
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await CountLockedByAdmin(siteId, cancellationToken).ConfigureAwait(false);
                return result;
            }

            

        }

        public async Task<int> CountFutureLockoutEndDate(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Users.CountAsync<SiteUser>(
                x => x.SiteId == siteId
                && x.LockoutEndDateUtc.HasValue
                && x.LockoutEndDateUtc.Value > DateTime.UtcNow
                ,
                cancellationToken)
                .ConfigureAwait(false);
            }

            
        }

        public async Task<PagedResult<IUserInfo>> GetPageFutureLockoutEndDate(
            Guid siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            using (var dbContext = _contextFactory.CreateContext())
            {
                IQueryable<IUserInfo> query
                = from x in dbContext.Users

                  where
                  (
                      x.SiteId == siteId
                        && x.LockoutEndDateUtc.HasValue
                        && x.LockoutEndDateUtc.Value > DateTime.UtcNow
                  )
                  orderby x.DisplayName
                  select x;

                var data = await query
                    .AsSingleQuery()
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync<IUserInfo>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<IUserInfo>();
                result.Data = data;
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await CountFutureLockoutEndDate(siteId, cancellationToken).ConfigureAwait(false);
                return result;
            }

        }

        public async Task<int> CountUnconfirmedEmail(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Users.CountAsync<SiteUser>(
                x => x.SiteId == siteId
                && x.EmailConfirmed == false
                ,
                cancellationToken)
                .ConfigureAwait(false);
            }
        }

        public async Task<PagedResult<IUserInfo>> GetPageUnconfirmedEmailUsers(
            Guid siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            using (var dbContext = _contextFactory.CreateContext())
            {
                IQueryable<IUserInfo> query
                = from x in dbContext.Users

                  where
                  (
                      x.SiteId == siteId
                      && x.EmailConfirmed == false
                  )
                  orderby x.DisplayName
                  select x;

                var data = await query
                    .AsSingleQuery()
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync<IUserInfo>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<IUserInfo>();
                result.Data = data;
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await CountUnconfirmedEmail(siteId, cancellationToken).ConfigureAwait(false);
                return result;

            }
        }

        public async Task<int> CountUnconfirmedPhone(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Users.CountAsync<SiteUser>(
                x => x.SiteId == siteId
                && x.PhoneNumberConfirmed == false
                ,
                cancellationToken)
                .ConfigureAwait(false);
            }
        }

        public async Task<PagedResult<IUserInfo>> GetPageUnconfirmedPhoneUsers(
            Guid siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            using (var dbContext = _contextFactory.CreateContext())
            {
                IQueryable<IUserInfo> query
                = from x in dbContext.Users

                  where
                  (
                      x.SiteId == siteId
                      && x.PhoneNumberConfirmed == false
                  )
                  orderby x.DisplayName
                  select x;

                var data = await query
                    .AsSingleQuery()
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync<IUserInfo>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<IUserInfo>();
                result.Data = data;
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await CountUnconfirmedPhone(siteId, cancellationToken).ConfigureAwait(false);
                return result;

            }
        }

        public async Task<int> CountNotApprovedUsers(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Users.CountAsync<SiteUser>(
                x => x.SiteId == siteId && x.AccountApproved == false,
                cancellationToken)
                .ConfigureAwait(false);
            }

        }

        public async Task<PagedResult<IUserInfo>> GetNotApprovedUsers(
            Guid siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            using (var dbContext = _contextFactory.CreateContext())
            {
                IQueryable<IUserInfo> query
                = from x in dbContext.Users
                  where
                  (
                      x.SiteId == siteId
                      && x.AccountApproved == false
                  )
                  orderby x.DisplayName
                  select x;

                var data = await query
                    .AsSingleQuery()
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync<IUserInfo>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<IUserInfo>();
                result.Data = data;
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await CountNotApprovedUsers(siteId, cancellationToken).ConfigureAwait(false);
                return result;
            }

        }

        public async Task<bool> EmailExistsInDB(
            Guid siteId,
            string email,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var found = await Fetch(siteId, email);
            if (found == null) { return false; }
            return true;

        }

        public async Task<bool> EmailExistsInDB(
            Guid siteId,
            Guid userId,
            string email,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var found = await Fetch(siteId, email);
            if (found == null) { return false; }
            if (found.Id != userId) { return false; }
            return true;

        }

        public async Task<bool> LoginExistsInDB(
            Guid siteId, 
            string loginName,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {

            using (var dbContext = _contextFactory.CreateContext())
            {
                var item = await dbContext.Users.SingleOrDefaultAsync(
                    x => x.SiteId == siteId
                    && x.UserName == loginName
                    ).ConfigureAwait(false);

                if (item == null) { return false; }
                return true;

            }

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
            Guid siteId,
            Guid userId,
            string loginName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var found = await FetchByLoginName(siteId, loginName, false, cancellationToken);
            if (found == null) { return true; }
            if (found.Id == userId) { return true; }
            return false;
        }


        public async Task<string> GetUserNameFromEmail(
            Guid siteId,
            string email,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var found = await Fetch(siteId, email, cancellationToken);
            if (found == null) { return string.Empty; }
            return found.UserName;
        }


        #endregion

        #region Roles
        
        public async Task<bool> RoleExists(
            Guid siteId,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var dbContext = _contextFactory.CreateContext())
            {
                int count = await dbContext.Roles.CountAsync<SiteRole>(
                r => r.SiteId == siteId && r.NormalizedRoleName == roleName
                , cancellationToken)
                .ConfigureAwait(false);

                return count > 0;

            }

        }

        public async Task<ISiteRole> FetchRole(
            Guid siteId,
            Guid roleId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var dbContext = _contextFactory.CreateContext())
            {
                SiteRole item
                = await dbContext.Roles.SingleOrDefaultAsync(
                    x => x.Id == roleId
                    , cancellationToken)
                    .ConfigureAwait(false);

                return item;

            }

        }

        public async Task<ISiteRole> FetchRole(
            Guid siteId,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var dbContext = _contextFactory.CreateContext())
            {
                SiteRole item
                = await dbContext.Roles.SingleOrDefaultAsync(
                    x => x.SiteId == siteId && (x.RoleName == roleName || x.NormalizedRoleName == roleName)
                    , cancellationToken)
                    .ConfigureAwait(false);

                return item;
            }

        }

        public async Task<List<string>> GetUserRoles(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.Roles
                            join y in dbContext.UserRoles
                            on x.Id equals y.RoleId
                            where y.UserId == userId
                            orderby x.RoleName
                            select x.RoleName
                        ;
                return await query
                    .AsNoTracking()
                    .ToListAsync<string>(cancellationToken)
                    .ConfigureAwait(false);

            }

        }

        public async Task<int> CountOfRoles(
            Guid siteId,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Roles.CountAsync<SiteRole>(
                x => x.SiteId.Equals(siteId)
                && (
                 (searchInput == "")
                        || x.RoleName.Contains(searchInput)
                        || x.NormalizedRoleName.Contains(searchInput)
                ),
                cancellationToken
                ).ConfigureAwait(false);

            }

        }

        public async Task<PagedResult<ISiteRole>> GetRolesBySite(
            Guid siteId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            using (var dbContext = _contextFactory.CreateContext())
            {
                var listQuery = from x in dbContext.Roles
                                where (
                                x.SiteId.Equals(siteId) &&
                                (searchInput == "" || x.RoleName.Contains(searchInput) || x.NormalizedRoleName.Contains(searchInput.ToUpper()))
                                )
                                orderby x.NormalizedRoleName ascending
                                select new
                                {
                                    Id = x.Id,
                                    SiteId = x.SiteId,
                                    NormalizedRoleName = x.NormalizedRoleName,
                                    RoleName = x.RoleName,
                                    MemberCount = dbContext.UserRoles.Count<UserRole>(u => u.RoleId == x.Id)
                                };

                var anonList = await listQuery
                    .AsSingleQuery()
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                var result = anonList.Select(x =>
                   new SiteRole
                   {
                       Id = x.Id,
                       SiteId = x.SiteId,
                       NormalizedRoleName = x.NormalizedRoleName,
                       RoleName = x.RoleName,
                       MemberCount = x.MemberCount
                   }

                );

                var data = result.ToList<ISiteRole>();

                var page = new PagedResult<ISiteRole>();
                page.Data = data;
                page.PageNumber = pageNumber;
                page.PageSize = pageSize;
                page.TotalItems = await CountOfRoles(siteId, searchInput, cancellationToken).ConfigureAwait(false);
                return page;

            }

        }


        public async Task<List<ISiteRole>> GetAllRolesBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var dbContext = _contextFactory.CreateContext())
            {
                var listQuery = from x in dbContext.Roles
                                where (
                                x.SiteId.Equals(siteId) 
                                )
                                orderby x.NormalizedRoleName ascending
                                select new
                                {
                                    Id = x.Id,
                                    SiteId = x.SiteId,
                                    NormalizedRoleName = x.NormalizedRoleName,
                                    RoleName = x.RoleName,
                                    MemberCount = dbContext.UserRoles.Count<UserRole>(u => u.RoleId == x.Id)
                                };

                var anonList = await listQuery
                    .AsSingleQuery()
                    .AsNoTracking()
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                var result = anonList.Select(x =>
                   new SiteRole
                   {
                       Id = x.Id,
                       SiteId = x.SiteId,
                       NormalizedRoleName = x.NormalizedRoleName,
                       RoleName = x.RoleName,
                       MemberCount = x.MemberCount
                   }
                );

                return result.ToList<ISiteRole>();
            }
        }


        public async Task<int> CountUsersInRole(
            Guid siteId,
            Guid roleId,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.Users
                            join y in dbContext.UserRoles
                            on x.Id equals y.UserId
                            where (
                                (x.SiteId.Equals(siteId) && y.RoleId.Equals(roleId))
                                && (
                                    (searchInput == "")
                                    || x.Email.Contains(searchInput)
                                    || x.DisplayName.Contains(searchInput)
                                    || x.UserName.Contains(searchInput)
                                    || (x.FirstName != null && x.FirstName.Contains(searchInput))
                                    || (x.LastName != null && x.LastName.Contains(searchInput))
                                )
                                )
                            select 1
                        ;

                return await query.CountAsync<int>(cancellationToken).ConfigureAwait(false);
            }

        }


        public async Task<PagedResult<IUserInfo>> GetUsersInRole(
            Guid siteId,
            Guid roleId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.Users
                            join y in dbContext.UserRoles
                            on x.Id equals y.UserId
                            orderby x.DisplayName
                            where (
                                (x.SiteId.Equals(siteId) && y.RoleId.Equals(roleId))
                                && (
                                    (searchInput == "")
                                    || x.Email.Contains(searchInput)
                                    || x.DisplayName.Contains(searchInput)
                                    || x.UserName.Contains(searchInput)
                                    || (x.FirstName != null && x.FirstName.Contains(searchInput))
                                    || (x.LastName != null && x.LastName.Contains(searchInput))
                                )
                                )
                            select x;

                var data = await query
                    .AsSingleQuery()
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync<IUserInfo>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<IUserInfo>();
                result.Data = data;
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await CountUsersInRole(siteId, roleId, searchInput, cancellationToken).ConfigureAwait(false);
                return result;
            }

        }

        public async Task<IList<ISiteUser>> GetUsersInRole(
            Guid siteId,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.Users
                            join y in dbContext.UserRoles
                            on x.Id equals y.UserId
                            join z in dbContext.Roles
                            on y.RoleId equals z.Id
                            orderby x.DisplayName
                            where
                                (x.SiteId.Equals(siteId) && z.NormalizedRoleName.Equals(roleName))

                            select x
                       ;

                var items = await query
                    .AsNoTracking()
                    .ToListAsync<ISiteUser>(cancellationToken)
                    .ConfigureAwait(false);

                return items;

            }
        }

        public async Task<int> CountUsersNotInRole(
            Guid siteId,
            Guid roleId,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from u in dbContext.Users
                            from r in dbContext.Roles
                            join ur in dbContext.UserRoles
                            on new { RoleId = r.Id, UserId = u.Id } equals new { ur.RoleId, ur.UserId } into t
                            from t2 in t.DefaultIfEmpty()
                            where (
                            u.SiteId == siteId
                            && r.SiteId == siteId
                            && r.Id == roleId
                            && (
                                    (searchInput == "")
                                    || u.Email.Contains(searchInput)
                                    || u.DisplayName.Contains(searchInput)
                                    || u.UserName.Contains(searchInput)
                                    || (u.FirstName != null && u.FirstName.Contains(searchInput))
                                    || (u.LastName != null && u.LastName.Contains(searchInput))
                                )

                            && t2 == null
                            )

                            select 1;

                return await query.CountAsync<int>(cancellationToken).ConfigureAwait(false);
            }

        }

        public async Task<PagedResult<IUserInfo>> GetUsersNotInRole(
            Guid siteId,
            Guid roleId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            int offset = (pageSize * pageNumber) - pageSize;
            // logging a warning DefaultIfEmpty() could not be translated and will be evaluated locally

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from u in dbContext.Users
                            from r in dbContext.Roles
                            join ur in dbContext.UserRoles
                            on new { RoleId = r.Id, UserId = u.Id } equals new { ur.RoleId, ur.UserId } into t
                            from t2 in t.DefaultIfEmpty()
                            where (
                            u.SiteId == siteId
                            && r.SiteId == siteId
                            && r.Id == roleId
                            && (
                                    (searchInput == "")
                                    || u.Email.Contains(searchInput)
                                    || u.DisplayName.Contains(searchInput)
                                    || u.UserName.Contains(searchInput)
                                    || (u.FirstName != null && u.FirstName.Contains(searchInput))
                                    || (u.LastName != null && u.LastName.Contains(searchInput))
                                )

                            && t2 == null
                            )
                            orderby u.DisplayName
                            select u;

                var data = await query
                    .AsSingleQuery()
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync<IUserInfo>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<IUserInfo>();
                result.Data = data;
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await CountUsersNotInRole(siteId, roleId, searchInput, cancellationToken).ConfigureAwait(false);
                return result;
            }

        }


        #endregion

        #region Claims
        
        public async Task<IList<IUserClaim>> GetClaimsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from l in dbContext.UserClaims
                            where l.SiteId == siteId && l.UserId == userId

                            select l;
                var items = await query
                    .AsNoTracking()
                    .ToListAsync<IUserClaim>(cancellationToken)
                    .ConfigureAwait(false);
                return items;

            }

        }

        public async Task<IList<ISiteUser>> GetUsersForClaim(
            Guid siteId,
            string claimType,
            string claimValue,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.Users
                            join y in dbContext.UserClaims
                            on x.Id equals y.UserId
                            where x.SiteId == siteId
                            && y.ClaimType == claimType
                            && y.ClaimValue == claimValue
                            orderby x.DisplayName
                            select x
                        ;

                var items = await query
                    .AsNoTracking()
                    .Distinct()
                    .ToListAsync<ISiteUser>(cancellationToken)
                    .ConfigureAwait(false);

                return items;

            }

            
        }


        #endregion

        #region Logins
        
        public async Task<IUserLogin> FindLogin(
            Guid siteId,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from l in dbContext.UserLogins
                            where (
                            l.SiteId == siteId
                            && l.LoginProvider == loginProvider
                            && l.ProviderKey == providerKey
                            )
                            select l;

                var items = await query
                    .AsNoTracking()
                    .SingleOrDefaultAsync<UserLogin>(cancellationToken)
                    .ConfigureAwait(false);

                return items;
            }

        }
        
        public async Task<IList<IUserLogin>> GetLoginsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from l in dbContext.UserLogins
                            where (
                            l.SiteId == siteId
                            && l.UserId == userId
                            )
                            select l;

                var items = await query
                    .AsNoTracking()
                    .ToListAsync<UserLogin>(cancellationToken)
                    .ConfigureAwait(false);

                return items.ToList<IUserLogin>();
            }

        }

        #endregion

        #region UserTokens

        public async Task<IUserToken> FindToken(
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

                var items = await query
                    .AsNoTracking()
                    .SingleOrDefaultAsync<UserToken>(cancellationToken)
                    .ConfigureAwait(false);

                return items;
            }

        }

        public async Task<List<IUserToken>> GetUserTokensByProvider(
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

                var items = await query
                    .AsNoTracking()
                    .ToListAsync<IUserToken>(cancellationToken)
                    .ConfigureAwait(false);

                return items;

            }
 
        }

        #endregion

        #region UserLocation

        public async Task<IUserLocation> FetchLocationByUserAndIpv4Address(
            Guid siteId,
            Guid userId,
            long ipv4AddressAsLong,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.UserLocations
                            where x.UserId == userId
                            && x.IpAddressLong == ipv4AddressAsLong
                            select x
                        ;

                return await query
                    .AsNoTracking()
                    .FirstOrDefaultAsync<UserLocation>(cancellationToken)
                    .ConfigureAwait(false);
            }

        }
        
        public async Task<int> CountUserLocationsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.UserLocations
                .Where(x => x.UserId == userId)
                .CountAsync<UserLocation>(cancellationToken);
            }

        }

        public async Task<PagedResult<IUserLocation>> GetUserLocationsByUser(
            Guid siteId,
            Guid userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            var result = new PagedResult<IUserLocation>();
            
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = dbContext.UserLocations
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.LastCaptureUtc)
                .Skip(offset)
                .Take(pageSize);

                var data = await query
                    .AsNoTracking()
                    .AsSingleQuery()
                    .ToListAsync<UserLocation>(cancellationToken)
                    .ConfigureAwait(false);

                result.Data = data.ToList<IUserLocation>();
            }

            result.TotalItems = await CountUserLocationsByUser(siteId, userId, cancellationToken).ConfigureAwait(false);
            return result;
        }

        #endregion
    }
}
