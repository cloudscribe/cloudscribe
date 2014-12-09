// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2014-12-09
// 

using cloudscribe.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace cloudscribe.Core.Models
{
    public static class CommonExtensions
    {
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

        
    }
}
