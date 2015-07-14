// Author:					Joe Audette
// Created:					2015-07-10
// Last Modified:			2015-07-11
// 

using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Navigation
{
    public static class NavigationNodeExtensions
    {

        public static string ResolveUrl(this NavigationNode node)
        {
            if (node.Url.Length > 0) return node.Url;
            string url = string.Empty;
            if((node.Controller.Length > 0)&&(node.Action.Length > 0))
            {
                if(node.Action == "Index")
                {
                    url = "~/" + node.Controller;
                }
                else
                {
                    url = "~/" + node.Controller + "/" + node.Action;
                }
                
            }

            return url;
        }
    }
}
