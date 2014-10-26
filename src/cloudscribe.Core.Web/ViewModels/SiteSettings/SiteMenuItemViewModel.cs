using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SiteMenuItemViewModel
    {
        public string ItemText { get; set; }
        public string ItemUrl { get; set; }
        public string CssClass { get; set; }
        public string AllowedRoles { get; set; }
    }
}
