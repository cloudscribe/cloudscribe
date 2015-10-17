// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-02
// Last Modified:			2015-10-17
// 

using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace cloudscribe.Web.Pagination
{
    [HtmlTargetElement("cs-pager", Attributes = PagingInfoAttributeName)]
    public class PagerTagHelper : TagHelper
    {
        private const string PagingInfoAttributeName = "cs-paging-info";
        private const string AjaxTargetAttributeName = "cs-ajax-target";
        private const string AjaxModeAttributeName = "cs-ajax-mode";
        private const string PageNumberParamAttributeName = "cs-pagenumber-param";

        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";
        private const string FragmentAttributeName = "asp-fragment";
        private const string HostAttributeName = "asp-host";
        private const string ProtocolAttributeName = "asp-protocol";
        private const string RouteAttributeName = "asp-route";
        private const string RouteValuesDictionaryName = "asp-all-route-data";
        private const string RouteValuesPrefix = "asp-route-";

       
        public PagerTagHelper(IHtmlGenerator generator, IBuildPaginationLinks linkBuilder)
        {
            Generator = generator;
            this.linkBuilder = linkBuilder;
        }

        private IBuildPaginationLinks linkBuilder;

        protected IHtmlGenerator Generator { get; }

        [HtmlAttributeName(PagingInfoAttributeName)]
        public PaginationSettings PagingModel { get; set; }
        
        [HtmlAttributeName(AjaxTargetAttributeName)]
        public string AjaxTarget { get; set; } = string.Empty;

        [HtmlAttributeName(AjaxModeAttributeName)]
        public string AjaxMode { get; set; } = "replace";

        [HtmlAttributeName(PageNumberParamAttributeName)]
        public string PageNumberParam { get; set; } = "pageNumber";

        [HtmlAttributeName("cs-show-first-last")]
        public bool ShowFirstLast { get; set; } = false;

        [HtmlAttributeName("cs-first-page-text")]
        public string FirstPageText { get; set; } = "<";

        [HtmlAttributeName("cs-first-page-title")]
        public string FirstPageTitle { get; set; } = "First page";

        [HtmlAttributeName("cs-last-page-text")]
        public string LastPageText { get; set; } = ">";

        [HtmlAttributeName("cs-last-page-title")]
        public string LastPageTitle { get; set; } = "Last page";

        [HtmlAttributeName("cs-previous-page-text")]
        public string PreviousPageText { get; set; } = "«";

        [HtmlAttributeName("cs-previous-page-title")]
        public string PreviousPageTitle { get; set; } = "Previous page";

        [HtmlAttributeName("cs-next-page-text")]
        public string NextPageText { get; set; } = "»";

        [HtmlAttributeName("cs-next-page-title")]
        public string NextPageTitle { get; set; } = "Next page";

        [HtmlAttributeName("cs-pager-ul-class")]
        public string UlCssClass { get; set; } = "pagination";

        [HtmlAttributeName("cs-pager-li-current-class")]
        public string LiCurrentCssClass { get; set; } = "active";

        [HtmlAttributeName("cs-pager-li-non-active-class")]
        public string LiNonActiveCssClass { get; set; } = "disabled";

        /// <summary>
        /// The name of the action method.
        /// </summary>
        /// <remarks>Must be <c>null</c> if <see cref="Route"/> is non-<c>null</c>.</remarks>
        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        /// <summary>
        /// The name of the controller.
        /// </summary>
        /// <remarks>Must be <c>null</c> if <see cref="Route"/> is non-<c>null</c>.</remarks>
        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        /// <summary>
        /// The protocol for the URL, such as &quot;http&quot; or &quot;https&quot;.
        /// </summary>
        [HtmlAttributeName(ProtocolAttributeName)]
        public string Protocol { get; set; }

        /// <summary>
        /// The host name.
        /// </summary>
        [HtmlAttributeName(HostAttributeName)]
        public string Host { get; set; }

        /// <summary>
        /// The URL fragment name.
        /// </summary>
        [HtmlAttributeName(FragmentAttributeName)]
        public string Fragment { get; set; }

        /// <summary>
        /// Name of the route.
        /// </summary>
        /// <remarks>
        /// Must be <c>null</c> if <see cref="Action"/> or <see cref="Controller"/> is non-<c>null</c>.
        /// </remarks>
        [HtmlAttributeName(RouteAttributeName)]
        public string Route { get; set; }


        /// <summary>
        /// Additional parameters for the route.
        /// </summary>
        [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
        public IDictionary<string, string> RouteValues
        { get; set; }
        = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);


        private string baseHref = string.Empty;
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
          
            if(PagingModel == null) 
            {
                output.SuppressOutput();
                return;
            }
            int totalPages = (int)Math.Ceiling(PagingModel.TotalItems / (double)PagingModel.ItemsPerPage);
            // don't render if only 1 page 
            if (totalPages <= 1) 
            {
                output.SuppressOutput();
                return;
            }
            
            //change the bs-pager element into a ul
            output.TagName = "ul";
            
            string querySeparator;

            //prepare things needed by generatpageeurl function
            TagBuilder linkTemplate = GenerateLinkTemplate();
            baseHref = linkTemplate.Attributes["href"];
            querySeparator = baseHref.Contains("?") ? "&" : "?";
            baseHref = baseHref + querySeparator + PageNumberParam + "=";

            List<PaginationLink> links = linkBuilder.BuildPaginationLinks(
                PagingModel, 
                GeneratePageUrl,
                FirstPageText,
                FirstPageTitle,
                PreviousPageText,
                PreviousPageTitle,
                NextPageText,
                NextPageTitle,
                LastPageText,
                LastPageTitle,
                "...");

            foreach(PaginationLink link in links)
            {
                var li = new TagBuilder("li");

                if (link.IsCurrent)
                {
                    li.AddCssClass(LiCurrentCssClass);
                }

                if (!link.Active)
                {
                    li.AddCssClass(LiNonActiveCssClass);
                }

                var a = new TagBuilder("a");

                if(link.Url.Length > 0)
                {
                    a.MergeAttribute("href", link.Url);
                }
                else
                {
                    a.MergeAttribute("href", "#");
                }
                

                if (link.Text == "«")
                {
                    //a.InnerHtml = "&laquo;";
                    a.InnerHtml.AppendEncoded("&laquo;");
                    
                }
                else if (link.Text == "»")
                {
                    //a.InnerHtml = "&raquo;";
                    a.InnerHtml.AppendEncoded("&raquo;");

                }
                else
                {
                    a.InnerHtml.Append(link.Text);
                }

                if(link.Title.Length > 0)
                {
                    a.MergeAttribute("title", link.Title);
                }
                
                if (AjaxTarget.Length > 0)
                {
                    a.MergeAttribute("data-ajax", "true");
                    a.MergeAttribute("data-ajax-mode", AjaxMode);
                    a.MergeAttribute("data-ajax-update", AjaxTarget);
                }
                
                li.InnerHtml.Append(a);
                
                output.Content.Append(li);
            }

            output.Attributes.Clear();
            output.Attributes.Add("class", UlCssClass);
            
        }

        private string GeneratePageUrl(int pageNumber)
        {
            return baseHref + pageNumber.ToString();
        }

        

        private TagBuilder GenerateLinkTemplate()
        {
            // here I'm just letting the framework generate an actionlink
            // in order to resolve the link url from the routing info
            // there may be a better way to do this
            // if I could find the implementation for Generator.GenerateActionLink
            // maybe I would get a better idea

            var routeValues = RouteValues.ToDictionary(
                    kvp => kvp.Key,
                    kvp => (object)kvp.Value,
                    StringComparer.OrdinalIgnoreCase);

            TagBuilder tagBuilder;
            if (Route == null)
            {
                tagBuilder = Generator.GenerateActionLink(linkText: string.Empty,
                                                          actionName: Action,
                                                          controllerName: Controller,
                                                          protocol: Protocol,
                                                          hostname: Host,
                                                          fragment: Fragment,
                                                          routeValues: routeValues,
                                                          htmlAttributes: null);
            }
            else if (Action != null || Controller != null)
            {
                // Route and Action or Controller were specified. Can't determine the href attribute.
                throw new InvalidOperationException("not enough info to build pager links");
            }
            else
            {
                tagBuilder = Generator.GenerateRouteLink(linkText: string.Empty,
                                                         routeName: Route,
                                                         protocol: Protocol,
                                                         hostName: Host,
                                                         fragment: Fragment,
                                                         routeValues: routeValues,
                                                         htmlAttributes: null);
            }

            return tagBuilder;
        }


    }
}
