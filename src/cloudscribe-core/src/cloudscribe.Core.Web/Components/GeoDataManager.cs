// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-22
// Last Modified:			2015-08-04
// 

using cloudscribe.Core.Models.Geography;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class GeoDataManager
    {
        public GeoDataManager(IGeoRepository geoRepository)
        {
            repo = geoRepository;
        }

        private IGeoRepository repo;

        public async Task<List<IGeoCountry>> GetCountriesPage(int pageNumber, int pageSize)
        {
            return await repo.GetCountriesPage(pageNumber, pageSize);
        }

        public async Task<int> GetCountryCount()
        {
            return await repo.GetCountryCount();
        }

        public async Task<IGeoCountry> FetchCountry(Guid guid)
        {
            return await repo.FetchCountry(guid);
        }

        public async Task<IGeoCountry> FetchCountry(string isoCode2)
        {
            return await repo.FetchCountry(isoCode2);
        }

        public async Task<List<IGeoCountry>> GetAllCountries()
        {
            return await repo.GetAllCountries();
        }

        public async Task<bool> Save(IGeoCountry geoCountry)
        {
            return await repo.Save(geoCountry);
        }

        public async Task<bool> DeleteCountry(IGeoCountry country)
        {
            bool result = await repo.DeleteGeoZonesByCountry(country.Guid);
            result = await repo.DeleteCountry(country.Guid);

            return result;
        }

        public async Task<List<IGeoZone>> GetGeoZonesByCountry(Guid countryGuid)
        {
            return await repo.GetGeoZonesByCountry(countryGuid);
        }

        public async Task<List<IGeoZone>> GetGeoZonePage(Guid countryGuid, int pageNumber, int pageSize)
        {
            return await repo.GetGeoZonePage(countryGuid, pageNumber, pageSize);
        }

        public async Task<int> GetGeoZoneCount(Guid countryGuid)
        {
            return await repo.GetGeoZoneCount(countryGuid);
        }

        public async Task<List<IGeoCountry>> CountryAutoComplete(string query, int maxRows)
        {
            return await repo.CountryAutoComplete(query, maxRows);
        }

        public async Task<List<IGeoZone>> StateAutoComplete(Guid countryGuid, string query, int maxRows)
        {
            return await repo.StateAutoComplete(countryGuid, query, maxRows);
        }

        public async Task<IGeoZone> FetchGeoZone(Guid guid)
        {
            return await repo.FetchGeoZone(guid);
        }

        public async Task<bool> Save(IGeoZone geoZone)
        {
            return await repo.Save(geoZone);
        }

        public async Task<bool> DeleteGeoZone(IGeoZone geoZone)
        {
            return await repo.DeleteGeoZone(geoZone.Guid);
        }



        public async Task<List<ICurrency>> GetAllCurrencies()
        {
            return await repo.GetAllCurrencies();
        }

        public async Task<ICurrency> FetchCurrency(Guid guid)
        {
            return await repo.FetchCurrency(guid);
        }

        public async Task<bool> Save(ICurrency currency)
        {
            return await repo.Save(currency);
        }

        public async Task<bool> DeleteCurrency(ICurrency currency)
        {
            return await repo.DeleteCurrency(currency.Guid);
        }

    }
}
