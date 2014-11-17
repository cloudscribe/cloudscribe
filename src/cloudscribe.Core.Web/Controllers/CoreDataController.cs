// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2014-11-16
// 

using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web.Helpers;
using cloudscribe.Core.Web.ViewModels.SiteSettings;
using cloudscribe.Core.Web.ViewModels.CoreData;
using cloudscribe.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Roles = "Admins,Content Administrators")]
    public class CoreDataController : CloudscribeBaseController
    {
        private IGeoRepository geoRepo;
        
        public CoreDataController(IGeoRepository geoRepository)
        {
            geoRepo = geoRepository;
        }

        // GET: /SiteAdmin
        public async Task<ActionResult> Index()
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Core Data Administration";

            AdminMenuViewModel model = new AdminMenuViewModel
            {
                MenuTitle = "Core Data Administration"
            };

            AdminMenuItemViewModel item = new AdminMenuItemViewModel();
            item.ItemText = "Currency Administration";
            item.ItemUrl = "/CoreData/Currency";
            item.CssClass = "mnu-coredata mnu-currencyadmin";
            model.Items.Add(item);

            item = new AdminMenuItemViewModel();
            item.ItemText = "Country List Administration";
            item.ItemUrl = "/CoreData/CountryListPage";
            item.CssClass = "mnu-coredata mnu-country";
            model.Items.Add(item);

            item = new AdminMenuItemViewModel();
            item.ItemText = "State List Administration";
            item.ItemUrl = "/CoreData/States";
            item.CssClass = "mnu-coredata mnu-states";
            model.Items.Add(item);

            return View(model);
        }

        public async Task<ActionResult> CountryListPage(int pageNumber = 1, int pageSize = -1)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Country List Administration";
            int itemsPerPage = AppSettings.DefaultPageSize_CountryList;
            if(pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            int totalPages = 0;
            List<IGeoCountry> countries = geoRepo.GetCountriesPage(pageNumber, itemsPerPage, out totalPages);

            CountryListPageViewModel model = new CountryListPageViewModel();
            model.Countries = countries;
            model.Heading = "Country List Administration";
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalPages = totalPages;

            return View(model);
        }

        // GET: /SiteAdmin/SiteInfo
        public async Task<ActionResult> CountryEdit(Guid? guid)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Country";

            GeoCountryViewModel model;

            if((guid != null)&&(guid.Value != Guid.Empty))
            {
                IGeoCountry country = geoRepo.FetchCountry(guid.Value);
                model = GeoCountryViewModel.FromIGeoCountry(country);
            }
            else
            {
                ViewBag.Title = "New Country";
                model = new GeoCountryViewModel();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CountryEdit(GeoCountryViewModel model)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Country";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            geoRepo.Save(model);

            return RedirectToAction("CountryListPage");

        }

        public async Task<ActionResult> StateListPage(Guid? countryGuid, int pageNumber = 1, int pageSize = -1)
        {
            if(!countryGuid.HasValue)
            {
                return RedirectToAction("CountryListPage");
            }

            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "State List Administration";
            int itemsPerPage = AppSettings.DefaultPageSize_CountryList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            //geoRepo.

            int totalPages = 0;
            List<IGeoZone> states = geoRepo.GetGeoZonePage(countryGuid.Value, pageNumber, itemsPerPage, out totalPages);

            StateListPageViewModel model = new StateListPageViewModel();

            IGeoCountry country = geoRepo.FetchCountry(countryGuid.Value);
            model.Country = GeoCountryViewModel.FromIGeoCountry(country);
            model.States = states;

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalPages = totalPages;

            return View(model);

        }

        public async Task<ActionResult> StateEdit(Guid countryGuid, Guid? guid, int? returnPageNumber)
        {
            if (countryGuid == Guid.Empty)
            {
                return RedirectToAction("CountryListPage");
            }

            int returnPage = 1;
            if (returnPageNumber.HasValue) { returnPage = returnPageNumber.Value; }

            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit State";

            GeoZoneViewModel model;

            if((guid.HasValue)&&(guid.Value != Guid.Empty))
            {
                IGeoZone state = geoRepo.FetchGeoZone(guid.Value);
                if((state != null)&&(state.CountryGuid == countryGuid))
                {
                    model = GeoZoneViewModel.FromIGeoZone(state);
                    model.Heading = "Edit State";
                    
                }
                else
                {
                    return RedirectToAction("CountryListPage", new { pageNumber = returnPage });
                }

            }
            else
            {
                model = new GeoZoneViewModel();
                model.Heading = "Create New State";
                model.CountryGuid = countryGuid;
            }

            model.ReturnPageNumber = returnPage;
            
            IGeoCountry country = geoRepo.FetchCountry(countryGuid);
            model.Country = GeoCountryViewModel.FromIGeoCountry(country);

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StateEdit(GeoZoneViewModel model)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Country";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            geoRepo.Save(model);

            return RedirectToAction("StateListPage", new { countryGuid = model.CountryGuid, pageNumber = model.ReturnPageNumber });
            

        }

    }
}
