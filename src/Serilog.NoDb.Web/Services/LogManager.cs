// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-26
// Last Modified:			2016-06-26
// 

using Microsoft.AspNetCore.Http;
using Serilog.Events;
using Serilog.NoDb.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.NoDb.Web.Services
{
    public class LogManager
    {
        public LogManager(
            LogQueries queries,
            IHttpContextAccessor contextAccessor
            )
        {
            this.queries = queries;
            httpContext = contextAccessor?.HttpContext;
        }

        private LogQueries queries;
        private readonly HttpContext httpContext;
        private CancellationToken CancellationToken => httpContext?.RequestAborted ?? CancellationToken.None;

        public int LogPageSize { get; set; } = 10;

        public async Task<PagedQueryResult> GetPageAsync(
            LogEventLevel level,
            int pageNumber,
            int pageSize
            )
        {
            return await queries.GetPageAsync(level, pageNumber, pageSize, CancellationToken);
        }

    }
}
