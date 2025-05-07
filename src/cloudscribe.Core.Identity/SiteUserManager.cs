﻿// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:				    2014-07-22
// Last Modified:		    2018-08-19
// 


using cloudscribe.Core.Models;
using cloudscribe.Core.Models.EventHandlers;
using cloudscribe.Core.Models.Identity;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    /// <summary>
    /// extends the standard UserManager with our own methods
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class SiteUserManager<TUser> : UserManager<TUser> where TUser : SiteUser
    {        
        public SiteUserManager(
            SiteContext                             currentSite,
            UserEvents                              userEventHandlers,
            IEnumerable<IHandleUserEmailConfirmed>  emailConfirmedHandlers,
            IEnumerable<IHandleUserEmailUpdated>    emailUpdatedHandlers,
            IUserCommands                           userCommands,
            IUserQueries                            userQueries,
            IUserStore<TUser>                       store,
            IOptions<IdentityOptions>               optionsAccessor,
            IOptions<MultiTenantOptions>            multiTenantOptionsAccessor,
            INewUserDisplayNameResolver             displayNameResolver,
            IPasswordHasher<TUser>                  passwordHasher,
            IEnumerable<IUserValidator<TUser>>      userValidators,
            IEnumerable<IPasswordValidator<TUser>>  passwordValidators,
            ILookupNormalizer                       lookupNormalizer,
            IdentityErrorDescriber                  errors,
            IServiceProvider                        serviceProvider,
            ILogger<UserManager<TUser>>             logger,
            IHttpContextAccessor                    contextAccessor,
            IEnumerable<IHandleUserRemovedFromRole> userRemovedFromRoleHandlers
            )
            : base(
                  store,
                  optionsAccessor,
                  passwordHasher,
                  userValidators,
                  passwordValidators,
                  lookupNormalizer,
                  errors,
                  serviceProvider,
                  logger)
        {
            _identityOptions             = optionsAccessor.Value;
            _userStore                   = store;
            _commands                    = userCommands ?? throw new ArgumentNullException(nameof(userCommands));
            _queries                     = userQueries  ?? throw new ArgumentNullException(nameof(userQueries));

            _siteSettings                = currentSite;
            _multiTenantOptions          = multiTenantOptionsAccessor.Value;
            _contextAccessor             = contextAccessor;
            _httpContext                 = contextAccessor?.HttpContext;
            _eventHandlers               = userEventHandlers;
            _passwordHasher              = passwordHasher;
            _emailConfirmedHandlers      = emailConfirmedHandlers;
            _emailUpdatedHandlers        = emailUpdatedHandlers;
            _displayNameResolver         = displayNameResolver;
            _userRemovedFromRoleHandlers = userRemovedFromRoleHandlers;
            _log                         = logger;
        }
        
        private IdentityOptions                        _identityOptions;
        private IUserStore<TUser>                      _userStore;
        private IUserCommands                          _commands;
        private IUserQueries                           _queries;
        private MultiTenantOptions                     _multiTenantOptions;
        private IHttpContextAccessor                   _contextAccessor;
        private HttpContext                            _httpContext;
        private UserEvents                             _eventHandlers;
        private IPasswordHasher<TUser>                 _passwordHasher;
        private IEnumerable<IHandleUserEmailConfirmed> _emailConfirmedHandlers;
        private IEnumerable<IHandleUserEmailUpdated>   _emailUpdatedHandlers;
        private INewUserDisplayNameResolver            _displayNameResolver;
        private readonly                               IEnumerable<IHandleUserRemovedFromRole> _userRemovedFromRoleHandlers;
        private ILogger                                _log;

        //private CancellationToken CancellationToken => httpContext?.RequestAborted ?? CancellationToken.None;

        private ISiteContext _siteSettings = null;

        internal IUserLockoutStore<TUser> GetUserLockoutStore()
        {
            var cast = Store as IUserLockoutStore<TUser>;
            if (cast == null)
            {
                throw new NotSupportedException("IUserLockoutStore was null");
            }
            return cast;
        }
        
        public ISiteContext Site { get { return _siteSettings; } }
        
        public Task<bool> LoginIsAvailable(Guid userId, string loginName)
        {
            Guid siteId = Site.Id;
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return _queries.LoginIsAvailable(siteId, userId, loginName, CancellationToken);
 
        }

        
        public Task<PagedResult<IUserInfo>> GetPage(Guid siteId, int pageNumber, int pageSize, string userNameBeginsWith, int sortMode)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return _queries.GetPage(siteId, pageNumber, pageSize, userNameBeginsWith, sortMode, CancellationToken);
        }

        public async Task<List<ISiteUser>> GetAllSiteUsers(Guid siteId, bool approvedOnly=true)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            if (approvedOnly)
                return await _queries.GetAllApprovedUsersForSite(siteId, CancellationToken);
            else
                return await _queries.GetAllUsersForSite(siteId, CancellationToken);
        }

        public Task<int> CountUsers(Guid siteId, string userNameBeginsWith)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return _queries.CountUsers(siteId, userNameBeginsWith, CancellationToken);
        }

        public Task<int> CountLockedOutUsers(Guid siteId)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return _queries.CountLockedByAdmin(siteId, CancellationToken);
        }

        public Task<PagedResult<IUserInfo>> GetPageLockedUsers(
            Guid siteId,
            int pageNumber,
            int pageSize)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return _queries.GetPageLockedByAdmin(siteId, pageNumber, pageSize, CancellationToken);
        }

        public Task<PagedResult<IUserInfo>> GetUserAdminSearchPage(Guid siteId, int pageNumber, int pageSize, string searchInput, int sortMode)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return _queries.GetUserAdminSearchPage(siteId, pageNumber, pageSize, searchInput, sortMode, CancellationToken);

        }

        public Task<int> CountUsersForAdminSearch(Guid siteId, string searchInput)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return _queries.CountUsersForAdminSearch(siteId, searchInput, CancellationToken);
        }

        public Task<int> CountNotApprovedUsers(Guid siteId)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return _queries.CountNotApprovedUsers(siteId, CancellationToken);
        }

        public Task<PagedResult<IUserInfo>> GetNotApprovedUsers(
            Guid siteId,
            int pageNumber,
            int pageSize)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return _queries.GetNotApprovedUsers(siteId, pageNumber, pageSize, CancellationToken);

        }

        public Task<List<IUserInfo>> GetByIPAddress(Guid siteId, string ipv4Address)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = Guid.Empty; }

            return _queries.GetByIPAddress(siteId, ipv4Address, CancellationToken);
        }
        
        public Task<ISiteUser> Fetch(Guid siteId, Guid userId)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return _queries.Fetch(siteId, userId, CancellationToken);
        }

        
        public Task<bool> EmailExistsInDB(Guid siteId, string email)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return _queries.EmailExistsInDB(siteId, email, CancellationToken);
        }

        public Task<bool> EmailExistsInDB(Guid siteId, Guid userGuid, string email)
        {
            if (_multiTenantOptions.UseRelatedSitesMode) { siteId = _multiTenantOptions.RelatedSiteId; }

            return _queries.EmailExistsInDB(siteId, userGuid, email, CancellationToken);

        }

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

        public async Task<IdentityResult> TryCreateAccountForExternalUser(Guid siteId, ExternalLoginInfo info)
        {
            if (info == null || info.Principal == null) return null;

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email)) return null; // not enough info

            var userName = await SuggestLoginNameFromEmail(Site.Id, email);

            var user = new SiteUser
            {
                SiteId = Site.Id,
                UserName = userName,
                Email = email,
                FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                AccountApproved = Site.RequireApprovalBeforeLogin ? false : true
            };
            //https://github.com/joeaudette/cloudscribe/issues/346
            user.DisplayName = _displayNameResolver.ResolveDisplayName(user);

            var result = await CreateAsync(user as TUser);
            if(result.Succeeded)
            {
                result = await AddLoginAsync(user as TUser, info);
            }


            return result;
        }

        private IUserPasswordStore<TUser> GetPasswordStore()
        {
            var cast = Store as IUserPasswordStore<TUser>;
            if (cast == null)
            {
                throw new NotSupportedException("StoreNotIUserPasswordStore");
            }
            return cast;
        }

        public async Task<IdentityResult> ChangeUserPassword(TUser user, string newPassword, bool validatePassword)
        {
            if (validatePassword)
            {
                var validate = await ValidatePassword(user, newPassword);
                if (!validate.Succeeded)
                {
                    return validate;
                }
            }
            // user.MustChangePwd will be set false by passwordStore.SetPasswordHashAsync so preserve it because this method is called by admins changing
            // a user password and would still want this as true after admin changes the user password
            var mustChangePwd = user.MustChangePwd; 

            var hash = newPassword != null ? this._passwordHasher.HashPassword(user, newPassword) : null;

            IdentityResult passwordDifferentFromPrevious = CheckPasswordIsDifferentFromPrevious(user, hash);
            if (!passwordDifferentFromPrevious.Succeeded)
            {
                return passwordDifferentFromPrevious;
            }

            var passwordStore = GetPasswordStore();
            await passwordStore.SetPasswordHashAsync(user, hash, CancellationToken);

            user.MustChangePwd = mustChangePwd;
            await UpdateAsync(user);
            
            return IdentityResult.Success;
        }

        protected async Task<IdentityResult> ValidatePassword(TUser user, string password)
        {
            var errors = new List<IdentityError>();
            foreach (var v in PasswordValidators)
            {
                var result = await v.ValidateAsync(this, user, password);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {
                Logger.LogWarning(14, "User {userId} password validation failed: {errors}.", await GetUserIdAsync(user), string.Join(";", errors.Select(e => e.Code)));
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }

        protected IdentityResult CheckPasswordIsDifferentFromPrevious(TUser user, string hash)
        {
            IdentityError errors;

            if (user.PasswordHash == hash)
            {
                errors = new IdentityError{
                    Code = "19",
                    Description = "New password cannot be the same as current password"
                };
                return IdentityResult.Failed(errors);
            }

            return IdentityResult.Success;
        }

        public async Task<PagedResult<IUserLocation>> GetUserLocations(Guid siteId, Guid userId, int pageNumber, int pageSize)
        {
            return await _queries.GetUserLocationsByUser(siteId, userId, pageNumber, pageSize);
            //var result = new PagedResult<IUserLocation>();
            //var list = await queries.GetUserLocationsByUser(siteId, userId, pageNumber, pageSize);
            //result.Data = list.ToList();
            //result.TotalItems = await queries.CountUserLocationsByUser(siteId, userId);
            //result.PageNumber = pageNumber;
            //result.PageSize = pageSize;

            //return result;
        }


        public async Task<IdentityResult> RemoveUserFromRole(Guid siteId, Guid userId, string roleName)
        {
            var errors = new List<IdentityError>();
            var role = await _queries.FetchRole(siteId, roleName);
            if(role == null)
            {
                var er = new IdentityError
                {
                    Description = "role not found"
                };
                errors.Add(er);
                return IdentityResult.Failed(errors.ToArray());
            }
            var user = await _queries.Fetch(siteId, userId);
            if (user == null)
            {
                var er = new IdentityError
                {
                    Description = "user not found"
                };
                errors.Add(er);
                return IdentityResult.Failed(errors.ToArray());
            }

            await _commands.RemoveUserFromRole(role.SiteId, role.Id, user.Id);

            user.RolesChanged = true;
            await _commands.Update(user);

            foreach (var handler in _userRemovedFromRoleHandlers)
            {
                try
                {
                    await handler.Handle(user, role);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message}-{ex.StackTrace}");
                }
            }


            return IdentityResult.Success;
        }

        #region Overrides

        public override async Task<IdentityResult> ConfirmEmailAsync(TUser user, string token)
        {
            var result = await base.ConfirmEmailAsync(user, token);

            if(result.Succeeded)
            {
                foreach(var handler in _emailConfirmedHandlers)
                {
                    try
                    {
                        await handler.HandleUserEmailConfirmed(user);
                    }
                    catch(Exception ex)
                    {
                        Logger.LogError(ex.Message + " stack trace: " + ex.StackTrace);
                    }
                }
            }

            return result;
        }

        public override async Task<IdentityResult> ChangeEmailAsync(TUser user, string newEmail, string token) 
        {
            var result = await base.ChangeEmailAsync(user, newEmail, token);

            if (result.Succeeded)
            {
                foreach (var handler in _emailUpdatedHandlers)
                {
                    try
                    {
                        await handler.HandleUserEmailUpdated(user);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex.Message + " stack trace: " + ex.StackTrace);
                    }
                }
            }

            return result;
        }


        public override async Task<IdentityResult> CreateAsync(TUser user)
        {
            var result = await base.CreateAsync(user);
            if(result.Succeeded)
            {
                await _eventHandlers.HandleUserCreated(user).ConfigureAwait(false);
            }

            return result;
        }

        public override async Task<IdentityResult> DeleteAsync(TUser user)
        {
            await _eventHandlers.HandleUserPreDelete(user.SiteId, user.Id).ConfigureAwait(false);

            return await base.DeleteAsync(user);
        }

        public override async Task<IdentityResult> UpdateAsync(TUser user)
        {
            await _eventHandlers.HandleUserPreUpdate(user.SiteId, user.Id).ConfigureAwait(false);
            if(string.IsNullOrWhiteSpace(user.SecurityStamp))
            {
               await UpdateSecurityStampAsync(user);
            }

            var result = await base.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _eventHandlers.HandleUserUpdated(user).ConfigureAwait(false);
            }

            return result;
        }

        //public override async Task<string> GenerateEmailConfirmationTokenAsync(TUser user)
        //{
        //    Guid registerConfirmGuid = Guid.NewGuid();
        //    bool result = await userRepo.SetRegistrationConfirmationGuid(user.UserGuid, registerConfirmGuid, CancellationToken.None);

        //    return registerConfirmGuid.ToString();
        //}

        //public override async Task<IdentityResult> ConfirmEmailAsync(TUser user, string token)
        //{
        //    if(token.Length != 36)
        //    {
        //        // TODO: log info or warning
        //        return IdentityResult.Failed();
        //    }
        //    Guid confirmGuid = new Guid(token);
        //    ISiteUser siteUser = await userRepo.FetchByConfirmationGuid(Site.SiteId, confirmGuid, CancellationToken);
        //    if((siteUser != null)&&(siteUser.UserGuid == user.UserGuid))
        //    {
        //        bool result = await userRepo.ConfirmRegistration(confirmGuid, CancellationToken.None);
        //        if(result) { return IdentityResult.Success; }
        //    }

        //    return IdentityResult.Failed();
        //}

        /// <summary>
        /// Increments the access failed count for the user as an asynchronous operation. 
        /// If the failed access account is greater than or equal to the configured maximum number of attempts, 
        /// the user will be locked out for the configured lockout time span.
        /// </summary>
        /// <param name="user">The user whose failed access count to increment.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        public override async Task<IdentityResult> AccessFailedAsync(TUser user)
        {
            //ThrowIfDisposed();
            var store = GetUserLockoutStore();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            // If this puts the user over the threshold for lockout, lock them out and reset the access failed count
            var count = await store.IncrementAccessFailedCountAsync(user, CancellationToken);

            //if (count < defaultIdentityOptions.Lockout.MaxFailedAccessAttempts)
            if (count < Site.MaxInvalidPasswordAttempts)
            {
                //return await UpdateUserAsync(user);
                await _commands.Update(user, CancellationToken.None);
                return IdentityResult.Success;
            }

            Logger.LogWarning(12, "User {userId} is locked out.", await GetUserIdAsync(user));

            // TODO: should DefaultLockoutTimeSpan be promoted to a site setting?
            await store.SetLockoutEndDateAsync(
                user, 
                DateTimeOffset.Now.Add(_identityOptions.Lockout.DefaultLockoutTimeSpan), // gets converted to UTC later
                CancellationToken);

            await store.ResetAccessFailedCountAsync(user, CancellationToken);
            //return await UpdateUserAsync(user);
            await _commands.Update(user, CancellationToken.None);

            return IdentityResult.Success;
        }

        /// <summary>
        /// Gets a list of valid two factor token providers for the specified <paramref name="user"/>,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user the whose two factor authentication providers will be returned.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents result of the asynchronous operation, a list of two
        /// factor authentication providers for the specified user.
        /// </returns>
        //public override async Task<IList<string>> GetValidTwoFactorProvidersAsync(TUser user)
        //{
        //    ThrowIfDisposed();
        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }
        //    var results = new List<string>();
        //    foreach (var f in _tokenProviders)
        //    {
        //        if (await f.Value.CanGenerateTwoFactorTokenAsync(this, user))
        //        {
        //            results.Add(f.Key);
        //        }
        //    }
        //    return results;
        //}

        public override async Task<bool> VerifyUserTokenAsync(TUser user, string tokenProvider, string purpose, string token)
        {
            var result = await base.VerifyUserTokenAsync(user, tokenProvider, purpose, token);
            if(!result && purpose == "ResetPassword")
            {
                // when migrating users from other systems we don't have a way to populate the security stamp
                // so passowrd reset fails. We need migrated users to be able to reset their password which will in turn create 
                // a hashed password and populate the security stamp
                // cleartext||0 indicates a clear text password
                // when migrating we use a random guid like guid||0 the user don't know the guid so can't login and needs recovery
                if (string.IsNullOrEmpty(user.SecurityStamp) && user.PasswordHash.EndsWith("||0")) 
                {
                    return true;
                }
            }

            return result;
        }



        #endregion


    }




}
