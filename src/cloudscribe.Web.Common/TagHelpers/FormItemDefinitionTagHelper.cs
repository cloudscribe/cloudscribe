// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-09
// Last Modified:			2017-07-10
// 

using cloudscribe.Web.Common.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace cloudscribe.Web.Common.TagHelpers
{
    [HtmlTargetElement("input", Attributes = FormItemDefinitionAttributeName)]
    [HtmlTargetElement("textarea", Attributes = FormItemDefinitionAttributeName)]
    [HtmlTargetElement("select", Attributes = FormItemDefinitionAttributeName)]
    public class FormItemDefinitionTagHelper : TagHelper
    {
        private const string FormItemDefinitionAttributeName = "cs-form-item-definition";

        public FormItemDefinitionTagHelper()
        {

        }

        [HtmlAttributeName(FormItemDefinitionAttributeName)]
        public FormItemDefinition FormItem { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // we don't need to output this attribute it was only used for matching in razor
            TagHelperAttribute matchAttribute = null;
            output.Attributes.TryGetAttribute(FormItemDefinitionAttributeName, out matchAttribute);
            if (matchAttribute != null) { output.Attributes.Remove(matchAttribute); }

            if (FormItem == null) return;

            if (FormItem.Required 
                | !string.IsNullOrEmpty(FormItem.RegexValidationExpression)
                | !string.IsNullOrWhiteSpace(FormItem.MaxLengthErrorMessage)
                )
            {
                output.Attributes.Add("data-val", "true");
            }
            if (FormItem.Required)
            {
                output.Attributes.Add("data-val-required", FormItem.RequiredErrorMessage);
            }
            if(!string.IsNullOrEmpty(FormItem.RegexValidationExpression))
            {
                output.Attributes.Add("data-val-regex", FormItem.RegexErrorMessage);
                output.Attributes.Add("data-val-regex-pattern", FormItem.RegexValidationExpression);
            }
            if(FormItem.MaxLength > -1 && !string.IsNullOrWhiteSpace(FormItem.MaxLengthErrorMessage))
            {
                output.Attributes.Add("data-val-length", FormItem.RegexErrorMessage);
                output.Attributes.Add("data-val-length-max", FormItem.MaxLength);
            }

            //TODO: are there other unobtrusive validations that we could easily suport here?
            

        }

    }
}
