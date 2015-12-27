// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-22
// Last Modified:		    2015-12-27
// 

using cloudscribe.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
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
        
        private UserStore() { }

        public UserStore(
            ILogger<UserStore<TUser>> logger,
            ISiteResolver siteResolver,
            IUserRepository userRepository,
            //ConfigHelper config,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor
            )
        {
            
            log = logger;

            if (siteResolver == null) { throw new ArgumentNullException(nameof(siteResolver)); }
            resolver = siteResolver;

            if (userRepository == null) { throw new ArgumentNullException(nameof(userRepository)); }
            repo = userRepository;
            
         
            multiTenantOptions = multiTenantOptionsAccessor.Value;

            //debugLog = config.GetOrDefault("AppSettings:UserStoreDebugEnabled", false);

            if (debugLog) { log.LogInformation("constructor"); }
        }

        private ILogger log;
        private bool debugLog = false;
        private ISiteResolver resolver;
        private ISiteSettings _siteSettings = null;
        private MultiTenantOptions multiTenantOptions;


        private ISiteSettings siteSettings
        {
            get
            {
                if (_siteSettings == null)
                {
                    _siteSettings = resolver.Resolve();
                }
                return _siteSettings;
            }
        }
        private IUserRepository repo;

        #region IUserStore


        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("CreateAsync"); }

            cancellationToken.ThrowIfCancellationRequested();
 
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

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

            cancellationToken.ThrowIfCancellationRequested();

            bool result = await repo.Save(user, cancellationToken);
            

            
            //IdentityResult identityResult;
            if (result)
            {
                cancellationToken.ThrowIfCancellationRequested();
                result = result && await AddUserToDefaultRoles(user, cancellationToken);
                    
            }
            //else
            //{
            //    identutyResult = IdentityResult.Failed;
            //}

            return IdentityResult.Success;


        }

        private async Task<bool> AddUserToDefaultRoles(ISiteUser siteUser, CancellationToken cancellationToken)
        {
            
            ISiteRole role;
            bool result = true;
            
            string defaultRoles = multiTenantOptions.DefaultNewUserRoles;

            if (defaultRoles.Length > 0)
            {
                if (defaultRoles.IndexOf(";") == -1)
                {
                    role = await repo.FetchRole(siteUser.SiteId, defaultRoles, cancellationToken);
                    if ((role != null) && (role.RoleId > -1))
                    {
                        result = await repo.AddUserToRole(role.RoleId, role.RoleGuid, siteUser.UserId, siteUser.UserGuid, cancellationToken);
                    }
                }
                else
                {
                    string[] roleArray = defaultRoles.Split(';');
                    foreach (string roleName in roleArray)
                    {
                        if (!string.IsNullOrEmpty(roleName))
                        {
                            role = await repo.FetchRole(siteUser.SiteId, roleName, cancellationToken);
                            if ((role != null) && (role.RoleId > -1))
                            {
                                result = result && await repo.AddUserToRole(role.RoleId, role.RoleGuid, siteUser.UserId, siteUser.UserGuid, cancellationToken);
                            }
                        }
                    }

                }

            }

            return result;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("UpdateAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            cancellationToken.ThrowIfCancellationRequested();
            bool result = await repo.Save(user, cancellationToken);

            return IdentityResult.Success;

        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("UpdateAsync"); }

            if (siteSettings.ReallyDeleteUsers)
            {
                //Task[] tasks = new Task[4];
                //cancellationToken.ThrowIfCancellationRequested();
                //tasks[0] = repo.DeleteLoginsByUser(-1, user.Id);
                //cancellationToken.ThrowIfCancellationRequested();
                //tasks[1] = repo.DeleteClaimsByUser(-1, user.Id);
                //cancellationToken.ThrowIfCancellationRequested();
                //tasks[2] = repo.DeleteUserRoles(user.UserId);
                //cancellationToken.ThrowIfCancellationRequested();
                //tasks[3] = repo.Delete(user.UserId);
                //cancellationToken.ThrowIfCancellationRequested();

                //Task.WaitAll(tasks);
                await repo.Delete(user.SiteId, user.UserId, cancellationToken);
            }
            else
            {
                cancellationToken.ThrowIfCancellationRequested();
                bool result = await repo.FlagAsDeleted(user.UserId, cancellationToken);
            }

            return IdentityResult.Success;
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("FindByIdAsync"); }
            cancellationToken.ThrowIfCancellationRequested();

            Guid userGuid = new Guid(userId);

            int siteId = siteSettings.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            ISiteUser siteUser = await repo.Fetch(siteId, userGuid, cancellationToken);

            return (TUser)siteUser;
        }


        public async Task<TUser> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("FindByNameAsync"); }
            cancellationToken.ThrowIfCancellationRequested();

            int siteId = siteSettings.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            ISiteUser siteUser = await repo.FetchByLoginName(siteId, userName, true, cancellationToken);
            return (TUser)siteUser;

        }

        public async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("SetEmailAsync"); }

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.SiteGuid == Guid.Empty)
            {
                user.SiteGuid = siteSettings.SiteGuid;
            }
            if (user.SiteId == -1)
            {
                user.SiteId = siteSettings.SiteId;
            }

            user.UserName = userName;
            cancellationToken.ThrowIfCancellationRequested();
            bool result = await repo.Save(user, cancellationToken);

        }

        public Task SetNormalizedUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("SetNormalizedUserNameAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.SiteGuid == Guid.Empty)
            {
                user.SiteGuid = siteSettings.SiteGuid;
            }
            if (user.SiteId == -1)
            {
                user.SiteId = siteSettings.SiteId;
            }

            user.UserName = userName;
            //cancellationToken.ThrowIfCancellationRequested();
            //bool result = await repo.Save(user);

            return Task.FromResult(0);

        }

        
//#pragma warning disable 1998

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetUserIdAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.UserGuid.ToString());

            
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetUserNameAsync"); }

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.UserName);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetUserNameAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.UserName);

            //return user.UserName.ToLowerInvariant();
        }

//#pragma warning restore 1998

        #endregion

        #region IUserEmailStore

        

        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetEmailAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Email);
        }

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetEmailAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.LoweredEmail);
        }



        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetEmailConfirmedAsync"); }

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.EmailConfirmed);
        }



        public async Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("SetEmailAsync"); }

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.SiteGuid == Guid.Empty)
            {
                user.SiteGuid = siteSettings.SiteGuid;
            }
            if (user.SiteId == -1)
            {
                user.SiteId = siteSettings.SiteId;
            }

            cancellationToken.ThrowIfCancellationRequested();

            user.Email = email;
            user.LoweredEmail = email.ToLower();

            bool result = await repo.Save(user, cancellationToken);

        }

        public Task SetNormalizedEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("SetEmailAsync"); }

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

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

            //cancellationToken.ThrowIfCancellationRequested();

            //bool result = await repo.Save(user);
            return Task.FromResult(0);

        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("SetEmailConfirmedAsync"); }

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.SiteGuid == Guid.Empty)
            {
                user.SiteGuid = siteSettings.SiteGuid;
            }
            if (user.SiteId == -1)
            {
                user.SiteId = siteSettings.SiteId;
            }
            cancellationToken.ThrowIfCancellationRequested();

            user.EmailConfirmed = confirmed;

           
            // I don't not sure this method is expected to
            // persist this change to the db
            //bool result = await repo.Save(user);
            return Task.FromResult(0);

        }

        public async Task<TUser> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("FindByEmailAsync"); }
            cancellationToken.ThrowIfCancellationRequested();

            int siteId = siteSettings.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            ISiteUser siteUser = await repo.Fetch(siteId, email, cancellationToken);

            return (TUser)siteUser;
        }

        #endregion

        #region IUserPasswordStore

        
//#pragma warning disable 1998

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetPasswordHashAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("HasPasswordAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("SetPasswordHashAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
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

            return Task.FromResult(0);

        }

//#pragma warning restore 1998

        #endregion

        #region IUserLockoutStore

        
        public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetAccessFailedCountAsync"); }

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.FailedPasswordAttemptCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetLockoutEnabledAsync"); }

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.IsLockedOut);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetLockoutEndDateAsync"); }

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            DateTimeOffset? d = new DateTimeOffset(DateTime.MinValue);
            if (user.LockoutEndDateUtc != null)
            {
                d = new DateTimeOffset(user.LockoutEndDateUtc.Value);
                return Task.FromResult(d);
            }
            return null;
        }




        public async Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("IncrementAccessFailedCountAsync"); }

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.FailedPasswordAttemptCount += 1;
            cancellationToken.ThrowIfCancellationRequested();
            await repo.UpdateFailedPasswordAttemptCount(user.UserGuid, user.FailedPasswordAttemptCount, cancellationToken);
            return user.FailedPasswordAttemptCount;
        }

        public async Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("ResetAccessFailedCountAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.FailedPasswordAttemptCount = 0;
            // EF implementation doesn't save here
            // but we have to since our save doesn't update this
            // we have specific methods as shown here
            bool result = await repo.UpdateFailedPasswordAttemptCount(user.UserGuid, user.FailedPasswordAttemptCount, cancellationToken);

        }

        public async Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("SetLockoutEnabledAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            bool result;
            // EF implementation doesn't save here but I think our save doesn't set lockout so we have to

            if (enabled)
            {
                result = await repo.LockoutAccount(user.UserGuid, cancellationToken);
            }
            else
            {
                result = await repo.UnLockAccount(user.UserGuid, cancellationToken);
            }

        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("SetLockoutEndDateAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (lockoutEnd.HasValue)
            {
                user.LockoutEndDateUtc = lockoutEnd.Value.DateTime;
            }
            else
            {
                user.LockoutEndDateUtc = DateTime.MinValue;
            }

            // EF implementation doesn't save here
            //bool result = await repo.Save(user);
            return Task.FromResult(0);
        }



        #endregion

        #region IUserClaimStore

        public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("AddClaimsAsync"); }

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            int siteId = siteSettings.SiteId;
            if(multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            foreach (Claim claim in claims)
            {
                UserClaim userClaim = new UserClaim();
                userClaim.SiteId = siteId;
                userClaim.UserId = user.UserGuid.ToString();
                userClaim.ClaimType = claim.Type;
                userClaim.ClaimValue = claim.Value;
                cancellationToken.ThrowIfCancellationRequested();
                bool result = await repo.SaveClaim(userClaim, cancellationToken);
            }
            

        }

        public async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("RemoveClaimAsync"); }

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            int siteId = siteSettings.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            foreach (Claim claim in claims)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await repo.DeleteClaimByUser(siteId, user.Id, claim.Type, cancellationToken);
            }
           
        }

        public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("ReplaceClaimAsync"); }

            cancellationToken.ThrowIfCancellationRequested();

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            int siteId = siteSettings.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            await repo.DeleteClaimByUser(siteId, user.Id, claim.Type, cancellationToken);

            UserClaim userClaim = new UserClaim();
            userClaim.SiteId = siteId;
            userClaim.UserId = user.UserGuid.ToString();
            userClaim.ClaimType = newClaim.Type;
            userClaim.ClaimValue = newClaim.Value;
            cancellationToken.ThrowIfCancellationRequested();
            bool result = await repo.SaveClaim(userClaim, cancellationToken);

        }

        public async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetClaimsAsync"); }

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IList<Claim> claims = new List<Claim>();
            cancellationToken.ThrowIfCancellationRequested();

            int siteId = siteSettings.SiteId;
            if(multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            IList<IUserClaim> userClaims = await repo.GetClaimsByUser(siteId, user.Id, cancellationToken);
            foreach (UserClaim uc in userClaims)
            {
                Claim c = new Claim(uc.ClaimType, uc.ClaimValue);
                claims.Add(c);
            }

            return claims;

        }

        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetRolesAsync"); }

            ThrowIfDisposed();
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            cancellationToken.ThrowIfCancellationRequested();

            int siteId = siteSettings.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            IList<ISiteUser> users = await repo.GetUsersForClaim(siteId, claim.Type, claim.Value, cancellationToken);

            return (IList<TUser>)users;
        }

        #endregion

        #region IUserLoginStore

        public async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("AddLoginAsync"); }

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            int siteId = siteSettings.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            UserLogin userlogin = new UserLogin();
            userlogin.SiteId = siteId;
            userlogin.UserId = user.UserGuid.ToString();
            userlogin.LoginProvider = login.LoginProvider;
            userlogin.ProviderKey = login.ProviderKey;
            userlogin.ProviderDisplayName = login.ProviderDisplayName;

            cancellationToken.ThrowIfCancellationRequested();
            bool result = await repo.CreateLogin(userlogin, cancellationToken);

        }

        public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("FindAsync called for " + loginProvider + " with providerKey " + providerKey); }

            log.LogInformation("FindAsync called for " + loginProvider + " with providerKey " + providerKey);
            cancellationToken.ThrowIfCancellationRequested();

            int siteId = siteSettings.SiteId;
            if(multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            IUserLogin userlogin = await repo.FindLogin(siteId, loginProvider, providerKey, cancellationToken);
            if (userlogin != null && userlogin.UserId.Length == 36)
            {
                log.LogInformation("FindAsync userLogin found for " + loginProvider + " with providerKey " + providerKey);

                Guid userGuid = new Guid(userlogin.UserId);
                cancellationToken.ThrowIfCancellationRequested();
                ISiteUser siteUser = await repo.Fetch(siteId, userGuid, cancellationToken);
                if (siteUser != null)
                {
                    return (TUser)siteUser;
                }
                else
                {
                    log.LogInformation("FindAsync siteUser not found for " + loginProvider + " with providerKey " + providerKey);
                   
                }
            }
            else
            {
                log.LogInformation("FindAsync userLogin not found for " + loginProvider + " with providerKey " + providerKey);
            }

            return default(TUser);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetLoginsAsync"); }
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IList<UserLoginInfo> logins = new List<UserLoginInfo>();
            cancellationToken.ThrowIfCancellationRequested();

            int siteId = siteSettings.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            IList<IUserLogin> userLogins = await repo.GetLoginsByUser(siteId, user.UserGuid.ToString(), cancellationToken);
            foreach (UserLogin ul in userLogins)
            {
                UserLoginInfo l = new UserLoginInfo(ul.LoginProvider, ul.ProviderKey, ul.ProviderDisplayName);
                logins.Add(l);
            }


            return logins;
        }

        public async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("RemoveLoginAsync"); }
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            cancellationToken.ThrowIfCancellationRequested();

            int siteId = siteSettings.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            bool result = await repo.DeleteLogin(
                siteId,
                loginProvider,
                providerKey,
                user.UserGuid.ToString(),
                cancellationToken
                );


        }

        #endregion

        #region IUserTwoFactorStore


        public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetTwoFactorEnabledAsync"); }

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.TwoFactorEnabled);
        }


        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("SetTwoFactorEnabledAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.TwoFactorEnabled = enabled;
            
            //EF implementation doesn't save here
            //bool result = await repo.Save(user);

            return Task.FromResult(0);
        }


        #endregion

        #region IUserPhoneNumberStore

       
        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetPhoneNumberAsync"); }

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetPhoneNumberConfirmedAsync"); }

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PhoneNumberConfirmed);
        }



        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("SetPhoneNumberAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PhoneNumber = phoneNumber;
            // EF implementation doesn't save here
            //bool result = await repo.Save(user);
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("SetPhoneNumberConfirmedAsync"); }
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PhoneNumberConfirmed = confirmed;
            // EF implementation does not save here
            //bool result = await repo.Save(user);
            return Task.FromResult(0);

        }

        #endregion

        #region IUserRoleStore

        public async Task AddToRoleAsync(TUser user, string role, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("AddToRoleAsync"); }

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            cancellationToken.ThrowIfCancellationRequested();

            int siteId = siteSettings.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            ISiteRole siteRole = await repo.FetchRole(siteId, role, cancellationToken);
            bool result = false;
            if (siteRole != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                result = await repo.AddUserToRole(siteRole.RoleId, siteRole.RoleGuid, user.UserId, user.UserGuid, cancellationToken);
            }

        }

        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetRolesAsync"); }

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IList<string> roles = new List<string>();

            cancellationToken.ThrowIfCancellationRequested();

            int siteId = siteSettings.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            roles = await repo.GetUserRoles(siteId, user.UserId, cancellationToken);

            return roles;
        }

        public async Task<bool> IsInRoleAsync(TUser user, string role, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("IsInRoleAsync"); }

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            bool result = false;

            cancellationToken.ThrowIfCancellationRequested();
            IList<string> roles = await repo.GetUserRoles(siteSettings.SiteId, user.UserId, cancellationToken);

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

        public async Task<IList<TUser>> GetUsersInRoleAsync(string role, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("GetRolesAsync"); }
            cancellationToken.ThrowIfCancellationRequested();

            int siteId = siteSettings.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            IList<ISiteUser> users = await repo.GetUsersInRole(siteId, role, cancellationToken);

            return (IList<TUser>)users; 
        }

        public async Task RemoveFromRoleAsync(TUser user, string role, CancellationToken cancellationToken)
        {
            if (debugLog) { log.LogInformation("RemoveFromRoleAsync"); }
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            cancellationToken.ThrowIfCancellationRequested();

            int siteId = siteSettings.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            ISiteRole siteRole = await repo.FetchRole(siteId, role, cancellationToken);
            bool result = false;
            if (siteRole != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                result = await repo.RemoveUserFromRole(siteRole.RoleId, user.UserId, cancellationToken);
            }

        }

        #endregion

        #region Helpers

        public string SuggestLoginNameFromEmail(int siteId, string email)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

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

        private void ThrowIfDisposed()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SiteRoleStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
