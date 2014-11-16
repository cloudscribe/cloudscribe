// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2014-11-15
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using cloudscribe.Core.Web.ViewModels;
using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.Helpers
{
    /// <summary>
    /// started to write this but found a nice one here:
    /// https://github.com/martijnboland/MvcPaging
    /// but it expects totalitems not totalpages which we would prefer since our db methods work that way
    /// if we continue to develop this we need a lot of improvements, especially a paging strategy like CutePager had or 
    /// like Martijn has in his so that we can limit the total nujmber of pager links when there are more that can fit on the page easily
    /// 
    /// </summary>
    public static class PagingHelpers
    {

        public static MvcHtmlString PageLinks(
            this HtmlHelper html, 
            PagingInfo pagingInfo, 
            Func<int, string> pageUrl
            )
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToInvariantString();
                tag.AddCssClass("btn btn-default");
                if(i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                result.Append(tag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}
