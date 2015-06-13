// Author:					Joe Audette
// Created:				    2007-12-30
// Last Modified:			2015-06-13
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[17];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter("queuedby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Value = queuedBy.ToString();

            arParams[3] = new NpgsqlParameter("taskname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Value = taskName;

            arParams[4] = new NpgsqlParameter("notifyoncompletion", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Value = notifyOnCompletion;

            arParams[5] = new NpgsqlParameter("notificationtoemail", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Value = notificationToEmail;

            arParams[6] = new NpgsqlParameter("notificationfromemail", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[6].Value = notificationFromEmail;

            arParams[7] = new NpgsqlParameter("notificationsubject", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[7].Value = notificationSubject;

            arParams[8] = new NpgsqlParameter("taskcompletemessage", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[8].Value = taskCompleteMessage;

            arParams[9] = new NpgsqlParameter("canstop", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[9].Value = canStop;

            arParams[10] = new NpgsqlParameter("canresume", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[10].Value = canResume;

            arParams[11] = new NpgsqlParameter("updatefrequency", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[11].Value = updateFrequency;

            arParams[12] = new NpgsqlParameter("queuedutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[12].Value = queuedUTC;

            arParams[13] = new NpgsqlParameter("completeratio", NpgsqlTypes.NpgsqlDbType.Double);
            arParams[13].Value = completeRatio;

            arParams[14] = new NpgsqlParameter("status", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[14].Value = status;

            arParams[15] = new NpgsqlParameter("serializedtaskobject", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[15].Value = serializedTaskObject;

            arParams[16] = new NpgsqlParameter("serializedtasktype", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[16].Value = serializedTaskType;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_insert(:guid,:siteguid,:queuedby,:taskname,:notifyoncompletion,:notificationtoemail,:notificationfromemail,:notificationsubject,:taskcompletemessage,:canstop,:canresume,:updatefrequency,:queuedutc,:completeratio,:status,:serializedtaskobject,:serializedtasktype)",
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[6];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("startutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Value = startUTC;

            arParams[2] = new NpgsqlParameter("completeutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Value = completeUTC;

            arParams[3] = new NpgsqlParameter("laststatusupdateutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Value = lastStatusUpdateUTC;

            arParams[4] = new NpgsqlParameter("completeratio", NpgsqlTypes.NpgsqlDbType.Double);
            arParams[4].Value = completeRatio;

            arParams[5] = new NpgsqlParameter("status", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Value = status;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_update(:guid,:startutc,:completeutc,:laststatusupdateutc,:completeratio,:status)",
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("startutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Value = startUTC;

            arParams[2] = new NpgsqlParameter("laststatusupdateutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Value = lastStatusUpdateUTC;

            arParams[3] = new NpgsqlParameter("completeratio", NpgsqlTypes.NpgsqlDbType.Double);
            arParams[3].Value = completeRatio;

            arParams[4] = new NpgsqlParameter("status", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Value = status;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_updatestart(:guid,:startutc,:laststatusupdateutc,:completeratio,:status)",
                arParams);

            return (rowsAffected > -1);

        }

        private bool UpdateStatus(
            Guid guid,
            DateTime lastStatusUpdateUTC,
            double completeRatio,
            string status)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("laststatusupdateutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Value = lastStatusUpdateUTC;

            arParams[2] = new NpgsqlParameter("completeratio", NpgsqlTypes.NpgsqlDbType.Double);
            arParams[2].Value = completeRatio;

            arParams[3] = new NpgsqlParameter("status", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Value = status;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_updatestatus(:guid,:laststatusupdateutc,:completeratio,:status)",
                arParams);

            return (rowsAffected > -1);

        }


        public bool UpdateNotification(
            Guid guid,
            DateTime notificationSentUtc)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("notificationsentutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Value = notificationSentUtc;

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_updatenotification(:guid,:notificationsentutc)",
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_delete(:guid)",
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_TaskQueue table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public DbDataReader GetOne(Guid guid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = guid.ToString();

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_select_one(:guid)",
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public int GetCount()
        {

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_count()",
                null));

        }


        /// <summary>
        /// Deletes all completed tasks from mp_TaskQueue table
        /// </summary>
        public void DeleteCompleted()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_taskqueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("completeutc IS NOT NULL ");
            sqlCommand.Append(";");


            AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                null);



            //AdoHelper.ExecuteNonQuery(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_taskqueue_deletecompleted()",
            //    null);

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public int GetCount(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_countbysite(:siteguid)",
                arParams));

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public int GetCountUnfinished()
        {

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_countunfinished()",
                null));

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public int GetCountUnfinished(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_countunfinishedbysite(:siteguid)",
                arParams));

        }

        public int GetCountUnfinishedByType(string taskType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_taskqueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("serializedtasktype LIKE :tasktype ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("tasktype", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[0].Value = taskType + "%";

            return Convert.ToInt32(AdoHelper.ExecuteScalar(
               readConnectionString,
               CommandType.Text,
               sqlCommand.ToString(),
               arParams));


        }

        public bool DeleteByType(string taskType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_taskqueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("serializedtasktype LIKE :tasktype ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("tasktype", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[0].Value = taskType + "%";

            int rowsAffected = AdoHelper.ExecuteNonQuery(
                writeConnectionString,
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_TaskQueue table.
        /// </summary>
        public DbDataReader GetTasksNotStarted()
        {
            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_select_tasksnotstarted()",
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
                "mp_taskqueue_select_tasksfornotification()",
                null);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_TaskQueue table.
        /// </summary>
        public DbDataReader GetUnfinished()
        {
            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_select_tasksnotfinished()",
                null);

        }


        /// <summary>
        /// Gets an IDataReader with all rows in the mp_TaskQueue table.
        /// </summary>
        public DbDataReader GetUnfinished(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_select_tasksnotfinishedbysite(:siteguid)",
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = pageNumber;

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageSize;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_selectpage(:pagenumber,:pagesize)",
                arParams);

        }

        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageNumber;

            arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageSize;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_selectpagebysite(:siteguid,:pagenumber,:pagesize)",
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Value = pageNumber;

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageSize;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_selectpageunfinished(:pagenumber,:pagesize)",
                arParams);

        }


        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Value = pageNumber;

            arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Value = pageSize;

            return AdoHelper.ExecuteReader(
                readConnectionString,
                CommandType.StoredProcedure,
                "mp_taskqueue_selectpageunfinishedbysite(:siteguid,:pagenumber,:pagesize)",
                arParams);

        }


    }
}

