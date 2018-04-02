// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-20
// Last Modified:			2018-04-02
// 

using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class PendingNotificationViewModel
    {
        [Display(Name = "UserId")]
        public Guid UserId { get; set; } = Guid.Empty;

        public bool DidSend { get; set; } = false;

        public string Code { get; set; }

        public string ReturnUrl { get; set; }

    }
}
