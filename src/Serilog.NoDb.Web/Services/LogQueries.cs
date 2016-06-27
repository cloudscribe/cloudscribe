// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-26
// Last Modified:			2016-06-27
// 

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoDb;
using Serilog.Events;
using Serilog.NoDb.Web.Models;
using Serilog.Sinks.NoDb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.NoDb.Web.Services
{
    public class LogQueries
    {
        public LogQueries(
            IBasicQueries<LogEvent> queries,
            IStoragePathResolver<LogEvent> pathResolver,
            IStringSerializer<LogEvent> serializer,
            IOptions<NoDbSinkOptions> optionsAccessor,
            ILogger<LogQueries> logger
            )
        {
            query = queries;
            this.serializer = serializer;
            this.pathResolver = pathResolver;
            options = optionsAccessor.Value;
            log = logger;
        }

        private IBasicQueries<LogEvent> query;
        private IStringSerializer<LogEvent> serializer;
        private IStoragePathResolver<LogEvent> pathResolver;
        private NoDbSinkOptions options;
        private ILogger<LogQueries> log;

        public virtual async Task<PagedQueryResult> GetPageAsync(
            LogEventLevel level,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFolder = await pathResolver.ResolvePath(options.ProjectId).ConfigureAwait(false);
            pathToFolder = Path.Combine(pathToFolder, level.ToString());


            var result = new PagedQueryResult();
            if (!Directory.Exists(pathToFolder)) return result;

            int offset = (pageSize * pageNumber) - pageSize;
            int skipped = 0;
            int added = 0;

            foreach (string file in Directory.EnumerateFiles(
                pathToFolder,
                "*" + serializer.ExpectedFileExtension,
                SearchOption.AllDirectories).OrderByDescending(f => f) // this is needed since events are nested below date folders
                )
            {
                if(offset > 0)
                {
                    if (skipped < offset)
                    {
                        skipped += 1;
                        result.TotalItems += 1;
                        continue;
                    }
                }
                

                if(added >= pageSize)
                {
                    result.TotalItems += 1;
                    continue;
                }

                var key = Path.GetFileNameWithoutExtension(file);
                var obj = LoadObject(file, key);
                result.Items.Add(new LogEventWrapper(obj,key));
                added += 1;
                result.TotalItems += 1;
            }

            return result;

        }

        protected LogEvent LoadObject(string pathToFile, string key)
        {
            using (StreamReader reader = File.OpenText(pathToFile))
            {
                var payload = reader.ReadToEnd();
                var result = serializer.Deserialize(payload, key);
                return result;
            }
        }

        private bool _disposed;

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// Dispose the store
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }

    }
}
