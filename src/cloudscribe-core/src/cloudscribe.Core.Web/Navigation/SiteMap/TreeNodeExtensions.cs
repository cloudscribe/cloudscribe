// Author:					Joe Audette
// Created:					2015-07-09
// Last Modified:			2015-07-09
// 

using cloudscribe.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Navigation.SiteMap
{
    public static class TreeNodeExtensions
    {
        public static string ToJson(this TreeNode<NavigationNode> node)
        {
         
            return JsonConvert.SerializeObject(node, Formatting.Indented);
        }
    }
}
