// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-19
//	Last Modified:		    2015-12-03
// 

using cloudscribe.Core.Models.Logging;
using Microsoft.Extensions.Logging;
using System;

namespace cloudscribe.Core.Web.Components.Logging
{
    public static class DbLoggerFactoryExtensions
    {
        public static ILoggerFactory AddDbLogger(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            ILogRepository logRepository,
            LogLevel minimumLogLevel)
        {
            factory.AddProvider(new DbLoggerProvider(minimumLogLevel, serviceProvider, logRepository));
            return factory;
        }

    }
}
