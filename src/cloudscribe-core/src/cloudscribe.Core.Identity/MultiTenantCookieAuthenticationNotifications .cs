// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-27
// Last Modified:			2015-07-28
// 

using Microsoft.AspNet.Authentication.Cookies;

namespace cloudscribe.Core.Identity
{
    //https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Cookies/Notifications/CookieAuthenticationNotifications.cs#L32

    public class MultiTenantCookieAuthenticationNotifications : CookieAuthenticationNotifications
    {
        public MultiTenantCookieAuthenticationNotifications(
            MultiTenantAuthCookieValidator validator):base()
        {
            OnValidatePrincipal = validator.ValidatePrincipal;
        }
    }
}
