// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2016-05-09
// 

using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.Geography
{
    public interface IGeoCommands : IDisposable
    {
        Task DeleteCountry(Guid guid, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteGeoZone(Guid guid, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteGeoZonesByCountry(Guid countryGuid, CancellationToken cancellationToken = default(CancellationToken));

        Task Add(IGeoCountry geoCountry, CancellationToken cancellationToken = default(CancellationToken));
        Task Update(IGeoCountry geoCountry, CancellationToken cancellationToken = default(CancellationToken));
        Task Add(IGeoZone geoZone, CancellationToken cancellationToken = default(CancellationToken));
        Task Update(IGeoZone geoZone, CancellationToken cancellationToken = default(CancellationToken));

        Task Add(ILanguage language, CancellationToken cancellationToken = default(CancellationToken));
        Task Update(ILanguage language, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteLanguage(Guid guid, CancellationToken cancellationToken = default(CancellationToken));

        Task Add(ICurrency currency, CancellationToken cancellationToken = default(CancellationToken));
        Task Update(ICurrency currency, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteCurrency(Guid guid, CancellationToken cancellationToken = default(CancellationToken));
    }
}
