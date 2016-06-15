// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-22
// Last Modified:			2016-06-15
// 

using cloudscribe.Core.Models.Geography;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class GeoDataManager
    {
        public GeoDataManager(
            IGeoCommands geoCommands,
            IGeoQueries geoQueries,
            IHttpContextAccessor contextAccessor)
        {
            commands = geoCommands;
            queries = geoQueries;
            _context = contextAccessor?.HttpContext;
        }

        private IGeoCommands commands;
        private IGeoQueries queries;
        private readonly HttpContext _context;
        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        public Task<List<IGeoCountry>> GetCountriesPage(int pageNumber, int pageSize)
        {
            return queries.GetCountriesPage(pageNumber, pageSize, CancellationToken);
        }

        public Task<int> GetCountryCount()
        {
            return queries.GetCountryCount(CancellationToken);
        }

        public Task<IGeoCountry> FetchCountry(Guid guid)
        {
            return queries.FetchCountry(guid, CancellationToken);
        }

        public Task<IGeoCountry> FetchCountry(string isoCode2)
        {
            return queries.FetchCountry(isoCode2, CancellationToken);
        }

        public Task<List<IGeoCountry>> GetAllCountries()
        {
            return queries.GetAllCountries(CancellationToken);
        }

        public async Task Add(IGeoCountry geoCountry)
        {
            if (geoCountry.Id == Guid.Empty) geoCountry.Id = Guid.NewGuid();
            await commands.Add(geoCountry, CancellationToken.None);
        }

        public async Task Update(IGeoCountry geoCountry)
        {
            await commands.Update(geoCountry, CancellationToken.None);
        }

        public async Task DeleteCountry(IGeoCountry country)
        {
            await commands.DeleteGeoZonesByCountry(country.Id, CancellationToken.None);
            await commands.DeleteCountry(country.Id, CancellationToken.None);
            
        }

        public Task<List<IGeoZone>> GetGeoZonesByCountry(Guid countryGuid)
        {
            return queries.GetGeoZonesByCountry(countryGuid, CancellationToken);
        }

        public Task<List<IGeoZone>> GetGeoZonePage(Guid countryGuid, int pageNumber, int pageSize)
        {
            return queries.GetGeoZonePage(countryGuid, pageNumber, pageSize, CancellationToken);
        }

        public Task<int> GetGeoZoneCount(Guid countryGuid)
        {
            return queries.GetGeoZoneCount(countryGuid, CancellationToken);
        }

        public Task<List<IGeoCountry>> CountryAutoComplete(string query, int maxRows)
        {
            return queries.CountryAutoComplete(query, maxRows, CancellationToken);
        }

        public Task<List<IGeoZone>> StateAutoComplete(Guid countryGuid, string query, int maxRows)
        {
            return queries.StateAutoComplete(countryGuid, query, maxRows, CancellationToken);
        }

        public Task<IGeoZone> FetchGeoZone(Guid guid)
        {
            return queries.FetchGeoZone(guid, CancellationToken);
        }

        public async Task Add(IGeoZone geoZone)
        {
            if (geoZone.Id == Guid.Empty) geoZone.Id = Guid.NewGuid();
            await commands.Add(geoZone, CancellationToken.None);
        }

        public async Task Update(IGeoZone geoZone)
        {
            await commands.Update(geoZone, CancellationToken.None);
        }

        public async Task DeleteGeoZone(IGeoZone geoZone)
        {
            await commands.DeleteGeoZone(geoZone.Id, CancellationToken.None);
        }
        
    }
}
