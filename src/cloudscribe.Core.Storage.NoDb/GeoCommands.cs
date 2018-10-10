// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-14
// Last Modified:           2018-10-10
// 

using cloudscribe.Core.Models.Geography;
using NoDb;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    public class GeoCommands : IGeoCommands, IGeoCommandsSingleton
    {
        public GeoCommands(
            //IProjectResolver projectResolver,
            IBasicQueries<GeoCountry> countryQueries,
            IBasicCommands<GeoCountry> countryCommands,
            IBasicQueries<GeoZone> stateQueries,
            IBasicCommands<GeoZone> stateCommands
           
            )
        {
            //this.projectResolver = new DefaultProjectResolver();
            this.countryQueries = countryQueries;
            this.countryCommands = countryCommands;
            this.stateQueries = stateQueries;
            this.stateCommands = stateCommands;
           
        }

        //private IProjectResolver projectResolver;
        private IBasicQueries<GeoCountry> countryQueries;
        private IBasicCommands<GeoCountry> countryCommands;
        private IBasicQueries<GeoZone> stateQueries;
        private IBasicCommands<GeoZone> stateCommands;
        
        //protected string projectId;

        //private async Task EnsureProjectId()
        //{
        //    if (string.IsNullOrEmpty(projectId))
        //    {
        //        projectId = await projectResolver.ResolveProjectId().ConfigureAwait(false);
        //    }

        //}

        public async Task Add(
            IGeoCountry geoCountry,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (geoCountry == null) throw new ArgumentException("geoCountry must not be null");
            if (geoCountry.Id == Guid.Empty) throw new ArgumentException("geoCountry must have a non-empty id");

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var country = GeoCountry.FromIGeoCountry(geoCountry); // convert from IGeoCountry

            await countryCommands.CreateAsync(
                projectId,
                country.Id.ToString(),
                country,
                cancellationToken).ConfigureAwait(false);

        }

        public async Task Update(
            IGeoCountry geoCountry,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (geoCountry == null) throw new ArgumentException("geoCountry must not be null");
            if (geoCountry.Id == Guid.Empty) throw new ArgumentException("geoCountry must have a non-empty id");

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var country = GeoCountry.FromIGeoCountry(geoCountry); // convert from IGeoCountry

            await countryCommands.UpdateAsync(
                projectId,
                country.Id.ToString(),
                country,
                cancellationToken).ConfigureAwait(false);

        }

        public async Task DeleteCountry(
            Guid countryId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (countryId == Guid.Empty) throw new ArgumentException("countryid must be a non-empty id");

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            await countryCommands.DeleteAsync(
                projectId,
                countryId.ToString(),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task Add(
            IGeoZone geoZone,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (geoZone == null) throw new ArgumentException("geoZone must not be null");
            if (geoZone.Id == Guid.Empty) throw new ArgumentException("geoZone must have a non-empty id");

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var state = GeoZone.FromIGeoZone(geoZone); // convert from IGeoZone

            await stateCommands.CreateAsync(
                projectId,
                state.Id.ToString(),
                state,
                cancellationToken).ConfigureAwait(false);

        }

        public async Task Update(
            IGeoZone geoZone,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (geoZone == null) throw new ArgumentException("geoZone must not be null");
            if (geoZone.Id == Guid.Empty) throw new ArgumentException("geoZone must have a non-empty id");
            if (geoZone.CountryId == Guid.Empty) throw new ArgumentException("geoZone must have a non-empty CountryId");

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var state = GeoZone.FromIGeoZone(geoZone); // convert from IGeoZone

            await stateCommands.UpdateAsync(
                projectId,
                state.Id.ToString(),
                state,
                cancellationToken).ConfigureAwait(false);

        }

        public async Task DeleteGeoZone(
            Guid stateId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (stateId == Guid.Empty) throw new ArgumentException("stateid must be a non-empty guid");

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            await stateCommands.DeleteAsync(
                projectId,
                stateId.ToString(),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteGeoZonesByCountry(
            Guid countryId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (countryId == Guid.Empty) throw new ArgumentException("countryId must be a non-empty guid");

            // await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";
            var allStates = await stateQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var states = allStates.ToList().AsQueryable();

            var query = from l in states
                        where l.CountryId == countryId
                        select l;

            foreach(var s in query)
            {
                await stateCommands.DeleteAsync(
                    projectId,
                    s.Id.ToString(),
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
