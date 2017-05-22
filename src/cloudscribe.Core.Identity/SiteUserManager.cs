// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-22
// Last Modified:		    2017-05-22
// 
//

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;

namespace cloudscribe.Core.Identity
{
    /// <summary>
    /// extends the standard UserManager with our own methods
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class SiteUserManager<TUser> : UserManager<TUser> where TUser : SiteUser
    {        
        public SiteUserManager(
            SiteContext currentSite,
            UserEvents userEventHandlers,
            IUserCommands userCommands,
            IUserQueries userQueries,
            IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer lookupNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider serviceProvider,
            ILogger<UserManager<TUser>> logger,
            IHttpContextAccessor contextAccessor)
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
            identityOptions = optionsAccessor.Value;
            userStore = store;

            if (userCommands == null) { throw new ArgumentNullException(nameof(userCommands)); }
            commands = userCommands;

            if (userQueries == null) { throw new ArgumentNullException(nameof(userQueries)); }
            queries = userQueries;

            siteSettings = currentSite;
            multiTenantOptions = multiTenantOptionsAccessor.Value;
            this.contextAccessor = contextAccessor;
            httpContext = contextAccessor?.HttpContext;
            eventHandlers = userEventHandlers;
        }
        
        private IdentityOptions identityOptions;
        private IUserStore<TUser> userStore;
        private IUserCommands commands;
        private IUserQueries queries;
        private MultiTenantOptions multiTenantOptions;
        private IHttpContextAccessor contextAccessor;
        private HttpContext httpContext;
        private UserEvents eventHandlers;

        //private CancellationToken CancellationToken => httpContext?.RequestAborted ?? CancellationToken.None;

        private ISiteContext siteSettings = null;

        internal IUserLockoutStore<TUser> GetUserLockoutStore()
        {
            var cast = Store as IUserLockoutStore<TUser>;
            if (cast == null)
            {
                throw new NotSupportedException("IUserLockoutStore was null");
            }
            return cast;
        }
        
        public ISiteContext Site { get { return siteSettings; } }
        
        public Task<bool> LoginIsAvailable(Guid userId, string loginName)
        {
            Guid siteId = Site.Id;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return queries.LoginIsAvailable(siteId, userId, loginName, CancellationToken);
 
        }

        
        public Task<List<IUserInfo>> GetPage(Guid siteId, int pageNumber, int pageSize, string userNameBeginsWith, int sortMode)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return queries.GetPage(siteId, pageNumber, pageSize, userNameBeginsWith, sortMode, CancellationToken);
        }
        
        public Task<int> CountUsers(Guid siteId, string userNameBeginsWith)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return queries.CountUsers(siteId, userNameBeginsWith, CancellationToken);
        }

        public Task<int> CountLockedOutUsers(Guid siteId)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return queries.CountLockedByAdmin(siteId, CancellationToken);
        }

        public Task<List<IUserInfo>> GetPageLockedUsers(
            Guid siteId,
            int pageNumber,
            int pageSize)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return queries.GetPageLockedByAdmin(siteId, pageNumber, pageSize, CancellationToken);
        }

        public Task<List<IUserInfo>> GetUserAdminSearchPage(Guid siteId, int pageNumber, int pageSize, string searchInput, int sortMode)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return queries.GetUserAdminSearchPage(siteId, pageNumber, pageSize, searchInput, sortMode, CancellationToken);

        }

        public Task<int> CountUsersForAdminSearch(Guid siteId, string searchInput)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return queries.CountUsersForAdminSearch(siteId, searchInput, CancellationToken);
        }

        public Task<int> CountNotApprovedUsers(Guid siteId)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return queries.CountNotApprovedUsers(siteId, CancellationToken);
        }

        public Task<List<IUserInfo>> GetNotApprovedUsers(
            Guid siteId,
            int pageNumber,
            int pageSize)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return queries.GetNotApprovedUsers(siteId, pageNumber, pageSize, CancellationToken);

        }

        public Task<List<IUserInfo>> GetByIPAddress(Guid siteId, string ipv4Address)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = Guid.Empty; }

            return queries.GetByIPAddress(siteId, ipv4Address, CancellationToken);
        }
        
        public Task<ISiteUser> Fetch(Guid siteId, Guid userId)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return queries.Fetch(siteId, userId, CancellationToken);
        }

        
        public Task<bool> EmailExistsInDB(Guid siteId, string email)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return queries.EmailExistsInDB(siteId, email, CancellationToken);
        }

        public Task<bool> EmailExistsInDB(Guid siteId, Guid userGuid, string email)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return queries.EmailExistsInDB(siteId, userGuid, email, CancellationToken);

        }

        public async Task<string> SuggestLoginNameFromEmail(Guid siteGuid, string email)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteGuid = multiTenantOptions.RelatedSiteId; }

            var login = email.Substring(0, email.IndexOf("@"));
            var offset = 1;
            // don't think we should make this async inside a loop
            while (await queries.LoginExistsInDB(siteGuid, login).ConfigureAwait(false))
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
                DisplayName = email.Substring(0, email.IndexOf("@")),
                FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                AccountApproved = Site.RequireApprovalBeforeLogin ? false : true
            };
            var result = await CreateAsync(user as TUser);
            if(result.Succeeded)
            {
                result = await AddLoginAsync(user as TUser, info);
            }


            return result;
        }

        #region Overrides

        public override async Task<IdentityResult> CreateAsync(TUser user)
        {
            var result = await base.CreateAsync(user);
            if(result.Succeeded)
            {
                await eventHandlers.HandleUserCreated(user).ConfigureAwait(false);
            }

            return result;
        }

        public override async Task<IdentityResult> DeleteAsync(TUser user)
        {
            await eventHandlers.HandleUserPreDelete(user.SiteId, user.Id).ConfigureAwait(false);

            return await base.DeleteAsync(user);
        }

        public override async Task<IdentityResult> UpdateAsync(TUser user)
        {
            await eventHandlers.HandleUserPreUpdate(user.SiteId, user.Id).ConfigureAwait(false);

            var result = await base.UpdateAsync(user);
            if (result.Succeeded)
            {
                await eventHandlers.HandleUserUpdated(user).ConfigureAwait(false);
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
                await commands.Update(user, CancellationToken.None);
                return IdentityResult.Success;
            }

            Logger.LogWarning(12, "User {userId} is locked out.", await GetUserIdAsync(user));

            // TODO: should DefaultLockoutTimeSpan be promoted to a site setting?
            await store.SetLockoutEndDateAsync(
                user, 
                DateTimeOffset.UtcNow.Add(identityOptions.Lockout.DefaultLockoutTimeSpan),
                CancellationToken);

            await store.ResetAccessFailedCountAsync(user, CancellationToken);
            //return await UpdateUserAsync(user);
            await commands.Update(user, CancellationToken.None);

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



        #endregion


    }




}
