// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2015-05-09
// 

using cloudscribe.Caching;
using cloudscribe.Configuration;
using cloudscribe.Core.Models.Geography;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Globalization;

namespace cloudscribe.Core.Repositories.Caching
{
    /// <summary>
    /// TODO: implement caching
    /// </summary>
    public class CachingGeoRepository : IGeoRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CachingUserRepository));

        private IGeoRepository repo = null;

        public CachingGeoRepository(IGeoRepository implementation)
        {
            if ((implementation == null) || (implementation is CachingGeoRepository))
            {
                throw new ArgumentException("you must pass in an implementation of IGeoRepository");
            }

            repo = implementation;

        }

        public async Task<bool> Save(IGeoCountry geoCountry)
        {
            return await repo.Save(geoCountry);
        }

        public async Task<IGeoCountry> FetchCountry(Guid guid)
        {
            return await repo.FetchCountry(guid);
        }

        public async Task<IGeoCountry> FetchCountry(string isoCode2)
        {
            return await repo.FetchCountry(isoCode2);
        }

        public async Task<bool> DeleteCountry(Guid guid)
        {
            return await repo.DeleteCountry(guid);
        }

        public async Task<int> GetCountryCount()
        {
            return await repo.GetCountryCount();
        }

        public async Task<List<IGeoCountry>> GetAllCountries()
        {
            return await repo.GetAllCountries();
        }

        public async Task<List<IGeoCountry>> GetCountriesPage(int pageNumber, int pageSize)
        {
            return await repo.GetCountriesPage(pageNumber, pageSize);
        }

        public async Task<bool> Save(IGeoZone geoZone)
        {
            return await repo.Save(geoZone);
        }

        public async Task<IGeoZone> FetchGeoZone(Guid guid)
        {
            return await repo.FetchGeoZone(guid);
        }

        public async Task<bool> DeleteGeoZone(Guid guid)
        {
            return await repo.DeleteGeoZone(guid);
        }

        public async Task<bool> DeleteGeoZonesByCountry(Guid countryGuid)
        {
            return await repo.DeleteGeoZonesByCountry(countryGuid);
        }

        public async Task<int> GetGeoZoneCount(Guid countryGuid)
        {
            return await repo.GetGeoZoneCount(countryGuid);
        }

        public async Task<List<IGeoZone>> GetGeoZonesByCountry(Guid countryGuid)
        {
            return await repo.GetGeoZonesByCountry(countryGuid);
        }

        public async Task<List<IGeoCountry>> CountryAutoComplete(string query, int maxRows)
        {
            return await repo.CountryAutoComplete(query, maxRows);
        }

        public async Task<List<IGeoZone>> StateAutoComplete(Guid countryGuid, string query, int maxRows)
        {
            return await repo.StateAutoComplete(countryGuid, query, maxRows);
        }

        public async Task<List<IGeoZone>> GetGeoZonePage(Guid countryGuid, int pageNumber, int pageSize)
        {
            return await repo.GetGeoZonePage(countryGuid, pageNumber, pageSize);
        }


        public async Task<bool> Save(ILanguage language)
        {
            return await repo.Save(language);
        }

        public async Task<ILanguage> FetchLanguage(Guid guid)
        {
            return await repo.FetchLanguage(guid);
        }

        public async Task<bool> DeleteLanguage(Guid guid)
        {
            return await repo.DeleteLanguage(guid);
        }

        public async Task<int> GetLanguageCount()
        {
            return await repo.GetLanguageCount();
        }

        public async Task<List<ILanguage>> GetAllLanguages()
        {
            return await repo.GetAllLanguages();
        }

        public async Task<List<ILanguage>> GetLanguagePage(int pageNumber, int pageSize)
        {
            return await repo.GetLanguagePage(pageNumber, pageSize);
        }

        public async Task<bool> Save(ICurrency currency)
        {
            return await repo.Save(currency);
        }

        public async Task<ICurrency> FetchCurrency(Guid guid)
        {
            return await repo.FetchCurrency(guid);
        }

        public async Task<bool> DeleteCurrency(Guid guid)
        {
            return await repo.DeleteCurrency(guid);
        }

        public async Task<List<ICurrency>> GetAllCurrencies()
        {
            return await repo.GetAllCurrencies();
        }

    }
}
