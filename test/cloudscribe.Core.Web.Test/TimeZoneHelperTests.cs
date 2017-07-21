using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using NodaTime.TimeZones;
using NodaTime.Extensions;
using NodaTime.Utility;
using Xunit;
using System.Runtime.InteropServices;
using cloudscribe.Core.Models;
using cloudscribe.Web.Common;
using System.Globalization;

namespace cloudscribe.Core.Web.Test
{
    public class TimeZoneHelperTests
    {
        // this test fials in CI on linux/mac because Eastern Standard Time is not a valid timezone on linux
        //[Fact]
        //public void Resolves_Correct_LocalTime()
        //{
        //    var tzHelper = new TimeZoneHelper(new DateTimeZoneCache(TzdbDateTimeZoneSource.Default));

        //    var utcNow = DateTime.UtcNow;

        //    var easternTzBcl = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        //    var bclLocal = TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(utcNow, DateTimeKind.Utc), easternTzBcl);

        //    var helperLocal = tzHelper.ConvertToLocalTime(utcNow, "America/New_York");
        //    Assert.True(bclLocal.Year == helperLocal.Year);
        //    Assert.True(bclLocal.Month == helperLocal.Month);
        //    Assert.True(bclLocal.Day == helperLocal.Day);
        //    Assert.True(bclLocal.Hour == helperLocal.Hour);
        //    Assert.True(bclLocal.Minute == helperLocal.Minute);
        //    Assert.True(bclLocal.Second == helperLocal.Second);
        //    Assert.True(bclLocal.Millisecond == helperLocal.Millisecond);
        //    Assert.True(bclLocal.Kind == DateTimeKind.Unspecified);
        //    Assert.True(helperLocal.Kind == DateTimeKind.Unspecified);
        //    // this one fails but for practical results the above work andarre good enough
        //    //Assert.True(bclLocal.Equals(helperLocal));

        //}

        // this test is wrong because DateTime.Parse already converts the utc to local system time
        // TODO: rewrite this test using DateTime.ParseExact as specified in the linked stackoverflow question
        //[Fact]
        //public void Resolves_Correct_LocalTime_From_Parsed_Utc()
        //{
        //    var utcFromDb = DateTime.Parse("2016-06-07T17:28:15.8579119Z");
        //    // 1:28PM Eastern is 17:28 or 5:28PM GMT // 4 hours different during daylight savings
        //    // eek this is true
        //    Assert.True(utcFromDb.Kind == DateTimeKind.Local);
        //    Assert.True(utcFromDb.Year == 2016);
        //    Assert.True(utcFromDb.Month == 6);
        //    Assert.True(utcFromDb.Day == 7);
        //    // really would have expected this to be utc ie it should be 17 but it isn't
        //    // http://stackoverflow.com/questions/10029099/datetime-parse2012-09-30t230000-0000000z-always-converts-to-datetimekind-l
        //    Assert.True(utcFromDb.Hour == 13); // and it isalready converted to eastern standard ie 1pm
        //    //Assert.True(utcFromDb.Minute == 19);

        //    var easternTzBcl = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); // 4hour offset
        //    //var easternTzBcl = TimeZoneInfo.FindSystemTimeZoneById("Eastern Daylight Time"); // 5 hour offset
        //    //var bclLocal = DateTime.SpecifyKind(
        //    //    TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(utcFromDb, DateTimeKind.Utc), easternTzBcl),
        //    //    DateTimeKind.Local);
        //    var bclLocal = TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(utcFromDb, DateTimeKind.Utc), easternTzBcl);

        //    // strange but true kind is unsepcified
        //    Assert.True(bclLocal.Kind == DateTimeKind.Unspecified);
        //    Assert.True(bclLocal.Year == 2016);
        //    Assert.True(bclLocal.Month == 6);
        //    Assert.True(bclLocal.Day == 7);
        //    //Assert.True(bclLocal.Hour != 13); // clearly the hour changed
        //    //Assert.True(bclLocal.Hour != 1); // in case it is as 1PM aka 13
        //    //Assert.True(bclLocal.Hour != 18); // didn't add 5 hours
        //    //Assert.True(bclLocal.Hour != 6); // 6PM aka 18
        //    //Assert.True(bclLocal.Hour != 8); // didn't subtract 5 hours
        //    Assert.True(bclLocal.Hour == 9); // so we subtracted 4 hours



        //    var tzHelper = new TimeZoneHelper(new DateTimeZoneCache(TzdbDateTimeZoneSource.Default));

        //    var helperLocal = tzHelper.ConvertToLocalTime(utcFromDb, "America/New_York");

        //    Assert.True(helperLocal.Kind == DateTimeKind.Unspecified);
        //    Assert.True(helperLocal.Year == 2016);
        //    Assert.True(helperLocal.Month == 6);
        //    Assert.True(helperLocal.Day == 7);
        //    Assert.True(helperLocal.Hour == 13); //aka 1PM as we want even though the parsed date created local
        //    //Assert.True(helperLocal.Minute == 28);

        //    var parsed = DateTime.Parse("2016-02-22 10:54:08");
        //    // so if Z is not there it doesn't adjust it and flags it as unspecified kind
        //    Assert.True(parsed.Kind == DateTimeKind.Unspecified);
        //    Assert.True(parsed.Year == 2016);
        //    Assert.True(parsed.Month == 2);
        //    Assert.True(parsed.Day == 22);
        //    Assert.True(parsed.Hour == 10);

        //}

        //2016-06-07T13:19:00.0000000Z

        //[Fact]
        //public void Resolves_Correct_Utc_From_Time()
        //{
        //    var tzHelper = new TimeZoneHelper(new DateTimeZoneCache(TzdbDateTimeZoneSource.Default));

        //    var utcNow = DateTime.UtcNow;

        //    var easternTzBcl = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        //    var bclLocal = TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(utcNow, DateTimeKind.Utc), easternTzBcl);
        //    //var bclLocal = DateTime.SpecifyKind(
        //    //    TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(utcNow, DateTimeKind.Utc), easternTzBcl),
        //    //    DateTimeKind.Local);

        //    var helperUtc = tzHelper.ConvertToUtc(bclLocal, "America/New_York");

        //    Assert.True(utcNow.Year == helperUtc.Year);
        //    Assert.True(utcNow.Month == helperUtc.Month);
        //    Assert.True(utcNow.Day == helperUtc.Day);
        //    Assert.True(utcNow.Hour == helperUtc.Hour);
        //    Assert.True(utcNow.Minute == helperUtc.Minute);
        //    Assert.True(utcNow.Second == helperUtc.Second);
        //    Assert.True(utcNow.Millisecond == helperUtc.Millisecond);
        //    Assert.True(utcNow.Kind == DateTimeKind.Utc);
        //    Assert.True(helperUtc.Kind == DateTimeKind.Utc);

        //}

        // This test fails on ci or other machine with different time zone
        //[Fact]
        //public void Round_Trip_Utc_From_Time()
        //{
        //    var tzHelper = new TimeZoneHelper(new DateTimeZoneCache(TzdbDateTimeZoneSource.Default));

        //    /*var localTime = DateTime.Parse("09/03/2016 1:46 PM");*/ // central time but local time is eastern 2:26
        //    var localTime = DateTime.Parse("2016-09-03T13:46");

        //    Assert.True(localTime.Kind == DateTimeKind.Unspecified);
        //    Assert.True(localTime.Hour == 13);
        //    Assert.True(localTime.Minute == 46);

        //    var helperUtc = tzHelper.ConvertToUtc(localTime, "America/Chicago");
        //    var serialized = helperUtc.ToString("O");
        //    Assert.True(serialized == "2016-09-03T18:46:00.0000000Z");

        //    var localTime2 = DateTime.Parse(serialized); //parsed with TZ convert to local system time which is eastern
        //    Assert.True(localTime2.Kind == DateTimeKind.Local);
        //    Assert.True(localTime2.Hour == 14); // local is eastern time so 1 hour later
        //    Assert.True(localTime2.Minute == 46);



        //    var helperLocal = tzHelper.ConvertToLocalTime(helperUtc, "America/New_York");

        //    Assert.True(helperLocal.Kind == DateTimeKind.Unspecified);
        //    Assert.True(helperLocal.Hour == 14); // should be 1 hour later in NY
        //    Assert.True(helperLocal.Minute == 46);

        //    //"yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'"
        //    var parsedUtc = DateTime.ParseExact(serialized,
        //                               "O",
        //                               CultureInfo.InvariantCulture,
        //                               DateTimeStyles.AssumeUniversal |
        //                               DateTimeStyles.AdjustToUniversal);

        //    Assert.True(parsedUtc.Kind == DateTimeKind.Utc);
        //    Assert.True(parsedUtc.Hour == 18); // same is in serialzed string ie utc
        //    Assert.True(parsedUtc.Minute == 46);


        //}

            // from UI datepicker 09/03/2016 2:35 PM
            // parsed converted to urc and saved as 2016-09-03T18:35:00.0000000Z


    }
}
