// Copyright (c) Idox Ltd All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Simon Annetts/ESDM
// Created:					2022-02-07
// Last Modified:			2022-02-07
//

using cloudscribe.Web.Common.Analytics.GA4;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using System.Text;

namespace cloudscribe.Web.Common.TagHelpers
{
    public class GoogleAnalyticsGA4TagHelper : TagHelper
    {
        public GoogleAnalyticsGA4TagHelper(
            IOptions<GoogleAnalyticsGA4Options> optionsAccessor
            )
        {
            _options = optionsAccessor.Value;
        }

        private GoogleAnalyticsGA4Options _options;

        private const string ProfileIdAttributeName = "profile-id";
        private const string UserIdAttributeName = "user-id";

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ProfileIdAttributeName)]
        public string ProfileId { get; set; }

        [HtmlAttributeName(UserIdAttributeName)]
        public string UserId { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(string.IsNullOrEmpty(ProfileId))
            {
                output.SuppressOutput();
                return;
            }

            // output.TagName = "script";    // Replaces <google-analytics> with <script> tag
            output.TagName = null;

            var sb = new StringBuilder();
            sb.AppendLine(""); //testing with G-1Z6272PNTZ and UA-256173305-1
            sb.AppendLine("<!-- Start Analytics.GA4 Tag Helper -->");
            sb.AppendLine("<!-- Google tag (gtag.js) -->");
            sb.AppendLine($"<script async src=\"https://www.googletagmanager.com/gtag/js?id={ProfileId}\"></script>");
            sb.AppendLine("<script>");
            sb.AppendLine("window.dataLayer = window.dataLayer || [];");
            sb.AppendLine("function gtag(){dataLayer.push(arguments);}");
            sb.AppendLine("gtag('js', new Date());");

            //Tracking by UserId is optional
            if (_options.TrackUserId && !string.IsNullOrWhiteSpace(UserId))
            {
                sb.AppendLine("gtag('config', '" + ProfileId + "', {'user_id': '" + UserId + "'});");
            }
            else
            {
                sb.AppendLine("gtag('config', '" + ProfileId + "');");
            }


            var eventList = ViewContext.TempData.GetGoogleAnalyticsGA4Events();
            foreach (var e in eventList)
            {
                if(e.IsValid())
                {
                    sb.Append("gtag('event'");
                    sb.Append(", '" + e.Name + "'");

                    if(e.Parameters.Count > 0)
                    {
                        sb.Append(", {");
                        var comma = "";
                        foreach(var f in e.Parameters)
                        {
                            sb.Append(comma + "'" + f.Key + "': '" + f.Value + "'");
                            comma = ",";
                        }
                        sb.Append("}");
                    }

                    sb.Append(");");
                    sb.AppendLine("");
                }
            }

            sb.AppendLine("</script>");
            sb.AppendLine("<!-- End Analytics.GA4 Tag Helper -->");
            var rawScript = sb.ToString();
            output.Content.SetHtmlContent(rawScript);

        }
    }
}
