// Author:					Joe Audette
// Created:				    2014-07-22
// Last Modified:		    2015-01-13
// 
// You must not remove this notice, or any other, from this software.


using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using log4net;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace cloudscribe.AspNet.Identity
{
    public sealed class UserStore<TUser> :
        IUserStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserEmailStore<TUser>,
        IUserLoginStore<TUser>,
        IUserRoleStore<TUser>,
        IUserClaimStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IUserLockoutStore<TUser, string>,
        IUserTwoFactorStore<TUser, string>
        where TUser : SiteUser
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserStore<TUser>));
        private bool debugLog = AppSettings.UserStoreDebugEnabled;

        private ISiteSettings siteSettings;
        private IUserRepository repo;
        

        private UserStore() { }

        public UserStore(
            ISiteSettings site, 
            IUserRepository userRepository
            )
        {
            if (site == null) { throw new ArgumentException("SiteSettings cannot be null"); }
            siteSettings = site;

            if (userRepository == null) { throw new ArgumentException("userRepository cannot be null"); }
            repo = userRepository;
            
            if (debugLog) { log.Info("constructor"); }
        }


        #region IUserStore

        //TODO: fix async tasks here are not really async
        // might have to implement async data access to really fix it
        // it still works synchronously but the method names imply async

        public async Task CreateAsync(TUser user)
        {
            if (debugLog) { log.Info("CreateAsync"); }

            if(user.SiteGuid == Guid.Empty)
            {
                user.SiteGuid = siteSettings.SiteGuid;
            }
            if(user.SiteId == -1)
            {
                user.SiteId = siteSettings.SiteId;
            }   

            if(user.UserName.Length == 0)
            {
                user.UserName = SuggestLoginNameFromEmail(siteSettings.SiteId, user.Email);
            }

            if (user.DisplayName.Length == 0)
            {
                user.DisplayName = SuggestLoginNameFromEmail(siteSettings.SiteId, user.Email);
            }

            bool result = await repo.Save(user);
            if(result)
            {
                result = result && await repo.AddUserToDefaultRoles(user);
            }
            
            
        }

        public async Task UpdateAsync(TUser user)
        {
            if (debugLog) { log.Info("UpdateAsync"); }

            bool result = await repo.Save(user);

        }

        public async Task DeleteAsync(TUser user)
        {
            if (debugLog) { log.Info("UpdateAsync"); }

            if(siteSettings.ReallyDeleteUsers)
            {
                Task[] tasks = new Task[4];
                tasks[0] = repo.DeleteLoginsByUser(user.Id);
                tasks[1] = repo.DeleteClaimsByUser(user.Id);
                tasks[2] = repo.DeleteUserRoles(user.UserId);
                tasks[3] = repo.Delete(user.UserId);

                Task.WaitAll(tasks);
            }
            else
            {
                bool result = await repo.FlagAsDeleted(user.UserId);
            }

        }

        public async Task<TUser> FindByIdAsync(string userId)
        {
            if (debugLog) { log.Info("FindByIdAsync"); }

            Guid userGuid = new Guid(userId);

            ISiteUser siteUser = await repo.Fetch(siteSettings.SiteId, userGuid);

            return (TUser)siteUser;
        }


        public async Task<TUser> FindByNameAsync(string userName)
        {
            if (debugLog) { log.Info("FindByNameAsync"); }

            ISiteUser siteUser = await repo.FetchByLoginName(siteSettings.SiteId, userName, true);
            return (TUser)siteUser;

        }

        #endregion

        #region IUserEmailStore

        public async Task<string> GetEmailAsync(TUser user)
        {
            if (debugLog) { log.Info("GetEmailAsync"); }

            return user.Email;
        }

        public async Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            if (debugLog) { log.Info("GetEmailConfirmedAsync"); }

            return user.EmailConfirmed;
        }

        public async Task SetEmailAsync(TUser user, string email)
        {
            if (debugLog) { log.Info("SetEmailAsync"); }

            if (user.SiteGuid == Guid.Empty)
            {
                user.SiteGuid = siteSettings.SiteGuid;
            }
            if (user.SiteId == -1)
            {
                user.SiteId = siteSettings.SiteId;
            }   

            user.Email = email;
            user.LoweredEmail = email.ToLower();

            bool result = await repo.Save(user);
            
        }

        public async Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            if (debugLog) { log.Info("SetEmailConfirmedAsync"); }

            if (user.SiteGuid == Guid.Empty)
            {
                user.SiteGuid = siteSettings.SiteGuid;
            }
            if (user.SiteId == -1)
            {
                user.SiteId = siteSettings.SiteId;
            }   

            user.EmailConfirmed = confirmed;
            
            // I don't not sure this method is expected to
            // persist this change to the db
            bool result = await repo.Save(user);
            
        }

        public async Task<TUser> FindByEmailAsync(string email)
        {
            if (debugLog) { log.Info("FindByEmailAsync"); }

            ISiteUser siteUser = await repo.Fetch(siteSettings.SiteId, email);

            return (TUser)siteUser;
        }

        #endregion

        #region IUserPasswordStore

        public async Task<string> GetPasswordHashAsync(TUser user)
        {
            if (debugLog) { log.Info("GetPasswordHashAsync"); }

            if (user == null)
            {
                return string.Empty;
            }

            return user.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(TUser user)
        {
            if (debugLog) { log.Info("HasPasswordAsync"); }

            if (user == null)
            {
                return false;
            }

            return string.IsNullOrEmpty(user.PasswordHash);
        }

        public async Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            if (debugLog) { log.Info("SetPasswordHashAsync"); }

            if (user == null)
            {
                return;
            }

            user.PasswordHash = passwordHash;

            // I don't think this method is expected to
            // persist this change to the db
            // it seems to call this before calling Create

            // for some reason this is called before Create
            // so this mthod is actually doing the create
            //if (user.SiteGuid == Guid.Empty)
            //{
            //    user.SiteGuid = siteSettings.SiteGuid;
            //}
            //if (user.SiteId == -1)
            //{
            //    user.SiteId = siteSettings.SiteId;
            //}   

            //user.PasswordHash = passwordHash;
            //userRepo.Save(user);


            
        }

        #endregion

        #region IUserLockoutStore

        public async Task<int> GetAccessFailedCountAsync(TUser user)
        {
            if (debugLog) { log.Info("GetAccessFailedCountAsync"); }

            return user.FailedPasswordAttemptCount;
        }

        public async Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            if (debugLog) { log.Info("GetLockoutEnabledAsync"); }

            return user.IsLockedOut;
        }

        public async Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            if (debugLog) { log.Info("GetLockoutEndDateAsync"); }

            DateTimeOffset d = new DateTimeOffset(DateTime.MinValue);
            if(user.LockoutEndDateUtc != null)
            {
                d = new DateTimeOffset(user.LockoutEndDateUtc.Value);
                
            }
            return d;
        }

        public async Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            if (debugLog) { log.Info("IncrementAccessFailedCountAsync"); }

            user.FailedPasswordAttemptCount += 1;
            await repo.UpdateFailedPasswordAttemptCount(user.UserGuid, user.FailedPasswordAttemptCount);
            return user.FailedPasswordAttemptCount;
        }

        public async Task ResetAccessFailedCountAsync(TUser user)
        {
            if (debugLog) { log.Info("ResetAccessFailedCountAsync"); }

            user.FailedPasswordAttemptCount = 0;
            bool result = await repo.UpdateFailedPasswordAttemptCount(user.UserGuid, user.FailedPasswordAttemptCount);
            
        }

        public async Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            if (debugLog) { log.Info("SetLockoutEnabledAsync"); }
            bool result;
            if (enabled)
            {
                result = await repo.LockoutAccount(user.UserGuid);
            }
            else
            {
                result = await repo.UnLockAccount(user.UserGuid);
            }

            
        }

        public async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            if (debugLog) { log.Info("SetLockoutEndDateAsync"); }

            user.LockoutEndDateUtc = lockoutEnd.DateTime;
            bool result = await repo.Save(user);
        }



        #endregion

        #region IUserClaimStore

        public async Task AddClaimAsync(TUser user, Claim claim)
        {
            if (debugLog) { log.Info("AddClaimAsync"); }

            UserClaim userClaim = new UserClaim();
            userClaim.UserId = user.UserGuid.ToString();
            userClaim.ClaimType = claim.Type;
            userClaim.ClaimValue = claim.Value;
            bool result = await repo.SaveClaim(userClaim);

        }

        public async Task RemoveClaimAsync(TUser user, Claim claim)
        {
            if (debugLog) { log.Info("RemoveClaimAsync"); }

            await repo.DeleteClaimByUser(user.Id, claim.Type);
        }

        public async Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            if (debugLog) { log.Info("GetClaimsAsync"); }

            IList<Claim> claims = new List<Claim>();

            IList<IUserClaim> userClaims = await repo.GetClaimsByUser(user.Id);
            foreach (UserClaim uc in userClaims)
            {
                Claim c = new Claim(uc.ClaimType, uc.ClaimValue);
                claims.Add(c);
            }

            return claims;

        }

        #endregion

        #region IUserLoginStore

        public async Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (debugLog) { log.Info("AddLoginAsync"); }

            UserLogin userlogin = new UserLogin();
            userlogin.UserId = user.UserGuid.ToString();
            userlogin.LoginProvider = login.LoginProvider;
            userlogin.ProviderKey = login.ProviderKey;
            bool result = await repo.CreateLogin(userlogin);

        }

        public async Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (debugLog) { log.Info("FindAsync"); }

            IUserLogin userlogin = await repo.FindLogin(login.LoginProvider, login.ProviderKey);
            if (userlogin != null && userlogin.UserId.Length == 36)
            {
                Guid userGuid = new Guid(userlogin.UserId);
                ISiteUser siteUser = await repo.Fetch(siteSettings.SiteId, userGuid);
                if (siteUser != null)
                {
                    return (TUser)siteUser;
                }
            }

            return default(TUser);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (debugLog) { log.Info("GetLoginsAsync"); }

            IList<UserLoginInfo> logins = new List<UserLoginInfo>();

            IList<IUserLogin> userLogins = await repo.GetLoginsByUser(user.UserGuid.ToString());
            foreach (UserLogin ul in userLogins)
            {
                UserLoginInfo l = new UserLoginInfo(ul.LoginProvider, ul.ProviderKey);
                logins.Add(l);
            }


            return logins;
        }

        public async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (debugLog) { log.Info("RemoveLoginAsync"); }

            bool result = await repo.DeleteLogin(
                login.LoginProvider,
                login.ProviderKey,
                user.UserGuid.ToString()
                );

            
        }

        #endregion

        #region IUserTwoFactorStore

        public async Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            if (debugLog) { log.Info("GetTwoFactorEnabledAsync"); }

            return user.TwoFactorEnabled;
        }

        public async Task SetTwoFactorEnabledAsync(TUser user,bool enabled)
        {
            if (debugLog) { log.Info("SetTwoFactorEnabledAsync"); }

            user.TwoFactorEnabled = enabled;
            bool result = await repo.Save(user);

            
        }


        #endregion

        #region IUserPhoneNumberStore

        public async Task<string> GetPhoneNumberAsync(TUser user)
        {
            if (debugLog) { log.Info("GetPhoneNumberAsync"); }

            return user.PhoneNumber;
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            if (debugLog) { log.Info("GetPhoneNumberConfirmedAsync"); }

            return user.PhoneNumberConfirmed;
        }

        public async Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            if (debugLog) { log.Info("SetPhoneNumberAsync"); }

            user.PhoneNumber = phoneNumber;
            bool result = await repo.Save(user);
        }

        public async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            if (debugLog) { log.Info("SetPhoneNumberConfirmedAsync"); }

            user.PhoneNumberConfirmed = confirmed;
            bool result = await repo.Save(user);

        }

        #endregion

        #region IUserRoleStore

        public async Task AddToRoleAsync(TUser user, string role)
        {
            if (debugLog) { log.Info("AddToRoleAsync"); }

            ISiteRole siteRole = await repo.FetchRole(siteSettings.SiteId, role);
            bool result = false;
            if (siteRole != null)
            {
                result = await repo.AddUserToRole(siteRole.RoleId, siteRole.RoleGuid, user.UserId, user.UserGuid);
            }

        }

        public async Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (debugLog) { log.Info("GetRolesAsync"); }

            IList<string> roles = new List<string>();

            if (user == null)
            {
                return roles;
            }

            roles = await repo.GetUserRoles(siteSettings.SiteId, user.UserId);

            return roles;
        }

        public async Task<bool> IsInRoleAsync(TUser user, string role)
        {
            if (debugLog) { log.Info("IsInRoleAsync"); }

            bool result = false;
            if (user == null) { return result; }

            IList<string> roles = await repo.GetUserRoles(siteSettings.SiteId, user.UserId);

            foreach (string r in roles)
            {
                if (string.Equals(r, role, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public async Task RemoveFromRoleAsync(TUser user, string role)
        {
            if (debugLog) { log.Info("RemoveFromRoleAsync"); }

            ISiteRole siteRole = await repo.FetchRole(siteSettings.SiteId, role);
            bool result = false;
            if (siteRole != null)
            {
                result = await repo.RemoveUserFromRole(siteRole.RoleId, user.UserId);
            }

        }

        #endregion

        #region Helpers

        public string SuggestLoginNameFromEmail(int siteId, string email)
        {
            string login = email.Substring(0, email.IndexOf("@"));
            int offset = 1;
            // don't think we should make this async inside a loop
            while (repo.LoginExistsInDB(siteId, login))
            {
                offset += 1;
                login = email.Substring(0, email.IndexOf("@")) + offset.ToInvariantString();

            }

            return login;
        }

        #endregion


        public void Dispose()
        {

        }

    }
}
