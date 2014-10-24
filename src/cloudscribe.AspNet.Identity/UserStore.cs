// Author:					Joe Audette
// Created:				    2014-07-22
// Last Modified:		    2014-08-25
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

            bool result = repo.Save(user);
            if(result)
            {
                repo.AddUserToDefaultRoles(user);
            }
            


            await Task.FromResult(result);

        }

        public async Task UpdateAsync(TUser user)
        {
            if (debugLog) { log.Info("UpdateAsync"); }

            bool result = repo.Save(user);


            await Task.FromResult(result);

        }

        public async Task DeleteAsync(TUser user)
        {
            if (debugLog) { log.Info("UpdateAsync"); }

            if(siteSettings.ReallyDeleteUsers)
            {
                repo.DeleteLoginsByUser(user.Id);
                repo.DeleteClaimsByUser(user.Id);
                repo.DeleteUserRoles(user.UserId);
                repo.Delete(user.UserId);
            }
            else
            {
                repo.FlagAsDeleted(user.UserId);
            }


            await Task.FromResult(0);

        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            if (debugLog) { log.Info("FindByIdAsync"); }

            Guid userGuid = new Guid(userId);

            ISiteUser siteUser = repo.Fetch(siteSettings.SiteId, userGuid);

            return Task.FromResult((TUser)siteUser);
        }


        public Task<TUser> FindByNameAsync(string userName)
        {
            if (debugLog) { log.Info("FindByNameAsync"); }

            ISiteUser siteUser = repo.FetchByLoginName(siteSettings.SiteId, userName, true);
            return Task.FromResult((TUser)siteUser);

        }

        #endregion

        #region IUserEmailStore

        public Task<string> GetEmailAsync(TUser user)
        {
            if (debugLog) { log.Info("GetEmailAsync"); }

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            if (debugLog) { log.Info("GetEmailConfirmedAsync"); }

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailAsync(TUser user, string email)
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

            

            bool result = repo.Save(user);
            return Task.FromResult(result);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
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
            //return Task.FromResult(true);

            // I don't not sure this method is expected to
            // persist this change to the db
            bool result = repo.Save(user);
            return Task.FromResult(result);
        }

        public Task<TUser> FindByEmailAsync(string email)
        {
            if (debugLog) { log.Info("FindByEmailAsync"); }

            ISiteUser siteUser = repo.Fetch(siteSettings.SiteId, email);

            return Task.FromResult((TUser)siteUser);
        }

        #endregion

        #region IUserPasswordStore

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            if (debugLog) { log.Info("GetPasswordHashAsync"); }

            if (user == null)
            {
                return Task.FromResult(string.Empty);
            }

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            if (debugLog) { log.Info("HasPasswordAsync"); }

            if (user == null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            if (debugLog) { log.Info("SetPasswordHashAsync"); }

            if (user == null)
            {
                return Task.FromResult(0);
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


            return Task.FromResult(0);
        }

        #endregion

        #region IUserLockoutStore

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            if (debugLog) { log.Info("GetAccessFailedCountAsync"); }

            return Task.FromResult(user.FailedPasswordAttemptCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            if (debugLog) { log.Info("GetLockoutEnabledAsync"); }

            return Task.FromResult(user.IsLockedOut);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            if (debugLog) { log.Info("GetLockoutEndDateAsync"); }

            DateTimeOffset d = new DateTimeOffset(DateTime.MinValue);
            if(user.LockoutEndDateUtc != null)
            {
                d = new DateTimeOffset(user.LockoutEndDateUtc.Value);
                
            }
            return Task.FromResult(d);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            if (debugLog) { log.Info("IncrementAccessFailedCountAsync"); }

            user.FailedPasswordAttemptCount += 1;
            repo.UpdateFailedPasswordAttemptCount(user.UserGuid, user.FailedPasswordAttemptCount);
            return Task.FromResult(user.FailedPasswordAttemptCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            if (debugLog) { log.Info("ResetAccessFailedCountAsync"); }

            user.FailedPasswordAttemptCount = 0;
            bool result = repo.UpdateFailedPasswordAttemptCount(user.UserGuid, user.FailedPasswordAttemptCount);
            return Task.FromResult(result);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            if (debugLog) { log.Info("SetLockoutEnabledAsync"); }

            if (enabled)
            {
                return Task.FromResult(repo.LockoutAccount(user.UserGuid));
            }
            else
            {
                return Task.FromResult(repo.UnLockAccount(user.UserGuid));
            }

            
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            if (debugLog) { log.Info("SetLockoutEndDateAsync"); }

            user.LockoutEndDateUtc = lockoutEnd.DateTime;
            bool result = repo.Save(user);

            return Task.FromResult(result);
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
            bool result = repo.SaveClaim(userClaim);

            await Task.FromResult(result);
        }

        public async Task RemoveClaimAsync(TUser user, Claim claim)
        {
            if (debugLog) { log.Info("RemoveClaimAsync"); }

            await Task.FromResult(repo.DeleteClaimByUser(user.Id, claim.Type));
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            if (debugLog) { log.Info("GetClaimsAsync"); }

            IList<Claim> claims = new List<Claim>();


            IList<IUserClaim> userClaims = repo.GetClaimsByUser(user.Id);
            foreach (UserClaim uc in userClaims)
            {
                Claim c = new Claim(uc.ClaimType, uc.ClaimValue);
                claims.Add(c);
            }


            return Task.FromResult(claims);

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
            var result = repo.CreateLogin(userlogin);

            await Task.FromResult(login);
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (debugLog) { log.Info("FindAsync"); }

            //TODO make this async, probably need to make the data layer async
            //http://www.tugberkugurlu.com/archive/asynchronous-database-calls-with-task-based-asynchronous-programming-model-tap-in-asp-net-mvc-4

            IUserLogin userlogin = repo.FindLogin(login.LoginProvider, login.ProviderKey);
            if (userlogin != null && userlogin.UserId.Length == 36)
            {
                Guid userGuid = new Guid(userlogin.UserId);
                ISiteUser siteUser = repo.Fetch(siteSettings.SiteId, userGuid);
                if (siteUser != null)
                {
                    return Task.FromResult((TUser)siteUser);
                }
            }

            return Task.FromResult(default(TUser));
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (debugLog) { log.Info("GetLoginsAsync"); }

            IList<UserLoginInfo> logins = new List<UserLoginInfo>();

            IList<IUserLogin> userLogins = repo.GetLoginsByUser(user.UserGuid.ToString());
            foreach (UserLogin ul in userLogins)
            {
                UserLoginInfo l = new UserLoginInfo(ul.LoginProvider, ul.ProviderKey);
                logins.Add(l);
            }


            return Task.FromResult(logins);
        }

        public async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (debugLog) { log.Info("RemoveLoginAsync"); }

            bool result = repo.DeleteLogin(
                login.LoginProvider,
                login.ProviderKey,
                user.UserGuid.ToString()
                );

            await Task.FromResult(result);
        }

        #endregion

        #region IUserTwoFactorStore

        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            if (debugLog) { log.Info("GetTwoFactorEnabledAsync"); }

            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(TUser user,bool enabled)
        {
            if (debugLog) { log.Info("SetTwoFactorEnabledAsync"); }

            user.TwoFactorEnabled = enabled;
            bool result = repo.Save(user);

            return Task.FromResult(result);
        }


        #endregion

        #region IUserPhoneNumberStore

        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            if (debugLog) { log.Info("GetPhoneNumberAsync"); }

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            if (debugLog) { log.Info("GetPhoneNumberConfirmedAsync"); }

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            if (debugLog) { log.Info("SetPhoneNumberAsync"); }

            user.PhoneNumber = phoneNumber;
            bool result = repo.Save(user);

            return Task.FromResult(result);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            if (debugLog) { log.Info("SetPhoneNumberConfirmedAsync"); }

            user.PhoneNumberConfirmed = confirmed;
            bool result = repo.Save(user);

            return Task.FromResult(result);
        }

        #endregion

        #region IUserRoleStore

        public async Task AddToRoleAsync(TUser user, string role)
        {
            if (debugLog) { log.Info("AddToRoleAsync"); }

            ISiteRole siteRole = repo.FetchRole(siteSettings.SiteId, role);
            if (siteRole != null)
            {
                repo.AddUserToRole(siteRole.RoleId, siteRole.RoleGuid, user.UserId, user.UserGuid);
            }

            await Task.FromResult(1);
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (debugLog) { log.Info("GetRolesAsync"); }

            IList<string> roles = new List<string>();

            if (user == null)
            {
                return Task.FromResult(roles);
            }

            roles = repo.GetUserRoles(siteSettings.SiteId, user.UserId);


            //roles = SiteUser.GetRoles((SiteUser)user.mojoSiteUser);

            return Task.FromResult(roles);
        }

        public Task<bool> IsInRoleAsync(TUser user, string role)
        {
            if (debugLog) { log.Info("IsInRoleAsync"); }

            bool result = false;
            if (user == null) { return Task.FromResult(result); }

            IList<string> roles = repo.GetUserRoles(siteSettings.SiteId, user.UserId);

            foreach (string r in roles)
            {
                if (string.Equals(r, role, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = true;
                    break;
                }
            }

            return Task.FromResult(result);
        }

        public async Task RemoveFromRoleAsync(TUser user, string role)
        {
            if (debugLog) { log.Info("RemoveFromRoleAsync"); }

            ISiteRole siteRole = repo.FetchRole(siteSettings.SiteId, role);
            bool result = false;
            if (siteRole != null)
            {
                result = repo.RemoveUserFromRole(siteRole.RoleId, user.UserId);
            }

            await Task.FromResult(result);


        }

        #endregion

        public string SuggestLoginNameFromEmail(int siteId, string email)
        {
            string login = email.Substring(0, email.IndexOf("@"));
            int offset = 1;
            while (repo.LoginExistsInDB(siteId, login))
            {
                offset += 1;
                login = email.Substring(0, email.IndexOf("@")) + offset.ToInvariantString();

            }

            return login;
        }

        public void Dispose()
        {

        }

    }
}
