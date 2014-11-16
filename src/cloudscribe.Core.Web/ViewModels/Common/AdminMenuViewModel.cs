// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2014-10-26
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class AdminMenuViewModel
    {
        public string MenuTitle { get; set; }

        private List<AdminMenuItemViewModel> items = new List<AdminMenuItemViewModel>();
        public List<AdminMenuItemViewModel> Items { get { return items; } }
    }
}
