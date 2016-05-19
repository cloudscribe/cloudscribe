// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-11
// Last Modified:			2016-05-18
// 

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace cloudscribe.Core.Identity
{
    public static class ClaimsPrincipalExtensions
    {

        public static bool IsInRoles(this ClaimsPrincipal principal, string allowedRolesCsv)
        {
            if(string.IsNullOrWhiteSpace(allowedRolesCsv)) { return true; } // empty indicates no role filtering
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
            


            if(roles.Length == 0) { return true; }

            if (!principal.Identity.IsAuthenticated) { return false; } 

            foreach(string role in roles)
            {
                if (role.Length == 0) continue;
                if(role == "All Users") { return true; }
                if(principal.IsInRole(role)) { return true; }
            }


            return false;

        }

        public static string GetDisplayName(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst("DisplayName");
            return claim != null ? claim.Value : null;
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim != null ? claim.Value : null;
        }

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? claim.Value : null;
        }
    }
}
