// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
//using cloudscribe.Resources;


namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        //[Display(Name = "PhoneNumber", ResourceType = typeof(CommonResources))]
        public string Number { get; set; }
    }
}
