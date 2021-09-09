// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-22
// Last Modified:		    2018-06-21
// 

using cloudscribe.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace cloudscribe.Core.Identity
{
    public sealed class UserStore<TUser> :
        //UserStoreBase<TUser, TKey, TUserClaim, TUserLogin, TUserToken>,
        IUserStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserEmailStore<TUser>,
        IUserLoginStore<TUser>,
        IUserRoleStore<TUser>,
        IUserClaimStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IUserLockoutStore<TUser>,
        IUserTwoFactorStore<TUser>,
        IUserAuthenticationTokenStore<TUser>,
        IUserAuthenticatorKeyStore<TUser>,
        IUserTwoFactorRecoveryCodeStore<TUser>
        where TUser : SiteUser
        //where TKey : IEquatable<Guid>
        //where TUserClaim : UserClaim
        //where TUserLogin : UserLogin
        //where TUserToken : IdentityUserToken<TKey>, new()
    {
        
        //private UserStore() { }

        public UserStore(
            SiteContext currentSite,
            ILogger<UserStore<TUser>> logger,
            IUserCommands userCommands,
            IUserQueries userQueries,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<NewUserOptions> newUserOptionsAccessor,
            IdentityErrorDescriber describer = null
            ) //: base(describer ?? new IdentityErrorDescriber())
        {
            
            _log = logger;
            _siteSettings = currentSite ?? throw new ArgumentNullException(nameof(currentSite));
            _commands = userCommands ?? throw new ArgumentNullException(nameof(userCommands));
            _queries = userQueries ?? throw new ArgumentNullException(nameof(userQueries));

            _newUserOptions = newUserOptionsAccessor.Value;
            _multiTenantOptions = multiTenantOptionsAccessor.Value;

            //debugLog = config.GetOrDefault("AppSettings:UserStoreDebugEnabled", false);

            if (debugLog) { _log.LogInformation("constructor"); }
        }

        private ILogger _log;
        private bool debugLog = false;
        private ISiteContext _siteSettings = null;
        private MultiTenantOptions _multiTenantOptions;
        private NewUserOptions _newUserOptions;
        private IUserCommands _commands;
        private IUserQueries _queries;

        private ISiteContext SiteSettings
        {
            get { return _siteSettings; }
        }
  
        #region IUserStore


        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("CreateAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            // New user created from a tenant site should belong to the parent site,
            // if this related sites mode is used
            if (_multiTenantOptions.UseRelatedSitesMode) 
            { 
                if (_multiTenantOptions.RelatedSiteId != Guid.Empty)
                    user.SiteId = _multiTenantOptions.RelatedSiteId; 
            }
            else if (user.SiteId == Guid.Empty)
            {
                user.SiteId = SiteSettings.Id;
            }

            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                user.UserName = await SuggestLoginNameFromEmail(SiteSettings.Id, user.Email);
            }

            if (string.IsNullOrWhiteSpace(user.DisplayName))
            {
                user.DisplayName = await SuggestLoginNameFromEmail(SiteSettings.Id, user.Email);
            }

            cancellationToken.ThrowIfCancellationRequested();

            await _commands.Create(user, cancellationToken);
            await AddUserToDefaultRoles(user, cancellationToken);
              
            return IdentityResult.Success;


        }

        private async Task AddUserToDefaultRoles(ISiteUser siteUser, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            ISiteRole role;
            
            string defaultRoles = _newUserOptions.DefaultNewUserRoles;

            if (!string.IsNullOrWhiteSpace(defaultRoles))
            {
                if (defaultRoles.IndexOf(";") == -1)
                {
                    role = await _queries.FetchRole(siteUser.SiteId, defaultRoles, cancellationToken);
                    if ((role != null) && (role.Id != Guid.Empty))
                    {
                        await _commands.AddUserToRole(role.SiteId, role.Id, siteUser.Id, cancellationToken);
                    }
                }
                else
                {
                    string[] roleArray = defaultRoles.Split(';');
                    foreach (string roleName in roleArray)
                    {
                        if (!string.IsNullOrWhiteSpace(roleName))
                        {
                            role = await _queries.FetchRole(siteUser.SiteId, roleName, cancellationToken);
                            if ((role != null) && (role.Id != Guid.Empty))
                            {
                                await _commands.AddUserToRole(role.SiteId, role.Id, siteUser.Id, cancellationToken);
                            }
                        }
                    }

                }

            }

           
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("UpdateAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            await _commands.Update(user, cancellationToken);

            return IdentityResult.Success;

        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("UpdateAsync"); 
            
            await _commands.Delete(user.SiteId, user.Id, cancellationToken);
            
            return IdentityResult.Success;
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogTrace("FindByIdAsync"); 
            
            var userGuid = new Guid(userId);
            var siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            var siteUser = await _queries.Fetch(siteGuid, userGuid, cancellationToken);

            // jk second chance - this may be a visitor from another tenant
            if (siteUser == null && _multiTenantOptions.RootUserCanSignInToTenants)
            {
                siteUser = await _queries.Fetch(_multiTenantOptions.RootSiteId, userGuid, cancellationToken);
            }

            return (TUser)siteUser;
        }


        public async Task<TUser> FindByNameAsync(string normailzedUserName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("FindByNameAsync"); 
            
            var siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }
            
            var allowEmailFallback = SiteSettings.UseEmailForLogin;
            var siteUser = await _queries.FetchByLoginName(siteGuid, normailzedUserName, allowEmailFallback, cancellationToken);

            // jk second chance - this may be a visitor from another tenant
            if(siteUser==null && _multiTenantOptions.RootUserCanSignInToTenants)
            {
                siteUser = await _queries.FetchByLoginName(_multiTenantOptions.RootSiteId, normailzedUserName, allowEmailFallback, cancellationToken);
            }

            return (TUser)siteUser;

        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("SetEmailAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.SiteId == Guid.Empty)
            {
                user.SiteId = SiteSettings.Id;
            }
            
            user.UserName = userName;

            //bool result = await repo.Save(user, cancellationToken);

            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedUserName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("SetNormalizedUserNameAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.SiteId == Guid.Empty)
            {
                user.SiteId = SiteSettings.Id;
            }
            
            user.NormalizedUserName = normalizedUserName;
            //cancellationToken.ThrowIfCancellationRequested();
            //bool result = await repo.Save(user);

            return Task.FromResult(0);

        }

        
//#pragma warning disable 1998

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetUserIdAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.Id.ToString());

            
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetUserNameAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.UserName);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetUserNameAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.NormalizedUserName);

            //return user.UserName.ToLowerInvariant();
        }

        //#pragma warning restore 1998

        #endregion

        #region ISecurityStampStore

        /// <summary>
        /// Sets the provided security <paramref name="stamp"/> for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose security stamp should be set.</param>
        /// <param name="stamp">The security stamp to set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
            //bool result = await repo.Save(user, cancellationToken);

        }

        /// <summary>
        /// Get the security stamp for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose security stamp should be set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the security stamp for the specified <paramref name="user"/>.</returns>
        public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.SecurityStamp);
        }

        #endregion

        #region IUserEmailStore


        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetEmailAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Email);
        }

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetEmailAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.NormalizedEmail);
        }



        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetEmailConfirmedAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.EmailConfirmed);
        }



        public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("SetEmailAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.SiteId == Guid.Empty)
            {
                user.SiteId = SiteSettings.Id;
            }
            
            cancellationToken.ThrowIfCancellationRequested();

            user.Email = email;
            //user.NormalizedEmail = email.ToLower();
            // TODO: are we supposed to save here?
            //bool result = await repo.Save(user, cancellationToken);
            return Task.FromResult(0);
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("SetEmailAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.SiteId == Guid.Empty)
            {
                user.SiteId = SiteSettings.Id;
            }
            
            //user.Email = email;
            user.NormalizedEmail = normalizedEmail;

            //cancellationToken.ThrowIfCancellationRequested();

            //bool result = await repo.Save(user);
            return Task.FromResult(0);

        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("SetEmailConfirmedAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.SiteId == Guid.Empty)
            {
                user.SiteId = SiteSettings.Id;
            }
            
            user.EmailConfirmed = confirmed;

           
            // I don't not sure this method is expected to
            // persist this change to the db
            //bool result = await repo.Save(user);
            return Task.FromResult(0);

        }

        public async Task<TUser> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("FindByEmailAsync"); 
            
            Guid siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            ISiteUser siteUser = await _queries.Fetch(siteGuid, email, cancellationToken);

            // jk second chance - this may be a visitor from another tenant
            if (siteUser == null && _multiTenantOptions.RootUserCanSignInToTenants)
            {
                siteUser = await _queries.Fetch(_multiTenantOptions.RootSiteId, email, cancellationToken);
            }

            return (TUser)siteUser;
        }

        #endregion

        #region IUserPasswordStore

        
//#pragma warning disable 1998

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetPasswordHashAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                string result = null;
                return Task.FromResult(result);
            }

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("HasPasswordAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            _log.LogDebug("SetPasswordHashAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PasswordHash = passwordHash;
            user.LastPasswordChangeUtc = DateTime.UtcNow;
            user.MustChangePwd = false;

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

        
        

        /// <summary>
        /// Retrieves a flag indicating whether user lockout can enabled for the specified user.
        /// </summary>
        /// <param name="user">The user whose ability to be locked out should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, true if a user can be locked out, otherwise false.
        /// </returns>
        public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetLockoutEnabledAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            //return Task.FromResult(user.IsLockedOut);
            return Task.FromResult(true); // any user can be locked out, maybe should not be posible to lockout some users like admin user
        }

        /// <summary>
        /// Gets the last <see cref="DateTimeOffset"/> a user's last lockout expired, if any.
        /// Any time in the past should be indicates a user is not locked out.
        /// </summary>
        /// <param name="user">The user whose lockout date should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the result of the asynchronous query, a <see cref="DateTimeOffset"/> containing the last time
        /// a user's lockout expired, if any.
        /// </returns>
        public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetLockoutEndDateAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            DateTimeOffset? d;
            if (user.LockoutEndDateUtc != null)
            {
                if(user.LockoutEndDateUtc.Value == DateTime.MinValue) { return null; }

                d = new DateTimeOffset(user.LockoutEndDateUtc.Value);
                return Task.FromResult(d);
            }

            d = new DateTimeOffset(DateTime.UtcNow.AddDays(-2));
            return Task.FromResult(d);
        }


        /// <summary>
        /// Retrieves the current failed access count for the specified <paramref name="user"/>..
        /// </summary>
        /// <param name="user">The user whose failed access count should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the failed access count.</returns>
        public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetAccessFailedCountAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.AccessFailedCount);
        }


        /// <summary>
        /// Records that a failed access has occurred, incrementing the failed access count.
        /// </summary>
        /// <param name="user">The user whose cancellation count should be incremented.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the incremented failed access count.</returns>
        public async Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("IncrementAccessFailedCountAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCount += 1;
            cancellationToken.ThrowIfCancellationRequested();
            await _commands.UpdateFailedPasswordAttemptCount(user.SiteId, user.Id, user.AccessFailedCount, cancellationToken);
            return user.AccessFailedCount;
        }

        /// <summary>
        /// Resets a user's failed access count.
        /// </summary>
        /// <param name="user">The user whose failed access count should be reset.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <remarks>This is typically called after the account is successfully accessed.</remarks>
        public async Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("ResetAccessFailedCountAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCount = 0;
            // EF implementation doesn't save here
            // but we have to since our save doesn't update this
            // we have specific methods as shown here
            await _commands.UpdateFailedPasswordAttemptCount(user.SiteId, user.Id, user.AccessFailedCount, cancellationToken);

        }

        /// <summary>
        /// Set the flag indicating if the specified <paramref name="user"/> can be locked out..
        /// </summary>
        /// <param name="user">The user whose ability to be locked out should be set.</param>
        /// <param name="enabled">A flag indicating if lock out can be enabled for the specified <paramref name="user"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("SetLockoutEnabledAsync"); 
            
            // I was confused about this property and tried to map it to IsLockedOut
            // but really IsLockedOut is determined only by the lockoutenddate
            // if it is null or less than now the user is not locked out
            // LockoutEnabled must be for a different purpose
            // like maybe it should not be possible for some users like admins to get locked out
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            //bool result;
            // EF implementation doesn't save here but I think our save doesn't set lockout so we have to

            // this method is not supposed to do the actual lockout
            //if (enabled)
            //{
            //    result = await repo.LockoutAccount(user.UserGuid, cancellationToken);
            //}
            //else
            //{
            //    result = await repo.UnLockAccount(user.UserGuid, cancellationToken);
            //}

            return Task.FromResult(0);

        }

        /// <summary>
        /// Locks out a user until the specified end date has passed. Setting a end date in the past immediately unlocks a user.
        /// </summary>
        /// <param name="user">The user whose lockout date should be set.</param>
        /// <param name="lockoutEnd">The <see cref="DateTimeOffset"/> after which the <paramref name="user"/>'s lockout should end.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("SetLockoutEndDateAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            //bool result;
            if (lockoutEnd.HasValue)
            {
                user.LockoutEndDateUtc = lockoutEnd.Value.DateTime;
                //if(user.LockoutEndDateUtc > DateTime.UtcNow)
                //{
                //    //result = await repo.LockoutAccount(user.UserGuid, cancellationToken);
                //    //user.IsLockedOut = true;
                    
                //}
                //else
                //{
                //    //result = await repo.UnLockAccount(user.UserGuid, cancellationToken);
                //    //user.IsLockedOut = false;
                //}

                //result = await repo.Save(user, cancellationToken);
            }
            else
            {
                user.LockoutEndDateUtc = null;
                //user.IsLockedOut = false;
                //result = await repo.UnLockAccount(user.UserGuid, cancellationToken);
                //result = await repo.Save(user, cancellationToken);
            }

            // EF implementation doesn't save here
            //bool result = await repo.Save(user);
            return Task.FromResult(0);
        }



        #endregion

        #region IUserClaimStore

        public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("AddClaimsAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Guid siteGuid = SiteSettings.Id;
            if(_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            foreach (Claim claim in claims)
            {
                UserClaim userClaim = new UserClaim
                {
                    SiteId = siteGuid,
                    UserId = user.Id,
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                };
                cancellationToken.ThrowIfCancellationRequested();
                await _commands.CreateClaim(userClaim, cancellationToken);
            }

            // this is so the middleware will sign the user out and in again to get current claims
            user.RolesChanged = true;
            await _commands.Update(user);
            

        }

        public async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("RemoveClaimAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Guid siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            foreach (Claim claim in claims)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _commands.DeleteClaimByUser(siteGuid, user.Id, claim.Type, claim.Value, cancellationToken);
            }

            // this is so the middleware will sign the user out and in again to get current claims
            user.RolesChanged = true;
            await _commands.Update(user);

        }

        public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("ReplaceClaimAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Guid siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            await _commands.DeleteClaimByUser(siteGuid, user.Id, claim.Type, claim.Value, cancellationToken);

            UserClaim userClaim = new UserClaim
            {
                SiteId = siteGuid,
                UserId = user.Id,
                ClaimType = newClaim.Type,
                ClaimValue = newClaim.Value
            };
            cancellationToken.ThrowIfCancellationRequested();
            await _commands.CreateClaim(userClaim, cancellationToken);
            // this is so the middleware will sign the user out and in again to get current claims
            user.RolesChanged = true;
            await _commands.Update(user);

        }

        public async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetClaimsAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var claims = new List<Claim>();
            
            var siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            var userClaims = await _queries.GetClaimsByUser(siteGuid, user.Id, cancellationToken);
            foreach (UserClaim uc in userClaims)
            {
                Claim c = new Claim(uc.ClaimType, uc.ClaimValue);
                claims.Add(c);
            }

            return claims;

        }

        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetRolesAsync"); 
            
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            var siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            var users = await _queries.GetUsersForClaim(siteGuid, claim.Type, claim.Value, cancellationToken);

            return (IList<TUser>)users;
        }

        #endregion

        #region IUserLoginStore

        public async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("AddLoginAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Guid siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            UserLogin userlogin = new UserLogin
            {
                SiteId = siteGuid,
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                ProviderDisplayName = login.ProviderDisplayName
            };

            cancellationToken.ThrowIfCancellationRequested();
            await _commands.CreateLogin(userlogin, cancellationToken);

        }

        public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("FindAsync called for " + loginProvider + " with providerKey " + providerKey); 
            
            Guid siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            var userlogin = await _queries.FindLogin(siteGuid, loginProvider, providerKey, cancellationToken);

            // jk second chance - this may be a visitor from another tenant
            if (userlogin == null && _multiTenantOptions.RootUserCanSignInToTenants)
            {
                userlogin = await _queries.FindLogin(_multiTenantOptions.RootSiteId, loginProvider, providerKey, cancellationToken);
            }

            if (userlogin != null && userlogin.UserId != Guid.Empty)
            {
                _log.LogDebug("FindAsync userLogin found for " + loginProvider + " with providerKey " + providerKey);
                
                cancellationToken.ThrowIfCancellationRequested();
                var siteUser = await _queries.Fetch(siteGuid, userlogin.UserId, cancellationToken);
                if (siteUser != null)
                {
                    return (TUser)siteUser;
                }
                else
                {
                    _log.LogDebug("FindAsync siteUser not found for " + loginProvider + " with providerKey " + providerKey);
                   
                }
            }
            else
            {
                _log.LogDebug("FindAsync userLogin not found for " + loginProvider + " with providerKey " + providerKey);
            }

            return default(TUser);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetLoginsAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var logins = new List<UserLoginInfo>();
            
            var siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            var userLogins = await _queries.GetLoginsByUser(siteGuid, user.Id, cancellationToken);
            foreach (UserLogin ul in userLogins)
            {
                var l = new UserLoginInfo(ul.LoginProvider, ul.ProviderKey, ul.ProviderDisplayName);
                logins.Add(l);
            }


            return logins;
        }

        public async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("RemoveLoginAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            var siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            await _commands.DeleteLogin(
                siteGuid,
                user.Id,
                loginProvider,
                providerKey,
                cancellationToken
                );


        }

        #endregion

        #region IUserTwoFactorStore


        /// <summary>
        /// Returns a flag indicating whether the specified <paramref name="user "/>has two factor authentication enabled or not,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose two factor authentication enabled status should be set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a flag indicating whether the specified 
        /// <paramref name="user "/>has two factor authentication enabled or not.
        /// </returns>
        public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetTwoFactorEnabledAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.TwoFactorEnabled);
        }

        /// <summary>
        /// Sets a flag indicating whether the specified <paramref name="user "/>has two factor authentication enabled or not,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose two factor authentication enabled status should be set.</param>
        /// <param name="enabled">A flag indicating whether the specified <paramref name="user"/> has two factor authentication enabled.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("SetTwoFactorEnabledAsync"); 
            
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

        #region IUserAuthenticatorKeyStore, IUserAuthenticationTokenStore, IUserTwoFactorRecoveryCodeStore

        public async Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var entry = await _queries.FindToken(SiteSettings.Id, user.Id, loginProvider, name, cancellationToken);
            return entry?.Value;
        }

        private const string InternalLoginProvider = "[AspNetUserStore]";
        private const string AuthenticatorKeyTokenName = "AuthenticatorKey";
        private const string RecoveryCodeTokenName = "RecoveryCodes";

        public Task SetAuthenticatorKeyAsync(TUser user, string key, CancellationToken cancellationToken)
            => SetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);

        
        public Task<string> GetAuthenticatorKeyAsync(TUser user, CancellationToken cancellationToken)
            => GetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, cancellationToken);
        

        public async Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var token = await _queries.FindToken(SiteSettings.Id, user.Id, loginProvider, name, cancellationToken);
            if (token == null)
            {
                var newToken = new UserToken
                {
                    SiteId = SiteSettings.Id,
                    UserId = user.Id,
                    LoginProvider = loginProvider,
                    Name = name,
                    Value = value
                };
                await _commands.CreateToken(newToken);
            }
            else
            {
                token.Value = value;
                await _commands.UpdateToken(token);
            }
        }

        
        public async Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var entry = await _queries.FindToken(SiteSettings.Id, user.Id, loginProvider, name, cancellationToken);
            if (entry != null)
            {
                await _commands.DeleteToken(entry.SiteId, entry.UserId, entry.LoginProvider, entry.Name);
            }
        }

        

        public Task ReplaceCodesAsync(TUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            var mergedCodes = string.Join(";", recoveryCodes);
            return SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
        }

        public async Task<bool> RedeemCodeAsync(TUser user, string code, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken) ?? "";
            var splitCodes = mergedCodes.Split(';');
            if (splitCodes.Contains(code))
            {
                var updatedCodes = new List<string>(splitCodes.Where(s => s != code));
                await ReplaceCodesAsync(user, updatedCodes, cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<int> CountCodesAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken) ?? "";
            if (mergedCodes.Length > 0)
            {
                return mergedCodes.Split(';').Length;
            }
            return 0;
        }

        #endregion

        #region IUserPhoneNumberStore


        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetPhoneNumberAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetPhoneNumberConfirmedAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PhoneNumberConfirmed);
        }



        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("SetPhoneNumberAsync"); 
            
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
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("SetPhoneNumberConfirmedAsync"); 
            
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
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("AddToRoleAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            var siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            var siteRole = await _queries.FetchRole(siteGuid, role, cancellationToken);
            if (siteRole != null)
            {
                await _commands.AddUserToRole(siteRole.SiteId, siteRole.Id, user.Id, cancellationToken);
            }

        }

        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetRolesAsync"); 
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var roles = new List<string>();
            
            var siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            roles = await _queries.GetUserRoles(siteGuid, user.Id, cancellationToken);

            return roles;
        }

        public async Task<bool> IsInRoleAsync(TUser user, string role, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("IsInRoleAsync"); 

           
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = false;
            var roles = await _queries.GetUserRoles(SiteSettings.Id, user.Id, cancellationToken);

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
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("GetRolesAsync"); 
           
            var siteGuid = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            var users = await _queries.GetUsersInRole(siteGuid, role, cancellationToken);

            return (IList<TUser>)users; 
        }

        public async Task RemoveFromRoleAsync(TUser user, string role, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            _log.LogDebug("RemoveFromRoleAsync"); 
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            var siteId = SiteSettings.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            var siteRole = await _queries.FetchRole(siteId, role, cancellationToken);
            
            if (siteRole != null)
            {  
                await _commands.RemoveUserFromRole(siteId, siteRole.Id, user.Id, cancellationToken);
            }

        }

        #endregion

        #region Helpers

        public async Task<string> SuggestLoginNameFromEmail(Guid siteGuid, string email)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteGuid = _multiTenantOptions.RelatedSiteId; }

            var login = email.Substring(0, email.IndexOf("@"));
            var offset = 1;
            // don't think we should make this async inside a loop
            while (await _queries.LoginExistsInDB(siteGuid, login).ConfigureAwait(false))
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
