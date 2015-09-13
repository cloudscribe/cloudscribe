// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-15
// Last Modified:			2015-09-06
// 

using System.Collections.Generic;
using System.Security.Claims;

namespace cloudscribe.Web.Navigation.Helpers
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

        /// <summary>
        /// this is replicated from cloudscribe.Core.Identity.ClaimsPrincipalExtensions.cs
        /// so that this project does not have a dependency and can be used without other cloudscribe components
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="allowedRolesCsv"></param>
        /// <returns></returns>
        public static bool IsInRoles(this ClaimsPrincipal principal, string allowedRolesCsv)
        {
            if (string.IsNullOrEmpty(allowedRolesCsv)) { return true; } // empty indicates no role filtering
            string[] roles;
            // in some cases we are using semicolon separated not comma
            if (allowedRolesCsv.Contains(";"))
            {
                roles = allowedRolesCsv.Split(';');
            }
            else
            {
                roles = allowedRolesCsv.Split(',');
            }
            if (roles.Length == 0) { return true; }

            //if (!principal.IsSignedIn()) { return false; }

            foreach (string role in roles)
            {
                if (role.Length == 0) continue;
                if (role == "All Users") { return true; }
                if (principal.IsInRole(role)) { return true; }
            }


            return false;

        }

        

        
    }
}
