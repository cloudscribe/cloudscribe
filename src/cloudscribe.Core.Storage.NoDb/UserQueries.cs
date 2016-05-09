// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-26
// Last Modified:           2016-05-09
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
    public class UserQueries
    {
        public UserQueries(
            IProjectResolver projectResolver,
            IBasicQueries<SiteUser> userQueries,
            IBasicQueries<SiteRole> roleQueries,
            IBasicQueries<UserRole> userRoleQueries,
            IBasicQueries<UserClaim> claimQueries,
            IBasicQueries<UserLogin> loginQueries,
            IBasicQueries<UserLocation> locationQueries,
            IStoragePathResolver<UserLogin> loginPathResolver
            )
        {
            this.projectResolver = projectResolver;
            this.userQueries = userQueries;
            this.roleQueries = roleQueries;
            this.userRoleQueries = userRoleQueries;
            this.claimQueries = claimQueries;
            this.locationQueries = locationQueries;
            this.loginPathResolver = loginPathResolver;
            this.loginQueries = loginQueries;
        }

        private IProjectResolver projectResolver;
        private IBasicQueries<SiteUser> userQueries;
        private IBasicQueries<SiteRole> roleQueries;
        private IBasicQueries<UserRole> userRoleQueries;
        private IBasicQueries<UserClaim> claimQueries;
        private IBasicQueries<UserLogin> loginQueries;
        private IBasicQueries<UserLocation> locationQueries;
        private IStoragePathResolver<UserLogin> loginPathResolver;

        protected string projectId;

        private async Task EnsureProjectId()
        {
            if (string.IsNullOrEmpty(projectId))
            {
                await projectResolver.ResolveProjectId().ConfigureAwait(false);
            }

        }

        #region User





        #endregion

        #region Roles

        
        public async Task<bool> RoleExists(
            Guid siteGuid,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteGuid == siteGuid
                && x.RoleName == roleName
            );

            return filteredRoles.ToList().Count > 0;
        }

        public async Task<ISiteRole> FetchRole(
            Guid roleGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            return await roleQueries.FetchAsync(
                projectId,
                roleGuid.ToString(),
                cancellationToken).ConfigureAwait(false);

        }

        public async Task<ISiteRole> FetchRole(
            Guid siteGuid,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteGuid == siteGuid
                && x.RoleName == roleName
            );

            return filteredRoles.FirstOrDefault();

        }

        public async Task<List<string>> GetUserRoles(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteGuid == siteGuid
            );

            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in filteredRoles
                        join y in allUserRoles
                        on x.RoleGuid equals y.RoleGuid
                        where y.UserGuid == userGuid
                        orderby x.RoleName
                        select x.RoleName
                        ;

            return query.ToList<string>();

        }

        public async Task<int> CountOfRoles(
            Guid siteGuid,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteGuid == siteGuid
                && (
                 (searchInput == "")
                        || x.DisplayName.Contains(searchInput)
                        || x.RoleName.Contains(searchInput)
                )
            );

            return filteredRoles.ToList().Count;

        }

        public async Task<IList<ISiteRole>> GetRolesBySite(
            Guid siteGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredRoles = allRoles.Where(
                x => x.SiteGuid == siteGuid
                && (
                 (searchInput == "")
                        || x.DisplayName.Contains(searchInput)
                        || x.RoleName.Contains(searchInput)
                )
            );

            int offset = (pageSize * pageNumber) - pageSize;

            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var listQuery = from x in filteredRoles
                            orderby x.RoleName ascending
                            select x;

            return listQuery
                .Skip(offset)
                .Take(pageSize)
                .ToList<ISiteRole>()
                ;

        }

        public async Task<int> CountUsersInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in allUsers
                        join y in allUserRoles
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

                        select x
                        ;

            return query.ToList().Count();

        }

        public async Task<IList<IUserInfo>> GetUsersInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);



            var query = from x in allUsers
                        join y in allUserRoles
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

            int offset = (pageSize * pageNumber) - pageSize;

            return query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;

        }

        public async Task<IList<ISiteUser>> GetUsersInRole(
            Guid siteGuid,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var siteRoles = allRoles.Where(x => x.SiteGuid == siteGuid);

            var query = from x in allUsers
                        join y in allUserRoles
                        on x.UserGuid equals y.UserGuid
                        join z in siteRoles
                        on y.RoleGuid equals z.RoleGuid
                        orderby x.DisplayName
                        where
                            (x.SiteGuid.Equals(siteGuid) && z.RoleName.Equals(roleName))

                        select x
                        ;

            var items = query
                .ToList<ISiteUser>()
                ;

            return items;
        }

        public async Task<int> CountUsersNotInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var siteRoles = allRoles.Where(x => x.SiteGuid == siteGuid);

            var query = from u in allUsers
                        from r in siteRoles
                        join ur in allUserRoles
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

                        select u;

            return query.ToList().Count;

        }

        public async Task<IList<IUserInfo>> GetUsersNotInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allUserRoles = await userRoleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var allRoles = await roleQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var siteRoles = allRoles.Where(x => x.SiteGuid == siteGuid);

            var query = from u in allUsers
                        from r in siteRoles
                        join ur in allUserRoles
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
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var all = await claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x =>
                 x.SiteGuid == siteGuid
                 && x.UserGuid == userGuid
            );

            return filtered as IList<IUserClaim>;

        }


        public async Task<IList<ISiteUser>> GetUsersForClaim(
            Guid siteGuid,
            string claimType,
            string claimValue,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var allClaims = await claimQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filteredClaims = allClaims.Where(x =>
                 x.SiteGuid == siteGuid
                 && x.ClaimType == claimType
                 && x.ClaimValue == claimValue
            );

            var allUsers = await userQueries.GetAllAsync(
                projectId,
                cancellationToken).ConfigureAwait(false);

            var query = from x in allUsers
                        join y in filteredClaims
                        on x.UserGuid equals y.UserGuid
                        where x.SiteGuid == siteGuid
                        orderby x.DisplayName
                        select x
                        ;

            return query.ToList() as IList<ISiteUser>;


        }


        #endregion

        #region Logins
        
        public async Task<IUserLogin> FindLogin(
            Guid siteGuid,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var folderPath = await loginPathResolver.ResolvePath(projectId).ConfigureAwait(false);

            if (!Directory.Exists(folderPath)) return null;

            // understand structure of key which is the filename
            //var key = login.UserGuid.ToString()
            //    + "~" + login.SiteGuid.ToString()
            //    + "~" + login.LoginProvider
            //    + "~" + login.ProviderKey;

            var matchPattern = "*~" + siteGuid.ToString()
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
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var folderPath = await loginPathResolver.ResolvePath(projectId).ConfigureAwait(false);

            // understand structure of key which is the filename
            //var key = login.UserGuid.ToString()
            //    + "~" + login.SiteGuid.ToString()
            //    + "~" + login.LoginProvider
            //    + "~" + login.ProviderKey;

            var matchPattern = "*~" + siteGuid.ToString() + "~*";

            var dir = new DirectoryInfo(folderPath);
            var matches = dir.GetFiles(matchPattern);

            var result = new List<IUserLogin>();
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
            Guid userGuid,
            long ipv4AddressAsLong,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            var all = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in all
                        where x.UserGuid == userGuid
                        && x.IpAddressLong == ipv4AddressAsLong
                        select x
                        ;

            return query.FirstOrDefault<UserLocation>();

        }

        public async Task<int> CountUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            var result = 0;

            await EnsureProjectId().ConfigureAwait(false);

            var all = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var filtered = all.Where(x => x.UserGuid == userGuid);
            result = filtered.ToList().Count;

            return result;

        }

        public async Task<IList<IUserLocation>> GetUserLocationsByUser(
            Guid userGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            int offset = (pageSize * pageNumber) - pageSize;

            var all = await locationQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = all
                .OrderBy(x => x.IpAddressLong)
                .Select(p => p)
                .Skip(offset)
                .Take(pageSize)
                .Where(x => x.UserGuid == userGuid)
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
