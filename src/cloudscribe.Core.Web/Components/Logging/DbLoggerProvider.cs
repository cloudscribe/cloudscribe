// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-19
//	Last Modified:		    2015-08-19
// 

using cloudscribe.Core.Models.Logging;
using Microsoft.Framework.Logging;
using System;

namespace cloudscribe.Core.Web.Components.Logging
{
    public class DbLoggerProvider : ILoggerProvider
    {
        public DbLoggerProvider(
            LogLevel minimumLevel,
            IServiceProvider serviceProvider,
            ILogRepository logRepository)
        {
            logRepo = logRepository;
            services = serviceProvider;
            this.minimumLevel = minimumLevel;
        }

        private ILogRepository logRepo;
        private IServiceProvider services;
        private LogLevel minimumLevel;

        public ILogger CreateLogger(string name)
        {
            return new DbLogger(name, minimumLevel, services, logRepo);
        }

        public void Dispose()
        {
        }

    }
}
