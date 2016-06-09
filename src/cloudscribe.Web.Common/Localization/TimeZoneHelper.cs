// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-05
// Last Modified:			2016-06-08
// 

// http://nodatime.org/unstable/api/
// http://nodatime.org/unstable/userguide/
// http://blog.nodatime.org/2010/11/joys-of-datetime-arithmetic.html

using Microsoft.Extensions.Logging;
using NodaTime;
using NodaTime.TimeZones;
using System;
using System.Collections.Generic;

namespace cloudscribe.Web.Common
{
    public class TimeZoneHelper : ITimeZoneHelper
    {
        public TimeZoneHelper(
            IDateTimeZoneProvider timeZoneProvider,
            ILogger<TimeZoneHelper> logger = null
            )
        {
            tzSource = timeZoneProvider;
            log = logger;
        }

        private IDateTimeZoneProvider tzSource;
        private ILogger log;

        public DateTime ConvertToLocalTime(DateTime utcDateTime, string timeZoneId)
        {
            DateTime dUtc;
            switch(utcDateTime.Kind)
            {
                case DateTimeKind.Utc:
                dUtc = utcDateTime;
                    break;
                case DateTimeKind.Local:
                    dUtc = utcDateTime.ToUniversalTime();
                    break;
                default: //DateTimeKind.Unspecified
                    dUtc = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
                    break;
            }

            var tz = tzSource.GetZoneOrNull(timeZoneId);
            if (tz == null)
            {
                if(log != null)
                {
                    log.LogWarning("failed to find timezone for " + timeZoneId);
                }
                
                return utcDateTime;
            }

            var nInst = Instant.FromDateTimeUtc(dUtc);
            var nZ = new ZonedDateTime(nInst, tz);
            var local = new DateTime(
                nZ.Year,
                nZ.Month,
                nZ.Day,
                nZ.Hour,
                nZ.Minute,
                nZ.Second,
                nZ.Millisecond,
                DateTimeKind.Unspecified); 

            return local;
        }

        public DateTime ConvertToUtc(
            DateTime localDateTime, 
            string timeZoneId,
            ZoneLocalMappingResolver resolver = null
            )
        {
            if (localDateTime.Kind == DateTimeKind.Utc) return localDateTime;

            if (resolver == null) resolver = Resolvers.LenientResolver;
            var tz = tzSource.GetZoneOrNull(timeZoneId);
            if (tz == null)
            {
                if (log != null)
                {
                    log.LogWarning("failed to find timezone for " + timeZoneId);
                }
                return localDateTime;
            }

            var loc = LocalDateTime.FromDateTime(localDateTime);
            var nZ = tz.ResolveLocal(loc, resolver);
            return nZ.ToDateTimeUtc();
        }

        public IReadOnlyCollection<string> GetTimeZoneList()
        {
            return tzSource.Ids;
        }
    }
}
