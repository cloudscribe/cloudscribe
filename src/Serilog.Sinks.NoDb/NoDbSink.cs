// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-25
// Last Modified:			2016-06-25
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;
using NoDb;
using Serilog.Events;
using Microsoft.Extensions.Options;

namespace Serilog.Sinks.NoDb
{
    public class NoDbSink : ILogEventSink
    {
        public NoDbSink(
            IBasicCommands<LogEvent> commands,
            IOptions<NoDbSinkOptions> optionsAccessor
            )
        {
            this.commands = commands;
            options = optionsAccessor.Value;
        }

        private IBasicCommands<LogEvent> commands;
        private NoDbSinkOptions options;

        public void Emit(LogEvent logEvent)
        {
            
            var key = ResolveKey(logEvent);
            Task t = commands.CreateAsync(options.ProjectId, key, logEvent);
        }

        private string ResolveKey(LogEvent logEvent)
        {
            var source = logEvent.Properties["SourceContext"];
            string keyPart;
            //if(source != null)
            //{
            //    keyPart = source.ToString();
            //}
            //else
            //{
                keyPart = Guid.NewGuid().ToString();
            //}

            string key = logEvent.Timestamp.ToUniversalTime().ToString("yyyyMMddHHmmssfff") 
                + "-" + logEvent.Level.ToString()
                + "-" + keyPart
                ;

            return key;
        }
    }
}
