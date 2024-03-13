using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;

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
        public static HtmlString Resource(this IHtmlHelper HtmlHelper, Func<object, HelperResult> Template, string Type, bool AllowDuplicates=false)
        {
            if (HtmlHelper.ViewContext.HttpContext.Items[Type] != null)
            {
                var httpTemplates = HtmlHelper.ViewContext.HttpContext.Items[Type] as List<Func<object, HelperResult>>;

                if (httpTemplates != null)
                {
                    if (AllowDuplicates) 
                    {
                        httpTemplates.Add(Template);
                    }
                    else
                    {
                        var prevItem = from q in httpTemplates where q.Target.ToString() == Template.Target.ToString() select q;

                        // watch for multiple invocations from one partial view (Target)
                        if (prevItem.Any())
                        {
                            // do we have any from both the same target and also the same html reference when rendered?
                            var candidate = "";
                            using (var writer = new StringWriter())
                            {
                                Template(null).WriteTo(writer, HtmlEncoder.Default);
                                candidate = writer.ToString();
                                writer.Flush();

                                using (var writer2 = new StringWriter())
                                {
                                    foreach (var pre in prevItem)
                                    {
                                        pre(null).WriteTo(writer2, HtmlEncoder.Default);
                                        writer2.Flush();
                                    }
                                    if (!writer2.ToString().Contains(candidate)) httpTemplates.Add(Template);
                                }
                            }
                        }
                        else
                        {
                            httpTemplates.Add(Template);
                        }
                    }
                }
            }

            //if (HtmlHelper.ViewContext.HttpContext.Items[Type] != null) ((List<Func<object, HelperResult>>)HtmlHelper.ViewContext.HttpContext.Items[Type]).Add(Template);

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
                    if (Resource != null) HtmlHelper.ViewContext.Writer.WriteLine(Resource(null));
                }
            }

            return new HtmlString(String.Empty);
        }
    }
}
