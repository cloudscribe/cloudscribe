// Author:					Joe Audette
// Created:					2010-04-06
// Last Modified:			2015-06-15
// 
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.SqlCe;
using Microsoft.Framework.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Globalization;
using System.Text;

namespace cloudscribe.Core.Repositories.SqlCe
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_TaskQueue ");
            sqlCommand.Append("(");
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
            sqlCommand.Append("SerializedTaskType ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
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
            sqlCommand.Append("@SerializedTaskType ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[17];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@QueuedBy", SqlDbType.UniqueIdentifier);
            arParams[2].Value = queuedBy;

            arParams[3] = new SqlCeParameter("@TaskName", SqlDbType.NVarChar, 255);
            arParams[3].Value = taskName;

            arParams[4] = new SqlCeParameter("@NotifyOnCompletion", SqlDbType.Bit);
            arParams[4].Value = notifyOnCompletion;

            arParams[5] = new SqlCeParameter("@NotificationToEmail", SqlDbType.NVarChar, 255);
            arParams[5].Value = notificationToEmail;

            arParams[6] = new SqlCeParameter("@NotificationFromEmail", SqlDbType.NVarChar, 255);
            arParams[6].Value = notificationFromEmail;

            arParams[7] = new SqlCeParameter("@NotificationSubject", SqlDbType.NVarChar, 255);
            arParams[7].Value = notificationSubject;

            arParams[8] = new SqlCeParameter("@TaskCompleteMessage", SqlDbType.NText);
            arParams[8].Value = taskCompleteMessage;

            arParams[9] = new SqlCeParameter("@CanStop", SqlDbType.Bit);
            arParams[9].Value = canStop;

            arParams[10] = new SqlCeParameter("@CanResume", SqlDbType.Bit);
            arParams[10].Value = canResume;

            arParams[11] = new SqlCeParameter("@UpdateFrequency", SqlDbType.Int);
            arParams[11].Value = updateFrequency;

            arParams[12] = new SqlCeParameter("@QueuedUTC", SqlDbType.DateTime);
            arParams[12].Value = queuedUTC;

            arParams[13] = new SqlCeParameter("@CompleteRatio", SqlDbType.Float);
            arParams[13].Value = completeRatio;

            arParams[14] = new SqlCeParameter("@Status", SqlDbType.NVarChar, 255);
            arParams[14].Value = status;

            arParams[15] = new SqlCeParameter("@SerializedTaskObject", SqlDbType.NText);
            arParams[15].Value = serializedTaskObject;

            arParams[16] = new SqlCeParameter("@SerializedTaskType", SqlDbType.NVarChar, 255);
            arParams[16].Value = serializedTaskType;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
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
            if ((startUTC == DateTime.MinValue) && (completeUTC == DateTime.MinValue))
            {
                return UpdateStatus(guid, lastStatusUpdateUTC, completeRatio, status);
            }

            if (completeUTC == DateTime.MinValue)
            {
                return UpdateStatus(guid, startUTC, lastStatusUpdateUTC, completeRatio, status);

            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_TaskQueue ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("StartUTC = @StartUTC, ");
            sqlCommand.Append("CompleteUTC = @CompleteUTC, ");
            sqlCommand.Append("LastStatusUpdateUTC = @LastStatusUpdateUTC, ");
            sqlCommand.Append("CompleteRatio = @CompleteRatio, ");
            sqlCommand.Append("Status = @Status ");


            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@StartUTC", SqlDbType.DateTime);
            arParams[1].Value = startUTC;

            arParams[2] = new SqlCeParameter("@CompleteUTC", SqlDbType.DateTime);
            arParams[2].Value = completeUTC;

            arParams[3] = new SqlCeParameter("@LastStatusUpdateUTC", SqlDbType.DateTime);
            arParams[3].Value = lastStatusUpdateUTC;

            arParams[4] = new SqlCeParameter("@CompleteRatio", SqlDbType.Float);
            arParams[4].Value = completeRatio;

            arParams[5] = new SqlCeParameter("@Status", SqlDbType.NVarChar, 255);
            arParams[5].Value = status;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        private bool UpdateStatus(
            Guid guid,
            DateTime startUTC,
            DateTime lastStatusUpdateUTC,
            double completeRatio,
            string status)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_TaskQueue ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("StartUTC = @StartUTC, ");
            sqlCommand.Append("LastStatusUpdateUTC = @LastStatusUpdateUTC, ");
            sqlCommand.Append("CompleteRatio = @CompleteRatio, ");
            sqlCommand.Append("Status = @Status ");


            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@StartUTC", SqlDbType.DateTime);
            arParams[1].Value = startUTC;

            arParams[2] = new SqlCeParameter("@LastStatusUpdateUTC", SqlDbType.DateTime);
            arParams[2].Value = lastStatusUpdateUTC;

            arParams[3] = new SqlCeParameter("@CompleteRatio", SqlDbType.Float);
            arParams[3].Value = completeRatio;

            arParams[4] = new SqlCeParameter("@Status", SqlDbType.NVarChar, 255);
            arParams[4].Value = status;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        private bool UpdateStatus(
            Guid guid,
            DateTime lastStatusUpdateUTC,
            double completeRatio,
            string status)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_TaskQueue ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("LastStatusUpdateUTC = @LastStatusUpdateUTC, ");
            sqlCommand.Append("CompleteRatio = @CompleteRatio, ");
            sqlCommand.Append("Status = @Status ");


            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@LastStatusUpdateUTC", SqlDbType.DateTime);
            arParams[1].Value = lastStatusUpdateUTC;

            arParams[2] = new SqlCeParameter("@CompleteRatio", SqlDbType.Float);
            arParams[2].Value = completeRatio;

            arParams[3] = new SqlCeParameter("@Status", SqlDbType.NVarChar, 255);
            arParams[3].Value = status;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public bool UpdateNotification(
            Guid guid,
            DateTime notificationSentUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_TaskQueue ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("NotificationSentUTC = @NotificationSentUTC ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@NotificationSentUTC", SqlDbType.DateTime);
            arParams[1].Value = notificationSentUtc;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
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
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = guid;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes all completed tasks from mp_TaskQueue table
        /// </summary>
        public void DeleteCompleted()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("[CompleteUTC] IS NOT NULL ");
            sqlCommand.Append(";");

            AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
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
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = guid;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
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

            //SqlCeParameter[] arParams = new SqlCeParameter[1];

            //arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            //arParams[0].Value = applicationId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null));

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        public int GetCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = siteGuid;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

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
            sqlCommand.Append("[CompleteUTC] IS NULL ");
            sqlCommand.Append(";");

            //SqlCeParameter[] arParams = new SqlCeParameter[1];

            //arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            //arParams[0].Value = applicationId;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null));

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        public int GetCountUnfinished(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("[CompleteUTC] IS NULL ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = siteGuid;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public int GetCountUnfinishedByType(string taskType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SerializedTaskType LIKE @TaskType + '%' ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@TaskType", SqlDbType.NVarChar, 255);
            arParams[0].Value = taskType;

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public bool DeleteByType(string taskType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SerializedTaskType LIKE @TaskType + '%' ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@TaskType", SqlDbType.NVarChar, 255);
            arParams[0].Value = taskType;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            sqlCommand.Append("[NotifyOnCompletion] = 1 ");
            sqlCommand.Append("AND [CompleteUTC] IS NOT NULL ");
            sqlCommand.Append("AND [NotificationSentUTC] IS NULL ");
            sqlCommand.Append("ORDER BY [QueuedUTC] ");
            sqlCommand.Append(";");

            //SqlCeParameter[] arParams = new SqlCeParameter[1];

            //arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            //arParams[0].Value = applicationId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }

        /// <summary>
        /// Gets an IDataReader with all tasks in the mp_TaskQueue table that have not been started yet.
        /// </summary>
        public DbDataReader GetTasksNotStarted()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StartUTC IS NULL ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("[QueuedUTC]");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }

        /// <summary>
        /// Gets an IDataReader with all incomplete tasks in the mp_TaskQueue table.
        /// </summary>
        public DbDataReader GetUnfinished()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CompleteUTC IS NULL ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("[QueuedUTC]");
            sqlCommand.Append(";");

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }

        /// <summary>
        /// Gets an IDataReader with all incomplete tasks in the mp_TaskQueue table.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        public DbDataReader GetUnfinished(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("CompleteUTC IS NULL ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("[QueuedUTC]");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = siteGuid;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") * ");

            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("[QueuedUTC] DESC  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            //SqlCeParameter[] arParams = new SqlCeParameter[1];

            //arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            //arParams[0].Value = applicationId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }

        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") * ");

            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("[QueuedUTC] DESC  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = siteGuid;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") * ");

            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("[CompleteUTC] IS NULL ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("[QueuedUTC] DESC  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            //SqlCeParameter[] arParams = new SqlCeParameter[1];

            //arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            //arParams[0].Value = applicationId;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") * ");

            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("[CompleteUTC] IS NULL ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("[QueuedUTC] DESC  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Value = siteGuid;

            return AdoHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
