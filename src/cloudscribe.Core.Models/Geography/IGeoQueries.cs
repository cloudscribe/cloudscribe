// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2016-05-12
// 

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.Geography
{
    public interface IGeoQueries : IDisposable
    {
        Task<IGeoCountry> FetchCountry(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        Task<IGeoCountry> FetchCountry(string isoCode2, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IGeoCountry>> GetAllCountries(CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IGeoCountry>> GetCountriesPage(int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IGeoCountry>> CountryAutoComplete(string query, int maxRows, CancellationToken cancellationToken = default(CancellationToken));
        Task<int> GetCountryCount(CancellationToken cancellationToken = default(CancellationToken));

        Task<IGeoZone> FetchGeoZone(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        Task<int> GetGeoZoneCount(Guid countryId, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IGeoZone>> GetGeoZonePage(Guid countryId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IGeoZone>> GetGeoZonesByCountry(Guid countryId, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IGeoZone>> StateAutoComplete(Guid countryId, string query, int maxRows, CancellationToken cancellationToken = default(CancellationToken));

    }
}
