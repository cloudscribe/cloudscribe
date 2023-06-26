// Copyright (c) Idox Software Ltd All rights reserved.
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
using System.Text.RegularExpressions;
using System.Web;

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

            output.TagName = null; //not doing tag replacement

            var sb = new StringBuilder();
            sb.AppendLine(""); //testing with G-1Z6272PNTZ and UA-256173305-1
            sb.AppendLine("<!-- Start Analytics.GA4 Tag Helper -->");
            sb.AppendLine("<!-- Google tag (gtag.js) -->");
            sb.AppendLine($"<script async src=\"https://www.googletagmanager.com/gtag/js?id={ProfileId}\"></script>");
            sb.AppendLine("<script>");
            sb.AppendLine("window.dataLayer = window.dataLayer || [];");
            sb.AppendLine("function gtag(){dataLayer.push(arguments);}");
            sb.AppendLine("gtag('js', new Date());");
            var url = ViewContext.HttpContext.Request.Path;
            sb.AppendLine($"gtag('set', 'page_location', '{url}');");
            var referer = ViewContext.HttpContext.Request.Headers["Referer"];
            sb.AppendLine($"gtag('set', 'page_referrer', '{referer}');");

            //configure the profile id
            sb.Append("gtag('config', '" + ProfileId + "', {  ");
            // Optionally set user id. This enables user-level reports and remarketing across devices.
            if (_options.TrackUserId && !string.IsNullOrWhiteSpace(UserId)) sb.Append("'user_id': '" + UserId + "', ");
            // Optionally enable debug view: https://support.google.com/analytics/answer/7201382
            if (_options.EnableDebugMode) sb.Append("'debug_mode': true, ");
            sb.Remove(sb.Length - 2, 2);
            sb.AppendLine(" });");

            //add any events that may have been stored in TempData
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
                            if(int.TryParse(f.Value, out int v)) sb.Append(comma + "'" + f.Key + "': " + f.Value);
                            else 
                            {
                                // caution about allowing injected user input back out here - from pen testing
                                var safeValue = Regex.Replace(f.Value, "[><']", "")
                                                     .Replace("%3C", "")
                                                     .Replace("%3E", "")
                                                     .Replace("%27", "");
                                sb.Append(comma + "'" + f.Key + "': '" + safeValue + "'");
                            }

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
