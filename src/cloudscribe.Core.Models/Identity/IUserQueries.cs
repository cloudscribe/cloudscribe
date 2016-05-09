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
        Task<bool> EmailExistsInDB(
            Guid siteGuid,
            Guid userGuid,
            string email,
            CancellationToken cancellationToken);

        Task<bool> EmailExistsInDB(
            Guid siteGuid,
            string email,
            CancellationToken cancellationToken);

        Task<ISiteUser> Fetch(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken);


        Task<ISiteUser> Fetch(
            Guid siteGuid,
            string email,
            CancellationToken cancellationToken);

        Task<ISiteUser> FetchByLoginName(
            Guid siteGuid,
            string userName,
            bool allowEmailFallback,
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetByIPAddress(
            Guid siteGuid,
            string ipv4Address,
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetCrossSiteUserListByEmail(
            string email,
            CancellationToken cancellationToken);



        Task<List<IUserInfo>> GetPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode,
            CancellationToken cancellationToken);

        Task<int> CountUsers(
            Guid siteGuid,
            string userNameBeginsWith,
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetNotApprovedUsers(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<int> CountNotApprovedUsers(
            Guid siteGuid,
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetPageUnconfirmedEmailUsers(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<int> CountUnconfirmedEmail(
            Guid siteGuid,
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetPageUnconfirmedPhoneUsers(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<int> CountUnconfirmedPhone(
            Guid siteGuid,
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetPageLockedByAdmin(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<int> CountLockedByAdmin(
            Guid siteGuid,
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetPageFutureLockoutEndDate(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<int> CountFutureLockoutEndDate(
            Guid siteGuid,
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetUserAdminSearchPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            CancellationToken cancellationToken);

        Task<int> CountUsersForAdminSearch(
            Guid siteGuid,
            string searchInput,
            CancellationToken cancellationToken);
        
        Task<string> GetUserNameFromEmail(
            Guid siteGuid,
            string email,
            CancellationToken cancellationToken);
        
        bool LoginExistsInDB(Guid siteGuid, string loginName);
        int GetCount(Guid siteGuid);

        Task<bool> LoginIsAvailable(
            Guid siteGuid,
            Guid userGuid,
            string loginName,
            CancellationToken cancellationToken);

        //roles

        Task<int> CountOfRoles(
            Guid siteGuid,
            string searchInput,
            CancellationToken cancellationToken);

        Task<bool> RoleExists(
            Guid siteGuid,
            string roleName,
            CancellationToken cancellationToken);

        Task<ISiteRole> FetchRole(
            Guid roleGuid,
            CancellationToken cancellationToken);

        Task<ISiteRole> FetchRole(
            Guid siteGuid,
            string roleName,
            CancellationToken cancellationToken);

        Task<IList<ISiteRole>> GetRolesBySite(
            Guid siteGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<List<string>> GetUserRoles(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken);

        //IList<ISiteRole> GetRolesUserIsNotIn(int siteId, int userId);
        Task<IList<ISiteUser>> GetUsersInRole(
            Guid siteGuid,
            string roleName,
            CancellationToken cancellationToken);

        Task<IList<IUserInfo>> GetUsersInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<IList<IUserInfo>> GetUsersNotInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<int> CountUsersInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            CancellationToken cancellationToken);

        Task<int> CountUsersNotInRole(
            Guid siteGuid,
            Guid roleGuid,
            string searchInput,
            CancellationToken cancellationToken);

        // claims

        Task<IList<IUserClaim>> GetClaimsByUser(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken);

        Task<IList<ISiteUser>> GetUsersForClaim(
            Guid siteGuid,
            string claimType,
            string claimValue,
            CancellationToken cancellationToken);

        // logins

        Task<IUserLogin> FindLogin(
            Guid siteGuid,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken);

        Task<IList<IUserLogin>> GetLoginsByUser(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken);

        Task<IUserLocation> FetchLocationByUserAndIpv4Address(
            Guid userGuid,
            long ipv4AddressAsLong,
            CancellationToken cancellationToken);

        Task<int> CountUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken);

        Task<IList<IUserLocation>> GetUserLocationsByUser(
            Guid userGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

    }
}
