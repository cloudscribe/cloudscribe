// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2014-11-03
// 


using cloudscribe.Core.Models.Geography;
using System;
using System.Collections.Generic;
using System.Data;

namespace cloudscribe.Core.Repositories.SQLite
{
    public class GeoRepository : IGeoRepository
    {
        /// <summary>
        /// Persists a new instance of GeoCountry.
        /// </summary>
        /// <returns></returns>
        public void Save(IGeoCountry geoCountry)
        {
            if (geoCountry == null) { return; }

            if (geoCountry.Guid == Guid.Empty)
            {
                geoCountry.Guid = Guid.NewGuid();

                DBGeoCountry.Create(
                    geoCountry.Guid,
                    geoCountry.Name,
                    geoCountry.ISOCode2,
                    geoCountry.ISOCode3);
            }
            else
            {
                DBGeoCountry.Update(
                    geoCountry.Guid,
                    geoCountry.Name,
                    geoCountry.ISOCode2,
                    geoCountry.ISOCode3);

            }
        }


        /// <param name="guid"> guid </param>
        public IGeoCountry FetchCountry(Guid guid)
        {
            using (IDataReader reader = DBGeoCountry.GetOne(guid))
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
        public bool DeleteCountry(Guid guid)
        {
            return DBGeoCountry.Delete(guid);
        }


        /// <summary>
        /// Gets a count of GeoCountry. 
        /// </summary>
        public int GetCountryCount()
        {
            return DBGeoCountry.GetCount();
        }


        /// <summary>
        /// Gets an IList with all instances of GeoCountry.
        /// </summary>
        public List<IGeoCountry> GetAllCountries()
        {
            IDataReader reader = DBGeoCountry.GetAll();
            return LoadCountryListFromReader(reader);

        }

        /// <summary>
        /// Gets an IList with page of instances of GeoCountry.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public List<IGeoCountry> GetCountriesPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBGeoCountry.GetPage(pageNumber, pageSize, out totalPages);
            return LoadCountryListFromReader(reader);
        }



        /// <summary>
        /// Persists a new instance of GeoZone.
        /// </summary>
        /// <returns></returns>
        public void Save(IGeoZone geoZone)
        {
            if (geoZone == null) { return; }

            if (geoZone.Guid == Guid.Empty)
            {
                geoZone.Guid = Guid.NewGuid();

                DBGeoZone.Create(
                    geoZone.Guid,
                    geoZone.CountryGuid,
                    geoZone.Name,
                    geoZone.Code);
            }
            else
            {
                DBGeoZone.Update(
                    geoZone.Guid,
                    geoZone.CountryGuid,
                    geoZone.Name,
                    geoZone.Code);

            }
        }


        /// <param name="guid"> guid </param>
        public IGeoZone FetchGeoZone(Guid guid)
        {
            using (IDataReader reader = DBGeoZone.GetOne(guid))
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
        public bool DeleteGeoZone(Guid guid)
        {
            return DBGeoZone.Delete(guid);
        }


        /// <summary>
        /// Gets a count of GeoZone. 
        /// </summary>
        public int GetGeoZoneCount(Guid countryGuid)
        {
            return DBGeoZone.GetCount(countryGuid);
        }


        /// <summary>
        /// Gets an IList with all instances of GeoZone.
        /// </summary>
        public List<IGeoZone> GetGeoZonesByCountry(Guid countryGuid)
        {
            IDataReader reader = DBGeoZone.GetByCountry(countryGuid);
            return LoadGeoZoneListFromReader(reader);

        }

        /// <summary>
        /// Gets an IList with page of instances of GeoZone.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public List<IGeoZone> GetGeoZonePage(Guid countryGuid, int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBGeoZone.GetPage(countryGuid, pageNumber, pageSize, out totalPages);
            return LoadGeoZoneListFromReader(reader);
        }


        private List<IGeoZone> LoadGeoZoneListFromReader(IDataReader reader)
        {
            List<IGeoZone> geoZoneList = new List<IGeoZone>();

            try
            {
                while (reader.Read())
                {
                    GeoZone geoZone = new GeoZone();
                    LoadFromReader(reader, geoZone);
                    geoZoneList.Add(geoZone);

                }
            }
            finally
            {
                reader.Close();
            }

            return geoZoneList;

        }

        private void LoadFromReader(IDataReader reader, IGeoZone geoZone)
        {
            geoZone.Guid = new Guid(reader["Guid"].ToString());
            geoZone.CountryGuid = new Guid(reader["CountryGuid"].ToString());
            geoZone.Name = reader["Name"].ToString();
            geoZone.Code = reader["Code"].ToString();
        }


        private void LoadFromReader(IDataReader reader, IGeoCountry geoCountry)
        {
            geoCountry.Guid = new Guid(reader["Guid"].ToString());
            geoCountry.Name = reader["Name"].ToString();
            geoCountry.ISOCode2 = reader["ISOCode2"].ToString();
            geoCountry.ISOCode3 = reader["ISOCode3"].ToString();
        }

        private List<IGeoCountry> LoadCountryListFromReader(IDataReader reader)
        {
            List<IGeoCountry> geoCountryList = new List<IGeoCountry>();

            try
            {
                while (reader.Read())
                {
                    GeoCountry geoCountry = new GeoCountry();
                    LoadFromReader(reader, geoCountry);
                    geoCountryList.Add(geoCountry);

                }
            }
            finally
            {
                reader.Close();
            }

            return geoCountryList;

        }


        public void Save(ILanguage language)
        {
            if (language == null) { return; }

            if (language.Guid == Guid.Empty)
            {
                language.Guid = Guid.NewGuid();

                DBLanguage.Create(
                    language.Guid,
                    language.Name,
                    language.Code,
                    language.Sort);
            }
            else
            {
                DBLanguage.Update(
                    language.Guid,
                    language.Name,
                    language.Code,
                    language.Sort);

            }
        }

        public ILanguage FetchLanguage(Guid guid)
        {
            using (IDataReader reader = DBLanguage.GetOne(guid))
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
        public bool DeleteLanguage(Guid guid)
        {
            return DBLanguage.Delete(guid);
        }

        public int GetLanguageCount()
        {
            return DBLanguage.GetCount();
        }

        public List<ILanguage> GetAllLanguages()
        {
            IDataReader reader = DBLanguage.GetAll();
            return LoadLanguageListFromReader(reader);

        }

        public List<ILanguage> GetLanguagePage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBLanguage.GetPage(pageNumber, pageSize, out totalPages);
            return LoadLanguageListFromReader(reader);
        }

        private void LoadFromReader(IDataReader reader, ILanguage language)
        {
            language.Guid = new Guid(reader["Guid"].ToString());
            language.Name = reader["Name"].ToString();
            language.Code = reader["Code"].ToString();
            language.Sort = Convert.ToInt32(reader["Sort"]);
        }

        private List<ILanguage> LoadLanguageListFromReader(IDataReader reader)
        {
            List<ILanguage> languageList = new List<ILanguage>();

            try
            {
                while (reader.Read())
                {
                    Language language = new Language();
                    LoadFromReader(reader, language);
                    languageList.Add(language);

                }
            }
            finally
            {
                reader.Close();
            }

            return languageList;

        }

        public void Save(ICurrency currency)
        {
            if (currency == null) { return; }

            if (currency.Guid == Guid.Empty)
            {
                currency.Guid = Guid.NewGuid();

                DBCurrency.Create(
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
                DBCurrency.Update(
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
        }

        public ICurrency FetchCurrency(Guid guid)
        {
            using (IDataReader reader = DBCurrency.GetOne(guid))
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

        public bool DeleteCurrency(Guid guid)
        {
            return DBCurrency.Delete(guid);
        }

        public List<ICurrency> GetAllCurrencies()
        {
            IDataReader reader = DBCurrency.GetAll();
            return LoadCurrencyListFromReader(reader);

        }

        private void LoadFromReader(IDataReader reader, ICurrency currency)
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

        private List<ICurrency> LoadCurrencyListFromReader(IDataReader reader)
        {
            List<ICurrency> currencyList = new List<ICurrency>();

            try
            {
                while (reader.Read())
                {
                    Currency currency = new Currency();
                    LoadFromReader(reader, currency);
                    currencyList.Add(currency);

                }
            }
            finally
            {
                reader.Close();
            }

            return currencyList;

        }

    }
}
