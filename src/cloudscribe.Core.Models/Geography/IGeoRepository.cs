// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2015-05-09
// 

// TODO: we should update all the async signatures to take a cancellationtoken

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.Geography
{
    public interface IGeoRepository
    {
        Task<bool> DeleteCountry(Guid guid);
        Task<bool> DeleteGeoZone(Guid guid);
        Task<bool> DeleteGeoZonesByCountry(Guid countryGuid);
        Task<IGeoCountry> FetchCountry(Guid guid);
        Task<IGeoCountry> FetchCountry(string isoCode2);
        Task<IGeoZone> FetchGeoZone(Guid guid);
        Task<List<IGeoCountry>> GetAllCountries();
        Task<List<IGeoCountry>> GetCountriesPage(int pageNumber, int pageSize);
        Task<int> GetCountryCount();
        Task<int> GetGeoZoneCount(Guid countryGuid);
        Task<List<IGeoZone>> GetGeoZonePage(Guid countryGuid, int pageNumber, int pageSize);
        Task<List<IGeoZone>> GetGeoZonesByCountry(Guid countryGuid);
        Task<List<IGeoCountry>> CountryAutoComplete(string query, int maxRows);
        Task<List<IGeoZone>> StateAutoComplete(Guid countryGuid, string query, int maxRows);
        Task<bool> Save(IGeoCountry geoCountry);
        Task<bool> Save(IGeoZone geoZone);

        Task<bool> Save(ILanguage language);
        Task<ILanguage> FetchLanguage(Guid guid);
        Task<bool> DeleteLanguage(Guid guid);
        Task<int> GetLanguageCount();
        Task<List<ILanguage>> GetAllLanguages();
        Task<List<ILanguage>> GetLanguagePage(int pageNumber, int pageSize);

        Task<bool> Save(ICurrency currency);
        Task<ICurrency> FetchCurrency(Guid guid);
        Task<bool> DeleteCurrency(Guid guid);
        Task<List<ICurrency>> GetAllCurrencies();
    }
}
