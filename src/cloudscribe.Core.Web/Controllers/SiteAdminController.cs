// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2014-11-15
// 

using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web.Helpers;
using cloudscribe.Core.Web.ViewModels.SiteSettings;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
    public class SiteAdminController : CloudscribeBaseController
    {
        private IGeoRepository geoRepo;

        public SiteAdminController(IGeoRepository geoRepository)
        {
            geoRepo = geoRepository;
        }

        // GET: /SiteAdmin
        public async Task<ActionResult> Index(SiteAdminMessageId? message)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Site Administration";

            AdminMenuViewModel model = new AdminMenuViewModel
            {
                MenuTitle = "Site Administration"
            };

            AdminMenuItemViewModel item = new AdminMenuItemViewModel();
            item.ItemText = "Basic Settings";
            item.ItemUrl = "/SiteAdmin/SiteInfo";
            item.CssClass = "mnu-siteinfo";
            model.Items.Add(item);

            item = new AdminMenuItemViewModel();
            item.ItemText = "Site List";
            item.ItemUrl = "/SiteAdmin/SiteList";
            item.CssClass = "mnu-sitelist";
            model.Items.Add(item);

            item = new AdminMenuItemViewModel();
            item.ItemText = "Roles";
            item.ItemUrl = "/SiteAdmin/Roles";
            item.CssClass = "mnu-roles";
            model.Items.Add(item);

            if(Site.SiteSettings.IsServerAdminSite)
            {
                item = new AdminMenuItemViewModel();
                item.ItemText = "Core Data";
                item.ItemUrl = "/CoreData";
                item.CssClass = "mnu-coredata";
                model.Items.Add(item);
            }

            return View(model);
        }

        // GET: /SiteAdmin/SiteInfo
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
            model.AllTimeZones = DateTimeHelper.GetTimeZoneList();
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

            model.AvailableCountries.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var countries = geoRepo.GetAllCountries();
            Guid selectedCountryGuid = Guid.Empty;
            foreach (var country in countries)
            {
                if(country.ISOCode2 == model.CompanyCountry)
                {
                    selectedCountryGuid = country.Guid;
                }
                model.AvailableCountries.Add(new SelectListItem()
                {
                    Text = country.Name,
                    Value = country.ISOCode2.ToString()

                });
            }

            if (selectedCountryGuid != Guid.Empty)
            {
                var states = geoRepo.GetGeoZonesByCountry(selectedCountryGuid);
                foreach (var state in states)
                {
                    model.AvailableStates.Add(new SelectListItem()
                    {
                        Text = state.Name,
                        Value = state.Code
                    });
                }

            }
           

            return View(model);
        }

        // Post: /SiteAdmin/SiteInfo
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
