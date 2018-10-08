// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2018-10-08
// 

using cloudscribe.Core.Models.Geography;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public class GeoCommands : IGeoCommands, IGeoCommandsSingleton
    {
        public GeoCommands(ICoreDbContextFactory coreDbContextFactory)
        {
            _contextFactory = coreDbContextFactory;
        }

        private readonly ICoreDbContextFactory _contextFactory;
        
        public async Task Add(
            IGeoCountry geoCountry, 
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (geoCountry == null) throw new ArgumentException("geoCountry must not be null");
            if (geoCountry.Id == Guid.Empty) throw new ArgumentException("geoCountry must have a non-empty id");

            var country = GeoCountry.FromIGeoCountry(geoCountry); // convert from IGeoCountry

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.Countries.Add(country);

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task Update(
            IGeoCountry geoCountry, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (geoCountry == null) throw new ArgumentException("geoCountry must not be null");
            if (geoCountry.Id == Guid.Empty) throw new ArgumentException("geoCountry must have a non-empty id");

            var country = GeoCountry.FromIGeoCountry(geoCountry); // convert from IGeoCountry

            using (var dbContext = _contextFactory.CreateContext())
            {
                bool tracking = dbContext.ChangeTracker.Entries<GeoCountry>().Any(x => x.Entity.Id == country.Id);
                if (!tracking)
                {
                    dbContext.Countries.Update(country);
                }

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task DeleteCountry(
            Guid countryId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (countryId == Guid.Empty) throw new ArgumentException("id must be a non-empty guid");

            using (var dbContext = _contextFactory.CreateContext())
            {
                var itemToRemove = await dbContext.Countries.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);
                if (itemToRemove == null) throw new InvalidOperationException("geoCountry not found");

                dbContext.Countries.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task Add(
            IGeoZone geoZone,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (geoZone == null) throw new ArgumentException("geoZone must not be null");
            if (geoZone.Id == Guid.Empty) throw new ArgumentException("geoZone must have a non-empty id");

            var state = GeoZone.FromIGeoZone(geoZone); // convert from IGeoZone

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.States.Add(state);

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task Update(
            IGeoZone geoZone,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (geoZone == null) throw new ArgumentException("geoZone must not be null");
            if (geoZone.Id == Guid.Empty) throw new ArgumentException("geoZone must have a non-empty id");
            if (geoZone.CountryId == Guid.Empty) throw new ArgumentException("geoZone must have a non-empty CountryId");

            var state = GeoZone.FromIGeoZone(geoZone); // convert from IGeoZone

            using (var dbContext = _contextFactory.CreateContext())
            {
                bool tracking = dbContext.ChangeTracker.Entries<GeoZone>().Any(x => x.Entity.Id == state.Id);
                if (!tracking)
                {
                    dbContext.States.Update(state);
                }

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task DeleteGeoZone(
            Guid stateId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (stateId == Guid.Empty) throw new ArgumentException("id must be a non-empty guid");

            using (var dbContext = _contextFactory.CreateContext())
            {
                var itemToRemove = await dbContext.States.SingleOrDefaultAsync(x => x.Id == stateId, cancellationToken);
                if (itemToRemove == null) throw new InvalidOperationException("geoZone not found");

                dbContext.States.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public async Task DeleteGeoZonesByCountry(
            Guid countryId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (countryId == Guid.Empty) throw new ArgumentException("countryId must be a non-empty guid");

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from l in dbContext.States
                            where l.CountryId == countryId
                            select l;

                dbContext.States.RemoveRange(query);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

    }
}
