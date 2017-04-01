// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2017-04-01
// 

using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class CaptchaSettingsViewModel
    {
        public Guid SiteId { get; set; } = Guid.Empty;
        public string SiteName { get; set; } = string.Empty;
        public bool RequireCaptchaOnLogin { get; set; } = false;
        public bool RequireCaptchaOnRegistration { get; set; } = false;
        public bool UseInvisibleCaptcha { get; set; } = false;

        [StringLength(255, ErrorMessage = "Maximum length of 255 characters")]
        public string RecaptchaPublicKey { get; set; } = string.Empty;
        [StringLength(255, ErrorMessage = "Maximum length of 255 characters")]
        public string RecaptchaPrivateKey { get; set; } = string.Empty;

    }
}
