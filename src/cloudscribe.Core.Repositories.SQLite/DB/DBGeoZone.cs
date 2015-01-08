// Author:					Joe Audette
// Created:				    2008-06-22
// Last Modified:			2015-01-08
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.SQLite;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.SQLite
{
    
    internal static class DBGeoZone
    {
        
        private static string GetConnectionString()
        {
            return ConnectionString.GetConnectionString();
        }


        /// <summary>
        /// Inserts a row in the mp_GeoZone table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="countryGuid"> countryGuid </param>
        /// <param name="name"> name </param>
        /// <param name="code"> code </param>
        /// <returns>bool</returns>
        public static bool Create(
            Guid guid,
            Guid countryGuid,
            string name,
            string code)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_GeoZone (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("CountryGuid, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Code )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":CountryGuid, ");
            sqlCommand.Append(":Name, ");
            sqlCommand.Append(":Code )");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[4];

            arParams[0] = new SQLiteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SQLiteParameter(":CountryGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = countryGuid.ToString();

            arParams[2] = new SQLiteParameter(":Name", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = name;

            arParams[3] = new SQLiteParameter(":Code", DbType.String, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = code;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return rowsAffected > 0;

        }


        /// <summary>
        /// Updates a row in the mp_GeoZone table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="countryGuid"> countryGuid </param>
        /// <param name="name"> name </param>
        /// <param name="code"> code </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid countryGuid,
            string name,
            string code)
        {
            
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_GeoZone ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("CountryGuid = :CountryGuid, ");
            sqlCommand.Append("Name = :Name, ");
            sqlCommand.Append("Code = :Code ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[4];

            arParams[0] = new SQLiteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SQLiteParameter(":CountryGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = countryGuid.ToString();

            arParams[2] = new SQLiteParameter(":Name", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = name;

            arParams[3] = new SQLiteParameter(":Code", DbType.String, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = code;


            int rowsAffected = AdoHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_GeoZone table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByCountry(Guid countryGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = :CountryGuid ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":CountryGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = countryGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_GeoZone table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return AdoHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_GeoZone table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByCode(Guid countryGuid, string code)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = :CountryGuid ");
            sqlCommand.Append("AND Code = :Code ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[2];

            arParams[0] = new SQLiteParameter(":CountryGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = countryGuid.ToString();

            arParams[1] = new SQLiteParameter(":Code", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = code;

            return AdoHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_GeoZone table.
        /// </summary>
        public static IDataReader GetByCountry(Guid countryGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = :CountryGuid ");
            sqlCommand.Append("ORDER BY Name ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":CountryGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = countryGuid.ToString();

            return AdoHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets a count of rows in the mp_GeoZone table.
        /// </summary>
        public static int GetCount(Guid countryGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_GeoZone ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CountryGuid = :CountryGuid ");
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[1];

            arParams[0] = new SQLiteParameter(":CountryGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = countryGuid.ToString();

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets a page of data from the mp_GeoZone table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public static IDataReader GetPage(
            Guid countryGuid,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCount(countryGuid);

            //if (pageSize > 0) totalPages = totalRows / pageSize;

            //if (totalRows <= pageSize)
            //{
            //    totalPages = 1;
            //}
            //else
            //{
            //    int remainder;
            //    Math.DivRem(totalRows, pageSize, out remainder);
            //    if (remainder > 0)
            //    {
            //        totalPages += 1;
            //    }
            //}

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	gz.*, ");
            sqlCommand.Append("gc.Name As CountryName ");
            sqlCommand.Append("FROM	mp_GeoZone gz ");
            sqlCommand.Append("JOIN mp_GeoCountry gc ");
            sqlCommand.Append("ON gz.CountryGuid = gc.Guid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("gz.CountryGuid = :CountryGuid ");
            sqlCommand.Append("ORDER BY gz.Name ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SQLiteParameter[] arParams = new SQLiteParameter[3];

            arParams[0] = new SQLiteParameter(":CountryGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = countryGuid.ToString();

            arParams[1] = new SQLiteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SQLiteParameter(":OffsetRows", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return AdoHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }
    }
}
