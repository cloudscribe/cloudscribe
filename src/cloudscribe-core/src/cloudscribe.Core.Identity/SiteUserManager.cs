// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-22
// Last Modified:		    2015-08-28
// 
//

using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IEnumerable<IUserTokenProvider<TUser>> tokenProviders,
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
                  tokenProviders,
                  logger,
                  contextAccessor)
        {
            userRepo = userRepository;
            this.siteResolver = siteResolver;
            
        }

        private IUserRepository userRepo;
        private ISiteResolver siteResolver;


        private ISiteSettings siteSettings = null;


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
            return await userRepo.LoginIsAvailable(Site.SiteId, userId, loginName);
 
        }

        public async Task<List<IUserInfo>> GetPage(int siteId, int pageNumber, int pageSize, string userNameBeginsWith, int sortMode)
        {
            return await userRepo.GetPage(siteId, pageNumber, pageSize, userNameBeginsWith, sortMode);
        }

        public async Task<int> CountUsers(int siteId, string userNameBeginsWith)
        {
            return await userRepo.CountUsers(siteId, userNameBeginsWith);
        }

        public async Task<List<IUserInfo>> GetUserAdminSearchPage(int siteId, int pageNumber, int pageSize, string searchInput, int sortMode)
        {
            return await userRepo.GetUserAdminSearchPage(siteId, pageNumber, pageSize, searchInput, sortMode);

        }

        public async Task<int> CountUsersForAdminSearch(int siteId, string searchInput)
        {
            return await userRepo.CountUsersForAdminSearch(siteId, searchInput);
        }

        public async Task<List<IUserInfo>> GetByIPAddress(Guid siteGuid, string ipv4Address)
        {
            return await userRepo.GetByIPAddress(siteGuid, ipv4Address);
        }

        public async Task<ISiteUser> Fetch(int siteId, int userId)
        {
            return await userRepo.Fetch(siteId, userId);
        }

        public async Task<bool> Save(ISiteUser user)
        {
            return await userRepo.Save(user);
        }

    }




}
