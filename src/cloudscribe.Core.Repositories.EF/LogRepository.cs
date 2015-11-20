// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2015-11-20
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Logging;

namespace cloudscribe.Core.Repositories.EF
{
    public class LogRepository
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
            //var count = (from l in dbContext.LogItems ).
            //TODO:make this async how?

            return dbContext.LogItems.Count<LogItem>();
        }

        
        public async Task<List<ILogItem>> GetPageAscending(
            int pageNumber,
            int pageSize)
        {
            int offset = pageNumber == 1 ? 0 : (pageSize * (pageNumber - 1));

            var query = from l in dbContext.LogItems
                        .Skip(offset)
                        .Take(pageSize)
                        select l ;

            List<LogItem> items = await query.ToAsyncEnumerable<LogItem>().ToList<LogItem>();
            // this is supposed to return List<ILogItem>
            // how to convert it?

            List<ILogItem> result = new List<ILogItem>(items);
            //result.AddRange(items); // will this work?

            return result;

        }


    }
}
