// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2015-10-16
// 

using cloudscribe.Core.Models.Geography;
using cloudscribe.DbHelpers.MySql;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MySql
{
    public sealed class GeoRepository : IGeoRepository
    {
        public GeoRepository(
            IOptions<MySqlConnectionOptionscs> configuration,
            ILoggerFactory loggerFactory)
        {
            if(configuration == null) { throw new ArgumentNullException(nameof(configuration)); }
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }

            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(typeof(GeoRepository).FullName);

            readConnectionString = configuration.Value.ReadConnectionString;
            writeConnectionString = configuration.Value.WriteConnectionString;

            dbGeoCountry = new DBGeoCountry(readConnectionString, writeConnectionString, logFactory);
            dbGeoZone = new DBGeoZone(readConnectionString, writeConnectionString, logFactory);
            dbLanguage = new DBLanguage(readConnectionString, writeConnectionString, logFactory);
            dbCurrency = new DBCurrency(readConnectionString, writeConnectionString, logFactory);
        }

        private ILoggerFactory logFactory;
        private ILogger log;
        private string readConnectionString;
        private string writeConnectionString;
        private DBGeoCountry dbGeoCountry;
        private DBGeoZone dbGeoZone;
        private DBLanguage dbLanguage;
        private DBCurrency dbCurrency;

        /// <summary>
        /// Persists a new instance of GeoCountry.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Save(IGeoCountry geoCountry)
        {
            if (geoCountry == null) { return false; }
            bool result;
            if (geoCountry.Guid == Guid.Empty)
            {
                geoCountry.Guid = Guid.NewGuid();

                result = await dbGeoCountry.Create(
                    geoCountry.Guid,
                    geoCountry.Name,
                    geoCountry.ISOCode2,
                    geoCountry.ISOCode3);
            }
            else
            {
                result = await dbGeoCountry.Update(
                    geoCountry.Guid,
                    geoCountry.Name,
                    geoCountry.ISOCode2,
                    geoCountry.ISOCode3);

            }

            return result;
        }


        /// <param name="guid"> guid </param>
        public async Task<IGeoCountry> FetchCountry(Guid guid)
        {
            using (DbDataReader reader = await dbGeoCountry.GetOne(guid))
            {
                if (reader.Read())
                {
                    GeoCountry geoCountry = new GeoCountry();
                    LoadFromReader(reader, geoCountry);
                    return geoCountry;

                }
            }

            return null;
        }

        public async Task<IGeoCountry> FetchCountry(string isoCode2)
        {
            using (DbDataReader reader = await dbGeoCountry.GetByISOCode2(isoCode2))
            {
                if (reader.Read())
                {
                    GeoCountry geoCountry = new GeoCountry();
                    LoadFromReader(reader, geoCountry);
                    return geoCountry;

                }
            }

            return null;
        }


        /// <summary>
        /// Deletes an instance of GeoCountry. Returns true on success.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteCountry(Guid guid)
        {
            return await dbGeoCountry.Delete(guid);
        }


        /// <summary>
        /// Gets a count of GeoCountry. 
        /// </summary>
        public async Task<int> GetCountryCount()
        {
            return await dbGeoCountry.GetCount();
        }


        /// <summary>
        /// Gets an IList with all instances of GeoCountry.
        /// </summary>
        public async Task<List<IGeoCountry>> GetAllCountries()
        {
            DbDataReader reader = await dbGeoCountry.GetAll();
            return LoadCountryListFromReader(reader);

        }

        /// <summary>
        /// Gets an IList with page of instances of GeoCountry.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public async Task<List<IGeoCountry>> GetCountriesPage(int pageNumber, int pageSize)
        {
            DbDataReader reader = await dbGeoCountry.GetPage(pageNumber, pageSize);
            return LoadCountryListFromReader(reader);
        }



        /// <summary>
        /// Persists a new instance of GeoZone.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Save(IGeoZone geoZone)
        {
            if (geoZone == null) { return false; }
            bool result;
            if (geoZone.Guid == Guid.Empty)
            {
                geoZone.Guid = Guid.NewGuid();

                result = await dbGeoZone.Create(
                    geoZone.Guid,
                    geoZone.CountryGuid,
                    geoZone.Name,
                    geoZone.Code);
            }
            else
            {
                result = await dbGeoZone.Update(
                    geoZone.Guid,
                    geoZone.CountryGuid,
                    geoZone.Name,
                    geoZone.Code);

            }
            return result;
        }


        /// <param name="guid"> guid </param>
        public async Task<IGeoZone> FetchGeoZone(Guid guid)
        {
            using (DbDataReader reader = await dbGeoZone.GetOne(guid))
            {
                if (reader.Read())
                {
                    GeoZone geoZone = new GeoZone();
                    geoZone.Guid = new Guid(reader["Guid"].ToString());
                    geoZone.CountryGuid = new Guid(reader["CountryGuid"].ToString());
                    geoZone.Name = reader["Name"].ToString();
                    geoZone.Code = reader["Code"].ToString();

                    return geoZone;

                }
            }

            return null;
        }


        /// <summary>
        /// Deletes an instance of GeoZone. Returns true on success.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteGeoZone(Guid guid)
        {
            return await dbGeoZone.Delete(guid);
        }

        public async Task<bool> DeleteGeoZonesByCountry(Guid countryGuid)
        {
            return await dbGeoZone.DeleteByCountry(countryGuid);
        }

        /// <summary>
        /// Gets a count of GeoZone. 
        /// </summary>
        public async Task<int> GetGeoZoneCount(Guid countryGuid)
        {
            return await dbGeoZone.GetCount(countryGuid);
        }


        /// <summary>
        /// Gets an IList with all instances of GeoZone.
        /// </summary>
        public async Task<List<IGeoZone>> GetGeoZonesByCountry(Guid countryGuid)
        {
            DbDataReader reader = await dbGeoZone.GetByCountry(countryGuid);
            return LoadGeoZoneListFromReader(reader);

        }

        public async Task<List<IGeoCountry>> CountryAutoComplete(string query, int maxRows)
        {
            DbDataReader reader = await dbGeoCountry.AutoComplete(query, maxRows);
            return LoadCountryListFromReader(reader);
        }

        public async Task<List<IGeoZone>> StateAutoComplete(Guid countryGuid, string query, int maxRows)
        {
            DbDataReader reader = await dbGeoZone.AutoComplete(countryGuid, query, maxRows);
            return LoadGeoZoneListFromReader(reader);
        }

        /// <summary>
        /// Gets an IList with page of instances of GeoZone.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public async Task<List<IGeoZone>> GetGeoZonePage(Guid countryGuid, int pageNumber, int pageSize)
        {
            DbDataReader reader = await dbGeoZone.GetPage(countryGuid, pageNumber, pageSize);
            return LoadGeoZoneListFromReader(reader);
        }


        private List<IGeoZone> LoadGeoZoneListFromReader(DbDataReader reader)
        {
            List<IGeoZone> geoZoneList = new List<IGeoZone>();

            using(reader)
            {
                while (reader.Read())
                {
                    GeoZone geoZone = new GeoZone();
                    LoadFromReader(reader, geoZone);
                    geoZoneList.Add(geoZone);

                }
            }
           
            return geoZoneList;

        }

        private void LoadFromReader(DbDataReader reader, IGeoZone geoZone)
        {
            geoZone.Guid = new Guid(reader["Guid"].ToString());
            geoZone.CountryGuid = new Guid(reader["CountryGuid"].ToString());
            geoZone.Name = reader["Name"].ToString();
            geoZone.Code = reader["Code"].ToString();
        }


        private void LoadFromReader(DbDataReader reader, IGeoCountry geoCountry)
        {
            geoCountry.Guid = new Guid(reader["Guid"].ToString());
            geoCountry.Name = reader["Name"].ToString();
            geoCountry.ISOCode2 = reader["ISOCode2"].ToString();
            geoCountry.ISOCode3 = reader["ISOCode3"].ToString();
        }

        private List<IGeoCountry> LoadCountryListFromReader(DbDataReader reader)
        {
            List<IGeoCountry> geoCountryList = new List<IGeoCountry>();

            using(reader)
            {
                while (reader.Read())
                {
                    GeoCountry geoCountry = new GeoCountry();
                    LoadFromReader(reader, geoCountry);
                    geoCountryList.Add(geoCountry);

                }
            }
            
            return geoCountryList;

        }


        public async Task<bool> Save(ILanguage language)
        {
            if (language == null) { return false; }
            bool result;
            if (language.Guid == Guid.Empty)
            {
                language.Guid = Guid.NewGuid();

                result = await dbLanguage.Create(
                    language.Guid,
                    language.Name,
                    language.Code,
                    language.Sort);
            }
            else
            {
                result = await dbLanguage.Update(
                    language.Guid,
                    language.Name,
                    language.Code,
                    language.Sort);

            }
            return result;
        }

        public async Task<ILanguage> FetchLanguage(Guid guid)
        {
            using (DbDataReader reader = await dbLanguage.GetOne(guid))
            {
                if (reader.Read())
                {
                    Language language = new Language();
                    LoadFromReader(reader, language);
                    return language;

                }
            }

            return null;
        }

        /// <summary>
        /// Deletes an instance of Language. Returns true on success.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteLanguage(Guid guid)
        {
            return await dbLanguage.Delete(guid);
        }

        public async Task<int> GetLanguageCount()
        {
            return await dbLanguage.GetCount();
        }

        public async Task<List<ILanguage>> GetAllLanguages()
        {
            DbDataReader reader = await dbLanguage.GetAll();
            return LoadLanguageListFromReader(reader);

        }

        public async Task<List<ILanguage>> GetLanguagePage(int pageNumber, int pageSize)
        {
            DbDataReader reader = await dbLanguage.GetPage(pageNumber, pageSize);
            return LoadLanguageListFromReader(reader);
        }

        private void LoadFromReader(DbDataReader reader, ILanguage language)
        {
            language.Guid = new Guid(reader["Guid"].ToString());
            language.Name = reader["Name"].ToString();
            language.Code = reader["Code"].ToString();
            language.Sort = Convert.ToInt32(reader["Sort"]);
        }

        private List<ILanguage> LoadLanguageListFromReader(DbDataReader reader)
        {
            List<ILanguage> languageList = new List<ILanguage>();

            using(reader)
            {
                while (reader.Read())
                {
                    Language language = new Language();
                    LoadFromReader(reader, language);
                    languageList.Add(language);

                }
            }
            
            return languageList;

        }

        public async Task<bool> Save(ICurrency currency)
        {
            if (currency == null) { return false; }
            bool result;
            if (currency.Guid == Guid.Empty)
            {
                currency.Guid = Guid.NewGuid();

                result = await dbCurrency.Create(
                    currency.Guid,
                    currency.Title,
                    currency.Code,
                    currency.SymbolLeft,
                    currency.SymbolRight,
                    currency.DecimalPointChar,
                    currency.ThousandsPointChar,
                    currency.DecimalPlaces,
                    currency.Value,
                    currency.LastModified,
                    currency.Created);
            }
            else
            {
                result = await dbCurrency.Update(
                    currency.Guid,
                    currency.Title,
                    currency.Code,
                    currency.SymbolLeft,
                    currency.SymbolRight,
                    currency.DecimalPointChar,
                    currency.ThousandsPointChar,
                    currency.DecimalPlaces,
                    currency.Value,
                    currency.LastModified);

            }
            return result;
        }

        public async Task<ICurrency> FetchCurrency(Guid guid)
        {
            using (DbDataReader reader = await dbCurrency.GetOne(guid))
            {
                if (reader.Read())
                {
                    Currency currency = new Currency();
                    LoadFromReader(reader, currency);
                    return currency;

                }
            }

            return null;
        }

        public async Task<bool> DeleteCurrency(Guid guid)
        {
            return await dbCurrency.Delete(guid);
        }

        public async Task<List<ICurrency>> GetAllCurrencies()
        {
            DbDataReader reader = await dbCurrency.GetAll();
            return LoadCurrencyListFromReader(reader);

        }

        private void LoadFromReader(DbDataReader reader, ICurrency currency)
        {
            currency.Guid = new Guid(reader["Guid"].ToString());
            currency.Title = reader["Title"].ToString();
            currency.Code = reader["Code"].ToString();
            currency.SymbolLeft = reader["SymbolLeft"].ToString();
            currency.SymbolRight = reader["SymbolRight"].ToString();
            currency.DecimalPointChar = reader["DecimalPointChar"].ToString();
            currency.ThousandsPointChar = reader["ThousandsPointChar"].ToString();
            currency.DecimalPlaces = reader["DecimalPlaces"].ToString();
            currency.Value = Convert.ToDecimal(reader["Value"]);
            currency.LastModified = Convert.ToDateTime(reader["LastModified"]);
            currency.Created = Convert.ToDateTime(reader["Created"]);
        }

        private List<ICurrency> LoadCurrencyListFromReader(DbDataReader reader)
        {
            List<ICurrency> currencyList = new List<ICurrency>();

            using(reader)
            {
                while (reader.Read())
                {
                    Currency currency = new Currency();
                    LoadFromReader(reader, currency);
                    currencyList.Add(currency);

                }
            }
            
            return currencyList;

        }

    }
}
