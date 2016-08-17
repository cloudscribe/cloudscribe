// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-26
// Last Modified:           2016-08-17
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
    public class UserQueries : IUserQueries
    {
        public UserQueries(
            //IProjectResolver projectResolver,
            IBasicQueries<SiteUser> userQueries,
            IBasicQueries<SiteRole> roleQueries,
            IBasicQueries<UserRole> userRoleQueries,
            IBasicQueries<UserClaim> claimQueries,
            IBasicQueries<UserLogin> loginQueries,
            IBasicQueries<UserLocation> locationQueries,
            IStoragePathResolver<UserLogin> loginPathResolver
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
        }

        //private IProjectResolver projectResolver;
        private IBasicQueries<SiteUser> userQueries;
        private IBasicQueries<SiteRole> roleQueries;
        private IBasicQueries<UserRole> userRoleQueries;
        private IBasicQueries<UserClaim> claimQueries;
        private IBasicQueries<UserLogin> loginQueries;
        private IBasicQueries<UserLocation> locationQueries;
        private IStoragePathResolver<UserLogin> loginPathResolver;

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
                    && x.IsDeleted == false
                    && x.AccountApproved == true
                    && (
                    userNameBeginsWith == string.Empty
                    || x.DisplayName.StartsWith(userNameBeginsWith)
                    )
                )
                ).ToList().Count;

        }

        public async Task<List<IUserInfo>> GetPage(
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
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
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
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
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
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };



                    break;
            }


            return query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;


        }


        public async Task<int> CountUsersForAdminSearch(
            Guid siteId,
            string searchInput,
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
                    && (
                    searchInput == string.Empty
                    || x.Email.Contains(searchInput)
                    || x.UserName.Contains(searchInput)
                    || x.FirstName.Contains(searchInput)
                    || x.LastName.Contains(searchInput)
                    || x.DisplayName.Contains(searchInput)
                    )
                )

                )
                .ToList().Count;
        }

        public async Task<List<IUserInfo>> GetUserAdminSearchPage(
            Guid siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
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

            IQueryable<IUserInfo> query
                = from x in users

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
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
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

            return query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;


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

        public async Task<List<IUserInfo>> GetPageLockedByAdmin(
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
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            return query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;

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


        public async Task<List<IUserInfo>> GetPageFutureLockoutEndDate(
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
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            return query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;

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

        public async Task<List<IUserInfo>> GetPageUnconfirmedEmailUsers(
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
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            return query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;

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

        public async Task<List<IUserInfo>> GetPageUnconfirmedPhoneUsers(
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
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            return query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;

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

        public async Task<List<IUserInfo>> GetNotApprovedUsers(
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
                      IsDeleted = x.IsDeleted,
                      IsLockedOut = x.IsLockedOut,
                      LastLoginUtc = x.LastLoginUtc,
                      LastName = x.LastName,
                      PhoneNumber = x.PhoneNumber,
                      PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                      SiteId = x.SiteId,
                      TimeZoneId = x.TimeZoneId,
                      Trusted = x.Trusted,
                      UserName = x.UserName,
                      WebSiteUrl = x.WebSiteUrl

                  };


            return query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;

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

        public async Task<IList<ISiteRole>> GetRolesBySite(
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

            return listQuery
                .Skip(offset)
                .Take(pageSize)
                .ToList<ISiteRole>()
                ;

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
                                || x.FirstName.Contains(searchInput)
                                || x.LastName.Contains(searchInput)
                            )
                            )

                        select x
                        ;

            return query.ToList().Count();

        }

        public async Task<IList<IUserInfo>> GetUsersInRole(
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
                                || x.FirstName.Contains(searchInput)
                                || x.LastName.Contains(searchInput)
                            )
                            )
                        select x
                        ;

            int offset = (pageSize * pageNumber) - pageSize;

            return query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;

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
                                || u.FirstName.Contains(searchInput)
                                || u.LastName.Contains(searchInput)
                            )

                        && t2 == null
                        )

                        select u;

            return query.ToList().Count;

        }

        public async Task<IList<IUserInfo>> GetUsersNotInRole(
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
                                || u.FirstName.Contains(searchInput)
                                || u.LastName.Contains(searchInput)
                            )

                        && t2 == null
                        )
                        orderby u.DisplayName
                        select u;

            int offset = (pageSize * pageNumber) - pageSize;

            return query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;

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

            return query.ToList<ISiteUser>();


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

            var matchPattern = "*~" + siteId.ToString()
                + "~" + loginProvider
                + "~" + providerKey;

            var dir = new DirectoryInfo(folderPath);
            var matches = dir.GetFiles(matchPattern);
            var foundFileKey = string.Empty;
            foreach (var match in matches)
            {
                foundFileKey = Path.GetFileNameWithoutExtension(match.Name);
                break; // should only be one so we won't keep interating
            }

            if (!string.IsNullOrEmpty(foundFileKey))
            {
                return await loginQueries.FetchAsync(
                    projectId,
                    foundFileKey,
                    cancellationToken).ConfigureAwait(false);
            }

            return null;
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

        public async Task<IList<IUserLocation>> GetUserLocationsByUser(
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
                .OrderBy(x => x.IpAddressLong)
                .Select(p => p)
                .Skip(offset)
                .Take(pageSize)
                .Where(x => x.UserId == userId)
                ;

            return query.ToList() as IList<IUserLocation>;

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
