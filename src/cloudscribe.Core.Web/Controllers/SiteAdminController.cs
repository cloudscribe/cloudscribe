// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2014-10-26
// 

using cloudscribe.AspNet.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.SiteSettings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
    public class SiteAdminController : CloudscribeBaseController
    {
        public async Task<ActionResult> Index(SiteAdminMessageId? message)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Site Administration";

            SiteMenuViewModel model = new SiteMenuViewModel
            {
                MenuTitle = "Site Administration"
            };

            SiteMenuItemViewModel item = new SiteMenuItemViewModel();
            item.ItemText = "Basic Settings";
            item.ItemUrl = "/SiteAdmin/SiteInfo";
            item.CssClass = "menu_siteinfo";
            model.Items.Add(item);

            item = new SiteMenuItemViewModel();
            item.ItemText = "Site List";
            item.ItemUrl = "/SiteAdmin/SiteList";
            item.CssClass = "menu_sitelist";
            model.Items.Add(item);

            item = new SiteMenuItemViewModel();
            item.ItemText = "Roles";
            item.ItemUrl = "/RoleAdmin";
            item.CssClass = "menu_roles";
            model.Items.Add(item);

            return View(model);
        }


        public enum SiteAdminMessageId
        {
            CreateSiteSuccess,
            UpdateSettingsSuccess,
            ClosedSiteSuccess,
            Error
        }
    }
}
