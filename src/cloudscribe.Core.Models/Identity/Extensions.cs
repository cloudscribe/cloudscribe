// Author:					Joe Audette
// Created:					2014-12-26
// Last Modified:			2014-12-26
// 

using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace cloudscribe.Core.Models.Identity
{
    public static class Extensions
    {

        public static string GetDisplayName(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return null;
            }
            return claimsIdentity.FindFirstValue("DisplayName");
        }
    }
}
