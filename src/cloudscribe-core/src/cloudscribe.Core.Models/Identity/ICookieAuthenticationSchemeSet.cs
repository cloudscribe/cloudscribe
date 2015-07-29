// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-29
// Last Modified:			2015-07-29
// 

namespace cloudscribe.Core.Models.Identity
{
    public interface ICookieAuthenticationSchemeSet
    {
        string ApplicationScheme { get; }
        string ExternalScheme { get; }
        string TwoFactorUserIdScheme { get; }
        string TwoFactorRememberMeScheme { get; }
    }
}
