// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-05
// Last Modified:			2016-06-06
// 

// http://nodatime.org/unstable/api/
// http://nodatime.org/unstable/userguide/
// http://blog.nodatime.org/2010/11/joys-of-datetime-arithmetic.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodaTime.TimeZones;
using NodaTime;
using Microsoft.Extensions.Logging;

namespace cloudscribe.Web.Common
{
    public class TimeZoneHelper : ITimeZoneHelper
    {
        public TimeZoneHelper(
            IDateTimeZoneProvider timeZoneProvider,
            ILogger<TimeZoneHelper> logger
            )
        {
            tzSource = timeZoneProvider;
            log = logger;
        }

        private IDateTimeZoneProvider tzSource;
        private ILogger log;

        public DateTime ConvertToLocalTime(DateTime utcDateTime, string timeZoneId)
        {
            var dUtc = new DateTime(
                utcDateTime.Year,
                utcDateTime.Month,
                utcDateTime.Day,
                utcDateTime.Hour,
                utcDateTime.Minute,
                utcDateTime.Second,
                utcDateTime.Millisecond,
                DateTimeKind.Utc);
            
            var tz = tzSource.GetZoneOrNull(timeZoneId);
            if (tz == null)
            {
                log.LogWarning("failed to find timezone for " + timeZoneId);
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
                DateTimeKind.Local);

            return local;
        }

        public DateTime ConvertToUtc(
            DateTime localDateTime, 
            string timeZoneId,
            ZoneLocalMappingResolver resolver = null
            )
        {
            if (resolver == null) resolver = Resolvers.LenientResolver;
            var tz = tzSource.GetZoneOrNull(timeZoneId);
            if (tz == null)
            {
                log.LogWarning("failed to find timezone for " + timeZoneId);
                return localDateTime;
            }

            var loc = new LocalDateTime(
                localDateTime.Year,
                localDateTime.Month,
                localDateTime.Day,
                localDateTime.Hour,
                localDateTime.Minute,
                localDateTime.Second,
                localDateTime.Millisecond);

            var nZ = tz.ResolveLocal(loc, resolver);

            return nZ.ToDateTimeUtc();
        }

        public IReadOnlyCollection<string> GetTimeZoneList()
        {
            return tzSource.Ids;
        }
    }
}
