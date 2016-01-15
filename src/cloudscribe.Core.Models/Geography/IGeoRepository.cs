// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2016-01-15
// 

// TODO: we should update all the async signatures to take a cancellationtoken

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.Geography
{
    public interface IGeoRepository
    {
        Task<bool> DeleteCountry(Guid guid, CancellationToken cancellationToken);
        Task<bool> DeleteGeoZone(Guid guid, CancellationToken cancellationToken);
        Task<bool> DeleteGeoZonesByCountry(Guid countryGuid, CancellationToken cancellationToken);
        Task<IGeoCountry> FetchCountry(Guid guid, CancellationToken cancellationToken);
        Task<IGeoCountry> FetchCountry(string isoCode2, CancellationToken cancellationToken);
        Task<IGeoZone> FetchGeoZone(Guid guid, CancellationToken cancellationToken);
        Task<List<IGeoCountry>> GetAllCountries(CancellationToken cancellationToken);
        Task<List<IGeoCountry>> GetCountriesPage(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<int> GetCountryCount(CancellationToken cancellationToken);
        Task<int> GetGeoZoneCount(Guid countryGuid, CancellationToken cancellationToken);
        Task<List<IGeoZone>> GetGeoZonePage(Guid countryGuid, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<List<IGeoZone>> GetGeoZonesByCountry(Guid countryGuid, CancellationToken cancellationToken);
        Task<List<IGeoCountry>> CountryAutoComplete(string query, int maxRows, CancellationToken cancellationToken);
        Task<List<IGeoZone>> StateAutoComplete(Guid countryGuid, string query, int maxRows, CancellationToken cancellationToken);
        Task<bool> Add(IGeoCountry geoCountry, CancellationToken cancellationToken);
        Task<bool> Update(IGeoCountry geoCountry, CancellationToken cancellationToken);
        Task<bool> Add(IGeoZone geoZone, CancellationToken cancellationToken);
        Task<bool> Update(IGeoZone geoZone, CancellationToken cancellationToken);

        Task<bool> Add(ILanguage language, CancellationToken cancellationToken);
        Task<bool> Update(ILanguage language, CancellationToken cancellationToken);
        Task<ILanguage> FetchLanguage(Guid guid, CancellationToken cancellationToken);
        Task<bool> DeleteLanguage(Guid guid, CancellationToken cancellationToken);
        Task<int> GetLanguageCount(CancellationToken cancellationToken);
        Task<List<ILanguage>> GetAllLanguages(CancellationToken cancellationToken);
        Task<List<ILanguage>> GetLanguagePage(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<bool> Add(ICurrency currency, CancellationToken cancellationToken);
        Task<bool> Update(ICurrency currency, CancellationToken cancellationToken);
        Task<ICurrency> FetchCurrency(Guid guid, CancellationToken cancellationToken);
        Task<bool> DeleteCurrency(Guid guid, CancellationToken cancellationToken);
        Task<List<ICurrency>> GetAllCurrencies(CancellationToken cancellationToken);
    }
}
