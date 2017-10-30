// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// Author:                  Joe Audette
// Created:                 2017-10-30
// Last Modified:           2017-10-30
// 

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using System.Text;


namespace cloudscribe.Web.Common.TagHelpers
{
    /// <summary>
    /// 
    /// https://cookieconsent.insites.com/documentation/javascript-api/
    /// </summary>
    public class CookieConsentTagHelper : TagHelper
    {

        [HtmlAttributeName("compliance-type")]
        public string ComplianceType { get; set; } = "info"; // or opt-in or opt-out

        [HtmlAttributeName("popup-background-color")]
        public string PopupBackgroundColor { get; set; } = "#000";

        [HtmlAttributeName("popup-text-color")]
        public string PopupTextColor { get; set; } 

        [HtmlAttributeName("button-background-color")]
        public string ButtonBackgroundColor { get; set; } = "#f1d600";

        [HtmlAttributeName("button-text-color")]
        public string ButtonTextColor { get; set; }

        [HtmlAttributeName("js-url")]
        public string JsUrl { get; set; } = "//cdnjs.cloudflare.com/ajax/libs/cookieconsent2/3.0.3/cookieconsent.min.js";

        [HtmlAttributeName("css-url")]
        public string CssUrl { get; set; } = "//cdnjs.cloudflare.com/ajax/libs/cookieconsent2/3.0.3/cookieconsent.min.css";

        [HtmlAttributeName("header-text")]
        public string HeaderText { get; set; } = "Cookies used on the website!";

        [HtmlAttributeName("message-text")]
        public string MessageText { get; set; } = "This website uses cookies to ensure you get the best experience on our website.";

        [HtmlAttributeName("dismiss-text")]
        public string DismissText { get; set; } = "Got it!";

        [HtmlAttributeName("allow-text")]
        public string AllowText { get; set; } = "Allow cookies";

        [HtmlAttributeName("deny-text")]
        public string DenyText { get; set; } = "Decline";

        [HtmlAttributeName("link-text")]
        public string LinkText { get; set; } = "Learn more";

        [HtmlAttributeName("link-url")]
        public string LinkUrl { get; set; } = "http://cookiesandyou.com";

        [HtmlAttributeName("close-text")]
        public string CloseText { get; set; } = "&#x274c;";

        [HtmlAttributeName("position")]
        public string Position { get; set; } = "bottom"; //or top, top-left,top-right,bottom-left,bottom-right

        [HtmlAttributeName("static")]
        public bool Static { get; set; }

        [HtmlAttributeName("dismiss-on-timeout")]
        public bool Dismiss { get; set; }

        [HtmlAttributeName("auto-open")]
        public bool AutoOpen { get; set; } = true;

        [HtmlAttributeName("show-link")]
        public bool ShowLink { get; set; } = true;

        [HtmlAttributeName("theme")]
        public string Theme { get; set; }

        [HtmlAttributeName("enabled")]
        public bool Enabled { get; set; } = true;

        [HtmlAttributeName("debug")]
        public bool Debug { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Enabled)
            {
                output.SuppressOutput();
                return;
            }

            output.TagName = null;   

            var sb = new StringBuilder();

            sb.AppendLine("<link rel='stylesheet' type='text/css' href='" + CssUrl + "' />");
            sb.AppendLine("<script src='" + JsUrl + "'></script>");
            sb.AppendLine("<script>");
            
            sb.AppendLine("document.addEventListener(\"DOMContentLoaded\", function(){");

            // init
            sb.Append("var consentStatus = {};");
            sb.Append("consentStatus.ComplianceType = '" + ComplianceType + "';");
            sb.Append("consentStatus.DidConsent = false;");
            sb.Append("window.CookieConsentStatus = consentStatus;");
            //sb.Append("alert(window.CookieConsentStatus.DidConsent);");

            if (Debug) sb.AppendLine("console.log(\"cookie consent\");");
            
            sb.AppendLine("window.cookieconsent.initialise({");
            sb.AppendLine("\"palette\": {");
            sb.AppendLine("\"popup\": {");
            sb.Append("\"background\": \"" + PopupBackgroundColor + "\"");
            if(!string.IsNullOrEmpty(PopupTextColor))
            {
                sb.Append(",\"text\": \"" + PopupTextColor + "\"");
            }
            sb.Append("},"); //end popup
            sb.AppendLine("\"button\": {");
            sb.Append("\"background\": \"" + ButtonBackgroundColor + "\"");
            if (!string.IsNullOrEmpty(ButtonTextColor))
            {
                sb.Append(",\"text\": \"" + ButtonTextColor + "\"");
            }
            sb.Append("}"); //end button
            sb.Append("},"); //end palette

            sb.Append("\"type\": \"" + ComplianceType + "\"");
            sb.Append(",\"position\": \"" + Position + "\"");
            if(Static)
            {
                sb.Append(",\"static\": true");
            }
            if(!string.IsNullOrWhiteSpace(Theme))
            {
                sb.Append(",\"theme\": \"" + Theme + "\"");
            }

            sb.Append(",\"content\": {");
            sb.Append("\"header\": '" + HeaderText + "',");
            sb.Append("\"message\": '" + MessageText + "',");
            sb.Append("\"dismiss\": '" + DismissText + "',");
            sb.Append("\"allow\": '" + AllowText + "',");
            sb.Append("\"deny\": '" + DenyText + "',");
            sb.Append("\"link\": '" + LinkText + "',");
            sb.Append("\"href\": '" + LinkUrl + "',");
            sb.Append("\"close\": '" + CloseText + "'");
            sb.Append("}"); //end content

            if (!AutoOpen)
            {
                sb.Append(",\"autoOpen\": false");
            }

            if (Dismiss)
            {
                sb.Append(",\"dismissOnTimeout\": true");
            }

            sb.Append(",onInitialise: function (status) {"); //this apparently only fires if the user has already consented or declined
            //sb.Append("alert('oninitialize');");
            sb.Append("var consentStatus = {};");
            sb.Append("consentStatus.ComplianceType = this.options.type;");
            sb.Append("consentStatus.DidConsent = this.hasConsented();");
            sb.Append("window.CookieConsentStatus = consentStatus;");
            //sb.Append("alert(window.CookieConsentStatus.DidConsent);");
            if (Debug) sb.Append("console.log(\"cookieConsent.onInitialize\");");
            sb.Append("}"); // end onInitialise

            sb.Append(",onStatusChange: function(status, chosenBefore) {");
            sb.Append("var consentStatus = {};");
            sb.Append("consentStatus.ComplianceType = this.options.type;");
            sb.Append("consentStatus.DidConsent = this.hasConsented();");
            sb.Append("window.CookieConsentStatus = consentStatus;");
            //sb.Append("alert(window.CookieConsentStatus.DidConsent);");
            if (Debug) sb.Append("console.log(\"cookieConsent.onStatusChange\");");
            sb.Append("}"); //end onStatusChange


            sb.Append("})");
            sb.Append("})"); //end add event listener
            sb.Append(";");
            sb.AppendLine("</script>");

            output.Content.SetHtmlContent(sb.ToString());


        }

    }
}
