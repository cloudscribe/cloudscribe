﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cloudscribe.Core.Web
{
    /// <summary>
    /// jk  https://stackoverflow.com/questions/5433531/using-sections-in-editor-display-templates/5433722#5433722
    /// HTML helper designed to allow rendering of JS resource strings inside partial views
    /// to be deferred to the final  @Html.RenderResources("js"); invocation in _Layout file
    /// (can also be applied to other resource types - but need to be rendered after the partial that declares them).
    /// 
    /// Thus partial views can make rendering of js scripts conditional on their own razor logic
    /// 
    /// Usage: 
    /// @Html.Resource(@<script type="text/javascript">alert("in the js");</script>, "js")
    /// </summary>
    /// 
    public static class HtmlExtensions
    {
        public static HtmlString Resource(this IHtmlHelper HtmlHelper, Func<object, HelperResult> Template, string Type)
        {
            var httpTemplates = HtmlHelper.ViewContext.HttpContext.Items[Type] as List<Func<object, HelperResult>>;
            if (httpTemplates != null) {
                var prevItem = from q in httpTemplates where q(null).ToString() == Template(null).ToString() select q;
                if (!prevItem.Any())
                {
                    httpTemplates.Add(Template);
                }
                    
            }
            
            else HtmlHelper.ViewContext.HttpContext.Items[Type] = new List<Func<object, HelperResult>>() { Template };

            return new HtmlString(String.Empty);
        }

        public static HtmlString RenderResources(this IHtmlHelper HtmlHelper, string Type)
        {
            if (HtmlHelper.ViewContext.HttpContext.Items[Type] != null)
            {
                List<Func<object, HelperResult>> Resources = (List<Func<object, HelperResult>>)HtmlHelper.ViewContext.HttpContext.Items[Type];

                foreach (var Resource in Resources)
                {
                    if (Resource != null) HtmlHelper.ViewContext.Writer.Write(Resource(null));
                }
            }

            return new HtmlString(String.Empty);
        }
    }
}