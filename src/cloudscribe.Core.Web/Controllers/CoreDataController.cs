// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2014-11-15
// 

using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web.Helpers;
using cloudscribe.Core.Web.ViewModels.SiteSettings;
using cloudscribe.Core.Web.ViewModels.CoreData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
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

            SiteMenuViewModel model = new SiteMenuViewModel
            {
                MenuTitle = "Core Data Administration"
            };

            SiteMenuItemViewModel item = new SiteMenuItemViewModel();
            item.ItemText = "Currency Administration";
            item.ItemUrl = "/CoreData/Currency";
            item.CssClass = "mnu-coredata mnu-currencyadmin";
            model.Items.Add(item);

            item = new SiteMenuItemViewModel();
            item.ItemText = "Country List Administration";
            item.ItemUrl = "/CoreData/CountryListPage";
            item.CssClass = "mnu-coredata mnu-country";
            model.Items.Add(item);

            item = new SiteMenuItemViewModel();
            item.ItemText = "State List Administration";
            item.ItemUrl = "/CoreData/States";
            item.CssClass = "mnu-coredata mnu-states";
            model.Items.Add(item);

            return View(model);
        }

        public async Task<ActionResult> CountryListPage(int pageNumber = 1, int pageSize = 10)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Country List Administration";

            int totalPages = 0;
            List<IGeoCountry> countries = geoRepo.GetCountriesPage(pageNumber, pageSize, out totalPages);

            CountryListPageViewModel model = new CountryListPageViewModel();
            model.Countries = countries;
            model.Heading = "Country List Administration";
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = pageSize;
            model.Paging.TotalPages = totalPages;

            return View(model);
        }

    }
}
