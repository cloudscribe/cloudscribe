// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-18
// Last Modified:			2016-05-12
// 

using System;
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
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUsersBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken));


        Task FlagAsDeleted(
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task FlagAsNotDeleted(
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task LockoutAccount(
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task UnLockAccount(
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateFailedPasswordAttemptCount(
            Guid userId,
            int failedPasswordAttemptCount,
            CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateLastLoginTime(
            Guid userId,
            DateTime lastLoginTime,
            CancellationToken cancellationToken = default(CancellationToken));

        //roles
        Task AddUserToRole(
            Guid roleId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteRole(
            Guid roleId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteRolesBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserRoles(
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserRolesByRole(
            Guid roleId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserRolesBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task RemoveUserFromRole(
            Guid roleId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task CreateRole(
            ISiteRole role,
            CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateRole(
            ISiteRole role,
            CancellationToken cancellationToken = default(CancellationToken));

        //claims
        Task DeleteClaim(
            Guid claimId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteClaimsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteClaimsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteClaimByUser(
            Guid siteId,
            Guid userId,
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
            Guid siteId,
            Guid userId,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteLoginsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteLoginsByUser(
            Guid siteId,
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task AddUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserLocation(
            Guid userLocationId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserLocationsByUser(
            Guid userId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserLocationsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken));

    }
}
