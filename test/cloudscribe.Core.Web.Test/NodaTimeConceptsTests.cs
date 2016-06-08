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

namespace cloudscribe.Core.Web.Test
{
    /// <summary>
    /// these are not important tests, they are just for me to confirm my understanding of
    /// how to work with NodaTime, TimeZones, and conversion to Utc
    /// in my projects I store datetime in the database as utc and adjust it for display to 
    /// the user's timezone or the default timezone of a site
    /// for my purposes the TimeZoneInfo class in the bcl has generally been sufficient to meet my needs
    /// however, working with cross platform we discover that TimeZoneIds are not the same on *nix systems
    /// as on windows. For example my windows time zone is "Eastern Standard Time" but on a mac/*nix box it is "America/New_York"
    /// *nix uses IANA time zones
    /// http://stackoverflow.com/questions/17348807/how-to-translate-between-windows-and-iana-time-zones/17348822#17348822
    /// In most cases, an IANA time zone can be mapped to a single Windows time zone. But the reverse is not true. 
    /// A single Windows time zone might be mapped to more than one IANA time zone.
    /// NodaTime contains an embedded copy of the CLDR mappings http://unicode.org/repos/cldr/trunk/common/supplemental/windowsZones.xml
    /// so I want to use it for people to choose their time zone in a way that can work on any machine
    /// it may be sufficent for my needs to just convert that back to a bcl TimeZoneInfo instance
    /// </summary>
    public class NodaTimeConceptsTests
    {
        //private static readonly bool s_isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        // These tests pass but were just for learning,not testing my own code so commented out
        //[Fact]
        //public void Can_Get_List_Of_TimeZones_FromNoda()
        //{
        //    var tzSource = TzdbDateTimeZoneSource.Default;
        //    Assert.True(tzSource.ZoneLocations.Count > 1);
        //}

        //[Fact]
        //public void Can_Get_America_NewYork_FromNoda()
        //{
        //    var tzSource = TzdbDateTimeZoneSource.Default;
        //    var eastern  = tzSource.ForId("America/New_York");
        //    Assert.NotNull(eastern);
        //    Assert.Equal("America/New_York", eastern.Id);

        //}

        //[Fact]
        //public void Can_RoundTrip_Utc_ToTimeZone_BackTo_Utc_FromNoda()
        //{
        //    //2016-06-05 14:13:53.890
        //    //var d = Convert.ToDateTime("2016-06-05 14:13:53.890");
        //    var dUtc = new DateTime(2016, 6, 5, 14, 13, 53, 890, DateTimeKind.Utc);
            

        //    var nInst = Instant.FromDateTimeUtc(dUtc);
        //    var roundTrippedUtc = nInst.ToDateTimeUtc();
        //    Assert.True(dUtc == roundTrippedUtc);
            
        //    var tzSource = TzdbDateTimeZoneSource.Default;
        //    var eastern = tzSource.ForId("America/New_York");
        //    var nZ = new ZonedDateTime(nInst, eastern);
        //    roundTrippedUtc = nZ.ToDateTimeUtc();
        //    Assert.True(dUtc == roundTrippedUtc);
            
        //}

        //[Fact]
        //public void Local_DateTime_FromBcl_Matches_FromNoda()
        //{
        //    var dUtc = new DateTime(2016, 6, 5, 14, 13, 53, 890, DateTimeKind.Utc);
        //    var nInst = Instant.FromDateTimeUtc(dUtc);

        //    var tzSource = TzdbDateTimeZoneSource.Default;
        //    var eastern = tzSource.ForId("America/New_York");
        //    var nZ = new ZonedDateTime(nInst, eastern);

        //    var easternTzBcl = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        //    Assert.NotNull(easternTzBcl);
        //    var dLocalBcl = TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(dUtc, DateTimeKind.Utc), easternTzBcl);

        //    Assert.True(nZ.Year == dLocalBcl.Year);
        //    Assert.True(nZ.Month == dLocalBcl.Month);
        //    Assert.True(nZ.Day == dLocalBcl.Day);
        //    Assert.True(nZ.Minute == dLocalBcl.Minute);
        //    Assert.True(nZ.Second == dLocalBcl.Second);
        //    Assert.True(nZ.Millisecond == dLocalBcl.Millisecond);
        //}

    }
}
