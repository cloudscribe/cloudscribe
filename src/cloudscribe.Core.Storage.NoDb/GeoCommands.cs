// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-14
// Last Modified:           2016-05-14
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using NoDb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    public class GeoCommands : IGeoCommands
    {
        public GeoCommands(
            IProjectResolver projectResolver,
            IBasicQueries<GeoCountry> countryQueries,
            IBasicCommands<GeoCountry> countryCommands,
            IBasicQueries<GeoZone> stateQueries,
            IBasicCommands<GeoZone> stateCommands,
            IBasicCommands<Language> langCommands,
            IBasicCommands<Currency> currencyCommands
            )
        {
            this.projectResolver = projectResolver;
            this.countryQueries = countryQueries;
            this.countryCommands = countryCommands;
            this.stateQueries = stateQueries;
            this.stateCommands = stateCommands;
            this.langCommands = langCommands;
            this.currencyCommands = currencyCommands;

        }

        private IProjectResolver projectResolver;
        private IBasicQueries<GeoCountry> countryQueries;
        private IBasicCommands<GeoCountry> countryCommands;
        private IBasicQueries<GeoZone> stateQueries;
        private IBasicCommands<GeoZone> stateCommands;
        private IBasicCommands<Language> langCommands;
        private IBasicCommands<Currency> currencyCommands;

        protected string projectId;

        private async Task EnsureProjectId()
        {
            if (string.IsNullOrEmpty(projectId))
            {
                await projectResolver.ResolveProjectId().ConfigureAwait(false);
            }

        }

        public async Task Add(
            IGeoCountry geoCountry,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (geoCountry == null) throw new ArgumentException("geoCountry must not be null");
            if (geoCountry.Id == Guid.Empty) throw new ArgumentException("geoCountry must have a non-empty id");

            await EnsureProjectId().ConfigureAwait(false);

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

            await EnsureProjectId().ConfigureAwait(false);

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

            await EnsureProjectId().ConfigureAwait(false);
            
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

            await EnsureProjectId().ConfigureAwait(false);

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

            await EnsureProjectId().ConfigureAwait(false);

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

            await EnsureProjectId().ConfigureAwait(false);

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

            await EnsureProjectId().ConfigureAwait(false);
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

        public async Task Add(
            ILanguage language,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (language == null) throw new ArgumentException("language must not be null");
            if (language.Id == Guid.Empty) throw new ArgumentException("language must have a non-empty id");

            await EnsureProjectId().ConfigureAwait(false);
            var lang = Language.FromILanguage(language);
            await langCommands.CreateAsync(
                projectId,
                lang.Id.ToString(),
                lang,
                cancellationToken).ConfigureAwait(false);

        }

        public async Task Update(
            ILanguage language,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (language == null) throw new ArgumentException("language must not be null");
            if (language.Id == Guid.Empty) throw new ArgumentException("language must have a non-empty id");

            await EnsureProjectId().ConfigureAwait(false);
            var lang = Language.FromILanguage(language);
            await langCommands.UpdateAsync(
                projectId,
                lang.Id.ToString(),
                lang,
                cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteLanguage(
            Guid languageId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (languageId == Guid.Empty) throw new ArgumentException("id must be a non-empty guid");

            await EnsureProjectId().ConfigureAwait(false);

            await langCommands.DeleteAsync(
                projectId,
                languageId.ToString(),
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task Add(
            ICurrency currency,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (currency == null) throw new ArgumentException("currency must not be null");
            if (currency.Id == Guid.Empty) throw new ArgumentException("currency must have a non-empty id");

            var c = Currency.FromICurrency(currency);

            await EnsureProjectId().ConfigureAwait(false);
            await currencyCommands.CreateAsync(
                projectId,
                c.Id.ToString(),
                c,
                cancellationToken).ConfigureAwait(false);

        }

        public async Task Update(
            ICurrency currency,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (currency == null) throw new ArgumentException("currency must not be null");
            if (currency.Id == Guid.Empty) throw new ArgumentException("currency must have a non-empty id");

            var c = Currency.FromICurrency(currency);

            await EnsureProjectId().ConfigureAwait(false);

            await currencyCommands.UpdateAsync(
                projectId,
                c.Id.ToString(),
                c,
                cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteCurrency(
            Guid currencyId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (currencyId == Guid.Empty) throw new ArgumentException("id must be a non-empty guid");

            await EnsureProjectId().ConfigureAwait(false);

            await currencyCommands.DeleteAsync(
                projectId,
                currencyId.ToString(),
                cancellationToken).ConfigureAwait(false);
            
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
