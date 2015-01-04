// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2015-01-04
// 

using cloudscribe.Caching;
using cloudscribe.Configuration;
using cloudscribe.Core.Models.Geography;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
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

        public void Save(IGeoCountry geoCountry)
        {
            repo.Save(geoCountry);
        }

        public IGeoCountry FetchCountry(Guid guid)
        {
            return repo.FetchCountry(guid);
        }

        public bool DeleteCountry(Guid guid)
        {
            return repo.DeleteCountry(guid);
        }

        public int GetCountryCount()
        {
            return repo.GetCountryCount();
        }

        public List<IGeoCountry> GetAllCountries()
        {
            return repo.GetAllCountries();
        }

        public List<IGeoCountry> GetCountriesPage(int pageNumber, int pageSize, out int totalPages)
        {
            return repo.GetCountriesPage(pageNumber, pageSize, out totalPages);
        }

        public void Save(IGeoZone geoZone)
        {
            repo.Save(geoZone);
        }

        public IGeoZone FetchGeoZone(Guid guid)
        {
            return repo.FetchGeoZone(guid);
        }

        public bool DeleteGeoZone(Guid guid)
        {
            return repo.DeleteGeoZone(guid);
        }

        public bool DeleteGeoZonesByCountry(Guid countryGuid)
        {
            return repo.DeleteGeoZonesByCountry(countryGuid);
        }

        public int GetGeoZoneCount(Guid countryGuid)
        {
            return repo.GetGeoZoneCount(countryGuid);
        }

        public List<IGeoZone> GetGeoZonesByCountry(Guid countryGuid)
        {
            return repo.GetGeoZonesByCountry(countryGuid);
        }

        public List<IGeoZone> GetGeoZonePage(Guid countryGuid, int pageNumber, int pageSize, out int totalPages)
        {
            return repo.GetGeoZonePage(countryGuid, pageNumber, pageSize, out totalPages);
        }


        public void Save(ILanguage language)
        {
            repo.Save(language);
        }

        public ILanguage FetchLanguage(Guid guid)
        {
            return repo.FetchLanguage(guid);
        }

        public bool DeleteLanguage(Guid guid)
        {
            return repo.DeleteLanguage(guid);
        }

        public int GetLanguageCount()
        {
            return repo.GetLanguageCount();
        }

        public List<ILanguage> GetAllLanguages()
        {
            return repo.GetAllLanguages();
        }

        public List<ILanguage> GetLanguagePage(int pageNumber, int pageSize, out int totalPages)
        {
            return repo.GetLanguagePage(pageNumber, pageSize, out totalPages);
        }

        public void Save(ICurrency currency)
        {
            repo.Save(currency);
        }

        public ICurrency FetchCurrency(Guid guid)
        {
            return repo.FetchCurrency(guid);
        }

        public bool DeleteCurrency(Guid guid)
        {
            return repo.DeleteCurrency(guid);
        }

        public List<ICurrency> GetAllCurrencies()
        {
            return repo.GetAllCurrencies();
        }

    }
}
