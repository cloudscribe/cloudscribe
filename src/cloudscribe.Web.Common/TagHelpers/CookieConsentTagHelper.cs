// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// Author:                  Joe Audette
// Created:                 2017-10-30
// Last Modified:           2017-11-01
// 

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
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

        [HtmlAttributeName("cdn-js-url")]
        public string CdnJsUrl { get; set; } = "//cdnjs.cloudflare.com/ajax/libs/cookieconsent2/3.0.3/cookieconsent.min.js";

        [HtmlAttributeName("cdn-css-url")]
        public string CdnCssUrl { get; set; } = "//cdnjs.cloudflare.com/ajax/libs/cookieconsent2/3.0.3/cookieconsent.min.css";

        [HtmlAttributeName("js-url")]
        public string LocalJsUrl { get; set; } = "/cr/js/cookieconsent.min.js";

        [HtmlAttributeName("css-url")]
        public string LocalCssUrl { get; set; } = "/cr/css/cookieconsent.min.css";

        [HtmlAttributeName("cdn-enable")]
        public bool UseCdn { get; set; } = true;

        [HtmlAttributeName("local-disable")]
        public bool DisableLocalJsAndCss { get; set; } 

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

        /// <summary>
        /// By Default, the tool is automatically appended to the HTML body. Use this option to select the parent element. 
        /// Use autoOpen: false to prevent the tool automatically appending itself to any container.
        /// This setting supports use of a css selector ie #someelementid, div.container
        /// best to use a very specific css selector if you use this setting
        /// </summary>
        [HtmlAttributeName("container")]
        public string Container { get; set; }

        /// <summary>
        /// default: true. Used to disable link on existing layouts. If false, replaces element messagelink with message and removes content of link.
        /// </summary>
        [HtmlAttributeName("show-link")]
        public bool ShowLink { get; set; } = true;

        [HtmlAttributeName("revocable")]
        public bool Revocable { get; set; }

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
        public string CookieName { get; set; } = "cookieconsent_status";

        [HtmlAttributeName("cookie-path")]
        public string CookiePath { get; set; } = "/";

        [HtmlAttributeName("cookie-domain")]
        public string CookieDomain { get; set; } = "";

        [HtmlAttributeName("cookie-expiry-days")]
        public int CookieExpiryDays { get; set; } = 365;


        /// <summary>
        /// array ,  not clear to me the difference between whitelist and blacklist, documentation doesn't say
        /// </summary>
        [HtmlAttributeName("whitelist-pages")]
        public string WhitelistPages { get; set; } = "[]";

        [HtmlAttributeName("backlist-pages")]
        public string BlacklistPages { get; set; } = "[]";

        /// <summary>
        /// The location services are disabled by default. You are encouraged to implement a handler 
        /// to your own service, rather than using the free ones provided.
        /// To enable the basic location services, set ‘location’ to ‘true’. To add you own services 
        /// or configure the order or execution, pass an object with configuration properties.
        /// </summary>
        [HtmlAttributeName("location")]
        public string Location { get; set; } = "false";

        /// <summary>
        /// Rather than getting the country code from the location services, you can hard code a particular country into the tool.
        /// </summary>
        [HtmlAttributeName("law-country-code")]
        public string LawCountryCode { get; set; }

        /// <summary>
        /// If false, then we only enable the popup if the country has the cookie law. We ignore all other country specific rules.
        /// </summary>
        [HtmlAttributeName("law-regional-law")]
        public bool LawRegionalLaw { get; set; } = true;

        [HtmlAttributeName("localizer")]
        public IStringLocalizer Localizer { get; set; } = null;

        private string Localize(string input)
        {
            if (Localizer == null) return input;
            return Localizer[input];
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Enabled)
            {
                output.SuppressOutput();
                return;
            }

            output.TagName = null;   

            var sb = new StringBuilder();

            if(UseCdn)
            {
                sb.AppendLine("<link rel='stylesheet' type='text/css' href='" + CdnCssUrl + "' />");
                sb.AppendLine("<script src='" + CdnJsUrl + "'></script>");
            }
            else if(!DisableLocalJsAndCss)
            {
                sb.AppendLine("<link rel='stylesheet' type='text/css' href='" + LocalCssUrl + "' />");
                sb.AppendLine("<script src='" + LocalJsUrl + "'></script>");
            }
            
            sb.AppendLine("<script>");
            
            sb.Append("document.addEventListener(\"DOMContentLoaded\", function(){");
            
            if (Debug) sb.Append("console.log(\"cookie consent about to initialize\");");
            
            sb.Append("window.cookieconsent.initialise({");
            sb.Append("\"layout\": \"" + Layout + "\"");

            if(!string.IsNullOrWhiteSpace(Container))
            {
                sb.Append(",\"container\": \"" + Container + "\"");
            }

            sb.Append(",\"palette\": {");
            sb.Append("\"popup\": {");
            sb.Append("\"background\": \"" + PopupBackgroundColor + "\"");
            if(!string.IsNullOrEmpty(PopupTextColor))
            {
                sb.Append(",\"text\": \"" + PopupTextColor + "\"");
            }
            sb.Append("},"); //end popup
            sb.Append("\"button\": {");
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
            sb.Append("\"header\": '" + Localize(HeaderText) + "',");
            sb.Append("\"message\": '" + Localize(MessageText) + "',");
            sb.Append("\"dismiss\": '" + Localize(DismissText) + "',");
            sb.Append("\"allow\": '" + Localize(AllowText) + "',");
            sb.Append("\"deny\": '" + Localize(DenyText) + "',");
            sb.Append("\"link\": '" + Localize(LinkText) + "',");
            sb.Append("\"href\": '" + Localize(LinkUrl) + "',");
            sb.Append("\"close\": '" + Localize(CloseText) + "'");
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

            if(!ShowLink)
            {
                sb.Append(",\"showLink\":false");
            }

            if(Revocable)
            {
                //https://cookieconsent.insites.com/documentation/javascript-api/
                // this spelling comes from the documentation could be typo in the documentation
                sb.Append(",\"revokable\":true");
            }

            sb.Append(",cookie:{");
            sb.Append("\"name\":\"" + CookieName + "\"");
            sb.Append(",\"path\":\"" + CookiePath + "\"");
            sb.Append(",\"domain\":\"" + CookieDomain + "\"");
            sb.Append(",\"expiryDays\":" + CookieExpiryDays );
            sb.Append("}");
            


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

            if (!string.IsNullOrWhiteSpace(WhitelistPages))
            {
                sb.Append(",whitelistPage:" + WhitelistPages);
            }

            if (!string.IsNullOrWhiteSpace(BlacklistPages))
            {
                sb.Append(",blacklistPage:" + BlacklistPages);
            }

            if(!string.IsNullOrWhiteSpace(Location) && Location != "false")
            {
                if(Location.StartsWith("{"))
                {
                    sb.Append(",location:" + Location);
                }
                else
                {
                    sb.Append(",location:\"" + Location + "\"");
                }
            }

            sb.Append(",law:{");
            sb.Append("\"regionalLaw\":" + LawRegionalLaw.ToString().ToLowerInvariant());
            if (!string.IsNullOrWhiteSpace(LawCountryCode))
            {
                sb.Append(",countryCode:\"" + LawCountryCode + "\"");
            }
            
            sb.Append("}");

            

            //sb.Append(",onInitialise: function (status) {"); //this apparently only fires if the user has already consented or declined
            //                                                 //sb.Append("alert('oninitialize');");
            //sb.Append("console.log('cookie name ' + this.options.cookie.name);");

            //if (Debug) sb.Append("console.log(\"cookieConsent.onInitialize\");");
            //sb.Append("}"); // end onInitialise

            sb.Append("})");
            sb.Append("})"); //end add event listener
            sb.Append(";");
            sb.AppendLine("</script>");

            output.Content.SetHtmlContent(sb.ToString());


        }

    }
}
