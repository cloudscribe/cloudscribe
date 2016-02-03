// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-22
// Last Modified:			2016-02-03
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

        public Task<List<IGeoCountry>> GetCountriesPage(int pageNumber, int pageSize)
        {
            return repo.GetCountriesPage(pageNumber, pageSize, CancellationToken);
        }

        public Task<int> GetCountryCount()
        {
            return repo.GetCountryCount(CancellationToken);
        }

        public Task<IGeoCountry> FetchCountry(Guid guid)
        {
            return repo.FetchCountry(guid, CancellationToken);
        }

        public Task<IGeoCountry> FetchCountry(string isoCode2)
        {
            return repo.FetchCountry(isoCode2, CancellationToken);
        }

        public Task<List<IGeoCountry>> GetAllCountries()
        {
            return repo.GetAllCountries(CancellationToken);
        }

        public Task<bool> Add(IGeoCountry geoCountry)
        {
            return repo.Add(geoCountry, CancellationToken.None);
        }

        public Task<bool> Update(IGeoCountry geoCountry)
        {
            return repo.Update(geoCountry, CancellationToken.None);
        }

        public async Task<bool> DeleteCountry(IGeoCountry country)
        {
            bool result = await repo.DeleteGeoZonesByCountry(country.Guid, CancellationToken.None);
            result = await repo.DeleteCountry(country.Guid, CancellationToken.None);

            return result;
        }

        public Task<List<IGeoZone>> GetGeoZonesByCountry(Guid countryGuid)
        {
            return repo.GetGeoZonesByCountry(countryGuid, CancellationToken);
        }

        public Task<List<IGeoZone>> GetGeoZonePage(Guid countryGuid, int pageNumber, int pageSize)
        {
            return repo.GetGeoZonePage(countryGuid, pageNumber, pageSize, CancellationToken);
        }

        public Task<int> GetGeoZoneCount(Guid countryGuid)
        {
            return repo.GetGeoZoneCount(countryGuid, CancellationToken);
        }

        public Task<List<IGeoCountry>> CountryAutoComplete(string query, int maxRows)
        {
            return repo.CountryAutoComplete(query, maxRows, CancellationToken);
        }

        public Task<List<IGeoZone>> StateAutoComplete(Guid countryGuid, string query, int maxRows)
        {
            return repo.StateAutoComplete(countryGuid, query, maxRows, CancellationToken);
        }

        public Task<IGeoZone> FetchGeoZone(Guid guid)
        {
            return repo.FetchGeoZone(guid, CancellationToken);
        }

        public Task<bool> Add(IGeoZone geoZone)
        {
            return repo.Add(geoZone, CancellationToken.None);
        }

        public Task<bool> Update(IGeoZone geoZone)
        {
            return repo.Update(geoZone, CancellationToken.None);
        }

        public Task<bool> DeleteGeoZone(IGeoZone geoZone)
        {
            return repo.DeleteGeoZone(geoZone.Guid, CancellationToken.None);
        }



        public Task<List<ICurrency>> GetAllCurrencies()
        {
            return repo.GetAllCurrencies(CancellationToken);
        }

        public Task<ICurrency> FetchCurrency(Guid guid)
        {
            return repo.FetchCurrency(guid, CancellationToken);
        }

        public Task<bool> Add(ICurrency currency)
        {
            return repo.Add(currency, CancellationToken.None);
        }

        public Task<bool> Update(ICurrency currency)
        {
            return repo.Update(currency, CancellationToken.None);
        }

        public Task<bool> DeleteCurrency(ICurrency currency)
        {
            return repo.DeleteCurrency(currency.Guid, CancellationToken.None);
        }

        public Task<bool> Add(ILanguage language)
        {
            return repo.Add(language, CancellationToken.None);
        }

        public Task<bool> Update(ILanguage language)
        {
            return repo.Update(language, CancellationToken.None);
        }

        public Task<int> GetLanguageCount()
        {
            return repo.GetLanguageCount(CancellationToken);
        }

    }
}
