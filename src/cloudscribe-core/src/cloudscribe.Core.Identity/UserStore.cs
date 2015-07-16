// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-22
// Last Modified:		    2015-07-06
// 


using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Logging;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;


namespace cloudscribe.Core.Identity
{
    public sealed class UserStore<TUser> :
        IUserStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserEmailStore<TUser>,
        IUserLoginStore<TUser>,
        IUserRoleStore<TUser>,
        IUserClaimStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IUserLockoutStore<TUser>,
        IUserTwoFactorStore<TUser>
        where TUser : SiteUser
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(UserStore<TUser>));
        private ILoggerFactory logFactory;
        private ILogger log;
        private bool debugLog = false;
        private ISiteResolver resolver;
        private ISiteSettings _siteSettings = null;
        private IConfiguration config;

        public ISiteSettings siteSettings
        {
            get {
                if(_siteSettings == null)
                {
                    _siteSettings = resolver.Resolve();
                }
                return _siteSettings;
            }
        }
        private IUserRepository repo;


        private UserStore() { }

        public UserStore(
            ILoggerFactory loggerFactory,
            ISiteResolver siteResolver,
            IUserRepository userRepository,
            IConfiguration configuration
            )
        {
            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(this.GetType().FullName);

            if (siteResolver == null) { throw new ArgumentNullException(nameof(siteResolver)); }
            resolver = siteResolver;

            if (userRepository == null) { throw new ArgumentNullException(nameof(userRepository)); }
            repo = userRepository;

            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }
            config = configuration;

            debugLog = config.UserStoreDebugEnabled();

            if (debugLog) { log.LogInformation("constructor"); }
        }


        #region IUserStore


        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("CreateAsync"); }

            token.ThrowIfCancellationRequested();

            if (user.SiteGuid == Guid.Empty)
            {
                user.SiteGuid = siteSettings.SiteGuid;
            }
            if (user.SiteId == -1)
            {
                user.SiteId = siteSettings.SiteId;
            }

            if (user.UserName.Length == 0)
            {
                user.UserName = SuggestLoginNameFromEmail(siteSettings.SiteId, user.Email);
            }

            if (user.DisplayName.Length == 0)
            {
                user.DisplayName = SuggestLoginNameFromEmail(siteSettings.SiteId, user.Email);
            }

            token.ThrowIfCancellationRequested();

            bool result = await repo.Save(user);
            

            
            //IdentityResult identityResult;
            if (result)
            {
                token.ThrowIfCancellationRequested();
                result = result && await repo.AddUserToDefaultRoles(user);
                    
            }
            //else
            //{
            //    identutyResult = IdentityResult.Failed;
            //}

            return IdentityResult.Success;


        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("UpdateAsync"); }
            token.ThrowIfCancellationRequested();
            bool result = await repo.Save(user);

            return IdentityResult.Success;

        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("UpdateAsync"); }

            if (siteSettings.ReallyDeleteUsers)
            {
                Task[] tasks = new Task[4];
                token.ThrowIfCancellationRequested();
                tasks[0] = repo.DeleteLoginsByUser(user.Id);
                token.ThrowIfCancellationRequested();
                tasks[1] = repo.DeleteClaimsByUser(user.Id);
                token.ThrowIfCancellationRequested();
                tasks[2] = repo.DeleteUserRoles(user.UserId);
                token.ThrowIfCancellationRequested();
                tasks[3] = repo.Delete(user.UserId);
                token.ThrowIfCancellationRequested();

                Task.WaitAll(tasks);
            }
            else
            {
                token.ThrowIfCancellationRequested();
                bool result = await repo.FlagAsDeleted(user.UserId);
            }

            return IdentityResult.Success;
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("FindByIdAsync"); }
            token.ThrowIfCancellationRequested();

            Guid userGuid = new Guid(userId);

            ISiteUser siteUser = await repo.Fetch(siteSettings.SiteId, userGuid);

            return (TUser)siteUser;
        }


        public async Task<TUser> FindByNameAsync(string userName, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("FindByNameAsync"); }
            token.ThrowIfCancellationRequested();
            ISiteUser siteUser = await repo.FetchByLoginName(siteSettings.SiteId, userName, true);
            return (TUser)siteUser;

        }

        public async Task SetUserNameAsync(TUser user, string userName, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("SetEmailAsync"); }

            if (user.SiteGuid == Guid.Empty)
            {
                user.SiteGuid = siteSettings.SiteGuid;
            }
            if (user.SiteId == -1)
            {
                user.SiteId = siteSettings.SiteId;
            }

            user.UserName = userName;
            token.ThrowIfCancellationRequested();
            bool result = await repo.Save(user);

        }

        public async Task SetNormalizedUserNameAsync(TUser user, string userName, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("SetEmailAsync"); }

            if (user.SiteGuid == Guid.Empty)
            {
                user.SiteGuid = siteSettings.SiteGuid;
            }
            if (user.SiteId == -1)
            {
                user.SiteId = siteSettings.SiteId;
            }

            user.UserName = userName;
            token.ThrowIfCancellationRequested();
            bool result = await repo.Save(user);

        }

        //disable warning about not really being async
        // we know it is not, it is not needed to hit the db in these
#pragma warning disable 1998

        public async Task<string> GetUserIdAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetUserIdAsync"); }

            return user.UserGuid.ToString();
        }

        public async Task<string> GetUserNameAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetUserNameAsync"); }

            return user.UserName;
        }

        public async Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetUserNameAsync"); }

            return user.UserName.ToLowerInvariant();
        }

#pragma warning restore 1998

        #endregion

        #region IUserEmailStore

        //disable warning about not really being async
        // we know it is not, it is not needed to hit the db in these
#pragma warning disable 1998

        public async Task<string> GetEmailAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetEmailAsync"); }

            return user.Email;
        }

        public async Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetEmailAsync"); }

            return user.LoweredEmail;
        }



        public async Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetEmailConfirmedAsync"); }

            return user.EmailConfirmed;
        }

#pragma warning restore 1998

        public async Task SetEmailAsync(TUser user, string email, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("SetEmailAsync"); }

            if (user.SiteGuid == Guid.Empty)
            {
                user.SiteGuid = siteSettings.SiteGuid;
            }
            if (user.SiteId == -1)
            {
                user.SiteId = siteSettings.SiteId;
            }

            token.ThrowIfCancellationRequested();

            user.Email = email;
            user.LoweredEmail = email.ToLower();

            bool result = await repo.Save(user);

        }

        public async Task SetNormalizedEmailAsync(TUser user, string email, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("SetEmailAsync"); }

            if (user.SiteGuid == Guid.Empty)
            {
                user.SiteGuid = siteSettings.SiteGuid;
            }
            if (user.SiteId == -1)
            {
                user.SiteId = siteSettings.SiteId;
            }

            //user.Email = email;
            user.LoweredEmail = email.ToLower();

            token.ThrowIfCancellationRequested();

            bool result = await repo.Save(user);

        }

        public async Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("SetEmailConfirmedAsync"); }

            if (user.SiteGuid == Guid.Empty)
            {
                user.SiteGuid = siteSettings.SiteGuid;
            }
            if (user.SiteId == -1)
            {
                user.SiteId = siteSettings.SiteId;
            }

            user.EmailConfirmed = confirmed;

            token.ThrowIfCancellationRequested();
            // I don't not sure this method is expected to
            // persist this change to the db
            bool result = await repo.Save(user);

        }

        public async Task<TUser> FindByEmailAsync(string email, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("FindByEmailAsync"); }
            token.ThrowIfCancellationRequested();
            ISiteUser siteUser = await repo.Fetch(siteSettings.SiteId, email);

            return (TUser)siteUser;
        }

        #endregion

        #region IUserPasswordStore

        //disable warning about not really being async
        // we know it is not, it is not needed to hit the db in these
#pragma warning disable 1998

        public async Task<string> GetPasswordHashAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetPasswordHashAsync"); }

            if (user == null)
            {
                return string.Empty;
            }

            return user.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("HasPasswordAsync"); }

            if (user == null)
            {
                return false;
            }

            return string.IsNullOrEmpty(user.PasswordHash);
        }

        public async Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("SetPasswordHashAsync"); }

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
            //token.ThrowIfCancellationRequested();
            //user.PasswordHash = passwordHash;
            //userRepo.Save(user);



        }

#pragma warning restore 1998

        #endregion

        #region IUserLockoutStore

        //disable warning about not really being async
        // we know it is not, it is not needed to hit the db in these
#pragma warning disable 1998

        public async Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetAccessFailedCountAsync"); }

            return user.FailedPasswordAttemptCount;
        }

        public async Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetLockoutEnabledAsync"); }

            return user.IsLockedOut;
        }

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetLockoutEndDateAsync"); }

            DateTimeOffset d = new DateTimeOffset(DateTime.MinValue);
            if (user.LockoutEndDateUtc != null)
            {
                d = new DateTimeOffset(user.LockoutEndDateUtc.Value);
                return d;
            }
            return null;
        }


#pragma warning restore 1998

        public async Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("IncrementAccessFailedCountAsync"); }

            user.FailedPasswordAttemptCount += 1;
            token.ThrowIfCancellationRequested();
            await repo.UpdateFailedPasswordAttemptCount(user.UserGuid, user.FailedPasswordAttemptCount);
            return user.FailedPasswordAttemptCount;
        }

        public async Task ResetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("ResetAccessFailedCountAsync"); }

            user.FailedPasswordAttemptCount = 0;
            token.ThrowIfCancellationRequested();
            bool result = await repo.UpdateFailedPasswordAttemptCount(user.UserGuid, user.FailedPasswordAttemptCount);

        }

        public async Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("SetLockoutEnabledAsync"); }
            bool result;
            token.ThrowIfCancellationRequested();
            if (enabled)
            {
                result = await repo.LockoutAccount(user.UserGuid);
            }
            else
            {
                result = await repo.UnLockAccount(user.UserGuid);
            }


        }

        public async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("SetLockoutEndDateAsync"); }

            if(lockoutEnd.HasValue)
            {
                user.LockoutEndDateUtc = lockoutEnd.Value.DateTime;
            }
            else
            {
                user.LockoutEndDateUtc = DateTime.MinValue;
            }

            token.ThrowIfCancellationRequested();
            bool result = await repo.Save(user);
        }



        #endregion

        #region IUserClaimStore

        public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("AddClaimsAsync"); }

            foreach(Claim claim in claims)
            {
                UserClaim userClaim = new UserClaim();
                userClaim.UserId = user.UserGuid.ToString();
                userClaim.ClaimType = claim.Type;
                userClaim.ClaimValue = claim.Value;
                token.ThrowIfCancellationRequested();
                bool result = await repo.SaveClaim(userClaim);
            }
            

        }

        public async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("RemoveClaimAsync"); }
            foreach(Claim claim in claims)
            {
                token.ThrowIfCancellationRequested();
                await repo.DeleteClaimByUser(user.Id, claim.Type);
            }
           
        }

        public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("ReplaceClaimAsync"); }

            token.ThrowIfCancellationRequested();
            await repo.DeleteClaimByUser(user.Id, claim.Type);

            UserClaim userClaim = new UserClaim();
            userClaim.UserId = user.UserGuid.ToString();
            userClaim.ClaimType = newClaim.Type;
            userClaim.ClaimValue = newClaim.Value;
            token.ThrowIfCancellationRequested();
            bool result = await repo.SaveClaim(userClaim);

        }

        public async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetClaimsAsync"); }

            IList<Claim> claims = new List<Claim>();
            token.ThrowIfCancellationRequested();
            IList<IUserClaim> userClaims = await repo.GetClaimsByUser(user.Id);
            foreach (UserClaim uc in userClaims)
            {
                Claim c = new Claim(uc.ClaimType, uc.ClaimValue);
                claims.Add(c);
            }

            return claims;

        }

        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetRolesAsync"); }
            token.ThrowIfCancellationRequested();
            IList<ISiteUser> users = await repo.GetUsersForClaim(siteSettings.SiteId, claim.Type, claim.Value);

            return (IList<TUser>)users;
        }

        #endregion

        #region IUserLoginStore

        public async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("AddLoginAsync"); }

            UserLogin userlogin = new UserLogin();
            userlogin.UserId = user.UserGuid.ToString();
            userlogin.LoginProvider = login.LoginProvider;
            userlogin.ProviderKey = login.ProviderKey;
            token.ThrowIfCancellationRequested();
            bool result = await repo.CreateLogin(userlogin);

        }

        public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("FindAsync"); }
            token.ThrowIfCancellationRequested();
            IUserLogin userlogin = await repo.FindLogin(loginProvider, providerKey);
            if (userlogin != null && userlogin.UserId.Length == 36)
            {
                Guid userGuid = new Guid(userlogin.UserId);
                token.ThrowIfCancellationRequested();
                ISiteUser siteUser = await repo.Fetch(siteSettings.SiteId, userGuid);
                if (siteUser != null)
                {
                    return (TUser)siteUser;
                }
            }

            return default(TUser);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetLoginsAsync"); }

            IList<UserLoginInfo> logins = new List<UserLoginInfo>();
            token.ThrowIfCancellationRequested();
            IList<IUserLogin> userLogins = await repo.GetLoginsByUser(user.UserGuid.ToString());
            foreach (UserLogin ul in userLogins)
            {
                UserLoginInfo l = new UserLoginInfo(ul.LoginProvider, ul.ProviderKey, ul.LoginProvider);
                logins.Add(l);
            }


            return logins;
        }

        public async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("RemoveLoginAsync"); }
            token.ThrowIfCancellationRequested();
            bool result = await repo.DeleteLogin(
                loginProvider,
                providerKey,
                user.UserGuid.ToString()
                );


        }

        #endregion

        #region IUserTwoFactorStore

        //disable warning about not really being async
        // we know it is not, it is not needed to hit the db in these
#pragma warning disable 1998

        public async Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetTwoFactorEnabledAsync"); }

            return user.TwoFactorEnabled;
        }

#pragma warning restore 1998

        public async Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("SetTwoFactorEnabledAsync"); }

            user.TwoFactorEnabled = enabled;
            token.ThrowIfCancellationRequested();
            bool result = await repo.Save(user);


        }


        #endregion

        #region IUserPhoneNumberStore

        //disable warning about not really being async
        // we know it is not, it is not needed to hit the db in these
#pragma warning disable 1998

        public async Task<string> GetPhoneNumberAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetPhoneNumberAsync"); }

            return user.PhoneNumber;
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetPhoneNumberConfirmedAsync"); }

            return user.PhoneNumberConfirmed;
        }

#pragma warning restore 1998

        public async Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("SetPhoneNumberAsync"); }

            user.PhoneNumber = phoneNumber;
            token.ThrowIfCancellationRequested();
            bool result = await repo.Save(user);
        }

        public async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("SetPhoneNumberConfirmedAsync"); }

            user.PhoneNumberConfirmed = confirmed;
            token.ThrowIfCancellationRequested();
            bool result = await repo.Save(user);

        }

        #endregion

        #region IUserRoleStore

        public async Task AddToRoleAsync(TUser user, string role, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("AddToRoleAsync"); }
            token.ThrowIfCancellationRequested();
            ISiteRole siteRole = await repo.FetchRole(siteSettings.SiteId, role);
            bool result = false;
            if (siteRole != null)
            {
                token.ThrowIfCancellationRequested();
                result = await repo.AddUserToRole(siteRole.RoleId, siteRole.RoleGuid, user.UserId, user.UserGuid);
            }

        }

        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetRolesAsync"); }

            IList<string> roles = new List<string>();

            if (user == null)
            {
                return roles;
            }
            token.ThrowIfCancellationRequested();
            roles = await repo.GetUserRoles(siteSettings.SiteId, user.UserId);

            return roles;
        }

        public async Task<bool> IsInRoleAsync(TUser user, string role, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("IsInRoleAsync"); }

            bool result = false;
            if (user == null) { return result; }
            token.ThrowIfCancellationRequested();
            IList<string> roles = await repo.GetUserRoles(siteSettings.SiteId, user.UserId);

            foreach (string r in roles)
            {
                if (string.Equals(r, role, StringComparison.CurrentCultureIgnoreCase))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public async Task<IList<TUser>> GetUsersInRoleAsync(string role, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("GetRolesAsync"); }
            token.ThrowIfCancellationRequested();
            IList<ISiteUser> users = await repo.GetUsersInRole(siteSettings.SiteId, role);

            return (IList<TUser>)users; 
        }

        public async Task RemoveFromRoleAsync(TUser user, string role, CancellationToken token)
        {
            if (debugLog) { log.LogInformation("RemoveFromRoleAsync"); }
            token.ThrowIfCancellationRequested();
            ISiteRole siteRole = await repo.FetchRole(siteSettings.SiteId, role);
            bool result = false;
            if (siteRole != null)
            {
                token.ThrowIfCancellationRequested();
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
