// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// Author:                  Joe Audette
// Created:                 2017-01-04
// Last Modified:           2017-01-04
// 

using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace cloudscribe.Web.Common.TagHelpers
{
    public class GoogleAnalyticsTagHelper : TagHelper
    {
        private const string ProfileIdAttributeName = "profile-id";
        private const string AllowAnchorAttributeName = "allow-anchor";

        [HtmlAttributeName(ProfileIdAttributeName)]
        public string ProfileId { get; set; }

        [HtmlAttributeName(AllowAnchorAttributeName)]
        public bool AllowAnchor { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(string.IsNullOrEmpty(ProfileId))
            {
                output.SuppressOutput();
                return;
            }

            output.TagName = "script";    // Replaces <google-analytics> with <script> tag

            var sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){");
            sb.AppendLine("(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),");
            sb.AppendLine("m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)");
            sb.AppendLine("})(window,document,'script','https://www.google-analytics.com/analytics.js','ga');");
            sb.Append("ga('create',");
            sb.Append("'" + ProfileId + "', 'auto'");
            if(AllowAnchor)
            {
                sb.Append(", { 'allowAnchor': true }");
            }
            sb.Append(");");
            sb.AppendLine("");
            sb.AppendLine("ga('send', 'pageview');");

            output.Content.SetHtmlContent(sb.ToString());


        }
    }
}
