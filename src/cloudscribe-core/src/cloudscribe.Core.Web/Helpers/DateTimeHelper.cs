// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:				Joe Audette
// Created:			    2005-08-22
// Last Modified:	    2015-08-23
// 


using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Globalization;


namespace cloudscribe.Core.Web.Helpers
{
    public static class DateTimeHelper
    {

        //public static TimeZoneInfo GetTimeZone(string id)
        //{
        //    return TimeZoneInfo.FindSystemTimeZoneById(id);
        //}

        //public static TimeZoneInfo GetTimeZone(List<TimeZoneInfo> timeZones, string id)
        //{
        //    foreach (TimeZoneInfo t in timeZones)
        //    {

        //        if (t.StandardName == id) { return t; }
        //    }

        //    return null;
        //}

        //public static double GetUtcOffsetHours(DateTime localDateTime, TimeZoneInfo timeZone)
        //{
        //    TimeSpan ts = timeZone.GetUtcOffset(localDateTime);
        //    return ts.TotalHours;

        //}

        public static DateTime ToLocalTime(this DateTime utcDate, TimeZoneInfo timeZone)
        {

            return TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(utcDate, DateTimeKind.Utc), timeZone);
            //return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utcDate, DateTimeKind.Utc), timeZone);
        }

        public static DateTime ToUtc(this DateTime localDate, TimeZoneInfo timeZone)
        {
            //return TimeZoneInfo.ConvertTimeToUtc(localDate, timeZone);

            return TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(localDate, DateTimeKind.Local), TimeZoneInfo.Utc);
        }

        /// <summary>
        /// this is needed to configure a datetime picker to match DateTime.ToString("g")
        /// http://trentrichardson.com/examples/timepicker/#tp-formatting
        /// the standard ShortTimePattern uses tt where this time picker expects TT
        /// in .NET tt means use AM or PM but in this js it means am or pm
        /// we need TT to get AM or PM
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToDatePickerWithTimeFormat(this DateTimeFormatInfo t)
        {
            return t.ShortTimePattern.Replace("tt", "TT");
        }

        /// <summary>
        /// in .NET M means month 1 -12 with no leading zero
        /// in javascript it means month name like Dec
        /// we need m for month with no leading zero, so we need to lower it
        /// also in C# yyyy means four digit year but in js yy means 4 digit year so
        /// we must replace yyyy with yy
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToDatePickerFormat(this DateTimeFormatInfo t)
        {
            return t.ShortDatePattern.Replace("M", "m").Replace("yyyy", "yy");
        }

        public static ReadOnlyCollection<TimeZoneInfo> GetTimeZoneList()
        {
            return BuildTimeZoneList();

            //List<TimeZoneInfo> timeZones = null;

            //if (HttpContext.Current != null)
            //{
            //    timeZones = HttpContext.Current.Items["tzlist"] as List<TimeZoneInfo>;
            //}
            //if (timeZones == null)
            //{
            //    if (AppSettings.CacheTimeZoneList)
            //    {

            //        string cachekey = "tzlist";

            //        DateTime expiration = DateTime.Now.AddSeconds(AppSettings.CacheDurationInSeconds_TimeZoneList);

            //        try
            //        {
            //            timeZones = CacheManager.Cache.Get<List<TimeZoneInfo>>(cachekey, expiration, () =>
            //            {
            //                // This is the anonymous function which gets called if the data is not in the cache.
            //                // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.

            //                return BuildTimeZoneList();
            //            });

            //            return timeZones;
            //        }
            //        catch (Exception ex)
            //        {
            //            log.Error("failed to get timeZones list from cache so loading it directly", ex);

            //        }

            //        timeZones = BuildTimeZoneList();


            //    }
            //    else // not caching timezonelist
            //    {
            //        timeZones = BuildTimeZoneList();
            //    }

            //    // we are putting it in the httpcontext to avoid
            //    // building it more than once during a given web request
            //    if (timeZones != null)
            //    {
            //        if (HttpContext.Current != null)
            //        {
            //            HttpContext.Current.Items["tzlist"] = timeZones;
            //        }
            //    }
            //}
            //return timeZones;
        }


        


        /// <summary>
        /// probably best to use the caching method GetTimeZoneList above instead of this one in most cases
        /// 
        /// The plan is to use this for setting the site default time zone and user time zones.
        /// As of 2009-04-06 its throwing NotImplementedException on Mono if you invoke this method.
        /// TimeZoneInfo is a new class in 3.5 .NET
        /// https://bugzilla.novell.com/show_bug.cgi?id=492924
        /// </summary>
        /// <returns></returns>
        public static ReadOnlyCollection<TimeZoneInfo> BuildTimeZoneList()
        {
            //List<TimeZoneInfo> timeZones = new List<TimeZoneInfo>();

            return TimeZoneInfo.GetSystemTimeZones();

            //TODO: make our own extension method for FromSerializedString
            // to support dnxcore50
//#if DNX451

//            timeZones.Add(TimeZoneInfo.FromSerializedString(AfghanistanStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(AlaskanStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(ArabianStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(ArabicStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(ArabStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(ArgentinaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(AtlanticStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(AUSCentralStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(AUSEasternStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(AzerbaijanStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(AzoresStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(CanadaCentralStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(CapeVerdeStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(CaucasusStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(CenAustraliaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(CentralAmericaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(CentralAsiaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(CentralBrazilianStandard));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(CentralEuropeanStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(CentralEuropeStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(CentralPacificStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(CentralStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(CentralStandardTimeMexico));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(ChinaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(DatelineStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(EAfricaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(EAustraliaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(EEuropeStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(ESouthAmericaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(EasternStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(EgyptStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(EkaterinburgStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(FijiStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(FLEStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(GeorgianStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(GMTStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(GreenlandStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(GreenwichStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(GTBStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(HawaiianStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(IndiaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(IranStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(IsraelStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(JordanStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(KoreaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(MauritiusStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(MidAtlanticStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(MiddleEastStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(MontevideoStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(MoroccoStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(MountainStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(MountainStandardTimeMexico));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(MyanmarStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(NCentralAsiaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(NamibiaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(NepalStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(NewfoundlandStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(NewZealandStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(NorthAsiaEastStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(NorthAsiaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(PacificSAStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(PacificStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(PacificStandardTimeMexico));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(PakistanStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(RomanceStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(RussianStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(SAEasternStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(SamoaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(SAPacificStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(SAWesternStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(SEAsiaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(SingaporeStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(SouthAfricaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(SriLankaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(TaipeiStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(TasmaniaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(TokyoStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(TongaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(USEasternStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(USMountainStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(VenezuelaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(VladivostokStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(WAustraliaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(WCentralAfricaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(WEuropeStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(WestAsiaStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(WestPacificStandardTime));
//            timeZones.Add(TimeZoneInfo.FromSerializedString(YakutskStandardTime));

//#endif

//            timeZones.Sort(delegate (TimeZoneInfo left, TimeZoneInfo right)
//            {
//                int comparison = left.BaseUtcOffset.CompareTo(right.BaseUtcOffset);
//                return comparison == 0 ? string.CompareOrdinal(left.DisplayName, right.DisplayName) : comparison;
//            });

//            return timeZones;

        }

        //#region TimeZoneInfo Serialized as Constants

        //// this is because they are normally stored in the system registry but are inconsistent across machines and OS.
        //// so we hard code them here to avoid system specific issues. 
        //// Drawback is if time zones change daylight savings rules we need to change them here
        //// but that rarely happens

        //public const string AfghanistanStandardTime = "Afghanistan Standard Time;270;(GMT+04:30) Kabul;Afghanistan Standard Time;Afghanistan Daylight Time;;";
        //public const string AlaskanStandardTime = "Alaskan Standard Time;-540;(GMT-09:00) Alaska;Alaskan Standard Time;Alaskan Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];";
        //public const string ArabianStandardTime = "Arabian Standard Time;240;(GMT+04:00) Abu Dhabi, Muscat;Arabian Standard Time;Arabian Daylight Time;;";
        //public const string ArabicStandardTime = "Arabic Standard Time;180;(GMT+03:00) Baghdad;Arabic Standard Time;Arabic Daylight Time;[01:01:0001;12:31:2006;60;[0;03:00:00;4;1;0;];[0;04:00:00;10;1;0;];][01:01:2007;12:31:2007;60;[0;03:00:00;4;1;0;];[0;04:00:00;10;1;1;];];";
        //public const string ArabStandardTime = "Arab Standard Time;180;(GMT+03:00) Kuwait, Riyadh;Arab Standard Time;Arab Daylight Time;;";
        //public const string ArgentinaStandardTime = "Argentina Standard Time;-180;(GMT-03:00) Buenos Aires;Argentina Standard Time;Argentina Daylight Time;[01:01:2007;12:31:2007;60;[0;00:00:00;12;5;0;];[0;00:00:00;1;1;1;];][01:01:2008;12:31:2008;60;[0;23:59:59.999;10;3;6;];[0;00:00:00;3;3;0;];][01:01:2009;12:31:9999;60;[0;23:59:59.999;10;3;6;];[0;23:59:59.999;3;2;6;];];";
        //public const string AtlanticStandardTime = "Atlantic Standard Time;-240;(GMT-04:00) Atlantic Time (Canada);Atlantic Standard Time;Atlantic Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];";
        //public const string AUSCentralStandardTime = "AUS Central Standard Time;570;(GMT+09:30) Darwin;AUS Central Standard Time;AUS Central Daylight Time;;";
        //public const string AUSEasternStandardTime = "AUS Eastern Standard Time;600;(GMT+10:00) Canberra, Melbourne, Sydney;AUS Eastern Standard Time;AUS Eastern Daylight Time;[01:01:0001;12:31:2007;60;[0;02:00:00;10;5;0;];[0;03:00:00;3;5;0;];][01:01:2008;12:31:9999;60;[0;02:00:00;10;1;0;];[0;03:00:00;4;1;0;];];";
        //public const string AzerbaijanStandardTime = "Azerbaijan Standard Time;240;(GMT+04:00) Baku;Azerbaijan Standard Time;Azerbaijan Daylight Time;[01:01:0001;12:31:9999;60;[0;04:00:00;3;5;0;];[0;05:00:00;10;5;0;];];";
        //public const string AzoresStandardTime = "Azores Standard Time;-60;(GMT-01:00) Azores;Azores Standard Time;Azores Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";
        //public const string CanadaCentralStandardTime = "Canada Central Standard Time;-360;(GMT-06:00) Saskatchewan;Canada Central Standard Time;Canada Central Daylight Time;;";
        //public const string CapeVerdeStandardTime = "Cape Verde Standard Time;-60;(GMT-01:00) Cape Verde Is.;Cape Verde Standard Time;Cape Verde Daylight Time;;";
        //public const string CaucasusStandardTime = "Caucasus Standard Time;240;(GMT+04:00) Yerevan;Caucasus Standard Time;Caucasus Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";
        //public const string CenAustraliaStandardTime = "Cen. Australia Standard Time;570;(GMT+09:30) Adelaide;Cen. Australia Standard Time;Cen. Australia Daylight Time;[01:01:0001;12:31:2007;60;[0;02:00:00;10;5;0;];[0;03:00:00;3;5;0;];][01:01:2008;12:31:9999;60;[0;02:00:00;10;1;0;];[0;03:00:00;4;1;0;];];";
        //public const string CentralAmericaStandardTime = "Central America Standard Time;-360;(GMT-06:00) Central America;Central America Standard Time;Central America Daylight Time;;";
        //public const string CentralAsiaStandardTime = "Central Asia Standard Time;360;(GMT+06:00) Astana, Dhaka;Central Asia Standard Time;Central Asia Daylight Time;;";
        //public const string CentralBrazilianStandard = "Central Brazilian Standard Time;-240;(GMT-04:00) Manaus;Central Brazilian Standard Time;Central Brazilian Daylight Time;[01:01:0001;12:31:2006;60;[0;00:00:00;11;1;0;];[0;02:00:00;2;2;0;];][01:01:2007;12:31:2007;60;[0;00:00:00;10;2;0;];[0;00:00:00;2;5;0;];][01:01:2008;12:31:2008;60;[0;23:59:59.999;10;3;6;];[0;00:00:00;2;3;0;];][01:01:2009;12:31:9999;60;[0;23:59:59.999;10;3;6;];[0;23:59:59.999;2;2;6;];];";
        //public const string CentralEuropeanStandardTime = "Central European Standard Time;60;(GMT+01:00) Sarajevo, Skopje, Warsaw, Zagreb;Central European Standard Time;Central European Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";
        //public const string CentralEuropeStandardTime = "Central Europe Standard Time;60;(GMT+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague;Central Europe Standard Time;Central Europe Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";
        //public const string CentralPacificStandardTime = "Central Pacific Standard Time;660;(GMT+11:00) Magadan, Solomon Is., New Caledonia;Central Pacific Standard Time;Central Pacific Daylight Time;;";
        //public const string CentralStandardTime = "Central Standard Time;-360;(GMT-06:00) Central Time (US & Canada);Central Standard Time;Central Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];";
        //public const string CentralStandardTimeMexico = "Central Standard Time (Mexico);-360;(GMT-06:00) Guadalajara, Mexico City, Monterrey;Central Standard Time (Mexico);Central Daylight Time (Mexico);[01:01:0001;12:31:9999;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];];";
        //public const string ChinaStandardTime = "China Standard Time;480;(GMT+08:00) Beijing, Chongqing, Hong Kong, Urumqi;China Standard Time;China Daylight Time;;";
        //public const string DatelineStandardTime = "Dateline Standard Time;-720;(GMT-12:00) International Date Line West;Dateline Standard Time;Dateline Daylight Time;;";
        //public const string EAfricaStandardTime = "E. Africa Standard Time;180;(GMT+03:00) Nairobi;E. Africa Standard Time;E. Africa Daylight Time;;";
        //public const string EAustraliaStandardTime = "E. Australia Standard Time;600;(GMT+10:00) Brisbane;E. Australia Standard Time;E. Australia Daylight Time;;";
        //public const string EEuropeStandardTime = "E. Europe Standard Time;120;(GMT+02:00) Minsk;E. Europe Standard Time;E. Europe Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";
        //public const string ESouthAmericaStandardTime = "E. South America Standard Time;-180;(GMT-03:00) Brasilia;E. South America Standard Time;E. South America Daylight Time;[01:01:0001;12:31:2006;60;[0;00:00:00;11;1;0;];[0;02:00:00;2;2;0;];][01:01:2007;12:31:2007;60;[0;00:00:00;10;2;0;];[0;00:00:00;2;5;0;];][01:01:2008;12:31:2008;60;[0;23:59:59.999;10;3;6;];[0;00:00:00;2;3;0;];][01:01:2009;12:31:9999;60;[0;23:59:59.999;10;3;6;];[0;23:59:59.999;2;2;6;];];";
        //public const string EasternStandardTime = "Eastern Standard Time;-300;(GMT-05:00) Eastern Time (US & Canada);Eastern Standard Time;Eastern Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];";
        //public const string EgyptStandardTime = "Egypt Standard Time;120;(GMT+02:00) Cairo;Egypt Standard Time;Egypt Daylight Time;[01:01:0001;12:31:2005;60;[0;00:00:00;4;5;5;];[0;23:59:59.999;9;5;4;];][01:01:2006;12:31:2006;60;[0;00:00:00;4;5;5;];[0;23:59:59.999;9;3;4;];][01:01:2007;12:31:2007;60;[0;23:59:59.999;4;5;4;];[0;23:59:59.999;9;1;4;];][01:01:2008;12:31:2008;60;[0;23:59:59.999;4;5;4;];[0;23:59:59.999;8;5;4;];][01:01:2009;12:31:2009;60;[0;23:59:59.999;4;4;4;];[0;23:59:59.999;9;5;4;];][01:01:2010;12:31:9999;60;[0;23:59:59.999;4;5;4;];[0;23:59:59.999;9;5;4;];];";
        //public const string EkaterinburgStandardTime = "Ekaterinburg Standard Time;300;(GMT+05:00) Ekaterinburg;Ekaterinburg Standard Time;Ekaterinburg Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";
        //public const string FijiStandardTime = "Fiji Standard Time;720;(GMT+12:00) Fiji, Kamchatka, Marshall Is.;Fiji Standard Time;Fiji Daylight Time;;";
        //public const string FLEStandardTime = "FLE Standard Time;120;(GMT+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius;FLE Standard Time;FLE Daylight Time;[01:01:0001;12:31:9999;60;[0;03:00:00;3;5;0;];[0;04:00:00;10;5;0;];];";
        //public const string GeorgianStandardTime = "Georgian Standard Time;180;(GMT+03:00) Tbilisi;Georgian Standard Time;Georgian Daylight Time;;";
        //public const string GMTStandardTime = "GMT Standard Time;0;(GMT) Greenwich Mean Time : Dublin, Edinburgh, Lisbon, London;GMT Standard Time;GMT Daylight Time;[01:01:0001;12:31:9999;60;[0;01:00:00;3;5;0;];[0;02:00:00;10;5;0;];];";
        //public const string GreenlandStandardTime = "Greenland Standard Time;-180;(GMT-03:00) Greenland;Greenland Standard Time;Greenland Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];];";
        //public const string GreenwichStandardTime = "Greenwich Standard Time;0;(GMT) Monrovia, Reykjavik;Greenwich Standard Time;Greenwich Daylight Time;;";
        //public const string GTBStandardTime = "GTB Standard Time;120;(GMT+02:00) Athens, Bucharest, Istanbul;GTB Standard Time;GTB Daylight Time;[01:01:0001;12:31:9999;60;[0;03:00:00;3;5;0;];[0;04:00:00;10;5;0;];];";
        //public const string HawaiianStandardTime = "Hawaiian Standard Time;-600;(GMT-10:00) Hawaii;Hawaiian Standard Time;Hawaiian Daylight Time;;";
        //public const string IndiaStandardTime = "India Standard Time;330;(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi;India Standard Time;India Daylight Time;;";
        //public const string IranStandardTime = "Iran Standard Time;210;(GMT+03:30) Tehran;Iran Standard Time;Iran Daylight Time;[01:01:0001;12:31:2005;60;[0;02:00:00;3;1;0;];[0;02:00:00;9;4;2;];][01:01:2008;12:31:2008;60;[0;23:59:59.999;3;3;4;];[0;23:59:59.999;9;3;6;];][01:01:2009;12:31:9999;60;[0;23:59:59.999;3;3;6;];[0;23:59:59.999;9;3;1;];];";
        //public const string IsraelStandardTime = "Israel Standard Time;120;(GMT+02:00) Jerusalem;Jerusalem Standard Time;Jerusalem Daylight Time;[01:01:2005;12:31:2005;60;[0;02:00:00;4;1;5;];[0;02:00:00;10;2;0;];][01:01:2006;12:31:2006;60;[0;02:00:00;3;5;5;];[0;02:00:00;10;1;0;];][01:01:2007;12:31:2007;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;3;0;];][01:01:2008;12:31:2008;60;[0;02:00:00;3;5;5;];[0;02:00:00;10;1;0;];][01:01:2009;12:31:2009;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;5;0;];][01:01:2010;12:31:2010;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;2;0;];][01:01:2011;12:31:2011;60;[0;02:00:00;4;1;5;];[0;02:00:00;10;1;0;];][01:01:2012;12:31:2012;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;4;0;];][01:01:2013;12:31:2013;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;2;0;];][01:01:2014;12:31:2014;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;4;0;];][01:01:2015;12:31:2015;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;3;0;];][01:01:2016;12:31:2016;60;[0;02:00:00;4;1;5;];[0;02:00:00;10;2;0;];][01:01:2017;12:31:2017;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;4;0;];][01:01:2018;12:31:2018;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;3;0;];][01:01:2019;12:31:2019;60;[0;02:00:00;3;5;5;];[0;02:00:00;10;1;0;];][01:01:2020;12:31:2020;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;4;0;];][01:01:2021;12:31:2021;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;2;0;];][01:01:2022;12:31:2022;60;[0;02:00:00;4;1;5;];[0;02:00:00;10;1;0;];];";
        //public const string JordanStandardTime = "Jordan Standard Time;120;(GMT+02:00) Amman;Jordan Standard Time;Jordan Daylight Time;[01:01:0001;12:31:2006;60;[0;00:00:00;3;5;4;];[0;01:00:00;9;5;5;];][01:01:2007;12:31:9999;60;[0;23:59:59.999;3;5;4;];[0;01:00:00;10;5;5;];];";
        //public const string KoreaStandardTime = "Korea Standard Time;540;(GMT+09:00) Seoul;Korea Standard Time;Korea Daylight Time;;";
        //public const string MauritiusStandardTime = "Mauritius Standard Time;240;(GMT+04:00) Port Louis;Mauritius Standard Time;Mauritius Daylight Time;[01:01:2008;12:31:2008;60;[0;02:00:00;10;5;0;];[0;00:00:00;1;1;2;];][01:01:2009;12:31:9999;60;[0;02:00:00;10;5;0;];[0;02:00:00;3;5;0;];];";
        //public const string MidAtlanticStandardTime = "Mid-Atlantic Standard Time;-120;(GMT-02:00) Mid-Atlantic;Mid-Atlantic Standard Time;Mid-Atlantic Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;02:00:00;9;5;0;];];";
        //public const string MiddleEastStandardTime = "Middle East Standard Time;120;(GMT+02:00) Beirut;Middle East Standard Time;Middle East Daylight Time;[01:01:0001;12:31:9999;60;[0;00:00:00;3;5;0;];[0;00:00:00;10;5;0;];];";
        //public const string MontevideoStandardTime = "Montevideo Standard Time;-180;(GMT-03:00) Montevideo;Montevideo Standard Time;Montevideo Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;9;2;0;];[0;02:00:00;3;2;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;10;1;0;];[0;02:00:00;3;2;0;];];";
        //public const string MoroccoStandardTime = "Morocco Standard Time;0;(GMT) Casablanca;Morocco Standard Time;Morocco Daylight Time;[01:01:2008;12:31:2008;60;[0;23:59:59.999;5;5;6;];[0;23:59:59.999;8;5;0;];];";
        //public const string MountainStandardTime = "Mountain Standard Time;-420;(GMT-07:00) Mountain Time (US & Canada);Mountain Standard Time;Mountain Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];";
        //public const string MountainStandardTimeMexico = "Mountain Standard Time (Mexico);-420;(GMT-07:00) Chihuahua, La Paz, Mazatlan;Mountain Standard Time (Mexico);Mountain Daylight Time (Mexico);[01:01:0001;12:31:9999;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];];";
        //public const string MyanmarStandardTime = "Myanmar Standard Time;390;(GMT+06:30) Yangon (Rangoon);Myanmar Standard Time;Myanmar Daylight Time;;";
        //public const string NCentralAsiaStandardTime = "N. Central Asia Standard Time;360;(GMT+06:00) Almaty, Novosibirsk;N. Central Asia Standard Time;N. Central Asia Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";
        //public const string NamibiaStandardTime = "Namibia Standard Time;120;(GMT+02:00) Windhoek;Namibia Standard Time;Namibia Daylight Time;[01:01:0001;12:31:9999;-60;[0;02:00:00;4;1;0;];[0;02:00:00;9;1;0;];];";
        //public const string NepalStandardTime = "Nepal Standard Time;345;(GMT+05:45) Kathmandu;Nepal Standard Time;Nepal Daylight Time;;";
        //public const string NewfoundlandStandardTime = "Newfoundland Standard Time;-210;(GMT-03:30) Newfoundland;Newfoundland Standard Time;Newfoundland Daylight Time;[01:01:0001;12:31:2006;60;[0;00:01:00;4;1;0;];[0;00:01:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;00:01:00;3;2;0;];[0;00:01:00;11;1;0;];];";
        //public const string NewZealandStandardTime = "New Zealand Standard Time;720;(GMT+12:00) Auckland, Wellington;New Zealand Standard Time;New Zealand Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;10;1;0;];[0;03:00:00;3;3;0;];][01:01:2007;12:31:2007;60;[0;02:00:00;9;5;0;];[0;03:00:00;3;3;0;];][01:01:2008;12:31:9999;60;[0;02:00:00;9;5;0;];[0;03:00:00;4;1;0;];];";
        //public const string NorthAsiaEastStandardTime = "North Asia East Standard Time;480;(GMT+08:00) Irkutsk, Ulaan Bataar;North Asia East Standard Time;North Asia East Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";
        //public const string NorthAsiaStandardTime = "North Asia Standard Time;420;(GMT+07:00) Krasnoyarsk;North Asia Standard Time;North Asia Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";
        //public const string PacificSAStandardTime = "Pacific SA Standard Time;-240;(GMT-04:00) Santiago;Pacific SA Standard Time;Pacific SA Daylight Time;[01:01:0001;12:31:2007;60;[0;23:59:59.999;10;2;6;];[0;23:59:59.999;3;2;6;];][01:01:2008;12:31:2008;60;[0;23:59:59.999;10;2;6;];[0;23:59:59.999;3;5;6;];][01:01:2009;12:31:9999;60;[0;23:59:59.999;10;2;6;];[0;23:59:59.999;3;2;6;];];";
        //public const string PacificStandardTime = "Pacific Standard Time;-480;(GMT-08:00) Pacific Time (US & Canada);Pacific Standard Time;Pacific Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];";
        //public const string PacificStandardTimeMexico = "Pacific Standard Time (Mexico);-480;(GMT-08:00) Tijuana, Baja California;Pacific Standard Time (Mexico);Pacific Daylight Time (Mexico);[01:01:0001;12:31:9999;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];];";
        //public const string PakistanStandardTime = "Pakistan Standard Time;300;(GMT+05:00) Islamabad, Karachi;Pakistan Standard Time;Pakistan Daylight Time;[01:01:2008;12:31:2008;60;[0;23:59:59.999;5;5;6;];[0;23:59:59.999;10;5;5;];];";
        //public const string RomanceStandardTime = "Romance Standard Time;60;(GMT+01:00) Brussels, Copenhagen, Madrid, Paris;Romance Standard Time;Romance Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";
        //public const string RussianStandardTime = "Russian Standard Time;180;(GMT+03:00) Moscow, St. Petersburg, Volgograd;Russian Standard Time;Russian Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";
        //public const string SAEasternStandardTime = "SA Eastern Standard Time;-180;(GMT-03:00) Georgetown;SA Eastern Standard Time;SA Eastern Daylight Time;;";
        //public const string SamoaStandardTime = "Samoa Standard Time;-660;(GMT-11:00) Midway Island, Samoa;Samoa Standard Time;Samoa Daylight Time;;";
        //public const string SAPacificStandardTime = "SA Pacific Standard Time;-300;(GMT-05:00) Bogota, Lima, Quito, Rio Branco;SA Pacific Standard Time;SA Pacific Daylight Time;;";
        //public const string SAWesternStandardTime = "SA Western Standard Time;-240;(GMT-04:00) La Paz;SA Western Standard Time;SA Western Daylight Time;;";
        //public const string SEAsiaStandardTime = "SE Asia Standard Time;420;(GMT+07:00) Bangkok, Hanoi, Jakarta;SE Asia Standard Time;SE Asia Daylight Time;;";
        //public const string SingaporeStandardTime = "Singapore Standard Time;480;(GMT+08:00) Kuala Lumpur, Singapore;Malay Peninsula Standard Time;Malay Peninsula Daylight Time;;";
        //public const string SouthAfricaStandardTime = "South Africa Standard Time;120;(GMT+02:00) Harare, Pretoria;South Africa Standard Time;South Africa Daylight Time;;";
        //public const string SriLankaStandardTime = "Sri Lanka Standard Time;330;(GMT+05:30) Sri Jayawardenepura;Sri Lanka Standard Time;Sri Lanka Daylight Time;;";
        //public const string TaipeiStandardTime = "Taipei Standard Time;480;(GMT+08:00) Taipei;Taipei Standard Time;Taipei Daylight Time;;";
        //public const string TasmaniaStandardTime = "Tasmania Standard Time;600;(GMT+10:00) Hobart;Tasmania Standard Time;Tasmania Daylight Time;[01:01:0001;12:31:2007;60;[0;02:00:00;10;1;0;];[0;03:00:00;3;5;0;];][01:01:2008;12:31:9999;60;[0;02:00:00;10;1;0;];[0;03:00:00;4;1;0;];];";
        //public const string TokyoStandardTime = "Tokyo Standard Time;540;(GMT+09:00) Osaka, Sapporo, Tokyo;Tokyo Standard Time;Tokyo Daylight Time;;";
        //public const string TongaStandardTime = "Tonga Standard Time;780;(GMT+13:00) Nuku'alofa;Tonga Standard Time;Tonga Daylight Time;;";
        //public const string USEasternStandardTime = "US Eastern Standard Time;-300;(GMT-05:00) Indiana (East);US Eastern Standard Time;US Eastern Daylight Time;;";
        //public const string USMountainStandardTime = "US Mountain Standard Time;-420;(GMT-07:00) Arizona;US Mountain Standard Time;US Mountain Daylight Time;;";
        //public const string VenezuelaStandardTime = "Venezuela Standard Time;-270;(GMT-04:30) Caracas;Venezuela Standard Time;Venezuela Daylight Time;;";
        //public const string VladivostokStandardTime = "Vladivostok Standard Time;600;(GMT+10:00) Vladivostok;Vladivostok Standard Time;Vladivostok Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";
        //public const string WAustraliaStandardTime = "W. Australia Standard Time;480;(GMT+08:00) Perth;W. Australia Standard Time;W. Australia Daylight Time;[01:01:2006;12:31:2006;60;[1;02:00:00;12;1;];[1;00:00:00;1;1;];][01:01:2007;12:31:9999;60;[0;02:00:00;10;5;0;];[0;03:00:00;3;5;0;];];";
        //public const string WCentralAfricaStandardTime = "W. Central Africa Standard Time;60;(GMT+01:00) West Central Africa;W. Central Africa Standard Time;W. Central Africa Daylight Time;;";
        //public const string WEuropeStandardTime = "W. Europe Standard Time;60;(GMT+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna;W. Europe Standard Time;W. Europe Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";
        //public const string WestAsiaStandardTime = "West Asia Standard Time;300;(GMT+05:00) Tashkent;West Asia Standard Time;West Asia Daylight Time;;";
        //public const string WestPacificStandardTime = "West Pacific Standard Time;600;(GMT+10:00) Guam, Port Moresby;West Pacific Standard Time;West Pacific Daylight Time;;";
        //public const string YakutskStandardTime = "Yakutsk Standard Time;540;(GMT+09:00) Yakutsk;Yakutsk Standard Time;Yakutsk Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];";


        //#endregion

        //public static TimeZoneInfo GetTimeZone(string id)
        //{
        //    if (string.IsNullOrEmpty(id)) { return null; }
        //    List<TimeZoneInfo> timeZones = GetTimeZoneList();
        //    return GetTimeZone(timeZones, id);
        //}

        

    }
}
