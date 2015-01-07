// Author:					Joe Audette
// Created:					2014-08-18
// Last Modified:			2015-01-07
// 


using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public interface IUserRepository : IDisposable
    {
        bool ConfirmRegistration(Guid registrationGuid);
        bool Delete(int userId);
        bool EmailExistsInDB(int siteId, int userId, string email);
        bool EmailExistsInDB(int siteId, string email);
        ISiteUser Fetch(int siteId, Guid userGuid);
        ISiteUser Fetch(int siteId, int userId);
        ISiteUser Fetch(int siteId, string email);
        ISiteUser FetchByConfirmationGuid(int siteId, Guid confirmGuid);
        ISiteUser FetchByLoginName(int siteId, string userName, bool allowEmailFallback);
        ISiteUser FetchNewest(int siteId);
        bool FlagAsDeleted(int userId);
        bool FlagAsNotDeleted(int userId);
        bool LockoutAccount(Guid userGuid);
        bool UnLockAccount(Guid userGuid);
        bool UpdateFailedPasswordAttemptCount(Guid userGuid, int failedPasswordAttemptCount);
        List<IUserInfo> GetByIPAddress(Guid siteGuid, string ipv4Address);
        List<IUserInfo> GetCrossSiteUserListByEmail(string email);
        int GetCount(int siteId);
        int GetNewestUserId(int siteId);
        List<IUserInfo> GetNotApprovedUsers(int siteId, int pageNumber, int pageSize, out int totalPages);
        List<IUserInfo> GetPage(int siteId, int pageNumber, int pageSize, string userNameBeginsWith, int sortMode, out int totalPages);
        List<IUserInfo> GetPageLockedUsers(int siteId, int pageNumber, int pageSize, out int totalPages);
        List<IUserInfo> GetUserAdminSearchPage(int siteId, int pageNumber, int pageSize, string searchInput, int sortMode, out int totalPages);
        DataTable GetUserListForPasswordFormatChange(int siteId);
        string GetUserNameFromEmail(int siteId, string email);
        List<IUserInfo> GetUserSearchPage(int siteId, int pageNumber, int pageSize, string searchInput, int sortMode, out int totalPages);
        bool LoginExistsInDB(int siteId, string loginName);
        bool LoginIsAvailable(int siteId, int userId, string loginName);
        bool Save(ISiteUser user);
        bool UpdatePasswordAndSalt(int userId, int passwordFormat, string password, string passwordSalt);
        void UpdateTotalRevenue();
        void UpdateTotalRevenue(Guid userGuid);
        int UserCount(int siteId, string userNameBeginsWith);
        int UsersOnlineSinceCount(int siteId, DateTime sinceTime);

        //roles
        bool AddUserToRole(int roleId, Guid roleGuid, int userId, Guid userGuid);
        void AddUserToDefaultRoles(ISiteUser siteUser);
        Task<int> CountOfRoles(int siteId, string searchInput);
        int GetRoleMemberCount(int roleId);
        Task<bool> DeleteRole(int roleID);
        bool DeleteUserRoles(int userId);
        bool DeleteUserRolesByRole(int roleId);
        Task<bool> RoleExists(int siteId, string roleName);
        Task<ISiteRole> FetchRole(int roleID);
        ISiteRole FetchRole(int siteId, string roleName);
        Task<IList<ISiteRole>> GetRolesBySite(
            int siteId, 
            string searchInput,
            int pageNumber,
            int pageSize);
        List<string> GetUserRoles(int siteId, int userId);
        List<int> GetRoleIds(int siteId, string roleNamesSeparatedBySemiColons);
        IList<ISiteRole> GetRolesUserIsNotIn(int siteId, int userId);
        Task<IList<IUserInfo>> GetUsersInRole(int siteId, int roleId, string searchInput, int pageNumber, int pageSize);
        IList<IUserInfo> GetUsersNotInRole(int siteId, int roleId, string searchInput, int pageNumber, int pageSize, out int totalPages);
        Task<int> CountUsersInRole(int siteId, int roleId, string searchInput);
        bool RemoveUserFromRole(int roleId, int userId);
        Task<bool> SaveRole(ISiteRole role);

        //claims
        bool DeleteClaim(int id);
        bool DeleteClaimsBySite(Guid siteGuid);
        bool DeleteClaimsByUser(string userId);
        bool DeleteClaimByUser(string userId, string claimType);
        IList<IUserClaim> GetClaimsByUser(string userId);
        bool SaveClaim(IUserClaim userClaim);

        //logins
        bool CreateLogin(cloudscribe.Core.Models.IUserLogin userLogin);
        bool DeleteLogin(string loginProvider, string providerKey, string userId);
        bool DeleteLoginsBySite(Guid siteGuid);
        bool DeleteLoginsByUser(string userId);
        IUserLogin FindLogin(string loginProvider, string providerKey);
        IList<IUserLogin> GetLoginsByUser(string userId);
    }
}
