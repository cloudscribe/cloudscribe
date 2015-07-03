// Author:					Joe Audette
// Created:					2015-07-02
// Last Modified:			2015-07-03
// 

using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace cloudscribe.Core.Web.TagHelpers
{
    [TargetElement("bs-pager", Attributes = TotalItemsAttributeName)]
    [TargetElement("bs-pager", Attributes = PageSizeAttributeName)]
    [TargetElement("bs-pager", Attributes = CurrentPageAttributeName)]
    [TargetElement("bs-pager", Attributes = LinkUrlAttributeName)]
    public class PagerTagHelper : TagHelper
    {
        public PagerTagHelper()
        {

        }

        private const string TotalItemsAttributeName = "bs-total-items";
        private const string PageSizeAttributeName = "bs-page-size";
        private const string CurrentPageAttributeName = "bs-current-page";
        private const string MaxPagesAttributeName = "bs-max-pages";

        private const string ActionAttributeName = "bs-action";
        private const string ControllerAttributeName = "bs-controller";
        private const string RouteValuesDictionaryName = "asp-all-route-data";
        private const string RouteValuesPrefix = "bs-route-";



        private const string LinkUrlAttributeName = "bs-link-url";
        private const string PageNumberParamAttributeName = "bs-pagenumber-param";


        private const string Href = "href";

        ///// <summary>
        ///// Additional parameters for the route.
        ///// </summary>
        ///// [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
        //[HtmlAttributeName(RouteValuesDictionaryName)]
        //public IDictionary<string, string> RouteValues
        //{ get; set; }
        //= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        ///// <summary>
        ///// The name of the action method.
        ///// </summary>
        ///// <remarks>Must be <c>null</c> if <see cref="Route"/> is non-<c>null</c>.</remarks>
        //[HtmlAttributeName(ActionAttributeName)]
        //public string Action { get; set; }

        ///// <summary>
        ///// The name of the controller.
        ///// </summary>
        ///// <remarks>Must be <c>null</c> if <see cref="Route"/> is non-<c>null</c>.</remarks>
        //[HtmlAttributeName(ControllerAttributeName)]
        //public string Controller { get; set; }

        [HtmlAttributeName(TotalItemsAttributeName)]
        public int TotalItems { get; set; } = 0;

        [HtmlAttributeName(CurrentPageAttributeName)]
        public int CurrentPage { get; set; } = 1;

        [HtmlAttributeName(PageSizeAttributeName)]
        public int PageSize { get; set; } = 10;

        [HtmlAttributeName(MaxPagesAttributeName)]
        public int MaxPages { get; set; } = 10;

        //public int TotalPages { get; set; }


        [HtmlAttributeName(LinkUrlAttributeName)]
        public string LinkUrl { get; set; } = string.Empty;

        [HtmlAttributeName(PageNumberParamAttributeName)]
        public string PageNumberParam { get; set; } = "pageNumber";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
          
            int totalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);
            // don't render if only 1 page or if no link url provided
            //if((totalPages <= 1)||(string.IsNullOrEmpty(LinkUrl)))
            //{
            //    output.SuppressOutput();
            //    return;
            //}

            output.TagName = "div";
            output.PreContent.SetContent("<ul class=\"pagination\">");

            var querySeparator = LinkUrl.Contains("?") ? "&" : "?";

            var items = new StringBuilder();
            for (var i = 1; i <= totalPages; i++)
            {
                var li = new TagBuilder("li");

                var a = new TagBuilder("a");
                a.MergeAttribute("href", $"{LinkUrl}{querySeparator}{PageNumberParam}={i}");
                a.MergeAttribute("title", $"Click to go to page {i}");
                a.InnerHtml = i.ToString();
                if (i == CurrentPage)
                {
                    li.AddCssClass("active");
                }
                li.InnerHtml = a.ToString();
                items.AppendLine(li.ToString());
            }
            output.Content.SetContent(items.ToString());
            output.PostContent.SetContent("</ul>");
            output.Attributes.Clear();
            output.Attributes.Add("class", "pager");
        }

        
    }
}
