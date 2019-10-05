//using System;
//using System.Collections.Generic;
//using NodaTime.TimeZones;

//namespace cloudscribe.Web.Common
//{
//    [Obsolete("Please use the cloudscribe.DateTimeUtils version of this interface")]
//    public interface ITimeZoneHelper
//    {
//        DateTime ConvertToLocalTime(DateTime utcDateTime, string timeZoneId);
//        DateTime ConvertToUtc(DateTime localDateTime, string timeZoneId, ZoneLocalMappingResolver resolver = null);
//        IReadOnlyCollection<string> GetTimeZoneList();
//    }
//}