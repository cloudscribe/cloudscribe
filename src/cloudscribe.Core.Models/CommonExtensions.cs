// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2016-02-04
// 

using Microsoft.AspNet.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public static class CommonExtensions
    {
        public static string StartingSegment(this PathString path, out PathString remaining)
        {
            var startingSegment = string.Empty;

            var spath = path.ToString();
            for (var i = 1; i < spath.Length; i++)
            {
                if (spath[i] == '/')
                {
                    remaining = spath.Substring(i, spath.Length - i);
                    return startingSegment;
                }

                startingSegment += spath[i];
            }

            remaining = PathString.Empty;
            return startingSegment;
        }

        public static string StartingSegment(this PathString path)
        {
            PathString remainder;
            return path.StartingSegment(out remainder);
        }

        public static string ToInvariantString(this int i)
        {
            return i.ToString(CultureInfo.InvariantCulture);

        }

        public static string ToInvariantString(this float i)
        {
            return i.ToString(CultureInfo.InvariantCulture);

        }

        public static List<string> SplitOnChar(this string s, char c)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(s)) { return list; }

            string[] a = s.Split(c);
            foreach (string item in a)
            {
                if (!string.IsNullOrEmpty(item)) { list.Add(item); }
            }


            return list;
        }

        public static List<string> SplitOnCharAndTrim(this string s, char c)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(s)) { return list; }

            string[] a = s.Split(c);
            foreach (string item in a)
            {
                if (!string.IsNullOrEmpty(item)) { list.Add(item.Trim()); }
            }


            return list;
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

        /// <summary>
        /// an extenstion method to avoid the warning when firing an async task without await
        /// from inside another async task
        /// for fire and forget scenarios where we don't need to wait for the task to complete
        /// http://stackoverflow.com/questions/22629951/suppressing-warning-cs4014-because-this-call-is-not-awaited-execution-of-the
        /// </summary>
        /// <param name="task"></param>
        public static void Forget(this Task task)
        {

        }



    }
}
