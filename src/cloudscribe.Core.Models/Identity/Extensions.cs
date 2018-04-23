//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:					2014-12-26
//// Last Modified:			2016-05-17
//// 

//using System;
//using System.Security.Claims;
//using System.Security.Principal;

//namespace cloudscribe.Core.Models.Identity
//{
//    public static class Extensions
//    {

//        public static string GetDisplayName(this IIdentity identity)
//        {
//            if (identity == null)
//            {
//                throw new ArgumentNullException("identity");
//            }
//            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
//            if (claimsIdentity == null)
//            {
//                return null;
//            }
//            //return claimsIdentity.FindFirstValue("DisplayName");
//            return claimsIdentity.FindFirst("DisplayName").Value;
//        }
//    }
//}
