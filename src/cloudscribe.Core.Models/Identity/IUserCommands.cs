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
            CancellationToken cancellationToken);

        Task Update(
            ISiteUser user,
            CancellationToken cancellationToken);

        Task Delete(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken);

        Task DeleteUsersBySite(
            Guid siteGuid,
            CancellationToken cancellationToken);


        Task FlagAsDeleted(
            Guid userGuid,
            CancellationToken cancellationToken);

        Task FlagAsNotDeleted(
            Guid userGuid,
            CancellationToken cancellationToken);

        Task LockoutAccount(
            Guid userGuid,
            CancellationToken cancellationToken);

        Task UnLockAccount(
            Guid userGuid,
            CancellationToken cancellationToken);

        Task UpdateFailedPasswordAttemptCount(
            Guid userGuid,
            int failedPasswordAttemptCount,
            CancellationToken cancellationToken);

        Task UpdateLastLoginTime(
            Guid userGuid,
            DateTime lastLoginTime,
            CancellationToken cancellationToken);

        //roles
        Task AddUserToRole(
            Guid roleGuid,
            Guid userGuid,
            CancellationToken cancellationToken);

        Task DeleteRole(
            Guid roleGuid,
            CancellationToken cancellationToken);

        Task DeleteRolesBySite(
            Guid siteGuid,
            CancellationToken cancellationToken);

        Task DeleteUserRoles(
            Guid userGuid,
            CancellationToken cancellationToken);

        Task DeleteUserRolesByRole(
            Guid roleGuid,
            CancellationToken cancellationToken);

        Task DeleteUserRolesBySite(
            Guid siteGuid,
            CancellationToken cancellationToken);

        Task RemoveUserFromRole(
            Guid roleGuid,
            Guid userGuid,
            CancellationToken cancellationToken);

        Task CreateRole(
            ISiteRole role,
            CancellationToken cancellationToken);

        Task UpdateRole(
            ISiteRole role,
            CancellationToken cancellationToken);

        //claims
        Task DeleteClaim(
            Guid id,
            CancellationToken cancellationToken);

        Task DeleteClaimsBySite(
            Guid siteGuid,
            CancellationToken cancellationToken);

        Task DeleteClaimsByUser(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken);

        Task DeleteClaimByUser(
            Guid siteGuid,
            Guid userGuid,
            string claimType,
            CancellationToken cancellationToken);

        Task CreateClaim(
            IUserClaim userClaim,
            CancellationToken cancellationToken);

        Task UpdateClaim(
            IUserClaim userClaim,
            CancellationToken cancellationToken);

        //logins
        Task CreateLogin(
            IUserLogin userLogin,
            CancellationToken cancellationToken);

        Task DeleteLogin(
            Guid siteGuid,
            Guid userGuid,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken);

        Task DeleteLoginsBySite(
            Guid siteGuid,
            CancellationToken cancellationToken);

        Task DeleteLoginsByUser(
            Guid siteGuid,
            Guid userGuid,
            CancellationToken cancellationToken);

        Task AddUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken);

        Task UpdateUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken);

        Task DeleteUserLocation(
            Guid rowGuid,
            CancellationToken cancellationToken);

        Task DeleteUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken);

        Task DeleteUserLocationsBySite(
            Guid siteGuid,
            CancellationToken cancellationToken);

    }
}
