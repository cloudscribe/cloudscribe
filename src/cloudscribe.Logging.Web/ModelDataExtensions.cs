// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-09
// Last Modified:			2015-12-26
// 

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;

namespace cloudscribe.Logging.Web
{
    public static class ModelDataExtensions
    {
        public static void LoadFromReader(this ILogItem logItem, DbDataReader reader)
        {
            logItem.Id = Convert.ToInt32(reader["ID"]);
            logItem.LogDateUtc = Convert.ToDateTime(reader["LogDate"]);
            logItem.IpAddress = reader["IpAddress"].ToString();
            logItem.Culture = reader["Culture"].ToString();
            logItem.Url = reader["Url"].ToString();
            logItem.ShortUrl = reader["ShortUrl"].ToString();
            logItem.Thread = reader["Thread"].ToString();
            logItem.LogLevel = reader["LogLevel"].ToString();
            logItem.Logger = reader["Logger"].ToString();
            logItem.Message = reader["Message"].ToString();

        }

    }
}
