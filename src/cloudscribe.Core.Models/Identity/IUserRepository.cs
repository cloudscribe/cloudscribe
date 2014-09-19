using System;
using System.Collections.Generic;
using System.Data;

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
        List<ISiteUser> GetByIPAddress(Guid siteGuid, string ipv4Address);
        List<ISiteUser> GetCrossSiteUserListByEmail(string email);
        int GetCount(int siteId);
        int GetNewestUserId(int siteId);
        List<ISiteUser> GetNotApprovedUsers(int siteId, int pageNumber, int pageSize, out int totalPages);
        List<ISiteUser> GetPage(int siteId, int pageNumber, int pageSize, string userNameBeginsWith, int sortMode, out int totalPages);
        List<ISiteUser> GetPageLockedUsers(int siteId, int pageNumber, int pageSize, out int totalPages);
        List<cloudscribe.Core.Models.ISiteUser> GetUserAdminSearchPage(int siteId, int pageNumber, int pageSize, string searchInput, int sortMode, out int totalPages);
        DataTable GetUserListForPasswordFormatChange(int siteId);
        string GetUserNameFromEmail(int siteId, string email);
        List<ISiteUser> GetUserSearchPage(int siteId, int pageNumber, int pageSize, string searchInput, int sortMode, out int totalPages);
        bool LoginExistsInDB(int siteId, string loginName);
        bool Save(ISiteUser user);
        bool UpdatePasswordAndSalt(int userId, int passwordFormat, string password, string passwordSalt);
        void UpdateTotalRevenue();
        void UpdateTotalRevenue(Guid userGuid);
        int UserCount(int siteId, string userNameBeginsWith);
        int UsersOnlineSinceCount(int siteId, DateTime sinceTime);

        //roles
        bool AddUserToRole(int roleId, Guid roleGuid, int userId, Guid userGuid);
        void AddUserToDefaultRoles(ISiteUser siteUser);
        int CountOfRoles(int siteId);
        int GetRoleMemberCount(int roleId);
        bool DeleteRole(int roleID);
        bool DeleteUserRoles(int userId);
        bool RoleExists(int siteId, string roleName);
        ISiteRole FetchRole(int roleID);
        ISiteRole FetchRole(int siteId, string roleName);
        IList<ISiteRole> GetRolesBySite(int siteId);
        List<string> GetUserRoles(int siteId, int userId);
        List<int> GetRoleIds(int siteId, string roleNamesSeparatedBySemiColons);
        IList<ISiteRole> GetRolesUserIsNotIn(int siteId, int userId);
        IList<IUserInfo> GetUsersInRole(int siteId, int roleId, int pageNumber, int pageSize, out int totalPages);
        IList<IUserInfo> GetUsersNotInRole(int siteId, int roleId, int pageNumber, int pageSize, out int totalPages);
        bool RemoveUserFromRole(int roleId, int userId);
        bool SaveRole(cloudscribe.Core.Models.ISiteRole role);

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
