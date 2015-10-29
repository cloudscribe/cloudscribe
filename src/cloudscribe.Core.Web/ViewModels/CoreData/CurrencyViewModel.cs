// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-24
// Last Modified:			2014-11-25
//

//using cloudscribe.Resources;
using System;
using System.ComponentModel.DataAnnotations;
//using cloudscribe.Configuration.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.CoreData
{
    public class CurrencyViewModel
    {
        private Guid guid = Guid.Empty;

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        private string title = string.Empty;

        //[Display(Name = "Title", ResourceType = typeof(CommonResources))]
        //[Required(ErrorMessageResourceName = "TitleRequired", ErrorMessageResourceType = typeof(CommonResources))]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string code = string.Empty;

        //[Display(Name = "Code", ResourceType = typeof(CommonResources))]
        //[Required(ErrorMessageResourceName = "CodeRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[StringLengthWithConfig(MinimumLength = 1, MaximumLength = 3, MinLengthKey = "CurrencyCodeMinLength", MaxLengthKey = "CurrencyCodeMaxLength", ErrorMessageResourceName = "CurrencyCodeLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
    }
}
