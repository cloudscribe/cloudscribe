// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2017-09-12
// Last Modified:           2017-09-12
// 

using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;

namespace cloudscribe.Web.Common.TagHelpers
{
    public class AddThisTagHelper : TagHelper
    {
        private const string ProfileIdAttributeName = "profile-id";
        private const string _addThisFormat = "//s7.addthis.com/js/300/addthis_widget.js#pubid={0}";

        [HtmlAttributeName(ProfileIdAttributeName)]
        public string ProfileId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(ProfileId))
            {
                output.SuppressOutput();
                return;
            }

            output.TagName = "script";    // Replaces <add-this> with <script> tag

            var url = string.Format(CultureInfo.InvariantCulture, _addThisFormat, ProfileId);
            output.Attributes.Add("src", url);


        }
    }
}
