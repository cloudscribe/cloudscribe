// Author:					Joe Audette
// Created:				    2008-11-19
// Last Modified:			2015-06-23
// 
// You must not remove this notice, or any other, from this software.


using cloudscribe.DbHelpers.pgsql;
using Microsoft.Framework.Logging;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace cloudscribe.Core.Repositories.pgsql
{

    internal class DBRedirectList
    {

        internal DBRedirectList(
            string dbReadConnectionString,
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;
        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string readConnectionString;
        private string writeConnectionString;

        /// <summary>
        /// Inserts a row in the mp_RedirectList table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="siteID"> siteID </param>
        /// <param name="oldUrl"> oldUrl </param>
        /// <param name="newUrl"> newUrl </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="expireUtc"> expireUtc </param>
        /// <returns>int</returns>
        public int Create(
            Guid rowGuid,
            Guid siteGuid,
            int siteID,
            string oldUrl,
            string newUrl,
            DateTime createdUtc,
            DateTime expireUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_redirectlist (");
            sqlCommand.Append("rowguid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("siteid, ");
            sqlCommand.Append("oldurl, ");
            sqlCommand.Append("newurl, ");
            sqlCommand.Append("createdutc, ");
            sqlCommand.Append("expireutc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":rowguid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":siteid, ");
            sqlCommand.Append(":oldurl, ");
            sqlCommand.Append(":newurl, ");
            sqlCommand.Append(":createdutc, ");
            sqlCommand.Append(":expireutc ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[7];

            arParams[0] = new NpgsqlParameter("rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = siteID;

            arParams[3] = new NpgsqlParameter("oldurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Value = oldUrl;

            arParams[4] = new NpgsqlParameter("newurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Value = newUrl;

            arParams[5] = new NpgsqlParameter("createdutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[5].Value = createdUtc;

            arParams[6] = new NpgsqlParameter("expireutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[6].Value = expireUtc;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_RedirectList table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="oldUrl"> oldUrl </param>
        /// <param name="newUrl"> newUrl </param>
        /// <param name="expireUtc"> expireUtc </param>
        /// <returns>bool</returns>
        public bool Update(
            Guid rowGuid,
            string oldUrl,
            string newUrl,
            DateTime expireUtc)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_redirectlist ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("oldurl = :oldurl, ");
            sqlCommand.Append("newurl = :newurl, ");
            sqlCommand.Append("expireutc = :expireutc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("rowguid = :rowguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new NpgsqlParameter("oldurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = oldUrl;

            arParams[2] = new NpgsqlParameter("newurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Value = newUrl;

            arParams[3] = new NpgsqlParameter("expireutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Value = expireUtc;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_RedirectList table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public bool Delete(Guid rowGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_redirectlist ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("rowguid = :rowguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = rowGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_RedirectList table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public DbDataReader GetOne(Guid rowGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_redirectlist ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("rowguid = :rowguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = rowGuid.ToString();

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }


        /// <summary>
        /// Gets an IDataReader with one row from the mp_RedirectList table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public DbDataReader GetBySiteAndUrl(int siteId, string oldUrl)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_redirectlist ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append("AND oldUrl = :oldurl ");
            sqlCommand.Append("AND expireutc < :currenttime ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("oldurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = oldUrl;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Value = DateTime.UtcNow;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


        /// <summary>
        /// returns true if the record exists
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public bool Exists(int siteId, string oldUrl)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_redirectlist ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append("AND oldUrl = :oldurl ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("oldurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = oldUrl;

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }


        /// <summary>
        /// Gets a count of rows in the mp_RedirectList table.
        /// </summary>
        public int GetCount(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_redirectlist ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }


        /// <summary>
        /// Gets a page of data from the mp_RedirectList table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public DbDataReader GetPage(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCount(siteId);

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



            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_redirectlist  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append("ORDER BY oldurl ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }


    }
}
