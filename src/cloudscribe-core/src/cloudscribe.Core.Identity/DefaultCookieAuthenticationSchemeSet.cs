// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-29
// Last Modified:		    2015-07-29
// 
//

using cloudscribe.Core.Models.Identity;
using Microsoft.AspNet.Identity;

namespace cloudscribe.Core.Identity
{
    public class DefaultCookieAuthenticationSchemeSet : ICookieAuthenticationSchemeSet
    {
        public string ApplicationScheme { get; set; } = IdentityOptions.ApplicationCookieAuthenticationScheme;
        public string ExternalScheme { get; set; } = IdentityOptions.ExternalCookieAuthenticationScheme;
        public string TwoFactorUserIdScheme { get; set; } = IdentityOptions.TwoFactorUserIdCookieAuthenticationScheme;
        public string TwoFactorRememberMeScheme { get; set; } = IdentityOptions.TwoFactorRememberMeCookieAuthenticationType;

    }
}
