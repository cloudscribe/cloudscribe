// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-18
//	Last Modified:		    2015-08-18
// 

using cloudscribe.Core.Models.DataExtensions;
using cloudscribe.Core.Models.Logging;
using cloudscribe.DbHelpers.SqlCe;
using Microsoft.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.SqlCe
{
#pragma warning disable 1998

    public class LogRepository : ILogRepository
    {
        public LogRepository(
            SqlCeConnectionStringResolver connectionStringResolver)
        {
            if (connectionStringResolver == null) { throw new ArgumentNullException(nameof(connectionStringResolver)); }
            //if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }

            //logFactory = loggerFactory;
            //log = loggerFactory.CreateLogger(typeof(LogRepository).FullName);
            connectionString = connectionStringResolver.Resolve();

            dbSystemLog = new DBSystemLog(connectionString);
        }

        //private ILoggerFactory logFactory;
        //private ILogger log;
        private string connectionString;
        private DBSystemLog dbSystemLog;

        public int AddLogItem(
            DateTime logDate,
            string ipAddress,
            string culture,
            string url,
            string shortUrl,
            string thread,
            string logLevel,
            string logger,
            string message)
        {
            return dbSystemLog.Create(
                logDate,
                ipAddress,
                culture,
                url,
                shortUrl,
                thread,
                logLevel,
                logger,
                message);
        }

        public async Task<int> GetCount()
        {
            return dbSystemLog.GetCount();
        }

        public async Task<List<ILogItem>> GetPageAscending(
            int pageNumber,
            int pageSize)
        {
            List<ILogItem> logItems = new List<ILogItem>();
            using (DbDataReader reader = dbSystemLog.GetPageAscending(pageNumber, pageSize))
            {
                while (reader.Read())
                {
                    LogItem logitem = new LogItem();
                    logitem.LoadFromReader(reader);
                    logItems.Add(logitem);
                }
            }

            return logItems;
        }

        public async Task<List<ILogItem>> GetPageDescending(
            int pageNumber,
            int pageSize)
        {
            List<ILogItem> logItems = new List<ILogItem>();
            using (DbDataReader reader = dbSystemLog.GetPageDescending(pageNumber, pageSize))
            {
                while (reader.Read())
                {
                    LogItem logitem = new LogItem();
                    logitem.LoadFromReader(reader);
                    logItems.Add(logitem);
                }
            }

            return logItems;
        }

        public async Task<bool> DeleteAll()
        {
            return dbSystemLog.DeleteAll();
        }

        public async Task<bool> Delete(int logItemId)
        {
            return dbSystemLog.Delete(logItemId);
        }

        public async Task<bool> DeleteOlderThan(DateTime cutoffDateUtc)
        {
            return dbSystemLog.DeleteOlderThan(cutoffDateUtc);
        }

        public async Task<bool> DeleteByLevel(string logLevel)
        {
            return dbSystemLog.DeleteByLevel(logLevel);
        }


    }

#pragma warning restore 1998
}
