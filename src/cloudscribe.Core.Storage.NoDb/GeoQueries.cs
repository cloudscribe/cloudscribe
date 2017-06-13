// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-14
// Last Modified:           2016-08-03
// 

using cloudscribe.Core.Models.Geography;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    public class GeoQueries : IGeoQueries
    {
        public GeoQueries(
            //IProjectResolver projectResolver,
            IBasicQueries<GeoCountry> countryQueries,
            IBasicQueries<GeoZone> stateQueries
            )
        {
            //this.projectResolver = new DefaultProjectResolver();
            this.countryQueries = countryQueries;
            this.stateQueries = stateQueries;
           
        }

        //private IProjectResolver projectResolver;
        private IBasicQueries<GeoCountry> countryQueries;
        private IBasicQueries<GeoZone> stateQueries;
        
        //protected string projectId;

        //private async Task EnsureProjectId()
        //{
        //    if (string.IsNullOrEmpty(projectId))
        //    {
        //        projectId = await projectResolver.ResolveProjectId().ConfigureAwait(false); 
        //    }

        //}

        public async Task<IGeoCountry> FetchCountry(
            Guid countryId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            return await countryQueries.FetchAsync(
                projectId,
                countryId.ToString(),
                cancellationToken).ConfigureAwait(false);
            
        }

        public async Task<IGeoCountry> FetchCountry(
            string isoCode2,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var all = await countryQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var countries = all.ToList().AsQueryable();

            return countries.Where(
                x => x.ISOCode2 == isoCode2
                )
                .SingleOrDefault();
        }

        public async Task<int> GetCountryCount(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var all = await countryQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            return all.ToList().Count;
        }

        public async Task<List<IGeoCountry>> GetAllCountries(CancellationToken cancellationToken = default(CancellationToken))
        { 
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var all = await countryQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var countries = all.ToList().AsQueryable();

            var query = from c in countries
                        orderby c.Name ascending
                        select c;

            return query.ToList<IGeoCountry>();
            
        }

        public async Task<List<IGeoCountry>> GetCountriesPage(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var all = await countryQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var countries = all.ToList().AsQueryable();

            int offset = (pageSize * pageNumber) - pageSize;

            var query = countries.OrderBy(x => x.Name)
                .Select(p => p)
                .Skip(offset)
                .Take(pageSize)
                ;

            return query.ToList<IGeoCountry>();

        }

        public async Task<IGeoZone> FetchGeoZone(
            Guid stateId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            return await stateQueries.FetchAsync(
                projectId,
                stateId.ToString(),
                cancellationToken).ConfigureAwait(false);

        }

        public async Task<int> GetGeoZoneCount(
            Guid countryId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var all = await stateQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            return all.Where(
                g => g.CountryId == countryId).ToList().Count;

            
        }

        public async Task<List<IGeoZone>> GetGeoZonesByCountry(
            Guid countryId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var all = await stateQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var states = all.ToList().AsQueryable();

            var query = states
                        .Where(x => x.CountryId == countryId)
                        .OrderBy(x => x.Name)
                        .Select(x => x);

            return  query.ToList<IGeoZone>();
            
        }

        public async Task<List<IGeoCountry>> CountryAutoComplete(
            string query,
            int maxRows,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var all = await countryQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var countries = all.ToList().AsQueryable();

            // approximation of a LIKE operator query
            //http://stackoverflow.com/questions/17097764/linq-to-entities-using-the-sql-like-operator
            
            var listQuery = countries
                            .Where(x => x.Name.Contains(query) || x.ISOCode2.Contains(query))
                            .OrderBy(x => x.Name)
                            .Take(maxRows)
                            .Select(x => x);

            return listQuery.ToList<IGeoCountry>();
            
        }

        public async Task<List<IGeoZone>> StateAutoComplete(
            Guid countryId,
            string query,
            int maxRows,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var all = await stateQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var states = all.ToList().AsQueryable();
            
            var listQuery = states
                            .Where(x =>
                           x.CountryId == countryId &&
                           ( x.Code.Contains(query))
                            )
                            .OrderBy(x => x.Code)
                            .Take(maxRows)
                            .Select(x => x);

            return listQuery.ToList<IGeoZone>();

        }

        public async Task<List<IGeoZone>> GetGeoZonePage(
            Guid countryId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var all = await stateQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var states = all.ToList().AsQueryable();

            int offset = (pageSize * pageNumber) - pageSize;

            var query = states
               .Where(x => x.CountryId == countryId)
               .OrderBy(x => x.Name)
               .Skip(offset)
               .Take(pageSize)
               .Select(p => p)
               ;

            return  query.ToList<IGeoZone>();

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
