// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-05
// Last Modified:			2016-06-05
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodaTime.TimeZones;
using NodaTime;

namespace cloudscribe.Common
{
    public class TimeZoneHelper
    {
        public TimeZoneHelper()
        {
            // TODO: inject this cached tz stuff
            var tzS = TzdbDateTimeZoneSource.Default;
            tzSource = new DateTimeZoneCache(tzS);
        }

        private IDateTimeZoneProvider tzSource;

        public DateTime ConvertToLocalTime(DateTime utcDate, string timeZoneId)
        {
            var dUtc = new System.DateTime(
                utcDate.Year,
                utcDate.Month,
                utcDate.Day,
                utcDate.Hour,
                utcDate.Minute,
                utcDate.Second,
                utcDate.Millisecond,
                DateTimeKind.Utc);
            
            var tz = tzSource.GetZoneOrNull(timeZoneId);
            // TODO: log this?
            if (tz == null) return utcDate;

            var nInst = Instant.FromDateTimeUtc(dUtc);
            var nZ = new ZonedDateTime(nInst, tz);
            var local = new System.DateTime(
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

        public System.DateTime ConvertToUtc(System.DateTime localDateTime, string timeZoneId)
        {
            var tz = tzSource.GetZoneOrNull(timeZoneId);
            // TODO: log this?
            if (tz == null) return localDateTime;

            var loc = new LocalDateTime(
                localDateTime.Year,
                localDateTime.Month,
                localDateTime.Day,
                localDateTime.Hour,
                localDateTime.Minute,
                localDateTime.Second,
                localDateTime.Millisecond);

            var nZ = tz.ResolveLocal(loc, Resolvers.LenientResolver);

            return nZ.ToDateTimeUtc();
        }
    }
}
