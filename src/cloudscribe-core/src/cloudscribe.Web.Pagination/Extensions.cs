// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-10-12
// Last Modified:			2015-10-12

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Web.Pagination
{
    public static class Extensions
    {
        public static List<string> ToStringList(this char[] chars)
        {
            List<string> list = new List<string>();
            foreach (char c in chars)
            {
                list.Add(c.ToString());
            }

            return list;
        }
    }
}
