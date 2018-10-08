// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2018-10-08
// 

using cloudscribe.Core.Models.Geography;
using cloudscribe.Pagination.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public class GeoQueries : IGeoQueries, IGeoQueriesSingleton
    {
        public GeoQueries(ICoreDbContextFactory coreDbContextFactory)
        {
            _contextFactory = coreDbContextFactory;
        }

        private readonly ICoreDbContextFactory _contextFactory;

        public async Task<IGeoCountry> FetchCountry(
            Guid countryId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var item = await dbContext.Countries.SingleOrDefaultAsync(
                    x => x.Id == countryId,
                    cancellationToken)
                    .ConfigureAwait(false);

                return item;
            }      
        }

        public async Task<IGeoCountry> FetchCountry(
            string isoCode2,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Countries.SingleOrDefaultAsync(
                x => x.ISOCode2 == isoCode2,
                cancellationToken)
                .ConfigureAwait(false);
            }
            
        }

        public async Task<int> GetCountryCount(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Countries.CountAsync<GeoCountry>(cancellationToken).ConfigureAwait(false);
            }
            
        }

        public async Task<List<IGeoCountry>> GetAllCountries(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from c in dbContext.Countries
                            orderby c.Name ascending
                            select c;

                var items = await query.AsNoTracking()
                    .ToListAsync<IGeoCountry>(cancellationToken)
                    .ConfigureAwait(false)
                    ;

                return items;
            }
            
        }

        public async Task<PagedResult<IGeoCountry>> GetCountriesPage(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = dbContext.Countries.OrderBy(x => x.Name)
                .Select(p => p)
                .Skip(offset)
                .Take(pageSize)
                ;


                var data = await query
                     .AsNoTracking()
                     .ToListAsync<IGeoCountry>(cancellationToken)
                     .ConfigureAwait(false);

                var result = new PagedResult<IGeoCountry>();
                result.Data = data;
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await GetCountryCount(cancellationToken);

                return result;
            }
            
        }

        public async Task<IGeoZone> FetchGeoZone(
            Guid stateId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var item = await dbContext.States.SingleOrDefaultAsync(
                    x => x.Id == stateId,
                    cancellationToken)
                    .ConfigureAwait(false);

                return item;
            }
            
        }

        public async Task<int> GetGeoZoneCount(
            Guid countryId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.States.CountAsync<GeoZone>(
                    g => g.CountryId == countryId, cancellationToken);
            }

        }

        public async Task<List<IGeoZone>> GetGeoZonesByCountry(
            Guid countryId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = dbContext.States
                        .Where(x => x.CountryId == countryId)
                        .OrderBy(x => x.Name)
                        .Select(x => x);

                var items = await query
                    .AsNoTracking()
                    .ToListAsync<IGeoZone>(cancellationToken)
                    .ConfigureAwait(false);

                return items;
            }

        }

        public async Task<List<IGeoCountry>> CountryAutoComplete(
            string query,
            int maxRows,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var listQuery = dbContext.Countries
                            .Where(x => x.Name.Contains(query) || x.ISOCode2.Contains(query))
                            .OrderBy(x => x.Name)
                            .Take(maxRows)
                            .Select(x => x);

                var items = await listQuery
                    .AsNoTracking()
                    .ToListAsync<IGeoCountry>(cancellationToken)
                    .ConfigureAwait(false);

                return items;
            }

        }

        public async Task<List<IGeoZone>> StateAutoComplete(
            Guid countryId,
            string query,
            int maxRows,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var listQuery = dbContext.States
                            .Where(x =>
                           x.CountryId == countryId &&
                           (x.Code.Contains(query))
                            )
                            .OrderBy(x => x.Code)
                            .Take(maxRows)
                            .Select(x => x);

                return await listQuery
                    .AsNoTracking()
                    .ToListAsync<IGeoZone>(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task<PagedResult<IGeoZone>> GetGeoZonePage(
            Guid countryId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = dbContext.States
                   .Where(x => x.CountryId == countryId)
                   .OrderBy(x => x.Name)
                   .Skip(offset)
                   .Take(pageSize)
                   .Select(p => p)
                   ;

                var data = await query
                    .AsNoTracking()
                    .ToListAsync<IGeoZone>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<IGeoZone>();
                result.Data = data;
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await GetGeoZoneCount(countryId, cancellationToken);

                return result;
            }
            
        }
        
        

    }
}
