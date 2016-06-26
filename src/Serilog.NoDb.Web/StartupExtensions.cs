// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-26
// Last Modified:			2016-06-26
// 

using Microsoft.Extensions.Configuration;
using Serilog.Sinks.NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    
    public static class StartupExtensions
    {
        public static IServiceCollection AddNoDbSerilogWeb(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddNoDbSerilogSink(configuration);

            return services;
        }


    }
}
