// Author:					Joe Audette
// Created:				    2007-12-29
// Last Modified:			2015-06-09
//
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.MSSQL;
using Microsoft.Framework.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.MSSQL
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
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_TaskQueue_Insert", 
                17);

            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@QueuedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, queuedBy);
            sph.DefineSqlParameter("@TaskName", SqlDbType.NVarChar, 255, ParameterDirection.Input, taskName);
            sph.DefineSqlParameter("@NotifyOnCompletion", SqlDbType.Bit, ParameterDirection.Input, notifyOnCompletion);
            sph.DefineSqlParameter("@NotificationToEmail", SqlDbType.NVarChar, 255, ParameterDirection.Input, notificationToEmail);
            sph.DefineSqlParameter("@NotificationFromEmail", SqlDbType.NVarChar, 255, ParameterDirection.Input, notificationFromEmail);
            sph.DefineSqlParameter("@NotificationSubject", SqlDbType.NVarChar, 255, ParameterDirection.Input, notificationSubject);
            sph.DefineSqlParameter("@TaskCompleteMessage", SqlDbType.NVarChar, -1, ParameterDirection.Input, taskCompleteMessage);
            sph.DefineSqlParameter("@CanStop", SqlDbType.Bit, ParameterDirection.Input, canStop);
            sph.DefineSqlParameter("@CanResume", SqlDbType.Bit, ParameterDirection.Input, canResume);
            sph.DefineSqlParameter("@UpdateFrequency", SqlDbType.Int, ParameterDirection.Input, updateFrequency);
            sph.DefineSqlParameter("@QueuedUTC", SqlDbType.DateTime, ParameterDirection.Input, queuedUTC);
            sph.DefineSqlParameter("@CompleteRatio", SqlDbType.Float, ParameterDirection.Input, completeRatio);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, 255, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@SerializedTaskObject", SqlDbType.NVarChar, -1, ParameterDirection.Input, serializedTaskObject);
            sph.DefineSqlParameter("@SerializedTaskType", SqlDbType.NVarChar, 255, ParameterDirection.Input, serializedTaskType);
            int rowsAffected = sph.ExecuteNonQuery();
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

            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_TaskQueue_Update", 
                6);

            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@StartUTC", SqlDbType.DateTime, ParameterDirection.Input, startUTC);
            sph.DefineSqlParameter("@CompleteUTC", SqlDbType.DateTime, ParameterDirection.Input, completeUTC);
            sph.DefineSqlParameter("@LastStatusUpdateUTC", SqlDbType.DateTime, ParameterDirection.Input, lastStatusUpdateUTC);
            sph.DefineSqlParameter("@CompleteRatio", SqlDbType.Float, ParameterDirection.Input, completeRatio);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, 255, ParameterDirection.Input, status);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        private bool UpdateStatus(
            Guid guid,
            DateTime startUTC,
            DateTime lastStatusUpdateUTC,
            double completeRatio,
            string status)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_TaskQueue_UpdateStart", 
                5);

            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@StartUTC", SqlDbType.DateTime, ParameterDirection.Input, startUTC);
            sph.DefineSqlParameter("@LastStatusUpdateUTC", SqlDbType.DateTime, ParameterDirection.Input, lastStatusUpdateUTC);
            sph.DefineSqlParameter("@CompleteRatio", SqlDbType.Float, ParameterDirection.Input, completeRatio);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, 255, ParameterDirection.Input, status);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);


        }

        private bool UpdateStatus(
            Guid guid,
            DateTime lastStatusUpdateUTC,
            double completeRatio,
            string status)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_TaskQueue_UpdateStatus", 
                4);

            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@LastStatusUpdateUTC", SqlDbType.DateTime, ParameterDirection.Input, lastStatusUpdateUTC);
            sph.DefineSqlParameter("@CompleteRatio", SqlDbType.Float, ParameterDirection.Input, completeRatio);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, 255, ParameterDirection.Input, status);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);


        }

        public bool UpdateNotification(
            Guid guid,
            DateTime notificationSentUtc)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_TaskQueue_UpdateNotification", 
                2);

            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@NotificationSentUTC", SqlDbType.DateTime, ParameterDirection.Input, notificationSentUtc);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);


        }

        /// <summary>
        /// Deletes a row from the mp_TaskQueue table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_TaskQueue_Delete", 
                1);

            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public bool DeleteByType(string taskType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                writeConnectionString, 
                "mp_TaskQueue_DeleteByType", 
                1);

            sph.DefineSqlParameter("@TaskType", SqlDbType.NVarChar, 255, ParameterDirection.Input, taskType);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }


        /// <summary>
        /// Deletes all completed tasks from mp_TaskQueue table
        /// </summary>
        public void DeleteCompleted()
        {

            AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.StoredProcedure,
                "mp_TaskQueue_DeleteCompleted",
                null);

        }

        ///// <summary>
        ///// Deletes a rows from the mp_TaskQueue table. Returns true if row deleted.
        ///// </summary>
        ///// <param name="siteGuid"> siteGuid </param>
        ///// <returns>bool</returns>
        //public static bool DeleteCompleted(Guid siteGuid)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "mp_TaskQueue_DeleteCompletedBySite", 1);
        //    sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > 0);

        //}

        /// <summary>
        /// Gets an IDataReader with one row from the mp_TaskQueue table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public DbDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_TaskQueue_SelectOne", 
                1);

            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();
        }


        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public int GetCount()
        {

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_TaskQueue_GetCount",
                null));

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        public int GetCount(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_TaskQueue_GetCountBySite", 
                1);

            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

            return Convert.ToInt32(sph.ExecuteScalar());


        }

        public int GetCountUnfinishedByType(string taskType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_TaskQueue_CountIncompleteByType", 
                1);

            sph.DefineSqlParameter("@TaskType", SqlDbType.NVarChar, 255, ParameterDirection.Input, taskType);

            return Convert.ToInt32(sph.ExecuteScalar());


        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public int GetCountUnfinished()
        {

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_TaskQueue_GetUnfinishedCount",
                null));

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        public int GetCountUnfinished(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_TaskQueue_GetUnfinishedCountBySite", 
                1);

            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

            return Convert.ToInt32(sph.ExecuteScalar());


        }

        /// <summary>
        /// Gets an IDataReader with all tasks in the mp_TaskQueue table that have not been started yet.
        /// </summary>
        public DbDataReader GetTasksNotStarted()
        {

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_TaskQueue_SelectTasksNotStarted",
                null);

        }


        /// <summary>
        /// Gets an IDataReader with all tasks in the mp_TaskQueue table that have completed but not yet sent notification.
        /// </summary>
        public DbDataReader GetTasksForNotification()
        {

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_TaskQueue_SelectForNotification",
                null);

        }


        /// <summary>
        /// Gets an IDataReader with all incomplete tasks in the mp_TaskQueue table.
        /// </summary>
        public DbDataReader GetUnfinished()
        {

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_TaskQueue_SelectIncomplete",
                null);

        }


        /// <summary>
        /// Gets an IDataReader with all incomplete tasks in the mp_TaskQueue table.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        public DbDataReader GetUnfinished(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_TaskQueue_SelectIncompleteBySite", 
                1);

            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();

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
            //totalPages = 1;
            //int totalRows
            //    = GetCount();

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

            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_TaskQueue_SelectPage", 
                2);

            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

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
            //totalPages = 1;
            //int totalRows
            //    = GetCount(siteGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_TaskQueue_SelectPageBySite", 
                3);

            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

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
            //totalPages = 1;
            //int totalRows
            //    = GetCountUnfinished();

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

            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_TaskQueue_SelectPageIncomplete", 
                2);

            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

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
            //totalPages = 1;
            //int totalRows
            //    = GetCountUnfinished(siteGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(
                logFactory,
                readConnectionString, 
                "mp_TaskQueue_SelectPageIncompleteBySite", 
                3);

            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

    }

}
