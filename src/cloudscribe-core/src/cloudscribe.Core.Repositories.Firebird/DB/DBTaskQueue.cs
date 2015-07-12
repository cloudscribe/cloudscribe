// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2007-12-30
// Last Modified:			2015-06-12
// 


using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
using cloudscribe.DbHelpers.Firebird;
using Microsoft.Framework.Logging;

namespace cloudscribe.Core.Repositories.Firebird
{
    internal class DBTaskQueue
    {
        
        internal DBTaskQueue(
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

            FbParameter[] arParams = new FbParameter[17];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@QueuedBy", FbDbType.Char, 36);
            arParams[2].Value = queuedBy.ToString();

            arParams[3] = new FbParameter("@TaskName", FbDbType.VarChar, 255);
            arParams[3].Value = taskName;

            arParams[4] = new FbParameter("@NotifyOnCompletion", FbDbType.SmallInt);
            arParams[4].Value = intNotifyOnCompletion;

            arParams[5] = new FbParameter("@NotificationToEmail", FbDbType.VarChar, 255);
            arParams[5].Value = notificationToEmail;

            arParams[6] = new FbParameter("@NotificationFromEmail", FbDbType.VarChar, 255);
            arParams[6].Value = notificationFromEmail;

            arParams[7] = new FbParameter("@NotificationSubject", FbDbType.VarChar, 255);
            arParams[7].Value = notificationSubject;

            arParams[8] = new FbParameter("@TaskCompleteMessage", FbDbType.VarChar);
            arParams[8].Value = taskCompleteMessage;

            arParams[9] = new FbParameter("@CanStop", FbDbType.SmallInt);
            arParams[9].Value = intCanStop;

            arParams[10] = new FbParameter("@CanResume", FbDbType.SmallInt);
            arParams[10].Value = intCanResume;

            arParams[11] = new FbParameter("@UpdateFrequency", FbDbType.Integer);
            arParams[11].Value = updateFrequency;

            arParams[12] = new FbParameter("@QueuedUTC", FbDbType.TimeStamp);
            arParams[12].Value = queuedUTC;

            arParams[13] = new FbParameter("@CompleteRatio", FbDbType.Float);
            arParams[13].Value = completeRatio;

            arParams[14] = new FbParameter("@Status", FbDbType.VarChar, 255);
            arParams[14].Value = status;

            arParams[15] = new FbParameter("@SerializedTaskObject", FbDbType.VarChar);
            arParams[15].Value = serializedTaskObject;

            arParams[16] = new FbParameter("@SerializedTaskType", FbDbType.VarChar, 255);
            arParams[16].Value = serializedTaskType;

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
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@QueuedBy, ");
            sqlCommand.Append("@TaskName, ");
            sqlCommand.Append("@NotifyOnCompletion, ");
            sqlCommand.Append("@NotificationToEmail, ");
            sqlCommand.Append("@NotificationFromEmail, ");
            sqlCommand.Append("@NotificationSubject, ");
            sqlCommand.Append("@TaskCompleteMessage, ");
            sqlCommand.Append("@CanStop, ");
            sqlCommand.Append("@CanResume, ");
            sqlCommand.Append("@UpdateFrequency, ");
            sqlCommand.Append("@QueuedUTC, ");
            sqlCommand.Append("@CompleteRatio, ");
            sqlCommand.Append("@Status, ");
            sqlCommand.Append("@SerializedTaskObject, ");
            sqlCommand.Append("@SerializedTaskType );");

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
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
                sqlCommand.Append("StartUTC = @StartUTC, ");

            if (completeUTC > DateTime.MinValue)
                sqlCommand.Append("CompleteUTC = @CompleteUTC, ");

            sqlCommand.Append("LastStatusUpdateUTC = @LastStatusUpdateUTC, ");

            sqlCommand.Append("CompleteRatio = @CompleteRatio, ");
            sqlCommand.Append("Status = @Status ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ;");

            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@StartUTC", FbDbType.TimeStamp);
            arParams[1].Value = startUTC;

            arParams[2] = new FbParameter("@CompleteUTC", FbDbType.TimeStamp);
            arParams[2].Value = completeUTC;

            arParams[3] = new FbParameter("@LastStatusUpdateUTC", FbDbType.TimeStamp);
            arParams[3].Value = lastStatusUpdateUTC;

            arParams[4] = new FbParameter("@CompleteRatio", FbDbType.Float);
            arParams[4].Value = completeRatio;

            arParams[5] = new FbParameter("@Status", FbDbType.VarChar, 255);
            arParams[5].Value = status;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
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
            sqlCommand.Append("NotificationSentUTC = @NotificationSentUTC ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@NotificationSentUTC", FbDbType.TimeStamp);
            arParams[1].Value = notificationSentUTC;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
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
            sqlCommand.Append("Guid = @Guid ;");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool DeleteByType(string taskType)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UPPER(SerializedTaskType) LIKE UPPER(@TaskType) ");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@TaskType", FbDbType.VarChar, 255);
            arParams[0].Value = taskType + "%";

            int rowsAffected = AdoHelper.ExecuteNonQuery(
               writeConnectionString,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
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
            sqlCommand.Append("Guid = @Guid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

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
                writeConnectionString,
                sqlCommand.ToString(),
                null);

        }


        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ;");

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
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
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                sqlCommand.ToString(),
                arParams));

        }

        public int GetCountUnfinishedByType(string taskType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UPPER(SerializedTaskType) LIKE UPPER(@TaskType) ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@TaskType", FbDbType.VarChar, 255);
            arParams[0].Value = taskType + "%";

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                sqlCommand.ToString(),
                arParams));
        }


        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table that have not been marked as complete.
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
                readConnectionString,
                sqlCommand.ToString(),
                null));

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table that have not been marked as complete.
        /// </summary>
        public int GetCountUnfinished(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND CompleteUTC IS NULL ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                sqlCommand.ToString(),
                arParams));

        }


        /// <summary>
        /// Gets an IDataReader with all rows in the mp_TaskQueue table that have not been marked as started
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
                readConnectionString,
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
                readConnectionString,
                sqlCommand.ToString(),
                null);

        }


        /// <summary>
        /// Gets an IDataReader with all rows in the mp_TaskQueue table that have not been marked as started
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
                readConnectionString,
                sqlCommand.ToString(),
                null);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_TaskQueue table that have not been marked as started
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        public DbDataReader GetUnfinished(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND CompleteUTC IS NULL ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }


        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString() + " ");
            sqlCommand.Append("	SKIP " + pageLowerBound.ToString() + " ");
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("QueuedUTC DESC  ");
            sqlCommand.Append("	; ");


            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                null);

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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString() + " ");
            sqlCommand.Append("	SKIP " + pageLowerBound.ToString() + " ");
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();


            return AdoHelper.ExecuteReader(
                readConnectionString,
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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString() + " ");
            sqlCommand.Append("	SKIP " + pageLowerBound.ToString() + " ");
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CompleteUTC IS NULL ");
            sqlCommand.Append("	; ");

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                null);

        }

        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString() + " ");
            sqlCommand.Append("	SKIP " + pageLowerBound.ToString() + " ");
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND CompleteUTC IS NULL ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            return AdoHelper.ExecuteReader(
                readConnectionString,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
