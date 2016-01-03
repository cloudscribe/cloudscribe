// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-22
// Last Modified:			2016-01-03
// 

using cloudscribe.Core.Models.Geography;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class GeoDataManager
    {
        public GeoDataManager(
            IGeoRepository geoRepository,
            IHttpContextAccessor contextAccessor)
        {
            repo = geoRepository;
            _context = contextAccessor?.HttpContext;
        }

        private IGeoRepository repo;
        private readonly HttpContext _context;
        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        public async Task<List<IGeoCountry>> GetCountriesPage(int pageNumber, int pageSize)
        {
            return await repo.GetCountriesPage(pageNumber, pageSize, CancellationToken);
        }

        public async Task<int> GetCountryCount()
        {
            return await repo.GetCountryCount(CancellationToken);
        }

        public async Task<IGeoCountry> FetchCountry(Guid guid)
        {
            return await repo.FetchCountry(guid, CancellationToken);
        }

        public async Task<IGeoCountry> FetchCountry(string isoCode2)
        {
            return await repo.FetchCountry(isoCode2, CancellationToken);
        }

        public async Task<List<IGeoCountry>> GetAllCountries()
        {
            return await repo.GetAllCountries(CancellationToken);
        }

        public async Task<bool> Save(IGeoCountry geoCountry)
        {
            return await repo.Save(geoCountry, CancellationToken.None);
        }

        public async Task<bool> DeleteCountry(IGeoCountry country)
        {
            bool result = await repo.DeleteGeoZonesByCountry(country.Guid, CancellationToken.None);
            result = await repo.DeleteCountry(country.Guid, CancellationToken.None);

            return result;
        }

        public async Task<List<IGeoZone>> GetGeoZonesByCountry(Guid countryGuid)
        {
            return await repo.GetGeoZonesByCountry(countryGuid, CancellationToken);
        }

        public async Task<List<IGeoZone>> GetGeoZonePage(Guid countryGuid, int pageNumber, int pageSize)
        {
            return await repo.GetGeoZonePage(countryGuid, pageNumber, pageSize, CancellationToken);
        }

        public async Task<int> GetGeoZoneCount(Guid countryGuid)
        {
            return await repo.GetGeoZoneCount(countryGuid, CancellationToken);
        }

        public async Task<List<IGeoCountry>> CountryAutoComplete(string query, int maxRows)
        {
            return await repo.CountryAutoComplete(query, maxRows, CancellationToken);
        }

        public async Task<List<IGeoZone>> StateAutoComplete(Guid countryGuid, string query, int maxRows)
        {
            return await repo.StateAutoComplete(countryGuid, query, maxRows, CancellationToken);
        }

        public async Task<IGeoZone> FetchGeoZone(Guid guid)
        {
            return await repo.FetchGeoZone(guid, CancellationToken);
        }

        public async Task<bool> Save(IGeoZone geoZone)
        {
            return await repo.Save(geoZone, CancellationToken.None);
        }

        public async Task<bool> DeleteGeoZone(IGeoZone geoZone)
        {
            return await repo.DeleteGeoZone(geoZone.Guid, CancellationToken.None);
        }



        public async Task<List<ICurrency>> GetAllCurrencies()
        {
            return await repo.GetAllCurrencies(CancellationToken);
        }

        public async Task<ICurrency> FetchCurrency(Guid guid)
        {
            return await repo.FetchCurrency(guid, CancellationToken);
        }

        public async Task<bool> Save(ICurrency currency)
        {
            return await repo.Save(currency, CancellationToken.None);
        }

        public async Task<bool> DeleteCurrency(ICurrency currency)
        {
            return await repo.DeleteCurrency(currency.Guid, CancellationToken.None);
        }

    }
}
