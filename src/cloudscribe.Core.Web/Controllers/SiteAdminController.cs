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
            item.ItemUrl = "/SiteAdmin/Roles";
            item.CssClass = "menu_roles";
            model.Items.Add(item);

            return View(model);
        }

        public async Task<ActionResult> SiteInfo(SiteAdminMessageId? message)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Site Basic Settings";

            SiteBasicSettingsViewModel model = new SiteBasicSettingsViewModel();
            model.SiteId = Site.SiteSettings.SiteId;
            model.SiteGuid = Site.SiteSettings.SiteGuid;
            model.SiteName = Site.SiteSettings.SiteName;
            model.Slogan = Site.SiteSettings.Slogan;
            model.TimeZoneId = Site.SiteSettings.TimeZoneId;
            model.CompanyName = Site.SiteSettings.CompanyName;
            model.CompanyStreetAddress = Site.SiteSettings.CompanyStreetAddress;
            model.CompanyStreetAddress2 = Site.SiteSettings.CompanyStreetAddress2;
            model.CompanyLocality = Site.SiteSettings.CompanyLocality;
            model.CompanyRegion = Site.SiteSettings.CompanyRegion;
            model.CompanyPostalCode = Site.SiteSettings.CompanyPostalCode;
            model.CompanyCountry = Site.SiteSettings.CompanyCountry;
            model.CompanyPhone = Site.SiteSettings.CompanyPhone;
            model.CompanyFax = Site.SiteSettings.CompanyFax;
            model.CompanyPublicEmail = Site.SiteSettings.CompanyPublicEmail;
            model.SiteFolderName = Site.SiteSettings.SiteFolderName;
           

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SiteInfo(SiteBasicSettingsViewModel model)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //model.SiteId = Site.SiteSettings.SiteId;
            //model.SiteGuid = Site.SiteSettings.SiteGuid;

            Site.SiteSettings.SiteName = model.SiteName;
            Site.SiteSettings.Slogan = model.Slogan;
            Site.SiteSettings.TimeZoneId = model.TimeZoneId;
            Site.SiteSettings.CompanyName = model.CompanyName;
            Site.SiteSettings.CompanyStreetAddress = model.CompanyStreetAddress;
            Site.SiteSettings.CompanyStreetAddress2 = model.CompanyStreetAddress2;
            Site.SiteSettings.CompanyLocality = model.CompanyLocality;
            Site.SiteSettings.CompanyRegion = model.CompanyRegion;
            Site.SiteSettings.CompanyPostalCode = model.CompanyPostalCode;
            Site.SiteSettings.CompanyCountry = model.CompanyCountry;
            Site.SiteSettings.CompanyPhone = model.CompanyPhone;
            Site.SiteSettings.CompanyFax = model.CompanyFax;
            Site.SiteSettings.CompanyPublicEmail = model.CompanyPublicEmail;
            Site.SiteSettings.SiteFolderName = model.SiteFolderName;

            Site.SiteRepository.Save(Site.SiteSettings);

            SiteAdminMessageId? message = SiteAdminMessageId.UpdateSettingsSuccess;

            return RedirectToAction("Index", new { Message = message });

        }

        //public async Task<ActionResult> SiteList(SiteAdminMessageId? message)
        //{

        //}

        //public async Task<ActionResult> Roles(SiteAdminMessageId? message)
        //{

        //}


        public enum SiteAdminMessageId
        {
            CreateSiteSuccess,
            UpdateSettingsSuccess,
            ClosedSiteSuccess,
            Error
        }
    }
}
