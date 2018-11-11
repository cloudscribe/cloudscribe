using System;
using System.Collections.Generic;
using NodaTime.TimeZones;

namespace cloudscribe.DateTimeUtils
{
    public interface ITimeZoneHelper
    {
        DateTime ConvertToLocalTime(DateTime utcDateTime, string timeZoneId);
        DateTime ConvertToUtc(DateTime localDateTime, string timeZoneId, ZoneLocalMappingResolver resolver = null);
        IReadOnlyCollection<string> GetTimeZoneList();
    }
}