// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-18
//	Last Modified:		    2015-12-26
// 

// TODO: we should update all the async signatures to take a cancellationtoken

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Logging.Web
{
    public interface ILogRepository
    {
        int AddLogItem(
            DateTime logDate,
            string ipAddress,
            string culture,
            string url,
            string shortUrl,
            string thread,
            string logLevel,
            string logger,
            string message);

        Task<int> GetCount();
        Task<List<ILogItem>> GetPageAscending(
            int pageNumber,
            int pageSize);
        Task<List<ILogItem>> GetPageDescending(
            int pageNumber,
            int pageSize);

        Task<bool> DeleteAll();
        Task<bool> Delete(int logItemId);
        Task<bool> DeleteOlderThan(DateTime cutoffDateUtc);
        Task<bool> DeleteByLevel(string logLevel);
    }
}
