﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2018-04-26
// 

using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace cloudscribe.Core.Web.Components
{
    public static class CommonExtensions
    {
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

        public static List<string> ToStringList(this char[] chars)
        {
            List<string> list = new List<string>();
            foreach (char c in chars)
            {
                list.Add(c.ToString());
            }

            return list;
        }

        public static long ToLong(this IPAddress ipAddress)
        {
            long result = 0;

            byte[] b = ipAddress.GetAddressBytes();
            if (b.Length >= 4) // prevent index out of range error
            {
                result = (long)(b[0] * 16777216);
                result += (long)(b[1] * 65536);
                result += (long)(b[2] * 256);
                result += (long)(b[3] * 1);
            }

            return result;
        }

        public static long ConvertIpv4ToLong(string ipv4Address)
        {
            long result = 0;
            if (ipv4Address.Contains(":")) { return result; } // an ipv6 address was passed instead

            IPAddress ipAddress;
            if (IPAddress.TryParse(ipv4Address, out ipAddress))
            {
                byte[] b = ipAddress.GetAddressBytes();
                if (b.Length >= 4) // prevent index out of range error
                {
                    result = (long)(b[0] * 16777216);
                    result += (long)(b[1] * 65536);
                    result += (long)(b[2] * 256);
                    result += (long)(b[3] * 1);
                }
            }

            return result;
        }

    }
}
