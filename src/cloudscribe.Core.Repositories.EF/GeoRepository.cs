// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2015-11-22
// 

using cloudscribe.Core.Models.Geography;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.EF
{
    public class GeoRepository : IGeoRepository
    {

        public GeoRepository(CoreDbContext dbContext)
        {
       
            this.dbContext = dbContext;
        }

        private CoreDbContext dbContext;

        public async Task<bool> Save(IGeoCountry geoCountry)
        {
            if (geoCountry == null) { return false; }
            
            GeoCountry country = GeoCountry.FromIGeoCountry(geoCountry); // convert from IGeoCountry
            dbContext.Countries.Add(country);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<IGeoCountry> FetchCountry(Guid guid)
        {
            GeoCountry item 
                = await dbContext.Countries.FirstOrDefaultAsync(x => x.Guid == guid);

            return item;
        }

        public async Task<IGeoCountry> FetchCountry(string isoCode2)
        {
            return await dbContext.Countries.FirstOrDefaultAsync(x => x.ISOCode2 == isoCode2);
        }

        public async Task<bool> DeleteCountry(Guid guid)
        {
            var result = false;
            var itemToRemove = await dbContext.Countries.FirstOrDefaultAsync(x => x.Guid == guid);
            if (itemToRemove != null)
            {
                dbContext.Countries.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;

        }

        public async Task<int> GetCountryCount()
        {
            return await dbContext.Countries.CountAsync<GeoCountry>();
        }

        public async Task<List<IGeoCountry>> GetAllCountries()
        {
            var query = from c in dbContext.Countries
                        orderby c.Name ascending
                        select c;

            var items = await query.ToAsyncEnumerable<GeoCountry>().ToList<GeoCountry>();
            
            List<IGeoCountry> result = new List<IGeoCountry>(items); // will this work?

            return result;

        }

        public async Task<List<IGeoCountry>> GetCountriesPage(int pageNumber, int pageSize)
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var query = from l in dbContext.Countries
                        .Skip(offset)
                        .Take(pageSize)
                        orderby l.Name ascending
                        select l;

            var items = await query.ToAsyncEnumerable<GeoCountry>().ToList<GeoCountry>();
            
            List<IGeoCountry> result = new List<IGeoCountry>(items); // will this work?

            return result;
        }

        public async Task<bool> Save(IGeoZone geoZone)
        {
            if (geoZone == null) { return false; }

            GeoZone state = GeoZone.FromIGeoZone(geoZone); // convert from IGeoZone
            dbContext.States.Add(state);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public async Task<IGeoZone> FetchGeoZone(Guid guid)
        {
            GeoZone item
                = await dbContext.States.FirstOrDefaultAsync(x => x.Guid == guid);

            return item;
        }

        public async Task<bool> DeleteGeoZone(Guid guid)
        {
            var result = false;
            var itemToRemove = await dbContext.States.FirstOrDefaultAsync(x => x.Guid == guid);
            if (itemToRemove != null)
            {
                dbContext.States.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;
        }

        public async Task<bool> DeleteGeoZonesByCountry(Guid countryGuid)
        {
            var query = from l in dbContext.States
                        where l.CountryGuid == countryGuid
                        select l;

            dbContext.States.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<int> GetGeoZoneCount(Guid countryGuid)
        {
            return await dbContext.States.CountAsync<GeoZone>(
                g => g.CountryGuid == countryGuid);
        }

        public async Task<List<IGeoZone>> GetGeoZonesByCountry(Guid countryGuid)
        {
            var query = from l in dbContext.States
                        where l.CountryGuid == countryGuid
                        orderby l.Name descending
                        select l;

            var items = await query.ToAsyncEnumerable<GeoZone>().ToList<GeoZone>();
            
            List<IGeoZone> result = new List<IGeoZone>(items); // will this work?

            return result;

        }

        public async Task<List<IGeoCountry>> CountryAutoComplete(string query, int maxRows)
        {
            // approximation of a LIKE operator query
            //http://stackoverflow.com/questions/17097764/linq-to-entities-using-the-sql-like-operator

            var listQuery = from l in dbContext.Countries
                            .Take(maxRows)
                            where l.Name.Contains(query) || l.ISOCode2.Contains(query)
                            orderby l.Name ascending
                            select l;

            var items = await listQuery.ToAsyncEnumerable<GeoCountry>().ToList<GeoCountry>();

            List<IGeoCountry> result = new List<IGeoCountry>(items); // will this work?

            return result;
        }

        public async Task<List<IGeoZone>> StateAutoComplete(Guid countryGuid, string query, int maxRows)
        {
            var listQuery = from l in dbContext.States
                            .Take(maxRows)
                            where (
                            l.CountryGuid == countryGuid &&
                            (l.Name.Contains(query) || l.Code.Contains(query))
                            )
                            orderby l.Code ascending
                            select l;

            var items = await listQuery.ToAsyncEnumerable<GeoZone>().ToList<GeoZone>();

            List<IGeoZone> result = new List<IGeoZone>(items); // will this work?

            return result;
        }

        public async Task<List<IGeoZone>> GetGeoZonePage(Guid countryGuid, int pageNumber, int pageSize)
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var query = from l in dbContext.States
                        .Skip(offset)
                        .Take(pageSize)
                        where l.CountryGuid == countryGuid
                        orderby l.Name ascending
                        select l;

            var items = await query.ToAsyncEnumerable<GeoZone>().ToList<GeoZone>();

            List<IGeoZone> result = new List<IGeoZone>(items); // will this work?

            return result;

        }

        public async Task<bool> Save(ILanguage language)
        {
            if (language == null) { return false; }

            Language lang = Language.FromILanguage(language); 
            dbContext.Languages.Add(lang);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }

        public async Task<ILanguage> FetchLanguage(Guid guid)
        {
            Language item
                = await dbContext.Languages.FirstOrDefaultAsync(x => x.Guid == guid);

            return item;
        }

        public async Task<bool> DeleteLanguage(Guid guid)
        {
            var result = false;
            var itemToRemove = await dbContext.Languages.FirstOrDefaultAsync(x => x.Guid == guid);
            if (itemToRemove != null)
            {
                dbContext.Languages.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;
        }

        public async Task<int> GetLanguageCount()
        {
            return await dbContext.Languages.CountAsync<Language>();

        }

        public async Task<List<ILanguage>> GetAllLanguages()
        {
            var query = from c in dbContext.Languages
                        orderby c.Name ascending
                        select c;

            var items = await query.ToAsyncEnumerable<Language>().ToList<Language>();

            List<ILanguage> result = new List<ILanguage>(items); 

            return result;

        }

        public async Task<List<ILanguage>> GetLanguagePage(int pageNumber, int pageSize)
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var query = from l in dbContext.Languages
                        .Skip(offset)
                        .Take(pageSize)
                        orderby l.Name ascending
                        select l;

            var items = await query.ToAsyncEnumerable<Language>().ToList<Language>();

            List<ILanguage> result = new List<ILanguage>(items); 

            return result;

        }


        public async Task<bool> Save(ICurrency currency)
        {
            if (currency == null) { return false; }

            Currency c = Currency.FromICurrency(currency);
            dbContext.Currencies.Add(c);
            int rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0;

        }


        public async Task<ICurrency> FetchCurrency(Guid guid)
        {
            Currency item
                = await dbContext.Currencies.FirstOrDefaultAsync(x => x.Guid == guid);

            return item;
        }

        public async Task<bool> DeleteCurrency(Guid guid)
        {
            var result = false;
            var itemToRemove = await dbContext.Currencies.FirstOrDefaultAsync(x => x.Guid == guid);
            if (itemToRemove != null)
            {
                dbContext.Currencies.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;

        }

        public async Task<List<ICurrency>> GetAllCurrencies()
        {
            var query = from c in dbContext.Currencies
                        orderby c.Title ascending
                        select c;

            var items = await query.ToAsyncEnumerable<Currency>().ToList<Currency>();

            List<ICurrency> result = new List<ICurrency>(items);

            return result;

        }


    }
}
