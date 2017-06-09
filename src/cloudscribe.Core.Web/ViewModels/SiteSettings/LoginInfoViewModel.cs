// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-09-13
// Last Modified:			2017-06-09
//

using System;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class LoginInfoViewModel
    {
        public Guid SiteId { get; set; } = Guid.Empty;
        public string LoginInfoTop { get; set; } = string.Empty;
        public string LoginInfoBottom { get; set; } = string.Empty;

        
    }
}
