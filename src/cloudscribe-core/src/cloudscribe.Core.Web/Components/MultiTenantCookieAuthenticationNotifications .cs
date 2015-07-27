// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-27
// Last Modified:			2015-07-27
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class MultiTenantCookieAuthenticationNotifications : CookieAuthenticationNotifications
    {
        public MultiTenantCookieAuthenticationNotifications(
            MultiTenantAuthCookieValidator validator)
        {
            OnValidatePrincipal = validator.ValidatePrincipal;
        }
    }
}
