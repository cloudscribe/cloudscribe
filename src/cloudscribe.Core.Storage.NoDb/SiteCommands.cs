// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-13
// Last Modified:           2018-08-19
// 

using cloudscribe.Core.Models;
using NoDb;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    public class SiteCommands : ISiteCommands
    {
        public SiteCommands(
            //IProjectResolver projectResolver,
            IBasicQueries<SiteSettings> queries,
            IBasicCommands<SiteSettings> commands,
            IBasicQueries<SiteHost> hostQueries,
            IBasicCommands<SiteHost> hostCommands
            )
        {
            //this.projectResolver = new DefaultProjectResolver();
            this.queries = queries;
            this.commands = commands;
            this.hostQueries = hostQueries;
            this.hostCommands = hostCommands;
        }

        //private IProjectResolver projectResolver;
        private IBasicQueries<SiteSettings> queries;
        private IBasicCommands<SiteSettings> commands;
        private IBasicQueries<SiteHost> hostQueries;
        private IBasicCommands<SiteHost> hostCommands;

        //protected string projectId;

        //private async Task EnsureProjectId()
        //{
        //    if (string.IsNullOrEmpty(projectId))
        //    {
        //        projectId = await projectResolver.ResolveProjectId().ConfigureAwait(false);
        //    }

        //}

        public async Task Create(
            ISiteSettings site,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (site == null) throw new ArgumentException("site must not be null");
            if (site.Id == Guid.Empty) throw new ArgumentException("site must have a non-empty Id");

            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            
            var siteSettings = SiteSettings.FromISiteSettings(site);
            var projectId = siteSettings.Id.ToString();

            await commands.CreateAsync(
                projectId,
                siteSettings.Id.ToString(),
                siteSettings,
                cancellationToken).ConfigureAwait(false);

        }

        public async Task Update(
            ISiteSettings site,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (site == null) throw new ArgumentException("site must not be null");
            if (site.Id == Guid.Empty) throw new ArgumentException("site must have a non-empty Id");

            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);

            var siteSettings = SiteSettings.FromISiteSettings(site);
            siteSettings.LastModifiedUtc = DateTime.UtcNow;
            var projectId = siteSettings.Id.ToString();

            await commands.UpdateAsync(
                projectId,
                siteSettings.Id.ToString(),
                siteSettings,
                cancellationToken).ConfigureAwait(false);

        }

        public async Task Delete(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (siteId == Guid.Empty) throw new ArgumentException("siteid must not be empty guid");

            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            await commands.DeleteAsync(
                projectId,
                siteId.ToString(),
                cancellationToken).ConfigureAwait(false);

        }

        public async Task AddHost(
            Guid siteId,
            string hostName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (siteId == Guid.Empty) throw new ArgumentException("siteId must not be empty guid");

            if (string.IsNullOrEmpty(hostName)) throw new ArgumentException("hostName must be provided");

            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var host = new SiteHost();
            host.SiteId = siteId;
            host.HostName = hostName;

            await hostCommands.CreateAsync(
                projectId, 
                host.Id.ToString(), 
                host, 
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task DeleteHost(
            Guid siteId,
            Guid hostId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (hostId == Guid.Empty) throw new ArgumentException("hostId must not be empty guid");
            
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            await hostCommands.DeleteAsync(
                projectId,
                hostId.ToString(),
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task DeleteHostsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (siteId == Guid.Empty) throw new ArgumentException("siteId must not be empty guid");

            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allHosts = await hostQueries.GetAllAsync(
                projectId,
                cancellationToken).ConfigureAwait(false);

            var query = from x in allHosts.Where(x => x.SiteId == siteId)
                        select x;

            foreach(var h in query)
            {
                await hostCommands.DeleteAsync(
                    projectId,
                    h.Id.ToString(),
                    cancellationToken).ConfigureAwait(false);
            }

            
        }


        #region IDisposable Support

        private void ThrowIfDisposed()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

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
