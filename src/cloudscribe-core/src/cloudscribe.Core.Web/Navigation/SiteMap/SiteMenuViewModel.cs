// Author:					Joe Audette
// Created:					2015-07-10
// Last Modified:			2015-07-10
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Navigation
{
    public class SiteMenuViewModel
    {
        public SiteMenuViewModel()
        {

        }

        public TreeNode<NavigationNode> RootNode { get; set; }
    }
}
