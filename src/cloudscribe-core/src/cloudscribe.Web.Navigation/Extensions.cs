// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-15
// Last Modified:			2015-07-15
// 

using Microsoft.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        /// <summary>
        /// this is replicated from cloudscribe.AspNet.Identity.ClaimsPrincipalExtensions.cs
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

        /// <summary>
        /// this is replicated from cloudscribe.Configuration.ConfigurationExtensions
        /// so that this project has no "cloudscribe" dependencies and can be used without using other cloudscribe components
        /// </summary>
        /// <param name="config"></param>
        /// <param name="key"></param>
        /// <param name="defaultIfNotFound"></param>
        /// <returns></returns>
        public static string GetOrDefault(this IConfiguration config, string key, string defaultIfNotFound)
        {
            string result = config.Get(key);

            if (string.IsNullOrEmpty(result)) { return defaultIfNotFound; }

            return result;
        }
    }
}
