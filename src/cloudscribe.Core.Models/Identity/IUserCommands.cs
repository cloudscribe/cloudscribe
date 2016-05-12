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
    public interface IUserCommands
    {
        Task Create(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken));

        Task Update(
            ISiteUser user,
            CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUsersBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken));


        Task FlagAsDeleted(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task FlagAsNotDeleted(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task LockoutAccount(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task UnLockAccount(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateFailedPasswordAttemptCount(
            Guid userGuid,
            int failedPasswordAttemptCount,
            CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateLastLoginTime(
            Guid userGuid,
            DateTime lastLoginTime,
            CancellationToken cancellationToken = default(CancellationToken));

        //roles
        Task AddUserToRole(
            Guid roleGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteRole(
            Guid roleGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteRolesBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserRoles(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserRolesByRole(
            Guid roleGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserRolesBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task RemoveUserFromRole(
            Guid roleGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task CreateRole(
            ISiteRole role,
            CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateRole(
            ISiteRole role,
            CancellationToken cancellationToken = default(CancellationToken));

        //claims
        Task DeleteClaim(
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteClaimsBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteClaimsByUser(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteClaimByUser(
            Guid siteGuid,
            Guid userGuid,
            string claimType,
            CancellationToken cancellationToken = default(CancellationToken));

        Task CreateClaim(
            IUserClaim userClaim,
            CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateClaim(
            IUserClaim userClaim,
            CancellationToken cancellationToken = default(CancellationToken));

        //logins
        Task CreateLogin(
            IUserLogin userLogin,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteLogin(
            Guid siteGuid,
            Guid userGuid,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteLoginsBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteLoginsByUser(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task AddUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserLocation(
            Guid rowGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserLocationsBySite(
            Guid siteGuid,
            CancellationToken cancellationToken = default(CancellationToken));

    }
}
