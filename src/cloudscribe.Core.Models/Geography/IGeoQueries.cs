// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2017-12-28
// 

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using cloudscribe.Pagination.Models;

namespace cloudscribe.Core.Models.Geography
{
    public interface IGeoQueries 
    {
        Task<IGeoCountry> FetchCountry(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        Task<IGeoCountry> FetchCountry(string isoCode2, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IGeoCountry>> GetAllCountries(CancellationToken cancellationToken = default(CancellationToken));
        Task<PagedResult<IGeoCountry>> GetCountriesPage(int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IGeoCountry>> CountryAutoComplete(string query, int maxRows, CancellationToken cancellationToken = default(CancellationToken));
        Task<int> GetCountryCount(CancellationToken cancellationToken = default(CancellationToken));

        Task<IGeoZone> FetchGeoZone(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        Task<int> GetGeoZoneCount(Guid countryId, CancellationToken cancellationToken = default(CancellationToken));
        Task<PagedResult<IGeoZone>> GetGeoZonePage(Guid countryId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IGeoZone>> GetGeoZonesByCountry(Guid countryId, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IGeoZone>> StateAutoComplete(Guid countryId, string query, int maxRows, CancellationToken cancellationToken = default(CancellationToken));

    }

    // a marker interface so we can inject as singleton
    public interface IGeoQueriesSingleton : IGeoQueries
    {

    }

}
