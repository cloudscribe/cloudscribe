// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2015-12-07
// 

using cloudscribe.Core.Models.Logging;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Repositories.EF
{
    public class LogRepository : ILogRepository
    {
        public LogRepository(CoreDbContext dbContext)
        {

            this.dbContext = dbContext;
        }

        private CoreDbContext dbContext;

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
            LogItem logItem = new LogItem();
            logItem.LogDateUtc = logDate;
            logItem.IpAddress = ipAddress;
            logItem.Culture = culture;
            logItem.Url = url;
            logItem.ShortUrl = shortUrl;
            logItem.Thread = thread;
            logItem.LogLevel = logLevel;
            logItem.Logger = logger;
            logItem.Message = message;

            dbContext.Add(logItem);
            dbContext.SaveChanges();

            return logItem.Id;
        }

        public async Task<int> GetCount()
        {
            return await dbContext.LogItems.CountAsync<LogItem>();
        }

        
        public async Task<List<ILogItem>> GetPageAscending(
            int pageNumber,
            int pageSize)
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var query = from l in dbContext.LogItems
                        .Take(pageSize)
                        orderby l.Id ascending
                        select l ;

            if (offset > 0) { return await query.Skip(offset).ToListAsync<ILogItem>(); }

            return await query.ToListAsync<ILogItem>();
            
        }

        public async Task<List<ILogItem>> GetPageDescending(
            int pageNumber,
            int pageSize)
        {
            int offset = (pageSize * pageNumber) - pageSize;

            var query = from l in dbContext.LogItems
                        .Take(pageSize)
                        orderby l.Id descending
                        select l;

            if (offset > 0) { return await query.Skip(offset).ToListAsync<ILogItem>(); }

            return await query.ToListAsync<ILogItem>();
            
        }

        public async Task<bool> DeleteAll()
        {
            dbContext.LogItems.RemoveAll();
            int rowsAffected = await dbContext.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> Delete(int logItemId)
        {
            var result = false;
            var itemToRemove = await dbContext.LogItems.SingleOrDefaultAsync(x => x.Id == logItemId);
            if(itemToRemove != null)
            {
                dbContext.LogItems.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync();
                result = rowsAffected > 0;
            }

            return result;
        }

        public async Task<bool> DeleteOlderThan(DateTime cutoffDateUtc)
        {
            var query = from l in dbContext.LogItems
                       where l.LogDateUtc < cutoffDateUtc
                        select l;

            dbContext.LogItems.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync();
            return rowsAffected > 0;
            
        }

        public async Task<bool> DeleteByLevel(string logLevel)
        {
            var query = from l in dbContext.LogItems
                        where l.LogLevel == logLevel
                        select l;

            dbContext.LogItems.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync();
            return rowsAffected > 0;
        }


    }
}
