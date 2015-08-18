// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-18
//	Last Modified:		    2015-08-18
// 

using System;


namespace cloudscribe.Core.Models.Logging
{
    public class LogItem : ILogItem
    {
        public LogItem()
        { }

        public int Id { get; set; } = -1;
        public DateTime LogDateUtc { get; set; } = DateTime.UtcNow;
        public string IpAddress { get; set; } = string.Empty;
        public string Culture { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public string Thread { get; set; } = string.Empty;
        public string LogLevel { get; set; } = string.Empty;
        public string Logger { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

    }
}
