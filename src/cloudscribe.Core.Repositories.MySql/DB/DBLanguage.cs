// Author:					Joe Audette
// Created:				    2008-06-22
// Last Modified:			2015-01-08
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.MySql;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MySql
{
    internal static class DBLanguage
    {
       
        /// <summary>
        /// Inserts a row in the mp_Language table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="name"> name </param>
        /// <param name="code"> code </param>
        /// <param name="sort"> sort </param>
        /// <returns>int</returns>
        public static async Task<bool> Create(
            Guid guid,
            string name,
            string code,
            int sort)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Language (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Code, ");
            sqlCommand.Append("Sort )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?Name, ");
            sqlCommand.Append("?Code, ");
            sqlCommand.Append("?Sort )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = name;

            arParams[2] = new MySqlParameter("?Code", MySqlDbType.VarChar, 2);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = code;

            arParams[3] = new MySqlParameter("?Sort", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sort;

            int rowsAffected = await  AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected > 0;

        }


        /// <summary>
        /// Updates a row in the mp_Language table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="name"> name </param>
        /// <param name="code"> code </param>
        /// <param name="sort"> sort </param>
        /// <returns>bool</returns>
        public static async Task<bool> Update(
            Guid guid,
            string name,
            string code,
            int sort)
        {
            

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Language ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Name = ?Name, ");
            sqlCommand.Append("Code = ?Code, ");
            sqlCommand.Append("Sort = ?Sort ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = name;

            arParams[2] = new MySqlParameter("?Code", MySqlDbType.VarChar, 2);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = code;

            arParams[3] = new MySqlParameter("?Sort", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sort;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_Language table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static async Task<bool> Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Language ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Language table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static async Task<DbDataReader> GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Language ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Language table.
        /// </summary>
        public static async Task<DbDataReader> GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Language ");
            sqlCommand.Append("ORDER BY Sort  ");
            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null);
        }

        /// <summary>
        /// Gets a count of rows in the mp_Language table.
        /// </summary>
        public static async Task<int> GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Language ");
            sqlCommand.Append(";");

            object result = await AdoHelper.ExecuteScalarAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null);

            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Gets a page of data from the mp_Language table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static async Task<DbDataReader> GetPage(
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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_Language  ");
            //sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ORDER BY Sort  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageSize;

            arParams[1] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageLowerBound;

            return await AdoHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }
    }
}
