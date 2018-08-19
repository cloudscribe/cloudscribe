// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// Author:                  Joe Audette
// Created:                 2017-01-04
// Last Modified:           2018-08-19
// 

using cloudscribe.Web.Common.Analytics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text;

namespace cloudscribe.Web.Common.TagHelpers
{
    public class GoogleAnalyticsTagHelper : TagHelper
    {
        public GoogleAnalyticsTagHelper(
            IOptions<GoogleAnalyticsOptions> optionsAccessor
            )
        {
            _options = optionsAccessor.Value;
        }

        private GoogleAnalyticsOptions _options;

        private const string ProfileIdAttributeName = "profile-id";
        private const string UserIdAttributeName = "user-id";
        private const string AllowAnchorAttributeName = "allow-anchor";

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ProfileIdAttributeName)]
        public string ProfileId { get; set; }

        [HtmlAttributeName(UserIdAttributeName)]
        public string UserId { get; set; } = string.Empty;

        [HtmlAttributeName(AllowAnchorAttributeName)]
        public bool AllowAnchor { get; set; } = false;

        /// <summary>
        /// if true then page tracking won't happen until the window.load function completes
        /// the default is false
        /// </summary>
        [HtmlAttributeName("track-after-page-load")]
        public bool TrackAfterPageLoad { get; set; } = false;
        
        [HtmlAttributeName("debug")]
        public bool Debug { get; set; } = false;

        [HtmlAttributeName("check-cookie-consent")]
        public bool CheckCookieConsent { get; set; } = true;

        /// <summary>
        /// you can set a function name here for a custom function to replace our default cookie consent check function.
        /// it should be a function that returns true or false and takes no parameters.
        /// </summary>
        [HtmlAttributeName("cookie-consent-check-function")]
        public string CookieConsentCheckFunction { get; set; } = "defaultCookieConsentCheck";

        [HtmlAttributeName("consent-cookie-name")]
        public string ConsentCookieName { get; set; } = "cookieconsent_status";

        /// <summary>
        /// if true will include the default cookie consent check js in the page
        /// default is true, if you are providing your own function for cookieconsent check
        /// with cookie-consent-check-function then you can set this to false.
        /// </summary>
        [HtmlAttributeName("include-cookie-consent-functions")]
        public bool IncludeDefaultCookieConsentFunctions { get; set; } = true;

        /// <summary>
        /// if true then in the absence of a cookie consent cookie cookies will not be used by GA
        /// if false then GA will use cookies unless there is explicit opt out
        /// the default is false
        /// </summary>
        [HtmlAttributeName("require-explicit-cookie-consent")]
        public bool RequireExplicitCookieConsent { get; set; } = false;

        /// <summary>
        /// this setting applies to the default cookie consent function.
        /// if true, then a value of "dismiss" in the consent cookie is treated 
        /// the same as "allow". This is useful especially for compliance type info
        /// where all the user can do is dismiss the popup and no opt-out option is provided
        /// the default is true
        /// </summary>
        [HtmlAttributeName("cookie-consent-accept-dismiss")]
        public bool AcceptDismissAsCookieConsent { get; set; } = true;

        /// <summary>
        /// this setting makes it use the newer cookie consent feature not the one with the cookie consent taghelper
        /// This is the recommended setting now so it defaults to true.
        /// </summary>
        [HtmlAttributeName("use-standard-cookie-consent")]
        public bool UseStandardCookieConsent { get; set; } = true;

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

            if(CheckCookieConsent && IncludeDefaultCookieConsentFunctions)
            {
                sb.Append("function gaGetCookie(name) {");
                sb.Append("var value = \"; \" + document.cookie;");
                sb.Append("var parts = value.split(\"; \" + name + \"=\");");
                sb.Append("if (parts.length == 2) return parts.pop().split(\";\").shift();");
                sb.Append("}");

                sb.Append("function defaultCookieConsentCheck() {");
                sb.Append("var consentStatus = gaGetCookie(\"" + ConsentCookieName + "\");");
                if(Debug)
                {
                    sb.Append("console.log('cookie consent status:' + consentStatus);");
                }
#if DEBUG
                sb.Append("console.log('cookie consent status:' + consentStatus);");
#endif

                if (UseStandardCookieConsent)
                {
                    sb.Append("if (consentStatus == \"yes\") { return true; } else { return false; }");

                }
                else
                {
                    // use legacy cookie consent
                    sb.Append("if (consentStatus == \"deny\") return false;");
                    if (AcceptDismissAsCookieConsent)
                    {
                        sb.Append("if (consentStatus == \"dismiss\") return true;");
                    }

                    sb.Append("if (consentStatus == \"allow\") return true;");
                    if (RequireExplicitCookieConsent)
                    {
                        sb.Append("return false;");
                    }
                    else
                    {
                        sb.Append("return true;");
                    }

                }

                


                sb.Append("}");
            }
            

            if(TrackAfterPageLoad)
            {
                sb.AppendLine("window.addEventListener(\"load\", function(){");
                if(Debug) sb.AppendLine("console.log(\"analytics\");");
            }

            if(CheckCookieConsent)
            {
                sb.Append("var gacanUseCookies = " + CookieConsentCheckFunction + "();");
                if (Debug)
                {
                    sb.Append("if(gacanUseCookies) { console.log('ga can use cookies'); } else { console.log('ga will not use cookies'); }");
                }
#if DEBUG
                sb.Append("if(gacanUseCookies) { console.log('ga can use cookies'); } else { console.log('ga will not use cookies'); }");
#endif

            }



            sb.Append("ga('create',");
            sb.Append("'" + ProfileId + "', 'auto'");
            
            sb.Append(", { ");

            var comma = "";

            //if (_options.TrackUserId && !string.IsNullOrWhiteSpace(UserId))
            //{
            //    sb.Append("'userId': " + "'" + UserId + "'");
            //    comma = ",";
            //}

            if (AllowAnchor)
            {
                sb.Append(comma);
                sb.Append(" 'allowAnchor': true ");
            }
            
            sb.Append(" }");
            
            sb.Append(");");
            
            sb.AppendLine("");

            if (CheckCookieConsent)
            {
                sb.Append("if(gacanUseCookies) {");
            }

            if (_options.TrackUserId && !string.IsNullOrWhiteSpace(UserId))
            {
                sb.Append("ga('set','userId'," + "'" + UserId + "');");
            }
            sb.Append("ga('require', 'displayfeatures');");
            sb.Append("ga('set', 'anonymizeIp', undefined);");

            if(Debug)
            {
                sb.Append("console.log('ga tracked with cookies'); ");
            }
#if DEBUG
            sb.Append("console.log('ga tracked with cookies'); ");
#endif

            if (CheckCookieConsent)
            {
                sb.Append("} else {"); //can't use cookies

                sb.Append("ga('set', 'displayFeaturesTask', null);");
                sb.Append("ga('set', 'anonymizeIp', true);");

                if (Debug)
                {
                    sb.Append("console.log('ga tracked without cookies'); ");
                }
#if DEBUG
                sb.Append("console.log('ga tracked without cookies'); ");
#endif

                sb.Append("}");
            }
            
            sb.AppendLine("ga('send', 'pageview');");

            var eventList = ViewContext.TempData.GetGoogleAnalyticsEvents();
            
            foreach (var e in eventList)
            {
                if(e.IsValid())
                {
                    sb.Append("ga('send', 'event'");
                    sb.Append(", '" + e.Category + "'");
                    sb.Append(", '" + e.Action + "'");

                    if(!string.IsNullOrWhiteSpace(e.Label))
                    {
                        sb.Append(", '" + e.Label + "'");
                    }
                    else
                    {
                        sb.Append(", 'undefined'");
                    }

                    if (!string.IsNullOrWhiteSpace(e.Value))
                    {
                        sb.Append(", '" + e.Value + "'");
                    }

                    if(e.Fields.Count > 0)
                    {
                        sb.Append(",{ ");
                         comma = "";
                        foreach(var f in e.Fields)
                        {
                            sb.Append(comma + "'" + f.Key + "' : '" + f.Value + "'");
                            comma = ",";
                        }
                        sb.Append("}");
                    }

                    sb.Append(");");
                    sb.AppendLine("");
                }
            }

            var transactions = ViewContext.TempData.GetGoogleAnalyticsTransactions();
            if(transactions.Count > 0)
            {
                sb.Append("ga('require', 'ecommerce');");
                foreach(var t in transactions)
                {
                    sb.Append("ga('ecommerce:addTransaction', {");
                    sb.Append("'id': '" + t.Id + "'");
                    if(!string.IsNullOrWhiteSpace(t.Affilitation))
                    {
                        sb.Append(",'affiliation': '" + t.Affilitation + "'");
                    }
                    if (t.Revenue > 0)
                    {
                        //decimal point with up to 6 decimal places
                        var num = string.Format(CultureInfo.InvariantCulture, "{0:#.######}", t.Revenue);
                        sb.Append(",'revenue': '" + num + "'");
                    }
                    if (t.Shipping > 0)
                    {
                        var num = string.Format(CultureInfo.InvariantCulture, "{0:#.######}", t.Shipping);
                        sb.Append(",'shipping': '" + num + "'");
                    }
                    if (t.Tax > 0)
                    {
                        var num = string.Format(CultureInfo.InvariantCulture, "{0:#.######}", t.Tax);
                        sb.Append(",'tax': '" + num + "'");
                    }
                    if (!string.IsNullOrWhiteSpace(t.CurrencyCode))
                    {
                        sb.Append(",'currency': '" + t.CurrencyCode + "'");
                    }


                    sb.Append("});");

                    foreach(var item in t.Items)
                    {
                        sb.Append("ga('ecommerce:addItem', {");
                        sb.Append("'id': '" + t.Id + "'");
                        sb.Append(",'name': '" + item.Name + "'");
                        if (!string.IsNullOrWhiteSpace(item.Sku))
                        {
                            sb.Append(",'sku': '" + item.Sku + "'");
                        }
                        if (!string.IsNullOrWhiteSpace(item.Category))
                        {
                            sb.Append(",'category': '" + item.Category + "'");
                        }
                        if (item.Price > 0)
                        {
                            //decimal point with up to 6 decimal places
                            var num = string.Format(CultureInfo.InvariantCulture, "{0:#.######}", item.Price);
                            sb.Append(",'price': '" + num + "'");
                        }
                        if(item.Quantity > 0)
                        {
                            sb.Append(",'quantity': '" + item.Quantity.ToString(CultureInfo.InvariantCulture) + "'");
                        }
                        if (!string.IsNullOrWhiteSpace(item.CurrencyCode))
                        {
                            sb.Append(",'currency': '" + item.CurrencyCode + "'");
                        }

                        sb.Append("});");
                    }

                }

                sb.Append("ga('ecommerce:send');");
            }

            if (TrackAfterPageLoad)
            {
                sb.Append("});"); //end add event listener
            }


            output.Content.SetHtmlContent(sb.ToString());


        }
    }
}
