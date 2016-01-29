// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2016-01-29
// 

using cloudscribe.DbHelpers;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace cloudscribe.Core.Repositories.pgsql
{

    internal class DBSiteUser
    {
        internal DBSiteUser(
            string dbReadConnectionString,
            string dbWriteConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            readConnectionString = dbReadConnectionString;
            writeConnectionString = dbWriteConnectionString;

            // possibly will change this later to have NpgSqlFactory/DbProviderFactory injected
            AdoHelper = new AdoHelper(Npgsql.NpgsqlFactory.Instance);
        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string readConnectionString;
        private string writeConnectionString;
        private AdoHelper AdoHelper;

        public DbDataReader GetUserCountByYearMonth(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("cast(date_part('year', datecreated) as int4) As y,  ");
            sqlCommand.Append("cast(date_part('month', datecreated) as int4) As m, ");
            sqlCommand.Append("cast(date_part('year', datecreated) as varchar(10)) || '-' || cast(date_part('month', datecreated) as varchar(3))  As label, ");
            sqlCommand.Append("COUNT(*) As users ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append("GROUP BY cast(date_part('year', datecreated) as int4), cast(date_part('month', datecreated) as int4), cast(date_part('year', datecreated) as varchar(10)) || '-' || cast(date_part('month', datecreated) as varchar(3)) ");
            sqlCommand.Append("ORDER BY cast(date_part('year', datecreated) as int4), cast(date_part('month', datecreated) as int4) ");
            sqlCommand.Append("; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


        public DbDataReader GetUserList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserID, ");
            sqlCommand.Append("name, ");
            sqlCommand.Append("passwordsalt, ");
            sqlCommand.Append("pwd, ");
            sqlCommand.Append("email ");
            sqlCommand.Append("FROM mp_users ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("email");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);



        }


        //public DbDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[2];

        //    arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new NpgsqlParameter("query", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
        //    arParams[1].Value = query + "%";

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT ");

        //    sqlCommand.Append("userid, ");
        //    sqlCommand.Append("userguid, ");
        //    sqlCommand.Append("firstname, ");
        //    sqlCommand.Append("lastname, ");
        //    sqlCommand.Append("email, ");
        //    sqlCommand.Append("name AS siteuser ");

        //    sqlCommand.Append("FROM mp_users ");

        //    sqlCommand.Append("WHERE siteid = :siteid ");
        //    sqlCommand.Append("AND isdeleted = false ");
        //    sqlCommand.Append("AND (  ");
        //    sqlCommand.Append(" (name LIKE :query) ");
        //    sqlCommand.Append(" OR (firstname LIKE :query) ");
        //    sqlCommand.Append(" OR (lastname LIKE :query) ");
        //    sqlCommand.Append(") ");

        //    sqlCommand.Append("UNION ");

        //    sqlCommand.Append("SELECT ");

        //    sqlCommand.Append("userid, ");
        //    sqlCommand.Append("userguid, ");
        //    sqlCommand.Append("firstname, ");
        //    sqlCommand.Append("lastname, ");
        //    sqlCommand.Append("email, ");
        //    sqlCommand.Append("email As siteuser ");

        //    sqlCommand.Append("FROM mp_users ");

        //    sqlCommand.Append("WHERE siteid = :siteid ");
        //    sqlCommand.Append("AND isdeleted = false ");
        //    sqlCommand.Append("AND email LIKE :query   ");

        //    sqlCommand.Append("ORDER BY siteuser ");

        //    sqlCommand.Append("LIMIT " + rowsToGet.ToString());

        //    sqlCommand.Append(";");

        //    return AdoHelper.ExecuteReader(
        //       readConnectionString,
        //       CommandType.Text,
        //       sqlCommand.ToString(),
        //       arParams);



        //}

        public DbDataReader EmailLookup(int siteId, string query, int rowsToGet)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("query", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Value = query + "%";

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");

            sqlCommand.Append("userid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("email ");

            sqlCommand.Append("FROM mp_users ");

            sqlCommand.Append("WHERE siteid = :siteid ");
            sqlCommand.Append("AND isdeleted = false ");
            sqlCommand.Append("AND (  ");
            sqlCommand.Append(" (email LIKE :query) ");
            sqlCommand.Append("OR (name LIKE :query) ");
            sqlCommand.Append(" OR (firstname LIKE :query) ");
            sqlCommand.Append(" OR (lastname LIKE :query) ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY email ");

            sqlCommand.Append("LIMIT " + rowsToGet.ToString());

            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
               readConnectionString,
               CommandType.Text,
               sqlCommand.ToString(),
               arParams);
        }






        public int UserCount(int siteId)
        {

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");

            //int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_users_count(:siteid)", 
            //    arParams));

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public async Task<int> CountUsers(
            int siteId, 
            string userNameBeginsWith,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("usernamebeginswith", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Value = userNameBeginsWith + "%";

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("name like :usernamebeginswith ");
            sqlCommand.Append(";");

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return count;

        }


        public int CountUsersByRegistrationDateRange(
            int siteId,
            DateTime beginDate,
            DateTime endDate)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Value = beginDate;

            arParams[2] = new NpgsqlParameter("enddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Value = endDate;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("datecreated >= :begindate ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("datecreated < :enddate ");
            sqlCommand.Append(";");

            //int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_users_countbyregistrationdaterange(:siteid,:begindate,:enddate)", 
            //    arParams));

            int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return count;

        }


        //public int CountOnlineSince(int siteId, DateTime sinceTime)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[2];

        //    arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new NpgsqlParameter("sincetime", NpgsqlTypes.NpgsqlDbType.Timestamp);
        //    arParams[1].Value = sinceTime;

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  Count(*) ");
        //    sqlCommand.Append("FROM	mp_users ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("siteid = :siteid ");
        //    sqlCommand.Append("AND ");
        //    sqlCommand.Append("lastactivitydate > :sincetime ");
        //    sqlCommand.Append(";");
            
        //    int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        readConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams));

        //    return count;

        //}

        //public DbDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[2];

        //    arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new NpgsqlParameter("sincetime", NpgsqlTypes.NpgsqlDbType.Timestamp);
        //    arParams[1].Value = sinceTime;

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_users ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("siteid = :siteid ");
        //    sqlCommand.Append("AND ");
        //    sqlCommand.Append("lastactivitydate > :sincetime ");
        //    sqlCommand.Append(";");

        //    return AdoHelper.ExecuteReader(
        //        readConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public DbDataReader GetTop50UsersOnlineSince(int siteId, DateTime sinceTime)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[2];

        //    arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[0].Value = siteId;

        //    arParams[1] = new NpgsqlParameter("sincetime", NpgsqlTypes.NpgsqlDbType.Timestamp);
        //    arParams[1].Value = sinceTime;

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_users ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("siteid = :siteid ");
        //    sqlCommand.Append("AND ");
        //    sqlCommand.Append("lastactivitydate > :sincetime ");
        //    sqlCommand.Append("LIMIT 50");
        //    sqlCommand.Append(";");
            
        //    return AdoHelper.ExecuteReader(
        //        readConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams);
        //}

        public async Task<int> GetNewestUserId(
            int siteId,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  cast(max(userid) as int4) ");
            sqlCommand.Append("FROM	mp_users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");
            
            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int count = Convert.ToInt32(result);

            return count;

        }


        public async Task<DbDataReader> GetUserListPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith,
            int sortMode,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = pageLowerBound;

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter("usernamebeginswith", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[2].Value = userNameBeginsWith + "%";

            arParams[3] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	u.* ");

            sqlCommand.Append("FROM	mp_users u ");

            sqlCommand.Append("WHERE u.accountapproved = true   ");
            sqlCommand.Append("AND u.siteid = :siteid   ");

            if (userNameBeginsWith.Length > 0)
            {
                sqlCommand.Append(" AND u.name LIKE :usernamebeginswith ");
            }

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY u.datecreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append(" ORDER BY u.lastname, u.firstname, u.name ");
                    break;

                case 0:
                default:
                    sqlCommand.Append(" ORDER BY u.name ");
                    break;
            }

            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<int> CountUsersForSearch(
            int siteId, 
            string searchInput,
            CancellationToken cancellationToken)
        {

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT Count(*) FROM mp_users WHERE siteid = :siteid ");
            sqlCommand.Append("AND accountapproved = true ");
            sqlCommand.Append("AND displayinmemberlist = true ");
            sqlCommand.Append("AND isdeleted = false ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (name LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (loginname LIKE :searchinput) ");
                //sqlCommand.Append(" OR ");
                //sqlCommand.Append(" (email LIKE :searchinput) ");

                sqlCommand.Append(")");

            }
            sqlCommand.Append(" ;  ");


            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("searchinput", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Value = "%" + searchInput + "%";

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return Convert.ToInt32(result);


        }

        public async Task<DbDataReader> GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("searchinput", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Value = "%" + searchInput + "%";

            arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageSize;

            arParams[3] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid = :siteid  ");

            sqlCommand.Append("AND accountapproved = true ");
            sqlCommand.Append("AND displayinmemberlist = true ");
            sqlCommand.Append("AND isdeleted = false ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (name LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (loginname LIKE :searchinput) ");
                //sqlCommand.Append(" OR ");
                //sqlCommand.Append(" (email LIKE :searchinput) ");

                sqlCommand.Append(")");

            }

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY datecreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append(" ORDER BY lastname, firstname, name ");
                    break;

                case 0:
                default:
                    sqlCommand.Append(" ORDER BY name ");
                    break;
            }

            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<int> CountUsersForAdminSearch(
            int siteId, 
            string searchInput,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT Count(*) FROM mp_users WHERE siteid = :siteid ");
            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (name LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (loginname LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (email LIKE :searchinput) ");

                sqlCommand.Append(")");

            }
            sqlCommand.Append(" ;  ");


            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("searchinput", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Value = "%" + searchInput + "%";

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return Convert.ToInt32(result);

        }

        public async Task<DbDataReader> GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("searchinput", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Value = "%" + searchInput + "%";

            arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageSize;

            arParams[3] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid = :siteid  ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");
                sqlCommand.Append(" (name LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (loginname LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (email LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (lastname LIKE :searchinput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (firstname LIKE :searchinput) ");
                sqlCommand.Append(")");

            }

            switch (sortMode)
            {
                case 1:
                    sqlCommand.Append(" ORDER BY datecreated DESC ");
                    break;

                case 2:
                    sqlCommand.Append(" ORDER BY lastname, firstname, name ");
                    break;

                case 0:
                default:
                    sqlCommand.Append(" ORDER BY name ");
                    break;
            }

            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);


        }

        public async Task<int> CountLockedOutUsers(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT Count(*) FROM mp_users WHERE siteid = :siteid AND islockedout = true; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return Convert.ToInt32(result);

        }

        public async Task<DbDataReader> GetPageLockedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid = :siteid  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("islockedout = true ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("name  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }



        public async Task<int> CountNotApprovedUsers(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT Count(*) FROM mp_users WHERE siteid = :siteid AND accountapproved = false; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return Convert.ToInt32(result);
        }

        public async Task<DbDataReader> GetPageNotApprovedUsers(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid = :siteid  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("accountapproved = false ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("name  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<int> CountEmailUnconfirmed(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT Count(*) FROM mp_users WHERE siteid = :siteid AND emailconfirmed = false; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return Convert.ToInt32(result);
        }

        public async Task<DbDataReader> GetPageEmailUnconfirmed(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid = :siteid  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("emailconfirmed = false ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("name  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<int> CountPhoneUnconfirmed(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT Count(*) FROM mp_users WHERE siteid = :siteid AND phonenumberconfirmed = false; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return Convert.ToInt32(result);
        }

        public async Task<DbDataReader> GetPagePhoneUnconfirmed(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid = :siteid  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("phonenumberconfirmed = false ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("name  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<int> CountFutureLockoutDate(
            int siteId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM mp_users ");
            sqlCommand.Append("WHERE siteid = :siteid ");
            sqlCommand.Append("AND lockoutenddateutc IS NOT NULL ");
            sqlCommand.Append("AND lockoutenddateutc > :currentutc ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("currentutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Value = DateTime.UtcNow;

            object result = await AdoHelper.ExecuteScalarAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return Convert.ToInt32(result);
        }

        public async Task<DbDataReader> GetFutureLockoutPage(
            int siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageLowerBound;

            arParams[3] = new NpgsqlParameter("currentutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Value = DateTime.UtcNow;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_users  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid = :siteid  ");
            sqlCommand.Append("AND lockoutenddateutc IS NOT NULL ");
            sqlCommand.Append("AND lockoutenddateutc > :currentutc ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("name  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<int> AddUser(
            Guid siteGuid,
            int siteId,
            string fullName,
            string loginName,
            string email,
            Guid userGuid,
            DateTime dateCreated,
            bool mustChangePwd,
            string firstName,
            string lastName,
            string timeZoneId,
            DateTime? dateOfBirth,
            bool emailConfirmed,
            string passwordHash,
            string securityStamp,
            string phoneNumber,
            bool phoneNumberConfirmed,
            bool twoFactorEnabled,
            DateTime? lockoutEndDateUtc,
            bool accountApproved,
            bool isLockedOut,
            bool displayInMemberList,
            string webSiteUrl,
            string country,
            string state,
            string avatarUrl,
            string signature,
            string authorBio,
            string comment,

            string normalizedUserName,
            string loweredEmail,
            bool canAutoLockout,

            CancellationToken cancellationToken

            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_users (");
            sqlCommand.Append("siteid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("name, ");
            sqlCommand.Append("loginname, ");
            sqlCommand.Append("email, ");
            sqlCommand.Append("loweredemail, ");
            sqlCommand.Append("firstname, ");
            sqlCommand.Append("lastname, ");
            sqlCommand.Append("timezoneid, ");
            sqlCommand.Append("mustchangepwd, ");
            sqlCommand.Append("roleschanged, ");
            sqlCommand.Append("datecreated, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("dateofbirth, ");
            sqlCommand.Append("emailconfirmed, ");
            sqlCommand.Append("passwordhash, ");
            sqlCommand.Append("securitystamp, ");
            sqlCommand.Append("phonenumber, ");
            sqlCommand.Append("phonenumberconfirmed, ");
            sqlCommand.Append("twofactorenabled, ");
            sqlCommand.Append("lockoutenddateutc, ");
            sqlCommand.Append("accountapproved, ");
            sqlCommand.Append("islockedout, ");
            sqlCommand.Append("displayinmemberlist, ");
            sqlCommand.Append("websiteurl, ");
            sqlCommand.Append("country, ");
            sqlCommand.Append("state, ");
            sqlCommand.Append("avatarurl, ");
            sqlCommand.Append("signature, ");
            sqlCommand.Append("authorbio, ");

            sqlCommand.Append("normalizedusername, ");
            sqlCommand.Append("canautolockout, ");

            sqlCommand.Append("comment ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":siteid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":name, ");
            sqlCommand.Append(":loginname, ");
            sqlCommand.Append(":email, ");
            sqlCommand.Append(":loweredemail, ");
            sqlCommand.Append(":firstname, ");
            sqlCommand.Append(":lastname, ");
            sqlCommand.Append(":timezoneid, ");
            sqlCommand.Append(":mustchangepwd, ");
            sqlCommand.Append("false, "); //roleschanged
            sqlCommand.Append(":datecreated, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":dateofbirth, ");
            sqlCommand.Append(":emailconfirmed, ");
            sqlCommand.Append(":passwordhash, ");
            sqlCommand.Append(":securitystamp, ");
            sqlCommand.Append(":phonenumber, ");
            sqlCommand.Append(":phonenumberconfirmed, ");
            sqlCommand.Append(":twofactorenabled, ");
            sqlCommand.Append(":lockoutenddateutc, ");
            sqlCommand.Append(":accountapproved, ");
            sqlCommand.Append(":islockedout, ");
            sqlCommand.Append(":displayinmemberlist, ");
            sqlCommand.Append(":websiteurl, ");
            sqlCommand.Append(":country, ");
            sqlCommand.Append(":state, ");
            sqlCommand.Append(":avatarurl, ");
            sqlCommand.Append(":signature, ");
            sqlCommand.Append(":authorbio, ");

            sqlCommand.Append(":normalizedusername, ");
            sqlCommand.Append(":canautolockout, ");

            sqlCommand.Append(":comment ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_users_userid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[32];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("name", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[1].Value = fullName;

            arParams[2] = new NpgsqlParameter("loginname", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[2].Value = loginName;

            arParams[3] = new NpgsqlParameter("email", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[3].Value = email;
            
            arParams[4] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Text, 36);
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new NpgsqlParameter("datecreated", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[5].Value = dateCreated;

            arParams[6] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[6].Value = siteGuid.ToString();

            arParams[7] = new NpgsqlParameter("loweredemail", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[7].Value = loweredEmail;

            arParams[8] = new NpgsqlParameter("mustchangepwd", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Value = mustChangePwd;

            arParams[9] = new NpgsqlParameter("firstname", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[9].Value = firstName;

            arParams[10] = new NpgsqlParameter("lastname", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[10].Value = lastName;

            arParams[11] = new NpgsqlParameter("timezoneid", NpgsqlTypes.NpgsqlDbType.Text, 32);
            arParams[11].Value = timeZoneId;
            
            arParams[12] = new NpgsqlParameter("dateofbirth", NpgsqlTypes.NpgsqlDbType.Timestamp);
            if (!dateOfBirth.HasValue)
            {
                arParams[12].Value = DBNull.Value;
            }
            else
            {
                arParams[12].Value = dateOfBirth;
            }

            arParams[13] = new NpgsqlParameter("emailconfirmed", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[13].Value = emailConfirmed;
            
            arParams[14] = new NpgsqlParameter("passwordhash", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[14].Value = passwordHash;

            arParams[15] = new NpgsqlParameter("securitystamp", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[15].Value = securityStamp;

            arParams[16] = new NpgsqlParameter("phonenumber", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[16].Value = phoneNumber;

            arParams[17] = new NpgsqlParameter("phonenumberconfirmed", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[17].Value = phoneNumberConfirmed;

            arParams[18] = new NpgsqlParameter("twofactorenabled", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[18].Value = twoFactorEnabled;

            arParams[19] = new NpgsqlParameter("lockoutenddateutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            if (lockoutEndDateUtc.HasValue)
            {
                arParams[19].Value = DBNull.Value;
            }
            else
            {
                arParams[19].Value = lockoutEndDateUtc;
            }

            arParams[20] = new NpgsqlParameter("accountapproved", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[20].Value = accountApproved;

            arParams[21] = new NpgsqlParameter("islockedout", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[21].Value = isLockedOut;

            arParams[22] = new NpgsqlParameter("displayinmemberlist", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[22].Value = displayInMemberList;

            arParams[23] = new NpgsqlParameter("websiteurl", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[23].Value = webSiteUrl;

            arParams[24] = new NpgsqlParameter("country", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[24].Value = country;

            arParams[25] = new NpgsqlParameter("state", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[25].Value = state;

            arParams[26] = new NpgsqlParameter("avatarurl", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[26].Value = avatarUrl;

            arParams[27] = new NpgsqlParameter("signature", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[27].Value = signature;

            arParams[28] = new NpgsqlParameter("authorbio", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[28].Value = authorBio;

            arParams[29] = new NpgsqlParameter("comment", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[29].Value = comment;

            arParams[30] = new NpgsqlParameter("normalizedusername", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[30].Value = normalizedUserName;

            arParams[31] = new NpgsqlParameter("canautolockout", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[31].Value = canAutoLockout;

            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int newID = Convert.ToInt32(result);

            return newID;
        }

        public async Task<bool> UpdateUser(
            int userId,
            string name,
            string loginName,
            string email,
            string gender,
            bool accountApproved,
            bool trusted,
            bool displayInMemberList,
            string webSiteUrl,
            string country,
            string state,
            string avatarUrl,
            string signature,
            string loweredEmail,
            string comment,
            bool mustChangePwd,
            string firstName,
            string lastName,
            string timeZoneId,
            string newEmail,
            bool rolesChanged,
            string authorBio,
            DateTime? dateOfBirth,
            bool emailConfirmed,
            string passwordHash,
            string securityStamp,
            string phoneNumber,
            bool phoneNumberConfirmed,
            bool twoFactorEnabled,
            DateTime? lockoutEndDateUtc,
            bool isLockedOut,

            string normalizedUserName,
            bool newEmailApproved,
            bool canAutoLockout,
            DateTime? lastPasswordChangedDate,

            CancellationToken cancellationToken
            )
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");
            sqlCommand.Append("SET email = :email , ");
            sqlCommand.Append("name = :name,");
            sqlCommand.Append("loginname = :loginname,");
            sqlCommand.Append("firstname = :firstname,");
            sqlCommand.Append("lastname = :lastname,");
            sqlCommand.Append("timezoneid = :timezoneid,");
            sqlCommand.Append("newemail = :newemail, ");
            sqlCommand.Append("mustchangepwd = :mustchangepwd,");
            sqlCommand.Append("roleschanged = :roleschanged,");
            sqlCommand.Append("gender = :gender,");
            sqlCommand.Append("accountapproved = :accountapproved,");
            sqlCommand.Append("trusted = :trusted,");
            sqlCommand.Append("displayinmemberlist = :displayinmemberlist,");
            sqlCommand.Append("websiteurl = :websiteurl,");
            sqlCommand.Append("country = :country,");
            sqlCommand.Append("state = :state,");
            sqlCommand.Append("avatarurl = :avatarurl,");
            sqlCommand.Append("signature = :signature,");
            sqlCommand.Append("authorbio = :authorbio,");
            sqlCommand.Append("loweredemail = :loweredemail,");
            sqlCommand.Append("comment = :comment,");
            sqlCommand.Append("dateofbirth = :dateofbirth, ");
            sqlCommand.Append("emailconfirmed = :emailconfirmed, ");
            sqlCommand.Append("passwordhash = :passwordhash, ");
            sqlCommand.Append("securitystamp = :securitystamp, ");
            sqlCommand.Append("phonenumber = :phonenumber, ");
            sqlCommand.Append("phonenumberconfirmed = :phonenumberconfirmed, ");
            sqlCommand.Append("twofactorenabled = :twofactorenabled, ");
            sqlCommand.Append("lockoutenddateutc = :lockoutenddateutc, ");

            sqlCommand.Append("normalizedusername = :normalizedusername, ");
            sqlCommand.Append("newemailapproved = :newemailapproved, ");
            sqlCommand.Append("canautolockout = :canautolockout, ");
            sqlCommand.Append("lastpasswordchangeddate = :lastpasswordchangeddate, ");


            sqlCommand.Append("islockedout = :islockedout    ");

            sqlCommand.Append("WHERE userid = :userid ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[34];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = userId;

            arParams[1] = new NpgsqlParameter("name", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[1].Value = name;

            arParams[2] = new NpgsqlParameter("loginname", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[2].Value = loginName;

            arParams[3] = new NpgsqlParameter("email", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[3].Value = email;

            arParams[4] = new NpgsqlParameter("loweredemail", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[4].Value = loweredEmail;
            
            arParams[5] = new NpgsqlParameter("gender", NpgsqlTypes.NpgsqlDbType.Text, 10);
            arParams[5].Value = gender;

            arParams[6] = new NpgsqlParameter("accountapproved", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Value = accountApproved;
            
            arParams[7] = new NpgsqlParameter("trusted", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Value = trusted;

            arParams[8] = new NpgsqlParameter("displayinmemberlist", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Value = displayInMemberList;

            arParams[9] = new NpgsqlParameter("websiteurl", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[9].Value = webSiteUrl;

            arParams[10] = new NpgsqlParameter("country", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[10].Value = country;

            arParams[11] = new NpgsqlParameter("state", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[11].Value = state;
            
            arParams[12] = new NpgsqlParameter("avatarurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[12].Value = avatarUrl;
            
            arParams[13] = new NpgsqlParameter("signature", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[13].Value = signature;
            
            arParams[14] = new NpgsqlParameter("comment", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[14].Value = comment;
            
            arParams[15] = new NpgsqlParameter("mustchangepwd", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[15].Value = mustChangePwd;

            arParams[16] = new NpgsqlParameter("firstname", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[16].Value = firstName;

            arParams[17] = new NpgsqlParameter("lastname", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[17].Value = lastName;

            arParams[18] = new NpgsqlParameter("timezoneid", NpgsqlTypes.NpgsqlDbType.Varchar, 32);
            arParams[18].Value = timeZoneId;
            
            arParams[19] = new NpgsqlParameter("newemail", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[19].Value = newEmail;
            
            arParams[20] = new NpgsqlParameter("roleschanged", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[20].Value = rolesChanged;

            arParams[21] = new NpgsqlParameter("authorbio", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[21].Value = authorBio;

            arParams[22] = new NpgsqlParameter("dateofbirth", NpgsqlTypes.NpgsqlDbType.Timestamp);
            if (!dateOfBirth.HasValue)
            {
                arParams[22].Value = DBNull.Value;
            }
            else
            {
                arParams[22].Value = dateOfBirth;
            }

            arParams[23] = new NpgsqlParameter("emailconfirmed", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[23].Value = emailConfirmed;
            
            arParams[24] = new NpgsqlParameter("passwordhash", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[24].Value = passwordHash;

            arParams[25] = new NpgsqlParameter("securitystamp", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[25].Value = securityStamp;

            arParams[26] = new NpgsqlParameter("phonenumber", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[26].Value = phoneNumber;

            arParams[27] = new NpgsqlParameter("phonenumberconfirmed", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[27].Value = phoneNumberConfirmed;

            arParams[28] = new NpgsqlParameter("twofactorenabled", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[28].Value = twoFactorEnabled;

            arParams[29] = new NpgsqlParameter("lockoutenddateutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            if (!lockoutEndDateUtc.HasValue)
            {
                arParams[29].Value = DBNull.Value;
            }
            else
            {
                arParams[29].Value = lockoutEndDateUtc;
            }

            arParams[30] = new NpgsqlParameter("islockedout", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[30].Value = isLockedOut;

            arParams[31] = new NpgsqlParameter("normalizedusername", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[31].Value = normalizedUserName;

            arParams[32] = new NpgsqlParameter("canautolockout", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[32].Value = canAutoLockout;

            arParams[33] = new NpgsqlParameter("lastpasswordchangeddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            if (!lastPasswordChangedDate.HasValue)
            {
                arParams[33].Value = DBNull.Value;
            }
            else
            {
                arParams[33].Value = lastPasswordChangedDate;
            }

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }
        

        public async Task<bool> DeleteUser(int userId, CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = userId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append(";");
            
            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int rowsAffected = Convert.ToInt32(result);

            return (rowsAffected > -1);

        }

        public async Task<bool> DeleteUsersBySite(
            int siteId,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");
            
            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int rowsAffected = Convert.ToInt32(result);

            return (rowsAffected > -1);

        }

        //public bool UpdateLastActivityTime(Guid userGuid, DateTime lastUpdate)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[2];

        //    arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
        //    arParams[0].Value = userGuid.ToString();

        //    arParams[1] = new NpgsqlParameter("lastactivitydate", NpgsqlTypes.NpgsqlDbType.Timestamp);
        //    arParams[1].Value = lastUpdate;

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_users ");
        //    sqlCommand.Append("set lastactivitydate = :lastactivitydate ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("userguid = :userguid ");
        //    sqlCommand.Append(";");
            
        //    int rowsAffected = Convert.ToInt32(AdoHelper.ExecuteScalar(
        //        writeConnectionString,
        //        CommandType.Text,
        //        sqlCommand.ToString(),
        //        arParams));

        //    return (rowsAffected > -1);

        //}

        public async Task<bool> UpdateLastLoginTime(
            Guid userGuid, 
            DateTime lastLoginTime,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");

            sqlCommand.Append("SET lastlogindate = :lastlogindate,  ");
            sqlCommand.Append("failedpasswordattemptcount = 0 ");

            sqlCommand.Append("WHERE userguid = :userguid  ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("lastlogindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Value = lastLoginTime;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

        public async Task<bool> AccountLockout(
            Guid userGuid, 
            DateTime lockoutTime,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("lastlockoutdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Value = lockoutTime;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");
            sqlCommand.Append("SET islockedout = true,  ");
            sqlCommand.Append("lastlockoutdate = :lastlockoutdate ");
            sqlCommand.Append("WHERE userguid = :userguid  ;");
            
            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int rowsAffected = Convert.ToInt32(result);

            return (rowsAffected > -1);

        }

        public bool UpdateLastPasswordChangeTime(Guid userGuid, DateTime lastPasswordChangeTime)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("lastpasswordchangeddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Value = lastPasswordChangeTime;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("lastpasswordchangeddate = :lastpasswordchangeddate ");
            sqlCommand.Append("WHERE userguid = :userguid  ;");
            
            int rowsAffected = Convert.ToInt32(AdoHelper.ExecuteScalar(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (rowsAffected > -1);

        }

        public bool UpdateFailedPasswordAttemptStartWindow(
            Guid userGuid,
            DateTime windowStartTime)
        {

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("windowstarttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Value = windowStartTime;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("failedpwdattemptwindowstart = :windowstarttime ");
            sqlCommand.Append("WHERE userguid = :userguid  ;");

            //int rowsAffected = Convert.ToInt32(AdoHelper.ExecuteScalar(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_users_setfailedpasswordattemptstartwindow(:userguid,:windowstarttime)",
            //    arParams));

            int rowsAffected = Convert.ToInt32(AdoHelper.ExecuteScalar(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (rowsAffected > -1);

        }

        public async Task<bool> UpdateFailedPasswordAttemptCount(
            Guid userGuid,
            int attemptCount,
            CancellationToken cancellationToken)
        {

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("attemptcount", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = attemptCount;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("failedpasswordattemptcount = :attemptcount ");
            sqlCommand.Append("WHERE userguid = :userguid  ;");

            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int rowsAffected = Convert.ToInt32(result);

            return (rowsAffected > -1);

        }

        public bool UpdateFailedPasswordAnswerAttemptStartWindow(
            Guid userGuid,
            DateTime windowStartTime)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("windowstarttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Value = windowStartTime;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("failedpwdanswerwindowstart = :windowstarttime ");
            sqlCommand.Append("WHERE userguid = :userguid  ;");

            //int rowsAffected = Convert.ToInt32(AdoHelper.ExecuteScalar(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_users_setfailedpasswordanswerattemptstartwindow(:userguid,:windowstarttime)",
            //    arParams));

            int rowsAffected = Convert.ToInt32(AdoHelper.ExecuteScalar(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (rowsAffected > -1);

        }

        public bool UpdateFailedPasswordAnswerAttemptCount(
            Guid userGuid,
            int attemptCount)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("attemptcount", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = attemptCount;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("failedpwdanswerattemptcount = :attemptcount ");
            sqlCommand.Append("WHERE userguid = :userguid  ;");

            //int rowsAffected = Convert.ToInt32(AdoHelper.ExecuteScalar(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_users_setfailedpasswordanswerattemptcount(:userguid,:attemptcount)",
            //    arParams));

            int rowsAffected = Convert.ToInt32(AdoHelper.ExecuteScalar(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (rowsAffected > -1);

        }

        public async Task<bool> SetRegistrationConfirmationGuid(
            Guid userGuid, 
            Guid registrationConfirmationGuid,
            CancellationToken cancellationToken
            )
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("registerconfirmguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Value = registrationConfirmationGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("islockedout = true, ");
            sqlCommand.Append("registerconfirmguid = :registerconfirmguid ");
            sqlCommand.Append("WHERE userguid = :userguid  ;");

            //int rowsAffected = Convert.ToInt32(AdoHelper.ExecuteScalar(
            //    writeConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_users_setregistrationguid(:userguid,:registerconfirmguid)",
            //    arParams));

           

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);

        }

        public async Task<bool> ConfirmRegistration(
            Guid emptyGuid, 
            Guid registrationConfirmationGuid,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("emptyguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = emptyGuid.ToString();

            arParams[1] = new NpgsqlParameter("registerconfirmguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Value = registrationConfirmationGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("islockedout = false, ");
            sqlCommand.Append("emailconfirmed = true,  ");
            sqlCommand.Append("registerconfirmguid = :emptyguid ");
            sqlCommand.Append("WHERE registerconfirmguid = :registerconfirmguid  ;");
            
            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int rowsAffected = Convert.ToInt32(result);

            return (rowsAffected > -1);

        }

        public async Task<bool> AccountClearLockout(
            Guid userGuid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");
            sqlCommand.Append("SET islockedout = false,  ");
            sqlCommand.Append("failedpasswordattemptcount = 0, ");
            sqlCommand.Append("failedpwdanswerattemptcount = 0 ");

            sqlCommand.Append("WHERE userguid = :userguid  ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = userGuid.ToString();

            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int rowsAffected = Convert.ToInt32(result);

            return (rowsAffected > -1);

        }

        
        public async Task<bool> FlagAsDeleted(
            int userId,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = userId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("isdeleted = true ");
            
            sqlCommand.Append("WHERE userid = :userid  ;");
            
            object result = await AdoHelper.ExecuteScalarAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            int rowsAffected = Convert.ToInt32(result);

            return (rowsAffected > -1);

        }

        public async Task<bool> FlagAsNotDeleted(
            int userId,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");
            sqlCommand.Append("SET isdeleted = false  ");

            sqlCommand.Append("WHERE userid = :userid  ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = userId;

            int rowsAffected = await AdoHelper.ExecuteNonQueryAsync(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

            return (rowsAffected > -1);
        }

       
        public async Task<DbDataReader> GetRolesByUser(
            int siteId, 
            int userId,
            CancellationToken cancellationToken)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("mp_roles.roleid, ");
            sqlCommand.Append("mp_roles.displayname, ");
            sqlCommand.Append("mp_roles.rolename ");

            sqlCommand.Append("FROM	 mp_userroles ");

            sqlCommand.Append("INNER JOIN mp_users ");
            sqlCommand.Append("ON mp_userroles.userid = mp_users.userid ");

            sqlCommand.Append("INNER JOIN mp_roles ");
            sqlCommand.Append("ON  mp_userroles.roleid = mp_roles.roleid ");

            sqlCommand.Append("WHERE mp_users.siteid = :siteid AND mp_users.userid = :userid  ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = userId;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetUserByRegistrationGuid(
            int siteId, 
            Guid registerConfirmGuid,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("registerconfirmguid = :registerconfirmguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("registerconfirmguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Value = registerConfirmGuid;

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }


        public async Task<DbDataReader> GetSingleUser(
            int siteId, 
            string email,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("email", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[1].Value = email.ToLower();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_users ");
            sqlCommand.Append("WHERE siteid = :siteid  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("loweredemail = :email ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetCrossSiteUserListByEmail(
            string email,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("email", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[0].Value = email.ToLower();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_users ");
            sqlCommand.Append("WHERE ");
            //sqlCommand.Append(" ");
            sqlCommand.Append("loweredemail = :email ");

            sqlCommand.Append(";");

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);



        }

        public async Task<DbDataReader> GetSingleUserByLoginName(
            int siteId, 
            string loginName, 
            bool allowEmailFallback,
            CancellationToken cancellationToken)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_users ");

            sqlCommand.Append("WHERE siteid = :siteid  ");

            if (allowEmailFallback)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("(");
                sqlCommand.Append("loginname = :loginname ");
                sqlCommand.Append("OR email = :loginname ");
                sqlCommand.Append("OR loweredemail = :loweredloginname ");
                sqlCommand.Append(")");
            }
            else
            {
                sqlCommand.Append("AND loginname = :loginname ");
            }

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("loginname", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[1].Value = loginName;

            arParams[2] = new NpgsqlParameter("loweredloginname", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[2].Value = loginName.ToLowerInvariant();

            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public DbDataReader GetSingleUserByLoginNameNonAsync(
            int siteId, 
            string loginName, 
            bool allowEmailFallback)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_users ");

            sqlCommand.Append("WHERE siteid = :siteid  ");

            if (allowEmailFallback)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("(");
                sqlCommand.Append("loginname = :loginname ");
                sqlCommand.Append("OR email = :loginname ");
                sqlCommand.Append("OR loweredemail = :loweredloginname ");
                sqlCommand.Append(")");
            }
            else
            {
                sqlCommand.Append("AND loginname = :loginname ");
            }

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("loginname", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[1].Value = loginName;

            arParams[2] = new NpgsqlParameter("loweredloginname", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[2].Value = loginName.ToLowerInvariant();

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public async Task<DbDataReader> GetSingleUser(
            int userId,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = userId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append(";");
            
            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        public async Task<DbDataReader> GetSingleUser(
            Guid userGuid,
            CancellationToken cancellationToken)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = userGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append(";");
            
            return await AdoHelper.ExecuteReaderAsync(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams,
                cancellationToken);

        }

        

        public string LoginByEmail(int siteId, string email, string password)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  name ");
            sqlCommand.Append("FROM	mp_users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("email = :email  ");
            sqlCommand.Append("AND siteid = :siteid  ");
            sqlCommand.Append("AND isdeleted = false ");
            sqlCommand.Append("AND pwd = :password   ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("email", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[1].Value = email;

            arParams[2] = new NpgsqlParameter("password", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Value = password;

            string userName = string.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
               readConnectionString,
               CommandType.Text,
               sqlCommand.ToString(),
               arParams))
            {
                if (reader.Read())
                {
                    userName = reader["Name"].ToString();
                }

            }

            return userName;

        }

        public string Login(int siteId, string loginName, string password)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  name ");
            sqlCommand.Append("FROM	mp_users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("loginname = :loginname  ");
            sqlCommand.Append("AND siteid = :siteid  ");
            sqlCommand.Append("AND isdeleted = false ");
            sqlCommand.Append("AND pwd = :password  ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("loginname", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[1].Value = loginName;

            arParams[2] = new NpgsqlParameter("password", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Value = password;

            string userName = string.Empty;

            using (DbDataReader reader = AdoHelper.ExecuteReader(
               readConnectionString,
               CommandType.Text,
               sqlCommand.ToString(),
               arParams))
            {
                if (reader.Read())
                {
                    userName = reader["Name"].ToString();
                }

            }

            return userName;

        }


        //public static DataTable GetNonLazyLoadedPropertiesForUser(Guid userGuid)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];

        //    arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
        //    arParams[0].Value = userGuid.ToString();

        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("UserGuid", typeof(String));
        //    dataTable.Columns.Add("PropertyName", typeof(String));
        //    dataTable.Columns.Add("PropertyValueString", typeof(String));
        //    dataTable.Columns.Add("PropertyValueBinary", typeof(object));

        //    using (IDataReader reader = AdoHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
        //        CommandType.StoredProcedure,
        //        "mp_userproperties_select_byuser(:userguid)",
        //        arParams))
        //    {
        //        while (reader.Read())
        //        {
        //            DataRow row = dataTable.NewRow();
        //            row["UserGuid"] = reader["UserGuid"].ToString();
        //            row["PropertyName"] = reader["PropertyName"].ToString();
        //            row["PropertyValueString"] = reader["PropertyValueString"].ToString();
        //            row["PropertyValueBinary"] = reader["PropertyValueBinary"];
        //            dataTable.Rows.Add(row);
        //        }

        //    }

        //    return dataTable;

        //}

        public DbDataReader GetLazyLoadedProperty(Guid userGuid, String propertyName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("propertyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = propertyName;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_userproperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("propertyname = :propertyname ");
            sqlCommand.Append("LIMIT 1 ");
            sqlCommand.Append(";");

            //return AdoHelper.ExecuteReader(
            //    readConnectionString,
            //    CommandType.StoredProcedure,
            //    "mp_userproperties_select_one(:userguid,:propertyname",
            //    arParams);

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


        public bool PropertyExists(Guid userGuid, string propertyName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("propertyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = propertyName;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_userproperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("propertyname = :propertyname ");
            sqlCommand.Append(";");

            //int count = Convert.ToInt32(AdoHelper.ExecuteScalar(
            //    readConnectionString,
            //        CommandType.StoredProcedure,
            //        "mp_userproperties_propertyexists(:userguid,:propertyname)",
            //        arParams));

            object obj = AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            int count = Convert.ToInt32(obj);

            return (count > 0);
        }

        public void CreateProperty(
            Guid propertyId,
            Guid userGuid,
            String propertyName,
            String propertyValues,
            byte[] propertyValueb,
            DateTime lastUpdatedDate,
            bool isLazyLoaded)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[7];

            arParams[0] = new NpgsqlParameter("propertyid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = propertyId.ToString();

            arParams[1] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new NpgsqlParameter("propertyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Value = propertyName;

            arParams[3] = new NpgsqlParameter("propertyvaluestring", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Value = propertyValues;

            arParams[4] = new NpgsqlParameter("propertyvaluebinary", NpgsqlTypes.NpgsqlDbType.Bytea);
            arParams[4].Value = propertyValueb;

            arParams[5] = new NpgsqlParameter("lastupdateddate", NpgsqlTypes.NpgsqlDbType.Date);
            arParams[5].Value = lastUpdatedDate;

            arParams[6] = new NpgsqlParameter("islazyloaded", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Value = isLazyLoaded;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_userproperties (");
            sqlCommand.Append("propertyid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("propertyname, ");
            sqlCommand.Append("propertyvaluestring, ");
            sqlCommand.Append("propertyvaluebinary, ");
            sqlCommand.Append("lastupdateddate, ");
            sqlCommand.Append("islazyloaded )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":propertyid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":propertyname, ");
            sqlCommand.Append(":propertyvaluestring, ");
            sqlCommand.Append(":propertyvaluebinary, ");
            sqlCommand.Append(":lastupdateddate, ");
            sqlCommand.Append(":islazyloaded ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            //AdoHelper.ExecuteNonQuery(
            //    writeConnectionString,
            //        CommandType.StoredProcedure,
            //        "mp_userproperties_insert(:propertyid,:userguid,:propertyname,:propertyvaluestring,:propertyvaluebinary,:lastupdateddate,:islazyloaded)",
            //        arParams);

            AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                    CommandType.Text,
                    sqlCommand.ToString(),
                    arParams);


        }

        public void UpdateProperty(
            Guid userGuid,
            String propertyName,
            String propertyValues,
            byte[] propertyValueb,
            DateTime lastUpdatedDate,
            bool isLazyLoaded)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[6];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("propertyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Value = propertyName;

            arParams[2] = new NpgsqlParameter("propertyvaluestring", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Value = propertyValues;

            arParams[3] = new NpgsqlParameter("propertyvaluebinary", NpgsqlTypes.NpgsqlDbType.Bytea);
            arParams[3].Value = propertyValueb;

            arParams[4] = new NpgsqlParameter("lastupdateddate", NpgsqlTypes.NpgsqlDbType.Date);
            arParams[4].Value = lastUpdatedDate;

            arParams[5] = new NpgsqlParameter("islazyloaded", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Value = isLazyLoaded;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_userproperties ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("propertyvaluestring = :propertyvaluestring, ");
            sqlCommand.Append("propertyvaluebinary = :propertyvaluebinary, ");
            sqlCommand.Append("lastupdateddate = :lastupdateddate, ");
            sqlCommand.Append("islazyloaded = :islazyloaded ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("propertyname = :propertyname ");
            sqlCommand.Append(";");

            //AdoHelper.ExecuteNonQuery(
            //    writeConnectionString,
            //        CommandType.StoredProcedure,
            //        "mp_userproperties_update(:userguid,:propertyname,:propertyvaluestring,:propertyvaluebinary,:lastupdateddate,:islazyloaded)",
            //        arParams);

            AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                    CommandType.Text,
                    sqlCommand.ToString(),
                    arParams);


        }


        public bool DeletePropertiesByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_userproperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }



    }
}
