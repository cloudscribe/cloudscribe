// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2016-01-18
// 

//using cloudscribe.Configuration.DataAnnotations;
//using cloudscribe.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Mvc.Rendering;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class CaptchaSettingsViewModel
    {
        

        [Display(Name = "SiteId")]
        public int SiteId { get; set; } = -1;

        [Display(Name = "SiteGuid")]
        public Guid SiteGuid { get; set; } = Guid.Empty;

        //[Required(ErrorMessageResourceName = "SiteNameRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //[Display(Name = "SiteName", ResourceType = typeof(CommonResources))]
        public string SiteName { get; set; } = string.Empty;

        //[Display(Name = "RequireCaptchaOnLogin", ResourceType = typeof(CommonResources))]
        public bool RequireCaptchaOnLogin { get; set; } = false;
        
        //[Display(Name = "RequireCaptchaOnRegistration", ResourceType = typeof(CommonResources))]
        public bool RequireCaptchaOnRegistration { get; set; } = false;

        //[Display(Name = "RecaptchaPublicKey", ResourceType = typeof(CommonResources))]
        [Display(Name = "Public Key")]
        public string RecaptchaPublicKey { get; set; } = string.Empty;

        //[Display(Name = "RecaptchaPrivateKey", ResourceType = typeof(CommonResources))]
        [Display(Name = "Private Key")]
        public string RecaptchaPrivateKey { get; set; } = string.Empty;


    }
}
