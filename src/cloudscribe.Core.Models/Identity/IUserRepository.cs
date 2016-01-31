// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-18
// Last Modified:			2016-01-28
// 

// TODO: we should update all the async signatures to take a cancellationtoken

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    /// <summary>
    /// User repository definition for cloudscribe core user accounts
    /// 
    /// A note about CancellationTokens:
    /// 
    /// My understanding is that the main benefit of passing in the token, is in the case of web requests that get aborted
    /// ie, the user closes the browser or navigates away while the request is processing, if we have already passed in a 
    /// cancellation token then we can request cancellation for aborted requests and by passing the token on down to the data
    /// layer code the actual sql query can in some cases be cancelled reducing unneccessary  workload on the db.
    /// Especially useful on any reporting or search or any kind of read only query really
    /// 
    /// I think we would not want to cancel data updates however in most cases. From our code we are passing in 
    /// CancellationToken.None. I'm not really sure we should take a cancellation token for update methods in the api surface here
    /// since I can't think of any good reason to cancel but then I figured this is just the repository api
    /// maybe it is best to make that decision higher in the stack in case there ever is a good reason 
    /// and it keeps all the async methods consistent for whatever that is worth
    /// this decision makes me uncertain since I feel I could go either way on this, so might revisit this later
    /// The default ASP.NET Core Identity UserManager does seem to pass in the same request abort cancellation token for data updates.
    /// 
    /// </summary>
    public interface IUserRepository : IDisposable
    { 
        Task<bool> EmailExistsInDB(
            int siteId, 
            int userId, 
            string email, 
            CancellationToken cancellationToken);

        Task<bool> EmailExistsInDB(
            int siteId, 
            string email, 
            CancellationToken cancellationToken);

        Task<ISiteUser> Fetch(
            int siteId, 
            Guid userGuid, 
            CancellationToken cancellationToken);

        Task<ISiteUser> Fetch(
            int siteId, 
            int userId, 
            CancellationToken cancellationToken);

        Task<ISiteUser> Fetch(
            int siteId, 
            string email, 
            CancellationToken cancellationToken);

        Task<ISiteUser> FetchByLoginName(
            int siteId, 
            string userName, 
            bool allowEmailFallback, 
            CancellationToken cancellationToken);

        //Task<ISiteUser> FetchNewest(int siteId);

        Task<bool> Delete(
            int siteId, 
            int userId, 
            CancellationToken cancellationToken);

        Task<bool> DeleteUsersBySite(
            int siteId, 
            CancellationToken cancellationToken);

        //Task<bool> Delete(ISiteUser user);

        Task<bool> FlagAsDeleted(
            int userId, 
            CancellationToken cancellationToken);

        Task<bool> FlagAsNotDeleted(
            int userId, 
            CancellationToken cancellationToken);

        Task<bool> LockoutAccount(
            Guid userGuid, 
            CancellationToken cancellationToken);

        Task<bool> UnLockAccount(
            Guid userGuid, 
            CancellationToken cancellationToken);

        Task<bool> UpdateFailedPasswordAttemptCount(
            Guid userGuid, 
            int failedPasswordAttemptCount, 
            CancellationToken cancellationToken);

        Task<bool> UpdateLastLoginTime(
            Guid userGuid, 
            DateTime lastLoginTime, 
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetByIPAddress(
            Guid siteGuid, 
            string ipv4Address, 
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetCrossSiteUserListByEmail(
            string email, 
            CancellationToken cancellationToken);

        

        Task<List<IUserInfo>> GetPage(
            int siteId, 
            int pageNumber, 
            int pageSize, 
            string userNameBeginsWith, 
            int sortMode, 
            CancellationToken cancellationToken);

        Task<int> CountUsers(
            int siteId,
            string userNameBeginsWith,
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<int> CountNotApprovedUsers(
            int siteId,
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetPageUnconfirmedEmailUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<int> CountUnconfirmedEmail(
            int siteId,
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetPageUnconfirmedPhoneUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<int> CountUnconfirmedPhone(
            int siteId,
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetPageLockedByAdmin(
            int siteId, 
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken);

        Task<int> CountLockedByAdmin(
            int siteId,
            CancellationToken cancellationToken);

        Task<List<IUserInfo>> GetPageFutureLockoutEndDate(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<int> CountFutureLockoutEndDate(
            int siteId,
            CancellationToken cancellationToken);
        
        Task<List<IUserInfo>> GetUserAdminSearchPage(
            int siteId, 
            int pageNumber, 
            int pageSize, 
            string searchInput, 
            int sortMode, 
            CancellationToken cancellationToken);

        Task<int> CountUsersForAdminSearch(
            int siteId,
            string searchInput,
            CancellationToken cancellationToken);

        Task<bool> Save(
            ISiteUser user, 
            CancellationToken cancellationToken);

        Task<string> GetUserNameFromEmail(
            int siteId, 
            string email, 
            CancellationToken cancellationToken);

        
        //Task<int> CountUsersForSearch(int siteId, string searchInput);
        
        bool LoginExistsInDB(int siteId, string loginName);

        Task<bool> LoginIsAvailable(
            int siteId, 
            int userId, 
            string loginName, 
            CancellationToken cancellationToken);
        
        int GetCount(int siteId);
        

        //roles
        Task<bool> AddUserToRole(
            int roleId, 
            Guid roleGuid, 
            int userId, 
            Guid userGuid, 
            CancellationToken cancellationToken);

        Task<int> CountOfRoles(
            int siteId, 
            string searchInput, 
            CancellationToken cancellationToken);

        Task<bool> DeleteRole(
            int roleId, 
            CancellationToken cancellationToken);

        Task<bool> DeleteRolesBySite(
            int siteId, 
            CancellationToken cancellationToken);

        Task<bool> DeleteUserRoles(
            int userId, 
            CancellationToken cancellationToken);

        Task<bool> DeleteUserRolesByRole(
            int roleId, 
            CancellationToken cancellationToken);

        Task<bool> DeleteUserRolesBySite(
            int siteId, 
            CancellationToken cancellationToken);

        Task<bool> RoleExists(
            int siteId, 
            string roleName, 
            CancellationToken cancellationToken);

        Task<ISiteRole> FetchRole(
            int roleId, 
            CancellationToken cancellationToken);

        Task<ISiteRole> FetchRole(
            int siteId, 
            string roleName, 
            CancellationToken cancellationToken);

        Task<IList<ISiteRole>> GetRolesBySite(
            int siteId,
            string searchInput,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<List<string>> GetUserRoles(
            int siteId, 
            int userId, 
            CancellationToken cancellationToken);
        
        //IList<ISiteRole> GetRolesUserIsNotIn(int siteId, int userId);
        Task<IList<ISiteUser>> GetUsersInRole(
            int siteId, 
            string roleName, 
            CancellationToken cancellationToken);

        Task<IList<IUserInfo>> GetUsersInRole(
            int siteId, 
            int roleId, 
            string searchInput, 
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken);

        Task<IList<IUserInfo>> GetUsersNotInRole(
            int siteId, 
            int roleId, 
            string searchInput, 
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken);

        Task<int> CountUsersInRole(
            int siteId, 
            int roleId, 
            string searchInput, 
            CancellationToken cancellationToken);

        Task<int> CountUsersNotInRole(
            int siteId, 
            int roleId, 
            string searchInput, 
            CancellationToken cancellationToken);

        Task<bool> RemoveUserFromRole(
            int roleId, 
            int userId, 
            CancellationToken cancellationToken);

        Task<bool> SaveRole(
            ISiteRole role, 
            CancellationToken cancellationToken);

        //claims
        Task<bool> DeleteClaim(
            int id, 
            CancellationToken cancellationToken);

        Task<bool> DeleteClaimsBySite(
            int siteId, 
            CancellationToken cancellationToken);

        Task<bool> DeleteClaimsByUser(
            int siteId, 
            string userId, 
            CancellationToken cancellationToken);

        Task<bool> DeleteClaimByUser(
            int siteId, 
            string userId, 
            string claimType, 
            CancellationToken cancellationToken);

        Task<IList<IUserClaim>> GetClaimsByUser(
            int siteId, 
            string userId, 
            CancellationToken cancellationToken);

        Task<IList<ISiteUser>> GetUsersForClaim(
            int siteId, 
            string claimType, 
            string claimValue, 
            CancellationToken cancellationToken);

        Task<bool> SaveClaim(
            IUserClaim userClaim, 
            CancellationToken cancellationToken);

        //logins
        Task<bool> CreateLogin(
            IUserLogin userLogin, 
            CancellationToken cancellationToken);

        Task<bool> DeleteLogin(
            int siteId, 
            string loginProvider, 
            string providerKey, 
            string userId, 
            CancellationToken cancellationToken);

        Task<bool> DeleteLoginsBySite(
            int siteId, 
            CancellationToken cancellationToken);

        Task<bool> DeleteLoginsByUser(
            int siteId, 
            string userId, 
            CancellationToken cancellationToken);

        Task<IUserLogin> FindLogin(
            int siteId, 
            string loginProvider, 
            string providerKey, 
            CancellationToken cancellationToken);

        Task<IList<IUserLogin>> GetLoginsByUser(
            int siteId, 
            string userId, 
            CancellationToken cancellationToken);


        // the commented method belongs in a service class
        ///// <summary>
        ///// for quickly adding or updating a userlocation, ie for ipaddres tracking
        ///// </summary>
        ///// <param name="userGuid"></param>
        ///// <param name="ipv4Address"></param>
        ///// <param name="hostName"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //Task<bool> TackUserLocation(
        //    Guid siteGuid,
        //    Guid userGuid,
        //    string ipv4Address,
        //    string hostName,
        //    CancellationToken cancellationToken);


        Task<IUserLocation> FetchLocationByUserAndIpv4Address(
            Guid userGuid,
            long ipv4AddressAsLong,
            CancellationToken cancellationToken);

        //Task<IList<IUserLocation>> FetchByUser(
        //    Guid userGuid,
        //    CancellationToken cancellationToken);

        // this should also probably go in the service 
        //Task<bool> SaveUserLocation(
        //    IUserLocation userLocation,
        //    CancellationToken cancellationToken);

        Task<bool> AddUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken);

        Task<bool> UpdateUserLocation(
            IUserLocation userLocation,
            CancellationToken cancellationToken);

        Task<bool> DeleteUserLocation(
            Guid rowGuid,
            CancellationToken cancellationToken);

        Task<bool> DeleteUserLocationsByUser(
            Guid userGuid,
            CancellationToken cancellationToken);

        Task<bool> DeleteUserLocationsBySite(
            Guid siteGuid,
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
