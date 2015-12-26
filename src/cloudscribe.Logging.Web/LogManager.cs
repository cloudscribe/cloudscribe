// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2015-12-23
//	Last Modified:		    2015-12-26
// 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Logging.Web
{
    public class LogManager
    {
        public LogManager(ILogRepository logRepository)
        {
            logRepo = logRepository;
        }

        private ILogRepository logRepo;

        public int LogPageSize { get; set; } = 10;

        public async Task<int> GetLogItemCount()
        {
            return await logRepo.GetCount();
        }

        public async Task<List<ILogItem>> GetLogsDescending(int pageNumber, int pageSize)
        {
            return await logRepo.GetPageDescending(pageNumber, pageSize);
        }

        public async Task<List<ILogItem>> GetLogsAscending(int pageNumber, int pageSize)
        {
            return await logRepo.GetPageAscending(pageNumber, pageSize);
        }

        public async Task<bool> DeleteLogItem(int id)
        {
            return await logRepo.Delete(id);
        }

        public async Task<bool> DeleteAllLogItems()
        {
            return await logRepo.DeleteAll();
        }

    }
}
