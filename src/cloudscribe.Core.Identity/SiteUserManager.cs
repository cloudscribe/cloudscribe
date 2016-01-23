// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-22
// Last Modified:		    2015-12-27
// 
//

using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
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
            ISiteResolver siteResolver,
            IUserRepository userRepository,
            IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
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
                  keyNormalizer,
                  errors,
                  serviceProvider,
                  logger,
                  contextAccessor)
        {
            defaultIdentityOptions = optionsAccessor.Value;
            userStore = store;
            userRepo = userRepository;
            this.siteResolver = siteResolver;
            multiTenantOptions = multiTenantOptionsAccessor.Value;
            this.contextAccessor = contextAccessor;
            _context = contextAccessor?.HttpContext;
        }

        private IdentityOptions defaultIdentityOptions;
        private IUserStore<TUser> userStore;
        private IUserRepository userRepo;
        private ISiteResolver siteResolver;
        private MultiTenantOptions multiTenantOptions;
        private IHttpContextAccessor contextAccessor;
        private readonly HttpContext _context;
        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        private ISiteSettings siteSettings = null;

        internal IUserLockoutStore<TUser> GetUserLockoutStore()
        {
            var cast = Store as IUserLockoutStore<TUser>;
            if (cast == null)
            {
                throw new NotSupportedException("IUserLockoutStore was null");
            }
            return cast;
        }



        public ISiteSettings Site
        {
            get
            {
                if (siteSettings == null) { siteSettings = siteResolver.Resolve(); }
                return siteSettings;
            }
        }
        //private ILogger<UserManager<TUser>> logger;

        public async Task<bool> LoginIsAvailable(int userId, string loginName)
        {
            int siteId = Site.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.LoginIsAvailable(siteId, userId, loginName, CancellationToken);
 
        }

        public async Task<string> GetUserNameFromEmail(int siteId, string email)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.GetUserNameFromEmail(siteId, email, CancellationToken);
        }

        public async Task<List<IUserInfo>> GetPage(int siteId, int pageNumber, int pageSize, string userNameBeginsWith, int sortMode)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.GetPage(siteId, pageNumber, pageSize, userNameBeginsWith, sortMode, CancellationToken);
        }

        public int GetCount(int siteId)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return userRepo.GetCount(siteId);
        }

        public async Task<int> CountUsers(int siteId, string userNameBeginsWith)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.CountUsers(siteId, userNameBeginsWith, CancellationToken);
        }

        public async Task<int> CountLockedOutUsers(int siteId)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.CountLockedOutUsers(siteId, CancellationToken);
        }

        public async Task<List<IUserInfo>> GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.GetPageLockedOutUsers(siteId, pageNumber, pageSize, CancellationToken);
        }

        public async Task<List<IUserInfo>> GetUserAdminSearchPage(int siteId, int pageNumber, int pageSize, string searchInput, int sortMode)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.GetUserAdminSearchPage(siteId, pageNumber, pageSize, searchInput, sortMode, CancellationToken);

        }

        public async Task<int> CountUsersForAdminSearch(int siteId, string searchInput)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.CountUsersForAdminSearch(siteId, searchInput, CancellationToken);
        }

        public async Task<int> CountNotApprovedUsers(int siteId)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.CountNotApprovedUsers(siteId, CancellationToken);
        }

        public async Task<List<IUserInfo>> GetNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.GetNotApprovedUsers(siteId, pageNumber, pageSize, CancellationToken);

        }

        public async Task<List<IUserInfo>> GetByIPAddress(Guid siteGuid, string ipv4Address)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteGuid = Guid.Empty; }

            return await userRepo.GetByIPAddress(siteGuid, ipv4Address, CancellationToken);
        }

        public async Task<ISiteUser> Fetch(int siteId, int userId)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.Fetch(siteId, userId, CancellationToken);
        }

        public async Task<ISiteUser> Fetch(int siteId, Guid userGuid)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.Fetch(siteId, userGuid, CancellationToken);
        }

        public async Task<ISiteUser> FetchByConfirmationGuid(int siteId, Guid confirmGuid)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.FetchByConfirmationGuid(siteId, confirmGuid, CancellationToken);
        }

        public async Task<bool> Save(ISiteUser user)
        {
            return await userRepo.Save(user, CancellationToken);
        }

        public async Task<bool> EmailExistsInDB(int siteId, string email)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.EmailExistsInDB(siteId, email, CancellationToken);
        }

        public async Task<bool> EmailExistsInDB(int siteId, int userId, string email)
        {
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            return await userRepo.EmailExistsInDB(siteId, userId, email, CancellationToken);

        }

        #region Overrides

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
                await userRepo.Save(user, CancellationToken.None);
                return IdentityResult.Success;
            }

            Logger.LogWarning(12, "User {userId} is locked out.", await GetUserIdAsync(user));

            // TODO: should DefaultLockoutTimeSpan be promoted to a site setting?
            await store.SetLockoutEndDateAsync(
                user, 
                DateTimeOffset.UtcNow.Add(defaultIdentityOptions.Lockout.DefaultLockoutTimeSpan),
                CancellationToken);

            await store.ResetAccessFailedCountAsync(user, CancellationToken);
            //return await UpdateUserAsync(user);
            await userRepo.Save(user, CancellationToken.None);

            return IdentityResult.Success;
        }


        #endregion


    }




}
