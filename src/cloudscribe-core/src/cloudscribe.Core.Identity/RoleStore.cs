// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-06-19
// Last Modified:		    2015-07-05
// 
// You must not remove this notice, or any other, from this software.


using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Configuration;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    //https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity.EntityFramework/RoleStore.cs
    //TODO: implement
    public sealed class RoleStore<TRole> : IRoleStore<TRole> where TRole : SiteRole
    {
        private ILoggerFactory logFactory;
        private ILogger log;
        private bool debugLog = false;

        private ISiteResolver resolver;
        private ISiteSettings siteSettings = null;
        private IConfiguration config;

        public ISiteSettings SiteSettings
        {
            get {
                if(siteSettings == null) { siteSettings = resolver.Resolve(); }
                return siteSettings;
            }
        }
        private IUserRepository repo;


        private RoleStore() { }

        public RoleStore(
            ILoggerFactory loggerFactory,
            ISiteResolver siteResolver,
            IUserRepository userRepository,
            IConfiguration configuration
            )
        {
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            if (siteResolver == null) { throw new ArgumentNullException(nameof(siteResolver)); }
            if (userRepository == null) { throw new ArgumentNullException(nameof(userRepository)); }
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }

            resolver = siteResolver;
            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(this.GetType().FullName);
            config = configuration;
            debugLog = config.UserStoreDebugEnabled();


            //siteSettings = site;


            repo = userRepository;

            if (debugLog) { log.LogInformation("constructor"); }
        }

        public Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
