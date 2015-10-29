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
    public static class DbLoggerFactoryExtensions
    {
        public static ILoggerFactory AddDbLogger(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            ILogRepository logRepository)
        {
            factory.AddProvider(new DbLoggerProvider(factory.MinimumLevel, serviceProvider, logRepository));
            return factory;
        }

    }
}
