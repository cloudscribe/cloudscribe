// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-06-19
// Last Modified:		    2016-12-07
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public sealed class RoleStore<TRole> : IRoleStore<TRole> where TRole : SiteRole
    {
        public RoleStore(
            SiteContext currentSite,
            ILogger<RoleStore<TRole>> logger,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IUserCommands userCommands,
            IUserQueries userQueries
            )
        {
            _commands = userCommands ?? throw new ArgumentNullException(nameof(userCommands));
            _queries = userQueries ?? throw new ArgumentNullException(nameof(userQueries));
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
            _siteSettings = currentSite ?? throw new ArgumentNullException(nameof(currentSite));
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            
        }

        private MultiTenantOptions _multiTenantOptions;
        private ILogger _log;
        private IUserCommands _commands;
        private IUserQueries _queries;
        private ISiteContext _siteSettings = null;
        
        private ISiteContext Site
        {
            get { return _siteSettings; }
        }

        private Guid GetSiteId()
        {
            if(_multiTenantOptions.UseRelatedSitesMode)
            {
                if(_multiTenantOptions.RelatedSiteId != Guid.Empty)
                {
                    return _multiTenantOptions.RelatedSiteId;
                }
            }

            return Site.Id;
        }
        
        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            if (role.SiteId == Guid.Empty){ role.SiteId = GetSiteId(); }
            if (role.Id == Guid.Empty) role.Id = Guid.NewGuid();
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            
            await _commands.CreateRole(role, cancellationToken);
            return IdentityResult.Success;

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
            await _commands.DeleteUserRolesByRole(role.SiteId, role.Id, cancellationToken);
            await _commands.DeleteRole(role.SiteId, role.Id, cancellationToken);

            return IdentityResult.Success; 
            
        }

        public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(roleId)) throw new ArgumentException("invalid roleid");
            if(roleId.Length != 36) throw new ArgumentException("invalid roleid");
            var roleGuid = new Guid(roleId);
            var role = await _queries.FetchRole(GetSiteId(), roleGuid, cancellationToken);

            return (TRole)role;
        }

        public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            
            var role = await _queries.FetchRole(GetSiteId(), normalizedRoleName, cancellationToken);

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

            return Task.FromResult(role.NormalizedRoleName);
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return Task.FromResult(role.Id.ToString());
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
            // we are not allowing RoleName to ever be updated
            // only DisplayName can be updated for an existing role
            role.NormalizedRoleName = normalizedName;
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

            await _commands.UpdateRole(role, cancellationToken);

            return IdentityResult.Success; 

            
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
