// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-26
// Last Modified:			2016-06-26
// 

using Microsoft.Extensions.Options;
using NoDb;
using Serilog.Events;
using Serilog.Sinks.NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serilog.NoDb.Web.Services
{
    public class LogCommands
    {
        public LogCommands(
            IBasicCommands<LogEvent> commands,
            IBasicQueries<LogEvent> queries,
            IOptions<NoDbSinkOptions> optionsAccessor)
        {
            this.commands = commands;
            query = queries;
            options = optionsAccessor.Value;
        }

        private IBasicCommands<LogEvent> commands;
        private IBasicQueries<LogEvent> query;
        private NoDbSinkOptions options;




    }
}
