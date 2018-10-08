// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2018-10-08
// 

using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.Geography
{
    public interface IGeoCommands 
    {
        Task DeleteCountry(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteGeoZone(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteGeoZonesByCountry(Guid countryId, CancellationToken cancellationToken = default(CancellationToken));

        Task Add(IGeoCountry geoCountry, CancellationToken cancellationToken = default(CancellationToken));
        Task Update(IGeoCountry geoCountry, CancellationToken cancellationToken = default(CancellationToken));
        Task Add(IGeoZone geoZone, CancellationToken cancellationToken = default(CancellationToken));
        Task Update(IGeoZone geoZone, CancellationToken cancellationToken = default(CancellationToken));

    }

    // a marker interface so we can inject as singleton
    public interface IGeoCommandsSingleton : IGeoCommands
    {

    }
}
