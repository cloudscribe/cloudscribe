// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-12-30
// Last Modified:			2015-06-14
// 

using cloudscribe.DbHelpers.Sqlite;
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Framework.Logging;
using System.Text;

namespace cloudscribe.Core.Repositories.SQLite
{
    internal class DBTaskQueue
    {
        internal DBTaskQueue(
            string dbConnectionString,
            ILoggerFactory loggerFactory)
        {
            logFactory = loggerFactory;
            connectionString = dbConnectionString;


        }

        private ILoggerFactory logFactory;
        //private ILogger log;
        private string connectionString;

        /// <summary>
        /// Inserts a row in the mp_TaskQueue table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="queuedBy"> queuedBy </param>
        /// <param name="taskName"> taskName </param>
        /// <param name="notifyOnCompletion"> notifyOnCompletion </param>
        /// <param name="notificationToEmail"> notificationToEmail </param>
        /// <param name="notificationFromEmail"> notificationFromEmail </param>
        /// <param name="notificationSubject"> notificationSubject </param>
        /// <param name="taskCompleteMessage"> taskCompleteMessage </param>
        /// <param name="canStop"> canStop </param>
        /// <param name="canResume"> canResume </param>
        /// <param name="updateFrequency"> updateFrequency </param>
        /// <param name="queuedUTC"> queuedUTC </param>
        /// <param name="completeRatio"> completeRatio </param>
        /// <param name="status"> status </param>
        /// <param name="serializedTaskObject"> serializedTaskObject </param>
        /// <param name="serializedTaskType"> serializedTaskType </param>
        /// <returns>int</returns>
        public int Create(
            Guid guid,
            Guid siteGuid,
            Guid queuedBy,
            string taskName,
            bool notifyOnCompletion,
            string notificationToEmail,
            string notificationFromEmail,
            string notificationSubject,
            string taskCompleteMessage,
            bool canStop,
            bool canResume,
            int updateFrequency,
            DateTime queuedUTC,
            double completeRatio,
            string status,
            string serializedTaskObject,
            string serializedTaskType)
        {
            #region Bit Conversion

            int intNotifyOnCompletion;
            if (notifyOnCompletion)
            {
                intNotifyOnCompletion = 1;
            }
            else
            {
                intNotifyOnCompletion = 0;
            }

            int intCanStop;
            if (canStop)
            {
                intCanStop = 1;
            }
            else
            {
                intCanStop = 0;
            }

            int intCanResume;
            if (canResume)
            {
                intCanResume = 1;
            }
            else
            {
                intCanResume = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_TaskQueue (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("QueuedBy, ");
            sqlCommand.Append("TaskName, ");
            sqlCommand.Append("NotifyOnCompletion, ");
            sqlCommand.Append("NotificationToEmail, ");
            sqlCommand.Append("NotificationFromEmail, ");
            sqlCommand.Append("NotificationSubject, ");
            sqlCommand.Append("TaskCompleteMessage, ");
            sqlCommand.Append("CanStop, ");
            sqlCommand.Append("CanResume, ");
            sqlCommand.Append("UpdateFrequency, ");
            sqlCommand.Append("QueuedUTC, ");
            sqlCommand.Append("CompleteRatio, ");
            sqlCommand.Append("Status, ");
            sqlCommand.Append("SerializedTaskObject, ");
            sqlCommand.Append("SerializedTaskType )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":QueuedBy, ");
            sqlCommand.Append(":TaskName, ");
            sqlCommand.Append(":NotifyOnCompletion, ");
            sqlCommand.Append(":NotificationToEmail, ");
            sqlCommand.Append(":NotificationFromEmail, ");
            sqlCommand.Append(":NotificationSubject, ");
            sqlCommand.Append(":TaskCompleteMessage, ");
            sqlCommand.Append(":CanStop, ");
            sqlCommand.Append(":CanResume, ");
            sqlCommand.Append(":UpdateFrequency, ");
            sqlCommand.Append(":QueuedUTC, ");
            sqlCommand.Append(":CompleteRatio, ");
            sqlCommand.Append(":Status, ");
            sqlCommand.Append(":SerializedTaskObject, ");
            sqlCommand.Append(":SerializedTaskType )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[17];

            arParams[0] = new SqliteParameter(":Guid", DbType.String);
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":QueuedBy", DbType.String);
            arParams[2].Value = queuedBy.ToString();

            arParams[3] = new SqliteParameter(":TaskName", DbType.String);
            arParams[3].Value = taskName;

            arParams[4] = new SqliteParameter(":NotifyOnCompletion", DbType.Int32);
            arParams[4].Value = intNotifyOnCompletion;

            arParams[5] = new SqliteParameter(":NotificationToEmail", DbType.String);
            arParams[5].Value = notificationToEmail;

            arParams[6] = new SqliteParameter(":NotificationFromEmail", DbType.String);
            arParams[6].Value = notificationFromEmail;

            arParams[7] = new SqliteParameter(":NotificationSubject", DbType.String);
            arParams[7].Value = notificationSubject;

            arParams[8] = new SqliteParameter(":TaskCompleteMessage", DbType.Object);
            arParams[8].Value = taskCompleteMessage;

            arParams[9] = new SqliteParameter(":CanStop", DbType.Int32);
            arParams[9].Value = intCanStop;

            arParams[10] = new SqliteParameter(":CanResume", DbType.Int32);
            arParams[10].Value = intCanResume;

            arParams[11] = new SqliteParameter(":UpdateFrequency", DbType.Int32);
            arParams[11].Value = updateFrequency;

            arParams[12] = new SqliteParameter(":QueuedUTC", DbType.DateTime);
            arParams[12].Value = queuedUTC;

            arParams[13] = new SqliteParameter(":CompleteRatio", DbType.Double);
            arParams[13].Value = completeRatio;

            arParams[14] = new SqliteParameter(":Status", DbType.String);
            arParams[14].Value = status;

            arParams[15] = new SqliteParameter(":SerializedTaskObject", DbType.Object);
            arParams[15].Value = serializedTaskObject;

            arParams[16] = new SqliteParameter(":SerializedTaskType", DbType.String);
            arParams[16].Value = serializedTaskType;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_TaskQueue table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="startUTC"> startUTC </param>
        /// <param name="completeUTC"> completeUTC </param>
        /// <param name="lastStatusUpdateUTC"> lastStatusUpdateUTC </param>
        /// <param name="completeRatio"> completeRatio </param>
        /// <param name="status"> status </param>
        /// <returns>bool</returns>
        public bool Update(
            Guid guid,
            DateTime startUTC,
            DateTime completeUTC,
            DateTime lastStatusUpdateUTC,
            double completeRatio,
            string status)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_TaskQueue ");
            sqlCommand.Append("SET  ");

            if (startUTC > DateTime.MinValue)
                sqlCommand.Append("StartUTC = :StartUTC, ");

            if (completeUTC > DateTime.MinValue)
                sqlCommand.Append("CompleteUTC = :CompleteUTC, ");

            sqlCommand.Append("LastStatusUpdateUTC = :LastStatusUpdateUTC, ");
            sqlCommand.Append("CompleteRatio = :CompleteRatio, ");
            sqlCommand.Append("Status = :Status ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":Guid", DbType.String);
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":StartUTC", DbType.DateTime);
            arParams[1].Value = startUTC;

            arParams[2] = new SqliteParameter(":CompleteUTC", DbType.DateTime);
            arParams[2].Value = completeUTC;

            arParams[3] = new SqliteParameter(":LastStatusUpdateUTC", DbType.DateTime);
            arParams[3].Value = lastStatusUpdateUTC;

            arParams[4] = new SqliteParameter(":CompleteRatio", DbType.Double);
            arParams[4].Value = completeRatio;

            arParams[5] = new SqliteParameter(":Status", DbType.String);
            arParams[5].Value = status;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Updates a row in the mp_TaskQueue table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="notificationSentUTC"> notificationSentUTC </param>
        /// <returns>bool</returns>
        public bool UpdateNotification(
            Guid guid,
            DateTime notificationSentUTC)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_TaskQueue ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("NotificationSentUTC = :NotificationSentUTC ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":Guid", DbType.String);
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":NotificationSentUTC", DbType.DateTime);
            arParams[1].Value = notificationSentUTC;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_TaskQueue table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String);
            arParams[0].Value = guid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes all completed tasks from mp_TaskQueue table
        /// </summary>
        public void DeleteCompleted()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE CompleteUTC IS NOT NULL; ");

            AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                null);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_TaskQueue table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public DbDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String);
            arParams[0].Value = guid.ToString();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append(";");

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                null));
        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public int GetCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));
        }

        public int GetCountUnfinishedByType(string taskType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SerializedTaskType LIKE :TaskType ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TaskType", DbType.String);
            arParams[0].Value = taskType + "%";

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));

        }

        public bool DeleteByType(string taskType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SerializedTaskType LIKE :TaskType ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TaskType", DbType.String);
            arParams[0].Value = taskType + "%";

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public int GetCountUnfinished()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CompleteUTC IS NULL ");
            sqlCommand.Append(";");

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                null));
        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public int GetCountUnfinished(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append("AND CompleteUTC IS NULL ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_TaskQueue table.
        /// </summary>
        public DbDataReader GetTasksNotStarted()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StartUTC IS NULL ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                null);
        }

        /// <summary>
        /// Gets an IDataReader with all tasks in the mp_TaskQueue table that have completed but not yet sent notification.
        /// </summary>
        public DbDataReader GetTasksForNotification()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("NotifyOnCompletion = 1 ");
            sqlCommand.Append("AND CompleteUTC IS NOT NULL ");
            sqlCommand.Append("AND NotificationSentUTC IS NULL ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                null);
        }


        /// <summary>
        /// Gets an IDataReader with all rows in the mp_TaskQueue table.
        /// </summary>
        public DbDataReader GetUnfinished()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CompleteUTC IS NULL ");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                null);
        }


        /// <summary>
        /// Gets an IDataReader with all rows in the mp_TaskQueue table.
        /// </summary>
        public DbDataReader GetUnfinished(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND CompleteUTC IS NULL ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[0].Value = siteGuid.ToString();

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }



        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public DbDataReader GetPage(
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
            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("QueuedUTC DESC  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[0].Value = pageSize;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }


        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        /// <param name="siteGuid"> guid </param>
        public DbDataReader GetPageBySite(
            Guid siteGuid,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCount(siteGuid);

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
            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("QueuedUTC DESC  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Value = pageSize;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }


        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public DbDataReader GetPageUnfinished(
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCountUnfinished();

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
            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CompleteUTC IS NULL ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("QueuedUTC DESC  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[0].Value = pageSize;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }


        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        /// <param name="siteGuid"> guid </param>
        public DbDataReader GetPageUnfinishedBySite(
            Guid siteGuid,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            //int totalRows = GetCountUnfinished(siteGuid);

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
            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND CompleteUTC IS NULL ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("QueuedUTC DESC  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", :PageSize ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String);
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Value = pageSize;

            return AdoHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString(),
                arParams);
        }

    }
}
