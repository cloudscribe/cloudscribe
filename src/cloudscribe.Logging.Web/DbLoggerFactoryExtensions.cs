// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-19
//	Last Modified:		    2015-12-26
// 

using Microsoft.Extensions.Logging;
using System;

namespace cloudscribe.Logging.Web
{
    public static class DbLoggerFactoryExtensions
    {
        public static ILoggerFactory AddDbLogger(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            ILogRepository logRepository,
            LogLevel minimumLogLevel)
        {
            Func<string, LogLevel, bool> logFilter = delegate (string loggerName, LogLevel logLevel)
            {
                if (logLevel < minimumLogLevel) { return false; }
               
                return true;
            };

            factory.AddProvider(new DbLoggerProvider(logFilter, serviceProvider, logRepository));
            return factory;
        }

        public static ILoggerFactory AddDbLogger(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            ILogRepository logRepository,
            Func<string, LogLevel, bool> logFilter)
        {
           
            factory.AddProvider(new DbLoggerProvider(logFilter, serviceProvider, logRepository));
            return factory;
        }

    }
}
