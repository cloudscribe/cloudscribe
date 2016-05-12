// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-18
// Last Modified:			2016-05-09
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public interface IUserQueries
    {
        // TODO: are these non-async methods needed/used anywhere?
        bool LoginExistsInDB(Guid siteGuid, string loginName);
        int GetCount(Guid siteGuid);

        Task<bool> EmailExistsInDB(
            Guid siteGuid,
            Guid userGuid,
            string email,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> EmailExistsInDB(
            Guid siteGuid,
            string email,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<ISiteUser> Fetch(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));
        
        Task<ISiteUser> Fetch(
            Guid siteGuid,
            string email,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<ISiteUser> FetchByLoginName(
            Guid siteGuid,
            string userName,
            bool allowEmailFallback,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<IUserInfo>> GetByIPAddress(
            Guid siteGuid,
            string ipv4Address,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<IUserInfo>> GetCrossSiteUserListByEmail(
            string email,
            CancellationToken cancellationToken = default(CancellationToken));



        Task<List<IUserInfo>> GetPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> CountUsers(
            Guid siteGuid,
            string userNameBeginsWith,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<IUserInfo>> GetNotApprovedUsers(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> CountNotApprovedUsers(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<IUserInfo>> GetPageUnconfirmedEmailUsers(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> CountUnconfirmedEmail(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<IUserInfo>> GetPageUnconfirmedPhoneUsers(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> CountUnconfirmedPhone(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<IUserInfo>> GetPageLockedByAdmin(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> CountLockedByAdmin(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<IUserInfo>> GetPageFutureLockoutEndDate(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> CountFutureLockoutEndDate(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<IUserInfo>> GetUserAdminSearchPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> CountUsersForAdminSearch(
            Guid siteGuid,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken));
        
        Task<string> GetUserNameFromEmail(
            Guid siteGuid,
            string email,
            CancellationToken cancellationToken = default(CancellationToken));
        
        

        Task<bool> LoginIsAvailable(
            Guid siteGuid,
            Guid userGuid,
            string loginName,
            CancellationToken cancellationToken = default(CancellationToken));

        //roles

        Task<int> CountOfRoles(
            Guid siteGuid,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> RoleExists(
            Guid siteGuid,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<ISiteRole> FetchRole(
            Guid roleGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<ISiteRole> FetchRole(
            Guid siteGuid,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<IList<ISiteRole>> GetRolesBySite(
            Guid siteGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<string>> GetUserRoles(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        //IList<ISiteRole> GetRolesUserIsNotIn(int siteId, int userId);
        Task<IList<ISiteUser>> GetUsersInRole(
            Guid siteGuid,
            string roleName,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<IList<IUserInfo>> GetUsersInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<IList<IUserInfo>> GetUsersNotInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> CountUsersInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> CountUsersNotInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            CancellationToken cancellationToken = default(CancellationToken));

        // claims

        Task<IList<IUserClaim>> GetClaimsByUser(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<IList<ISiteUser>> GetUsersForClaim(
            Guid siteGuid,
            string claimType,
            string claimValue,
            CancellationToken cancellationToken = default(CancellationToken));

        // logins

        Task<IUserLogin> FindLogin(
            Guid siteGuid,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<IList<IUserLogin>> GetLoginsByUser(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<IUserLocation> FetchLocationByUserAndIpv4Address(
            Guid userGuid,
            long ipv4AddressAsLong,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> CountUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<IList<IUserLocation>> GetUserLocationsByUser(
            Guid userGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));

    }
}
