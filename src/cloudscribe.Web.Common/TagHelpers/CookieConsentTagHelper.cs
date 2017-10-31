// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// Author:                  Joe Audette
// Created:                 2017-10-30
// Last Modified:           2017-10-31
// 

using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;


namespace cloudscribe.Web.Common.TagHelpers
{
    /// <summary>
    /// 
    /// https://cookieconsent.insites.com/documentation/javascript-api/
    /// </summary>
    public class CookieConsentTagHelper : TagHelper
    {
        /// <summary>
        /// info, opt-in, or opt-out default is info
        /// see https://cookieconsent.insite
        /// The standard cookie consent popup is purely informational.
        /// </summary>
        [HtmlAttributeName("compliance-type")]
        public string ComplianceType { get; set; } = "info"; // or opt-in or opt-out

        /// <summary>
        /// options are basic, basic-close, basic-header - default is basic
        /// </summary>
        [HtmlAttributeName("layout")]
        public string Layout { get; set; } = "basic";

        /// <summary>
        /// options are top, top-left, top-right, bottom, bottom-left, bottom-right - default is bottom
        /// </summary>
        [HtmlAttributeName("position")]
        public string Position { get; set; } = "bottom"; //or top, top-left,top-right,bottom-left,bottom-right

        /// <summary>
        /// default is false
        /// If true, the popup uses position fixed to stay in one place on the screen despite any scroll bars. 
        /// This option makes the popup position static so it displays at the top of the page. A height 
        /// animation has also been added by default so the popup doesn’t make the page jump, but gradually grows and fades in.
        /// </summary>
        [HtmlAttributeName("static")]
        public bool Static { get; set; }

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


        /// <summary>
        /// boolean/integer, default is false
        /// Set value as time in milliseconds to autodismiss after set time.
        /// Only works if compliance type is info, then the consent cookie will be set automatically to dismiss
        /// on the timeout, so next request the popup will not be shown.
        /// </summary>
        [HtmlAttributeName("dismiss-on-timeout")]
        public string DismissOnTimeout { get; set; } = "false";

        /// <summary>
        /// boolean/integer, default is false
        /// Set value as scroll range to enable ex 5
        ///  Only works if compliance type is info, then the consent cookie will be set automatically to dismiss
        ///  on the timeout, so next request the popup will not be shown. doesn't seem to dismiss the popup proactively
        /// </summary>
        [HtmlAttributeName("dismiss-on-scroll")]
        public string DismissOnScroll { get; set; } = "false";

        [HtmlAttributeName("auto-open")]
        public bool AutoOpen { get; set; } = true;

        [HtmlAttributeName("show-link")]
        public bool ShowLink { get; set; } = true;

        [HtmlAttributeName("theme")]
        public string Theme { get; set; }

        /// <summary>
        /// default is true
        /// </summary>
        [HtmlAttributeName("enabled")]
        public bool Enabled { get; set; } = true;

        [HtmlAttributeName("debug")]
        public bool Debug { get; set; } = false;

        [HtmlAttributeName("popup-open-callback")]
        public string PopupOpenCallback { get; set; }

        [HtmlAttributeName("popup-close-callback")]
        public string PopupCloseCallback { get; set; }

        /// <summary>
        /// this apparently only fires if the user has already consented or declined
        /// </summary>
        [HtmlAttributeName("initialise-callback")]
        public string InitialiseCallback { get; set; }

        [HtmlAttributeName("status-change-callback")]
        public string StatusChangeCallback { get; set; }

        [HtmlAttributeName("revoke-choice-callback")]
        public string RevokeChoiceCallback { get; set; }

        /// <summary>
        /// default is cookieconsent_status
        /// </summary>
        [HtmlAttributeName("cookie-name")]
        public string CookieName { get; set; }


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
            
            if (Debug) sb.AppendLine("console.log(\"cookie consent about to initialize\");");
            
            sb.Append("window.cookieconsent.initialise({");
            sb.Append("\"layout\": \"" + Layout + "\"");
            sb.Append(",\"palette\": {");
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

            if (!string.IsNullOrWhiteSpace(DismissOnTimeout) && DismissOnTimeout != "false")
            {
                sb.Append(",\"dismissOnTimeout\":" + DismissOnTimeout);
            }

            if (!string.IsNullOrWhiteSpace(DismissOnScroll) && DismissOnScroll != "false")
            {
                sb.Append(",\"dismissOnScroll\":" + DismissOnScroll);
            }

            if(!string.IsNullOrWhiteSpace(CookieName) && CookieName != "cookieconsent_status")
            {
                //sb.Append(",\"cookie.name\": \"" + CookieName + "\"");
                sb.Append(",cookie:{");
                sb.Append("\"name\":\"" + CookieName + "\"");
                sb.Append("}");
            }


            if(!string.IsNullOrWhiteSpace(PopupOpenCallback))
            {
                sb.Append(",onPopupOpen:" + PopupOpenCallback);
            }

            if (!string.IsNullOrWhiteSpace(PopupCloseCallback))
            {
                sb.Append(",onPopupClose:" + PopupCloseCallback);
            }

            if (!string.IsNullOrWhiteSpace(InitialiseCallback))
            {
                sb.Append(",onInitialise:" + InitialiseCallback);
            }

            if (!string.IsNullOrWhiteSpace(StatusChangeCallback))
            {
                sb.Append(",onStatusChange:" + StatusChangeCallback);
            }

            if (!string.IsNullOrWhiteSpace(RevokeChoiceCallback))
            {
                sb.Append(",onRevokeChoice:" + RevokeChoiceCallback);
            }

            sb.Append(",onInitialise: function (status) {"); //this apparently only fires if the user has already consented or declined
                                                             //sb.Append("alert('oninitialize');");
            sb.Append("console.log('cookie name ' + this.options.cookie.name);");
            
            if (Debug) sb.Append("console.log(\"cookieConsent.onInitialize\");");
            sb.Append("}"); // end onInitialise

            //sb.Append(",onInitialise: function (status) {"); //this apparently only fires if the user has already consented or declined
            //                                                    //sb.Append("alert('oninitialize');");
            //sb.Append("var consentStatus = {};");
            //sb.Append("consentStatus.ComplianceType = this.options.type;");
            //sb.Append("consentStatus.DidConsent = this.hasConsented();");
            //sb.Append("window.CookieConsentStatus = consentStatus;");
            ////sb.Append("alert(window.CookieConsentStatus.DidConsent);");
            //if (Debug) sb.Append("console.log(\"cookieConsent.onInitialize\");");
            //sb.Append("}"); // end onInitialise

            //sb.Append(",onStatusChange: function(status, chosenBefore) {");
            //sb.Append("var consentStatus = {};");
            //sb.Append("consentStatus.ComplianceType = this.options.type;");
            //sb.Append("consentStatus.DidConsent = this.hasConsented();");
            //sb.Append("window.CookieConsentStatus = consentStatus;");
            ////sb.Append("alert(window.CookieConsentStatus.DidConsent);");
            //if (Debug) sb.Append("console.log(\"cookieConsent.onStatusChange\");");
            //sb.Append("}"); //end onStatusChange


            sb.Append("})");
            sb.Append("})"); //end add event listener
            sb.Append(";");
            sb.AppendLine("</script>");

            output.Content.SetHtmlContent(sb.ToString());


        }

    }
}
