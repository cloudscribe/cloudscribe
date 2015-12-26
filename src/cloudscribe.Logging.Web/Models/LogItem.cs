// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-18
//	Last Modified:		    2015-12-26
// 

using System;


namespace cloudscribe.Logging.Web
{
    public class LogItem : ILogItem
    {
        public LogItem()
        { }

        public int Id { get; set; } = -1;
        public DateTime LogDateUtc { get; set; } = DateTime.UtcNow;

        private string ipAddress = string.Empty;
        public string IpAddress
        {
            get { return ipAddress ?? string.Empty; }
            set { ipAddress = value; }
        }

        private string culture = string.Empty;
        public string Culture
        {
            get { return culture ?? string.Empty; }
            set { culture = value; }
        }

        private string url = string.Empty;
        public string Url
        {
            get { return url ?? string.Empty; }
            set { url = value; }
        }

        private string shortUrl = string.Empty;
        public string ShortUrl
        {
            get { return shortUrl ?? string.Empty; }
            set { shortUrl = value; }
        }

        private string thread = string.Empty;
        public string Thread
        {
            get { return thread ?? string.Empty; }
            set { thread = value; }
        }

        private string logLevel = string.Empty;
        public string LogLevel
        {
            get { return logLevel ?? string.Empty; }
            set { logLevel = value; }
        }

        private string logger = string.Empty;
        public string Logger
        {
            get { return logger ?? string.Empty; }
            set { logger = value; }
        }

        private string message = string.Empty;
        public string Message
        {
            get { return message ?? string.Empty; }
            set { message = value; }
        }


        

    }
}
