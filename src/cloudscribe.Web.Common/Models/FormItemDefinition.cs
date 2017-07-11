// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-09
// Last Modified:			2017-07-09
// 

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace cloudscribe.Web.Common.Models
{
    public class FormItemDefinition
    {
        public FormItemDefinition()
        {
            Options = new List<SelectListItem>();
        }

        public string Key { get; set; }

        public string Label { get; set; }

        public string Tooltip { get; set; }

        public string DefaultValue { get; set; }

        public string EditPartialViewName { get; set; } = "FormItemInputPartial";

        public string ReadOnlyPartialViewName { get; set; } = "FormItemLabelPartial";

        public string CssClass { get; set; }

        public string IconCssClass { get; set; }

        public bool IconOnLeft { get; set; } = true;


        public int MaxLength { get; set; } = -1;

        public string MaxLengthErrorMessage { get; set; }

        public bool Required { get; set; }

        public string RequiredErrorMessage { get; set; }

        public string RegexValidationExpression { get; set; }
        public string RegexErrorMessage { get; set; }

        public List<SelectListItem> Options { get; set; } = null;
    }
}
