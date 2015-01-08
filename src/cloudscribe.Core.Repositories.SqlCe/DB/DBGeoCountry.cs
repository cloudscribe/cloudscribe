// Author:					Joe Audette
// Created:					2010-04-04
// Last Modified:			2014-08-29
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.SqlCe;
using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Globalization;
using System.Text;

namespace cloudscribe.Core.Repositories.SqlCe
{
    internal static class DBGeoCountry
    {
        private static String GetConnectionString()
        {
            return ConnectionString.GetConnectionString();
        }

        /// <summary>
        /// Inserts a row in the mp_GeoCountry table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="name"> name </param>
        /// <param name="iSOCode2"> iSOCode2 </param>
        /// <param name="iSOCode3"> iSOCode3 </param>
        /// <returns>bool</returns>
        public static bool Create(
            Guid guid,
            string name,
            string iSOCode2,
            string iSOCode3)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_GeoCountry ");
            sqlCommand.Append("(");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("ISOCode2, ");
            sqlCommand.Append("ISOCode3 ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@Name, ");
            sqlCommand.Append("@ISOCode2, ");
            sqlCommand.Append("@ISOCode3 ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = name;

            arParams[2] = new SqlCeParameter("@ISOCode2", SqlDbType.NChar, 2);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = iSOCode2;

            arParams[3] = new SqlCeParameter("@ISOCode3", SqlDbType.NChar, 3);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = iSOCode3;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected > 0;

        }

        /// <summary>
        /// Updates a row in the mp_GeoCountry table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="name"> name </param>
        /// <param name="iSOCode2"> iSOCode2 </param>
        /// <param name="iSOCode3"> iSOCode3 </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            string name,
            string iSOCode2,
            string iSOCode3)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_GeoCountry ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Name = @Name, ");
            sqlCommand.Append("ISOCode2 = @ISOCode2, ");
            sqlCommand.Append("ISOCode3 = @ISOCode3 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = name;

            arParams[2] = new SqlCeParameter("@ISOCode2", SqlDbType.NChar, 2);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = iSOCode2;

            arParams[3] = new SqlCeParameter("@ISOCode3", SqlDbType.NChar, 3);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = iSOCode3;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_GeoCountry table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GeoCountry ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_GeoCountry table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoCountry ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            return AdoHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_GeoCountry table.
        /// </summary>
        /// <param name="countryISOCode2"> countryISOCode2 </param>
        public static IDataReader GetByISOCode2(string countryISOCode2)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoCountry ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ISOCode2 = @ISOCode2 ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ISOCode2", SqlDbType.NChar, 2);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = countryISOCode2;

            return AdoHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        /// <summary>
        /// Gets a count of rows in the mp_GeoCountry table.
        /// </summary>
        public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_GeoCountry ");
            sqlCommand.Append(";");

            //SqlCeParameter[] arParams = new SqlCeParameter[1];

            //arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = applicationId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_GeoCountry table.
        /// </summary>
        public static IDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GeoCountry ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("[Name] ");
            sqlCommand.Append(";");

            //SqlCeParameter[] arParams = new SqlCeParameter[1];

            //arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = applicationId;

            return AdoHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }

        /// <summary>
        /// Gets a page of data from the mp_GeoCountry table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public static IDataReader GetPage(
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCount();

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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") * ");

            sqlCommand.Append("FROM	mp_GeoCountry  ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("[Name] ");

            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY t1.[Name] DESC  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            sqlCommand.Append("ORDER BY t2.[Name] ");
            sqlCommand.Append(";");


            //SqlCeParameter[] arParams = new SqlCeParameter[1];

            //arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = applicationId;

            return AdoHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }


    }
}
