// Author:					Joe Audette
// Created:				    2007-12-29
// Last Modified:			2014-08-29
//
// You must not remove this notice, or any other, from this software.

using cloudscribe.DbHelpers.MSSQL;
using System;
using System.Data;

namespace cloudscribe.Core.Repositories.MSSQL
{

    internal static class DBTaskQueue
    {
       
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
        public static int Create(
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TaskQueue_Insert", 17);
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
        public static bool Update(
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TaskQueue_Update", 6);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@StartUTC", SqlDbType.DateTime, ParameterDirection.Input, startUTC);
            sph.DefineSqlParameter("@CompleteUTC", SqlDbType.DateTime, ParameterDirection.Input, completeUTC);
            sph.DefineSqlParameter("@LastStatusUpdateUTC", SqlDbType.DateTime, ParameterDirection.Input, lastStatusUpdateUTC);
            sph.DefineSqlParameter("@CompleteRatio", SqlDbType.Float, ParameterDirection.Input, completeRatio);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, 255, ParameterDirection.Input, status);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        private static bool UpdateStatus(
            Guid guid,
            DateTime startUTC,
            DateTime lastStatusUpdateUTC,
            double completeRatio,
            string status)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TaskQueue_UpdateStart", 5);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@StartUTC", SqlDbType.DateTime, ParameterDirection.Input, startUTC);
            sph.DefineSqlParameter("@LastStatusUpdateUTC", SqlDbType.DateTime, ParameterDirection.Input, lastStatusUpdateUTC);
            sph.DefineSqlParameter("@CompleteRatio", SqlDbType.Float, ParameterDirection.Input, completeRatio);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, 255, ParameterDirection.Input, status);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);


        }

        private static bool UpdateStatus(
            Guid guid,
            DateTime lastStatusUpdateUTC,
            double completeRatio,
            string status)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TaskQueue_UpdateStatus", 4);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@LastStatusUpdateUTC", SqlDbType.DateTime, ParameterDirection.Input, lastStatusUpdateUTC);
            sph.DefineSqlParameter("@CompleteRatio", SqlDbType.Float, ParameterDirection.Input, completeRatio);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, 255, ParameterDirection.Input, status);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);


        }

        public static bool UpdateNotification(
            Guid guid,
            DateTime notificationSentUtc)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TaskQueue_UpdateNotification", 2);
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
        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TaskQueue_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByType(string taskType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TaskQueue_DeleteByType", 1);
            sph.DefineSqlParameter("@TaskType", SqlDbType.NVarChar, 255, ParameterDirection.Input, taskType);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }


        /// <summary>
        /// Deletes all completed tasks from mp_TaskQueue table
        /// </summary>
        public static void DeleteCompleted()
        {

            AdoHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_TaskQueue_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();
        }


        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public static int GetCount()
        {

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_TaskQueue_GetCount",
                null));

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        public static int GetCount(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_TaskQueue_GetCountBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

            return Convert.ToInt32(sph.ExecuteScalar());


        }

        public static int GetCountUnfinishedByType(string taskType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_TaskQueue_CountIncompleteByType", 1);
            sph.DefineSqlParameter("@TaskType", SqlDbType.NVarChar, 255, ParameterDirection.Input, taskType);

            return Convert.ToInt32(sph.ExecuteScalar());


        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public static int GetCountUnfinished()
        {

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_TaskQueue_GetUnfinishedCount",
                null));

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        public static int GetCountUnfinished(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_TaskQueue_GetUnfinishedCountBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

            return Convert.ToInt32(sph.ExecuteScalar());


        }

        /// <summary>
        /// Gets an IDataReader with all tasks in the mp_TaskQueue table that have not been started yet.
        /// </summary>
        public static IDataReader GetTasksNotStarted()
        {

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_TaskQueue_SelectTasksNotStarted",
                null);

        }


        /// <summary>
        /// Gets an IDataReader with all tasks in the mp_TaskQueue table that have completed but not yet sent notification.
        /// </summary>
        public static IDataReader GetTasksForNotification()
        {

            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_TaskQueue_SelectForNotification",
                null);

        }


        /// <summary>
        /// Gets an IDataReader with all incomplete tasks in the mp_TaskQueue table.
        /// </summary>
        public static IDataReader GetUnfinished()
        {
            
            return AdoHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_TaskQueue_SelectIncomplete",
                null);

        }


        /// <summary>
        /// Gets an IDataReader with all incomplete tasks in the mp_TaskQueue table.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        public static IDataReader GetUnfinished(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_TaskQueue_SelectIncompleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public static IDataReader GetPage(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCount();

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_TaskQueue_SelectPage", 2);
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
        public static IDataReader GetPageBySite(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCount(siteGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_TaskQueue_SelectPageBySite", 3);
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
        public static IDataReader GetPageUnfinished(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCountUnfinished();

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_TaskQueue_SelectPageIncomplete", 2);
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
        public static IDataReader GetPageUnfinishedBySite(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCountUnfinished(siteGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_TaskQueue_SelectPageIncompleteBySite", 3);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

    }

}
