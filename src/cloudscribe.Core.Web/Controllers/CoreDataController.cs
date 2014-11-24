// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2014-11-21
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
using MvcSiteMapProvider;


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
            ViewBag.Heading = "Core Data Administration";

            return View();

            //AdminMenuViewModel model = new AdminMenuViewModel
            //{
            //    MenuTitle = "Core Data Administration"
            //};

            //AdminMenuItemViewModel item = new AdminMenuItemViewModel();
            //item.ItemText = "Currency Administration";
            //item.ItemUrl = "/CoreData/CurrencyList";
            //item.CssClass = "mnu-coredata mnu-currencyadmin";
            //model.Items.Add(item);

            //item = new AdminMenuItemViewModel();
            //item.ItemText = "Country List Administration";
            //item.ItemUrl = "/CoreData/CountryListPage";
            //item.CssClass = "mnu-coredata mnu-country";
            //model.Items.Add(item);

            //item = new AdminMenuItemViewModel();
            //item.ItemText = "State List Administration";
            //item.ItemUrl = "/CoreData/States";
            //item.CssClass = "mnu-coredata mnu-states";
            //model.Items.Add(item);

            //return View(model);
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
        [MvcSiteMapNode(Title = "Edit Country", ParentKey = "CountryListPage", Key = "CountryEdit")] 
        public async Task<ActionResult> CountryEdit(Guid? guid, int returnPageNumber = 1)
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
        
        public async Task<ActionResult> CountryEdit(GeoCountryViewModel model, int returnPageNumber = 1)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Country";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            geoRepo.Save(model);

            return RedirectToAction("CountryListPage", new { pageNumber = returnPageNumber} );

        }

        //[MvcSiteMapNode(Title = "States", ParentKey = "CountryListPage", Key = "StateListPage")] 
        public async Task<ActionResult> StateListPage(
            Guid? countryGuid, 
            int pageNumber = 1, 
            int pageSize = -1,
            int countryReturnPageNumber = 1)
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
            model.CountryListReturnPageNumber = countryReturnPageNumber;

            // below we are just manipiulating the bread crumbs
            ISiteMap map = SiteMaps.GetSiteMap();
            if (map.CurrentNode != null)
            {
                map.CurrentNode.Title = model.Country.Name + " States";
                // parent node is country  list page
                // unfortunately this does nothing
                //map.CurrentNode.ParentNode.RouteValues.Add("pageNumber", countryReturnPageNumber);
                
                
            }
            
            

            return View(model);

        }

        [MvcSiteMapNode(Title = "Edit State", ParentKey = "StateListPage", Key = "StateEdit")] 
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

            ISiteMap map = SiteMaps.GetSiteMap();
            if (map.CurrentNode != null)
            {
                map.CurrentNode.Title = model.Heading;
                //map.CurrentNode.ParentNode.RouteValues.Add("pageNumber")
            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StateDelete(Guid countryGuid, Guid guid, int returnPageNumber =1)
        {
            geoRepo.DeleteGeoZone(guid);

            return RedirectToAction("StateListPage", new { countryGuid = countryGuid, pageNumber = returnPageNumber });
        }

        public async Task<ActionResult> CurrencyList()
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Currency Administration";
            ViewBag.Heading = "Currency Administration";

            List<ICurrency> model = geoRepo.GetAllCurrencies();

            return View(model);
        }

        [MvcSiteMapNode(Title = "Edit Currency", ParentKey = "CurrencyList", Key = "CurrencyEdit")]
        public async Task<ActionResult> CurrencyEdit(Guid? currencyGuid)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Currency";
            ViewBag.Heading = "Edit Currency";

            CurrencyViewModel model = new CurrencyViewModel();

            if(currencyGuid.HasValue)
            {
                ICurrency currency = geoRepo.FetchCurrency(currencyGuid.Value);
                model.Guid = currency.Guid;
                model.Title = currency.Title;
                model.Code = currency.Code;
            }


            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CurrencyEdit(CurrencyViewModel model)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Currency";
            ViewBag.Heading = "Edit Currency";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ICurrency currency = null;
            if(model.Guid != Guid.Empty)
            {
                currency = geoRepo.FetchCurrency(model.Guid);
            }
            else
            {
                currency = new Currency();
            }

            currency.Code = model.Code;
            currency.Title = model.Title;

            geoRepo.Save(currency);

            return RedirectToAction("CurrencyList");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CurrencyDelete(Guid currencyGuid)
        {
            geoRepo.DeleteCurrency(currencyGuid);

            return RedirectToAction("CurrencyList");
        }
    }
}
