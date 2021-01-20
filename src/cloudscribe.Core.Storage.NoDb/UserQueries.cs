// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-26
// Last Modified:           2020-12-17 - jk
// 

using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;
using NoDb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    public class UserQueries : IUserQueries, IUserQueriesSingleton
    {
        public UserQueries(
            //IProjectResolver projectResolver,
            IBasicQueries<SiteUser> userQueries,
            IBasicQueries<SiteRole> roleQueries,
            IBasicQueries<UserRole> userRoleQueries,
            IBasicQueries<UserClaim> claimQueries,
            IBasicQueries<UserLogin> loginQueries,
            IBasicQueries<UserToken> tokenQueries,
            IBasicQueries<UserLocation> locationQueries,
            IStoragePathResolver<UserLogin> loginPathResolver,
            IStoragePathResolver<UserToken> tokenPathResolver
            )
        {
            //this.projectResolver = projectResolver;
            this.userQueries = userQueries;
            this.roleQueries = roleQueries;
            this.userRoleQueries = userRoleQueries;
            this.claimQueries = claimQueries;
            this.locationQueries = locationQueries;
            this.loginPathResolver = loginPathResolver;
            this.loginQueries = loginQueries;
            this.tokenQueries = tokenQueries;
            this.tokenPathResolver = tokenPathResolver;
        }

        //private IProjectResolver projectResolver;
        private IBasicQueries<SiteUser> userQueries;
        private IBasicQueries<SiteRole> roleQueries;
        private IBasicQueries<UserRole> userRoleQueries;
        private IBasicQueries<UserClaim> claimQueries;
        private IBasicQueries<UserLogin> loginQueries;
        private IBasicQueries<UserToken> tokenQueries;
        private IBasicQueries<UserLocation> locationQueries;
        private IStoragePathResolver<UserLogin> loginPathResolver;
        private IStoragePathResolver<UserToken> tokenPathResolver;

        //protected string projectId;

        //private async Task EnsureProjectId()
        //{
        //    if (string.IsNullOrEmpty(projectId))
        //    {
        //        projectId = await projectResolver.ResolveProjectId().ConfigureAwait(false);
        //    }

        //}

        #region User

        public async Task<ISiteUser> Fetch(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var item = await userQueries.FetchAsync(
                projectId, 
                userId.ToString(), 
                cancellationToken).ConfigureAwait(false);

            if (item.SiteId != siteId) return null;

            return item;
        }

        public async Task<ISiteUser> Fetch(
            Guid siteId,
            string email,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

           // string loweredEmail = email.ToLowerInvariant();

            return allUsers.Where(
                x => x.SiteId == siteId && x.NormalizedEmail == email
                ).FirstOrDefault();
        }

        public async Task<List<ISiteUser>> GetUsers(
            Guid siteId,
            List<Guid> userIds,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            return allUsers.Where(
                x => x.SiteId == siteId && userIds.Contains(x.Id)
                ).ToList<ISiteUser>();

        }



        public async Task<ISiteUser> FetchByLoginName(
            Guid siteId,
            string userName,
            bool allowEmailFallback,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            //string loweredUserName = userName.ToLowerInvariant();

            return allUsers.Where(
                x => x.SiteId == siteId
                && (
                    (x.NormalizedUserName == userName)
                    || (allowEmailFallback && x.NormalizedEmail == userName)
                    )
                ).FirstOrDefault();
        }

        public async Task<List<IUserInfo>> GetByIPAddress(
            Guid siteId,
            string ipv4Address,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allLocations = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in allUsers
                        join y in allLocations
                        on x.SiteId equals y.SiteId
                        where x.Id == y.UserId && y.IpAddress == ipv4Address
                        select x
                        ;

            var items = query.ToList<IUserInfo>();

            return items;

        }

        //public async Task<List<IUserInfo>> GetCrossSiteUserListByEmail(
        //    string email,
        //    CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    ThrowIfDisposed();
        //    cancellationToken.ThrowIfCancellationRequested();

        //    await EnsureProjectId().ConfigureAwait(false);

        //    var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

        //    //string loweredEmail = email.ToLowerInvariant();

        //    var query = from c in allUsers
        //                where c.NormalizedEmail == email
        //                orderby c.DisplayName ascending
        //                select c;

        //    var items = query.ToList<IUserInfo>();

        //    return items;

        //}

        public async Task<int> CountUsers(
            Guid siteId,
            string userNameBeginsWith,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            return allUsers.Where(
                x =>
                (
                    x.SiteId == siteId
                    //&& x.IsDeleted == false
                    && x.AccountApproved == true
                    && (
                    userNameBeginsWith == string.Empty
                    || x.DisplayName.ToUpper().StartsWith(userNameBeginsWith)
                    )
                )
                ).ToList().Count;

        }

        public async Task<PagedResult<IUserInfo>> GetPage(
            Guid siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var users = allUsers.ToList().AsQueryable();

            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            int offset = (pageSize * pageNumber) - pageSize;

            userNameBeginsWith = userNameBeginsWith.ToUpper();

            IQueryable<IUserInfo> query;
            switch (sortMode)
            {
                case 2:
                    //query = query.OrderBy(sl => sl.LastName).ThenBy(s2 => s2.FirstName).AsQueryable();
                    query
                = from x in users

                  where
                  (
                      x.SiteId == siteId
                     // && x.IsDeleted == false
                      && x.AccountApproved == true
                      && (
                      userNameBeginsWith == string.Empty
                      || x.DisplayName.ToUpper().StartsWith(userNameBeginsWith)
                      )
                  )
                  orderby x.LastName, x.FirstName
                  select new UserInfo
                  {
                      Id = x.Id,
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };



                    break;
                case 1:
                    //query = query.OrderByDescending(sl => sl.CreatedUtc).AsQueryable();

                    query
                = from x in users

                  where
                  (
                      x.SiteId == siteId
                      //&& x.IsDeleted == false
                      && x.AccountApproved == true
                      && (
                      userNameBeginsWith == string.Empty
                      || x.DisplayName.ToUpper().StartsWith(userNameBeginsWith)
                      )
                  )
                  orderby x.CreatedUtc descending
                  select new UserInfo
                  {
                      Id = x.Id,
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


                    break;

                case 0:
                default:
                    //query = query.OrderBy(sl => sl.DisplayName).AsQueryable();

                    query
                = from x in users

                  where
                  (
                      x.SiteId == siteId
                     // && x.IsDeleted == false
                      && x.AccountApproved == true
                      && (
                      userNameBeginsWith == string.Empty
                      || x.DisplayName.ToUpper().StartsWith(userNameBeginsWith)
                      )
                  )
                  orderby x.DisplayName
                  select new UserInfo
                  {
                      Id = x.Id,
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };



                    break;
            }


            var data = query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;

            var result = new PagedResult<IUserInfo>();
            result.Data = data;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = await CountUsers(siteId, userNameBeginsWith, cancellationToken).ConfigureAwait(false);

            return result;

        }


        public async Task<int> CountUsersForAdminSearch(
            Guid siteId,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId   = siteId.ToString();
            var searchTerms = searchInput.Trim().ToUpper().Split(" ");

            var allUsers    = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var users       = allUsers.ToList().AsQueryable();

            IQueryable<IUserInfo> query = users.Where(x => x.SiteId == siteId);

            foreach (var term in searchTerms)
            {
                if (!string.IsNullOrWhiteSpace(term))
                {
                    // Note each term is already in upper case
                    query = query.Where(x =>
                                               ((SiteUser)x).NormalizedEmail.Contains(term)
                                            || ((SiteUser)x).NormalizedUserName.Contains(term)
                                            || (x.FirstName != null && x.FirstName.ToUpper().Contains(term))
                                            || (x.LastName  != null && x.LastName .ToUpper().Contains(term))
                                            ||  x.DisplayName.ToUpper().Contains(term)
                     );
                }
            }

            query = query.Distinct();

            return query.ToList().Count();
        }

        public async Task<PagedResult<IUserInfo>> GetUserAdminSearchPage(
            Guid siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (searchInput == null) searchInput = string.Empty;

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            //allows user to enter multiple words (e.g. to allow full name search)
            var searchTerms = searchInput.Trim().ToUpper().Split(" ");

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var users = allUsers.ToList().AsQueryable();

            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query = users.Where(x => x.SiteId == siteId);

            foreach (var term in searchTerms)
            {
                if (!string.IsNullOrWhiteSpace(term))
                {
                    // Note each term is already in upper case
                    query = query.Where(x =>
                                               ((SiteUser)x).NormalizedEmail.Contains(term)
                                            || ((SiteUser)x).NormalizedUserName.Contains(term)
                                            || (x.FirstName != null && x.FirstName.ToUpper().Contains(term))
                                            || (x.LastName  != null && x.LastName .ToUpper().Contains(term))
                                            || x.DisplayName.ToUpper().Contains(term)
                     );
                }
            }

            query = query.Distinct();

            query = query.Select(x => new UserInfo
                {
                    Id                   = x.Id,
                    AvatarUrl            = x.AvatarUrl,
                    AccountApproved      = x.AccountApproved,
                    CreatedUtc           = x.CreatedUtc,
                    DateOfBirth          = x.DateOfBirth,
                    DisplayInMemberList  = x.DisplayInMemberList,
                    DisplayName          = x.DisplayName,
                    Email                = x.Email,
                    FirstName            = x.FirstName,
                    Gender               = x.Gender,
                    IsLockedOut          = x.IsLockedOut,
                    LastLoginUtc         = x.LastLoginUtc,
                    LastName             = x.LastName,
                    PhoneNumber          = x.PhoneNumber,
                    PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                    SiteId               = x.SiteId,
                    TimeZoneId           = x.TimeZoneId,
                    UserName             = x.UserName,
                    WebSiteUrl           = x.WebSiteUrl
                }
            );

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

            var data = query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;

            return new PagedResult<IUserInfo>
            {
                Data       = data,
                PageNumber = pageNumber,
                PageSize   = pageSize,
                TotalItems = await CountUsersForAdminSearch(siteId, searchInput, cancellationToken).ConfigureAwait(false)
            };
        }

        public async Task<int> CountLockedByAdmin(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            
            return allUsers.Where(
                x => x.SiteId == siteId && x.IsLockedOut == true
                )
                .ToList().Count;
        }

        public async Task<PagedResult<IUserInfo>> GetPageLockedByAdmin(
            Guid siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var users = allUsers.ToList().AsQueryable();

            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in users

                  where
                  (
                      x.SiteId == siteId
                      && x.IsLockedOut == true
                  )
                  orderby x.DisplayName
                  select new UserInfo
                  {
                      Id = x.Id,
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            var data = query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;
            var result = new PagedResult<IUserInfo>();
            result.Data = data;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = await CountLockedByAdmin(siteId, cancellationToken).ConfigureAwait(false);
            return result;

        }

        public async Task<int> CountFutureLockoutEndDate(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            return allUsers.Where(
                x => x.SiteId == siteId
                && x.LockoutEndDateUtc.HasValue
                && x.LockoutEndDateUtc.Value > DateTime.UtcNow
                )
                .ToList().Count;
        }


        public async Task<PagedResult<IUserInfo>> GetPageFutureLockoutEndDate(
            Guid siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var users = allUsers.ToList().AsQueryable();

            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in users

                  where
                  (
                      x.SiteId == siteId
                        && x.LockoutEndDateUtc.HasValue
                        && x.LockoutEndDateUtc.Value > DateTime.UtcNow
                  )
                  orderby x.DisplayName
                  select new UserInfo
                  {
                      Id = x.Id,
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            var data = query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>();

            var result = new PagedResult<IUserInfo>();
            result.Data = data;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = await CountFutureLockoutEndDate(siteId, cancellationToken).ConfigureAwait(false);
            return result;

        }

        public async Task<int> CountUnconfirmedEmail(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            return allUsers.Where(
                x => x.SiteId == siteId
                && x.EmailConfirmed == false
                )
                .ToList().Count;
        }

        public async Task<PagedResult<IUserInfo>> GetPageUnconfirmedEmailUsers(
            Guid siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var users = allUsers.ToList().AsQueryable();

            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in users

                  where
                  (
                      x.SiteId == siteId
                      && x.EmailConfirmed == false
                  )
                  orderby x.DisplayName
                  select new UserInfo
                  {
                      Id = x.Id,
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            var data = query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>();

            var result = new PagedResult<IUserInfo>();
            result.Data = data;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = await CountUnconfirmedEmail(siteId, cancellationToken);
            return result;

        }

        public async Task<int> CountUnconfirmedPhone(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            return allUsers.Where(
                x => x.SiteId == siteId
                && x.PhoneNumberConfirmed == false
                )
                .ToList().Count;
        }

        public async Task<PagedResult<IUserInfo>> GetPageUnconfirmedPhoneUsers(
            Guid siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var users = allUsers.ToList().AsQueryable();

            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in users

                  where
                  (
                      x.SiteId == siteId
                      && x.PhoneNumberConfirmed == false
                  )
                  orderby x.DisplayName
                  select new UserInfo
                  {
                      Id = x.Id,
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            var data = query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>();

            var result = new PagedResult<IUserInfo>();
            result.Data = data;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = await CountUnconfirmedPhone(siteId, cancellationToken).ConfigureAwait(false);
            return result;

        }

        public async Task<int> CountNotApprovedUsers(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            return allUsers.Where(
                x => x.SiteId == siteId && x.AccountApproved == false
                )
                .ToList().Count;

        }

        public async Task<PagedResult<IUserInfo>> GetNotApprovedUsers(
            Guid siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var users = allUsers.ToList().AsQueryable();

            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query
                = from x in users
                  where
                  (
                      x.SiteId == siteId
                      && x.AccountApproved == false
                  )
                  orderby x.DisplayName
                  select new UserInfo
                  {
                      Id = x.Id,
                      AvatarUrl = x.AvatarUrl,
                      AccountApproved = x.AccountApproved,
                      CreatedUtc = x.CreatedUtc,
                      DateOfBirth = x.DateOfBirth,
                      DisplayInMemberList = x.DisplayInMemberList,
                      DisplayName = x.DisplayName,
                      Email = x.Email,
                      FirstName = x.FirstName,
                      Gender = x.Gender,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            var data = query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>();

            var result = new PagedResult<IUserInfo>();
            result.Data = data;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = await CountNotApprovedUsers(siteId, cancellationToken).ConfigureAwait(false);
            return result;

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
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var item = allUsers.Where(
                    x => x.SiteId == siteId
                    && x.UserName == loginName
                    ).SingleOrDefault();

            if (item == null) { return false; }
            return true;
        }

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
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteId == siteId
                && x.NormalizedRoleName == roleName
            );

            return filteredRoles.ToList().Count > 0;
        }

        public async Task<ISiteRole> FetchRole(
            Guid siteId,
            Guid roleId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            return await roleQueries.FetchAsync(
                projectId,
                roleId.ToString(),
                cancellationToken).ConfigureAwait(false);

        }

        public async Task<ISiteRole> FetchRole(
            Guid siteId,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteId == siteId
                && (x.RoleName == roleName || x.NormalizedRoleName == roleName)
            );

            return filteredRoles.FirstOrDefault();

        }

        public async Task<List<string>> GetUserRoles(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteId == siteId
            );

            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in filteredRoles
                        join y in allUserRoles
                        on x.Id equals y.RoleId
                        where y.UserId == userId
                        orderby x.RoleName
                        select x.RoleName
                        ;

            return query.ToList<string>();

        }

        public async Task<int> CountOfRoles(
            Guid siteId,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteId == siteId
                && (
                 (searchInput == "")
                        || x.RoleName.Contains(searchInput)
                        || x.NormalizedRoleName.Contains(searchInput)
                )
            );

            return filteredRoles.ToList().Count;

        }

        public async Task<PagedResult<ISiteRole>> GetRolesBySite(
            Guid siteId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteId == siteId
                && (
                 (searchInput == "")
                        || x.RoleName.Contains(searchInput)
                        || x.NormalizedRoleName.Contains(searchInput)
                )
            );

            int offset = (pageSize * pageNumber) - pageSize;

            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var listQuery = from x in filteredRoles
                            orderby x.NormalizedRoleName ascending
                            select new SiteRole
                            {
                                Id = x.Id,
                                SiteId = x.SiteId,
                                NormalizedRoleName = x.NormalizedRoleName,
                                RoleName = x.RoleName,
                                MemberCount = allUserRoles.Count<UserRole>(u => u.RoleId == x.Id)
                            };

            var data = listQuery
                .Skip(offset)
                .Take(pageSize)
                .ToList<ISiteRole>();

            var result = new PagedResult<ISiteRole>();
            result.Data = data;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = await CountOfRoles(siteId, searchInput, cancellationToken).ConfigureAwait(false);
            return result;

        }

        public async Task<int> CountUsersInRole(
            Guid siteId,
            Guid roleId,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in allUsers
                        join y in allUserRoles
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

                        select x
                        ;

            return query.ToList().Count();

        }

        public async Task<PagedResult<IUserInfo>> GetUsersInRole(
            Guid siteId,
            Guid roleId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);



            var query = from x in allUsers
                        join y in allUserRoles
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
                        select x
                        ;

            int offset = (pageSize * pageNumber) - pageSize;

            var data = query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>();

            var result = new PagedResult<IUserInfo>();
            result.Data = data;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = await CountUsersInRole(siteId, roleId, searchInput, cancellationToken).ConfigureAwait(false);
            return result;

        }

        public async Task<IList<ISiteUser>> GetUsersInRole(
            Guid siteId,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var siteRoles = allRoles.Where(x => x.SiteId == siteId);

            var query = from x in allUsers
                        join y in allUserRoles
                        on x.Id equals y.UserId
                        join z in siteRoles
                        on y.RoleId equals z.Id
                        orderby x.DisplayName
                        where
                            (x.SiteId.Equals(siteId) && z.NormalizedRoleName.Equals(roleName))

                        select x
                        ;

            var items = query
                .ToList<ISiteUser>()
                ;

            return items;
        }

        public async Task<int> CountUsersNotInRole(
            Guid siteId,
            Guid roleId,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var siteRoles = allRoles.Where(x => x.SiteId == siteId);

            var query = from u in allUsers
                        from r in siteRoles
                        join ur in allUserRoles
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

                        select u;

            return query.ToList().Count;

        }

        public async Task<PagedResult<IUserInfo>> GetUsersNotInRole(
            Guid siteId,
            Guid roleId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var siteRoles = allRoles.Where(x => x.SiteId == siteId);

            var query = from u in allUsers
                        from r in siteRoles
                        join ur in allUserRoles
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

            int offset = (pageSize * pageNumber) - pageSize;

            var data = query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>();

            var result = new PagedResult<IUserInfo>();
            result.Data = data;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = await CountUsersNotInRole(siteId, roleId, searchInput, cancellationToken).ConfigureAwait(false);
            return result;

        }


        #endregion

        #region Claims
        
        public async Task<IList<IUserClaim>> GetClaimsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var all = await claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            return all.Where(x =>
                 x.SiteId == siteId
                 && x.UserId == userId
            ).ToList<IUserClaim>();

            //return filtered as IList<IUserClaim>;

        }


        public async Task<IList<ISiteUser>> GetUsersForClaim(
            Guid siteId,
            string claimType,
            string claimValue,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allClaims = await claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredClaims = allClaims.Where(x =>
                 x.SiteId == siteId
                 && x.ClaimType == claimType
                 && x.ClaimValue == claimValue
            );

            var allUsers = await userQueries.GetAllAsync(
                projectId,
                cancellationToken).ConfigureAwait(false);

            var query = from x in allUsers
                        join y in filteredClaims
                        on x.Id equals y.UserId
                        where x.SiteId == siteId
                        orderby x.DisplayName
                        select x
                        ;

            return query.Distinct().ToList<ISiteUser>();


        }


        #endregion

        #region Logins
        
        public async Task<IUserLogin> FindLogin(
            Guid siteId,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var folderPath = await loginPathResolver.ResolvePath(projectId).ConfigureAwait(false);

            if (!Directory.Exists(folderPath)) return null;

            // understand structure of key which is the filename
            //var key = login.UserGuid.ToString()
            //    + "~" + login.SiteGuid.ToString()
            //    + "~" + login.LoginProvider
            //    + "~" + login.ProviderKey;

            var all = await loginQueries.GetAllAsync(projectId, cancellationToken);
            return all.Where(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey).FirstOrDefault();

            //var matchPattern = "*~" + siteId.ToString()
            //    + "~" + loginProvider
            //    + "~" + providerKey;

            //var dir = new DirectoryInfo(folderPath);
            //var matches = dir.GetFiles(matchPattern);
            //var foundFileKey = string.Empty;
            //foreach (var match in matches)
            //{
            //    foundFileKey = Path.GetFileNameWithoutExtension(match.Name);
            //    break; // should only be one so we won't keep interating
            //}

            //if (!string.IsNullOrEmpty(foundFileKey))
            //{
            //    return await loginQueries.FetchAsync(
            //        projectId,
            //        foundFileKey,
            //        cancellationToken).ConfigureAwait(false);
            //}

            //return null;
        }
        
        public async Task<IList<IUserLogin>> GetLoginsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var folderPath = await loginPathResolver.ResolvePath(projectId).ConfigureAwait(false);

            // understand structure of key which is the filename
            //var key = login.UserGuid.ToString()
            //    + "~" + login.SiteGuid.ToString()
            //    + "~" + login.LoginProvider
            //    + "~" + login.ProviderKey;
            var result = new List<IUserLogin>();
            if (!Directory.Exists(folderPath)) return result;

            var matchPattern = "*~" + siteId.ToString() + "~*";

            var dir = new DirectoryInfo(folderPath);
            var matches = dir.GetFiles(matchPattern);

            
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

        #region UserTokens

        public async Task<IUserToken> FindToken(
            Guid siteId,
            Guid userId,
            string loginProvider,
            string name,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectId = siteId.ToString();


            //var folderPath = await tokenPathResolver.ResolvePath(projectId).ConfigureAwait(false);

            //if (!Directory.Exists(folderPath)) return null;

            // understand structure of key which is the filename
            //var key = login.UserGuid.ToString()
            //    + "~" + login.SiteGuid.ToString()
            //    + "~" + login.LoginProvider
            //    + "~" + login.Name;
            var all = await tokenQueries.GetAllAsync(projectId);

            return all.Where(x => x.UserId == userId 
                        && x.LoginProvider == loginProvider 
                        && x.Name == name).FirstOrDefault();




            //var key =
            //    userId.ToString()
            //    + "~" + siteId.ToString()
            //    + "~" + loginProvider
            //    + "~" + name;

            //return await tokenQueries.FetchAsync(
            //        projectId,
            //        key,
            //        cancellationToken).ConfigureAwait(false);






            //var dir = new DirectoryInfo(folderPath);
            //var matches = dir.GetFiles(matchPattern);
            //var foundFileKey = string.Empty;
            //foreach (var match in matches)
            //{
            //    foundFileKey = Path.GetFileNameWithoutExtension(match.Name);
            //    break; // should only be one so we won't keep interating
            //}

            //if (!string.IsNullOrEmpty(foundFileKey))
            //{
            //    return await tokenQueries.FetchAsync(
            //        projectId,
            //        foundFileKey,
            //        cancellationToken).ConfigureAwait(false);
            //}

            //return null;
        }

        public async Task<List<IUserToken>> GetUserTokensByProvider(
           Guid siteId,
           Guid userId,
           string loginProvider,
           CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectId = siteId.ToString();
            var all = await tokenQueries.GetAllAsync(projectId);

            return all.Where(x => x.UserId == userId
                        && x.LoginProvider == loginProvider
                        ).ToList<IUserToken>();
        }



        #endregion

        #region UserLocation

        public async Task<IUserLocation> FetchLocationByUserAndIpv4Address(
            Guid siteId,
            Guid userId,
            long ipv4AddressAsLong,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var all = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in all
                        where x.UserId == userId
                        && x.IpAddressLong == ipv4AddressAsLong
                        select x
                        ;

            return query.FirstOrDefault<UserLocation>();

        }

        public async Task<int> CountUserLocationsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            var result = 0;

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var all = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x => x.UserId == userId);
            result = filtered.ToList().Count;

            return result;

        }

        public async Task<PagedResult<IUserLocation>> GetUserLocationsByUser(
            Guid siteId,
            Guid userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            int offset = (pageSize * pageNumber) - pageSize;

            var all = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = all
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.LastCaptureUtc)
                .Select(p => p)
                .Skip(offset)
                .Take(pageSize)
                ;

            var data = query.ToList<IUserLocation>();
            var result = new PagedResult<IUserLocation>();
            result.Data = data;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = await CountUserLocationsByUser(siteId, userId, cancellationToken).ConfigureAwait(false);
            return result;

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
