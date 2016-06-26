// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-25
// Last Modified:			2016-06-25
// 

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NoDb;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddNoDbSerilogSink(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<NoDbSinkOptions>(configuration.GetSection("MultiTenantOptions"));
            services.TryAddSingleton<IStoragePathResolver<LogEvent>, LogEventStoragePathResolver>();
            services.AddNoDb<LogEvent>();
            services.AddSingleton<NoDbSink, NoDbSink>();

            return services;
        }

        public static LoggerConfiguration NoDb(
            this LoggerSinkConfiguration sinkConfiguration,
            NoDbSink sink,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null
            
            )
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (sink == null) throw new ArgumentNullException(nameof(sink));

            return sinkConfiguration.Sink(sink, restrictedToMinimumLevel, levelSwitch);
        }
    }
}
