// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-06-19
// Last Modified:		    2016-02-04
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public sealed class RoleStore<TRole> : IRoleStore<TRole> where TRole : SiteRole
    {
        public RoleStore(
            SiteSettings currentSite,
            ILogger<RoleStore<TRole>> logger,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IUserRepository userRepository
            )
        {
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            if (currentSite == null) { throw new ArgumentNullException(nameof(currentSite)); }
            if (userRepository == null) { throw new ArgumentNullException(nameof(userRepository)); }
           
            log = logger;
            siteSettings = currentSite;

            multiTenantOptions = multiTenantOptionsAccessor.Value;
            userRepo = userRepository;

            //if (debugLog) { log.LogInformation("constructor"); }
        }

        private MultiTenantOptions multiTenantOptions;
        private ILogger log;
        //private bool debugLog = false;
        private IUserRepository userRepo;
        private ISiteSettings siteSettings = null;
        
        private ISiteSettings Site
        {
            get { return siteSettings; }
        }
        
        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            if (role.SiteGuid == Guid.Empty)
            {
                role.SiteGuid = Site.SiteGuid;
            }
            if(role.SiteId == -1)
            {
                role.SiteId = Site.SiteId;
            }

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            bool result = await userRepo.SaveRole(role, cancellationToken);

            if(result) { return IdentityResult.Success; }

            return IdentityResult.Failed(null);
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            // remove all users form the role
            bool result = await userRepo.DeleteUserRolesByRole(role.RoleId, cancellationToken);
            result = await userRepo.DeleteRole(role.RoleId, cancellationToken);

            if (result) { return IdentityResult.Success; }

            return IdentityResult.Failed(null);
        }

        public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            ISiteRole role = await userRepo.FetchRole(Convert.ToInt32(roleId), cancellationToken);

            return (TRole)role;
        }

        public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            int siteId = Site.SiteId;
            if (multiTenantOptions.UseRelatedSitesMode) { siteId = multiTenantOptions.RelatedSiteId; }

            ISiteRole role = await userRepo.FetchRole(siteId, normalizedRoleName, cancellationToken);

            return (TRole)role;
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return Task.FromResult(role.RoleName);
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return Task.FromResult(role.RoleId.ToInvariantString());
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return Task.FromResult(role.RoleName);
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            role.RoleName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            role.RoleName = roleName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            bool result = await userRepo.SaveRole(role, cancellationToken);

            if (result) { return IdentityResult.Success; }

            return IdentityResult.Failed(null);
        }

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
